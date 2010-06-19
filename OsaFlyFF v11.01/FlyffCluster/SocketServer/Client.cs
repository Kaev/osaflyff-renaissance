using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
namespace FlyffCluster
{
    public class Client
    {
        #region Headers

        public const int RECV_CHARACTER_LIST = 0xF6,
                 RECV_DELETE_CHARACTER = 0xF5,
                 RECV_PING = 0x14,
                 RECV_CREATE_CHARACTER = 0xF4,
                 RECV_WORLD_TRANSFER = 0xFF05;
        public const int PAK_SERVER_IP = 0xF2,
                         PAK_CHARACTER_LIST = 0xF3,
                         PAK_PONG = 0x14,
                         PAK_MESSAGE = 0xFE,
                         PAK_WORLD_TRANSFER = 0xFF05;

        #endregion
        #region Variables

        public Socket c_socket;
        public int dwMyUserID;
        public uint dwSessionKey;
        public string strUsername;
        public string strPassword;
        public bool bIsBusy;
        public byte[] remains = null;
        public bool bLoggedIn = false;
        public List<Character> characters = new List<Character>();

        #endregion
        #region Constructor and other help functions

        public string GetUnsafePasswordString(string str)
        {
            if (Shared.others.bUnsafePasswords)
                return Shared.MD5.ComputeString("kikugalanet" + str);
            return str;
        }
        public Client(Socket s)
        {
            this.c_socket = s;
            this.dwMyUserID = -1;
            this.dwSessionKey = (uint)new Random().Next(0, ushort.MaxValue * 5);
            this.strUsername = "";
            this.strPassword = "";
            this.bIsBusy = false;
            Log.Write(Log.MessageType.info, "New client connected from {0}. Session key: {1}", s.RemoteEndPoint.ToString(), dwSessionKey);
            SendSessionKey();
        }
        public void LoadCharacters()
        {
            characters.Clear();
            ResultSet rs = new ResultSet("SELECT * FROM flyff_characters WHERE flyff_userid={0} AND flyff_clusterid={1}", dwMyUserID, Server.clusterid);
            while (rs.Advance())
            {
                Character c = new Character();
                c.charactername = rs.Readstring("flyff_charactername");
                c.slot = rs.Readint("flyff_characterslot");
                c.mapID = rs.Readint("flyff_mapid");
                c.x = rs.Readfloat("flyff_positionx");
                c.y = rs.Readfloat("flyff_positiony");
                c.z = rs.Readint("flyff_positionz");
                c.id = rs.Readint("flyff_characterid");
                c.hairstyle = rs.Readint("flyff_hairstyle");
                c.haircolor = rs.Readstring("flyff_haircolor");
                c.facemodel = rs.Readint("flyff_facemodel");
                c.gender = rs.Readint("flyff_gender");
                c.jobid = rs.Readint("flyff_jobid");
                c.level = rs.Readint("flyff_level");
                c.strength = rs.Readint("flyff_strength");
                c.stamina = rs.Readint("flyff_stamina");
                c.dexterity = rs.Readint("flyff_dexterity");
                c.intelligence = rs.Readint("flyff_intelligence");
                ResultSet rs2 = new ResultSet("SELECT flyff_itemid FROM flyff_equipments WHERE flyff_characterid={0}", c.id);
                while (rs2.Advance())
                    c.items.Add(rs2.Readint("flyff_itemid"));
                characters.Add(c);
            }
        }
        public void Write(string str, params object[] args)
        {
            string pre = "";
            if (strUsername == "" || dwMyUserID == -1)
            {
                pre = "[unverified client]: ";
            }
            else
            {
                pre = "[" + strUsername + "]: ";
            }
            Log.Write(Log.MessageType.info, pre + str, args);
        }
        public void Write(string str, Log.MessageType type, params object[] args)
        {
            string pre = "";
            if (strUsername == "" || dwMyUserID == -1)
            {
                pre = "[unverified client]: ";
            }
            else
            {
                pre = "[" + strUsername + "]: ";
            }
            Log.Write(type, pre + str, args);
        }
        public void Destruct(string strReason)
        {
            Write("disconnected - {0}", strReason);
            try
            {
                if (c_socket.Connected)
                {
                    c_socket.Disconnect(false);
                    c_socket.Close();
                }
            }
            finally
            {
                ClientManager.DeleteClient(this);
            }
        }
        public void Destruct(string strReason, Log.MessageType type)
        {
            Write("disconnected - {0}", type, strReason);
            try
            {
                if (c_socket.Connected)
                {
                    c_socket.Disconnect(false);
                    c_socket.Close();
                }
            }
            finally
            {
                ClientManager.DeleteClient(this);
            }
        }
        public void HandlePacket(DataPacket dp)
        {
            if (remains != null)
            {
                byte[] buffer = new byte[remains.Length + dp.buffer.Length];
                Array.Copy(remains, 0, buffer, 0, remains.Length);
                Array.Copy(dp.buffer, 0, buffer, 0, dp.buffer.Length);
                dp.buffer = buffer;
                remains = null;
            }
            if (!dp.VerifyHeaders((int)dwSessionKey))
            {
                remains = dp.buffer;
            }
            else
            {
                dp.dwPointer = 17;
                int dwHeader = dp.Readint32();
                switch ((uint)dwHeader)
                {
                    case RECV_CHARACTER_LIST:
                        CharacterListRequest(dp);
                        break;
                    case RECV_CREATE_CHARACTER:
                        CreateCharacterRequest(dp);
                        break;
                    case RECV_DELETE_CHARACTER:
                        DeleteCharacterRequest(dp);
                        break;
                    case RECV_PING:
                        LatencyCheckRequest(dp);
                        break;
                    case RECV_WORLD_TRANSFER:
                        WorldTransferRequest(dp);
                        break;
                    default:
                        Write("Unknown packet header {0}.", Log.MessageType.warning, dwHeader);
                        break;
                }
            }
        }
        public string GetWorldIP(int dwServerID)
        {
            for (int i = 0; i < Server.servers.Count; i++)
                if (Server.servers[i].dwWorldID == dwServerID)
                    return Server.servers[i].strWorldIP;
            throw new Exception();
        }
        public static byte[] GetBytesFromHex(string hex)
        {
            hex = hex.Replace(" ", "");
            if (hex.Length % 2 != 0)
                hex = hex.Substring(0, hex.Length - 1);
            byte[] ret = new byte[hex.Length / 2];
            for (int i = 0; i < hex.Length; i = i + 2)
                ret[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return ret;
        }

        #endregion
        #region Received packets

        public void CharacterListRequest(DataPacket dp)
        {
            string strBuildDate = dp.Readstring();
            int dwTimeGetTime = dp.Readint32();
            this.strUsername = dp.Readstring();
            this.strPassword = GetUnsafePasswordString(dp.Readstring());
            int dwServerID = dp.Readint32();
            ResultSet set = new ResultSet("SELECT flyff_userid, flyff_passwordhash, flyff_authoritylevel FROM flyff_accounts WHERE flyff_accountname='{0}'", Database.Escape(this.strUsername));
            if (!set.Advance())
            {
                Destruct("Possible hack attempt: account does not exist in database", Log.MessageType.hack);
                return;
            }
            this.dwMyUserID = set.Readint("flyff_userid");
            if (Server.client_builddate != strBuildDate)
            {
                Destruct("Invalid build date");
                return;
            }
            int dwAuthority = set.Readint("flyff_authoritylevel");
            if (dwAuthority <= 0)
            {
                Destruct("Account banned");
                return;
            }
            string strPassword_DB = set.Readstring("flyff_passwordhash");
            if (strPassword.ToLower() != strPassword_DB.ToLower())
            {
                Destruct("Possible hack attempt: invalid password", Log.MessageType.hack);
                return;
            }
            string strWorldIP = "";
            try
            {
                strWorldIP = GetWorldIP(dwServerID);
            }
            catch
            {
                Destruct("Invalid world selected by user");
                return;
            }
            if (c_socket.RemoteEndPoint.ToString().StartsWith("127.0.0.1"))
                strWorldIP = "127.0.0.1"; // Localization for localhost
            LoadCharacters();
            SendServerIP(strWorldIP);
            SendCharacterList(dwTimeGetTime);
        }
        public void DeleteCharacterRequest(DataPacket dp)
        {
            if (this.strUsername != dp.Readstring() || this.strPassword != GetUnsafePasswordString(dp.Readstring()))
            {
                Destruct("Possible hack attempt: invalid username/password while trying to delete character", Log.MessageType.hack);
                return;
            }
            dp.Readstring();
            int dwCharacterID = dp.Readint32();
            int dwTimeGetTime = dp.Readint32();
            ResultSet set;
            if (!(set = new ResultSet("SELECT flyff_characterid FROM flyff_characters WHERE flyff_characterid={0} AND flyff_userid={1} AND flyff_clusterid={2}", dwCharacterID, dwMyUserID, Server.clusterid)).Advance())
            {
                set.Free();
                Destruct("Possible hack attempt: invalid character ID received", Log.MessageType.hack);
                return;
            }
            set.Free();
            Database.Execute("UPDATE flyff_characters SET flyff_userid=-1, flyff_olduserid={0} WHERE flyff_characterid={1}", dwMyUserID, dwCharacterID);
            Database.Execute("DELETE FROM flyff_guildmembers WHERE flyff_characterID={0}", dwCharacterID);
            LoadCharacters();
            SendCharacterList(dwTimeGetTime);
        }
        public void LatencyCheckRequest(DataPacket dp)
        {
            dp.dwPointer = dp.dwSize - 4;
            SendLatencyData(dp.Readint32());
        }
        public void WorldTransferRequest(DataPacket dp)
        {
            // If any checks should be done before allowing the user to connect to the world --- do it here.
            // If we don't send this packet to the client, the client will never be able to connect to the world server.
            SendWorldTransfer();
        }
        public void CreateCharacterRequest(DataPacket dp)
        {
            string strUser = dp.Readstring();
            string strPass = GetUnsafePasswordString(dp.Readstring());
            if (strUser != this.strUsername || strPass.ToLower() != this.strPassword.ToLower())
            {
                Destruct("Possible hack attempt: invalid username/password received when trying to create character", Log.MessageType.hack);
                return;
            }
            int dwDstSlot = dp.Readbyte();
            string strCharacterName = dp.Readstring();
            dp.dwPointer += 3;
            int dwHairstyle = dp.Readbyte();
            string strHaircolor = dp.Readbyte().ToString("X2") + dp.Readbyte().ToString("X2") + dp.Readbyte().ToString("X2") + dp.Readbyte().ToString("X2");
            int dwGender = dp.Readbyte();
            dp.dwPointer++;
            int dwFacemodel = dp.Readbyte();
            dp.dwPointer = dp.dwSize - 4;
            int dwTimeGetTime = dp.Readint32();
            ResultSet set;
            if ((set = new ResultSet("SELECT flyff_characterslot FROM flyff_characters WHERE flyff_characterslot={0} AND flyff_userid={1} AND flyff_clusterid={2}", dwDstSlot, dwMyUserID, Server.clusterid)).Advance())
            {
                set.Free();
                Destruct("Possible hack attempt: user trying to create character on a used slot", Log.MessageType.hack);
                return;
            }
            set.Free();
            if ((set = new ResultSet("SELECT flyff_characterid FROM flyff_characters WHERE flyff_charactername='{0}' AND flyff_userid != -1", Database.Escape(strCharacterName))).Advance())
            {
                SendMessage(0x524);
                return;
            }
            Database.Execute("INSERT INTO flyff_characters (flyff_charactername, flyff_clusterid, flyff_userid, flyff_characterslot, flyff_haircolor, flyff_gender, flyff_facemodel, flyff_hairstyle) VALUES ('{0}', {1}, {2}, {3}, '{4}', {5}, {6}, {7})", Database.Escape(strCharacterName), Server.clusterid, dwMyUserID, dwDstSlot, strHaircolor, dwGender, dwFacemodel, dwHairstyle);
            set = new ResultSet("SELECT flyff_characterid FROM flyff_characters WHERE flyff_clusterid={0} AND Flyff_characterslot={1} AND flyff_charactername='{2}' AND flyff_userid={3}", Server.clusterid, dwDstSlot, Database.Escape(strCharacterName), dwMyUserID);
            if (!set.Advance())
            {
                Write("failed to get new character ID for \"{0}\" while creating character", strCharacterName);
                SendCharacterList(dwTimeGetTime);
                return;
            }
            int dwCharacterID = set.Readint("flyff_characterid");
            set.Free();
            Database.Execute("INSERT INTO flyff_skills (flyff_characterid, flyff_skillid, flyff_skilllevel) VALUES ({0}, 1, 0), ({0}, 2, 0), ({0}, 3, 0)", dwCharacterID);
            List<ItemInfo> items, equips;
            if (dwGender == 1)
            {
                items = Server.itemsF;
                equips = Server.equipsF;
            }
            else
            {
                items = Server.itemsM;
                equips = Server.equipsM;
            }
            for (int i = 0; i < items.Count; i++)
            {
                ItemInfo item = (ItemInfo)items[i];
                Database.Execute("INSERT INTO flyff_items (`flyff_characterid`,`flyff_itemid`,`flyff_slotnum`) VALUES ({0},{1},{2})", dwCharacterID, item.id, item.slot);
            }
            for (int i = 0; i < equips.Count; i++)
            {
                ItemInfo item = (ItemInfo)equips[i];
                Database.Execute("INSERT INTO flyff_equipments (`flyff_characterid`,`flyff_itemid`,`flyff_slotnum`) VALUES ({0},{1},{2})", dwCharacterID, item.id, item.slot);
            }
            LoadCharacters();
            SendCharacterList(dwTimeGetTime);
        }

        #endregion
        #region Outgoing packets

        public void SendSessionKey()
        {
            Packet pak = new Packet();
            pak.Addint32(0);
            pak.Addint32(dwSessionKey);
            pak.Send(c_socket);
        }
        public void SendCharacterList(int dwTimeGetTime)
        {
            Packet pak = new Packet();
            pak.Addint32(PAK_CHARACTER_LIST);
            pak.Addint32(dwTimeGetTime);
            if (characters.Count < 1)
            {
                pak.Addint64(0);
                pak.Send(c_socket);
                return;
            }
            pak.Addint32(characters.Count);
            for (int i = 0; i < characters.Count; i++)
            {
                Character c = (Character)characters[i];
                pak.Addint32(c.slot);
                pak.Addint32(1);
                pak.Addint32(c.mapID);
                pak.Addint32(0x0B + c.gender);
                pak.Addstring(c.charactername);
                pak.Addfloat(c.x);
                pak.Addfloat(c.y);
                pak.Addfloat(c.z);
                pak.Addint32(c.id);
                pak.Addint64(0); //no that should be 0 and duel point 
                pak.Addint64(0);//no that should be karma and disposition
                pak.Addint32(c.hairstyle);
                pak.Addbytes(GetBytesFromHex(c.haircolor));
                pak.Addint32(c.facemodel);
                pak.Addbyte(c.gender);
                pak.Addint32(c.jobid);
                pak.Addint32(c.level);
                pak.Addint32(0);
                pak.Addint32(c.strength);
                pak.Addint32(c.stamina);
                pak.Addint32(c.dexterity);
                pak.Addint32(c.intelligence);
                pak.Addint32(0);
                pak.Addint32(c.items.Count);
                for (int l = 0; l < c.items.Count; l++)
                    pak.Addint32(c.items[l]);
            }
            pak.Addint32(characters.Count);
            for (int i = 0; i < characters.Count; i++)
            {
                pak.Addint32(i);
                pak.Addint32(2);
            }
            pak.Send(c_socket);
        }
        public void SendServerIP(string strWorldIP)
        {
            Packet pak = new Packet();
            pak.Addint32(PAK_SERVER_IP);
            pak.Addstring(strWorldIP);
            pak.Send(c_socket);
        }
        public void SendLatencyData(int dwTimeGetTime)
        {
            Packet pak = new Packet();
            pak.Addint32(PAK_PONG);
            pak.Addint32(dwTimeGetTime);
            pak.Send(c_socket);
        }
        public void SendWorldTransfer()
        {
            Packet pak = new Packet();
            pak.Addint32(PAK_WORLD_TRANSFER);
            pak.Send(c_socket);
        }
        public void SendMessage(int dwMessage)
        {
            Packet pak = new Packet();
            pak.Addint32(PAK_MESSAGE);
            pak.Addint32(dwMessage); // 0x524: character name already in use
            pak.Send(c_socket);
        }

        #endregion
    }
    /*
        private void PrepareCharacterList(DataPacket dp)
        {
            builddate = dp.Readstring();
            int clientcode = dp.Readint32();
            username = dp.Readstring();
            password = dp.Readstring();
            int serverID = dp.Readint32();
            ResultSet rs = new ResultSet("SELECT flyff_userid,flyff_passwordhash,flyff_authoritylevel FROM flyff_accounts WHERE flyff_accountname='{0}'", Database.Escape(username));
            if (!rs.Advance())
            {
                Destruct("Invalid user data");
                return;
            }
            MyUserID = rs.Readint("flyff_userid");
            if (Server.client_builddate != builddate)
            {
                Destruct("Invalid build date");
                return;
            }
            int authority = rs.Readint("flyff_authoritylevel");
            if (authority <= 0)
            {
                Destruct("Banned");
                return;
            }
            string pass = rs.Readstring("flyff_passwordhash");
            if (Shared.others.bUnsafePasswords)
                password = Shared.MD5.ComputeString("kik" + "u" + "gala" + "net" + password);
            MyUserID = rs.Readint("flyff_userid");
            if (pass.ToLower() != password.ToLower())
            {
                Destruct("Invalid user data");
                return;
            }
            Log.Write(Log.MessageType.info, "{0} has chosen server #{1}.", username, serverID);
            string ip = "";
            try
            {
                ip = GetWorldIP(serverID);
            }
            catch (Exception)
            {
                Log.Write(Log.MessageType.warning, "Server IP for server #{0} was not found, using 127.0.0.1", serverID);
                ip = "127.0.0.1";
            }
            if (ClientSocket.RemoteEndPoint.ToString().Split(':')[0] == "127.0.0.1")
                ip = "127.0.0.1";
            LoadCharacters();
            SendServerIP(ip);
            SendCharacterList(clientcode);
        }
        #endregion
        #region Outgoing packets
        public void SendServerIP(string ip)
        {
            Packet pak = new Packet();
            pak.Addint32(PAK_SERVER_IP);
            pak.Addstring(ip);
            pak.Send(ClientSocket);
        }
        [Obsolete("Please avoid using this method as much as possible.. thanks.")]
        public static byte[] GetByteFromHex(string hex)
        {
            hex = hex.Replace(" ", "");
            if (hex.Length % 2 != 0)
                hex = hex.Substring(0, hex.Length - 1);
            byte[] ret = new byte[hex.Length / 2];
            for (int i = 0; i < hex.Length; i = i + 2)
                ret[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return ret;
        }
        public void SendCharacterList(int clientcode)
        {
            Packet pak = new Packet();
            pak.Addint32(PAK_CHARACTER_LIST);
            pak.Addint32(clientcode);
            if (characters.Count < 1)
            {
                pak.Addint64(0);
                pak.Send(ClientSocket);
                return;
            }
            pak.Addint32(characters.Count);
            for (int i = 0; i < characters.Count; i++)
            {
                Character c = (Character)characters[i];
                pak.Addint32(c.slot);
                pak.Addint32(1);
                pak.Addint32(c.mapID);
                pak.Addint32(0x0B + c.gender);
                pak.Addstring(c.charactername);
                pak.Addfloat(c.x);
                pak.Addfloat(c.y);
                pak.Addfloat(c.z);
                pak.Addint32(c.id);
                pak.Addint64(0);
                pak.Addint64(0);
                pak.Addint32(c.hairstyle);
                pak.Addbytes(GetByteFromHex(c.haircolor));
                pak.Addint32(c.facemodel);
                pak.Addbyte(c.gender);
                pak.Addint32(c.jobid);
                pak.Addint32(c.level);
                pak.Addint32(0);
                pak.Addint32(c.strength);
                pak.Addint32(c.stamina);
                pak.Addint32(c.dexterity);
                pak.Addint32(c.intelligence);
                pak.Addint32(0);
                pak.Addint32(c.items.Count);
                for (int l = 0; l < c.items.Count; l++)
                    pak.Addint32(c.items[l]);
            }
            pak.Addint32(characters.Count);
            for (int i = 0; i < characters.Count; i++)
            {
                pak.Addint32(i);
                pak.Addint32(2);
            }
            pak.Send(ClientSocket);
        }
        public string GetWorldIP(int dwServerID)
        {
            for (int i = 0; i < Server.servers.Count; i++)
                if (Server.servers[i].dwWorldID == dwServerID)
                    return Server.servers[i].strWorldIP;
            Log.Write(Log.MessageType.debug, "ClientHandler::GetWorldIP: not found");
            return "127.0.0.1";
        }
        public void SendWarmWelcome()
        {
            Packet pak = new Packet();
            pak.Addint32(0);
            pak.Addint32(dwMySessionKey);
            pak.Send(ClientSocket);
        }
        public void SendCharacterInUse()
        {
            Packet pak = new Packet();
            pak.Addint32(0xfe);
            pak.Addint32(1316);
            pak.Send(ClientSocket);
        }
        public void SendPong(int clientcode)
        {
            Packet pak = new Packet();
            pak.Addint32(PAK_PONG);
            pak.Addint32(clientcode);
            pak.Send(ClientSocket);
        }
        public void SendWorldTransferPacket()
        {
            Packet pak = new Packet();
            pak.Addint32(PAK_WORLD_TRANSFER);
            pak.Send(ClientSocket);
        }
        #endregion
        #region Other
        public void LoadCharacters()
        {
            characters.Clear();
            ResultSet rs = new ResultSet("SELECT * FROM flyff_characters WHERE flyff_userid={0} AND flyff_clusterid={1}", MyUserID, Server.clusterid);
            while (rs.Advance())
            {
                Character c = new Character();
                c.charactername = rs.Readstring("flyff_charactername");
                c.slot = rs.Readint("flyff_characterslot");
                c.mapID = rs.Readint("flyff_mapid");
                c.x = rs.Readfloat("flyff_positionx");
                c.y = rs.Readfloat("flyff_positiony");
                c.z = rs.Readint("flyff_positionz");
                c.id = rs.Readint("flyff_characterid");
                c.hairstyle = rs.Readint("flyff_hairstyle");
                c.haircolor = rs.Readstring("flyff_haircolor");
                c.facemodel = rs.Readint("flyff_facemodel");
                c.gender = rs.Readint("flyff_gender");
                c.jobid = rs.Readint("flyff_jobid");
                c.level = rs.Readint("flyff_level");
                c.strength = rs.Readint("flyff_strength");
                c.stamina = rs.Readint("flyff_stamina");
                c.dexterity = rs.Readint("flyff_dexterity");
                c.intelligence = rs.Readint("flyff_intelligence");
                ResultSet rs2 = new ResultSet("SELECT flyff_itemid FROM flyff_equipments WHERE flyff_characterid={0}", c.id);
                while (rs2.Advance())
                    c.items.Add(rs2.Readint("flyff_itemid"));
                characters.Add(c);
            }
        }
        #endregion
    }*/
}
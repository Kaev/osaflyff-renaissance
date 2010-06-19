using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Threading;

namespace FlyffLogin
{
    public class Client
    {
        #region Headers
        public const int RECV_LOGIN_REQUEST = 0xFC,
                 RECV_RELOG_REQUEST = 0x16,
                 RECV_PING = 0x14,
                 RECV_SOCK_FIN = 0xFF;
        public const int PAK_SERVER_LIST = 0xFD,
                         PAK_LOGIN_MESSAGE = 0xFE,
                         PAK_PONG = 0x0B;
        public const int ERROR_ACCOUNT_BANNED = 0x77,
                         ERROR_INVALID_PASSWORD = 0x78,
                         ERROR_INVALID_USERNAME = 0x79,
                         ERROR_VERIFICATION_REQIURED = 0x7A,
                         ERROR_ACCOUNT_MAINTENANCE = 0x85,
                         ERROR_SERVER_ERROR = 0x88,
                         ERROR_SERVICE_DOWN = 0x6D,
                         ERROR_ACCOUNT_CONNECTED = 0x67,
                         ERROR_RESOURCE_FALSIFIED = 0x8A;
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

        #endregion
        #region Constructor and other help functions

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
                dp.dwPointer = 13;
                int dwHeader = dp.Readint32();
                switch ((uint)dwHeader)
                {
                    case RECV_LOGIN_REQUEST:
                        LoginRequest(dp);
                        break;
                    case RECV_PING:
                        break;
                    case RECV_SOCK_FIN:
                        break;
                    case RECV_RELOG_REQUEST:
                        RelogRequest(dp);
                        break;
                    default:
                        Write("Unknown packet header {0}.", Log.MessageType.warning, dwHeader);
                        break;
                }
            }
        }

        #endregion

        #region Received packets
        public void LoginRequest(DataPacket dp)
        {
            string strBuildDate = dp.Readstring();
            string strClientHash = dp.Readstring();
            this.strUsername = dp.Readstring();
            this.strPassword = dp.Readstring();
            ResultSet set;
            if ((set = new ResultSet("SELECT `ip` FROM `flyff_bans` WHERE `active`='1' AND `ip`='{0}'", c_socket.RemoteEndPoint.ToString().Remove(c_socket.RemoteEndPoint.ToString().IndexOf(':')))).Advance())
            {
                set.Free();
                Destruct("Login failed (IP banned)");
                return;
            }
            set.Free();
            if ((Server.use_flyff_a && Server.client_hash.ToLower() != strClientHash.ToLower()) || Server.client_builddate != strBuildDate)
            {
                SendLoginFailure(ERROR_RESOURCE_FALSIFIED);
                Destruct("Login failed (Resource was falsified)");
                return;
            }
            set = new ResultSet("SELECT * FROM flyff_accounts WHERE flyff_accountname='{0}'", Database.Escape(strUsername));
            if (!set.Advance())
            {
                set.Free();
                SendLoginFailure(ERROR_INVALID_USERNAME);
                Destruct("Login failed (Wrong ID)");
                return;
            }
            string strPassword_DB = set.Readstring("flyff_passwordhash");
            if (Shared.others.bUnsafePasswords)
                strPassword = Shared.MD5.ComputeString("kik" + "u" + "gala" + "net" + strPassword);
            if (strPassword.ToLower() != strPassword_DB.ToLower())
            {
                SendLoginFailure(ERROR_INVALID_PASSWORD);
                Destruct("Login failed (Wrong password)");
                return;
            }
            if (set.Readint("flyff_authoritylevel") <= 0)
            {
                SendLoginFailure(ERROR_ACCOUNT_BANNED);
                Destruct("Login failed (Account suspended)");
                return;
            }
            if (set.Readint("flyff_awaitingverification") != 0)
            {
                SendLoginFailure(ERROR_VERIFICATION_REQIURED);
                Destruct("Login failed (Awaiting verification)");
                return;
            }
            this.dwMyUserID = set.Readint("flyff_userid");
            set.Free();
            set = new ResultSet("SELECT * FROM flyff_accountchecks WHERE flyff_userid={0}", this.dwMyUserID);
            if (set.Advance())
            {
                SendLoginFailure(ERROR_ACCOUNT_MAINTENANCE);
                Destruct("Login failed (Account under maintenance)");
                return;
            }
            set.Free();
            lock (ClientManager.srClientListRoot)
                for (int i = 0; i < ClientManager.c_clients.Count; i++)
                {
                    if (ClientManager.c_clients[i].dwMyUserID == dwMyUserID && ClientManager.c_clients[i].bLoggedIn && ClientManager.c_clients[i].GetHashCode() != GetHashCode())
                    {
                        SendLoginFailure(ERROR_ACCOUNT_CONNECTED);
                        return;
                    }
                }
            Database.Execute("UPDATE flyff_accounts SET flyff_lastlogin=NOW(),flyff_lastip='{0}',flyff_linestatus=0 WHERE flyff_userid={1}", c_socket.RemoteEndPoint.ToString(), dwMyUserID);
            Write("successful login.");
            bLoggedIn = true;
            SendServerList();
        }
        public void RelogRequest(DataPacket dp)
        {
            string strUser = dp.Readstring();
            string strPass = dp.Readstring();
            if (Shared.others.bUnsafePasswords)
                strPass = Shared.MD5.ComputeString(string.Format("{0}{1}{2}{3}{4}", "kik", "u", "gala", "net", strPass));
            if (strUser == this.strUsername && strPass.ToLower() == this.strPassword.ToLower())
                ISCRemoteServer.SendKickFromServers(dwMyUserID);
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
        public void SendServerList()
        {
            Packet pak = new Packet();
            pak.Addint32(PAK_SERVER_LIST);
            pak.Addint32(0);
            pak.Addbyte(1);
            pak.Addstring(strUsername.ToLower());
            pak.Addint32(0x0B);
            for (int i = 0; i < Server.serverlist.Count; i++)
            {
                ClusterInfo cluster = Server.serverlist[i];
                string IP = cluster.strClusterIP;
                if (c_socket.RemoteEndPoint.ToString().Split(':')[0] == "127.0.0.1")
                    IP = "127.0.0.1";
                pak.Addint32(-1);
                pak.Addint32(cluster.dwClusterID);
                pak.Addstring(cluster.strClusterName);
                pak.Addstring(IP);
                for (int l = 0; l < 2; l++)
                    pak.Addint64(l);
                for (int l = 0; l < cluster.worlds.Count; l++)
                {
                    WorldInfo server = cluster.worlds[l];
                    pak.Addint32(server.dwClusterID);
                    pak.Addint32(server.dwWorldID);
                    pak.Addstring(server.strWorldName);
                    pak.Addint64(0);
                    pak.Addint32(server.dwOnline);
                    pak.Addint32(1);
                    pak.Addint32(server.dwCapacity);
                }
            }
            pak.Send(c_socket);
        }
        public void SendLoginFailure(int dwErrorCode)
        {
            Packet pak = new Packet();
            pak.Addint32(PAK_LOGIN_MESSAGE);
            pak.Addint32(dwErrorCode);
            pak.Send(c_socket);
        }
        #endregion
    }
}
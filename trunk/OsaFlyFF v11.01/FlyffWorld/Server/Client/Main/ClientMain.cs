using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
namespace FlyffWorld
{
    public partial class Client : Mover
    {
        #region Client variables

        public Socket c_socket;
        public AsyncCallback callback;
        public PlayerData c_data;
        public Timers timers = new Timers();
        public bool playerSpawned = false;
        public Mover fun_mover = null;
        public ScrollProtection scrollprotection;
        public const int INVITING_CHARACTER = 0,
                         INVITING_GUILD = 1;
        public int[] guildInvitationData = new int[] { -1, -1 };
        public string strTradeNPC;

        #endregion
        #region Client constructor&destructor
        
        public Client(Socket s)
            : base(MOVER_PLAYER)
        {
            
            if (s == null)
                return;
            this.c_socket = s;
            string ip = s.RemoteEndPoint.ToString().Split(':')[0];
            ResultSet rs = new ResultSet("SELECT * FROM `flyff_bans` WHERE `ip`='{0}';", ip);
            if (rs.Advance())
            {
                rs.Free();
                s.Close();
                return;
            }
            rs.Free();
            Packet pak = new Packet();
            pak.Addpad(8);
            pak.Send(this);
            c_data = new PlayerData(this);
            c_attributes[DST_SPEED] = 100; //100% speed
            dataHandling();
            base.child = this;
            
        }
        
        public void Destruct(string Reason)
        {
            WorldServer.world_players.Remove(this);
            if (c_data.strPlayerName == "")
                Log.Write(Log.MessageType.notice, "Illegal client destruct: {0}", this.c_socket.RemoteEndPoint.ToString());
            else
            {
                Log.Write(Log.MessageType.info, "Player {0} disconnected. Reason: {1}", c_data.strPlayerName, Reason);
                /// ????????????????
                /// [Adidishen]: pointless. we're destructing the player, why check if the player had buffs? O_____O
                /// [divinepunition]: because otherwise the current attributes will save to the database and raised attributes become regular
                /// [Adidishen]: uncommented until we get a proper solution.
                if (c_data.buffs.Count > 0) //if player had buff on him
                {
                    for (int i = 0; i < c_data.buffs.Count; i++)
                    {

                        Buff curBuff = c_data.buffs[i];
                        int[] listBuffEffect = BuffDB.getBuffBonus(curBuff);

                        c_attributes[listBuffEffect[1]] -= listBuffEffect[0]; //we delete effect of older buff


                        if (listBuffEffect[2] != 0 && listBuffEffect[3] != 0)//if buff is GT
                        {
                            c_attributes[listBuffEffect[3]] -= listBuffEffect[2];
                        }


                    }
                }
                SaveAll(true);
                for (int i = 0; i < c_spawns.Count; i++)
                    if (c_spawns[i] is Client)
                        ((Client)c_spawns[i]).DespawnPlayer(this);
                OnFriendsNetworkDisconnect();

                try
                {
                    this.c_socket.Disconnect(false);
                    this.c_socket.Close();
                }
                catch (Exception) { }
            }
        }

        #endregion
        #region Received packets handlers

        public void dataHandling()
        {
            try
            {
                if (this.callback == null)
                    this.callback = new AsyncCallback(parseAsyncResult);
                DataPacket dp = new DataPacket();
                this.c_socket.BeginReceive(dp.buffer, 0, 1000, SocketFlags.None, this.callback, dp);
            }
            catch (Exception e)
            {
                Log.Write(Log.MessageType.error, "Error getting data from the client: " + e.Message);
            }
        }
        public void parseAsyncResult(IAsyncResult res)
        {
            DataPacket head_dp = (DataPacket)res.AsyncState;
            try
            {
                int head_size = c_socket.EndReceive(res);
                if (head_size < 13)
                {
                    Destruct("Client finished session");
                    return;
                }
                int readData = 0;
                List<DataPacket> dps = new List<DataPacket>();
                while (readData < head_size - 1)
                {
                    if (head_dp.Readbyte() == 0x5E)
                    {
                        head_dp.Readint();
                        int thissize = head_dp.Readint();
                        DataPacket newdp = head_dp.CutBuffer(readData, readData + 13 + thissize);
                        head_dp.ResetPointer(readData + 13 + thissize);
                        dps.Add(newdp);
                        readData += 13 + thissize;
                    }
                    else
                    {
                        Destruct("Invalid packet structure");
                        break;
                    }
                }
                for (int i = 0; i < dps.Count; i++)
                {
                    DataPacket dp = dps[i];
                    dp.ResetPointer();
                    parseIncomingPackets(dp);
                }
                if (this.c_socket.Connected)
                    dataHandling();
            }
            catch (Exception e)
            {
                Destruct("(exception) " + e.Message + (Server.enable_debugging ? ("\r\nStack trace:\r\n" + e.StackTrace) : ""));
            }
        }

        #endregion
        public void parseIncomingPackets(DataPacket dp)
        {
            dp.ResetPointer(17);
            uint header = (uint)dp.Readint();
            switch (header)
            {
                case RECV_SELFSPAWN_REQUEST: PlayerJoinWorld(dp); break;
                case RECV_PLAYER_CHAT: PlayerChat(dp); break;
                case RECV_MOVE_ITEM_INV: ItemMoveSlot(dp); break;
                case RECV_STATS_UPDATE: PlayerNewStats(dp); break;
                case RECV_SKILLS_UPDATE: PlayerNewSkills(dp); break;
                case RECV_CLICK_MOVE: MovementMouse(dp); break;
                case RECV_WASD_MOVE: MovementKeyboard(dp); break;
                case RECV_EQUIP_ITEM: ItemEquip(dp); break;
                case RECV_REMOVE_ITEM: ItemUnequip(dp); break;
                case RECV_REMOVE_CARD: ItemRemoveCard(dp); break; // BlackGiant -> Remove pierced card
                case RECV_NEW_HOTSLOT: PlayerRegisterHotslot(dp); break;
                case RECV_DELETE_HOTSLOT: PlayerDeleteHotslot(dp); break;
                case RECV_NEW_KEYBIND: PlayerRegisterKeybind(dp); break;
                case RECV_DELETE_KEYBIND: PlayerDeleteKeybind(dp); break;
                //case RECV_SHOP_ADDITEM: PlayerShopAddItem(dp); break;
                //case RECV_SHOP_OPEN: PlayerShopOpen(dp); break;
                //case RECV_SHOP_REMOVEITEM: PlayerShopRemoveItem(dp); break;
                //case RECV_SHOP_CLOSE: PlayerShopClose(dp); break;
                //case RECV_SHOP_VIEW: PlayerShopView(dp); break;
                case RECV_CAST_SKILL: PlayerCastSkill(dp); break;
                case RECV_DELETE_ITEM: ItemDelete(dp); break;
                //case RECV_SHOP_BUYITEM: PlayerShopBuyItem(dp); break;
                case RECV_FRIEND_DECLINE: FriendsRefuse(dp); break;
                case RECV_FRIEND_BLOCK: FriendsBlock(dp); break;
                case RECV_FRIEND_ADDBYMENU: FriendsAddMenu(dp); break;
                case RECV_FRIEND_ADDNEW: FriendsAdd(dp); break;
                case RECV_NPC_BUFFPANG: NPCBuffPang(); break;
                case RECV_MSN_STATUSCHANGE: FriendsChangeStatus(dp); break;
                case RECV_FRIEND_INVCONFIRM: FriendsAccept(dp); break;
                case RECV_FRIEND_DELETE: FriendsDelete(dp); break;
                case RECV_NPC_WANNACHAT: NPCOpenDialog(dp); break;
                case RECV_NPC_WANNATRADE: NPCOpenShop(dp); break;
                case RECV_ITEM_ON_ITEM: ItemCheckUpgrading(dp); break;
                case RECV_CLOSE_NPCSHOP: NPCCloseShop(dp); break;
                case RECV_BUY_NPCSHOP: NPCBuyItem(dp); break;
                case RECV_SELL_NPCSHOP: NPCSellItem(dp); break;
                case RECV_RESURRECT_LODE: PlayerReviveLodelight(); break;
                case RECV_RESURRECT_ORIGINAL: PlayerReviveOriginal(); break;
                case RECV_GUILDS_SETNAME: GuildSetName(dp); break;
                case RECV_GUILDS_NOTICE: GuildSetNotice(dp); break;
                case RECV_GUILDS_DISBAND: GuildDisband(dp); break;
                case RECV_GUILDS_INVITE: GuildSendInvitation(dp); break;
                case RECV_GUILDS_ACCEPTINV: GuildAcceptInvitation(dp); break;
                case RECV_GUILDS_LEAVE: GuildRemoveMember(dp); break;
                case RECV_GUILDS_SETLOGO: GuildSetLogo(dp); break;
                case RECV_GUILDS_SETCLASS: GuildSetClass(dp); break;
                case RECV_GUILDS_SETRANK: GuildSetRank(dp); break;
                case RECV_MOVEMENT_CORRECT: MovementCorrect(dp); break;
                case RECV_ATTACK: PlayerAttackMonster(dp); break;
                case RECV_GUILDS_GDREQUEST: GuildDuelRequest(dp); break;
                case RECV_GUILDS_GDACCEPT: GuildDuelAccept(dp); break;
                case RECV_GUILDS_GDTRUCEINV: GuildDuelTruceRequest(dp); break;
                case RECV_GUILDS_GDTRUCE: GuildDuelTruceAccept(dp); break;
                case RECV_GUILDS_GDGIVEUP: GuildDuelGiveUp(dp); break;
                case RECV_GUILDS_SETSALARY: GuildSetSalary(dp); break;
                case RECV_GUILDS_SETTITLES: GuildSetTitle(dp); break;
                case RECV_TARGET_INFORMATION: PlayerSetTarget(dp); break;
                case RECV_HAIR_DESIGNER: PlayerNewHair(dp); break;
                case RECV_MAKEUP_ARTIST: PlayerNewFace(dp); break;
                case RECV_MOVEMENT_FOLLOW: MovementFollow(dp); break;
                case RECV_TELEPORT_ARENA: NPCArenaTeleport(true); break;
                case RECV_TELEPORT_EXITARENA: NPCArenaTeleport(false); break;
                case RECV_AWAKENING_REQUEST: ItemAwake(dp); break; // Nicco -> Awake item
                case RECV_CHANGE_JOB: ScrollChangeJob(dp); break; // Nicco -> changing job!
                case RECV_CHANGE_NAME: ScrollChangeName(dp); break; // Nicco -> changing name!
                case RECV_NPC_MAKESHINESTONE: NPCCreateShiningOricalkum(dp); break; // Nicco -> Shining oricalkum
                default:
                    {
                        if (Server.enable_debugging)
                        {
                            Log.Write(Log.MessageType.debug, "Unknown packet header received. Header: 0x{0}. Size: {1}",
                                header.ToString("X8"),
                                dp.size);
#if DEBUG
                            Log.Write(Log.MessageType.debug, "Packet data: ");
                            Log.Write(Log.MessageType.debug, Packets.MakePString(dp.buffer, dp.size));
#endif
                        }
                        Log.Write(Log.MessageType.packet, "{0}{1}{2}", "0x" + header.ToString("X8"), dp.size, Packets.MakePString(dp.buffer, dp.size));
                        break;
                    }
            }
        }
        public void SendToAll(Packet pak)
        {
            pak.SendTo(WorldServer.world_players);
        }
    }
}
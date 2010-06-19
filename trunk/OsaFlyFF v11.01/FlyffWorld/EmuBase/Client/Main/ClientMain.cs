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

        public int dwMySessionKey = 0;

        public Socket c_socket;
        public AsyncCallback callback;
        public PlayerData c_data;
        public Timers timers = new Timers();
        public bool playerSpawned = false;
        public Mover fun_mover = null;
        //public ScrollProtection scrollprotection;
        public const int INVITING_CHARACTER = 0,
                         INVITING_GUILD = 1;
        public int[] guildInvitationData = new int[] { -1, -1 };

        public bool bIsBusy = false;
        

        public string strTradeNPC;
        public MoverAttributes _lastAttributes = new MoverAttributes();
        public MoverAttributes c_totalAttributes = new MoverAttributes();
        public void RebuildAttributes() // by exos
        {
            MoverAttributes setBonuses = new MoverAttributes();
            Item curItem = null;
            int attributeValue = 0;
            Item helmet = GetSlotByPosition(48).c_item,
                 suit = GetSlotByPosition(44).c_item,
                 gauntlets = GetSlotByPosition(46).c_item,
                 boots = GetSlotByPosition(47).c_item;

            // Set refine and bonuses
            if (helmet != null && suit != null && gauntlets != null && boots != null)
                setBonuses = WorldHelper.GetSetBonuses(helmet, suit, gauntlets, boots);

            for (int attributeID = 1; attributeID < 150; attributeID++)
            {
                int r_attributeID = attributeID;
                if (r_attributeID > 94)
                    r_attributeID += 9906;
                attributeValue = c_attributes[r_attributeID] + setBonuses[r_attributeID];

                // Weapons, fashions and stuff only
                int[] stuff = new int[] { 48, 44, 46, 47, 68, 69, 70, 71, 50, 54, 52 };
                foreach (int i in stuff)
                {
                    curItem = GetSlotByPosition(i).c_item;
                    if (curItem != null)
                    {
                        // Item attributes
                        for (int j = 0; j < 3; j++)
                            if (curItem.Data.destAttributes[j] == r_attributeID)
                                attributeValue += curItem.Data.adjAttributes[j];
                        // Item awakenings
                        for (int j = 0; j < 3; j++)
                            if (curItem.c_awakenings[j].attribute == r_attributeID)
                                attributeValue += (curItem.c_awakenings[j].negative ? -1 : 1) * curItem.c_awakenings[j].value;
                    }
                }


                _lastAttributes[r_attributeID] = c_totalAttributes[r_attributeID];
                c_totalAttributes[r_attributeID] = attributeValue;
            }


            // Jewels
            int[] jewels = new int[] { 61, 62, 64 };
            int[] necklace_gore = new int[] { 82, 118, 154, 190, 226, 262, 298, 334, 370, 418, 466, 514, 562, 610, 658, 706, 766, 826, 886, 946, 1018 };
            int[] necklace_other = new int[] { 57, 65, 73, 81, 89, 97, 105, 113, 121, 129, 137, 146, 155, 164, 173, 182, 191, 200, 209, 218, 227 };
            int[] earring_plug = new int[] { 44, 49, 54, 59, 64, 70, 76, 82, 89, 96, 104, 113, 123, 135, 150, 168, 189, 213, 240, 270, 310 };
            int[] earring_demol = new int[] { 10, 15, 20, 25, 30, 35, 41, 48, 56, 66, 77, 90, 105, 125, 150, 170, 200, 235, 275, 320, 375 };
            int[] rings = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 13, 15, 17, 19, 21, 24, 27, 31, 35, 40 };
            foreach (int i in jewels)
            {
                curItem = GetSlotByPosition(i).c_item;
                if (curItem != null)
                {
                    switch (curItem.dwItemID)
                    {
                        case 26464: // vigor
                            c_totalAttributes[DST_STR] += rings[curItem.dwRefine]; break;
                        case 26465: // stam
                            c_totalAttributes[DST_STA] += rings[curItem.dwRefine]; break;
                        case 26466: // arek
                            c_totalAttributes[DST_DEX] += rings[curItem.dwRefine]; break;
                        case 26467: // intel
                            c_totalAttributes[DST_INT] += rings[curItem.dwRefine]; break;
                        case 26468: // demol
                            c_totalAttributes[DST_ATKPOWER] += earring_demol[curItem.dwRefine]; break;
                        case 26469: // plug
                            c_totalAttributes[DST_ADJDEF] += earring_plug[curItem.dwRefine]; break;
                        case 26470: // gore
                            c_totalAttributes[DST_HP_MAX] += necklace_gore[curItem.dwRefine]; break;
                        case 26471: // mental
                            c_totalAttributes[DST_MP_MAX] += necklace_other[curItem.dwRefine]; break;
                        case 26472: // paison
                            c_totalAttributes[DST_FP_MAX] += necklace_other[curItem.dwRefine]; break;
                    }
                }
            }
        }

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
            pak.Addint32(0);
            pak.Addint32(dwMySessionKey = WorldServer.c_random.Next());
            pak.Send(this);
            c_data = new PlayerData(this);
            c_attributes[DST_SPEED] = 100; //100% speed
            dataHandling();
            base.child = this;
            
        }
        /*        public void Write(string str, params object[] args)
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
        }*/
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

                        c_attributes[curBuff._skill.dwDestParam1] -= curBuff._skill.dwAdjParamVal1; //we delete effect of older buff
                        if (curBuff._skill.dwAdjParamVal2 != 0) //if there is a second effect
                            c_attributes[curBuff._skill.dwDestParam2] -= curBuff._skill.dwAdjParamVal2;

                    }
                }
                ///Ok here i will remove buff from cs item, sorry adidishen but like for buff by skill i had to remove them
                if (c_data.activateditem.Count > 0)
                {
                    for (int i = 0; i < c_data.activateditem.Count; i++)
                    {
                        ActiveItems curactive = c_data.activateditem[i];
                        if (curactive == null)
                            continue;
                        ItemData itemData = WorldHelper.GetItemDataByItemID(curactive.itemid);
                        switch (itemData.itemID)
                        {
                            case 10209: // Bull Hamstern !!

                                c_attributes[itemData.destAttributes[0]] -= itemData.adjAttributes[1];
                                SendPlayerAttribDecrease(itemData.destAttributes[0], itemData.adjAttributes[1]);
                                if (c_attributes[FlyFFAttributes.DST_HP] > c_data.f_MaxHP)
                                    c_attributes[FlyFFAttributes.DST_HP] = c_data.f_MaxHP;
                                break;

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
                this.c_socket.BeginReceive(dp.buffer, 0, 1452, SocketFlags.None, this.callback, dp);
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
                DataPacket[] dps = DataPacket.SplitNaglePackets(head_dp);
                for (int i = 0; i < dps.Length; i++)
                {
                    DataPacket dp = dps[i];
                    if (dp.VerifyHeaders(dwMySessionKey))
                        parseIncomingPackets(dp);
                    else
                        Log.Write(Log.MessageType.hack, "Possible hack attempt @ ClientMain::parseAsyncResult - probably packet editing (invalid packet headers!)");
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
            dp.dwPointer = 17;
            uint header = (uint)dp.Readint32();
            switch (header)
            {
                case RECV_SELFSPAWN_REQUEST: PlayerJoinWorld(dp); break;
                case RECV_PLAYER_CHAT: PlayerChat(dp); break;
                case RECV_MOVE_ITEM_INV: ItemMoveSlot(dp); break;
                case RECV_STATS_UPDATE: PlayerNewStats(dp); break;
                case RECV_SKILLS_UPDATE: PlayerNewSkills(dp); break;
                case RECV_CLICK_MOVE: MovementMouse(dp); break;
                case RECV_WASD_MOVE: MovementKeyboard(dp); break;
                case RECV_ELEMENT_REMOVAL: ItemRemoveElement(dp); break;
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
                case RECV_FRIEND_USERDATA: SendFriendDataAll(); break;
                //case RECV_SHOP_BUYITEM: PlayerShopBuyItem(dp); break;
                case RECV_FRIEND_DECLINE: FriendsRefuse(dp); break;
                case RECV_FRIEND_BLOCK: FriendsBlock(dp); break;
                case RECV_FRIEND_ADDBYMENU: FriendsAddMenu(dp); break;
                case RECV_FRIEND_ADDNEW: FriendsAddRemote(dp); break;
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
                case RECV_ATTACK: PlayerAttackMover(dp); break;
                case RECV_ATTACK_BOW: PlayerAttackMover(dp); break;
                case RECV_ATTACK_WAND: PlayerAttackMover(dp); break;
                case RECV_GUILDS_GDREQUEST: GuildDuelRequest(dp); break;
                case RECV_GUILDS_GDACCEPT: GuildDuelAccept(dp); break;
                case RECV_GUILDS_GDTRUCEINV: GuildDuelTruceRequest(dp); break;
                case RECV_GUILDS_GDTRUCE: GuildDuelTruceAccept(dp); break;
                case RECV_GUILDS_GDGIVEUP: GuildDuelGiveUp(dp); break;
                case RECV_GUILDS_SETSALARY: GuildSetSalary(dp); break;
                case RECV_GUILDS_SETTITLES: GuildSetTitle(dp); break;
                case RECV_TARGET_INFORMATION: PlayerSetTarget(dp); break;
                case RECV_PARTY_ACCEPT: AddPartyMember(dp); break;  //add by divinepunition for v11
                case RECV_PARTY_INVIT: inviteForParty(dp); break; //add by divinepunition for v11
                case RECV_PARTY_INVNO: declinePartyInvite(dp); break;  //add by divinepunition for v11
                case RECV_PARTY_GIVELEAD: giveLead(dp); break;   //add by divinepunition for v11
                case RECV_PARTY_KICK: partyKickUser(dp); break;   //add by divinepunition for v11
                case RECV_PARTY_ITEMDIST: partyChangeItemdistrib(dp); break;   //add by divinepunition for v11
                case RECV_HAIR_DESIGNER: PlayerNewHair(dp); break;
                case RECV_MAKEUP_ARTIST: PlayerNewFace(dp); break;
                case RECV_MOVEMENT_FOLLOW: MovementFollow(dp); break;
                case RECV_TELEPORT_ARENA: NPCArenaTeleport(true); break;
                case RECV_TELEPORT_EXITARENA: NPCArenaTeleport(false); break;
                case RECV_AWAKENING_REQUEST: ItemAwake(dp); break; // Nicco -> Awake item
                case RECV_CHANGE_JOB: ScrollChangeJob(dp); break; // Nicco -> changing job!
                case RECV_CHANGE_NAME: ScrollChangeName(dp); break; // Nicco -> changing name!
                case RECV_NPC_MAKESHINESTONE: NPCCreateShiningOricalkum(dp); break; // Nicco -> Shining oricalkum
                case RECV_SCROLL_AWAKE: ScrollAwakeSystem(dp); break; //divine for reduction scroll v11
                case RECV_REVERT_ITEM: RevertItem(dp); break; //divine for reduction scroll v11
                case RECV_SOCKET_ADDCARD: SocketAddCard(dp); break; //by divinepunition for v11
                case RECV_NPC_MAKEUNIQUEWEAPON: ItemCreateUniqueWeapon(dp); break; //by Divinepunition for v11
                case RECV_NPC_MAKEJEWEL: ItemCreateJewel(dp); break; //by divinepunition for v11
                case RECV_BANK_MENU: BankAskPassword(); break; //by divine for v11 bank system
                case RECV_BANK_NEWPASS: BankCreateNewPassword(dp); break;  //by divine for v11 bank system
                case RECV_BANK_PASS: BankCheckPassword(dp); break;  //by divine for v11 bank system
                case RECV_BANK_ADDITEM: BankAddItem(dp); break;  //by divine for v11 bank system
                case RECV_BANK_ADDPENYA: BankAddPenya(dp); break;   //by divine for v11 bank system
                case RECV_BANK_TAKEITEM: BankTakeItem(dp); break;   //by divine for v11 bank system
                case RECV_BANK_TAKEPENYA: BankTakePenya(dp); break; //by divine for v11 bank system
                case RECV_CREATEFOOD: PetCreateFood(dp); break; //by divinepunition for v11
                case RECV_MAIL_SHOWLIST: MailShowList(); break; //by divinepunition for v11
                case RECV_MAIL_SEND: MailSendMessage(dp); break; //by divinepunition for v11
                case RECV_MAIL_READ: MailReadMessage(dp); break; //by divinepunition for v11
                case RECV_MAIL_TAKEMONEY: MailTakeMoney(dp); break; //by divinepunition for v11
                case RECV_MAIL_TAKEOBJECT: MailTakeObject(dp); break; //by divinepunition for v11
                case RECV_MAIL_DELETE: MailDelete(dp); break; //by divinepunition for v11
                case RECV_RESURECT_NO: c_data.tempSkills = null; break; //if you say no delete temporary res skills
                case RECV_RESURRECT_YES: PlayerReviveBySkill(); break;
                case RECV_MAGE_TELEPORTDESTINY: Skills.MageTeleport(dp,this); break; //add by divinepunition for v11
                case RECV_UPDATE_ACTIONSLOT: PlayerUpdateActionSlot(dp); break;//add by divinepunition for v11
                case PAK_GUILDSIEGE_ENTER: GuildSiegeEnter(false); break;
                case PAK_GUILDSIEGE_CONFIRMENTER: GuildSiegeEnter(true); break;
                case RECV_COLLECT_START: CollectStart(); break; //add by Seymour for v11.01
                case RECV_COLLECT_STOP: CollectStop(); break; //add by Seymour for v11.01
                default:
                    {
                        if (Server.enable_debugging)
                        {
                            Log.Write(Log.MessageType.debug, "Unknown packet header received. Header: 0x{0}. Size: {1}",
                                header.ToString("X8"),
                                dp.dwSize);
#if DEBUG
                            Log.Write(Log.MessageType.debug, "Packet data: ");
                            //Log.Write(Log.MessageType.debug, Packet.MakePString(dp.buffer, dp.dwSize));
#endif
                        }
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
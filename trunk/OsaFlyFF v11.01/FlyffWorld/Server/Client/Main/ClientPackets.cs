using System;
using System.Collections.Generic;
using System.Text;

namespace FlyffWorld
{
    public partial class Client
    {
        public void MovementCorrect(DataPacket dp)
        {
            float x = dp.Readfloat(), y = dp.Readfloat(), z = dp.Readfloat();
            Point newpos = new Point(x, y, z);
            if (!c_position.IsInCircle(newpos, 2))
                return;
            lock (c_data.dataLock)
                c_position = c_destiny = newpos;
        }
        public void ItemAwake(DataPacket dp)
        {
            int dwUniqueID = dp.Readint();
            Slot slot = GetSlotByID(dwUniqueID);
            if (slot == null || slot.c_item.c_awakening != 0) 
                return;
            if (c_data.dwPenya < 100000)
            {
                SendMessageInfo(FlyFF.TID_GAME_LACKMONEY);
                return;
            }
            if (slot.c_item.Data.itemkind[0] != IK1_ARMOR && slot.c_item.Data.itemkind[0] != IK1_WEAPON)
            {
                SendMessageInfo(3469);
                return;
            }
            c_data.dwPenya -= 100000;
            slot.c_item.c_awakening = Awakening.Generate();
            SendItemAwakening(slot);
            SendEffect(FlyFF.INT_SUCCESS);
            SendMessageInfo(3574);
            SendPlayerPenya();
        }
        public void MovementFollow(DataPacket dp)
        {
            int moverID = dp.Readint();
            followMover(moverID);
            try
            {
                Mover mvr = MoversHandler.GetMover(moverID);
                if (mvr.MoverType == Mover.MOVER_ITEM) // Nicco->Drops
                    PickDrop((Drop)mvr);
            }
            catch (Exception)
            {
                SendMoverDespawn(moverID);
            }
            
        }
        public void PlayerNewFace(DataPacket dp)
        {
            int myID = dp.Readint(), face = dp.Readint();
            if (c_data.dwCharacterID != myID)
                return;
            if (c_data.dwFaceID != face)
            {
                if (1000000 > c_data.dwPenya)
                {
                    SendMessageInfo(TID_GAME_LACKMONEY);
                    return;
                }
                c_data.dwPenya -= 1000000;
                c_data.dwFaceID = face;
                SendPlayerNewFace();
                SendPlayerPenya();
            }
        }
        public void PlayerNewHair(DataPacket dp)
        {
            int style = dp.Readbyte(), b = dp.Readbyte(), g = dp.Readbyte(), r = dp.Readbyte();
            int penya = 0;
            if (style != c_data.dwHairID)
                penya += 2000000;
            if (r != c_data.c_haircolor.dwRed || b != c_data.c_haircolor.dwBlue || g != c_data.c_haircolor.dwGreen)
                penya += 4000000;
            if (penya > c_data.dwPenya)
            {
                SendMessageInfo(TID_GAME_LACKMONEY);
                return;
            }
            c_data.dwHairID = style;
            c_data.c_haircolor = new FlyffColor((byte)r, (byte)g, (byte)b, 0);
            c_data.dwPenya -= penya;
            SendPlayerNewHair();
            SendPlayerPenya();
        }
        public void ScrollChangeName(DataPacket dp)
        {
            int id = dp.Readint();
            string name = dp.Readstring();
            Slot slot = GetSlotByID(id);
            if (slot == null || slot.c_item == null || slot.c_item.dwItemID != SYS_SYS_SCR_CHANAM)
                return;
            ResultSet rs = new ResultSet("SELECT * FROM `flyff_characters` WHERE `flyff_charactername`='{0}' AND `flyff_olduserid`=-1", name);
            if (rs.Advance())
            {
                SendMessageInfoNotice("A character with this name already exists.");
                rs.Free();
                return;
            }
            rs.Free();
            Database.Execute("UPDATE `flyff_characters` SET `flyff_charactername`='{0}' WHERE `flyff_characterid`={1}", name, c_data.dwCharacterID);
            c_data.strPlayerName = name;
            DecreaseQuantity(slot);
            SendPlayerNameChange();
        }
        public void ScrollChangeJob(DataPacket dp)
        {
            int job = dp.Readint();
            JobState state = GetJobState(job), mystate = GetJobState(c_data.dwClass);
            if (state != mystate || (state != JobState.FirstJob && state != JobState.SecondJob))
                return;
            Slot slot = GetSlotByItemID(SYS_SYS_SCR_CHACLA);
            if (slot == null)
                return;
            DecreaseQuantity(slot);
            c_data.dwClass = job;
        }
        public void PlayerSetTarget(DataPacket dp)
        {
            int MoverID = dp.Readint();
            byte state = dp.Readbyte();
            /// state
            /// 1: clear target
            /// 2 & moverID != -1: set new target
            /// 2 & moverID == -1: set null target
            if (state == 1 && c_target != null)
            {
                // clear target
                if (bIsFighting && c_target.bIsFighting && c_target.dwMoverID == dwMoverID)
                {
                    c_target.bIsTargeted = false;
                    bIsFighting = false;
                }
                c_target = null;
            }
            else if (state == 2)
            {
                if (MoverID == -1)
                {
                    c_target = null;
                }
                else
                {
                    c_target = MoversHandler.GetMover(MoverID, c_spawns);
                    if (c_target == null)
                    {
                        Log.Write(Log.MessageType.warning, "setNewTarget(): Player {0} is trying to target an unknown mover: {1}", c_data.strPlayerName, MoverID);
#if DEBUG
                        SendMessageHud("#b#cffffff00[WARNING] trying to target an unknown mover: " + MoverID);
#endif
                    }
                }
            }
        }
        public void GuildDuelGiveUp(DataPacket dp)
        {
            int MyID = dp.Readint();
            Guild guild;
            if (MyID != c_data.dwCharacterID || c_data.dwGuildID < 1 || (guild = GuildHandler.getGuildByGuildID(c_data.dwGuildID)) == null || guild.founderID != c_data.dwCharacterID || guild.duelInfo.dwDuelID == 0)
                return;
            GuildHandler.GDGiveUp(guild.duelInfo, guild.guildID);
        }
        public void GuildDuelTruceAccept(DataPacket dp)
        {
            int MyID = dp.Readint();
            Guild guild;
            if (MyID != c_data.dwCharacterID || c_data.dwGuildID < 1 || (guild = GuildHandler.getGuildByGuildID(c_data.dwGuildID)) == null || guild.founderID != c_data.dwCharacterID || !guild.duelInfo.bTruceRequested || guild.duelInfo.dwDuelID == 0)
                return;
            GuildHandler.GDTruce(guild.duelInfo);
        }
        public void GuildDuelTruceRequest(DataPacket dp)
        {
            int MyID = dp.Readint();
            Guild guild;
            if (MyID != c_data.dwCharacterID || c_data.dwGuildID < 1 || (guild = GuildHandler.getGuildByGuildID(c_data.dwGuildID)) == null || guild.founderID != c_data.dwCharacterID || guild.duelInfo.dwDuelID == 0)
                return;
            Guild other = GuildHandler.getGuildByGuildID(guild.duelInfo.dwAttackerGuildID == guild.guildID ? guild.duelInfo.dwDefenderGuildID : guild.duelInfo.dwAttackerGuildID);
            if (other == null)
            {
                Log.Write(Log.MessageType.error, "Error finding enemy guild for truce! duelInfo: {0}", guild.duelInfo.ToString());
                SendMessageHud("Server error occured. Please contact server administrator.");
                return;
            }
            Client c = WorldHelper.GetClientByPlayerID(other.founderID);
            if (c == null)
            {
                Log.Write(Log.MessageType.error, "Error finding enemy guild's leader for truce!");
                SendMessageHud("Server error occured. Please contact server administrator.");
                return;
            }
            other.duelInfo.bTruceRequested = true;
            c.SendGuildDuelTruce();
        }
        public void GuildDuelRequest(DataPacket dp)
        {
            int MyID = dp.Readint();
            string strTarget = dp.Readstring();
            Guild guild;
            GuildMember me;
            if (MyID != c_data.dwCharacterID || c_data.dwGuildID < 1 || (guild = GuildHandler.getGuildByGuildID(c_data.dwGuildID)) == null || guild.founderID != c_data.dwCharacterID || (me = guild.getMember(c_data.dwCharacterID)) == null || guild.duelInfo.dwDuelID != 0 || strTarget == guild.guildName)
                return;
            // user can declare war (founder)
            Guild target = GuildHandler.getGuildByGuildName(strTarget);
            if (target == null)
            {
                // TODO: send notification "guild doesn't exist"
                return;
            }
            if (target.duelInfo.dwDuelID != 0 || guild.duelInfo.dwDuelID != 0)
            {
                // TODO: notification "other guild is busy"
                return;
            }
            target.duelInfo.dwAttackerGuildID = guild.guildID;
            target.duelInfo.dwDefenderGuildID = target.guildID;
            guild.duelInfo = target.duelInfo;
            // get leader client and send
            Client leader = WorldHelper.GetClientByPlayerID(target.founderID);
            if (leader == null)
            {
                // TODO: notification "offline leader"
                return;
            }
            leader.SendGuildDuelRequest(guild.guildID, c_data.strPlayerName);
        }
        public void GuildDuelAccept(DataPacket dp)
        {
            int MyID = dp.Readint();
            int target_gID = dp.Readint();
            Guild guild;
            if (MyID != c_data.dwCharacterID || c_data.dwGuildID < 1 || (guild = GuildHandler.getGuildByGuildID(c_data.dwGuildID)) == null || guild.founderID != c_data.dwCharacterID || guild.duelInfo.dwAttackerGuildID != target_gID)
                return;
            // Start new duel!
            GuildHandler.GDStart(guild.duelInfo);
        }
        public void GuildSetSalary(DataPacket dp)
        {
            int MyID = dp.Readint(), guildID = dp.Readint(), targetRank = dp.Readint(), salary = dp.Readint(); ;
            Guild guild;
            if (MyID != c_data.dwCharacterID || c_data.dwGuildID < 1 || c_data.dwGuildID != guildID || (guild = GuildHandler.getGuildByGuildID(c_data.dwGuildID)) == null || guild.founderID != c_data.dwCharacterID)
                return;
            LimitNumber(ref targetRank, 0, 4);
            guild.memberPayment[targetRank] = LimitNumber(salary, 0, 999999);
            SendGuildSalaryUpdate(targetRank, salary, guild.getClients());
            string[] title = new string[] { "Master", "Kingpin", "Captain", "Supporter", "Rookie" };
            Database.Execute("UPDATE flyff_guilds SET flyff_payment{0} = {1} WHERE flyff_guildID = '{2}'", title[targetRank], salary, guild.guildID);
        }
        public void GuildSetTitle(DataPacket dp)
        {
            int MyID = dp.Readint(), guildID = dp.Readint();
            Guild guild;
            if (MyID != c_data.dwCharacterID || c_data.dwGuildID < 1 || c_data.dwGuildID != guildID || (guild = GuildHandler.getGuildByGuildID(c_data.dwGuildID)) == null || guild.founderID != c_data.dwCharacterID)
                return;
            dp.Readint();
            for (int i = 1; i < 5; i++)
                guild.memberPrivileges[i] = dp.Readint();
            SendGuildTitlesUpdate(guild.getClients(), guild.memberPrivileges);
            string[] title = new string[] { "Master", "Kingpin", "Captain", "Supporter", "Rookie" };
            Database.Execute("UPDATE flyff_guilds SET flyff_privilegesMaster = 255, flyff_privilegesKingpin = {1}, flyff_privilegesCaptain = {2}, flyff_privilegesSupporter = {3}, flyff_privilegesRookie = {4} WHERE flyff_guildID = '{0}'", guild.guildID, guild.memberPrivileges[1], guild.memberPrivileges[2], guild.memberPrivileges[3], guild.memberPrivileges[4]);
        }
        public void GuildSetClass(DataPacket dp)
        {
            byte direction = dp.Readbyte();
            int MyID = dp.Readint();
            int DestID = dp.Readint();
            Guild guild;
            if (MyID != c_data.dwCharacterID || c_data.dwGuildID < 1 || (guild = GuildHandler.getGuildByGuildID(c_data.dwGuildID)) == null)
                return;
            GuildMember me = GuildHandler.getGuildMemberByGuildID(guild.guildID, c_data.dwCharacterID); //TODO: performance
            if (me == null)
                return;
            if ((guild.memberPrivileges[me.memberRank] & GuildHandler.PRIVILEGE_SETSYMBOLCOUNT) != GuildHandler.PRIVILEGE_SETSYMBOLCOUNT)
                return;
            GuildMember target = GuildHandler.getGuildMemberByGuildID(guild.guildID, DestID);
            if (target == null)
                return;
            // check direction, rank, class etc
            int myRank = me.memberRank;
            int theirRank = target.memberRank;
            int theirClass = target.memberRankSymbolCount;
            if (myRank >= theirRank || (direction == 1 && theirClass == 2) || (direction == 0 && theirClass == 0))
                return;
            GuildHandler.SetMemberClass(guild, target, direction == 1 ? theirClass + 1 : theirClass - 1);
        }
        public void GuildSetRank(DataPacket dp)
        {
            // Promote/demote, RECV_GUILDS_SETRANK, memberRank
            int myID = dp.Readint();
            int destID = dp.Readint();
            int destRank = dp.Readint();
            Guild guild;
            // supporter can't promote a rookie, even if the supporter has privileges.
            if (myID != c_data.dwCharacterID || c_data.dwGuildID < 1 || (guild = GuildHandler.getGuildByGuildID(c_data.dwGuildID)) == null)
                return;
            GuildMember me = GuildHandler.getGuildMemberByGuildID(guild.guildID, c_data.dwCharacterID);
            GuildMember target = GuildHandler.getGuildMemberByGuildID(guild.guildID, destID);
            if (me == null || target == null || (guild.memberPrivileges[me.memberRank] & GuildHandler.PRIVILEGE_PROMOTE) != GuildHandler.PRIVILEGE_PROMOTE || me.memberRank >= target.memberRank || destRank == me.memberRank || destRank < 1 || destRank > 4)
                return;
            GuildHandler.SetMemberRank(guild, target, destRank);
        }
        public void GuildSetLogo(DataPacket dp)
        {
            int logoID = dp.Readint();
            Guild guild;
            if (c_data.dwGuildID < 1 || (guild = GuildHandler.getGuildByGuildID(c_data.dwGuildID)) == null || guild.founderID != c_data.dwCharacterID || guild.guildLogo != 0)
                return;
            GuildHandler.SetGuildLogo(guild, logoID);
        }
        public void GuildRemoveMember(DataPacket dp)
        {
            int kickerID = dp.Readint();
            int kickeeID = dp.Readint();
            Guild guild;
            if (kickerID != c_data.dwCharacterID || c_data.dwGuildID < 1 || (guild = GuildHandler.getGuildByGuildID(c_data.dwGuildID)) == null || (kickerID != kickeeID && guild.founderID != kickerID) || (kickerID == kickeeID && guild.founderID == kickerID))
                return;
            GuildHandler.RemoveMember(guild, kickeeID);
            if (kickeeID == c_data.dwCharacterID)
            {
                SendGuildOnJoin(0);
                c_data.dwGuildID = -1;
            }
            else
            {
                Client other = WorldHelper.GetClientByPlayerID(kickeeID);
                if (other != null)
                {
                    other.SendGuildOnJoin(0);
                    other.c_data.dwGuildID = 0;
                }
            }
        }
        public void GuildAcceptInvitation(DataPacket dp)
        {
            if (guildInvitationData[INVITING_GUILD] == -1 || guildInvitationData[INVITING_CHARACTER] == -1)
                return;
            Guild guild;
            int sourceID = guildInvitationData[INVITING_CHARACTER];
            int sourceGuild = guildInvitationData[INVITING_GUILD];
            guildInvitationData = new int[] { -1, -1 };
            if (sourceID != dp.Readint() || c_data.dwCharacterID != dp.Readint() || WorldHelper.GetClientByPlayerID(sourceID) == null || (guild = GuildHandler.getGuildByGuildID(sourceGuild)) == null)
                return;
            c_data.dwGuildID = sourceGuild;
            GuildHandler.AddMember(guild, c_data.dwCharacterID, c_data.strPlayerName);
            SendGuildOnJoin(sourceGuild);
            SendGuildDataSingle(guild);
            SendGuildPlayer(guild);
        }
        public void GuildSendInvitation(DataPacket dp)
        {
            Guild guild;
            if (c_data.dwGuildID < 1 || (guild = GuildHandler.getGuildByGuildID(c_data.dwGuildID)) == null)
                return;
            GuildMember me = GuildHandler.getGuildMemberByGuildID(guild.guildID, c_data.dwCharacterID);
            if (me == null)
                return;
            if ((guild.memberPrivileges[me.memberRank] & GuildHandler.PRIVILEGE_INVITE) != GuildHandler.PRIVILEGE_INVITE)
                return;
            int otherMoverID = dp.Readint();
            Client other = WorldHelper.GetClientByMoverID(otherMoverID);
            if (other == null)
                return;
            if (other.c_data.dwGuildID > 0)
                return;
            SendMessageInfo(0x2B7, other.c_data.strPlayerName);
            other.guildInvitationData[INVITING_CHARACTER] = c_data.dwCharacterID;
            other.guildInvitationData[INVITING_GUILD] = guild.guildID;
            other.SendGuildInvitation(guild.guildID, c_data.dwCharacterID);
        }
        public void GuildDisband(DataPacket dp)
        {
            int charID = dp.Readint();
            Guild guild;
            if (charID != c_data.dwCharacterID || c_data.dwGuildID < 1 || (guild = GuildHandler.getGuildByGuildID(c_data.dwGuildID)) == null || guild.founderID != c_data.dwCharacterID)
                return;
            GuildHandler.DisbandGuild(guild, c_data.strPlayerName);
        }
        public void GuildSetNotice(DataPacket dp)
        {
            Guild guild;
            if (c_data.dwGuildID < 1 || (guild = GuildHandler.getGuildByGuildID(c_data.dwGuildID)) == null || guild.founderID != c_data.dwCharacterID)
                return;
            GuildHandler.SetGuildNotice(guild, dp.Readstring());
        }
        public void GuildSetName(DataPacket dp)
        {
            Guild guild;
            int ID = dp.Readint();
            if (c_data.dwCharacterID != ID)
                return;
            int myguild = dp.Readint();
            if (c_data.dwGuildID < 1 || myguild != c_data.dwGuildID || (guild = GuildHandler.getGuildByGuildID(c_data.dwGuildID)) == null || guild.guildName != "" || guild.founderID != c_data.dwCharacterID)
                return;
            GuildHandler.RenameGuild(c_data.dwGuildID, dp.Readstring());
        }
        public void PlayerReviveLodelight()
        {
            if (c_attributes[FlyFF.DST_HP] > 0)
                return; // user not dead
            // update hp mp
            c_attributes[FlyFF.DST_HP] = c_data.f_MaxHP * 30 / 100;
            c_attributes[FlyFF.DST_MP] = c_data.f_MaxMP * 30 / 100;
            SendMoverRevival();

            // New revival regions code.
            RevivalRegion region = null;
            for (int i = 0; i < WorldServer.data_resrgns.Count; i++)
            {
                if (WorldServer.data_resrgns[i].IsInRegion(c_position, c_data.dwMapID))
                {
                    region = WorldServer.data_resrgns[i];
                    break;
                }
            }
            if (region == null)
                region = WorldHelper.DefaultRevivalRegion;
            // set stuff o_O
            c_position = new Point(region.c_destiny.x, c_position.y, region.c_destiny.z, c_position.angle);
            if (region.dwDestMap != c_data.dwMapID)
            {
                c_data.dwMapID = region.dwDestMap;
                SendPlayerMapTransfer();
            }
            else
            {
                SendMoverNewPosition();
            }


            /// [Adidishen]
            /// New revival regions code by me -- the below is useless as CODE. the information is important
            /// Make sure to insert it to the database!!

            /*

            if ((c_position.x > 7180 && c_position.x < 9140) && (c_position.z > 1750 && c_position.z < 4530)) //Saint morning and rhisis
            {
                // teleport player to saint morning
                c_position.x = 8471;
                c_position.y = 100;
                c_position.z = 3636;
            }
            else if ((c_position.x > 4872 && c_position.x < 6250) && (c_position.z > 3200 && c_position.z < 4900))//darkon 1
            {
                // teleport player to ereon factory
                c_position.x = 5618;
                c_position.y = 75;
                c_position.z = 3868;
            }
            else if ((c_position.x > 1500 && c_position.x <= 4872) && (c_position.z > 3550 && c_position.z < 4900))//darkon 2 and north of darkon3
            {
                // teleport player to darken
                c_position.x = 3806;
                c_position.y = 59;
                c_position.z = 4450;
            }
            else if ((c_position.x > 1500 && c_position.x < 4500) && (c_position.z > 1850 && c_position.z <= 3550))//darkon 3
            {
                // teleport player ti darkon 3 shop
                c_position.x = 3214;
                c_position.y = 11;
                c_position.z = 3418;
            }
            else //by default flaris
            {
                // teleport player
                c_position.x = 6967;
                c_position.y = 100;
                c_position.z = 3333;
            }
            */

            int expLoss = (int)((double)ClientDB.EXP[c_data.dwLevel] / 100d * (double)ClientDB.EXPLossPercents[c_data.dwLevel]);
            c_data.qwExperience -= expLoss;
            SendPlayerCombatInfo();
         }
        public void PlayerReviveOriginal()
        {
            lock (c_data.dataLock)
            {
                if (c_attributes[DST_HP] > 0)
                    return;
                Slot slot = GetSlotByItemID(SYS_SYS_SCR_RESURRECTION);
                if (slot == null)
                    return;
                DecreaseQuantity(slot);
                c_attributes[DST_HP] = c_data.f_MaxHP * 30 / 100;
                SendMoverRevival();
            }
        }
        public void NPCBuffPang()
        {
            Client x;
            x = WorldHelper.GetClientByPlayerID(c_data.dwCharacterID);
            if ((x.c_data.dwLevel >= Server.helper_levelreq_min) && (x.c_data.dwLevel <= Server.helper_levelreq_max))
            {
                /*
                 * BuffPangConfig by Nicco
                 */

                // Check if cost is on, if on then check if player have that money...
                bool do_buff = true;
                if (Server.helper_buff_cost > 0)
                {
                    if (x.c_data.dwPenya >= Server.helper_buff_cost)
                    {
                        x.c_data.dwPenya = x.c_data.dwPenya - Server.helper_buff_cost;
                        x.SendPlayerPenya();
                    }
                    else do_buff = false;
                }
                // Buff the player
                if (do_buff)
                {
                    for (int i = 0; i < Server.helper_buffs.Count; i++)
                    {
                        Buff cBuff = new Buff();
                        cBuff.buffID = (int)Server.helper_buffs[i];
                        cBuff.buffLevel = (int)Server.helper_buffs_levels[i];
                        cBuff.buffTime = (int)Server.helper_buff_duration * 60 * 1000;
                        x.SendPlayerBuff(cBuff);
                        Buff.setBuffEffect(cBuff, x);
                    }
                }
                else SendMessageHud(Server.buff_cmsg);
            }
            else
            {
                SendMessageHud(Server.buff_emsg);
            }
        }
        public void NPCOpenDialog(DataPacket dp)
        {
            int npcMoverID = dp.Readint();
            string statetext = dp.Readstring();
            // we have an option to check npcmoverid and clientmoverid values, but we'll pass
            // prepare ncbd
            NPC npc = WorldHelper.GetNPCByMoverID(npcMoverID);
            if (npc == null)
                return;
            NPCChatBoxData ncbd = new NPCChatBoxData(npc.npc_dlg_txt_scrn_count, this, npc);
            NPCHandler.parseNpcChatRequest(ncbd, statetext,c_data.language);
        }
        public void NPCOpenShop(DataPacket dp)
        {
            int npcmoverid = dp.Readint();
            NPC npc = WorldHelper.GetNPCByMoverID(npcmoverid);
            if (npc == null)
                return;
            NPCShopData nsd = WorldHelper.GetNPCShop(npc.npc_type_name);
            if (nsd != null)
            {
                strTradeNPC = npc.npc_type_name;  // by me
                SendNPCShop(nsd, npcmoverid);
            }
            else
                Log.Write(Log.MessageType.warning, "No shop data was found for {0}!", npc.npc_type_name);
        }
        public void PlayerAttackMonster(DataPacket dp)
        {
            int motion = dp.Readint(), target_mID = dp.Readint();
            // The rest: junk (perhaps..)
            if (!bIsFighting || c_target == null || target_mID != c_target.dwMoverID)
            {
                c_target = MoversHandler.GetMover(target_mID);
                if (c_target == null || !(c_target is Monster))
                    return;
                bIsFighting = true;
            }
            if (c_target.bIsFighting && c_target.c_target.dwMoverID != dwMoverID && c_target.c_target.c_target != null && c_target.c_target.c_target.dwMoverID == c_target.dwMoverID)
            {
                SendMessageInfo(TID_GAME_PRIORITYMOB);
                return;
            }
            if (!c_target.bIsFighting || c_target.c_target.dwMoverID != dwMoverID)
            {
                Monster mob = Monster.getMobByMoverID(c_target.dwMoverID);
                mob.c_target = this;
                mob.bIsFollowing = true;
                mob.bIsFighting = true;
            }
            AttackFlags dwType = AttackFlags.NORMAL;
            int damage = 0, real_damage = 0, rate;
            if (!DiceRoller.Roll(rate = c_data.f_HitRate((Monster)c_target)))
            {
                dwType = AttackFlags.MISS;
                goto AfterDamageCalculations;
            }
            damage = DiceRoller.RandomNumber(c_data.f_DamageMin, c_data.f_DamageMax);
            if (DiceRoller.Roll(c_data.f_CritRate))
            {
                dwType |= AttackFlags.CRITICAL;
                damage += (int)(damage * 0.345678); // correct this
            }
            real_damage = c_target.c_attributes[DST_HP] - damage < 0 ? c_target.c_attributes[DST_HP] : damage;
        AfterDamageCalculations:
            SendPlayerAttackMotion(motion, c_target.dwMoverID);
            c_target.c_attributes[DST_HP] -= real_damage;
            c_target.SendMoverDamaged(dwMoverID, real_damage, (int)dwType);
            if (c_target.c_attributes[DST_HP] <= 0)
            {
                c_data.qwExperience += ((((Monster)c_target).Data.mobExpPoints) * Server.exp_rate);
                OnCheckLevelGroup();
                SendPlayerCombatInfo();
                c_target.SendMoverDeath();
                c_target.c_target = null;
                ((Monster)c_target).mob_OnDeath();
                DropMobItems((Monster)c_target, this, c_data.dwMapID);
            }
        }
        public void ItemCheckUpgrading(DataPacket dp)
        {
            int dwDst = dp.Readint();
            int dwSrc = dp.Readint();
            Slot src = GetSlotByID(dwSrc), dst = GetSlotByID(dwDst);
            if (src == null || src.c_item == null || dst == null || dst.c_item == null)
                return;
            if (!UpgradeSystem(dst, src))
                Log.Write(Log.MessageType.warning, "UpgradeSystem(): function evaluation returned false");
        }
        /// <summary>
        /// (RECEIVED PACKET)
        /// </summary>
        /// <param name="dp">(RECEIVED PACKET)</param>
        public void ItemDelete(DataPacket dp)
        {
            int dwDstID = dp.Readint();
            int dwQuantity = dp.Readint();
            Slot slot = GetSlotByID(dwDstID);
            if (slot == null || slot.c_item == null)
                return;
            DecreaseQuantity(slot, dwQuantity);
        }
        public void NPCBuyItem(DataPacket dp)
        {
            int dwTabID = dp.Readbyte();
            int dwTabPos = dp.Readbyte();
            int dwQuantity = dp.Readshort();
            int dwItemID = dp.Readint();
            NPCShopData shop = WorldHelper.GetNPCShop(strTradeNPC);
            if (shop == null)
            {
                Log.Write(Log.MessageType.hack, "[{0}] Attempt to buy an item from NPC {1} with no shop associated with the NPC.", c_data.strPlayerName, strTradeNPC);
                return;
            }
            if (shop.shopitems[dwTabID, dwTabPos].id != dwItemID)
            {
                Log.Write(Log.MessageType.hack, "[{0}] Attempt to buy an item from NPC {1} - the item ({2}) doesn't exist in the shop data.", c_data.strPlayerName, strTradeNPC, dwItemID);
            }
            Item item = new Item() { dwItemID = dwItemID };
            int dwPrice = item.Data.npcPrice * dwQuantity;
            if (c_data.dwPenya < dwPrice)
            {
                SendMessageInfo(TID_GAME_LACKMONEY);
                return;
            }
            Slot dstSlot = GetFirstAvailableSlot();
            if (dstSlot == null)
            {
                SendMessageInfo(TID_GAME_LACKSPACE);
                return;
            }
            c_data.dwPenya -= dwPrice;
            SendPlayerPenya();
            item.dwQuantity = dwQuantity;
            CreateItem(item, dstSlot);
        }
        public void NPCCloseShop(DataPacket dp)
        {
          strTradeNPC = "";
        }
        public void NPCSellItem(DataPacket dp)
        {
            int dwID = dp.Readbyte();
            int dwQuantity = dp.Readshort();
            Slot slot = GetSlotByID(dwID);
            if (slot == null || slot.c_item == null)
                return;
            c_data.dwPenya += slot.c_item.Data.npcPrice / 4 * dwQuantity;
            DecreaseQuantity(slot, dwQuantity);
            SendPlayerPenya();
        }
        public void NPCCreateShiningOricalkum(DataPacket dp)
        {
            /// [Adidishen]
            /// Converted to r10 item system base + optimized

            Slot suns = GetSlotByItemID(GEN_MAT_SUNSTONE);
            if (suns == null || suns.c_item == null || suns.c_item.dwQuantity < 5)
            {
                suns = GetSlotByItemID(2082);
                if (suns == null || suns.c_item == null || suns.c_item.dwQuantity < 5)
                {
                    return;
                }
            }
            Slot moons = GetSlotByItemID(2036);
            if (moons == null || moons.c_item == null || moons.c_item.dwQuantity < 5)
            {
                moons = GetSlotByItemID(2083);
                if (moons == null || moons.c_item == null || moons.c_item.dwQuantity < 5)
                {
                    return;
                }
            }
            Slot dst = GetFirstAvailableSlot();
            if (dst == null)
            {
                SendMessageInfo(TID_GAME_LACKSPACE);
                return;
            }
            DecreaseQuantity(suns, 5);
            DecreaseQuantity(moons, 5);
            CreateItem(new Item() { dwItemID = 2034 }, dst);
        }
        public void PlayerRegisterKeybind(DataPacket dp)
        {
            byte fSlot = dp.Readbyte();
            byte fKey = dp.Readbyte();
            int opcode = dp.Readint();
            int id = dp.Readint();
            string text = "";
            if (opcode == 0x08)
            {
                dp.IncreasePosition(16);
                text = dp.Readstring();
            }
            DeleteKeybind(fKey, fSlot);
            Keybind keybind = new Keybind();
            keybind.dwKeyIndex = fKey;
            keybind.dwPageIndex = fSlot;
            keybind.dwID = id;
            keybind.dwOperation = opcode;
            keybind.strText = text;
            c_data.keybinds.Add(keybind);
        }
        public void MovementKeyboard(DataPacket dp)
        {
            if (bIsFollowing)
                bIsFollowing = false;
            float x = dp.Readfloat();
            float y = dp.Readfloat();
            float z = dp.Readfloat();
            // unknown 3 integers.
            // Caali: rotation vector
            dp.Readint();
            dp.Readint();
            dp.Readint();
            float angle = dp.Readfloat();
            int moveFlags = dp.Readint();
            int motionFlags = dp.Readint();
            int actionFlags = dp.Readint();
            c_position = new Point(x, y, z, angle);
            if ((actionFlags & 5) == 5)
            {
                bIsKeyboardWalking = true;
                // Create a c_destiny that is far from the position...
                int vectorSize = 1000;
                float vectorX = (float)(c_position.x + (vectorSize * Math.Cos(angle * (Math.PI / 180))));
                float vectorZ = (float)(c_position.z + (vectorSize * Math.Sin(angle * (Math.PI / 180))));
                c_destiny.z -= vectorX;
                c_destiny.x += vectorZ;
            }
            else
            {
                bIsKeyboardWalking = false;
                c_position = c_destiny = new Point(x, y, z, angle);
            }
        }
        public void MovementMouse(DataPacket dp)
        {
            if (bIsFollowing)
                bIsFollowing = false;
            dp.ResetPointer(24);
            float x = dp.Readfloat();
            float y = dp.Readfloat();
            float z = dp.Readfloat();
            bIsKeyboardWalking = false;
            c_destiny = new Point(x, y, z);
            if (teleportMode)
            {
                c_position = c_destiny;
                SendMoverNewPosition();
            }
            else
                SendMoverNewDestination(c_destiny);
        }
        public void PlayerChat(DataPacket dp)
        {
            string saywhat = dp.Readstring();
            if (saywhat[0] == '.' || saywhat[0] == '/')
            {
                if (!parseCommands(saywhat.Substring(1)))
                    SendMoverChatBalloon(saywhat);
            }
            else if (saywhat[0] == '!' && c_data.dwAuthority >= 80)
                parseCommands("n2 " + saywhat.Substring(1));
            else
                SendMoverChatBalloon(saywhat);
        }
        public void ItemMoveSlot(DataPacket dp)
        {
            dp.Readbyte();
            int dwFrom = dp.Readbyte();
            int dwTo = dp.Readbyte();
            Slot from = GetSlotByPosition(dwFrom), to = GetSlotByPosition(dwTo);
            if (from == null || to == null || from.c_item == null)
            {
                Log.Write(Log.MessageType.error, "ItemMoveSlot(): source item is null! dwFrom = {0}, dwTo = {1}", dwFrom, dwTo);
                return;
            }
            if (to.c_item == null)
            {
                SwapSlots(from, to);
            }
            else
            {
                if (from.c_item.dwItemID == to.c_item.dwItemID) // TODO: PROPER CHECK!!!!
                {
                    int stack = from.c_item.Data.stackMax;
                    int newquantity = from.c_item.dwQuantity + to.c_item.dwQuantity;
                    if (newquantity > stack)
                    {
                        newquantity = stack - to.c_item.dwQuantity;
                        to.c_item.dwQuantity += newquantity;
                        from.c_item.dwQuantity -= newquantity;
                    }
                    else
                    {
                        to.c_item.dwQuantity += from.c_item.dwQuantity;
                        from.c_item = null;
                    }
                }
                else
                {
                    SwapSlots(from, to);
                }
            }
            SendItemPositionMove(dwFrom, dwTo);
        }
        public void PlayerJoinWorld(DataPacket dp)
        {
            dp.ResetPointer(69);
            string
                playerName = dp.Readstring(),
                username = dp.Readstring(),
                password = dp.Readstring();
            ResultSet rs = new ResultSet("SELECT * FROM flyff_characters LEFT JOIN flyff_accounts ON flyff_characters.flyff_userid=flyff_accounts.flyff_userid WHERE flyff_accounts.flyff_accountname='{0}' AND flyff_accounts.flyff_passwordhash='{1}' AND flyff_characters.flyff_charactername='{2}'",
                Database.Escape(username),
                Database.Escape(password),
                Database.Escape(playerName));
            if (!rs.Advance())
            {
                Log.Write(Log.MessageType.info, "Remote client disconnected from {0}.", c_socket.RemoteEndPoint.ToString());
                this.c_socket.Close();
                return;
            }

            Log.Write(Log.MessageType.info, "User \"{0}\" is logging in with character \"{1}\".", username, playerName);
            c_data.strUsername = username;
            c_data.strPassword = password;
            c_data.strPlayerName = playerName;
            short sizemod = (short)rs.Readint("flyff_charactersize");
            if (sizemod > 1000) sizemod = 1000;
            dwMoverSize = sizemod;
            c_attributes[DST_HP] = rs.Readint("flyff_currenthitpts");
            c_data.dwGender = rs.Readint("flyff_gender");
            c_data.c_haircolor = new FlyffColor(rs.Readstring("flyff_haircolor"));
            c_data.dwHairID = rs.Readint("flyff_hairstyle");
            c_data.qwExperience = rs.Readlong("flyff_experiencepoints");
            c_data.dwFaceID = rs.Readint("flyff_facemodel");
            c_data.dwCharacterID = rs.Readint("flyff_characterid");
            c_data.dwClass = rs.Readint("flyff_jobid");
            c_attributes[DST_STR] = rs.Readint("flyff_strength");
            c_attributes[DST_STA] = rs.Readint("flyff_stamina");
            c_attributes[DST_INT] = rs.Readint("flyff_intelligence");
            c_attributes[DST_DEX] = rs.Readint("flyff_dexterity");
            c_data.dwLevel = rs.Readint("flyff_level");
            c_data.dwMapID = rs.Readint("flyff_mapid");
            c_data.dwAuthority = rs.Readint("flyff_authoritylevel");
            c_data.dwPenya = rs.Readint("flyff_penya");
            c_data.dwKarma = rs.Readint("flyff_pkpoints");
            c_position.x = rs.Readfloat("flyff_positionx");
            c_position.y = rs.Readfloat("flyff_positiony");
            c_position.z = rs.Readfloat("flyff_positionz");
            c_destiny = c_position;
            c_data.dwReputation = rs.Readint("flyff_pvppoints");
            c_data.dwDisposition = rs.Readint("flyff_disposition");
            c_attributes[FlyFF.DST_MP] = rs.Readint("flyff_currentmanapts");
            c_attributes[FlyFF.DST_FP] = rs.Readint("flyff_currentforcepts");
            c_data.dwFlyLevel = rs.Readint("flyff_flyinglevel");
            c_data.dwFlyEXP = rs.Readint("flyff_flyingexp");
            c_data.dwSkillPoints = rs.Readint("flyff_skillpoints");
            c_data.dwStatPoints = rs.Readint("flyff_statpoints");
            c_data.dwCharSlot = rs.Readint("flyff_characterslot");
            ResultSet rs2 = new ResultSet("SELECT flyff_guildID FROM flyff_guildmembers WHERE flyff_characterID = {0}", c_data.dwCharacterID);
            if (rs2.Advance())
                c_data.dwGuildID = rs2.Readint("flyff_guildID");
            else
                c_data.dwGuildID = -1;
            c_data.dwNetworkStatus = rs.Readint("flyff_msgState");
            c_data.m_MotionFlags = rs.Readint("flyff_motionflags");
            c_data.m_MovingFlags = rs.Readint("flyff_moveflags");
            c_data.m_PlayerFlags = rs.Readint("flyff_playerflags");
            c_data.bank.strPassword = rs.Readstring("flyff_bankpassword");
            c_data.dwAccountID = rs.Readint("flyff_userid");
            Database.Execute("UPDATE flyff_accounts SET flyff_linestatus='{0}',flyff_lastcharacter={2} WHERE flyff_userid={1}",
                Server.clusterid.ToString().PadLeft(2, '0') + "0" + Server.worldid.ToString().PadLeft(2, '0'), c_data.dwAccountID, c_data.dwCharacterID);
            for (int i = 0; i < 3; i++)
                c_data.bank.dwPenyaArr[i] = rs.Readint("flyff_bankpenya:" + i);
            DateTime s1 = rs.Readtime("flyff_baglastuntil:1"), s2 = rs.Readtime("flyff_baglastuntil:2");
            rs.Free();
            rs = new ResultSet("SELECT flyff_characterid FROM flyff_characters WHERE flyff_userid={0}", c_data.dwAccountID);
            {
                int i = 0;
                while (rs.Advance())
                    if (i > 2)
                        break;
                    else
                        c_data.bank.dwCharacterIDArr[i++] = rs.Readint("flyff_characterid");
            }
            rs.Free();  

            rs = new ResultSet("SELECT * FROM flyff_skills WHERE flyff_characterid={0} ORDER BY flyff_skillid ASC", c_data.dwCharacterID);
            while (rs.Advance())
            {
                Skill skill = new Skill();
                skill.dwSkillID = rs.Readint("flyff_skillid");
                skill.dwSkillLevel = rs.Readint("flyff_skilllevel");
                c_data.skills.Add(skill);
             }
            rs.Free();
            rs = new ResultSet("SELECT * FROM flyff_accounts WHERE flyff_userid={0}", c_data.dwAccountID);
            while (rs.Advance())
            {
                c_data.language = rs.Readint("flyff_lang");
            }
            rs.Free();
            //when Player spawn we must send to him his buffs
            rs = new ResultSet("SELECT * FROM flyff_buffs WHERE flyff_charid={0}", c_data.dwCharacterID);
           
                Client target = WorldHelper.GetClientByPlayerID(c_data.dwCharacterID);
                while (rs.Advance())
                {
                    Buff buff = new Buff();
                    buff.buffID = rs.Readint("flyff_buffid");
                    buff.buffLevel = rs.Readint("flyff_buffLevel");
                    buff.buffAbility1 = rs.Readint("flyff_ability1");
                    buff.buffAbilityType1 = rs.Readint("flyff_abilityType1");
                    buff.buffAbility2 = rs.Readint("flyff_ability2");
                    buff.buffAbilityType2 = rs.Readint("flyff_abilityType2");
                    buff.buffChangeParam1 = rs.Readint("flyff_buffChangeParam1");
                    buff.buffChangeParam2 = rs.Readint("flyff_buffChangeParam2");
                    buff.probability = rs.Readint("flyff_probability");
                    buff.probability_PVP = rs.Readint("flyff_probability_PVP");                 
                    buff.buffTime = rs.Readint("flyff_remainingTime");
                    c_data.buffs.Add(buff);
                    

                }
            
            rs.Free();
            rs = new ResultSet("SELECT * FROM flyff_items WHERE flyff_characterid = '{0}'", c_data.dwCharacterID);
            Item item;
            while (rs.Advance())
            {
                string sockets = rs.Readstring("flyff_sockets");
                if (sockets == "")
                {
                    item = new Item();
                }
                else
                {
                    string[] socketdata = rs.Readstring("flyff_sockets").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    item = new Item(socketdata.Length);
                    for (int i = 0; i < socketdata.Length; i++)
                    {
                        try
                        {
                            item.c_sockets[i] = int.Parse(socketdata[i]);
                        }
                        catch
                        {
                            item.c_sockets[i] = 0;
                        }
                    }
                }
                item.dwElement = rs.Readint("flyff_elementType");
                item.dwEleRefine = rs.Readint("flyff_elementRefine");
                item.dwItemID = rs.Readint("flyff_itemid");
                item.dwQuantity = rs.Readint("flyff_itemcount");
                item.dwRefine = rs.Readint("flyff_refinelevel");
                item.c_awakening = rs.Readlong("flyff_awakening");
                item.bExpired = rs.Readbool("flyff_itemexpired");
                int slot = rs.Readint("flyff_slotnum");
                GetSlotByPosition(slot).c_item = item;
            }
            rs.Free();
            rs = new ResultSet("SELECT * FROM flyff_equipments WHERE flyff_characterid='{0}'", c_data.dwCharacterID);
            while (rs.Advance())
            {
                string sockets = rs.Readstring("flyff_sockets");
                if (sockets == "")
                {
                    item = new Item();
                }
                else
                {
                    string[] socketdata = rs.Readstring("flyff_sockets").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    item = new Item(socketdata.Length);
                    for (int i = 0; i < socketdata.Length; i++)
                    {
                        try
                        {
                            item.c_sockets[i] = int.Parse(socketdata[i]);
                        }
                        catch
                        {
                            item.c_sockets[i] = 0;
                        }
                    }
                }
                item.dwElement = rs.Readint("flyff_elementType");
                item.dwEleRefine = rs.Readint("flyff_elementRefine");
                item.dwItemID = rs.Readint("flyff_itemid");
                item.dwQuantity = rs.Readint("flyff_itemcount");
                item.dwRefine = rs.Readint("flyff_refinelevel");
                item.c_awakening = rs.Readlong("flyff_awakening");
                item.bExpired = rs.Readbool("flyff_itemexpired");
                int slot = rs.Readint("flyff_slotnum");
                GetSlotByPosition(slot).c_item = item;
            }
            rs.Free();
            bool bBag1Enabled = s1 > DateTime.Now, bBag2Enabled = s2 > DateTime.Now;
            c_data.basebag = new Bag(true, true);
            c_data.bag1 = new Bag(bBag1Enabled, false, s1);
            c_data.bag2 = new Bag(bBag2Enabled, false, s2);
            // TODO: bags!
            // #obsolete
            //rs.Free();
            /*bool bag1e = false, bag2e = false;
            if (s1 > DateTime.Now)
                bag1e = true;
            if (s2 > DateTime.Now)
                bag2e = true;
            c_data.basebag = new Bag(true, true);
            c_data.bag1 = new Bag(bag1e, false, s1);
            c_data.bag2 = new Bag(bag2e, false, s2);
            rs = new ResultSet("SELECT * FROM flyff_bagitems WHERE flyff_characterid='{0}' AND flyff_bagnumber=0", c_data.dwCharacterID);
            while (rs.Advance())
            {
                string[] socketdata = rs.Readstring("flyff_sockets").Split(',');
                currentItem = new Item(socketdata.Length);
                currentItem.element = rs.Readint("flyff_elementType");
                currentItem.eRefine = rs.Readint("flyff_elementRefine");
                currentItem.itemid = rs.Readint("flyff_itemid");
                currentItem.quantity = rs.Readint("flyff_itemcount");
                currentItem.refine = rs.Readint("flyff_refinelevel");
                for (int i = 0; i < socketdata.Length; i++)
                {
                    try
                    {
                        currentItem.sockets[i] = int.Parse(socketdata[i]);
                    }
                    catch (Exception)
                    {
                        currentItem.sockets[i] = 0;
                    }
                }
                currentItem.itemslot = rs.Readint("flyff_slotnum");
                currentItem.awakening = rs.Readlong("flyff_awakening");
                currentItem.expired = rs.Readbool("flyff_itemexpired");
                currentItem.uniqueid = currentItem.itemslot;
                c_data.basebag.itempos.Add(currentItem.itemslot);
                c_data.basebag.items.Add(currentItem);
            }
            if (bag1e)
            {
                rs.Free();
                rs = new ResultSet("SELECT * FROM flyff_bagitems WHERE flyff_characterid='{0}' AND flyff_bagnumber=1", c_data.dwCharacterID);
                while (rs.Advance())
                {
                    string[] socketdata = rs.Readstring("flyff_sockets").Split(',');
                    currentItem = new Item(socketdata.Length);
                    currentItem.element = rs.Readint("flyff_elementType");
                    currentItem.eRefine = rs.Readint("flyff_elementRefine");
                    currentItem.itemid = rs.Readint("flyff_itemid");
                    currentItem.quantity = rs.Readint("flyff_itemcount");
                    currentItem.refine = rs.Readint("flyff_refinelevel");
                    for (int i = 0; i < socketdata.Length; i++)
                    {
                        try
                        {
                            currentItem.sockets[i] = int.Parse(socketdata[i]);
                        }
                        catch (Exception)
                        {
                            currentItem.sockets[i] = 0;
                        }
                    }
                    currentItem.itemslot = rs.Readint("flyff_slotnum");
                    currentItem.awakening = rs.Readlong("flyff_awakening");
                    currentItem.expired = rs.Readbool("flyff_itemexpired");
                    currentItem.uniqueid = currentItem.itemslot;
                    c_data.bag1.itempos.Add(currentItem.itemslot);
                    c_data.bag1.items.Add(currentItem);
                }
            }
            if (bag2e)
            {
                rs.Free();
                rs = new ResultSet("SELECT * FROM flyff_bagitems WHERE flyff_characterid='{0}' AND flyff_bagnumber=2", c_data.dwCharacterID);
                while (rs.Advance())
                {
                    string[] socketdata = rs.Readstring("flyff_sockets").Split(',');
                    currentItem = new Item(socketdata.Length);
                    currentItem.element = rs.Readint("flyff_elementType");
                    currentItem.eRefine = rs.Readint("flyff_elementRefine");
                    currentItem.itemid = rs.Readint("flyff_itemid");
                    currentItem.quantity = rs.Readint("flyff_itemcount");
                    currentItem.refine = rs.Readint("flyff_refinelevel");
                    for (int i = 0; i < socketdata.Length; i++)
                    {
                        try
                        {
                            currentItem.sockets[i] = int.Parse(socketdata[i]);
                        }
                        catch (Exception)
                        {
                            currentItem.sockets[i] = 0;
                        }
                    }
                    currentItem.itemslot = rs.Readint("flyff_slotnum");
                    currentItem.awakening = rs.Readlong("flyff_awakening");
                    currentItem.expired = rs.Readbool("flyff_itemexpired");
                    currentItem.uniqueid = currentItem.itemslot;
                    c_data.bag2.itempos.Add(currentItem.itemslot);
                    c_data.bag2.items.Add(currentItem);
                }
            }
            rs.Free();*/
            rs = new ResultSet("SELECT * FROM flyff_friends WHERE flyff_charid1={0}", c_data.dwCharacterID);
            while (rs.Advance())
            {
                int friendid = rs.Readint("flyff_charid2");
                int blocked = rs.Readint("flyff_blocked");
                Friend friend = BuildFriend(friendid, blocked);
                if (friend == null)
                    continue;
                c_data.friends.Add(friend);
            }
            rs.Free();
            rs = new ResultSet("SELECT COUNT(*) mailCount FROM flyff_mails WHERE flyff_charid={0}", c_data.dwCharacterID);
            if (rs.Advance())
                if (rs.Readint("mailCount") > 0)
                    c_data.m_PlayerFlags |= PlayerFlags.NEWMAIL;
            rs.Free();
            rs = new ResultSet("SELECT * FROM flyff_hotslots WHERE flyff_characterid={0}", c_data.dwCharacterID);
            while (rs.Advance())
            {
                Hotslot currentSlot = new Hotslot();
                currentSlot.dwID = rs.Readint("flyff_hotslotid");
                currentSlot.dwOperation = rs.Readint("flyff_opcode");
                currentSlot.dwSlot = rs.Readint("flyff_slot");
                currentSlot.strText = rs.Readstring("flyff_text");
                c_data.hotslots.Add(currentSlot);
            }
            rs.Free();
            rs = new ResultSet("SELECT * FROM flyff_keybinds WHERE flyff_characterid={0}", c_data.dwCharacterID);
            while (rs.Advance())
            {
                Keybind currentBind = new Keybind();
                currentBind.dwID = rs.Readint("flyff_keybindid");
                currentBind.dwOperation = rs.Readint("flyff_opcode");
                currentBind.dwPageIndex = rs.Readint("flyff_fslot");
                currentBind.strText = rs.Readstring("flyff_text");
                currentBind.dwKeyIndex = rs.Readint("flyff_fkey");
                c_data.keybinds.Add(currentBind);
            }
            rs.Free();
            Guild guild;
            if (c_data.dwGuildID > 0)
            {
                guild = GuildHandler.getGuildByGuildID(c_data.dwGuildID);
                if (guild == null)
                {
                    Log.Write(Log.MessageType.error, "Player #{0} is in guild ID #{1} but the guild was not found in world_guilds!", c_data.dwCharacterID, c_data.dwGuildID);
                    c_data.dwGuildID = -1;
                }
            }
            else
                guild = null;
            SendPlayerSpawnSelf();
            SendGuildDataAll();
            if (Server.enable_wmessage)
                SendMessageHud(Server.welcome_message);
            playerSpawned = true;
            timers.ResetAll();
            SendFriendOnConnect();
            SendMessageEventNotice(400);
            SendMessageInfo(400);
            if (guild != null)
                SendGuildDataSingle(guild);
            scrollprotection = new ScrollProtection(c_data.dwCharacterID, false);
            if (c_data.buffs.Count > 0) //if buff are in database we buff the player
            {

                for (int i = 0; i < c_data.buffs.Count; i++)
                {
                    Buff curBuff = c_data.buffs[i];
                    int[] listBuffEffect = BuffDB.getBuffBonus(curBuff);

                    target.c_attributes[listBuffEffect[1]] += listBuffEffect[0]; //we delete effect of older buff
                    target.SendPlayerAttribRaise(listBuffEffect[1], listBuffEffect[0]);

                    if (listBuffEffect[2] != 0 && listBuffEffect[3] != 0)//if buff is GT
                    {
                        target.c_attributes[listBuffEffect[3]] += listBuffEffect[2];
                        target.SendPlayerAttribRaise(listBuffEffect[3], listBuffEffect[2]);
                    }
                    //we send buff
                    target.SendPlayerBuff(curBuff);

                }

                target.isBuffed = true; //start a timercheck
            }
        }
        public void PlayerNewStats(DataPacket dp)
        {
            int sp = c_data.dwStatPoints;
            if (sp < 1)
                return;
            int str = dp.Readint(),
                sta = dp.Readint(),
                dex = dp.Readint(),
                Int = dp.Readint();
            int pointsToDecrease = str + sta + dex + Int;
            if (pointsToDecrease > sp)
                return;
            c_data.dwStatPoints -= pointsToDecrease;
            c_attributes[DST_STR] += str;
            c_attributes[DST_STA] += sta;
            c_attributes[DST_DEX] += dex;
            c_attributes[DST_INT] += Int;
            SendPlayerStats();
        }
        public void PlayerNewSkills(DataPacket dp)
        {
            if (c_data.dwSkillPoints < 1)
                return;
            for (int i = 0; i < 45; i++)
            {
                int id;
                Skill skill;
                if ((id = dp.Readint()) == -1 || (skill = GetSkillByID(id)) == null)
                {
                    dp.Readint();
                    continue;
                }
                int pointsPerLevel = 1;
                int level = dp.Readint();
                bool hasBeenDecided = false;
                if (level <= skill.dwSkillLevel)
                    continue;
                if (id == 1 || id == 2 || id == 3)
                    hasBeenDecided = true;
                if (!hasBeenDecided)
                    for (int l = 1; l <= 4; l++)
                    {
                        int[] tree = SkillTree.GetTree(l);
                        if (ArrayContains(tree, id))
                        {
                            hasBeenDecided = true;
                            pointsPerLevel = 2;
                            break;
                        }
                    }
                if (!hasBeenDecided)
                    for (int l = 6; l <= 13; l++)
                        if (ArrayContains(SkillTree.GetTree(l), id))
                        {
                            hasBeenDecided = true;
                            pointsPerLevel = 3;
                            break;
                        }
                if (!hasBeenDecided)
                    continue;
                Log.Write(Log.MessageType.debug, "Skill ID: {0}. pointsPerLevel: {1}. skilllevel: {2}. level: {3}. total: {4}. sp: {5}", id, pointsPerLevel, skill.dwSkillLevel, level, pointsPerLevel * (level - skill.dwSkillLevel), c_data.dwSkillPoints);
                int total = pointsPerLevel * (level - skill.dwSkillLevel);
                if (c_data.dwSkillPoints < total)
                    break;
                c_data.dwSkillPoints -= total;
                skill.dwSkillLevel = level;
            }
            SendPlayerSkills();
        }
        public void ItemRemoveCard(DataPacket dp)
        {
            /// Original snippet by BlackGiant
            int dwID = dp.Readbyte();
            Slot slot = GetSlotByID(dwID);
            if (slot == null || slot.c_item == null)
            {
                return;
            }
            int dwLastSlot = slot.c_item.c_sockets.Length - 1;
            if (dwLastSlot < 0 || slot.c_item.c_sockets.Length == 0)
                return;
            if (c_data.dwPenya < 1000000)
            {
                SendMessageInfo(TID_GAME_LACKMONEY);
                return;
            }
            slot.c_item.c_sockets[dwLastSlot] = 0;
            c_data.dwPenya -= 1000000;
            SendPlayerPenya();
            SendItemUpdate(new ItemUpdateStatus(slot.dwID, ITEM_MODTYPE_CARD, 0));
        }
        public void ItemUnequip(DataPacket dp)
        {
            // Contribution of Caali
            int id = dp.Readint();
            Slot slot = GetSlotByID(id);
            if (slot == null || slot.c_item == null)
            {
                return;
            }
            int to = slot.dwPos - 0x2A;
            Slot target = GetFirstAvailableSlot();
            if (target == null)
            {
                SendMessageInfo(TID_GAME_LACKSPACE);
                return;
            }
            SwapSlots(slot, target);
            OnRemoveItems();
            SendPlayerEquiping(slot, to, false);
        }
        public void ItemEquip(DataPacket dp)
        {
            // Contribution of Caali
            dp.Readshort();
            int id = dp.Readshort();
            dp.Readint();
            Slot slot = GetSlotByID(id);
            if (slot == null)
            {
                return;
            }
            if (slot.c_item != null)
            {
                if (ProcessItemUsage(slot))
                    return;
                int dwTarget = slot.c_item.Data.equipSlot[0] + 0x2A;
                Slot target = GetSlotByPosition(dwTarget);
                if (target == null)
                    return;
                if (target.c_item != null)
                {
                    bool bFound = false;
                    for (int i = 0; i < 73; i++)
                    {
                        if (c_data.inventory[i].dwPos >= 42)
                            continue;
                        if (c_data.inventory[i].c_item == null)
                        {
                            target.dwPos = c_data.inventory[i].dwPos;
                            c_data.inventory[i].dwPos = slot.dwPos;
                            bFound = true;
                            break;
                        }
                    }
                    if (!bFound)
                        target.dwPos = slot.dwPos;
                    slot.dwPos = dwTarget;
                }
                else
                {
                    bool bFound = false;
                    for (int i = 0; i < 73; i++)
                    {
                        if (c_data.inventory[i].dwPos < 42 || i == slot.dwID)
                            continue;
                        if (c_data.inventory[i].c_item == null)
                        {
                            target.dwPos = c_data.inventory[i].dwPos;
                            c_data.inventory[i].dwPos = slot.dwPos;
                            bFound = true;
                            break;
                        }
                    }
                    if (!bFound)
                        target.dwPos = slot.dwPos;
                    slot.dwPos = dwTarget;
                }
                OnEquipItems();
                SendPlayerEquiping(slot, slot.dwPos - 0x2A, true);
            }
        }
        public void PlayerRegisterHotslot(DataPacket dp)
        {
            int slot = dp.Readbyte();
            int opcode = dp.Readint();
            int id = dp.Readint();
            string text = "";
            dp.IncreasePosition(16); // 2x(int)0 (int)slot 2x(int)0
            if (opcode == 0x08)
                text = dp.Readstring();
            Hotslot hotslot = new Hotslot();
            hotslot.dwOperation = opcode;
            hotslot.dwSlot = slot;
            hotslot.strText = text;
            hotslot.dwID = id;
            c_data.hotslots.Add(hotslot);
        }
        public void PlayerDeleteKeybind(DataPacket dp)
        {
            int fSlot = dp.Readbyte();
            int fKey = dp.Readbyte();
            for (int i = 0; i < c_data.keybinds.Count; i++)
            {
                Keybind current = c_data.keybinds[i];
                if (current.dwPageIndex == fSlot && current.dwKeyIndex == fKey)
                    c_data.keybinds.Remove(current);
            }
        }
        public void PlayerDeleteHotslot(DataPacket dp)
        {
            int slot = dp.Readbyte();
            for (int i = 0; i < c_data.hotslots.Count; i++)
            {
                Hotslot current = c_data.hotslots[i];
                if (current.dwSlot == slot)
                    c_data.hotslots.Remove(current);
            }
        }
        public void NPCArenaTeleport(bool state)
        {
            if (state)
            {
                //we modify character position for server
                c_position.x = 541;
                c_position.y = 139;
                c_position.z = 495;
                c_data.dwMapID = FlyffWorlds.WORLD_ARENA;
                SendPlayerMapTransfer();//we send to the client
            }
            else
            {
                c_position.x = 6936;
                c_position.y = 100;
                c_position.z = 3257;
                c_data.dwMapID = FlyffWorlds.WORLD_MADRIGAL;
                SendPlayerMapTransfer();//we send to the client
            }

        }
        public void PlayerCastSkill(DataPacket dp)
        {
            dp.Readshort();
            int skillIndex = dp.Readshort();
            Skill skill = GetSkillByIndex(skillIndex);
            int moverIDTarget = dp.Readint();
            Client target;
            if (moverIDTarget == dwMoverID)
                target = this;
            else
                target = WorldHelper.GetClientByMoverID(moverIDTarget);
            if (target == null)
                target = this;//just for allowing function to work in case target is null
                //return;
            Client user = this;
            //we're looking if player has the skill and which level he has
            for (int i = 0; i < c_data.skills.Count; i++)
            {
                Skill curSkill = c_data.skills[i];
                if (curSkill.dwSkillID == skill.dwSkillID)
                {
                    
                    SkillData myskills = SkillData.getSkillByNameAndLevel(curSkill.dwSkillID, curSkill.dwSkillLevel);
                    // Determine type of the skill...
                    
                    switch (SkillData.skillType(skill.dwSkillID))
                    {

                        case "buff":
                            Buff buff = new Buff();
                            buff.buffID = skill.dwSkillID;
                            buff.buffLevel = skill.dwSkillLevel;
                            buff.buffTime = BuffDB.getBuffInterval(skill, c_attributes[DST_INT]); // 5 minutes const int - TODO
                            Buff.setBuffEffect(buff, target);
                            break;
                        case "attack":
                            //send to an attack function
                            break;
                        case "heal": SkillFunctions.healTarget(myskills, target); Log.Write(Log.MessageType.debug, "Je suis dans le heal du switch"); break;
                        case "resurrect":SkillFunctions.resurrectPlayer(myskills, target, user); break;
                        case "teleport":
                            //send to a teleport function
                            break;
                        case "debuff":
                            //send to a debuff function
                            break;
                        case "cure":
                            //send to a cure function
                            break;
                        case "buffwitheffect":
                            //send to a buffwitheffect function
                            break;
                        case "antibuff":
                            //send to an antibuff function
                            break;
                        case "unknown":
                        default:
                            Log.Write(Log.MessageType.debug, "skill which ID is : {0} can t be identify", skill.dwSkillID);
                            break;


                    }
                    
                    SkillFunctions.decreaseMPFP(myskills, user); // we decrease mana from user of the skill
                    SendPlayerSkillMotion(skill.dwSkillID, target);
                }
                
            }
        }
        public void FriendsAddMenu(DataPacket dp)
        {
            if (dp.Readint() != c_data.dwCharacterID)
                return;
            int dwID = dp.Readint();
            Client other = WorldHelper.GetClientByPlayerID(dwID);
            if (other == null)
            {
                // don't alert user is offline if it's null.. aka System.NullReferenceException.
                return;
            }
            SendFriendInvitation(other);
        }
        public void FriendsAdd(DataPacket dp)
        {
            int charid = dp.Readint();
            if (charid != c_data.dwCharacterID)
                return;
            string otherpname = dp.Readstring();
            if (otherpname.ToLower() == c_data.strPlayerName.ToLower())
                return;
            Client otherclient = WorldHelper.GetClientByPlayerName(otherpname);
            if (otherclient == null)
            {
                SendMessageUserOffline(otherpname);
                return;
            }
            SendFriendInvitation(otherclient);
        }
        public void FriendsChangeStatus(DataPacket dp)
        {
            int charid = dp.Readint();
            if (charid != c_data.dwCharacterID)
                return;
            int status = dp.Readint();
            c_data.dwNetworkStatus = status;
            SendFriendNewStatus();
        }
        public void FriendsAccept(DataPacket dp)
        {
            int otherfriend = dp.Readint();
            int currentID = dp.Readint();
            if (currentID != c_data.dwCharacterID)
                return;
            Client otherclient = WorldHelper.GetClientByPlayerID(otherfriend);
            if (otherclient == null)
                return;
            Friend thisfriend = BuildFriend(this);
            Friend other = BuildFriend(otherclient);
            SendFriendNew(otherfriend, otherclient.c_data.strPlayerName);
            SendFriendData(other);
            SendMessageInfo(TID_GAME_MSGINVATECOM, otherclient.c_data.strPlayerName);
            otherclient.SendFriendNew(c_data.dwCharacterID, c_data.strPlayerName);
            otherclient.SendFriendData(thisfriend);
            otherclient.SendMessageInfo(TID_GAME_MSGINVATECOM, c_data.strPlayerName);
            c_data.friends.Add(other);
            otherclient.c_data.friends.Add(thisfriend);
        }
        public void FriendsDelete(DataPacket dp)
        {
            int myid = dp.Readint();
            if (myid != c_data.dwCharacterID)
                return;
            int otherID = dp.Readint();
            Friend other = GetFriendByID(otherID);
            if (other == null)
                return;
            c_data.friends.Remove(other);
            SendFriendDelete(otherID);
            Client otherclient = WorldHelper.GetClientByPlayerID(otherID);
            if (otherclient != null)
                SendFriendOnDisconnect(otherclient);
        }
        /// [divinepunition]
        /// Some more stuff added during friend system rewrite.

        public void FriendsRefuse(DataPacket dp)
        {
            int dwSender = dp.Readint();
            int dwReceiver = dp.Readint();
            Client dst = WorldHelper.GetClientByPlayerID(dwSender);
            SendFriendRefuse(dst);
        }
        public void FriendsBlock(DataPacket dp)
        {
            if (dp.Readint() != c_data.dwCharacterID)
                return;
            int dwID = dp.Readint();
            Client other = WorldHelper.GetClientByPlayerID(dwID);
            if (other == null)
            {
                Log.Write(Log.MessageType.error, "FriendsBlock(): @ other == null");
                return;
            }
            for (int i = 0; i < c_data.friends.Count; i++)
            {
                if (c_data.friends[i].dwCharacterID == dwID)
                {
                    if (c_data.friends[i].nBlocked == 0)
                    {
                        c_data.friends[i].nBlocked = 1;
                        SendFriendBlocked(other);
                    }
                    else
                    {
                        c_data.friends[i].nBlocked = 0;
                        SendFriendUnblocked(other);
                    }
                    break;
                }
            }
        }
    }
}

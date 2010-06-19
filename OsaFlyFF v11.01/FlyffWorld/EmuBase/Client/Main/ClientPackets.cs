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
            int dwUniqueID = dp.Readint32();
            Slot slot = GetSlotByID(dwUniqueID);
            if (slot == null)
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
            if (slot.c_item.c_awakening != 0)
            {
                if (slot.c_item.c_awakening < 0x8000000000000000) //if it's not reduction A
                {
                    if (slot.c_item.c_awakening < 0x4000000000000000) //if it's not reduction B
                        return;
                    else //if it's reduction B
                    {
                        slot.c_item.c_awakening = Awakening.Generate();
                        slot.c_item.c_awakening += 0x4000000000000000;
                    }
                }
                else //if it's reduction A
                {
                    slot.c_item.c_awakening = Awakening.Generate();
                    slot.c_item.c_awakening += 0x8000000000000000;
                }
            }
            else
                slot.c_item.c_awakening = Awakening.Generate();
            SendItemAwakening(slot);
            SendEffect(FlyFF.INT_SUCCESS);
            SendMessageInfo(3574);
            SendPlayerPenya();
        }
        public void ScrollAwakeSystem(DataPacket dp)
        {
            int slotscroll = dp.Readint32();
            int slotitem = dp.Readint32();
            if (slotitem == null || slotscroll == null)
                return;
            Slot scroll = GetSlotByPosition(slotscroll);
            Slot itemslot = GetSlotByPosition(slotitem);
            if (scroll == null || itemslot == null)
                return;

            ulong awakevalue = 0;
            switch (scroll.c_item.dwItemID)
            {
                case 26458:
                case 26459: //scroll of reduction A and B
                    if (scroll.c_item.dwItemID == 26458)
                        awakevalue = 0x4000000000000000;
                    if (scroll.c_item.dwItemID == 26459)
                        awakevalue = 0x8000000000000000;
                    if (itemslot.c_item.c_awakening != 0)
                        itemslot.c_item.c_awakening += awakevalue;
                    else
                        itemslot.c_item.c_awakening = awakevalue;
                    break;
                case 26462: //scroll of reversion

                    if (itemslot.c_item.c_awakening == 0)//item is not awake
                        return;
                    if (itemslot.c_item.c_awakening < 0x8000000000000000) //we don't have scroll of reduction A effect
                    {
                        if (itemslot.c_item.c_awakening < 0x4000000000000000) //we don't have scroll of reduction B effect
                            awakevalue = 0;
                        else //we have reduction B effect
                            awakevalue = 0x4000000000000000;
                    }
                    else //we have reduction A effect
                        awakevalue = 0x8000000000000000;

                    itemslot.c_item.c_awakening = awakevalue;
                    break;
                default: return; break;
            }

            SendItemAwakening(itemslot);
            DecreaseQuantity(scroll, 1);

        }
        public void ItemCreateUniqueWeapon(DataPacket dp)
        {
            int weaponslotid = dp.Readint32();
            int jewelslotid = dp.Readint32();
            int sunstoneslotid = dp.Readint32();


            Slot weaponslot = GetSlotByPosition(weaponslotid);
            Slot jewelslot = GetSlotByPosition(jewelslotid);
            Slot sunstoneslot = GetSlotByPosition(sunstoneslotid);
            if (weaponslot == null || jewelslot == null || sunstoneslot == null)
                return;

            //We don't need to check if we have correct jewel because client do this

            Item item = new Item();

            if (DiceRoller.Roll(2)) //2% of chance
            {
                //success we create an unique item corresponding to weapon used
                SendEffect(INT_SUCCESS);
                SendPlayerSound(SND_INF_UPGRADESUCCESS);
                //ok now we have to determin with unique or ultimate item could result
                if (weaponslot.c_item.dwItemID >= 22026 && weaponslot.c_item.dwItemID <= 22065) //if it's already an unique weapon
                    item = ExchangeByUltimWeapon(weaponslot);
                else
                    item = ExchangeByUniqWeapon(weaponslot);
                CreateItem(item);
                SendValidItemUpdate();
                //you success or failed, All item are destroyed
                DecreaseQuantity(weaponslot);
                DecreaseQuantity(jewelslot);
                DecreaseQuantity(sunstoneslot);
                SendNPCMessageBoxResult(0, MESSAGEBOX_CREATEWEAPON);
                return;

            }

            //failed so we need to just delete item and send sound and effet for failed
            SendEffect(INT_FAIL);
            SendPlayerSound(SND_INF_UPGRADEFAIL);
            SendValidItemUpdate();
            //you success or failed, All item are destroyed
            DecreaseQuantity(weaponslot);
            DecreaseQuantity(jewelslot);
            DecreaseQuantity(sunstoneslot);
            SendNPCMessageBoxResult(1, MESSAGEBOX_CREATEWEAPON);

        }
        public void ItemCreateJewel(DataPacket dp)
        {
            int itemslotid = dp.Readint32();

            Slot weapon = GetSlotByPosition(itemslotid);
            if (weapon == null)
                return;
            int chance = 5 + 5 * weapon.c_item.dwRefine;
            if (DiceRoller.Roll(chance))
            {
                Item item = new Item();
                item.dwQuantity = 1 + weapon.c_item.dwRefine / 3;
                //success we will create a new item corresponding to level of weapon
                if (weapon.c_item.Data.reqLevel >= 60 && weapon.c_item.Data.reqLevel <= 70)
                    item.dwItemID = 2033; //topaze
                else if (weapon.c_item.Data.reqLevel >= 71 && weapon.c_item.Data.reqLevel <= 85)
                    item.dwItemID = 2032; //ruby
                else if (weapon.c_item.Data.reqLevel >= 86 && weapon.c_item.Data.reqLevel <= 100)
                    item.dwItemID = 2031; //saphire
                else if (weapon.c_item.Data.reqLevel >= 101 && weapon.c_item.Data.reqLevel <= 119)
                    item.dwItemID = 2030; //Emerald
                else if (weapon.c_item.Data.reqLevel >= 120)
                    item.dwItemID = 2029; //Diamond
                SendEffect(INT_SUCCESS);
                SendPlayerSound(SND_INF_UPGRADESUCCESS);
                for (int i = 0; i < c_data.inventory.Count; i++)
                {
                    Slot curslot = c_data.inventory[i];
                    if (curslot == null)
                        continue;
                    if (curslot.c_item.dwItemID == item.dwItemID) //if we already have this item type
                    {
                        if (curslot.c_item.dwQuantity == item.Data.stackMax)
                            continue;
                        curslot.c_item.dwQuantity += item.dwQuantity;
                        if (curslot.c_item.dwQuantity > curslot.c_item.Data.stackMax) //if we have more than stackmax
                        {
                            int remaining = curslot.c_item.dwQuantity - curslot.c_item.Data.stackMax;
                            curslot.c_item.dwQuantity = curslot.c_item.Data.stackMax;
                            //update actual qantity and create new item for remaining
                            SendItemUpdate(new ItemUpdateStatus(curslot.dwID, ITEM_MODTYPE_QUANTITY, curslot.c_item.dwQuantity, 0));
                            item.dwQuantity = remaining;
                            CreateItem(item);
                            SendNPCMessageBoxResult(0, MESSAGEBOX_CREATEJEWEL);
                            SendValidItemUpdate();
                            DecreaseQuantity(weapon);
                            return;
                        }
                        else
                        {
                            //we have less that stackmax
                            SendItemUpdate(new ItemUpdateStatus(curslot.dwID, ITEM_MODTYPE_QUANTITY, curslot.c_item.dwQuantity, 0));
                            SendNPCMessageBoxResult(0, MESSAGEBOX_CREATEJEWEL);
                            SendValidItemUpdate();
                            DecreaseQuantity(weapon);
                            return;
                        }
                    }
                }

                //we are here if we haven't any other in inventory
                CreateItem(item);
                SendNPCMessageBoxResult(0, MESSAGEBOX_CREATEJEWEL);
                SendValidItemUpdate();
                DecreaseQuantity(weapon);
                return;
            }
            else
            {
                //failed so we need to just delete item and send sound and effet for failed
                SendEffect(INT_FAIL);
                SendPlayerSound(SND_INF_UPGRADEFAIL);
                SendNPCMessageBoxResult(1, MESSAGEBOX_CREATEJEWEL);
                DecreaseQuantity(weapon);
                SendValidItemUpdate();
            }
        }
        public void MovementFollow(DataPacket dp)
        {
            
            int moverID = dp.Readint32();
            int range = dp.Readint32();
            
                Log.Write(Log.MessageType.debug, "we send a follow packet");
                //followMover(moverID, range);
           
            try
            {
                Mover mvr = MoversHandler.GetMover(moverID);
                if (mvr.MoverType == Mover.MOVER_ITEM) // Nicco->Drops
                    PickDrop((Drop)mvr);
            }
            catch
            {
                SendMoverDespawn(moverID);
            }
            
        }
        public void PlayerNewFace(DataPacket dp)
        {
            int myID = dp.Readint32(), face = dp.Readint32();
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
            int id = dp.Readint32();
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
        public void RevertItem(DataPacket dp) //to delete scroll of reduction effect
        {
            int itemslotid = dp.Readint32();
            Slot item = GetSlotByPosition(itemslotid);
            if (item == null)
                return;
            if (c_data.dwPenya < 100000)
            {
                SendMessageInfo(FlyFF.TID_GAME_LACKMONEY);
                return;
            }
            c_data.dwPenya -= 100000;
            Log.Write(Log.MessageType.debug, "Awake value is : {0}", item.c_item.c_awakening);
            if (item.c_item.c_awakening < 0x8000000000000000)
            {
                if (item.c_item.c_awakening < 0x4000000000000000)
                    return;
                else
                    item.c_item.c_awakening -= 0x4000000000000000;
            }
            else
                item.c_item.c_awakening -= 0x8000000000000000;

            SendPlayerPenya();
            SendItemAwakening(item);
        }
        public void ScrollChangeJob(DataPacket dp)
        {
            int job = dp.Readint32();
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
            int MoverID = dp.Readint32();
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
            int MyID = dp.Readint32();
            Guild guild;
            if (MyID != c_data.dwCharacterID || c_data.dwGuildID < 1 || (guild = GuildHandler.getGuildByGuildID(c_data.dwGuildID)) == null || guild.founderID != c_data.dwCharacterID || guild.duelInfo.dwDuelID == 0)
                return;
            GuildHandler.GDGiveUp(guild.duelInfo, guild.guildID);
        }
        public void GuildDuelTruceAccept(DataPacket dp)
        {
            int MyID = dp.Readint32();
            Guild guild;
            if (MyID != c_data.dwCharacterID || c_data.dwGuildID < 1 || (guild = GuildHandler.getGuildByGuildID(c_data.dwGuildID)) == null || guild.founderID != c_data.dwCharacterID || !guild.duelInfo.bTruceRequested || guild.duelInfo.dwDuelID == 0)
                return;
            GuildHandler.GDTruce(guild.duelInfo);
        }
        public void GuildDuelTruceRequest(DataPacket dp)
        {
            int MyID = dp.Readint32();
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
            int MyID = dp.Readint32();
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
            int MyID = dp.Readint32();
            int target_gID = dp.Readint32();
            Guild guild;
            if (MyID != c_data.dwCharacterID || c_data.dwGuildID < 1 || (guild = GuildHandler.getGuildByGuildID(c_data.dwGuildID)) == null || guild.founderID != c_data.dwCharacterID || guild.duelInfo.dwAttackerGuildID != target_gID)
                return;
            // Start new duel!
            GuildHandler.GDStart(guild.duelInfo);
        }
        public void GuildSetSalary(DataPacket dp)
        {
            int MyID = dp.Readint32(), guildID = dp.Readint32(), targetRank = dp.Readint32(), salary = dp.Readint32(); ;
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
            int MyID = dp.Readint32(), guildID = dp.Readint32();
            Guild guild;
            if (MyID != c_data.dwCharacterID || c_data.dwGuildID < 1 || c_data.dwGuildID != guildID || (guild = GuildHandler.getGuildByGuildID(c_data.dwGuildID)) == null || guild.founderID != c_data.dwCharacterID)
                return;
            dp.Readint32();
            for (int i = 1; i < 5; i++)
                guild.memberPrivileges[i] = dp.Readint32();
            SendGuildTitlesUpdate(guild.getClients(), guild.memberPrivileges);
            string[] title = new string[] { "Master", "Kingpin", "Captain", "Supporter", "Rookie" };
            Database.Execute("UPDATE flyff_guilds SET flyff_privilegesMaster = 255, flyff_privilegesKingpin = {1}, flyff_privilegesCaptain = {2}, flyff_privilegesSupporter = {3}, flyff_privilegesRookie = {4} WHERE flyff_guildID = '{0}'", guild.guildID, guild.memberPrivileges[1], guild.memberPrivileges[2], guild.memberPrivileges[3], guild.memberPrivileges[4]);
        }
        public void GuildSetClass(DataPacket dp)
        {
            byte direction = dp.Readbyte();
            int MyID = dp.Readint32();
            int DestID = dp.Readint32();
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
            int myID = dp.Readint32();
            int destID = dp.Readint32();
            int destRank = dp.Readint32();
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
            int logoID = dp.Readint32();
            Guild guild;
            if (c_data.dwGuildID < 1 || (guild = GuildHandler.getGuildByGuildID(c_data.dwGuildID)) == null || guild.founderID != c_data.dwCharacterID || guild.guildLogo != 0)
                return;
            GuildHandler.SetGuildLogo(guild, logoID);
        }
        public void GuildRemoveMember(DataPacket dp)
        {
            int kickerID = dp.Readint32();
            int kickeeID = dp.Readint32();
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
            if (sourceID != dp.Readint32() || c_data.dwCharacterID != dp.Readint32() || WorldHelper.GetClientByPlayerID(sourceID) == null || (guild = GuildHandler.getGuildByGuildID(sourceGuild)) == null)
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
            int otherMoverID = dp.Readint32();
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
            int charID = dp.Readint32();
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
            int ID = dp.Readint32();
            if (c_data.dwCharacterID != ID)
                return;
            int myguild = dp.Readint32();
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
            ActiveItems activeitem = GetPlayerActivatedItem(10430);
            if (activeitem != null) //get if player has scroll of blessing activated
            {

                //we delete activated item from activated item list
                c_data.activateditem.Remove(activeitem);
                
                //send debuff itm
                SendPlayerRemoveBuffByItem(activeitem.itemid);
            }
            else
            {
                int expLoss = (int)((double)ClientDB.EXP[c_data.dwLevel] / 100d * (double)ClientDB.EXPLossPercents[c_data.dwLevel]);
                c_data.qwExperience -= expLoss;
                SendPlayerCombatInfo();
            }
            return;
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
        public void PlayerReviveBySkill() //case someone resurrection with resurrect skill by divinepunition
        {
            //try to get temporary skill used to resurrect
            if (c_data.tempSkills != null)
            {
                Skills curSkill = c_data.tempSkills;
                // update hp
                c_attributes[FlyFF.DST_HP] = curSkill.dwAdjParamVal1;
                //delete screen of lodelight
                SendPlayerResurrectScreenRemoval();
                //determine exp loosing

                if (c_data.qwExperience > 0)
                {
                    c_data.qwExperience -= ClientDB.EXP[c_data.dwLevel] * 4 / 100 * curSkill.dwAdjParamVal2 / 100;
                    if (c_data.qwExperience < 0)
                        c_data.qwExperience = 0;
                    SendPlayerCombatInfo();
                }
                SendMoverRevival();
                c_data.tempSkills = null;

            }
            else //curiously we don't have the temporary skills...
            {
                // update hp mp
                c_attributes[FlyFF.DST_HP] = c_data.f_MaxHP * 30 / 100;

                //determine exp loosing
                if (c_data.qwExperience > 0)
                {
                    c_data.qwExperience -= ClientDB.EXP[c_data.dwLevel] * 4 / 100;
                    if (c_data.qwExperience < 0)
                        c_data.qwExperience = 0;
                    SendPlayerCombatInfo();
                }
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
                        Skills skill = Skills.getSkillByNameIDAndLevel((int)Server.helper_buffs[i], (int)Server.helper_buffs_levels[i]);
                        cBuff._skill = skill;
                        cBuff.dwTime = (int)Server.helper_buff_duration * 60 * 1000;
                        x.SendPlayerBuff(cBuff);
                        Skills.setBuffEffect(cBuff, x);
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
            int npcMoverID = dp.Readint32();
            string statetext = dp.Readstring();
            // we have an option to check npcmoverid and clientmoverid values, but we'll pass
            // prepare ncbd
            NPC npc = WorldHelper.GetNPCByMoverID(npcMoverID);
            if (npc == null)
                return;
            NPCChatBoxData ncbd = new NPCChatBoxData(this, npc);
            NPCHandler.parseNpcChatRequest(ncbd, statetext, c_data.dwLanguage);
        }
        public void NPCOpenShop(DataPacket dp)
        {
            int npcmoverid = dp.Readint32();
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
        public void PlayerAttackMover(DataPacket dp)
        {
            
            int motion = dp.Readint32(), target_mID = dp.Readint32();
            // The rest: junk (perhaps..)
            if (!bIsFighting || c_target == null || target_mID != c_target.dwMoverID)
            {
                c_target = MoversHandler.GetMover(target_mID);
                if (c_target == null)
                    return;
                bIsFighting = true;
            }
            if (c_target.bIsFighting && c_target.c_target.dwMoverID != dwMoverID && c_target.c_target.c_target != null && c_target.c_target.c_target.dwMoverID == c_target.dwMoverID)
            {
                SendMessageInfo(TID_GAME_PRIORITYMOB);
                return;
            }
            Monster mob = null;
            Client target = null;
            if (!(c_target is Monster))
            {
                //Manage PK system)
                target = WorldHelper.GetClientByMoverID(c_target.dwMoverID);
                return;
            }
            else
            {
                 mob = Monster.getMobByMoverID(c_target.dwMoverID);
                 if (!c_target.bIsFighting || c_target.c_target.dwMoverID != dwMoverID)
                 {
                
                        mob.c_target = this;
                        mob.bIsFollowing = true;
                        mob.bIsFighting = true;
                  }
                  Slot slot = GetSlotByPosition(52);
                  AttackFlags dwType = AttackFlags.NORMAL;//attack flag
                  if (slot !=null && slot.c_item != null)
                  {
                      if (slot.c_item.Data.itemkind[2] == FlyffItemkind.IK3_BOW)
                      {
                          dwType = AttackFlags.BOW;
                          //we decrease arrow :
                          Slot arrow = GetSlotByPosition(67);
                          if (arrow == null)
                              return;
                          else
                          {
                              c_data.inventory[arrow.dwID].c_item.dwQuantity -= 1;
                              SendItemUpdate(new ItemUpdateStatus(arrow.dwID, ITEM_MODTYPE_QUANTITY, arrow.c_item.dwQuantity, 0));

                          }
                      }
                  }
                  
            int damage = 0, real_damage = 0, rate;
            if (!DiceRoller.Roll(rate = c_data.f_HitRate((Monster)c_target)))
            {
                dwType = AttackFlags.MISS;
                goto AfterDamageCalculations;
            }
            damage = c_data.f_CalculateDamageAgainstMob(mob);
            if (DiceRoller.Roll(c_data.f_CritRate))
            {
                dwType = AttackFlags.CRITICAL;
                if (mob.Data.mobLevel>c_data.dwLevel)
                damage += DiceRoller.RandomNumber(mob.Data.atkMin,mob.Data.atkMax); // same that 2*base damage +...-...
                else
                    damage += (DiceRoller.RandomNumber(mob.Data.atkMin, mob.Data.atkMax) / 10);
            }
            real_damage = c_target.c_attributes[DST_HP] - damage < 0 ? c_target.c_attributes[DST_HP] : damage;
                //if mob is buffed with holycross double damage
                if (mob.antibuffs.Count>0)
                    for (int i = 0; i < mob.antibuffs.Count; i++)
                    {
                        if (mob.antibuffs[i]._skill.dwNameID == 145) //holycross
                            real_damage *= 2;
                    }
        AfterDamageCalculations:
            SendPlayerAttackMotion(motion, c_target.dwMoverID);
            c_target.c_attributes[DST_HP] -= real_damage;
            c_target.SendMoverDamaged(dwMoverID, real_damage, dwType,c_target.c_position,0);
                //now we check if a one of the effect on layer work again mob (stun, poison etc)               
            ManageBuffEffectonMover(c_target, this);
            if (c_target.c_attributes[DST_HP] <= 0)
            {
                int xprate = 1;
                if (c_data.dwLevel < 60) xprate = Server.exp_rate;
                else if (c_data.dwLevel >= 60 && c_data.dwLevel < 90) xprate = Server.exp_rate60;
                else if (c_data.dwLevel >= 90) xprate = Server.exp_rate90;
                c_data.qwExperience += (int)((double)(((Monster)c_target).Data.mobExpPoints) * (double)xprate * c_data.f_RateModifierEXP);
                OnCheckLevelGroup();
                SendPlayerCombatInfo();
                c_target.SendMoverDeath();
                c_target.c_target = null;
                c_target.bIsBlocked = false;
                ((Monster)c_target).mob_OnDeath();
                DropMobItems((Monster)c_target, this, c_data.dwMapID);
                DelayedActions.ClearMoverAntibuffAndDelayed(c_target);
            }
            }
        }
        public void ItemCheckUpgrading(DataPacket dp)
        {
            int dwDst = dp.Readint32();
            int dwSrc = dp.Readint32();
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
            int dwDstID = dp.Readint32();
            int dwQuantity = dp.Readint32();
            Slot slot = GetSlotByID(dwDstID);
            if (slot == null || slot.c_item == null)
                return;
            DecreaseQuantity(slot, dwQuantity);
        }
        public void NPCBuyItem(DataPacket dp)
        {
            int dwTabID = dp.Readbyte();
            int dwTabPos = dp.Readbyte();
            int dwQuantity = dp.Readint16();
            int dwItemID = dp.Readint32();
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
            int dwQuantity = dp.Readint16();
            Slot slot = GetSlotByID(dwID);
            if (slot == null || slot.c_item == null)
                return;
            c_data.dwPenya += slot.c_item.Data.npcPrice / 4 * dwQuantity;
            DecreaseQuantity(slot, dwQuantity);
            SendPlayerPenya();
        }
        public void NPCCreateShiningOricalkum(DataPacket dp)
        {
            /// [Divinepunition]
            /// Rewrited wih packet structure 
            for (int i = 0; i < 5; i++)
            {
                int sunstoneslotid = dp.Readint32();
                Slot sunslot = GetSlotByID(sunstoneslotid);
                DecreaseQuantity(sunslot);
            }
            for (int i = 0; i < 5; i++)
            {
                int moonstoneslotid = dp.Readint32();
                Slot moonslot = GetSlotByID(moonstoneslotid);
                DecreaseQuantity(moonslot);
            }
            Slot dst = GetFirstAvailableSlot();
            if (dst == null)
            {
                SendMessageInfo(TID_GAME_LACKSPACE);
                return;
            }
            
            CreateItem(new Item() { dwItemID = 2034,dwQuantity=1 }, dst);
            CreateShiningsunstonevalid();
        }
        public void PlayerRegisterKeybind(DataPacket dp)
        {
            byte fSlot = dp.Readbyte();
            byte fKey = dp.Readbyte();
            int opcode = dp.Readint32();
            int id = dp.Readint32();
            string text = "";
            if (opcode == 0x08)
            {
                dp.dwPointer += 16;
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
            if (c_attributes[FlyFF.DST_CHRSTATE] == 524288 || c_attributes[FlyFF.DST_CHRSTATE] == 1048576) //if player is blocked caused du blade hero skill
                return;
            if (bIsFollowing)
                bIsFollowing = false;
            float x = dp.Readfloat();
            float y = dp.Readfloat();
            float z = dp.Readfloat();
            // unknown 3 integers.
            // Caali: rotation vector
            dp.Readint32();
            dp.Readint32();
            dp.Readint32();
            float angle = dp.Readfloat();
            int moveFlags = dp.Readint32();
            int motionFlags = dp.Readint32();
            int actionFlags = dp.Readint32();
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
            SendMoverMovement(dp);
        }
        public void MovementMouse(DataPacket dp)
        {
            if (c_attributes[FlyFF.DST_CHRSTATE] == 524288 || c_attributes[FlyFF.DST_CHRSTATE] == 1048576) //if player is blocked caused du blade hero skill
                return;
            if (bIsFollowing)
                bIsFollowing = false;
            dp.dwPointer = 24;
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
                    SendMoverChatBalloon(StringUtilities.FilterChat(saywhat));
            }
            else if (saywhat[0] == '!' && c_data.dwAuthority >= 80)
                parseCommands("n2 " + saywhat.Substring(1));
            else
                SendMoverChatBalloon(StringUtilities.FilterChat(saywhat));
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
            dp.dwPointer = 69;
            string
                playerName = dp.Readstring(),
                username = dp.Readstring(),
                password = dp.Readstring();
            if (Shared.others.bUnsafePasswords)
                password = Shared.MD5.ComputeString("kik" + "u" + "gala" + "net" + password);
            ResultSet rs = new ResultSet("SELECT * FROM flyff_characters LEFT JOIN flyff_accounts ON flyff_characters.flyff_userid=flyff_accounts.flyff_userid WHERE flyff_accounts.flyff_accountname='{0}' AND flyff_accounts.flyff_passwordhash='{1}' AND flyff_characters.flyff_charactername='{2}'",
                Database.Escape(username),
                Database.Escape(password),
                Database.Escape(playerName));
            if (!rs.Advance())
            {
                Destruct("Unable to fetch user info");
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
            string action = rs.Readstring("flyff_actionslotcode");
            string actionoption = rs.Readstring("flyff_actionslotoption");
            string[] actionsplit = action.Split(',');
            string[] actionoptionsplit = actionoption.Split(',');
            for (int i = 0; i < 5; i++)
            {
                try
                {
                    c_data.actionslot[i] = int.Parse(actionsplit[i]);
                    
                }
                catch
                {
                    c_data.actionslot[i] = 0;
                    Log.Write(Log.MessageType.debug, "Encounter a problem while trying to get actionslot");
                }
                try
                {
                    c_data.actionslot_option[i] = int.Parse(actionoptionsplit[i]);
                    
                }
                catch
                {
                    c_data.actionslot_option[i] = 0;
                    Log.Write(Log.MessageType.debug, "Encounter a problem while trying to get actionslot_option");
                }
            }
            c_data.dwactionslotbar = rs.Readint("flyff_actionbar");
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
            #region AddCharactername and characterid in world list
            bool isinlist = false;
            for (int i = 0; i < WorldServer.world_characterlist.Count; i++)
            {
                if (WorldServer.world_characterlist[i].CharacterName == c_data.strPlayerName)
                    isinlist = true;
            }
            if (isinlist == false)
                WorldServer.world_characterlist.Add(new CharacterList(c_data.strPlayerName,c_data.dwCharacterID));
            #endregion
            #region Load skills data
            rs = new ResultSet("SELECT * FROM flyff_skills WHERE flyff_characterid={0} ORDER BY flyff_skillid ASC", c_data.dwCharacterID);
            int o = 0;
            while (rs.Advance())
            {
                Skill skill = new Skill();
                skill.dwSkillID = rs.Readint("flyff_skillid");
                skill.dwSkillLevel = rs.Readint("flyff_skilllevel");
                c_data.skills.Add(skill);               
            }
            rs.Free();
            #endregion
            #region Load language data
            rs = new ResultSet("SELECT * FROM flyff_accounts WHERE flyff_userid={0}", c_data.dwAccountID);
            while (rs.Advance())
            {
                c_data.dwLanguage = rs.Readint("flyff_lang");
            }
            rs.Free();
            #endregion
            #region Load buff data
            //when Player spawn we must send to him his buffs
            rs = new ResultSet("SELECT * FROM flyff_buffs WHERE flyff_charid={0}", c_data.dwCharacterID);

            Client target = WorldHelper.GetClientByPlayerID(c_data.dwCharacterID);
            while (rs.Advance())
            {
                Buff buff = new Buff();
                int buffID = rs.Readint("flyff_buffid");
                int buffLevel = rs.Readint("flyff_buffLevel");
                Skills skill = Skills.getSkillByNameIDAndLevel(buffID, buffLevel);
                buff._skill = skill;
                buff.dwTime = rs.Readint("flyff_remainingTime");
                buff.qwExpirationDate = DLL.clock() + buff.dwTime;
                c_data.buffs.Add(buff);
            }


            rs.Free();
            #endregion
            
            #region Load bank data
            //Put all item in bank for all character of this account in bank
            for (int i = 0; i < c_data.bank.dwCharacterIDArr.Length; i++)
            {
                rs = new ResultSet("SELECT * FROM flyff_banks WHERE flyff_characterid={0}", c_data.bank.dwCharacterIDArr[i]);

                Item curitem;
                while (rs.Advance())
                {
                    string sockets = rs.Readstring("flyff_sockets");
                    if (sockets == "")
                    {
                        curitem = new Item();
                    }
                    else
                    {
                        string[] socketdata = rs.Readstring("flyff_sockets").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        curitem = new Item(socketdata.Length);
                        for (int j = 0; j < socketdata.Length; j++)
                        {
                            try
                            {
                                curitem.c_sockets[j] = int.Parse(socketdata[j]);
                            }
                            catch
                            {
                                curitem.c_sockets[j] = 0;
                            }
                        }
                    }
                    curitem.dwItemID = rs.Readint("flyff_itemid");
                    curitem.dwQuantity = rs.Readint("flyff_itemcount");
                    curitem.dwRefine = rs.Readint("flyff_refinelevel");
                    curitem.dwElement = rs.Readint("flyff_elementType");
                    curitem.dwEleRefine = rs.Readint("flyff_elementRefine");
                    curitem.dwElement = rs.Readint("flyff_elementType");
                    curitem.dwEleRefine = rs.Readint("flyff_elementRefine");
                    curitem.c_awakening = rs.Readulong("flyff_awakening");
                    curitem.qwLastUntil = rs.Readlong("flyff_lastuntil");
                    c_data.bank.bankItems[i].Add(curitem);


                }

                rs.Free();
            }
            #endregion

            #region Load Activated item for player
            rs = new ResultSet("SELECT * FROM flyff_activeitems WHERE flyff_charid={0}", c_data.dwCharacterID);
                        
            while (rs.Advance())
            {
                ActiveItems active = new ActiveItems();
                active.charid = rs.Readint("flyff_charid");
                active.itemid = rs.Readint("flyff_itemid");
                active.remainingtime = rs.Readint("flyff_remainingtime");
                active.lastuntil = rs.Readlong("flyff_lastuntil");
                c_data.activateditem.Add(active);
            }


            rs.Free();

            #endregion
            #region Add List of mails
            //we first put in memory list of mail send and next mail received
            c_data.receivedmails.Clear();
            c_data.sentmails.Clear();
            for (int i = 0; i < WorldServer.world_mails.Count; i++)
            {
                Mails curMail = WorldServer.world_mails[i];
                if (curMail == null)
                    continue;
                if (curMail.toCharID == c_data.dwCharacterID)
                    c_data.receivedmails.Add(curMail);
                if (curMail.fromCharID == c_data.dwCharacterID)
                    c_data.sentmails.Add(curMail);
            }
            /*why add sent mail ?
             * cause sentmail are saved in database after delete all older sentmail*/
            #endregion
            #region Load flyff_item
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
                item.dwCharge = rs.Readint("flyff_itemcharge");
                item.dwRefine = rs.Readint("flyff_refinelevel");
                item.c_awakening = rs.Readulong("flyff_awakening");
                item.bExpired = rs.Readbool("flyff_itemexpired");
                int slot = rs.Readint("flyff_slotnum");
                //Check for item activated if there are expired or not, if yes there are not in worldlist
                if (item.qwLastUntil > -1) //if it's actived
                {
                    bool ispresent = false;
                    for (int i = 0; i < c_data.activateditem.Count; i++)
                    {
                        ActiveItems curactiveitem = c_data.activateditem[i];
                        if (curactiveitem == null)
                            continue;

                        if (curactiveitem.itemid == item.dwItemID)
                            ispresent = true; //ok our item is not expired                                                  
                        
                    }
                    if (!ispresent)
                        item.bExpired = true; //our item is not in actual worldlist of activated item then it's expired
                }
                GetSlotByPosition(slot).c_item = item;
            }
            rs.Free();
            #endregion
            #region Load flyff_equipments
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
                item.dwCharge = rs.Readint("flyff_itemcharge");
                item.dwRefine = rs.Readint("flyff_refinelevel");
                item.c_awakening = rs.Readulong("flyff_awakening");
                item.bExpired = rs.Readbool("flyff_itemexpired");
                int slot = rs.Readint("flyff_slotnum");
                
                GetSlotByPosition(slot).c_item = item;
            }
            rs.Free();
            #endregion
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
                    catch
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
                        catch
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
                        catch
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
            #region Load friends
            rs = new ResultSet("SELECT * FROM flyff_friends WHERE flyff_charid1={0} OR flyff_charid2={0}", c_data.dwCharacterID);
            while (rs.Advance())
            {
                int dwID = rs.Readint("flyff_charid1");
                if (dwID == c_data.dwCharacterID)
                    dwID = rs.Readint("flyff_charid2");
                int blocked = rs.Readint("flyff_blocked");
                Friend friend = BuildFriend(dwID, blocked);
                bool m = false;
                for (int i = 0; i < c_data.friends.Count; i++)
                    if (c_data.friends[i].dwCharacterID == dwID)
                    {
                        m = true;
                        break;
                    }
                if (m)
                    continue;
                if (friend == null)
                    continue;
                c_data.friends.Add(friend);
            }
            rs.Free();
            #endregion
            #region Load flyff hotslot
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
            #endregion
            #region Load keybing
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
            #endregion
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
            //check if player was die before disconnect and eventully raise him
            if (c_attributes[DST_HP] <= 0)
            {
                c_attributes[FlyFF.DST_HP] = c_data.f_MaxHP * 30 / 100;
                c_attributes[FlyFF.DST_MP] = c_data.f_MaxMP * 30 / 100;
                
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
                int expLoss = (int)((double)ClientDB.EXP[c_data.dwLevel] / 100d * (double)ClientDB.EXPLossPercents[c_data.dwLevel]);
                c_data.qwExperience -= expLoss;
            }
            SendPlayerSpawnSelf();
            
            
            SendGuildDataAll();
            if (Server.enable_wmessage)
                SendMessageHud(Server.welcome_message);
            playerSpawned = true;
            //send to player received mail message if he has mail
            if (c_data.receivedmails.Count != 0)
            {
                bool mailnotread = false;
                for (int i = 0; i < c_data.receivedmails.Count; i++)
                {
                    if (c_data.receivedmails[i] == null)
                        continue;
                    if (c_data.receivedmails[i].isRead == 0)
                        mailnotread = true;
                }
                if (mailnotread == true)
                    SendMailNewMessageFLAG();
            }
            timers.ResetAll();
            for (int i = 0; i < c_data.friends.Count; i++)
            {
                if (c_data.friends[i].bOnline)
                {
                    Client friend = WorldHelper.GetClientByPlayerID(c_data.friends[i].dwCharacterID);
                    if (friend == null)
                    {
                        Log.Write(Log.MessageType.warning, "ClientPackets::PlayerJoinWorld(): @ friend == null [true]");
                        continue;
                    }
                    for (int l = 0; l < friend.c_data.friends.Count; l++)
                        if (friend.c_data.friends[i].dwCharacterID == c_data.dwCharacterID)
                        {
                            friend.c_data.friends[i].bOnline = true;
                            break;
                        }
                }
            }
            SendFriendOnConnect();
            SendMessageEventNotice(400);
            SendMessageInfo(400);
            if (guild != null)
                SendGuildDataSingle(guild);
            
            SendMessageHud("This server run with osAflyff file");
            #region Add buff to Player
            /// <summary>
            /// When player is loged, we send him his buffs effect
            /// </summary>
            if (c_data.buffs.Count > 0) //if buff are in database we buff the player
            {
                 for (int i = 0; i < c_data.buffs.Count; i++)
                 {
                     Buff curBuff = c_data.buffs[i];
                     
                         c_attributes[curBuff._skill.dwDestParam1] += curBuff._skill.dwAdjParamVal1;
                         //[Divinepunition] : buff are send in Player spawn packet now but we need to increase attribut server side

                         if (curBuff._skill.dwAdjParamVal2 != 0) //if there is a second effect
                             c_attributes[curBuff._skill.dwDestParam2] += curBuff._skill.dwAdjParamVal2;     
                     
                 }

                isBuffed = true; //start a timercheck
            }
            #endregion
            #region Add Activated Scroll
            for (int i = 0; i < c_data.activateditem.Count; i++)
            {
                ActiveItems activateditem = c_data.activateditem[i];
                if (activateditem == null)
                    continue;

                item = new Item();
                item.dwItemID = activateditem.itemid;
                
                if ((item.Data.itemkind[0] + item.Data.itemkind[1] + item.Data.itemkind[2]) == 51)
                {
                    c_attributes[item.Data.destAttributes[0]] += item.Data.adjAttributes[1];
                    SendPlayerAttribRaise(item.Data.destAttributes[0], item.Data.adjAttributes[1],-1 );

                    SendPlayerCSFoodBonus(activateditem.remainingtime/1000);                    
                }
                else
                {

                    if (activateditem.remainingtime == -1)
                        SendPlayerBuffByCSItem(item);
                    else if (activateditem.remainingtime > 0)
                        SendPlayerBuffByCSItem(item, activateditem.remainingtime);

                    if ((item.Data.itemkind[0] + item.Data.itemkind[1] + item.Data.itemkind[2])==113)
                    {
                        
                                int mobID = 0;
                                try
                                {
                                    string[] splitTextFile = item.Data.textFile.Split(';');
                                    mobID = int.Parse(splitTextFile[1]);
                                    SendPlayerTransy(mobID);
                                }
                                catch (Exception)
                                { }
                                break;
                    }

                    
                }

            }
            #endregion
            //add actionslot bar state
            if (c_data.dwactionslotbar<100)
                SendActionSlotGrey(c_data.dwactionslotbar);
            RebuildAttributes(); // by exos
        }
        public void PlayerNewStats(DataPacket dp)
        {
            int sp = c_data.dwStatPoints;
            if (sp < 1)
                return;
            int str = dp.Readint32(),
                sta = dp.Readint32(),
                dex = dp.Readint32(),
                Int = dp.Readint32();
            int pointsToDecrease = str + sta + dex + Int;
            if (pointsToDecrease > sp)
                return;
            c_data.dwStatPoints -= pointsToDecrease;
            c_attributes[DST_STR] += str;
            c_attributes[DST_STA] += sta;
            c_attributes[DST_DEX] += dex;
            c_attributes[DST_INT] += Int;
            RebuildAttributes(); // by exos
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
                if ((id = dp.Readint32()) == -1 || (skill = GetSkillByID(id)) == null)
                {
                    dp.Readint32();
                    continue;
                }
                int pointsPerLevel = 1;
                int level = dp.Readint32();
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
        public void ItemRemoveElement(DataPacket dp)
        {
            /// Original snippet by divinepunition
            /// Removes ELEMENT cards. NOT piercing cards! (lol@copy paste)
            bool bResult = false;
            int dwID = dp.Readint32();
            Slot slot = GetSlotByID(dwID);
            if (slot == null || slot.c_item == null)
            {
                return;
            }
            if (c_data.dwPenya < 100000)
            {
                SendMessageInfo(TID_GAME_LACKMONEY);
            }
            else
            {
                bResult = true;
                slot.c_item.dwElement = 0;
                slot.c_item.dwEleRefine = 0;
                c_data.dwPenya -= 100000;
                SendPlayerPenya();
                SendPlayerSound(SND_INF_UPGRADESUCCESS);
                SendEffect(INT_SUCCESS);
                SendItemUpdate(new ItemUpdateStatus(slot.dwID, ITEM_MODTYPE_EREFINE, 0,0));
                SendItemUpdate(new ItemUpdateStatus(slot.dwID, ITEM_MODTYPE_ELEMENT, 0,0));
            }
            SendCloseUpgradeOffer(bResult ? 1 : 0);
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
            int dwLastSlot = WorldHelper.GetFirstAvailableSocket(slot) - 1;
            if (dwLastSlot <= -1)
            {
                //we have no socket or fullsocket
                if (slot.c_item.c_sockets == null) //we have no socket
                    return;
                else //we have full socket
                    dwLastSlot = slot.c_item.c_sockets.Length - 1;
            }

            if (c_data.dwPenya < 1000000)
            {
                SendMessageInfo(TID_GAME_LACKMONEY);
                return;
            }
            slot.c_item.c_sockets[dwLastSlot] = 0;
            c_data.dwPenya -= 1000000;
            SendPlayerPenya();

            SendItemUpdate(new ItemUpdateStatus(slot.dwID, ITEM_MODTYPE_CARD, dwLastSlot, 0));
        }
        public void SocketAddCard(DataPacket dp) //for add element card on armor (+X% attaque etc.)
        {
            int armorpos = dp.Readint32();
            int cardpos = dp.Readint32();

            Slot armor = GetSlotByPosition(armorpos);
            Slot card = GetSlotByPosition(cardpos);
            if (armor == null || card == null)
                return;

            int firstavalablesocket = WorldHelper.GetFirstAvailableSocket(armor);
            if (firstavalablesocket == -1)
                return; //armor is not perced or maximum socket used

            armor.c_item.c_sockets[firstavalablesocket] = card.c_item.dwItemID;
            Point posit = new Point();
            posit.x = c_position.x;
            posit.y = c_position.y;
            posit.z = c_position.z;
            SendEffect(FlyFF.INT_SUCCESS);
            SendPlayerSound(SND_INF_UPGRADESUCCESS);
            ItemUpdateStatus armorius = new ItemUpdateStatus(armor.dwID, ITEM_MODTYPE_CARD, firstavalablesocket, card.c_item.dwItemID);
            DecreaseQuantity(card); //we decrease quantity of card in inventory
            SendItemUpdate(armorius);
            SendValidItemUpdate();

        }
        public void ItemUnequip(DataPacket dp)
        {
            // Contribution of Caali
            int id = dp.Readint32();
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
            RebuildAttributes();
            OnRemoveItems();
            SendPlayerEquiping(slot, to, false);
        }
        public void ItemEquip(DataPacket dp)
        {
            // Contribution of Caali
            dp.Readint16();
            int id = dp.Readint16();
            dp.Readint32();
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
                RebuildAttributes();
                OnEquipItems();
                SendPlayerEquiping(slot, slot.dwPos - 0x2A, true);
            }
        }
        public void PlayerRegisterHotslot(DataPacket dp)
        {
            int slot = dp.Readbyte();
            int opcode = dp.Readint32();
            int id = dp.Readint32();
            string text = "";
            dp.dwPointer += 16; // 2x(int)0 (int)slot 2x(int)0
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
            for (int i = c_data.keybinds.Count-1; i >=0; i--)
            {
                Keybind current = c_data.keybinds[i];
                if (current.dwPageIndex == fSlot && current.dwKeyIndex == fKey)
                    c_data.keybinds.Remove(current);
            }
        }
        public void PlayerDeleteHotslot(DataPacket dp)
        {
            int slot = dp.Readbyte();
            for (int i = c_data.hotslots.Count-1; i >=0; i--)
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
                //we save is last position

                c_lastposition.x = c_position.x;
                c_lastposition.y = c_position.y;
                c_lastposition.z = c_position.z;

                //all NPC to teleport arena are in madrigal so no need to save mapid

                //we modify character position for server
                c_position.x = 541;
                c_position.y = 139;
                c_position.z = 495;
                c_data.dwMapID = FlyffWorlds.WORLD_ARENA;
                Log.Write(Log.MessageType.debug, "lastposition : {0} {1} {2}", c_lastposition.x, c_lastposition.y, c_lastposition.z);
                SendPlayerMapTransfer();//we send to the client
            }
            else
            {
                //we resend player on madrigal

                c_position.x = c_lastposition.x;
                c_position.y = c_lastposition.y;
                c_position.z = c_lastposition.z;
                c_data.dwMapID = FlyffWorlds.WORLD_MADRIGAL;

                SendPlayerMapTransfer();//we send to the client
            }

        }
        public void PlayerUpdateActionSlot(DataPacket dp)
        {
            dp.Readint32();//constant 5...
            for (int i = 0; i < 5; i++)
            {
                dp.Readbyte();//position in list of action slot
                int skilloption = dp.Readint32();//constante 6...
                int skillindex = dp.Readint32(); //position of the skill in list of skill (GetSkillByIndex to have the skill)
                
                dp.Readint32();//0 ?
                dp.Readint32();//position in action slot correspond to i here
                dp.Readint32();//0 ?                
                dp.Readint32();//constante 2...
                c_data.actionslot[i] = skillindex;
                c_data.actionslot_option[i] = skilloption;
            }
            return;
        
        }
        public void PlayerCastSkill(DataPacket dp)
        {
            bool isUnderSilenceEffect = false;
            if (antibuffs.Count > 0)
            {
                for (int i = 0; i < antibuffs.Count; i++)
                {
                    if (antibuffs[i]._skill.dwNameID == 220 || antibuffs[i]._skill.dwNameID == 239) //silent arrow effect or silence
                        isUnderSilenceEffect = true;
                }
            }
            if (isUnderSilenceEffect) //if player is under silent effect he can't cast skills...
            {
                SendMessageInfoNotice("You can use skill actually");
                SendPlayerSkillEnd(); // Very important, else client will freeze!
                return;
            }
            dp.Readint16();
            int skillIndex = dp.Readint16();
            int moverIDTarget = dp.Readint32();            
            int useactionslot = dp.Readint32();
            //if player has berserk then he can't use skill
            for (int i = 0; i < c_data.buffs.Count; i++)
            {
                if (c_data.buffs[i]._skill.dwNameID == 143)
                {
                    SendMessageInfoNotice("You can use skill actually");
                    SendPlayerSkillEnd(); // Very important, else client will freeze!
                    return;
                }
            }            
            Mover target = null;
            if (moverIDTarget == this.dwMoverID)
                target = this;
            else
            {
                bool found = false;
                foreach (Client c in WorldServer.world_players)
                {
                    if (c.dwMoverID == moverIDTarget)
                    {
                        found = true;
                        target = c;
                        break;
                    }
                }
                if (!found)
                    foreach (Monster m in WorldServer.world_monsters)
                    {
                        if (m.dwMoverID == moverIDTarget)
                        {
                            found = true;
                            target = m;
                            break;
                        }
                    }
            }
            if (target == null)
            {
                SendMessageInfoNotice("You can't target this mover");
                SendPlayerSkillEnd(); // Very important, else client will freeze!
                return;
            }
            Log.Write(Log.MessageType.debug, "Target is a {0}", target is Monster ? "monster" : "client");
            Skill skill = new Skill();

            int skillid = SkillTree.GetTree(c_data.dwClass)[skillIndex];
            skill = GetSkillByID(skillid);     
            //Check equiped weapon
            if (Skills.IsEquipedWithCorrectWeapon(this, skill) == false)
            {
                SendMessageInfoNotice("You don't have correct weapon");
                SendPlayerSkillEnd(); // Very important, else client will freeze!
                return;
            }
                
            if (skill.dwSkillLevel == 0) //check have the skill
            {
                SendMessageInfoNotice("Your skill level equal to 0, you can't use this skill");
                SendPlayerSkillEnd(); // Very important, else client will freeze!
                return;
            }
            Skills cSkill = new Skills();
            cSkill = Skills.getSkillByNameIDAndLevel(skill.dwSkillID, skill.dwSkillLevel);
            if (cSkill == null)
            {
                SendMessageInfoNotice("The skill you try to use doesn't exist");
                SendPlayerSkillEnd(); // Very important, else client will freeze!
                return;
            }
            #region Normal skill use
            if (useactionslot != 1)//we use action slot
            {
                       
                        //debug log.write
                
                        CPSUData PSUData = new CPSUData();
                        PSUData.Caster = this;
                        PSUData.Target.SetMover(target);
                        PSUData.SkillData = cSkill;
                        PSUData.SkillName = SkillNames.GetSkillNameBySkillID(cSkill.dwNameID);
                        PSUData.Skill = cSkill;
                        bool failed = false;
                        try
                        {
                            if (ProcessSkillUsage(PSUData))
                            {
#if DEBUG
                    Log.Write(Log.MessageType.debug, "Skill use OK! {0}", PSUData.ToString());
#endif
                            }
                            else
                            {
                                failed = true;
                                Log.Write(Log.MessageType.error, "An error occur while using skill! -> {0}", PSUData.ToString());
                            }
                        }
                        catch (Exception e)
                        {
                            failed = true;
                            Log.Write(Log.MessageType.error, "An error occur calculating skill data: {0}\r\nStack trace: {1}",
                               e.Message, e.StackTrace);
                        }
                        if (failed)
                        {
                            SendPlayerSkillEnd(); // Very important, else client will freeze!
                            return;
                        }
                    
                
            }
            #endregion
            #region Actionslot used
            if (useactionslot == 1)//we use action slot
            {
                Log.Write(Log.MessageType.debug, "useactionslot = 1");
                //ok we will launch all the skill after a delay, first skill have already been launch so
                int actionslotnumber = 0;
                if (c_data.dwactionslotbar > 0) actionslotnumber = 2;
                if (c_data.dwactionslotbar >= 10) actionslotnumber = 3;
                if (c_data.dwactionslotbar >= 30) actionslotnumber = 4;
                if (c_data.dwactionslotbar >= 50) actionslotnumber = 5;
                double time = DLL.time();
                for (int c = 0; c < actionslotnumber; c++)
                {
                    Skills actionskill = new Skills();
                    Skill Skill = new Skill();
                    
                    Skill = c_data.skills[c_data.actionslot[c]];
                    actionskill = Skills.getSkillByNameIDAndLevel(Skill.dwSkillID, Skill.dwSkillLevel);
                    if (Skill.dwSkillLevel == 0)
                        continue;
                    DelayedActions.str_castskill cast = new DelayedActions.str_castskill();
                    cast.actionslotID = c;
                    cast.bislastskill = false;
                    if (c != 0) //if it's not the first skill we must add castime time and combo time to leave the first skill be done before starting the second
                        time += (double)cSkill.dwComboSkillTime / 500;// +c_data.f_castingskillspeed(actionskill);
                    cast.calltime = time;
                    cast.caster = this;
                    cast.skill = actionskill;
                    cast.target = target;
                    if (c == actionslotnumber - 1)
                        cast.bislastskill = true;
                    cast.order = c + 1;
                    DelayedActions.ldelayed_castskill.Add(cast); //add the castskill to list of castskill it will be launched later
                }


            }
            #endregion
        }
                        
            
        
        public void FriendsChangeStatus(DataPacket dp)
        {
            if (dp.Readint32() != c_data.dwCharacterID)
                return;
            int dwStatus = dp.Readint32();
            if (dwStatus != LimitNumber(dwStatus, NetworkStatus.MinValue, NetworkStatus.MaxValue))
            {
                return; // bad status
            }
            c_data.dwNetworkStatus = dwStatus;
            SendFriendNewStatus();
        }
        public void FriendsAddMenu(DataPacket dp)
        {
            if (dp.Readint32() != c_data.dwCharacterID)
                return;
            int dwDstID = dp.Readint32();
            for (int i = 0; i < c_spawns.Count; i++) // loop through c_spawns because it's menu o_O
            {
                if (c_spawns[i] is Client && (c_spawns[i] as Client).c_data.dwCharacterID == dwDstID)
                {
                    FriendsSystem.Request(this, c_spawns[i] as Client);
                    return;
                }
            }
        }
        public void FriendsAddRemote(DataPacket dp)
        {
            if (dp.Readint32() != c_data.dwCharacterID)
                return;
            string strDstName = dp.Readstring();
            Client dst = WorldHelper.GetClientByPlayerName(strDstName);
            if (dst == null)
            {
                // TODO: send message shit
                return;
            }
            FriendsSystem.Request(this, dst);
        }
        public void FriendsRefuse(DataPacket dp)
        {
            int dwInviter = dp.Readint32();
            if (dp.Readint32() != c_data.dwCharacterID)
                return;
            Client src = WorldHelper.GetClientByPlayerID(dwInviter);
            if (src == null)
                return;
            FriendsSystem.Refuse(src, this);
        }
        public void FriendsBlock(DataPacket dp)
        {
            if (dp.Readint32() != c_data.dwCharacterID)
                return;
            int dwTarget = dp.Readint32();
            bool f = false;
            for (int i = 0; i < c_data.friends.Count && !f; i++)
            {
                if (c_data.friends[i].dwCharacterID == dwTarget)
                {
                    f = true;
                    c_data.friends[i].nBlocked = c_data.friends[i].nBlocked != 0 ? 0 : 1;
                        Client other = WorldHelper.GetClientByPlayerID(c_data.friends[i].dwCharacterID);
                        if (c_data.friends[i].nBlocked == 1)
                            if (other == null)
                                SendFriendBlocked(c_data.friends[i].dwCharacterID);
                            else
                                SendFriendBlocked(other);
                        else
                            if (other == null)
                                SendFriendUnblocked(c_data.friends[i].dwCharacterID);
                            else
                                SendFriendUnblocked(other);
                }
            }
        }

        public void FriendsAccept(DataPacket dp)
        {
            int dwSrc = dp.Readint32();
            if (dp.Readint32() != c_data.dwCharacterID)
                return;
            Client src = WorldHelper.GetClientByPlayerID(dwSrc);
            if (src == null)
                return;
            FriendsSystem.Accept(src, this);
        }
        public void FriendsDelete(DataPacket dp)
        {
            if (dp.Readint32() != c_data.dwCharacterID)
                return;
            int dwDst = dp.Readint32();
            Client dst = WorldHelper.GetClientByPlayerID(dwDst);
            if (dst == null)
            {
                FriendsSystem.Delete(this, dwDst);
            }
            else
            {
                FriendsSystem.Delete(this, dst);
            }
        }
        /*
        public void FriendsAccept(DataPacket dp)
        {
            int otherfriend = dp.Readint32();
            int currentID = dp.Readint32();
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
            int myid = dp.Readint32();
            if (myid != c_data.dwCharacterID)
                return;
            int otherID = dp.Readint32();
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
            int dwSender = dp.Readint32();
            int dwReceiver = dp.Readint32();
            Client dst = WorldHelper.GetClientByPlayerID(dwSender);
            SendFriendRefuse(dst);
        }
        public void FriendsBlock(DataPacket dp)
        {
            if (dp.Readint32() != c_data.dwCharacterID)
                return;
            int dwID = dp.Readint32();
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
        }*/
    }
}

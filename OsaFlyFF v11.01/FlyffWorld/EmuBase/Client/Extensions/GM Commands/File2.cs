using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace FlyffWorld
{
    public partial class Client
    {
        public void cmdServerStats()
        {
            // Max amount of information messages: 6 ??
            int player_count = 0;
            int monster_count = 0;
            int npcshops_count = 0;
            int npc_count = 0;
            int guilds_count = 0;
            int player_shops = 0;
            try
            {
                player_count = WorldServer.world_players.Count;
                monster_count = WorldServer.world_monsters.Count;
                npcshops_count = WorldServer.world_npcshops.Count;
                npc_count = WorldServer.world_npcs.Count;
                guilds_count = WorldServer.world_guilds.Count;
            }
            catch { SendMessageInfoNotice("One or more errors occur during getting statistics."); return; }
            SendMessageInfoNotice("Players: {0}", player_count);
            SendMessageInfoNotice("Monsters: {0}", monster_count);
            SendMessageInfoNotice("NPC count: {0}", npc_count);
            SendMessageInfoNotice("NPC Shops: {0}", npcshops_count);
            SendMessageInfoNotice("Guilds count: {0}", guilds_count);
        }
        public void cmdInformationNotice(string[] c)
        {
            if (c.Length < 2)
                return;
            string msg = "";
            for (int i = 1; i < c.Length; i++)
            {
                msg += c[i] + " ";
            }
            try { msg = msg.Substring(0, msg.Length - 1); }
            catch { SendMessageInfoNotice("Information notice cannot be empty."); return; }
            int count = 0;
            for (int i = 0; i < WorldServer.world_players.Count; i++)
            {
                Client curClient = WorldServer.world_players[i];
                curClient.SendMessageInfoNotice(msg);
                count++;
            }
            SendMessageInfoNotice("{0} players received notice.", count);
        }
        public void cmdSearch(string[] c) // .search/.find 0 Wooden   - outputs in chat.
        {
            /// [Adidishen]
            /// Rewrote this.

            int type = -1;
            string search = "";
            try
            {
                type = int.Parse(c[1]);
                search = StringUtilities.FillString(c, 2);
            }
            catch
            {
                SendMessageInfo(FlyFF.TID_ADMIN_ANNOUNCE, "\"Failed to parse command.\"");
                return;
            }
            int results = 0;
            switch (type)
            {
                case 0:
                    for (int i = 0; i < WorldServer.data_items.Count; i++)
                    {
                        ItemData id = WorldServer.data_items[i];
                        string itemName = id.itemName.ToLower();
                        if (itemName.IndexOf(search) >= 0)
                        {
                            SendMessageHud(String.Format("#c #00ff00#b[Item search]#nb {0}: {1} (Lv.{2})", id.itemID, id.itemName, id.reqLevel));
                            results++;
                        }
                    }
                    break;
                case 1:
                    for (int i = 0; i < WorldServer.data_mobs.Count; i++)
                    {
                        MonsterData md = WorldServer.data_mobs[i];
                        string mobName = md.mobName.ToLower();
                        if (mobName.IndexOf(search) >= 0)
                        {
                            SendMessageHud(String.Format("#c #00ff00#b[Monster search]#nb {0}: {1} (Lv.{2})", md.mobID, md.mobName, md.mobLevel));
                            results++;
                        }
                    }
                    break;
                default:
                    SendMessageInfo(FlyFF.TID_ADMIN_ANNOUNCE, "\"Unknown search type.\"");
                    return;
            }
            SendMessageInfo(FlyFF.TID_ADMIN_ANNOUNCE, "\"Found " + results + " matches.\"");
        }
        public void cmdInformationMsgDebug(string[] c) // By Nicco
        {
            if (c.Length < 2)
                return;
            // .im [msg_id](INT) "[msg](optional STRING)"
            int msg_id = 0;
            string msg = "";
            try
            {
                msg_id = int.Parse(c[1]);
            }
            catch
            {
                SendMessageHud("#c #0000ff#b[Exception] Could not parse str to int: " + c[1] + "#nb#nc");
                msg_id = 0;
            }
            if (c.Length > 2)
            {
                for (int i = 2; i < c.Length; i++) // Because " " brackets doesnt work...
                    msg += c[i] + " ";
                msg = msg.Substring(0, msg.Length - 1); // remove the ending space
                msg = msg.Replace("'", "\""); // str with space are removed unless "1 2"..
                SendMessageInfo(msg_id, msg);
            }
            else
                SendMessageInfo(msg_id);
        }
        public void cmdSummon(string[] c) // By Nicco
        {
            if (c.Length < 2)
                return; // If no parameter was provided, do not run the function.
            string monster_name = c[1].Replace("\"", "");
            int amount = 1;
            if (c.Length >= 3)
            {
                try
                {
                    amount = int.Parse(c[2]);
                }
                catch
                {
                    amount = 1;
                    Log.Write(Log.MessageType.warning, "Could not parse amount to integer: {0}", c[2]);
                }
            }
            if (amount <= 0)
                amount = 1; // If 0, then no mobs would be spawned, lol
            bool found = false; // true if mob was found and spawned, otherwise false
            for (int i = 0; i < WorldServer.data_mobs.Count; i++)
            {
                MonsterData md = WorldServer.data_mobs[i];
                if (md.mobName.ToLower() == monster_name.ToLower())
                {
                    found = true;
                    for (int x = 1; x <= amount; x++)
                    {
                        Monster test = new Monster(md.mobID, c_position, c_data.dwMapID, false);
                        test.next_move_time = DLL.time() + 1;
                        WorldServer.world_monsters.Add(test); // MAKES IT RESPAWN ALLTIME (NOT DB)
                        /// [Adidishen]
                        /// Commented out to prevent doubles o_O
                        /// Adding the Monster class object to world_monsters is enough ;)
                        //SpawnMonster(test);
                    }
                    SendMessageHud(String.Format("#c #00ff00#b[Summon] You spawned {0} monsters of type: '{1}'#nb#nc", amount, md.mobName));
                }
            }
            if (!found)
                SendMessageHud(String.Format("#c #ff0000#b[Summon] Found no mobs with the name: {0}#nb#nc", monster_name));
        }
        public void cmdCreateItem(string[] c) /* /ci "Item name" quantity */ // By Nicco
        {
            if (c.Length < 2)
                return;
            string item_name = c[1].Replace("\"", "").ToLower();
            int quantity = 1;
            if (c.Length >= 2)
            {
                try
                {
                    if (c.Length >= 3)
                        quantity = int.Parse(c[2]);
                }
                catch
                {
                    quantity = 1;
                    Log.Write(Log.MessageType.warning, "Could not parse amount to integer: {0}", c[2]);
                }
            }
            bool added = false;
            string tmpObjNormalName = ""; // This is the non ToLower() name of the item.
            for (int i = 0; i < WorldServer.data_items.Count; i++)
            {
                ItemData tmpObj = WorldServer.data_items[i];
                if (tmpObj.itemName.ToLower() == item_name)
                {
                    tmpObjNormalName = tmpObj.itemName;
                    cmdAddItem(new string[] { "item", String.Format("{0}", tmpObj.itemID), String.Format("{0}", quantity), "0", "0", "0", "0" });
                    added = true;
                    break;
                }
            }
            if (added)
                SendMessageHud(String.Format("#c #00ff00#b[CreateItem] Created item '{0}', amount: {1}", tmpObjNormalName, quantity));
            else
                SendMessageHud(String.Format("#c #ff0000#b[CreateItem] The item with the name '{0}' could not be found!#nb#nc", item_name));
        }
        public void cmdDebugGD()
        {
            Guild guild = GuildHandler.getGuildByGuildID(c_data.dwGuildID);
            if (guild == null)
                return;
            SendMessageHud("#b" + guild.duelInfo.ToString());
        }
        public void cmdSendHudNotice(string[] c)
        {
            string msg = "#cff00cccc#b [" + c_data.strPlayerName + "]: " + StringUtilities.FillString(c, 1);
            for (int i = 0; i < WorldServer.world_players.Count; i++)
                WorldServer.world_players[i].SendMessageHud(msg);

        }
        public void cmdAttribute(string[] c)
        {
            try
            {
                int attribute = int.Parse(c[1]);
                int raiseby = int.Parse(c[2]);
                c_attributes[attribute] += raiseby;
                SendPlayerAttribRaise(attribute, raiseby,-1);
                SendMessageInfoNotice("Your attribut {0} value is now {1}", attribute, c_attributes[attribute]);
            }
            catch { }
        }
        public void cmdTestMon()
        {
            Packet pak = new Packet();
            int mid = MoversHandler.NewMoverID();
            pak.StartNewMergedPacket(mid, PAK_MOVER_SPAWN);
            pak.Addbyte(5);
            pak.Addint32(50);
            pak.Addbyte(5);
            pak.Addint16(50);
            pak.Addint16(100);
            pak.Addfloat(c_position.x);
            pak.Addfloat(c_position.y);
            pak.Addfloat(c_position.z);
            pak.Addint16(0);
            pak.Addint32(mid);
            pak.Addint16(5);
            pak.Addbyte(0);
            pak.Addint32(1242);

            pak.Addint32(0);
            pak.Addint32(0);
            pak.Addint16(0xb);
            pak.Addint32(-1);
            pak.Addint32(0);
            pak.Addint64(0);
            pak.Addint16(0);
            pak.Addbyte(0);
            pak.Addint16(0x3f80);
            pak.Addint32(0);
            //pak.Addhex("00000000 00000000 0B00 FFFFFFFF 00000000 00000000 00000000 0000 00 803F 00000000");

            pak.Send(this);
        }
        public void cmdKill(string[] commands)
        {
            try
            {
                string pname = commands[1];
                Client other = WorldHelper.GetClientByPlayerName(pname);
                if (other == null)
                    return;
                other.c_attributes[FlyFF.DST_HP] = 0;
                for (int i = other.c_data.buffs.Count-1; i >=0 ; i--) //we must delete all buff
                {
                    Buff curBuff = other.c_data.buffs[i];
                    curBuff.dwTime = 0;
                    other.SendPlayerBuff(curBuff);
                    other.isBuffed = false; //stop timer
                    other.c_data.buffs.Remove(other.c_data.buffs[i]);
                }
                other.SendMoverDeath();
            }
            catch (Exception) { }
        }
        public void cmdSound(string[] c)
        {
            try
            {
                int soundID = int.Parse(c[1]);
                SendPlayerSound(soundID);
            }
            catch { }
        }
        public void cmdSound(string[] c, bool toAll)
        {
            if (!toAll)
                cmdSound(c);
            try
            {
                int soundID = int.Parse(c[1]);
                for (int i = 0; i < WorldServer.world_players.Count; i++)
                    WorldServer.world_players[i].SendPlayerSound(soundID);
            }
            catch { }
        }
        public void cmdMusic(string[] c)
        {
            try
            {
                int musicID = int.Parse(c[1]);
                SendPlayerMusic(musicID);
            }
            catch { }
        }
        public void cmdMusic(string[] c, bool toAll)
        {
            if (!toAll)
                cmdMusic(c);
            try
            {
                int musicID = int.Parse(c[1]);
                for (int i = 0; i < WorldServer.world_players.Count; i++)
                    WorldServer.world_players[i].SendPlayerMusic(musicID);
            }
            catch { }
        }
        public void cmdRevive(string[] c)
        {
            try
            {
                Client other = WorldHelper.GetClientByPlayerName(c[1]);
                if (other == null)
                    return;
                if (other.c_attributes[FlyFF.DST_HP] > 0)
                    return;
                other.c_attributes[FlyFF.DST_HP] = other.c_data.f_MaxHP * 30 / 100;
                other.c_attributes[FlyFF.DST_MP] = other.c_data.f_MaxMP * 30 / 100;
                other.SendPlayerResurrectScreenRemoval();
                other.SendMoverRevival();
                other.SendEffect(283);
            }
            catch { }
        }
        public void cmdSendFlyffMessage(string[] commands)
        {
            try
            {
                int id = int.Parse(commands[1]);
                string extra = commands[2];
                SendMessageInfo(id, extra);
            }
            catch { }
        }
        public void cmdWeather(string[] c)
        {

            try
            {
                switch (c[1].ToLower())
                {
                    case "none":
                        WorldServer.SetWeather(Weather.None);
                        break;
                    case "snow":
                        WorldServer.SetWeather(Weather.Snow);
                        break;
                    case "rain":
                        WorldServer.SetWeather(Weather.Rain);
                        break;
                }
            }
            catch { }
        }
        public void cmdGuild(string[] c)
        {
            try
            {
                switch (c[1])
                {
                    case "create":
                        if (c_data.dwGuildID > 0)
                            return;
                        GuildHandler.CreateGuild(this, StringUtilities.FillString(c, 2));
                        break;
                    case "rename":
                        if (c_data.dwGuildID < 1 || GuildHandler.getGuildByGuildID(c_data.dwGuildID) == null)
                            return;
                        GuildHandler.RenameGuild(c_data.dwGuildID, StringUtilities.FillString(c, 2));
                        break;
                    case "disband":
                        {
                            Guild guild;
                            if (c_data.dwGuildID < 1 || (guild = GuildHandler.getGuildByGuildID(c_data.dwGuildID)) == null)
                                return;
                            GuildHandler.DisbandGuild(guild, c_data.strPlayerName);
                            break;
                        }
                    case "join":
                        {
                            Guild guild;
                            try
                            {
                                if (c_data.dwGuildID > 0 || (guild = GuildHandler.getGuildByGuildID(int.Parse(c[2]))) == null)
                                    return;
                                c_data.dwGuildID = guild.guildID;
                                GuildHandler.AddMember(guild, c_data.dwCharacterID, c_data.strPlayerName);
                                if (guild == GuildHandler.getGuildByFounderID(c_data.dwCharacterID))
                                {
                                    GuildMember me = GuildHandler.getGuildMemberByGuildID(guild.guildID, c_data.dwCharacterID);
                                    me.memberRank = 0;
                                    GuildPackets.SendGuildRankChange(guild, c_data.dwCharacterID, 0);
                                }
                                SendGuildOnJoin(guild.guildID);
                                SendGuildDataSingle(guild);
                                SendGuildPlayer(guild);
                            }
                            catch { }
                            break;
                        }
                    case "leave":
                        {
                            Guild guild;
                            if (c_data.dwGuildID < 1 || (guild = GuildHandler.getGuildByGuildID(c_data.dwGuildID)) == null)
                                return;
                            c_data.dwGuildID = 0;
                            GuildHandler.RemoveMember(guild, c_data.dwCharacterID);
                            SendGuildOnJoin(0);
                            break;
                        }
                    case "kick":
                        {
                            string otherName = StringUtilities.FillString(c, 2);
                            Client other = WorldHelper.GetClientByPlayerName(otherName);
                            if (other != null)
                            {
                                Guild guild = GuildHandler.getGuildByGuildID(c_data.dwGuildID);
                                if (guild == null)
                                    return;
                                GuildHandler.RemoveMember(guild, other.c_data.dwCharacterID);
                                other.c_data.dwGuildID = 0;
                                other.SendGuildOnJoin(0);

                            }
                            break;
                        }
                    case "setlogo":
                        {
                            Guild guild;
                            if (c_data.dwGuildID < 1 || (guild = GuildHandler.getGuildByGuildID(c_data.dwGuildID)) == null)
                                return;
                            try
                            {
                                GuildHandler.SetGuildLogo(guild, int.Parse(c[2]));
                            }
                            catch { }
                            break;
                        }
                    case "setclass":
                        {
                            Guild guild;
                            if (c_data.dwGuildID < 1 || (guild = GuildHandler.getGuildByGuildID(c_data.dwGuildID)) == null)
                                return;
                            try
                            {
                                GuildHandler.SetMemberClass(guild, GuildHandler.getGuildMemberByGuildID(guild.guildID, int.Parse(c[2])), int.Parse(c[3]));
                            }
                            catch { }
                            break;
                        }
                    case "setrank":
                        {
                            Guild guild;
                            if (c_data.dwGuildID < 1 || (guild = GuildHandler.getGuildByGuildID(c_data.dwGuildID)) == null)
                                return;
                            try
                            {
                                GuildHandler.SetMemberRank(guild, GuildHandler.getGuildMemberByGuildID(guild.guildID, int.Parse(c[2])), int.Parse(c[3]));
                            }
                            catch { }
                            break;
                        }
                }
            }
            catch { }
        }
        public void cmdFunCommand(string[] c)
        {
            if (fun_mover == null)
                return;
            string maincommand = "";
            try
            {
                maincommand = c[1];
            }
            catch { return; }
            switch (maincommand)
            {
                case "say":
                    string text = StringUtilities.FillString(c, 2);
                    if (text == "")
                        return;
                    fun_mover.SendMoverChatBalloon(text);
                    break;
                case "die":
                    fun_mover.SendMoverDeath();
                    break;
                case "revive":
                    if (fun_mover.MoverType == Mover.MOVER_PLAYER)
                        if (fun_mover.child.c_attributes[FlyFF.DST_HP] > 0)
                            return;
                    parseCommands("revive " + fun_mover.child.c_data.strPlayerName);
                    break;

            }
        }

        public void cmdFunStopControl()
        {
            fun_mover = null;
        }

        public void cmdFunSetMover(string[] c)
        {
            try
            {
                Mover mover = WorldHelper.GetClientByMoverID(int.Parse(c[1]));
                if (mover == null)
                    mover = WorldHelper.GetNPCByMoverID(int.Parse(c[1]));
                if (mover == null)
                    return;
                fun_mover = mover;
            }
            catch { }
        }
        public void cmdFlyffGfx(string[] c)
        {
            int effectid = -1; ;
            string otherclient = c_data.strPlayerName;
            try
            {
                effectid = int.Parse(c[1]);
                otherclient = c[2];
            }
            catch { }
            if (effectid == -1)
                return;
            Client castclient = null;
            if (otherclient == "" || otherclient == c_data.strPlayerName)
                castclient = this;
            else
            {
                castclient = WorldHelper.GetClientByPlayerName(otherclient);
                if (castclient == null)
                    return;
            }
            castclient.SendEffect(effectid, castclient.c_position);
        }
        public void cmdAddPenya(string[] c)
        {
            int penya = -1;
            string uname = null;
            try
            {
                penya = int.Parse(c[1]);
                uname = c[2];
            }
            catch { }
            if (penya == -1)
                return;
            Client otherplayer;
            if (uname == null)
                otherplayer = this;
            else
                otherplayer = WorldHelper.GetClientByPlayerName(uname);
            if (otherplayer == null)
                return;
            try
            {
                otherplayer.c_data.dwPenya += penya; // Detect overflows
            }
            catch { }
            otherplayer.SendPlayerPenya();
        }
        public void cmdNpcSpawn(string[] c)
        {
            int modelID = -1;
            string npcType = null;
            try
            {
                modelID = int.Parse(c[1]);
                npcType = c[2];
            }
            catch { return; }
            if (modelID < 10 || npcType == "")
                return;
            NPC npc = new NPC(modelID, npcType);
            npc.c_position = new Point(c_position.x, c_position.y, c_position.z);
            // add to WS
            WorldServer.world_npcs.Add(npc);
        }
        public void cmdSkillGFX(string[] c)
        {
            try
            {
                if (c[2] != null)
                    SendPlayerSkillMotion(Skills.getSkillByNameIDAndLevel(int.Parse(c[1]), 1), (Mover)WorldHelper.GetClientByPlayerName(c[2]),ACTIONSLOT_ORDER_NO, UNKNOWN_MOTIONVALUE3);
                else
                    SendPlayerSkillMotion(Skills.getSkillByNameIDAndLevel(int.Parse(c[1]),1),3);
            }
            catch
            {
                try
                {
                    SendPlayerSkillMotion(Skills.getSkillByNameIDAndLevel(int.Parse(c[1]),1),3);
                }
                catch { }
            }
        }
        public void cmdAllbuffs(string[] c)
        {
            string uname = null;
            try
            {
                uname = c[1];
            }
            catch { }
            Client x;
            if (uname == null)
                x = this;
            else
                x = WorldHelper.GetClientByPlayerName(uname);
            if (x == null)
                return;
            int[] buffs = new int[]{
                49,50,52,53,114,115,116,20,146,147,148,150
            };
            for (int i = 0; i < buffs.Length; i++)
            {
                Buff curBuff = new Buff();
                Skills skill = new Skills();
                if (buffs[i] == 146|| buffs[i] == 147||buffs[i] == 148||buffs[i] == 150)
                    skill = Skills.getSkillByNameIDAndLevel(buffs[i], 10);
                else
                skill = Skills.getSkillByNameIDAndLevel(buffs[i],20);
                curBuff._skill = skill;                
                curBuff.dwTime = 1200 * 1000;
                x.SendPlayerBuff(curBuff);
                Skills.setBuffEffect(curBuff, x);
                x.SendPlayerStats();
            }
        }
        public void cmdSetPvpPoints(string[] c)
        {
            string uname = null;
            int points = -1;
            try
            {
                points = int.Parse(c[1]);
                uname = c[2];
            }
            catch { }
            if (points == -1)
                return;
            Client player;
            if (uname == null)
                player = this;
            else
                player = WorldHelper.GetClientByPlayerName(uname);
            if (player == null)
                return;
            player.c_data.dwReputation = points;
            player.SendPlayerFameUpdate();
        }
        public void cmdKick(string[] c)
        {
            try
            {
                Client player = WorldHelper.GetClientByPlayerName(StringUtilities.FillString(c, 1));
                ISCRemoteServer.SendKickFromServers(player.c_data.dwAccountID);
            }
            catch { }
        }
        public void cmdSetJob(string[] c)
        {
            string uname = null;
            int jobid = -1;
            try
            {
                jobid = int.Parse(c[1]);
                uname = c[2];
            }
            catch { }
            if (jobid == -1)
                return;
            Client player;
            if (uname == null)
                player = this;
            else
                player = WorldHelper.GetClientByPlayerName(uname);
            if (player == null)
                return;
            player.c_data.dwClass = jobid;
        }
        public void cmdSetCheer(string[] c)
        {
            string otherplayer = "";
            int cheerCount = -1;
            int cheerTime = -1;
            try
            {
                cheerCount = int.Parse(c[1]);
                otherplayer = c[2];
                cheerTime = int.Parse(c[3]);
            }
            catch { }
            Client otherclient = null;
            if (otherplayer != "")
                otherclient = WorldHelper.GetClientByPlayerName(otherplayer);
            else
                otherclient = this;
            if (otherclient == null)
                return;
            if (cheerCount != -1)
                otherclient.c_data.dwCheerPoints = cheerCount;
            if (cheerTime != -1)
                otherclient.timers.nextCheer = DLL.time() + 5 + (uint)cheerTime;
            otherclient.SendPlayerCheerData();
        }
        public void cmdTeleTo(string[] c)
        {
            string otherplayer = "";
            try { otherplayer = c[1]; }
            catch { return; }
            Client otherclient = WorldHelper.GetClientByPlayerName(otherplayer);
            if (otherclient == null)
                return;
            c_position.x = otherclient.c_position.x;
            c_position.y = otherclient.c_position.y;
            c_position.z = otherclient.c_position.z;
            if (otherclient.c_data.dwMapID != c_data.dwMapID)
            {
                c_data.dwMapID = otherclient.c_data.dwMapID;
                SendPlayerMapTransfer();
            }
            else
            {
                SendMoverNewPosition();
            }
        }
        public void cmdTeleToMe(string[] c)
        {
            string otherplayer = "";
            try
            {
                otherplayer = c[1];
            }
            catch { return; }
            Log.Write(Log.MessageType.debug, otherplayer);
            Client otherclient = WorldHelper.GetClientByPlayerName(otherplayer);
            if (otherclient == null)
                return; // player doesn't exist
            otherclient.c_position.x = c_position.x;
            otherclient.c_position.y = c_position.y;
            otherclient.c_position.z = c_position.z;
            if (otherclient.c_data.dwMapID != c_data.dwMapID)
            {
                otherclient.c_data.dwMapID = c_data.dwMapID;
                otherclient.SendPlayerMapTransfer();
            }
            else
            {
                otherclient.SendMoverNewPosition();
            }
        }
        public void cmdStat(string[] c) // Rewritten by Nicco (Added .stat setall nr)
        {
            string stat = "";
            string pname = "";
            int points = 0;
            try { stat = c[1]; }
            catch (Exception) { return; }
            try { points = int.Parse(c[2]); }
            catch (Exception) { points = 15; }
            try { pname = c[3]; }
            catch (Exception) { pname = null; }
            Client otherclient = null;
            if (pname == null)
                otherclient = this;
            else
                otherclient = WorldHelper.GetClientByPlayerName(pname);
            switch (stat.ToLower())
            {
                case "str":
                    otherclient.c_attributes[DST_STR] = points;
                    otherclient.RebuildAttributes(); // by exos
                    break;
                case "sta":
                    otherclient.c_attributes[DST_STA] = points;
                    otherclient.RebuildAttributes();
                    break;
                case "dex":
                    otherclient.c_attributes[DST_DEX] = points;
                    otherclient.RebuildAttributes();
                    break;
                case "int":
                    otherclient.c_attributes[DST_INT] = points;
                    otherclient.RebuildAttributes();
                    break;
                case "setall":
                    {
                        otherclient.c_attributes[DST_STR] = points;
                        otherclient.c_attributes[DST_STA] = points;
                        otherclient.c_attributes[DST_DEX] = points;
                        otherclient.c_attributes[DST_INT] = points;
                        otherclient.RebuildAttributes();
                        break;
                    }
                default:
                    {
                        SendMessageInfo(FlyFF.TID_ADMIN_ANNOUNCE, "\"Unknown stat: " + stat + "\"");
                        return;
                    }
            }
            SendMessageInfo(FlyFF.TID_ADMIN_ANNOUNCE, "\"Base stats updated on character: " + otherclient.c_data.strUsername + "\"");
            SendMessageInfo(FlyFF.TID_ADMIN_ANNOUNCE,
                String.Format("\"STR: {0}, STA: {1}, DEX: {2}, INT: {3}\"", otherclient.c_attributes[DST_STR],
                otherclient.c_attributes[DST_STA], otherclient.c_attributes[DST_DEX], otherclient.c_attributes[DST_INT]));
            otherclient.SendPlayerStats();
        }
        public void cmdTeleport(string[] c)
        {
            /*
             * Teleport bug fixed (revision 7)
             * Many thanks to Rynti from InGamers.de for finding out the source of the bug!!!
             */
            int mapid = 0;
            float x = 0, z = 0, y = c_position.y;
            try
            {
                mapid = int.Parse(c[1]);
                string c2 = c[2], c3 = c[3];
                c[2] = c[2].Split('.')[0];
                c[3] = c[3].Split('.')[0];
                if (c2 == c[2])
                    c[2] = c[2].Split(',')[0];
                if (c2 == c[2])
                {
                    Log.Write(Log.MessageType.warning, "Client::cmdTeleport(): failed to convert position X");
                    return;
                }
                if (c3 == c[3])
                    c[3] = c[3].Split(',')[0];
                if (c3 == c[3])
                {
                    Log.Write(Log.MessageType.warning, "Client::cmdTeleport(): failed to convert position Z");
                    return;
                }
                x = int.Parse(c[2]);
                z = int.Parse(c[3]);
            }
            catch
            {
                try
                {
                    if (c[1].StartsWith("\"") && c[1].EndsWith("\""))
                    {
                        Client cc = WorldHelper.GetClientByPlayerName(c[1].Replace("\"", ""));
                        if (cc == null)
                            return;
                        x = cc.c_position.x;
                        z = cc.c_position.z;
                        y = cc.c_position.y;
                        mapid = cc.c_data.dwMapID;
                    }
                    else
                        return;
                }
                catch { return; }
            }
            c_position.x = x;
            c_position.z = z;
            c_position.y = y;
            if (c_data.dwMapID != mapid)
            {
                c_data.dwMapID = mapid;
                SendPlayerMapTransfer();
            }
            else
                SendMoverNewPosition();
        }
        public void cmdLua(string[] c)
        {
            string path = StringUtilities.FillString(c, 1);
            if (!System.IO.File.Exists("lua\\" + path + ".lua"))
            {
                SendMessageInfo(FlyFF.TID_ADMIN_ANNOUNCE, "\"File does not exist.\"");
                return;
            }
            LuaInterface.Lua lua = new LuaInterface.Lua();
            lua["luabin"] = new LuaBinary();
            try
            {
                lua.DoFile("lua\\" + path + ".lua");
            }
            catch (Exception e)
            {
                SendMessageHud(e.Message);
            }
        }
        public void cmdTele(string[] c)
        {
            float[] pos = new float[3];
            try
            {
                pos[0] = Convert.ToSingle(c[1]);
                pos[1] = Convert.ToSingle(c[2]);
                pos[2] = Convert.ToSingle(c[3]);
            }
            catch { return; }
            c_position.x = pos[0];
            c_position.y = pos[1];
            c_position.z = pos[2];
            SendMoverNewPosition();
        }
        public void cmdGo(string[] c)
        {
            string locationname = "";

            try { locationname = c[1]; }
            catch { return; }

            List<MAKELOCATION> svLoc = Server.goLocations;
            MAKELOCATION valid = null;
            for (int i = 0; i < svLoc.Count; i++)
            {
                MAKELOCATION curLoc = svLoc[i];
                if (locationname.ToLower() == curLoc.name.ToLower())
                {
                    valid = curLoc;
                    break;
                }
                else
                {
                    if (!Server.go_enable_indexing)
                        continue;
                    try
                    {
                        if (int.Parse(locationname) == i)
                        {
                            valid = curLoc;
                            break;
                        }
                    }
                    catch { }
                }
            }
            if (valid == null)
                return;
            c_position.x = valid.x;
            c_position.y = valid.y;
            c_position.z = valid.z;
            if (valid.mapID != c_data.dwMapID)
            {
                c_data.dwMapID = valid.mapID;
                SendPlayerMapTransfer();
            }
            else
                SendMoverNewPosition();
        }
        public void cmdSetLevel(string[] c)
        {
            string uname = null;
            int newlevel = 1;
            try
            {
                newlevel = int.Parse(c[1]);
                uname = c[2];
            }
            catch { }
            Client player;
            if (uname == null)
                player = this;
            else
                player = WorldHelper.GetClientByPlayerName(uname);
            if (player == null)
                return;
            if (newlevel < 1)
                return;
            //check if has the correct job lvl
            if ((newlevel>15)&&(c_data.dwClass ==0))
                newlevel = 15; //can't go more
            if ((newlevel>60)&&(c_data.dwClass <5))
                newlevel = 60; //must get the secong job before
            #region Stat Part
            if (player.c_data.dwLevel < newlevel)
            {
                player.c_data.dwStatPoints += (newlevel - player.c_data.dwLevel) * 2;
                player.SendPlayerStatPoints();
            }
            else
            {
                //reset attribut
                c_attributes[DST_STR] = 15;
                c_attributes[DST_STA] = 15;
                c_attributes[DST_DEX] = 15;
                c_attributes[DST_INT] = 15;
                
                int statPoints = (newlevel * 2) - 2;
                int LvlLess60 = newlevel - 60;
                if (LvlLess60 < 0)
                    LvlLess60 = 0;
                if (c_data.dwClass >= 16)
                    statPoints += LvlLess60;
                if (c_data.dwClass >= 24)
                    statPoints += 15;
                c_data.dwStatPoints = statPoints;
                SendPlayerStats();
            }
            #endregion
            #region Skill part - easiest way = reskill
            int vagrant = 0, // Extra Skillpoints which get added by job.
                                           mecenary = 40,
                                           acrobat = 50,
                                           assist = 60,
                                           magician = 90,
                                           knight = 80,
                                           blade = 80,
                                           elementer = 300,
                                           psykeeper = 90,
                                           billposter = 120,
                                           ringmaster = 100,
                                           ranger = 100,
                                           jester = 100;

            //reset all skill level from server side :
            for (int i = 0; i < c_data.skills.Count; i++)
            {
                if (c_data.skills.Count > 0)
                {
                    Skill curSkill = c_data.skills[i];
                    curSkill.dwSkillLevel = 0;
                }
            }

            //calculate point for level NOTE that all if will be read for a character level 120 for exemple

            c_data.dwSkillPoints = newlevel * 2;
            if (newlevel >= 20)
                c_data.dwSkillPoints += (newlevel - 19); //+1 for each level above 20 (corresponding to+3)
            if (newlevel >= 40)
                c_data.dwSkillPoints += (newlevel - 39); //+1 for each level above 40(corresponding to+4)
            if (newlevel >= 60)
                c_data.dwSkillPoints += (newlevel - 59); //+1 for each level above 60(corresponding to+5)
            if (newlevel >= 80)
                c_data.dwSkillPoints += (newlevel - 79); //+1for each level above 80(corresponding to+6)
            if (newlevel >= 100)
                c_data.dwSkillPoints += (newlevel - 99); //+1 for each level above 100(corresponding to+7)


            switch (c_data.dwClass)
            {
                case 0: break;
                case 1: c_data.dwSkillPoints += mecenary; break;
                case 2: c_data.dwSkillPoints += acrobat; break;
                case 3: c_data.dwSkillPoints += assist; break;
                case 4: c_data.dwSkillPoints += magician; break;
                case 6: c_data.dwSkillPoints += knight + mecenary; break;
                case 7: c_data.dwSkillPoints += blade + mecenary; break;
                case 8: c_data.dwSkillPoints += jester + acrobat; break;
                case 9: c_data.dwSkillPoints += ranger + acrobat; break;
                case 10: c_data.dwSkillPoints += ringmaster + assist; break;
                case 11: c_data.dwSkillPoints += billposter + assist; break;
                case 12: c_data.dwSkillPoints += psykeeper + magician; break;
                case 13: c_data.dwSkillPoints += elementer + magician; break;
                case 16:
                case 24:
                case 17:
                case 25: c_data.dwSkillPoints = 658; break;
                case 18:
                case 19:
                case 26:
                case 27: c_data.dwSkillPoints = 688; break;
                case 20:
                case 28: c_data.dwSkillPoints = 698; break;
                case 21:
                case 29: c_data.dwSkillPoints = 718; break;
                case 22:
                case 30: c_data.dwSkillPoints = 718; break;
                case 23:
                case 31: c_data.dwSkillPoints = 928; break;
                default:
                    SendMessageHud("Invalid Job!"); break;
            }
            SendPlayerSkills();

            #endregion
            player.c_data.dwLevel = newlevel;
            player.c_data.qwExperience = ClientDB.EXP[newlevel-1];
            player.SendPlayerCombatInfo();
        }
        public void cmdSetEXP(string[] c)
        {
            string uname = null;
            long newexp = 0;
            try
            {
                newexp = long.Parse(c[1]);
                uname = c[2];
            }
            catch { }
            Client player;
            if (uname == null)
                player = this;
            else
                player = WorldHelper.GetClientByPlayerName(uname);
            if (player == null)
                return;
            player.c_data.qwExperience = newexp;
            player.SendPlayerCombatInfo();
        }
        public void cmdSetSP(string[] c)
        {
            string uname = null;
            int newsp = 1;
            try
            {
                newsp = int.Parse(c[1]);
                uname = c[2];
            }
            catch { }
            Client player;
            if (uname == null)
                player = this;
            else
                player = WorldHelper.GetClientByPlayerName(uname);
            if (player == null)
                return;
            player.c_data.dwSkillPoints = newsp;
            player.SendPlayerCombatInfo();
        }
        public void cmdSetAP(string[] c)
        {
            string uname = null;
            int newap = 1;
            try
            {
                newap = int.Parse(c[1]);
                uname = c[2];
            }
            catch { }
            Client player;
            if (uname == null)
                player = this;
            else
                player = WorldHelper.GetClientByPlayerName(uname);
            if (player == null)
                return;
            player.c_data.dwStatPoints = newap;
            player.SendPlayerStatPoints();
        }
        public void cmdDebugItems()
        {
        }
        public void cmdClearInventory()
        {
            for (int i = 0; i < c_data.inventory.Count; i++)
                if (c_data.inventory[i].dwPos < 0x2A)
                    DeleteItem(c_data.inventory[i]);
        }
        public void cmdAddItem(string[] c)
        {
            Slot slot = GetFirstAvailableSlot();
            if (slot == null)
            {
                SendMessageInfo(TID_GAME_LACKSPACE);
                return;
            }
            int dwID = -1, dwQuantity = 1, dwElement = 0, dwEleRefine = 0, dwSocketCount = 0, dwRefine = 0;
            try
            {
                dwID = int.Parse(c[1]);
                dwQuantity = int.Parse(c[2]);
                dwRefine = int.Parse(c[3]);
                dwElement = int.Parse(c[4]);
                dwEleRefine = int.Parse(c[5]);
                dwSocketCount = int.Parse(c[6]);
                if (dwSocketCount > 4)
                {
                    if (dwID >= 22366 && dwID <= 22417)//ultimate weapon
                        dwSocketCount = 5;
                    else
                        dwSocketCount = 4;
                }
            }
            catch
            {
                if (dwID == -1)
                {
                    SendMessageHud("Wrong syntax! usage: .item <ID> [quantity] [refine] [element] [element refine] [socket count]");
                    return;
                }
            }
            Item item = new Item()
            {
                dwItemID = dwID,
                dwElement = LimitNumber(dwElement, 0, 5),
                dwEleRefine = LimitNumber(dwEleRefine, 0, 10),
                c_sockets = new ItemSockets(dwSocketCount),
                dwRefine = LimitNumber(dwRefine, 0, 10)
            };
            item.dwQuantity = LimitNumber(dwQuantity, 1, item.Data.stackMax);
            CreateItem(item, slot);
        }
        public void cmdSetLogLevel(string[] c)
        {
            try
            {
                int t = int.Parse(c[1]);
                Log.DebugLevel = t;
            }
            catch { }
        }
        public void cmdDebug()
        {
            SendMessageHud("Spawned movers: " + c_spawns.Count);
            int clients = 0, npcs = 0;
            for (int i = 0; i < c_spawns.Count; i++)
            {
                if (c_spawns[i] is Client)
                {
                    Client thisclient = (Client)c_spawns[i];
                    SendMessageHud(string.Format("Spawned player mover: {0} name: {1}", thisclient.dwMoverID, thisclient.c_data.strPlayerName));
                    clients++;
                }
                else if (c_spawns[i] is NPC)
                {
                    NPC npcthis = (NPC)c_spawns[i];
                    SendMessageHud(string.Format("Spawned NPC mover: {0} model: {1} type: {2}", npcthis.dwMoverID, npcthis.npc_model_id, npcthis.npc_type_name));
                    npcs++;
                }
            }
            SendMessageHud(string.Format("Spawned NPCs: {0}/{1}", npcs, c_spawns.Count));
            SendMessageHud(string.Format("Spawned clients: {0}/{1}", clients, c_spawns.Count));
            SendMessageHud("Amount of threads running in server: " + System.Diagnostics.Process.GetCurrentProcess().Threads.Count);
            SendMessageHud("Position: " + c_position);
            SendMessageHud("PlayerFlags: " + c_data.m_PlayerFlags);
        }
        public void configUpdateWorldRates()
        {
            TextWriter newRatesFile = new StreamWriter("conf/WorldRates.ini");
            newRatesFile.WriteLine("## Exp rate");
            newRatesFile.WriteLine("exp_rate={0}", Server.exp_rate);
            newRatesFile.WriteLine();
            newRatesFile.WriteLine("## Drop rate, for each increase it will calculate again items of the mob.");
            newRatesFile.WriteLine("## Note: Penya and Quest Items will only drop one per mob, unless you have partyskill!");
            newRatesFile.WriteLine("## Drop rate increases amount of calculations, which mean if u have 10x you can get 10 items");
            newRatesFile.WriteLine("## but that's only if the chance to get one item is 100%");
            newRatesFile.WriteLine("drop_rate={0}", Server.drop_rate);
            newRatesFile.WriteLine();
            newRatesFile.WriteLine("## Penya rate");
            newRatesFile.WriteLine("## If mob would drop 100,000 penya with 1x, then 2x penya rate would make it 200,000");
            newRatesFile.Write("penya_rate={0}", Server.penya_rate);
            newRatesFile.Close();
            SendMessageInfoNotice("New rates succefully saved to WorldRates.ini");
        }
    }
}

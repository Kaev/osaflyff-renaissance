using System;
using System.Collections;
using System.Text;
using System.IO;
namespace FlyffWorld.Databases
{
    public class Initializer
    {
        public static void Load()
        {
            Log.Write(Log.MessageType.load, "Loading world data..");
            for (int i = 0; i < WorldServer.world_players.Count; i++)
            {
                Client other = WorldServer.world_players[i];
                while (other.c_spawns.Count > 0)
                {
                    if (other.c_spawns[0] is Monster)
                        other.DespawnMonster((Monster)other.c_spawns[0]);
                    else if (other.c_spawns[0] is NPC)
                        other.DespawnNPC((NPC)other.c_spawns[0]);
                }
            }
            WorldServer.data_boxes.Clear();
            WorldServer.world_npcs.Clear();
            WorldServer.world_warpzones.Clear();
            WorldServer.world_npcshops.Clear();
            WorldServer.world_monsters.Clear();
            WorldServer.data_items.Clear();
            WorldServer.data_skills.Clear();
            WorldServer.data_mobs.Clear();
            WorldServer.world_parties.Clear();
            WorldServer.data_resrgns.Clear();
            WorldServer.world_mails.Clear();
            WorldServer.world_characterlist.Clear();
            WorldServer.data_collrgns.Clear();            
            
            LoadOthers();
            

            #region Load spawns_npcs
            ResultSet rs = new ResultSet("SELECT * FROM flyff_spawns_npcs");
            while (rs.Advance())
            {
                int worldID = rs.Readint("flyff_worldid");
                int model = rs.Readint("flyff_model");
                string type = rs.Readstring("flyff_typename");
                float x = rs.Readfloat("flyff_positionx");
                float y = rs.Readfloat("flyff_positiony");
                float z = rs.Readfloat("flyff_positionz");
                int angle = rs.Readint("flyff_angle");
                int sizemod = rs.Readint("flyff_size");
                int speechdelay = rs.Readint("flyff_speechdelay");
                string speech = rs.Readstring("flyff_speechtext");
                Point pos = new Point(x, y, z, angle);
                NPC curNPC = new NPC(model, type);
                curNPC.c_position = pos;
                curNPC.npc_size = sizemod;
                curNPC.npc_mapid = worldID;
                curNPC.npc_speech_delay = speechdelay;
                curNPC.npc_speech = speech;
                curNPC.npc_next_speech_time = DLL.time() + curNPC.npc_speech_delay + WorldServer.c_random.Next(5, 20);
                WorldServer.world_npcs.Add(curNPC);
            }
            rs.Free();
            Log.Write(Log.MessageType.load, "Loaded {0} spawned NPCs.", WorldServer.world_npcs.Count);
            #endregion
            #region Load characters list
            rs = new ResultSet("SELECT * FROM flyff_characters");
            while (rs.Advance())
            {
                CharacterList charlist = new CharacterList();
                charlist.CharacterName = rs.Readstring("flyff_charactername");
                charlist.dwCharID = rs.Readint("flyff_characterid");

                WorldServer.world_characterlist.Add(charlist);
            }
            rs.Free();
            Log.Write(Log.MessageType.load, "Loaded {0} playernames.", WorldServer.world_characterlist.Count);
            #endregion

            #region Load spawns_mobs
            rs = new ResultSet("SELECT * FROM flyff_spawns_mobs");
            while (rs.Advance())
            {
                Monster mob;
                int modelID = rs.Readint("flyff_modelID");
                int chance = rs.Readint("flyff_aggressiveMobChance");
                bool aggressive = DiceRoller.Roll(chance);
                Point pos = new Point(
                    rs.Readfloat("flyff_positionx"),
                    rs.Readfloat("flyff_positiony"),
                    rs.Readfloat("flyff_positionz"));
                int respawnDelay = rs.Readint("flyff_respawnDelay");
                //int spawnID = rs.Readint("flyff_spawnID");
                int mapID = rs.Readint("flyff_mapID");
                bool frozen = rs.Readint("flyff_frozen") != 0;
                bool giant = rs.Readint("flyff_mobIsGiant") != 0;
                mob = new Monster(modelID, pos, mapID, aggressive);
                mob.mob_isdead = false;
                mob.mob_respawndelay = respawnDelay;
                mob.c_destiny = pos;
                mob.c_attributes[FlyFF.DST_SPEED] = 100;
                WorldServer.world_monsters.Add(mob);
                switch (mob.mob_mapID)
                {
                    case 1: //madrigal
                        //need to separate flaris/saint morning and darkon
                        if ((mob.c_position.x > 6250 && mob.c_position.x < 8260) && (mob.c_position.z > 2850 && mob.c_position.z < 4665))//flaris region
                            WorldServer.Flaris_monsters.Add(mob);
                        else if ((mob.c_position.x > 8261 && mob.c_position.x < 9210) && (mob.c_position.z > 1700 && mob.c_position.z < 4665))//saint morning region
                            WorldServer.SM_monsters.Add(mob);
                        else//in other case
                            WorldServer.Darkon_monsters.Add(mob);
                        break;

                    case 2: //azria
                        WorldServer.Azria_monsters.Add(mob);
                        break;
                    case 3://cIsland
                        WorldServer.Cisland_monsters.Add(mob); break;
                    case 151:
                    case 152:
                    case 153:
                    case 154:
                    case 155:
                    case 156:
                    case 157:
                    case 158:
                    case 159:
                    case 160:
                    case 161:
                    case 162:
                    case 163:
                    case 164:
                    case 165:
                    case 166:
                    case 167: //forgotten tower
                        WorldServer.Tower_monsters.Add(mob);
                        break;
                    case 200://mas dungeon
                    case 210:
                    case 220:
                    case 230:
                        WorldServer.Flaris_monsters.Add(mob);
                        break;
                    case 201: //darkon dungeon
                        WorldServer.Darkon_monsters.Add(mob);
                        break;
                    case 204:
                    case 205:
                        WorldServer.SM_monsters.Add(mob);
                        break;
                    default:
                        WorldServer.other_monsters.Add(mob);
                        break;
                }
                    

            }
            rs.Free();
            Log.Write(Log.MessageType.load, "Loaded {0} spawned monsters.({1} in flaris, {2} in saint morning, {3} in darkon, {4} in azria, {5} in coral island, {6} in other worlds) ", WorldServer.world_monsters.Count, WorldServer.Flaris_monsters.Count, WorldServer.SM_monsters.Count, WorldServer.Darkon_monsters.Count, WorldServer.Azria_monsters.Count, WorldServer.Cisland_monsters.Count, WorldServer.other_monsters.Count);
            #endregion
            #region Load warpzones
            rs = new ResultSet("SELECT * FROM flyff_warpzones");
            while (rs.Advance())
            {
                Point source = new Point(rs.Readfloat("flyff_sourcex"), rs.Readfloat("flyff_sourcey"), rs.Readfloat("flyff_sourcez"));
                Point dest = new Point(rs.Readfloat("flyff_destx"), rs.Readfloat("flyff_desty"), rs.Readfloat("flyff_destz"));
                float radius = rs.Readfloat("flyff_radius");
                int sMap = rs.Readint("flyff_sourcemap"), dMap = rs.Readint("flyff_destmap");
                WorldServer.world_warpzones.Add(new Warpzone(source, dest, sMap, dMap, radius));
            }
            rs.Free();
            Log.Write(Log.MessageType.load, "Loaded {0} warpzones.", WorldServer.world_warpzones.Count);
            #endregion
            #region Load guilds
            if (WorldServer.world_guilds.Count != 0)
            {
                rs = new ResultSet("SELECT * FROM flyff_guilds");
                while (rs.Advance())
                {
                    Guild guild = new Guild();
                    guild.guildID = rs.Readint("flyff_guildID");
                    guild.founderID = rs.Readint("flyff_founderID");
                    guild.guildName = rs.Readstring("flyff_guildName");
                    guild.guildLogo = rs.Readint("flyff_guildLogo");
                    guild.guildLevel = rs.Readint("flyff_guildLevel");
                    guild.guildContributionEXP = rs.Readint("flyff_guildContributionEXP");
                    guild.gwWins = rs.Readint("flyff_gwWins");
                    guild.gwLoses = rs.Readint("flyff_gwLoses");
                    guild.gwForfeits = rs.Readint("flyff_gwForfeits");
                    string[] ranks = new string[] { "Master", "Kingpin", "Captain", "Supporter", "Rookie" };
                    for (int i = 0; i < 5; i++)
                    {
                        guild.memberPrivileges[i] = rs.Readint("flyff_privileges" + ranks[i]);
                        guild.memberPayment[i] = rs.Readint("flyff_payment" + ranks[i]);
                    }
                    guild.guildNotice = rs.Readstring("flyff_guildNotice");
                    guild.bankPenya = rs.Readint("flyff_bankPenya");
                    ResultSet rs2 = new ResultSet("SELECT * FROM flyff_guildmembers WHERE flyff_guildID = {0}", guild.guildID);
                    while (rs2.Advance())
                    {
                        GuildMember member = new GuildMember();
                        member.characterID = rs2.Readint("flyff_characterID");
                        member.memberNickname = rs2.Readstring("flyff_memberNickname");
                        member.questContribution = rs2.Readint("flyff_memberQuestContribution");
                        member.penyaContribution = rs2.Readint("flyff_memberPenyaContribution");
                        member.memberRank = rs2.Readint("flyff_memberRank");
                        member.memberRankSymbolCount = rs2.Readint("flyff_memberRankSymbolCount");
                        member.gwForfeits = rs2.Readint("flyff_gwForfeits");
                        guild.members.Add(member);
                    }
                    rs2.Free();
                    WorldServer.world_guilds.Add(guild);
                }
                rs.Free();
                Log.Write(Log.MessageType.load, "Loaded {0} guilds.", WorldServer.world_guilds.Count);
            }
            #endregion
            #region Load shopdata
            try
            {
                string[] npcfiles = Directory.GetFiles("db\\shopdata");
                for (int i = 0; i < npcfiles.Length; i++)
                {
                    string npctype = npcfiles[i].Split('.')[0].Split('\\')[2];
                    NPCShopData nsd = new NPCShopData(npctype);
                    nsd.npctype = npctype;
                    WorldServer.world_npcshops.Add(nsd);
                }
            }
            catch (Exception e)
            {
                Log.Write(Log.MessageType.fatal, "Error loading NPC shops! " + e.Message);
            }
            Log.Write(Log.MessageType.load, "Loaded {0} NPC shops.", WorldServer.world_npcshops.Count);
            #endregion
            MDDLoader.LoadMDDFiles();
            SummonBalls.LoadSummonBalls();
            //QuestLoader.LoadQuest();
            #region Load event
            try
            {
                System.IO.StreamReader sw = new System.IO.StreamReader(@"db\events.db");
                for (int x = 0; x < 1024; x++)
                {
                    Server.events[x] = sw.BaseStream.ReadByte();
                    if (Server.events[x] == -1)
                    {
                        Server.events[x] = 0;
                        Log.Write(Log.MessageType.warning, "Incomplete events database. required file size is 1024 bytes.");
                        break;
                    }
                }

                Log.Write(Log.MessageType.load, "Loaded events. {0} events are up, {1} events are down.", Server.CountByValue(Server.events, 1), Server.CountByValue(Server.events, 0));
            }
            catch
            {
                Log.Write(Log.MessageType.error, "Failed to load events information!");
            }
            #endregion
            #region Load Buff Pang
            Server.helper_buffs.Clear();
            Server.helper_buffs_levels.Clear();
            IniFile BuffPangConfig = new IniFile("conf/BuffPang.ini");
            Server.helper_levelreq_min = BuffPangConfig.Readint("minlvl", 1);
            Server.helper_levelreq_max = BuffPangConfig.Readint("maxlvl", 59);
            Server.helper_buff_cost = BuffPangConfig.Readint("buffcost", 0);
            Server.buff_emsg = BuffPangConfig.ReadValue("lvlmsg", "");
            Server.buff_cmsg = BuffPangConfig.ReadValue("costmsg", "");
            while (Server.buff_emsg.IndexOf("<minlvl>") > 0)
                Server.buff_emsg = Server.buff_emsg.Replace("<minlvl>", String.Format("{0}", Server.helper_levelreq_min));
            while (Server.buff_emsg.IndexOf("<maxlvl>") > 0)
                Server.buff_emsg = Server.buff_emsg.Replace("<maxlvl>", String.Format("{0}", Server.helper_levelreq_max));
            while (Server.buff_cmsg.IndexOf("<buffcost>") > 0)
                Server.buff_cmsg = Server.buff_cmsg.Replace("<buffcost>", String.Format("{0}", Server.helper_buff_cost));
            int l = 0;
            while (true)
            {
                string cID = BuffPangConfig.ReadValue("buff_" + l + "_id", null);
                if (cID == null) break;
                string cLV = BuffPangConfig.ReadValue("buff_" + l + "_lv", null);
                if (cLV == null)
                {
                    Log.Write(Log.MessageType.warning, "No buff level assigned for buff id: {0}", cID);
                    l++;
                    continue;
                }
                try
                {
                    // Add id's/lvl's to the arrays
                    Server.helper_buffs.Add(int.Parse(cID));
                    Server.helper_buffs_levels.Add(int.Parse(cLV));
                }
                catch { Log.Write(Log.MessageType.fatal, "Could not add buff {0} to the array!", cID); }
                l++;
            }
            BuffPangConfig.destroy();
            Log.Write(Log.MessageType.load, "Loaded {0} helper NPC buffs.", l);
            #endregion
            #region Load boxdata            
            DirectoryInfo boxdir = new DirectoryInfo(@"db/boxdata");            
            FileInfo[] boxfiles = boxdir.GetFiles("*.ibd");
            int boxtotal = 0;
            for (int i = 0; i < boxfiles.Length; i++)
            {                
                string dest = "db\\boxdata\\" + boxfiles[i].Name;
                
                ItemBoxData box = new ItemBoxData();                
                box.ExecuteScript(dest);                
                WorldServer.data_boxes.Add(box);
                boxtotal++;
            }
            Log.Write(Log.MessageType.load, "Loaded {0} item boxes.", boxtotal);

            Log.Write(Log.MessageType.info, "World data load finished at {0}.", DateTime.Now.ToString());
            #endregion
        }
        public static void LoadOthers()
        {
            #region Load set bonus
            ResultSet rs = new ResultSet("SELECT * FROM flyff_data_sets"); // by exos
            while (rs.Advance())
            {
                SetData set = new SetData();
                set.helmetID = rs.Readint("helmetID");
                for (int i = 1; i < 8; i++)
                {
                    int abilityType = rs.Readint("abilityType" + i.ToString());
                    int abilityValue = rs.Readint("abilityValue" + i.ToString());
                    int partCount = rs.Readint("partCount" + i.ToString());
                    switch (partCount)
                    {
                        case 2:
                            set.setBonuses2[abilityType] += abilityValue;
                            set.setBonuses3[abilityType] += abilityValue;
                            set.setBonuses4[abilityType] += abilityValue;
                            break;
                        case 3:
                            set.setBonuses3[abilityType] += abilityValue;
                            break;
                        case 4:
                            set.setBonuses4[abilityType] += abilityValue;
                            break;
                    }
                }
                WorldServer.data_sets.Add(set);
            }
            rs.Free();
            Log.Write(Log.MessageType.load, "Loaded {0} sets data.", WorldServer.data_sets.Count);
            #endregion
            #region Load revival region
            rs = new ResultSet("SELECT * FROM flyff_revivalregions");
            while (rs.Advance())
            {
                RevivalRegion region = new RevivalRegion();
                region.c_destiny = new Point(rs.Readfloat("fDestX"), rs.Readfloat("fDestZ"));
                region.dwDestMap = rs.Readint("dwDestMapID");
                region.dwSrcMap = rs.Readint("dwSrcMapID");
                region.f_northwest_x = rs.Readfloat("fNorthWestCornerX");
                region.f_northwest_z = rs.Readfloat("fNorthWestCornerZ");
                region.f_southeast_x = rs.Readfloat("fSouthEastCornerX");
                region.f_southeast_z = rs.Readfloat("fSouthEastCornerZ");
                WorldServer.data_resrgns.Add(region);
            }
            rs.Free();
            Log.Write(Log.MessageType.load, "Loaded {0} revival regions.", WorldServer.data_resrgns.Count);
            #endregion
            #region Load collect region
            rs = new ResultSet("SELECT * FROM flyff_collectregions");
            while (rs.Advance())
            {
                CollectRegion region = new CollectRegion();
                region.Map = rs.Readint("worldid");
                region.f_northwest_x = rs.Readfloat("fNorthWestCornerX");
                region.f_northwest_z = rs.Readfloat("fNorthWestCornerZ");
                region.f_southeast_x = rs.Readfloat("fSouthEastCornerX");
                region.f_southeast_z = rs.Readfloat("fSouthEastCornerZ");
                WorldServer.data_collrgns.Add(region);
            }
            rs.Free();
            Log.Write(Log.MessageType.load, "Loaded {0} collect regions.", WorldServer.data_collrgns.Count);
            #endregion
            
            #region Load data items
            rs = new ResultSet("SELECT * FROM flyff_data_items");
            while (rs.Advance())
            {
                ItemData item = new ItemData();
                item.itemID = rs.Readint("flyff_itemID");
                item.itemName = rs.Readstring("flyff_itemName");
                item.stackMax = rs.Readint("flyff_stackmax");
                item.itemkind = new int[]
                {
                    rs.Readint("flyff_itemkind1"),
                    rs.Readint("flyff_itemkind2"),
                    rs.Readint("flyff_itemkind3")
                };
                item.reqJob = rs.Readint("flyff_reqJob");
                item.adjAttributes = new int[]
                {
                    rs.Readint("flyff_adjParamValue1"),
                    rs.Readint("flyff_adjParamValue2"),
                    rs.Readint("flyff_adjParamValue3")
                };
                item.canShop = rs.Readbool("flyff_shopable");
                item.chgAttributes = new int[]
                {
                    rs.Readint("flyff_chgParamValue1"),
                    rs.Readint("flyff_chgParamValue2"),
                    rs.Readint("flyff_chgParamValue3")
                };
                item.destAttributes = new int[]
                {
                    rs.Readint("flyff_destParam1"),
                    rs.Readint("flyff_destParam2"),
                    rs.Readint("flyff_destParam3")
                };
                item.equipSlot = new int[]
                {
                    rs.Readint("flyff_equipSlotMain"),
                    rs.Readint("flyff_equipSlotSub")
                };
                item.npcPrice = rs.Readint("flyff_npcPrice");
                item.reqGender = rs.Readint("flyff_reqGender");
                item.reqJob = rs.Readint("flyff_reqJob");
                item.reqLevel = rs.Readint("flyff_reqLevel");
                item.twoHanded = rs.Readint("flyff_xhanded") > 1;
                item.min_ability = rs.Readint("flyff_abilitymin");
                item.max_ability = rs.Readint("flyff_abilitymax");
                item.endurance = rs.Readint("flyff_endurance");
                item.itemAtkOrder1 = rs.Readint("itemAtkOrder1");
                item.itemAtkOrder2 = rs.Readint("itemAtkOrder2");
                item.itemAtkOrder3 = rs.Readint("itemAtkOrder3");
                item.weaponType = rs.Readint("weaponType");
                item.skillTime = rs.Readint("skillTime");
                item.textFile = rs.Readstring("textFile");
                item.circleTime = rs.Readint("circleTime");
                WorldServer.data_items.Add(item);
            }
            Log.Write(Log.MessageType.load, "Loaded {0} items.", WorldServer.data_items.Count);
            rs.Free();
            #endregion
            #region load mails
            rs = new ResultSet("SELECT * FROM flyff_mails");
            while (rs.Advance())
            {
                Mails mail = new Mails();
                mail.mailid = WorldServer.world_mails.Count+1;
                mail.fromCharID = rs.Readint("flyff_fromcharid");
                mail.toCharID = rs.Readint("flyff_tocharid");
                mail.date = rs.Readint("flyff_date");
                Item item ;
                string sockets = rs.Readstring("flyff_sockets");
                if (sockets == "")
                {
                    item = new Item();
                }
                else
                {
                    string[] socketdata = rs.Readstring("flyff_sockets").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    item = new Item(socketdata.Length);
                    for (int j = 0; j < socketdata.Length; j++)
                    {
                        try
                        {
                            item.c_sockets[j] = int.Parse(socketdata[j]);
                        }
                        catch
                        {
                            item.c_sockets[j] = 0;
                        }
                    }
                }
                item.dwItemID = rs.Readint("flyff_itemid");
                item.dwQuantity = rs.Readint("flyff_itemcount");
                item.dwRefine = rs.Readint("flyff_refinelevel");
                item.dwElement = rs.Readint("flyff_elementType");
                item.dwEleRefine = rs.Readint("flyff_elementRefine");
                item.c_awakening = rs.Readulong("flyff_awakening");
                item.qwLastUntil = rs.Readlong("flyff_lastuntil");
                mail.attachedItem = item;
                mail.attachedPenya = rs.Readint("flyff_attpenya");
                mail.topic = rs.Readstring("flyff_topic");
                mail.message = rs.Readstring("flyff_message");
                mail.isRead = rs.Readint("flyff_isRead");
                WorldServer.world_mails.Add(mail);
            }
            Log.Write(Log.MessageType.load, "Loaded {0} mails.", WorldServer.world_mails.Count);
            rs.Free();
            #endregion
            #region Load data mobs
            rs = new ResultSet("SELECT * FROM flyff_data_mobs");
            while (rs.Advance())
            {
                MonsterData mob = new MonsterData();
                mob.isFlying = rs.Readbool("flyff_isFlying");
                mob.isKillable = rs.Readbool("flyff_isKillable");
                mob.mobAspd = rs.Readint("flyff_attackspeed");
                mob.mobDex = rs.Readint("flyff_dexterity");
                mob.mobElement = rs.Readint("flyff_element");
                mob.mobElementPower = rs.Readint("flyff_elementpower");
                mob.mobExpPoints = rs.Readint("flyff_experiencepoints");
                mob.mobHP = rs.Readint("flyff_hitpoints");
                mob.mobID = rs.Readint("flyff_mobID");
                mob.mobInt = rs.Readint("flyff_intelligence");
                mob.mobLevel = rs.Readint("flyff_level");
                mob.mobMoveSpeed = rs.Readfloat("flyff_movespeed");
                mob.mobMp = rs.Readint("flyff_manapoints");
                mob.mobName = rs.Readstring("flyff_mobName");
                mob.mobFlee = rs.Readint("flyff_evasion");
                mob.mobResistMagic = rs.Readint("dwResisMagic");
                mob.mobResistance = new float[]
                {
                    rs.Readfloat("flyff_resist:fire"),
                    rs.Readfloat("flyff_resist:water"),
                    rs.Readfloat("flyff_resist:electricity"),
                    rs.Readfloat("flyff_resist:wind"),
                    rs.Readfloat("flyff_resist:earth")
                };
                mob.mobSize = rs.Readint("flyff_size");
                mob.mobSta = rs.Readint("flyff_stamina");
                mob.mobStr = rs.Readint("flyff_strength");
                mob.mobCash = rs.Readint("dwCash"); // Nicco->Drops
                /* ADDED */
                mob.atkMin = rs.Readint("dwAtkMin");
                mob.atkMax = rs.Readint("dwAtkMax");
                mob.atkDelay = rs.Readint("dwReAttackDelay");
                mob.attacks[0] = rs.Readint("dwAtk1");
                mob.attacks[1] = rs.Readint("dwAtk2");
                mob.attacks[2] = rs.Readint("dwAtk3");
                mob.def = rs.Readint("dwNaturealArmor");
                WorldServer.data_mobs.Add(mob);
            }
            rs.Free();
            Log.Write(Log.MessageType.load, "Loaded {0} monster data records.", WorldServer.data_mobs.Count);
            #endregion
            #region Load data skills
            //add data_skills table
            rs = new ResultSet("SELECT * FROM flyff_data_skills");
            while (rs.Advance())
            {
                int dwName = rs.Readint("dwName"); //=dwSkillid for skill table and for send packet. Adidishen use the "name" like an id
                int dwSkillLvl = rs.Readint("dwSkillLvl");
                
                int dwAbilityMin = rs.Readint("dwAbilityMin");
                int dwAtkAbilityMax = rs.Readint("dwAtkAbilityMax");
                int dwAbilityMinPVP = rs.Readint("dwAbilityMinPVP");
                int dwAbilityMaxPVP = rs.Readint("dwAbilityMaxPVP");
                int dwAttackSpeed = rs.Readint("dwAttackSpeed");
                int dwDmgShift = rs.Readint("dwDmgShift");
                int dwProbability = rs.Readint("dwProbability");
                int dwProbabilityPVP = rs.Readint("dwProbabilityPVP");
                int dwTaunt = rs.Readint("dwTaunt");
                int dwDestParam1 = rs.Readint("dwDestParam1");
                int dwDestParam2 = rs.Readint("dwDestParam2");
                int dwAdjParamVal1 = rs.Readint("dwAdjParamVal1");
                int dwAdjParamVal2 = rs.Readint("dwAdjParamVal2");
                int dwChangeParamVal1 = rs.Readint("dwChangeParamVal1");
                int dwChangeParamVal2 = rs.Readint("dwChangeParamVal2");
                int dwdestData1 = rs.Readint("dwdestData1");
                int dwdestData2 = rs.Readint("dwdestData2");
                int dwdestData3 = rs.Readint("dwdestData3");
                int dwactiveskill = rs.Readint("dwactiveskill");
                int dwActiveSkillRate = rs.Readint("dwActiveSkillRate");
                int dwActiveSkillRatePVP = rs.Readint("dwActiveSkillRatePVP");
                int dwReqMp = rs.Readint("dwReqMp");
                int dwReqFP = rs.Readint("dwReqFP");
                int dwCooldown = rs.Readint("dwCooldown");
                int dwCastingTime = rs.Readint("dwCastingTime");
                int dwSkillRange = rs.Readint("dwSkillRange");
                int dwCircleTime = rs.Readint("dwCircleTime");
                int dwPainTime = rs.Readint("dwPainTime");
                int dwSkillTime = rs.Readint("dwSkillTime");
                int dwSkillCount = rs.Readint("dwSkillCount");
                int dwSkillExp = rs.Readint("dwSkillExp");
                int dwExp = rs.Readint("dwExp");
                int dwComboSkillTime = rs.Readint("dwComboSkillTime");
                Skills skills = new Skills(dwName, dwSkillLvl, dwAbilityMin, dwAtkAbilityMax, dwAbilityMinPVP, dwAbilityMaxPVP, dwAttackSpeed, dwDmgShift, dwProbability, dwProbabilityPVP, dwTaunt, dwDestParam1, dwDestParam2, dwAdjParamVal1, dwAdjParamVal2, dwChangeParamVal1, dwChangeParamVal2, dwdestData1, dwdestData2, dwdestData3, dwactiveskill, dwActiveSkillRate, dwActiveSkillRatePVP, dwReqMp, dwReqFP, dwCooldown, dwCastingTime, dwSkillRange, dwCircleTime, dwPainTime, dwSkillTime, dwSkillCount, dwSkillExp, dwExp, dwComboSkillTime);
                WorldServer.data_skills.Add(skills);
            }
            rs.Free();
            Log.Write(Log.MessageType.load, "Loaded {0} skills.", WorldServer.data_skills.Count);
            //end section data_skill
            #endregion


        }
        /*
flyff_mobID	int	11	0	0	0	0	0	0	0	0					-1	0
flyff_mobName	varchar	100	0	0	0	0	0	0		0		latin1	latin1_swedish_ci		0	0
flyff_size	float	0	0	0	0	0	0	0	1	0					0	0
dwAI	int	11	0	0	0	0	0	0		0					0	0
flyff_strength	int	11	0	0	0	0	0	0		0					0	0
flyff_stamina	int	11	0	0	0	0	0	0		0					0	0
flyff_dexterity	int	11	0	0	0	0	0	0		0					0	0
flyff_intelligence	int	11	0	0	0	0	0	0		0					0	0
dwHR	int	11	0	0	0	0	0	0		0					0	0
dwER	int	11	0	0	0	0	0	0		0					0	0
dwRace	int	11	0	0	0	0	0	0		0					0	0
dwBelligerence	int	11	0	0	0	0	0	0		0					0	0
flyff_gender	int	11	0	0	0	0	0	0		0					0	0
flyff_level	int	11	0	0	0	0	0	0		0					0	0
dwFilghtLevel	int	11	0	0	0	0	0	0		0					0	0
dwSize	int	11	0	0	0	0	0	0		0					0	0
dwClass	int	11	0	0	0	0	0	0		0					0	0
bIfPart	int	11	0	0	0	0	0	0		0					0	0
dwKarma	int	11	0	0	0	0	0	0		0					0	0
dwUseable	int	11	0	0	0	0	0	0		0					0	0
dwActionRadius	int	11	0	0	0	0	0	0		0					0	0
dwAtkMin	int	11	0	0	0	0	0	0		0					0	0
dwAtkMax	int	11	0	0	0	0	0	0		0					0	0
dwAtk1	int	11	0	0	0	0	0	0		0					0	0
dwAtk2	int	11	0	0	0	0	0	0		0					0	0
dwAtk3	int	11	0	0	0	0	0	0		0					0	0
dwHorizontalRate	int	11	0	0	0	0	0	0		0					0	0
dwVerticalRate	int	11	0	0	0	0	0	0		0					0	0
dwDiagonalRate	int	11	0	0	0	0	0	0		0					0	0
dwThrustRate	int	11	0	0	0	0	0	0		0					0	0
dwChestRate	int	11	0	0	0	0	0	0		0					0	0
dwHeadRate	int	11	0	0	0	0	0	0		0					0	0
dwArmRate	int	11	0	0	0	0	0	0		0					0	0
dwLegRate	int	11	0	0	0	0	0	0		0					0	0
flyff_attackspeed	int	11	0	0	0	0	0	0		0					0	0
dwReAttackDelay	int	11	0	0	0	0	0	0		0					0	0
flyff_hitpoints	int	11	0	0	0	0	0	0		0					0	0
flyff_manapoints	int	11	0	0	0	0	0	0		0					0	0
dwNaturealArmor	int	11	0	0	0	0	0	0		0					0	0
nAbrasion	int	11	0	0	0	0	0	0		0					0	0
nHardness	int	11	0	0	0	0	0	0		0					0	0
dwAdjAtkDelay	int	11	0	0	0	0	0	0		0					0	0
flyff_element	int	11	0	0	0	0	0	0		0					0	0
flyff_elementpower	int	11	0	0	0	0	0	0		0					0	0
dwHideLevel	int	11	0	0	0	0	0	0		0					0	0
flyff_movespeed	float	0	0	0	0	0	0	0		0					0	0
dwShelter	int	11	0	0	0	0	0	0		0					0	0
flyff_isFlying	int	11	0	0	0	0	0	0		0					0	0
dwJumpIng	int	11	0	0	0	0	0	0		0					0	0
dwAirJump	int	11	0	0	0	0	0	0		0					0	0
bTaming	int	11	0	0	0	0	0	0		0					0	0
dwResisMagic	int	11	0	0	0	0	0	0		0					0	0
flyff_resist:electricity	float	0	0	0	0	0	0	0		0					0	0
flyff_resist:fire	float	0	0	0	0	0	0	0		0					0	0
flyff_resist:wind	float	0	0	0	0	0	0	0		0					0	0
flyff_resist:water	float	0	0	0	0	0	0	0		0					0	0
flyff_resist:earth	float	0	0	0	0	0	0	0		0					0	0
dwCash	int	11	0	0	0	0	0	0		0					0	0
dwSourceMaterial	int	11	0	0	0	0	0	0		0					0	0
dwMaterialAmount	int	11	0	0	0	0	0	0		0					0	0
dwCohesion	int	11	0	0	0	0	0	0		0					0	0
dwHoldingTime	int	11	0	0	0	0	0	0		0					0	0
dwCorrectionValue	int	11	0	0	0	0	0	0		0					0	0
flyff_experiencepoints	int	11	0	0	0	0	0	0		0					0	0
nFxpValue	int	11	0	0	0	0	0	0		0					0	0
nBodyState	int	11	0	0	0	0	0	0		0					0	0
dwAddAbility	int	11	0	0	0	0	0	0		0					0	0
flyff_isKillable	int	11	0	0	0	0	0	0		0					0	0
dwVirtItem1	int	11	0	0	0	0	0	0		0					0	0
dwVirtType1	int	11	0	0	0	0	0	0		0					0	0
dwVirtItem2	int	11	0	0	0	0	0	0		0					0	0
dwVirtType2	int	11	0	0	0	0	0	0		0					0	0
dwVirtItem3	int	11	0	0	0	0	0	0		0					0	0
dwVirtType3	int	11	0	0	0	0	0	0		0					0	0
dwSndAtk1	int	11	0	0	0	0	0	0		0					0	0
dwSndAtk2	int	11	0	0	0	0	0	0		0					0	0
dwSndDie1	int	11	0	0	0	0	0	0		0					0	0
dwSndDie2	int	11	0	0	0	0	0	0		0					0	0
dwSndDmg1	int	11	0	0	0	0	0	0		0					0	0
dwSndDmg2	int	11	0	0	0	0	0	0		0					0	0
dwSndDmg3	int	11	0	0	0	0	0	0		0					0	0
dwSndIdle1	int	11	0	0	0	0	0	0		0					0	0
dwSndIdle2	int	11	0	0	0	0	0	0		0					0	0
szComment	varchar	300	0	0	0	0	0	0		0		latin1	latin1_swedish_ci		0	0
*/
    }
}
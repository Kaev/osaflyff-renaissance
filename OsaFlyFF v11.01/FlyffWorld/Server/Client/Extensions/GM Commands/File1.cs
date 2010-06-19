/* CMD Rev 3.0 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace FlyffWorld
{
    public class CAuthority
    {
        public bool Normal()
        {
            bool ret = false;
            if (auth_lvl >= 70)
                ret = true;
            return ret;
        }
        public bool GM()
        {
            bool ret = false;
            if (auth_lvl >= 80)
                ret = true;
            return ret;
        }
        public bool Admin()
        {
            bool ret = false;
            if (auth_lvl >= 100)
                ret = true;
            return ret;
        }
        public bool Owner()
        {
            bool ret = false;
            if (auth_lvl >= 120)
                ret = true;
            return ret;
        }
        private int auth_lvl = 70;
        public int dwAuthoritylvl()
        {
            return auth_lvl;
        }
        public CAuthority(int dwAuthority)
        {
            auth_lvl = dwAuthority;
        }
    }
    public partial class Client
    {
        public bool teleportMode = false;
        public bool invisibleMode = false; // used by .hide, .show and /invisible (swapping)
        public bool parseUserCommands(string c)
        {
            string[] commands = StringUtilities.QuoteIgnoringSplit(c, ' ', true);
            switch (commands[0])
            {
                case "s":
                case "shout":
                    SendPlayerShout(StringUtilities.FillString(commands, 1));
                    break;
                case "say":
                    cmdSay(commands); // /say "Nicco" PM System
                    break;
                default:
                    return false;
            }
            return true; // false = show message
        }
        public bool parseCommands(string c)
        {
            CAuthority Authority = new CAuthority(c_data.dwAuthority);
            if (!Authority.GM())
                return parseUserCommands(c);
            string[] commands = StringUtilities.QuoteIgnoringSplit(c, ' ', true);
            switch (commands[0])
            {
#if DEBUG       // command will not be added if the worldserver is not in debug mode.
                case "__ENTER_BREAKPOINT__":
                    ;
                    break;
#endif
                case "clearinv":
                    cmdClearInventory();
                    break;
                case "n2":
                    cmdSendHudNotice(commands);
                    break;
                case "lang":
                    cmdLanguage(commands);
                case "hide":
                    if ((c_data.m_PlayerFlags & PlayerFlags.INVISIBLE) != PlayerFlags.INVISIBLE)
                        c_data.m_PlayerFlags |= PlayerFlags.INVISIBLE;
                    SendPlayerFlagsUpdate();
                    invisibleMode = true;
                    SendMessageInfoNotice("You are now invisible from other players.");
                    break;
                case "show":
                    if ((c_data.m_PlayerFlags & PlayerFlags.INVISIBLE) == PlayerFlags.INVISIBLE)
                        c_data.m_PlayerFlags ^= PlayerFlags.INVISIBLE;
                    SendPlayerFlagsUpdate();
                    invisibleMode = false;
                    SendMessageInfoNotice("You are now visible to other players.");
                    break;
                case "triggertelemode":
                    teleportMode = !teleportMode;
                    if (teleportMode)
                    {
                        SendMessageHud("Teleport mode is ON.");
                        SendMessageHud("Mouse click moving will now be instant teleport to the location.");
                        SendMessageHud("Since this is server sided, there may be a delay between the mouse click and the teleport.");
                    }
                    else
                        SendMessageHud("Teleport mode is OFF.");
                    break;
                case "StopSnow":
                case "StopRain":
                    if (Authority.Admin()) // Admin only
                        parseCommands("weather none");
                    break;
                case "debugitems":
                    if (Authority.Admin()) // Admin only
                        cmdDebugItems();
                    break;
                case "createguild":
                    parseCommands("guild create" + StringUtilities.FillString(commands, 1));
                    break;
                case "FallSnow":
                    if (Authority.Admin()) // Admin only
                        parseCommands("weather snow");
                    break;
                case "FallRain":
                    if (Authority.Admin()) // Admin only
                        parseCommands("weather rain");
                    break;
                case "sound":
                    cmdSound(commands);
                    break;
                case "soundall":
                    if (Authority.Admin()) // Admin only
                        cmdSound(commands, true);
                    break;
                case "music":
                    cmdMusic(commands);
                    break;
                case "musicall":
                    if (Authority.Admin()) // Admin only
                        cmdMusic(commands, true);
                    break;
                case "revive":
                    cmdRevive(commands);
                    break;
                case "kill":
                    if (Authority.Admin()) // Admin only
                        cmdKill(commands);
                    break;
                case "fun_setmover":
                    if (Authority.Admin()) // Admin only
                        cmdFunSetMover(commands);
                    break;
                case "fun_stop":
                    if (Authority.Admin()) // Admin only
                        cmdFunStopControl();
                    break;
                case "fun_command":
                    if (Authority.Admin()) // Admin only
                        cmdFunCommand(commands);
                    break;
                case "guild":
                    cmdGuild(commands);
                    break;
                case "sendmessage":
                    if (Authority.Admin()) // Admin only
                        cmdSendFlyffMessage(commands);
                    break;
                case "spawnnpc":
                    if (Authority.Admin()) // Admin only
                        cmdNpcSpawn(commands);
                    break;
                case "_debug_":
                    if (Authority.Admin()) // Admin only
                        cmdDebug();
                    break;
                case "getgold":
                case "addpenya":
                    cmdAddPenya(commands);
                    break;
                case "lua":
                    cmdLua(commands);
                    break;
                case "notice":
                case "n":
                    if (Authority.Admin()) // Admin only
                    {
                        try
                        {
                            string notice = c.Split(new char[] { ' ' }, 2)[1];
                            for (int i = 0; i < WorldServer.world_players.Count; i++)
                                WorldServer.world_players[i].SendMessageAnnouncement(c_data.strPlayerName + ": " + notice);
                        }
                        catch (Exception) { }
                    }
                    break;
                case "__item":
                    cmdItemTest(commands);
                    break;
                case "trace":
                    cmdSetMoverTrace();
                    break;
                case "item":
                    cmdAddItem(commands);
                    break;
                case "loglevel":
                    if (Authority.Admin()) // Admin only
                        cmdSetLogLevel(commands);
                    break;
                case "reload":
                    if (Authority.Admin()) // Admin only
                        ISCRemoteServer.SendRefresh();
                    Databases.Initializer.Load();
                    break;
                case "tele":
                    cmdTele(commands);
                    break;
                case "teleport":
                    cmdTeleport(commands);
                    break;
                case "go":
                    cmdGo(commands);
                    break;
                case "fpak":
                    if (Authority.Admin()) // Admin only
                        ParseFilePacketType1(commands);
                    break;
                case "fpak2":
                    if (Authority.Admin()) // Admin only
                        ParseFilePacketType2(commands);
                    break;
                case "fpak3":
                    if (Authority.Admin()) // Admin only
                        ParseFilePacketType3(commands);
                    break;
                case "level": // by fnL
                case "setlvl":
                    cmdSetLevel(commands);
                    break;
                case "setexp":
                    cmdSetEXP(commands);
                    break;
                case "setsp":
                    cmdSetSP(commands);
                    break;
                case "setap":
                    cmdSetAP(commands);
                    break;
                case "stat":
                    cmdStat(commands);
                    break;
                case "teletome":
                    cmdTeleToMe(commands);
                    break;
                case "teleto":
                    cmdTeleTo(commands);
                    break;
                case "setcheer":
                    cmdSetCheer(commands);
                    break;
                case "kick":
                    cmdKick(commands);
                    break;
                case "setjob":
                    cmdSetJob(commands);
                    break;
                case "setpvp":
                    cmdSetPvpPoints(commands);
                    break;
                case "mute":
                    break;
                case "allbuffs":
                    cmdAllbuffs(commands);
                    break;
                case "skillgfx":
                    if (Authority.Admin()) // Admin only
                        cmdSkillGFX(commands);
                    break;
                case "flyffgfx":
                    if (Authority.Admin()) // Admin only
                        cmdFlyffGfx(commands);
                    break;
                case "weather":
                    if (Authority.Admin()) // Admin only
                        cmdWeather(commands);
                    break;
                case "attrib":
                    cmdAttribute(commands);
                    break;
                case "_gdinfo":
                    if (Authority.Admin()) // Admin only
                        cmdDebugGD();
                    break;
                case "ci":
                case "createitem": // client command: /ci or /createitem
                    cmdCreateItem(commands);
                    break;
                case "summon": // summon "[monster name]" [amount]
                    if (Authority.Admin()) // Admin only
                        cmdSummon(commands);
                    break;
                case "im": // im = information message debug
                    if (Authority.Admin()) // Admin only
                        cmdInformationMsgDebug(commands);
                    break;
                case "find": // By Nicco
                case "search":
                    cmdSearch(commands);
                    break;
                case "in": // in = information notice ~ Nicco
                case "infnotice":
                    cmdInformationNotice(commands);
                    break;
                case "serverstats":
                case "stats":
                case "ss": // I like short ones lol
                    cmdServerStats();
                    break;
                case "maxskills":
                    cmdMaxSkills();
                    break;
                case "heal":
                    cmdHeal(commands);
                    break;
                case "sa":
                case "setauthority": // Owner only, and the user has to be online! (Can be fixed tho)
                    if (Authority.Owner())
                        cmdSetAuthority(commands);
                    break;
                case "ak":
                case "aroundkill":
                    if (Authority.Admin())
                        cmdAroundKill(commands);
                    break;
                case "debugitems2":
                    if (Authority.Admin())
                        cmdDebugItems2();
                    break;
                case "invisible":
                    cmdInvisible();
                    break;
                case "karma":
                    cmdPKState(commands, 0);
                    break;
                case "disposition":
                    cmdPKState(commands, 1);
                    break;
                case "save":
                    cmdSave(false);
                    break;
                case "saveall": // Really usefull if u have to emergency restart server
                    cmdSave(true);
                    break;
                case "changejob": // client predefined command: /changejob "Knight" !
                    cmdChangeJob(commands);
                    break;
                case "sys":
                case "system":
                    if (Authority.Admin())
                        cmdSystem(commands);
                    break;
                case "drop":
                    if (Authority.Admin())
                        cmdDrop(commands);
                    break;
                case "droptime":
                    if (Authority.Owner())
                        cmdDropTime(commands);
                    break;
                case "restart":
                    if (Authority.Admin())
                        cmdRestart();
                    break;
                case "rate":
                    if (Authority.Admin())
                        cmdRate(commands);
                    break;
                case "cb":
                case "clearbuffs":
                    cmdClearBuffs(commands);
                    break;
                default:
                    return parseUserCommands(c);
            }
            string playerPos = String.Format("{0:0}, {1:0}, {2:0}, Map: {3}",
                c_position.x, c_position.y, c_position.z, c_data.dwMapID);
            Log.Write(Log.MessageType.gmcmd, "{0} [Account: {1}, Authority: {2}, Position: {3}]: {4}",
                c_data.strPlayerName, c_data.strUsername, c_data.dwAuthority, playerPos, c);
            return true;
        }
        public void cmdDropTime(string[] c)
        {/*#marked
            if (c.Length < 2)
                return;
            int itemid;
            int droptime = DLL.time()+36000;
            try
            {
                itemid = int.Parse(c[1]);
            }
            catch (Exception) { sendInformationNotice("Failed to convert parameter itemid to int."); return; }
            try
            {
                quantity = int.Parse(c[2]);
            }
            catch (Exception) { quantity = 1; }
            Item item = new Item();
            item.itemid = itemid;
            item.quantity = quantity;
            Drop newDrop = new Drop(item, 0, c_data.dwMapID, c_position, false);
            string itemName = item.Data.itemName.Replace(" @", ""); // Remove that ugly @ on ultimates
            sendInformationNotice("Dropped itemid: {0} (Quantity: {1})", itemName, item.quantity);
            WorldServer.world_drops.Add(newDrop);*/
        }
        public void cmdClearBuffs(string[] c)
        {
            for (int i = 0; i < c_data.buffs.Count; i++)
            {
                Buff cBuff = c_data.buffs[i];
                cBuff.buffTime = 5;
                SendPlayerBuff(cBuff);
            }
        }
        public void cmdRate(string[] c)
        {
            try
            {
                switch (c[1].ToLower())
                {
                    case "exp":
                        {
                            Server.exp_rate = int.Parse(c[2]);
                            SendMessageInfoNotice("Exp rate has been changed to: {0}x", Server.exp_rate);
                            configUpdateWorldRates();
                            break;
                        }
                    case "drop":
                        {
                            Server.drop_rate = int.Parse(c[2]);
                            SendMessageInfoNotice("Drop rate has been changed to: {0}x", Server.drop_rate);
                            configUpdateWorldRates();
                            break;
                        }
                    case "penya":
                        {
                            Server.penya_rate = int.Parse(c[2]);
                            SendMessageInfoNotice("Penya rate has been changed to: {0}x", Server.penya_rate);
                            configUpdateWorldRates();
                            break;
                        }
                    default: SendMessageInfoNotice("Unknown variable: {0}", c[1]); break;
                }
            }
            catch (Exception)
            {
                SendMessageInfoNotice("An error occured in the command, try again.");
                return;
            }
        }
        public void cmdRestart() // ONLY TO BE USED WHEN SERVERRESTARTER IS ONLINE
        {
            cmdSave(true); // Avoid exp/items loss
            for (int i = 0; i < WorldServer.world_players.Count; i++)
            {
                Client c = WorldServer.world_players[i];
                c.SendMessageInfoNotice("World server is being restarted, please re-login.");
            }
            System.Threading.Thread.Sleep(50);
            for (int i = 0; i < WorldServer.world_players.Count; i++)
            {
                Client c = WorldServer.world_players[i];
                ISCRemoteServer.SendKickUser(c.c_data.dwAccountID);
            }
            System.Threading.Thread.Sleep(100);
            Environment.Exit(0);
        }
        public void cmdDrop(string[] c) // Nicco->Drops
        {
            if (c.Length < 2)
                return;
            int itemid;
            int quantity = 1;
            try
            {
                itemid = int.Parse(c[1]);
            }
            catch (Exception) { SendMessageInfoNotice("Failed to convert parameter itemid to int."); return; }
            try
            {
                quantity = int.Parse(c[2]);
            }
            catch (Exception) { quantity = 1; }
            Item item = new Item();
            item.dwItemID = itemid;
            item.dwQuantity = quantity;
            Drop newDrop = new Drop(item, 0, c_data.dwMapID, c_position, false);
            string itemName = item.Data.itemName.Replace(" @", ""); // Remove that ugly @ on ultimates
            SendMessageInfoNotice("Dropped itemid: {0} (Quantity: {1})", itemName, item.dwQuantity);
            WorldServer.world_drops.Add(newDrop);
        }
        public void cmdSystem(string[] c)
        {
            if (c.Length < 2)
                return;
            string msg = "";
            for (int i = 1; i < c.Length; i++)
                msg += c[i] + " ";
            msg = msg.Remove(msg.Length - 1);
            if (msg == "")
                return;
            for (int i = 0; i < WorldServer.world_players.Count; i++)
            {
                Client client = WorldServer.world_players[i];
                client.SendMessageAnnouncement(msg);
            }
        }
        public void cmdChangeJob(string[] c)
        {
            if (c.Length < 2)
                return;
            string jobname = "";
            for (int i = 1; i < c.Length; i++)
                jobname += c[i] + " ";
            jobname = jobname.Remove(jobname.Length - 1).ToLower();
            string[] jobs = new string[] {
                "vagrant","mercenary","acrobat","assist","magician","knight","blade","jester","ranger",
                "ringmaster","billposter","psykeeper","elementor","master knight","master blade",
                "master jester","master ranger","master ringmaster","master billposter",
                "master psykeeper","master elementor","hero knight","hero blade","hero jester","hero ranger",
                "hero ringmaster","hero billposter","hero psykeeper","hero elementor" };
            int[] jobids = new int[] { 0, 1, 2, 3, 4, 6, 7, 8, 9, 10, 11, 12, 13, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31 };
            for (int i = 0; i < jobs.Length; i++)
            {
                if (String.Equals(jobs[i], jobname))
                {
                    c_data.dwClass = jobids[i];
                    return;
                }
            }
            SendMessageInfoNotice("No jobs were found with that name.");
        }
        public void cmdSave(bool bSaveEveryone)
        {
            int playersSaved = 0;
            if (bSaveEveryone)
            {
                for (int i = 0; i < WorldServer.world_players.Count; i++)
                {
                    Client client = WorldServer.world_players[i];
                    client.SaveAll(false);
                    playersSaved++;
                }
                SendMessageInfoNotice("{0} players saved.", playersSaved);
            }
            else
            {
                SaveAll(false);
                SendMessageInfoNotice("Your character have been saved.");
            }
        }
        public void cmdPKState(string[] c, int state)
        {
            if (c.Length < 2)
                return;
            Client target = this;
            if (c.Length > 2)
                try
                {
                    target = WorldHelper.GetClientByPlayerName(c[2]);
                }
                catch (Exception) { target = this; }
            int modifier = 0;
            try
            {
                modifier = int.Parse(c[1]);
            }
            catch (Exception) { return; }
            switch (state)
            {
                case 0:
                    target.c_data.dwKarma = modifier;
                    break;
                case 1:
                    target.c_data.dwDisposition = modifier;
                    break;
            }
        }
        public void cmdInvisible()
        {
            if (invisibleMode)
            {
                if ((c_data.m_PlayerFlags & PlayerFlags.INVISIBLE) == PlayerFlags.INVISIBLE)
                    c_data.m_PlayerFlags ^= PlayerFlags.INVISIBLE;
                SendPlayerFlagsUpdate();
                invisibleMode = false;
            }
            else
            {
                if ((c_data.m_PlayerFlags & PlayerFlags.INVISIBLE) != PlayerFlags.INVISIBLE)
                    c_data.m_PlayerFlags |= PlayerFlags.INVISIBLE;
                SendPlayerFlagsUpdate();
                invisibleMode = true;
            }
        }
        public void cmdDebugItems2()
        {
            
        }
        public void cmdAroundKill(string[] c)
        {
            int range = 70; // 70 is OK? [adidishen] 70 is alright, 75 would cover the whole minimap area :p
            try
            {
                range = int.Parse(c[1]);
            }
            catch { }
            /// [Adidishen]
            /// The following should NOT loop through all mobs
            /*
            for (int i = 0; i < WorldServer.world_monsters.Count; i++)
            {
                Monster m = WorldServer.world_monsters[i];
                if (c_position.IsInCircle(m.absolute_position, range))
                {
                    if (m.c_attributes[DST_HP] > 0)
                    {
                        int dmg = m.c_attributes[DST_HP];
                        m.sendDamage(dwMoverID, dmg, 0);
                        m.c_attributes[DST_HP] = 0;
                        c_data.qwExperience += ((((Monster)c_target).Data.mobExpPoints) * Server.exp_rate); // Don't copy from attackMonster() LOL
                        OnCheckLevelGroup();
                        updateLevelGroup();
                        m.sendMoverDeath();
                        m.mob_OnDeath();
                    }
                }
            }*/
            /// We have no choice but to limit it to c_spawns
            for (int i = 0; i < c_spawns.Count; i++)
            {
                if (c_spawns[i] is Monster && c_spawns[i].c_position.IsInCircle(c_position, range))
                {
                    Monster mob = (Monster)c_spawns[i];
                    if (mob.mob_isdead)
                        continue;
                    mob.SendMoverDamaged(dwMoverID, mob.c_attributes[DST_HP], (int)AttackFlags.CRITICAL); // critical: fun :)
                    mob.c_attributes[DST_HP] = 0;
                    c_data.qwExperience += mob.Data.mobExpPoints * Server.exp_rate;
                    OnCheckLevelGroup();
                    SendPlayerCombatInfo();
                    mob.SendMoverDeath();
                    mob.mob_OnDeath();
                }
            }
        }
        public void cmdSetAuthority(string[] c)
        {
            Client target;
            if (c.Length < 3) // .sa Nicco 120
                return;
            try
            {
                target = WorldHelper.GetClientByPlayerName(c[1]);
            }
            catch (Exception)
            {
                target = null;
                SendMessageInfoNotice("No users where found with the name: {0}", c[1]);
                return;
            }
            int newAuthority = 0;
            try
            {
                newAuthority = int.Parse(c[2]);
            }
            catch (Exception)
            {
                SendMessageInfoNotice("Please provide a new authority level!");
                return;
            }
            target.c_data.dwAuthority = newAuthority;
            Database.Execute("UPDATE `flyff_accounts` SET `flyff_authoritylevel`='{0}' WHERE `flyff_accountname`='{1}';",
                newAuthority, target.c_data.strUsername);
            SendMessageInfoNotice("Authority changed on {0} (account: {1}) to: {2}",
                target.c_data.strPlayerName,
                target.c_data.strUsername,
                newAuthority);
            if (c.Length == 4)
                if (c[3] == "0")
                {
                    target.SendMessageInfoNotice("Your authority level has been changed.");
                    return;
                }
            target.SendMessageInfoNotice("Your authority level has been changed, please relogin.");
            System.Threading.Thread.Sleep(500); // If kicking instant, the notice wont come.
            ISCRemoteServer.SendKickUser(target.c_data.dwAccountID);
        }
        public void cmdHeal(string[] c)
        {
            Client target;
            switch (c.Length)
            {
                case 2: // Must be 2 (.heal Nicco)
                    target = WorldHelper.GetClientByPlayerName(c[1]);
                    if (target == null)
                    {
                        SendMessageInfoNotice("No users where found with the name: {0}", c[1]);
                        return;
                    }
                    break;
                default:
                    target = this;
                    break;
            }
            /*while (true)
            {
                if(
                    (target.c_attributes[DST_HP] == target.c_data.f_MaxHP) &&
                    (target.c_attributes[DST_MP] == target.c_data.f_MaxMP) &&
                    (target.c_attributes[DST_FP] == target.c_data.f_MaxFP)
                ) break;
                target.idleHeal(); // Tried my own way but this works best.
            }*/
            SendPlayerAttribRaise(DST_HP, (target.c_data.f_MaxHP - target.c_attributes[DST_HP]));
            SendPlayerAttribRaise(DST_MP, (target.c_data.f_MaxMP - target.c_attributes[DST_MP]));
            SendPlayerAttribRaise(DST_FP, (target.c_data.f_MaxFP - target.c_attributes[DST_FP]));
            target.c_attributes[DST_HP] = target.c_data.f_MaxHP;
            target.c_attributes[DST_MP] = target.c_data.f_MaxMP;
            target.c_attributes[DST_FP] = target.c_data.f_MaxFP;
            SendMessageInfoNotice("Updated attributes on {0}: HP: {1}, MP: {2}, FP: {3}",
                target.c_data.strPlayerName, target.c_attributes[DST_HP],
                target.c_attributes[DST_MP], target.c_attributes[DST_FP]);
            if (target.c_data.strPlayerName != c_data.strPlayerName)
                target.SendMessageInfoNotice("{0} has healed you up.", c_data.strPlayerName);
        }
        public void cmdMaxSkills()
        {
            for (int i = 0; i < c_data.skills.Count; i++)
            {
                Skill curSkill = c_data.skills[i];
                switch (curSkill.dwSkillID)
                {
                    // vagrant skills
                    case 1:
                    case 2:
                    case 3:
                        curSkill.dwSkillLevel = 10;
                        break;

                    // 2nd job skills
                    case 128:
                    case 129:
                    case 130:
                    case 131:
                    case 132:
                    case 133:
                    case 134:
                    case 135:
                    case 136:
                    case 137:
                    case 138:
                    case 139:
                    case 140:
                    case 141:
                    case 142:
                    case 143:
                    case 207:
                    case 208:
                    case 209:
                    case 210:
                    case 211:
                    case 212:
                    case 213:
                    case 214:
                    case 215:
                    case 216:
                    case 217:
                    case 218:
                    case 219:
                    case 220:
                    case 221:
                    case 222:
                    case 144:
                    case 145:
                    case 146:
                    case 147:
                    case 148:
                    case 149:
                    case 150:
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
                    case 167:
                    case 168:
                    case 169:
                    case 170:
                    case 171:
                    case 172:
                    case 173:
                    case 174:
                    case 175:
                    case 176:
                    case 177:
                    case 178:
                    case 179:
                    case 180:
                    case 181:
                    case 182:
                    case 183:
                    case 184:
                    case 185:
                    case 186:
                        curSkill.dwSkillLevel = 10;
                        break;

                    // master skills
                    case 310:
                    case 309:
                    case 311:
                    case 312:
                    case 316:
                    case 315:
                    case 314:
                    case 313:
                        curSkill.dwSkillLevel = 5;
                        break;

                    // hero skills
                    case 238:
                    case 237:
                    case 239:
                    case 240:
                    case 244:
                    case 243:
                    case 242:
                    case 241:
                        curSkill.dwSkillLevel = 5;
                        break;

                    // 1st job skills
                    default:
                        curSkill.dwSkillLevel = 20;
                        break;
                }
            }
            SendPlayerSkills();
            SendMessageInfoNotice("All skills are now maxed.");
        }
    }
}

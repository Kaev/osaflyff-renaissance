using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using LuaInterface;

/// [Adidishen]
/// Now you don't need to modify using directives. :)

namespace FlyffWorld
{
    public partial class Client
    {
        public void cmdItemTest(string[] c)
        {
            int itemID = -1,
                type = -1,
                val = -1;
            try
            {
                itemID = int.Parse(c[1]);
                type = int.Parse(c[2]);
                val = int.Parse(c[3]);
            }
            catch { return; }
            Slot slot;
            if (type == 0)
                slot = GetSlotByPosition(val);
            else
                slot = GetSlotByID(val);
            CreateItem(new Item() { dwItemID = itemID }, slot);
        }
        public void cmdSetMoverTrace()
        {
            c_target.bIsTraced = !c_target.bIsTraced;
        }
        public void cmdSay(string[] c)
        {
            try
            {
                string msgTo = c[1];
                string msg = StringUtilities.FillString(c, 2);
                if (msg == "")
                    return;
                Client target = WorldHelper.GetClientByPlayerName(msgTo);
                if (target == null)
                {
                    SendMessageInfoNotice("The player {0} is not logged in or does not exist.", msgTo);
                    return;
                }
                SendMessagePM(this, target, msg);
            }
            catch { }
        }
        public void cmdLanguage(string[] c)
        {
            try
            {
                int dwLang = int.Parse(c[1]);
                string targetName = c[2];
                Client target;
                if (targetName == null || targetName == "")
                {
                    targetName = c_data.strPlayerName;
                    target = this;
                }
                else
                    target = WorldHelper.GetClientByPlayerName(targetName);
                if (target == null)
                    return;
                if (dwLang < 0 || dwLang > 6)
                    dwLang = 0;
                target.c_data.dwLanguage = dwLang;
                string lang = "";
                switch (dwLang)
                {
                    case 0: lang = "English"; break;
                    case 1: lang = "French"; break;
                    case 2: lang = "German"; break;
                    case 3: lang = "Spanish"; break;
                    case 4: lang = "Thai"; break;
                    case 5: lang = "Polish"; break;
                    default: lang = "English"; break;
                        SendMessageHud("Language of " + targetName + " is set to " + lang);
                }
            }
            catch { }
        }
        public void cmdPartyInvite(string[] c)
        {
            string targetName = "";
            try
            {
                targetName = c[1];
            }
            catch (Exception) { }

            Client target = WorldHelper.GetClientByPlayerName(targetName);
            if (target == null)
            {
                //sendInformationMessage(FlyFF.TID_GAME_NOTLOGIN);
                SendMessageInfoNotice("The player {0} is not logged in or does not exist.", targetName);
                return;
            }
            //we will need to check if player are in the same channel later
            //if asker is leader he can launch an invite unless he can not OR if asker is not in party we create one
            if (c_data.isInParty)
            {
                Party party = Party.getPartyByID(c_data.dwPartyID);
                if (c_data.dwCharacterID == party.leaderID)
                    sendInviteToParty(target);
            }
            else
            {
                sendInviteToParty(target);
            }

        }
        public void cmdSpawnMonsterDB(string[] c)
         {
             int mobID = 0, respawnDelay = 25, aggrochance = 0;
             try
             {
                 switch (c.Length)
                 {
                     case 2: mobID = int.Parse(c[1]); break;
                     case 3: mobID = int.Parse(c[1]); respawnDelay = int.Parse(c[2]); break;
                     case 4: mobID = int.Parse(c[1]); respawnDelay = int.Parse(c[2]); aggrochance = int.Parse(c[3]); break;
                     default: SendMessageInfoNotice("Could not parse Commands: No parameter given!"); return;
                 }
             }
             catch (Exception ex)
             { Log.Write(Log.MessageType.error, "Could not parse Commands: {0}", ex.Message); return; }

             Monster mob = new Monster(mobID, c_position, c_data.dwMapID, false);
             mob.respawn = true;
             mob.next_move_time = DLL.time() + 1;
             mob.mob_respawndelay = respawnDelay;

             if (aggrochance > 100) { aggrochance = 100; }

             if (DiceRoller.RandomNumber(1, 100) <= aggrochance)
                 mob.mob_aggressive = true;
            
             WorldServer.world_monsters.Add(mob);
             SendMonsterSpawn(mob);
             mob.SendMoverNewDestination();
             c_spawns.Add(mob);

             Database.Execute("INSERT INTO flyff_spawns_mobs " +
                 "( flyff_modelID,flyff_aggressiveMobChance,flyff_positionx,flyff_positiony,flyff_positionz,flyff_respawnDelay,flyff_mapID,flyff_frozen,flyff_mobIsGiant) VALUES " +
                 "(" + mobID + "," + aggrochance + "," + Convert.ToInt32(c_position.x) + "," + Convert.ToInt32(c_position.y) + "," + Convert.ToInt32(c_position.z) + "," + respawnDelay + "," + c_data.dwMapID + ",0,0);");
         }
         public void cmdSpawnNpcDB(string[] c)
         {
             // .addnpc modelid npc_name [size]
             int modelID = 0, size = 100;
             string npcName = "";
             try
             {
                 switch (c.Length)
                 {
                     case 3: modelID = int.Parse(c[1]); npcName = c[2]; break;
                     case 4: modelID = int.Parse(c[1]); npcName = c[2]; size = int.Parse(c[3]); break;
                     default: Log.Write(Log.MessageType.info, "Could not parse Commands: Unknown parameter!"); return;
                 }
             }
             catch (Exception ex)
             { Log.Write(Log.MessageType.error, "Could not parse Commands: {0}", ex.Message); return; }

             try
             {
                 NPC npc = new NPC(modelID, npcName);
                 npc.npc_size = size;
                 npc.c_position = c_position;
                 npc.npc_mapid = c_data.dwMapID;
                 WorldServer.world_npcs.Add(npc);
                 SendNPCSpawn(npc);
                 c_spawns.Add(npc);

                 Database.Execute("INSERT INTO flyff_spawns_npcs ( flyff_model, flyff_size, flyff_typename, flyff_speechtime, flyff_worldid, flyff_positionx, flyff_positiony, flyff_positionz, flyff_angle, flyff_speechdelay, flyff_speechtext) VALUES"
                     + "( " + modelID + ", " + size + ", '" + npcName + "', 15, " + c_data.dwMapID + ", " + Convert.ToInt32(c_position.x) + ", " + Convert.ToInt32(c_position.y) + ", " + Convert.ToInt32(c_position.z) + ", " + Convert.ToInt32(c_position.angle) + ", 10, '' );");
             }
             catch (Exception ex)
             { Log.Write(Log.MessageType.error, "Error at spawning NPC: {0} \n Stacktrace: \n {1}", ex.Message, ex.StackTrace); }
            
         }
         public void cmdResetStats(string[] c)
         {
             // .resetstats BlackGiant
             Client target;

             switch (c.Length)
             {
                 case 2:
                     target = WorldHelper.GetClientByPlayerName(c[1]); break;
                     if (target == null)
                     { SendMessageInfoNotice("No user was found with name: {0}", c[1]); return; }
                 default: // Use own character
                     target = this; break;
             }
             target.c_attributes[DST_STR] = 15;
             target.c_attributes[DST_STA] = 15;
             target.c_attributes[DST_DEX] = 15;
             target.c_attributes[DST_INT] = 15;
             int statPoints = (target.c_data.dwLevel * 2) - 2;
             int LvlLess60 = target.c_data.dwLevel - 60;
             if (LvlLess60 < 0)
                 LvlLess60 = 0;
             if (target.c_data.dwClass >= 16)
                 statPoints += LvlLess60;
             if (target.c_data.dwClass >= 24)
                 statPoints += 15;
             target.c_data.dwStatPoints = statPoints;
             SendPlayerStats();
             if (target == this)
                 SendMessageInfoNotice("Your stats were reseted!");
             else
                 SendMessageInfoNotice("The stats of {0} were reseted", target.c_data.strPlayerName);
         }
    }
}

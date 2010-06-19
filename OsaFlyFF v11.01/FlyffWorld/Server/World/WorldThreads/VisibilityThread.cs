using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace FlyffWorld
{
    public partial class WorldThreads
    {
        public void MonsterVisibilityThreadProcess()
        {
            Log.Write(Log.MessageType.pthread, "Monster visibility thread started, delay: {0} seconds", (double)Server.threadDelay_visibility / 1000);
            while (true)
            {
                if (ThreadsCalculationEnabled)
                {
                    // loop through all clients
                    for (int i = 0; i < WorldServer.world_players.Count; i++)
                    {
                        Client thisclient = WorldServer.world_players[i];
                        if (!thisclient.playerSpawned)
                            continue;
                        for (int l = 0; l < WorldServer.world_monsters.Count; l++)
                        {
                            Monster mob = WorldServer.world_monsters[l];
                            if (thisclient.c_position.IsInCircle(mob.c_position, 75) && thisclient.c_data.dwMapID == mob.mob_mapID && !thisclient.c_spawns.Contains(mob) && !mob.mob_isdead)
                                thisclient.SpawnMonster(mob);
                        }
                    }
                }
                Thread.Sleep(Server.threadDelay_visibility + ThreadsIdleSleepAddition);
            }
        }
        public void VisibilityThreadProcess()
        {
            Log.Write(Log.MessageType.pthread, "Visibility thread started, delay: {0} seconds", (double)Server.threadDelay_visibility / 1000);
            while (true)
            {
                if (ThreadsCalculationEnabled)
                {
                    for (int i = 0; i < WorldServer.world_players.Count; i++)
                    {
                        Client thisclient = WorldServer.world_players[i];
                        if (!thisclient.playerSpawned)
                            continue;
                        for (int l = 0; l < WorldServer.world_players.Count; l++)
                        {
                            Client otherclient = WorldServer.world_players[l];
                            if (otherclient == thisclient)
                                continue;
                            if (!otherclient.playerSpawned)
                                continue;
                            if (thisclient.c_position.IsInCircle(otherclient.c_position, 75) && thisclient.c_data.dwMapID == otherclient.c_data.dwMapID)
                            {
                                if (!thisclient.c_spawns.Contains(otherclient))
                                    thisclient.SpawnPlayer(otherclient);
                                if (!otherclient.c_spawns.Contains(thisclient))
                                    otherclient.SpawnPlayer(thisclient);
                            }
                        }
                        for (int l = 0; l < WorldServer.world_npcs.Count; l++)
                        {
                            NPC npc = WorldServer.world_npcs[l];
                            if (thisclient.c_position.IsInCircle(npc.c_position, 75) && !thisclient.c_spawns.Contains(npc) && thisclient.c_data.dwMapID == npc.npc_mapid)
                                thisclient.SpawnNPC(npc);
                        }
                        for (int l = 0; l < thisclient.c_spawns.Count; l++)
                        {
                            if (thisclient.c_spawns[l] is Client)
                            {
                                Client otherclient = (Client)thisclient.c_spawns[l];
                                if (!thisclient.c_position.IsInCircle(otherclient.c_position, 75) || thisclient.c_data.dwMapID != otherclient.c_data.dwMapID)
                                {
                                    if (otherclient == thisclient.c_target)
                                    {
                                        thisclient.bIsFollowing = false;
                                        thisclient.c_destiny = thisclient.c_position;
                                    }
                                    if (thisclient == otherclient.c_target)
                                    {
                                        otherclient.bIsFollowing = false;
                                        otherclient.c_destiny = otherclient.c_position;
                                    }
                                    thisclient.DespawnPlayer(otherclient);
                                    otherclient.DespawnPlayer(thisclient);
                                }
                            }
                            else if (thisclient.c_spawns[l] is NPC)
                            {
                                NPC npc = (NPC)thisclient.c_spawns[l];
                                if (!thisclient.c_position.IsInCircle(npc.c_position, 75) || thisclient.c_data.dwMapID != npc.npc_mapid)
                                {
                                    if (npc == thisclient.c_target)
                                    {
                                        thisclient.bIsFollowing = false;
                                        thisclient.c_destiny = thisclient.c_position;
                                    }
                                    thisclient.DespawnNPC(npc);
                                }
                            }
                            else if (thisclient.c_spawns[l] is Monster)
                            {
                                Monster mob = (Monster)thisclient.c_spawns[l];
                                if (!thisclient.c_position.IsInCircle(mob.c_position, 75) || thisclient.c_data.dwMapID != mob.mob_mapID)
                                {
                                    if (mob == thisclient.c_target)
                                    {
                                        thisclient.bIsFollowing = false;
                                        thisclient.c_destiny = thisclient.c_position;
                                    }
                                    thisclient.DespawnMonster(mob);
                                }
                            }
                            else if (thisclient.c_spawns[l] is Drop)
                            {
                                Drop drop = (Drop)thisclient.c_spawns[l];
                                if (!thisclient.c_position.IsVisible(drop.c_position) || thisclient.c_data.dwMapID != drop.MapID)
                                {
                                    thisclient.DespawnDrop(drop);
                                }
                            }
                        }
                        for (int l = 0; l < WorldServer.world_warpzones.Count; l++)
                        {
                            Warpzone warpzone = WorldServer.world_warpzones[l];
                            if (warpzone.srcMapID != thisclient.c_data.dwMapID)
                                continue;
                            if (warpzone.position.IsInSphere(thisclient.c_position, warpzone.radius))
                            {
                                lock (thisclient.c_data.dataLock)
                                {
                                    thisclient.c_position = warpzone.destination;
                                    thisclient.c_data.dwMapID = warpzone.dstMapID;
                                    thisclient.c_destiny = warpzone.destination;
                                }
                                thisclient.SendPlayerMapTransfer();
                            }
                        }
                        if (thisclient.bIsFollowing && thisclient.c_target != null)
                        {
                            thisclient.c_destiny = thisclient.c_target.c_position;
                            if (thisclient.c_position.IsInSphere(thisclient.c_target.c_position, 2f))
                                thisclient.bIsFollowing = false;
                        }
                        else if (thisclient.c_target == null && thisclient.bIsFollowing)
                            thisclient.bIsFollowing = false;
                        for (int l = 0; l < WorldServer.world_drops.Count; l++)
                        {
                            Drop drop = WorldServer.world_drops[i];
                            if (thisclient.c_position.IsInCircle(drop.c_position, 75) && (thisclient.c_data.dwMapID == drop.MapID) && !thisclient.c_spawns.Contains(drop))
                            {
                                thisclient.SpawnDrop(drop);
                            }
                        }
                    }
                }
                Thread.Sleep(Server.threadDelay_visibility + ThreadsIdleSleepAddition);
            }
        }
    }
}
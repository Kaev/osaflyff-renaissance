using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace FlyffWorld
{
    public partial class WorldThreads
    {
        public void MoverThreadProcess()
        {
            Log.Write(Log.MessageType.pthread, "Mover thread started, delay: {0} seconds", (double)Server.threadDelay_mover / 1000);
            while (true)
            {
                if (ThreadsCalculationEnabled)
                {
                    for (int i = 0; i < WorldServer.world_drops.Count; i++) // Nicco->Drops
                    {
                        Drop drop = WorldServer.world_drops[i];
                        lock (drop.moverLock)
                        {
                            drop.c_spawns = MoversHandler.GetSpawnedClients(drop);
                            if (!drop.neverDespawn) // If true the item always lays on floor
                            {
                                if (drop.despawnTime <= DLL.clock())
                                {
                                    for (int x = 0; x < drop.c_spawns.Count; x++)
                                    {
                                        Client c = (Client)drop.c_spawns[x];
                                        c.DespawnDrop(drop);
                                        WorldServer.world_drops.Remove(drop);
                                    }
                                }
                            }
                            else
                            {
                                if (drop.doRespawn)
                                {
                                    //if (drop.pickedup)
                                    //drop.generateNewDropData(drop.c_position, );
                                    if (drop.respawnTime <= DLL.clock())
                                    {
                                        if (drop.neverDespawn)
                                            drop.respawnTime = DLL.clock() + DiceRoller.RandomNumber(30000, 59999);//30s <-> 1m
                                        drop.despawnTime = DLL.clock() + 300000;
                                        for (int x = 0; x < drop.c_spawns.Count; x++)
                                        {
                                            Client c = (Client)drop.c_spawns[x];
                                            c.SpawnDrop(drop);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    for (int i = 0; i < WorldServer.world_npcs.Count; i++)
                    {
                        NPC thisnpc = WorldServer.world_npcs[i];
                        lock (thisnpc.moverLock)
                        {
                            thisnpc.c_spawns = MoversHandler.GetSpawnedClients(thisnpc);
                            if (thisnpc.npc_speech != "")
                                if (thisnpc.npc_next_speech_time <= DLL.time())
                                {
                                    thisnpc.SendMoverChatBalloon(thisnpc.npc_speech);
                                    thisnpc.npc_next_speech_time = DLL.time() + thisnpc.npc_speech_delay + rollette.Next(5, 20);
                                }
                        }
                    }
                    for (int i = 0; i < WorldServer.world_monsters.Count; i++)
                    {
                        Monster mob = WorldServer.world_monsters[i];
                        lock (mob.moverLock)
                        {
                            mob.c_spawns = MoversHandler.GetSpawnedClients(mob);
                            if (!mob.c_spawns.Contains(mob.c_target))
                            {
                                mob.c_target = null;
                                mob.c_destiny = mob.absolute_position;
                            }
                            if (!mob.mob_isdead)
                            {
                                mob.UpdatePositionValues();
                                if (mob.bIsTraced)
                                {
                                    if (mob.qwLastTraceTime <= DLL.time())
                                    {
                                        mob.SendEffect(283, mob.c_position);
                                        mob.qwLastTraceTime = DLL.time() + 1;
                                    }
                                }
                                if (mob.c_target != null)
                                {
                                    if (mob.c_attributes[FlyFF.DST_HP] <= 0)
                                    {
                                        mob.ClearTarget();
                                    }
                                }
                                if (!mob.bIsFollowing && !mob.bIsFighting || mob.c_target == null)
                                {
                                    if (mob.next_move_time <= DLL.time())
                                    {
                                        Point n = DiceRoller.RandomPointInCircle(mob.c_position, 12.5f);
                                        mob.c_destiny.x = n.x;
                                        mob.c_destiny.y = n.y;
                                        mob.c_destiny.z = n.z;
                                        mob.next_move_time = DLL.time() + DiceRoller.RandomNumber(5, 15);
                                        mob.SendMoverNewDestination();
                                    }
                                    if (!mob.absolute_position.IsInCircle(mob.c_position, 17f))
                                    {
                                        mob.c_destiny = mob.absolute_position;
                                        mob.next_move_time = DLL.time() + DiceRoller.RandomNumber(5, 15);
                                        mob.SendMoverNewDestination();
                                    }
                                }
                                else
                                {
                                    // Follow target.
                                    if (mob.bIsFollowing && mob.c_target != null)
                                    {
                                        mob.c_destiny = mob.c_target.c_position;
                                        if (mob.next_move_time <= DLL.time() && !mob.c_target.c_position.IsInCircle(mob.c_position, 2.5f))
                                        {
                                            mob.next_move_time = DLL.time() + WorldServer.c_random.Next(1, 3);
                                            mob.SendMoverNewDestination();
                                        }
                                    }
                                    /// [Adidishen]
                                    /// The following was commented out due to mobs running away.
                                    /*Point destPos = new Point(
                                    mob.c_target.c_position.x + 2 * (mob.c_position.x - mob.c_target.c_position.x) / mob.c_position.Distance2D(mob.c_target.c_position),
                                    mob.c_target.c_position.y,
                                    mob.c_target.c_position.z + 2 * (mob.c_position.z - mob.c_target.c_position.z) / mob.c_position.Distance2D(mob.c_target.c_position)
                                    );
                                    if (destPos.Distance2D(mob.c_destiny) > 0.1f)
                                    {
                                        mob.c_destiny = destPos;
                                        mob.sendSetMoverDestiny();
                                    }*/
                                }
                            }
                            else
                            {
                                if (mob.mob_isdead && mob.next_despawn_time <= DLL.time())
                                {
                                    mob.next_despawn_time = long.MaxValue;
                                    for (int l = 0; l < mob.c_spawns.Count; l++)
                                        ((Client)mob.c_spawns[l]).DespawnMonster(mob);
                                }
                                if (mob.next_spawn_time <= DLL.time())
                                {
                                    mob.next_spawn_time = 0;
                                    mob.next_despawn_time = 0;
                                    mob.c_position = mob.absolute_position;
                                    mob.c_attributes[FlyFF.DST_HP] = mob.c_attributes[FlyFF.DST_HP_MAX];
                                    mob.mob_isdead = false;
                                }
                            }
                        }
                    }
                }
                Thread.Sleep(Server.threadDelay_mover + ThreadsIdleSleepAddition);
            }
        }
    }
}
/*
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace FlyffWorld
{
    public partial class WorldThreads
    {
        public void MoverThreadProcess()
        {
            Log.Write(Log.MessageType.pthread, "Mover thread started, delay: {0} seconds", (double)Server.threadDelay_mover / 1000);
            while (true)
            {
                for (int i = 0; i < WorldServer.world_npcs.Count; i++)
                {
                    NPC thisnpc = (NPC)WorldServer.world_npcs[i];
                    lock (thisnpc.moverLock)
                    {
                        thisnpc.spawnedMovers = MoversHandler.getClientsWhoHaveThisMoverSpawned(thisnpc);
                        if (thisnpc.npc_speech != "")
                            if (thisnpc.npc_next_speech_time <= DLL.time())
                            {
                                thisnpc.sendMoverChat(thisnpc.npc_speech);
                                thisnpc.npc_next_speech_time = DLL.time() + thisnpc.npc_speech_delay + rollette.Next(5, 20);
                            }
                    }
                }
                for (int i = 0; i < WorldServer.world_monsters.Count; i++)
                {
                    Monster mob = (Monster)WorldServer.world_monsters[i];
                    lock (mob.moverLock)
                    {
                        mob.spawnedMovers = MoversHandler.getClientsWhoHaveThisMoverSpawned(mob);
                        if (mob.next_move_time <= DLL.time())
                        {
                            if (!mob.IsFighting)
                                mob.destiny = DiceRoller.RadnomPointInCircle(mob.position, 12.5f);
                            mob.next_move_time = DLL.time() + mob.random.Next(5, 15);
                            mob.sendSetMoverDestiny();
                        }
                        if (!mob.absolute_position.IsInCircle(mob.position, 17f))
                        {
                            mob.target = -1;
                            mob.destiny = mob.absolute_position;
                            mob.next_move_time = DLL.time() + mob.random.Next(5, 15);
                            mob.sendSetMoverDestiny();
                        }
                        if (mob.mob_aggressive && !mob.IsFighting)
                        {
                            for (int l = 0; l < mob.spawnedMovers.Count; l++)
                            {
                                Client player = (Client)mob.spawnedMovers[l];
                                if (mob.position.IsInCircle(player.position, 7.5f))
                                {
                                    Console.WriteLine("Distance from player: " + mob.position.Distance2D(player.position));
                                    Console.WriteLine("Enemy spotted!");
                                    mob.target = player.mover_id;
                                    mob.sendMoverChat("Prepare yourself, " + player.userinfo.playername + "! I challenge you to a battle! Hahahaha! Here I come!!! *insert random text here*");
                                    mob.destiny = player.position;
                                    mob.sendSetMoverDestiny();
                                    break;
                                }
                            }
                        }
                        if (mob.IsFighting)
                        {
                            Client player = WorldHelper.GetClientByMoverID(mob.target);
                            if (!mob.position.IsInCircle(player.position, 17f))
                            {
                                Console.WriteLine("Clearing mob target");
                                mob.target = -1;
                                mob.destiny = mob.absolute_position;
                                mob.next_move_time = DLL.time() + mob.random.Next(5, 15);
                                mob.sendSetMoverDestiny();
                            }
                            else
                            {
                                mob.destiny = player.position;
                            }
                        }
                        mob.UpdatePositionValues(40);
                    }
                }
                Thread.Sleep(Server.threadDelay_mover);
            }
        }
    }
}
*/
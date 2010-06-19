using System;
using System.Collections.Generic;

using System.Text;
using System.Threading;

namespace FlyffWorld
{
    public partial class WorldThreads
    {
        public void TimersThreadProcess()
        {
            Log.Write(Log.MessageType.pthread, "Timers thread started, delay: {0} seconds", (double)Server.threadDelay_timers / 1000);
            while (true)
            {
                if (ThreadsCalculationEnabled)
                {
                    for (int i = 0; i < WorldServer.world_players.Count; i++)
                    {
                        Client thisclient = WorldServer.world_players[i];
                        if (!thisclient.playerSpawned)
                            continue;
                        thisclient.UpdatePositionValues();
                        if (thisclient.timers.nextRecovery <= DLL.time())
                            thisclient.OnHealTimer();
                        if (thisclient.timers.nextSave <= DLL.time())
                            thisclient.SaveAll(false);
                        if (thisclient.timers.nextCheer <= DLL.time())
                            thisclient.OnCheerTimer();
                        if ((thisclient.timers.nextBuffCheck <= DLL.time()) && (thisclient.isBuffed == true))//not need to check if layer isn't buffed
                            thisclient.checkBuffTime();
                    }
                    for (int j = 0; j < WorldServer.world_monsters.Count; j++)
                    {
                        Monster thismonster = WorldServer.world_monsters[j];
                        if (thismonster.bIsFighting)
                        {
                            if (thismonster.c_position.Distance2D(thismonster.c_target.c_position) < 2.1f)
                            { // 2u de distance pour attaquer, changer si le mob attaque a distance
                                if (thismonster.timers.nextAttack <= DLL.time())
                                {
                                    thismonster.SendMonsterAttackMotion(thismonster.Data.attacks[DiceRoller.RandomNumber(0, 2)], thismonster.c_target.dwMoverID);
                                    thismonster.attackPlayer();
                                    thismonster.timers.nextAttack = DLL.time() + (thismonster.Data.atkDelay / 1000);
                                }
                            }
                        }
                    }
                }
                Thread.Sleep(Server.threadDelay_timers + ThreadsIdleSleepAddition);
            }
        }

    }
}

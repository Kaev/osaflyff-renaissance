using System;
using System.Collections;
using System.Text;
using System.Net.Sockets;
using System.Threading;

namespace FlyffWorld
{
    public class Lock : Object
    {
        public override string ToString()
        {
            return "Thread lock";
        }
    }
    public partial class WorldThreads
    {
        /// <summary>
        /// Random optimization stuff, only works when idle :|
        /// </summary>
        public int ThreadsIdleSleepAddition
        {
            get
            {
                if (WorldServer.world_players.Count == 0)
                {
                    return 5000;
                }
                return 10;
            }
        }        
        /// <summary>
        /// Random optimization stuff, only works when idle :|
        /// </summary>
        public bool ThreadsCalculationEnabled
        {
            get
            {
                return WorldServer.world_players.Count != 0;
            }
        }
        /// [Adidishen]
        /// VISIBILITY THREADS - PLEASE NOTE!
        /// Do NOT! add any more visibility threads OTHER THAN mobVisibilityThread & visibilityThread. 
        /// mobVisibilityThread: This one handles MOBS ONLY. Only mob visibility stuff go in this one and NOTHING ELSE.
        /// visibilityThread: Everything else that is related to --VISIBILITY-- goes in this one.
        ///                   And this includes, but is not limited to:
        ///                     - Warpzone visibility (although they're not visible, they deal with it in a way)
        ///                     - NPCs visibility
        ///                     - Players visibility
        ///                     - (Since rev 9) drops visibility
        ///                     - Despawning:
        ///                       * Monster despawns
        ///                       * Drop despawns
        ///                       * Player despawns
        ///                       * NPC despawns
        /// worldThread: general world events thread such as weather.
        /// moverThread: anything that is related to mover-specific actions such as npc shout-chats.
        /// timersThread: specific to timers such as next cheer timer, next idle heal timer, next save timer, et cetera.
        /// databasePingThread: NOTHING but ping.
        /// 
        /// I hope it's all clear now, so don't add any other threads because too many threads make it harder to handle!
        public Thread visibilityThread;
        public Thread databasePingThread;
        public Thread timersThread;
        public Thread moverThread;
        public Thread worldThread;
        public Thread mobVisibilityThread;
        public Thread delayedActionThread;
        
        public WorldThreads()
        {
            visibilityThread = new Thread(new ThreadStart(VisibilityThreadProcess));
            databasePingThread = new Thread(new ThreadStart(PingThreadProcess));
            timersThread = new Thread(new ThreadStart(TimersThreadProcess));
            moverThread = new Thread(new ThreadStart(MoverThreadProcess));
            worldThread = new Thread(new ThreadStart(WorldThreadProcess)); 
            mobVisibilityThread = new Thread(new ThreadStart(MonsterVisibilityThreadProcess));
            
            
            visibilityThread.Start();
            databasePingThread.Start();
            timersThread.Start();
            moverThread.Start();
            worldThread.Start();
            mobVisibilityThread.Start();
            
        }
    }
}

using System;
using System.Collections.Generic;

using System.Text;
using System.Threading;

namespace FlyffWorld
{
    public partial class WorldThreads
    {
        public void PingThreadProcess()
        {
            Log.Write(Log.MessageType.pthread, "MySQL ping thread started, delay: {0} seconds", (double)Server.threadDelay_ping / 1000);
            while (true)
            {
                Database.Execute("SELECT \"nothing\"");
                Thread.Sleep(Server.threadDelay_ping + ThreadsIdleSleepAddition);
            }
        }
    }
}

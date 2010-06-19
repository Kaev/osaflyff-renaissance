using System;
using System.Collections.Generic;
using System.Threading;
using System.Text;

namespace FlyffWorld
{
    public partial class WorldThreads
    {
        public bool weatherNone = true;
        public long clearWeatherTimer = 0;
        public long newWeatherTimer = 0;
        public Lock worldLock = new Lock();
        Random rollette
        {
            get
            {
                return WorldServer.c_random;
            }
        }
        public void WorldThreadProcess()
        {
            Log.Write(Log.MessageType.pthread, "World thread started, delay: {0} seconds", (double)Server.threadDelay_world / 1000);
            try
            {
                while (true)
                {
                    if (ThreadsCalculationEnabled)
                    {
                        lock (worldLock)
                        {
                            if (Server.weather != 0x60 && weatherNone)
                            {
                                weatherNone = false;
                                clearWeatherTimer = DLL.time() + rollette.Next(1, 5) * 60;
                            }
                            else if (clearWeatherTimer <= DLL.time() && clearWeatherTimer != 0)
                            {
                                weatherNone = true;
                                WorldServer.SetWeather(Weather.None);
                                clearWeatherTimer = 0;
                                newWeatherTimer = DLL.time() + rollette.Next(5, 31) * 60;
                            }
                            else if (Server.weather == 0x60 && weatherNone && newWeatherTimer != 0 && newWeatherTimer <= DLL.time())
                            {
                                weatherNone = false;
                                WorldServer.SetWeather((Weather)rollette.Next(1, 3));
                                clearWeatherTimer = DLL.time() + rollette.Next(1, 5) * 60;
                                newWeatherTimer = 0;
                            }
                        }
                    }
                    Thread.Sleep(Server.threadDelay_world + ThreadsIdleSleepAddition);
                }
            }
            catch (Exception e)
            {
                Log.Write(Log.MessageType.fatal, "World thread terminated due to an exception.");
                Log.Write(Log.MessageType.fatal, "Error message: " + e.Message);
                Log.Write(Log.MessageType.fatal, "Stack trace: " + e.StackTrace);
                Log.Write(Log.MessageType.notice, "Restarting world thread.");
                worldThread = new Thread(new ThreadStart(WorldThreadProcess));
                worldThread.Start();
            }
        }
    }
}
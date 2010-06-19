using System;
using System.Collections.Generic;
using System.Threading;
using System.Text;

namespace FlyffLogin
{
    public class Prog
    {
        public static long peak_memory_usage = 0; // If current memory usage above this, it will update
        public static Int32 seconds_till_next_log = 3600000; // One hour
        static string getSize(long size)
        {
            string[] types = new string[7] { "B", "KB", "MB", "GB", "TB", "PB", "EB" };
            double dwSize = size;
            int type = 0;
            while (dwSize >= 1024)
            {
                dwSize /= 1024;
                type++;
            }
            return String.Format("{0:0.##} {1}", dwSize, types[type]).Replace(",", ".");
        }
        static void updateMemoryUsage()
        {
            while (true)
            {
                seconds_till_next_log--;
                System.Diagnostics.Process p = System.Diagnostics.Process.GetCurrentProcess();
                long curMemoryUsage = p.WorkingSet64;
                if (curMemoryUsage > peak_memory_usage)
                    peak_memory_usage = curMemoryUsage;
                Console.Title = String.Format("osAflyff (Login server) - Memory usage: {0} - Running threads: {1}", getSize(curMemoryUsage), p.Threads.Count);
                if (seconds_till_next_log == 0)
                {
                    seconds_till_next_log = 3600000; // another hour
                    Log.Write(Log.MessageType.info, "Current memory usage: {0} (Peak: {1})",
                        getSize(curMemoryUsage),
                        getSize(peak_memory_usage));
                }
                Thread.Sleep(1000);
            }
        }
        [MTAThread]
        static void Main(string[] args)
        {
            DotNetVerifier.Verifier.Verify(DotNetVerifier.Verifier.DOT_NET_MINIMAL_RUNTIME_VERSION, false, "3.5");
            Thread updateMemoryUsageThread = new Thread(new ThreadStart(updateMemoryUsage));
            updateMemoryUsageThread.Start();
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write(("Welcome to osAflyff emulator! current revision: " + DotNetVerifier.Verifier.OSAFLYFF).PadRight(Console.BufferWidth));
            Console.ResetColor();
            Server.Refresh(); // loads configurations
            Database.Connect(
                Server.myodbc_username,
                Server.myodbc_password,
                Server.myodbc_database,
                Server.myodbc_driver,
                Server.myodbc_host);
            new Thread(new ThreadStart(ISCRemoteServer.Connect)).Start();
            SocketServer.Start();
        }
        
    }
}

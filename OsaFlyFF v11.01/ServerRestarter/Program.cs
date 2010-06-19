//#define DEVELOPERMODE
using System;
using System.Threading;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
namespace ServerRestarter
{
    class Program
    {
        static void Main(string[] args)
        {
            DotNetVerifier.Verifier.Verify(DotNetVerifier.Verifier.DOT_NET_MINIMAL_RUNTIME_VERSION, false, "3.5");
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write(("Welcome to osAflyff emulator! current revision: " + DotNetVerifier.Verifier.OSAFLYFF).PadRight(Console.BufferWidth));
            Console.ResetColor();
            Console.Title = "osAflyff - Server restarter";
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Running StartProcessThread thread");
            new Thread(new ThreadStart(StartProcessThread)).Start();
        }
        static void StartProcessThread()
        {
            while (true)
            {
                Thread.Sleep(100);
                if (Process.GetProcessesByName("FlyffISCProgram").Length < 1)
                {
                    Console.WriteLine("ISC server is down, restarting..");
#if DEVELOPERMODE
                    File.Delete("FlyffISCProgram.exe");
                    File.Copy(Environment.CurrentDirectory + @"\..\ISC\obj\Release\FlyffISCProgram.exe", "FlyffISCProgram.exe");
#endif
                    StartProcess(Environment.CurrentDirectory + "\\FlyffISCProgram.exe");
                    continue;
                }
                if (Process.GetProcessesByName("FlyffLoginServer").Length < 1)
                {
                    Console.WriteLine("Login server is down, restarting..");
#if DEVELOPERMODE
                    File.Delete("FlyffLoginServer.exe");
                    File.Copy(Environment.CurrentDirectory + @"\..\FlyffLogin\obj\Release\FlyffLoginServer.exe", "FlyffLoginServer.exe");
#endif
                    StartProcess(Environment.CurrentDirectory + "\\FlyffLoginServer.exe");
                    continue;
                }
                if (Process.GetProcessesByName("FlyffClusterServer").Length < 1)
                {
                    Console.WriteLine("Cluster server is down, restarting..");
#if DEVELOPERMODE
                    File.Delete("FlyffClusterServer.exe");
                    File.Copy(Environment.CurrentDirectory + @"\..\FlyffCluster\obj\Release\FlyffClusterServer.exe", "FlyffClusterServer.exe");
#endif
                    StartProcess(Environment.CurrentDirectory + "\\FlyffClusterServer.exe");
                    continue;
                }
                if (Process.GetProcessesByName("FlyffWorldServer").Length < 1)
                {
                    Console.WriteLine("World server is down, restarting..");
#if DEVELOPERMODE
                    File.Delete("FlyffWorldServer.exe");
                    File.Copy(Environment.CurrentDirectory + @"\..\FlyffWorld\obj\Release\FlyffWorldServer.exe", "FlyffWorldServer.exe");
#endif
                    StartProcess(Environment.CurrentDirectory + "\\FlyffWorldServer.exe");
                    continue;
                }
            }
        }
        static void StartProcess(string path)
        {
            Process.Start(path);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;
namespace FlyffWorld
{

    public class Log
    {
        public enum MessageType
        {
            info,
            warning,
            hack,
            fatal,
            error,
            notice,
            debug,
            packet,
            pthread,
            gmcmd,
            load
        };
        public const int // SW types
                            STREAM_MAIN = 0,
                            STREAM_DEBUG = 1,
                            STREAM_ERROR = 2,
                            STREAM_COMMANDS = 3;
        public const int // debug levels 
                            LOGLEVEL_NONE = 0,
                            LOGLEVEL_WARNING = 1,
                            LOGLEVEL_HACK = 2,
                            LOGLEVEL_FATAL = 4,
                            LOGLEVEL_ERROR = 8,
                            LOGLEVEL_NOTICE = 16,
                            LOGLEVEL_DEBUG = 32,
                            LOGLEVEL_PACKET = 64,
                            LOGLEVEL_PTHREAD = 128,
                            LOGLEVEL_GMCOMMAND = 256,
                            LOGLEVEL_LOAD = 512,
                            LOGLEVEL_INFO = 1024,
                            LOGLEVEL_ALL = 2047;
        /*
        private static StreamWriter sw_packet = new StreamWriter("Packets.log", true);
        private static StreamWriter sw_all = new StreamWriter("Console.log", true);
         * */
        private static StreamWriter[] SW = new StreamWriter[4];
        private static bool bStreamsInitialized = false;
        public static int DebugLevel = LOGLEVEL_ALL;
        private static void Color(ConsoleColor Foreground)
        {
            Console.ForegroundColor = Foreground;
        }
        private static void Reset()
        {
            Console.ForegroundColor = ConsoleColor.White;
        }
        private static void InitializeStreams()
        {
            try
            {
                SW[STREAM_COMMANDS] = new StreamWriter("wslog/commands.log", true);
                SW[STREAM_DEBUG] = new StreamWriter("wslog/debug.log", true);
                SW[STREAM_ERROR] = new StreamWriter("wslog/error.log", true);
                SW[STREAM_MAIN] = new StreamWriter("wslog/main.log", true);
                for (int i = 0; i < SW.Length; i++)
                {
                    SW[i].WriteLine(Environment.NewLine + "================================================================" + Environment.NewLine + "NEW SESSION STARTED AT " + DateTime.Now.ToString() + Environment.NewLine + "================================================================" + Environment.NewLine);
                }
                bStreamsInitialized = true;
            }
            catch (Exception e)
            {
                Console.WriteLine("One or more streams could not be initialized!" + e.Message);
                Environment.Exit(12346);
            }
        }
        private static Lock syncroot = new Lock();
        public static void Write(MessageType type, string format, params object[] args)
        {
            if (!bStreamsInitialized)
                InitializeStreams();
            string FormattedMessage = String.Format(format, args);
            lock (syncroot)
            {
                switch (type)
                {
                    case MessageType.load:
                        if ((DebugLevel & LOGLEVEL_LOAD) != LOGLEVEL_LOAD)
                            return;
                        Color(ConsoleColor.DarkMagenta);
                        Console.Write("[LOADING] ");
                        break;
                    case MessageType.info:
                        if ((DebugLevel & LOGLEVEL_INFO) != LOGLEVEL_INFO)
                            return;
                        Color(ConsoleColor.Green);
                        Console.Write("[INFO] ");
                        break;
                    case MessageType.hack:
                        if ((DebugLevel & LOGLEVEL_HACK) != LOGLEVEL_HACK)
                            return;
                        Color(ConsoleColor.Red);
                        Console.Write("[HACK] ");
                        break;
                    case MessageType.fatal:
                        if ((DebugLevel & LOGLEVEL_FATAL) != LOGLEVEL_FATAL)
                            return;
                        Color(ConsoleColor.Red);
                        Console.Write("[FATAL] ");
                        break;
                    case MessageType.error:
                        if ((DebugLevel & LOGLEVEL_ERROR) != LOGLEVEL_ERROR)
                            return;
                        Color(ConsoleColor.DarkRed);
                        Console.Write("[ERROR] ");
                        break;
                    case MessageType.notice:
                        if ((DebugLevel & LOGLEVEL_NOTICE) != LOGLEVEL_NOTICE)
                            return;
                        Color(ConsoleColor.Cyan);
                        Console.Write("[NOTICE] ");
                        break;
                    case MessageType.debug:
                        if ((DebugLevel & LOGLEVEL_DEBUG) != LOGLEVEL_DEBUG)
                            return;
                        Color(ConsoleColor.Blue);
                        Console.Write("[DEBUG] ");
                        break;
                    case MessageType.warning:
                        if ((DebugLevel & LOGLEVEL_WARNING) != LOGLEVEL_WARNING)
                            return;
                        Color(ConsoleColor.Yellow);
                        Console.Write("[WARNING] ");
                        break;
                    case MessageType.pthread:
                        if ((DebugLevel & LOGLEVEL_PTHREAD) != LOGLEVEL_PTHREAD)
                            return;
                        Color(ConsoleColor.Gray);
                        Console.Write("[THREADS] ");
                        break;
                    case MessageType.gmcmd:
                        if ((DebugLevel & LOGLEVEL_GMCOMMAND) != LOGLEVEL_GMCOMMAND)
                            return;
                        Color(ConsoleColor.Magenta);
                        Console.Write("[GM COMMAND] ");
                        break;
                }
                Reset();
                if (type != MessageType.packet)
                    Console.WriteLine(FormattedMessage);
            }
            StreamWriter sw;
            switch (type)
            {
                case MessageType.error:
                case MessageType.fatal:
                case MessageType.hack:
                case MessageType.warning:
                    sw = SW[STREAM_ERROR];
                    break;

                case MessageType.pthread:
                case MessageType.notice:
                case MessageType.load:
                case MessageType.info:
                    sw = SW[STREAM_MAIN];
                    break;

                case MessageType.debug:
                    sw = SW[STREAM_DEBUG];
                    break;

                case MessageType.gmcmd:
                    sw = SW[STREAM_COMMANDS];
                    break;

                default:
                    return;
            }
            sw.WriteLine("[" + DateTime.Now.ToString() + "] " + FormattedMessage);
            sw.Flush();
        }
    }
}
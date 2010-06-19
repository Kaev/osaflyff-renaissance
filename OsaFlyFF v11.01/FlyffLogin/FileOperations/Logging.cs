using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;
namespace FlyffLogin
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
            packet
        };
        private static void Color(ConsoleColor Foreground)
        {
            Console.ForegroundColor = Foreground;
        }
        private static void Reset()
        {
            Console.ForegroundColor = ConsoleColor.White;
        }
        private static object syncroot = new object();
        /// <summary>
        /// Writes some text to the console and logs it to a file. Note: when using packet MT, Format parameter is ignored, and you should provide 3 extra arguments: The header, the size and the packet (as a string).
        /// </summary>
        /// <param name="Type">Based on MessageType enum. Can be one of these: info, warning, hack, fatal, error, notice, debug.</param>
        /// <param name="Format">The string to format and write/log.</param>
        /// <param name="Arguments">The arguments to add to the formatted string.</param>
        public static void Write(MessageType Type, string Format, params object[] Arguments)
        {
            string FormattedMessage = String.Format(Format, Arguments);
            lock (syncroot)
            {
                switch (Type)
                {
                    case MessageType.info:
                        Color(ConsoleColor.Green);
                        Console.Write("[INFO] ");
                        break;
                    case MessageType.hack:
                        Color(ConsoleColor.Red);
                        Console.Write("[HACK] ");
                        break;
                    case MessageType.fatal:
                        Color(ConsoleColor.Red);
                        Console.Write("[FATAL] ");
                        break;
                    case MessageType.error:
                        Color(ConsoleColor.DarkRed);
                        Console.Write("[ERROR] ");
                        break;
                    case MessageType.notice:
                        Color(ConsoleColor.Cyan);
                        Console.Write("[NOTICE] ");
                        break;
                    case MessageType.debug:
                        Color(ConsoleColor.Blue);
                        Console.Write("[DEBUG] ");
                        break;
                    case MessageType.warning:
                        Color(ConsoleColor.Yellow);
                        Console.Write("[WARNING] ");
                        break;
                }
                Reset();
                if (Type != MessageType.packet)
                    Console.WriteLine(FormattedMessage);
            }
            StreamWriter sw;
            try
            {
                if (Type == MessageType.packet)
                    sw = new StreamWriter("Packets.log", true);
                else
                    sw = new StreamWriter("Console.log", true);
            }
            catch (Exception)
            {
                return;
            }
            string Timestamp = "[" + DateTime.Now.Hour.ToString().PadLeft(2, Convert.ToChar("0")) + ":" + DateTime.Now.Minute.ToString().PadLeft(2, Convert.ToChar("0")) + "]";
            if (Type != MessageType.packet)
                sw.WriteLine(Timestamp + ": " + FormattedMessage);
            else
                sw.WriteLine(String.Format(Timestamp + ": Received packet; header: {0}. Size: {1}." + sw.NewLine + "{2}", Arguments));
            sw.Close();
        }
    }
}

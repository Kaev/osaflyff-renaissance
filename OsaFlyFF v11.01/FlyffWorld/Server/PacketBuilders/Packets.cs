using System;
using System.Collections.Generic;
using System.Text;

namespace FlyffWorld
{
    /// <summary>
    /// Global packets class with functions that can help when using packets.
    /// </summary>
    class Packets
    {
        /// <summary>
        /// Makes a byte array from the given integer.
        /// </summary>
        /// <param name="i">The integer to convert into a byte array.</param>
        /// <returns>The byte array after reversing the integer.</returns>
        public static byte[] MakeInt(int i)
        {
            string temp = i.ToString("x").PadLeft(8,Convert.ToChar("0")).ToUpper();
            byte[] newBytes ={
                Convert.ToByte(temp.Substring(6,2),16),
                Convert.ToByte(temp.Substring(4,2),16),
                Convert.ToByte(temp.Substring(2,2),16),
                Convert.ToByte(temp.Substring(0,2),16)
            };
            return newBytes;
        }
        public static byte[] MakeUnsignedInt(uint ui)
        {
            string temp = ui.ToString("x").PadLeft(8, '0').ToUpper();
            byte[] newBytes ={
                Convert.ToByte(temp.Substring(6,2),16),
                Convert.ToByte(temp.Substring(4,2),16),
                Convert.ToByte(temp.Substring(2,2),16),
                Convert.ToByte(temp.Substring(0,2),16)
            };
            return newBytes;
        }
        public static byte[] MakeLong(long ul)
        {
            string temp = ul.ToString("x").PadLeft(16, '0').ToUpper();
            byte[] newBytes ={
                Convert.ToByte(temp.Substring(14,2),16),
                Convert.ToByte(temp.Substring(12,2),16),
                Convert.ToByte(temp.Substring(10,2),16),
                Convert.ToByte(temp.Substring(8,2),16),
                Convert.ToByte(temp.Substring(6,2),16),
                Convert.ToByte(temp.Substring(4,2),16),
                Convert.ToByte(temp.Substring(2,2),16),
                Convert.ToByte(temp.Substring(0,2),16)
            };
            return newBytes;
        }
        /// <summary>
        /// Makes a byte array from the given short.
        /// </summary>
        /// <param name="shorty">The short to convert into a byte array.</param>
        /// <returns>The byte array after reversing the short.</returns>
        public static byte[] MakeShort(short shorty)
        {
            string temp = shorty.ToString("x").PadLeft(4, '0').ToUpper();
            byte[] newBytes =
            {
                Convert.ToByte(temp.Substring(2,2),16),
                Convert.ToByte(temp.Substring(0,2),16)
            };
            return newBytes;
        }
        /// <summary>
        /// Makes an integer of the byte array supplied, reverses it and returns.
        /// </summary>
        /// <param name="byteArray">The byte array to convert.</param>
        /// <returns>A reversed integer made from the byte array.</returns>
        public static int ParseInt(byte[] byteArray)
        {
            string hex = 
                byteArray[3].ToString("x").PadLeft(2,Convert.ToChar("0")) + "" + 
                byteArray[2].ToString("x").PadLeft(2,Convert.ToChar("0")) + "" + 
                byteArray[1].ToString("x").PadLeft(2,Convert.ToChar("0")) + "" + 
                byteArray[0].ToString("x").PadLeft(2,Convert.ToChar("0"));
            return Convert.ToInt32(hex, 16);
        }
        /// <summary>
        /// Converts the buffer byte array into a readable hex string which shows the ASCII values of each byte. (^ would turn into 5E)
        /// </summary>
        /// <param name="buffer">The byte array to convert.</param>
        /// <param name="size">The amount of bytes to convert.</param>
        /// <returns>A readable hex string which shows the ASCII values of each byte.</returns>
        public static string MakePString(byte[] buffer,int size)
        {
            string str = "";
            for (int i = 0; i < size; i++)
                str = str + "" + buffer[i].ToString("x").ToUpper().PadLeft(2,Convert.ToChar("0")) + " ";
            return str;
        }
        public static string MakePString(string buf)
        {
            string str = "";
            for (int i = 0; i < buf.Length; i++)
            {
                str += ((byte)buf[i]).ToString("X2") + " ";
            }
            return str;
        }
        /// <summary>
        /// Converts a byte array into a string, which could be used with Packet.Addhex void.
        /// </summary>
        /// <param name="buffer">The byte array to convert.</param>
        /// <param name="size">The amount of bytes to convert.</param>
        /// <param name="noSpacesAlwaysTrue">Specifying this parameter (either true or false) will trigger the second function which is compatible with Packet.Addhex void. Not specifying this parameter will trigger the first MakePString instead.</param>
        /// <returns>A readable hex string which shows the ASCII values of each byte with no spaces between each byte.</returns>
        public static string MakePString(byte[] buffer, int size, bool noSpacesAlwaysTrue)
        {
            string str = "";
            for (int i = 0; i < size; i++)
                str = str + "" + buffer[i].ToString("x").ToUpper().PadLeft(2, Convert.ToChar("0"));
            return str;
        }
    }
}

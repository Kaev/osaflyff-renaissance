using System;
using System.Collections.Generic;
using System.Text;

namespace FlyffWorld
{
    public struct AwakeAttribute
    {
        public byte attribute;
        public int value;
        public bool negative; // True: attrib[attribute]-=value; else: +=value
    }
    public class Awakening
    {
        private static byte[] awakeattr = new byte[14] { 1, 2, 3, 4, 9, 11, 24, 35, 36, 37, 72, 75, 77, 83 };
        private static int[] maxvalue = new int[14] { 28, 28, 28, 28, 17, 17, 17, 450, 450, 450, 100, 17, 17, 125 };
        public static AwakeAttribute[] ToAwakeAttributeArray(long awakening)
        {
            AwakeAttribute[] aa = new AwakeAttribute[3];
            for (int i = 0; i < 3; i++)
            {
                aa[i].attribute = 0;
                aa[i].value = 0;
                aa[i].negative = false;
            }
            return aa;
        }
        public static long Generate()
        {
            int[,] attributes = new int[3, 3];
            for (int i = 0; i < 3; i++)
            {
                attributes[i, 0] = 0;
                attributes[i, 1] = 0;
                attributes[i, 2] = 0;
            }

            int awakeningCount = WorldServer.c_random.Next(1, 3);

            List<AwakeAttribute> awakes = new List<AwakeAttribute>();
            for (int i = awakeningCount; i >= 0; i--)
            {
                AwakeAttribute aa = new AwakeAttribute();
                int attribIndex;
                int value;
                bool negative;
                attribIndex = WorldServer.c_random.Next(0, 14);
                negative = ((WorldServer.c_random.Next(1, 101) <= 50) ? true : false);
                value = WorldServer.c_random.Next(1, maxvalue[attribIndex] + 1);
                value = ((value <= 0) ? 1 : value);
                Log.Write(Log.MessageType.debug, "Attribute: {0}{1}{2}",
                    awakeattr[attribIndex],
                    (negative ? "-" : "+"),
                    value);
                aa.attribute = awakeattr[attribIndex];
                aa.negative = negative;
                aa.value = value;
                attributes[i, 0] = aa.attribute;
                attributes[i, 1] = (byte)(aa.negative ? 1 : 0);
                attributes[i, 2] = aa.value;
                awakes.Add(aa);
            }
            return ToInt64(awakes);
        }
        // Credit to Rhisis project for this way of compiling...
        public static long ToInt64(List<AwakeAttribute> a)
        {
            long b = 0;
            for (int i = 0; i < a.Count; i++)
            {
                b <<= 7;
                b |= a[i].attribute;
                b <<= 1;
                b |= a[i].negative ? 1 : 0;
                b <<= 9;
                b |= a[i].value;
                if (i != a.Count - 1)
                {
                    b <<= 1;
                    b |= 0;
                }
            }
            b <<= 8;
            return b;
        }
        public static void GetAttributeTypes(byte[,] attributes, int index, ref string attr1, ref string attr2)
        {
            switch (attributes[index, 0])
            {
                case 1: attr1 = "STR"; break;
                case 2: attr1 = "DEX"; break;
                case 3: attr1 = "INT"; break;
                case 4: attr1 = "STA"; break;
                case 9: attr1 = "Crit Rate:"; break;
                case 11: attr1 = "Speed"; break;
                case 24: attr1 = "Attack Speed:"; break;
                case 35: attr1 = "Max HP"; break;
                case 36: attr1 = "Max MP"; break;
                case 37: attr1 = "Max FP"; break;
                case 72: attr1 = "Defense Rate"; break;
                case 75: attr1 = "Motion Decrease"; break;
                case 77: attr1 = "Add damage of Critical Hit"; break;
                case 83: attr1 = "Attack Power"; break;
                default: attr1 = "Unknown"; break;
            }
            switch (attributes[index, 1])
            {
                case 0: attr2 = "+"; break;
                case 1: attr2 = "-"; break;
                default: attr2 = "?"; break;
            }
        }
        /* TODO: Add extraction method to get attributes and values from an awakening */
    }
}
/*
Attributes:
 * 1: STR
 * 2: DEX
 * 3: INT
 * 4: STA
 * 9: Critical Rate
 * 11: Speed
 * 24: Attack Speed
 * 35: Max HP
 * 36: Max MP
 * 37: Max FP
 * 72: Defense Rate
 * 75: Spell Rate (Motion Decrease)
 * 77: Add Damage of Critical Hit
 * 83: Attack Power
*/
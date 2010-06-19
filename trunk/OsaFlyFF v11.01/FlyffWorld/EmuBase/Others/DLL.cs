using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace FlyffWorld
{
    public class DLL
    {
        [DllImport("vcfuncs.dll", EntryPoint = "c_clock")]
        public static extern long clock();
        [DllImport("vcfuncs.dll", EntryPoint = "c_rand")]
        public static extern int rand();
        public const int RAND_MAX = 0x7FFF;
        public static long time(DateTime time)
        {
            return (long)(time - new DateTime(1970, 1, 1)).TotalSeconds - 7200;
        }
        public static long time()
        {
            return time(DateTime.Now);
        }
    }
}

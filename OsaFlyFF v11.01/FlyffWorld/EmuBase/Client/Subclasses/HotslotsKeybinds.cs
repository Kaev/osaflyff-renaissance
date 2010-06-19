using System;
using System.Collections.Generic;

using System.Text;

namespace FlyffWorld
{
    public class Keybind
    {
        public int dwPageIndex = 0; // 1 - 8
        public int dwKeyIndex = 0;  // F1-F9
        public string strText = "";
        public int dwID = 0;
        public int dwOperation = 0;
    }
    public class Hotslot
    {
        public int dwSlot = 0;
        public int dwOperation = 0;
        public string strText = "";
        public int dwID = 0;
    }
}

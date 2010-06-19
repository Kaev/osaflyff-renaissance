using System;
using System.Collections.Generic;

using System.Text;

namespace FlyffWorld
{
    public class Bank
    {
        public int[] dwPenyaArr = new int[3];
        public int[] dwCharacterIDArr = new int[] { 0, 0, 0 };
        public List<Item>[] bankItems = new List<Item>[3];
        public Bank()
        {
            for (int i = 0; i < bankItems.Length; i++)
                bankItems[i] = new List<Item>();
        }
        public string strPassword = "";
    }
}

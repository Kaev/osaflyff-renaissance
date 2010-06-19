using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace FlyffWorld
{
    public class Bag
    {
        public Bag(bool enabled, bool isbase) { isEnabled = enabled; slots = isbase ? 5 : 24; }
        public Bag(bool enabled, bool isbase, DateTime tillwhen) { isEnabled = enabled; slots = isbase ? 5 : 24; dtLastUntil = tillwhen; }
        public List<Item> items = new List<Item>();
        
        public DateTime dtLastUntil = DateTime.Now;
        public bool isEnabled = false;
        public int slots = 0;
        
    }
}

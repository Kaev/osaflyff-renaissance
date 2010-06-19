using System;
using System.Collections;
using System.Text;

namespace FlyffWorld
{
    public class ItemData
    {
        public int itemID = 0;
        public string itemName = "";
        public int stackMax = 1;
        public int[] itemkind = new int[] { 0, 0, 0 };
        public int reqJob = 0;
        public int reqGender = 0;
        public int reqLevel = 0;
        public int npcPrice = 0;
        public bool twoHanded = false;
        public int[] equipSlot = new int[] { 0, 0 };
        public bool canShop = false;
        public int[] destAttributes = new int[] { 0, 0, 0 };
        public int[] adjAttributes = new int[] { 0, 0, 0 };
        public int[] chgAttributes = new int[] { 0, 0, 0 };
        public int min_ability = 0, max_ability = 0;
        public long endurance = 0;
        public int weaponType,
                   itemAtkOrder1,
                   itemAtkOrder2,
                   itemAtkOrder3,
                   skillTime = 0,
                   circleTime = 0;
        public string textFile = "";
    }
}

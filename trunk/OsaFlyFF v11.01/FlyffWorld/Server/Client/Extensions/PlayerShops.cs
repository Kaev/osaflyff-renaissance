using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace FlyffWorld
{
    public class ShopItemMain
    {
        public int id = 0;
        public int quantity = 0;
        public int pos = 0;
    }
    public class NPCShopItem : ShopItemMain
    {
        public int tab = 0;
    }
    public partial class PlayerData
    {
    }
}
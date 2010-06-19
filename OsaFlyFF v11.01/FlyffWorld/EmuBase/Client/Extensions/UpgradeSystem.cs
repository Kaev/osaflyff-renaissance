using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace FlyffWorld
{
    public partial class Client
    {
        /// <summary>
        /// The upgrade system - refining methods and such (mostly item on item handler)
        /// </summary>
        /// <param name="item">The item that will be upgraded</param>
        /// <param name="material">The material used on the item</param>
         public bool UpgradeSystem(Slot dst, Slot src)
        {
            switch (src.c_item.Data.itemkind[2])
            {
                case IK3_ENCHANT: // Sunstone
                    {
                        bool val = UpgradeSystemRefineItem(dst, src);
                        return val;
                    }
                case IK3_ELECARD: // Element card
                    {
                        bool val = UpgradeSystemRefineItemElement(dst, src);
                        return val;
                    }
                case IK3_PIERDICE: // Moonstone
                    {
                        bool val = false;
                        if (dst.c_item.Data.itemkind[2] == IK3_COLLECTER)
                            val = UpgradeSystemRefineCollecter(dst, src);
                        else
                            val = UpgradeSystemRefineJewelry(dst, src);
                        return val;
                    }
                default: // Unknown upgrade material
                    {
                        string msg = String.Format("Not recognized: Materialkind: {0}, MaterialID: {1}, ItemID: {2}", src.c_item.Data.itemkind[2], src.c_item.dwItemID, dst.c_item.dwItemID);
                        SendMessageHud(String.Format("#c #00ff00#b[Warning] {0}#nc#nb", msg));
                        return false;
                    }
            }
        }
        public bool GetHasScrollactivated(int scrollid)
        {
            for (int i = 0; i < c_data.activateditem.Count; i++)
            {
                ActiveItems activateitem = c_data.activateditem[i];
                if (activateitem == null)
                    continue;
                if (activateitem.itemid == scrollid)
                    return true;
            }
            return false;
        }
        private bool UpgradeSystemRefineItemElement(Slot dst, Slot src)
        {

            if (dst.c_item.Data.itemkind[0] != IK1_ARMOR && dst.c_item.Data.itemkind[0] != IK1_WEAPON)
                return false;
            if (dst.c_item.Data.itemkind[1] != IK2_ARMOR && dst.c_item.Data.itemkind[1] != IK2_WEAPON_DIRECT && dst.c_item.Data.itemkind[1] != IK2_WEAPON_MAGIC)
                return false;
            if (!SuitableCardMaterial(dst.c_item.dwElement, dst.c_item.dwEleRefine, src.c_item.dwItemID))
            {
                SendMessageInfo(TID_UPGRADE_ERROR_WRONGSUPITEM);
                return false;
            }
            try
            {
                CSVParser csv = new CSVParser("db\\rates\\element.csv");
                int chance = csv.GetIntList(0)[dst.c_item.dwEleRefine];
                if ((dst.c_item.dwEleRefine < 8) && GetHasScrollactivated(10468) || (dst.c_item.dwEleRefine < 8) && GetHasScrollactivated(26203)) //we have +10%
                {
                   
                    ActiveItems activeitem = GetPlayerActivatedItem(10468);
                    if (activeitem == null)
                        activeitem = GetPlayerActivatedItem(26203);
                    if (activeitem.itemid == 10468)
                        chance += 10;
                    else if (activeitem.itemid == 26203)
                        chance += 100;
                    c_data.activateditem.Remove(activeitem);
                    
                    //send packet to remove scroll icone
                    SendPlayerRemoveBuffByItem(activeitem.itemid);
                }
                int newRefine = RefineBase(src, dst, ITEM_MODTYPE_EREFINE, dst.c_item.dwEleRefine, chance, 3, 10);
                if (newRefine != -1)
                {
                    dst.c_item.dwEleRefine = newRefine;
                    int dwElement = GetElementByCardID(src.c_item.dwItemID);
                    if (dst.c_item.dwElement != dwElement)
                    {
                        dst.c_item.dwElement = dwElement;
                        SendItemUpdate(new ItemUpdateStatus(dst.dwID, ITEM_MODTYPE_ELEMENT, dwElement,0));
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        private bool UpgradeSystemRefineJewelry(Slot dst, Slot src)
        {
            if (dst.c_item.Data.itemkind[1] != IK2_JEWELRY)
                return false;
            try
            {
                CSVParser csv = new CSVParser("db\\rates\\jewelry.csv");
                int chance = csv.GetIntList(0)[dst.c_item.dwRefine];
                if ((dst.c_item.dwRefine < 8) && GetHasScrollactivated(10468) || (dst.c_item.dwRefine < 8) && GetHasScrollactivated(26203)) //we have +10%
                {
                    ActiveItems activeitem = GetPlayerActivatedItem(10468);
                    if (activeitem == null)
                        activeitem = GetPlayerActivatedItem(26203);
                    if (activeitem.itemid == 10468)
                        chance += 10;
                    else if (activeitem.itemid == 26203)
                        chance += 100;
                    c_data.activateditem.Remove(activeitem);
                    
                    //send packet to remove scroll icone
                    SendPlayerRemoveBuffByItem(activeitem.itemid);
                }
                int newRefine = RefineBase(src, dst, ITEM_MODTYPE_REFINE, dst.c_item.dwRefine, chance, 3, 20);
                if (newRefine != -1)
                    dst.c_item.dwRefine = newRefine;
                return true;
            }
            catch
            {
                return false;
            }
        }
        private bool UpgradeSystemRefineCollecter(Slot dst, Slot src)
        {
            if (dst.c_item.Data.itemID != 26452)
                return false;
            if (dst.c_item.dwRefine >= 5)
                    return false;
            try
            {
                CSVParser csv = new CSVParser("db\\rates\\upgrade_collector.csv");
                int chance = csv.GetIntList(0)[dst.c_item.dwRefine];                
                int newRefine = RefineBase(src, dst, ITEM_MODTYPE_REFINE, dst.c_item.dwRefine, chance, 3, 20);
                if (newRefine != -1)
                    dst.c_item.dwRefine = newRefine;
                return true;
            }
            catch
            {
                return false;
            }
        }
        private bool UpgradeSystemRefineItem(Slot dst, Slot src)
        {
            if (dst.c_item.Data.itemkind[0] != IK1_ARMOR && dst.c_item.Data.itemkind[0] != IK1_WEAPON)
                return false;
            try
            {
                int chance = 0;
                if (!IsUltimateWeapon(dst.c_item.dwItemID))
                {
                    CSVParser csv = new CSVParser("db\\rates\\upgrade.csv");
                    chance = csv.GetIntList(0)[dst.c_item.dwRefine];
                    if ((dst.c_item.dwRefine < 8) && GetHasScrollactivated(10468) || (dst.c_item.dwRefine < 8) && GetHasScrollactivated(26203)) //we have +10%
                    {
                        ActiveItems activeitem = GetPlayerActivatedItem(10468);
                        if (activeitem == null)
                            activeitem = GetPlayerActivatedItem(26203);
                        if (activeitem.itemid == 10468)
                            chance += 10;
                        else if (activeitem.itemid == 26203)
                            chance += 100;
                        c_data.activateditem.Remove(activeitem);
                        
                        //send packet to remove scroll icone
                        SendPlayerRemoveBuffByItem(activeitem.itemid);
                    }
                }
                else
                {
                    CSVParser csv = new CSVParser("db\\rates\\upgrade_ultimate.csv");
                    chance = csv.GetIntList(0)[dst.c_item.dwRefine];
                    if ((dst.c_item.dwRefine < 8) && GetHasScrollactivated(10488)) //scroll of Xprotec we have +10%
                    {
                        ActiveItems activeitem = GetPlayerActivatedItem(10488);
                        if (activeitem == null)
                            activeitem = new ActiveItems();
                        chance += 10;                        
                        c_data.activateditem.Remove(activeitem);
                       
                        //send packet to remove scroll icone
                        SendPlayerRemoveBuffByItem(activeitem.itemid);
                    }
                }
                int newRefine = RefineBase(src, dst, ITEM_MODTYPE_REFINE, dst.c_item.dwRefine, chance, 3, 10);
                if (newRefine != -1)
                    dst.c_item.dwRefine = newRefine;
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Returns the new refine level of the item after doing some refine stuff on it.
        /// </summary>
        private int RefineBase(Slot src, Slot dst, int mod, int value, int chance, int destroyMinLevel, int maxLevel)
        {
            if (value >= maxLevel)
            {
                SendMessageInfo(TID_UPGRADE_MAXOVER);
                return value;
            }
            if (DiceRoller.Roll(chance))
            {
                value++;
                SendMessageInfo(TID_UPGRADE_SUCCEEFUL);
                SendEffect(INT_SUCCESS, c_position);
                SendPlayerSound(SND_INF_UPGRADESUCCESS);
                bool suppressscroll = HasCorrectScroll(src);
                SendItemUpdate(new ItemUpdateStatus(dst.dwID, mod, value,0));
            }
            else
            {
                //check if we have correct scroll
                
                SendMessageInfo(TID_UPGRADE_FAIL);
                SendEffect(INT_FAIL, c_position);
                SendPlayerSound(SND_INF_UPGRADEFAIL);
                if (value >= destroyMinLevel && !HasCorrectScroll(src) && DiceRoller.Roll(value * 4))
                {
                    DeleteItem(dst);
                    return -1;
                }
                
            }
            
            DecreaseQuantity(src);
            return value;
        }
        public bool HasCorrectScroll (Slot src)
        {
            bool result = false;
            ActiveItems activeitem;
            

            //determine scroll type :
            switch (src.c_item.Data.itemkind[2])
            {
                case IK3_ENCHANT: // Sunstone                    

                    if (GetHasScrollactivated(10464))
                    {
                        activeitem = GetPlayerActivatedItem(10464);
                        
                        c_data.activateditem.Remove(activeitem);
                        //send pâcket to remove item
                        SendPlayerRemoveBuffByItem(activeitem.itemid);
                        result = true;
                    }
                    else if (GetHasScrollactivated(10487))
                    {
                        activeitem = GetPlayerActivatedItem(10487);
                        
                        c_data.activateditem.Remove(activeitem);
                        //send pâcket to remove item
                        SendPlayerRemoveBuffByItem(activeitem.itemid);
                        result = true;
                    }

                    break;
                case IK3_ELECARD: // Element card
                    if (GetHasScrollactivated(10464))
                    {
                        activeitem = GetPlayerActivatedItem(10464);
                        
                        c_data.activateditem.Remove(activeitem);
                        //send pâcket to remove item
                        SendPlayerRemoveBuffByItem(activeitem.itemid);
                        result = true;
                    }
                    else if (GetHasScrollactivated(10487))
                    {
                        activeitem = GetPlayerActivatedItem(10487);
                       
                        c_data.activateditem.Remove(activeitem);
                        //send pâcket to remove item
                        SendPlayerRemoveBuffByItem(activeitem.itemid);
                        result = true;
                    }

                    break;
                case IK3_PIERDICE: // Moonstone
                    if (GetHasScrollactivated(26470))
                    {
                        activeitem = GetPlayerActivatedItem(26470);
                        
                        c_data.activateditem.Remove(activeitem);
                        SendPlayerRemoveBuffByItem(activeitem.itemid);
                        //send pâcket to remove item
                        result = true;
                    }
                    break;
            }
            return result;
        }
        public bool SuitableCardMaterial(int dwEle, int dwRef, int dwID)
        {
            int item_firstLevel = -1, item_secondLevel = -1;
            switch (dwEle)
            {
                case Item.ELEMENT_NONE:
                    return true;
                case Item.ELEMENT_FIRE:
                    item_firstLevel = 3205;
                    item_secondLevel = 3206;
                    break;
                case Item.ELEMENT_ELECTRICITY:
                    item_firstLevel = 3215;
                    item_secondLevel = 3216;
                    break;
                case Item.ELEMENT_EARTH:
                    item_firstLevel = 3220;
                    item_secondLevel = 3221;
                    break;
                case Item.ELEMENT_WATER:
                    item_firstLevel = 3210;
                    item_secondLevel = 3211;
                    break;
                case Item.ELEMENT_WIND:
                    item_firstLevel = 3225;
                    item_secondLevel = 3226;
                    break;
                default:
                    return false;
            }
            return ((dwID == item_firstLevel && dwRef < 5) || (dwID == item_secondLevel && dwRef >= 5));
        }
        public int GetElementByCardID(int dwID)
        {
            switch (dwID)
            {
                case 3205:
                case 3206:
                    return Item.ELEMENT_FIRE;
                case 3215:
                case 3216:
                    return Item.ELEMENT_ELECTRICITY;
                case 3220:
                case 3221:
                    return Item.ELEMENT_EARTH;
                case 3210:
                case 3211:
                    return Item.ELEMENT_WATER;
                case 3225:
                case 3226:
                    return Item.ELEMENT_WIND;
                default:
                    return 0;
            }
        }
    }
    public class DiceRoller
    {
        static Random dice
        {
            get
            {
                return WorldServer.c_random;
            }
        }
        public static bool Roll(int chance)
        {
            return dice.Next(1, 100) <= chance;
        }
        public static Point RandomPointInCircle(Point center, float radius)
        {
            Point point = new Point(0, center.y, 0, center.angle);
            float angle = (float)(DLL.rand() * (Math.PI * 2) / DLL.RAND_MAX);
            float distance = (float)Math.Sqrt(DLL.rand() * 1.0 / DLL.RAND_MAX) * radius;
            point.x = (float)Math.Cos(angle) * distance + center.x;
            point.z = (float)Math.Sin(angle) * distance + center.z;
            return point;
        }
        public static int RandomNumber(int from, int to)
        {
            return dice.Next(from, to + 1);
        }
    }
    public class CSVParser
    {
        string csv = "";
        string[] contents;
        public CSVParser(string csvFile)
        {
            csv = csvFile;
            contents = new StreamReader(csv).ReadToEnd().Split('\n');
        }
        public int[] GetIntList(int column)
        {
            List<int> objects = new List<int>();
            for (int i = 0; i < contents.Length; i++)
            {
                try
                {
                    objects.Add(int.Parse(contents[i].Split(',')[column]));
                }
                catch
                {
                    Log.Write(Log.MessageType.warning, "CSVParser::GetIntList({0}): can't read row {1}", column, i);
                }
            }
            return objects.ToArray();
        }
        public int[] GetIntRow(int row)
        {
            int[] objects;
            try
            {
                string[] c = contents[row].Split(',');
                objects = new int[c.Length];
                for (int i = 0; i < c.Length; i++)
                    objects[i] = int.Parse(c[i]);
                return objects;
            }
            catch
            {
                Log.Write(Log.MessageType.warning, "CSVParser::GetIntRow({0}): can't read row", row);
                return null;
            }
        }
    }
}
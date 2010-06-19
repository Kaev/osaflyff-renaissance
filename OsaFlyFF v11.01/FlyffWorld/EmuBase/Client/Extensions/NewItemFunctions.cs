using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using LuaInterface;

/// [Adidishen]
/// Now you don't need to modify using directives. :)

namespace FlyffWorld
{
    public partial class Client
    {
        /// <summary>
        /// Gets the amount of items associated with the given item ID in the inventory, regardless of the amount of stacks.
        /// </summary>
        /// <param name="itemID">The item ID of the item to search for.</param>
        /// <returns></returns>
        public int GetItemCount(int itemID)
        {
            int C = 0;
            for (int i = 0; i < c_data.inventory.Count; i++)
                if (c_data.inventory[i].c_item != null && c_data.inventory[i].c_item.dwItemID == itemID)
                    C += c_data.inventory[i].c_item.dwQuantity;
            return C;
        }
        /// <summary>
        /// Gets all slots containing an item associated with the given item ID.
        /// </summary>
        /// <param name="itemID">The item ID of the item to search for.</param>
        /// <returns></returns>
        public List<Slot> GetItemStacks(int itemID)
        {
            List<Slot> slots = new List<Slot>();
            for (int i = 0; i < c_data.inventory.Count; i++)
                if (c_data.inventory[i].c_item != null && c_data.inventory[i].c_item.dwItemID == itemID)
                    slots.Add(c_data.inventory[i]);
            return slots;
        }
        /// <summary>
        /// Gets the first slot containing an item with the given ID.
        /// </summary>
        /// <param name="itemID">The item ID to search for.</param>
        /// <returns></returns>
        public Slot GetSlotByItemID(int itemID)
        {
            for (int i = 0; i < c_data.inventory.Count; i++)
                if (c_data.inventory[i].c_item != null && c_data.inventory[i].c_item.dwItemID == itemID)
                    return c_data.inventory[i];
            return null;
        }

        /// <summary>
        /// Decreases the quantity of the item in the given slot by 1.
        /// </summary>
        /// <param name="src">The slot containing an item to decrease the quantity by 1.</param>
        public void DecreaseQuantity(Slot src)
        {
            DecreaseQuantity(src, 1);
        }
        /// <summary>
        /// Decreases the quantity of the item in the given slot by a given number.
        /// </summary>
        /// <param name="src">The slot containing an item to decrease the quantity by.</param>
        /// <param name="by">The amount of items to remove from the stack.</param>
        public void DecreaseQuantity(Slot src, int by)
        {
            if (src.c_item != null)
            {
                src.c_item.dwQuantity -= by;
                if (src.c_item.dwQuantity <= 0)
                    src.c_item.dwQuantity = 0;
                SendItemUpdate(new ItemUpdateStatus(src.dwID, ITEM_MODTYPE_QUANTITY, src.c_item.dwQuantity,0));
                if (src.c_item.dwQuantity <= 0)
                    src.c_item = null;
            }
        }
        /// <summary>
        /// Safely swaps two slots.
        /// </summary>
        /// <param name="src">The first slot to swap.</param>
        /// <param name="dst">The second slot to swap.</param>
        public void SwapSlots(Slot src, Slot dst)
        {
            int a = src.dwPos;
            src.dwPos = dst.dwPos;
            dst.dwPos = a;
        }
        /// <summary>
        /// Deletes an item from the given position.
        /// </summary>
        /// <param name="dwSlot">The position number to delete the item from.</param>
        public void DeleteItem(int dwSlot)
        {
            Slot slot = GetSlotByPosition(dwSlot);
            if (slot != null)
                DeleteItem(slot);
        }
        /// <summary>
        /// Deletes the given item from the first slot which has it.
        /// </summary>
        /// <param name="item">The item to delete.</param>
        public void DeleteItem(Item item)
        {
            Slot slot = GetSlotByItem(item);
            if (slot != null)
                DeleteItem(slot);
        }
        /// <summary>
        /// Deletes the item from the given slot.
        /// </summary>
        /// <param name="slot">The slot to empty.</param>
        public void DeleteItem(Slot slot)
        {
            SendItemUpdate(new ItemUpdateStatus(slot.dwID, ITEM_MODTYPE_QUANTITY, 0,0));
            slot.c_item = null;
        }
        /// <summary>
        /// Creates an item on the first available slot.
        /// </summary>
        /// <param name="item">The item to create.</param>
        public void CreateItem(Item item)
        {
            CreateItem(item, GetFirstAvailableSlot());
        }
        /// <summary>
        /// Creates an item on a specific slot position number.
        /// </summary>
        /// <param name="item">The item to create.</param>
        /// <param name="slot">The slot position number where the item will be created at.</param>
        public void CreateItem(Item item, int slot)
        {
            CreateItem(item, GetSlotByPosition(slot));
        }
        /// <summary>
        /// Creates an item on a FlyffWorld.Slot variable.
        /// </summary>
        /// <param name="item">The item to create.</param>
        /// <param name="slot">The FlyffWorld.Slot class to create the item on.</param>
        public void CreateItem(Item item, Slot slot)
        {
            if (slot == null || slot.c_item != null)
            {
                SendMessageInfo(TID_GAME_LACKSPACE);
                return;
            }
            slot.c_item = item;
            Log.Write(Log.MessageType.debug, "New item created in position {0}, unique ID {1}", slot.dwPos, slot.dwID);
            SendItemCreation(slot);
        }
        /// <summary>
        /// Gets the first available slot in the player's inventory.
        /// </summary>
        /// <returns></returns>
        public Slot GetFirstAvailableSlot()
        {
            int i = 0;
            Slot current;
            while (i < 0x2A)
                if ((current = GetSlotByPosition(i)).c_item == null)
                    return current;
                else
                    i++;
            return null;
        }
        /// <summary>
        /// Gets the first available slot position number in the player's inventory.
        /// </summary>
        /// <returns></returns>
        public int GetFirstAvailableSlotPos()
        {
            int i = 0;
            while (i < 0x2A)
                if (SlotFree(i))
                    return i;
                else
                    i++;
            return -1;
        }
        /// <summary>
        /// Determines if the slot position number provided has an item on it - true if it doesn't; otherwise, false.
        /// </summary>
        /// <param name="slot">The slot position number that will be checked.</param>
        /// <returns></returns>
        public bool SlotFree(int slot)
        {
            Slot s = GetSlotByPosition(slot);
            return s.c_item == null;
        }
        /// <summary>
        /// Gets the FlyffWorld.Slot variable that is associated with the given slot position number.
        /// </summary>
        /// <param name="pos">The slot's position number.</param>
        /// <returns></returns>
        public Slot GetSlotByPosition(int pos)
        {
            for (int i = 0; i < c_data.inventory.Count; i++)
                if (c_data.inventory[i].dwPos == pos)
                    return c_data.inventory[i];
            return null;
        }
        /// <summary>
        /// Gets the FlyffWorld.Slot variable that is associated with the given slot position number.
        /// </summary>
        /// <param name="pos">The slot's position number.</param>
        /// <param name="itemSlot">Will hold the slot that was found if it was found. Note: pass the slot uninitialized.</param>
        /// <returns>A boolean which declares if the slot was found or not. (true for found, false for not found)</returns>
        public bool TryGetSlotByPosition(int pos, out Slot itemSlot)
        {
            // Goes through slots.
            for (int i = 0; i < c_data.inventory.Count; i++)
            {
                // Check if the slot position corresponds to the current slot index. (check to see if this is the one...)
                if (c_data.inventory[i].dwPos == pos)
                {
                    // Found the slot.
                    itemSlot = c_data.inventory[i]; // set out slot argument.
                    return true; // return true stating that we found it.
                }
            }
            // We went through all of the item slots and didn't find it.
            itemSlot = null; // set out parameter to null.
            return false; // return false stating that we didn't find it.
        }
        /// <summary>
        /// Gets the FlyffWorld.Slot variable that is associated with the given item.
        /// </summary>
        /// <param name="item">The slot's item.</param>
        /// <returns></returns>
        public Slot GetSlotByItem(Item item)
        {
            for (int i = 0; i < c_data.inventory.Count; i++)
                if (c_data.inventory[i].c_item == item)
                    return c_data.inventory[i];
            return null;
        }
        /// <summary>
        /// Gets the FlyffWorld.Slot variable that is associated with the given slot ID.
        /// </summary>
        /// <param name="id">The slot's unique ID.</param>
        /// <returns></returns>
        public Slot GetSlotByID(int id)
        {
            for (int i = 0; i < c_data.inventory.Count; i++)
                if (c_data.inventory[i].dwID == id)
                    return c_data.inventory[i];
            return null;
        }
        /// <summary>
        /// Exchange normal weapon by unique Weapon.
        /// </summary>
        /// <param name="weaponslot">The slot where is your normal weapon.</param>
        /// <returns></returns>
        public Item ExchangeByUniqWeapon(Slot weaponslot)
        {
            Item item = new Item();
            item.dwQuantity = 1;
            //we have to test item kind used and level

            if (weaponslot.c_item.Data.reqLevel >= 60 && weaponslot.c_item.Data.reqLevel <= 70)
            {
                switch (weaponslot.c_item.Data.reqJob)
                {
                    case 1: item.dwItemID = (weaponslot.c_item.Data.itemkind[2] == 2) ? 22026 : 22034; break; //is it a sword or axe ?
                    case 2: item.dwItemID = (weaponslot.c_item.Data.itemkind[2] == 12) ? 22050 : 22054; break; //is it a bow or a yoyo ?
                    case 3: item.dwItemID = (weaponslot.c_item.Data.itemkind[2] == 5) ? 22042 : 22046; break;
                    case 4: item.dwItemID = (weaponslot.c_item.Data.itemkind[2] == 6) ? 22058 : 22062; break; //wand or staff ?

                    case 6: item.dwItemID = (weaponslot.c_item.Data.itemkind[2] == 2) ? 22030 : 22038; break;

                }
            }
            if (weaponslot.c_item.Data.reqLevel > 70 && weaponslot.c_item.Data.reqLevel <= 90)
            {
                switch (weaponslot.c_item.Data.reqJob)
                {
                    case 1: item.dwItemID = (weaponslot.c_item.Data.itemkind[2] == 2) ? 22027 : 22035; break; //is it for mercenary or knight ?
                    case 2: item.dwItemID = (weaponslot.c_item.Data.itemkind[2] == 12) ? 22051 : 22055; break; //is it for acrobat or ?
                    case 3: item.dwItemID = (weaponslot.c_item.Data.itemkind[2] == 5) ? 22043 : 22047; break;
                    case 4: item.dwItemID = (weaponslot.c_item.Data.itemkind[2] == 6) ? 22059 : 22063; break; //wand or staff ?
                    case 6: item.dwItemID = (weaponslot.c_item.Data.itemkind[2] == 2) ? 22031 : 22039; break;

                }

            }
            if (weaponslot.c_item.Data.reqLevel > 90 && weaponslot.c_item.Data.reqLevel <= 110)
            {
                switch (weaponslot.c_item.Data.reqJob)
                {
                    case 1: item.dwItemID = (weaponslot.c_item.Data.itemkind[2] == 2) ? 22028 : 22036; break; //is it for mercenary or knight ?
                    case 2: item.dwItemID = (weaponslot.c_item.Data.itemkind[2] == 12) ? 22052 : 22056; break; //is it for acrobat or ?
                    case 3: item.dwItemID = (weaponslot.c_item.Data.itemkind[2] == 5) ? 22044 : 22048; break;
                    case 4: item.dwItemID = (weaponslot.c_item.Data.itemkind[2] == 6) ? 22060 : 22064; break; //wand or staff ?
                    case 6: item.dwItemID = (weaponslot.c_item.Data.itemkind[2] == 2) ? 22032 : 22040; break;

                }

            }
            if (weaponslot.c_item.Data.reqLevel > 110 && weaponslot.c_item.Data.reqLevel <= 120)
            {
                switch (weaponslot.c_item.Data.reqJob)
                {
                    case 1: item.dwItemID = (weaponslot.c_item.Data.itemkind[2] == 2) ? 22029 : 22037; break;//is it for mercenary or knight ?
                    case 2: item.dwItemID = (weaponslot.c_item.Data.itemkind[2] == 12) ? 22053 : 22057; break; //is it for acrobat or ?
                    case 3: item.dwItemID = (weaponslot.c_item.Data.itemkind[2] == 5) ? 22045 : 22049; break;
                    case 4: item.dwItemID = (weaponslot.c_item.Data.itemkind[2] == 6) ? 22061 : 22065; break; //wand or staff ?
                    case 6: item.dwItemID = (weaponslot.c_item.Data.itemkind[2] == 2) ? 22033 : 22041; break;

                }

            }
            return item;
        }
        /// <summary>
        /// Exchange unique weapon by ultimate Weapon.
        /// </summary>
        /// <param name="weaponslot">The slot where is your unique weapon.</param>
        /// <returns></returns>
        public Item ExchangeByUltimWeapon(Slot weaponslot)
        {
            Item item = new Item();
            item.dwQuantity = 1;
            //we have to test item kind used and level
            switch (weaponslot.c_item.Data.reqLevel)
            {
                case 60:
                    switch (weaponslot.c_item.Data.reqJob)
                    {
                        case 1: item.dwItemID = (weaponslot.c_item.Data.itemkind[2] == 2) ? 22368 : 22378; break; //is it a sword or axe ?
                        case 2: item.dwItemID = (weaponslot.c_item.Data.itemkind[2] == 12) ? 22398 : 22403; break; //is it a bow or a yoyo ?
                        case 3: item.dwItemID = (weaponslot.c_item.Data.itemkind[2] == 5) ? 22388 : 22393; break;
                        case 4: item.dwItemID = (weaponslot.c_item.Data.itemkind[2] == 6) ? 22408 : 22413; break; //wand or staff ?

                        case 6: item.dwItemID = (weaponslot.c_item.Data.itemkind[2] == 2) ? 22030 : 22383; break;

                    }
                    break;
                case 75:
                    switch (weaponslot.c_item.Data.reqJob)
                    {
                        case 1: item.dwItemID = (weaponslot.c_item.Data.itemkind[2] == 2) ? 22369 : 22379; break; //is it for mercenary or knight ?
                        case 2: item.dwItemID = (weaponslot.c_item.Data.itemkind[2] == 12) ? 22399 : 22404; break; //is it for acrobat or ?
                        case 3: item.dwItemID = (weaponslot.c_item.Data.itemkind[2] == 5) ? 22389 : 22394; break;
                        case 4: item.dwItemID = (weaponslot.c_item.Data.itemkind[2] == 6) ? 22408 : 22414; break; //wand or staff ?
                        case 6: item.dwItemID = (weaponslot.c_item.Data.itemkind[2] == 2) ? 22374 : 22384; break;

                    }
                    break;
                case 90:
                    switch (weaponslot.c_item.Data.reqJob)
                    {
                        case 1: item.dwItemID = (weaponslot.c_item.Data.itemkind[2] == 2) ? 22370 : 22380; break; //is it for mercenary or knight ?
                        case 2: item.dwItemID = (weaponslot.c_item.Data.itemkind[2] == 12) ? 22400 : 22405; break; //is it for acrobat or ?
                        case 3: item.dwItemID = (weaponslot.c_item.Data.itemkind[2] == 5) ? 22390 : 22395; break;
                        case 4: item.dwItemID = (weaponslot.c_item.Data.itemkind[2] == 6) ? 22410 : 22415; break; //wand or staff ?
                        case 6: item.dwItemID = (weaponslot.c_item.Data.itemkind[2] == 2) ? 22375 : 22385; break;

                    }
                    break;
                case 105:
                    switch (weaponslot.c_item.Data.reqJob)
                    {
                        case 1: item.dwItemID = (weaponslot.c_item.Data.itemkind[2] == 2) ? 22371 : 22381; break;//is it for mercenary or knight ?
                        case 2: item.dwItemID = (weaponslot.c_item.Data.itemkind[2] == 12) ? 22401 : 22406; break; //is it for acrobat or ?
                        case 3: item.dwItemID = (weaponslot.c_item.Data.itemkind[2] == 5) ? 22391 : 22396; break;
                        case 4: item.dwItemID = (weaponslot.c_item.Data.itemkind[2] == 6) ? 22411 : 22416; break; //wand or staff ?
                        case 6: item.dwItemID = (weaponslot.c_item.Data.itemkind[2] == 2) ? 22376 : 22386; break;

                    }
                    break;

            }
            return item;
        }
        /// <summary>
        /// Create a complete copy of an item.
        /// </summary>
        /// <param name="item">Original Item.</param>
        /// <returns></returns>
        public Item CopyItem(Item item)
        {
            Item newitem = new Item();

            newitem.bExpired = item.bExpired;
            newitem.c_awakening = item.c_awakening;
            if (item.c_sockets.Length > 0)
            {
                ItemSockets newitemsocket = new ItemSockets(item.c_sockets.Length);
                for (int i = 0; i < item.c_sockets.Length; i++)
                {
                    newitemsocket[i] = item.c_sockets._sockets[i];
                }
                newitem.c_sockets = newitemsocket;
            }

            newitem.dwElement = item.dwElement;
            newitem.dwEleRefine = item.dwEleRefine;
            newitem.dwItemID = item.dwItemID;
            newitem.dwQuantity = item.dwQuantity;
            newitem.dwRefine = item.dwRefine;
            newitem.qwLastUntil = item.qwLastUntil;
            return newitem;
        }
        /// <summary>
        /// Return a bool to tell you if a weapon is an ultimate weapon.
        /// </summary>
        /// <param name="itemid">itemid of your weapon.</param>
        /// <returns></returns>
        public bool IsUltimateWeapon(int itemid)
        {
            if (itemid > 22368 && itemid < 22417)
                return true;
            else return false;
        }
        /// <summary>
        /// Exchange normal weapon by unique Weapon.
        /// </summary>
        /// <param name="weaponslot">The slot where is your normal weapon.</param>
        /// <returns></returns>
        public bool IsAmplificationScroll(int itemid)
        {
            if ((itemid > 26205 && itemid < 26216) || (itemid == 26219) || (itemid == 10473) || (itemid == 30148) || (itemid == 30149) || (itemid == 30150))
                return true;
            else return false;
        }


    }
}

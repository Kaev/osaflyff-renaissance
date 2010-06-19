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
                SendItemUpdate(new ItemUpdateStatus(src.dwID, ITEM_MODTYPE_QUANTITY, src.c_item.dwQuantity));
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
            SendItemUpdate(new ItemUpdateStatus(slot.dwID, ITEM_MODTYPE_QUANTITY, 0));
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
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace FlyffWorld
{
    public partial class Client
    {
        public void SendItemAwakening(Slot slot)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_AWAKENING);
            pak.Addbyte(slot.dwID);
            pak.Addbyte(0xA);
            pak.Addlong(slot.c_item.c_awakening);
            pak.Send(this);
        }
        public void SendItemUpdate(ItemUpdateStatus ius)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_UPDATE_ITEM);
            pak.Addbyte(NULL);
            pak.Addbyte(ius.itemUniqueID);
            pak.Addbyte(ius.updateModifier);
            pak.Addint(ius.modifierData);
            pak.Send(this);
        }
        public void SendItemPositionMove(int o, int n)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_MOVE_ITEM_INV);
            pak.Addbyte(0);
            pak.Addbyte(o);
            pak.Addbyte(n);
            pak.Send(this);
        }
        public void SendItemCreation(Slot slot)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_NEW_ITEM);
            pak.Addbyte(0);
            pak.Addint(-1);
            pak.AddItemData(slot.c_item);
            pak.Addbyte(1);
            pak.Addbyte(slot.dwID);
            pak.Addbyte(slot.c_item.dwQuantity);
            pak.Addbyte(0);
            pak.Send(this);
        }
    }
}

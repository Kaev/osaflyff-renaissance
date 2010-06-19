using System;
using System.Collections.Generic;
using System.Text;

namespace FlyffWorld
{
    public partial class Client
    {
        public void SendCollectStart()
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_COLLECT_START);
            pak.Send(this);
        }
        public void SendCollectStop()
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_COLLECT_STOP);
            pak.Send(this);
        }
        public void SendCollectedItem(Slot coll, Slot slot)
        {
            Packet pak = new Packet();
            pak.Addint32(0xFFFFFF00);
            pak.Addint32(dwMoverID);
            pak.Addint16(3);
            pak.Addint32(dwMoverID);
            pak.Addint16(PAK_UPDATE_ITEM);
            pak.Addbyte(NULL);
            pak.Addbyte(0);
            pak.Addbyte(1);
            pak.Addint16(coll.c_item.dwCharge);
            pak.Addint16(0);
            //----- Nouvel item
            pak.Addint32(dwMoverID);
            pak.Addint16(PAK_NEW_ITEM);
            pak.Addbyte(0);
            pak.Addint32(-1);
            pak.AddItemData(slot.c_item);
            pak.Addbyte(1);
            pak.Addbyte(slot.dwID);
            pak.Addbyte(slot.c_item.dwQuantity);
            pak.Addbyte(0);
            //----- Reset de la barre de collecte (+ mess ?)
            pak.Addint32(dwMoverID);
            pak.Addint16(PAK_RESET_COLLECT_JAUGE);
            pak.Addint32(slot.c_item.dwItemID);
            pak.Send(this);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;

namespace FlyffWorld
{
    public partial class Client
    {
        public void sendAskPassword(int passwordexist)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PACK_BANK_SENDASKPASSWORD);
            pak.Addint32(passwordexist);
            pak.Send(this);

        }
        public void sendBankPassword(int passwordOK)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PACK_BANK_OPENBANK);
            pak.Addint32(passwordOK);
            pak.Addint32(-1);
            pak.Addint32(0);
            pak.Send(this);

        }
        public void SendBankAddItem(Item item, int quantity, int bankposition)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PACK_BANK_ADDITEM);
            pak.Addbyte(bankposition);
            pak.Addint32(-1);
            pak.AddItemData(item);
            /*
            pak.Addint32(item.dwItemID);
            pak.Addint32(0);//time remaining ?
            pak.Addint32(0);
            pak.Addint16(quantity);
            pak.Addbyte(0);
            if (item.Data.itemkind[2] > 24 && item.Data.itemkind[2] != 57 || item.Data.itemkind[2] != 58)//if it's an item that don't need endurance value
            {
                pak.Addint32(-1);
                pak.Addint32(0);
            }
            else
                pak.Addint64(item.Data.endurance);
            pak.Addbyte(0);
            pak.Addint32(item.dwRefine);
            pak.Addint32(0);
            pak.Addbyte(item.dwElement);
            pak.Addint32(item.dwEleRefine);
            pak.Addint32(0);
            pak.Addbyte(item.c_sockets.Length);//number of socket
            for (int i = 0; i < item.c_sockets.Length; i++)
            {
                pak.Addint16(item.c_sockets[i]);
            }
            pak.Addbyte(0);
            pak.AddUint64(item.c_awakening);
            pak.Addint32(0);
            */
            pak.Send(this);
        }
        public void SendBankUpdatePenya(int penyainbank, int bankposition)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PACK_BANK_UPDATEPENYA);
            pak.Addbyte(bankposition);
            pak.Addint32(c_data.dwPenya);
            pak.Addint32(penyainbank);
            pak.Send(this);

        }
        public void SendBankTakeItem(Item item, int bankposition, int bankslot, int itemremaining, int quantity)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PACK_BANK_TAKEITEM); //add item in inventory
            pak.Addint32(-1);
            pak.AddItemData(item);
            /*
            pak.Addint32(item.dwItemID);
            pak.Addint32(0);
            pak.Addint32(0);
            pak.Addint16(quantity);
            pak.Addbyte(0);
            if (item.Data.endurance == 0)
            {
                pak.Addint32(-1);
                pak.Addint32(0);
            }
            else
                pak.Addint64(item.Data.endurance);
            pak.Addbyte(0);
            pak.Addint32(item.dwRefine);
            pak.Addint32(0);
            pak.Addbyte(item.dwElement);
            pak.Addint32(item.dwEleRefine);
            pak.Addint32(0);
            pak.Addbyte(item.c_sockets.Length);//number of socket
            for (int i = 0; i < item.c_sockets.Length; i++)
            {
                pak.Addint16(item.c_sockets[i]);
            }
            pak.Addbyte(0);
            pak.AddUint64(item.c_awakening);
            pak.Addint32(0);
            pak.Addint32(0);
            pak.StartNewMergedPacket(dwMoverID, PACK_BANK_UPDATETEM);
            pak.Addbyte(bankposition);
            pak.Addint16(bankslot);
            pak.Addint32(itemremaining);*/
            pak.Send(this);
        }

    }
}
using System;
using System.Collections.Generic;
using System.Text;

namespace FlyffWorld
{
    public partial class Client
    {
        /*public void SendPlayerShopView(Client shop)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(shop.dwMoverID, PAK_SHOP_SHOWINTERFACE);
            pak.Addbyte(shop.c_data.shop.items.Count);
            for (int i = 0; i < shop.c_data.shop.items.Count; i++)
            {
                PlayerShopItem si = shop.c_data.shop.items[i];
                if (si == null)
                    continue;
                Item item = shop.GetSlotByID(si.id).c_item;
                if (item == null)
                    continue;
                pak.Addbyte(si.pos);
                pak.Addint(si.id);
                pak.AddItemData(item);
                pak.Addshort(si.quantity);
                pak.Addshort(NULL);
                pak.Addint(si.price);
            }
            pak.Addint(0x00000001);
            pak.Send(this);
        }
        public void SendPlayerShopInterface()
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_SHOP_SHOWINTERFACE);
            pak.Send(this);
        }
        public void SendPlayerShopClose()
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_SHOP_CLOSE);
            pak.Addint(1);
            pak.Send(this);
            SendToVisible(pak);
        }
        public void SendPlayerShopOpen(Client otherclient)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(otherclient.dwMoverID, PAK_SHOP_OPEN);
            pak.Addstring(otherclient.c_data.shop.name);
            pak.Send(this);
        }
        public void SendPlayerShopOpen()
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_SHOP_OPEN);
            pak.Addstring(c_data.shop.name);
            pak.Send(this);
            SendToVisible(pak);
        }
        public void SendPlayerShopItemRemove(byte pos)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_SHOP_REMOVEITEM);
            pak.Addbyte(pos);
            pak.Send(this);
        }
        public void SendPlayerShopItemNew(PlayerShopItem si)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_SHOP_ADDITEM);
            pak.Addshort(si.pos);
            pak.Addbyte(si.id);
            pak.Addshort(si.quantity);
            pak.Addint(si.price);
            pak.Send(this);
        }
        public void SendPlayerShopItemUpdate(int p, int q)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_SHOP_UPDATEQUANTITY);
            pak.Addbyte(p);
            pak.Addshort(q);
            pak.Send(this);
            for (int i = 0; i < c_data.shop.consumers.Count; i++)
                pak.Send(c_data.shop.consumers[i]);
        }*/
    }
}

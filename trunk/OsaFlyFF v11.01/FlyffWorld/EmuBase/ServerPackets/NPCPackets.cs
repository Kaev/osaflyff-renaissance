using System;
using System.Collections.Generic;
using System.Text;

namespace FlyffWorld
{
    public partial class Client
    {
        public void SendNPCSpawn(NPC npc)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(npc.dwMoverID, PAK_MOVER_SPAWN);
            pak.Addbyte(5);
            pak.Addint32(npc.npc_model_id); // npc model!
            pak.Addbyte(5);
            pak.Addint16(npc.npc_model_id); // npc model here too
            pak.Addint16(npc.npc_size);
            pak.Addfloat(npc.c_position.x);
            pak.Addfloat(npc.c_position.y);
            pak.Addfloat(npc.c_position.z);
            pak.Addint16((short)(npc.c_position.angle * 50)); // angle (*50 = correction)
            pak.Addint32(npc.dwMoverID);

            pak.Addbyte(1);
            pak.Addint16(0);
            pak.Addint32(-1);
            pak.Addint64(1);
            pak.Addbyte(1);
            pak.Addint64(0);
            pak.Addint32(0);
            pak.Addint16(0);
            //pak.Addhex("01 00 00 FF FF FF FF 01 00 00 00 00 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00");

            pak.Addstring(npc.npc_type_name);
            pak.Addint32(0x3f800000);
            pak.Addint32(0);
            //pak.Addhex("00 00 80 3f 00 00 00 00");
            pak.Send(this);
        }
        public void SendNPCShop(NPCShopData nsd, int npc_moverid)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(npc_moverid, PAK_NPC_SHOP);
            for (int tab = 0; tab < 4; tab++)
            {
                for (int i = 0; i < 100; i++)
                    pak.Addint32(i);
                pak.Addbyte(nsd.tabinfo[tab]); // amount of items.
                for (int i = 0; i < 100; i++)
                {
                    if (nsd.shopitems[tab, i] == null)
                        continue;
                    NPCShopItem si = nsd.shopitems[tab, i];
                    Item item = new Item();
                    item.dwItemID = si.id;
                    item.dwQuantity = si.quantity;
                    pak.Addbyte(i);
                    pak.Addint32(i);
                    pak.AddItemData(item);
                }
                for (int i = 0; i < 100; i++)
                    pak.Addint32(i);
            }

            pak.Send(this);
        }
        public void SendCloseUpgradeOffer()
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_VALID_ITEMUPDATE);
            pak.Send(this);
        }
        public void SendCloseUpgradeOffer(int dwResult)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_CLOSE_CONFIRMWINDOWS);
            pak.Addint32(dwResult);
            pak.Send(this);
        }
    }
}
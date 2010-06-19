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
            pak.Addint(npc.npc_model_id); // npc model!
            pak.Addbyte(5);
            pak.Addshort(npc.npc_model_id); // npc model here too
            pak.Addshort(npc.npc_size);
            pak.Addfloat(npc.c_position.x);
            pak.Addfloat(npc.c_position.y);
            pak.Addfloat(npc.c_position.z);
            pak.Addshort((short)(npc.c_position.angle * 50)); // angle (*50 = correction)
            pak.Addint(npc.dwMoverID);
            pak.Addhex("01 00 00 FF FF FF FF 01 00 00 00 00 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00");
            pak.Addstring(npc.npc_type_name);
            pak.Addhex("0000803f00000000");
            pak.Send(this);
        }
        public void SendNPCShop(NPCShopData nsd, int npc_moverid)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(npc_moverid, PAK_NPC_SHOP);
            for (int tab = 0; tab < 4; tab++)
            {
                for (int i = 0; i < 100; i++)
                    pak.Addint(i);
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
                    pak.Addint(i);
                    pak.AddItemData(item);
                }
                for (int i = 0; i < 100; i++)
                    pak.Addint(i);
            }

            pak.Send(this);
        }
    }
}
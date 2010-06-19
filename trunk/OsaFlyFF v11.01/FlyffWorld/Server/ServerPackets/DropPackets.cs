using System;
using System.Collections.Generic;
using System.Text;

namespace FlyffWorld
{
    public partial class Client
    {
        public void SendDropSpawn(Drop drop) // Nicco->Drops
        {
            // TODO: Test out this packet and see if it works etc
            if (drop.item.dwQuantity <= 0)
                drop.item.dwQuantity = 1;
            Packet pak = new Packet();
            pak.StartNewMergedPacket(drop.dwMoverID, PAK_MOVER_SPAWN);
            pak.Addbyte(4);
            pak.Addint(drop.item.dwItemID);
            pak.Addbyte(4);
            pak.Addshort(drop.item.dwItemID);
            pak.Addshort(drop.dwMoverSize);
            pak.Addfloat(drop.c_position.x);
            pak.Addfloat(drop.c_position.y);
            pak.Addfloat(drop.c_position.z);
            pak.Addshort((short)(drop.c_position.angle * 50));
            pak.Addint(drop.dwMoverID);
            pak.AddDropItemData(drop.item); // Nicco
            pak.Send(this);
        }
    }
}
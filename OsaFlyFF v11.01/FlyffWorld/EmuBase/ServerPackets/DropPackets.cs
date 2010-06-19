using System;
using System.Collections.Generic;
using System.Text;

namespace FlyffWorld
{
    public partial class Client
    {
        public void SendDropSpawn(Drop drop) // Nicco->Drops
        {
            Log.Write(Log.MessageType.debug, "on est dnas le packet");
            // TODO: Test out this packet and see if it works etc
            if (drop.item.dwQuantity <= 0)
                drop.item.dwQuantity = 1;
            Packet pak = new Packet();
            pak.StartNewMergedPacket(drop.dwMoverID, PAK_MOVER_SPAWN);
            pak.Addbyte(4);
            pak.Addint32(drop.item.dwItemID);
            pak.Addbyte(4);
            pak.Addint16(drop.item.dwItemID);
            pak.Addint16(drop.dwMoverSize);
            pak.Addfloat(drop.c_position.x);
            pak.Addfloat(drop.c_position.y);
            pak.Addfloat(drop.c_position.z);
            pak.Addint16((short)(drop.c_position.angle * 50));
            pak.Addint32(drop.dwMoverID);
            pak.Addint32(-1);
           // pak.AddItemData(drop.item); // Nicco  [Caali] same functions
            pak.Addint32(drop.item.dwItemID);
            pak.Addint32(0);//time remaining ?
            pak.Addint32(0);
            pak.Addint16(drop.item.dwQuantity);
            pak.Addbyte(0);
            if (drop.item.Data.endurance == 0)
            {
                pak.Addint32(-1);
                pak.Addint32(0);
            }
            else
                pak.Addint64(drop.item.Data.endurance);
            pak.Addbyte(0);
            pak.Addint32(drop.item.dwRefine);
            pak.Addint32(0);
            pak.Addbyte(drop.item.dwElement);
            pak.Addint32(drop.item.dwEleRefine);
            pak.Addint32(0);
            pak.Addbyte(drop.item.c_sockets.Length);//number of socket
            for (int j = 0; j < drop.item.c_sockets.Length; j++)
            {
                pak.Addint16(drop.item.c_sockets[j]);
            }
            pak.Addbyte(0);
            pak.AddUint64(drop.item.c_awakening);
            pak.Addint64(0);
            Log.Write(Log.MessageType.debug, "on l'envoi");
            pak.Send(this);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;

namespace FlyffWorld
{
    public partial class Client
    {
        public void SendMonsterSpawn(Monster mob)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(mob.dwMoverID, PAK_MOVER_SPAWN);
            pak.Addbyte(5);
            pak.Addint32(mob.mob_model);
            pak.Addbyte(5);
            pak.Addint16(mob.mob_model);
            pak.Addint16(mob.dwMoverSize); // size
            pak.Addfloat(mob.c_position.x);
            pak.Addfloat(mob.c_position.y);
            pak.Addfloat(mob.c_position.z);
            pak.Addint16(0);
            pak.Addint32(mob.dwMoverID);
            pak.Addint16(5);
            pak.Addbyte(0);
            pak.Addint32(mob.c_attributes[DST_HP]); // Mob HP

            pak.Addint64(0);
            pak.Addint16(0xb);
            pak.Addint32(-1);
            pak.Addint16(0);
            //pak.Addhex("00 00 00 00 00 00 00 00 0B 00 FF FF FF FF 00 00");

            pak.Addbyte(mob.mob_aggressive ? 1 : 0);

            pak.Addint32(0);
            pak.Addint32(0);
            pak.Addint32(0);
            pak.Addint16(0x3f80);
            pak.Addint32(0);
            //pak.Addhex("00 00 00 00 00 00 00 00 00 00 00 00 80 3F 00 00 00 00");

            pak.Send(this);
        }
    }
}

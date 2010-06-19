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
            pak.Addint(mob.mob_model);
            pak.Addbyte(5);
            pak.Addshort(mob.mob_model);
            pak.Addshort(mob.dwMoverSize); // size
            pak.Addfloat(mob.c_position.x);
            pak.Addfloat(mob.c_position.y);
            pak.Addfloat(mob.c_position.z);
            pak.Addshort(0);
            pak.Addint(mob.dwMoverID);
            pak.Addshort(5);
            pak.Addbyte(0);
            pak.Addint(mob.c_attributes[DST_HP]); // Mob HP
            pak.Addhex("00 00 00 00 00 00 00 00 0B 00 FF FF FF FF 00 00");
            pak.Addbyte(mob.mob_aggressive ? 1 : 0);
            pak.Addhex("00 00 00 00 00 00 00 00 00 00 00 00 80 3F 00 00 00 00");
            pak.Send(this);
        }
    }
}

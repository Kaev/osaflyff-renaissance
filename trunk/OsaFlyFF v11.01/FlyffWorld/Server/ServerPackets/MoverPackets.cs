using System;
using System.Collections.Generic;
using System.Text;

namespace FlyffWorld
{
    public partial class Client
    {
        public void SendPlayerShout(string shoutText)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(-1, PAK_SHOUT);
            pak.Addint(dwMoverID);
            pak.Addstring(c_data.strPlayerName);
            pak.Addstring(shoutText);
            pak.SendTo(WorldServer.world_players);
        }
        public void SendPlayerShout(string shoutText, int color)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(-1, PAK_SHOUT);
            pak.Addint(dwMoverID);
            pak.Addstring(c_data.strPlayerName);
            pak.Addstring(shoutText);
            pak.Addint(color);
            pak.SendTo(WorldServer.world_players);
        }
    }
    public partial class Mover
    {
        public void SendPlayerAttackMotion(int motion, int dwTarget)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_ATTACK_MELEE);
            pak.Addint(motion);
            pak.Addint(dwTarget);
            pak.Addint(0);
            pak.Addint(0x10000);
            SendToVisible(pak);
        }
        public void SendMonsterAttackMotion(int motion, int dwTarget)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_ATTACK_MOTION);
            pak.Addint(0); // a décoder
            pak.Addint(dwTarget);
            pak.Addint(motion);
            pak.Addint(0);
            SendToVisible(pak);
        }
        public void SendMoverDamaged(int atker_mID, int damage, int flags)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_DAMAGE);
            pak.Addint(atker_mID);
            pak.Addint(damage);
            pak.Addint(flags);
            if (this.child != null)
                pak.Send(child);
            SendToVisible(pak);
        }
        public void SendMoverDamaged(int atker_mID, int damage, int flags, Point kbfrom)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_DAMAGE);
            pak.Addint(atker_mID);
            pak.Addint(damage);
            pak.Addint(flags);
            if ((flags & 0x10000000) == 0x10000000)
            {
                pak.Addfloat(kbfrom.x);
                pak.Addfloat(kbfrom.y);
                pak.Addfloat(kbfrom.z);
            }
            if (this.child != null)
                pak.Send(child);
            SendToVisible(pak);            
        }
        public void SendMoverNewPosition()
        {
            c_destiny = c_position;
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_POS_TELEPORT);
            pak.Addfloat(c_position.x);
            pak.Addfloat(c_position.y);
            pak.Addfloat(c_position.z);
            if (this.child != null)
                pak.Send(child);
            SendToVisible(pak);
        }
        public void SendMoverChatBalloon(string playerChat)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_MOVER_CHAT);
            pak.Addstring(playerChat);
            if (child != null)
                pak.Send(child);
            SendToVisible(pak);
        }
        public void SendMoverDeath()
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_MOVER_DEATH);
            if (child != null)
                pak.Send(child);
            SendToVisible(pak);
        }
        public void SendMoverNewDestination(Point pos)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_MOVING_MOUSECLICK);
            pak.Addfloat(pos.x);
            pak.Addfloat(pos.y);
            pak.Addfloat(pos.z);
            pak.Addbyte(1);
            SendToVisible(pak);
        }
        public void SendMoverNewDestination()
        {
            SendMoverNewDestination(c_destiny);
        }
        public void SendMoverRevival()
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_REVIVE);
            if (child != null)
                pak.Send(child);
            SendToVisible(pak);
        }
        public void SendEffect(int effectID, Point pos)
        {
            // effectID: refer to byteidentifiers->convertedheaders->flyffgfx.cs for more info
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_FLYFF_EFFECT);
            pak.Addint(effectID);
            pak.Addfloat(pos.x);
            pak.Addfloat(pos.y);
            pak.Addfloat(pos.z);
            pak.Addint(0);
            if (child != null)
                pak.Send(child);
            SendToVisible(pak);
        }
        public void SendEffect(int effectID)
        {
            SendEffect(effectID, new Point());
        }
    }
}

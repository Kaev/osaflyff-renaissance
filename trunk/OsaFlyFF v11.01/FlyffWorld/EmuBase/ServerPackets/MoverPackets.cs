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
            pak.Addint32(dwMoverID);
            pak.Addstring(c_data.strPlayerName);
            pak.Addstring(shoutText);
            pak.SendTo(WorldServer.world_players);
        }
        public void SendPlayerShout(string shoutText, int color)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(-1, PAK_SHOUT);
            pak.Addint32(dwMoverID);
            pak.Addstring(c_data.strPlayerName);
            pak.Addstring(shoutText);
            pak.Addint32(color);
            pak.SendTo(WorldServer.world_players);
        }
    }
    public partial class Mover
    {
        public void SendPlayerAttackMotion(int motion, int dwTarget)
        {
            Packet pak = new Packet();
            
            pak.StartNewMergedPacket(dwMoverID, PAK_ATTACK_MELEE);
            pak.Addint32(motion);
            pak.Addint32(dwTarget);
            pak.Addint32(0);
            pak.Addint32(0x10000);
            //SendToVisible(pak);
        }
        public void SendMonsterAttackMotion(int motion, int dwTarget)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_ATTACK_MOTION);
            pak.Addint32(0); // to understand
            pak.Addint32(dwTarget);
            pak.Addint32(motion);
            pak.Addint32(0);
            SendToVisible(pak);
        }
        public void SendMoverDamaged(int atker_mID, int damage, AttackFlags dwFlags, Point pKnockbackFrom, float fKnockbackAngle)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_DAMAGE);
            pak.Addint32(atker_mID);
            pak.Addint32(damage);
            pak.Addint32((int)dwFlags);
            if ((dwFlags & AttackFlags.KNOCKBACK) == AttackFlags.KNOCKBACK)
            {
                pak.Addfloat(pKnockbackFrom.x);
                pak.Addfloat(pKnockbackFrom.y);
                pak.Addfloat(pKnockbackFrom.z);
                pak.Addint16((short)(fKnockbackAngle * 50)); // TODO: angle
            }
            if (this.child != null)
                pak.Send(child);
            SendToVisible(pak);
        }
        
        public void SendMoverDamaged(int atker_mID, int damage, int flags, Point kbfrom)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_DAMAGE);
            pak.Addint32(atker_mID);
            pak.Addint32(damage);
            pak.Addint32(flags);
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
        
        public void SendMoverMovement(DataPacket dp)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_MOVING_KEYBOARD);
            dp.dwPointer = 21;
            /*            float x = dp.Readfloat();
            float y = dp.Readfloat();
            float z = dp.Readfloat();
            // unknown 3 integers.
            // Caali: rotation vector
            dp.Readint32();
            dp.Readint32();
            dp.Readint32();
            float angle = dp.Readfloat();
            int moveFlags = dp.Readint32();
            int motionFlags = dp.Readint32();
            int actionFlags = dp.Readint32();*/
            pak.Addfloat(dp.Readfloat());
            pak.Addfloat(dp.Readfloat());
            pak.Addfloat(dp.Readfloat());

            pak.Addfloat(dp.Readfloat());
            pak.Addfloat(dp.Readfloat());
            pak.Addfloat(dp.Readfloat());

            pak.Addfloat(dp.Readfloat());
            pak.Addint32(dp.Readint32());
            pak.Addint32(dp.Readint32());
            pak.Addint32(dp.Readint32());
            try
            {
                while (true)
                    pak.Addbyte(dp.Readbyte());
            }
            catch { }
            // p.s incomplete
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
            pak.StartNewMergedPacket(dwMoverID, 0xF4);//this appear when monster die too...
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
            pak.Addint32(effectID);
            pak.Addfloat(pos.x);
            pak.Addfloat(pos.y);
            pak.Addfloat(pos.z);
            pak.Addint32(0);
            if (child != null)
                pak.Send(child);
            SendToVisible(pak);
        }
        public void SendEffect(int effectID)
        {
            SendEffect(effectID, new Point());
        }
        public void SendMoverBuff(Buff antibuff, int Applyeffect)
        {
            
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_BUFF);
            pak.Addint32(dwMoverID);
            pak.Addint16(Applyeffect);
            pak.Addint16(antibuff._skill.dwNameID);
            pak.Addint32(antibuff._skill.dwSkillLvl);
            pak.Addint32(antibuff._skill.dwSkillTime);
            if (child != null)
                pak.Send(child);
            SendToVisible(pak);
            
        }
        public void SendMoverRemoveAntiBuff(Buff antibuff)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_REMOVE_ACTIVATEDITEM);
            pak.Addint16(1);
            pak.Addint16(antibuff._skill.dwNameID);
            pak.Addint32(antibuff._skill.dwSkillLvl); 
            if (child != null)
                pak.Send(child);
            SendToVisible(pak);
        }
        public void SendMoverAttribSet(int attributeID, int count)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_SET_ATTRIBUTE);
            pak.Addint32(attributeID);
            pak.Addint32(count);
            if (child != null)
                pak.Send(child);
            SendToVisible(pak);
        }
        public void SendMoverAttribRaise(int attributeID, int amount, int csitembonus)
        {
            if (amount == 0) // we don't do < 1 because we want negative numbers too
                return;
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_RAISE_ATTRIBUTE);
            pak.Addint32(attributeID);
            pak.Addint32(amount);
            pak.Addint32(csitembonus);
            if (child != null)
                pak.Send(child);
            SendToVisible(pak);
        }
        public void SendMoverInstantMove(Point position)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_INSTANTMOVE);
            pak.Addfloat(position.x);
            pak.Addfloat(position.y);
            pak.Addfloat(position.z);
            pak.Addint32(-1);
            if (child != null)
                pak.Send(child);
            SendToVisible(pak);
        }
        public void SendMoverInstantMove2(Point position)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_INSTANTMOVE2);
            pak.Addfloat(position.x);
            pak.Addfloat(position.y);
            pak.Addfloat(position.z);
            pak.Addint32(0); //unknown float x from ?
            pak.Addint32(0); //unknown float y from ?
            pak.Addint32(0); //unknown float z from ?
            pak.Addint32(-1);
            if (child != null)
                pak.Send(child);
            SendToVisible(pak);
        }

        public void SendMoverAttribDecrease(int attributeID, int amount) // by exos
        {
            if (attributeID == 0 || amount == 0)
                return;
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_DECREASE_ATTRIBUTE);
            pak.Addint32(attributeID);
            pak.Addint32(amount);
            if (child != null)
                pak.Send(child);
            SendToVisible(pak);
        }
        public void SendChangeState(int Time)
        {
            
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_STATEEFFECT);
            pak.Addint32(Time);
            if (child != null)
                pak.Send(child);
            SendToVisible(pak);
        }
        public void SendUnknowFollowRelatedPacket(int atk_movid) //this packet is send by server after a mover been damaged and receive and effect...
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_UNKNOWFOLLOWRELATED);
            pak.Addint32(atk_movid);
            pak.Addint32(0);
        }

       

    }
}

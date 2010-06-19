using System;
using System.Collections.Generic;
using System.Text;

namespace FlyffWorld
{
    public partial class Client
    {
        public void SendPlayerFlagsUpdate()
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_PLAYER_FLAGS);
            pak.Addint32(c_data.m_PlayerFlags);
            pak.Send(this);
            SendToVisible(pak);
        }
        public void SendPlayerSound(int soundID)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_SOUND);
            pak.Addbyte(0);
            pak.Addint32(soundID);
            pak.Send(this);
        }
        public void SendPlayerNewFace()
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_UPDATE_FACE);
            pak.Addint32(c_data.dwCharacterID);
            pak.Addint32(c_data.dwFaceID);
            pak.Send(this);
            SendToVisible(pak);
        }
        public void SendPlayerNewHair()
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_UPDATE_HAIR);
            pak.Addbyte(c_data.dwHairID);

            pak.Addbytes
                (
                    c_data.c_haircolor.dwBlue, c_data.c_haircolor.dwGreen, c_data.c_haircolor.dwRed
                );
            //pak.Addhex(c_data.c_haircolor.hexstr.Substring(0, 6));

            pak.Send(this);
            SendToVisible(pak);
        }
        public void SendPlayerMusic(int musicID)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_MUSIC);
            pak.Addint32(musicID);
            pak.Send(this);
        }
        public void SendPlayerCombatInfo()
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_UPDATE_LEVELGROUP);
            pak.Addint64(c_data.qwExperience);
            pak.Addint32(c_data.dwLevel);
            pak.Addint16(0x0000);
            pak.Addint32(c_data.dwSkillPoints);
            pak.Send(this);
        }
        public void SendPlayerStats()
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_UPDATE_STATS);
            pak.Addint32(c_attributes[DST_STR]);
            pak.Addint32(c_attributes[DST_STA]);
            pak.Addint32(c_attributes[DST_DEX]);
            pak.Addint32(c_attributes[DST_INT]);
            pak.Addint32(0);
            pak.Addint32(c_data.dwStatPoints);
            pak.Send(this);
        }
        public void SendPlayerSkills()
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_UPDATE_SKILLS);
            int skillCount = c_data.skills.Count;
            for (int i = 0; i < skillCount; i++)
            {
                Skill curSkill = c_data.skills[i];
                pak.Addint32(curSkill.dwSkillID);
                pak.Addint32(curSkill.dwSkillLevel);
            }
            for (; skillCount < 45; skillCount++)
            {
                pak.Addint32(-1);
                pak.Addint32(0);
            }
            pak.Addint32(c_data.dwSkillPoints);
            pak.Send(this);
        }
        public void SendPlayerCSFoodBonus(int duration)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_SENDCSFOODBUFF);
            pak.Addint32(8);
            pak.Addint32(duration); // duration => in sec.
            pak.Send(this);
            SendToVisible(pak);
        }
        public void SendPlayerInformationMessage(int idMessage)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_INFORMATION_MESSAGE);
            pak.Addint32(idMessage);
            pak.Send(this);
        }
       
        public void SendMoverDespawn(int moverID)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(moverID, PAK_MOVER_DESPAWN);
            pak.Send(this);
        }
        public void SendPlayerSpawnOther(Client c)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(c.c_data.dwCharacterID, PAK_MOVER_SPAWN);
            pak.Addbyte(5);
            pak.Addint32(0xB + c.c_data.dwGender);
            pak.Addbyte(5);
            pak.Addint16(0xB + c.c_data.dwGender);
            pak.Addint16(c.dwMoverSize);
            pak.Addfloat(c.c_position.x);
            pak.Addfloat(c.c_position.y);
            pak.Addfloat(c.c_position.z);
            pak.Addint16(0); // angle
            pak.Addint32(c.dwMoverID);
            pak.Addint16(0);
            pak.Addbyte(1);
            pak.Addint32(c.c_attributes[FlyFF.DST_HP]);
            pak.Addint32(c.c_data.m_MovingFlags);
            pak.Addint32(c.c_data.m_MotionFlags);
            pak.Addbyte(1);
            pak.Addstring(c.c_data.strPlayerName);
            pak.Addbyte(c.c_data.dwGender);
            pak.Addbyte(0);
            pak.Addbyte(c.c_data.dwHairID);
            pak.Addint32(c.c_data.c_haircolor.dword);
            pak.Addbyte(c.c_data.dwFaceID);
            pak.Addint32(c.c_data.dwCharacterID);
            pak.Addbyte(c.c_data.dwClass);
            pak.Addint16(c.c_attributes[DST_STR]);
            pak.Addint16(c.c_attributes[DST_STA]);
            pak.Addint16(c.c_attributes[DST_DEX]);
            pak.Addint16(c.c_attributes[DST_INT]);
            pak.Addint16(c.c_data.dwLevel);
            pak.Addint32(c.c_data.dwGuildID > 0 ? 0xBB8 : -1);
            pak.Addint32(0x00000000);
            pak.Addbyte(c.c_data.dwGuildID > 0 ? 1 : 0);
            if (c.c_data.dwGuildID > 0)
            {
                Guild guild = GuildHandler.getGuildByGuildID(c.c_data.dwGuildID);
                if (guild == null)
                {
                    Log.Write(Log.MessageType.error, "Unknown guild ID error - guild ID {0} does not exist?", c.c_data.dwGuildID);
                    pak.Addint64(0);
                }
                else
                {
                    pak.Addint32(c.c_data.dwGuildID);
                    pak.Addint32(guild.duelInfo.dwDuelID); // TODO: Guild duel ID
                }
            }
            pak.Addint32(0);
            pak.Addbyte(0);
            pak.Addbyte(c.c_data.dwAuthority);
            pak.Addint32(c.c_data.m_PlayerFlags);

            pak.Addint32(0);
            pak.Addint32(0x1f8);
            pak.Addint32(0);
            //pak.Addhex("00 00 00 00   F8 01 00 00   00 00 00 00");

            pak.Addint32(c.c_data.dwKarma);
            pak.Addint32(c.c_data.dwDisposition);
            pak.Addint32(0x00000000);
            pak.Addint32(c.c_data.dwReputation);
            pak.Addbyte(0);
            for (int i = 0x2a; i < 0x49; i++)
            {
                Item item = c.GetSlotByPosition(i).c_item;
                if (item == null)
                    pak.Addint32(0);
                else
                {
                    pak.Addbyte(item.dwRefine);
                    pak.Addbyte(0);
                    pak.Addbyte(item.dwElement);
                    pak.Addbyte(item.dwEleRefine);
                }
            }
            for (int i = 0; i < 28; i++)
                pak.Addint32(0);
            pak.Addbyte(c.c_data.EquipsItemCount);
            for (int i = 0; i < 73; i++)
            {
                Slot slot = c.c_data.inventory[i];
                if (slot.dwPos >= 0x2A && slot.c_item != null)
                {
                    pak.Addbyte(slot.dwPos - 0x2A);
                    pak.Addint16(slot.c_item.dwItemID);
                    pak.Addbyte(0);
                }
            }
            pak.Send(this);
        }
        public void SendPlayerAttribRaise(int attributeID, int amount,int csitembonus)
        {
            if (amount == 0) // we don't do < 1 because we want negative numbers too
                return;
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_RAISE_ATTRIBUTE);
            pak.Addint32(attributeID);
            pak.Addint32(amount);
            pak.Addint32(csitembonus);
            pak.Send(this);
            SendToVisible(pak);
        }
        public void SendPlayerAttribDecrease(int attributeID, int amount) // by exos
        {
            if (attributeID == 0 || amount == 0)
                return;
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_DECREASE_ATTRIBUTE);
            pak.Addint32(attributeID);
            pak.Addint32(amount);
            pak.Send(this);
            SendToVisible(pak);
        }
        public void SendPlayerTransy(int mobID)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, 0xF5);
            pak.Addint32(mobID);
            pak.Send(this);
            SendToVisible(pak);
        }
        public void SendPlayerCheerData()
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_UDPATE_CHEER);
            pak.Addint32(c_data.dwCheerPoints);
            pak.Addint32((int)(timers.nextCheer - DLL.time()) * 1000);
            pak.Send(this);
        }
        public void SendPlayerMapTransfer()
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_WORLD_TELEPORT);
            pak.Addint32(c_data.dwMapID);
            pak.Addfloat(c_position.x);
            pak.Addfloat(c_position.y);
            pak.Addfloat(c_position.z);
            pak.Send(this);
            playerSpawned = false;
            SendPlayerSpawnSelf();
            playerSpawned = true;
        }
        public void SendPlayerStatPoints()
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_UPDATE_AP);
            pak.Addint32(c_data.dwStatPoints);
            pak.Addint16(0x0000);
            pak.Addbyte(0x00);
            pak.Send(this);
        }
        public void SendPlayerNewJob()
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_UPDATE_JOB);
            pak.Addint32(c_data.dwClass);
            int last = 0;
            for (int i = 0; i < c_data.skills.Count; i++)
            {
                Skill s = c_data.skills[i];
                pak.Addint32(s.dwSkillID);
                pak.Addint32(s.dwSkillLevel);
                last = i;
            }
            for (int i = last; i < 45; i++)
            {
                pak.Addint32(-1);
                pak.Addint32(0);
            }
            pak.Send(this);
            /*
             * TODO add skills:
             * for(int i=0;i<45;i++){
             * pak.Addint32(skillid)
             * pak.Addint32(skilllevel);
             * }
             */
            SendToVisible(pak);
        }

        public void SendPlayerResurrectionOffer(Skills skills, Client target, Client user)
        {
            /*Packet pak = new Packet();
            pak.StartNewMergedPacket(user.dwMoverID, 0x19);
            pak.Addint32(skills.dwNameID);
            pak.Addint32(skills.dwSkillLvl);
            pak.Addint32(target.dwMoverID);
            pak.Addint32(0);
            pak.Addint32(3);
            pak.Send(this);*/
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_RESURRECT_OFFER);
            //pak.Addint(0x99); //don't need of this but i have it...
            pak.Send(this);
            
        }
        public void SendPlayerResurrectScreenRemoval()
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_RES_SCREEN_REMOVAL);
            pak.Send(this);
        }

        public void SendPlayerNameChange()
        {
            Packet pak = new Packet();
            pak.Addint32(PAK_NAME_CHANGE);
            pak.Addint32(c_data.dwCharacterID);
            pak.Addstring(c_data.strPlayerName);
            SendToAll(pak);
        }
        public void SendPlayerSpawnSelf()
        {
            Item currentItem;
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_MOVER_SPAWN, 0x0000FF00);
            pak.Addbyte(0x05);
            pak.Addint32(0xb + c_data.dwGender); // model
            pak.Addbyte(0x05);
            pak.Addint16(0xb + c_data.dwGender); //model
            pak.Addint16(dwMoverSize);
            pak.Addfloat(c_position.x);
            pak.Addfloat(c_position.y);
            pak.Addfloat(c_position.z);
            pak.Addint16((short)(c_position.angle * 50)); // angle*50
            pak.Addint32(dwMoverID);
            pak.Addint16(0x0000);
            pak.Addbyte(0x01);
            pak.Addint32(c_attributes[FlyFF.DST_HP]);
            pak.Addint32(c_data.m_MovingFlags);
            pak.Addint32(c_data.m_MotionFlags);
            pak.Addbyte(0x01);
            pak.Addstring(c_data.strPlayerName);
            pak.Addbyte(c_data.dwGender);
            pak.Addbyte(0x00);
            pak.Addbyte(c_data.dwHairID);
            pak.Addint32(c_data.c_haircolor.dword);
            pak.Addbyte(c_data.dwFaceID);
            pak.Addint32(c_data.dwCharacterID);
            pak.Addbyte(c_data.dwClass);
            pak.Addint16(c_attributes[DST_STR]);
            pak.Addint16(c_attributes[DST_STA]);
            pak.Addint16(c_attributes[DST_DEX]);
            pak.Addint16(c_attributes[DST_INT]);
            pak.Addint16(c_data.dwLevel);
            pak.Addint32(c_data.dwGuildID > 0 ? 0xbb8 : -1);
            pak.Addint32(0x00000000);
            pak.Addbyte(c_data.dwGuildID > 0 ? 1 : 0);
            if (c_data.dwGuildID > 0)
            {
                Guild guild = GuildHandler.getGuildByGuildID(c_data.dwGuildID);
                if (guild == null)
                {
                    Log.Write(Log.MessageType.error, "Unknown guild ID error - guild ID {0} does not exist?", c_data.dwGuildID);
                    pak.Addint64(0);
                }
                else
                {
                    pak.Addint32(c_data.dwGuildID);
                    pak.Addint32(guild.duelInfo.dwDuelID); // TODO: Guild duel ID
                }
            }
            pak.Addint32(0x00000000);
            pak.Addbyte(0x00);
            pak.Addbyte(c_data.dwAuthority);
            pak.Addint32(c_data.m_PlayerFlags);
            pak.Addint32(0x00000000);
            pak.Addint32(0x1f8);
            pak.Addint32(0x00000000);
            pak.Addint32(c_data.dwKarma);
            pak.Addint32(c_data.dwDisposition);
            pak.Addint32(0x00000000);
            pak.Addint32(c_data.dwReputation);
            pak.Addbytes(StringUtilities.GetByteFromHex("0000000000000000000800000000000000080000000800000008000000000000000000000000000000070004080000000000000000000000000000000000000000000000000000000000000000140000000500000005000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000"));
            pak.Addint16(c_attributes[FlyFF.DST_MP]);
            pak.Addint16(c_attributes[FlyFF.DST_FP]);
            pak.Addint16(c_data.dwFlyLevel);
            pak.Addint32(c_data.dwFlyEXP);
            pak.Addint32(c_data.dwPenya);
            pak.Addint64(c_data.qwExperience);
            pak.Addint32(c_data.dwLevel);
            pak.Addint32(c_data.dwSkillPoints);
            pak.Addint64(c_data.qwExperience);
            pak.Addint32(c_data.dwLevel);
            for (int i = 0; i < 32; i++)
                pak.Addint32(0x00000000);
            pak.Addint32(1);
            pak.Addfloat(c_position.x);
            pak.Addfloat(c_position.y);
            pak.Addfloat(c_position.z);
            // Quests
            pak.Addbyte(0x00); // Monster hunting quests amount
            pak.Addbyte(0x00); // Regular quests amount
            pak.Addint32(0x00000000);
            pak.Addint32(c_data.dwStatPoints);
            pak.Addbytes(StringUtilities.GetByteFromHex("ffffffffffffffff2c000000ffffffff2e0000002f00000030000000ffffffffffffffff3300000034000000ffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff4400000045000000ffffffff47000000ffffffff"));
            int[] tree = SkillTree.GetTree(c_data.dwClass);
            for (int i = 0; i < tree.Length; i++)
            {
                Skill curSkill = GetSkillByID(tree[i]);
                pak.Addint32(curSkill.dwSkillID);
                pak.Addint32(curSkill.dwSkillLevel);
            }
            /*
            for (int i = 0; i < c_data.skills.Count; i++)
            {
                Skill s = c_data.skills[i];
                pak.Addint32(s.dwSkillID);
                pak.Addint32(s.dwSkillLevel);
            }*/
            for (int i = c_data.skills.Count; i < 45; i++)
            {
                pak.Addint32(-1);
                pak.Addint32(0x00000000);
            }
           
            pak.Addbyte(c_data.dwCheerPoints);
            pak.Addint32(0x36EE80);
            pak.Addbyte(c_data.dwCharSlot);
            for (int i = 0; i < 3; i++)
                pak.Addint32(c_data.bank.dwPenyaArr[i]);
            for (int i = 0; i < 3; i++)
                pak.Addint32(c_data.bank.dwCharacterIDArr[i]);
            pak.Addbytes(StringUtilities.GetByteFromHex("00000000ffffff000000000000000000000000"));
            for (int i = 0; i <= 0x48; i++)
                if (i < 0x2a)
                    pak.Addint32(i);
                else
                    if (SlotFree(i))
                        pak.Addint32(-1);
                    else
                        pak.Addint32(i);
            int count = c_data.ItemCount;
            pak.Addbyte(count);
            for (int i = 0; i < c_data.inventory.Count; i++)
            {
                Slot slot = c_data.inventory[i];
                if (slot.c_item != null)
                {
                    pak.Addbyte(slot.dwPos);
                    pak.Addint32(slot.dwID);
                    pak.AddItemData(slot.c_item);
                }
            }
            pak.Addbytes(StringUtilities.GetByteFromHex("000000000100000002000000030000000400000005000000060000000700000008000000090000000a0000000b0000000c0000000d0000000e0000000f000000100000001100000012000000130000001400000015000000160000001700000018000000190000001a0000001b0000001c0000001d0000001e0000001f000000200000002100000022000000230000002400000025000000260000002700000028000000290000002a0000002b0000002c0000002d0000002e0000002f000000300000003100000032000000330000003400000035000000360000003700000038000000390000003a0000003b0000003c0000003d0000003e0000003f00000040000000410000004200000043000000440000004500000046000000ffffffffffffffff"));
            // bank
            for (int l = 0; l < 3; l++)
            {
                for (int i = 0; i < 0x2a; i++)
                    pak.Addint32(i);

                pak.Addbyte(c_data.bank.bankItems[l].Count); // amount of items
                for (int j = 0; j < c_data.bank.bankItems[l].Count; j++)
                {
                    Item curItem = c_data.bank.bankItems[l][j];
                    pak.Addbyte(j);
                    pak.Addint32(j);
                    pak.AddItemData(curItem);
                    /*
                    pak.Addint32(curItem.dwItemID);
                    pak.Addint32(0);
                    pak.Addint32(0);
                    pak.Addint16(curItem.dwQuantity);
                    pak.Addbyte(0);
                    pak.Addint64(0x6DDD00);//normally it's endurance but no need
                    pak.Addbyte(0);
                    pak.Addint32(curItem.dwRefine);
                    pak.Addint32(0);
                    pak.Addbyte((byte)curItem.dwElement);
                    pak.Addint32(curItem.dwEleRefine);
                    pak.Addint32(0);
                    pak.Addbyte(curItem.c_sockets.Length);//number of socket
                    for (int i = 0; i < curItem.c_sockets.Length; i++)
                    {
                        pak.Addint16(curItem.c_sockets[i]);
                    }
                    pak.Addbyte(0);
                    pak.Addint32(0);
                    pak.Addint32(0);
                    pak.Addint32(0);
                    pak.Addint32(0);*/

                }
                for (int i = 0; i < 0x2a; i++)
                    pak.Addint32(i);
            }
            pak.Addint32(-1); // TODO: Figure out pet buff shit

            pak.Addbyte(0x01);
            for (int i = 0; i < 6; i++)
                pak.Addint32(i);
            //#mark
            //itemCount = c_data.basebag.items.Count;
            pak.Addbyte(0);//itemCount);
            //#mark
            /*for (int i = 0; i < itemCount; i++)
            {
                currentItem = c_data.basebag.items[i];
                pak.Addbyte(currentItem.itemslot);
                pak.Addint32(currentItem.itemslot);
                pak.AddItemData(currentItem);
            }*/
            for (int i = 0; i < 6; i++)
                pak.Addint32(i);

            pak.Addint32(0x00000000);
            pak.Addint32(0x00000000);
            // Bag 1
            //bool e = c_data.bag1.isEnabled;
            //#mark
            //pak.Addbyte(e ? 1 : 0);
            pak.Addbyte(0);/*
            if (e)
            {
                for (int i = 0; i < 24; i++)
                    pak.Addint32(i);
                itemCount = c_data.bag1.items.Count;
                pak.Addbyte(itemCount);
                for (int i = 0; i < itemCount; i++)
                {
                    currentItem = c_data.bag1.items[i];
                    pak.Addbyte(currentItem.itemslot);
                    pak.Addint32(currentItem.itemslot);
                    pak.AddItemData(currentItem);
                }
                for (int i = 0; i < 24; i++)
                    pak.Addint32(i);
                pak.Addint32(0x00000000);
                pak.Addint32((int)DLL.time(c_data.bag1.lastuntil));
                pak.Addint32((int)(DLL.time(c_data.bag1.lastuntil) - DLL.time()));
            }
            e = c_data.bag2.isEnabled;*/
            pak.Addbyte(0);//e ? 1 : 0);
            /*
            if (e)
            {
                for (int i = 0; i < 24; i++)
                    pak.Addint32(i);
                pak.Addbyte(c_data.bag2.items.Count);
                for (int i = 0; i < itemCount; i++)
                {
                    currentItem = c_data.bag2.items[i];
                    pak.Addbyte(currentItem.itemslot);
                    pak.Addint32(currentItem.itemslot);
                    pak.AddItemData(currentItem);
                }
                for (int i = 0; i < 24; i++)
                    pak.Addint32(i);
                pak.Addint32(0x00000000);
                pak.Addint32((int)DLL.time(c_data.bag1.lastuntil));
                pak.Addint32((int)(DLL.time(c_data.bag1.lastuntil) - DLL.time()));
            }*/
            pak.Addint32(0x00000000);

            // Buffs...
            //pak.Addint32(0x00000000); // will get to it in another time, for now this=0
            //qwExpirationDate = DLL.clock() + dwTime

            pak.Addint32(c_data.buffs.Count); // Buffs + any other CS items (probably)
            for (int i = 0; i < c_data.buffs.Count; i++)
            {
                pak.Addint16(1);            // Buff type (skill/item/pet) | 1 = skill
                pak.Addint16(c_data.buffs[i]._skill.dwNameID);
                pak.Addint32(c_data.buffs[i]._skill.dwSkillLvl);
                pak.Addint32(c_data.buffs[i].qwExpirationDate - DLL.clock());
            }

            pak.StartNewMergedPacket(dwMoverID, PAK_GAME_TIME);
            //try this : int unixTime = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds; later [Divinepunition]
            pak.Addbytes(StringUtilities.GetByteFromHex("e500000009a1a748"));  // Game time?!
           
            pak.StartNewMergedPacket(dwMoverID, PAK_SEND_HOTSLOTS);
            pak.Addint32(c_data.hotslots.Count);
            for (int i = 0; i < c_data.hotslots.Count; i++)
            {
                Hotslot cur = c_data.hotslots[i];
                pak.Addint32(cur.dwSlot);
                pak.Addint32(cur.dwOperation);
                pak.Addint32(cur.dwID);
                for (int l = 0; l < 4; l++)
                    pak.Addint32(0x00000000);
                if (cur.dwOperation == 0x08)
                    pak.Addstring(cur.strText);
            }

            pak.Addint32(c_data.keybinds.Count);
            for (int i = 0; i < c_data.keybinds.Count; i++)
            {
                Keybind cur = c_data.keybinds[i];
                pak.Addint32(cur.dwPageIndex);
                pak.Addint32(cur.dwKeyIndex);
                pak.Addint32(cur.dwOperation);
                pak.Addint32(cur.dwID);
                for (int l = 0; l < 3; l++)
                    pak.Addint32(0x00000000);
                pak.Addint32(0x00000001);
                if (cur.dwOperation == 0x08)
                    pak.Addstring(cur.strText);
            }
            int number = 0;
            for (int i = 0; i < 5; i++) { if (c_data.actionslot_option[i] != 0) number++; }
            pak.Addint32(number);
            for (int i = 0; i < number; i++)
            {
                pak.Addint32(i);
                pak.Addint32(c_data.actionslot_option[i]);
                pak.Addint32(c_data.actionslot[i]);
                pak.Addint32(0);
                pak.Addint32(i);
                pak.Addint32(0);
                pak.Addint32(2);
            }
            // Hotslot data comes in here
            pak.Addint32(100);
            
            //--------------------------------

            pak.StartNewMergedPacket(-1, (uint)Server.weather);
            pak.Addint64(Server.weather == 0x60 ? 0x0000000000000000 : 0x0000000000000001);

            for (int i = 0; i < c_data.friends.Count; i++)
            {
                Friend friend = c_data.friends[i];
                pak.StartNewMergedPacket(-1, PAK_ADD_FRIENDDATA);
                pak.Addint32(friend.dwCharacterID);
                pak.Addstring(friend.strPlayerName);
                pak.Addbyte(friend.dwClass);
                pak.Addbyte(friend.dwLevel);
                pak.Addbyte(friend.dwGender);
                pak.Addbyte(0x00); //??
                if (friend.bOnline)
                    pak.Addint32(friend.dwNetworkStatus);
                else
                    pak.Addint32(0x00000001);
                pak.Addint32(0x00000000);
            }

            pak.StartNewMergedPacket(dwMoverID, PAK_FRIEND_MSNSTATE);
            pak.Addint32(c_data.dwNetworkStatus);
            pak.Addint32(c_data.friends.Count);

            for (int i = 0; i < c_data.friends.Count; i++)
            {
                Friend friend = c_data.friends[i];
                pak.Addint32(friend.dwCharacterID);
                pak.Addint32(friend.nBlocked);
                if (friend.bOnline)
                    pak.Addint32(friend.dwNetworkStatus);
                else
                    pak.Addint32(0x00000001);
            }

            pak.StartNewMergedPacket(dwMoverID, PAK_PARTY_DEFAULT_NAME);
            pak.Addstring("None"); // <--- party default name (I think)

            // OnGameJoin
            pak.StartNewMergedPacket(dwMoverID, PAK_GAME_JOIN);
            pak.Addint32(0xD6);


            pak.StartNewMergedPacket(-1, PAK_GAMESERVER_SETTINGS);


            for (int i = 0; i < 1024; i++)
                pak.Addbyte(Server.events[i]);

            pak.StartNewMergedPacket(-1, PAK_NEW_MAILS_MESSAGE);
            int mailunread = 0;
            for (int i = 0; i < c_data.receivedmails.Count; i++)
            {
                Mails mails = c_data.receivedmails[i];
                if (mails.isRead !=0)
                    mailunread++;
            }
            pak.Addint16(mailunread); //number of mail not read

            pak.StartNewMergedPacket(-1, 0x3E);
            pak.Addint16(0);
            // Game rates..?
            pak.StartNewMergedPacket(dwMoverID, PAK_GAME_RATES);
            pak.Addfloat(1);
            pak.Addbyte(0x00);
            for (int i = 1; i <= 3; i++)
            {
                pak.StartNewMergedPacket(dwMoverID, PAK_GAME_RATES);
                pak.Addfloat(i);
                pak.Addbyte(0x10 + i);
            }
            pak.StartNewMergedPacket(dwMoverID, PAK_UNKNOWN_4E);
            pak.Addint32(0x00000000);
            pak.Addint32(0x00000000);
            pak.StartNewMergedPacket(dwMoverID, 0xB7);
            pak.Addint32(0x00000001);
            pak.Addint32(0x00000000);
            pak.StartNewMergedPacket(dwMoverID, 0xB8);
            pak.Addbyte(0x30);
            pak.Addint32(0x00000001);
            pak.Addint32(0x00000069);
            pak.StartNewMergedPacket(dwMoverID, 0xB8);
            pak.Addbyte(0x31);
            pak.Addint32(0x800c17ac);
            pak.Addint32(0x00000000);
            pak.StartNewMergedPacket(dwMoverID, 0xB8);
            pak.Addbyte(0x00);
            pak.Addint32(0x0000004d);
            pak.Addint32(0x00000702);
            pak.Addint32(2);
            pak.StartNewMergedPacket(dwMoverID, 0xB8);
            pak.Addbyte(0x07);
            pak.Addint32(0x0001577a);
            pak.StartNewMergedPacket(dwMoverID, 0xB8);
            pak.Addbyte(0x08);
            pak.Addint32(0x00000000);     

            pak.Send(this);
        }
        public void SendPlayerAttribSet(int attributeID, int count)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_SET_ATTRIBUTE);
            pak.Addint32(attributeID);
            pak.Addint32(count);
            pak.Send(this);
            SendToVisible(pak);
        }
        public void CreateShiningsunstonevalid()
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_NPC_CREATERESULT);
            pak.Addint32(1);
            pak.Send(this);
        }
        public void SendPlayerPenya()
        {
            SendPlayerAttribSet(10000, c_data.dwPenya);
        }
        public void SendPlayerBuff(Buff buff)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_BUFF);
            pak.Addint32(dwMoverID);
            pak.Addint16(1);
            pak.Addint16(buff._skill.dwNameID);
            pak.Addint32(buff._skill.dwSkillLvl);
            pak.Addint32(buff.dwTime);
            pak.Send(this);
            SendToVisible(pak);
        }
        public void SendPlayerBuffByCSItem(Item item)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_BUFF);
            pak.Addint32(dwMoverID);
            pak.Addint16(0);
            pak.Addint32(item.dwItemID);
            pak.Addint16(0);
            pak.Addint32(item.Data.skillTime);
            pak.Send(this);
            SendToVisible(pak);
            
        }
        public void SendPlayerBuffByCSItem(Item item,int timeremaining)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_BUFF);
            pak.Addint32(dwMoverID);
            pak.Addint16(0);
            pak.Addint32(item.dwItemID);
            pak.Addint16(0);
            pak.Addint32(timeremaining);
            pak.Send(this);
            SendToVisible(pak);

        }
        public void SendPlayerRemoveBuffByItem(int itemid)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_REMOVE_ACTIVATEDITEM);
            pak.Addint16(0);
            pak.Addint32(itemid);
            pak.Addint32(0);
            pak.Addint32(0x089d);
            pak.Send(this);
        }
        public void SendPlayerTargetStateInfos(int target_mID, Buff antibuff)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_STUN_STATE);
            pak.Addint32(target_mID);
            pak.Addint32(antibuff._skill.dwNameID);
            pak.Addint32(antibuff._skill.dwSkillLvl);
            pak.Send(this);
        }

        public void SendPlayerSkillMotion(Skills skill, int unknownvalue)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_SKILL_MOTION);
            pak.Addint32(skill.dwNameID);
            pak.Addint32(skill.dwSkillLvl);
            pak.Addint32(dwMoverID);
            pak.Addint32(NULL);    //0 not action slot 1 = action slot ?
            pak.Addint32(unknownvalue);
            
            pak.Send(this);
            SendToVisible(pak);
        }
        public void SendPlayerSkillMotion(Skills skill, Mover c,int actionslot, int unknownvalue)
        {
            if (c == null)
            {
                SendPlayerSkillMotion(skill, unknownvalue);
                return;
            }
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_SKILL_MOTION);
            pak.Addint32(skill.dwNameID);
            pak.Addint32(skill.dwSkillLvl);
            pak.Addint32(c.dwMoverID);                     
            pak.Addint32(actionslot);
            pak.Addint32(unknownvalue);
            //pak.Addbyte(NULL);
            pak.Send(this);
            SendToVisible(pak);
        }
        public void SendPlayerSkillUnknowpacket()
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_UNKOWNSKILLRELATED);
            pak.Addint32(-1);
            pak.Send(this);
        }
        public void SendActionSlotGrey(int actionslotremaining)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_ACTIONSLOT_GREY);
            pak.Addint32(actionslotremaining);
            pak.Send(this);
        }
        public void SendPlayerSkillEnd()
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_SKILLEND);
            pak.Send(this);
        }
                
        public void SendPlayerEquiping(Slot slot, int to, bool equiping)
        {
            
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_MOVE_EQUIP);
            pak.Addbyte(slot.dwID);
            pak.Addint32(NULL);
            pak.Addbyte(equiping ? 1 : 0);
            pak.Addint32(slot.c_item.dwItemID);
            pak.Addint16(slot.c_item.dwRefine);
            pak.Addbyte(slot.c_item.dwElement);
            pak.Addbyte(slot.c_item.dwEleRefine);
            pak.Addint32(0);
            pak.Addint32(to);
            pak.Send(this);
            SendToVisible(pak);
        }
        public void SendPlayerFameUpdate()
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_UPDATE_PVP);
            pak.Addint32(c_data.dwReputation);
            pak.Send(this);
            SendToVisible(pak);
        }
    }
}

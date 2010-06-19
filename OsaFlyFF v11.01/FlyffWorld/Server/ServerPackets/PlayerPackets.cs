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
            pak.Addint(c_data.m_PlayerFlags);
            pak.Send(this);
            SendToVisible(pak);
        }
        public void SendPlayerSound(int soundID)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_SOUND);
            pak.Addbyte(0);
            pak.Addint(soundID);
            pak.Send(this);
        }
        public void SendPlayerNewFace()
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_UPDATE_FACE);
            pak.Addint(c_data.dwCharacterID);
            pak.Addint(c_data.dwFaceID);
            pak.Send(this);
            SendToVisible(pak);
        }
        public void SendPlayerNewHair()
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_UPDATE_HAIR);
            pak.Addbyte(c_data.dwHairID);
            pak.Addhex(c_data.c_haircolor.hexstr.Substring(0, 6));
            pak.Send(this);
            SendToVisible(pak);
        }
        public void SendPlayerMusic(int musicID)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_MUSIC);
            pak.Addint(musicID);
            pak.Send(this);
        }
        public void SendPlayerCombatInfo()
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_UPDATE_LEVELGROUP);
            pak.Addlong(c_data.qwExperience);
            pak.Addint(c_data.dwLevel);
            pak.Addshort(0x0000);
            pak.Addint(c_data.dwSkillPoints);
            pak.Send(this);
        }
        public void SendPlayerStats()
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_UPDATE_STATS);
            pak.Addint(c_attributes[DST_STR]);
            pak.Addint(c_attributes[DST_STA]);
            pak.Addint(c_attributes[DST_DEX]);
            pak.Addint(c_attributes[DST_INT]);
            pak.Addint(0);
            pak.Addint(c_data.dwStatPoints);
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
                pak.Addint(curSkill.dwSkillID);
                pak.Addint(curSkill.dwSkillLevel);
            }
            for (; skillCount < 45; skillCount++)
            {
                pak.Addint(-1);
                pak.Addint(0);
            }
            pak.Addint(c_data.dwSkillPoints);
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
            pak.Addint(0xB + c.c_data.dwGender);
            pak.Addbyte(5);
            pak.Addshort(0xB + c.c_data.dwGender);
            pak.Addshort(c.dwMoverSize);
            pak.Addfloat(c.c_position.x);
            pak.Addfloat(c.c_position.y);
            pak.Addfloat(c.c_position.z);
            pak.Addshort(0); // angle
            pak.Addint(c.dwMoverID);
            pak.Addshort(0);
            pak.Addbyte(1);
            pak.Addint(c.c_attributes[FlyFF.DST_HP]);
            pak.Addint(c.c_data.m_MovingFlags);
            pak.Addint(c.c_data.m_MotionFlags);
            pak.Addbyte(1);
            pak.Addstring(c.c_data.strPlayerName);
            pak.Addbyte(c.c_data.dwGender);
            pak.Addbyte(0);
            pak.Addbyte(c.c_data.dwHairID);
            pak.Addint(c.c_data.c_haircolor.dword);
            pak.Addbyte(c.c_data.dwFaceID);
            pak.Addint(c.c_data.dwCharacterID);
            pak.Addbyte(c.c_data.dwClass);
            pak.Addshort(c.c_attributes[DST_STR]);
            pak.Addshort(c.c_attributes[DST_STA]);
            pak.Addshort(c.c_attributes[DST_DEX]);
            pak.Addshort(c.c_attributes[DST_INT]);
            pak.Addshort(c.c_data.dwLevel);
            pak.Addint(c.c_data.dwGuildID > 0 ? 0xBB8 : -1);
            pak.Addint(0x00000000);
            pak.Addbyte(c.c_data.dwGuildID > 0 ? 1 : 0);
            if (c.c_data.dwGuildID > 0)
            {
                Guild guild = GuildHandler.getGuildByGuildID(c.c_data.dwGuildID);
                if (guild == null)
                {
                    Log.Write(Log.MessageType.error, "Unknown guild ID error - guild ID {0} does not exist?", c.c_data.dwGuildID);
                    pak.Addlong(0);
                }
                else
                {
                    pak.Addint(c.c_data.dwGuildID);
                    pak.Addint(guild.duelInfo.dwDuelID); // TODO: Guild duel ID
                }
            }
            pak.Addint(0);
            pak.Addbyte(0);
            pak.Addbyte(c.c_data.dwAuthority);
            pak.Addint(c.c_data.m_PlayerFlags);
            pak.Addhex("00 00 00 00 F8 01 00 00 00 00 00 00");
            pak.Addint(c.c_data.dwKarma);
            pak.Addint(c.c_data.dwDisposition);
            pak.Addint(0x00000000);
            pak.Addint(c.c_data.dwReputation);
            pak.Addbyte(0);
            for (int i = 0x2a; i < 0x49; i++)
            {
                Item item = c.GetSlotByPosition(i).c_item;
                if (item == null)
                    pak.Addint(0);
                else
                {
                    pak.Addbyte(item.dwRefine);
                    pak.Addbyte(0);
                    pak.Addbyte(item.dwElement);
                    pak.Addbyte(item.dwEleRefine);
                }
            }
            for (int i = 0; i < 28; i++)
                pak.Addint(0);
            pak.Addbyte(c.c_data.EquipsItemCount);
            for (int i = 0; i < 73; i++)
            {
                Slot slot = c.c_data.inventory[i];
                if (slot.dwPos >= 0x2A && slot.c_item != null)
                {
                    pak.Addbyte(slot.dwPos - 0x2A);
                    pak.Addshort(slot.c_item.dwItemID);
                    pak.Addbyte(0);
                }
            }
            pak.Send(this);
        }
        public void SendPlayerAttribRaise(int attributeID, int amount)
        {
            if (amount == 0) // we don't do < 1 because we want negative numbers too
                return;
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_RAISE_ATTRIBUTE);
            pak.Addint(attributeID);
            pak.Addint(amount);
            pak.Addint(-1);
            pak.Send(this);
            SendToVisible(pak);
        }
        public void SendPlayerCheerData()
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_UDPATE_CHEER);
            pak.Addint(c_data.dwCheerPoints);
            pak.Addint((int)(timers.nextCheer - DLL.time()) * 1000);
            pak.Send(this);
        }
        public void SendPlayerMapTransfer()
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_WORLD_TELEPORT);
            pak.Addint(c_data.dwMapID);
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
            pak.Addint(c_data.dwStatPoints);
            pak.Addshort(0x0000);
            pak.Addbyte(0x00);
            pak.Send(this);
        }
        public void SendPlayerNewJob()
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_UPDATE_JOB);
            pak.Addint(c_data.dwClass);
            int last = 0;
            for (int i = 0; i < c_data.skills.Count; i++)
            {
                Skill s = c_data.skills[i];
                pak.Addint(s.dwSkillID);
                pak.Addint(s.dwSkillLevel);
                last = i;
            }
            for (int i = last; i < 45; i++)
            {
                pak.Addint(-1);
                pak.Addint(0);
            }
            pak.Send(this);
            /*
             * TODO add skills:
             * for(int i=0;i<45;i++){
             * pak.addint(skillid)
             * pak.addint(skilllevel);
             * }
             */
            SendToVisible(pak);
        }

        public void SendPlayerResurrectionOffer(SkillData skills, Client target, Client user)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(user.dwMoverID, 0x19);
            pak.Addint(skills.dwName);
            pak.Addint(skills.dwSkillLvl);
            pak.Addint(target.dwMoverID);
            pak.Addint(0);
            pak.Addint(3);
            pak.Send(this);
            SendToVisible(pak);
        }

        public void SendPlayerNameChange()
        {
            Packet pak = new Packet();
            pak.Addint(PAK_NAME_CHANGE);
            pak.Addint(c_data.dwCharacterID);
            pak.Addstring(c_data.strPlayerName);
            SendToAll(pak);
        }
        public void SendPlayerSpawnSelf()
        {
            Item currentItem;
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_MOVER_SPAWN, 0x0000FF00);
            pak.Addbyte(0x05);
            pak.Addint(0xb + c_data.dwGender); // model
            pak.Addbyte(0x05);
            pak.Addshort(0xb + c_data.dwGender); //model
            pak.Addshort(dwMoverSize);
            pak.Addfloat(c_position.x);
            pak.Addfloat(c_position.y);
            pak.Addfloat(c_position.z);
            pak.Addshort((short)(c_position.angle * 50)); // angle*50
            pak.Addint(dwMoverID);
            pak.Addshort(0x0000);
            pak.Addbyte(0x01);
            pak.Addint(c_attributes[FlyFF.DST_HP]);
            pak.Addint(c_data.m_MovingFlags);
            pak.Addint(c_data.m_MotionFlags);
            pak.Addbyte(0x01);
            pak.Addstring(c_data.strPlayerName);
            pak.Addbyte(c_data.dwGender);
            pak.Addbyte(0x00);
            pak.Addbyte(c_data.dwHairID);
            pak.Addint(c_data.c_haircolor.dword);
            pak.Addbyte(c_data.dwFaceID);
            pak.Addint(c_data.dwCharacterID);
            pak.Addbyte(c_data.dwClass);
            pak.Addshort(c_attributes[DST_STR]);
            pak.Addshort(c_attributes[DST_STA]);
            pak.Addshort(c_attributes[DST_DEX]);
            pak.Addshort(c_attributes[DST_INT]);
            pak.Addshort(c_data.dwLevel);
            pak.Addint(c_data.dwGuildID > 0 ? 0xbb8 : -1);
            pak.Addint(0x00000000);
            pak.Addbyte(c_data.dwGuildID > 0 ? 1 : 0);
            if (c_data.dwGuildID > 0)
            {
                Guild guild = GuildHandler.getGuildByGuildID(c_data.dwGuildID);
                if (guild == null)
                {
                    Log.Write(Log.MessageType.error, "Unknown guild ID error - guild ID {0} does not exist?", c_data.dwGuildID);
                    pak.Addlong(0);
                }
                else
                {
                    pak.Addint(c_data.dwGuildID);
                    pak.Addint(guild.duelInfo.dwDuelID); // TODO: Guild duel ID
                }
            }
            pak.Addint(0x00000000);
            pak.Addbyte(0x00);
            pak.Addbyte(c_data.dwAuthority);
            pak.Addint(c_data.m_PlayerFlags);
            pak.Addint(0x00000000);
            pak.Addint(0x1f8);
            pak.Addint(0x00000000);
            pak.Addint(c_data.dwKarma);
            pak.Addint(c_data.dwDisposition);
            pak.Addint(0x00000000);
            pak.Addint(c_data.dwReputation);
            pak.Addhex("0000000000000000000800000000000000080000000800000008000000000000000000000000000000070004080000000000000000000000000000000000000000000000000000000000000000140000000500000005000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
            pak.Addshort(c_attributes[FlyFF.DST_MP]);
            pak.Addshort(c_attributes[FlyFF.DST_FP]);
            pak.Addshort(c_data.dwFlyLevel);
            pak.Addint(c_data.dwFlyEXP);
            pak.Addint(c_data.dwPenya);
            pak.Addlong(c_data.qwExperience);
            pak.Addint(c_data.dwLevel);
            pak.Addint(c_data.dwSkillPoints);
            pak.Addlong(c_data.qwExperience);
            pak.Addint(c_data.dwLevel);
            for (int i = 0; i < 32; i++)
                pak.Addint(0x00000000);
            pak.Addint(1);
            pak.Addfloat(c_position.x);
            pak.Addfloat(c_position.y);
            pak.Addfloat(c_position.z);
            // Quests
            pak.Addbyte(0x00); // Monster hunting quests amount
            pak.Addbyte(0x00); // Regular quests amount
            pak.Addint(0x00000000);
            pak.Addint(c_data.dwStatPoints);
            pak.Addhex("ffffffffffffffff2c000000ffffffff2e0000002f00000030000000ffffffffffffffff3300000034000000ffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff4400000045000000ffffffff47000000ffffffff");
            for (int i = 0; i < c_data.skills.Count; i++)
            {
                Skill s = c_data.skills[i];
                pak.Addint(s.dwSkillID);
                pak.Addint(s.dwSkillLevel);
            }
            for (int i = c_data.skills.Count; i < 45; i++)
            {
                pak.Addint(-1);
                pak.Addint(0x00000000);
            }
            pak.Addbyte(c_data.dwCheerPoints);
            pak.Addint(0x36EE80);
            pak.Addbyte(c_data.dwCharSlot);
            for (int i = 0; i < 3; i++)
                pak.Addint(c_data.bank.dwPenyaArr[i]);
            for (int i = 0; i < 3; i++)
                pak.Addint(c_data.bank.dwCharacterIDArr[i]);
            pak.Addhex("00000000ffffff000000000000000000000000");
            for (int i = 0; i <= 0x48; i++)
                if (i < 0x2a)
                    pak.Addint(i);
                else
                    if (SlotFree(i))
                        pak.Addint(-1);
                    else
                        pak.Addint(i);
            int count = c_data.ItemCount;
            pak.Addbyte(count);
            for (int i = 0; i < c_data.inventory.Count; i++)
            {
                Slot slot = c_data.inventory[i];
                if (slot.c_item != null)
                {
                    pak.Addbyte(slot.dwPos);
                    pak.Addint(slot.dwID);
                    pak.AddItemData(slot.c_item);
                }
            }
            pak.Addhex("000000000100000002000000030000000400000005000000060000000700000008000000090000000a0000000b0000000c0000000d0000000e0000000f000000100000001100000012000000130000001400000015000000160000001700000018000000190000001a0000001b0000001c0000001d0000001e0000001f000000200000002100000022000000230000002400000025000000260000002700000028000000290000002a0000002b0000002c0000002d0000002e0000002f000000300000003100000032000000330000003400000035000000360000003700000038000000390000003a0000003b0000003c0000003d0000003e0000003f00000040000000410000004200000043000000440000004500000046000000ffffffffffffffff");
            // bank
            for (int l = 0; l < 3; l++)
            {
                for (int i = 0; i < 0x2a; i++)
                    pak.Addint(i);
                pak.Addbyte(0x00); // amount of items
                /*
                 * if items exist, add them like so:
                 * slot byte
                 * slot int
                 * data (same as inv and eq)
                 */
                for (int i = 0; i < 0x2a; i++)
                    pak.Addint(i);
            }
            pak.Addint(-1); // TODO: Figure out pet buff shit

            pak.Addbyte(0x01);
            for (int i = 0; i < 6; i++)
                pak.Addint(i);
            //#mark
            //itemCount = c_data.basebag.items.Count;
            pak.Addbyte(0);//itemCount);
            //#mark
            /*for (int i = 0; i < itemCount; i++)
            {
                currentItem = c_data.basebag.items[i];
                pak.Addbyte(currentItem.itemslot);
                pak.Addint(currentItem.itemslot);
                pak.AddItemData(currentItem);
            }*/
            for (int i = 0; i < 6; i++)
                pak.Addint(i);

            pak.Addint(0x00000000);
            pak.Addint(0x00000000);
            // Bag 1
            //bool e = c_data.bag1.isEnabled;
            //#mark
            //pak.Addbyte(e ? 1 : 0);
            pak.Addbyte(0);/*
            if (e)
            {
                for (int i = 0; i < 24; i++)
                    pak.Addint(i);
                itemCount = c_data.bag1.items.Count;
                pak.Addbyte(itemCount);
                for (int i = 0; i < itemCount; i++)
                {
                    currentItem = c_data.bag1.items[i];
                    pak.Addbyte(currentItem.itemslot);
                    pak.Addint(currentItem.itemslot);
                    pak.AddItemData(currentItem);
                }
                for (int i = 0; i < 24; i++)
                    pak.Addint(i);
                pak.Addint(0x00000000);
                pak.Addint((int)DLL.time(c_data.bag1.lastuntil));
                pak.Addint((int)(DLL.time(c_data.bag1.lastuntil) - DLL.time()));
            }
            e = c_data.bag2.isEnabled;*/
            pak.Addbyte(0);//e ? 1 : 0);
            /*
            if (e)
            {
                for (int i = 0; i < 24; i++)
                    pak.Addint(i);
                pak.Addbyte(c_data.bag2.items.Count);
                for (int i = 0; i < itemCount; i++)
                {
                    currentItem = c_data.bag2.items[i];
                    pak.Addbyte(currentItem.itemslot);
                    pak.Addint(currentItem.itemslot);
                    pak.AddItemData(currentItem);
                }
                for (int i = 0; i < 24; i++)
                    pak.Addint(i);
                pak.Addint(0x00000000);
                pak.Addint((int)DLL.time(c_data.bag1.lastuntil));
                pak.Addint((int)(DLL.time(c_data.bag1.lastuntil) - DLL.time()));
            }*/
            pak.Addint(0x00000000);

            // Buffs...
            pak.Addint(0x00000000); // will get to it in another time, for now this=0
            pak.StartNewMergedPacket(dwMoverID, PAK_GAME_TIME);
            pak.Addhex("e500000009a1a748");  // Game time?!

            /*
             * Actionslot data:
             * (int)Slot num
             * (int)opcode
             * (int)ID
             * (int)0
             * (int)Slot num
             * (int)0
             * (int)2
             */
            pak.StartNewMergedPacket(dwMoverID, PAK_SEND_HOTSLOTS);
            pak.Addint(c_data.hotslots.Count);
            for (int i = 0; i < c_data.hotslots.Count; i++)
            {
                Hotslot cur = c_data.hotslots[i];
                pak.Addint(cur.dwSlot);
                pak.Addint(cur.dwOperation);
                pak.Addint(cur.dwID);
                for (int l = 0; l < 4; l++)
                    pak.Addint(0x00000000);
                if (cur.dwOperation == 0x08)
                    pak.Addstring(cur.strText);
            }
            pak.Addint(c_data.keybinds.Count);
            for (int i = 0; i < c_data.keybinds.Count; i++)
            {
                Keybind cur = c_data.keybinds[i];
                pak.Addint(cur.dwPageIndex);
                pak.Addint(cur.dwKeyIndex);
                pak.Addint(cur.dwOperation);
                pak.Addint(cur.dwID);
                for (int l = 0; l < 3; l++)
                    pak.Addint(0x00000000);
                pak.Addint(0x00000001);
                if (cur.dwOperation == 0x08)
                    pak.Addstring(cur.strText);
            }
            pak.Addint(0x00000000); // amount of slots in actionslot (1-5)
            // Hotslot data comes in here
            pak.Addint(100);

            pak.StartNewMergedPacket(-1, (uint)Server.weather);
            pak.Addlong(Server.weather == 0x60 ? 0x0000000000000000 : 0x0000000000000001);

            for (int i = 0; i < c_data.friends.Count; i++)
            {
                Friend friend = c_data.friends[i];
                pak.StartNewMergedPacket(-1, PAK_ADD_FRIENDDATA);
                pak.Addint(friend.dwCharacterID);
                pak.Addstring(friend.strPlayerName);
                pak.Addbyte(friend.dwClass);
                pak.Addbyte(friend.dwLevel);
                pak.Addbyte(friend.dwGender);
                pak.Addbyte(0x00); //??
                if (friend.bOnline)
                    pak.Addint(friend.dwNetworkStatus);
                else
                    pak.Addint(0x00000001);
                pak.Addint(0x00000000);
            }

            pak.StartNewMergedPacket(dwMoverID, PAK_FRIEND_MSNSTATE);
            pak.Addint(c_data.dwNetworkStatus);
            pak.Addint(c_data.friends.Count);

            for (int i = 0; i < c_data.friends.Count; i++)
            {
                Friend friend = c_data.friends[i];
                pak.Addint(friend.dwCharacterID);
                pak.Addint(friend.nBlocked);
                if (friend.bOnline)
                    pak.Addint(friend.dwNetworkStatus);
                else
                    pak.Addint(0x00000001);
            }

            pak.StartNewMergedPacket(dwMoverID, PAK_PARTY_DEFAULT_NAME);
            pak.Addstring("None"); // <--- party default name (I think)

            // OnGameJoin
            pak.StartNewMergedPacket(dwMoverID, PAK_GAME_JOIN);
            pak.Addint(0xD6);


            pak.StartNewMergedPacket(-1, PAK_GAMESERVER_SETTINGS);


            for (int i = 0; i < 1024; i++)
                pak.Addbyte(Server.events[i]);

            pak.Addint(-1);
            pak.Addint(0x3E);
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
            pak.Addint(0x00000000);
            pak.Addint(0x00000000);
            pak.Send(this);
        }
        public void SendPlayerAttribSet(int attributeID, int count)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_SET_ATTRIBUTE);
            pak.Addint(attributeID);
            pak.Addint(count);
            pak.Send(this);
            SendToVisible(pak);
        }
        public void SendPlayerPenya()
        {
            SendPlayerAttribSet(10000, c_data.dwPenya);
        }
        public void SendPlayerBuff(Buff buff)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_BUFF);
            pak.Addint(dwMoverID);
            pak.Addshort(1);
            pak.Addshort(buff.buffID);
            pak.Addint(buff.buffLevel);
            pak.Addint(buff.buffTime);
            pak.Send(this);
            SendToVisible(pak);
        }
                
        public void SendPlayerSkillMotion(int skillID)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, 0x19);
            pak.Addint(skillID);
            pak.Addint(1);
            pak.Addint(dwMoverID);
            pak.Addint(NULL);
            pak.Addshort(3);
            pak.Addbyte(NULL);
            pak.Send(this);
            SendToVisible(pak);
        }
        public void SendPlayerSkillMotion(int skillID, Client c)
        {
            if (c == null)
            {
                SendPlayerSkillMotion(skillID);
                return;
            }
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, 0x19);
            pak.Addint(skillID);
            pak.Addint(1);
            pak.Addint(c.dwMoverID);
            pak.Addint(NULL);
            pak.Addshort(3);
            pak.Addbyte(NULL);
            pak.Send(this);
            SendToVisible(pak);
        }
                
        public void SendPlayerEquiping(Slot slot, int to, bool equiping)
        {
            Log.Write(Log.MessageType.debug, "equipRemoveItem()");
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_MOVE_EQUIP);
            pak.Addbyte(slot.dwID);
            pak.Addint(NULL);
            pak.Addbyte(equiping ? 1 : 0);
            pak.Addint(slot.c_item.dwItemID);
            pak.Addshort(slot.c_item.dwRefine);
            pak.Addbyte(slot.c_item.dwElement);
            pak.Addbyte(slot.c_item.dwEleRefine);
            pak.Addint(0);
            pak.Addint(to);
            pak.Send(this);
            SendToVisible(pak);
        }
        public void SendPlayerFameUpdate()
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_UPDATE_PVP);
            pak.Addint(c_data.dwReputation);
            pak.Send(this);
            SendToVisible(pak);
        }
    }
}

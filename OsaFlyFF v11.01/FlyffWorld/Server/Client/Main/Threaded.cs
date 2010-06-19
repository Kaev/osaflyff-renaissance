using System;
using System.Collections.Generic;
using System.Text;

namespace FlyffWorld
{
    public partial class Client
    {
        public void OnHealTimer()
        {
            timers.nextRecovery = DLL.time() + 10;
            if (c_attributes[FlyFF.DST_HP] == 0)
                return; // don't heal while dead
            if (bIsFighting)
                return;
            int[] l = new int[] { c_data.f_RecoveryHP, c_data.f_RecoveryMP, c_data.f_RecoveryFP };
            if (l[0] + c_attributes[FlyFF.DST_HP] > c_data.f_MaxHP)
                l[0] = c_data.f_MaxHP - c_attributes[FlyFF.DST_HP];
            if (l[1] + c_attributes[FlyFF.DST_MP] > c_data.f_MaxMP)
                l[1] = c_data.f_MaxMP - c_attributes[FlyFF.DST_MP];
            if (l[2] + c_attributes[FlyFF.DST_FP] > c_data.f_MaxFP)
                l[2] = c_data.f_MaxFP - c_attributes[FlyFF.DST_FP];
            c_attributes[FlyFF.DST_HP] += l[0];
            c_attributes[FlyFF.DST_MP] += l[1];
            c_attributes[FlyFF.DST_FP] += l[2];
            SendPlayerAttribRaise(DST_HP, l[0]);
            SendPlayerAttribRaise(DST_MP, l[1]);
            SendPlayerAttribRaise(DST_FP, l[2]);
        }
        public void OnCheerTimer()
        {
            if (c_data.dwCheerPoints > 2)
                return;
            c_data.dwCheerPoints++;
            timers.nextCheer = DLL.time() + 3600;
            SendPlayerCheerData();
        }
        
        
        // Save method splitting suggestion by divinepunition @ ragezone (http://forum.ragezone.com/members/divinepunition-550683.html)
        public void SaveData()
        {
            if (c_data.dwLevel >= 20)
                c_data.dwFlyLevel = Math.Max(1, c_data.dwFlyLevel);
            string query = "UPDATE flyff_characters SET ";
            query += "flyff_jobid={0},flyff_hairstyle={1},flyff_haircolor='{2}',flyff_facemodel={3},flyff_gender={4},";
            query += "flyff_strength={5},flyff_stamina={6},flyff_dexterity={7},flyff_intelligence={8},flyff_skillpoints={9},";
            query += "flyff_statpoints={10},flyff_level={11},flyff_experiencepoints={12},flyff_mapid={13},flyff_positionx={14},";
            query += "flyff_positiony={15},flyff_positionz={16},flyff_angle={17},flyff_penya={18},flyff_flyinglevel={19},";
            query += "flyff_flyingexp={20},flyff_currenthitpts={21},flyff_currentmanapts={22},flyff_currentforcepts={23},";
            query += "flyff_charactersize={24},flyff_pvppoints={25},flyff_pkpoints={26},flyff_disposition={27},flyff_guildid={28},";
            query += "`flyff_bankpenya:0`={29},`flyff_bankpenya:1`={30},`flyff_bankpenya:2`={31},flyff_bankpassword='{32}',";
            query += "flyff_msgState={33},flyff_lang ={34}";
            query += " WHERE flyff_characterid=" + c_data.dwCharacterID;
            Database.Execute(query,
                c_data.dwClass,
                c_data.dwHairID,
                c_data.c_haircolor.hexstr,
                c_data.dwFaceID,
                c_data.dwGender,
                c_attributes[DST_STR],
                c_attributes[DST_STA],
                c_attributes[DST_DEX],
                c_attributes[DST_INT],
                c_data.dwSkillPoints,
                c_data.dwStatPoints,
                c_data.dwLevel,
                c_data.qwExperience,
                c_data.dwMapID,
                (int)c_position.x,
                 (int)c_position.y,
                 (int)c_position.z,
                0,              // TODO: angle
                c_data.dwPenya,
                c_data.dwFlyLevel,
                c_data.dwFlyEXP,
                c_attributes[DST_HP],
                c_attributes[DST_MP],
                c_attributes[DST_FP],
                dwMoverSize,
                c_data.dwReputation,
                c_data.dwKarma,
                c_data.dwDisposition,
                c_data.dwGuildID,
                c_data.bank.dwPenyaArr[0],
                c_data.bank.dwPenyaArr[1],
                c_data.bank.dwPenyaArr[2],
                c_data.bank.strPassword,
                c_data.dwNetworkStatus,
                c_data.language);
        }
        public void SaveSkills()
        {
            Database.Execute("DELETE FROM flyff_skills WHERE flyff_characterid={0}", c_data.dwCharacterID);
            for (int i = 0; i < c_data.skills.Count; i++)
            {
                Skill curSkill = c_data.skills[i];
                Database.Execute("INSERT INTO flyff_skills (`flyff_characterid`,`flyff_skillid`,`flyff_skilllevel`) VALUES ({0},{1},{2})", c_data.dwCharacterID, curSkill.dwSkillID, curSkill.dwSkillLevel);
            }
        }
        public void SaveBuffs()
        {
            Database.Execute("DELETE FROM flyff_buffs WHERE flyff_charid={0}", c_data.dwCharacterID);
            for (int i = 0; i < c_data.buffs.Count; i++)
            {
                Buff curBuff = c_data.buffs[i];
                Database.Execute("INSERT INTO flyff_buffs (`flyff_charid`,`flyff_buffid`,`flyff_buffLevel`,`flyff_ability1`,`flyff_abilityType1`,`flyff_ability2`,`flyff_abilityType2`," +
                    "`flyff_buffChangeParam1`,`flyff_buffChangeParam2`,`flyff_probability`,`flyff_probability_PVP`,`flyff_remainingTime`) " +
                    "VALUES ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11})",
                c_data.dwCharacterID,
                curBuff.buffID,
                curBuff.buffLevel,
                curBuff.buffAbility1,
                curBuff.buffAbilityType1,
                curBuff.buffAbility2,
                curBuff.buffAbilityType2,
                curBuff.buffChangeParam1,
                curBuff.buffChangeParam2,
                curBuff.probability,
                curBuff.probability_PVP,
                curBuff.buffTime);
            }
        }
        public void SaveHotslots()
        {
            Database.Execute("DELETE FROM flyff_hotslots WHERE flyff_characterid={0}", c_data.dwCharacterID);
            for (int i = 0; i < c_data.hotslots.Count; i++)
            {
                Hotslot curHotslot = c_data.hotslots[i];
                Database.Execute("INSERT INTO flyff_hotslots (`flyff_characterid`,`flyff_slot`,`flyff_opcode`,`flyff_text`,`flyff_hotslotid`) VALUES ({0},{1},{2},'{3}',{4})", c_data.dwCharacterID, curHotslot.dwSlot, curHotslot.dwOperation, curHotslot.strText, curHotslot.dwID);
            }
        }
        public void SaveGuildMemberData()
        {
            Database.Execute("DELETE FROM flyff_guildmembers WHERE flyff_characterID = {0}", c_data.dwCharacterID);
            if (c_data.dwGuildID > 0)
            {
                GuildMember me = GuildHandler.getGuildMemberByGuildID(c_data.dwGuildID, c_data.dwCharacterID);
                if (me == null)
                    Log.Write(Log.MessageType.error, "GuildHandler.getGuildMemberByGuildID({0}, {1}): returning null when guildid > 0", c_data.dwGuildID, c_data.dwCharacterID);
                else
                    Database.Execute("INSERT INTO flyff_guildmembers " +
                        "(`flyff_guildID`,`flyff_characterID`,`flyff_memberNickname`,`flyff_memberQuestContribution`,`flyff_memberPenyaContribution`,`flyff_memberRank`,`flyff_memberRankSymbolCount`,`flyff_gwForfeits`) " +
                        "VALUES ({0}, {1}, '{2}', {3}, {4}, {5}, {6}, {7})",
                        c_data.dwGuildID,
                        c_data.dwCharacterID,
                        me.memberNickname,
                        me.questContribution,
                        me.penyaContribution,
                        me.memberRank,
                        me.memberRankSymbolCount,
                        me.gwForfeits
                        );
            }
        }
        public void SaveKeybinds()
        {
            Database.Execute("DELETE FROM flyff_keybinds WHERE flyff_characterid={0}", c_data.dwCharacterID);
            for (int i = 0; i < c_data.keybinds.Count; i++)
            {
                Keybind cur = c_data.keybinds[i];
                Database.Execute("INSERT INTO flyff_keybinds (`flyff_characterid`,`flyff_fslot`,`flyff_opcode`,`flyff_text`,`flyff_keybindid`,`flyff_fkey`) VALUES ({0},{1},{2},'{3}',{4},{5})",
                    c_data.dwCharacterID, cur.dwPageIndex, cur.dwOperation, cur.strText, cur.dwID, cur.dwKeyIndex);
            }
        }
        public void SaveFriends()
        {
            Database.Execute("DELETE FROM flyff_friends WHERE flyff_charid1={0}", c_data.dwCharacterID);
            for (int i = 0; i < c_data.friends.Count; i++)
            {
                Friend cur = c_data.friends[i];
                Database.Execute("INSERT INTO flyff_friends (`flyff_charid1`,`flyff_blocked`,`flyff_charid2`) VALUES ({0},{1},{2})", c_data.dwCharacterID, cur.nBlocked, cur.dwCharacterID);
            }
        }
        public void SaveItems()
        {
            Database.Execute("DELETE FROM flyff_items WHERE flyff_characterid={0}", c_data.dwCharacterID);
            for (int i = 0; i < c_data.inventory.Count; i++)
            {
                if (c_data.inventory[i].c_item != null && c_data.inventory[i].dwPos < 0x2A)
                {
                    Item item = c_data.inventory[i].c_item;
                    Database.Execute("INSERT INTO `flyff_items` " +
                        "(`flyff_characterid`,`flyff_slotnum`,`flyff_itemid`,`flyff_itemcount`,`flyff_refinelevel`,`flyff_sockets`,`flyff_elementType`,`flyff_elementRefine`,`flyff_awakening`,`flyff_itemexpired`,`flyff_uniqueid`) " +
                        "VALUES ({0},{1},{2},{3},{4},'{5}',{6},{7},{8},{9},{10})",
                        c_data.dwCharacterID, c_data.inventory[i].dwPos, item.dwItemID, item.dwQuantity, item.dwRefine, item.c_sockets.ToString(), item.dwElement, item.dwEleRefine, item.c_awakening, item.bExpired, c_data.inventory[i].dwID);
                }
            }
        }
        public void SaveEquips()
        {
            Database.Execute("DELETE FROM flyff_equipments WHERE flyff_characterid={0}", c_data.dwCharacterID);
            for (int i = 0; i < c_data.inventory.Count; i++)
            {
                if (c_data.inventory[i].c_item != null && c_data.inventory[i].dwPos > 0x29)
                {
                    Item item = c_data.inventory[i].c_item;
                    Database.Execute("INSERT INTO `flyff_equipments` " +
                        "(`flyff_characterid`,`flyff_slotnum`,`flyff_itemid`,`flyff_itemcount`,`flyff_refinelevel`,`flyff_sockets`,`flyff_elementType`,`flyff_elementRefine`,`flyff_awakening`,`flyff_itemexpired`,`flyff_uniqueid`) " +
                        "VALUES ({0},{1},{2},{3},{4},'{5}',{6},{7},{8},{9},{10})",
                        c_data.dwCharacterID, c_data.inventory[i].dwPos, item.dwItemID, item.dwQuantity, item.dwRefine, item.c_sockets.ToString(), item.dwElement, item.dwEleRefine, item.c_awakening, item.bExpired, c_data.inventory[i].dwID);
                }
            }
        }
       
        public void SaveAll(bool exitingGame)
        {
            SaveData();
            SaveSkills();
            SaveHotslots();
            SaveGuildMemberData();
            SaveKeybinds();
            SaveFriends();
            SaveItems();
            SaveEquips();
            SaveBuffs();
            try
            {
                scrollprotection.SaveData();
            }
            catch
            {
                scrollprotection = new ScrollProtection(c_data.dwCharacterID, false);
                scrollprotection.SaveData();
            }
            if (!exitingGame)
                timers.nextSave = DLL.time() + 120;
            
            
           
        }
        public bool isBuffed = false;

        public void checkBuffTime()
        {
            
            //this function called by worldtimmer each 10s decrease by 10s each buff of player
            //if buff.time = 0 then we delete buff.
            //if player quit game the timer is automatically stop for him
            timers.nextBuffCheck = DLL.time() + 10; //we check in 10s for decreasing buff
            if (c_data.buffs.Count <= 0)
                isBuffed = false; //don't need to check anymore timeremaining if player is not buffed
            else
            {
                for (int i = 0; i < c_data.buffs.Count; i++)
                {
                    Buff curBuff = c_data.buffs[i];
                    if (curBuff.buffTime < 10000) //If time passed equal to time remaining
                    {
                        int[] listBuff = BuffDB.getBuffBonus(curBuff);
                        c_attributes[listBuff[1]] -= listBuff[0]; //we decrease attribut
                        SendPlayerAttribRaise(listBuff[1], -listBuff[0]); //we send it to client

                        if (listBuff[2] != 0 && listBuff[3] != 0) //if buff is GT
                        {
                            c_attributes[listBuff[3]] -= listBuff[2];
                            SendPlayerAttribRaise(listBuff[3], -listBuff[2]);
                        }
                        c_data.buffs.Remove(c_data.buffs[i]);
                        //we delete this buff in buffdatabase
                        Database.Execute("DELETE FROM flyff_buffs WHERE flyff_charid ={0} AND flyff_buffid ={1};", c_data.dwCharacterID, curBuff.buffID);
                    }
                    else if (c_attributes[DST_HP] <= 0) //if player die
                    {
                        //we delete this buff in buffdatabase
                        Database.Execute("DELETE FROM flyff_buffs WHERE flyff_charid ={0} AND flyff_buffid ={1};", c_data.dwCharacterID, curBuff.buffID);

                    }
                    else
                    {
                        curBuff.buffTime -= 10000; //we decrease by 10s each time = 10 000 ms
                        c_data.buffs[i] = curBuff;
                        if (curBuff.buffTime > 10000)
                            Database.Execute("UPDATE flyff_buffs SET flyff_remainingTime = {0} WHERE flyff_charid ={1} AND flyff_buffid ={2};", curBuff.buffTime, c_data.dwCharacterID, curBuff.buffID);
                    }
                }
            }
            
        }
    }
}

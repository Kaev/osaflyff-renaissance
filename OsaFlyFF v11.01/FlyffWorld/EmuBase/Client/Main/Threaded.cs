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
            SendPlayerAttribRaise(DST_HP, l[0],-1);
            SendPlayerAttribRaise(DST_MP, l[1],-1);
            SendPlayerAttribRaise(DST_FP, l[2],-1);
        }
        public void OnCheerTimer()
        {
            if (c_data.dwCheerPoints > 2)
                return;
            c_data.dwCheerPoints++;
            timers.nextCheer = DLL.time() + 3600;
            SendPlayerCheerData();
        }
        
        public void OnActionSlotTimer()
        {
            if (c_data.dwactionslotbar == 100)//no need more
                return;
            else
            {
                c_data.dwactionslotbar += 4;
                
                if (c_data.dwactionslotbar > 100)//can't go more than 100
                    c_data.dwactionslotbar = 100;
                Log.Write(Log.MessageType.debug, "We have {0}% in action slot ", c_data.dwactionslotbar);
            }             

                timers.nextActionSlot = DLL.time()+5;
        }
        // Save method splitting suggestion by divinepunition @ ragezone (http://forum.ragezone.com/members/divinepunition-550683.html)
        public void SaveData()
        {
            string actionslot = String.Concat(c_data.actionslot[0].ToString(), ",", c_data.actionslot[1].ToString(), ",", c_data.actionslot[2].ToString(), ",", c_data.actionslot[3].ToString(), ",", c_data.actionslot[4].ToString());
            string actionslotoption = String.Concat(c_data.actionslot_option[0].ToString(), ",", c_data.actionslot_option[1].ToString(), ",", c_data.actionslot_option[2].ToString(), ",", c_data.actionslot_option[3].ToString(), ",", c_data.actionslot_option[4].ToString());
            

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
            query += "flyff_msgState={33},flyff_actionslotcode='{34}',flyff_actionslotoption='{35}',flyff_actionbar = {36}";
            
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
                actionslot,
                actionslotoption,
                c_data.dwactionslotbar
                );
            Database.Execute("UPDATE flyff_accounts SET flyff_lang={0},flyff_linestatus=-1 WHERE flyff_userid={1}", c_data.dwLanguage, c_data.dwAccountID);
        }
        
        public void SaveBank()
        {
            Database.Execute("DELETE FROM flyff_banks WHERE flyff_characterid={0}", c_data.dwCharacterID);

            for (int j = 0; j < 3; j++)
            {

                for (int i = 0; i < c_data.bank.bankItems[j].Count; i++)
                {
                    Item curItem = c_data.bank.bankItems[j][i];
                    Log.Write(Log.MessageType.debug, "Actual item id {0} and quantity {1}", curItem.dwItemID, curItem.dwQuantity);

                    Database.Execute("INSERT INTO `flyff_banks` " +
                            "(`flyff_characterid`,`flyff_itemid`,`flyff_itemcount`,`flyff_refinelevel`,`flyff_sockets`,`flyff_elementType`,`flyff_elementRefine`,`flyff_awakening`,`flyff_lastuntil`) " +
                            "VALUES ({0},{1},{2},{3},'{4}',{5},{6},{7},{8})", c_data.dwCharacterID, curItem.dwItemID, curItem.dwQuantity, curItem.dwRefine, curItem.c_sockets.ToString(), curItem.dwElement, curItem.dwEleRefine, curItem.c_awakening, curItem.qwLastUntil);
                }
            }
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
        public void SaveMails() 
        {
            Database.Execute("DELETE FROM flyff_mails WHERE flyff_fromcharid={0}", c_data.dwCharacterID);
            
            for (int i = 0; i < c_data.sentmails.Count; i++)
            {
                Mails curMail = c_data.sentmails[i];
                
                //we save mail we have send cuase mail can be received when we areoffline
                //but mail are obliged to b sent when client is online so...
                if (curMail == null)
                    continue;
                //Log.Write(Log.MessageType.debug, "VALUES ({0},{1},{2},{3},{4},{5},{6},'{7}',{8},{9},{10},{11},{12},'{13}','{14}',{15})", curMail.mailid, curMail.fromCharID, curMail.toCharID, curMail.date, curMail.attachedItem.dwItemID, curMail.attachedItem.dwQuantity, curMail.attachedItem.dwRefine, curMail.attachedItem.c_sockets.ToString(), curMail.attachedItem.dwElement, curMail.attachedItem.dwEleRefine, curMail.attachedItem.c_awakening, curMail.attachedItem.qwLastUntil, curMail.attachedPenya, curMail.topic, curMail.message, curMail.isRead);
                Database.Execute("INSERT INTO flyff_mails (`flyff_mailid`,`flyff_fromcharid`,`flyff_tocharid`,`flyff_date`,`flyff_itemid`,`flyff_itemcount`,`flyff_refinelevel`,`flyff_sockets`,`flyff_elementType`,`flyff_elementRefine`,`flyff_awakening`,`flyff_lastuntil`,`flyff_attpenya`,`flyff_topic`,`flyff_message`,`flyff_isRead`) VALUES ({0},{1},{2},{3},{4},{5},{6},'{7}',{8},{9},{10},{11},{12},'{13}','{14}',{15})", curMail.mailid, curMail.fromCharID, curMail.toCharID, curMail.date, curMail.attachedItem.dwItemID, curMail.attachedItem.dwQuantity, curMail.attachedItem.dwRefine, curMail.attachedItem.c_sockets.ToString(), curMail.attachedItem.dwElement, curMail.attachedItem.dwEleRefine, curMail.attachedItem.c_awakening, curMail.attachedItem.qwLastUntil, curMail.attachedPenya, curMail.topic, curMail.message, curMail.isRead);
            }
            Database.Execute("DELETE FROM flyff_mails WHERE flyff_tocharid={0}", c_data.dwCharacterID);
            for (int i = 0; i < c_data.receivedmails.Count; i++)
            {
                Mails curMail = c_data.receivedmails[i];
                
                //we save mail we have send cuase mail can be received when we areoffline
                //but mail are obliged to b sent when client is online so...
                if (curMail == null)
                    continue;
                Database.Execute("INSERT INTO flyff_mails (`flyff_mailid`,`flyff_fromcharid`,`flyff_tocharid`,`flyff_date`,`flyff_itemid`,`flyff_itemcount`,`flyff_refinelevel`,`flyff_sockets`,`flyff_elementType`,`flyff_elementRefine`,`flyff_awakening`,`flyff_lastuntil`,`flyff_attpenya`,`flyff_topic`,`flyff_message`,`flyff_isRead`) VALUES ({0},{1},{2},{3},{4},{5},{6},'{7}',{8},{9},{10},{11},{12},'{13}','{14}',{15})", curMail.mailid, curMail.fromCharID, curMail.toCharID, curMail.date, curMail.attachedItem.dwItemID, curMail.attachedItem.dwQuantity, curMail.attachedItem.dwRefine, curMail.attachedItem.c_sockets.ToString(), curMail.attachedItem.dwElement, curMail.attachedItem.dwEleRefine, curMail.attachedItem.c_awakening, curMail.attachedItem.qwLastUntil, curMail.attachedPenya, curMail.topic, curMail.message, curMail.isRead);
            }

        }
        public void SaveBuffs()
        {
            Database.Execute("DELETE FROM flyff_buffs WHERE flyff_charid={0}", c_data.dwCharacterID);
            for (int i = 0; i < c_data.buffs.Count; i++)
            {
                Buff curBuff = c_data.buffs[i];
                Database.Execute("INSERT INTO flyff_buffs (`flyff_charid`,`flyff_buffid`,`flyff_buffLevel`,`flyff_remainingTime`) " +
                    "VALUES ({0},{1},{2},{3})",
                c_data.dwCharacterID,
                curBuff._skill.dwNameID,
                curBuff._skill.dwSkillLvl,
                curBuff.dwTime);
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
            Database.Execute("DELETE FROM flyff_friends WHERE flyff_charid1={0} OR flyff_charid2={0}", c_data.dwCharacterID);
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
                        "(`flyff_characterid`,`flyff_slotnum`,`flyff_itemid`,`flyff_itemcount`,`flyff_itemcharge`,`flyff_refinelevel`,`flyff_sockets`,`flyff_elementType`,`flyff_elementRefine`,`flyff_awakening`,`flyff_itemexpired`,`flyff_uniqueid`) " +
                        "VALUES ({0},{1},{2},{3},{4},{5},'{6}',{7},{8},{9},{10}, {11})",
                        c_data.dwCharacterID, c_data.inventory[i].dwPos, item.dwItemID, item.dwQuantity, item.dwCharge, item.dwRefine, item.c_sockets.ToString(), item.dwElement, item.dwEleRefine, item.c_awakening, item.bExpired, c_data.inventory[i].dwID);
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
                        "(`flyff_characterid`,`flyff_slotnum`,`flyff_itemid`,`flyff_itemcount`,`flyff_itemcharge`,`flyff_refinelevel`,`flyff_sockets`,`flyff_elementType`,`flyff_elementRefine`,`flyff_awakening`,`flyff_itemexpired`,`flyff_uniqueid`) " +
                        "VALUES ({0},{1},{2},{3},{4},{5},'{6}',{7},{8},{9},{10}, {11})",
                        c_data.dwCharacterID, c_data.inventory[i].dwPos, item.dwItemID, item.dwQuantity, item.dwCharge, item.dwRefine, item.c_sockets.ToString(), item.dwElement, item.dwEleRefine, item.c_awakening, item.bExpired, c_data.inventory[i].dwID);
                }
            }
        }
        public void SaveActiveItem()
        {
            Database.Execute("DELETE FROM flyff_activeitems WHERE flyff_charid={0}", c_data.dwCharacterID);
            for (int i = 0; i < c_data.activateditem.Count; i++)
            {
                if (c_data.activateditem[i] != null)
                {
                    ActiveItems activeitem = c_data.activateditem[i];
                    if (activeitem == null)
                        continue;
                    Database.Execute("INSERT INTO `flyff_activeitems` " +
                        "(`flyff_charid`,`flyff_itemid`,`flyff_lastuntil`,`flyff_remainingtime`) VALUES ({0},{1},{2},{3})",
                        activeitem.charid, activeitem.itemid, activeitem.lastuntil,activeitem.remainingtime);
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
            SaveBank();
            SaveMails();
            SaveActiveItem();
            
            if (!exitingGame)
                timers.nextSave = DLL.time() + 120;
        }
       #region Buff data
        /// <summary>
        /// Thread to check buff state
        /// </summary>
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
                for (int i = c_data.buffs.Count-1; i >=0; i--)
                {
                    Buff curBuff = c_data.buffs[i];
                    if (curBuff.dwTime < 10000) //If time passed equal to time remaining
                    {

                        Buff.ClearABuff(this, curBuff._skill.dwNameID);
                                                
                    }
                    else if (c_attributes[DST_HP] <= 0) //if player die
                    {
                        Buff.ClearAllBuffs(this);
                        isBuffed = false;
                    }
                    else
                    {
                        curBuff.dwTime -= 10000; //we decrease by 10s each time = 10 000 ms
                        c_data.buffs[i] = curBuff;                        
                    }
                }
            }

        }
        
        #endregion
        #region Check Scroll
        /// <summary>
        /// Thread to check scroll remaining time
        /// </summary>
        public void checkScrollTimeRemaining()
        {
            
            //this function called by worldtimmer each 30s decrease by 30s each scroll remainingtime
            //if activeitem.remainingtime = 0 then we delete activeitem from list.
            //But if active item has not remaining time (ticket or permanent scroll) we check if it have a lastuntil value (ticket) et if lastuntil is
            //inferior to actual value item is perimed and deleted from the list
            
            timers.nextScrollReaminingTimeCheck = DLL.time() + 10; //we check in 30s for decreasing remaining time
            for (int i = c_data.activateditem.Count-1; i >=0; i--)
            {
                ActiveItems curactive = c_data.activateditem[i];
                if (curactive == null)
                    continue;
                //of first we need to check if item has a remaining time value
                if (curactive.remainingtime != -1)
                {
                    //ok we must decrease remaining time
                    c_data.activateditem[i].remainingtime -= 10000;
                    //is there any other time left ?
                    if (c_data.activateditem[i].remainingtime <= 0)
                    {
                        ItemData itemData = WorldHelper.GetItemDataByItemID(curactive.itemid);
                        if (itemData.itemID==10209) // Bull Hamstern !!
                        {      
                                    c_attributes[itemData.destAttributes[0]] -= itemData.adjAttributes[1];
                                    SendPlayerAttribDecrease(itemData.destAttributes[0], itemData.adjAttributes[1]);
                                    SendPlayerCSFoodBonus(0);
                                    c_data.activateditem.Remove(curactive);
                                    if (c_attributes[FlyFFAttributes.DST_HP] > c_data.f_MaxHP)
                                          c_attributes[FlyFFAttributes.DST_HP] = c_data.f_MaxHP;                                        
                                   
                                    continue;
                                
                        }
                            switch (itemData.itemkind[0] + itemData.itemkind[1] + itemData.itemkind[2])
                            {
                                case 113:
                                    int TransMobID = 0;
                                    if (c_data.dwGender == 0)
                                        TransMobID = 11;
                                    else
                                        TransMobID = 12;
                                    SendPlayerTransy(TransMobID);
                                    
                                    break;
                                case 124: /* Medicines */
                                case 125: /* Medicines */
                                case 126: /* Medicines */
                                case 127: /* Medicines */
                                case 128: /* Medicines */
                                    c_attributes[itemData.destAttributes[0]] -= itemData.adjAttributes[0];
                                    SendPlayerAttribDecrease(itemData.destAttributes[0], itemData.adjAttributes[0]);
                                                                  
                                    break;
                            }

                            //we delete item effect from list
                            SendPlayerRemoveBuffByItem(curactive.itemid);
                            c_data.activateditem.Remove(curactive);
                        
                       
                    }
                    //if remain some time we follow the flow
                }
                else if ((curactive.lastuntil != -1) && (curactive.lastuntil != 999999999)) //item not illimited and which have a date limit
                {
                    if (DLL.time() > curactive.lastuntil)//item is perimed
                    {
                        //update item status
                        Slot slot = GetSlotByItemID(curactive.itemid);
                        if (slot == null)
                            continue;
                        c_data.inventory[slot.dwID].c_item.bExpired = true;
                        //update list of activated item
                        c_data.activateditem.Remove(curactive);
                    }                
                }
            }
        }

        #endregion
        
    }
}

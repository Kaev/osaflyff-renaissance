using System;
using System.Collections.Generic;
using System.Text;


namespace FlyffWorld
{
    public partial class Client
    {
        #region Delay Variables
        /// <summary>
        /// Last usage of food. (Miliseconds)
        /// </summary>
        public long FoodDelay = 0;
        /// <summary>
        /// Last usage of pill. (Miliseconds)
        /// </summary>
        public long PillDelay = 0;
        #endregion

        #region Process Item Usage
        /// <summary>
        /// Checks if an item is not an equip and acts accordingly.
        /// </summary>
        /// <param name="ItemSlot">Slot of the item.</param>
        /// <returns>True if the item was consumed and is not equip, false if item is equipments.</returns>
        public bool ProcessItemUsage(Slot slot)
        {
            Item item = slot.c_item;
            switch (GetItemType(item))
            {
                #region SpecialScrolls
                case ItemTypes.SpecialScrolls:
                    {
                        ActiveItems activeitem = new ActiveItems(item.dwItemID,c_data.dwCharacterID,item.Data.skillTime,-1);
                        switch (item.Data.itemID)
                        {
                            #region Perin
                            case 26456:
                                {
                                    if (c_data.dwPenya >= 2000000000)
                                    {
                                        SendMessageHud("You have too much penya in your inventory!");
                                        return true;
                                    }
                                    c_data.dwPenya += 100000000;
                                    SendPlayerPenya();
                                    DecreaseQuantity(slot);
                                    return true;
                                }
                                break;
                            #endregion
                            #region Smelting / aprotect / Sprotect/Sprotec2/Xprotec
                            case 10468: //smelting
                            case 26203: //Hsmelting
                            case 26473: //aprotect
                            case 10464: //sprotect
                            case 10487: //sprotect2
                            case 10430: //scroll of blessing
                                {
                                    SendPlayerBuffByCSItem(item);
                                    SendEffect(107);
                                    DecreaseQuantity(slot);
                                    c_data.activateditem.Add(activeitem);
                                    return true;
                                }
                                break;
                            #endregion
                            #region Amplification 
                            case 26205: //ES 20  
                            //case 26206: //ES1 //case 26207: //ES2 you have not to use them directly !
                            case 26208: //ES 40
                            //case 26209: //ES1 case 26210: //ES2 you have not to use them directly !
                            case 26211: //ES 60
                            //case 26212: //ES1 case 26213: //ES2 you have not to use them directly !
                            case 26214: //ES 80
                            //case 26215: //ES1  case 26216: //ES2 you have not to use them directly !                        
                                //first we need to check if player has the correct level
                                switch (item.dwItemID)
                                {
                                    case 26205: //ES 20
                                        if (c_data.dwLevel > 20) return true; break; 
                                    case 26208: //ES 40
                                        if ((c_data.dwLevel > 40) || (c_data.dwLevel < 21)) return true; break; 
                                    case 26211: //ES 60
                                        if ((c_data.dwLevel > 60) || (c_data.dwLevel < 41)) return true; break; 
                                    case 26214: //ES 80
                                        if ((c_data.dwLevel > 80) || (c_data.dwLevel < 61)) return true; break; 
                                     
                                }
                                //ok now we check if player has already a scroll of this type
                                bool differentscrolltype = false;
                                
                                for (int i = 0; i < c_data.activateditem.Count; i++)
                                {
                                    ActiveItems curactive = c_data.activateditem[i];
                                    if (curactive == null)
                                        continue;
                                    if (IsAmplificationScroll(curactive.itemid))
                                    {
                                        if ((curactive.itemid != item.dwItemID) && (curactive.itemid != item.dwItemID + 1) && (curactive.itemid != item.dwItemID + 2))
                                            differentscrolltype = true; //it's a scroll of amplification but another type so we can add this one
                                    }
                                }
                                if (differentscrolltype == true)
                                    return true; //we can add a scroll if another one of another type is here
                                //ok now we check if we can add another one :
                                int count = 0;
                                for (int i = 0; i < c_data.activateditem.Count; i++)
                                {
                                    ActiveItems curactive = c_data.activateditem[i];
                                    if (curactive == null)
                                        continue;
                                    if ((curactive.itemid == item.dwItemID) || (curactive.itemid == item.dwItemID + 1) || (curactive.itemid == item.dwItemID + 2))
                                        count++;
                                }
                                if (count == 1) item.dwItemID += 1;
                                else if (count == 2) item.dwItemID += 2;
                                else if (count == 3) return true;
                                Log.Write(Log.MessageType.debug, "Actuall count value : {0}", count);
                                    SendPlayerBuffByCSItem(item);
                                    SendMessageUsageInfo(1, "Won experience increased by 50%");
                                    activeitem.itemid = item.dwItemID;
                                    activeitem.lastuntil = -1;
                                    activeitem.remainingtime = item.Data.skillTime; //in millisecond
                                    SendEffect(107);
                                    DecreaseQuantity(slot);
                                    c_data.activateditem.Add(activeitem);
                                    return true;
                                
                                break;
                            case 26219: //ES 120
                            case 30148:
                            case 30149:
                            case 30150:
                            case 10473:
                                if (item.dwItemID == 26219 && c_data.dwLevel < 81)
                                    return true;
                                //ok now we check if player has already a scroll of this type
                                differentscrolltype = false;

                                for (int i = 0; i < c_data.activateditem.Count; i++)
                                {
                                    ActiveItems curactive = c_data.activateditem[i];
                                    if (curactive == null)
                                        continue;
                                    if (IsAmplificationScroll(curactive.itemid))
                                        differentscrolltype = true; //we already have one so get out of here
                                    
                                }
                                if (differentscrolltype == true)
                                    return true; //we can add a scroll if another one of another type is here
                                SendPlayerBuffByCSItem(item);
                                SendMessageUsageInfo(1, "Won experience increased by 50%");
                                activeitem.lastuntil = -1;
                                activeitem.remainingtime = item.Data.skillTime; //in millisecond
                                SendEffect(107);
                                DecreaseQuantity(slot);
                                c_data.activateditem.Add(activeitem);
                                return true;
                                break;
                            

                            case 10474: //amplification EM xprate *2 only one
                           
                                {
                                    SendPlayerBuffByCSItem(item);
                                    SendMessageUsageInfo(1, "Won experience increased by 100%");
                                    activeitem.lastuntil = -1;
                                    activeitem.remainingtime = item.Data.skillTime; //in millisecond
                                    SendEffect(107);
                                    DecreaseQuantity(slot);
                                    c_data.activateditem.Add(activeitem);
                                    return true;
                                }
                                break;
                            #endregion
                        }
                    } break;
                #endregion
                #region CS Pet
                case ItemTypes.CSPet:
                    {
                        SendMessageInfoNotice("Cash shop pets don't work in this version.");
                        return true;
                    }
                #endregion
                #region Egg
                case ItemTypes.Egg:
                    {
                        SendMessageInfoNotice("Eggs don't work in this version.");
                        return true;
                    }
                #endregion
                #region Food
                case ItemTypes.Food:
                    {
                        if (!(FoodDelay <= DLL.clock()))
                        {
                            SendMessageInfo(FlyFF.TID_GAME_ATTENTIONCOOLTIME);
                            return true;
                        }
                        FoodDelay = DLL.clock() + 2300;
                        int hpRecovery = ((item.Data.adjAttributes[0] > (c_data.f_MaxHP - c_attributes[DST_HP]) ? (c_data.f_MaxHP - c_attributes[DST_HP]) : item.Data.adjAttributes[0]));
                        if (c_attributes[DST_HP] >= item.Data.min_ability)
                        {
                            hpRecovery /= 4;
                            SendMessageInfo(FlyFF.TID_GAME_LIMITHP);
                        }
                        c_attributes[DST_HP] += hpRecovery;
                        SendPlayerAttribSet(DST_HP, c_attributes[DST_HP]);
                        SendEffect(FlyFF.GEN_CURE01);
                        lock (c_data.dataLock) // ----------------> NEEDED?!
                            DecreaseQuantity(slot);
                        return true;
                    }
                #endregion
                #region Pill
                case ItemTypes.Pill:
                    {
                        if (PillDelay <= DLL.clock())
                        {
                            SendMessageInfo(FlyFF.TID_GAME_ATTENTIONCOOLTIME);
                            return true;
                        }
                        PillDelay = DLL.clock() + 8000;
                        int hpRecovery = ((item.Data.adjAttributes[0] > (c_data.f_MaxHP - c_attributes[DST_HP]) ? c_data.f_MaxHP - c_attributes[DST_HP] : item.Data.adjAttributes[0]));
                        if (c_attributes[DST_HP] >= item.Data.min_ability)
                        {
                            hpRecovery /= 4;
                            SendMessageInfo(FlyFF.TID_GAME_LIMITHP);
                        }
                        c_attributes[DST_HP] += hpRecovery;
                        SendPlayerAttribSet(DST_HP, c_attributes[DST_HP]);
                        SendEffect(FlyFF.GEN_CURE01);
                        lock (c_data.dataLock) // ----------------> NEEDED?!
                            DecreaseQuantity(slot);
                        return true;
                    }
                #endregion
                #region Refresher
                case ItemTypes.Refresher:
                    {
                        int mpRecovery = ((item.Data.adjAttributes[0] > (c_data.f_MaxMP - c_attributes[DST_MP]) ? c_data.f_MaxMP - c_attributes[DST_MP] : item.Data.adjAttributes[0]));
                        if (c_attributes[DST_MP] >= item.Data.min_ability)
                        {
                            mpRecovery /= 4;
                            SendMessageInfo(FlyFF.TID_GAME_LIMITMP);
                        }
                        c_attributes[DST_MP] += mpRecovery;
                        SendPlayerAttribSet(DST_MP, c_attributes[DST_MP]);
                        SendEffect(FlyFF.GEN_REF01);
                        lock (c_data.dataLock) // ----------------> NEEDED?!
                            DecreaseQuantity(slot);
                        return true;
                    }
                #endregion
                #region Scroll
                case ItemTypes.Scroll:
                    {
                        switch (item.Data.itemID)
                        {
                            #region Reskill
                            case 10434: // Re-Skill by BlackGiant
                                   {
                                       int vagrant = 0, // Extra Skillpoints which get added by job.
                                           mecenary = 40,
                                           acrobat = 50,
                                           assist = 60,
                                           magician = 90,
                                           knight = 80,
                                           blade = 80,
                                           elementer = 300,
                                           psykeeper = 90,
                                           billposter = 120,
                                           ringmaster = 100,
                                           ranger = 100,
                                           jester = 100;

                                       //reset all skill level from server side :
                                       for (int i = 0; i < c_data.skills.Count; i++)
                                       {
                                           if (c_data.skills.Count > 0)
                                           {
                                               Skill curSkill = c_data.skills[i];
                                               curSkill.dwSkillLevel = 0;
                                           }
                                       }
  
                                      
                                        //calculate point for level NOTE that all if will be read for a character level 120 for exemple
                                      
                                           c_data.dwSkillPoints = c_data.dwLevel * 2;
                                       if (c_data.dwLevel >= 20)
                                           c_data.dwSkillPoints += (c_data.dwLevel - 19); //+1 for each level above 20 (corresponding to+3)
                                       if (c_data.dwLevel >= 40)
                                           c_data.dwSkillPoints += (c_data.dwLevel - 39); //+1 for each level above 40(corresponding to+4)
                                       if (c_data.dwLevel >= 60)
                                           c_data.dwSkillPoints += (c_data.dwLevel - 59); //+1 for each level above 60(corresponding to+5)
                                       if (c_data.dwLevel >= 80)
                                           c_data.dwSkillPoints += (c_data.dwLevel - 79); //+1for each level above 80(corresponding to+6)
                                       if (c_data.dwLevel >= 100)
                                           c_data.dwSkillPoints += (c_data.dwLevel - 99); //+1 for each level above 100(corresponding to+7)
                                      

                                       switch (c_data.dwClass)
                                       {
                                           case 0:break;                                              
                                           case 1 : c_data.dwSkillPoints += mecenary;break;
                                           case 2 : c_data.dwSkillPoints += acrobat;break;
                                           case 3 : c_data.dwSkillPoints += assist;break;
                                           case 4 : c_data.dwSkillPoints += magician;break;
                                           case 6 : c_data.dwSkillPoints += knight + mecenary;break;
                                           case 7 : c_data.dwSkillPoints += blade + mecenary;break;
                                           case 8 : c_data.dwSkillPoints += jester + acrobat;break;
                                           case 9 : c_data.dwSkillPoints += ranger + acrobat;break;
                                           case 10: c_data.dwSkillPoints += ringmaster + assist;break;
                                           case 11: c_data.dwSkillPoints += billposter + assist;break;
                                           case 12: c_data.dwSkillPoints += psykeeper + magician;break;
                                           case 13: c_data.dwSkillPoints += elementer + magician;break;
                                           case 16:
                                           case 24:
                                           case 17:
                                           case 25: c_data.dwSkillPoints = 658; break;
                                           case 18:
                                           case 19:
                                           case 26:
                                           case 27: c_data.dwSkillPoints = 688; break;
                                           case 20:
                                           case 28: c_data.dwSkillPoints = 698; break;
                                           case 21:
                                           case 29: c_data.dwSkillPoints = 718; break;
                                           case 22:
                                           case 30: c_data.dwSkillPoints = 718; break;
                                           case 23:
                                           case 31: c_data.dwSkillPoints = 928; break;
                                           default:
                                               SendMessageHud("Invalid Job!"); break;
                                       }
                                       SendPlayerSkills();
                                       DecreaseQuantity(slot);
                                      SendEffect(FlyFF.SYS_EXPAN01, c_position);
                                       SendMessageHud("Your Skill-Points were restated!");
                                      return true;
                                   }
                                   break;
                               
                               
                    
                            #endregion
                            #region Restats
                            case 10211: //Restat
                                {
                                    c_attributes[DST_STR] = 15;
                                    c_attributes[DST_STA] = 15;
                                    c_attributes[DST_DEX] = 15;
                                    c_attributes[DST_INT] = 15;
                                    int statPoints = (c_data.dwLevel * 2) - 2;
                                    int LvlLess60 = c_data.dwLevel - 60;
                                    if (LvlLess60 < 0)
                                        LvlLess60 = 0;
                                    if (c_data.dwClass >= 16)
                                        statPoints += LvlLess60;
                                    if (c_data.dwClass >= 24)
                                        statPoints += 15;
                                    c_data.dwStatPoints = statPoints;
                                    SendPlayerStats();
                                    DecreaseQuantity(slot, 1);
                                    SendEffect(FlyFF.SYS_EXPAN01, c_position);
                                    return true;
                                }
                                break;
                            case 30151: // Restat STR
                            case 30152: // Restat STA
                            case 30153: // Restat DEX
                            case 30154: // Restat INT
                                {
                                    int stat = 0;
                                    switch (item.Data.itemID)
                                    {
                                        case 30151: stat = DST_STR; break;
                                        case 30153: stat = DST_STA; break;
                                        case 30152: stat = DST_DEX; break;
                                        case 30154: stat = DST_INT; break;
                                    }
                                    if (stat == 0) return true;
                                    c_data.dwStatPoints += (c_attributes[stat] - 15);
                                    c_attributes[stat] = 15;
                                    SendPlayerStats();
                                    DecreaseQuantity(slot);
                                    SendEffect(FlyFF.SYS_EXPAN01, c_position);
                                    return true;
                                }

                            #endregion
                            #region Scroll of Forgive
                            case 10427: // Scroll of Forgive
                                {
                                    if (c_data.dwKarma > 0)
                                    {
                                        c_data.dwKarma--;
                                        SendMessageInfo(FlyFF.TID_GAME_GETKARMA);
                                        SendEffect(FlyFF.SYS_RELEASE01, c_position);
                                        DecreaseQuantity(slot);
                                    }
                                    break;
                                }
                            #endregion
                            #region Scroll Of Holy
                            case 10426: // Scroll of Holy
                                {
                                    if (c_data.dwDisposition > 0)
                                    {
                                        int dispRecover = 100;
                                        if (c_data.dwDisposition < dispRecover)
                                            dispRecover = c_data.dwDisposition;
                                        c_data.dwDisposition -= dispRecover;
                                        SendEffect(FlyFF.SYS_RELEASE01, c_position);
                                        DecreaseQuantity(slot);
                                    }
                                    break;
                                }
                            #endregion
                            default:
                                SendMessageInfoNotice("Protection scrolls don't work in this version.");
                                return true;
                        }
                    } 
                    break;
                #endregion
                #region VitalDrink
                case ItemTypes.VitalDrink:
                    {
                        int fpRecovery = ((item.Data.adjAttributes[0] > (c_data.f_MaxFP - c_attributes[DST_FP]) ? c_data.f_MaxFP - c_attributes[DST_FP] : item.Data.adjAttributes[0]));
                        if (c_attributes[DST_FP] >= item.Data.min_ability)
                        {
                            fpRecovery /= 4;
                            SendMessageInfo(FlyFF.TID_GAME_LIMITFP);
                        }
                        c_attributes[DST_FP] += fpRecovery;
                        SendPlayerAttribSet(DST_FP, c_attributes[DST_FP]);
                        SendEffect(FlyFF.GEN_CURE01);
                        lock (c_data.dataLock) // ----------------> NEEDED?!
                            DecreaseQuantity(slot);
                        return true;
                    }
                #endregion
                #region Firebombs
                case ItemTypes.Fireworks:
                    {
                        switch (item.Data.itemID)
                        {
                            case 2908: // HW Fireworks
                                {
                                    SendEffect(FlyFF.NAT_HWFIREWORKS01, c_position);
                                    SendPlayerSound(FlyFF.SND_ITEM_FIRESHOWER);
                                    DecreaseQuantity(slot);
                                } break;
                            case 2907: // Goodbye Bomb
                                {
                                    SendEffect(FlyFF.NAT_MAGICBOMB03, c_position);
                                    SendPlayerSound(FlyFF.SND_ITEM_HAPPYNEWYEARBOMB);
                                    DecreaseQuantity(slot);
                                } break;
                            case 2906: // Sulnal Bomb
                                {
                                    SendEffect(FlyFF.NAT_MAGICBOMB02, c_position);
                                    SendPlayerSound(FlyFF.SND_ITEM_HAPPYNEWYEARBOMB);
                                    DecreaseQuantity(slot);
                                } break;
                            case 2905: // Fireshower
                                {
                                    SendEffect(FlyFF.NAT_FIRESHOWER01, c_position);
                                    SendPlayerSound(FlyFF.SND_ITEM_FIRESHOWER);
                                    DecreaseQuantity(slot);
                                } break;
                            case 2904: // New Year's Bomb
                                {
                                    SendEffect(FlyFF.NAT_MAGICBOMB01, c_position);
                                    SendPlayerSound(FlyFF.SND_ITEM_HAPPYNEWYEARBOMB);
                                    DecreaseQuantity(slot);
                                } break;
                        }
                        return true;
                    }
                    break;
                #endregion
                #region Cracker
                case ItemTypes.Cracker:
                    {
                        switch (item.Data.itemID)
                        {
                            case 10224: // Twister Cracker
                                {
                                    SendEffect(FlyFF.NAT_TWISTER01, c_position);
                                    SendPlayerSound(FlyFF.NAT_FIRESHOWER01);
                                    DecreaseQuantity(slot);
                                } break;
                            case 10222: // Heart Cracker
                                {
                                    SendEffect(FlyFF.NAT_HEART01, c_position);
                                    SendPlayerSound(FlyFF.SND_ITEM_HAPPYNEWYEARBOMB);
                                    DecreaseQuantity(slot);
                                } break;
                            case 10221: // Rocket Cracker
                                {
                                    SendEffect(FlyFF.NAT_ROCKET01);
                                    SendPlayerSound(FlyFF.SND_ITEM_HAPPYNEWYEARBOMB);
                                    DecreaseQuantity(slot);
                                } break;
                        }
                        return true;
                    }
                #endregion
                #region Blinkwing 
                //take data in database to allow custom blinkwings
                case ItemTypes.TownBlinkwings:
                    {
                        
                            // New revival regions code.
                            RevivalRegion region = null;
                            for (int i = 0; i < WorldServer.data_resrgns.Count; i++)
                            {
                                if (WorldServer.data_resrgns[i].IsInRegion(c_position, c_data.dwMapID))
                                {
                                    region = WorldServer.data_resrgns[i];
                                    break;
                                }
                            }
                            if (region == null)
                                region = WorldHelper.DefaultRevivalRegion;
                            // set stuff o_O
                            c_position = new Point(region.c_destiny.x, c_position.y, region.c_destiny.z, c_position.angle);
                            if (region.dwDestMap != c_data.dwMapID)
                            {
                                c_data.dwMapID = region.dwDestMap;
                                SendPlayerMapTransfer();
                            }
                            else
                            {
                                SendMoverNewPosition();
                            }
                            DecreaseQuantity(slot);
                            return true;
                    } break;
                case ItemTypes.Blinkwings:
                    {
                        
                            int x = item.Data.itemAtkOrder1;
                            int y = item.Data.itemAtkOrder2;
                            int z = item.Data.itemAtkOrder3;
                            int mapid = item.Data.weaponType;

                            if (c_data.dwMapID == mapid)
                            {
                                c_position.x = x;
                                c_position.y = y;
                                c_position.z = z;
                                SendMoverNewPosition();
                            }
                            else
                            {
                                c_data.dwMapID = mapid;
                                c_position.x = x;
                                c_position.y = y;
                                c_position.z = z;
                                SendPlayerMapTransfer();
                            }
                        
                    DecreaseQuantity(slot);
                    return true;
                    } break;
                #endregion
                #region Tickets
                case ItemTypes.Tickets:
                    {
                        if (item.bExpired)
                            return true;
                        long Duration = item.Data.min_ability;
                        switch (item.Data.itemID)
                        {
                                
                            #region Ticket for azria
                            case 26530: // 15 day
                            case 26529: //7 day
                            case 26528: //24 hours
                                
                                {
                                     if (c_data.dwMapID !=2)
                                     {
                                         c_position.x = 1265;
                                         c_position.y = 113;
                                         c_position.z = 1330;
                                         c_data.dwMapID = 2;
                                         SendPlayerMapTransfer();
                                     }
                                     else
                                     {
                                         c_data.dwMapID = 1;
                                         c_position.x = 7161;
                                         c_position.y = 100;
                                         c_position.z = 3264;
                                         SendPlayerMapTransfer();
                                     }
                                }
                                //to do, modifyitem duration
                                break;
                            #endregion
                            #region Ticket for corail island
                            case 26682: // 15 day
                            case 26681: //7 day
                            
                                
                                {
                                    if (c_data.dwMapID != 2)
                                    {
                                        c_position.x = 1265;
                                        c_position.y = 113;
                                        c_position.z = 1330;
                                        c_data.dwMapID = 2;
                                        SendPlayerMapTransfer();
                                    }
                                    else
                                    {
                                        c_data.dwMapID = 1;
                                        c_position.x = 7161;
                                        c_position.y = 100;
                                        c_position.z = 3264;
                                        SendPlayerMapTransfer();
                                    }
                                }
                                
                                break;
                            #endregion
                        }
                        if ((item.qwLastUntil == -1) && (!item.bExpired))//not activated
                        {

                            //to do, modifyitem duration
                            c_data.inventory[slot.dwID].c_item.c_awakening += 0xE300000000000000;
                            //slot.c_item.qwLastUntil = (long)DLL.time() + Duration*60;
                            c_data.inventory[slot.dwID].c_item.qwLastUntil = (long)DLL.time() + 60; //for test purpose
                            //Add this into activeitem
                            ActiveItems activeitem = new ActiveItems();
                            activeitem.itemid = slot.c_item.dwItemID;
                            activeitem.charid = c_data.dwCharacterID;
                            activeitem.lastuntil = c_data.inventory[slot.dwID].c_item.qwLastUntil;
                            c_data.activateditem.Add(activeitem);
                            SendItemUpdate(new ItemUpdateStatus(slot.dwID, ITEM_MODTYPE_TICKET, item.Data.min_ability, 0));

                            SendPlayerBuffByCSItem(item);
                        }
                        SendItemUpdate(new ItemUpdateStatus(slot.dwID, 0, 1, 0));

                        
                        return true;
                    } break;
                #endregion
                #region SummonBalls
                case ItemTypes.SummonBalls:
                    {
                        try
                        {
                            Boolean foundball = false;
                            int SbNo = 0;
                            for (int i = 0; i < WorldServer.world_summonballs.Count; i++)
                            {
                                if (WorldServer.world_summonballs[i].itemID == item.dwItemID)
                                { foundball = true; SbNo = i; break; }
                            }
                            if (foundball == false)
                            { Log.Write(Log.MessageType.error, "Could not find SummonBall: ItemID: {0}", item.dwItemID); return true; }

                            SummonBalls sb = WorldServer.world_summonballs[SbNo];
                            int maxMovers = sb.listballs.Count;
                            int rollvalue = DiceRoller.RandomNumber(1, 100);
                            int amount = DiceRoller.RandomNumber(1, 10);
                            int mobID, chance = 0;

                            for (int c = 0; c < maxMovers; c++)
                            {
                                mobID = Convert.ToInt32(sb.listballs[c].moverID);
                                chance = Convert.ToInt32(sb.listballs[c].chance);
                                Boolean foundMob = false;

                                if (rollvalue < chance)
                                {
                                    for (int a = 0; a <= amount; a++)
                                    {
                                        Monster mob = new Monster(mobID, c_position, c_data.dwMapID, false);
                                        try
                                        {
                                            foundMob = true;
                                            mob.respawn = false;
                                            mob.bIsFollowing = false;
                                            mob.bIsFighting = false;
                                            mob.next_move_time = DLL.time() + 1;
                                            mob.c_target = null;
                                            WorldServer.world_monsters.Add(mob);
                                            SendMonsterSpawn(mob);
                                            mob.SendMoverNewDestination();
                                            c_spawns.Add(mob);
                                        }
                                        catch (Exception ex)
                                        { Log.Write(Log.MessageType.error, "Error at spawn Monster: Unknown ModelID({0}:{1})", mobID, ex.Message); return true; }
                                    }
                                }
                                else { rollvalue -= chance; }
                                if (foundMob)
                                    break;
                            }
                            DecreaseQuantity(slot);
                        }
                        catch (Exception ex)
                        { Log.Write(Log.MessageType.error, "Error at spawning Monster: {0}", ex.Message); }
                    }
                    return true;
                #endregion
                #region PotionAttributes

                case ItemTypes.PotionAttributes:
                    {
                        switch (item.dwItemID)
                        {
                            case 26220: // Medicine of Sharpness
                            case 26221: // Medecine of attack
                            case 26222: // Medicine of hit 
                            case 26223: // Medicine of thinking
                            case 26224: // Medicine of STR
                            case 26225: // Medicine of DEX
                            case 26226: // Medicine of INT
                            case 26227: // Medicine of STA
                            case 26228: // Medicine of Protection
                            case 26229: // Medicine of Resistance
                            case 26230: // Medicine for MP
                            case 26231: // Medicine of magic
                            case 26232: // Medicine of Evasion
                            case 26233: // Medicine of Copious Bleeding
                            case 30017: // Medicine of Strong DEX
                            case 30018: // Medicine of Strong INT
                            case 30019: // Medicine of Strong STA
                            case 30016: // Medicine of Strong STR
                            case 30020: // Medicine of Strong Defense    
                                {
                                    ActiveItems activeitem = new ActiveItems(item.dwItemID, c_data.dwCharacterID, item.Data.skillTime, -1);
                                    Boolean alreadyActivated = false;
                                    for (int i = 0; i < c_data.activateditem.Count; i++)
                                    {
                                        if (c_data.activateditem[i].itemid == item.dwItemID)
                                            alreadyActivated = true; break;
                                    }
                                    if (alreadyActivated)
                                        return true;
                                    SendPlayerBuffByCSItem(item);
                                    c_attributes[item.Data.destAttributes[0]] += item.Data.adjAttributes[0];
                                    SendPlayerAttribRaise(item.Data.destAttributes[0], item.Data.adjAttributes[0],-1);
                                    SendMessageUsageInfo(1, "You used a " + item.Data.itemName + "!");
                                    activeitem.remainingtime = item.Data.skillTime;
                                    SendEffect(107);
                                    c_data.activateditem.Add(activeitem);
                                    break;
                                }
                                break;
                            default:
                                { SendMessageInfoNotice("Item function not done yet!"); return true; }
                        }
                        DecreaseQuantity(slot);
                        return true;
                    }

                #endregion
                #region Monster Transy
                case ItemTypes.MonsterTransy: // [BlackGiant]
                    {
                        ActiveItems activeitem = new ActiveItems(item.dwItemID, c_data.dwCharacterID, item.Data.skillTime, -1);
                        Boolean alreadyActivated = false;
                        int moverID = 0;
                        for (int i = 0; i < c_data.activateditem.Count; i++)
                        {
                            if (c_data.activateditem[i].itemid == item.dwItemID)
                            {
                                alreadyActivated = true;
                                break;
                            }
                            ItemData TransyItemData = WorldHelper.GetItemDataByItemID(c_data.activateditem[i].itemid);
                            if (TransyItemData.itemkind[0] + TransyItemData.itemkind[1] + TransyItemData.itemkind[2] == 113)
                                alreadyActivated = true; break;
                        }
                        if (alreadyActivated) { return true; }
                        try
                        {
                            string[] splitTextFile = item.Data.textFile.Split(';');
                            moverID = Convert.ToInt32(splitTextFile[1]);
                        }
                        catch (Exception)
                        { return true; }

                        if (moverID == 0) { return true; }
                        SendPlayerBuffByCSItem(item);
                        SendPlayerTransy(moverID);
                        SendEffect(107);
                        activeitem.remainingtime = item.Data.skillTime;
                        activeitem.lastuntil = -1;
                        DecreaseQuantity(slot);
                        c_data.activateditem.Add(activeitem);
                        return true;
                    }
                #endregion
                #region NoDisguise
                case ItemTypes.NoDisguise: // [BlackGiant]
                    {
                        int ActiveItemPosition = 0;
                        for (int i = 0; i < c_data.activateditem.Count; i++)
                        {
                            ItemData TransyItemData = WorldHelper.GetItemDataByItemID(c_data.activateditem[i].itemid);
                            
                            if (TransyItemData.itemkind[0] + TransyItemData.itemkind[1] + TransyItemData.itemkind[2] == 113)
                            {
                                SendPlayerRemoveBuffByItem(c_data.activateditem[i].itemid);
                                c_data.activateditem.Remove(c_data.activateditem[i]);
                                switch (c_data.dwGender)
                                { case 0: SendPlayerTransy(11); Log.Write(Log.MessageType.info, "SendPlayerTransy(11) : MALE"); break; case 1: SendPlayerTransy(12); Log.Write(Log.MessageType.info, "SendPlayerTransy(12) : FEMALE"); break; }
                                SendEffect(107);
                                DecreaseQuantity(slot);
                                break;
                            }
                        }
                        return true;
                    }
                #endregion
                #region CSFood
                case ItemTypes.CSFood:
                    {
                        ActiveItems activeitem = new ActiveItems(item.dwItemID, c_data.dwCharacterID, -1, item.Data.circleTime * 1000);
                        for (int i = 0; i < c_data.activateditem.Count; i++)
                        {
                            if (c_data.activateditem[i].itemid == item.dwItemID)
                                return false;
                        }
                        switch (item.dwItemID)
                        {
                            case 10209: // Bull Hamstern
                                
                                c_attributes[item.Data.destAttributes[0]] += item.Data.adjAttributes[1];
                                SendPlayerAttribRaise(item.Data.destAttributes[0], item.Data.adjAttributes[1], -1);
                                SendPlayerCSFoodBonus(item.Data.circleTime);
                                SendEffect(111);                                
                                c_data.activateditem.Add(activeitem);
                                break;
                        }
                        DecreaseQuantity(slot);
                        SendMessageUsageInfo(1, "You used a " + item.Data.itemName + "!");
                        return true;
                    }
                #endregion
                #region CollectBattery
                case ItemTypes.CollectBattery:
                    {
                        switch (item.dwItemID)
                        {
                            case 26453:
                                Slot weapon = GetSlotByPosition(52);
                                if (weapon.c_item != null && weapon.c_item.dwItemID == 26452)
                                {
                                    SendItemUpdate(new ItemUpdateStatus(weapon.dwID, ITEM_MODTYPE_CHARGE, 1800, 0));
                                    weapon.c_item.dwCharge = 1800;
                                    DecreaseQuantity(slot);
                                }
                                else
                                    SendPlayerInformationMessage(0xD31);
                                break;
                   /*         case 26454:
                            case 26455:
                                {
                                    ActiveItems activeitem = new ActiveItems(item.dwItemID, c_data.dwCharacterID, -1, item.Data.min_ability * 1000);
                                    for (int i = 0; i < c_data.activateditem.Count; i++)
                                    {
                                        if (c_data.activateditem[i].itemid == item.dwItemID)
                                            return false;
                                    }
                                    SendPlayerBuffByCSItem(item);
                                    SendEffect(107);
                                    DecreaseQuantity(slot);
                                    c_data.activateditem.Add(activeitem);
                                    return true;
                                }
                                break;*/
                        }
                        return true;
                    }
                #endregion

            }
            return false;
        }
        #endregion

        #region Item Types
        /// <summary>
        /// Defines a set of typical items a player can consume or use.
        /// </summary>
        public enum ItemTypes
        {
            Other,
            Refresher,
            Food,
            VitalDrink,
            Scroll,
            Pill,
            CSPet,
            Egg,
            SpecialScrolls,
            Fireworks,
            Cracker,
            PotionAttributes,
            Blinkwings,
            SummonBalls,
            TownBlinkwings,
            Tickets,
            MonsterTransy,
            NoDisguise,
            CSFood,
            CollectBattery
        }
        #endregion

        #region GetItemType function
        /// <summary>
        /// Gets an enumeration of the type of item that represents item id.
        /// </summary>
        /// <param name="ItemID">Item ID of the item.</param>
        /// <returns>Enumeration of type ItemTypes representing type of item.</returns>
        public ItemTypes GetItemType(Item item)
        {
            //need to change this method...
            switch (item.Data.itemkind[0] + item.Data.itemkind[1] + item.Data.itemkind[2])
            {
                
                case 39:
                case 52: return ItemTypes.Refresher;
                case 47:
                case 48:
                case 49: return ItemTypes.Food;
                case 50: return ItemTypes.VitalDrink;
                case 51: if (item.Data.itemkind[0] == 6) return ItemTypes.CSFood; else return ItemTypes.Fireworks;
                case 54: return ItemTypes.Cracker;
                case 60: return ItemTypes.PotionAttributes;
                case 72: if (item.dwItemID == 26453) return ItemTypes.CollectBattery; else return ItemTypes.Blinkwings;
                case 74: if (item.dwItemID == 4805) return ItemTypes.TownBlinkwings; else return ItemTypes.SpecialScrolls;
                case 75: return ItemTypes.Scroll;
                case 76: return ItemTypes.Pill;
                case 88: return ItemTypes.CollectBattery;
                case 109: return ItemTypes.SummonBalls;
                case 113: return ItemTypes.MonsterTransy;
                case 115: return ItemTypes.NoDisguise;
                case 124: // ==> Medicines (STR)
                case 125: // ==> Medicines (DEX)
                case 126: // ==> Medicines (STA)
                case 127: // ==> Medicines (INT)
                case 128: return ItemTypes.PotionAttributes; // ==> Medicines (Defense)
                case 131: return ItemTypes.CSPet;
                case 146: return ItemTypes.Tickets;
                case 151: return ItemTypes.Egg;       
                
            }
            return ItemTypes.Other;
        }
        #endregion
    }
}
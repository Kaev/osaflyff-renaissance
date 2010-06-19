using System;
using System.Collections.Generic;
using System.Text;

namespace FlyffWorld
{
    public partial class PlayerData
    {
        public int f_MaxHP
        {
            get
            {
                int s_sta = parent.c_totalAttributes[FlyFF.DST_STA] ;
                double a = dwLevel * ClientDB.MHP[dwClass] * 0.5;
                double b = (dwLevel + 1) * 0.25;
                double e = (s_sta * 0.02 + 1.0) * a * b + s_sta * 10.0 + 80.0;
                return (int)Math.Floor((Math.Floor(e) * (1 + ((double)parent.c_totalAttributes[FlyFF.DST_HP_MAX_RATE] / 100d)) + parent.c_totalAttributes[FlyFF.DST_STAT_ALLUP] + parent.c_totalAttributes[FlyFF.DST_HP_MAX]));
            }
        }
        public int f_MaxMP
        {
            get
            {
                int s_int = parent.c_totalAttributes[FlyFF.DST_INT] + parent.c_totalAttributes[FlyFF.DST_STAT_ALLUP];
                double a = ((dwLevel << 1) + s_int * 8) * ClientDB.MMP[dwClass] + 22 + s_int * ClientDB.MMP[dwClass];
                return (int)Math.Floor((Math.Floor(a) + parent.c_totalAttributes[FlyFF.DST_MP_MAX]) * (1 + ((double)parent.c_totalAttributes[FlyFF.DST_MP_MAX_RATE] / 100d)));
            }
        }
        public int f_MaxFP
        {
            get
            {
                int s_sta = parent.c_totalAttributes[FlyFF.DST_STA] + parent.c_totalAttributes[FlyFF.DST_STAT_ALLUP];
                double a = ((dwLevel << 1) + s_sta * 6) * ClientDB.MFP[dwClass] + s_sta * ClientDB.MFP[dwClass];
                return (int)Math.Floor((Math.Floor(a) + parent.c_totalAttributes[FlyFF.DST_FP_MAX]) * (1 + ((double)parent.c_totalAttributes[FlyFF.DST_FP_MAX_RATE] / 100d)));
            }
        }
        public float f_RecoveryFactor
        {
            get
            {
                return 1f;
            }
        }
        public int f_MovingSpeed
        {
            get
            {
                // TODO:
                return 0;
            }
        }
        public int f_castingskillspeed(Skills skill)
        {
            if (Skills.isMagicAttackSkill(skill.dwNameID))
            {
                if (parent.c_attributes[FlyFFAttributes.DST_SPELL_RATE] > 90)
                    parent.c_attributes[FlyFFAttributes.DST_SPELL_RATE] = 90; //in case of people abuse of GMCommand...

                int spellrate = (skill.dwCastingTime - skill.dwCastingTime * parent.c_attributes[FlyFFAttributes.DST_SPELL_RATE] / 100)/1000;
                return spellrate; 
            }
            else
            {
                if (parent.c_attributes[FlyFFAttributes.DST_ATTACKSPEED_RATE] > 100)
                    parent.c_attributes[FlyFFAttributes.DST_SPELL_RATE] = 100; //in case of people abuse of GMCommand...
                if (parent.c_attributes[FlyFFAttributes.DST_ATTACKSPEED] > 100)
                    parent.c_attributes[FlyFFAttributes.DST_ATTACKSPEED] = 100; //in case of people abuse of GMCommand...

                int atkrate = (skill.dwAttackSpeed + (100 - parent.c_attributes[FlyFFAttributes.DST_ATTACKSPEED]) * skill.dwAttackSpeed - skill.dwAttackSpeed * parent.c_attributes[FlyFFAttributes.DST_ATTACKSPEED_RATE] / 100)/1000;
                return atkrate;
            }

                           
        }
        public int f_MinDefense
        {
            get
            {
                int armordefmin=0;
                for (int i=42;i<=53;i++)
                {
                    Item myitem = parent.GetSlotByPosition(i).c_item; //suit
                    if (myitem ==null)
                        continue;
                    switch (i) //suit
                    {
                        case 44:
                        case 46:
                        case 47:
                        case 48:
                            armordefmin += myitem.Data.min_ability;
                            break;
                        case 53: //is this a shield ?
                            if (myitem.Data.itemkind[2] ==16)
                                armordefmin +=myitem.Data.min_ability;
                                break;
                     }
                 }
                armordefmin /= 5;
                double def = Math.Floor(((double)(parent.c_attributes[FlyFF.DST_STA] - 15) * 1.19d - 0.26d));
                def += Math.Floor(((double)(parent.c_data.dwLevel - 1) * 0.72d - 0.35d));
                def += (double)parent.c_attributes[FlyFF.DST_ADJDEF] + (double)parent.c_attributes[FlyFF.DST_DEFHITRATE_DOWN];
                def += (double)(Math.Floor((double)def * ((double)parent.c_attributes[FlyFF.DST_ADJDEF_RATE] / 100)));
                def += armordefmin;
                /*
                 * i prefer to use formula i find by experiementation : Divinepunition
                float def = (float)(Math.Floor(((float)parent.c_attributes[FlyFF.DST_STA] / 2.0f + (float)dwLevel * 2f) * 0.3571429f) - 4f + Math.Floor((float)(parent.c_attributes[FlyFF.DST_STA] - 14) * (float)ClientDB.DEF[dwClass]));
                def += (float)parent.c_attributes[FlyFF.DST_ADJDEF] + (float)parent.c_attributes[FlyFF.DST_DEFHITRATE_DOWN];
                def += (float)(Math.Floor((float)def * ((float)parent.c_attributes[FlyFF.DST_ADJDEF_RATE] / 100)));
                def += armordefmin;*/
                return (int)Math.Max(def, 0f);
            }
        }
        public int f_MaxDefense
        {
            get
            {
                int armordefmax=0;
                for (int i=42;i<=53;i++)
                {
                    Item myitem = parent.GetSlotByPosition(i).c_item; //suit
                if (myitem ==null)
                    continue;
                switch (i) //suit
                {
                    case 44:
                    case 46:
                    case 47:
                    case 48:
                    armordefmax += myitem.Data.max_ability;
                        break;
                    case 53: //is this a shield ?
                        if (myitem.Data.itemkind[2] ==16)
                            armordefmax +=myitem.Data.max_ability;
                        break;
                }
                }
                armordefmax /=5;
                double def = Math.Floor(((double)(parent.c_attributes[FlyFF.DST_STA] - 15) * 1.19d - 0.26d));
                def += Math.Floor(((double)(parent.c_data.dwLevel - 1) * 0.72d - 0.35d));
                def += (double)parent.c_attributes[FlyFF.DST_ADJDEF] + (double)parent.c_attributes[FlyFF.DST_DEFHITRATE_DOWN];
                def += (double)(Math.Floor((double)def * ((double)parent.c_attributes[FlyFF.DST_ADJDEF_RATE] / 100)));
                def += armordefmax;
                /*
                 * i prefer to use formula i find by experiementation : Divinepunition
                float def = (float)(Math.Floor(((float)parent.c_attributes[FlyFF.DST_STA] / 2f + (float)dwLevel * 2f) * 0.3571429f) - 4f + Math.Floor((float)(parent.c_attributes[FlyFF.DST_STA] - 1) * ClientDB.DEF[dwClass]));
                def += (float)parent.c_attributes[FlyFF.DST_ADJDEF] + (float)parent.c_attributes[FlyFF.DST_DEFHITRATE_DOWN];
                def += (float)(Math.Floor((float)def * ((float)parent.c_attributes[FlyFF.DST_ADJDEF_RATE] / 100)));
                def += armordefmax;*/
                return (int)Math.Max(def, 0f);
            }
        }
        public int f_RecoveryHP
        {
            get
            {
                double a = f_MaxHP / (dwLevel * 500);
                double b = parent.c_attributes[FlyFF.DST_STA] * ClientDB.HPR[dwClass] + a;
                double c = dwLevel * 0.333333 + b;
                return (int)Math.Floor(Math.Floor(c) + parent.c_attributes[FlyFF.DST_HP_RECOVERY]);
            }
        }
        public int f_RecoveryMP
        {
            get
            {
                double a = f_MaxMP / (dwLevel * 500);
                double b = parent.c_attributes[FlyFF.DST_INT] * ClientDB.MPR[dwClass] + a;
                double c = (dwLevel * 1.5d + b) * 0.2;
                return (int)Math.Floor(Math.Floor(c)+parent.c_attributes[FlyFF.DST_MP_RECOVERY]);
            }
        }
        public int f_RecoveryFP
        {
            get
            {
                double a = f_MaxFP / (dwLevel * 500);
                double b = parent.c_attributes[FlyFF.DST_STA] * ClientDB.FPR[dwClass] + a;
                double c = ((dwLevel << 1) + b) * 0.2;
                return (int)Math.Floor(Math.Floor(c) + parent.c_attributes[FlyFF.DST_FP_RECOVERY]);
            }
        }
        public int f_PercentHP
        {
            get
            {
                return (int)Math.Floor((double)(parent.c_attributes[FlyFF.DST_HP] * 100) / (double)f_MaxHP);
            }
        }
        public int f_PercentMP
        {
            get
            {
                return (int)Math.Floor((double)(parent.c_attributes[FlyFF.DST_MP] * 100) / (double)f_MaxMP);
            }
        }
        public int f_PercentFP
        {
            get
            {
                return (int)Math.Floor((double)(parent.c_attributes[FlyFF.DST_FP] * 100) / (double)f_MaxFP);
            }
        }
        public int f_CritRate
        {
            get
            {
                double crit = Math.Floor((double)(parent.c_attributes[FlyFF.DST_DEX]) / 10 * ClientDB.CRIT[dwClass]) + (double)parent.c_attributes[FlyFF.DST_CHR_CHANCECRITICAL];
                return (int)Math.Min(crit, 100d);
            }
        }
        public int f_BlockRate
        {
            get
            {
                // TODO: block rate modifiers
                double block = Math.Floor((double)(ClientDB.BLOCK[dwClass] * parent.c_attributes[FlyFF.DST_DEX] * dwLevel * 0.002d));
                return (int)Math.Min(block, 100);
            }
        }
        public double f_RateModifierEXP
        {
            get
            {
                double baseRate = 0d, rateMul = 1d;
                if (dwClass > 13)
                    baseRate = 0.5d;
                else
                    baseRate = 1d;
                int countampliscroll = 0;
                int countampliEM = 0;
                for (int i = 0; i < parent.c_data.activateditem.Count; i++)//search in our activated item if we have scroll
                {
                    ActiveItems curactiveitem = parent.c_data.activateditem[i];
                    if (curactiveitem == null)
                        continue;
                    if (parent.IsAmplificationScroll(curactiveitem.itemid))
                        countampliscroll++; //increase counter
                    if (curactiveitem.itemid == 10474)//scroll amplification EM rate*2
                        countampliEM++;
                }
                if (countampliEM != 0 && countampliscroll != 0) //if we have two scroll activated...only ampliEM will work
                    countampliscroll = 0;

                rateMul += 0.5d * (double)countampliscroll + 1.0d * countampliEM;
                
                return baseRate * rateMul;
            }
        }
        public double f_RateModifierDrop
        {
            get
            {
                // Change this "formula" if you want to
                return 1;
            }
        }
        /*
        public int f_DamageMin
        {
            get
            {
                Slot slot = parent.GetSlotByPosition(52);
                if (slot == null)
                {
                    return 0; // correct this
                }
                int dwBaseAtk = slot.c_item.Data.min_ability;
                int nWeaponKind = slot.c_item.Data.itemkind[2];
                int dwAdjAtk = parent.c_attributes[FlyFF.DST_ADJ_HITRATE];
                int dwWeaponRefine = slot.c_item.dwRefine;
                return (int)Math.Floor(((dwBaseAtk << 1) + GetWeaponAttack(nWeaponKind) + dwAdjAtk) * GetRefineMultiplier(dwWeaponRefine) + Math.Pow(dwWeaponRefine, 1.5));
            }
        }
        public int f_DamageMax
        {
            get
            {
                Slot slot = parent.GetSlotByPosition(52);
                if (slot == null)
                {
                    return 0; // correct this
                }
                int dwBaseAtk = slot.c_item.Data.min_ability;
                int nWeaponKind = slot.c_item.Data.itemkind[2];
                int dwAdjAtk = parent.c_attributes[FlyFF.DST_ADJ_HITRATE];
                int dwWeaponRefine = slot.c_item.dwRefine;
                return (int)Math.Floor(((dwBaseAtk << 1) + GetWeaponAttack(nWeaponKind) + dwAdjAtk) * GetRefineMultiplier(dwWeaponRefine) + Math.Pow(dwWeaponRefine, 1.5));
            }
        }
        private double GetWeaponAttack(int nWeaponKind)
        {
            int s_str = parent.c_totalAttributes[FlyFF.DST_STR] + parent.c_totalAttributes[FlyFF.DST_STAT_ALLUP];
            int s_sta = parent.c_totalAttributes[FlyFF.DST_STA] + parent.c_totalAttributes[FlyFF.DST_STAT_ALLUP];
            int s_dex = parent.c_totalAttributes[FlyFF.DST_DEX] + parent.c_totalAttributes[FlyFF.DST_STAT_ALLUP];
            int s_int = parent.c_totalAttributes[FlyFF.DST_INT] + parent.c_totalAttributes[FlyFF.DST_STAT_ALLUP];
            switch (nWeaponKind)
            {
                case FlyffItemkind.IK3_SWD:
                case FlyffItemkind.IK3_THSWD:
                    return (s_str - 12) * 4.5d + dwLevel * 1.1d;
                case FlyffItemkind.IK3_AXE:
                case FlyffItemkind.IK3_THAXE:
                    return (s_str - 12) * 5.5d + dwLevel * 1.2d;
                case FlyffItemkind.IK3_STAFF:
                    return (s_str - 10) * 0.8d + dwLevel * 1.1d;
                case FlyffItemkind.IK3_CHEERSTICK:
                    return (s_str - 10) * 3.0d + dwLevel * 1.3d;
                case FlyffItemkind.IK3_KNUCKLEHAMMER:
                    return (s_str - 10) * 5.0d + dwLevel * 1.2d;
                case FlyffItemkind.IK3_WAND:
                    return (s_int - 10) * 6.0d + dwLevel * 1.2d;
                case FlyffItemkind.IK3_YOYO:
                    return (s_str - 12) * 4.2d + dwLevel * 1.1d;
                case FlyffItemkind.IK3_YOBO:
                    return ((s_dex - 14) * 4.0d + s_str * 0.2 + dwLevel * 1.3d) * 0.7d;
                case -1:
                    return 1;
                default:
                    Log.Write(Log.MessageType.warning, "PlayerData::GetWeaponAttack(): unknown nWeaponKind {0}", nWeaponKind);
                    return 1;
            }
        }
         * */
        private double GetRefineMultiplier(int dwWeaponRefine)
        {
            int[] refineTable = new int[] { 0, 2, 4, 6, 8, 10, 13, 16, 19, 21, 24 };
            try
            {
                return (refineTable[dwWeaponRefine] + 100) * 0.01;
            }
            catch
            {
                return 1;
            }
        }

        /// <summary>
        /// Calculate ATK value that is showed on character infos and is used to calculate change to hit
        /// </summary>        
        public int f_ATKValue()
        {
            ItemData item = new ItemData(); ;
            Slot slot = parent.GetSlotByPosition(52);
            Item myitem = slot.c_item;
            #region if we use hand
            if (slot == null || myitem == null)
            {
                //we are using hand
                item.itemID = 11;
                item.itemkind[2] = 1;
                item.min_ability = 1;
                item.max_ability = 2;
                item.itemName = "Hand";
                myitem = new Item();
                myitem.dwItemID = 11;

            }
            #endregion
            else
                item = slot.c_item.Data;
            

            //calculate base ATK value
            int ATKvalue = 8 + parent.c_data.dwLevel - 1 + parent.c_data.dwLevel / 10; //base atk before equiping weapon
            int minvalue = 0;
            int maxvalue = 0;
            int leveleffect = 0;
            double refinemodifier = GetRefineMultiplier(myitem.dwRefine);
            #region Damage corresponding to itemkind
            switch (item.itemkind[2])
            {
                case 1: //hand
                    ATKvalue = (item.min_ability+item.max_ability)/2+ (int)Math.Floor(1.1d * ATKvalue);                    
                    break;
                case 2: //sword
                case 8: //two handed sword
                    ATKvalue += ((int)((item.min_ability + item.max_ability) / 2 * refinemodifier) * 2) + (int)Math.Floor(3.7d * (parent.c_attributes[1] - 14));
                    if (item.itemkind[2] == 8)
                        ATKvalue += parent.c_attributes[FlyFFAttributes.DST_TWOHANDMASTER_DMG];
                    if (item.itemkind[2] == 2)
                    ATKvalue += parent.c_attributes[FlyFFAttributes.DST_SWD_DMG];
                    
                    break;
                case 3: //Axe
                    ATKvalue += ((int)((item.min_ability + item.max_ability) / 2 * refinemodifier) * 2) + (int)Math.Floor(4.7d * (parent.c_attributes[1] - 14));
                                          
                    if (item.itemkind[2] == 3)
                    ATKvalue += parent.c_attributes[FlyFFAttributes.DST_AXE_DMG];
                    break;
                case 4: //stick
                    ATKvalue += ((int)((item.min_ability + item.max_ability) / 2 * refinemodifier) * 2) + (int)Math.Floor(2.2d * (parent.c_attributes[1] - 14));
                    break;
                case 5: //knuckle
                    ATKvalue += ((int)((item.min_ability + item.max_ability) / 2 * refinemodifier) * 2) + (int)Math.Floor(4.2d * (parent.c_attributes[1] - 14));
                    ATKvalue += parent.c_attributes[FlyFFAttributes.DST_KNUCKLE_DMG]  +(ATKvalue * parent.c_attributes[FlyFFAttributes.DST_KNUCKLEMASTER_DMG]) / 100;
                    break;
                case 6: //wand
                    ATKvalue += ((int)((item.min_ability+item.max_ability)/2 * refinemodifier) * 2) + (int)Math.Floor(5.2d * (parent.c_attributes[3] - 14));
                    break;
                case 7: //staff
                    ATKvalue += ((int)((item.min_ability+item.max_ability)/2 * refinemodifier) * 2) - 4;
                    break;
                case 9: //two handed axe
                    ATKvalue += ((int)((item.min_ability + item.max_ability) / 2 * refinemodifier) * 2) + (int)Math.Floor(4.7d * (parent.c_attributes[1] - 14) + 10.93);
                    ATKvalue += (ATKvalue * parent.c_attributes[FlyFFAttributes.DST_TWOHANDMASTER_DMG]) / 100 + parent.c_attributes[FlyFFAttributes.DST_AXE_DMG];
                    break;
                case 11: //yoyo
                    ATKvalue += ((int)((item.min_ability + item.max_ability) / 2 * refinemodifier) * 2) + (int)Math.Floor(3.4d * (parent.c_attributes[1] - 14) + 2.23);
                    ATKvalue += parent.c_attributes[FlyFFAttributes.DST_YOY_DMG] + (ATKvalue*parent.c_attributes[FlyFFAttributes.DST_YOYOMASTER_DMG])/100;
                    break;
                case 12: //Bow
                    ATKvalue += ((int)((item.min_ability + item.max_ability) / 2 * refinemodifier) * 2) + 2 * (parent.c_attributes[2] - 14) - 7;
                    ATKvalue += parent.c_attributes[FlyFFAttributes.DST_BOW_DMG] + (ATKvalue*parent.c_attributes[FlyFFAttributes.DST_BOWMASTER_DMG])/100;
                    break;
                default:
                    
                    ATKvalue = (item.min_ability+item.max_ability)/2 + (int)Math.Floor(1.1d * ATKvalue);
                    break;
            }
            #endregion
            return ATKvalue;
        }
        /// <summary>
        /// Calculate ATK value maximum used to calculate damage
        /// </summary>        
        public int f_ATKValuemmax()
        {
            ItemData item = new ItemData(); ;
            Slot slot = parent.GetSlotByPosition(52);
            Item myitem = slot.c_item;
            #region if we use hand
            if (slot == null || myitem == null)
            {
                //we are using hand
                item.itemID = 11;
                item.itemkind[2] = 1;
                item.min_ability = 1;
                item.max_ability = 2;
                item.itemName = "Hand";
                myitem = new Item();
                myitem.dwItemID = 11;

            }
            #endregion
            else
                item = slot.c_item.Data;
            

            //calculate base ATK value
            int ATKvalue = 8 + parent.c_data.dwLevel - 1 + parent.c_data.dwLevel / 10; //base atk before equiping weapon
            int minvalue = 0;
            int maxvalue = 0;
            int leveleffect = 0;
            double refinemodifier = GetRefineMultiplier(myitem.dwRefine);
            #region Damage corresponding to itemkind
            switch (item.itemkind[2])
            {
                case 1: //hand
                    ATKvalue = (item.min_ability + item.max_ability) / 2 + (int)Math.Floor(1.1d * ATKvalue);
                    break;
                case 2: //sword
                case 8: //two handed sword
                    ATKvalue += ((int)((item.min_ability + item.max_ability) / 2 * refinemodifier) * 2) + (int)Math.Floor(3.7d * (parent.c_attributes[1] - 14));
                    if (item.itemkind[2] == 8)
                        ATKvalue += parent.c_attributes[FlyFFAttributes.DST_TWOHANDMASTER_DMG];
                    if (item.itemkind[2] == 2)
                        ATKvalue += parent.c_attributes[FlyFFAttributes.DST_SWD_DMG];

                    break;
                case 3: //Axe
                    ATKvalue += ((int)((item.min_ability + item.max_ability) / 2 * refinemodifier) * 2) + (int)Math.Floor(4.7d * (parent.c_attributes[1] - 14));

                    if (item.itemkind[2] == 3)
                        ATKvalue += parent.c_attributes[FlyFFAttributes.DST_AXE_DMG];
                    break;
                case 4: //stick
                    ATKvalue += ((int)((item.min_ability + item.max_ability) / 2 * refinemodifier) * 2) + (int)Math.Floor(2.2d * (parent.c_attributes[1] - 14));
                    break;
                case 5: //knuckle
                    ATKvalue += ((int)((item.min_ability + item.max_ability) / 2 * refinemodifier) * 2) + (int)Math.Floor(4.2d * (parent.c_attributes[1] - 14));
                    ATKvalue += parent.c_attributes[FlyFFAttributes.DST_KNUCKLE_DMG] + (ATKvalue * parent.c_attributes[FlyFFAttributes.DST_KNUCKLEMASTER_DMG]) / 100;
                    break;
                case 6: //wand
                    ATKvalue += ((int)((item.min_ability + item.max_ability) / 2 * refinemodifier) * 2) + (int)Math.Floor(5.2d * (parent.c_attributes[3] - 14));
                    break;
                case 7: //staff
                    ATKvalue += ((int)((item.min_ability + item.max_ability) / 2 * refinemodifier) * 2) - 4;
                    break;
                case 9: //two handed axe
                    ATKvalue += ((int)((item.min_ability + item.max_ability) / 2 * refinemodifier) * 2) + (int)Math.Floor(4.7d * (parent.c_attributes[1] - 14) + 10.93);
                    ATKvalue += (ATKvalue * parent.c_attributes[FlyFFAttributes.DST_TWOHANDMASTER_DMG]) / 100 + parent.c_attributes[FlyFFAttributes.DST_AXE_DMG];
                    break;
                case 11: //yoyo
                    ATKvalue += ((int)((item.min_ability + item.max_ability) / 2 * refinemodifier) * 2) + (int)Math.Floor(3.4d * (parent.c_attributes[1] - 14) + 2.23);
                    ATKvalue += parent.c_attributes[FlyFFAttributes.DST_YOY_DMG] + (ATKvalue * parent.c_attributes[FlyFFAttributes.DST_YOYOMASTER_DMG]) / 100;
                    break;
                case 12: //Bow
                    ATKvalue += ((int)((item.min_ability + item.max_ability) / 2 * refinemodifier) * 2) + 2 * (parent.c_attributes[2] - 14) - 7;
                    ATKvalue += parent.c_attributes[FlyFFAttributes.DST_BOW_DMG] + (ATKvalue * parent.c_attributes[FlyFFAttributes.DST_BOWMASTER_DMG]) / 100;
                    break;
                default:

                    ATKvalue = (item.min_ability + item.max_ability) / 2 + (int)Math.Floor(1.1d * ATKvalue);
                    break;
            }
            #endregion
            return ATKvalue+parent.c_attributes[FlyFFAttributes.DST_ATKPOWER];
        }
        /// <summary>
        /// Calculate ATK value minimum used to calculate damage
        /// </summary>        
        public int f_ATKValuemin()
        {
            ItemData item = new ItemData(); ;
            Slot slot = parent.GetSlotByPosition(52);
            Item myitem = slot.c_item;
            #region if we use hand
            if (slot == null || myitem == null)
            {
                //we are using hand
                item.itemID = 11;
                item.itemkind[2] = 1;
                item.min_ability = 1;
                item.max_ability = 2;
                item.itemName = "Hand";
                myitem = new Item();
                myitem.dwItemID = 11;

            }
            #endregion
            else
                item = slot.c_item.Data;
            

            //calculate base ATK value
            int ATKvalue = 8 + parent.c_data.dwLevel - 1 + parent.c_data.dwLevel / 10; //base atk before equiping weapon
            int minvalue = 0;
            int maxvalue = 0;
            int leveleffect = 0;
            double refinemodifier = GetRefineMultiplier(myitem.dwRefine);
            #region Damage corresponding to itemkind
            switch (item.itemkind[2])
            {
                case 1: //hand
                    ATKvalue = (item.min_ability + item.max_ability) / 2 + (int)Math.Floor(1.1d * ATKvalue);
                    break;
                case 2: //sword
                case 8: //two handed sword
                    ATKvalue += ((int)((item.min_ability + item.max_ability) / 2 * refinemodifier) * 2) + (int)Math.Floor(3.7d * (parent.c_attributes[1] - 14));
                    if (item.itemkind[2] == 8)
                        ATKvalue += parent.c_attributes[FlyFFAttributes.DST_TWOHANDMASTER_DMG];
                    if (item.itemkind[2] == 2)
                        ATKvalue += parent.c_attributes[FlyFFAttributes.DST_SWD_DMG];

                    break;
                case 3: //Axe
                    ATKvalue += ((int)((item.min_ability + item.max_ability) / 2 * refinemodifier) * 2) + (int)Math.Floor(4.7d * (parent.c_attributes[1] - 14));

                    if (item.itemkind[2] == 3)
                        ATKvalue += parent.c_attributes[FlyFFAttributes.DST_AXE_DMG];
                    break;
                case 4: //stick
                    ATKvalue += ((int)((item.min_ability + item.max_ability) / 2 * refinemodifier) * 2) + (int)Math.Floor(2.2d * (parent.c_attributes[1] - 14));
                    break;
                case 5: //knuckle
                    ATKvalue += ((int)((item.min_ability + item.max_ability) / 2 * refinemodifier) * 2) + (int)Math.Floor(4.2d * (parent.c_attributes[1] - 14));
                    ATKvalue += parent.c_attributes[FlyFFAttributes.DST_KNUCKLE_DMG] + (ATKvalue * parent.c_attributes[FlyFFAttributes.DST_KNUCKLEMASTER_DMG]) / 100;
                    break;
                case 6: //wand
                    ATKvalue += ((int)((item.min_ability + item.max_ability) / 2 * refinemodifier) * 2) + (int)Math.Floor(5.2d * (parent.c_attributes[3] - 14));
                    break;
                case 7: //staff
                    ATKvalue += ((int)((item.min_ability + item.max_ability) / 2 * refinemodifier) * 2) - 4;
                    break;
                case 9: //two handed axe
                    ATKvalue += ((int)((item.min_ability + item.max_ability) / 2 * refinemodifier) * 2) + (int)Math.Floor(4.7d * (parent.c_attributes[1] - 14) + 10.93);
                    ATKvalue += (ATKvalue * parent.c_attributes[FlyFFAttributes.DST_TWOHANDMASTER_DMG]) / 100 + parent.c_attributes[FlyFFAttributes.DST_AXE_DMG];
                    break;
                case 11: //yoyo
                    ATKvalue += ((int)((item.min_ability + item.max_ability) / 2 * refinemodifier) * 2) + (int)Math.Floor(3.4d * (parent.c_attributes[1] - 14) + 2.23);
                    ATKvalue += parent.c_attributes[FlyFFAttributes.DST_YOY_DMG] + (ATKvalue * parent.c_attributes[FlyFFAttributes.DST_YOYOMASTER_DMG]) / 100;
                    break;
                case 12: //Bow
                    ATKvalue += ((int)((item.min_ability + item.max_ability) / 2 * refinemodifier) * 2) + 2 * (parent.c_attributes[2] - 14) - 7;
                    ATKvalue += parent.c_attributes[FlyFFAttributes.DST_BOW_DMG] + (ATKvalue * parent.c_attributes[FlyFFAttributes.DST_BOWMASTER_DMG]) / 100;
                    break;
                default:

                    ATKvalue = (item.min_ability + item.max_ability) / 2 + (int)Math.Floor(1.1d * ATKvalue);
                    break;
            }
            #endregion
            return ATKvalue;
        }



        /// <summary>
        /// Gets the chance to hit monster
        /// </summary>
        /// <param name="Monster mob">Monster you want to fight</param>
        /// <returns></returns>
        public int f_HitRate(Monster mob)
        {
            /*[Divinepunition] OK Hitrate = chance to hit target. It's equal to ATK value +/- all ajust hitrate +/-effect of level -effect of mob evasion value
             *  In future version i will take a look to this value, for now i leave it like this
             * */   
            Slot slot = parent.GetSlotByPosition(52);
            if (slot == null || slot.c_item == null)
                return 20;
            MonsterData mobdata = mob.Data;
            ItemData item = slot.c_item.Data;
            if (mobdata == null)
            {
                return 20;
            }
            double hit = (double)f_ATKValue();
            
            double a = hit + mobdata.mobFlee;
            double b = dwLevel + mobdata.mobLevel;
            int c = (int)(Math.Floor(Math.Floor((hit * 1.6 / a) * (dwLevel * 1.2 / b) * 150d) + parent.c_attributes[FlyFF.DST_ADJ_HITRATE]));
            c += parent.c_attributes[FlyFF.DST_DEFHITRATE_DOWN]+parent.c_attributes[FlyFF.DST_STAT_ALLUP];
            c = Math.Max(c, 20);
            return Math.Min(c, 96);
        }
        //done by divinepunition for v11...recalculate formula :
        #region Damage formula against mob
        public int f_CalculateDamageAgainstMob(Monster mob)
        {
            ItemData item = new ItemData(); ;
            Slot slot = parent.GetSlotByPosition(52);
            Item myitem = slot.c_item;
            #region if we use hand
            if (slot == null || myitem == null)
            {
                //we are using hand
                item.itemID = 11;
                item.itemkind[2] = 1;
                item.min_ability = 1;
                item.max_ability = 2;
                item.itemName = "Hand";
                myitem = new Item();
                myitem.dwItemID = 11;

            }
            #endregion
            else
                item = slot.c_item.Data;
            int leveleffect = 0;
            int hit = DiceRoller.RandomNumber(f_ATKValuemin(), f_ATKValuemmax());
            //calculate effectof difference in level
            leveleffect = GetLevelEffectForWeapon(mob);
            
            //Add effect of level and eventual adjust of hitrate
            hit += leveleffect;            
            //add elemental effect
            #region Add element type effect
            switch (GetElementOpposition(mob,myitem))
            {
                case "neutral": break;
                case "same":
                    int nulldamageprobability = 5 * myitem.dwEleRefine;
                    if (DiceRoller.Roll(nulldamageprobability))
                        return 0;//we have 5% by element refine to make 0 damage if we use same element
                    break;
                case "stronger":
                    hit = (int)Math.Floor(1.1d * hit);                    
                    break;
                case "weaker":
                    hit = (int)Math.Floor(0.9d * hit);
                    break;
            }
            #endregion

            //now we delete Natural armor of mobs
            hit -= (mob.Data.def+ mob.c_attributes[FlyFF.DST_ADJDEF]);
            hit += parent.c_attributes[FlyFF.DST_CHR_DMG] + parent.c_attributes[FlyFF.DST_STAT_ALLUP] / 100;
            return hit;
        }
        public int f_CalculateMagicalDamageVsMonster(Monster mob, Skills skills)
        {
            int leveleffect = 0;  
            int minvalue = f_ATKValuemin();
            int maxvalue = f_ATKValuemmax();
            //calculate effectof difference in level
            leveleffect = GetLevelEffectForWeapon(mob);    //to replace by test with magical attack and effect of level        
            //add abilitymin-max effect
            minvalue += skills.dwAbilityMin*10;
            maxvalue += skills.dwAbilityMax*10;
            int damage = DiceRoller.RandomNumber(minvalue, maxvalue);
            //add skill level effect 
            damage += leveleffect ;            
            //add effect of element            
            #region Add element type effect
            switch (GetElementOpposition(mob, skills))
            {
                case "neutral": break;
                case "same":
                    int nulldamageprobability = 25 - skills.dwSkillLvl;
                    if (DiceRoller.Roll(nulldamageprobability))
                        return 0;//we have 5% by element refine to make 0 damage if we use same element
                    break;
                case "stronger":
                    damage = (int)Math.Floor(1.04f * damage);
                    break;
                case "weaker":
                    damage = (int)Math.Floor(0.47f * damage);
                    break;
            }
            switch (Skills.GetElementType(skills)) //effect of skills mastery
            {
                case 0://no element do nothing
                    break;
                case 1: //fire
                    damage += parent.c_attributes[FlyFFAttributes.DST_MASTRY_FIRE];
                    damage -= (int)(mob.Data.mobResistance[0] * (float)damage);
                    damage -= (damage * mob.c_attributes[FlyFF.DST_RESIST_FIRE])/100;
                    break;
                
                case 2: //water
                    damage += parent.c_attributes[FlyFFAttributes.DST_MASTRY_WATER];
                    damage -= (int)(mob.Data.mobResistance[1] * (float)damage);
                    damage -= (damage * mob.c_attributes[FlyFF.DST_RESIST_WATER]) / 100;
                    break;
                case 3: //lightning
                    damage += parent.c_attributes[FlyFFAttributes.DST_MASTRY_ELECTRICITY];
                    damage -= (int)(mob.Data.mobResistance[2] * (float)damage);
                    damage -= (damage * mob.c_attributes[FlyFF.DST_RESIST_ELECTRICITY]) / 100;
                    break;
                case 4://wind
                    damage += parent.c_attributes[FlyFFAttributes.DST_MASTRY_WIND];
                    damage -= (int)(mob.Data.mobResistance[3] * (float)damage);
                    damage -= (damage * mob.c_attributes[FlyFF.DST_RESIST_WATER]) / 100;
                    break;
                case 5: //earth
                    damage += parent.c_attributes[FlyFFAttributes.DST_MASTRY_EARTH]; break;
                    damage -= (int)(mob.Data.mobResistance[4] * (float)damage);
                    damage -= (damage * mob.c_attributes[FlyFF.DST_RESIST_EARTH]) / 100;
                default: break;
            }
            #endregion
            //remove natural armor of mobs
            damage -= (mob.Data.def+ mob.c_attributes[FlyFF.DST_ADJDEF]+mob.Data.mobResistMagic);
            return damage;
        }
        #endregion
        #region DefineElement status
        public string GetElementOpposition(Monster mob, Item myitem)
        {
            int myelementtype = myitem.dwElement;
            if (myelementtype == 0)
                return "neutral";

            switch (mob.Data.mobElement)
            {
                case 0: return "neutral"; break;
                case 1:
                    if (myelementtype == 1)
                        return "same";
                    else if (myelementtype == 2)//i use water vs fire
                        return "stronger";
                    else if (myelementtype == 4)//i use wind against fire
                        return "weaker";
                    else
                        return "neutral";
                    break;
                case 2:
                    if (myelementtype == 2)
                        return "same";
                    else if (myelementtype == 3)//i use electricity vs water
                        return "stronger";
                    else if (myelementtype == 1)//i use fire against water
                        return "weaker";
                    else
                        return "neutral";
                    break;
                case 3:
                    if (myelementtype == 3)
                        return "same";
                    else if (myelementtype == 5)//i use earth vs electricity
                        return "stronger";
                    else if (myelementtype == 2)//i use water against electricity
                        return "weaker";
                    else
                        return "neutral";
                    break;
                case 4:
                    if (myelementtype == 4)
                        return "same";
                    else if (myelementtype == 1)//i use fire vs wind
                        return "stronger";
                    else if (myelementtype == 5)//i use earth against wind
                        return "weaker";
                    else
                        return "neutral";
                    break;
                case 5:
                    if (myelementtype == 5)
                        return "same";
                    else if (myelementtype == 4)//i use wind vs earth
                        return "stronger";
                    else if (myelementtype == 3)//i use electricity against earth
                        return "weaker";
                    else
                        return "neutral";
                    break;
            }
            return "neutral";
        }
        public string GetElementOpposition(Monster mob, Skills skills)
        {
            int element = Skills.GetElementType(skills);
            if (element == 0)
                return "neutral";

            switch (mob.Data.mobElement)
            {
                case 0: return "neutral"; break;
                case 1:
                    if (element == 1)
                        return "same";
                    else if (element == 2)//i use water vs fire
                        return "stronger";
                    else if (element == 4)//i use wind against fire
                        return "weaker";
                    else
                        return "neutral";
                    break;
                case 2:
                    if (element == 2)
                        return "same";
                    else if (element == 3)//i use electricity vs water
                        return "stronger";
                    else if (element == 1)//i use fire against water
                        return "weaker";
                    else
                        return "neutral";
                    break;
                case 3:
                    if (element == 3)
                        return "same";
                    else if (element == 5)//i use earth vs electricity
                        return "stronger";
                    else if (element == 2)//i use water against electricity
                        return "weaker";
                    else
                        return "neutral";
                    break;
                case 4:
                    if (element == 4)
                        return "same";
                    else if (element == 1)//i use fire vs wind
                        return "stronger";
                    else if (element == 5)//i use earth against wind
                        return "weaker";
                    else
                        return "neutral";
                    break;
                case 5:
                    if (element == 5)
                        return "same";
                    else if (element == 4)//i use wind vs earth
                        return "stronger";
                    else if (element == 3)//i use electricity against earth
                        return "weaker";
                    else
                        return "neutral";
                    break;
            }
            return "neutral";
        }

        #endregion
        #region Level effect
        public int GetLevelEffectForWeapon(Monster mob)
        {
            int mylevel = parent.c_data.dwLevel;
            int moblvl = mob.Data.mobLevel;
            int leveleffect = 0;
            if (mylevel > moblvl)
                leveleffect = 1 * (mylevel - moblvl) / 3;
            else if (mylevel < moblvl)
                leveleffect -= 6 * (moblvl - mylevel);
            return leveleffect;
        }
        public int GetLevelEffectForHand(Monster mob)
        {
            int mylevel = parent.c_data.dwLevel;
            int moblvl = mob.Data.mobLevel;
            int leveleffect = 0;
            if (mylevel > moblvl)
                leveleffect = 1 * (mylevel - moblvl);
            else if (mylevel < moblvl)
                leveleffect -= 3 * (moblvl - mylevel);
            return leveleffect;
        }
        #endregion

        public int f_CalculateDamagefromMonster(Monster mob)
        {
            int minvalue = mob.Data.atkMin;
            int maxvalue = mob.Data.atkMax;
            int defense = DiceRoller.RandomNumber(f_MinDefense, f_MaxDefense);
            int dmgvalue = DiceRoller.RandomNumber(minvalue, maxvalue);
            //effect of level
            //TO DO !

            int damage =dmgvalue - defense + mob.c_attributes[FlyFF.DST_CHR_DMG];

            if (damage < 0)
                damage = dmgvalue/10; //it's a minimum that could be decrease next by level difference
            Item myitem = parent.GetSlotByPosition(44).c_item; //suit
            if (myitem == null)
                return damage;
            
                        
            //effect of element
            #region Add element type effect
            switch (GetElementOpposition(mob, myitem))
            {
                case "neutral": break;
                case "same":
                    int nulldamageprobability = 5 * myitem.dwEleRefine;
                    if (DiceRoller.Roll(nulldamageprobability))
                        return 0;//we have 5% by element refine to make 0 damage if we use same element
                    break;
                case "stronger":
                    damage = (int)Math.Floor(1.1d * damage);
                    break;
                case "weaker":
                    damage = (int)Math.Floor(0.9d * damage);
                    break;
            }
            #endregion
            //TO DO :add effect of level from monster i put a temporary value :
            if (parent.c_data.dwLevel < mob.Data.mobLevel)
            {
                damage += (mob.Data.mobLevel - parent.c_data.dwLevel) * 3;
            }
            else if (parent.c_data.dwLevel > mob.Data.mobLevel)
            {
                damage -= (parent.c_data.dwLevel - mob.Data.mobLevel);
            }
            
            return damage;
        }
 
        //to do damage formula against a player
    }
}

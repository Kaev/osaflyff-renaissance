using System;
using System.Collections.Generic;

using System.Text;

namespace FlyffWorld
{
    class BuffDB
    {
        const int NULL = 0;
        public const int    // Buff names
                            BUFF_PATIENCE = 46,
                            BUFF_PREVENTION = 48,//don't work actually
                            BUFF_HEAPUP = 49,
                            BUFF_CANNONBALL = 50,
                            BUFF_MENTALSIGN = 52,
                            BUFF_BEEFUP = 53,
                            BUFF_STONEHAND = 113,
                            BUFF_QUICKSTEP = 114,
                            BUFF_CATSREFLEX = 115,
                            BUFF_ACCURACY = 116,
                            BUFF_HASTE = 20,
                            BUFF_PROTECT = 146,
                            BUFF_HOLYGUARD = 147,
                            BUFF_SF = 148,
                            BUFF_GT = 150,
                            BUFF_MERC_PROTECTION = 9,
                            BUFF_MERC_PANBARRIER = 10,
                            BUFF_MERC_IMPOWERWEAPON = 111,
                            BUFF_MERC_AXEMASTER = 8,
                            BUFF_MERC_SWORDMASTER = 7,
                            BUFF_MERC_SMITEAXE = 109,
                            BUFF_MERC_BLAZINGSWORD = 108,
                            BUFF_KNI_GUARD = 128,
                            BUFF_KNI_RAGE = 130,
                            BUFF_BLA_BERSERK = 143,
                            BUFF_BILL_ASMODEUS = 156,
                            BUFF_ELE_FIREMASTER = 169,
                            BUFF_ELE_EARTHMASTER = 175,
                            BUFF_ELE_WINDMASTER = 181,
                            BUFF_ELE_LIGHTINGMASTER = 172,
                            BUFF_ELE_WATERMASTER = 178,
                            BUFF_ACR_FASTWALKER = 195,
                            BUFF_ACR_ABSOLUTEBLOCK = 199,
                            BUFF_ACR_YOYOMASTER = 191,
                            BUFF_ACR_BOWMASTER = 192,
                            BUFF_JST_POISON = 208, //actually i haven't implemented poison effect on target, just increase damage
                            BUFF_JST_CRITICALSWING = 207,
                            BUFF_JST_BLEEDING	= 209,
                            BUFF_JST_ABSORB = 210,
            //BUFF_JST_BACKSTAB           = 211,not implemented yet
                            BUFF_RAN_FASTATTACK = 215,
                            BUFF_RAN_NATURE = 221;

                            
        public static int[] buffSkillTable = new int[]{ // Has all skills that are buffs in it. used to determine if a skill is a buff or not.
            46,  48,  49,  50,
            52,  53,  113, 114,
            115, 116, 20,  146,
            147, 148, 150, 9,
            10, 111, 128, 130,
            143, 156, 169, 175,
            181, 172, 178, 195,
            199, 191, 192, 208,
            207, 215, 221, 8, 7,
            109, 108, 209, 210,
            208

        };

        public static bool isBuff(int skillID)
        {
            for (int i = 0; i < buffSkillTable.Length; i++)
                if (buffSkillTable[i] == skillID)
                    return true;
            return false;
        }
        
        // Main function now
        public static int getBuffInterval(Skill buff, int Int)
        {
            Log.Write(Log.MessageType.debug, "Je suis passe par le getinterval");
            if (!isBuff(buff.dwSkillID))
                return 0;
            int interval = 100000;
            double intMultiplier = 2;
            switch (buff.dwSkillID)
            {
                case BUFF_MENTALSIGN:
                case BUFF_HEAPUP:
                case BUFF_BEEFUP:
                case BUFF_ACCURACY:
                case BUFF_PATIENCE:
                case BUFF_CANNONBALL:
                case BUFF_CATSREFLEX:
                    if (buff.dwSkillLevel % 2 == 0)
                        interval = 240000;
                    else
                        interval = 180000;
                    if (buff.dwSkillLevel >= 20)
                        interval = 300000;
                    break;
                case BUFF_QUICKSTEP:
                    interval = 600000;
                    break;
                case BUFF_HASTE:
                    interval = 100000 + buff.dwSkillLevel * 20000;
                    break;
                case BUFF_PROTECT:
                    interval = 30000 + buff.dwSkillLevel * 5000;
                    break;
                case BUFF_HOLYGUARD:
                    switch (buff.dwSkillLevel)
                    {
                        case 1: interval = 10000; break;
                        case 2: interval = 20000; break;
                        case 3: interval = 40000; break;
                        case 4: interval = 50000; break;
                        case 5: interval = 70000; break;
                        case 6: interval = 80000; break;
                        case 7: interval = 100000; break;
                        case 8: interval = 110000; break;
                        case 9: interval = 130000; break;
                        case 10: interval = 150000; break;
                        default: interval = 10000; break;
                    }
                    break;
                case BUFF_SF:
                    interval = buff.dwSkillLevel * 5000;
                    intMultiplier = 0.2;
                    break;
                case BUFF_GT:
                    interval = 20000; // always 20
                    intMultiplier = 0.2;
                    break;
                case BUFF_PREVENTION:
                    interval = 150000;
                    break;
                case BUFF_MERC_PROTECTION:
                    switch (buff.dwSkillLevel)
                    {
                        case 1: case 3: case 5: case 7: 
                        case 9: interval = 90000; break;
                        case 2: case 4: case 6: case 8: case 10:
                        case 11: case 13: case 15:
                        case 17:interval = 120000; break;
                        case 12: case 14: case 16:
                        case 18: case 19:interval = 150000; break;
                        case 20:interval = 200000; break;
                        default: interval = 10000; break;
                    }
                    break;
                case BUFF_MERC_PANBARRIER:
                    interval = 85000;
                    for (int i=1;i<=buff.dwSkillLevel;i++)
                    {
                        interval +=5000;
                    }
                    if (buff.dwSkillLevel ==20)
                        interval += 15000;

                    break;
                case BUFF_MERC_IMPOWERWEAPON:
                    interval = 100000;
                    for (int i=1;i<=buff.dwSkillLevel;i++)
                    {
                        interval +=20000;
                    }
                    if (buff.dwSkillLevel ==20)
                        interval += 10000;
                    break;
                case BUFF_MERC_AXEMASTER:
                case BUFF_MERC_SWORDMASTER:
                    interval = 95000;
                    for (int i=0;i<buff.dwSkillLevel;i++)
                    {
                        if (i<=9)
                            interval += 5000;
                        if (i>9)
                            interval += 10000;
                        if (i == 20)
                            interval += 350000;
                    }
                    break;
                case BUFF_MERC_SMITEAXE:
                case BUFF_MERC_BLAZINGSWORD:
                    interval = 255000 + 45000 * buff.dwSkillLevel;
                    if (buff.dwSkillLevel == 20)
                        interval += 15000;
                    break;
                case BUFF_KNI_GUARD:
                    interval = 10000 + 1000*buff.dwSkillLevel;
                    break;
                case BUFF_KNI_RAGE:
                    interval = 3000;
                    break;
                case BUFF_BLA_BERSERK:
                    interval = 10000;
                    for (int i=1;i<=buff.dwSkillLevel;i++)
                    {
                        if ((i %2) == 0)//each two level
                            interval += 5000;
                    }
                    break;
                case BUFF_BILL_ASMODEUS :
                    interval = 5000 +5000*buff.dwSkillLevel;
                    break;
                case BUFF_ELE_FIREMASTER:
                case BUFF_ELE_EARTHMASTER:
                case BUFF_ELE_WINDMASTER:
                case BUFF_ELE_LIGHTINGMASTER:
                case BUFF_ELE_WATERMASTER:
                    interval = 2000;
                    break;
                case BUFF_ACR_FASTWALKER:
                    interval = 4500 + 500*buff.dwSkillLevel;
                    if (buff.dwSkillLevel ==20)
                        interval += 500;
                    break;
                case BUFF_ACR_ABSOLUTEBLOCK:
                    interval = 10000 +10000*buff.dwSkillLevel;
                    break;
                case BUFF_ACR_YOYOMASTER:
                case BUFF_ACR_BOWMASTER:
                    interval = 500;
                    break;
                case BUFF_JST_POISON:
                case BUFF_JST_CRITICALSWING:
                case BUFF_JST_ABSORB:
                case BUFF_JST_BLEEDING:
                    interval = 25000 +5000*buff.dwSkillLevel;
                    if (buff.dwSkillLevel == 20)
                        interval += 5000;
                    break;
                             
                case BUFF_RAN_FASTATTACK:
                    interval = 35000 + 5000*buff.dwSkillLevel;
                    break;
                case BUFF_RAN_NATURE:
                    interval = 50000 +10000*buff.dwSkillLevel;
                    break;
                case BUFF_STONEHAND:
                    interval = 9000;
                    break;

                
                /*case BUFF_JST_BACKSTAB:
                interval = 5000;
                break;*/
                
                





            }
            return interval + (int)((double)(Int * intMultiplier) * 1000);
        }
        //function to determine bonus
        public static int[] getBuffBonus(Buff buff)
        {
             SkillData curSkills = SkillData.getSkillByNameAndLevel(buff.buffID, buff.buffLevel);
            //{buffbonus,buffbonusability1,buffbonus2,buffability2,buffChangeParam1,buffChangeParam2,buffprobability, buffprobability_PVP}
            int[] listBuffBonus = { 0, 0, 0, 0, 0, 0, 0, 0 };
            listBuffBonus[0] = curSkills.dwAdjParamVal1;
            listBuffBonus[1] = curSkills.dwDestParam1;
            listBuffBonus[2] = curSkills.dwAdjParamVal2;
            listBuffBonus[3] = curSkills.dwDestParam2;
            listBuffBonus[4] = curSkills.dwChangeParamVal1;
            listBuffBonus[5] = curSkills.dwChangeParamVal2;
            listBuffBonus[6] = curSkills.dwProbability;
            listBuffBonus[7] = curSkills.dwProbabilityPVP;
            return listBuffBonus;




            /*int buffBonus = 0, buffBonus2 = 0, buffDestParam1 = 0, buffDestParam2 = 0,buffChangeParam1 = 0,buffChangeParam2 = 0,buffprobability=0,buffprobability_PVP=0;
            //{buffbonus,buffbonusability1,buffbonus2,buffability2,buffChangeParam1,buffChangeParam2,buffprobability, buffprobability_PVP}
            int[] listBuffBonus ={0,0,0,0,0,0,0,0};  
            /*GT have 2 bonus so we need to return an array so for all bonus except GT we 
             * look to listBuffBonus[0] but for GT we need to lokk in 
             * listBuffBonus[0] AND listBuffBonus[1]
            if (!isBuff(buff.buffID))
                return listBuffBonus;
            switch (buff.buffID)
            {
                case BUFF_MENTALSIGN:
                    buffDestParam1 = FlyFFAttributes.DST_INT;
                    buffBonus = buff.buffLevel;
                    break;
                case BUFF_ACCURACY:
                    buffDestParam1 = FlyFFAttributes.DST_ADJ_HITRATE;
                    buffBonus = buff.buffLevel;
                    break;
                case BUFF_BEEFUP:
                    buffDestParam1 = FlyFFAttributes.DST_STR;
                    buffBonus = buff.buffLevel;
                    break;
                case BUFF_CANNONBALL:
                    buffDestParam1 = FlyFFAttributes.DST_DEX;
                    buffBonus = buff.buffLevel;
                    break;
                case BUFF_HEAPUP:
                    buffDestParam1 = FlyFFAttributes.DST_STA;
                    buffBonus = buff.buffLevel * 2;
                    break;
                case BUFF_PATIENCE:
                    buffDestParam1 = FlyFFAttributes.DST_HP_MAX;
                    buffBonus = 40;
                    for (int i = 1; i <= buff.buffLevel; i++)
                    {
                        buffBonus += 5;
                        if (i == 5)
                            buffBonus += 10; //to make +70 for lvl 5...
                        if ((i == 10) || (i == 15))
                            buffBonus += 5; //to make 110 for lvl 10...and 13 lvl 15
                        if (i == 20)
                            buffBonus += 55; //to make 210 to lvl 20...
                    }
                    break;
                case BUFF_CATSREFLEX:
                    buffDestParam1 = FlyFFAttributes.DST_BLOCK_MELEE;
                    buffBonus = buff.buffLevel / 2; //round to inferior so :
                    if (buff.buffLevel % 2 > 0) //for exempla 9/2 = 4 modulo = 1 or lvl 9 =>+5%
                        buffBonus += 1;
                    if (buff.buffLevel == 20)
                        buffBonus += 2;  //so 10+2 = 12% blocking rate ^^
                    break;
                case BUFF_QUICKSTEP:
                    buffDestParam1 = FlyFFAttributes.DST_SPEED;
                    buffBonus = 9;
                    for (int i = 1; i <= buff.buffLevel; i++)
                    {
                        buffBonus += 1;
                    }
                    break;
                case BUFF_HASTE:
                    buffDestParam1 = FlyFFAttributes.DST_ATTACKSPEED;
                    buffBonus = 80;
                    for (int i = 1; i <= buff.buffLevel; i++)
                    {
                        buffBonus += 20;
                        if (i == 4)
                            buffBonus += 20; //to make +180 for lvl 4...
                    }
                    break;
                case BUFF_PROTECT:
                    buffDestParam1 = FlyFFAttributes.DST_ADJDEF;
                    for (int i = 1; i <= buff.buffLevel; i++)
                    {
                        buffBonus += 5;

                    }
                    break;
                case BUFF_HOLYGUARD:
                    buffDestParam1 = FlyFFAttributes.DST_RESIST_MAGIC;
                    for (int i = 1; i <= buff.buffLevel; i++)
                    {
                        buffBonus += 5;
                        
                    }
                    break;
                case BUFF_SF:
                    buffDestParam1 = FlyFFAttributes.DST_CHR_DMG;
                    buffBonus = 40;
                    for (int i = 1; i <= buff.buffLevel; i++)
                    {
                        buffBonus += 10;

                    }
                    break;
                case BUFF_GT: //This case is particular cause GT have 2 bonus !
                    buffDestParam1 = FlyFFAttributes.DST_CHR_DMG;
                    buffDestParam2 = FlyFFAttributes.DST_ATTACKSPEED;

                    for (int i = 1; i <= buff.buffLevel; i++)
                    {
                        buffBonus2 += 50;
                        if (i >= 1 && i <= 5)
                            buffBonus += 20;
                        if (i >= 6 && i <= 10)
                            buffBonus += 10;

                    }
                    break;
                case BUFF_MERC_PROTECTION: //mercenaire protection
                    buffDestParam1 = FlyFFAttributes.DST_ADJDEF;
                    buffBonus = buff.buffLevel;
                    break;
                case BUFF_MERC_PANBARRIER:
                    buffDestParam1 = FlyFFAttributes.DST_BLOCK_RANGE;
                    buffBonus = buff.buffLevel*2;
                    break;
                case BUFF_MERC_IMPOWERWEAPON:
                    buffDestParam1 = FlyFFAttributes.DST_CHR_WEAEATKCHANGE;
                    buffBonus = buff.buffLevel/2; //+1 for each 2 lvl
                    if (buff.buffLevel %2 >0)
                        buffBonus += 1;
                    break;
                case BUFF_MERC_AXEMASTER:
                case BUFF_MERC_SWORDMASTER:
                    buffDestParam1 = FlyFFAttributes.DST_AXE_DMG;
                    buffBonus = buff.buffLevel*5; //+5 for each  lvl
                    break;
                case BUFF_KNI_GUARD:
                    buffDestParam1 = FlyFFAttributes.DST_CHR_DMG;
                    buffBonus = -320 + 20*buff.buffLevel; //+1 for each 2 lvl
                    if (buff.buffLevel>1)
                        buffBonus += 20;
                    buffDestParam2 = FlyFFAttributes.DST_ADJDEF;
                    buffBonus2 = 70 +30*buff.buffLevel;
                    if (buff.buffLevel == 19)
                        buffBonus2 += 20;
                    else if (buff.buffLevel == 20)
                        buffBonus2 += 40;
                    break;
                case BUFF_KNI_RAGE:
                    buffDestParam1 = FlyFFAttributes.DST_HPDMG_UP;
                    buffBonus = 80 + 20*buff.buffLevel; //+1 for each 2 lvl
                    buffDestParam2 = FlyFFAttributes.DST_DEFHITRATE_DOWN;
                    buffBonus2 = 0;
                    for (int i=0;i<buff.buffLevel;i++)
                    {
                        if (i<=4 && i>=0)
                        buffBonus2 -= 5;
                        else
                        buffBonus2 -=4;
                    }
                    if (buff.buffLevel == 20)
                        buffBonus2 +=2;
                    break;
                case BUFF_BLA_BERSERK:
                    buffDestParam1 = FlyFFAttributes.DST_ATTACKSPEED;
                    buffBonus = 30 + 20*buff.buffLevel; //+1 for each 2 lvl
                    buffDestParam2 = FlyFFAttributes.DST_CHR_DMG;
                    buffBonus2 = 50 + 10*buff.buffLevel;
                    break;
                case BUFF_BILL_ASMODEUS :
                    buffDestParam1 = FlyFFAttributes.DST_KNUCKLE_DMG;
                    buffBonus = 58 + 2*buff.buffLevel; //+1 for each 2 lvl
                    break;
                case BUFF_ELE_FIREMASTER:
                    buffDestParam1 = FlyFFAttributes.DST_MASTRY_FIRE;
                    buffBonus = 0;
                    for (int i=0;i<buff.buffLevel;i++)
                    {
                        if (i==1 || i>7)
                            buffBonus += 1;
                        else
                            buffBonus +=2;
                    }
                    break;
                case BUFF_ELE_EARTHMASTER:
                    buffDestParam1 = FlyFFAttributes.DST_MASTRY_EARTH;
                    buffBonus = 0;
                    for (int i=0;i<buff.buffLevel;i++)
                    {
                        if (i==1 || i>7)
                            buffBonus += 1;
                        else
                            buffBonus +=2;
                    }
                    break;
                case BUFF_ELE_WINDMASTER:
                    buffDestParam1 = FlyFFAttributes.DST_MASTRY_WIND;
                    buffBonus = 0;
                    for (int i=0;i<buff.buffLevel;i++)
                    {
                        if (i==1 || i>7)
                            buffBonus += 1;
                        else
                            buffBonus +=2;
                    }
                    break;
                case BUFF_ELE_LIGHTINGMASTER:
                    buffDestParam1 = FlyFFAttributes.DST_MASTRY_ELECTRICITY;
                    buffBonus = 0;
                    for (int i=0;i<buff.buffLevel;i++)
                    {
                        if (i==1 || i>7)
                            buffBonus += 1;
                        else
                            buffBonus +=2;
                    }
                    break;
                case BUFF_ELE_WATERMASTER:
                    buffDestParam1 = FlyFFAttributes.DST_MASTRY_WATER;
                    buffBonus = 0;
                    for (int i=0;i<buff.buffLevel;i++)
                    {
                        if (i==1 || i>7)
                            buffBonus += 1;
                        else
                            buffBonus +=2;
                    }
                    break;
                case BUFF_ACR_FASTWALKER:
                    buffDestParam1 = FlyFFAttributes.DST_SPEED;
                    buffBonus = 10 + 2*buff.buffLevel;
                    break;
                case BUFF_ACR_ABSOLUTEBLOCK:
                    buffDestParam1 = FlyFFAttributes.DST_BLOCK_MELEE;
                    buffBonus = buff.buffLevel;
                    break;
                case BUFF_ACR_YOYOMASTER:
                    buffDestParam1 = FlyFFAttributes.DST_YOYOMASTER_DMG;
                    buffBonus = 14*buff.buffLevel;
                    break;
                case BUFF_ACR_BOWMASTER:
                    buffDestParam1 = FlyFFAttributes.DST_BOWMASTER_DMG;
                    buffBonus = 14*buff.buffLevel;
                    break;
                case BUFF_JST_POISON:
                    buffDestParam1 = FlyFFAttributes.DST_CHR_CHANCEPOISON;
                    buffBonus = 2*buff.buffLevel;
                    buffDestParam2 = FlyFFAttributes.DST_CHR_DMG;
                    buffBonus2 = 10 + 5*buff.buffLevel;
                    buffChangeParam1 = 4069 + 1 * buff.buffLevel;
                    break;
                case BUFF_JST_CRITICALSWING:
                    buffDestParam1 = FlyFFAttributes.DST_CHR_CHANCECRITICAL;
                    buffBonus = 4*buff.buffLevel;
                    break;
                case BUFF_JST_BLEEDING:
                    buffDestParam1 = FlyFFAttributes.DST_CHR_CHANCEBLEEDING;
                    buffBonus = 2 * buff.buffLevel;
                    buffDestParam2 = FlyFFAttributes.DST_CHR_DMG;
                    buffBonus2 = 5 * buff.buffLevel;
                    buffChangeParam1 = 4099 + buff.buffLevel;
                    break;
                case BUFF_JST_ABSORB:
                    buffDestParam1 = FlyFFAttributes.DST_CHR_CHANCESTEALHP;
                    buffBonus = 18 +2 * buff.buffLevel;
                    if (buff.buffLevel > 6)
                        buffBonus += 2;
                    buffDestParam2 = FlyFFAttributes.DST_CHR_DMG;
                    buffBonus2 = 4 + 2 * buff.buffLevel;
                    buffChangeParam1 = 4109 + buff.buffLevel;
                    break;

                /*case BUFF_JST_BACKSTAB:
                    buffDestParam1 = FlyFFAttributes.DST_CHRSTATE;
                    buffBonus = CHS_STUN;
                    buffprobability = 15 + 5*buff.buffLevel;
                    buffprobability_PVP = buffprobability/2;
                    break; //change in state aren't implemented yet...
                case BUFF_RAN_FASTATTACK:
                    buffDestParam1 = FlyFFAttributes.DST_ATTACKSPEED;
                    buffBonus = 50*buff.buffLevel;
                    break;
                case BUFF_RAN_NATURE:
                    buffDestParam1 = FlyFFAttributes.DST_RESIST_MAGIC;
                    buffBonus = 25 + 5*buff.buffLevel;
                    break;
                case BUFF_MERC_SMITEAXE:
                    buffDestParam1 = FlyFFAttributes.DST_AXE_DMG;
                    buffBonus = 0;
                    for (int i = 0; i < buff.buffLevel; i++)
                    {
                        buffBonus += 2;
                        if (buff.buffLevel >= 7)
                            buffBonus += 2;
                        if (buff.buffLevel >= 18)
                            buffBonus += 2;
                        if (buffBonus == 19)
                            buffBonus += 3;
                        if (buffBonus == 20)
                            buffBonus += 6;
                    }
                    buffDestParam2 = FlyFFAttributes.DST_ADJDEF;
                    buffBonus2 = 1 + buff.buffLevel;
                    if (buff.buffLevel == 20)
                        buffBonus2 += 4;
                    break;
                case BUFF_MERC_BLAZINGSWORD:
                    buffDestParam1 = FlyFFAttributes.DST_SWD_DMG;
                    buffBonus = 0;
                    for (int i = 0; i < buff.buffLevel; i++)
                    {
                        buffBonus += 2;
                        if (buff.buffLevel >= 7)
                            buffBonus += 2;
                        if (buff.buffLevel >= 18)
                            buffBonus += 2;
                        if (buffBonus == 19)
                            buffBonus += 3;
                        if (buffBonus == 20)
                            buffBonus += 6;
                    }
                    buffDestParam2 = FlyFFAttributes.DST_ADJ_HITRATE;
                    buffBonus2 = 1 + buff.buffLevel/2;
                    break;
                    

            }
            //Here we put buff bonus value and type of bonus, all bonus except GT need only [0] and [1]
            listBuffBonus[0] =buffBonus;
            listBuffBonus[1] = buffDestParam1;
            listBuffBonus[2] = buffBonus2;
            listBuffBonus[3] = buffDestParam2;
            listBuffBonus[4] = buffChangeParam1;
            listBuffBonus[5] = buffChangeParam2;
            listBuffBonus[6] = buffprobability;
            listBuffBonus[7] = buffprobability_PVP;


            return listBuffBonus;*/
        }
    }
}

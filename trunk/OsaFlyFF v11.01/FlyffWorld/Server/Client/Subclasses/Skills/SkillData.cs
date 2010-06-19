using System;
using System.Collections;
using System.Text;

namespace FlyffWorld
{
    public class SkillData
    {
        public int dwName = 0, //=dwSkillid for skill table and for send packet. Adidishen use the "name" like an id
            dwSkillLvl = 0,
            dwAbilityMin = 0,
            dwAtkAbilityMax = 0,
            dwAbilityMinPVP = 0,
            dwAbilityMaxPVP = 0,
            dwAttackSpeed = 0,
            dwDmgShift = 0,
            dwProbability = 0,
            dwProbabilityPVP = 0,
            dwTaunt = 0,
            dwDestParam1 = 0,
            dwDestParam2 = 0,
            dwAdjParamVal1 = 0,
            dwAdjParamVal2 = 0,
            dwChangeParamVal1 = 0,
            dwChangeParamVal2 = 0,
            dwdestData1 = 0,
            dwdestData2 = 0,
            dwdestData3 = 0,
            dwactiveskill = 0,
            dwActiveSkillRate = 0,
            dwActiveSkillRatePVP = 0,
            dwReqMp = 0,
            dwReqFP = 0,
            dwCooldown = 0,
            dwCastingTime = 0,
            dwSkillRange = 0,
            dwCircleTime = 0,
            dwPainTime = 0,
            dwSkillTime = 0,
            dwSkillCount = 0,
            dwSkillExp = 0,
            dwExp = 0,
            dwComboSkillTime = 0;
        public SkillData()
        {
        }

        public SkillData(int dwName, int dwSkillLvl, int dwAbilityMin, int dwAtkAbilityMax, int dwAbilityMinPVP, int dwAbilityMaxPVP, int dwAttackSpeed, int dwDmgShift, int dwProbability, int dwProbabilityPVP, int dwTaunt, int dwDestParam1, int dwDestParam2, int dwAdjParamVal1, int dwAdjParamVal2, int dwChangeParamVal1, int dwChangeParamVal2, int dwdestData1, int dwdestData2, int dwdestData3, int dwactiveskill, int dwActiveSkillRate, int dwActiveSkillRatePVP, int dwReqMp, int intdwReqFP, int dwCooldown, int dwCastingTime, int dwSkillRange, int dwCircleTime, int dwPainTime, int dwSkillTime, int dwSkillCount, int dwSkillExp, int dwExp, int dwComboSkillTime)
        {
            this.dwName = dwName;
            this.dwSkillLvl = dwSkillLvl;
            this.dwAbilityMin = dwAbilityMin;
            this.dwAtkAbilityMax = dwAtkAbilityMax;
            this.dwAbilityMinPVP = dwAbilityMinPVP;
            this.dwAbilityMaxPVP = dwAbilityMaxPVP;
            this.dwAttackSpeed = dwAttackSpeed;
            this.dwDmgShift = dwDmgShift;
            this.dwProbability = dwProbability;
            this.dwProbabilityPVP = dwProbabilityPVP;
            this.dwTaunt = dwTaunt;
            this.dwDestParam1 = dwDestParam1;
            this.dwDestParam2 = dwDestParam2;
            this.dwAdjParamVal1 = dwAdjParamVal1;
            this.dwAdjParamVal2 = dwAdjParamVal2;
            this.dwChangeParamVal1 = dwChangeParamVal1;
            this.dwChangeParamVal2 = dwChangeParamVal2;
            this.dwdestData1 = dwdestData1;
            this.dwdestData2 = dwdestData2;
            this.dwdestData3 = dwdestData3;
            this.dwactiveskill = dwactiveskill;
            this.dwActiveSkillRate = dwActiveSkillRate;
            this.dwActiveSkillRatePVP = dwActiveSkillRatePVP;
            this.dwReqMp = dwReqMp;
            this.dwReqFP = dwReqFP;
            this.dwCooldown = dwCooldown;
            this.dwCastingTime = dwCastingTime;
            this.dwSkillRange = dwSkillRange;
            this.dwCircleTime = dwCircleTime;
            this.dwPainTime = dwPainTime;
            this.dwSkillTime = dwSkillTime;
            this.dwSkillCount = dwSkillCount;
            this.dwSkillExp = dwSkillExp;
            this.dwExp = dwExp;
            this.dwComboSkillTime = dwComboSkillTime;


        }
        public static SkillData getSkillByNameAndLevel(int name, int level)
        {
            
            for (int i = 0; i < WorldServer.data_skills.Count; i++)
            {
                SkillData curSkill = WorldServer.data_skills[i];
                if ((curSkill.dwName == name) && (curSkill.dwSkillLvl == level))
                {
                    
                    return curSkill;
                }
            }
                
            return null;
        }


//---------------------------------------------------------------------------//
// Those function will allow us to order skill in group
//---------------------------------------------------------------------------//

        public static int[] attackSkillTable = new int[]{//skill that will attack target(s)
        //here i will put skillName (=skillid for adidishen)
        1,2,3,104,117,4,5,112,105,6,14,11,12,194,197,198,
        201,196,203,204,202,200,205,121,64,119,118,69,32,
        34,36,65,70,30,31,33,35,133,134,132,135,131,138,
        140,137,139,141,142,136,209,210,211,212,214,217,
        219,216,218,220,222,151,152,154,153,158,155,159,
        160,162,164,166,167,174,168,171,177,180,173,176,
        170,179,182,183,184,185,186
        
        };
        public static bool isAttackSkill(int skillID) //look if the skill is an attack
        {
            for (int i = 0; i < attackSkillTable.Length; i++)
                if (attackSkillTable[i] == skillID)
                    return true;
            return false;
        }
        public static int[] buffWithEffectOnOtherTable = new int[]{
            129,165, 163, 193,113,208
        //stone hand, power stamp, pain reflextion, Enchant poison, crucio, psychic wall, dark illusion
        
        
        };
        public static bool isBuffWithEffectOnOther(int skillID) //buff that have an effect on other player when fighting
        {
            for (int i = 0; i < buffWithEffectOnOtherTable.Length; i++)
                if (buffWithEffectOnOtherTable[i] == skillID)
                    return true;
            return false;
        }
        public static int[] aoeTable = new int[]{
            105,132, 140, 139,141,218,151,153,158,164,166,167,173,176,170,179,183,184,185,186
        //all aoe skill are here
        
        
        };
        public static bool isaoeSkill(int skillID) //skills that are AOE
        {
            for (int i = 0; i < aoeTable.Length; i++)
                if (aoeTable[i] == skillID)
                    return true;
            return false;
        }
        public static int[] AttackWithEffectTable = new int[]{
            112,197, 201, 203,202,205,118,65,70,30,33,35,134,135,131,140,209,210,211,212,
            214,217,219,216,220,222,154,162,
        //all attack which have an effect on target are here
        
        
        };
        public static bool isAttackWithEffect(int skillID) //search for attack skill that have an effect against target
        {
            for (int i = 0; i < AttackWithEffectTable.Length; i++)
                if (AttackWithEffectTable[i] == skillID)
                    return true;
            return false;
        }
//-------------------------------------------------------------------//
//   We determine skill type to know what we have to do with it
//-------------------------------------------------------------------//

        public static string skillType (int skillID) //determine type of skill that was cast
        {
            string type ="";
            

            if (BuffDB.isBuff(skillID))
                type = "buff";
            else if (isAttackSkill(skillID))
                type = "attack";
            else if (skillID == 44 || skillID == 51 || skillID == 144)
                type = "heal";
            else if (skillID == 45)
                type = "resurrect";
            else if (skillID == 107) //blinkpool
                type = "teleport";
            else if (skillID == 213)//escape
                type = "debuff";
            else if (skillID == 149)//Gvur tialla
                type = "cure";
            else if (isBuffWithEffectOnOther(skillID))
                type = "buffwitheffect";
            else if (skillID == 13 || skillID == 206 || skillID == 37 || skillID == 145 || skillID == 157 || skillID == 161)
                type = "antibuff";
            else
                type = "unknown";

            return type;

        }

    }

}


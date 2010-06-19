using System;
using System.Collections;
using System.Text;

namespace FlyffWorld
{
    public partial class Skills
    {
        #region Define variables
        /// <summary>
        /// define wariable used in Skills
        /// </summary>
        public int dwNameID = 0, //=dwSkillid for skill table and for send packet. Adidishen use the "name" like an id
            dwSkillLvl = 0,
            dwAbilityMin = 0,
            dwAbilityMax = 0,
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
        
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor functions
        /// </summary>
        public Skills()
        {
        }

        public Skills(int dwName, int dwSkillLvl, int dwAbilityMin, int dwAtkAbilityMax, int dwAbilityMinPVP, int dwAbilityMaxPVP, int dwAttackSpeed, int dwDmgShift, int dwProbability, int dwProbabilityPVP, int dwTaunt, int dwDestParam1, int dwDestParam2, int dwAdjParamVal1, int dwAdjParamVal2, int dwChangeParamVal1, int dwChangeParamVal2, int dwdestData1, int dwdestData2, int dwdestData3, int dwactiveskill, int dwActiveSkillRate, int dwActiveSkillRatePVP, int dwReqMp, int dwReqFP, int dwCooldown, int dwCastingTime, int dwSkillRange, int dwCircleTime, int dwPainTime, int dwSkillTime, int dwSkillCount, int dwSkillExp, int dwExp, int dwComboSkillTime)
        {
            this.dwNameID = dwName;
            this.dwSkillLvl = dwSkillLvl;
            this.dwAbilityMin = dwAbilityMin;
            this.dwAbilityMax = dwAtkAbilityMax;
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
        #endregion
        /// <summary>
        /// Return Skills (and all data) from data_skill table
        /// </summary>
        /// <param name="name">Id of the skill</param>
        /// <param name="level">Level of the skill</param>
        /// <returns></returns>
        public static Skills getSkillByNameIDAndLevel(int name, int level)
        {
            Skills skill = new Skills();
            for (int i = 0; i < WorldServer.data_skills.Count; i++)
            {
                
                skill = WorldServer.data_skills[i];

                if (skill.dwNameID == name)
                {
                    if (skill.dwSkillLvl == level)
                        return skill;
                }
                
            }
            Log.Write(Log.MessageType.debug, "don't have find the skills {0} with level {1} in flyff_data_skills",name,level);
            return null;
        }
        #region Put the skills into a group

        #region Table with skillnameid
        /// <summary>
        /// Table with skillname and correspond to type of skills
        /// </summary>

        public static int[] attackSkillTable = new int[]{//skill that will attack target(s)
        //here i will put skillName (=skillid for adidishen)
        1,2,3,14,104,117,4,5,112,6,11,12,194,197,198,
        201,196,203,204,202,200,205,121,64,119,118,69,32,
        34,36,65,70,30,31,33,35,133,134,131,138,
        137,142,136,209,210,211,212,214,
        216,220,222,152,154,155,159,
        160,162,174,168,171,177,180,
        182,219
        
        };
        public static int[] MagicAttackSkills = new int[]
        {
            30, 31, 32, 33, 34, 35, 36, 64, 65, 69, 70,118, 119, 120, 121,   
                160, 162, 163, 165,168,169,171,172,174,175,177,178,180,181,182
        };                
        
        public static int[] aoeTable = new int[]{
            105,132,135, 140, 139,141,218,151,153,158,164,166,167,173,176,170,179,183,184,185,186,217,218
        //all aoe skill are here
                
        };
        
        public static int[] antibuffTable = new int[]{
            13,37,145,157,161,239,241,242
        //all skill that decrease target capacity
        };
        public static int[] buffSkillTable = new int[]{ // Has all skills that are buffs in it. used to determine if a skill is a buff or not.
            46,  48,  49,  50,
            52,  53,  113, 114,
            115, 116, 20,  146,
            147, 148,  9, 165,113,
            10, 111, 128, 129, 130,
            143, 156, 169, 175,
            181, 172, 178, 195,
            199, 191, 192, 208,
            207, 215, 221, 8, 7,
            109, 108, 209, 210,
            208, 309,310,311,312,
            313,314,315,237,240,193

        };
        #endregion

        #region Test Type of Skills
        /// <summary>
        /// Test if a skill is a Buff (good buff)
        /// </summary>
        /// <param name="skillID">Id of the skill we want to test.</param>
        /// <returns></returns>
        public static bool isBuff(int skillID)
        {
            for (int i = 0; i < buffSkillTable.Length; i++)
                if (buffSkillTable[i] == skillID)
                    return true;
            return false;
        }
        /// <summary>
        /// Test if a skill is an Attack skill (not magical attack (good buff)
        /// </summary>
        /// <param name="skillID">Id of the skill we want to test.</param>
        /// <returns></returns>
        public static bool isAttackSkill(int skillID) //look if the skill is an attack
        {
            for (int i = 0; i < attackSkillTable.Length; i++)
                if (attackSkillTable[i] == skillID)
                    return true;
            return false;
        }
        /// <summary>
        /// Test if a skill is a magical attack (good buff)
        /// </summary>
        /// <param name="skillID">Id of the skill we want to test.</param>
        /// <returns></returns>
        public static bool isMagicAttackSkill(int skillID)
        {
            for (int i = 0; i < MagicAttackSkills.Length; i++)
                if (MagicAttackSkills[i] == skillID)
                    return true;
            return false;
        }
       
        /// <summary>
        /// Test if a skill is an AOE type (good buff)
        /// </summary>
        /// <param name="skillID">Id of the skill we want to test.</param>
        /// <returns></returns>
        public static bool isaoeSkill(int skillID) //skills that are AOE
        {
            for (int i = 0; i < aoeTable.Length; i++)
                if (aoeTable[i] == skillID)
                    return true;
            return false;
        }/// <summary>
        /// Test if a skill is a Bad Buff (good buff)
        /// </summary>
        /// <param name="skillID">Id of the skill we want to test.</param>
        /// <returns></returns>
        public static bool isAntiBuff(int skillID) //skills that decrease target capacity
        {
            for (int i = 0; i < antibuffTable.Length; i++)
                if (antibuffTable[i] == skillID)
                    return true;
            return false;
        }
        
        #endregion

        #region Get Skill Type
        /// <summary>
        /// Test type of the skill and return a string
        /// </summary>
        /// <param name="skillID">Id of the skill we want to test.</param>
        /// <returns></returns>
        public static string skillType(int skillID) //determine type of skill that was cast
        {
            string type = "";


            if (Skills.isBuff(skillID))
                type = "buff";
            else if (isAttackSkill(skillID)&& (!isaoeSkill(skillID))&&(!isMagicAttackSkill(skillID)))
                type = "attack";
            else if (isaoeSkill(skillID))
                type = "AoE";
            else if (skillID == 44 || skillID == 51 || skillID == 144)
                type = "heal";
            else if (skillID == 45)
                type = "resurrect";
            else if (skillID == 150 || skillID == 316)
                type = "partyS";
            else if (skillID == 107 || skillID == 310 || skillID == 244 || skillID == 206 || skillID == 238) //blinkpool or summon or return
                type = "teleport";
            else if (skillID == 213|| skillID == 243)//escape | disenchant
                type = "debuff";
            else if (skillID == 149 )//Gvur tialla or blessing
                type = "cure";            
            else if (isAntiBuff(skillID))
                type = "antibuff";
            else if (isMagicAttackSkill(skillID))
                type = "MagicalAttack";
            else
                type = "unknown";

            return type;

        }
        #endregion
        #endregion

        #region return Element of a skill
        public static int[] FireSkill = new int[]{//skill that will use fire
        //here i will put skillName (=skillid for adidishen)
        64,65,30,217,168,170,183        
        };
        public static int[] WaterSkill = new int[]{//skill that will use fire
        //here i will put skillName (=skillid for adidishen)
        118,32,33,216,177,186        
        };
        public static int[] EarthSkill = new int[]{//skill that will use fire
        //here i will put skillName (=skillid for adidishen)
        120,36,37,174,176,185        
        };
        public static int[] LigthningSkill = new int[]{//skill that will use fire
        //here i will put skillName (=skillid for adidishen)
        119,34,35,171,173,184        
        };
        public static int[] WindgSkill = new int[]{//skill that will use fire
        //here i will put skillName (=skillid for adidishen)
        31,69,70,180,182        
        };
        public static bool isFire(int skillID)
        {
            for (int i = 0; i < FireSkill.Length; i++)
                if (FireSkill[i] == skillID)
                    return true;
            return false;
        }
        public static bool isWater(int skillID)
        {
            for (int i = 0; i < WaterSkill.Length; i++)
                if (WaterSkill[i] == skillID)
                    return true;
            return false;
        }
        public static bool isWind(int skillID)
        {
            for (int i = 0; i < WindgSkill.Length; i++)
                if (WindgSkill[i] == skillID)
                    return true;
            return false;
        }
        public static bool isEarth(int skillID)
        {
            for (int i = 0; i < EarthSkill.Length; i++)
                if (EarthSkill[i] == skillID)
                    return true;
            return false;
        }
         public static bool isLightning(int skillID)
        {
            for (int i = 0; i < LigthningSkill.Length; i++)
                if (LigthningSkill[i] == skillID)
                    return true;
            return false;
        }
         /// <summary>
         /// Return type of element of a skill 
         /// </summary>
         /// <param name="skills">skill we want to test</param>
         /// <returns></returns>
         public static int GetElementType(Skills skills)
         {
             if (isFire(skills.dwNameID)) return 1;
             else if (isWater(skills.dwNameID)) return 2;
             else if (isLightning(skills.dwNameID)) return 3;
             else if (isWind(skills.dwNameID)) return 4;
             else if (isEarth(skills.dwNameID)) return 5;
             else return 0;
         }
        #endregion

    }
    public class Buff
    {
        public Skills _skill = null;
        public int dwTime;
        public long qwExpirationDate = DLL.clock();

        public static void ClearAllBuffs(Client client)
        {
            for (int i = client.c_data.buffs.Count - 1; i >= 0; i--) //we must delete all buff
            {
                Buff curBuff = client.c_data.buffs[i];
                curBuff.dwTime = 0;
                Skills skill = curBuff._skill;
                client.SendPlayerBuff(curBuff);
                client.c_attributes[skill.dwDestParam1] -= skill.dwAdjParamVal1;
                if (skill.dwDestParam1 < 0) //if we decrease something
                    client.SendMoverAttribRaise(skill.dwDestParam1, Math.Abs(skill.dwAdjParamVal1), -1);
                    
                else
                    client.SendMoverAttribDecrease(skill.dwDestParam1, skill.dwAdjParamVal1);
                if (skill.dwDestParam2 != 0)//if there is a second effect
                {
                    client.c_attributes[skill.dwDestParam2] -= skill.dwAdjParamVal2;
                    if (skill.dwDestParam2 < 0) //if we decrease something
                        client.SendMoverAttribRaise(skill.dwDestParam2, Math.Abs(skill.dwAdjParamVal2), -1);
                        
                    else
                        client.SendMoverAttribDecrease(skill.dwDestParam2, skill.dwAdjParamVal2);
                        
                }
                
                client.c_data.buffs.Remove(curBuff);


            }


        }
        public static void ClearABuff(Client client,int buffID)
        {
            for (int i = 0; i >client.c_data.buffs.Count; i++)
            {
                Buff curBuff = client.c_data.buffs[i];
                if (curBuff._skill.dwNameID == buffID)
                {
                    curBuff.dwTime = 0;
                    Skills skill = curBuff._skill;
                    
                    //we remove buff so we do the contrary of when we buff player
                    client.c_attributes[skill.dwDestParam1] -= skill.dwAdjParamVal1;
                    Log.Write(Log.MessageType.debug, "skill para1 {0} skill dest param 1 {1} skill param 2 {2} skill dest param 2 {3}", skill.dwAdjParamVal1, skill.dwDestParam1, skill.dwAdjParamVal2, skill.dwDestParam2);
                    if (skill.dwDestParam1 < 0) //if we decrease something
                        client.SendMoverAttribRaise(skill.dwDestParam1, Math.Abs(skill.dwAdjParamVal1), -1);

                    else
                        client.SendMoverAttribDecrease(skill.dwDestParam1, skill.dwAdjParamVal1);
                    if (skill.dwDestParam2 != 0)//if there is a second effect
                    {
                        client.c_attributes[skill.dwDestParam2] -= skill.dwAdjParamVal2;
                        if (skill.dwDestParam2 < 0) //if we decrease something
                            client.SendMoverAttribRaise(skill.dwDestParam2, Math.Abs(skill.dwAdjParamVal2), -1);

                        else
                            client.SendMoverAttribDecrease(skill.dwDestParam2, skill.dwAdjParamVal2);

                    }
                    client.SendPlayerBuff(curBuff);
                    client.c_data.buffs.Remove(curBuff);
                }

            }


        }
    }
    public class AntiBuff
    {
        public Buff _buff = null;
        public int dwMoverID = -1;
    }



}


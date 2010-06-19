using System;
using System.Collections.Generic;
using System.Text;

namespace FlyffWorld
{
    public class SkillFormulas
    {
        public static bool GetDamageFromSkillFormula(CPSUData PSUData, ref int Damage)
        {
            switch (PSUData.SkillName)
            {
                case SkillNames.SkillList.Asalraalaikum:  //not good even don't take effect from ability of the skill and his lvl
                    {
                        int mp = PSUData.Caster.c_attributes[FlyFF.DST_MP];
                        PSUData.Caster.SendPlayerAttribSet(FlyFF.DST_MP, 0);
                        PSUData.Caster.c_attributes[FlyFF.DST_MP] = 0;
                        int str = PSUData.Caster.c_attributes[FlyFF.DST_STR];
                        Damage += (int)(str / 10 * mp + 3000 * 0.60);
                        return true;
                    }
                #region Mercenary skills and vagrant
                case SkillNames.SkillList.CleanHit:
                case SkillNames.SkillList.Slash:
                case SkillNames.SkillList.Brandish:                
                case SkillNames.SkillList.OverCut:
                case SkillNames.SkillList.Blindside:
                case SkillNames.SkillList.SpecialHit:                
                case SkillNames.SkillList.BloodyStrike:
                case SkillNames.SkillList.Guilotine:
                    
                        int minvalue = PSUData.SkillData.dwAbilityMin * 6 + 10 * PSUData.SkillData.dwSkillLvl;
                        int maxvalue = PSUData.SkillData.dwAbilityMax * 6 + 10 * PSUData.SkillData.dwSkillLvl;
                        int value = DiceRoller.RandomNumber(minvalue, maxvalue);
                        Damage += value;
                        return true;
                
                case SkillNames.SkillList.HitReflect: //need to add % damage taken last time
                        minvalue = PSUData.SkillData.dwAbilityMin * 20 + 20 * PSUData.SkillData.dwSkillLvl;
                        maxvalue = PSUData.SkillData.dwAbilityMax * 20 + 20 * PSUData.SkillData.dwSkillLvl;
                        value = DiceRoller.RandomNumber(minvalue, maxvalue);
                        Damage += value;
                        return true;
                case SkillNames.SkillList.Keenwheel:
                        minvalue = PSUData.SkillData.dwAbilityMin * 135 + 10 * PSUData.SkillData.dwSkillLvl;
                        maxvalue = PSUData.SkillData.dwAbilityMax * 135 + 10 * PSUData.SkillData.dwSkillLvl;
                        value = DiceRoller.RandomNumber(minvalue, maxvalue);
                        Damage += value;
                        Damage /= 3; //damage divised by the number of attack
                        return true;
                #endregion
                #region  Assist skill
                case SkillNames.SkillList.StraightPunch:
                case SkillNames.SkillList.PowerFist:
                case SkillNames.SkillList.BurstCrack:
                         minvalue = PSUData.SkillData.dwAbilityMin * 10 + 20 * PSUData.SkillData.dwSkillLvl;
                         maxvalue = PSUData.SkillData.dwAbilityMax * 10 + 20 * PSUData.SkillData.dwSkillLvl;
                         value = DiceRoller.RandomNumber(minvalue, maxvalue);
                        Damage += value;
                        return true;
                #endregion
                #region  Mage
                case SkillNames.SkillList.MentalStrike:
                case SkillNames.SkillList.FlameBall:
                case SkillNames.SkillList.FlameGeyser:
                case SkillNames.SkillList.FireStrike:
                case SkillNames.SkillList.Swordwind:
                case SkillNames.SkillList.Strongwind:
                case SkillNames.SkillList.WindCutter:
                case SkillNames.SkillList.IceMissile:
                case SkillNames.SkillList.Waterball:
                case SkillNames.SkillList.WaterWell:
                case SkillNames.SkillList.StaticBall:
                case SkillNames.SkillList.LightningBall:
                case SkillNames.SkillList.LightningShock:
                case SkillNames.SkillList.StoneSpike:
                case SkillNames.SkillList.Rockcrash:
                case SkillNames.SkillList.Looting:
                        Damage += 20 * PSUData.SkillData.dwSkillLvl;
                        return true;
                #endregion               
                #region Acrobat skills type yoyo and bow
                case SkillNames.SkillList.SlowStep:
                case SkillNames.SkillList.CounterAttack:          
                case SkillNames.SkillList.CrossLine:
                case SkillNames.SkillList.JunkArrow:
                
                case SkillNames.SkillList.AutoShot:   
                        minvalue = PSUData.SkillData.dwAbilityMin * 5 + 15 * PSUData.SkillData.dwSkillLvl;
                        maxvalue = PSUData.SkillData.dwAbilityMax * 5 + 15 * PSUData.SkillData.dwSkillLvl;
                        value = DiceRoller.RandomNumber(minvalue, maxvalue);
                        Damage += value;
                        return true;
                case SkillNames.SkillList.Snitch:
                        minvalue = PSUData.SkillData.dwAbilityMin * 2 + 36 * PSUData.SkillData.dwSkillLvl;
                        maxvalue = PSUData.SkillData.dwAbilityMax * 2 + 36 * PSUData.SkillData.dwSkillLvl;
                        value = DiceRoller.RandomNumber(minvalue, maxvalue);
                        Damage += value;
                        return true;
                case SkillNames.SkillList.DeadlySwing: //temporary to recalculate with a character on fame
                        minvalue = PSUData.SkillData.dwAbilityMin * 5 + 55 * PSUData.SkillData.dwSkillLvl;
                        maxvalue = PSUData.SkillData.dwAbilityMax * 5 + 55 * PSUData.SkillData.dwSkillLvl;
                        value = DiceRoller.RandomNumber(minvalue, maxvalue);
                        Damage += value;
                        return true;
                case SkillNames.SkillList.AimedShot:
                        //skill is done like if yo don't success probability damaged is /2 
                        //so if you success it's like if you make damage *2
                        //i do this like official on official lvl 1 skill do less damage than normal attack... so they /2 damage...
                        minvalue = PSUData.SkillData.dwAbilityMin * 10 + 70 * PSUData.SkillData.dwSkillLvl;
                        maxvalue = PSUData.SkillData.dwAbilityMax * 10 + 70 * PSUData.SkillData.dwSkillLvl;
                        value = DiceRoller.RandomNumber(minvalue, maxvalue);
                        Damage += value;
                    if (!DiceRoller.Roll(PSUData.Skill.dwProbability)) 
                        Damage = Damage/2;
                        return true;
                case SkillNames.SkillList.SilentShot:
                case SkillNames.SkillList.ArrowRain:
                        minvalue = PSUData.SkillData.dwAbilityMin * 10 + 50 * PSUData.SkillData.dwSkillLvl;
                        maxvalue = PSUData.SkillData.dwAbilityMax * 10 + 50 * PSUData.SkillData.dwSkillLvl;
                        value = DiceRoller.RandomNumber(minvalue, maxvalue);
                        Damage += value;
                        Damage = Damage / 2;
                        return true;
                

                #endregion 
                #region  Billposter skill
                case SkillNames.SkillList.BelialSmashing:
                case SkillNames.SkillList.PiercingSerpent:                
                case SkillNames.SkillList.Sonichand:
                case SkillNames.SkillList.BgvurTialbold:
                        minvalue = PSUData.SkillData.dwAbilityMin * 6 + 55 * PSUData.SkillData.dwSkillLvl;
                        maxvalue = PSUData.SkillData.dwAbilityMax * 6 + 55 * PSUData.SkillData.dwSkillLvl;
                        value = DiceRoller.RandomNumber(minvalue, maxvalue);
                        Damage += value;
                        return true;
                case SkillNames.SkillList.BloodFist:
                        minvalue = PSUData.SkillData.dwAbilityMin * 4 + 55 * PSUData.SkillData.dwSkillLvl;
                        maxvalue = PSUData.SkillData.dwAbilityMax * 4 + 55 * PSUData.SkillData.dwSkillLvl;
                        value = DiceRoller.RandomNumber(minvalue, maxvalue);
                        Damage += value;
                        return true;
                #endregion
                #region  jester skill
                case SkillNames.SkillList.PenyaStrike:
                        minvalue = PSUData.SkillData.dwAbilityMin * 75 + 50 * PSUData.SkillData.dwSkillLvl;
                        maxvalue = PSUData.SkillData.dwAbilityMax * 75 + 50 * PSUData.SkillData.dwSkillLvl;
                        value = DiceRoller.RandomNumber(minvalue, maxvalue);
                        Damage += value;
                        return true;
                case SkillNames.SkillList.BackStab:
                case SkillNames.SkillList.VitalStab:  //provisory formula need to make test :p
                
                        minvalue = PSUData.SkillData.dwAbilityMin * 10 + 50 * PSUData.SkillData.dwSkillLvl;
                        maxvalue = PSUData.SkillData.dwAbilityMax * 10 + 50 * PSUData.SkillData.dwSkillLvl;
                        value = DiceRoller.RandomNumber(minvalue, maxvalue);
                        Damage += value;
                        return true;
                #endregion
                #region Ranger skills
                case SkillNames.SkillList.IceArrow:
                case SkillNames.SkillList.FlameArrow:
                case SkillNames.SkillList.PiercingArrow:
                case SkillNames.SkillList.PoisonArrow:
                case SkillNames.SkillList.SilentArrow:
                case SkillNames.SkillList.TripleShot:
                        minvalue = PSUData.SkillData.dwAbilityMin * 8 + 85 * PSUData.SkillData.dwSkillLvl;
                        maxvalue = PSUData.SkillData.dwAbilityMax * 8 + 85 * PSUData.SkillData.dwSkillLvl;
                        value = DiceRoller.RandomNumber(minvalue, maxvalue);
                        Damage += value;
                        return true;
                #endregion 
                #region Psykeeper All skills formula test unless psychic square
                case SkillNames.SkillList.Demonology:
                case SkillNames.SkillList.PsychicBomb:
                case SkillNames.SkillList.PsychicSquare: 

                        Damage += 30 + 80 * (PSUData.SkillData.dwSkillLvl - 1);
                        return true;
                case SkillNames.SkillList.SpiritBomb:
                    int damage_factor = 100;
                    if (PSUData.Caster.c_attributes[FlyFF.DST_MP] <= (PSUData.Caster.c_data.f_MaxMP * 80 / 100) && PSUData.Caster.c_attributes[FlyFF.DST_MP] > (PSUData.Caster.c_data.f_MaxMP * 50 / 100))
                        damage_factor = 80;
                    if (PSUData.Caster.c_attributes[FlyFF.DST_MP] < (PSUData.Caster.c_data.f_MaxMP * 50 / 100))
                        damage_factor = 67;
                    Damage += (int)((7.5 * (double)PSUData.Skill.dwAbilityMin + 7.5 * (double)PSUData.Skill.dwAbilityMax) / 2 + (double)PSUData.SkillData.dwSkillLvl * 134.5);
                    Damage = (int)((double)Damage * (double)damage_factor / 100);
                    return true;
                case SkillNames.SkillList.PsychicWall:

                    Damage -= (int)PSUData.Caster.c_data.f_ATKValue();
                    Damage -= 10 * PSUData.Skill.dwAbilityMax; //we remove damage add in magicalattackskill
                    Damage += 3 * PSUData.Skill.dwAbilityMin + 4 * PSUData.Skill.dwSkillLvl;
                    return true;
                case SkillNames.SkillList.MaximumCrisis:
                    Damage += PSUData.Skill.dwSkillLvl * 66;
                    return true;
                    
                #endregion   
                #region RingMaster Temporary Formula
                case SkillNames.SkillList.MerkabaHanzelrusha:


                        Damage += (PSUData.Caster.c_attributes[FlyFFAttributes.DST_INT] - 15) * 4 - (int)Math.Floor(2.2d * (PSUData.Caster.c_attributes[1] - 14));//we remove effect of strengh in ATK value
                        return true;
                #endregion   
                #region Blade skills temporary formulas
                case SkillNames.SkillList.CrossStrike:
                case SkillNames.SkillList.SonicBlade:
                case SkillNames.SkillList.SpringAttack:
                case SkillNames.SkillList.SilentStrike:
                case SkillNames.SkillList.BladeDance:
                case SkillNames.SkillList.HawkAttack:
                

                        minvalue = PSUData.SkillData.dwAbilityMin * 5;
                        maxvalue = PSUData.SkillData.dwAbilityMax * 5;
                        value = DiceRoller.RandomNumber(minvalue, maxvalue);
                        Damage += value;
                        return true;
                case SkillNames.SkillList.ArmorPenetrate:
                        minvalue = PSUData.SkillData.dwAbilityMin * 5;
                        maxvalue = PSUData.SkillData.dwAbilityMax * 5;
                        value = DiceRoller.RandomNumber(minvalue, maxvalue)+ PSUData.Target.AsMonster.Data.def/2; //+50% of mob defense (effect of the skill)
                        Damage += value;
                        return true;

                #endregion 
                #region Knight skills Temporary formula
                case SkillNames.SkillList.PainDealer:
                        minvalue = PSUData.SkillData.dwAbilityMin * 1;
                        maxvalue = PSUData.SkillData.dwAbilityMax * 1;
                        value = DiceRoller.RandomNumber(minvalue, maxvalue);
                        Damage += value;
                        return true;

                case SkillNames.SkillList.Charge:               
                case SkillNames.SkillList.EarthDivider:
                case SkillNames.SkillList.PowerSwing:

                        minvalue = PSUData.SkillData.dwAbilityMin * 11;
                        maxvalue = PSUData.SkillData.dwAbilityMax * 11;
                    if (minvalue>maxvalue)
                        value = DiceRoller.RandomNumber(maxvalue, minvalue);
                    else
                        value = DiceRoller.RandomNumber(minvalue, maxvalue);
                        Damage += value;
                        return true;

                #endregion
                #region Elementor skills Temporary formula
                
                case SkillNames.SkillList.Firebird:
                case SkillNames.SkillList.StoneSpear:
                case SkillNames.SkillList.Void:
                case SkillNames.SkillList.ThunderStrike:
                case SkillNames.SkillList.Iceshark:
                case SkillNames.SkillList.Burningfield:
                case SkillNames.SkillList.Earthquake:
                case SkillNames.SkillList.Windfield:
                case SkillNames.SkillList.ElectricShock:
                case SkillNames.SkillList.PoisonCloud:
                case SkillNames.SkillList.MeteoShower:
                case SkillNames.SkillList.Sandstorm:
                case SkillNames.SkillList.LightningStorm:
                case SkillNames.SkillList.Blizzard:

                        Damage += PSUData.SkillData.dwAbilityMin * 20; //10 for normal magic skill +20* for elementor skill
                        return true;

                #endregion
                case SkillNames.SkillList.EFFECT_FRAMEARROW_Burn:
                        minvalue = PSUData.SkillData.dwAbilityMin * 15;
                        maxvalue = PSUData.SkillData.dwAbilityMax * 15;
                        value = DiceRoller.RandomNumber(minvalue, maxvalue);
                        Damage += value;
                        return true;
                
                        
            }
            return false;
        }
    }
}

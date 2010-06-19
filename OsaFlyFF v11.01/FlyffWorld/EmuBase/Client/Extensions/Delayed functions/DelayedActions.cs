using System;
using System.Collections.Generic;
using System.Text;

namespace FlyffWorld
{
    public class DelayedActions
    {
        /* [Divinepunition]
         * Ok so while skills need to send damage after skill motin effect
         * actionslot need to send castskills with delay between them
         * effect on mover need to be remover after a delay (and we don't need to read all the list of mover each 0.1seconds...
         * I had created a thread to manage delayed action. Each time we need to make a delayed action, we create structure with parameters needed
         * create a list of this structure, and add in delayedactionthread to read this list
         * each time an action is done it's removed from the list, and while list are reordered we need to read the list from
         * the end to the beginning. This class give us the structures and functions we need
         * */
        #region structures
        /// <summary>
        /// Structure to get parameters to send damage
        /// </summary>
        /// <param name="itemID">The item ID of the item to search for.</param>
        /// <returns></returns>
        public struct str_delayedSendDamage
        {
            public Mover target;
            public Mover caster;
            public int damage;
            public AttackFlags attackFlag;            
            
        }
        /// <summary>
        /// Get parameter to cast a skill with delay.
        /// </summary>
        /// <param name="itemID">The item ID of the item to search for.</param>
        /// <returns></returns>
        public struct str_castskill
        {
            public Mover target;
            public Client caster;
            public Skills skill;
            public bool bislastskill;
            public int order;
            public int actionslotID;
            public double calltime; //used to know when we call it (DLL.Time+skillcasttime)
            str_castskill(Mover target, Client caster, Skills skill, int actionslotID,int order, bool bislastskill, long calltime)
            {
                this.target = target;
                this.caster = caster;
                this.skill = skill;
                this.actionslotID = actionslotID;
                this.bislastskill = bislastskill;
                this.order = order;
                this.calltime = calltime;
            }
        }
        /// <summary>
        /// Structure used to keep info of delayer list content. When we add a delayed action in delayer then we will add 
        /// it in a list stored for each mover (mob or client can have it like this.
        /// </summary>
        /// <param name="ulong Index">The index position of the delayed action in delayer list.</param>
        /// <param name="string CallMethodeName">It the name of the methode called in the delayed action, helpfull to search it.</param>
        /// <param name="int Refvalue">A reference value, depend of type of methode called, for skill it will be skillID, for damage, dmg value....</param>
        /// <returns></returns>
        public struct Delaycontent
        {
            public ulong Index;//index in delayer list
            public int MethodName;
            public int Refvalue; //for a buff or a skill it will be skillID ,for a damage it will be dmg value etc.            
        }
        #endregion

        public const int  METHOD_CALLSKILL = 1,
                          METHOD_SENDAMAGE =2,
                          METHOD_REMOVEBUFF = 3,
                          METHOD_REMOVEANTIBUFF = 4;
        /// <summary>
        /// Used to clear the antibuff and delayed action list for a mover when he die
        /// </summary>
        /// <param name="Mover mover">The mover which die</param>
        /// <returns></returns>
        public static void ClearMoverAntibuffAndDelayed(Mover mover)
        {
            if (mover.antibuffs.Count>0)//if mover has antibuff
            {
                for (int i=mover.antibuffs.Count-1;i>=0;i--)
                {
                    mover.antibuffs.Remove(mover.antibuffs[i]);
                }
            }
            if (mover.list_delayedprocess.Count>0)//if mover has antibuff
            {
                for (int i=mover.list_delayedprocess.Count-1;i>=0;i--)
                {
                    if (mover.list_delayedprocess[i].MethodName != METHOD_REMOVEANTIBUFF || mover.list_delayedprocess[i].MethodName != METHOD_REMOVEBUFF)
                    {
                        Delayer.CancelDelayedAction(mover.list_delayedprocess[i].Index);
                        mover.list_delayedprocess.Remove(mover.list_delayedprocess[i]);
                    }
                }
            }


        }
        //Name of the methode called
        public static List<str_castskill> ldelayed_castskill = new List<str_castskill>();//those list
        public static List<str_delayedSendDamage> ldelayed_senddamage = new List<str_delayedSendDamage>();

        #region List of action we will call
        /// <summary>
        /// Used for actionslot it cast a skill after a delay you give to the structure
        /// </summary>
        /// <param name="str_castskill">Structure used to take parameters needed to cast a skill</param>
        /// <returns></returns>
        public static void Delayed_castskill(str_castskill castskill)
        {
            if (castskill.skill == null)
                return;


            CPSUData PSUData = new CPSUData();
            PSUData.Caster = castskill.caster;
            PSUData.Target.SetMover(castskill.target);
            PSUData.SkillData = castskill.skill;
            PSUData.SkillName = SkillNames.GetSkillNameBySkillID(castskill.skill.dwNameID);
            PSUData.Skill = castskill.skill;
            PSUData.IsFromActionSlot = true;
            PSUData.actionslotorder = castskill.order;
            bool failed = false;
            try
            {
                if (castskill.caster.ProcessSkillUsage(PSUData))
                {
#if DEBUG
                    Log.Write(Log.MessageType.debug, "Skill use OK! {0}", PSUData.ToString());
#endif
                }
                else
                {
                    failed = true;
                    Log.Write(Log.MessageType.error, "An error occur while using skill! -> {0}", PSUData.ToString());
                }
            }
            catch (Exception e)
            {
                failed = true;
                Log.Write(Log.MessageType.error, "An error occur calculating skill data: {0}\r\nStack trace: {1}",
                   e.Message, e.StackTrace);
            }
            
            if ((failed) || (castskill.bislastskill))
                castskill.caster.SendPlayerSkillEnd(); // Very important, else client will freeze!
            //ok so now we must decrease % actionslotremaining
            
                    int decreasevalue = 0;
                    switch (castskill.order)
                    {
                        case 1: decreasevalue = 0; 
                                break;
                        case 2: decreasevalue = 6; //-6%
                                break;
                        case 3: decreasevalue = 8; 
                                break;
                        case 4: decreasevalue = 11; 
                                break;
                        case 5: decreasevalue = 28; 
                                break;

                    }
                    Log.Write(Log.MessageType.debug, " {0} pourcent of action slot have been take", decreasevalue);
                    PSUData.Caster.c_data.dwactionslotbar -= decreasevalue;
                    if (PSUData.Caster.c_data.dwactionslotbar < 0)
                        PSUData.Caster.c_data.dwactionslotbar = 0; //no negative value
                    castskill.caster.SendActionSlotGrey(PSUData.Caster.c_data.dwactionslotbar);
                    //now we delete this action from list of delayed action
                    for (int i = castskill.caster.list_delayedprocess.Count-1; i >=0; i--)
                    {
                        Delaycontent cdelaycontent = castskill.caster.list_delayedprocess[i];
                        if (cdelaycontent.MethodName == METHOD_CALLSKILL && cdelaycontent.Refvalue == castskill.skill.dwNameID)
                        {
                            castskill.caster.list_delayedprocess.Remove(cdelaycontent);
                        }
                    }
                
        }


        /// <summary>
        /// Send damage after a delay, used actually by skills to allow damage to come after skillmotions
        /// </summary>
        /// <param name="str_delayedsenddamage">Structure used to take parameters needed to send damage</param>
        /// <returns></returns>
        public static void Delayed_senddamage(str_delayedSendDamage senddamage)
        {
            Mover target = senddamage.target;
            Log.Write(Log.MessageType.debug, "I'm in delayed send damage");
            int dmg = senddamage.damage;
            if (target == null || target.c_attributes[FlyFF.DST_HP] <= 0)
            {
                Log.Write(Log.MessageType.error, "Target == null in delayed damage or target is dead !");
                return;
            }
            int real_damage = target.c_attributes[FlyFFAttributes.DST_HP] - dmg < 0 ? target.c_attributes[FlyFFAttributes.DST_HP] : dmg;
            target.c_attributes[FlyFFAttributes.DST_HP] -= real_damage;
            Log.Write(Log.MessageType.debug, "delayeddamage : damage done = {0}",real_damage);
            target.SendMoverDamaged(senddamage.caster.dwMoverID, dmg, senddamage.attackFlag,target.c_position,0);
            target.c_target = senddamage.caster;//like this monster will attack me
            target.bIsFighting = true;
            target.bIsFollowing = true;
            if (target.c_attributes[FlyFFAttributes.DST_HP] <= 0)
            {
                if ((WorldHelper.GetClientByMoverID(target.dwMoverID) == null)&& (WorldHelper.GetClientByMoverID(senddamage.caster.dwMoverID)!=null))
                {
                    Client me = senddamage.caster as Client;
                    Monster mob = target as Monster;
                    int xprate = 1;
                    if (me.c_data.dwLevel < 60) xprate = Server.exp_rate;
                    else if (me.c_data.dwLevel >= 60 && me.c_data.dwLevel < 90) xprate = Server.exp_rate60;
                    else if (me.c_data.dwLevel >= 90) xprate = Server.exp_rate90;
                    me.c_data.qwExperience += (int)((double)(mob.Data.mobExpPoints) * (double)xprate * me.c_data.f_RateModifierEXP);
                    me.OnCheckLevelGroup();

                    me.SendPlayerCombatInfo();
                    target.SendMoverDeath();
                    target.c_target = null;
                    mob.mob_OnDeath();
                    me.DropMobItems(mob, me, me.c_data.dwMapID);
                    //we must clear antiffu list and delayeed action list on this mover
                    ClearMoverAntibuffAndDelayed(target);
                }
                else
                {
                    target.SendMoverDeath();
                    target.c_target = null;
                    senddamage.caster.c_target = null;
                }
            }
            //now we delete this action from list of delayed action
            for (int i = target.list_delayedprocess.Count-1; i >=0 ; i--)
            {
                Delaycontent cdelaycontent = target.list_delayedprocess[i];
                if (cdelaycontent.MethodName == METHOD_SENDAMAGE && cdelaycontent.Refvalue == senddamage.damage)
                {
                    Log.Write(Log.MessageType.debug, "We remove this delayed damage form delayed damage list");
                    senddamage.caster.list_delayedprocess.Remove(cdelaycontent);
                }
            }

        }
        /// <summary>
        /// Remove AntiBuff on Mover
        /// </summary>
        /// <param name="CPSUData PSUData">Structure that contain all parameters used for the buff</param>
        /// <returns></returns>
        public static void Delayed_RemoveBuffonMover(Mover target,Client caster,Buff antibuff,bool changestate,bool Attributchange) //attribut change is to know if the buff we remove have done some change on attribut all true unless stonehand and poison effect for acrobat/yoyo
        {

            Skills skill = new Skills();
            skill = antibuff._skill;
                if (Attributchange)
                {
                    target.c_attributes[skill.dwDestParam1] -= skill.dwAdjParamVal1;
                    if (skill.dwAdjParamVal1 < 0)
                        target.SendMoverAttribRaise(skill.dwDestParam1, Math.Abs(skill.dwAdjParamVal1), -1);
                    else
                        target.SendMoverAttribDecrease(skill.dwDestParam1, skill.dwAdjParamVal1);
                    if (skill.dwDestParam2 != 0) //if second effect
                    {
                        if (skill.dwAdjParamVal2 < 0)
                            target.SendMoverAttribRaise(skill.dwDestParam2, Math.Abs(skill.dwAdjParamVal2), -1);
                        else
                            target.SendMoverAttribDecrease(skill.dwDestParam2, skill.dwAdjParamVal2);
                    }
                }
            
            if (changestate)
                target.SendChangeState(0);
            switch (skill.dwNameID)
            {
                case 37:                
                case 161:
                case 162:
                    target.c_attributes[FlyFF.DST_SPEED] = 100;
                    target.bIsBlocked = false;
                   
                    target.followMover(caster.dwMoverID,PacketCommands.RANGE_MELEE);
                    break;                
                case 227:
                    
                    target.bIsBlocked = false; //if mob die before receiving this ? to se later
                    target.followMover(caster.dwMoverID, PacketCommands.RANGE_MELEE);
                    break;
            }
            target.SendMoverRemoveAntiBuff(antibuff);
            target.antibuffs.Remove(antibuff);
            //now we delete this action from list of delayed action
            for (int i = target.list_delayedprocess.Count-1; i >=0 ; i--)
            {
                Delaycontent cdelaycontent = target.list_delayedprocess[i];
                if (cdelaycontent.MethodName == METHOD_REMOVEBUFF && cdelaycontent.Refvalue == antibuff._skill.dwNameID)
                {
                    caster.list_delayedprocess.Remove(cdelaycontent);
                }
            }

        }

        #endregion


    }
}

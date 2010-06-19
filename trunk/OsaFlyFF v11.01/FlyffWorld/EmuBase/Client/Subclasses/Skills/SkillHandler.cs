using System;
using System.Collections.Generic;
using System.Text;

namespace FlyffWorld
{
    public partial class Skills
    {
        #region Buff data
        /// <summary>
        /// This function allow us to know how many time we will have the buff
        /// </summary>
        /// <param name="Buff">Buff containing information about buff we want to apply</param>
        /// <param name="INT">intelligence of caster</param>
        /// <returns></returns>
        public static int getBuffInterval(Skills buff, int Int)
        {

            
            int interval = 100000;
            double intMultiplier = 2;
            
            interval = buff.dwSkillTime;
            return interval + (int)((double)(Int * intMultiplier) * 1000);
        }
        #endregion

        //-------------------------------------------------------------
        //Functions that will handle what skill are supposed to do
        //-------------------------------------------------------------

        #region Skills Type Heal
        /// <summary>
        /// Function that manage healing by heal/heal rain and circle healing
        /// </summary>
        /// <param name="Skills">Skills containing information about skill we want to apply</param>
        /// <param name="Client">Target of this buff</param>
        /// <param name="Client">Caster of this buff</param>
        /// <returns></returns>
        public static void healTarget(Skills myskills, Client target, Client user)
        {

            //we will see wich type of heal is it
            switch (myskills.dwNameID)
            {
                case 44:
                    target.c_attributes[FlyFFAttributes.DST_HP] += (myskills.dwAdjParamVal1 + 3 * target.c_attributes[3]);
                    if (target.c_attributes[FlyFFAttributes.DST_HP] > target.c_data.f_MaxHP) //you can't heal more than HP_MAX
                        target.c_attributes[FlyFFAttributes.DST_HP] = target.c_data.f_MaxHP;
                    target.SendEffect(280);
                    target.SendPlayerAttribSet(FlyFFAttributes.DST_HP, target.c_attributes[FlyFFAttributes.DST_HP]);
                    break;
                case 51:
                    Log.Write(Log.MessageType.debug, "i'm on circle healing case");
                    Party party = new Party();
                    party = Party.getPartyByID(user.c_data.dwPartyID);
                    if (party != null)
                    {
                        for (int i = 0; i < party.members.Count; i++)
                        {
                            Party.membersData member = party.members[i];
                            Client cClient = WorldHelper.GetClientByPlayerID(member.membercharID);
                            if (cClient.c_position.IsInCircle(user.c_position, 7.0f))
                            {
                                cClient.c_attributes[FlyFFAttributes.DST_HP] += (myskills.dwAdjParamVal1 + 3 * user.c_attributes[3]);
                                if (cClient.c_attributes[FlyFFAttributes.DST_HP] > cClient.c_data.f_MaxHP) //you can't heal more than HP_MAX
                                    cClient.c_attributes[FlyFFAttributes.DST_HP] = cClient.c_data.f_MaxHP;
                                cClient.SendEffect(280);
                                cClient.SendPlayerAttribSet(FlyFFAttributes.DST_HP, user.c_attributes[FlyFFAttributes.DST_HP]);

                            }
                        }
                    }

                    else
                    {
                        user.c_attributes[FlyFFAttributes.DST_HP] += (myskills.dwAdjParamVal1 + 3 * user.c_attributes[3]);
                        if (user.c_attributes[FlyFFAttributes.DST_HP] > user.c_data.f_MaxHP) //you can't heal more than HP_MAX
                            user.c_attributes[FlyFFAttributes.DST_HP] = user.c_data.f_MaxHP;
                        user.SendEffect(280);
                        user.SendPlayerAttribSet(FlyFFAttributes.DST_HP, user.c_attributes[FlyFFAttributes.DST_HP]);

                    }
                    break;
                case 144:
                    Party cparty = Party.getPartyByID(user.c_data.dwPartyID);
                    if (cparty != null)
                    {
                        for (int i = 0; i < cparty.members.Count; i++)
                        {
                            Party.membersData member = cparty.members[i];
                            Client cClient = WorldHelper.GetClientByPlayerID(member.membercharID);
                            if (cClient.c_position.IsInCircle(user.c_position, 7.0f))
                            {
                                cClient.c_attributes[FlyFFAttributes.DST_HP] += (myskills.dwAdjParamVal1 + 3 * user.c_attributes[3]);
                                if (cClient.c_attributes[FlyFFAttributes.DST_HP] > cClient.c_data.f_MaxHP) //you can't heal more than HP_MAX
                                    cClient.c_attributes[FlyFFAttributes.DST_HP] = cClient.c_data.f_MaxHP;
                                cClient.SendEffect(280);
                                cClient.SendPlayerAttribSet(FlyFFAttributes.DST_HP, user.c_attributes[FlyFFAttributes.DST_HP]);

                            }
                        }
                    }

                    else
                    {
                        user.c_attributes[FlyFFAttributes.DST_HP] += (myskills.dwAdjParamVal1 + 3 * user.c_attributes[3]);
                        if (user.c_attributes[FlyFFAttributes.DST_HP] > user.c_data.f_MaxHP) //you can't heal more than HP_MAX
                            user.c_attributes[FlyFFAttributes.DST_HP] = user.c_data.f_MaxHP;
                        user.SendEffect(280);
                        user.SendPlayerAttribSet(FlyFFAttributes.DST_HP, user.c_attributes[FlyFFAttributes.DST_HP]);

                    }
                    break;
            }
        }
        #endregion
        #region SkillPartySkill
        /// <summary>
        /// Function that manage Skills that have an effect on party's member
        /// </summary>
        /// <param name="Skills">Skills containing information about skill we want to apply</param>
        /// <param name="Client">Caster of this buff</param>
        /// <returns></returns>
        public static void SkillPartySkills(Skills myskills, Client user)
        {
            #region Tiphreth

            
            Party myparty = Party.getPartyByID(user.c_data.dwPartyID);
            if (myparty == null)
                return;
            Buff buff = new Buff();
            buff._skill = myskills;
            buff.dwTime = getBuffInterval(myskills, user.c_attributes[FlyFFAttributes.DST_INT]);
            Log.Write(Log.MessageType.debug, "Duration for typhret : {0}", buff.dwTime);
            for (int i = 0; i < myparty.members.Count; i++)
            {
                Party.membersData member = myparty.members[i];
                Client cClient = WorldHelper.GetClientByPlayerID(member.membercharID);
                Log.Write(Log.MessageType.debug, "working on client : {0}", cClient.c_data.strPlayerName);
                if (cClient == user)
                    continue;
                if (cClient.c_position.IsInCircle(user.c_position, 7.0f))
                {
                    setBuffEffect(buff, cClient);

                    Log.Write(Log.MessageType.debug, "i send buff effect");
                    
                    cClient.SendEffect(362);
                }

            }
            
            #endregion
            #region Blessing
            if (user.c_data.isInParty)
            {
                Party party = Party.getPartyByID(user.c_data.dwPartyID);
                if (myparty == null)
                    return;

                for (int i = 0; i < party.members.Count; i++)
                {
                    Party.membersData member = party.members[i];
                    Client cClient = WorldHelper.GetClientByPlayerID(member.membercharID);                    
                    if (cClient == user)
                        continue;
                    if (cClient.c_position.IsInCircle(user.c_position, 7.0f))
                    {
                        if (!DiceRoller.Roll(myskills.dwProbability))
                            continue;
                        for (int j = 0; j < cClient.antibuffs.Count; j++)
                        {
                            Buff curantibuff = cClient.antibuffs[j];

                            DelayedActions.Delayed_RemoveBuffonMover(cClient, user, curantibuff, true, true);
                            cClient.antibuffs.Remove(cClient.antibuffs[j]);
                        }
                        cClient.SendEffect(1878);
                    }

                }
            }
            #endregion
            return;
        }
        #endregion
        #region Resurrection
        /// <summary>
        /// Function that manage Resurrection of a player
        /// </summary>
        /// <param name="Skills">Skills containing information about skill we want to apply</param>
        /// <param name="Client">Caster of this buff</param>
        /// <returns></returns>
        public static void resurrectPlayer(Skills mySkills, Client target, Client user)
        {
            if (target.c_attributes[FlyFF.DST_HP] > 0)
                return; // user not dead
            //we send to player a box that ask him if he want or not to be ressurect
            //server need to suggest it to client every 10 seconde until client answer yes or no
            //actually we just send the offer, next we will see for the rest...
            target.c_data.tempSkills = mySkills; //stock skills used to resurrect in temporary variable
            target.SendPlayerResurrectionOffer(mySkills, target, user);
            target.SendEffect(283);
        }
        #endregion
        #region Buff effect
        /// <summary>
        /// OK, this function set Buff effect on player (modify attribut) and add Buff to buff list
        /// </summary>
        /// <param name="Buff">Buff containing information about buff we want to apply</param>
        /// <param name="Client">Target of this buff</param>
        /// <returns></returns>
        public static void setBuffEffect(Buff buff, Client target)
        {
            int isAlreadyActived = -1;

            for (int i = 0; i < target.c_data.buffs.Count; i++)
            {

                Buff curBuff = (Buff)target.c_data.buffs[i];//buff actually stocked in player
                if (curBuff._skill.dwNameID == buff._skill.dwNameID) //if we find that, this buff is already activated
                    isAlreadyActived = i;//we return position of this buff

            }
            if (isAlreadyActived != -1) //we already have this buff if new buff >older we update our buff unless no
            {

                Buff olderBuff = (Buff)target.c_data.buffs[isAlreadyActived];

                if (olderBuff._skill.dwSkillLvl <= buff._skill.dwSkillLvl)//if new buff is equal or superior we delete this one in db
                {
                    target.c_attributes[olderBuff._skill.dwDestParam1] -= olderBuff._skill.dwAdjParamVal1; //we delete effect of older buff
                    target.SendPlayerAttribRaise(olderBuff._skill.dwDestParam1, -olderBuff._skill.dwAdjParamVal1, -1);

                    if (olderBuff._skill.dwAdjParamVal2 != 0)//if there is a second effect
                    {
                        target.c_attributes[olderBuff._skill.dwDestParam2] -= olderBuff._skill.dwAdjParamVal2;
                        target.SendPlayerAttribRaise(olderBuff._skill.dwDestParam2, -olderBuff._skill.dwAdjParamVal2, -1);
                    }
                    target.c_data.buffs[isAlreadyActived] = buff;
                    //we send buff
                    target.SendPlayerBuff(buff);

                    //we update attribut

                    target.c_attributes[buff._skill.dwDestParam1] += buff._skill.dwAdjParamVal1;
                    target.SendPlayerAttribRaise(buff._skill.dwDestParam1, buff._skill.dwAdjParamVal1, -1);

                    if (buff._skill.dwAdjParamVal2 != 0)//if there is a second effect
                    {
                        target.c_attributes[buff._skill.dwDestParam2] += buff._skill.dwAdjParamVal2;
                        target.SendPlayerAttribRaise(buff._skill.dwDestParam2, buff._skill.dwAdjParamVal2, -1);
                    }
                    
                    //we need to put a timer :
                    target.isBuffed = true;
                }
                else return;


            }
            else if (isAlreadyActived == -1)
            {
                //we send buff and skill motion
                target.SendPlayerBuff(buff);
                //we update attribut
                target.c_attributes[buff._skill.dwDestParam1] += buff._skill.dwAdjParamVal1;
                target.SendPlayerAttribRaise(buff._skill.dwDestParam1, buff._skill.dwAdjParamVal1, -1);

                if (buff._skill.dwAdjParamVal2 != 0)//if there is a second effect
                {
                    target.c_attributes[buff._skill.dwDestParam2] += buff._skill.dwAdjParamVal2;
                    target.SendPlayerAttribRaise(buff._skill.dwDestParam2, buff._skill.dwAdjParamVal2, -1);
                }
                target.c_data.buffs.Add(buff);
                
                //we need to put a timer :
                target.isBuffed = true;
            }


        }
        #endregion
        #region Teleportation for mage class
        public static void MageTeleport(DataPacket dp,Client client)
        {
            float x = dp.Readfloat();
            float y = dp.Readfloat();
            float z = dp.Readfloat();
            client.c_position.x = x;
            client.c_position.y = y;
            client.c_position.z = z;
            client.SendMoverNewPosition();
            return;
        }
        #endregion


    }
}

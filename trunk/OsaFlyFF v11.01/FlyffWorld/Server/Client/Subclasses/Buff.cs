using System;
using System.Collections.Generic;

using System.Text;

namespace FlyffWorld
{
    public class Buff
    {
        public int buffID = 0;
        public int buffTime = 0;
        public int buffLevel = 0;
        public int buffAbility1 = 0;
        public int buffAbilityType1 = 0;
        public int buffAbility2 = 0;
        public int buffAbilityType2 = 0;
        public int buffChangeParam1 = 0;
        public int buffChangeParam2 = 0;
        public int probability = 0;
        public int probability_PVP = 0;

             
        


        /*public static Buff getBuffByID(int id)
        {
           
        }*/

        public static void setBuffEffect(Buff buff,Client target)
        {
            //we determin Effect of buff
            int[] listBuff = BuffDB.getBuffBonus(buff);
            buff.buffAbility1 = listBuff[0];
            buff.buffAbilityType1 = listBuff[1];
            buff.buffAbility2 = listBuff[2];
            buff.buffAbilityType2 = listBuff[3];
            int isAlreadyActived = -1;
            
            
            //first we look if there is already a buff and eventually update
            
                for (int i = 0; i < target.c_data.buffs.Count; i++)
                {
                     
                     Buff curBuff = (Buff)target.c_data.buffs[i];//buff actually stocked in player
                    if (curBuff.buffID == buff.buffID) //if we find that, this buff is already activated
                       isAlreadyActived = i;//we return position of this buff
                        
                 }
                if (isAlreadyActived != -1)
                {
                     
                     Buff olderBuff = (Buff)target.c_data.buffs[isAlreadyActived];

                    if (olderBuff.buffLevel <= buff.buffLevel)//if new buff is equal or superior we delete this one in db
                    {
                        Database.Execute("DELETE FROM flyff_buffs WHERE flyff_charid ={0} AND flyff_buffid ={1}", target.c_data.dwCharacterID, buff.buffID);

                        target.c_attributes[listBuff[1]] -= listBuff[0]; //we delete effect of older buff
                        target.SendPlayerAttribRaise(listBuff[1], -listBuff[0]);

                        if (listBuff[2] != 0 && listBuff[3] != 0)//if buff is GT
                        {
                            target.c_attributes[listBuff[3]] -= listBuff[2];
                            target.SendPlayerAttribRaise(listBuff[3], -listBuff[2]);
                        }
                        target.c_data.buffs[isAlreadyActived] = buff;
                        //if not or if we have deleted the older one :
                        Database.Execute("INSERT INTO flyff_buffs " +
                            "(`flyff_charid`,`flyff_buffid`,`flyff_buffLevel`,`flyff_ability1`,`flyff_abilityType1`,`flyff_ability2`,`flyff_abilityType2`,`flyff_buffChangeParam1`,`flyff_buffChangeParam2`,`flyff_probability`,`flyff_probability_PVP`,`flyff_remainingTime`) " +
                            "VALUES ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11})",
                            target.c_data.dwCharacterID,
                            buff.buffID,
                            buff.buffLevel,
                            buff.buffAbility1,
                            buff.buffAbilityType1,
                            buff.buffAbility2,
                            buff.buffAbilityType2,
                            buff.buffChangeParam1,
                            buff.buffChangeParam2,
                            buff.probability,
                            buff.probability_PVP,
                            buff.buffTime);
                        //we send buff
                        target.SendPlayerBuff(buff);

                        //we update attribut

                        target.c_attributes[listBuff[1]] += listBuff[0];
                        target.SendPlayerAttribRaise(listBuff[1], listBuff[0]);

                        if (listBuff[2] != 0 && listBuff[3] != 0)//if buff is GT
                        {
                            target.c_attributes[listBuff[3]] += listBuff[2];
                            target.SendPlayerAttribRaise(listBuff[3], listBuff[2]);
                        }
                        //we need to put a timer :
                        target.isBuffed = true;
                    }
                    else return;
                    
                        
                }
                else if (isAlreadyActived ==-1 )
                {
                        //if not or if we have deleted the older one :
                    Database.Execute("INSERT INTO flyff_buffs " +
                            "(`flyff_charid`,`flyff_buffid`,`flyff_buffLevel`,`flyff_ability1`,`flyff_abilityType1`,`flyff_ability2`,`flyff_abilityType2`,`flyff_buffChangeParam1`,`flyff_buffChangeParam2`,`flyff_probability`,`flyff_probability_PVP`,`flyff_remainingTime`) " +
                            "VALUES ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11})",
                        target.c_data.dwCharacterID,
                        buff.buffID,
                        buff.buffLevel,
                        buff.buffAbility1,
                        buff.buffAbilityType1,
                        buff.buffAbility2,
                        buff.buffAbilityType2,
                        buff.buffChangeParam1,
                        buff.buffChangeParam2,
                        buff.probability,
                        buff.probability_PVP,
                        buff.buffTime);
                        //we send buff and skill motion
                        target.SendPlayerBuff(buff);
                        

                        //we update attribut

                        target.c_attributes[listBuff[1]] += listBuff[0];
                        target.SendPlayerAttribRaise(listBuff[1], listBuff[0]);

                        if (listBuff[2] != 0 && listBuff[3] != 0)//if buff is GT
                        {
                            if (listBuff[2] > 0)
                            {

                                target.c_attributes[listBuff[3]] += listBuff[2];
                                target.SendPlayerAttribRaise(listBuff[3], listBuff[2]);
                            }
                            else
                            {
                                target.c_attributes[listBuff[3]] -= Math.Abs(listBuff[2]);
                                target.SendPlayerAttribRaise(listBuff[3], -Math.Abs(listBuff[2]));
                            }
                        }
                        target.c_data.buffs.Add(buff);
                    //we need to put a timer :
                target.isBuffed = true;
                }
                
              }
                
            
            
        





    }
}

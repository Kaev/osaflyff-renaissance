using System;
using System.Collections.Generic;

using System.Text;

namespace FlyffWorld
{
    class SkillFunctions : SkillData
    {
        //------------------------------------------------------------
        // General functions
        //------------------------------------------------------------

        public static void decreaseMPFP(SkillData myskills, Client user)
        {
            //this function decrease mana or FP from player which use skill
            if (myskills.dwReqMp > 0)
            {
                user.c_attributes[39] -= myskills.dwReqMp;
                user.SendPlayerAttribRaise(39, -myskills.dwReqMp);
            }
            if (myskills.dwReqFP > 0)
            {
                user.c_attributes[40] -= myskills.dwReqFP;
                user.SendPlayerAttribRaise(40, -myskills.dwReqFP);
            }
        }

        //-------------------------------------------------------------
        //Skills functions
        //-------------------------------------------------------------

        public static void healTarget(SkillData myskills, Client target)
        {
            //we will see wich type of heal is it
            if (myskills.dwName == 44) //heal one target
            {
                target.c_attributes[38] += (myskills.dwAdjParamVal1 + 3 * target.c_attributes[3]);
                if (target.c_attributes[38] > target.c_data.f_MaxHP) //you can't heal more than HP_MAX
                    target.c_attributes[38] = target.c_data.f_MaxHP;
                target.SendEffect(280);
                target.SendPlayerAttribRaise(myskills.dwDestParam1, (myskills.dwAdjParamVal1 + 3 * target.c_attributes[3]));
            }
            else if (myskills.dwName == 51) //circle healing
            {

                //now we have data for this skill we applied effect
                //while it's a zone effect skill we need to know if there are player in the area and if there are in the same group
                //need to make party system to continue
            }
            else if (myskills.dwName == 144)//heal rain
            {
                //need to make party system to continue
            }
        }
        public static void resurrectPlayer(SkillData mySkills, Client target, Client user)
        {
            if (target.c_attributes[FlyFF.DST_HP] > 0)
                return; // user not dead
            //we send to player a box that ask him if he want or not to be ressurect
            //server need to suggest it to client every 10 seconde until client answer yes or no
            //actually we just send the offer, next we will see for the rest...
            target.SendPlayerResurrectionOffer(mySkills, target, user);
            target.SendEffect(283);
        }
    }
}
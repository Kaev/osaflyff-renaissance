using System;
using System.Collections.Generic;
using System.Text;

namespace FlyffWorld
{
    public partial class Quest
    {
        /// <summary>
        /// This function initialize main part of the quest, .
        /// </summary>
        /// <param name="itemID">The item ID of the item to search for.</param>
        /// <returns></returns>
        [LuaFunction("AddQuest")]
        public void AddQuest(string questname,int questid,int XP,int Penya,int lvlmin,int lvlmax,int minimum, int maximum)
        {
            questName = questname;
            questPenya = Penya;
            questExp = XP;
            questID = questid;
            questMinLvl = lvlmin;
            questMaxLvl = lvlmax;
            questminstate = minimum;
            questmaxstate = maximum;
        }




    }
}

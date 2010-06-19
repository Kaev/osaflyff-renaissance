using System;
using System.Collections.Generic;
using System.Text;

namespace FlyffWorld
{
    public partial class Client
    {
        /*[Divinepunition]
         * Here th list of constant used in quest system*/

        public const string __YES__ = "__YES__", //Button option - Name of button
                                   __NO__ = "__NO__", //Button option  - Name of button
                                   QUEST_BEGIN_YES = "QUEST_BEGIN_YES", //Button option - Result of the button
                                   QUEST_BEGIN_NO = "QUEST_BEGIN_NO", //Button option - Result of the button
                                   QUEST_END_COMPLETE = "QUEST_END_COMPLETE", //given by the client when we valid a quest by __OK__ button. specify that this questID if finished
                                   __OK__ = "__OK__", //button option - Name of button
                                   QUEST_END = "QUEST_END", //specify that it for finish quest or state of a quest
                                   QUEST_BEGIN = "QUEST_BEGIN"; //to show start message
        #region JOB Constant
        public        const int   VAGRANT               = 0,
                                  MERCENARY             = 1,
                                  ACROBAT               = 2,
                                  ASSIST                = 3,
                                  MAGICIAN              = 4,
                                  KNIGHT                = 6,
                                  BLADE                 = 7,
                                  JESTER                = 8,
                                  RANGER                = 9,
                                  RINGMASTER            = 10,
                                  BILLPOSTER            = 11,
                                  PSYCHIKEEPER          = 12,
                                  ELEMENTOR             = 13,
                                  KNIGHT_MASTER         = 16,
                                  BLADE_MASTER          = 17,
                                  JESTER_MASTER         = 18,
                                  RANGER_MASTER         = 19,
                                  RINGMASTER_MASTER     = 20,
                                  BILLPOSTER_MASTER     = 21,
                                  PSYCHIKEEPER_MASTER   = 22,
                                  ELEMENTOR_MASTER      = 23,
                                  KNIGHT_HERO           = 24,
                                  BLADE_HERO            = 25,
                                  JESTER_HERO           = 26,
                                  RANGER_HERO           = 27,
                                  RINGMASTER_HERO       = 28,
                                  BILLPOSTER_HERO       = 29,
                                  PSYCHIKEEPER_HERO     = 30,
                                  ELEMENTOR_HERO        = 31;
        #endregion

    }
    public partial class Quest
    {
        //[Divinepunition] Ok here we will describe all parameter used for Quest system

        public int questID = 0,           //ID of the quest
                   questMinLvl = 1,       //lvl minimum to activate the quest
                   questMaxLvl = 121,     //lvl maximum to activate the quest
                   questPenya = 0,        //Penya given when finishing the quest
                   questExp = 0,          //Exp given when finishing the quest
                   questminstate = 0,     //set minimum state for the quest
                   questmaxstate = 3;     //set maximum state for the quest

       
        public DateTime DateStart;  //when will start the quest
        public DateTime DateStop;   //when will stop the quest

        public string questName = "No name",
                      startNPCName = "",
                      endNPCName   ="";

        public Dialogs[] dialoglist = new Dialogs[50];              // maximum 50 dialogs in total
        public ItemRequired[] itemreqlist =new ItemRequired[10];  //maximum 10 item required
        public MobRequired[] mobreqlist = new MobRequired[10];    //maximum 10 monster required

        #region Structures ...
        public struct Dialogs
        {
            public int state; //state of the quest
            public string questDialogs; //initialized to 20 string, more that what needed in case of
            public string questoption1; //will contain __YES__ or __NO__ option
            public string questoption2; //will contain __YES__ or __NO__ option
            public string questresult;  //the result of the quest choice : QUEST_BEGIN_YES/QUEST_BEGIN_NO/QUEST_END/QUEST_END_COMPLETE
        }

        public struct ItemRequired
        {
            public int state;       //state of the quest
            public int itemID;      //itemID required
            public int itemFind;    //tell how many "itemid" have been find
            public int itemAmount;  //tell how many itemID player must find
        }
        public struct MobRequired
        {
            public int state;                 //state of the quest
            public int monsterID;            //monsterID needed for the quest
            public int monsterkilled;       //tell how many "monsterid" have been killed
            public int monsterneedtokill;  //tell how many monsterID player must kill
        }
        #endregion
        //Job requirement
        public int[] jobrequired = new int[40]; //job required for the quest

    }
   

}

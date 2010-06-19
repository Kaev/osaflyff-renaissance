using System;
using System.Collections.Generic;
using System.Text;

namespace FlyffWorld
{
    public class Party
    {

        public string strpartyName = "Group";

        public List<membersData> members = new List<membersData>(); //we will add remove members

        public int partyID = 0,
                   partyLVL = 1,
                   leaderID = 0,
                   isAdvanced = 0,   //turn to 1 when party is advanced
                   expshare = 0, //0=level 1 =contrib
                   partyEXP = 0,
                   partySkillPoint = 0,
                   distribItem = 0; //(0:individually/1:sequentially/2:manually/3:randomly)




        public static Party getPartyByID(int partyid)
        {
            for (int i = 0; i < WorldServer.world_parties.Count; i++)
            {
                Party curParty = WorldServer.world_parties[i];
                if (curParty.partyID == partyid)
                    return curParty;
            }
            Log.Write(Log.MessageType.debug, "We don't find party for this id : {0}", partyid);
            return null; //if we don't find a party corresponding to this partyid


        }
        public struct membersData
        /*OK why not create just a List with client in there ?
         * because client is destruct when player leave game
         * but praty continue to have data about player until a defined time
         * so i keep information  about player un memory*/
        {
            public int membercharID,
                memberlvl,
                memberJob,
                memberNetStatus;

            public string memberName;

            public membersData(int id, int lvl, int job, string name, int status)
            {
                membercharID = id;
                memberlvl = lvl;
                memberJob = job;
                memberName = name;
                memberNetStatus = status;
            }

        }
        public static membersData getMembersData(Party party, int charID)
        {
            for (int i = 0; i < party.members.Count; i++)
            {
                membersData curMember = (membersData)party.members[i];
                if (curMember.membercharID == charID)
                    return curMember;

            }
            return new membersData(0, 0, 0, "", 0);
        }



    }



}

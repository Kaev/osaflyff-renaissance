using System;
using System.Collections.Generic;
using System.Text;

namespace FlyffWorld
{
    public partial class Client
    {
        public void inviteForParty(DataPacket dp) //this be done by player menu so no need to check channel/server
        {
            int myID = dp.Readint32();
            if (myID != c_data.dwCharacterID)
                return;
            int otherID = dp.Readint32();

            Client other = WorldHelper.GetClientByPlayerID(otherID);

            if (other == null)
            {
                SendMessageInfo(FlyFF.TID_GAME_NOTLOGIN);
                return;
            }
            //we must check  if asker is leader of the party! if asker is not in party he will become leader
            if (c_data.isInParty)
            {
                Party party = Party.getPartyByID(c_data.dwPartyID);
                if (party.leaderID != c_data.dwCharacterID) //if asker is not leader he can't invite someone
                    return;
            }
            //we must check if target is in party
            if (other.c_data.isInParty)
            {
                SendMessageHud("This player is already in a party");
                return;
            }

            sendInviteToParty(other);
        }

        public void declinePartyInvite(DataPacket dp)
        {
            int askerID = dp.Readint32();
            int myID = dp.Readint32();

            Client asker = WorldHelper.GetClientByPlayerID(askerID);
            if (asker == null)
                return;

            sendDeclineInvite(asker);

        }
        public void AddPartyMember(DataPacket dp)
        {
            int askerID = dp.Readint32();
            dp.Readint32();
            dp.Readint32();
            dp.Readint32();
            int targetID = dp.Readint32();
            Client asker = WorldHelper.GetClientByPlayerID(askerID);
            Client target = WorldHelper.GetClientByPlayerID(targetID); ;
            if ((asker == null) || (target == null))
                return;

            if (asker.c_data.isInParty == true)   //if asker is in party we will add reveiver in party only if asker is the lead
            {
                Party party = Party.getPartyByID(asker.c_data.dwPartyID);
                if (party == null)
                {
                    asker.SendMessageHud("an error appear when trying to find current partyid");
                    return;
                }
                if (party.members.Count >= 8)
                /*  why put it here and not in invite function ? cause someone else could have send an invite 
                 * before give you lead and the lead other one can answer yes before new selected one answer*/
                {
                    asker.SendMessageHud("Party is already full");
                    return;
                }

                if (party.leaderID != asker.c_data.dwCharacterID) //is asker is not the leader of the party
                    return; //do nothing 
                //ok now we can add player to the party :

                c_data.isInParty = true;                     //we show our player is now in a party
                c_data.dwPartyID = asker.c_data.dwPartyID;
                Party.membersData member = new Party.membersData(c_data.dwCharacterID, c_data.dwLevel, c_data.dwClass, c_data.strPlayerName, c_data.dwNetworkStatus);
                party.members.Add(member);

                //need to add some thing here but haven't time
                for (int i = 0; i < party.members.Count; i++)
                {
                    Client curClient = WorldHelper.GetClientByPlayerID(party.members[i].membercharID);
                    if (curClient == null)
                        continue;
                    curClient.updateParty(target, asker, party.partyID); //now we can add client to the party
                }
            }
            else  //so if asker has not party
            {
                Party party = new Party();
                party.leaderID = asker.c_data.dwCharacterID;
                Party.membersData member = new Party.membersData(asker.c_data.dwCharacterID, asker.c_data.dwLevel, asker.c_data.dwClass, asker.c_data.strPlayerName, asker.c_data.dwNetworkStatus);
                party.members.Add(member);
                //define memberdata for target
                member = new Party.membersData(target.c_data.dwCharacterID, target.c_data.dwLevel, target.c_data.dwClass, target.c_data.strPlayerName, target.c_data.dwNetworkStatus);
                party.members.Add(member);
                party.partyID = WorldServer.world_parties.Count + 1;
                WorldServer.world_parties.Add(party);        //add this group to worlds group
                c_data.isInParty = true;//we show our player is now in a party
                c_data.dwPartyID = party.partyID;
                asker.c_data.isInParty = true;                     //we show our player is now in a party
                asker.c_data.dwPartyID = party.partyID;
                asker.updateParty(target, asker, party.partyID);
                updateParty(target, asker, party.partyID);
            }

        }

        public void giveLead(DataPacket dp)
        {
            int myID = dp.Readint32();
            int newleadID = dp.Readint32();

            if (myID == newleadID)
                return;

            //do i have lead to have right to do that ?
            if (c_data.dwPartyID == 0) //no party so can't do this
                return;
            Party party = Party.getPartyByID(c_data.dwPartyID);
            if (party.leaderID != c_data.dwCharacterID) //if i'm not lead
                return;

            Client newlead = WorldHelper.GetClientByPlayerID(newleadID);
            if (newlead == null)
                return;

            //ok now we have to re arrange member in the group
            Party.membersData curMember = Party.getMembersData(party, newleadID);
            party.members.Remove(curMember);
            party.members.Insert(0, curMember); //put lead at top of the list
            party.leaderID = newlead.c_data.dwCharacterID;


            for (int i = 0; i < party.members.Count; i++)
            {
                Client curClient = WorldHelper.GetClientByPlayerID(party.members[i].membercharID);
                curClient.sendNewLeader(newleadID);
            }

        }

        public void partyKickUser(DataPacket dp)
        {
            int myselfID = dp.Readint32();
            int KickedID = dp.Readint32();

            if (!c_data.isInParty) //can't kick if your not in a party
                return;

            Party party = Party.getPartyByID(c_data.dwPartyID);
            if (party.leaderID != c_data.dwCharacterID)  //if you 're not a leader you can't kick other than yourself
                KickedID = myselfID;

            Client kicked = WorldHelper.GetClientByPlayerID(KickedID);
            if (kicked == null)
                return;
            /*now we must delete client from party member, decrease party number of member,
             * if there are less than 2 player party is destroyed...
               if leader destroyed himself we must give lead to another*/

            if (party.members.Count <= 2)
            {
                //we send a kick for this player and the other :

                //sendKickPlayer(this, this); //detroy party
                sendKickPlayer(kicked, this);

                Party.membersData curMember = Party.getMembersData(party, KickedID);
                party.members.Remove(curMember);
                curMember = Party.getMembersData(party, myselfID);
                party.members.Remove(curMember);
                //WorldServer.world_parties.Remove(party);
                /*if we remove it it will modify all partyID !!!so we need to keep it in memory until
                 * next maintenance*/
                kicked.c_data.dwPartyID = 0;      //reinitilize status for player
                kicked.c_data.isInParty = false;
                c_data.dwPartyID = 0;      //reinitilize status for player
                c_data.isInParty = false;
                return;
            }
            else
            {
                if (KickedID == party.leaderID)  //if leader kicked himself
                {
                    Log.Write(Log.MessageType.info, "Kicked player is the leader");
                    party.leaderID = party.members[1].membercharID; //we give next client the lead

                }
                Party.membersData curMember = Party.getMembersData(party, KickedID);
                party.members.Remove(curMember);

                for (int i = 0; i < party.members.Count; i++)
                {
                    Client curClient = WorldHelper.GetClientByPlayerID(party.members[i].membercharID);

                    curClient.updateParty(kicked, this, party.partyID);
                }
                kicked.sendKickPlayer(kicked, this);
                kicked.c_data.dwPartyID = 0;      //reinitilize status for player
                kicked.c_data.isInParty = false;
            }

        }

        public void partyChangeItemdistrib(DataPacket dp) //leader ask to change item distribution
        {
            dp.Readint32();
            int itemstatus = dp.Readint32();
            if (c_data.dwPartyID == 0)
                return;
            Party party = Party.getPartyByID(c_data.dwPartyID);
            if (party.leaderID != c_data.dwCharacterID) //only leader can change option
                return;

            party.distribItem = itemstatus;
            for (int i = 0; i < party.members.Count; i++)
            {
                Client curClient = WorldHelper.GetClientByPlayerID(party.members[i].membercharID);
                curClient.sendChangeItemDistrib(itemstatus);
            }

        }
    }
}

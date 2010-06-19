using System;
using System.Collections.Generic;
using System.Text;

namespace FlyffWorld
{
    public partial class Client
    {

        public void sendInviteToParty(Client other)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(other.dwMoverID, PAK_PARTY_INVIT);

            pak.Addint32(c_data.dwCharacterID);
            pak.Addint32(c_data.dwLevel);
            pak.Addint32(c_data.dwClass);
            pak.Addbyte(c_data.dwGender);

            pak.Addint32(other.c_data.dwCharacterID);
            pak.Addint32(other.c_data.dwLevel);
            pak.Addint32(other.c_data.dwClass);
            pak.Addbyte(other.c_data.dwGender);
            pak.Addstring(c_data.strPlayerName);
            pak.Addint32(0);

            pak.Send(other);

        }

        public void sendDeclineInvite(Client asker)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(asker.dwMoverID, PAK_PARTY_CANCELINV);
            pak.Addint32(asker.c_data.dwCharacterID);
            pak.Addint32(c_data.dwCharacterID);
            pak.Addint32(0);
            pak.Send(asker);

        }

        public void updateParty(Client target, Client asker, int partyID)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_UPDATE_PARTY);
            pak.Addint32(target.c_data.dwCharacterID);
            pak.Addstring(asker.c_data.strPlayerName);
            pak.Addstring(target.c_data.strPlayerName);

            Party party = Party.getPartyByID(partyID);
            pak.Addint32(party.members.Count);
            pak.Addint32(party.partyID);
            pak.Addint32(party.isAdvanced);   //0 for normal group 1 for advanced
            pak.Addint32(party.members.Count);
            pak.Addint32(party.partyLVL);
            pak.Addint32(party.partyEXP);
            pak.Addint32(party.partySkillPoint);
            pak.Addint32(party.expshare); //0: level 1 contrib
            pak.Addint32(party.distribItem); //(0:individually/1:sequentially/2:manually/3:randomly)
            pak.Addint32(0);
            pak.Addint32(0);
            pak.Addint32(0);
            pak.Addint32(0);
            pak.Addint32(0);
            if (party.isAdvanced == 1)   //name appear when party is advanced unless there is nothing
                pak.Addstring(party.strpartyName);

            for (int i = 0; i < party.members.Count; i++)
            {
                pak.Addint32(party.members[i].membercharID);
                pak.Addint32(party.members[i].memberNetStatus);

            }

            pak.Send(this);

        }
        public void sendKickPlayer(Client target, Client asker)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(target.dwMoverID, PAK_UPDATE_PARTY);
            pak.Addint32(target.c_data.dwCharacterID);
            pak.Addstring(asker.c_data.strPlayerName);
            pak.Addstring(target.c_data.strPlayerName);
            pak.Send(target);

        }

        public void sendNewLeader(int newleadID)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_PARTY_LEADER); //design new lead
            pak.Addint32(newleadID);
            pak.StartNewMergedPacket(dwMoverID, PAK_REORDER_PARTY); //reorder party list
            pak.Addint32(4); //channel ?
            pak.Addint32(0); //do not know what is it
            pak.Addint32(0); //do not know what is it
            pak.Send(this);
        }

        public void sendChangeItemDistrib(int status)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_PARTY_ITEMDISTRIB);
            pak.Addint32(status);
            pak.Send(this);
        }


    }
}

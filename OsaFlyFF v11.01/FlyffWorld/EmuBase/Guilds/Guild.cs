﻿using System;
using System.Collections;

using System.Text;

namespace FlyffWorld
{
    public class Guild
    {
        public ArrayList getClients()
        {
            ArrayList ret = ArrayList.Synchronized(new ArrayList());
            for (int i = 0; i < members.Count; i++)
            {
                Client client = PlayersHandler.getClientByID(((GuildMember)members[i]).characterID);
                if (client != null)
                    ret.Add(client);
            }
            return ret;
        }
        public GuildMember getMember(int memberID)
        {
            for (int i = 0; i < members.Count; i++)
            {
                GuildMember member = (GuildMember)members[i];
                if (member.characterID == memberID)
                    return member;
            }
            return null;
        }
        public GuildDuel duelInfo = new GuildDuel();
        public ArrayList members = ArrayList.Synchronized(new ArrayList());
        public int guildID = -1;
        public int founderID = -1;
        public string guildName = "";
        public int guildLogo = 0;
        public int guildContributionEXP = 0;
        public int gwWins = 0;
        public int gwLoses = 0;
        public int gwForfeits = 0;
        public int[] memberPrivileges = new int[5];
        public int[] memberPayment = new int[5];
        public string guildNotice = "";
        public int guildLevel = 0;
        public int bankPenya = 0;
    }
    public class GuildMember
    {
        public int characterID = -1;
        public string memberNickname = "";
        public int questContribution = 0;
        public int penyaContribution = 0;
        public int memberRank = 0;
        public int memberRankSymbolCount = 0;
        public int gwForfeits = 0;
    }
}

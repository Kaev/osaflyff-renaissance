using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace FlyffWorld
{
    public class GuildHandler
    {
        private static int _duelIncrement = 1;
        public static int GenerateDuelID()
        {
            return _duelIncrement++;
        }
        public const int PRIVILEGE_PROMOTE = 1,
                         PRIVILEGE_SETSYMBOLCOUNT = 2,
                         PRIVILEGE_INVITE = 4,
                         PRIVILEGE_TAKEPENYA = 8,
                         PRIVILEGE_TAKEITEM = 16,
                         PRIVILEGES_ALL = 31;
        public const int RANK_ROOKIE = 4,
                         RANK_SUPPORTER = 3,
                         RANK_CAPTAIN = 2,
                         RANK_KINGPIN = 1,
                         RANK_MASTER = 0;
        public static void GDTruce(GuildDuel gdinfo)
        {
            Guild one = getGuildByGuildID(gdinfo.dwAttackerGuildID), two = getGuildByGuildID(gdinfo.dwDefenderGuildID);
            if (one == null || two == null)
            {
                Log.Write(Log.MessageType.warning, "GuildHandler::GDTruce(): guild duel info is invalid");
                return; // error?
            }
            List<Client> c1 = one.getClients(), c2 = two.getClients();
            GuildPackets.SendGuildDuelResult(gdinfo, 8);
            gdinfo.ClearDuel();
            one.duelInfo = gdinfo;
            two.duelInfo = gdinfo;
            for (int i = 0; i < c1.Count; i++)
                ((Client)c1[i]).SendGuildDuelState(gdinfo.dwDuelID);
            for (int i = 0; i < c2.Count; i++)
                ((Client)c2[i]).SendGuildDuelState(gdinfo.dwDuelID);
        }
        public static void GDGiveUp(GuildDuel gdinfo, int givingUpGuild)
        {
            Guild one, two;
            if ((gdinfo.dwAttackerGuildID != givingUpGuild && gdinfo.dwDefenderGuildID != givingUpGuild) || (one = getGuildByGuildID(gdinfo.dwAttackerGuildID)) == null || (two = getGuildByGuildID(gdinfo.dwDefenderGuildID)) == null)
            {
                Log.Write(Log.MessageType.warning, "GuildHandler::GDGiveUp(): guild duel info is invalid");
                return; // error?
            }
            bool sendGdGu = true;
            int charID = gdinfo.dwAttackerGuildID == givingUpGuild ? one.founderID : two.founderID;
            string charName = Database.GetString("SELECT flyff_charactername FROM flyff_characters WHERE flyff_characterid = " + charID, 0);
            if (charName == null)
                sendGdGu = false;
            List<Client> c1 = one.getClients(), c2 = two.getClients();
            for (int i = 0; i < c1.Count; i++)
            {
                if (sendGdGu)
                    ((Client)c1[i]).SendGuildDuelForfeit(gdinfo.dwDuelID, charID, charName);
                ((Client)c1[i]).SendGuildDuelState(0);
            }
            for (int i = 0; i < c2.Count; i++)
            {
                if (sendGdGu)
                    ((Client)c1[i]).SendGuildDuelForfeit(gdinfo.dwDuelID, charID, charName);
                ((Client)c2[i]).SendGuildDuelState(0);
            }
            GuildPackets.SendGuildDuelResult(gdinfo, 2 + (gdinfo.dwAttackerGuildID == givingUpGuild ? 0 : 1));
            gdinfo.ClearDuel();
            one.duelInfo = gdinfo;
            two.duelInfo = gdinfo;
        }
        public static void GDStart(GuildDuel gdinfo)
        {
            Guild one = getGuildByGuildID(gdinfo.dwAttackerGuildID), two = getGuildByGuildID(gdinfo.dwDefenderGuildID);
            if (one == null || two == null)
            {
                Log.Write(Log.MessageType.warning, "GuildHandler::GDStart(): guild duel info is invalid");
                return; // error?
            }
            gdinfo.StartDuel();
            one.duelInfo = gdinfo;
            two.duelInfo = gdinfo;
            // send packets to all guild members, and then update world
            GuildPackets.SendGuildDuelNew(gdinfo);
            List<Client> c1 = one.getClients(), c2 = two.getClients();
            for (int i = 0; i < c2.Count; i++)
                ((Client)c2[i]).SendGuildDuelState(gdinfo.dwDuelID);
            for (int i = 0; i < c1.Count; i++)
                ((Client)c1[i]).SendGuildDuelState(gdinfo.dwDuelID);
        }
        public static Guild getGuildByFounderID(int founderID)
        {
            for (int i = 0; i < WorldServer.world_guilds.Count; i++)
            {
                Guild thisguild = WorldServer.world_guilds[i];
                if (thisguild.founderID == founderID)
                    return thisguild;
            }
            return null;
        }
        public static Guild getGuildByMemberID(int memberID)
        {
            for (int i = 0; i < WorldServer.world_guilds.Count; i++)
            {
                Guild thisguild = WorldServer.world_guilds[i];
                for (int l = 0; l < thisguild.members.Count; l++)
                {
                    GuildMember member = (GuildMember)thisguild.members[l];
                    if (member.characterID == memberID)
                        return thisguild;
                }
            }
            return null;
        }
        public static Guild getGuildByGuildID(int guildID)
        {
            for (int i = 0; i < WorldServer.world_guilds.Count; i++)
            {
                Guild thisguild = WorldServer.world_guilds[i];
                if (thisguild.guildID == guildID)
                    return thisguild;
            }
            return null;
        }
        public static GuildMember getGuildMemberByGuildID(int guildID, int characterID)
        {
            for (int i = 0; i < WorldServer.world_guilds.Count; i++)
            {
                Guild thisguild = WorldServer.world_guilds[i];
                if (thisguild.guildID != guildID)
                    continue;
                for (int l = 0; l < thisguild.members.Count; l++)
                {
                    GuildMember member = (GuildMember)thisguild.members[l];
                    if (member.characterID == characterID)
                        return member;
                }
            }
            return null;
        }
        public static Guild getGuildByGuildName(string guildName)
        {
            for (int i = 0; i < WorldServer.world_guilds.Count; i++)
            {
                Guild thisguild = WorldServer.world_guilds[i];
                if (thisguild.guildName == guildName)
                    return thisguild;
            }
            return null;
        }
        public static bool guildIDExists(int id)
        {
            for (int i = 0; i < WorldServer.world_guilds.Count; i++)
            {
                if (WorldServer.world_guilds[i].guildID == i)
                    return true;
            }
            return false;
        }
        public static int getNewGuildID()
        {
            int i = 1;
            while (guildIDExists(i++)) ;
            return i;
        }
        public static void CreateGuild(Client former)
        {
            int i = getNewGuildID();
            Guild guild = new Guild();
            guild.guildID = i;
            guild.guildLevel = 1;
            guild.memberPrivileges[0] = PRIVILEGES_ALL;
            guild.founderID = former.c_data.dwCharacterID;
            GuildMember member = new GuildMember();
            member.characterID = former.c_data.dwCharacterID;
            member.memberRank = 0;
            member.memberRankSymbolCount = 0;
            guild.members.Add(member);
            WorldServer.world_guilds.Add(guild);
            GuildPackets.SendGuildCreation(former.c_data.dwCharacterID, guild.guildID, former.c_data.strPlayerName);
        }
        public static void RenameGuild(int guildID, string newGuildName)
        {
            Guild guild = getGuildByGuildID(guildID);
            if (guild == null)
                return;
            guild.guildName = newGuildName;
            Database.Execute("UPDATE flyff_guilds SET flyff_guildName='{1}' WHERE flyff_guildID={0}", guild.guildID, guild.guildName);
            GuildPackets.SendGuildName(guildID, newGuildName);
        }
        public static void CreateGuild(Client former, string gname)
        {
            if (former.c_data.dwGuildID > 0)
            {
                Log.Write(Log.MessageType.warning, "{0}[{1}]: Cannot create new guild because character is already in one", former.c_data.strPlayerName, former.c_data.dwCharacterID);
                return;
            }
            int i = getNewGuildID();
            Guild guild = new Guild();
            guild.guildID = i;
            guild.guildLevel = 1;
            guild.memberPrivileges[0] = PRIVILEGES_ALL;
            guild.founderID = former.c_data.dwCharacterID;
            guild.guildName = gname;
            GuildMember member = new GuildMember();
            member.characterID = former.c_data.dwCharacterID;
            member.memberRank = 0;
            member.memberRankSymbolCount = 0;
            guild.members.Add(member);
            WorldServer.world_guilds.Add(guild);
            former.c_data.dwGuildID = guild.guildID;
            InsertToDatabase(guild);
            GuildPackets.SendGuildCreation(former.c_data.dwCharacterID, guild.guildID, former.c_data.strPlayerName, gname);
        }
        public static void InsertToDatabase(Guild guild)
        {
            Database.Execute("DELETE FROM flyff_guilds WHERE flyff_guildID={0}", guild.guildID);
            Database.Execute("DELETE FROM flyff_guildmembers WHERE flyff_guildID={0}", guild.guildID);
            Database.Execute("INSERT INTO flyff_guilds " +
                "(`flyff_guildID`,`flyff_founderID`,`flyff_guildName`,`flyff_guildLogo`,`flyff_guildContributionEXP`,`flyff_gwWins`,`flyff_gwLoses`,`flyff_gwForfeits`,`flyff_privilegesMaster`,`flyff_privilegesKingpin`,`flyff_privilegesCaptain`,`flyff_privilegesSupporter`,`flyff_privilegesRookie`,`flyff_paymentMaster`," +
                "`flyff_paymentKingpin`,`flyff_paymentCaptain`,`flyff_paymentSupporter`,`flyff_paymentRookie`,`flyff_guildNotice`,`flyff_guildLevel`,`flyff_bankPenya`) " +
                "VALUES ({0},{1},'{2}',{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},'{18}',{19},{20})",
                        guild.guildID,
                        guild.founderID,
                        Database.Escape(guild.guildName),
                        guild.guildLogo,
                        guild.guildContributionEXP,
                        guild.gwWins,
                        guild.gwLoses,
                        guild.gwForfeits,
                        guild.memberPrivileges[0],
                        guild.memberPrivileges[1],
                        guild.memberPrivileges[2],
                        guild.memberPrivileges[3],
                        guild.memberPrivileges[4],
                        guild.memberPayment[0],
                        guild.memberPayment[1],
                        guild.memberPayment[2],
                        guild.memberPayment[3],
                        guild.memberPayment[4],
                        Database.Escape(guild.guildNotice),
                        guild.guildLevel,
                        guild.bankPenya
                        );
            for (int i = 0; i < guild.members.Count; i++)
            {
                GuildMember me = (GuildMember)guild.members[i];
                Database.Execute("INSERT INTO flyff_guildmembers " +
                    "(`flyff_guildID`,`flyff_characterID`,`flyff_memberNickname`,`flyff_memberQuestContribution`,`flyff_memberPenyaContribution`,`flyff_memberRank`,`flyff_memberRankSymbolCount`,`flyff_gwForfeits`) " +
                    "VALUES ({0}, {1}, '{2}', {3}, {4}, {5}, {6}, {7})",
                        guild.guildID,
                        me.characterID,
                        me.memberNickname,
                        me.questContribution,
                        me.penyaContribution,
                        me.memberRank,
                        me.memberRankSymbolCount,
                        me.gwForfeits
                        );
            }
        }
        public static void SetGuildLogo(Guild guild, int logoID)
        {
            guild.guildLogo = logoID;
            Database.Execute("UPDATE flyff_guilds SET flyff_guildLogo={0} WHERE flyff_guildID={1}", logoID, guild.guildID);
            GuildPackets.SendGuildLogo(guild.guildID, logoID);
        }
        public static void SetGuildNotice(Guild guild, string p)
        {
            guild.guildNotice = p;
            Database.Execute("UPDATE flyff_guilds SET flyff_guildNotice='{0}' WHERE flyff_guildID={1}", p, guild.guildID);
            GuildPackets.SendGuildNotice(guild);
        }

        public static void DisbandGuild(Guild guild, string founderName)
        {
            // first, update sql
            Database.Execute("DELETE FROM flyff_guilds WHERE flyff_guildID={0}", guild.guildID);
            Database.Execute("DELETE FROM flyff_guildmembers WHERE flyff_guildID={0}", guild.guildID);
            // now update world characters
            List<Client> clients = guild.getClients();
            for (int i = 0; i < clients.Count; i++)
            {
                Client client = (Client)clients[i];
                client.c_data.dwGuildID = -1;
            }
            // finally send packet to all
            GuildPackets.SendGuildDisbanding(guild.guildID, founderName);
            // remove guild from WorldServer.world_guilds
            WorldServer.world_guilds.Remove(guild);
        }

        public static void AddMember(Guild guild, int id, string name)
        {
            GuildMember me = new GuildMember();
            me.characterID = id;
            me.memberRank = RANK_ROOKIE;
            Database.Execute("INSERT INTO flyff_guildmembers " +
                    "(`flyff_guildID`,`flyff_characterID`,`flyff_memberNickname`,`flyff_memberQuestContribution`,`flyff_memberPenyaContribution`,`flyff_memberRank`,`flyff_memberRankSymbolCount`,`flyff_gwForfeits`) " +
                    "VALUES ({0}, {1}, '{2}', {3}, {4}, {5}, {6}, {7})",
                        guild.guildID,
                        me.characterID,
                        me.memberNickname,
                        me.questContribution,
                        me.penyaContribution,
                        me.memberRank,
                        me.memberRankSymbolCount,
                        me.gwForfeits
                        );
            guild.members.Add(me);
            GuildPackets.SendGuildNewMember(id, name, guild);
        }

        public static void RemoveMember(Guild guild, int kickeeID)
        {
            GuildMember me = getGuildMemberByGuildID(guild.guildID, kickeeID);
            if (me == null)
                return;
            Database.Execute("DELETE FROM flyff_guildmembers WHERE flyff_characterID = {0}", me.characterID);
            guild.members.Remove(me);
            // send packets now
            GuildPackets.SendGuildRemoveMember(kickeeID, guild);
        }

        public static void SetMemberClass(Guild guild, GuildMember dest, int NewClass)
        {
            dest.memberRankSymbolCount = NewClass;
            Database.Execute("UPDATE flyff_guildmembers SET flyff_memberRankSymbolCount={0} WHERE flyff_characterID = {1}", NewClass, dest.characterID);
            GuildPackets.SendGuildClassChange(guild, dest.characterID, NewClass);
        }

        public static void SetMemberRank(Guild guild, GuildMember target, int destRank)
        {
            target.memberRank = destRank;
            target.memberRankSymbolCount = 0;
            Database.Execute("UPDATE flyff_guildmembers SET flyff_memberRank={0} WHERE flyff_characterID = {1}", destRank, target.characterID);
            GuildPackets.SendGuildRankChange(guild, target.characterID, destRank);
        }
    }
}
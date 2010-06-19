using System;
using System.Collections.Generic;
using System.Text;

namespace FlyffWorld
{
    public partial class Client
    {
        public void SendGuildDataSingle(Guild guild)
        {
            Packet pak = new Packet();
            Packet pakMembers = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_GUILDS_SINGLE);
            pak.Addint(guild.guildID);
            pak.Addint(guild.guildID);
            pak.Addint(guild.founderID);
            pak.Addint(0x0000000C);
            pak.Addstring(guild.guildName);
            pak.Addint(guild.guildLogo);
            pak.Addint(guild.bankPenya);
            pak.Addint(guild.gwWins);
            pak.Addint(guild.gwLoses);
            pak.Addint(guild.gwForfeits);
            for (int i = 0; i < 5; i++)
                pak.Addint(guild.memberPrivileges[i]);
            for (int i = 0; i < 5; i++)
                pak.Addint(guild.memberPayment[i]);
            pak.Addstring(guild.guildNotice);
            pak.Addint(guild.guildContributionEXP);
            pak.Addint(guild.guildLevel);
            pak.Addint(0);
            pak.Addshort(guild.members.Count);
            for (int i = 0; i < guild.members.Count; i++)
            {
                GuildMember member = (GuildMember)guild.members[i];
                Friend fm = BuildGuildMember(member.characterID);
                if (fm != null)
                {
                    pakMembers.StartNewMergedPacket(dwMoverID, PAK_ADD_FRIENDDATA);
                    pakMembers.Addint(member.characterID);
                    pakMembers.Addstring(fm.strPlayerName);
                    pakMembers.Addbyte(fm.dwClass);
                    pakMembers.Addbyte(fm.dwLevel);
                    pakMembers.Addbyte(fm.dwGender);
                    pakMembers.Addbyte(0);
                    pakMembers.Addint(0x11);
                    pakMembers.Addint(0);
                    pak.Addint(member.characterID);
                    pak.Addint(0xFFFFFF9A);
                    pak.Addint(member.penyaContribution);
                    pak.Addint(member.questContribution);
                    pak.Addshort(0);
                    pak.Addshort(member.gwForfeits);
                    pak.Addint(member.memberRank);
                    pak.Addint(0);
                    pak.Addbyte(0);
                    pak.Addint(member.memberRankSymbolCount);
                    pak.Addstring(member.memberNickname);
                }
                else
                {
                    pak.Addint(-1);
                    pak.Addint(0xFFFFFF9A);
                    pak.Addlong(0);
                    pak.Addint(0);
                    pak.Addint(4);
                    pak.Addint(0);
                    pak.Addbyte(0);
                    pak.Addint(0);
                    pak.Addstring("Void player");
                }
            }
            pak.Addhex("0000 01 01000000 0E000000 00000000");
            pak.Send(this);
            pakMembers.Send(this);
        }
        public void SendGuildPlayer(Guild guild)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(-1, PAK_ADD_FRIENDDATA);
            pak.Addint(c_data.dwCharacterID);
            pak.Addstring(c_data.strPlayerName);
            pak.Addbyte(c_data.dwClass);
            pak.Addbyte(c_data.dwLevel);
            pak.Addbyte(c_data.dwGender);
            pak.Addbyte(0);
            pak.Addint(0x11);
            pak.Addint(0);
            pak.SendTo(guild.getClients());
        }
        public void SendGuildDataAll()
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_GUILDS_ALL);
            pak.Addint(WorldServer.world_guilds.Count);
            pak.Addint(WorldServer.world_guilds.Count);
            for (int i = 0; i < WorldServer.world_guilds.Count; i++)
            {
                Guild guild = WorldServer.world_guilds[i];
                pak.Addint(guild.guildID);
                pak.Addint(guild.founderID);
                pak.Addint(0x0000000C);
                pak.Addstring(guild.guildName);
                pak.Addint(guild.guildLogo);
                pak.Addint(guild.bankPenya);
                pak.Addint(guild.gwWins);
                pak.Addint(guild.gwLoses);
                pak.Addint(guild.gwForfeits);
            }
            pak.Addint(1);
            for (int i = 0; i < 4; i++)
                pak.Addlong(0); // 8 dwords, remember that.
            pak.Send(this);
        }
        public void SendGuildOnJoin(int guildID)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_GUILDS_JOIN);
            pak.Addint(guildID);
            pak.Send(this);
            SendToVisible(pak);
        }
        public void SendGuildInvitation(int guildID, int sourceID)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(-1, PAK_GUILDS_SENDINV);
            pak.Addint(guildID);
            pak.Addint(sourceID);
            pak.Send(this);
        }
        public void SendGuildDuelRequest(int fromGuildID, string fromGuildLeader)
        {
            Packet pak = new Packet();
            pak.Addint(PAK_GUILDS_GDREQUEST);
            pak.Addint(fromGuildID);
            pak.Addstring(fromGuildLeader);
            pak.Send(this);
        }
        public void SendGuildDuelTruce()
        {
            Packet pak = new Packet();
            pak.Addint(PAK_GUILDS_GDTRUCEREQ);
            pak.Send(this);
        }
        public void SendGuildDuelState(int duelID)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_GUILDS_GDSTATE);
            pak.Addint(duelID);
            pak.Send(this);
            SendToVisible(pak);
        }
        public void SendGuildDuelForfeit(int duelID, int charID, string charName)
        {
            Packet pak = new Packet();
            pak.Addint(PAK_GUILDS_GDGIVEUP);
            pak.Addint(duelID);
            pak.Addint(charID);
            pak.Addstring(charName);
            pak.Addint(0);
            pak.Send(this);
        }
        public void SendGuildSalaryUpdate(int rank, int salary, List<Client> clients)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(-1, PAK_GUILDS_SALARY);
            pak.Addint(rank);
            pak.Addint(salary);
            pak.SendTo(clients);
        }
        public void SendGuildTitlesUpdate(List<Client> clients, int[] privileges)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(-1, PAK_GUILDS_AUTHORITY);
            pak.Addint(c_data.dwGuildID);
            for (int i = 1; i < 5; i++)
                pak.Addint(privileges[i]);
            pak.SendTo(clients);
        }
    }
    public class GuildPackets : PacketCommands
    {
        public static void SendGuildDuelResult(GuildDuel gdinfo, int result)
        {
            Packet pak = new Packet();
            pak.Addint(PAK_GUILDS_GDRESULT);
            pak.Addint(gdinfo.dwDuelID);
            pak.Addint(50); // unknown
            pak.Addint(0); // unknown
            pak.Addint(result);
            pak.SendTo(WorldServer.world_players);
        }
        public static void SendGuildDuelNew(GuildDuel gdinfo)
        {
            Packet pak = new Packet();
            pak.Addint(PAK_GUILDS_GDSTART);
            pak.Addint(gdinfo.dwDuelID);
            pak.Addint(gdinfo.dwDefenderGuildID);
            pak.Addint(gdinfo.dwAttackerGuildID);
            pak.SendTo(WorldServer.world_players);
        }
        public static void SendGuildCreation(int charID, int guildID, string charName)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(-1, PAK_GUILDS_NEW);
            pak.Addint(charID);
            pak.Addint(guildID);
            pak.Addstring(charName);
            pak.Addint(0);
            pak.SendTo(WorldServer.world_players); // sendtoall
        }
        public static void SendGuildCreation(int charID, int guildID, string charName, string guildName)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(-1, PAK_GUILDS_NEW);
            pak.Addint(charID);
            pak.Addint(guildID);
            pak.Addstring(charName);
            pak.Addstring(guildName);
            pak.SendTo(WorldServer.world_players); // sendtoall
        }
        public static void SendGuildName(int guildID, string newGuildName)
        {
            Packet pak = new Packet();
            pak.Addint(PAK_GUILDS_SETNAME);
            pak.Addint(guildID);
            pak.Addstring(newGuildName);
            pak.SendTo(WorldServer.world_players);
        }
        public static void SendGuildNotice(Guild guild)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(-1, PAK_GUILDS_NOTICE);
            pak.Addint(guild.guildID);
            pak.Addstring(guild.guildNotice);
            pak.SendTo(guild.getClients());
        }

        public static void SendGuildDisbanding(int guildID, string founderName)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(-1, PAK_GUILDS_DISBAND);
            pak.Addstring(founderName);
            pak.Addint(guildID);
            pak.SendTo(WorldServer.world_players);
        }
        public static void SendGuildNewMember(int id, string name, Guild guild)
        {
            Packet pak = new Packet();
            pak.Addint(PAK_GUILDS_NEWMEMBER);
            pak.Addint(id);
            pak.Addint(0x11c2c402);
            pak.Addint(guild.guildID);
            pak.Addstring(name);
            pak.SendTo(guild.getClients());
        }
        public static void SendGuildRemoveMember(int kickeeID, Guild guild)
        {
            Packet pak = new Packet();
            pak.Addint(PAK_GUILDS_DELMEMBER);
            pak.Addint(kickeeID);
            pak.Addint(guild.guildID);
            pak.SendTo(guild.getClients());
        }

        public static void SendGuildLogo(int guildID, int logoID)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(-1, PAK_GUILDS_SETLOGO);
            pak.Addint(guildID);
            pak.Addint(logoID);
            pak.SendTo(WorldServer.world_players);
        }

        public static void SendGuildClassChange(Guild guild, int destID, int NewClass)
        {
            Packet pak = new Packet();
            pak.Addint(PAK_GUILDS_SETCLASS);
            pak.Addint(destID);
            pak.Addint(NewClass);
            pak.SendTo(guild.getClients());
        }

        public static void SendGuildRankChange(Guild guild, int destID, int destRank)
        {
            Packet pak = new Packet();
            pak.Addint(PAK_GUILDS_SETRANK);
            pak.Addint(destID);
            pak.Addint(destRank);
            pak.SendTo(guild.getClients());
        }
    }
}

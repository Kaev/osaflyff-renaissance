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
            pak.Addint32(guild.guildID);
            pak.Addint32(guild.guildID);
            pak.Addint32(guild.founderID);
            pak.Addint32(0x0000000C);
            pak.Addstring(guild.guildName);
            pak.Addint32(guild.guildLogo);
            pak.Addint32(guild.bankPenya);
            pak.Addint32(guild.gwWins);
            pak.Addint32(guild.gwLoses);
            pak.Addint32(guild.gwForfeits);
            for (int i = 0; i < 5; i++)
                pak.Addint32(guild.memberPrivileges[i]);
            for (int i = 0; i < 5; i++)
                pak.Addint32(guild.memberPayment[i]);
            pak.Addstring(guild.guildNotice);
            pak.Addint32(guild.guildContributionEXP);
            pak.Addint32(guild.guildLevel);
            pak.Addint32(0);
            pak.Addint16(guild.members.Count);
            for (int i = 0; i < guild.members.Count; i++)
            {
                GuildMember member = (GuildMember)guild.members[i];
                Friend fm = BuildGuildMember(member.characterID);
                if (fm != null)
                {
                    pakMembers.StartNewMergedPacket(dwMoverID, PAK_ADD_FRIENDDATA);
                    pakMembers.Addint32(member.characterID);
                    pakMembers.Addstring(fm.strPlayerName);
                    pakMembers.Addbyte(fm.dwClass);
                    pakMembers.Addbyte(fm.dwLevel);
                    pakMembers.Addbyte(fm.dwGender);
                    pakMembers.Addbyte(0);
                    pakMembers.Addint32(0x11);
                    pakMembers.Addint32(0);
                    pak.Addint32(member.characterID);
                    pak.Addint32(0xFFFFFF9A);
                    pak.Addint32(member.penyaContribution);
                    pak.Addint32(member.questContribution);
                    pak.Addint16(0);
                    pak.Addint16(member.gwForfeits);
                    pak.Addint32(member.memberRank);
                    pak.Addint32(0);
                    pak.Addbyte(0);
                    pak.Addint32(member.memberRankSymbolCount);
                    pak.Addstring(member.memberNickname);
                }
                else
                {
                    pak.Addint32(-1);
                    pak.Addint32(0xFFFFFF9A);
                    pak.Addint64(0);
                    pak.Addint32(0);
                    pak.Addint32(4);
                    pak.Addint32(0);
                    pak.Addbyte(0);
                    pak.Addint32(0);
                    pak.Addstring("Void player");
                }
            }

            pak.Addint16(0);
            pak.Addbyte(1);
            pak.Addint32(1);
            pak.Addint32(0xe);
            pak.Addint32(0);
            //pak.Addhex("0000 01 01000000 0E000000 00000000");

            pak.Send(this);
            pakMembers.Send(this);
        }
        public void SendGuildPlayer(Guild guild)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(-1, PAK_ADD_FRIENDDATA);
            pak.Addint32(c_data.dwCharacterID);
            pak.Addstring(c_data.strPlayerName);
            pak.Addbyte(c_data.dwClass);
            pak.Addbyte(c_data.dwLevel);
            pak.Addbyte(c_data.dwGender);
            pak.Addbyte(0);
            pak.Addint32(0x11);
            pak.Addint32(0);
            pak.SendTo(guild.getClients());
        }
        public void SendGuildDataAll()
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_GUILDS_ALL);
            pak.Addint32(WorldServer.world_guilds.Count);
            pak.Addint32(WorldServer.world_guilds.Count);
            for (int i = 0; i < WorldServer.world_guilds.Count; i++)
            {
                Guild guild = WorldServer.world_guilds[i];
                pak.Addint32(guild.guildID);
                pak.Addint32(guild.founderID);
                pak.Addint32(0x0000000C);
                pak.Addstring(guild.guildName);
                pak.Addint32(guild.guildLogo);
                pak.Addint32(guild.bankPenya);
                pak.Addint32(guild.gwWins);
                pak.Addint32(guild.gwLoses);
                pak.Addint32(guild.gwForfeits);
            }
            pak.Addint32(1);
            for (int i = 0; i < 4; i++)
                pak.Addint64(0); // 8 dwords, remember that.
            pak.Send(this);
        }
        public void SendGuildOnJoin(int guildID)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_GUILDS_JOIN);
            pak.Addint32(guildID);
            pak.Send(this);
            SendToVisible(pak);
        }
        public void SendGuildInvitation(int guildID, int sourceID)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(-1, PAK_GUILDS_SENDINV);
            pak.Addint32(guildID);
            pak.Addint32(sourceID);
            pak.Send(this);
        }
        public void SendGuildDuelRequest(int fromGuildID, string fromGuildLeader)
        {
            Packet pak = new Packet();
            pak.Addint32(PAK_GUILDS_GDREQUEST);
            pak.Addint32(fromGuildID);
            pak.Addstring(fromGuildLeader);
            pak.Send(this);
        }
        public void SendGuildDuelTruce()
        {
            Packet pak = new Packet();
            pak.Addint32(PAK_GUILDS_GDTRUCEREQ);
            pak.Send(this);
        }
        public void SendGuildDuelState(int duelID)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_GUILDS_GDSTATE);
            pak.Addint32(duelID);
            pak.Send(this);
            SendToVisible(pak);
        }
        public void SendGuildDuelForfeit(int duelID, int charID, string charName)
        {
            Packet pak = new Packet();
            pak.Addint32(PAK_GUILDS_GDGIVEUP);
            pak.Addint32(duelID);
            pak.Addint32(charID);
            pak.Addstring(charName);
            pak.Addint32(0);
            pak.Send(this);
        }
        public void SendGuildSalaryUpdate(int rank, int salary, List<Client> clients)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(-1, PAK_GUILDS_SALARY);
            pak.Addint32(rank);
            pak.Addint32(salary);
            pak.SendTo(clients);
        }
        public void SendGuildTitlesUpdate(List<Client> clients, int[] privileges)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(-1, PAK_GUILDS_AUTHORITY);
            pak.Addint32(c_data.dwGuildID);
            for (int i = 1; i < 5; i++)
                pak.Addint32(privileges[i]);
            pak.SendTo(clients);
        }
    }
    public class GuildPackets : PacketCommands
    {
        public static void SendGuildDuelResult(GuildDuel gdinfo, int result)
        {
            Packet pak = new Packet();
            pak.Addint32(PAK_GUILDS_GDRESULT);
            pak.Addint32(gdinfo.dwDuelID);
            pak.Addint32(50); // unknown
            pak.Addint32(0); // unknown
            pak.Addint32(result);
            pak.SendTo(WorldServer.world_players);
        }
        public static void SendGuildDuelNew(GuildDuel gdinfo)
        {
            Packet pak = new Packet();
            pak.Addint32(PAK_GUILDS_GDSTART);
            pak.Addint32(gdinfo.dwDuelID);
            pak.Addint32(gdinfo.dwDefenderGuildID);
            pak.Addint32(gdinfo.dwAttackerGuildID);
            pak.SendTo(WorldServer.world_players);
        }
        public static void SendGuildCreation(int charID, int guildID, string charName)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(-1, PAK_GUILDS_NEW);
            pak.Addint32(charID);
            pak.Addint32(guildID);
            pak.Addstring(charName);
            pak.Addint32(0);
            pak.SendTo(WorldServer.world_players); // sendtoall
        }
        public static void SendGuildCreation(int charID, int guildID, string charName, string guildName)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(-1, PAK_GUILDS_NEW);
            pak.Addint32(charID);
            pak.Addint32(guildID);
            pak.Addstring(charName);
            pak.Addstring(guildName);
            pak.SendTo(WorldServer.world_players); // sendtoall
        }
        public static void SendGuildName(int guildID, string newGuildName)
        {
            Packet pak = new Packet();
            pak.Addint32(PAK_GUILDS_SETNAME);
            pak.Addint32(guildID);
            pak.Addstring(newGuildName);
            pak.SendTo(WorldServer.world_players);
        }
        public static void SendGuildNotice(Guild guild)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(-1, PAK_GUILDS_NOTICE);
            pak.Addint32(guild.guildID);
            pak.Addstring(guild.guildNotice);
            pak.SendTo(guild.getClients());
        }

        public static void SendGuildDisbanding(int guildID, string founderName)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(-1, PAK_GUILDS_DISBAND);
            pak.Addstring(founderName);
            pak.Addint32(guildID);
            pak.SendTo(WorldServer.world_players);
        }
        public static void SendGuildNewMember(int id, string name, Guild guild)
        {
            Packet pak = new Packet();
            pak.Addint32(PAK_GUILDS_NEWMEMBER);
            pak.Addint32(id);
            pak.Addint32(0x11c2c402);
            pak.Addint32(guild.guildID);
            pak.Addstring(name);
            pak.SendTo(guild.getClients());
        }
        public static void SendGuildRemoveMember(int kickeeID, Guild guild)
        {
            Packet pak = new Packet();
            pak.Addint32(PAK_GUILDS_DELMEMBER);
            pak.Addint32(kickeeID);
            pak.Addint32(guild.guildID);
            pak.SendTo(guild.getClients());
        }

        public static void SendGuildLogo(int guildID, int logoID)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(-1, PAK_GUILDS_SETLOGO);
            pak.Addint32(guildID);
            pak.Addint32(logoID);
            pak.SendTo(WorldServer.world_players);
        }

        public static void SendGuildClassChange(Guild guild, int destID, int NewClass)
        {
            Packet pak = new Packet();
            pak.Addint32(PAK_GUILDS_SETCLASS);
            pak.Addint32(destID);
            pak.Addint32(NewClass);
            pak.SendTo(guild.getClients());
        }

        public static void SendGuildRankChange(Guild guild, int destID, int destRank)
        {
            Packet pak = new Packet();
            pak.Addint32(PAK_GUILDS_SETRANK);
            pak.Addint32(destID);
            pak.Addint32(destRank);
            pak.SendTo(guild.getClients());
        }
    }
}

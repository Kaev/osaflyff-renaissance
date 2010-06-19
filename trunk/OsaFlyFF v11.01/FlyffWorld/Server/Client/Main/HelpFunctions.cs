using System;
using System.Collections;
using System.IO;
using System.Text;

namespace FlyffWorld
{
    public partial class Client
    {
        public void DeleteKeybind(int fkey, int fslot)
        {
            for (int i = 0; i < c_data.keybinds.Count; i++)
            {
                Keybind cur = c_data.keybinds[i];
                if (cur.dwKeyIndex == fkey && cur.dwPageIndex == fslot)
                    c_data.keybinds.Remove(cur);
            }
        }
        public void SpawnDrop(Drop drop) // Nicco->Drops
        {
            SendDropSpawn(drop);
            c_spawns.Add(drop);
        }
        public static JobState GetJobState(int job)
        {
            if (job == 0)
                return JobState.Vagrant;
            if (job < 6)
                return JobState.FirstJob;
            if (job < 16)
                return JobState.SecondJob;
            if (job < 24)
                return JobState.Master;
            return JobState.Hero;
        }
        public void DespawnDrop(Drop drop) // Nicco->Drops
        {
            c_spawns.Remove(drop);
            SendMoverDespawn(drop.dwMoverID);
        }
        public Friend GetFriendByID(int id)
        {
            for (int i = 0; i < c_data.friends.Count; i++)
            {
                Friend f = c_data.friends[i];
                if (id == f.dwCharacterID)
                    return f;
            }
            return null;
        }
        public static int LimitNumber(int num, int range_min, int range_max)
        {
            if (num < range_min)
                return range_min;
            if (num > range_max)
                return range_max;
            return num;
        }
        public static uint LimitNumber(uint num, uint range_min, uint range_max)
        {
            return (uint)LimitNumber((int)num, (int)range_min, (int)range_max);
        }
        public static void LimitNumber(ref int num, int range_min, int range_max)
        {
            num = LimitNumber(num, range_min, range_max);
        }
        public Friend BuildFriend(Client c)
        {
            Friend friend = new Friend();
            friend.nBlocked = 0;
            friend.strPlayerName = c.c_data.strPlayerName;
            friend.bOnline = true;
            friend.dwGender = c.c_data.dwGender;
            friend.dwCharacterID = c.c_data.dwCharacterID;
            friend.dwClass = c.c_data.dwClass;
            friend.dwLevel = c.c_data.dwLevel;
            friend.dwNetworkStatus = c.c_data.dwNetworkStatus;
            return friend;
        }
        public Friend BuildFriend(Client c, int b)
        {
            Friend friend = new Friend();
            friend.nBlocked = b;
            friend.strPlayerName = c.c_data.strPlayerName;
            friend.bOnline = true;
            friend.dwGender = c.c_data.dwGender;
            friend.dwCharacterID = c.c_data.dwCharacterID;
            friend.dwClass = c.c_data.dwClass;
            friend.dwLevel = c.c_data.dwLevel;
            friend.dwNetworkStatus = c.c_data.dwNetworkStatus;
            return friend;
        }
        public Skill GetSkillByID(int skillID)
        {
            for (int i = 0; i < c_data.skills.Count; i++)
            {
                Skill curSkill = c_data.skills[i];
                if (curSkill.dwSkillID == skillID)
                    return curSkill;
            }
            return null;
        }
        public Skill GetSkillByIndex(int index)
        {
            try
            {
                return c_data.skills[index];
            }
            catch (Exception) { return null; }
        }
        public Friend BuildGuildMember(int id) // We use Friend class because the friends and guildmember packets are identical (same header too)
        {
            Friend member = new Friend();
            ResultSet rs = new ResultSet("SELECT flyff_charactername,flyff_level,flyff_jobid,flyff_msgState,flyff_gender FROM flyff_characters WHERE flyff_characterid={0}", id);
            if (!rs.Advance())
            {
                Log.Write(Log.MessageType.warning, "Invalid guild member ID: {0}", id);
                Database.Execute("DELETE FROM flyff_guildmembers WHERE flyff_characterID = {0}", id);
                return null;
            }
            member.dwCharacterID = id;
            member.dwClass = rs.Readint("flyff_jobid");
            member.dwGender = rs.Readint("flyff_gender");
            member.strPlayerName = rs.Readstring("flyff_charactername");
            member.dwNetworkStatus = rs.Readint("flyff_msgState");
            member.dwLevel = rs.Readint("flyff_level");
            rs.Free();
            return member;
        }
        public Friend BuildFriend(int id, int blocked)
        {
            Friend friend = new Friend();
            ResultSet rs = new ResultSet("SELECT flyff_userid,flyff_charactername,flyff_level,flyff_jobid,flyff_msgState,flyff_gender FROM flyff_characters WHERE flyff_characterid={0}", id);
            if (!rs.Advance())
            {
                Log.Write(Log.MessageType.warning, "Invalid friend ID: {0}", id);
                Database.Execute("DELETE FROM flyff_friends WHERE flyff_charid1 = {0} OR flyff_charid2 = {0}", id);
                return null;
            }
            friend.dwCharacterID = id;
            friend.dwClass = rs.Readint("flyff_jobid");
            friend.dwGender = rs.Readint("flyff_gender");
            friend.strPlayerName = rs.Readstring("flyff_charactername");
            friend.nBlocked = blocked;
            friend.dwLevel = rs.Readint("flyff_level");
            int uid = rs.Readint("flyff_userid");
            rs.Free();
            Client c = WorldHelper.GetClientByPlayerID(id);
            if (c != null)
            {
                friend.bOnline = true;
                friend.dwNetworkStatus = c.c_data.dwNetworkStatus;
            }
            else
            {
                friend.bOnline = false;
                friend.dwNetworkStatus = Status.OFFLINE;
            }
            return friend;
        }
        public void ParseFilePacketType1(string[] c)
        {
            int id = 0;
            try
            {
                id = int.Parse(c[1]);
            }
            catch (Exception) { }
            IniFile f = new IniFile("packet.ini");
            string hex = f.ReadValue("p" + id, null);
            f.destroy();
            if (hex == null)
                return;
            Packet pak = new Packet();
            pak.Addint(0xFFFFFF00);
            pak.Addint(0);
            pak.Addshort(1);
            pak.Addint(dwMoverID);
            pak.Addhex(hex);
            pak.Send(this);
        }
        public void ParseFilePacketType2(string[] c)
        {
            int id = 0;
            try
            {
                id = int.Parse(c[1]);
            }
            catch (Exception) { }
            IniFile f = new IniFile("packet.ini");
            string hex = f.ReadValue("p" + id, null);
            f.destroy();
            if (hex == null)
                return;
            Packet pak = new Packet();
            pak.Addhex(hex);
            pak.Send(this);
        }
        public void ParseFilePacketType3(string[] c)
        {
            string id = "";
            try
            {
                id = c[1];
            }
            catch (Exception) { return; }
            try
            {
                // The easiest way would be to use LUA.
                Packet pak = new Packet();
                LuaInterface.Lua lua = new LuaInterface.Lua();
                if (!new FileInfo("ServerPackets\\" + id + ".fpak").Exists)
                {
                    SendMessageHud("File does not exist: ServerPackets\\" + id + ".fpak");
                    return;
                }
                lua["pak"] = pak;
                lua["bin"] = new LuaBinary();
                lua["MyMoverID"] = dwMoverID;
                lua = LuaRegisterer.registerFields(new PacketCommands(), LuaRegisterer.registerFields(c_data, lua));
                lua.DoFile("ServerPackets\\" + id + ".fpak");
                pak.Send(this);
            }
            catch (Exception e)
            {
                SendMessageHud("Error parsing fpak file: " + e.Message);
                return;
            }
        }
        public void SpawnMonster(Monster mob)
        {
            SendMonsterSpawn(mob);
            mob.SendMoverNewDestination();
            c_spawns.Add(mob);
        }
        public void DespawnMonster(Monster mob)
        {
            c_spawns.Remove(mob);
            SendMoverDespawn(mob.dwMoverID);
        }
        public void SpawnNPC(NPC npc)
        {
            c_spawns.Add(npc);
            SendNPCSpawn(npc);
        }
        public void DespawnNPC(NPC npc)
        {
            c_spawns.Remove(npc);
            SendMoverDespawn(npc.dwMoverID);
        }
        public void SpawnPlayer(Client c)
        {
            c_spawns.Add(c);
            SendPlayerSpawnOther(c);
        }
        public void DespawnPlayer(Client c)
        {
            c_spawns.Remove(c);
            SendMoverDespawn(c.dwMoverID);
        }
        public static bool ArrayContains(int[] array, int value)
        {
            for (int i = 0; i < array.Length; i++)
                if (array[i] == value)
                    return true;
            return false;
        }
        public void ResetUserAttributes() { }
    }
}

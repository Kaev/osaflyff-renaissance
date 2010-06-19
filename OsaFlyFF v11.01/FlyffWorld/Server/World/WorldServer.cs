using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace FlyffWorld
{
    public class WorldServer
    {
        public const int MAX_DATA_DROPS = 2048; // Max 2048 different >monsters<

        // WORLDSERVER DATA - LOADED DURING SERVER START
        public static List<SkillData> data_skills = new List<SkillData>();
        public static List<MonsterData> data_mobs = new List<MonsterData>();
        public static List<ItemData> data_items = new List<ItemData>();
        public static List<ItemBoxData> data_boxes = new List<ItemBoxData>();
        public static List<DropsData> data_drops = new List<DropsData>(MAX_DATA_DROPS);
        public static List<RevivalRegion> data_resrgns = new List<RevivalRegion>();


        // WORLDSERVER DYNAMIC - LOADED DURING SERVER START/DURING PLAYER ACTIONS
        public static List<Drop> world_drops = new List<Drop>();
        public static List<Monster> world_monsters = new List<Monster>();
        public static List<NPC> world_npcs = new List<NPC>();
        public static List<Warpzone> world_warpzones = new List<Warpzone>();
        public static List<NPCShopData> world_npcshops = new List<NPCShopData>();
        public static List<Client> world_players = new List<Client>();
        public static List<Guild> world_guilds = new List<Guild>();

        private static Random m_random = new Random(new Random(DLL.rand()).Next(int.MinValue, int.MaxValue));
        public static Random c_random
        {
            get
            {
                return m_random; 
            }
            set
            {
                m_random = value;
            }
        }
        public static void SetWeather(Weather weather)
        {
            Server.weather = (int)weather + 0x60;
            Packet pak = new Packet();
            pak.StartNewMergedPacket(-1, (uint)Server.weather);
            pak.Addlong(Server.weather != 0x60 ? 1 : 0);
            pak.SendTo(world_players);
        }
    }
    public enum Weather
    {
        None = 0,
        Snow = 1,
        Rain = 2
    }
}

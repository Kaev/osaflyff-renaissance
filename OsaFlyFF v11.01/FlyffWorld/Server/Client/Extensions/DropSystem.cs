using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using LuaInterface;

namespace FlyffWorld
{
    public partial class Client
    {
        public DropsData getDropsDataByMonster(Monster monster) { return getDropsDataByMonster(monster.Data.mobID); }
        public DropsData getDropsDataByMonster(int mob_id)
        {
            for (int i = 0; i < WorldServer.data_drops.Count; i++)
            {
                if (mob_id == WorldServer.data_drops[i].mob_ID)
                    return WorldServer.data_drops[i];
            }
            Log.Write(Log.MessageType.error, "No drops data were found for monster: {0}", mob_id);
            return null;
        }
        MonsterDropData mdd = new MonsterDropData();
        public void PickDrop(Drop drop) // Nicco->Drops
        {
            if (drop.ownerId != 0) // Owner id is dwMoverID of the owner, if 0 it has no owner
                if (drop.freeTime > DLL.time())
                {
                    SendMessageInfo(FlyFF.TID_GAME_PRIORITYITEMPER,
                        String.Format("\"{0}\"", drop.item.Data.itemName));
                    return;
                }
            if (drop.item.dwItemID >= 12 && drop.item.dwItemID <= 15)
            {
                int amount = drop.item.dwQuantity; // Amount of penya
                if ((c_data.dwPenya + amount) >= 2000000000)
                {
                    SendMessageInfo(FlyFF.TID_GAME_TOOMANYMONEY_USE_PERIN);
                    return;
                }
                c_data.dwPenya += amount;
                string
                    szAmount = String.Format("{0:#x###x###x###}", amount).Replace('x', ','),
                    szTotal = String.Format("{0:#x###x###x###}", c_data.dwPenya).Replace('x', ',');
                while (szAmount.StartsWith(","))
                    szAmount = szAmount.Substring(1);
                while (szTotal.StartsWith(","))
                    szTotal = szTotal.Substring(1);
                SendMessageInfo(FlyFF.TID_GAME_REAPMONEY,
                    String.Format("{0} {1}", szAmount, szTotal));
                SendPlayerPenya();
            }
            else
            {
                Slot slot = GetFirstAvailableSlot();
                if (slot == null)
                {
                    SendMessageInfo(FlyFF.TID_GAME_LACKSPACE);
                    return;
                }
                CreateItem(drop.item, slot);
                SendMessageInfo(FlyFF.TID_GAME_REAPITEM, String.Format("\"{0}\"", drop.item.Data.itemName));
            }
            DespawnDrop(drop);
            WorldServer.world_drops.Remove(drop);
        }
        public void DropMobItems(Monster monster, Client client, int mapID)
        {
            DropsData dd = getDropsDataByMonster(monster.Data.mobID);
            if (dd == null) // Mob doesnt have dropdata
                return;
            for (int i = 0; i < dd.TotalDrops; i++)
            {
                Drop drop = dd[i];
                double prob = WorldServer.c_random.Next(1, 300000001);
                if ((c_data.dwLevel + 1) > monster.Data.mobLevel)
                    prob *= 0.8; // 20% decrease
                Log.Write(Log.MessageType.debug, "item id {0} prob: if({1}<={2})",
                    drop.item.dwItemID, prob, drop.Probability);
                if (prob <= drop.Probability)
                {
                    drop.ownerId = client.dwMoverID;
                    drop.MapID = mapID;
                    drop.c_position = monster.c_position;
                    drop.despawnTime = DLL.clock() + 300000;
                    drop.freeTime = DLL.clock() + 60000;
                    WorldServer.world_drops.Add(drop);
                    SendDropSpawn(drop);
                }
            }
            if (client.c_data.dwLevel <= (monster.Data.mobLevel + 5))
            {
                Item pb = new Item();
                pb.dwQuantity = monster.Data.mobCash;
                int maxPenya = pb.dwQuantity + (monster.Data.mobLevel * 2);
                pb.dwQuantity = WorldServer.c_random.Next(pb.dwQuantity, maxPenya);
                pb.dwQuantity *= Server.penya_rate;
                if (pb.dwQuantity <= 10) pb.dwItemID = 12;
                else if (pb.dwQuantity <= 30) pb.dwItemID = 13;
                else if (pb.dwQuantity <= 50) pb.dwItemID = 14;
                else pb.dwItemID = 15;
                Drop pbDrop = new Drop(pb, dwMoverID, mapID, monster.c_position, false);
                pbDrop.c_position = monster.c_position;
                WorldServer.world_drops.Add(pbDrop);
                SendDropSpawn(pbDrop);
            }
        }
    }
    public class MonsterDropData2 // adds data into data_drops
    {
        public int MonsterID = -1;
        DropsData dd;
        public bool NewMonster(int monsterId)
        {
            try
            {
                if (dd != null)
                    dd = null;
                dd = new DropsData(monsterId);
            }
            catch (Exception) { return false; }
            return true;
        }
        public void AddDropProbability(int itemid, long probability)
        {
            if (MonsterID == -1)
                return;
            try
            {
                Item item = new Item();
                item.dwItemID = itemid;
                item.dwQuantity = 1;
                Drop drop = new Drop(item, 0, 1, new Point(), false);
                drop.Probability = probability;
                dd.AddDrop(drop);
            }
            catch (Exception e)
            {
                Log.Write(Log.MessageType.error, "Error making new drop for monster drop probabilitys! MonsterID: {0}, ItemID: {1}\r\nMessage: {2}",
                    MonsterID, itemid, e.Message);
                return;
            }
        }
        public bool AddDropsDataToArray()
        {
            for (int i = 0; i < WorldServer.data_drops.Count; i++)
                if (WorldServer.data_drops[i].mob_ID == MonsterID)
                {
                    Log.Write(Log.MessageType.error, "Double DropsData insertion to array!");
                    return false;
                }
            try
            {
                WorldServer.data_drops.Add(dd);
                dd = null;
            }
            catch (Exception) { return false; }
            return true;
        }
    }
    public class DropsData
    {
        public int mob_ID;
        public int TotalDrops { get { return drops.Count; } }
        private List<Drop> drops = new List<Drop>(WorldServer.MAX_DATA_DROPS);
        public Drop this[int index] { get { return drops[index]; } }
        public DropsData(int MonsterID)
        {
            mob_ID = MonsterID;
        }
        public void AddDrop(Drop drop)
        {
            drops.Add(drop);
        }
        public void RemoveDrop(Drop drop)
        {
            drops.Remove(drop);
        }
        public void RemoveDrop(int index)
        {
            drops.RemoveAt(index);
        }
        public void ClearDrops()
        {
            drops.Clear();
        }
    }
    public class MonsterDropData
    {
        public List<Item> items = new List<Item>();
        Random r
        {
            get
            {
                return WorldServer.c_random;
            }
        }
        public void Reset()
        {
            items.Clear();
        }
        public void AddDropProbability(int itemid, int probability) // 1/1000 !!!
        {
            int rnd = r.Next(0, 1000);
            for (int i = 0; i < Server.drop_rate; i++)
            {
                if (rnd <= probability)
                {
                    Item item = new Item();
                    item.dwItemID = itemid;
                    item.dwQuantity = 1;
                    items.Add(item);
                    break; // Only add one !
                }
            }
        }
    }
}
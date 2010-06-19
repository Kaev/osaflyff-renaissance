using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace FlyffWorld
{
    public partial class Client
    {
        public int count = 0;
        public bool isCollecting = false;
        CSVParser csv = new CSVParser("db\\rates\\collect.csv");

        public void CollectStart()
        {
            CollectRegion region = null;
            for (int i = 0; i < WorldServer.data_collrgns.Count; i++)
            {
                if (WorldServer.data_collrgns[i].IsInRegion(c_position, c_data.dwMapID))
                {
                    region = WorldServer.data_collrgns[i];
                    break;
                }
            }
            if (region != null)
            {
                SendCollectStart();
                isCollecting = true;
            }
            else
                SendPlayerInformationMessage(0xD2F);
        }
        public void CollectStop()
        {
            SendCollectStop();
            count = 0;
            isCollecting = false;
        }
        public void Collect()
        {
            timers.nextCollectDecreaseCharge = DLL.time() + 1;
            count++;
            Slot weapon = GetSlotByPosition(52);
            int time = csv.GetIntList(0)[weapon.c_item.dwRefine];
         //   ActiveItems activegoldbattery = GetPlayerActivatedItem(26454);
         //   ActiveItems activesilverbattery = GetPlayerActivatedItem(26455);
         //   if ((activegoldbattery == null) || (activesilverbattery == null))
          //  {
                if (weapon.c_item.dwCharge == 0)
                    CollectStop();
                else
                {
                    weapon.c_item.dwCharge--;
                    if (count == time+1) //add 1 to give the item after the {time}'s second
                    {
                        count = 0;
                        GiveCollectedItem(weapon);
                    }
                    else
                    {
                        SendItemUpdate(new ItemUpdateStatus(weapon.dwID, ITEM_MODTYPE_CHARGE, weapon.c_item.dwCharge, weapon.c_item.dwCharge));
                    }
                }
            /*  }
              else
              {
                  if (count == time+1) //add 1 to give the item after the {time}'s second
                  {
                      count = 0;
                      GiveCollectedItem(weapon);
                  }
                  else
                  {
                      SendItemUpdate(new ItemUpdateStatus(weapon.dwID, ITEM_MODTYPE_CHARGE, weapon.c_item.dwCharge, 0));
                  }
              }*/
        }
        public void GiveCollectedItem(Slot collector)
        { //randomisation by Maas. [seymour] need to change later to read items from a file
            int[] itemsid = new int[] { 26231, 26230, 26233, 26229, 26220, 26222, 26223, 26232, 26221, 26227, 26224, 26225, 26226, 26228, 30017, 30018, 30019, 30016, 30020 };
            int from = 0;
            int to = 18;
            int random = DiceRoller.RandomNumber(from, to);
            Item item = new Item() { dwItemID = itemsid[random], dwQuantity = 1 };
            Slot slotexist = GetSlotByItemID(itemsid[random]);
            Slot slot = GetFirstAvailableSlot();

            if (slotexist == null) //if payer don't have the item in is inventory
            {         
                if (slot == null || slot.c_item != null)
                {
                    SendMessageInfo(TID_GAME_LACKSPACE);
                    CollectStop();
                    isCollecting = false;
                    return;
                }
                slot.c_item = item;
                SendCollectedItem(collector, slot);
                return;
            }
            if (slotexist.c_item.dwQuantity == item.Data.stackMax)
            {
                slot.c_item = item;
                SendCollectedItem(collector, slot);
                return;
            }
            slotexist.c_item.dwQuantity += item.dwQuantity;
            SendCollectedItem(collector, slotexist);
            return;
        }
    }
}
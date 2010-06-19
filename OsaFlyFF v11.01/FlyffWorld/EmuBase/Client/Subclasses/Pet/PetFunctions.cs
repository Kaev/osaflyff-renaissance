using System;
using System.Collections.Generic;
using System.Text;

namespace FlyffWorld
{
    public partial class Client
    {
        public void PetCreateFood(DataPacket dp)
        {
            int itemID = dp.Readint32();
            dp.Readint32();
            int quantity = dp.Readint32();

            Slot slot = GetSlotByID(itemID);
            if (slot == null)
                return;

            int modifier = slot.c_item.Data.reqLevel / 5;

            int foodquantity = DiceRoller.RandomNumber(2, 20) * modifier * quantity;

            Item food = new Item();
            food.dwItemID = 21037;
            food.dwQuantity = foodquantity;
            //we need to check in inventory if we have already some food
            for (int i = 0; i < c_data.inventory.Count; i++)
            {
                Slot curslot = GetSlotByItemID(21037);
                if (curslot == null)
                    continue;
                else  //if we already have this item type
                {
                    if (curslot.c_item.dwQuantity == food.Data.stackMax)
                        continue;
                    curslot.c_item.dwQuantity += food.dwQuantity;
                    if (curslot.c_item.dwQuantity > curslot.c_item.Data.stackMax) //if we have more than stackmax
                    {
                        int remaining = curslot.c_item.dwQuantity - curslot.c_item.Data.stackMax;
                        curslot.c_item.dwQuantity = curslot.c_item.Data.stackMax;
                        //update actual qantity and create new item for remaining
                        SendItemUpdate(new ItemUpdateStatus(curslot.dwID, ITEM_MODTYPE_QUANTITY, curslot.c_item.dwQuantity, 0));
                        food.dwQuantity = remaining;
                        CreateItem(food);
                        SendFoodCreationResult(foodquantity);
                        DecreaseQuantity(slot, quantity);
                        return;
                    }
                    else
                    {
                        //we have less that stackmax
                        Log.Write(Log.MessageType.debug, "Quantity = {0}", curslot.c_item.dwQuantity);
                        SendItemUpdate(new ItemUpdateStatus(curslot.dwID, ITEM_MODTYPE_QUANTITY, curslot.c_item.dwQuantity, 0));
                        SendFoodCreationResult(foodquantity);
                        DecreaseQuantity(slot, quantity);
                        return;
                    }
                }
            }
            //if we are here it's because we haven't food in inventory
            CreateItem(food);
            DecreaseQuantity(slot, quantity);
            SendFoodCreationResult(foodquantity);
            return;
        }
    }
}


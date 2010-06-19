using System;
using System.Collections.Generic;

using System.Text;

namespace FlyffWorld
{
    public class Bank
    {
        public int[] dwPenyaArr = new int[3];
        public int[] dwCharacterIDArr = new int[] { 0, 0, 0 };
        public List<Item>[] bankItems = new List<Item>[3];
        public Bank()
        {
            for (int i = 0; i < bankItems.Length; i++)
                bankItems[i] = new List<Item>();
        }
        public string strPassword = "";


    }
    public partial class Client
    {
        public void BankCreateNewPassword(DataPacket dp)
        {
            if ((c_data.bank.strPassword == "") || (c_data.bank.strPassword == null))//we haven't define password
            {
                dp.Readint32();
                int addconstant1 = dp.Readbyte();
                int addconstant2 = dp.Readbyte();
                int addconstant3 = dp.Readbyte();
                int addconstant4 = dp.Readbyte();
                dp.Readint32();
                int addandvalue1 = dp.Readbyte() - addconstant1;
                int addandvalue2 = dp.Readbyte() - addconstant2;
                int addandvalue3 = dp.Readbyte() - addconstant3;
                int addandvalue4 = dp.Readbyte() - addconstant4;

                string spassword = String.Concat(addandvalue1.ToString(), addandvalue2.ToString(), addandvalue3.ToString(), addandvalue4.ToString());
                Database.Execute("UPDATE flyff_characters SET flyff_bankpassword='{0}' WHERE flyff_characterid={1}", spassword, c_data.dwCharacterID);
                c_data.bank.strPassword = spassword;
                BankAskPassword();
            }
            else //we want to change password
            {
                dp.Readint32();
                int addconstant1 = dp.Readbyte() - 48;
                int addconstant2 = dp.Readbyte() - 48;
                int addconstant3 = dp.Readbyte() - 48;
                int addconstant4 = dp.Readbyte() - 48;
                Log.Write(Log.MessageType.debug, "My old password contain : {0},{1},{2},{3}", addconstant1, addconstant2, addconstant3, addconstant4);
                string oldpassword = String.Concat(addconstant1.ToString(), addconstant2.ToString(), addconstant3.ToString(), addconstant4.ToString());
                if (oldpassword != c_data.bank.strPassword)
                {
                    sendBankPassword(0);
                }
                else
                {
                    dp.Readint32();
                    int addandvalue1 = dp.Readbyte() - 48; //0x30 = 48 in decimal
                    int addandvalue2 = dp.Readbyte() - 48;
                    int addandvalue3 = dp.Readbyte() - 48;
                    int addandvalue4 = dp.Readbyte() - 48;
                    string spassword = String.Concat(addandvalue1.ToString(), addandvalue2.ToString(), addandvalue3.ToString(), addandvalue4.ToString());
                    c_data.bank.strPassword = spassword;
                    BankAskPassword();
                }

            }
        }
        public void BankAskPassword()
        {
            //if It's first time we go in bank we need to set password
            if ((c_data.bank.strPassword == "") || (c_data.bank.strPassword == null))
                sendAskPassword(0); //we ask a new password
            else
                sendAskPassword(1);
        }
        public void BankCheckPassword(DataPacket dp)
        {
            dp.Readint32();
            int addandvalue1 = dp.Readbyte() - 48;
            int addandvalue2 = dp.Readbyte() - 48;
            int addandvalue3 = dp.Readbyte() - 48;
            int addandvalue4 = dp.Readbyte() - 48;

            string spassword = String.Concat(addandvalue1.ToString(), addandvalue2.ToString(), addandvalue3.ToString(), addandvalue4.ToString());

            if (spassword == c_data.bank.strPassword)
            {
                //open the bank windows
                sendBankPassword(1);
            }
            else
            {
                //say wrong password
                sendBankPassword(0);
            }
        }
        public int GetBanknumber()
        {
            for (int i = 0; i < c_data.bank.dwCharacterIDArr.Length; i++)
            {
                if (c_data.bank.dwCharacterIDArr[i] == c_data.dwCharacterID)
                    return i;
            }
            return -1;
        }

        //need to add addbankitem

        public void BankAddItem(DataPacket dp)
        {
            int bankposition = dp.Readbyte();
            int slotid = dp.Readbyte();
            int quantity = dp.Readint16();
            Slot slot = GetSlotByPosition(slotid);
            if (slot == null || quantity == 0)
                return;

            //if player bank is full he can't add item :
            if (c_data.bank.bankItems[bankposition].Count == 42)
            {
                SendMessageHud("Your bank is full, you can't add item");
                return;
            }
            Item item = CopyItem(slot.c_item);
            item.dwQuantity = quantity; //we give to bank only quantity given


            //we need now to add it in bank, but if it's a stakable item we neeed to add it to existing object
            for (int i = 0; i < c_data.bank.bankItems[bankposition].Count; i++)
            {

                //so we check in all item in bank if we have it
                Item curItem = c_data.bank.bankItems[bankposition][i];
                if (curItem.dwItemID == item.dwItemID)
                {

                    //ok but can we put more than one item in a slot ?
                    if (curItem.Data.stackMax > 1)
                    {
                        
                        //ok so we add item to existing item
                        curItem.dwQuantity += item.dwQuantity;
                        if (curItem.dwQuantity > curItem.Data.stackMax)
                        {
                           
                            //if we have more than stackmax
                            int morethanmax = curItem.dwQuantity - curItem.Data.stackMax;
                            //ok now we fix existing item to stack max and add the remaineing
                            c_data.bank.bankItems[bankposition][i].dwQuantity = curItem.Data.stackMax;
                            //we add a new item in bank
                            Item newitem = new Item();
                            newitem = curItem;
                            newitem.dwQuantity = morethanmax;
                            c_data.bank.bankItems[bankposition].Add(newitem);
                            //ok now we send packet.
                            SendBankAddItem(item, item.dwQuantity, bankposition);

                            if (slot.c_item.dwQuantity <= 0) //we need to delete item
                            
                                DeleteItem(slot);
                            
                            else
                            
                                DecreaseQuantity(slot, quantity);
                            
                            return;

                        }
                        else
                        {

                            //ok we don't have more than stack max, we update bank quantity
                            c_data.bank.bankItems[bankposition][i].dwQuantity = curItem.dwQuantity;
                            //ok now we send packet.
                            SendBankAddItem(item, item.dwQuantity, bankposition);

                            if (slot.c_item.dwQuantity <= 0) //we need to delete item
                                DeleteItem(slot);

                            else
                                DecreaseQuantity(slot, quantity);

                            return;

                        }
                    }
                    else
                    {
                        //we can't put more than 1 item in a slot so we add this item to the bank

                        c_data.bank.bankItems[bankposition].Add(curItem);
                        //ok now we send packet.
                        SendBankAddItem(item, item.dwQuantity, bankposition);

                        if (slot.c_item.dwQuantity <= 0) //we need to delete item
                            DeleteItem(slot);

                        else
                            DecreaseQuantity(slot, quantity);

                        return;

                    }
                }
            }

            //if there is nothin in bank or if there isn't any of this item in bank:

            c_data.bank.bankItems[bankposition].Add(item);

            //ok now we send packet.
            SendBankAddItem(item, item.dwQuantity, bankposition);

            if (slot.c_item.dwQuantity <= 0) //we need to delete item
                DeleteItem(slot);

            else
                DecreaseQuantity(slot, quantity);


        }

        public void BankAddPenya(DataPacket dp)
        {
            int bankpos = dp.Readbyte();
            int quantity = dp.Readint32();

            if (quantity == 0)
                return;

            //check if player has those penya in inventory
            if (c_data.dwPenya < quantity)
            {
                quantity = c_data.dwPenya;
            }
            c_data.dwPenya -= quantity;
            int bankposition = GetBanknumber();
            c_data.bank.dwPenyaArr[bankposition] += quantity;
            SendPlayerPenya();
            SendBankUpdatePenya(c_data.bank.dwPenyaArr[bankposition], bankposition);

        }
        public void BankTakeItem(DataPacket dp)
        {
            int bankposition = dp.Readbyte();
            int bankslot = dp.Readbyte();
            int quantity = dp.Readint16();
            int itemremaining = 0;
            if (quantity == 0)
                return;

            Item item = CopyItem(c_data.bank.bankItems[bankposition][bankslot]); //copy of the item to put in player inventory
            item.dwQuantity = quantity;
            

            //OK now we update item in bank
            c_data.bank.bankItems[bankposition][bankslot].dwQuantity -= quantity;//update quantity in bank

            if (c_data.bank.bankItems[bankposition][bankslot].dwQuantity != 0)
            {

                
                itemremaining = c_data.bank.bankItems[bankposition][bankslot].dwQuantity;
                SendBankTakeItem(c_data.bank.bankItems[bankposition][bankslot], bankposition, bankslot, itemremaining, quantity);
            }
            else
            {
                
                itemremaining = 0;
                SendBankTakeItem(c_data.bank.bankItems[bankposition][bankslot], bankposition, bankslot, itemremaining, quantity);
                c_data.bank.bankItems[bankposition].RemoveAt(bankslot); //we have take all item
            }
            //now take a look into player inventory.
            for (int i = 0; i < c_data.inventory.Count; i++)
            {
                
                if (c_data.inventory[i] == null)
                    continue;
                Slot newslot = new Slot();
                newslot = c_data.inventory[i];
                Item curitem;
                if (newslot.c_item == null)
                    continue;
                curitem = CopyItem(newslot.c_item);
                if (curitem == null)
                    continue;
                //we search if we have already this type of item in inventory
                if (curitem.dwItemID == item.dwItemID)
                {
                    //does this item allow to put more than one in a slot ?
                    if (curitem.Data.stackMax > 1)
                    {
                        
                        c_data.inventory[i].c_item.dwQuantity += quantity;
                        if (c_data.inventory[i].c_item.dwQuantity > curitem.Data.stackMax)
                        {
                            
                            int morethanmax = curitem.dwQuantity - curitem.Data.stackMax;
                            //ok so now we update curent item and add a new item in inventory with remaining..
                            c_data.inventory[i].c_item.dwQuantity = curitem.Data.stackMax; //actual iteù in ionventory is maxed
                            SendItemUpdate(new ItemUpdateStatus(c_data.inventory[i].dwID, ITEM_MODTYPE_QUANTITY, curitem.Data.stackMax, 0)); //update actual item in inventory
                            item.dwQuantity = morethanmax;
                            CreateItem(item); //create a new item with remaining number
                            
                            return;
                        }
                        else
                        {
                            
                            //we have just to update actual item in inventory
                            SendItemUpdate(new ItemUpdateStatus(c_data.inventory[i].dwID, ITEM_MODTYPE_QUANTITY, c_data.inventory[i].c_item.dwQuantity, 0)); //update actual item in inventory
                            return;
                        }
                    }
                    else
                    {
                        
                        //we can't add in the same slot so we create another one
                        CreateItem(item); //create a new item with remaining number
                        return;
                    }
                }
            }
            //we haven't this item into our inventory or we haven't anything in inventory
            
            CreateItem(item);
        }

        public void BankTakePenya(DataPacket dp)
        {
            int bankposition = dp.Readbyte();
            int quantity = dp.Readint32();

            if (quantity == 0)
                return;
            //check if player has those penya in bank
            if (c_data.bank.dwPenyaArr[bankposition] < quantity)
            {
                quantity = c_data.bank.dwPenyaArr[bankposition];
            }

            c_data.bank.dwPenyaArr[bankposition] -= quantity;
            c_data.dwPenya += quantity;
            SendBankUpdatePenya(c_data.bank.dwPenyaArr[bankposition], bankposition);

        }

    }
}
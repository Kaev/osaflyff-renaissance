using System;
using System.Collections.Generic;
using System.Text;

namespace FlyffWorld
{
    public partial class Client
    {
        public void MailShowList()
        {

            if (c_data.receivedmails.Count != 0)
            {
                int unixTime = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
                SendMailList(c_data.receivedmails, unixTime);
            }
        }
        public void MailSendMessage(DataPacket dp)
        {
            
            int itemslotid = dp.Readbyte(); //-1 if there isn't an item
            Log.Write(Log.MessageType.debug, "Slot id : {0}", itemslotid);
            int itemquantity = dp.Readint16();
            string targetplayername = dp.Readstring();
            int penya = dp.Readint32();
            string topic = dp.Readstring();
            string message = dp.Readstring();
            #region Checkmoney
            if (c_data.dwPenya < (500 + penya)) //if player hasn't suffisent penya...
            {
                SendMessageInfo(FlyFF.TID_GAME_LACKMONEY);
                return;
            }
            #endregion

            #region Check if player exist
            Client target;
            bool isinlist = false;
            int targetcharid = -1;
            for (int i = 0; i < WorldServer.world_characterlist.Count; i++)
            {
                if (WorldServer.world_characterlist[i].CharacterName == targetplayername)
                {
                    isinlist = true;
                    targetcharid = WorldServer.world_characterlist[i].dwCharID;
                }
            }
            if (isinlist == false || targetcharid==-1)
            {
               SendMessageUsageInfo(2,"Unknown charactername");
                return;
            }
            #endregion
            target = WorldHelper.GetClientByPlayerName(targetplayername);

            #region create new mail
            Mails newmail = new Mails();
            Item item;
            Slot slot =null;
            if (itemslotid == 0xFF)
            {
                item = new Item();
            }
            else
            {
                slot = GetSlotByID(itemslotid);
                if (slot == null)
                    item = new Item();
                else
                {

                    item = CopyItem(slot.c_item);
                }
            }
            item.dwQuantity = itemquantity;
            newmail.attachedItem = item;
            newmail.attachedPenya = penya;
            newmail.topic = topic;
            newmail.toCharID = targetcharid;
            newmail.fromCharID = c_data.dwCharacterID;
            newmail.message = message;
            newmail.date = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
            
            newmail.mailid = WorldServer.world_mails.Count+1; //mailid need to be fixed to a value we can't use autoincrement of database
            #endregion
            WorldServer.world_mails.Add(newmail);
            c_data.dwPenya -= (500 + penya);
            SendPlayerPenya();
            c_data.sentmails.Add(newmail);            
            if (target != null)
            {
                target.SendMailNewMessageFLAG();                
                target.c_data.receivedmails.Add(newmail);
                //send a message to target to say he has a message
            }

            SendMessageUsageInfo(2, "You have sent mail");
            //update inventory
            if (itemslotid != 0xFF)
            {
                slot.c_item.dwQuantity -= itemquantity;
                DecreaseQuantity(slot, itemquantity);
            }


        }
        public static Mails GetMailFromID(int id)
        {
            for (int i = 0; i < WorldServer.world_mails.Count; i++)
            {
                if (WorldServer.world_mails[i].mailid == id)
                    return WorldServer.world_mails[i];

            }
            return null;
        }
        public void MailReadMessage(DataPacket dp)
        {
            int mailid = dp.Readint32();
            
            Mails mail = GetMailFromID(mailid);
            if (mail == null)
                return;
            
            //update world_mails
            for (int i = 0; i < WorldServer.world_mails.Count; i++)
            {
                if (WorldServer.world_mails[i].mailid == mailid)
                   WorldServer.world_mails[i].isRead = 1;                   
            }
            //update my received list
            for (int i = 0; i < c_data.receivedmails.Count; i++)
            {
                if (c_data.receivedmails[i].mailid == mailid)
                   c_data.receivedmails[i].isRead = 1;
                    
             }
            //update sender list if he is online
            Client sender;
            
            sender = WorldHelper.GetClientByPlayerID(mail.fromCharID);
            if (sender == null)
            {
                SendMailRead(mailid, MAIL_READ_TAKENOTHING);
                return;
            }
            if (sender != null)
            {
                if (sender.c_data.sentmails.Count!=0)
                for (int i = 0; i < sender.c_data.sentmails.Count; i++)
                {
                    if (sender.c_data.sentmails[i] == null)
                        break;
                    if (sender.c_data.sentmails[i].mailid == mailid)
                       sender.c_data.sentmails[i].isRead = 1;
                }
            }
            SendMailRead(mailid, MAIL_READ_TAKENOTHING);
            if (sender == null)
                return;

        }
        public void MailTakeMoney(DataPacket dp)
        {
            int mailid = dp.Readint32();
            Mails mail = GetMailFromID(mailid);
            if (mail == null)
                return;
            c_data.dwPenya += mail.attachedPenya;
            SendPlayerPenya();
            //update world_mails
            for (int i = 0; i < WorldServer.world_mails.Count; i++)
            {
                if (WorldServer.world_mails[i].mailid == mailid)
                    WorldServer.world_mails[i].attachedPenya = 0;

            }
            //update my received list
            for (int i = 0; i < c_data.receivedmails.Count; i++)
            {
                if (c_data.receivedmails[i].mailid == mailid)
                    c_data.receivedmails[i].attachedPenya = 0;
            }
            //update sender list if he is online
            Client sender = WorldHelper.GetClientByPlayerID(GetMailFromID(mailid).fromCharID);
            if (sender != null)
            {
                for (int i = 0; i < sender.c_data.sentmails.Count; i++)
                {
                    if (sender.c_data.sentmails[i].mailid == mailid)
                        sender.c_data.sentmails[i].attachedPenya = 0;
                }
            }
            SendMailRead(mailid, MAIL_READ_MONEY);


        }
        public void MailTakeObject(DataPacket dp)
        {
            int mailid = dp.Readint32();
            Mails mail = GetMailFromID(mailid);
            if (mail == null)
                return;

            if (mail.attachedItem.dwItemID == 0)
                return;
            
            #region manage playerinventory
            //ad Item in inventory
           
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
                if (curitem.dwItemID == mail.attachedItem.dwItemID)
                {
                    
                    //does this item allow to put more than one in a slot ?
                    if (curitem.Data.stackMax > 1)
                    {
                        
                        c_data.inventory[i].c_item.dwQuantity += mail.attachedItem.dwQuantity;
                        if (c_data.inventory[i].c_item.dwQuantity > curitem.Data.stackMax)
                        {
                            
                            int morethanmax = curitem.dwQuantity - curitem.Data.stackMax;
                            //ok so now we update curent item and add a new item in inventory with remaining..
                            c_data.inventory[i].c_item.dwQuantity = curitem.Data.stackMax; //actual iteù in ionventory is maxed
                            SendItemUpdate(new ItemUpdateStatus(c_data.inventory[i].dwID, ITEM_MODTYPE_QUANTITY, curitem.Data.stackMax, 0)); //update actual item in inventory
                            mail.attachedItem.dwQuantity = morethanmax;
                            CreateItem(mail.attachedItem); //create a new item with remaining number
                            SendMailRead(mailid, MAIL_READ_TAKEOBJECT);
                            UpdateListForObject(mailid);
                            return;
                        }
                        else
                        {
                            
                            //we have just to update actual item in inventory
                            SendItemUpdate(new ItemUpdateStatus(c_data.inventory[i].dwID, ITEM_MODTYPE_QUANTITY, c_data.inventory[i].c_item.dwQuantity, 0)); //update actual item in inventory
                            SendMailRead(mailid, MAIL_READ_TAKEOBJECT);
                            UpdateListForObject(mailid);
                            return;
                        }
                    }
                    else
                    {
                        
                        //we can't add in the same slot so we create another one
                        CreateItem(mail.attachedItem); //create a new item with remaining number
                        SendMailRead(mailid, MAIL_READ_TAKEOBJECT);
                        UpdateListForObject(mailid);
                        return;
                    }
                }
            }
            
            //we haven't this item into our inventory or we haven't anything in inventory
            Item newitem = CopyItem(mail.attachedItem);
            CreateItem(newitem);
            
            SendMailRead(mailid, MAIL_READ_TAKEOBJECT);
            UpdateListForObject(mailid);
            #endregion
        }
        public void MailDelete(DataPacket dp)
        {
            int mailid = dp.Readint32();
            Mails mail = GetMailFromID(mailid);
            if (mail == null)
                return;
            //update world_mails
            for (int i = WorldServer.world_mails.Count; i >0 ; i--)
            {
                if (WorldServer.world_mails[i].mailid == mailid)
                    WorldServer.world_mails.Remove(WorldServer.world_mails[i]);

            }
            //update my received list
            for (int i = c_data.receivedmails.Count; i >0 ; i--)
            {
                if (c_data.receivedmails[i].mailid == mailid)
                    c_data.receivedmails.Remove(c_data.receivedmails[i]);
            }
            //update sender list if he is online
            
            if (mail == null)
                return;
            Client sender = WorldHelper.GetClientByPlayerID(mail.fromCharID);
            if (sender == null)
            {
                SendMailRead(mailid, MAIL_SUPPRESS);
                MailShowList();
                return;
            }
            if (sender != null)
            {
                for (int i = sender.c_data.sentmails.Count; i >0 ; i--)
                {
                    if (sender.c_data.sentmails[i].mailid == mailid)
                        sender.c_data.sentmails.Remove(sender.c_data.sentmails[i]);
                }
            }
            SendMailRead(mailid, MAIL_SUPPRESS);
            MailShowList();

        }
        public void UpdateListForObject(int mailid)
        {
            #region update Lists
            //update world_mails
            for (int i = 0; i < WorldServer.world_mails.Count; i++)
            {
                if (WorldServer.world_mails[i].mailid == mailid)
                    WorldServer.world_mails[i].attachedItem = new Item();

            }
            //update my received list
            for (int i = 0; i < c_data.receivedmails.Count; i++)
            {
                if (c_data.receivedmails[i].mailid == mailid)
                    c_data.receivedmails[i].attachedItem = new Item();
            }
            //update sender list if he is online
            Client sender = WorldHelper.GetClientByPlayerID(GetMailFromID(mailid).fromCharID);
            if (sender != null)
            {
                for (int i = 0; i < sender.c_data.sentmails.Count; i++)
                {
                    if (sender.c_data.sentmails[i].mailid == mailid)
                        sender.c_data.sentmails[i].attachedItem = new Item();
                }
            }
            #endregion
        }
    }
}

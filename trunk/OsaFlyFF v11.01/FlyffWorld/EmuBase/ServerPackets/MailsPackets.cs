using System;
using System.Collections.Generic;
using System.Text;

namespace FlyffWorld
{
    public partial class Client
    {

        public void SendMailList(List<Mails> Mails, int unixTime)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_MAIL_SHOWLIST);
            pak.Addint32(c_data.dwCharacterID);
            pak.Addint32(Mails.Count);
            for (int i=0;i<Mails.Count;i++)
            {
                pak.Addint32(Mails[i].mailid);
                pak.Addint32(Mails[i].fromCharID); 
                if (Mails[i].attachedItem.dwItemID != 0)
                {
                    Item item = Mails[i].attachedItem;
                    //add data item
                    pak.Addbyte(1); //yes there is an object
                    pak.Addint32(-1);
                    
                    pak.Addint32(item.dwItemID);
                    pak.Addint32(0);//time remaining ?
                    pak.Addint32(0);
                    pak.Addint16(item.dwQuantity);
                    pak.Addbyte(0);
                    if (item.Data.itemkind[2] > 24 && item.Data.itemkind[2] != 57 || item.Data.itemkind[2] != 58)//if it's an item that don't need endurance value
                    {
                        pak.Addint32(-1);
                        pak.Addint32(0);
                    }
                    else
                        pak.Addint64(item.Data.endurance);
                    pak.Addbyte(0);
                    pak.Addint32(item.dwRefine);
                    pak.Addint32(0);
                    pak.Addbyte(item.dwElement);
                    pak.Addint32(item.dwEleRefine);
                    pak.Addint32(0);
                    pak.Addbyte(item.c_sockets.Length);//number of socket
                    for (int j = 0; j < item.c_sockets.Length; j++)
                    {
                        pak.Addint16(item.c_sockets[j]);
                    }
                    pak.Addbyte(0);
                    pak.AddUint64(item.c_awakening);
                    pak.Addint64(0);
                }
                else
                    pak.Addbyte(0); //0 if there is no object linked 
                pak.Addint32(Mails[i].attachedPenya);
                pak.Addint32(unixTime-Mails[i].date); //nomber of second beetween now and when mail has been send
                pak.Addbyte(Mails[i].isRead); //0: not read, 1:read
                pak.Addstring(Mails[i].topic);
                pak.Addstring(Mails[i].message);
            }
            pak.Send(this);

        }
        
        public void SendMailNewMessageFLAG()
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_PLAYER_FLAGS);
            pak.Addint32(0x00028000);
            pak.Send(this);

        }
        public void SendMailRead(int mailid, int mailmodifier)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_MAIL_READMAIL);
            pak.Addint32(mailid);
            pak.Addint32(mailmodifier);
            pak.Send(this);

        }
        public void SendMailYouHaveNewMail(Client target)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_PLAYER_FLAGS);
            pak.Addint32(0x00028000);
            pak.Send(target);
        }
        
    }
}

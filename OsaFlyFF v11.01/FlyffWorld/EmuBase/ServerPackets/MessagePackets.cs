using System;
using System.Collections.Generic;
using System.Text;

namespace FlyffWorld
{
    public partial class Client
    {
        public void SendMessagePM(string strFrom, string strMessage)
        {
            Packet pak = new Packet();
            pak.Addint32(PAK_CHAT_SAY);
            pak.Addstring(strFrom);
            pak.Addstring(this.c_data.strPlayerName);
            pak.Addstring(strMessage);
            pak.Send(this);
        }
        public void SendMessagePM(Client from, Client to, string msg)
        {
            Packet pak = new Packet();
            pak.Addint32(PAK_CHAT_SAY);
            pak.Addstring(from.c_data.strPlayerName);
            pak.Addint32(0);
            pak.Addstring(msg);
            pak.Send(to);
            pak = new Packet();
            pak.Addint32(PAK_CHAT_SAY);
            pak.Addstring(from.c_data.strPlayerName);
            pak.Addstring(to.c_data.strPlayerName);
            pak.Addstring(msg);
            pak.Send(from);
        }
        public void SendMessageHud(string message)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_HUD_MESSAGE);
            pak.Addstring(message);
            pak.Addint32(-54321234);
            pak.Send(this);
        }
        public void SendMessageInfo(int i)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_FLYFF_INFORMATION);
            pak.Addint32(i);
            pak.Addint32(0);
            pak.Send(this);
        }
        public void SendMessageUsageInfo(int i,string s)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_SEND_USAGEINFO);
            pak.Addbyte(i);
            pak.Addstring(s);
            pak.Send(this);
        }
        public void SendMessageInfo(int i, string s)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_FLYFF_INFORMATION);
            pak.Addint32(i);
            pak.Addstring(s);
            pak.Send(this);
        }
        public void SendMessageEventNotice(int i)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_FLYFF_EVENTMSG);
            pak.Addint32(0);
            pak.Addint32(i);
            pak.Addint32(0);
            pak.Send(this);
        }
        public void SendMessageEventNotice(int i, string s)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_FLYFF_EVENTMSG);
            pak.Addint32(0);
            pak.Addint32(i);
            pak.Addstring(s); // may fail if without quotes
            pak.Send(this);
        }
        public void SendMessageAnnouncement(string message)
        {
            Packet pak = new Packet();
            pak.Addint32(PAK_SEND_NOTICE);
            pak.Addstring(message);
            pak.Send(this);
        }
        public void SendMessageInfoNotice(string message, params object[] args) // By Nicco
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_FLYFF_INFORMATION);
            pak.Addint32(FlyFF.TID_ADMIN_ANNOUNCE);
            pak.Addstring(String.Format("\"{0}\"", String.Format(message, args)));
            pak.Send(this);
        }
        public void SendMessageUserOffline(string otherPlayerName)
        {
            Packet pak = new Packet();
            pak.Addint32(PAK_OFFLINE_BOX);
            pak.Addstring(otherPlayerName);
            pak.Send(this);
        }
    }
}

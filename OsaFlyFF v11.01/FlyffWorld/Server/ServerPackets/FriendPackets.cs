using System;
using System.Collections.Generic;
using System.Text;

namespace FlyffWorld
{
    public partial class Client
    {
        public void SendFriendDelete(int id)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_FRIEND_DELETE);
            pak.Addint(id);
            pak.Send(this);
        }
        public void SendFriendData(Friend friend)
        {
            Packet pak = new Packet();
            pak.Addint(PAK_FRIEND_USERDATA);
            pak.Addint(1);
            pak.Addint(NULL);
            pak.Addint(friend.dwCharacterID);
            pak.Addint(friend.dwNetworkStatus);
            pak.Addint(4);
            pak.Send(this);
        }
        public void SendFriendNew(int id, string name)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_FRIEND_ADDNEW);
            pak.Addint(id);
            pak.Addint(NULL);
            pak.Addstring(name);
            pak.Send(this);
        }
        public void SendFriendOnConnect()
        {
            Packet pak = new Packet();
            pak.Addint(PAK_FRIEND_LOGIN);
            pak.Addint(c_data.dwCharacterID);
            pak.Addint(c_data.dwNetworkStatus);
            pak.Addint(Server.worldid);
            SendToFriends(pak);
        }
        public void SendFriendOnDisconnect()
        {
            Packet pak = new Packet();
            pak.Addint(PAK_FRIEND_DISCONNECT);
            pak.Addint(c_data.dwCharacterID);
            SendToFriends(pak);
        }
        public void SendFriendOnDisconnect(Client c)
        {
            Packet pak = new Packet();
            pak.Addint(PAK_FRIEND_DISCONNECT);
            pak.Addint(c_data.dwCharacterID);
            pak.Send(c);
        }
        public void SendFriendNewStatus()
        {
            Packet pak = new Packet();
            pak.Addint(PAK_FRIEND_STATUSCHANGE);
            pak.Addint(c_data.dwCharacterID);
            pak.Addint(c_data.dwNetworkStatus);
            pak.Send(this);
            SendToFriends(pak);
        }
        public void SendFriendInvitation(Client client)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(client.dwMoverID, PAK_FRIEND_INVITATION);
            pak.Addint(c_data.dwCharacterID);
            pak.Addhex("00 0C 00 00 00");
            pak.Addstring(c_data.strPlayerName);
            pak.Send(client);
        }
        public void SendFriendRefuse(Client dst)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_FRIEND_CANCELINV);
            pak.Send(dst);
        }
        public void SendToFriends(Packet pak)
        {
            for (int i = 0; i < c_data.friends.Count; i++)
            {
                if (c_data.friends[i].nBlocked == 1)
                    continue;
                Client c = WorldHelper.GetClientByPlayerID(c_data.friends[i].dwCharacterID);
                if (c != null)
                    pak.Send(c);
            }
        }
        public void SendFriendUnblocked(Client other)
        {
            Packet pak = new Packet();
            pak.Addint(PAK_FRIEND_UNBLOCK);
            pak.Addint(other.c_data.dwCharacterID);
            pak.Addint(other.c_data.dwNetworkStatus);
            pak.Send(this);
        }
        public void SendFriendBlocked(Client other)
        {
            Packet pak = new Packet();
            pak.Addint(PAK_FRIEND_BLOCK);
            pak.Addint(c_data.dwCharacterID);
            pak.Send(other);
            pak.Send(this);
        }
    }
}

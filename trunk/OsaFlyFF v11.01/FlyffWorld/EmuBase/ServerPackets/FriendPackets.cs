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
            pak.Addint32(id);
            pak.Send(this);
        }
        public void SendFriendDataAll()
        {
            Packet pak = new Packet();
            pak.Addint32(PAK_FRIEND_USERDATA);
            pak.Addint32(c_data.friends.Count);
            pak.Addint32(0);
            for (int i = 0; i < c_data.friends.Count; i++)
            {
                pak.Addint32(c_data.friends[i].dwCharacterID);
                pak.Addint32(c_data.friends[i].dwNetworkStatus);
                if (c_data.friends[i].bOnline)
                    pak.Addint32(Server.worldid);//not worl but channel !
                else
                    pak.Addint32(100);
            }
            pak.Send(this);
        }
        public void SendFriendData(Friend friend)
        {
            Packet pak = new Packet();
            pak.Addint32(PAK_FRIEND_USERDATA);
            pak.Addint32(1);
            pak.Addint32(1);
            pak.Addint32(friend.dwCharacterID);
            pak.Addint32(friend.dwNetworkStatus);
            pak.Addint32(1);
            pak.Send(this);
        }
        public void SendFriendNew(int id, string name)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_FRIEND_ADDNEW);
            pak.Addint32(id);
            pak.Addint32(NULL);
            pak.Addstring(name);
            pak.Send(this);
        }
        public void SendFriendOnConnect()
        {
            Packet pak = new Packet();
            pak.Addint32(PAK_FRIEND_LOGIN);
            pak.Addint32(c_data.dwCharacterID);
            pak.Addint32(c_data.dwNetworkStatus);
            pak.Addint32(Server.worldid);
            SendToFriends(pak);
        }
        public void SendFriendOnDisconnect()
        {
            Packet pak = new Packet();
            pak.Addint32(PAK_FRIEND_DISCONNECT);
            pak.Addint32(c_data.dwCharacterID);
            SendToFriends(pak);
        }
        public void SendFriendOnDisconnect(Client c)
        {
            Packet pak = new Packet();
            pak.Addint32(PAK_FRIEND_DISCONNECT);
            pak.Addint32(c_data.dwCharacterID);
            pak.Send(c);
        }
        public void SendFriendNewStatus()
        {
            Packet pak = new Packet();
            pak.Addint32(PAK_FRIEND_STATUSCHANGE);
            pak.Addint32(c_data.dwCharacterID);
            pak.Addint32(c_data.dwNetworkStatus);
            pak.Send(this);
            SendToFriends(pak);
        }
        /// <summary>
        /// Sends a friend invitation from Client "this" to Client "client" (the parameter). Call it from the requesting character!
        /// </summary>
        /// <param name="client">The client --receiving-- the invitation.</param>
        public void SendFriendInvitation(Client client)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(client.dwMoverID, PAK_FRIEND_INVITATION);
            pak.Addint32(c_data.dwCharacterID);
            pak.Addbyte(0); // CONST 
            pak.Addint32(c_data.dwClass);
            pak.Addstring(c_data.strPlayerName);
            pak.Send(client);
        }
        public void SendFriendRefuse()
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_FRIEND_CANCELINV);
            pak.Send(this);
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
            pak.Addint32(PAK_FRIEND_UNBLOCK);
            pak.Addint32(other.c_data.dwCharacterID);
            pak.Addint32(other.c_data.dwNetworkStatus);
            pak.Send(this);
        }
        public void SendFriendBlocked(Client other)
        {
            Packet pak = new Packet();
            pak.Addint32(PAK_FRIEND_BLOCK);
            pak.Addint32(c_data.dwCharacterID);
            pak.Addint32(other.c_data.dwCharacterID);
            pak.Send(other);
            pak.Send(this);
        }
        public void SendFriendBlocked(int dwTargetID)
        {
            Packet pak = new Packet();
            pak.Addint32(PAK_FRIEND_BLOCK);
            pak.Addint32(c_data.dwCharacterID);
            pak.Addint32(dwTargetID);
            pak.Send(this);
        }
        public void SendFriendUnblocked(int dwTargetID)
        {
            Packet pak = new Packet();
            pak.Addint32(PAK_FRIEND_UNBLOCK);
            pak.Addint32(dwTargetID);
            pak.Addint32((int)NetworkStatus.Offline);
            pak.Send(this);
        }
    }
}

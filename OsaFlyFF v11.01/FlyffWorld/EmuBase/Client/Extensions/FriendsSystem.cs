using System;
using System.Collections.Generic;
using System.Text;

namespace FlyffWorld
{
    public class FriendsSystem
    {
        public static void Request(Client src, Client dst)
        {
            if (ContainsFriend(src, dst))
            {
                Log.Write(Log.MessageType.warning, "FriendsSystem::Request(): @ ContainsFriend(src, dst) [true]");
                return;
            }
            if (src.c_data.dwCharacterID == dst.c_data.dwCharacterID)
            {
                Log.Write(Log.MessageType.warning, "FriendsSystem::Request(): @ dwCharacterID conflict");
                return;
            }
            src.SendFriendInvitation(dst);
        }
        public static void Delete(Client src, Client dst)
        {
            bool bSrcDeleted = false, bDstDeleted = false;
            for (int i = 0; i < src.c_data.friends.Count; i++)
                if (src.c_data.friends[i].dwCharacterID == dst.c_data.dwCharacterID)
                {
                    src.c_data.friends.RemoveAt(i);
                    bSrcDeleted = true;
                    break;
                }
            for (int i = 0; i < dst.c_data.friends.Count; i++)
                if (dst.c_data.friends[i].dwCharacterID == src.c_data.dwCharacterID)
                {
                    dst.c_data.friends.RemoveAt(i);
                    bDstDeleted = true;
                    break;
                }
            if (bDstDeleted)
                dst.SendFriendDelete(src.c_data.dwCharacterID);
            else
                Log.Write(Log.MessageType.warning, "FriendsSystem::Delete(): @ !bDstDeleted");
            if (bSrcDeleted)
                src.SendFriendDelete(dst.c_data.dwCharacterID);
            else
                Log.Write(Log.MessageType.warning, "FriendsSystem::Delete(Client, Client): @ !bSrcDeleted");
        }
        public static void Delete(Client src, int dwID)
        {
            bool bSrcDeleted = false;
            for (int i = 0; i < src.c_data.friends.Count; i++)
                if (src.c_data.friends[i].dwCharacterID == dwID)
                {
                    src.c_data.friends.RemoveAt(i);
                    bSrcDeleted = true;
                    break;
                }
            if (!bSrcDeleted)
                Log.Write(Log.MessageType.warning, "FriendsSystem::Delete(Client, Int32): @ !bSrcDeleted");
            else
                src.SendFriendDelete(dwID);
            src.SaveFriends();
        }
        public static void Refuse(Client src, Client dst)
        {
            src.SendFriendRefuse();
        }
        public static void Accept(Client src, Client dst)
        {
            // src: sender.
            // dst: receiver.
            src.SendMessageInfo(FlyFF.TID_GAME_MSGINVATECOM, dst.c_data.strPlayerName);
            dst.SendMessageInfo(FlyFF.TID_GAME_MSGINVATECOM, src.c_data.strPlayerName);
            src.SendFriendNew(dst.c_data.dwCharacterID, dst.c_data.strPlayerName);
            dst.SendFriendNew(src.c_data.dwCharacterID, src.c_data.strPlayerName);
            Friend fSrc, fDst;
            src.c_data.friends.Add(fDst = new Friend()
                {
                    bOnline = true,
                    dwCharacterID = dst.c_data.dwCharacterID,
                    dwClass = dst.c_data.dwClass,
                    dwGender = dst.c_data.dwGender,
                    dwLevel = dst.c_data.dwLevel,
                    dwNetworkStatus = dst.c_data.dwNetworkStatus,
                    nBlocked = 0,
                    strPlayerName = dst.c_data.strPlayerName
                }
            );
            dst.c_data.friends.Add(fSrc = new Friend()
                {
                    bOnline = true,
                    dwCharacterID = src.c_data.dwCharacterID,
                    dwClass = src.c_data.dwClass,
                    dwGender = src.c_data.dwGender,
                    dwLevel = src.c_data.dwLevel,
                    dwNetworkStatus = src.c_data.dwNetworkStatus,
                    nBlocked = 0,
                    strPlayerName = src.c_data.strPlayerName
                }
            );
            src.SendFriendData(fDst);
            dst.SendFriendData(fSrc);
        }
        public static bool ContainsFriend(Client src, Client dst)
        {
            for (int i = 0; i < src.c_data.friends.Count; i++)
                if (dst.c_data.friends[i].dwCharacterID == dst.c_data.dwCharacterID)
                    return true;
            return false;
        }
    }
}

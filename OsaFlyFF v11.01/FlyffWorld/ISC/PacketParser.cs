using System;
using System.Collections;

using System.Text;

namespace FlyffWorld
{
    public partial class ISCRemoteServer
    {
        public static void Authentication()
        {
            SendAuthentication();
        }
        public static void AuthenticationResult(DataPacket dp)
        {
            if (dp.Readint32() == 1)
            {
                Log.Write(Log.MessageType.notice, "ISC authentication complete.");
            }
            else
            {
                Log.Write(Log.MessageType.fatal, "ISC authentication failed!");
            }
        }
        public static void KickFromServers(DataPacket dp)
        {
            int dwAccountID = dp.Readint32();
            for (int i = 0; i < WorldServer.world_players.Count; i++)
            {
                if (WorldServer.world_players[i].c_data.dwAccountID == dwAccountID)
                {
                    WorldServer.world_players[i].Destruct("Kicked due to ISC request");
                    break;
                }
            }
        }
    }
}

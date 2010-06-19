using System;
using System.Collections.Generic;

using System.Text;

namespace FlyffCluster
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
            for (int i = 0; i < ClientManager.c_clients.Count; i++)
            {
                if (ClientManager.c_clients[i].dwMyUserID == dwAccountID)
                {
                    ClientManager.c_clients[i].Destruct("Kicked due to ISC request");
                    break;
                }
            }
        }
        public static void UpdateWorldTable(DataPacket dp)
        {
            Server.servers.Clear();
            int dwMyClusterID = dp.Readint32();
            if (dwMyClusterID != Server.clusterid)
            {
                Log.Write(Log.MessageType.warning, "ISC::UpdateWorldTable(): bad cluster ID from packet o__o");
            }
            int dwWorlds = dp.Readint32();
            for (int i = 0; i < dwWorlds; i++)
            {
                WorldInfo world = new WorldInfo();
                world.dwWorldID = dp.Readint32();
                world.strWorldIP = dp.Readstring();
                Server.servers.Add(world);
            }
        }
    }
}

using System;
using System.Collections.Generic;

using System.Text;

namespace FlyffLogin
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
        public static void UpdateServerList(DataPacket dp)
        {
            int dwClusters = dp.Readint32();
            Server.serverlist.Clear();
            for (int i = 0; i < dwClusters; i++)
            {
                ClusterInfo cluster = new ClusterInfo();
                cluster.dwClusterID = dp.Readint32();
                cluster.strClusterName = dp.Readstring();
                cluster.strClusterIP = Server.ToIP(dp.Readstring());
                int dwWorlds = dp.Readint32();
                for (int l = 0; l < dwWorlds; l++)
                {
                    WorldInfo world = new WorldInfo();
                    world.dwClusterID = cluster.dwClusterID;
                    world.dwWorldID = dp.Readint32();
                    world.dwCapacity = dp.Readint32();
                    world.strWorldName = dp.Readstring();
                    cluster.worlds.Add(world);
                }
                Server.serverlist.Add(cluster);

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
    }
}

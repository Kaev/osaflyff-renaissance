using System;
using System.Collections.Generic;
using System.Text;
using Shared;
namespace ISC
{
    public partial class ClientHandler
    {
        public static void KickFromServers(int dwAccountID)
        {
            Packet pak = new Packet();
            pak.Addint32((int)ISCShared.Commands.KickFromServers);
            pak.Addint32(dwAccountID);
            pak.SendTo(clientList);
        }
        public static void KickFromGuild(int dWClusterID, int dwCharacterID)
        {
            Packet pak = new Packet();
            pak.Addint32((int)ISCShared.Commands.KickFromGuild);
            pak.Addint32(dwCharacterID);
            pak.SendTo(GetWorldsByClusterID(dWClusterID));
            pak.Addint32(dWClusterID);
            pak.SendTo(GetInfoServers());
        }
        public static void SendNotice(string strNotice)
        {
            Packet pak = new Packet();
            pak.Addint32((int)ISCShared.Commands.SendNotice);
            pak.Addstring(strNotice);
            pak.SendTo(GetWorlds());
        }
        public static void SendNotice(string strNotice, int dwClusterID)
        {
            Packet pak = new Packet();
            pak.Addint32((int)ISCShared.Commands.SendNotice);
            pak.Addstring(strNotice);
            pak.SendTo(GetWorldsByClusterID(dwClusterID));
            pak.Addint32(dwClusterID);
            pak.SendTo(GetInfoServers());
        }
        public static void UpdateServerList()
        {
            Packet pak = new Packet();
            List<Client> clusters = GetClusters();
            pak.Addint32((int)ISCShared.Commands.UpdateServerList);
            pak.Addint32(clusters.Count);
            for (int i = 0; i < clusters.Count; i++)
            {
                ClusterServer cluster = clusters[i].serverinfo as ClusterServer;
                pak.Addint32(cluster.dwClusterID);
                pak.Addstring(cluster.strServerName);
                pak.Addstring(cluster.strServerIP);
                List<Client> worlds = GetWorldsByClusterID(cluster.dwClusterID);
                pak.Addint32(worlds.Count);
                for (int l = 0; l < worlds.Count; l++)
                {
                    WorldServer world = worlds[l].serverinfo as WorldServer;
                    pak.Addint32(world.dwWorldID);
                    pak.Addint32(world.dwCapacity);
                    pak.Addstring(world.strServerName);
                }
            }
            if (GetLogin() != null)
                pak.Send(GetLogin());
            Client loginClient = GetLogin();
            LoginServer login = null;
            if (loginClient != null)
                login = loginClient.serverinfo as LoginServer;
            if (login != null)
            {
                pak.Addstring(login.strServerIP);
            }
            pak.SendTo(GetInfoServers());
        }
        public static void UpdateWorldTable(int dwClusterID)
        {
            List<Client> worlds = GetWorldsByClusterID(dwClusterID);
            Packet pak = new Packet();
            pak.Addint32((int)ISCShared.Commands.UpdateWorldTable);
            pak.Addint32(dwClusterID);
            pak.Addint32(worlds.Count);
            for (int i = 0; i < worlds.Count; i++)
            {
                WorldServer world = worlds[i].serverinfo as WorldServer;
                pak.Addint32(world.dwWorldID);
                pak.Addstring(world.strServerIP);
            }
            if (GetClusterByID(dwClusterID) != null)
                pak.Send(GetClusterByID(dwClusterID));
            pak.SendTo(GetInfoServers());
        }
    }
}

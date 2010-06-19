using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
namespace ISC
{
    public partial class ClientHandler
    {
        public static List<Client> clientList = new List<Client>();
        
        [MTAThread]
        public static void RegisterSocket(Socket clientSocket)
        {
            Log.Write(Log.MessageType.info, "New client connected from {0}.", clientSocket.RemoteEndPoint.ToString());
            Client client = new Client(clientSocket);
            clientList.Add(client);
        }
        public static bool LoginExists()
        {
            for (int i = 0; i < clientList.Count; i++)
            {
                if (clientList[i].serverinfo is LoginServer)
                    return true;
            }
            return false;
        }
        public static bool ClusterExists(int dwClusterID)
        {
            for (int i = 0; i < clientList.Count; i++)
            {
                if (clientList[i].serverinfo is ClusterServer && (clientList[i].serverinfo as ClusterServer).dwClusterID == dwClusterID)
                    return true;
            }
            return false;
        }
        public static bool WorldExists(int dwClusterID, int dwWorldID)
        {
            for (int i = 0; i < clientList.Count; i++)
            {
                ServerBase s = clientList[i].serverinfo;
                if (s is WorldServer && (s as WorldServer).dwClusterID == dwClusterID && (s as WorldServer).dwWorldID == dwWorldID)
                    return true;
            }
            return false;
        }
        public static Client GetWorldByIDs(int dwClusterID, int dwWorldID)
        {
            for (int i = 0; i < clientList.Count; i++)
            {
                if (clientList[i].serverinfo is WorldServer)
                {
                    WorldServer s = clientList[i].serverinfo as WorldServer;
                    if (s.dwClusterID == dwClusterID && s.dwWorldID == dwWorldID)
                        return clientList[i];
                }
            }
            return null;
        }
        public static Client GetClusterByID(int dwClusterID)
        {
            for (int i = 0; i < clientList.Count; i++)
            {
                if (clientList[i].serverinfo is ClusterServer)
                {
                    ClusterServer s = clientList[i].serverinfo as ClusterServer;
                    if (s.dwClusterID == dwClusterID)
                        return clientList[i];
                }
            }
            return null;
        }
        public static List<Client> GetWorldsByClusterID(int dwClusterID)
        {
            List<Client> list = new List<Client>();
            for (int i = 0; i < clientList.Count; i++)
            {
                ServerBase s = clientList[i].serverinfo;
                if (s is WorldServer && (s as WorldServer).dwClusterID == dwClusterID)
                    list.Add(clientList[i]);
            }
            return list;
        }
        public static List<Client> GetWorlds()
        {
            List<Client> list = new List<Client>();
            for (int i = 0; i < clientList.Count; i++)
            {
                ServerBase s = clientList[i].serverinfo;
                if (s is WorldServer)
                    list.Add(clientList[i]);
            }
            return list;
        }
        public static List<Client> GetClusters()
        {
            List<Client> list = new List<Client>();
            for (int i = 0; i < clientList.Count; i++)
            {
                ServerBase s = clientList[i].serverinfo;
                if (s is ClusterServer)
                    list.Add(clientList[i]);
            }
            return list;
        }
        public static Client GetLogin()
        {
            for (int i = 0; i < clientList.Count; i++)
                if (clientList[i].serverinfo is LoginServer)
                    return clientList[i];
            return null;
        }
        public static List<Client> GetInfoServers()
        {
            List<Client> list = new List<Client>();
            for (int i = 0; i < clientList.Count; i++)
                if (clientList[i].serverinfo is InfoServer)
                    list.Add(clientList[i]);
            return list;
        }
        public static void UpdateLists()
        {
            ClientHandler.UpdateServerList();
            List<Client> list = ClientHandler.GetClusters();
            for (int i = 0; i < list.Count; i++)
                ClientHandler.UpdateWorldTable((list[i].serverinfo as ClusterServer).dwClusterID);
        }
    }
}
using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Shared;
using System.Collections.Generic;
namespace ISC
{
    public class Client
    {
        public ServerBase serverinfo = new ServerBase() { strServerName = "[unverified]" };
        #region Nothing much
        public Socket ClientSocket;
        public Client(Socket s)
        {
            ClientSocket = s;
            ClientSocket.NoDelay = false; // damn nagle!
            Authentication();
            dataHandling();
        }
        public void Destruct(string reason)
        {
            Log.Write(Log.MessageType.info, "{0} disconnected: {1}", MyID, reason);
            ClientHandler.clientList.Remove(this);
            ClientHandler.UpdateLists();
            ClientSocket.Disconnect(false);
            ClientSocket.Close();
        }
        public string MyID
        {
            get
            {
                string myID = "\"" + serverinfo.strServerName + "\"";
                if (serverinfo is ClusterServer)
                {
                    myID += "[" + (serverinfo as ClusterServer).dwClusterID + "]";
                }
                else if (serverinfo is WorldServer)
                {
                    myID += "[" + (serverinfo as WorldServer).dwClusterID + "]";
                    myID += "[" + (serverinfo as WorldServer).dwWorldID + "]";
                }
                return myID;
            }
        }
        public AsyncCallback callback;
        public void dataHandling()
        {
            try
            {
                if (callback == null)
                    callback = new AsyncCallback(parseAsyncResult);
                DataPacket dp = new DataPacket();
                ClientSocket.BeginReceive(dp.buffer, 0, 1000, SocketFlags.None, callback, dp);
            }
            catch (Exception e)
            {
                Log.Write(Log.MessageType.error, "Error getting data from client: " + e.Message);
            }
        }
        public void parseAsyncResult(IAsyncResult res)
        {
            DataPacket head_dp = (DataPacket)res.AsyncState;
            try
            {
                if (ClientSocket.Connected)
                {
                    int head_size = ClientSocket.EndReceive(res);
                    if (head_size < 5)
                    {
                        Destruct("Invalid packet structure");
                        return;
                    }
                    DataPacket[] dps = DataPacket.SplitNaglePackets(head_dp);
                    for (int i = 0; i < dps.Length; i++)
                    {
                        DataPacket dp = dps[i];
                        dp.dwPointer = 0;
                        parseIncomingPackets(dp);
                    }
                    if (ClientSocket.Connected)
                        dataHandling();
                }
                else
                {
                    Destruct("Client not connected");
                }
            }
            catch (Exception e)
            {
                Destruct("(exception) " + e.Message + "\r\n" + e.StackTrace);
            }
        }
        #endregion
        #region Incoming packets

        public void parseIncomingPackets(DataPacket dp)
        {
            dp.dwPointer = 5;
            int dwHeader = dp.Readint32();
            ISCShared.Commands command = (ISCShared.Commands)dwHeader;
            switch (command)
            {
                case ISCShared.Commands.Authentication:
                    ReadAuthentication(dp);
                    break;
                case ISCShared.Commands.KickFromServers:
                    ReadKickFromServers(dp);
                    break;
            }
        }

        public void ReadKickFromServers(DataPacket dp)
        {
            int dwAccountID = dp.Readint32();
            Packet pak = new Packet();
            pak.Addint32((int)Shared.ISCShared.Commands.KickFromServers);
            pak.Addint32(dwAccountID);
            pak.SendTo(ClientHandler.clientList);
        }

        public void ReadAuthentication(DataPacket dp)
        {
            string strPassword = dp.Readstring();
            if (strPassword != Server.inter_password)
            {
                AuthenticationResult(false);
                Destruct("Authentication failed (wrong password)");
                return;
            }
            string strServerName = dp.Readstring();
            int nServerType = dp.Readint32();
            switch ((ISCShared.ServerType)nServerType)
            {
                case ISCShared.ServerType.Login:
                    {
                        if (ClientHandler.LoginExists())
                        {
                            AuthenticationResult(false);
                            Destruct("Authentication failed (login server already exists)");
                            return;
                        }
                        string strServerIP = dp.Readstring();
                        serverinfo = new LoginServer() { strServerIP = strServerIP, strServerName = strServerName };
                        break;
                    }
                case ISCShared.ServerType.World:
                    {
                        if (!ClientHandler.LoginExists())
                        {
                            AuthenticationResult(false);
                            Destruct("Authentication failed (login server does not exist)");
                            return;
                        }
                        int dwClusterID = dp.Readint32();
                        if (!ClientHandler.ClusterExists(dwClusterID))
                        {
                            AuthenticationResult(false);
                            Destruct("Authentication failed (cluster server does not exist)");
                            return;
                        }
                        int dwWorldID = dp.Readint32();
                        if (ClientHandler.WorldExists(dwClusterID, dwWorldID))
                        {
                            AuthenticationResult(false);
                            Destruct("Authentication failed (duplicate world ID in the same cluster)");
                            return;
                        }
                        int dwCapacity = dp.Readint32();
                        string strServerIP = dp.Readstring();
                        serverinfo = new WorldServer() { dwClusterID = dwClusterID, dwWorldID = dwWorldID, strServerIP = strServerIP, strServerName = strServerName, dwCapacity = dwCapacity };
                        break;
                    }
                case ISCShared.ServerType.Cluster:
                    {
                        if (!ClientHandler.LoginExists())
                        {
                            AuthenticationResult(false);
                            Destruct("Authentication failed (login server does not exist)");
                            return;
                        }
                        int dwClusterID = dp.Readint32();
                        if (ClientHandler.ClusterExists(dwClusterID))
                        {
                            AuthenticationResult(false);
                            Destruct("Authentication failed (duplicate cluster ID)");
                            return;
                        }
                        string strServerIP = dp.Readstring();
                        serverinfo = new ClusterServer() { dwClusterID = dwClusterID, strServerIP = strServerIP, strServerName = strServerName };
                        break;
                    }
                case ISCShared.ServerType.PHPConnector:
                    {
                        serverinfo = new InfoServer() { strServerName = "InfoServer" };
                        break;
                    }
                default:
                    Destruct("Unknown type: " + nServerType);
                    return;

            }
            Log.Write(Log.MessageType.notice, "{0}: Server authenticated.", MyID);
            AuthenticationResult(true);
            ClientHandler.UpdateLists();
        }

        #endregion

        #region Outgoing packets

        public void Authentication()
        {
            Packet pak = new Packet();
            pak.Addint32((int)ISCShared.Commands.Authentication);
            pak.Send(this);
        }

        public void AuthenticationResult(bool bSucceeded)
        {
            Packet pak = new Packet();
            pak.Addint32((int)ISCShared.Commands.AuthenticationResult);
            pak.Addint32(bSucceeded ? 1 : 0);
            pak.Send(this);
        }
        

        #endregion
    }
}

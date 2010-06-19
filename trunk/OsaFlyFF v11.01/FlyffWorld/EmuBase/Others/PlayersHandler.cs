using System;
using System.Collections.Generic;

using System.Text;
using System.Collections;
using System.Net.Sockets;
using System.Threading;

namespace FlyffWorld
{
    public class PlayersHandler
    {
        [MTAThread]
        public static void newClient(Socket clientSock)
        {
            Client client = new Client(clientSock);
            Log.Write(Log.MessageType.info, "New client connected from {0}. dwMySessionKey: {1}", clientSock.RemoteEndPoint.ToString(), client.dwMySessionKey);
            WorldServer.world_players.Add(client);
        }
    }
}

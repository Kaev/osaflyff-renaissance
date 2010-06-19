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
            Log.Write(Log.MessageType.info, "New client connected from {0}.", clientSock.RemoteEndPoint.ToString());
            Client client = new Client(clientSock);
            WorldServer.world_players.Add(client);
        }
    }
}

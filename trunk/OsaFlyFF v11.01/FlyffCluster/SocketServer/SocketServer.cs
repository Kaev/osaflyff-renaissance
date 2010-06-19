using System;
using System.Net;
using System.Net.Sockets;

namespace FlyffCluster
{
    public class SocketServer
    {
        public static Socket c_server;
        public static void Start()
        {
            try
            {
                c_server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                c_server.Bind(new IPEndPoint(IPAddress.Any, 28000));
                c_server.Listen(10);
                Log.Write(Log.MessageType.info, "Listening on port 28000.");
                while (true)
                {
                    ClientManager.c_clients.Add(new Client(c_server.Accept()));
                }
            }
            catch (Exception e)
            {
                Log.Write(Log.MessageType.fatal, "SocketServer::Start(): {0}", e.Message);
                Environment.Exit(10241);
            }
        }
    }
}

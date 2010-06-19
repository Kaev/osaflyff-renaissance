using System;
using System.Net;
using System.Net.Sockets;

namespace ISC
{
    public class SocketServer
    {
        public static System.Threading.ManualResetEvent mreServer = new System.Threading.ManualResetEvent(true);
        public static Socket ListenSocket;
        private static object syncRoot = new object();
        public static void Start()
        {
            try
            {
                ListenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                ListenSocket.Bind(new IPEndPoint(IPAddress.Any, 17000));
                ListenSocket.Listen(50);
                Log.Write(Log.MessageType.info, "Listening on port 17000.");
                while (true)
                {
                    mreServer.Reset();
                    ListenSocket.BeginAccept(new AsyncCallback(AddClient), null);
                    mreServer.WaitOne();
                }
            }
            catch (Exception e)
            {
                Log.Write(Log.MessageType.fatal, e.Message);
                Environment.Exit(1);
            }
        }
        public static void AddClient(IAsyncResult callback)
        {
            mreServer.Set();
            lock (syncRoot)
            {
                Socket clientSocket = ListenSocket.EndAccept(callback);
                try
                {
                    ClientHandler.RegisterSocket(clientSocket);
                }
                catch (Exception e)
                {
                    Log.Write(Log.MessageType.error, "Error handling connected client: " + e.Message);
                    clientSocket.Disconnect(false);
                    clientSocket.Close();
                }
            }
            ListenSocket.BeginAccept(new AsyncCallback(AddClient), null);
        }
    }
}

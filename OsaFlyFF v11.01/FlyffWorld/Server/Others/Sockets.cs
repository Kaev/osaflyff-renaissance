using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace FlyffWorld
{
    public class Sockets
    {
        private Socket listenSock;
        private Lock syncRoot = new Lock();
        public ManualResetEvent mre;
        public Sockets()
        {
            mre = new ManualResetEvent(true);
        }
        public void startServer()
        {
            // load data
            listenSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint epConfiguration = new IPEndPoint(IPAddress.Any, 15400);
            try
            {
                listenSock.Bind(epConfiguration);
                listenSock.Listen(4);
                Log.Write(Log.MessageType.info, "Listening on port 15400.");
                while (true)
                {
                    mre.Reset();
                    listenSock.BeginAccept(new AsyncCallback(HandleConnectedClient), null);
                    mre.WaitOne();
                }
            }
            catch (SocketException se)
            {
                Log.Write(Log.MessageType.fatal, se.Message);
                Environment.Exit(1);
            }
        }

        public void HandleConnectedClient(IAsyncResult asyn)
        {
            mre.Set();
            lock (syncRoot)
            {
                Socket thissocket = listenSock.EndAccept(asyn);
                try
                {
                    PlayersHandler.newClient(thissocket);
                }
                catch (Exception e)
                {
                    Log.Write(Log.MessageType.error, "Error handling connected client: " + e.Message + "\r\n" + e.StackTrace);
                    thissocket.Disconnect(false);
                    thissocket.Close();
                }
            }
            listenSock.BeginAccept(new AsyncCallback(HandleConnectedClient), null);
        }
    }
}

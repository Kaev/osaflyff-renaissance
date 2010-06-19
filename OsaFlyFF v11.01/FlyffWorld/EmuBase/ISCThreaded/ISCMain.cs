using System;
using System.Collections;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
namespace FlyffWorld
{
    class ISCServer
    {
        #region Variables
        Socket iscSock;
        Thread iscThread;
        public ISCHandler iscHandler;
        public Client cClone = new Client(null);
        #endregion

        public ISCServer()
        {
            ISCConfig.Refresh();
            try
            {
                iscThread = new Thread(new ThreadStart(Receiver));
                iscSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                iscSock.Connect(ISCConfig.server_host, 17000);
                cClone.sock = iscSock;
                iscHandler = new ISCHandler(this);
                Log.Write(Log.MessageType.info, "Successfully connected to ISC server.");
                iscThread.Start();
                Log.Write(Log.MessageType.info, "ISC thread started.");
            }
            catch (Exception e)
            {
                Log.Write(Log.MessageType.fatal, e.Message);
                Environment.Exit(5);
            }
        }
        public void Receiver() {
            while (true)
            {
                ISCDataPacket dp = new ISCDataPacket();
                try
                {
                    if ((dp.size = iscSock.Receive(dp.buffer)) > 5)
                        iscHandler.Parse(dp);
                    else
                    {
                        if (dp.size == 0)
                        {
                            Log.Write(Log.MessageType.fatal, "Disconnected from ISC.");
                            Environment.Exit(7);
                        }
                        else
                            Log.Write(Log.MessageType.notice, "Bad packet received from ISC.");
                    }
                }
                catch (Exception)
                {
                    if (iscSock.Connected)
                        Log.Write(Log.MessageType.error, "Failed to receive packet from ISC server!");
                    else
                    {
                        Log.Write(Log.MessageType.fatal, "Disconnected from ISC.");
                        Environment.Exit(6);
                    }
                }
            }
        }
    }
}

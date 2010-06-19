using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace FlyffWorld
{
    public partial class ISCRemoteServer
    {
        static Socket socket;
        public static void Connect()
        {
            try
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(Server.server_host, 17000);
                while (true)
                {
                    if (socket.Connected)
                        ReceiveData();
                    else
                    {
                        Log.Write(Log.MessageType.fatal, "Disconnected from ISC!");
                        Environment.Exit(101);
                    }
                }
            }
            catch (Exception e)
            {
                if (!socket.Connected)
                    Log.Write(Log.MessageType.fatal, "Disconnected from ISC!");
                else
                    Log.Write(Log.MessageType.fatal, e.Message);
                Environment.Exit(101);
            }
        }
        public static void ReceiveData()
        {
            DataPacket dp = new DataPacket();
            byte[] buf = new byte[1452];
            dp.dwRecvSize = socket.Receive(buf);
            dp.buffer = buf;
            ParseData(dp);
        }
        public static void ParseData(DataPacket dp)
        {
            if (dp.dwRecvSize < 5)
            {
                socket.Disconnect(false);
                socket.Close();
                Log.Write(Log.MessageType.fatal, "Disconnected from ISC!");
                Environment.Exit(102);
            }
            dp.dwPointer = 5;
            int header = dp.Readint32();
            switch ((Shared.ISCShared.Commands)header)
            {
                case Shared.ISCShared.Commands.Authentication:
                    Authentication();
                    break;
                case Shared.ISCShared.Commands.AuthenticationResult:
                    AuthenticationResult(dp);
                    break;
                case Shared.ISCShared.Commands.KickFromServers:
                    KickFromServers(dp);
                    break;
            }
        }
    }
}

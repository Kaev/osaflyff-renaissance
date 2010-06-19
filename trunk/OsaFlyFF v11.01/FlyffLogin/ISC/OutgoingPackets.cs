using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlyffLogin
{
    public partial class ISCRemoteServer
    {
        public static void SendAuthentication()
        {
            Packet pak = new Packet();
            pak.Addint32((int)Shared.ISCShared.Commands.Authentication);
            pak.Addstring(Server.inter_password);
            pak.Addstring("LoginServer");
            pak.Addint32((int)Shared.ISCShared.ServerType.Login);
            pak.Addstring(Server.serverip);
            pak.Send(socket);
        }
        public static void SendKickFromServers(int dwAccountID)
        {
            Packet pak = new Packet();
            pak.Addint32((int)Shared.ISCShared.Commands.KickFromServers);
            pak.Addint32(dwAccountID);
            pak.Send(socket);
        }
    }
}

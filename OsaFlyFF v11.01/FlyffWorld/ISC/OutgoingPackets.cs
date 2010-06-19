using System;
using System.Collections.Generic;
using System.Text;

namespace FlyffWorld
{
    public partial class ISCRemoteServer
    {
        public static void SendAuthentication()
        {
            Packet pak = new Packet();
            pak.Addint32((int)Shared.ISCShared.Commands.Authentication);
            // password, name, type
            pak.Addstring(Server.inter_password);
            pak.Addstring(Server.worldname);
            pak.Addint32((int)Shared.ISCShared.ServerType.World);
            pak.Addint32(Server.clusterid);
            pak.Addint32(Server.worldid);
            pak.Addint32(Server.max_connections);
            pak.Addstring(Server.strMyIP);
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

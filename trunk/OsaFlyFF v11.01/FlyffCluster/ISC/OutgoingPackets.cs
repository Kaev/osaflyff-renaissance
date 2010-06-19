using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlyffCluster
{
    public partial class ISCRemoteServer
    {
        public static void SendAuthentication()
        {
            Packet pak = new Packet();
            pak.Addint32((int)Shared.ISCShared.Commands.Authentication);
            pak.Addstring(Server.inter_password);
            pak.Addstring(Server.clustername);
            pak.Addint32((int)Shared.ISCShared.ServerType.Cluster);
            pak.Addint32(Server.clusterid);
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

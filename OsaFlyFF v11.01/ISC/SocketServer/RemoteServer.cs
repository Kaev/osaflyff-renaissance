using System;
using System.Collections.Generic;

using System.Text;

namespace ISC
{
    public class RemoteServer
    {
        public string ServerName = "";
        public bool ServerAuthenticated = false;
    }
    public class ClusterServer : RemoteServer
    {
        public int ClusterID = -1;
    }
    public class WorldServer : ClusterServer
    {
        public int WorldID = -1;
    }
    public class LoginServer : RemoteServer
    {

    }
    public class FakeServer : RemoteServer
    {
        
    }
}

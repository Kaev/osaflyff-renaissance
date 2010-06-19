using System;
using System.Collections.Generic;
using System.Text;

namespace ISC
{
    public class ServerBase
    {
        public string strServerName = "";
        public string strServerIP = "";
    }
    public class LoginServer : ServerBase
    {

    }
    public class ClusterServer : ServerBase
    {
        public int dwClusterID = -1;
    }
    public class WorldServer : ServerBase
    {
        public int dwClusterID = -1;
        public int dwWorldID = -1;
        public int dwCapacity = -1;
    }
    public class InfoServer : ServerBase
    {

    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace FlyffWorld
{
    class ISCConfig
    {
        public static int minorVersion = 0,
                          majorVersion = 1;
        public static string server_host    = "127.0.0.1",
                             inter_password = "example";
        public static void Refresh()
        {
            IniFile isc = new IniFile("conf/isc.ini");
            majorVersion    = isc.ReadInt("version_major", 0);
            minorVersion    = isc.ReadInt("version_minor", 1);
            server_host     = isc.ReadValue("server_host", "127.0.0.1");
            inter_password  = isc.ReadValue("inter_password", "example");
            isc.destroy();
        }
    }
}

using System;
using System.Collections;
using System.Text;

namespace ISC
{
    class Server // instead of using the long name, this one is acceptable IMO
    {
        public static string
            myodbc_username,    // MyODBC username
            myodbc_password,    // MyODBC password
            myodbc_database,    // MyODBC database
            myodbc_driver,      // MyODBC driver
            myodbc_host;        // MyODBC host
        public static int minorVersion = 0,
                  majorVersion = 1;
        public static string server_host = "127.0.0.1",
                             inter_password = "example";
        public static string ToIP(string dns)
        {
            return System.Net.Dns.GetHostAddresses(dns)[0].ToString();
        }
        public static void Refresh()
        {
            IniFile myodbc = new IniFile("conf/myodbc.ini");
            Server.myodbc_database = myodbc.ReadValue("myodbc_database", "osaflyff");
            Server.myodbc_username = myodbc.ReadValue("myodbc_username", "root");
            Server.myodbc_password = myodbc.ReadValue("myodbc_password", "");
            Server.myodbc_driver   = myodbc.ReadValue("myodbc_driver"  , "MySQL ODBC 5.1 Driver");
            Server.myodbc_host     = ToIP(myodbc.ReadValue("myodbc_host"    , "localhost"));
            myodbc.destroy();
            IniFile isc = new IniFile("conf/isc.ini");
            majorVersion    = isc.ReadInt("version_major", 0);
            minorVersion    = isc.ReadInt("version_minor", 1);
            server_host     = ToIP(isc.ReadValue("server_host", "127.0.0.1"));
            inter_password  = isc.ReadValue("inter_password", "example");
            isc.destroy();
        }
    }
}

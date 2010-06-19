using System;
using System.Collections.Generic;
using System.Text;

namespace FlyffLogin
{
    class Server // instead of using the long name, this one is acceptable IMO
    {
        public static string
            client_builddate,   // Client build date
            client_hash,        // Client flyff.a hash
            myodbc_username,    // MyODBC username
            myodbc_password,    // MyODBC password
            myodbc_database,    // MyODBC database
            myodbc_driver,      // MyODBC driver
            myodbc_host,        // MyODBC host
            serverip;
        public static int
            max_connections;    // Max connections
        public static bool
            use_flyff_a;        // Use flyff.a file?
        public static int minorVersion = 0,
          majorVersion = 1;
        public static string server_host = "127.0.0.1",
                             inter_password = "example";
        public static List<ClusterInfo> serverlist = new List<ClusterInfo>();
        public static List<string> bans = new List<string>();
        public static string ToIP(string dns)
        {
            return System.Net.Dns.GetHostAddresses(dns)[0].ToString();
        }
        public static void Refresh()
        {
            Server.serverlist.Clear();
            IniFile myodbc = new IniFile("conf/myodbc.ini");
            Server.myodbc_database = myodbc.ReadValue("myodbc_database", "osaflyff");
            Server.myodbc_username = myodbc.ReadValue("myodbc_username", "root");
            Server.myodbc_password = myodbc.ReadValue("myodbc_password", "");
            Server.myodbc_driver = myodbc.ReadValue("myodbc_driver", "MySQL ODBC 5.1 Driver");
            Server.myodbc_host = ToIP(myodbc.ReadValue("myodbc_host", "localhost"));
            myodbc.destroy();
            IniFile login = new IniFile("conf/loginserver.ini");
            Server.serverip = login.ReadValue("serverip", "127.0.0.1");
            Server.max_connections = login.ReadInt("max_connections", 100);
            Server.client_builddate = login.ReadValue("client_builddate", "20070712");
            Server.client_hash = login.ReadValue("client_hash", "4e50ee67e759d470de67b461aa251868");
            Server.use_flyff_a = login.ReadInt("use_flyff_a", 1) != 0;
            IniFile isc = new IniFile("conf/isc.ini");
            server_host = ToIP(isc.ReadValue("server_host", "127.0.0.1"));
            inter_password = isc.ReadValue("inter_password", "example");
            isc.destroy();
            login.destroy();

            Log.Write(Log.MessageType.info, "Server configuration up to date ({0}).", DateTime.Now);
        }
    }
    public class ClusterInfo
    {
        public string strClusterName, strClusterIP;
        public int dwClusterID;
        public List<WorldInfo> worlds = new List<WorldInfo>();
    }
    public class WorldInfo
    {
        public int dwOnline
        {
            get
            {
                string svidlong = dwClusterID.ToString().PadLeft(2, '0') + "0" + dwWorldID.ToString().PadLeft(2, '0');
                ResultSet rs = new ResultSet("SELECT COUNT(*) amount FROM flyff_accounts WHERE flyff_linestatus='{0}'", svidlong);
                int online = 0;
                if (!rs.Advance())
                    Log.Write(Log.MessageType.error, "Could not get amount of online players for server {0}-{1}.", dwClusterID, dwWorldID);
                else
                    online = rs.Readint("amount");
                return online;
            }
        }
        public string strWorldName;
        public int dwCapacity, dwWorldID, dwClusterID;
    }
}
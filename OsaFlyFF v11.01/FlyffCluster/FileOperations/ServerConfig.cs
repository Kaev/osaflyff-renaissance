using System;
using System.Collections.Generic;
using System.Text;

namespace FlyffCluster
{
    class Server // instead of using the long name, this one is acceptable IMO
    {
        public static string
            client_builddate,   // The build date of neuz.exe
            myodbc_username,    // MyODBC username
            myodbc_password,    // MyODBC password
            myodbc_database,    // MyODBC database
            myodbc_driver,      // MyODBC driver
            myodbc_host,        // MyODBC host
            strMyIP, clustername;
        public static int
            clusterid,          // The cluster ID
            max_connections;    // Max connections
        public static int minorVersion = 0,
                          majorVersion = 1;
        public static string server_host = "127.0.0.1",
                             inter_password = "example";
        // Servers arraylist:
        public static List<WorldInfo> servers = new List<WorldInfo>();
        // MakeItems:
        public static List<ItemInfo> itemsF = new List<ItemInfo>(),
                                itemsM = new List<ItemInfo>(),
                                equipsF = new List<ItemInfo>(),
                                equipsM = new List<ItemInfo>();
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
            IniFile cluster = new IniFile("conf/clusterserver.ini");
            Server.max_connections = cluster.ReadInt("max_connections", 100);
            Server.client_builddate = cluster.ReadValue("client_builddate", "20070712");
            Server.clusterid = cluster.ReadInt("clusterid", 1);
            Server.strMyIP = cluster.ReadValue("serverip", "127.0.0.1");
            clustername = cluster.ReadValue("servername", "ClusterServer");
            cluster.destroy();
            IniFile isc = new IniFile("conf/isc.ini");
            majorVersion = isc.ReadInt("version_major", 0);
            minorVersion = isc.ReadInt("version_minor", 1);
            server_host = ToIP(isc.ReadValue("server_host", "127.0.0.1"));
            inter_password = isc.ReadValue("inter_password", "example");
            isc.destroy();

            itemsF.Clear();
            itemsM.Clear();
            equipsF.Clear();
            equipsM.Clear();



            // Lastly for makeitems..
            IniFile female = new IniFile("conf/makeitemsf.ini");
            int i = 0;
            while (true)
            {
                int itemid = female.ReadInt("equip_" + i, -1);
                if (itemid == -1)
                    break;
                int slot = female.ReadInt("equip_" + i + "_slot", -1);
                if (slot == -1)
                {
                    Log.Write(Log.MessageType.warning, "No slot for equip #{0} in MakeItemsF.ini.", i);
                    i++;
                    continue;
                }
                ItemInfo itemInstance = new ItemInfo();
                itemInstance.id = itemid;
                itemInstance.slot = slot;
                Server.equipsF.Add(itemInstance);
                i++;
            }
            i = 0;
            int incslot = 0;
            while (true)
            {
                int itemid = female.ReadInt("item_" + i, -1);
                if (itemid == -1)
                    break;
                int slot = incslot++;
                if (slot > 0x29)
                {
                    Log.Write(Log.MessageType.notice, "Reached the end of inventory while parsing new items.");
                    break;
                }
                ItemInfo itemInstance = new ItemInfo();
                itemInstance.id = itemid;
                itemInstance.slot = slot;
                Server.itemsF.Add(itemInstance);
                i++;
            }
            female.destroy();
            IniFile male = new IniFile("conf/makeitemsm.ini");
            i = 0;
            while (true)
            {
                int itemid = male.ReadInt("equip_" + i, -1);
                if (itemid == -1)
                    break;
                int slot = male.ReadInt("equip_" + i + "_slot", -1);
                if (slot == -1)
                {
                    Log.Write(Log.MessageType.warning, "No slot for equip #{0} in MakeItemsM.ini.", i);
                    i++;
                    continue;
                }
                ItemInfo itemInstance = new ItemInfo();
                itemInstance.id = itemid;
                itemInstance.slot = slot;
                Server.equipsM.Add(itemInstance);
                i++;
            }
            i = 0;
            incslot = 0;
            while (true)
            {
                int itemid = male.ReadInt("item_" + i, -1);
                if (itemid == -1)
                    break;
                int slot = incslot++;
                if (slot > 0x29)
                {
                    Log.Write(Log.MessageType.notice, "Reached the end of inventory while parsing new items.");
                    break;
                }
                ItemInfo itemInstance = new ItemInfo();
                itemInstance.id = itemid;
                itemInstance.slot = slot;
                Server.itemsM.Add(itemInstance);
                i++;
            }

            male.destroy();
            Log.Write(Log.MessageType.info, "Server configuration up to date ({0}).", DateTime.Now);
        }
    }
    public class ItemInfo
    {
        public int id, slot;
    }
    public class WorldInfo
    {
        public int dwWorldID = 0;
        public string strWorldIP = "";
    }
}

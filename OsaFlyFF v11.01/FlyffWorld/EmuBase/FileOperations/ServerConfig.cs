using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace FlyffWorld
{
    class Server // instead of using the long name, this one is acceptable IMO
    {
        // BuffPangConfig added by Nicco
        // These vars will be loaded upon .reload!!
        public static int
            helper_levelreq_min = 1,  // Min level (Default: 1)
            helper_levelreq_max = 59, // Max level (Default: 59)
            helper_buff_duration = 60,   // Buff time (in minutes)
            helper_buff_cost = 0;    // In penya, cost to buff, if 0 its disabled
        public static List<int>
            helper_buffs = new List<int>(),
            helper_buffs_levels = new List<int>();
        public static string
            buff_emsg = "You're level is not in required range...",        // Message if the character is below or above required levels.
            buff_cmsg = "you don't have enough penya !";        // Message if the penya is lower than what buffs cost.

        public static string worldname, strMyIP;

        public static int exp_rate, penya_rate, drop_rate, exp_rate60, exp_rate90;
        static string notes = "";
        static int notespos = 1;
        public static void AppendRemark(string s, params object[] args)
        {
            notes += notespos++ + ". " + string.Format(s, args) + Environment.NewLine;
        }
        public static void OnLoad()
        {
            // load developer notes
            if (notes != "")
            {
                Console.WriteLine("Latest bugfixes/messages from the developers:");
                Console.WriteLine("--------------------------------");
                Console.WriteLine(notes);
            }
        }
        public static string
            welcome_message,    // Welcome message
            myodbc_username,    // MyODBC username
            myodbc_password,    // MyODBC password
            myodbc_database,    // MyODBC database
            myodbc_driver,      // MyODBC driver
            myodbc_host;        // MyODBC host
        public static int
            max_connections,    // Max connections
            clusterid,          // Cluster ID
            worldid,            // World ID
            weather,            // Weather ID + 0x60 for packet
            levelup_state;      // Levelup state, refer to worldserver.ini for details OR FlyffWorld/Server/Client/Main/Events.cs if you can understand sourcecode.
        public static bool
            enable_wmessage,    // Enable/disable welcome message
            enable_debugging,   // Enable/disable debugging
            go_enable_indexing;
        public static int minorVersion = 0,
                          majorVersion = 1;
        public static string server_host = "127.0.0.1",
                             inter_password = "example";
        public static int threadDelay_visibility = 50;
        public static int threadDelay_ping = 3600000;
        public static int threadDelay_timers = 50;
        public static int threadDelay_mover = 100;
        public static int threadDelay_world = 1000;
        public static int threadDelay_action = 100;
        public static int threadDelay_CS = 60000;
        public static int[] events = new int[1024];
        public static List<MAKELOCATION> goLocations = new List<MAKELOCATION>();
        public static bool firstRun = true;
        public static int CountByValue(int[] a, int val)
        {
            int c = 0;
            for (int i = 0; i < a.Length; i++)
                c += a[i] == val ? 1 : 0;
            return c;
        }
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
            Server.myodbc_host     = myodbc.ReadValue("myodbc_host"    , "localhost");
            myodbc.destroy();
            IniFile world = new IniFile("conf/worldserver.ini");
            Server.max_connections  = world.Readint  ("max_connections" , 100);
            Server.worldid          = world.Readint  ("worldid"         , 1);
            Server.clusterid        = world.Readint  ("clusterid"       , 1);
            Server.weather          = world.Readint  ("default_weather" , 0) + 0x60;
            Server.enable_debugging = world.Readint  ("enable_debugging", 1) != 0;
            Server.enable_wmessage  = world.Readint  ("enable_wmessage" , 1) != 0;
            Server.welcome_message  = world.ReadValue("welcome_message" , "Welcome to open source Adidishen's Flyff emulator project!");
            Server.events[10]       = world.Readint  ("pk_everywhere"   , 1) != 0 ? 1 : 0;
            Server.levelup_state    = world.Readint  ("levelup_state"   , 2);
            Log.DebugLevel          = world.Readint  ("loglevel"        , Log.LOGLEVEL_ALL);
            Server.worldname = world.ReadValue("servername", "WorldServer");
            Server.strMyIP = world.ReadValue("serverip", "127.0.0.1");
            Server.levelup_state = Client.LimitNumber(Server.levelup_state, 0, 2);
            world.destroy();
            IniFile threads = new IniFile("conf/worlddelay.ini");
            threadDelay_ping = threads.Readint("sqldelay", 3600000);
            threadDelay_visibility = threads.Readint("visibilitydelay", 50);
            threadDelay_timers = threads.Readint("timersdelay", 50);
            threadDelay_mover = threads.Readint("moverdelay", 100);
            threadDelay_world = threads.Readint("worlddelay", 1000);
            threads.destroy();
            IniFile isc = new IniFile("conf/isc.ini");
            majorVersion = isc.Readint("version_major", 0);
            minorVersion = isc.Readint("version_minor", 1);
            server_host = isc.ReadValue("server_host", "127.0.0.1");
            inter_password = isc.ReadValue("inter_password", "example");
            isc.destroy();
            goLocations.Clear();
            // Load locations
            IniFile locations = new IniFile("conf/commandgo.ini"); // load configuration file commandgo.ini inside folder conf
            go_enable_indexing = locations.Readint("enable_indexing", 1) != 0;
            int i = 0;
            while (true)
            {
                string curLocation = locations.ReadValue("loc-" + i, null);
                if (curLocation == null) // no more locations
                    break;
                string curPositionData = locations.ReadValue("pos-" + i, null);
                if (curPositionData == null)
                {
                    Log.Write(Log.MessageType.notice, "Skipping location \"{0}\" because no position data is set.", curLocation);
                    i++;
                    continue; // move on to the next location
                }
                // check if locationdata is valid
                float[] pos = new float[3]; // a new float array containing maximum of 3 variables at once
                int worldID = 1;
                try
                {
                    string[] splitPos = curPositionData.Split(' ');
                    pos[0] = Convert.ToSingle(splitPos[0]);
                    pos[1] = Convert.ToSingle(splitPos[1]);
                    pos[2] = Convert.ToSingle(splitPos[2]); // parsed all x y z position, converted from string to float
                    try
                    {
                        worldID = int.Parse(splitPos[3]);
                    }
                    catch { }
                }
                catch {
                    Log.Write(Log.MessageType.warning, "Bad position data for location #{0} ({1}), skipping.", i, curLocation);
                    i++;
                    continue; // bad position data so we skip this one.
                }
                // so far so good, position exists with data converted to floats
                // add them to the configuration variable after construction
                // of a new position class
                MAKELOCATION currentLocation = new MAKELOCATION(curLocation, pos[0], pos[1], pos[2], worldID);
                // initialized stuff, now add to the arraylist:
                goLocations.Add(currentLocation); // done
                i++; // move on to the next location (prevents endless loop, yeah?!)
            }
            locations.destroy(); // close file reading stream to enable file editing
            IniFile worldrates = new IniFile("conf/worldrates.ini");
            exp_rate = worldrates.Readint("exp_rate", 1); //rate before lvl 60
            exp_rate60 = worldrates.Readint("exp_rate60", 1);//rate after lvl 60
            exp_rate90 = worldrates.Readint("exp_rate90", 1); //rate after lvl 90
            drop_rate = worldrates.Readint("drop_rate", 1);
            penya_rate = worldrates.Readint("penya_rate", 1);
            worldrates.destroy();
            Log.Write(Log.MessageType.info, "*** SERVER RATES: {0}x EXP less lvl 20, {1}x EXP for lvl 20-60 and {2}xEXP for lvl 60+, {3}x DROP, {4}x PENYA ***", exp_rate,exp_rate60,exp_rate90, drop_rate, penya_rate);
            Log.Write(Log.MessageType.info, "Server configuration up to date ({0}).", DateTime.Now);
        }
    }
    class MAKELOCATION
    {
        // used to store location stuff
        public float x, y, z;
        public int mapID = 1;
        public string name;
        public MAKELOCATION(string name, float x, float y, float z, int mapID) {
            this.x = x;
            this.y = y;
            this.z = z;
            this.mapID = mapID;
            this.name = name;
        } // must be constructed
    }
}

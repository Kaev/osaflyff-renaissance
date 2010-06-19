/*We will don't use anymore this system
 * 
 * using System;
using System.Collections.Generic;
using System.Text;

namespace FlyffWorld
{
    public class ScrollProtection
    {
        private bool[] bProtections = new bool[7];
        private int dwCharID = -1;
        /// <summary>
        /// Auto saves data in database each use of a scroll.
        /// </summary>
        public bool bAutoSave = false;
        /// <summary>
        /// Type of scroll.
        /// </summary>
        public const int
            SCROLL_SPROTECT = 0,  // Protects weapons/armors from being destroyed on upgrading
            SCROLL_XPROTECT = 1,  // Protects weapon from being destroyed on unique conversion
            SCROLL_GPROTECT = 2,  // Protects suits from being destroyed on socket slot upgrading
            SCROLL_APROTECT = 3,  // Protects jewelry from being destroyed on upgrading
            SCROLL_HSMELTING = 4, // Increases success rate by 100% to +7
            SCROLL_SMELTING = 5, // Increases success rate by 10% to +7
            SCROLL_BLESSING = 6; // Prevents exp loss upon death
        /// <summary>
        /// Creates a new object of ScrollProtection and defaults all values to false.
        /// </summary>
        public ScrollProtection()
        {
            for (int i = 0; i < 7; i++)
                bProtections[i] = false;
        }
        /// <summary>
        /// Creates a new object of ScrollProtection and inherit values from database.
        /// </summary>
        /// <param name="charid">Characters ID</param>
        /// <param name="autosave">Auto save to database</param>
        public ScrollProtection(int charid, bool autosave)
        {
            for (int i = 0; i < 7; i++)
                bProtections[i] = false;
            /* LOAD HERE 
            ResultSet rs = new ResultSet("SELECT `scrollid` FROM `flyff_activescrolls` WHERE `charid`='{0}';", charid);
            while (rs.Advance())
            {
                int scroll_id = rs.Readint("scrollid");
                if ((scroll_id >= 0) && (scroll_id <= 6))
                    bProtections[scroll_id] = true;
            }
            rs.Free();
            dwCharID = charid;
            bAutoSave = autosave;
        }
        /// <summary>
        /// Load data from database.
        /// </summary>
        /// <param name="charid">Characters ID</param>
        public void LoadData(int charid)
        {
            dwCharID = charid;
            ResultSet rs = new ResultSet("SELECT `scrollid` FROM `flyff_activescrolls` WHERE `charid`='{0}';", charid);
            while (rs.Advance())
            {
                int scroll_id = rs.Readint("scrollid");
                if ((scroll_id >= 0) && (scroll_id <= 6))
                    bProtections[scroll_id] = true;
            }
            rs.Free();
        }
        /// <summary>
        /// Save data to the database.
        /// </summary>
        /// <returns>True if characters ID where provided.</returns>
        public bool SaveData()
        {
            bool ret = true;
            if (dwCharID == -1)
                ret = false;
            else
            {
                Database.Execute("DELETE FROM `flyff_activescrolls` WHERE `charid`='{0}';", dwCharID);
                for (int i = 0; i < 7; i++)
                    if (bProtections[i])
                        Database.Execute("INSERT INTO `flyff_activeitems` (`charid`,`itemid`) VALUES ('{0}','{1}')",
                            dwCharID, i);
            }
            return ret;
        }
        /// <summary>
        /// Sets state of a scroll. Set true if scroll is activated, otherwise false.
        /// </summary>
        /// <param name="Type">Type of scroll</param>
        /// <param name="Value">State of scroll</param>
        public void setValue(int Type, bool Value)
        {
            bProtections[Type] = Value;
            if ((dwCharID != -1) && bAutoSave)
            {
                string sql = String.Format("DELETE FROM `flyff_activescrolls` WHERE `charid`='{0}' AND `scrollid`='{1}';", dwCharID, Type);
                Database.Execute(sql);
                if (Value == true)
                {
                    sql = String.Format("INSERT INTO `flyff_activescrolls` (`charid`,`scrollid`) VALUES ('{0}','{1}');", dwCharID, Type);
                    Database.Execute(sql);
                }
            }
        }
        /// <summary>
        /// Gets state of a scroll.
        /// </summary>
        /// <param name="Type">Type of scroll</param>
        /// <returns>Returns true if scroll is activated, otherwise false.</returns>
        public bool getValue(int Type)
        {
            return bProtections[Type];
        }
        /// <summary>
        /// Send scroll effect usage to surrounding players
        /// </summary>
        /// <param name="player">Player who uses the scroll</param>
        public void sendScrollEffect(Client player)
        {
            Point playerPos = new Point(player.c_position.x, player.c_position.y, player.c_position.z);
            player.SendEffect(FlyFF.SYS_RELEASE01, playerPos);
        }
        /// <summary>
        /// Icon that indicate you are using scroll
        /// </summary>
        /// <param name="player">Player to send to</param>
        /// <param name="icon">Icon to send</param>
        public void sendIcon(Client player, int scroll) // TODO: Atm can't debuff icon because unknown packet!
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(player.dwMoverID, PacketCommands.PAK_BUFF);
            pak.Addint16(0); // buff type
            switch (scroll)
            {
                case SCROLL_APROTECT: pak.Addint16(26473); break;
                case SCROLL_BLESSING: pak.Addint16(10430); break;
                case SCROLL_GPROTECT: pak.Addint16(10465); break;
                case SCROLL_HSMELTING: pak.Addint16(26203); break;
                case SCROLL_SMELTING: pak.Addint16(10468); break;
                case SCROLL_SPROTECT: pak.Addint16(10464); break;
                case SCROLL_XPROTECT: pak.Addint16(10488); break;
                default: return; break;
            }
            pak.Addint32(0); // buff level
            pak.Addint32(1); // buff time
            pak.Send(player);
        }
    }
}*/

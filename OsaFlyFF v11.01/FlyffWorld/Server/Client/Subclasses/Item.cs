using System;
using System.Collections.Generic;

using System.Text;

namespace FlyffWorld
{
    public class Item
    {
        public const int
           ELEMENT_NONE = 0,
           ELEMENT_FIRE = 1,
           ELEMENT_WATER = 2,
           ELEMENT_ELECTRICITY = 3,
           ELEMENT_WIND = 4,
           ELEMENT_EARTH = 5;
        ItemData _data = null;
        public ItemData Data
        {
            get
            {
                if (_data != null)
                    return _data;
                _data = WorldHelper.GetItemDataByItemID(dwItemID);
                if (_data == null)
                {
                    Log.Write(Log.MessageType.error, "Item.Data & ItemBank.getItemByID({0}): failed", dwItemID);
                    return new ItemData();
                }
                return _data;
            }
        }
        public int
            dwItemID = 0,
            dwRefine = 0,
            dwElement = 0,
            dwEleRefine = 0,
            dwQuantity = 0;
        public long qwLastUntil = -1;
        public bool bExpired = false;
        public ItemSockets c_sockets;
        public AwakeAttribute[] c_awakenings;
        private long m_qwAwakening = 0;
        public long c_awakening
        {
            get { return m_qwAwakening; }
            set
            {
                Awakening aw = new Awakening();
                m_qwAwakening = value;
                if (m_qwAwakening == 0)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        try
                        {
                            c_awakenings[i].attribute = 0;
                            c_awakenings[i].value = 0;
                            c_awakenings[i].negative = false;
                        }
                        catch (Exception e)
                        {
                            Log.Write(Log.MessageType.error, "Could not set awakeattributes[{0}]!\r\nMsg: {1}",
                                i, e.Message);
                        }
                    }
                }
                else
                    c_awakenings = Awakening.ToAwakeAttributeArray(m_qwAwakening);
            }
        }
        public Item()
        {
            c_awakenings = new AwakeAttribute[3]; // 0,1,2 attributes
            c_sockets = new ItemSockets(0);
        }
        public Item(int amount_of_item_sockets)
        {
            c_awakenings = new AwakeAttribute[3]; // 0,1,2 attributes
            c_sockets = new ItemSockets(amount_of_item_sockets);
        }
        public override string ToString()
        {
            return base.ToString();
        }
    }
    public class Slot
    {
        public int dwPos = 0, dwID = 0;
        public Item c_item = null;
    }
    public class ItemSockets
    {
        public ItemSockets(int socketsCount)
        {
            _sockets = new int[socketsCount];
        }
        public int Length
        {
            get
            {
                return _sockets.Length;
            }
        }
        public int[] _sockets;
        public int this[int socket_slot]
        {
            get
            {
                return _sockets[socket_slot];
            }
            set
            {
                _sockets[socket_slot] = value;
            }
        }
        /// <summary>
        /// Gets a string including all socket items in it seperated by a comma.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (_sockets.Length == 0)
                return "";
            string ret = "";
            for (int i = 0; i < _sockets.Length; i++)
                ret += _sockets[i] + ",";
            ret = ret.Substring(0, ret.Length - 1);
            return ret;
        }
    }
}

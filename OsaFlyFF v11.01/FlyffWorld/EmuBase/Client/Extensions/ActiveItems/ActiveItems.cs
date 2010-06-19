using System;
using System.Collections.Generic;
using System.Text;

namespace FlyffWorld
{
    public class ActiveItems
    {
        public int itemid = 0,
                   charid = 0,
                   remainingtime = -1;

        public long lastuntil = -1;

        public ActiveItems(int itemid,int charid,long lastuntil,int remainingtime)
        {
            this.itemid = itemid;
            this.charid = charid;
            this.lastuntil = lastuntil;
            this.remainingtime = remainingtime;
        }
        public ActiveItems()
        {
        }
    }
    public partial class Client
    {
        public ActiveItems GetPlayerActivatedItem(int itemid)
        {
            for (int i = 0; i < c_data.activateditem.Count; i++)
            {
                ActiveItems activateitem = c_data.activateditem[i];
                if (activateitem == null)
                    continue;
                if (activateitem.itemid == itemid)
                    return activateitem;
            }
            return null;
        }
    }
}

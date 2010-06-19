using System;
using System.Collections.Generic;
using System.Text;

namespace FlyffWorld
{
    public class Drop : Mover // Created by Nicco->Drops
    {
        private long iprobability; // hidden value, we have need min/max check so we use property
        /// <summary>
        /// A value between 1 and 3000000000.
        /// </summary>
        public long Probability
        {
            get { return iprobability; }
            set
            {
                if (value > 3000000000)
                    value = 3000000000;
                if (value < 1)
                    value = 1;
                iprobability = value;
            }
        }
        public int ownerId,
                   levelmin,//add by divinepunition to allow randomize item like armor/weapon etc
                   levelmax;
        public long despawnTime;
        public long freeTime;
        public long respawnTime;
        public bool neverDespawn,
                    isRandomItem;//add by divinepunition to allow randomize item like armor/weapon etc
        public bool doRespawn; // Case an drop that always should respawn
        public bool pickedup; // If true that means it's not on floor and should respawn if doRespawn==true
        public float spawnRange;
        public Item item;
        public int MapID;
        public Drop(Item theitem, int owner, int mapid, Point position, bool neverdespawn)
            : base(MOVER_ITEM)
        {
            item = theitem;
            freeTime = DLL.clock() + 60000; // A minute for user to pickup drop (then other can pick it up)
            despawnTime = DLL.clock() + 300000; // 5 minutes for user to pickup drop
            respawnTime = 0;
            doRespawn = false;
            pickedup = false;
            spawnRange = 3;
            dwMoverID = MoversHandler.NewMoverID();
            this.neverDespawn = neverDespawn;
            MapID = mapid;
            c_position = position;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;

namespace FlyffWorld
{
    public class CollectRegion : Region
    {
        public int Map = 1;

        public bool IsInRegion(Point point, int dwMapID)
        {
            if (base.RegionParametersInvalid)
            {
                return false;
            }
            if (dwMapID == Map && (base.IsInRegion(point) || RegionUnlimited))
            {
                return true;
            }
            return false;
        }
        public bool RegionUnlimited
        {
            get
            {

                return f_northwest_x == -1 || f_northwest_z == -1 || f_southeast_x == -1 || f_southeast_z == -1;
            }
        }
    }
}

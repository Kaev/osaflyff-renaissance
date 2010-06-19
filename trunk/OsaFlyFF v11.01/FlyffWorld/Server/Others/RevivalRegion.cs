using System;
using System.Collections.Generic;
using System.Text;

namespace FlyffWorld
{
    public class RevivalRegion : Region
    {
        /// [Adidishen]
        /// Due to the worldserver being a fucking loser program I have to use properties.

        private Point c_real_destiny;
        private bool c_destiny_protector = false;
        public Point c_destiny
        {
            get
            {
                return c_real_destiny;
            }
            set
            {
                if (!c_destiny_protector)
                {
                    c_destiny_protector = true;
                    c_real_destiny = value;
                }
            }
        }
        public int dwSrcMap = 1, dwDestMap = 1;

        public bool IsInRegion(Point point, int dwMapID)
        {
            if (base.RegionParametersInvalid)
            {
                return false;
            }
            if (dwMapID == dwSrcMap && (base.IsInRegion(point) || RegionUnlimited))
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

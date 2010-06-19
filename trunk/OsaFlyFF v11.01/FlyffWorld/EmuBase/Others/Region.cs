using System;
using System.Collections.Generic;
using System.Text;

namespace FlyffWorld
{
    public class Region
    {
        public float f_northwest_x = 0,
                     f_northwest_z = 0,

                     f_southeast_x = 0,
                     f_southeast_z = 0;

        public bool IsInRegion(Point point)
        {
            if (RegionParametersInvalid)
                return false;
            // check if the point is in the Z region
            if (f_northwest_z >= point.z && point.z >= f_southeast_z)
            {
                // check if the point is in the X region.
                if (f_northwest_x <= point.x && point.x <= f_southeast_x)
                {
                    return true;
                }
            }
            return false;
        }
        public bool RegionParametersInvalid
        {
            get
            {
                return f_northwest_z < f_southeast_z || f_northwest_x > f_southeast_x;
            }
        }
        /*
===================================
| northwest                       |
|  •---------------\              |
|  |   region.     |              |
|  |               |southeast     |
|  \---------------•              |
|                                 |
|                                 |
|     not in region here          |
|                                 |
|                                 |
|                                 |
|                                 |
===================================
         */
    }
}

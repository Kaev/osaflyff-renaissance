using System;
using System.Collections.Generic;

using System.Text;

namespace FlyffWorld
{
    public class Warpzone
    {
        public Point position = new Point();
        public Point destination = new Point();
        public int srcMapID = 0;
        public int dstMapID = 0;
        public float radius = 0;
        public Warpzone(Point sourcePoint, Point destPoint, int sourceMap, int destMap, float warpzoneRadius)
        {
            position = sourcePoint;
            destination = destPoint;
            srcMapID = sourceMap;
            dstMapID = destMap;
            this.radius = warpzoneRadius;
        }
    }
}

using System;
using System.Collections.Generic;

using System.Text;

namespace FlyffWorld.Databases
{
    public class BuffData
    {
        int levels = 0;
        public BuffDataSingle[] bufflevel;
        public BuffData(int levels)
        {
            this.levels = levels;
            bufflevel=new BuffDataSingle[levels];
        }
    }
    public class BuffDataSingle
    {
        //int level = 0,
          //  attribute = 0,
            //raiseby = 0,
            //raisebypercents = 0;
    }
}

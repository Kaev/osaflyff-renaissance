using System;
using System.Collections.Generic;
using System.Text;

namespace FlyffWorld
{
    public class ItemUpdateStatus
    {
        public int itemUniqueID     = 0,
                   modifierData     = 0,
                   updateModifier   = 0;
        public ItemUpdateStatus(int id, int mod, int data)
        {
            itemUniqueID = id;
            modifierData = data;
            updateModifier = mod;
        }
    }
}

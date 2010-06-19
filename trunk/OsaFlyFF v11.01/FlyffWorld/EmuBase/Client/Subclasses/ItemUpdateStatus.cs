using System;
using System.Collections.Generic;
using System.Text;

namespace FlyffWorld
{
    public class ItemUpdateStatus
    {
        public int itemUniqueID = 0,
                   modifierData = 0,
                   updateModifier = 0,
                   cardsocketID = 0;
        public ItemUpdateStatus(int id, int mod, int data, int cardID)
        {
            itemUniqueID = id;
            modifierData = data;
            updateModifier = mod;
            cardsocketID = cardID;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;


namespace FlyffWorld
{
    public partial class Client
    {
        #region Delay Variables
        /// <summary>
        /// Last usage of food. (Miliseconds)
        /// </summary>
        public long FoodDelay = 0;
        /// <summary>
        /// Last usage of pill. (Miliseconds)
        /// </summary>
        public long PillDelay = 0;
        #endregion

        #region Process Item Usage
        /// <summary>
        /// Checks if an item is not an equip and acts accordingly.
        /// </summary>
        /// <param name="ItemSlot">Slot of the item.</param>
        /// <returns>True if the item was consumed and is not equip, false if item is equipments.</returns>
        public bool ProcessItemUsage(Slot slot)
        {
            Item item = slot.c_item;
            switch (GetItemType(item))
            {
                #region CS Pet
                case ItemTypes.CSPet:
                    {
                        SendMessageInfoNotice("Cash shop pet's does not work in this version.");
                        return true;
                    }
                #endregion
                #region Egg
                case ItemTypes.Egg:
                    {
                        SendMessageInfoNotice("Egg's does not work in this version.");
                        return true;
                    }
                #endregion
                #region Food
                case ItemTypes.Food:
                    {
                        if (!(FoodDelay <= DLL.clock()))
                        {
                            SendMessageInfo(FlyFF.TID_GAME_ATTENTIONCOOLTIME);
                            return true;
                        }
                        FoodDelay = DLL.clock() + 2300;
                        int hpRecovery = ((item.Data.adjAttributes[0] > (c_data.f_MaxHP - c_attributes[DST_HP]) ? (c_data.f_MaxHP - c_attributes[DST_HP]) : item.Data.adjAttributes[0]));
                        if (c_attributes[DST_HP] >= item.Data.min_ability)
                        {
                            hpRecovery /= 4;
                            SendMessageInfo(FlyFF.TID_GAME_LIMITHP);
                        }
                        c_attributes[DST_HP] += hpRecovery;
                        SendPlayerAttribSet(DST_HP, c_attributes[DST_HP]);
                        SendEffect(FlyFF.GEN_CURE01);
                        lock (c_data.dataLock) // ----------------> NEEDED?!
                            DecreaseQuantity(slot);
                        return true;
                    }
                #endregion
                #region Pill
                case ItemTypes.Pill:
                    {
                        if (PillDelay <= DLL.clock())
                        {
                            SendMessageInfo(FlyFF.TID_GAME_ATTENTIONCOOLTIME);
                            return true;
                        }
                        PillDelay = DLL.clock() + 8000;
                        int hpRecovery = ((item.Data.adjAttributes[0] > (c_data.f_MaxHP - c_attributes[DST_HP]) ? c_data.f_MaxHP - c_attributes[DST_HP] : item.Data.adjAttributes[0]));
                        if (c_attributes[DST_HP] >= item.Data.min_ability)
                        {
                            hpRecovery /= 4;
                            SendMessageInfo(FlyFF.TID_GAME_LIMITHP);
                        }
                        c_attributes[DST_HP] += hpRecovery;
                        SendPlayerAttribSet(DST_HP, c_attributes[DST_HP]);
                        SendEffect(FlyFF.GEN_CURE01);
                        lock (c_data.dataLock) // ----------------> NEEDED?!
                            DecreaseQuantity(slot);
                        return true;
                    }
                #endregion
                #region Refresher
                case ItemTypes.Refresher:
                    {
                        int mpRecovery = ((item.Data.adjAttributes[0] > (c_data.f_MaxMP - c_attributes[DST_MP]) ? c_data.f_MaxMP - c_attributes[DST_MP] : item.Data.adjAttributes[0]));
                        if (c_attributes[DST_MP] >= item.Data.min_ability)
                        {
                            mpRecovery /= 4;
                            SendMessageInfo(FlyFF.TID_GAME_LIMITMP);
                        }
                        c_attributes[DST_MP] += mpRecovery;
                        SendPlayerAttribSet(DST_MP, c_attributes[DST_MP]);
                        SendEffect(FlyFF.GEN_REF01);
                        lock (c_data.dataLock) // ----------------> NEEDED?!
                            DecreaseQuantity(slot);
                        return true;
                    }
                #endregion
                #region Scroll
                case ItemTypes.Scroll:
                    {
                        SendMessageInfoNotice("Protection scrolls does not work in this version.");
                        return true;
                    }
                #endregion
                #region VitalDrink
                case ItemTypes.VitalDrink:
                    {
                        int fpRecovery = ((item.Data.adjAttributes[0] > (c_data.f_MaxFP - c_attributes[DST_FP]) ? c_data.f_MaxFP - c_attributes[DST_FP] : item.Data.adjAttributes[0]));
                        if (c_attributes[DST_FP] >= item.Data.min_ability)
                        {
                            fpRecovery /= 4;
                            SendMessageInfo(FlyFF.TID_GAME_LIMITFP);
                        }
                        c_attributes[DST_FP] += fpRecovery;
                        SendPlayerAttribSet(DST_FP, c_attributes[DST_FP]);
                        SendEffect(FlyFF.GEN_CURE01);
                        lock (c_data.dataLock) // ----------------> NEEDED?!
                            DecreaseQuantity(slot);
                        return true;
                    }
                #endregion
            }
            return false;
        }
        #endregion

        #region Item Types
        /// <summary>
        /// Defines a set of typical items a player can consume or use.
        /// </summary>
        public enum ItemTypes
        {
            Other,
            Refresher,
            Food,
            VitalDrink,
            Scroll,
            Pill,
            CSPet,
            Egg
        }
        #endregion

        #region GetItemType function
        /// <summary>
        /// Gets an enumeration of the type of item that represents item id.
        /// </summary>
        /// <param name="ItemID">Item ID of the item.</param>
        /// <returns>Enumeration of type ItemTypes representing type of item.</returns>
        public ItemTypes GetItemType(Item item)
        {
            switch (item.Data.itemkind[0] + item.Data.itemkind[1] + item.Data.itemkind[2])
            {
                case 39: return ItemTypes.Refresher;
                case 47:
                case 48:
                case 49: return ItemTypes.Food;
                case 50: return ItemTypes.VitalDrink;
                case 75: return ItemTypes.Scroll;
                case 76: return ItemTypes.Pill;
                case 131: return ItemTypes.CSPet;
                case 151: return ItemTypes.Egg;
            }
            return ItemTypes.Other;
        }
        #endregion
    }
}
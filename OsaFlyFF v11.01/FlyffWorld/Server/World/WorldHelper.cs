using System;
using System.Collections.Generic;
using System.Text;

namespace FlyffWorld
{
    /// <summary>
    /// Contains various static functions to easily fetch variables from world data.
    /// </summary>
    public class WorldHelper
    {
        /// <summary>
        /// Searches an item's data associated with the given item ID.
        /// </summary>
        /// <param name="itemID">The item ID of the item data object.</param>
        /// <returns></returns>
        public static ItemData GetItemDataByItemID(int itemID)
        {
            for (int i = 0; i < WorldServer.data_items.Count; i++)
            {
                ItemData item = WorldServer.data_items[i];
                if (item.itemID == itemID)
                    return item;
            }
            return null;
        }
        /// <summary>
        /// Searches a player associated with the given mover ID.
        /// </summary>
        /// <param name="dwMoverID">The mover ID of the player.</param>
        /// <returns></returns>
        public static Client GetClientByMoverID(int dwMoverID)
        {
            for (int i = 0; i < WorldServer.world_players.Count; i++)
                if (WorldServer.world_players[i].dwMoverID == dwMoverID)
                    return WorldServer.world_players[i];
            return null;
        }
        /// <summary>
        /// Searches a mover associated with the given mover ID in the given mover list.
        /// </summary>
        /// <param name="dwID">The mover ID of the mover.</param>
        /// <param name="moverList">The mover list that will be searched.</param>
        /// <returns></returns>
        public static Mover GetMoverFromListByID(int dwID, List<Mover> moverList)
        {
            for (int i = 0; i < moverList.Count; i++)
                if (moverList[i].dwMoverID == dwID)
                    return moverList[i];
            return null;
        }
        /// <summary>
        /// Searches a player associated with the given character ID.
        /// </summary>
        /// <param name="dwID">The character ID of the player.</param>
        /// <returns></returns>
        public static Client GetClientByPlayerID(int dwID)
        {
            for (int i = 0; i < WorldServer.world_players.Count; i++)
                if (WorldServer.world_players[i].c_data.dwCharacterID == dwID)
                    return WorldServer.world_players[i];
            return null;
        }
        /// <summary>
        /// Searches a player associated with the given player name.
        /// </summary>
        /// <param name="strName">The name of the player. (case-insensitive)</param>
        /// <returns></returns>
        [System.Diagnostics.DebuggerHidden()]
        public static Client GetClientByPlayerName(string strName)
        {
            for (int i = 0; i < WorldServer.world_players.Count; i++)
                if (WorldServer.world_players[i].c_data.strPlayerName.ToLower() == strName.ToLower())
                    return WorldServer.world_players[i];
            return null;
        }
        /// <summary>
        /// Gets the default revival region of the server.
        /// </summary>
        public static readonly RevivalRegion DefaultRevivalRegion = new RevivalRegion()
        {
            c_destiny = new Point(6967, 100, 3333),
            dwDestMap = 1,
            dwSrcMap = 1,
            f_northwest_x = -1,
            f_northwest_z = -1,
            f_southeast_x = -1,
            f_southeast_z = -1
        };
        /// <summary>
        /// Searches an NPC associated with the given mover ID.
        /// </summary>
        /// <param name="dwID">The mover ID of the NPC.</param>
        /// <returns></returns>
        public static NPC GetNPCByMoverID(int dwID)
        {
            for (int i = 0; i < WorldServer.world_npcs.Count; i++)
                if (WorldServer.world_npcs[i].dwMoverID == dwID)
                    return WorldServer.world_npcs[i];
            return null;
        }
        /// <summary>
        /// Searches an NPC shop object associated with the given NPC type.
        /// </summary>
        /// <param name="type">The type of the NPC.</param>
        /// <returns></returns>
        public static NPCShopData GetNPCShop(string strType)
        {
            for (int i = 0; i < WorldServer.world_npcshops.Count; i++)
                if (WorldServer.world_npcshops[i].npctype == strType)
                    return WorldServer.world_npcshops[i];
            return null;
        }
    }
}

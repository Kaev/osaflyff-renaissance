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
        public static bool SetUpdateItemInInventory (Client client, Slot slot)
        {
            for (int i=0;i<client.c_data.inventory.Count;i++)
            {
                Slot curslot = client.c_data.inventory[i];
                if (curslot==null)
                    continue;
                if (curslot.dwID == slot.dwID)
                {
                    client.c_data.inventory[i] = slot;
                    return true;
                }
            }
            return false;
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
        /// <summary>
        /// Searches the first available sockets for given item.
        /// </summary>
        /// <param name="type">The slot of item.</param>
        /// <returns></returns>
        public static int GetFirstAvailableSocket(Slot slot)
        {
            if (slot.c_item.c_sockets == null)
                return -1;
            for (int i = 0; i < slot.c_item.c_sockets.Length; i++)
            {
                if (slot.c_item.c_sockets._sockets[i] == 0)
                    return i;
            }
            return -1; //haven't find a free socket so it's full
        }
        // by exos
        public static MoverAttributes GetSetBonuses(Item helmet, Item suit, Item gauntlets, Item boots)
        {
            int setPartCount1 = 0;
            int setID1 = 0;
            int setPartCount2 = 0;
            int setID2 = 0;
            int setPartCount3 = 0;
            int setID3 = 0;
            int helmetID = (helmet == null ? 0 : helmet.dwItemID),
                suitID = (suit == null ? 0 : suit.dwItemID),
                gauntletsID = (suit == null ? 0 : gauntlets.dwItemID),
                bootsID = (boots == null ? 0 : boots.dwItemID);
            if (IsASet(helmetID))
            {
                setPartCount1++;
                setID1 = helmetID;
                if (suitID == helmetID + 1)
                    setPartCount1++;
                if (gauntletsID == helmetID + 2)
                    setPartCount1++;
                if (bootsID == helmetID + 3)
                    setPartCount1++;
            }
            if (IsASet(suitID - 1) && suitID != helmetID + 1)
            {
                setPartCount2++;
                setID2 = suitID - 1;
                if (gauntletsID == suitID + 1)
                    setPartCount2++;
                if (bootsID == suitID + 2)
                    setPartCount2++;
            }
            if (IsASet(gauntletsID - 2) && gauntletsID != suitID + 1 && gauntletsID != helmetID + 2)
            {
                setPartCount3++;
                setID3 = gauntletsID - 2;
                if (bootsID == gauntletsID + 1)
                    setPartCount3++;
            }
            MoverAttributes setBonuses1 = GetSetBonuses(setID1, setPartCount1);
            MoverAttributes setBonuses2 = GetSetBonuses(setID2, setPartCount2);
            MoverAttributes setBonuses3 = GetSetBonuses(setID3, setPartCount3);
            MoverAttributes totalBonuses = new MoverAttributes();
            for (int i = 1; i < 93; i++)
            {
                totalBonuses[i] = setBonuses1[i] + setBonuses2[i] + setBonuses3[i];
            }
            int maxRefine = helmet.dwRefine;
            maxRefine = Math.Min(maxRefine, suit.dwRefine);
            maxRefine = Math.Min(maxRefine, gauntlets.dwRefine);
            maxRefine = Math.Min(maxRefine, boots.dwRefine);
            switch (maxRefine)
            {
                case 10:
                    totalBonuses[FlyFF.DST_STAT_ALLUP] += 3;
                    totalBonuses[FlyFF.DST_HP_MAX_RATE] += 20;
                    totalBonuses[FlyFF.DST_BLOCK_MELEE] += 15;
                    totalBonuses[FlyFF.DST_ADJ_HITRATE] += 45;
                    break;
                case 9:
                    totalBonuses[FlyFF.DST_STAT_ALLUP] += 2;
                    totalBonuses[FlyFF.DST_HP_MAX_RATE] += 10;
                    totalBonuses[FlyFF.DST_BLOCK_MELEE] += 10;
                    totalBonuses[FlyFF.DST_ADJ_HITRATE] += 30;
                    break;
                case 8:
                    totalBonuses[FlyFF.DST_STAT_ALLUP] += 2;
                    totalBonuses[FlyFF.DST_HP_MAX_RATE] += 10;
                    totalBonuses[FlyFF.DST_BLOCK_MELEE] += 10;
                    totalBonuses[FlyFF.DST_ADJ_HITRATE] += 30;
                    break;
                case 7:
                    totalBonuses[FlyFF.DST_STAT_ALLUP] += 1;
                    totalBonuses[FlyFF.DST_HP_MAX_RATE] += 5;
                    totalBonuses[FlyFF.DST_BLOCK_MELEE] += 6;
                    totalBonuses[FlyFF.DST_ADJ_HITRATE] += 20;
                    break;
                case 6:
                    totalBonuses[FlyFF.DST_STAT_ALLUP] += 1;
                    totalBonuses[FlyFF.DST_HP_MAX_RATE] += 5;
                    totalBonuses[FlyFF.DST_BLOCK_MELEE] += 6;
                    totalBonuses[FlyFF.DST_ADJ_HITRATE] += 20;
                    break;
                case 5:
                    totalBonuses[FlyFF.DST_STAT_ALLUP] += 1;
                    totalBonuses[FlyFF.DST_HP_MAX_RATE] += 5;
                    totalBonuses[FlyFF.DST_BLOCK_MELEE] += 3;
                    totalBonuses[FlyFF.DST_ADJ_HITRATE] += 10;
                    break;
                case 4:
                    totalBonuses[FlyFF.DST_BLOCK_MELEE] += 3;
                    totalBonuses[FlyFF.DST_ADJ_HITRATE] += 10;
                    break;
                case 3:
                    totalBonuses[FlyFF.DST_BLOCK_MELEE] += 3;
                    totalBonuses[FlyFF.DST_ADJ_HITRATE] += 10;
                    break;
            }
            return totalBonuses;
        }
        public static MoverAttributes GetSetBonuses(int helmetID, int partCount)
        {
            for (int i = 0; i < WorldServer.data_sets.Count; i++)
            {
                if (WorldServer.data_sets[i].helmetID == helmetID)
                {
                    switch (partCount)
                    {
                        case 2:
                            return WorldServer.data_sets[i].setBonuses2; break;
                        case 3:
                            return WorldServer.data_sets[i].setBonuses3; break;
                        case 4:
                            return WorldServer.data_sets[i].setBonuses4; break;
                        default:
                            return new MoverAttributes(); break;
                    }
                }
            }
            return new MoverAttributes();
        }
        public static bool IsASet(int helmetID)
        {
            for (int i = 0; i < WorldServer.data_sets.Count; i++)
                if (WorldServer.data_sets[i].helmetID == helmetID)
                    return true;
            return false;
        }
        public static int calculateSkillPoint(Client client)
        {
            int vagrant = 0, // Extra Skillpoints which get added by job.
                                           mecenary = 40,
                                           acrobat = 50,
                                           assist = 60,
                                           magician = 90,
                                           knight = 80,
                                           blade = 80,
                                           elementer = 300,
                                           psykeeper = 90,
                                           billposter = 120,
                                           ringmaster = 100,
                                           ranger = 100,
                                           jester = 100;
           int skillpoint = client.c_data.dwLevel * 2;
           if (client.c_data.dwLevel >= 20)
               skillpoint += (client.c_data.dwLevel - 19); //+1 for each level above 20 (corresponding to+3)
           if (client.c_data.dwLevel >= 40)
               skillpoint += (client.c_data.dwLevel - 39); //+1 for each level above 40(corresponding to+4)
           if (client.c_data.dwLevel >= 60)
               skillpoint += (client.c_data.dwLevel - 59); //+1 for each level above 60(corresponding to+5)
           if (client.c_data.dwLevel >= 80)
               skillpoint += (client.c_data.dwLevel - 79); //+1for each level above 80(corresponding to+6)
           if (client.c_data.dwLevel >= 100)
               skillpoint += (client.c_data.dwLevel - 99); //+1 for each level above 100(corresponding to+7)


           switch (client.c_data.dwClass)
            {
                case 0: break;
                case 1: skillpoint += mecenary; break;
                case 2: skillpoint += acrobat; break;
                case 3: skillpoint += assist; break;
                case 4: skillpoint += magician; break;
                case 6: skillpoint += knight + mecenary; break;
                case 7: skillpoint += blade + mecenary; break;
                case 8: skillpoint += jester + acrobat; break;
                case 9: skillpoint += ranger + acrobat; break;
                case 10: skillpoint += ringmaster + assist; break;
                case 11: skillpoint += billposter + assist; break;
                case 12: skillpoint += psykeeper + magician; break;
                case 13: skillpoint += elementer + magician; break;
                case 16:
                case 24:
                case 17:
                case 25: skillpoint = 658; break;
                case 18:
                case 19:
                case 26:
                case 27: skillpoint = 688; break;
                case 20:
                case 28: skillpoint = 698; break;
                case 21:
                case 29: skillpoint = 718; break;
                case 22:
                case 30: skillpoint = 718; break;
                case 23:
                case 31: skillpoint = 928; break;
                default:
                    client.SendMessageHud("Invalid Job!"); break;
            }
           return skillpoint;
        }
        
    }
}

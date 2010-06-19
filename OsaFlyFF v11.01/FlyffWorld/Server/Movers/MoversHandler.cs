using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace FlyffWorld
{
    class MoversHandler
    {
        private static int LastMoverID = 0;
        public static int NewMoverID()
        {
            return ++LastMoverID;
        }
        public static List<Mover> GetSpawnedClients(object me)
        {
            if (!(me is Mover))
                return null;
            List<Mover> clients = new List<Mover>();
            for (int i = 0; i < WorldServer.world_players.Count; i++)
            {
                Client client = WorldServer.world_players[i];
                if (client.c_spawns.Contains((Mover)me))
                    clients.Add(client);
            }
            return clients;
        }
        public static Mover GetMover(int moverID, List<Mover> fromMoverList)
        {
            for (int i = 0; i < fromMoverList.Count; i++)
                if (fromMoverList[i].dwMoverID == moverID)
                    return fromMoverList[i];
            return null;
        }
        public static Mover GetMover(int moverID)
        {
            // Seek players
            for (int i = 0; i < WorldServer.world_players.Count; i++)
                if (WorldServer.world_players[i].dwMoverID == moverID)
                    return WorldServer.world_players[i];

            // Seek NPCs
            for (int i = 0; i < WorldServer.world_npcs.Count; i++)
                if (WorldServer.world_npcs[i].dwMoverID == moverID)
                    return WorldServer.world_npcs[i];

            // Seek mobs
            for (int i = 0; i < WorldServer.world_monsters.Count; i++)
                if (WorldServer.world_monsters[i].dwMoverID == moverID)
                    return WorldServer.world_monsters[i];
            // Seek drops
            for (int i = 0; i < WorldServer.world_drops.Count; i++) // Nicco->Drops
                if (WorldServer.world_drops[i].dwMoverID == moverID)
                    return WorldServer.world_drops[i];
            return null;
        }
    }
}

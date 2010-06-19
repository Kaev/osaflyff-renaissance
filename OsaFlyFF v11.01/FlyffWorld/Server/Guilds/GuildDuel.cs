using System;
using System.Collections.Generic;
using System.Text;

namespace FlyffWorld
{
    public class GuildDuel
    {
        public int dwDuelID = 0;
        public int dwAttackerGuildID = -1;
        public int dwDefenderGuildID = -1;
        public bool bTruceRequested = false;
        public void StartDuel()
        {
            dwDuelID = GuildHandler.GenerateDuelID();
        }
        public void ClearDuel()
        {
            dwDuelID = 0;
            dwAttackerGuildID = -1;
            dwDefenderGuildID = -1;
            bTruceRequested = false;
        }
        public override string ToString()
        {
            return string.Format("dwDuelID: {0}. dwAttackerGuildID: {1}. dwDefenderGuildID: {2}", dwDuelID, dwAttackerGuildID, dwDefenderGuildID);
        }
    }
}

using System;
using System.Collections.Generic;

using System.Text;

namespace FlyffWorld
{
    public class NPCPacket : PacketCommands
    {
        public static void sendNpcChatboxData(NPCChatBoxData ncbd)
        {
            // send ncbd..
            // first open screen
            Packet pak = new Packet();
            pak.StartNewMergedPacket(ncbd.client.dwMoverID, PAK_NPC_CHATBOX);
            pak.Addshort(NPC_CHATBOX_OPENBOX);
            // Add text screens:
            for (int i = 0; i < ncbd.textScreens.Length; i++)
            {
                if (ncbd.textScreens[i] != null)
                {
                    pak.StartNewMergedPacket(ncbd.client.dwMoverID, PAK_NPC_CHATBOX);
                    pak.Addshort(NPC_CHATBOX_ADDTXTSCRN);
                    pak.Addstring(ncbd.textScreens[i]);
                    pak.Addint(0);
                }
            }
            // Add links:
            for (int i = 0; i < 10; i++)
            {
                if (ncbd.linkOptions[i] != null)
                {
                    pak.StartNewMergedPacket(ncbd.client.dwMoverID, PAK_NPC_CHATBOX);
                    pak.Addshort(NPC_CHATBOX_ADDLINKLBL);
                    string[] linkoptions = ncbd.linkOptions[i].Split(new char[] { (char)0 });
                    string text = linkoptions[1], state = linkoptions[0];
                    // add stuff
                    pak.Addstring(text);
                    pak.Addstring(state);
                    pak.Addlong(0);
                }
            }
            pak.Send(ncbd.client);
        }
    }
}

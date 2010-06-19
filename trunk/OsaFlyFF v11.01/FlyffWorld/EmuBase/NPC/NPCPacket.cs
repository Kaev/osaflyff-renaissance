using System;
using System.Collections.Generic;

using System.Text;

namespace FlyffWorld
{
    public class NPCPacket : PacketCommands
    {
        public static void sendNpcChatboxData(NPCChatBoxData ncbd, string state1)
        {
            // send ncbd..
            // first open screen
            Packet pak = new Packet();
            pak.StartNewMergedPacket(ncbd.client.dwMoverID, PAK_NPC_CHATBOX);
            pak.Addint16(NPC_CHATBOX_OPENBOX);
            // Add text screens:
            for (int i = 0; i < ncbd.npcdialog.Count; i++)
            {
                if (ncbd.npcdialog[i].State == state1)
                {
                    pak.StartNewMergedPacket(ncbd.client.dwMoverID, PAK_NPC_CHATBOX);
                    pak.Addint16(NPC_CHATBOX_ADDTXTSCRN);
                    pak.Addstring(ncbd.npcdialog[i].Text);
                    pak.Addint32(0);
                }
            }
            // Add links:
            for (int i = 0; i < 10; i++)
            {
                if (ncbd.linkOptions[i] != null)
                {
                    pak.StartNewMergedPacket(ncbd.client.dwMoverID, PAK_NPC_CHATBOX);
                    pak.Addint16(NPC_CHATBOX_ADDLINKLBL);
                    string[] linkoptions = ncbd.linkOptions[i].Split(new char[] { (char)0 });
                    string text = linkoptions[1], state = linkoptions[0];
                    // add stuff
                    pak.Addstring(text);
                    pak.Addstring(state);
                    pak.Addint64(0);
                }
            }
            pak.Send(ncbd.client);
        }
        public static void SendNPCCloseTchat(Client client)
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(client.dwMoverID, PAK_NPC_CHATBOX);
            pak.Addint16(NPC_CHATBOX_CLOSE);
            pak.Send(client);
        }

      /*  public void SendPlayerSound(string npcvoice) //seem to allow to make NPC speak
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_SOUND);
            pak.Addbyte(1);
            pak.Addstring(npcvoice);
            pak.Send(this);
        }*/



    }
}
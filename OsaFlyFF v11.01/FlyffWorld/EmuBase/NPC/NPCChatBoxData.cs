using System;
using System.Collections.Generic;

using System.Text;

namespace FlyffWorld
{
    public class NPCChatBoxData
    {
        //public string[] textScreens;
        private string[] __REAL_linkOptions = new string[10]; // max of 10 links. private to force coder use functions.
        public string[] linkOptions
        {
            get
            {
                return __REAL_linkOptions;
            }
        }
        public Client client;
        public NPC npc;
        public List<npcdata> npcdialog = new List<npcdata>();
        public NPCChatBoxData(Client client, NPC npc)
        {
            this.client = client;
            this.npc = npc;
        }
        [LuaFunction("AddLink")]
        public bool AddLink(string text, string state)
        {
            bool added = false;
            for (int i = 0; i < 10; i++)
            {
                if (this.__REAL_linkOptions[i] != null)
                    continue;
                added = true;
                this.__REAL_linkOptions[i] = state + (char)0 + text;
                break;
            }
            return added;
        }

        public struct npcdata
        {
            string dialogue;
            string Etat;
            public string Text
            {
                get { return dialogue; }
                set { dialogue = value; }
            }
            public string State
            {
                get { return Etat; }
                set { Etat = value; }
            }
        }
        [LuaFunction("AddText")]
        public void AddText(string text, string state)
        {
            npcdata struc = new npcdata();
            struc.State = state;
            struc.Text = text;
            npcdialog.Add(struc);
        }

        public void CloseWindows(string text, Client client)
        {
            //send say last text and send close windows


            npc.SendMoverChatBalloon(text);
            NPCPacket.SendNPCCloseTchat(client);

        }
    }
}

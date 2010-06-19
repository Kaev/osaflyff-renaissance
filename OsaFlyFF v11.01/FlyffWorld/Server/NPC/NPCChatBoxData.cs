using System;
using System.Collections;

using System.Text;

namespace FlyffWorld
{
    public class NPCChatBoxData
    {
        public string[] textScreens;
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
        public NPCChatBoxData(int amount_of_text_screens, Client client, NPC npc)
        {
            this.client = client;
            this.npc = npc;
            this.textScreens = new string[amount_of_text_screens];
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

    }
}

using System;
using System.Collections.Generic;

using System.Text;

namespace FlyffWorld
{
    public class NPC : Mover
    {
        public int npc_model_id    = 0,
                   npc_size        = 100,
                   npc_mapid       = 1;
        public string npc_speech = "";
        public int npc_speech_delay = 10;
        public string npc_type_name = "";
        public long npc_next_speech_time = DLL.time();
        public int npc_dlg_txt_scrn_count = 10; // unless we set this manually to something else.
        public NPC(int model_id, string type) : base(MOVER_NPC)
        {
            npc_model_id = model_id;
            npc_type_name = type;
        }
    }
}

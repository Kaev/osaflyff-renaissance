using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Text;
using LuaInterface;
namespace FlyffWorld
{
    public class NPCHandler
    {



        public static void parseNpcChatRequest(NPCChatBoxData ncbd, string state,int language)
        {
            
            string dialogPath ="db\\npcdata\\EN\\";
            
            switch (language)
            {
                case 0: dialogPath = "db\\npcdata\\EN\\"; break; // English
                case 1: dialogPath = "db\\npcdata\\FR\\"; break; // French
                case 2: dialogPath = "db\\npcdata\\DE\\"; break; // Dutch
                case 3: dialogPath = "db\\npcdata\\ES\\"; break; // Spanish
                case 4: dialogPath = "db\\npcdata\\TA\\"; break; //tai  version
                default: dialogPath = "db\\npcdata\\EN\\"; break; // English - default
                //you can add other language here
            }
            
            FileInfo fi = new FileInfo(dialogPath + ncbd.npc.npc_type_name + ".fnc");
            if (!fi.Exists)
            {
                Log.Write(Log.MessageType.warning, "NPC type {0} is spawned but no dialog file was found!", ncbd.npc.npc_type_name);
                return;
            }
            Lua lua = LuaRegisterer.registerLuaFunctions(ncbd, new Lua());
            lua["Texts"] = ncbd.textScreens;
            lua["State"] = state;
            lua["Player"] = ncbd.client;
            // Register function
            try
            {
                lua.DoFile(dialogPath + ncbd.npc.npc_type_name + ".fnc");
            }
            catch (Exception e)
            {
                Log.Write(Log.MessageType.warning, "Lua scripting failed for model {0}: {1}", ncbd.npc.npc_type_name, e.Message);
                return;
            }
            NPCPacket.sendNpcChatboxData(ncbd);
        }
    

    }
}
using System;
using System.Collections;
using LuaInterface;
using System.Text;

namespace FlyffWorld
{
    public class NPCShopData
    {
        // private:
        public NPCShopItem[,] shopitems = new NPCShopItem[4, 100];
        public int[] tabinfo = new int[4];
        public string npctype = "";
        public NPCShopData(string type)
        {
            System.IO.FileInfo fi = new System.IO.FileInfo("db\\shopdata\\" + type + ".nsd");
            if (!fi.Exists)
            {
                Log.Write(Log.MessageType.warning, "NPCShopData(): NPC type {0} does not have NSD file assigned to it.", type);
                return;
            }
            Load(type);
        }
        private void Load(string type)
        {
            Lua lua = LuaRegisterer.registerLuaFunctions(this, new Lua());
            try
            {
                lua.DoFile("db\\shopdata\\" + type + ".nsd");
            }
            catch (Exception e)
            {
                Log.Write(Log.MessageType.warning, "Lua shop scripting failed for model {0}: {1}", type, e.Message);
            }
        }
        private int getNewestPosForThisTab(int tab)
        {
            return tabinfo[tab]++;
        }
        [LuaFunction("AddShopItem")]
        public bool AddShopItem(int tab, int id)
        {
            NPCShopItem si = new NPCShopItem();
            si.id = id;
            si.tab = tab;
            si.quantity = 0x7FFF; // max.
            tab = Client.LimitNumber(tab, 0, 3);
            int pos = getNewestPosForThisTab(tab);
            si.pos = pos;
            shopitems[tab, pos] = si;
            return true;
        }
    }
}

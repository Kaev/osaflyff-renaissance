using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace FlyffWorld
{
    public partial class Client
    {
    }
    class IBDLuaManager
    {
        public List<Item> ExtractedItems = new List<Item>();
        public int TotalPenya = 0;
        public int Effect = -1;
        public int MaxItems = -1;
        public void AddBoxItem(int itemid, int probability, int quantity)
        {
            Item item = new Item();
            if (DiceRoller.Roll(probability))
            {
                item.dwItemID = itemid;
                item.dwQuantity = quantity;
                ExtractedItems.Add(item);
            }
        }
        public void AddPenya(int min, int max, int probability)
        {
            if (DiceRoller.Roll(probability))
            {
                TotalPenya += DiceRoller.RandomNumber(min, max - 1);
            }
        }
        public void SetEffect(int effect)
        {
            Effect = effect;
        }
        public void SetMaxItems(int maxitems)
        {
            MaxItems = maxitems;
        }
    }
    public class ItemBoxData
    {
        private LuaInterface.Lua lua;
        private IBDLuaManager ibdlm;
        public ItemBoxData()
        {
            lua = new LuaInterface.Lua();
            ibdlm = new IBDLuaManager();
            lua.RegisterFunction("AddBoxItem", ibdlm, ibdlm.GetType().GetMethod("AddBoxItem"));
            lua.RegisterFunction("AddPenya", ibdlm, ibdlm.GetType().GetMethod("AddPenya"));
            lua.RegisterFunction("SetEffect", ibdlm, ibdlm.GetType().GetMethod("SetEffect"));
            lua.RegisterFunction("SetMaxItems", ibdlm, ibdlm.GetType().GetMethod("SetMaxItems"));
        }
        public int itemID = -1;
        public bool ExecuteScript(string FileName, params object[] args)
        {
            bool ret = false;
            try
            {
                ibdlm.ExtractedItems.Clear();
                ibdlm.TotalPenya = 0;
                ibdlm.MaxItems = -1;
                ibdlm.Effect = -1;
                string file = string.Format(FileName, args);
                lua.DoFile(file);
                string[] b;
                string strItemID = ((b = file.Split('\\'))[b.Length - 1]).Split('.')[0];
                itemID = int.Parse(strItemID);
                ret = true;
            }
            catch (Exception e)
            {
                Log.Write(Log.MessageType.error, "[LuaError] {0}", e.Message);
                ret = false;
            }
            return ret;
        }
        public List<Item> ExtractedItems
        {
            get { return ibdlm.ExtractedItems; }
        }
        public int TotalPenya
        {
            get { return ibdlm.TotalPenya; }
        }
        public int Effect
        {
            get { return ibdlm.Effect; }
        }
        public int MaxItems
        {
            get { return ibdlm.MaxItems; }
        }
    }
}

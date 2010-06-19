using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using LuaInterface;

namespace FlyffWorld
{
    public class MDDLoader
    {
        public static bool LoadMDDFiles()
        {
            WorldServer.data_drops.Clear();
            int iDropDataFiles = 0;
            try
            {
                DirectoryInfo di = new DirectoryInfo("db\\dropdata");
                FileInfo[] mddFiles =  di.GetFiles("*.mdd");
                foreach (FileInfo fi in mddFiles)
                {
                    string fName = fi.Name.Replace(".mdd", "");
                    int fID = -1;
                    try { fID = int.Parse(fName); }
                    catch { continue; }
                    //DropsData dd = new DropsData(fID);
                    try
                    {
                        MonsterDropData2 mdd2 = new MonsterDropData2();
                        mdd2.NewMonster(fID);
                        Lua lua = new Lua();
                        lua.RegisterFunction("AddDrop", mdd2, mdd2.GetType().GetMethod("AddDropProbability"));
                        lua.DoFile(String.Format("db\\dropdata\\{0}", fi.Name));
                        mdd2.AddDropsDataToArray();
                    }
                    catch (Exception e)
                    {
                        Log.Write(Log.MessageType.error, "Failed to load mdd file: {0}\r\n{1}", e.Message, e.StackTrace);
                    }
                    iDropDataFiles++;
                }
            }
            catch (Exception e)
            {
                Log.Write(Log.MessageType.error, "Could not load dropdata file(s): {0}\r\nStack trace: \r\n{1}",
                    e.Message, e.StackTrace);
                return false;
            }
            Log.Write(Log.MessageType.load, "Loaded {0} monster drops.", iDropDataFiles);
            return true;
        }
    }
}

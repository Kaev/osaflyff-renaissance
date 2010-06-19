using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using LuaInterface;

namespace FlyffWorld
{
    class QuestLoader
    {
        public static bool LoadQuest()
        {
            WorldServer.data_quests.Clear();
            int iQuestFiles = 0;
            try
            {
                #region Try to read the quest list
                StreamReader sr = null;
                string listpath = @"db\\quest\\questlist.qsd";
                string[] listofquest = new string[500];
                string s = "";
                if (File.Exists(listpath))
                {
                    try 
                    {
                    sr = new StreamReader(listpath);
                    int i = 0;
                    Log.Write(Log.MessageType.debug, "We have created the streamreader");
                    while ((s = sr.ReadLine()) != null)
                    {                        
                           if (!s.StartsWith("//")&& s !="") //if it's not a comment
                            {
                                listofquest[i] = s.Trim();
                                Log.Write(Log.MessageType.debug, "We have find this line : {0}", listofquest[i]);
                            }
                        
                        i++;
                    }
                   
                    
                    }
                    catch {Log.Write(Log.MessageType.error,"Can't open questlist.qsd file");return false;}
                    finally{if (sr != null)sr.Close();}
                }
                else
                { Log.Write(Log.MessageType.error, "Failed to read questlist"); return false; }
                #endregion
                
                foreach (string uri in listofquest)
                {
                    if (uri != "" && uri != null)
                    {
                        
                        Quest quest = new Quest();
                        Lua lua = new Lua();
                        lua.RegisterFunction("AddQuest", quest, quest.GetType().GetMethod("AddQuest"));
                        Log.Write(Log.MessageType.debug, "after register function we will read {0}", uri);
                        lua.DoFile(String.Format(@"db\\quest\\{0}",uri));
                        Log.Write(Log.MessageType.debug, "after lua do file");
                        string name = (string)lua["questName"];
                        double id = (double)lua["questID "];
                        double MinLvl = (double)lua["questMinLvl"];
                        double MaxLvl = (double)lua["questMaxLvl"];
                        double Exp = (double)lua["questExp"];
                        double Penya = (double)lua["questPenya"];
                        quest.questName = name;
                        quest.questID = (int)id;
                        quest.questMinLvl = (int)MinLvl;
                        quest.questMaxLvl = (int)MaxLvl;
                        quest.questExp = (int)Exp;
                        quest.questPenya = (int)Penya;
                        Log.Write(Log.MessageType.debug, "questname : {0} questID : {1} minlvl : {2} maxlvl {3} exp {4} penya {5}", quest.questName, quest.questID, quest.questMinLvl, quest.questMaxLvl, quest.questExp, quest.questPenya);
                        WorldServer.data_quests.Add(quest);

                        iQuestFiles++;
                    }
                    
                }
            }
            catch (Exception e)
            {
                Log.Write(Log.MessageType.error, "Failed to load quest file: {0}\r\n{1}", e.Message, e.StackTrace);
            }
            
            Log.Write(Log.MessageType.load, "Loaded {0} quests", iQuestFiles);
            return true;
        }
        





        
    }
}

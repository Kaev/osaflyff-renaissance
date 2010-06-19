using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using LuaInterface;

/// [Adidishen]
/// Now you don't need to modify using directives. :)

namespace FlyffWorld
{
    public partial class Client
    {
        public void cmdItemTest(string[] c)
        {
            int itemID = -1,
                type = -1,
                val = -1;
            try
            {
                itemID = int.Parse(c[1]);
                type = int.Parse(c[2]);
                val = int.Parse(c[3]);
            }
            catch { return; }
            Slot slot;
            if (type == 0)
                slot = GetSlotByPosition(val);
            else
                slot = GetSlotByID(val);
            CreateItem(new Item() { dwItemID = itemID }, slot);
        }
        public void cmdSetMoverTrace()
        {
            c_target.bIsTraced = !c_target.bIsTraced;
        }
        public void cmdSay(string[] c)
        {
            try
            {
                string msgTo = c[1];
                string msg = StringUtilities.FillString(c, 2);
                if (msg == "")
                    return;
                Client target = WorldHelper.GetClientByPlayerName(msgTo);
                if (target == null)
                {
                    //sendInformationMessage(FlyFF.TID_GAME_NOTLOGIN);
                    SendMessageInfoNotice("The player {0} is not logged in or does not exist.", msgTo);
                    return;
                }
                SendMessagePM(this, target, msg);
            }
            catch { }
        }
        public void cmdLanguage(string[] c)
        {
            try
            {
                int dwLang = int.Parse(c[1]);
                string targetName = c[2];
                Client target;
                if (targetName == null || targetName == "")
                {
                    targetName = c_data.strPlayerName;
                    target = this;
                }
                else
                    target = WorldHelper.GetClientByPlayerName(targetName);
                if (target == null)
                    return;
                if (dwLang < 0 || dwLang > 4)
                    dwLang = 0;
                target.c_data.language = dwLang;
                string lang = "";
                switch (dwLang)
                {
                    case 0: lang = "English"; break;
                    case 1: lang = "French"; break;
                    case 2: lang = "German"; break;
                    case 3: lang = "Spanish"; break;
                    case 4: lang = "Thai"; break;
                    default: lang = "English"; break;
                        SendMessageHud("Language of "+targetName+" is set to "+lang);
                }
            }
            catch { }
        }
            

    }
}

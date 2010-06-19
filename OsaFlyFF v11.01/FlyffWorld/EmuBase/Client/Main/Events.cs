using System;
using System.Collections.Generic;
using System.Text;

namespace FlyffWorld
{
    public partial class Client
    {
        public void OnEquipItems()
        {
            ResetUserAttributes();
        }
        public void OnRemoveItems()
        {
            ResetUserAttributes();
        }
        public void OnFriendsNetworkDisconnect()
        {
            for (int i = 0; i < c_data.friends.Count; i++)
            {
                Friend friend = c_data.friends[i];
                if (friend.bOnline)
                {
                    Client c = WorldHelper.GetClientByPlayerName(friend.strPlayerName);
                    if (c == null)
                        continue;
                    try
                    {
                        for (int l = 0; l < c.c_data.friends.Count; l++)
                        {
                            Friend cfriend = c.c_data.friends[l];
                            if (cfriend.dwCharacterID == c_data.dwCharacterID)
                            {
                                cfriend.bOnline = false;
                                break;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Log.Write(Log.MessageType.error, "onMsnDisconnect(): Exception caught while looping through friends.");
                        Log.Write(Log.MessageType.error, e.Message);
                    }
                }
            }
            SendFriendOnDisconnect();
        }
        public void OnCheckLevelGroup()
        {
            switch (Server.levelup_state)
            {
                case 0:
                    {
                        while (c_data.qwExperience >= ClientDB.EXP[c_data.dwLevel])
                        {
                            c_attributes[FlyFF.DST_HP] = c_data.f_MaxHP;
                            c_attributes[FlyFF.DST_MP] = c_data.f_MaxMP;
                            c_attributes[FlyFF.DST_FP] = c_data.f_MaxFP;
                            c_data.dwLevel++;
                            c_data.qwExperience = c_data.qwExperience - ClientDB.EXP[c_data.dwLevel - 1];
                            if (c_data.dwClass <= 13)
                            {
                                int sp_plus = 2;
                                if (c_data.dwLevel > 20)
                                    sp_plus++;
                                if (c_data.dwLevel > 40)
                                    sp_plus++;
                                if (c_data.dwLevel > 60)
                                    sp_plus++;
                                if (c_data.dwLevel > 80)
                                    sp_plus++;
                                if (c_data.dwLevel > 100)
                                    sp_plus++;
                                c_data.dwSkillPoints += sp_plus;
                            }
                            SendPlayerCombatInfo();
                            c_data.dwStatPoints += 2;
                            SendPlayerStatPoints();
                        }
                    }
                    break;
                case 1:
                    {
                        if (c_data.qwExperience >= ClientDB.EXP[c_data.dwLevel])
                        {
                            c_data.dwLevel++;
                            c_data.qwExperience = c_data.qwExperience - ClientDB.EXP[c_data.dwLevel - 1];
                            if (c_data.dwClass <= 13)
                            {
                                int sp_plus = 2;
                                if (c_data.dwLevel > 20)
                                    sp_plus++;
                                if (c_data.dwLevel > 40)
                                    sp_plus++;
                                if (c_data.dwLevel > 60)
                                    sp_plus++;
                                if (c_data.dwLevel > 80)
                                    sp_plus++;
                                if (c_data.dwLevel > 100)
                                    sp_plus++;
                                c_data.dwSkillPoints += sp_plus;
                            }
                            SendPlayerCombatInfo(); 
                            c_data.dwStatPoints += 2;
                            SendPlayerStatPoints();
                        }
                    }
                    break;
                case 2:
                    {
                        if (c_data.qwExperience >= ClientDB.EXP[c_data.dwLevel])
                        {
                            c_data.dwLevel++;
                            c_data.qwExperience = c_data.qwExperience - ClientDB.EXP[c_data.dwLevel - 1];
                            if (c_data.qwExperience >= ClientDB.EXP[c_data.dwLevel])
                                c_data.qwExperience = ClientDB.EXP[c_data.dwLevel] - 1;
                            if (c_data.dwClass <= 13)
                            {
                                int sp_plus = 2;
                                if (c_data.dwLevel > 20)
                                    sp_plus++;
                                if (c_data.dwLevel > 40)
                                    sp_plus++;
                                if (c_data.dwLevel > 60)
                                    sp_plus++;
                                if (c_data.dwLevel > 80)
                                    sp_plus++;
                                if (c_data.dwLevel > 100)
                                    sp_plus++;
                                c_data.dwSkillPoints += sp_plus;
                            }
                            SendPlayerCombatInfo();
                            c_data.dwStatPoints += 2;
                            SendPlayerStatPoints();
                        }
                    }
                    break;
            }
        }
    }
}

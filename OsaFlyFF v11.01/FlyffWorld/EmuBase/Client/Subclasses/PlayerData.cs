using System;
using System.Collections;
using System.Collections.Generic;
namespace FlyffWorld
{
    public class Friend
    {
        public Friend() { }
        public string strPlayerName = "";
        public int dwCharacterID = 0, dwLevel = 1, nBlocked = 0, dwClass = 0, dwNetworkStatus = 1, dwGender = 0;
        public bool bOnline = false;
    }
    public class Skill
    {
        public int dwSkillID;
        public int dwSkillLevel;
        public Skill()
        {
            dwSkillID = 0;
            dwSkillLevel = 0;
        }
        
    }
    
    public partial class PlayerData
    {
        public List<Slot> inventory = new List<Slot>();
        private Client parent;
        public Lock dataLock = new Lock();
        public List<Skill> skills = new List<Skill>();
        public List<Friend> friends = new List<Friend>();
        public List<Hotslot> hotslots = new List<Hotslot>();
        public List<Keybind> keybinds = new List<Keybind>();
        public List<Buff> buffs = new List<Buff>();
        public List<Mails> receivedmails = new List<Mails>();
        public List<Mails> sentmails = new List<Mails>();
        public List<ActiveItems> activateditem = new List<ActiveItems>();
        //action slot things
        public int[] actionslot = new int[]{0,0,0,0,0};
        public int[] actionslot_option = new int[] { 0, 0, 0, 0, 0 };
        //[Divinepunition]action slot bar have a total of 100% (slot 0:0 - slot 1: 10% - slot 2:20% - slot 3:20% slot 4:50%) when using it we decrease the % 
        public int dwactionslotbar = 100; 

        public PlayerData(Client parent)
        {
            this.parent = parent;
            for (int i = 0; i < 73; i++)
                inventory.Add(new Slot() { dwPos = i, dwID = i, c_item = null });
        }
        public int ItemCount
        {
            get
            {
                int c = 0;
                for (int i = 0; i < 73; i++)
                    if (inventory[i].c_item != null)
                        c++;
                return c;
            }
        }
        public int InventoryItemCount
        {
            get
            {
                int c = 0;
                for (int i = 0; i < 73; i++)
                    if (inventory[i].c_item != null && inventory[i].dwPos < 0x2a)
                        c++;
                return c;
            }
        }
        public int EquipsItemCount
        {
            get
            {
                int c = 0;
                for (int i = 0; i < 73; i++)
                    if (inventory[i].c_item != null && inventory[i].dwPos >= 0x2a)
                        c++;
                return c;
            }
        }
        [LuaMember]
        public string strUsername      = "",
                      strPassword      = "",
                      strPlayerName    = "";
        [LuaMember]
        public FlyffColor c_haircolor = new FlyffColor();
        public Bank bank = new Bank();
        public Bag basebag  = null, 
                   bag1     = null,
                   bag2 = null;
        [LuaMember]
        public Skills tempSkills = null; //temporary variable to stock skill lvl for resurrection for exemple
        [LuaMember]
        
        public int dwAccountID = -1,
                   dwCharSlot = 0,
                   dwCheerPoints = 0,
                   dwNetworkStatus = 0,
                   dwFaceID = 0,
                   dwHairID = 0,
                   dwSkillPoints = 0,
                   dwFlyLevel = 0,
                   dwFlyEXP = 0,
                   dwGender = 0,
                   dwCharacterID = -1,
                   dwReputation = 0,
                   dwKarma = 0,
                   dwDisposition = 0,
                   dwAuthority = 0,
                   dwStatPoints = 0,
                   dwGuildID = 0,
                   dwPenya = 0,
                   dwMapID = 1,
                   dwLanguage = 0,
                   dwPartyID = 0,
                   dwlastdamage, //to be used for allowing reflecthit work save the latest damage from a mob
                   m_MotionFlags = MotionFlags.NONE,
                   m_PlayerFlags = PlayerFlags.NONE,
                   m_MovingFlags = MoveFlags.NONE;
        
        public long m_qwRealExperience = 0;
        public bool isInParty = false;
        public List<DelayedActions.Delaycontent> Delayedactionslist = new List<DelayedActions.Delaycontent>();
        /*say if player is in party. We will need to put it in false when destruct client
                 * and true when login if his charid is in a group
                 * But we need to pu t a timer to delete him from group if he is disconnect since more than 2 min*/
        private int m_dwRealLevel = 0;
        private int m_dwRealClass = -1;
        private void OnJobChange(int dwOldJob, int dwNewJob)
        {
            dwNewJob = Client.LimitNumber(dwNewJob, 0, 32);
            int[] skills = SkillTree.GetTree(dwNewJob);
            {
                int i = 0;
                while (this.skills.Count > 0 && i < this.skills.Count)
                {
                    Skill userskill = this.skills[i];
                    if (!Client.ArrayContains(skills, userskill.dwSkillID))
                    {
                        
                        this.skills.Remove(userskill);
                    }
                    else
                    {
                        i++;
                    }
                }
            }
            for (int j = 0; j < skills.Length; j++)
            {
                if (!SkillExists(skills[j]))
                {
                    Skill newskill = new Skill();
                    newskill.dwSkillID = skills[j];
                    newskill.dwSkillLevel = 0;
                    this.skills.Add(newskill);
                }
            }
        }
        private bool SkillExists(int skillID)
        {
            for (int i = 0; i < skills.Count; i++)
                if (skills[i].dwSkillID == skillID)
                    return true;
            return false;
        }
        
        [LuaMember]
        public int dwClass
        {
            get
            {
                return m_dwRealClass;
            }
            set
            {
                // OnSetClass
                if (m_dwRealClass != -1) // NOT first job assign (during login, ofcourse)
                {
                    OnJobChange(m_dwRealClass, value);
                    m_dwRealClass = value;
                    parent.SendPlayerNewJob();
                }
                else
                    m_dwRealClass = value;
            }
        }
        [LuaMember]
        public int dwLevel
        {
            get
            {
                return m_dwRealLevel;
            }
            set
            {
                // OnSetLevel
                if (value > 120)
                {
                    value = 120;
                    qwExperience = ClientDB.EXP[ClientDB.EXP.Length - 1] - 1;
                }
                if (value < 1)
                    value = 1;
                m_dwRealLevel = value;
            }
        }
        [LuaMember]
        public long qwExperience
        {
            get
            {
                return m_qwRealExperience;
            }
            set
            {
                // OnSetExperiencePoints
                if (dwLevel == 120 && value > ClientDB.EXP[ClientDB.EXP.Length - 1] - 1)
                {
                    value = ClientDB.EXP[ClientDB.EXP.Length - 1] - 1;
                }
                /// [Adidishen]
                /// Fix for exp gain when you're 15 vag, or 60 first job
                /// [Nicco]
                /// Do not raise experience points if the player is a hero because it's useless crap.
                JobState state = Client.GetJobState(dwClass);
                if ((state == JobState.Vagrant && dwLevel >= 15) || (state == JobState.FirstJob && dwLevel >= 60) || state == JobState.Hero || value < 0)
                    value = 0;
                m_qwRealExperience = value;
            }
        }
    }
}

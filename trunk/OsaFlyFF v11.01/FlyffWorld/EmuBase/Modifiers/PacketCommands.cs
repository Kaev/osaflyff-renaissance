using System;
using System.Collections.Generic;
using System.Text;

namespace FlyffWorld
{
    public class PacketCommands : FlyFF
    {

        [LuaMember]
        public const uint    // PACK SEND BY SERVER TO CLIENT CLASSED BY NUMBER
                            PAK_MOVER_CHAT              = 0x00000001,
                            PAK_NEW_ITEM                = 0x00000003,
                            PAK_MOVE_ITEM_INV           = 0x00000004,
                            PAK_MOVE_EQUIP              = 0x00000006,
                            PAK_POS_TELEPORT            = 0x00000010,
                            PAK_UPDATE_LEVELGROUP       = 0x00000012, // StartNewMergedPacket()
                            PAK_NAME_CHANGE             = 0x00000012, // Addint() as packet header
                            PAK_DAMAGE                  = 0x00000013,
                            PAK_NPC_SHOP                = 0x00000014,
                            PAK_UPDATE_ITEM             = 0x00000018,
                            PAK_SKILL_MOTION            = 0x00000019,
                            PAK_NPC_CHATBOX             = 0x00000024,
                            PAK_RESURRECT_OFFER         = 0x00000027,

                            PAK_UPDATE_PVP              = 0x00000040,
                            PAK_INSTANTMOVE             = 0x00000041,//test for skill sneaker by divinepunition
                            PAK_SHOP_OPEN               = 0x00000042,
                            PAK_SHOP_CLOSE              = 0x00000043,
                            PAK_SHOP_ADDITEM            = 0x00000044,
                            PAK_SHOP_SHOWINTERFACE      = 0x00000045,
                            PAK_SHOP_UPDATEQUANTITY     = 0x00000046,
                            PAK_SHOP_REMOVEITEM         = 0x00000047,
                            PAK_UPDATE_HAIR             = 0x00000048,
                            PAK_UNKOWNSKILLRELATED      = 0x00000049,
                            PACK_BANK_ADDITEM           = 0x00000050,
                            PACK_BANK_TAKEITEM          = 0x00000051,
                            PACK_BANK_UPDATEPENYA       = 0x00000052,
                            PACK_BANK_UPDATETEM         = 0x00000054,
                            PACK_BANK_SENDASKPASSWORD   = 0x00000056,
                            PACK_BANK_OPENBANK          = 0x00000058,
                            PAK_STATEEFFECT             = 0x00000059,
                            PAK_WEATHER_NONE            = 0x00000060,
                            PAK_WEATHER_SNOW            = 0x00000061,
                            PAK_WEATHER_RAIN            = 0x00000062,
                            PAK_PARTY_DEFAULT_NAME      = 0x00000068,
                            PAK_FRIEND_ADDNEW           = 0x00000070,
                            PAK_FRIEND_INVITATION       = 0x00000071,
                            PAK_FRIEND_CANCELINV        = 0x00000072, // NotImplemented
                            PAK_FRIEND_GETNAME          = 0x00000073, // NotImplemented
                            PAK_FRIEND_MSNSTATE         = 0x00000074,
                            PAK_FRIEND_DELETE           = 0x00000075,
                            PAK_FRIEND_ADDERROR         = 0x00000076, // NotImplemented
                            PAK_FRIEND_CHANGEJOB        = 0x00000077, // NotImplemented
                            PAK_GAME_JOIN               = 0x00000078,
                            PAK_PARTY_LEADER            = 0x00000079,
                            PAK_UPDATE_PARTY            = 0x00000082,
                            PAK_PARTY_INVIT             = 0x00000083,
                            PAK_PARTY_CANCELINV         = 0x00000084,
                            PAK_HUD_MESSAGE             = 0x00000093,
                            PAK_INFORMATION_MESSAGE     = 0x00000094,
                            PAK_FLYFF_INFORMATION       = 0x00000095,
                            PAK_GAME_TIME               = 0x00000096,
                            PAK_SEND_HOTSLOTS           = 0x00000097,
                            PAK_NPC_CREATERESULT        = 0x00000100,
                            
                            PAK_FOODCREATION_RESULT     = 0x00000117,
                            PAK_CLOSE_CONFIRMWINDOWS    = 0x00000122,
                            PAK_COLLECT_START           = 0x00000123,
                            PAK_COLLECT_STOP            = 0x00000124,
                            PAK_AWAKENING               = 0x00000140,
                            PAK_ADD_FRIENDDATA          = 0x00000141,
                            PAK_RESET_COLLECT_JAUGE     = 0x00000143,

                            PAK_FLYFF_EFFECT            = 0x0000000F,
                            PAK_SKILLEND                = 0x0000001A,
                            PAK_VALID_ITEMUPDATE        = 0x0000001B,
                            PAK_RAISE_ATTRIBUTE         = 0x0000001C,
                            PAK_DECREASE_ATTRIBUTE      = 0x0000001D,
                            PAK_SET_ATTRIBUTE           = 0x0000001E,
                           
                            PAK_GAME_RATES              = 0x0000002E,
                            PAK_SENDCSFOODBUFF          = 0x0000003F,
                            PAK_UNKNOWFOLLOWRELATED     = 0x0000004A,
                            PAK_BUFF                    = 0x0000004C,
                            PAK_UPDATE_FACE             = 0x0000004D,
                            PAK_UNKNOWN_4E              = 0x0000004E,
                            PAK_INSTANTMOVE2            = 0x0000005F,
                            PAK_UPDATE_STATS            = 0x0000006A,
                            PAK_GUILDS_GDSTATE          = 0x0000007A,
                            PAK_UPDATE_SKILLS           = 0x0000007D,
                            PAK_REORDER_PARTY           = 0x0000008D,
                            PAK_GUILDS_SENDINV          = 0x0000009A,
                            PAK_GUILDS_JOIN             = 0x0000009B,
                            PAK_GUILDS_NEW              = 0x0000009C,
                            PAK_GUILDS_DISBAND          = 0x0000009D,
                            PAK_GUILDS_SINGLE           = 0x0000009E,
                            PAK_GUILDS_ALL              = 0x0000009F,

                            PAK_SEND_USAGEINFO          = 0x000000A0,
                            PAK_REVIVE                  = 0x000000A2,
                            PAK_UPDATE_AP               = 0x000000A6,
                            PAK_UPDATE_JOB              = 0x000000A7,
                            PAK_GAMESERVER_SETTINGS     = 0x000000B2,
                            PAK_UDPATE_CHEER            = 0x000000B4,
                            PAK_FLYFF_EVENTMSG          = 0x000000B9,
                            PAK_MOVING_KEYBOARD         = 0x000000CA,
                            PAK_MOVING_MOUSECLICK       = 0x000000C1,
                            PAK_MOVING_FOLLOW           = 0x000000C2,
                            PAK_ACTIONSLOT_GREY         = 0x000000C5,
                            PAK_MOVER_DEATH             = 0x000000C7,

                            PAK_SHOUT                   = 0x000000D0,
                            PAK_MUSIC                   = 0x000000D1,
                            PAK_SOUND                   = 0x000000D2,
                            PAK_PLAYER_FLAGS            = 0x000000D3,
                            PAK_STUN_STATE              = 0x000000D7,
                            PAK_ATTACK_MELEE            = 0x000000E0,
                            PAK_ATTACK_MOTION           = 0x000000E3,
                            PAK_MAIL_READMAIL           = 0x000000E7,
                            PAK_MAIL_SHOWLIST           = 0x000000E9,
                            PAK_RES_SCREEN_REMOVAL      = 0x000000EB,
                            PAK_MOVER_SPAWN             = 0x000000F0,
                            PAK_MOVER_DESPAWN           = 0x000000F1,
                            PAK_WORLD_TELEPORT          = 0x000000F2,
                            PAK_NEW_MAILS_MESSAGE       = 0x000000F7,
                            PAK_PARTY_ITEMDISTRIB       = 0x000000F8,
                            PAK_REMOVE_ACTIVATEDITEM    = 0x000000F8,
                            PAK_GUILDS_SETLOGO          = 0x000000FB,
                            PAK_GUILDS_NOTICE           = 0x000000FD,
                            PAK_GUILDS_AUTHORITY        = 0x000000FE,
                            PAK_GUILDS_SALARY           = 0x000000FF,

                            PAK_CHAT_SAY                = 0x00FF00E0,
                            PAK_SEND_NOTICE             = 0x00FF00EA,

                            PAK_GUILDS_SETNAME          = 0xF000B032,
                            PAK_GUILDS_GDREQUEST        = 0xF000B036,
                            PAK_GUILDS_GDSTART          = 0xF000B037,
                            PAK_GUILDS_GDRESULT         = 0xF000B046,
                            PAK_GUILDS_GDGIVEUP         = 0xF000B047,
                            PAK_GUILDS_GDTRUCEREQ       = 0xF000B048,
                            PAK_GUILDS_NEWMEMBER        = 0xFFFFFF33,
                            PAK_GUILDS_DELMEMBER        = 0xFFFFFF34,
                            PAK_GUILDS_SETRANK          = 0xFFFFFF3A,
                            PAK_FRIEND_USERDATA         = 0xFFFFFF64,
                            PAK_FRIEND_LOGIN            = 0xFFFFFF65,
                            PAK_FRIEND_DISCONNECT       = 0xFFFFFF66,
                            PAK_FRIEND_STATUSCHANGE     = 0xFFFFFF67,
                            PAK_FRIEND_BLOCK            = 0xFFFFFF68,
                            PAK_FRIEND_UNBLOCK          = 0xFFFFFF69,
                            PAK_OFFLINE_BOX             = 0xFFFFFF6C,
                            PAK_GUILDS_SETCLASS         = 0xFFFFFF74;


        [LuaMember]
        public const int    // Event modifiers
                            EVENT_FINISH_MOD            = 0x00000000,
                            EVENT_START_MOD             = 0x00000001,
                            EVENT_GOING_ON_MOD          = 0x00000002;
        [LuaMember]
        public const int    // Follow distance
                            RANGE_MELEE                 = 0x00000000,
                            RANGE_SHORT                 = 0x41200000,
                            RANGE_AVERAGE               = 0x41700000;
        [LuaMember]
        public const int    //Buff effect on mover
                            BUFF_CHANGESTATE            = 0x00000002,
                            BUFF_CHANGEELEMENTWEAK      = 0x00000003;
        [LuaMember]
        public const int    // MESSAGE BOX when create something with NPC
                            MESSAGEBOX_CREATEWEAPON      = 0x00000001,
                            MESSAGEBOX_CREATEJEWEL       = 0x00000003;
        [LuaMember]
        public const int    // order of skill in action slot 0:no actionslot/1:first skill/ 2:the others
                            ACTIONSLOT_ORDER_NO          = 0x00000000,
                            ACTIONSLOT_ORDER_1           = 0x00000001,
                            ACTIONSLOT_ORDER_OTHER       = 0x00000002;
                                    

        [LuaMember]
        public const int    // Type of skill used in action slot
                            UNKNOWN_MOTIONVALUE0         = 0x00000000,
                            UNKNOWN_MOTIONVALUE1         = 0x00000001,
                            UNKNOWN_MOTIONVALUE3         = 0x00000003,
                            UNKNOWN_MOTIONVALUE7         = 0x00000007;
        [LuaMember]
        public const int    // Item updating modifiers
                            ITEM_MODTYPE_QUANTITY        = 0x00000000,
                            ITEM_MODTYPE_CHARGE          = 0x00000001,
                            ITEM_MODTYPE_REFINE          = 0x00000003,
                            ITEM_MODTYPE_EREFINE         = 0x00000004,
                            ITEM_MODTYPE_ELEMENT         = 0x00000005,
                            ITEM_MODTYPE_SOCKETS         = 0x00000006,
                            ITEM_MODTYPE_CARD            = 0x00000007,
                            ITEM_MODTYPE_TICKET          = 0x0000000B;
        [LuaMember]
        public const int    // NPC chat box modifiers
                            NPC_CHATBOX_OPENBOX          = 0x0000001D,
                            NPC_CHATBOX_ADDTXTSCRN       = 0x00000012,
                            NPC_CHATBOX_ADDLINKLBL       = 0x00000010,
                            NPC_CHATBOX_ADDBUTTON        = 0x00000013,
                            NPC_CHATBOX_CLOSE            = 0x00000016;
        [LuaMember]
        public const int    // Mail chat box modifiers
                            MAIL_SUPPRESS                = 0x00000000,
                            MAIL_READ_TAKEOBJECT         = 0x00000001,
                            MAIL_READ_MONEY              = 0x00000002,
                            MAIL_READ_TAKENOTHING        = 0x00000003;
                             
        [LuaMember]
        public const uint    // PACKET RECEIVED BY SERVER FROM CLIENT--------------------

                            RECV_CHANGE_NAME            = 0x00000012,
                            RECV_TIME_SINCECHARCREATE   = 0x00000014,
                            RECV_MAIL_READ              = 0x00000024,
                            
                            RECV_MAIL_SEND              = 0x0000001A,
                            RECV_MAIL_DELETE            = 0x0000001B,
                            RECV_MAIL_TAKEOBJECT        = 0x0000001C,
                            RECV_MAIL_SHOWLIST          = 0x0000001D,
                            RECV_MAIL_TAKEMONEY         = 0x0000001F,
                            
                            RECV_CHANGE_JOB             = 0x00000F32,                            
                            
                            RECV_SELFSPAWN_REQUEST      = 0x0000FF00,
                            
                            RECV_SKILLS_UPDATE          = 0x000F0003,

                            RECV_PLAYER_CHAT            = 0x00FF0000,
                            RECV_MOVE_ITEM_INV          = 0x00FF0006,
                            RECV_REMOVE_ITEM            = 0x00FF000B,
                            RECV_ATTACK                 = 0x00FF0010,
                            RECV_ATTACK_WAND            = 0x00FF0011,
                            RECV_ATTACK_BOW             = 0x00FF0012,
                            RECV_DELETE_ITEM            = 0x00FF0019,
                            RECV_EQUIP_ITEM             = 0x00FF0021,
                            RECV_CAST_SKILL             = 0x00FF0020,
                            RECV_TARGET_INFORMATION     = 0x00FF0023,
                            RECV_MAGE_TELEPORTDESTINY   = 0x00FF0025,

                            RECV_SHOP_OPEN              = 0x00FF00A9,
                            RECV_SHOP_CLOSE             = 0x00FF00AA,
                            RECV_SHOP_ADDITEM           = 0x00FF00AB,
                            RECV_SHOP_VIEW              = 0x00FF00AC,
                            RECV_SHOP_BUYITEM           = 0x00FF00AD,
                            RECV_SHOP_REMOVEITEM        = 0x00FF00AE,
                            RECV_HAIR_DESIGNER          = 0x00FF00AF,
                            RECV_NPC_WANNACHAT          = 0x00FF00B0,
                            RECV_NPC_WANNATRADE         = 0x00FF00B1,
                            RECV_CLOSE_NPCSHOP          = 0x00FF00B2,
                            RECV_BUY_NPCSHOP            = 0x00FF00B3,
                            RECV_SELL_NPCSHOP           = 0x00FF00B4,
                            RECV_RESURRECT_ORIGINAL     = 0x00FF00C0,
                            RECV_RESURRECT_LODE         = 0x00FF00C1,
                            RECV_MAKEUP_ARTIST          = 0x00FF00EE,
                            
                            RECV_SCROLL_AWAKE           = 0x70000004,
                            RECV_REVERT_ITEM            = 0x70000005,
                            RECV_AWAKENING_REQUEST      = 0x70000008,
                            RECV_TELEPORT_ARENA         = 0x70000010,
                            RECV_TELEPORT_EXITARENA     = 0x70000011,                            

                            RECV_GUILDS_SETLOGO         = 0xF000B010,
                            RECV_GUILDS_NOTICE          = 0xF000B012,
                            RECV_ITEM_ON_ITEM           = 0xF000B024,
                            RECV_SOCKET_ADDCARD         = 0xF000B025,
                            RECV_GUILDS_SETTITLES       = 0xF000B026,
                            RECV_GUILDS_SETSALARY       = 0xF000B027,
                            RECV_GUILDS_SETNAME         = 0xF000B032,
                            RECV_GUILDS_GDREQUEST       = 0xF000B036,
                            RECV_GUILDS_GDACCEPT        = 0xF000B037,
                            RECV_GUILDS_GDGIVEUP        = 0xF000B047,
                            RECV_GUILDS_GDTRUCEINV      = 0xF000B048,
                            RECV_GUILDS_GDTRUCE         = 0xF000B049,

                            RECV_ELEMENT_REMOVAL        = 0xF000D00B,

                            RECV_NPC_MAKESHINESTONE     = 0xF000F110,
                            RECV_NPC_MAKEJEWEL          = 0xF000F111,
                            RECV_NPC_MAKEUNIQUEWEAPON   = 0xF000F112,
                            RECV_STATS_UPDATE           = 0xF000F501,
                            RECV_CREATEFOOD             = 0xF000F605,
                            PAK_GUILDSIEGE_APPLY        = 0xF000F700,
                            PAK_GUILDSIEGE_CHECKSTATUT  = 0xF000F701,
                            PAK_GUILDSIEGE_CANCEL       = 0xF000F703,
                            PAK_GUILDSIEGE_MEMBERLIST   = 0xF000F705,
                            PAK_GUILDSIEGE_ENTER        = 0xF000F708,
                            PAK_GUILDSIEGE_APFEE        = 0xF000F709,

                            RECV_COLLECT_START          = 0xF000F800,
                            RECV_COLLECT_STOP           = 0xF000F801,
                            PAK_GUILDSIEGE_CONFIRMENTER = 0xF000F802,
                            RECV_REMOVE_CARD            = 0xF000F809,
                            RECV_NPC_BUFFPANG           = 0xF000F813,                             

                            RECV_CLICK_MOVE             = 0xFFFFFF00,
                            RECV_WASD_MOVE              = 0xFFFFFF01,
                            RECV_MOVEMENT_CORRECT       = 0xFFFFFF05,
                            RECV_MOVEMENT_FOLLOW        = 0xFFFFFF07,
                            RECV_PARTY_ACCEPT           = 0xFFFFFF11,
                            RECV_PARTY_KICK             = 0xFFFFFF12,
                            RECV_PARTY_INVIT            = 0xFFFFFF17,
                            RECV_PARTY_INVNO            = 0xFFFFFF18,
                            RECV_PARTY_ITEMDIST         = 0xFFFFFF20,
                            RECV_PARTY_GIVELEAD         = 0xFFFFFF2F,
                            RECV_GUILDS_SETRANK         = 0xFFFFFF3A,
                            RECV_GUILDS_DISBAND         = 0xFFFFFF32,
                            RECV_GUILDS_ACCEPTINV       = 0xFFFFFF33,
                            RECV_GUILDS_LEAVE           = 0xFFFFFF34,
                            RECV_GUILDS_INVITE          = 0xFFFFFF35,
                            RECV_BANK_MENU              = 0xFFFFFF40,
                            RECV_BANK_CLOSE             = 0xFFFFFF41,//not implemented but useless
                            RECV_BANK_ADDITEM           = 0xFFFFFF42,
                            RECV_BANK_ADDPENYA          = 0xFFFFFF43,
                            RECV_BANK_TAKEITEM          = 0xFFFFFF44,
                            RECV_BANK_TAKEPENYA         = 0xFFFFFF45,
                            RECV_BANK_NEWPASS           = 0xFFFFFF47,
                            RECV_BANK_PASS              = 0xFFFFFF48,
                            RECV_FRIEND_INVCONFIRM      = 0xFFFFFF60,
                            RECV_FRIEND_ADDBYMENU       = 0xFFFFFF61,
                            RECV_FRIEND_DECLINE         = 0xFFFFFF62,
                            RECV_FRIEND_USERDATA        = 0xFFFFFF64,
                            RECV_MSN_STATUSCHANGE       = 0xFFFFFF67,
                            RECV_FRIEND_BLOCK           = 0xFFFFFF68,
                            RECV_GUILDS_SETCLASS        = 0xFFFFFF74,
                            RECV_RESURRECT_YES          = 0xFFFFFF78,
                            RECV_RESURECT_NO            = 0xFFFFFF79,
                            RECV_DELETE_HOTSLOT         = 0xFFFFFF0A,
                            RECV_NEW_HOTSLOT            = 0xFFFFFF0B,
                            RECV_NEW_KEYBIND            = 0xFFFFFF0C,
                            RECV_DELETE_KEYBIND         = 0xFFFFFF0D,
                            RECV_UPDATE_ACTIONSLOT      = 0xFFFFFF0E,
                            RECV_FRIEND_DELETE          = 0xFFFFFF6A,
                            RECV_FRIEND_ADDNEW          = 0xFFFFFF6B;






        [LuaMember]
        public const byte NULL = 0x00;

    }
}
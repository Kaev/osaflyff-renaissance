using System;
using System.Collections.Generic;
using System.Text;

namespace FlyffWorld
{
    public enum AttackFlags // needs verification and moficiation!
    {
        NORMAL = 1,
        MISS = 2,
        NORMAL_SKILL = 0x00000010,
        MAGIC = 0x00000020,
        BOW = 0x00004001,
        CRITICAL = 0x00000296,
        KNOCKBACK = 0x10000000
    }
    public class MoveFlags
    {
        public const int    NONE        = 0x00000000,
                            STANDING    = 0x00000001,
                            SITTING     = 0x00000003,
                            MOVEFORWARD = 0x00000004,
                            MOVEBACK    = 0x00000005,
                            TURNLEFT    = 0x00000100,
                            TURNRIGHT   = 0x00000200,
                            SPINUP      = 0x00000400,
                            SPINDOWN    = 0x00000800,
                            JUMP        = 0x00006000,
                            FALL        = 0x00007000,
                            MELEE_1     = 0x00010000,
                            MELEE_2     = 0x00020000,
                            MELEE_3     = 0x00030000,
                            MELEE_4     = 0x00040000,
                            WANDATTACK  = 0x00050000,
                            BOWATTACK   = 0x00060000,
                            SKILL       = 0x00090000,
                            WANDSKILL1  = 0x000B0000,
                            WANDSKILL2  = 0x000D0000,
                            FLY_KB      = 0x02000000, // Not sure
                            NORMAL_KB   = 0x60000000,
                            DEAD        = 0x08000000;
    }
    public class MotionFlags
    {
        public const int    NONE         =   0x00000000,
                            COMBATSTANCE =   0x00000001,
                            WALKING      =   0x00000002,
                            SITTING      =   0x00000004,
                            FLYING       =   0x00000008,
                            FLYINGNOMOVE =   0x00000009, //on a broom
                            FLYMOVING    =   0x00000010,
                            MOTIONING    =   0x00000020,
                            ACCELERATING =   0x00000080; // Using accelerator while flying
    }
    public class PlayerFlags
    {
        public const int    NONE        =   0x00000000,
                            INVINCIBLE  =   0x00000001,
                            INVISIBLE   =   0x00000002,
                            ONEHITKILL  =   0x00000004,
                            FROZEN      =   0x00000008,
                            INVINCIBLE2 =   0x00000020,
                            PK          =   0x00001000,
                            DISALLOWPVP =   0x00002000,
                            NEWMAIL     =   0x00009000,
                            DISABLEVIEW =   0x00020000,
                            SHOWWELCOME =   0x10000000;

    }
    public class StateFlags
    {
        public const int CHS_NORMAL = 0x00000000,
                            CHS_GUARDARROW = 0x00000001,
                            CHS_GUARDBULLET = 0x00000002,
                            CHS_GROGGY = 0x00000004,
                            CHS_STUN = 0x00000008,
                            CHS_ANOMY = 0x00000010,
                            CHS_STARVE = 0x00000020,
                            CHS_PLASYARM = 0x00000040,
                            CHS_MISSING = 0x00000080,
                            CHS_DARK = 0x00000100,
                            CHS_LITHOSKIN = 0x00000200,
                            CHS_INVISIBILITY = 0x00000400,
                            CHS_POISON = 0x00000800,
                            CHS_SLOW = 0x00001000,
                            CHS_DMGREFLECT = 0x00002000,
                            CHS_DOUBLE = 0x00004000,
                            CHS_BLEEDING = 0x00008000,
                            CHS_SILENT = 0x00010000,
                            CHS_DMG_COUNTERATTACK = 0x00020000,
                            CHS_ATK_COUNTERATTACK = 0x00040000,
                            CHS_LOOT = 0x00080000,
                            CHS_SETSTONE = 0x00100000,
                            CHS_DEBUFFALL = 0x00200000,
                            CHS_DARK_POISON = 0x00000900,
                            CHS_DARK_POISON_STUN = 0x00000908,
                            CHS_LOOT_SLOW = 0x00081000,
                            CHS_DARK_POISON_STUN_BLEEDING = 0x00008908,
                            CHS_DARK_POISON_STUN_BLEEDING_DEBUFFALL = 0x0031d908;
    }
}

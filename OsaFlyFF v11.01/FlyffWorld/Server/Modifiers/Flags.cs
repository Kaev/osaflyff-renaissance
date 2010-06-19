using System;
using System.Collections.Generic;
using System.Text;

namespace FlyffWorld
{
    public enum AttackFlags // needs verification and moficiation!
    {
        NORMAL = 1,
        MISS = 2,
        CRITICAL = 128,
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
}

using System;
using System.Collections.Generic;
using System.Text;

namespace FlyffWorld
{
    public partial class PlayerData
    {
        public int f_MaxHP
        {
            get
            {
                double a = dwLevel * ClientDB.MHP[dwClass] * 0.5;
                double b = (dwLevel + 1) * 0.25;
                double e = (parent.c_attributes[FlyFF.DST_STA] * 0.02 + 1.0) * a * b + parent.c_attributes[FlyFF.DST_STA] * 10.0 + 80.0;
                return (int)Math.Floor((Math.Floor(e) + parent.c_attributes[FlyFF.DST_HP_MAX] + parent.c_attributes[FlyFF.DST_HPDMG_UP]) * (1 + ((double)parent.c_attributes[FlyFF.DST_HP_MAX_RATE] / 100d)));
            }
        }
        public int f_MaxMP
        {
            get
            {
                double a = ((dwLevel << 1) + parent.c_attributes[FlyFF.DST_INT] * 8) * ClientDB.MMP[dwClass] + 22 + parent.c_attributes[FlyFF.DST_INT] * ClientDB.MMP[dwClass];
                return (int)Math.Floor((Math.Floor(a) + parent.c_attributes[FlyFF.DST_MP_MAX]) * (1 + ((double)parent.c_attributes[FlyFF.DST_MP_MAX_RATE] / 100d)));
            }
        }
        public int f_MaxFP
        {
            get
            {
                double a = ((dwLevel << 1) + parent.c_attributes[FlyFF.DST_STA] * 6) * ClientDB.MFP[dwClass] + parent.c_attributes[FlyFF.DST_STA] * ClientDB.MFP[dwClass];
                return (int)Math.Floor((Math.Floor(a) + parent.c_attributes[FlyFF.DST_FP_MAX]) * (1 + ((double)parent.c_attributes[FlyFF.DST_FP_MAX_RATE] / 100d)));
            }
        }
        public float f_RecoveryFactor
        {
            get
            {
                return 1f;
            }
        }
        public int f_MovingSpeed
        {
            get
            {
                // TODO:
                return 0;
            }
        }
        public int f_MinDefense
        {
            get
            {
                // TODO: armors
                float def = (float)(Math.Floor(((float)parent.c_attributes[FlyFF.DST_STA] / 2.0f + (float)dwLevel * 2f) * 0.3571429f) - 4f + Math.Floor((float)(parent.c_attributes[FlyFF.DST_STA] - 14) * (float)ClientDB.DEF[dwClass]));
                def += (float)parent.c_attributes[FlyFF.DST_ADJDEF] + (float)parent.c_attributes[FlyFF.DST_DEFHITRATE_DOWN];
                def += (float)(Math.Floor((float)def * ((float)parent.c_attributes[FlyFF.DST_ADJDEF_RATE] / 100)));
                return (int)Math.Max(def, 0f);
            }
        }
        public int f_MaxDefense
        {
            get
            {
                // TODO: armors
                float def = (float)(Math.Floor(((float)parent.c_attributes[FlyFF.DST_STA] / 2f + (float)dwLevel * 2f) * 0.3571429f) - 4f + Math.Floor((float)(parent.c_attributes[FlyFF.DST_STA] - 1) * ClientDB.DEF[dwClass]));
                def += (float)parent.c_attributes[FlyFF.DST_ADJDEF] + (float)parent.c_attributes[FlyFF.DST_DEFHITRATE_DOWN];
                def += (float)(Math.Floor((float)def * ((float)parent.c_attributes[FlyFF.DST_ADJDEF_RATE] / 100)));
                return (int)Math.Max(def, 0f);
            }
        }
        public int f_RecoveryHP
        {
            get
            {
                double a = f_MaxHP / (dwLevel * 500);
                double b = parent.c_attributes[FlyFF.DST_STA] * ClientDB.HPR[dwClass] + a;
                double c = dwLevel * 0.333333 + b;
                return (int)Math.Floor(Math.Floor(c) + parent.c_attributes[FlyFF.DST_HP_RECOVERY]);
            }
        }
        public int f_RecoveryMP
        {
            get
            {
                double a = f_MaxMP / (dwLevel * 500);
                double b = parent.c_attributes[FlyFF.DST_INT] * ClientDB.MPR[dwClass] + a;
                double c = (dwLevel * 1.5d + b) * 0.2;
                return (int)Math.Floor(Math.Floor(c)+parent.c_attributes[FlyFF.DST_MP_RECOVERY]);
            }
        }
        public int f_RecoveryFP
        {
            get
            {
                double a = f_MaxFP / (dwLevel * 500);
                double b = parent.c_attributes[FlyFF.DST_STA] * ClientDB.FPR[dwClass] + a;
                double c = ((dwLevel << 1) + b) * 0.2;
                return (int)Math.Floor(Math.Floor(c) + parent.c_attributes[FlyFF.DST_FP_RECOVERY]);
            }
        }
        public int f_PercentHP
        {
            get
            {
                return (int)Math.Floor((double)(parent.c_attributes[FlyFF.DST_HP] * 100) / (double)f_MaxHP);
            }
        }
        public int f_PercentMP
        {
            get
            {
                return (int)Math.Floor((double)(parent.c_attributes[FlyFF.DST_MP] * 100) / (double)f_MaxMP);
            }
        }
        public int f_PercentFP
        {
            get
            {
                return (int)Math.Floor((double)(parent.c_attributes[FlyFF.DST_FP] * 100) / (double)f_MaxFP);
            }
        }
        public int f_CritRate
        {
            get
            {
                double crit = Math.Floor((double)(parent.c_attributes[FlyFF.DST_DEX]) / 10 * ClientDB.CRIT[dwClass]) + (double)parent.c_attributes[FlyFF.DST_CHR_CHANCECRITICAL];
                return (int)Math.Min(crit, 100d);
            }
        }
        public int f_BlockRate
        {
            get
            {
                // TODO: block rate modifiers
                double block = Math.Floor((double)(ClientDB.BLOCK[dwClass] * parent.c_attributes[FlyFF.DST_DEX] * dwLevel * 0.002d));
                return (int)Math.Min(block, 100);
            }
        }
        public double f_RateModifierEXP
        {
            get
            {
                double baseRate = 0d, rateMul = 1d;
                if (dwClass > 13)
                    baseRate = 0.5d;
                else
                    baseRate = 1d;
                // if II_SYS_SYS_SCR_AMPESS exists rateMul+=0.5d
                // if II_SYS_SYS_SCR_AMPESS1 exists rateMul +=0.5d
                // if II_SYS_SYS_SCR_AMPESS2 exists rateMul+=0.5d
                // if II_SYS_SYS_SCR_AMPEM exists rateMul+=1d
                return baseRate * rateMul;
            }
        }
        public double f_RateModifierDrop
        {
            get
            {
                // Change this "formula" if you want to
                return 1;
            }
        }
        public int f_DamageMin
        {
            get
            {
                Slot slot = parent.GetSlotByPosition(52);
                if (slot == null)
                {
                    return 0; // correct this
                }
                int dwBaseAtk = slot.c_item.Data.min_ability;
                int nWeaponKind = slot.c_item.Data.itemkind[2];
                int dwAdjAtk = parent.c_attributes[FlyFF.DST_ADJ_HITRATE];
                int dwWeaponRefine = slot.c_item.dwRefine;
                return (int)Math.Floor(((dwBaseAtk << 1) + GetWeaponAttack(nWeaponKind) + dwAdjAtk) * GetRefineMultiplier(dwWeaponRefine) + Math.Pow(dwWeaponRefine, 1.5));
            }
        }
        public int f_DamageMax
        {
            get
            {
                Slot slot = parent.GetSlotByPosition(52);
                if (slot == null)
                {
                    return 0; // correct this
                }
                int dwBaseAtk = slot.c_item.Data.min_ability;
                int nWeaponKind = slot.c_item.Data.itemkind[2];
                int dwAdjAtk = parent.c_attributes[FlyFF.DST_ADJ_HITRATE];
                int dwWeaponRefine = slot.c_item.dwRefine;
                return (int)Math.Floor(((dwBaseAtk << 1) + GetWeaponAttack(nWeaponKind) + dwAdjAtk) * GetRefineMultiplier(dwWeaponRefine) + Math.Pow(dwWeaponRefine, 1.5));
            }
        }
        private double GetWeaponAttack(int nWeaponKind)
        {
            switch (nWeaponKind)
            {
                case FlyffItemkind.IK3_SWD:
                case FlyffItemkind.IK3_THSWD:
                    return (parent.c_attributes[FlyFF.DST_STR] - 12) * 4.5d + dwLevel * 1.1d;
                case FlyffItemkind.IK3_AXE:
                case FlyffItemkind.IK3_THAXE:
                    return (parent.c_attributes[FlyFF.DST_STR] - 12) * 5.5d + dwLevel * 1.2d;
                case FlyffItemkind.IK3_STAFF:
                    return (parent.c_attributes[FlyFF.DST_STR] - 10) * 0.8d + dwLevel * 1.1d;
                case FlyffItemkind.IK3_CHEERSTICK:
                    return (parent.c_attributes[FlyFF.DST_STR] - 10) * 3.0d + dwLevel * 1.3d;
                case FlyffItemkind.IK3_KNUCKLEHAMMER:
                    return (parent.c_attributes[FlyFF.DST_STR] - 10) * 5.0d + dwLevel * 1.2d;
                case FlyffItemkind.IK3_WAND:
                    return (parent.c_attributes[FlyFF.DST_INT] - 10) * 6.0d + dwLevel * 1.2d;
                case FlyffItemkind.IK3_YOYO:
                    return (parent.c_attributes[FlyFF.DST_STR] - 12) * 4.2d + dwLevel * 1.1d;
                case FlyffItemkind.IK3_YOBO:
                    return ((parent.c_attributes[FlyFF.DST_DEX] - 14) * 4.0d + parent.c_attributes[FlyFF.DST_STR] * 0.2 + dwLevel * 1.3d) * 0.7d;
                case -1:
                    return 1;
                default:
                    Log.Write(Log.MessageType.warning, "PlayerData::GetWeaponAttack(): unknown nWeaponKind {0}", nWeaponKind);
                    return 1;
            }
        }
        private double GetRefineMultiplier(int dwWeaponRefine)
        {
            int[] refineTable = new int[] { 0, 2, 4, 6, 8, 10, 13, 16, 19, 21, 24 };
            try
            {
                return (refineTable[dwWeaponRefine] + 100) * 0.01;
            }
            catch (Exception)
            {
                return 1;
            }
        }
        public int f_HitRate(Monster mob)
        {
            Slot slot = parent.GetSlotByPosition(52);
            if (slot == null || slot.c_item == null)
                return 20;
            MonsterData mobdata = mob.Data;
            ItemData item = slot.c_item.Data;
            if (mobdata == null)
            {
                return 20;
            }
            double hit = 0;
            if (item != null && item.itemkind[2] == FlyFF.IK3_WAND)
                hit = (double)parent.c_attributes[FlyFF.DST_INT];
            else
                hit = (double)parent.c_attributes[FlyFF.DST_DEX];
            double a = hit + mobdata.mobFlee;
            double b = dwLevel + mobdata.mobLevel;
            int c = (int)(Math.Floor(Math.Floor((hit * 1.6 / a) * (dwLevel * 1.2 / b) * 150d) + parent.c_attributes[FlyFF.DST_ADJ_HITRATE]));
            c += parent.c_attributes[FlyFF.DST_DEFHITRATE_DOWN];
            c = Math.Max(c, 20);
            return Math.Min(c, 96);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace FlyffWorld
{
    public class DamageFormulas
    {
        public static double GetDamage(int dwPlayerLevel, int dwStrength, int dwStamina, int dwDexterity, int dwIntelligence, int nWeaponKind, int dwBaseAtk, int dwWeaponRefine, int dwAdjAtk)
        {
            return Math.Floor(((dwBaseAtk << 1) + GetWeaponAttack(dwPlayerLevel, dwStrength, dwStamina, dwDexterity, dwIntelligence, nWeaponKind) + dwAdjAtk) * GetRefineMultiplier(dwWeaponRefine) + Math.Pow(dwWeaponRefine, 1.5));
        }
        private static double GetWeaponAttack(int dwPlayerLevel, int dwStrength, int dwStamina, int dwDexterity, int dwIntelligence, int nWeaponKind)
        {
            switch (nWeaponKind)
            {
                case FlyffItemkind.IK3_SWD:
                case FlyffItemkind.IK3_THSWD:
                    return (dwStrength - 12) * 4.5d + dwPlayerLevel * 1.1d;
                case FlyffItemkind.IK3_AXE:
                case FlyffItemkind.IK3_THAXE:
                    return (dwStrength - 12) * 5.5d + dwPlayerLevel * 1.2d;
                case FlyffItemkind.IK3_STAFF:
                    return (dwStrength - 10) * 0.8d + dwPlayerLevel * 1.1d;
                case FlyffItemkind.IK3_CHEERSTICK:
                    return (dwStrength - 10) * 3.0d + dwPlayerLevel * 1.3d;
                case FlyffItemkind.IK3_KNUCKLEHAMMER:
                    return (dwStrength - 10) * 5.0d + dwPlayerLevel * 1.2d;
                case FlyffItemkind.IK3_WAND:
                    return (dwIntelligence - 10) * 6.0d + dwPlayerLevel * 1.2d;
                case FlyffItemkind.IK3_YOYO:
                    return (dwStrength - 12) * 4.2d + dwPlayerLevel * 1.1d;
                case FlyffItemkind.IK3_YOBO:
                    return ((dwDexterity - 14) * 4.0d + dwStrength * 0.2 + dwPlayerLevel * 1.3d) * 0.7d;
                case -1:
                    return 1;
                default:
                    Log.Write(Log.MessageType.warning, "DamageFormulas::GetWeaponAttack(): unknown nWeaponKind {0}", nWeaponKind);
                    return 1;
            }
        }
        private static double GetRefineMultiplier(int dwWeaponRefine)
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
    }
}

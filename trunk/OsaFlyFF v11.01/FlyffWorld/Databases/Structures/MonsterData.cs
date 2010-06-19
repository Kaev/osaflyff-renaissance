using System;
using System.Collections.Generic;
using System.Text;

namespace FlyffWorld
{
    public class MonsterData
    {
        public int mobID = 0, mobSize = 100, mobStr = 0, mobSta = 0, mobDex = 0, mobInt = 0, mobLevel = 1, mobFlee = 0, mobAspd = 0, mobHP = 1, mobMp = 100, mobElement = 0, mobElementPower = 0, mobExpPoints = 0;
        public int mobCash = 0; // Nicco->Drops
        public int mobResistMagic = 0; //Divine for skills
        public float[] mobResistance = new float[] { 0, 0, 0, 0, 0 };
        public double mobMoveSpeed = 0;
        public bool isFlying = false, isKillable = true;
        public string mobName = "";
        // ADDED

        public long lastAtkDate = 0;
        public int atkDelay = 0;
        public int[] attacks = new int[] { 0, 0, 0 };
        public int currentAtk = 0;
        public bool effectSent = true;

        public int atkMin = 0;
        public int atkMax = 0;
        public int def = 0;
    }
}

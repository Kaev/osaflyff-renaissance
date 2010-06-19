using System;
using System.Collections.Generic;
using System.Text;

namespace FlyffWorld
{
    class ClientDB
    {
        public const int // JOBS
            JOB_VAGRANT = 0,
            JOB_MERCENARY = 1,
            JOB_ACROBAT = 2,
            JOB_ASSIST = 3,
            JOB_MAGICIAN = 4,
            JOB_PUPPETEER = 5,
            JOB_KNIGHT = 6,
            JOB_BLADE = 7,
            JOB_JESTER = 8,
            JOB_RANGER = 9,
            JOB_RINGMASTER = 10,
            JOB_BILLPOSTER = 11,
            JOB_PSYKEEPER = 12,
            JOB_ELEMENTOR = 13,
            JOB_GATEKEEPER = 14,
            JOB_DOPPLER = 15,
            JOB_KNIGHT_MASTER = 16,
            JOB_BLADE_MASTER = 17,
            JOB_JESTER_MASTER = 18,
            JOB_RANGER_MASTER = 19,
            JOB_RINGMASTER_MASTER = 20,
            JOB_BILLPOSTER_MASTER = 21,
            JOB_PSYKEEPER_MASTER = 22,
            JOB_ELEMENTOR_MASTER = 23,
            JOB_KNIGHT_HERO = 24,
            JOB_BLADE_HERO = 25,
            JOB_JESTER_HERO = 26,
            JOB_RANGER_HERO = 27,
            JOB_RINGMASTER_HERO = 28,
            JOB_BILLPOSTER_HERO = 29,
            JOB_PSYKEEPER_HERO = 30,
            JOB_ELEMENTOR_HERO = 31;

        public static readonly float[] MHP = new float[]
        {
            0.9f, 1.5f, 1.4f, 1.4f,
            1.4f, 1.6f, 2.0f, 1.6f,
            1.6f, 1.6f, 1.6f, 1.8f,
            1.5f, 1.5f, 0.7f, 0.7f,
            2.0f, 1.6f, 1.6f, 1.6f,
            1.6f, 1.8f, 1.5f, 1.5f,
            2.0f, 1.6f, 1.6f, 1.6f,
            1.6f, 1.8f, 1.5f, 1.5f
        };
        public static readonly float[] MMP = new float[]
        {
            0.3f, 0.5f, 0.5f, 1.3f,
            1.7f, 0.5f, 0.6f, 0.6f,
            0.5f, 0.5f, 1.8f, 1.0f,
            2.0f, 2.0f, 1.0f, 0.5f,
            0.6f, 0.6f, 0.5f, 0.5f,
            1.8f, 1.0f, 2.0f, 2.0f,
            0.6f, 0.6f, 0.5f, 0.5f,
            1.8f, 1.0f, 2.0f, 2.0f
        };
        public static readonly float[] MFP = new float[]
        { 
            0.3f, 0.7f, 0.5f, 0.6f,
            0.6f, 0.5f, 0.9f, 0.8f,
            0.7f, 0.6f, 0.4f, 0.7f,
            0.4f, 0.4f, 0.5f, 0.5f,
            0.9f, 0.8f, 0.7f, 0.6f,
            0.4f, 0.7f, 0.4f, 0.4f,
            0.9f, 0.8f, 0.7f, 0.6f,
            0.4f, 0.7f, 0.4f, 0.4f
        };
        public static readonly float[] HPR = new float[]
        {
            1.2f, 1.6f, 1.7f, 1.6f,
            1.5f, 1.2f, 2.1f, 1.7f,
            2.0f, 1.8f, 2.3f, 1.9f,
            1.2f, 1.2f, 0.5f, 0.5f,
            2.1f, 1.7f, 2.0f, 1.8f,
            2.3f, 1.9f, 1.2f, 1.2f,
            2.1f, 1.7f, 2.0f, 1.8f,
            2.3f, 1.9f, 1.2f, 1.2f
        };
        public static readonly float[] MPR = new float[]
        {
            0.50f, 0.50f, 0.50f, 0.50f,
            1.75f, 0.50f, 0.50f, 0.50f,
            0.70f, 1.30f, 1.90f, 1.60f,
            1.90f, 2.00f, 0.50f, 0.50f,
            0.50f, 0.50f, 0.70f, 1.30f,
            1.90f, 1.60f, 1.90f, 2.00f,
            0.50f, 0.50f, 0.70f, 1.30f,
            1.90f, 1.60f, 1.90f, 2.00f
        };
        public static readonly float[] FPR = new float[]
        {
            0.5f, 1.0f, 0.5f, 1.0f,
            0.6f, 0.5f, 1.4f, 1.2f,
            0.5f, 0.5f, 1.1f, 1.3f,
            0.5f, 0.5f, 0.5f, 0.5f,
            1.4f, 1.2f, 0.5f, 0.5f,
            1.1f, 1.3f, 0.5f, 0.5f,
            1.4f, 1.2f, 0.5f, 0.5f,
            1.1f, 1.3f, 0.5f, 0.5f
        };
        public static readonly float[] DEF = new float[]
        {
            1.00f, 1.35f, 1.40f, 1.20f,
            1.20f, 1.20f, 1.80f, 1.50f,
            1.60f, 1.50f, 1.20f, 1.70f,
            1.20f, 1.30f, 1.30f, 1.30f,
            1.80f, 1.50f, 1.60f, 1.50f,
            1.20f, 1.70f, 1.30f, 1.30f,
            1.80f, 1.50f, 1.60f, 1.50f,
            1.20f, 1.70f, 1.30f, 1.30f
        };
        public static readonly float[] CRIT = new float[]
        {
            1.0f, 1.0f, 1.0f, 1.0f,
            1.0f, 1.0f, 1.0f, 1.0f,
            4.0f, 1.0f, 1.0f, 1.0f,
            1.0f, 1.0f, 1.0f, 1.0f,
            1.0f, 1.0f, 4.0f, 1.0f,
            1.0f, 1.0f, 1.0f, 1.0f,
            1.0f, 1.0f, 4.0f, 1.0f,
            1.0f, 1.0f, 1.0f, 1.0f
        };
        public static readonly float[] BLOCK = new float[]
        {
            0.2f, 0.8f, 0.5f, 0.5f,
            0.2f, 1.2f, 1.0f, 1.5f,
            0.7f, 0.3f, 0.6f, 0.7f,
            0.3f, 0.3f, 0.2f, 0.2f,
            1.0f, 1.5f, 0.7f, 0.3f,
            0.6f, 0.7f, 0.3f, 0.3f,
            1.0f, 1.5f, 0.7f, 0.3f,
            0.6f, 0.7f, 0.3f, 0.3f
        };
        public static readonly float[] EXPLossPercents = new float[]
        {
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, // level 0->20
            6, 6, 6, 6, 6, 6, 6, 6, 6, // level 21->29
            5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, // level 30->59
            4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, // level 60->89
            3, 3, 3, 3, 3, 3, 3, 3, 3, 3, // level 90->99
            2, 2, 2, 2, 2, 2, 2, 2, 2, 2, // level 100->109
            1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f // level 110->120
        };
        public static readonly long[] EXP = new long[]
        {
            0, 14, 20, 36, 90, 152, 250, 352, 480, 
            591, 743, 973, 1290, 1632, 1928, 2340, 3480, 4125, 4995, 
            5880, 7840, 6875, 8243, 10380, 13052, 16450, 20700, 26143, 31950, 
            38640, 57035, 65000, 69125, 72000, 87239, 105863, 128694, 182307, 221450, 
            269042, 390368, 438550, 458137, 468943, 560177, 669320, 799963, 1115396, 1331100, 
            1590273, 2306878, 2594255, 2711490, 2777349, 3318059, 3963400, 4735913, 6600425, 7886110, 
            9421875, 13547310, 15099446, 15644776, 15885934, 18817757, 22280630, 26392968, 36465972, 43184958, 
            51141217, 73556918, 81991117, 84966758, 86252845, 102171368, 120995493, 143307208, 198000645, 234477760, 
            277716683, 381795797, 406848219, 403044458, 391191019, 442876559, 501408635, 567694433, 749813704, 849001357, 
            961154774, 1309582668, 1382799035, 1357505030, 1305632790, 1464862605, 1628695740, 1810772333, 2348583653, 2611145432, 
            2903009208, 3919352097, 4063358600, 3916810682, 4314535354, 4752892146, 5235785988, 5767741845, 6353744416, 6999284849, 
            7710412189, 8493790068, 9356759139, 10307405867, 11354638303, 12508269555, 13779109742, 15179067292, 16721260528, 18420140598, 
            20291626883, 22353256174
        };
    }
}
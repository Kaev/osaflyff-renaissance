using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace FlyffWorld
{
    #region Skill Groups
    /// <summary>
    /// Identifies a skill to it's group.
    /// </summary>
    public enum SkillGroups
    {
        Buff,
        Attack,
        MagicalAttack,
        AoE,
        Heal,
        Ressurect,
        PartySkill,
        Teleport,
        Debuff,
        Cure,
        BuffWithEffect,
        AntiBuff,
        Unknown
    }
    #endregion
        
    #region SkillNames Enumeration and GetSkillByID
    public class SkillNames
    {
        #region Skill List
        /// <summary>
        /// A list of every skill in the game.
        /// </summary>
        public enum SkillList
        {
            #region Unknown
            /// <summary>
            /// The skill is unknown.
            /// </summary>
            Unknown,
            #endregion

            #region Vagrant
            /// <summary>
            /// Vagrant -> Clean Hit
            /// </summary>
            CleanHit,
            /// <summary>
            /// Vagrant -> Flurry
            /// </summary>
            Brandish,
            /// <summary>
            /// Vagrant -> Over Cut
            /// </summary>
            OverCut,
            #endregion

            #region Mercenary
            /// <summary>
            /// Mercenary -> Slash
            /// </summary>
            Slash,
            /// <summary>
            /// Mercenary -> Keenwheel
            /// </summary>
            Keenwheel,
            /// <summary>
            /// Mercenary -> Blindside
            /// </summary>
            Blindside,
            /// <summary>
            /// Mercenary -> Special Hit
            /// </summary>
            SpecialHit,
            /// <summary>
            /// Mercenary -> Bloody Strike
            /// </summary>
            BloodyStrike,
            /// <summary>
            /// Mercenary -> Hit Reflect
            /// </summary>
            HitReflect,
            /// <summary>
            /// Mercenary -> Guilotine
            /// </summary>
            Guilotine,
            /// <summary>
            /// Mercenary -> Protection
            /// </summary>
            Protection,
            /// <summary>
            /// Mercenary -> Pan Barrier
            /// </summary>
            PanBarrier,
            /// <summary>
            /// Mercenary -> Sneaker
            /// </summary>
            Sneaker,
            /// <summary>
            /// Mercenary -> Empower Weapon
            /// </summary>
            EmpowerWeapon,
            /// <summary>
            /// Mercenary -> Smite Axe
            /// </summary>
            SmiteAxe,
            /// <summary>
            /// Mercenary -> Axe Mastery
            /// </summary>
            AxeMastery,
            /// <summary>
            /// Mercenary -> Blazing Sword
            /// </summary>
            BlazingSword,
            /// <summary>
            /// Mercenary -> Sword Mastery
            /// </summary>
            SwordMastery,
            #endregion

            #region Knight
            /// <summary>
            /// Knight -> Charge
            /// </summary>
            Charge,
            /// <summary>
            /// Knight -> Pain Dealer
            /// </summary>
            PainDealer,
            /// <summary>
            /// Knight -> Guard
            /// </summary>
            Guard,
            /// <summary>
            /// Knight -> Earth Divider
            /// </summary>
            EarthDivider,
            /// <summary>
            /// Knight -> Power Stomp
            /// </summary>
            PowerStomp,
            /// <summary>
            /// Knight -> Rage
            /// </summary>
            Rage,
            /// <summary>
            /// Knight -> Pain Reflection
            /// </summary>
            PainReflection,
            /// <summary>
            /// Knight -> Power Swing
            /// </summary>
            PowerSwing,
            #endregion

            #region Master and Hero Knight
            /// <summary>
            /// Master Knight -> Special Two Handed Weapon Mastery
            /// </summary>
            SpecialTwoHandedWeaponMastery,
            /// <summary>
            /// Hero Knight -> Drawing
            /// </summary>
            Drawing,
            #endregion

            #region Blade
            /// <summary>
            /// Blade -> Silent Strike
            /// </summary>
            SilentStrike,
            /// <summary>
            /// Blade -> Spring Attack
            /// </summary>
            SpringAttack,
            /// <summary>
            /// Blade -> Armor Penetrate
            /// </summary>
            ArmorPenetrate,
            /// <summary>
            /// Blade -> Blade Dance
            /// </summary>
            BladeDance,
            /// <summary>
            /// Blade -> Hawk Attack
            /// </summary>
            HawkAttack,
            /// <summary>
            /// Blade -> Berserk
            /// </summary>
            Berserk,
            /// <summary>
            /// Blade -> Sonic Blade
            /// </summary>
            SonicBlade,
            /// <summary>
            /// Blade -> Cross Strike
            /// </summary>
            CrossStrike,
            #endregion

            #region Master and Hero Blade
            /// <summary>
            /// Master Blade -> Special One Handed Weapon Mastery
            /// </summary>
            SpecialOneHandedWeaponMastery,
            /// <summary>
            /// Hero Blade -> Ultimate Defense
            /// </summary>
            UltimateDefense,
            #endregion

            #region Acrobat
            /// <summary>
            /// Acrobat -> Pulling
            /// </summary>
            Pulling,
            /// <summary>
            /// Acrobat -> Fast Walker
            /// </summary>
            FastWalker,
            /// <summary>
            /// Acrobat -> Dark Illusion
            /// </summary>
            DarkIllusion,
            /// <summary>
            /// Acrobat -> Perfect Block
            /// </summary>
            PerfectBlock,
            /// <summary>
            /// Acrobat -> Slow Step
            /// </summary>
            SlowStep,
            /// <summary>
            /// Acrobat -> Yoyo Mastery
            /// </summary>
            YoyoMastery,
            /// <summary>
            /// Acrobat -> Snitch
            /// </summary>
            Snitch,
            /// <summary>
            /// Acrobat -> Deadly Swing
            /// </summary>
            DeadlySwing,
            /// <summary>
            /// Acrobat -> Cross Line
            /// </summary>
            CrossLine,
            /// <summary>
            /// Acrobat -> Counter Attack
            /// </summary>
            CounterAttack,
            /// <summary>
            /// Acrobat -> Junk Arrow
            /// </summary>
            JunkArrow,
            /// <summary>
            /// Acrobat -> Bow Mastery
            /// </summary>
            BowMastery,
            /// <summary>
            /// Acrobat -> Silent Shot
            /// </summary>
            SilentShot,
            /// <summary>
            /// Acrobat -> Auto Shot
            /// </summary>
            AutoShot,
            /// <summary>
            /// Acrobat -> Aimed Shot
            /// </summary>
            AimedShot,
            /// <summary>
            /// Acrobat -> Arrow Rain
            /// </summary>
            ArrowRain,
            #endregion

            #region Jester
            /// <summary>
            /// Jester -> Enchant Poison
            /// </summary>
            EnchantPoison,
            /// <summary>
            /// Jester -> Enchant Blood
            /// </summary>
            EnchantBlood,
            /// <summary>
            /// Jester -> Escape
            /// </summary>
            Escape,
            /// <summary>
            /// Jester -> Critical Swing
            /// </summary>
            CriticalSwing,
            /// <summary>
            /// Jester -> Sneak Stab
            /// </summary>
            BackStab,
            /// <summary>
            /// Jester -> Enchant Absorb
            /// </summary>
            EnchantAbsorb,
            /// <summary>
            /// Jester -> Penya Strike
            /// </summary>
            PenyaStrike,
            /// <summary>
            /// Jester -> Vital Stab
            /// </summary>
            VitalStab,
            #endregion

            #region Master and Hero Jester
            /// <summary>
            /// Master Jester -> Special Yoyo Mastery
            /// </summary>
            SpecialYoyoMastery,
            /// <summary>
            /// Hero Jester -> Silence
            /// </summary>
            Silence,
            #endregion

            #region Ranger
            /// <summary>
            /// Ranger -> Ice Arrow
            /// </summary>
            IceArrow,
            /// <summary>
            /// Ranger -> Flame Arrow
            /// </summary>
            FlameArrow,
            /// <summary>
            /// Ranger -> Poison Arrow
            /// </summary>
            PoisonArrow,
            /// <summary>
            /// Ranger -> Fast Shot
            /// </summary>
            FastShot,
            /// <summary>
            /// Ranger -> Piercing Arrow
            /// </summary>
            PiercingArrow,
            /// <summary>
            /// Ranger -> Nature
            /// </summary>
            Nature,
            /// <summary>
            /// Ranger -> Silent Arrow
            /// </summary>
            SilentArrow,
            /// <summary>
            /// Ranger -> Triple Shot
            /// </summary>
            TripleShot,
            #endregion

            #region Master and Hero Ranger
            /// <summary>
            /// Master Ranger -> Special Bow Mastery
            /// </summary>
            SpecialBowMastery,
            /// <summary>
            /// Hero Ranger -> Hawk Eyes
            /// </summary>
            HawkEyes,
            #endregion

            #region Assist
            /// <summary>
            /// Assist -> Heal
            /// </summary>
            Heal,
            /// <summary>
            /// Assist -> Patience
            /// </summary>
            Patience,
            /// <summary>
            /// Assist -> Resurrection
            /// </summary>
            Resurrection,
            /// <summary>
            /// Assist -> Circle Healing
            /// </summary>
            CircleHealing,
            /// <summary>
            /// Assist -> Prevention
            /// </summary>
            Prevention,
            /// <summary>
            /// Assist -> Quick Step
            /// </summary>
            QuickStep,
            /// <summary>
            /// Assist -> Haste
            /// </summary>
            Haste,
            /// <summary>
            /// Assist -> Cat's Reflex
            /// </summary>
            CatsReflex,
            /// <summary>
            /// Assist -> Cannon Ball
            /// </summary>
            CannonBall,
            /// <summary>
            /// Assist -> Mental Sign
            /// </summary>
            MentalSign,
            /// <summary>
            /// Assist -> Heap Up
            /// </summary>
            HeapUp,
            /// <summary>
            /// Assist -> Beef Up
            /// </summary>
            BeefUp,
            /// <summary>
            /// Assist -> Accuracy
            /// </summary>
            Accuracy,
            /// <summary>
            /// Assist -> Straight Punch
            /// </summary>
            StraightPunch,
            /// <summary>
            /// Assist -> Stone Hand
            /// </summary>
            StoneHand,
            /// <summary>
            /// Assist -> Burst Crack
            /// </summary>
            BurstCrack,
            /// <summary>
            /// Assist -> Power Fist
            /// </summary>
            PowerFist,
            #endregion

            #region Billposter
            /// <summary>
            /// Billposter -> Belial Smashing
            /// </summary>
            BelialSmashing,
            /// <summary>
            /// Billposter -> Asmodeus
            /// </summary>
            Asmodeus,
            /// <summary>
            /// Billposter -> Blood Fist
            /// </summary>
            BloodFist,
            /// <summary>
            /// Billposter -> Baraqijal Esna
            /// </summary>
            BaraqijalEsna,
            /// <summary>
            /// Billposter -> Piercing Serpent
            /// </summary>
            PiercingSerpent,
            /// <summary>
            /// Billposter -> Bgvur Tialbold
            /// </summary>
            BgvurTialbold,
            /// <summary>
            /// Billposter -> Sonichand
            /// </summary>
            Sonichand,
            /// <summary>
            /// Billposter -> Asalraalaikum
            /// </summary>
            Asalraalaikum,
            #endregion

            #region Master and Hero Billposter
            /// <summary>
            /// Master Billposter -> Special Knuckle Mastery
            /// </summary>
            SpecialKnuckleMastery,
            /// <summary>
            /// Hero Billposter -> Disenchant
            /// </summary>
            Disenchant,
            #endregion

            #region Ringmaster
            /// <summary>
            /// Ringmaster -> Protect
            /// </summary>
            Protect,
            /// <summary>
            /// Ringmaster -> Holycross
            /// </summary>
            Holycross,
            /// <summary>
            /// Ringmaster -> Gvur Tialla
            /// </summary>
            GvurTialla,
            /// <summary>
            /// Ringmaster -> Holyguard
            /// </summary>
            Holyguard,
            /// <summary>
            /// Ringmaster -> Spirit Fortune
            /// </summary>
            SpiritFortune,
            /// <summary>
            /// Ringmaster -> Heal Rain
            /// </summary>
            HealRain,
            /// <summary>
            /// Ringmaster -> Geburah Tipreth
            /// </summary>
            GeburahTipreth,
            /// <summary>
            /// Ringmaster -> Merkaba Hanzelrusha
            /// </summary>
            MerkabaHanzelrusha,
            #endregion

            #region Master and Hero Ringmaster
            /// <summary>
            /// Master Ringmaster -> Blessing of Wise Man
            /// </summary>
            BlessingOfWiseMan,
            /// <summary>
            /// Hero Ringmaster -> Return
            /// </summary>
            Return,
            #endregion

            #region Magician
            /// <summary>
            /// Magician -> Mental Strike
            /// </summary>
            MentalStrike,
            /// <summary>
            /// Magician -> Blinkpool
            /// </summary>
            Blinkpool,
            /// <summary>
            /// Magician -> Flame Ball
            /// </summary>
            FlameBall,
            /// <summary>
            /// Magician -> Flame Geyser
            /// </summary>
            FlameGeyser,
            /// <summary>
            /// Magician -> Fire Strike
            /// </summary>
            FireStrike,
            /// <summary>
            /// Magician -> Sword Wind
            /// </summary>
            Swordwind,
            /// <summary>
            /// Magician -> Strong Wind
            /// </summary>
            Strongwind,
            /// <summary>
            /// Magician -> Wind Cutter
            /// </summary>
            WindCutter,
            /// <summary>
            /// Magician -> Ice Missile
            /// </summary>
            IceMissile,
            /// <summary>
            /// Magician -> Waterball
            /// </summary>
            Waterball,
            /// <summary>
            /// Magician -> Water Well
            /// </summary>
            WaterWell,
            /// <summary>
            /// Magician -> Static Ball
            /// </summary>
            StaticBall,
            /// <summary>
            /// Magician -> Lightning Ball
            /// </summary>
            LightningBall, // Lightning Ram?!?! @ http://flyff-wiki.gpotato.com/index.php/Magician
            /// <summary>
            /// Magician -> Lightning Shock
            /// </summary>
            LightningShock,
            /// <summary>
            /// Magician -> Stone Spike
            /// </summary>
            StoneSpike,
            /// <summary>
            /// Magician -> Rockcrash
            /// </summary>
            Rockcrash,
            /// <summary>
            /// Magician -> Rooting
            /// </summary>
            Looting,
            #endregion

            #region Elementor
            /// <summary>
            /// Elementor -> Firebird
            /// </summary>
            Firebird,
            /// <summary>
            /// Elementor -> Stone Spear
            /// </summary>
            StoneSpear,
            /// <summary>
            /// Elementor -> Void
            /// </summary>
            Void,
            /// <summary>
            /// Elementor -> Thunder Strike
            /// </summary>
            ThunderStrike,
            /// <summary>
            /// Elementor -> Iceshark
            /// </summary>
            Iceshark,
            /// <summary>
            /// Elementor -> Burningfield
            /// </summary>
            Burningfield,
            /// <summary>
            /// Elementor -> Earthquake
            /// </summary>
            Earthquake,
            /// <summary>
            /// Elementor -> Windfield
            /// </summary>
            Windfield,
            /// <summary>
            /// Elementor -> Electric Shock
            /// </summary>
            ElectricShock,
            /// <summary>
            /// Elementor -> Poison Cloud
            /// </summary>
            PoisonCloud,
            /// <summary>
            /// Elementor -> Fire Mastery
            /// </summary>
            FireMastery,
            /// <summary>
            /// Elementor -> Earth Mastery
            /// </summary>
            EarthMastery,
            /// <summary>
            /// Elementor -> Wind Mastery
            /// </summary>
            WindMastery,
            /// <summary>
            /// Elementor -> Lightning Mastery
            /// </summary>
            LightningMastery,
            /// <summary>
            /// Elementor -> Water Mastery
            /// </summary>
            WaterMastery,
            /// <summary>
            /// Elementor -> Meteo Shower
            /// </summary>
            MeteoShower,
            /// <summary>
            /// Elementor -> Sandstorm
            /// </summary>
            Sandstorm,
            /// <summary>
            /// Elementor -> Lightning Storm
            /// </summary>
            LightningStorm,
            /// <summary>
            /// Elementor -> Blizzard
            /// </summary>
            Blizzard,
            #endregion

            #region Master and Hero Elementor
            /// <summary>
            /// Master Elementor -> Special INT Mastery
            /// </summary>
            SpecialIntMasteryEle,
            /// <summary>
            /// Hero Elementor -> Curse Mind
            /// </summary>
            CurseMind,
            #endregion

            #region Psykeeper
            /// <summary>
            /// Psykeeper -> Demonology
            /// </summary>
            Demonology,
            /// <summary>
            /// Psykeeper -> Psychic Bomb
            /// </summary>
            PsychicBomb,
            /// <summary>
            /// Psykeeper -> Crucio Spell
            /// </summary>
            CrucioSpell,
            /// <summary>
            /// Psykeeper -> Satanology
            /// </summary>
            Satanology,
            /// <summary>
            /// Psykeeper -> Spirit Bomb
            /// </summary>
            SpiritBomb,
            /// <summary>
            /// Psykeeper -> Maximum Crisis
            /// </summary>
            MaximumCrisis,
            /// <summary>
            /// Psykeeper -> Psychic Wall
            /// </summary>
            PsychicWall,
            /// <summary>
            /// Psykeeper -> Psychic Square
            /// </summary>
            PsychicSquare,
            #endregion

            #region Master and Hero Psykeeper
            /// <summary>
            /// Master Psykeeper -> Special INT Mastery
            /// </summary>
            SpecialIntMasteryPsy,
            /// <summary>
            /// Hero Psykeeper -> Petrification
            /// </summary>
            Petrification,
            #endregion
            #region Skill for Effect
            /// <summary>
            /// Skill for effect -> SI_GEN_ATK_COUNTER
            /// </summary>
            EFFECT_ATK_Counter,
            /// <summary>
            /// Skill for effect -> SI_GEN_KNOCK_BACK
            /// </summary>
            EFFECT_KNOCK_Back,
            /// <summary>
            /// Skill for effect -> SI_GEN_FRAMEARROW_BURN
            /// </summary>
            EFFECT_FRAMEARROW_Burn,
            /// <summary>
            /// Skill for effect -> SI_GEN_POISON
            /// </summary>
            EFFECT_POISON,
            /// <summary>
            /// Skill for effect -> SI_GEN_STONEHAND
            /// </summary>
            EFFECT_STONEHAND,
            /// <summary>
            /// Skill for effect -> SI_GEN_BLEEDING
            /// </summary>
            EFFECT_BLEEDING,
            /// <summary>
            /// Skill for effect -> SI_GEN_STEALHP
            /// </summary>
            EFFECT_STEALHP,
            /// <summary>
            /// Skill for effect -> SI_GEN_SWORDCROSS
            /// </summary>
            EFFECT_SWORDCROSS,
            /// <summary>
            /// Skill for effect -> SI_GEN_POISONSTING
            /// </summary>
            EFFECT_POISONSTING,
            /// <summary>
            /// Skill for effect -> SI_MOB_BLEEDING
            /// </summary>
            EFFECT_MOBBLEEDING,
            #endregion
        }
        #endregion
        /// <summary>
        /// Gets a SkillList from given SkillID.
        /// </summary>
        /// <param name="SkillID">Skill ID of the skill.</param>
        /// <returns>Enumeration object of type SkillList representing a special skill.</returns>
        public static SkillList GetSkillNameBySkillID(int SkillID)
        {
            switch (SkillID)
            {
                case 1: return SkillList.CleanHit; break;
                case 2: return SkillList.Brandish; break;
                case 3: return SkillList.OverCut; break;
                case 4: return SkillList.Slash; break;
                case 5: return SkillList.Keenwheel; break;
                case 6: return SkillList.Blindside; break;
                case 7: return SkillList.SwordMastery; break;
                case 8: return SkillList.AxeMastery; break;
                case 9: return SkillList.Protection; break;
                case 10: return SkillList.PanBarrier; break;
                case 11: return SkillList.SpecialHit; break;
                case 12: return SkillList.Guilotine; break;
                case 13: return SkillList.Sneaker; break;
                case 14: return SkillList.HitReflect; break;
                case 20: return SkillList.Haste; break;
                case 30: return SkillList.FireStrike; break;
                case 31: return SkillList.WindCutter; break;
                case 32: return SkillList.Waterball; break;
                case 33: return SkillList.WaterWell; break;
                case 34: return SkillList.LightningStorm; break;
                case 35: return SkillList.LightningShock; break;
                case 36: return SkillList.Rockcrash; break;
                case 37: return SkillList.Looting; break;
                case 44: return SkillList.Heal; break;
                case 45: return SkillList.Resurrection; break;
                case 46: return SkillList.Patience; break;
                case 48: return SkillList.Prevention; break;
                case 49: return SkillList.HeapUp; break;
                case 50: return SkillList.CannonBall; break;
                case 51: return SkillList.CircleHealing; break;
                case 52: return SkillList.MentalSign; break;
                case 53: return SkillList.BeefUp; break;
                case 64: return SkillList.FlameBall; break;
                case 65: return SkillList.FlameGeyser; break;
                case 69: return SkillList.Swordwind; break;
                case 70: return SkillList.Strongwind; break;
                case 104: return SkillList.StraightPunch; break;
                case 105: return SkillList.BurstCrack; break;
                case 107: return SkillList.Blinkpool; break;
                case 108: return SkillList.BlazingSword; break;
                case 109: return SkillList.SmiteAxe; break;
                case 111: return SkillList.EmpowerWeapon; break;
                case 112: return SkillList.BloodyStrike; break;
                case 113: return SkillList.StoneHand; break;
                case 114: return SkillList.QuickStep; break;
                case 115: return SkillList.CatsReflex; break;
                case 116: return SkillList.Accuracy; break;
                case 117: return SkillList.PowerFist; break;
                case 118: return SkillList.IceMissile; break;
                case 119: return SkillList.LightningBall; break;
                case 120: return SkillList.StoneSpike; break;
                case 121: return SkillList.MentalStrike; break;
                case 128: return SkillList.Guard; break;
                case 129: return SkillList.PainReflection; break;
                case 130: return SkillList.Rage; break;
                case 131: return SkillList.PowerSwing; break;
                case 132: return SkillList.EarthDivider; break;
                case 133: return SkillList.Charge; break;
                case 134: return SkillList.PainDealer; break;
                case 135: return SkillList.PowerStomp; break;
                case 136: return SkillList.CrossStrike; break;
                case 137: return SkillList.ArmorPenetrate; break;
                case 138: return SkillList.SilentStrike; break;
                case 139: return SkillList.BladeDance; break;
                case 140: return SkillList.SpringAttack; break;
                case 141: return SkillList.HawkAttack; break;
                case 142: return SkillList.SonicBlade; break;
                case 143: return SkillList.Berserk; break;
                case 144: return SkillList.HealRain; break;
                case 145: return SkillList.Holycross; break;
                case 146: return SkillList.Protect; break;
                case 147: return SkillList.Holyguard; break;
                case 148: return SkillList.SpiritFortune; break;
                case 149: return SkillList.GvurTialla; break;
                case 150: return SkillList.GeburahTipreth; break;
                case 151: return SkillList.MerkabaHanzelrusha; break;
                case 152: return SkillList.BelialSmashing; break;
                case 153: return SkillList.PiercingSerpent; break;
                case 154: return SkillList.BloodFist; break;
                case 155: return SkillList.Sonichand; break;
                case 156: return SkillList.Asmodeus; break;
                case 157: return SkillList.BaraqijalEsna; break;
                case 158: return SkillList.BgvurTialbold; break;
                case 159: return SkillList.Asalraalaikum; break;
                case 160: return SkillList.Demonology; break;
                case 161: return SkillList.Satanology; break;
                case 162: return SkillList.PsychicBomb; break;
                case 163: return SkillList.PsychicWall; break;
                case 164: return SkillList.SpiritBomb; break;
                case 165: return SkillList.CrucioSpell; break;
                case 166: return SkillList.MaximumCrisis; break;
                case 167: return SkillList.PsychicSquare; break;
                case 168: return SkillList.Firebird; break;
                case 169: return SkillList.FireMastery; break;
                case 170: return SkillList.Burningfield; break;
                case 171: return SkillList.ThunderStrike; break;
                case 172: return SkillList.LightningMastery; break;
                case 173: return SkillList.ElectricShock; break;
                case 174: return SkillList.StoneSpear; break;
                case 175: return SkillList.EarthMastery; break;
                case 176: return SkillList.Earthquake; break;
                case 177: return SkillList.Iceshark; break;
                case 178: return SkillList.WaterMastery; break;
                case 179: return SkillList.PoisonCloud; break;
                case 180: return SkillList.Void; break;
                case 181: return SkillList.WindMastery; break;
                case 182: return SkillList.Windfield; break;
                case 183: return SkillList.MeteoShower; break;
                case 184: return SkillList.LightningStorm; break;
                case 185: return SkillList.Sandstorm; break;
                case 186: return SkillList.Blizzard; break;
                case 187: return SkillList.Iceshark; break;
                case 188: return SkillList.WaterMastery; break;
                case 189: return SkillList.PoisonCloud; break;
                case 191: return SkillList.YoyoMastery; break;
                case 192: return SkillList.BowMastery; break;
                case 193: return SkillList.DarkIllusion; break;
                case 194: return SkillList.JunkArrow; break;
                case 195: return SkillList.FastWalker; break;
                case 196: return SkillList.AimedShot; break;
                case 197: return SkillList.SlowStep; break;
                case 198: return SkillList.SilentShot; break;
                case 199: return SkillList.PerfectBlock; break;
                case 200: return SkillList.ArrowRain; break;
                case 201: return SkillList.CrossLine; break;
                case 202: return SkillList.AutoShot; break;
                case 203: return SkillList.Snitch; break;
                case 204: return SkillList.CounterAttack; break;
                case 205: return SkillList.DeadlySwing; break;
                case 206: return SkillList.Pulling; break;
                case 207: return SkillList.CriticalSwing; break;
                case 208: return SkillList.EnchantPoison; break;
                case 209: return SkillList.EnchantBlood; break;
                case 210: return SkillList.EnchantAbsorb; break;
                case 211: return SkillList.BackStab; break;
                case 212: return SkillList.PenyaStrike; break;
                case 213: return SkillList.Escape; break;
                case 214: return SkillList.VitalStab; break;
                case 215: return SkillList.FastShot; break;
                case 216: return SkillList.IceArrow; break;
                case 217: return SkillList.FlameArrow; break;
                case 218: return SkillList.PiercingArrow; break;
                case 219: return SkillList.PoisonArrow; break;
                case 220: return SkillList.SilentArrow; break;
                case 221: return SkillList.Nature; break;
                case 222: return SkillList.TripleShot; break;
                case 223: return SkillList.EFFECT_ATK_Counter; break;
                case 224: return SkillList.EFFECT_KNOCK_Back; break;
                case 225: return SkillList.EFFECT_FRAMEARROW_Burn; break;
                case 226: return SkillList.EFFECT_POISON; break;
                case 227: return SkillList.EFFECT_STONEHAND; break;
                case 228: return SkillList.EFFECT_BLEEDING; break;
                case 229: return SkillList.EFFECT_STEALHP; break;
                case 230: return SkillList.EFFECT_SWORDCROSS; break;
                case 231: return SkillList.EFFECT_POISONSTING; break;
                case 232: return SkillList.EFFECT_MOBBLEEDING; break;
                case 237: return SkillList.UltimateDefense; break;
                case 238: return SkillList.Drawing; break;
                case 239: return SkillList.Silence; break;
                case 240: return SkillList.HawkEyes; break;
                case 241: return SkillList.CurseMind; break;
                case 242: return SkillList.Petrification; break;
                case 243: return SkillList.Disenchant; break;
                case 244: return SkillList.Return; break;
                case 309: return SkillList.SpecialOneHandedWeaponMastery; break;
                case 310: return SkillList.SpecialTwoHandedWeaponMastery; break;
                case 311: return SkillList.SpecialYoyoMastery; break;
                case 312: return SkillList.SpecialBowMastery; break;
                case 313: return SkillList.SpecialIntMasteryEle; break;
                case 314: return SkillList.SpecialIntMasteryPsy; break;
                case 315: return SkillList.SpecialKnuckleMastery; break;
                case 316: return SkillList.BlessingOfWiseMan; break;
            }
            return SkillList.Unknown;
        }
    }
    #endregion
    public partial class Skills
    {
        #region define skill that could be used by weapon type


        public static int[] SkillUsingSWORD = new int[]
            {
                1,2,3,4, 5, 6, 9, 10, 108, 111, 112, 7,11, 12, 13, 14,138,137,139,143,142,136
            };
        public static int[] SkillUsingAXE = new int[]
            {
                1,2,3,4, 5, 6, 9, 10, 109, 111, 112, 8,11, 12, 13, 14,140,137,141,143,142,136
            };
        public static int[] SkillUsingTHSWORD = new int[]
            {
                133,128,132,130,129,131
            };
        public static int[] SkillUsingTHAXE = new int[]
            {
                134,135,130,129,131
            };
        public static int[] SkillUsingSTICK = new int[]
            {
                44, 45, 46, 48, 49, 50, 51, 52, 53, 113, 114, 115, 116, 20, 144, 145, 146, 147, 148, 149, 150, 151
            };
        public static int[] SkillUsingKNUCKLE = new int[]
            {
                104,105,117,113,152, 153, 154, 155, 156, 157, 158, 159
            };
        public static int[] SkillUsingSTAFF = new int[]
            {
                64, 65, 69, 70, 118, 119, 120, 121, 30, 31, 32, 33, 34, 35, 36, 37, 107, //basic magician
                168, 174, 180, 171, 177, 170, 176, 182, 173,179, 169, 175, 181, 172, 178, 183, 185, 184,186
            };
        public static int[] SkillUsingWAND = new int[]
            {
                64, 65, 69, 70, 118, 119, 120, 121, 30, 31, 32, 33, 34, 35, 36, 37, 107, //basic magician
                160, 161, 162, 163, 164, 165, 166, 167
            };
        public static int[] SkillUsingBOW = new int[]
            {
                191, 192, 193, 194, 195, 196, 197, 198, 200, 202, //basic acrobat
                215, 216, 217, 218, 219, 220, 221, 222
                
            };
        public static int[] SkillUsingYOYO = new int[]
            {
                191, 192, 193, 195, 197, 199,201, 203, 204, 205, 206, //basic acrobat
                207, 208, 209, 210, 211, 212, 213, 214
            };
        #endregion

        #region test if weapon used is correct
        public static bool IsEquipedWithCorrectWeapon(Client user, Skill skill)
        {
            Slot weapon = user.GetSlotByPosition(52);
            int weaponKind3 = weapon.c_item.Data.itemkind[2];
            
            bool result = false;
            switch (weaponKind3)
            {
                case FlyffItemkind.IK3_SWD:
                    
                    for (int i = 0; i < SkillUsingSWORD.Length; i++)
                    {
                        if (SkillUsingSWORD[i] == skill.dwSkillID)
                            result = true;

                    }
                    if (weapon.c_item.Data.twoHanded)
                    {
                        for (int i = 0; i < SkillUsingTHSWORD.Length; i++)
                        {
                            if (SkillUsingTHSWORD[i] == skill.dwSkillID)
                                result = true;

                        }
                    }
                    break;
                case FlyffItemkind.IK3_AXE:
                    
                    for (int i = 0; i < SkillUsingAXE.Length; i++)
                    {
                        if (SkillUsingAXE[i] == skill.dwSkillID)
                            result = true;

                    }
                    if (weapon.c_item.Data.twoHanded)
                    {
                        for (int i = 0; i < SkillUsingTHAXE.Length; i++)
                        {
                            if (SkillUsingTHAXE[i] == skill.dwSkillID)
                                result = true;

                        }
                    }

                    break;
                case FlyffItemkind.IK3_CHEERSTICK:
                    
                    for (int i = 0; i < SkillUsingSTICK.Length; i++)
                    {
                        
                        if (SkillUsingSTICK[i] == skill.dwSkillID)
                        result = true;
                            

                    }
                    break;

                case FlyffItemkind.IK3_KNUCKLEHAMMER:
                    
                    for (int i = 0; i < SkillUsingKNUCKLE.Length; i++)
                    {
                        if (SkillUsingKNUCKLE[i] == skill.dwSkillID)
                            result = true;

                    }

                    break;
                case FlyffItemkind.IK3_WAND:
                    
                    for (int i = 0; i < SkillUsingWAND.Length; i++)
                    {
                        if (SkillUsingWAND[i] == skill.dwSkillID)
                            result = true;

                    }

                    break;
                case FlyffItemkind.IK3_STAFF:
                    
                    for (int i = 0; i < SkillUsingSTAFF.Length; i++)
                    {
                        if (SkillUsingSTAFF[i] == skill.dwSkillID)
                            result = true;

                    }

                    break;
                case FlyffItemkind.IK3_THSWD:
                    
                    for (int i = 0; i < SkillUsingTHSWORD.Length; i++)
                    {
                        if (SkillUsingTHSWORD[i] == skill.dwSkillID)
                            result = true;

                    }

                    break;
                case FlyffItemkind.IK3_THAXE:
                    
                    for (int i = 0; i < SkillUsingTHAXE.Length; i++)
                    {
                        if (SkillUsingTHAXE[i] == skill.dwSkillID)
                            result = true;

                    }

                    break;
                case FlyffItemkind.IK3_YOYO:
                    
                    for (int i = 0; i < SkillUsingYOYO.Length; i++)
                    {
                        if (SkillUsingYOYO[i] == skill.dwSkillID)
                            result = true;

                    }

                    break;
                case FlyffItemkind.IK3_BOW:
                    for (int i = 0; i < SkillUsingBOW.Length; i++)
                    {
                        if (SkillUsingBOW[i] == skill.dwSkillID)
                            result = true;

                    }

                    break;
                default: result = false; break;
            }
            int resultat;
            if (result) resultat = 1; else resultat = 0;
            
            return result;


        }
        #endregion       

        
    }
    #region Process Skill Usage Data ~ PSUData
    /// <summary>
    /// Contains information about skill and user to use when a skill is being cast.
    /// </summary>
    public class CPSUData
    {
        #region Private Things (Not Porn lol)
        private SkillGroups sgGetGroup()
        {
            switch (Skills.skillType(Skill.dwNameID))
            {
                case "buff": return SkillGroups.Buff; break;
                case "attack": return SkillGroups.Attack; break;
                case "AoE": return SkillGroups.AoE; break;
                case "heal": return SkillGroups.Heal; break;
                case "resurrect": return SkillGroups.Ressurect; break;
                case "partyS": return SkillGroups.PartySkill; break;
                case "teleport": return SkillGroups.Teleport; break;
                case "debuff": return SkillGroups.Debuff; break;
                case "cure": return SkillGroups.Cure; break;
                case "buffwitheffect": return SkillGroups.BuffWithEffect; break;
                case "antibuff": return SkillGroups.AntiBuff; break;
                case "MagicalAttack": return SkillGroups.MagicalAttack; break;
                case "unknown":
                default:Log.Write(Log.MessageType.debug,"This skill has an unknown type");
                return SkillGroups.Unknown; break;
            }
        }
        
        private Skills privateSkill;
        #endregion

        /// <summary>
        /// Should not be used, only PSUData uses this.
        /// </summary>
        public class mvrTarget
        {
            private Mover mvr;
            public void SetMover(Mover target) { mvr = target; }
            public bool IsClient { get { return (mvr is Client); } }
            public bool IsMonster { get { return (mvr is Monster); } }
            public Mover AsMover
            {
                get { return mvr; }
                set { mvr = value; }
            }
            public Client AsClient
            {
                get { return (Client)mvr; }
                set { mvr = value; }
            }
            public Monster AsMonster
            {
                get { return (Monster)mvr; }
                set { mvr = value; }
            }
        }
        /// <summary>
        /// Gets or sets caster client.
        /// </summary>
        public Client Caster;
        /// <summary>
        /// Gets or sets target client.
        /// </summary>
        public mvrTarget Target = new mvrTarget();
        /// <summary>
        /// Gets or sets the skill data.
        /// </summary>
        public Skills SkillData;
        /// <summary>
        /// Gets or sets the skill.
        /// </summary>
        public Skills Skill
        {
            set
            {
                privateSkill = value;
                /*Skillreturn GetSkillNameBySkillID(privateSkill.dwSkillID);
                if (SkillName == SkillNames.SkillList.Unknown)
                    Log.Write(Log.MessageType.warning, "Skill {0} [Level: {1}] was not found in list (Caster: {2})!",
                        privateSkill.dwSkillID, privateSkill.dwSkillLevel, Caster.c_data.strPlayerName);*/
            }
            get { return privateSkill; }
        }
        /// <summary>
        /// Gets type of skill.
        /// </summary>
        public SkillGroups Type { get { return sgGetGroup(); } }
        /// <summary>
        /// Enumeration of type SkillList representing what skill it is.
        /// </summary>
        public SkillNames.SkillList SkillName = SkillNames.SkillList.Unknown;

        /// <summary>
        /// Checks if a target is set in the PSUData.
        /// </summary>
        public bool IsTargetSet { get { return (Target != null); } }
        /// <summary>
        /// Checks if the PSUData is valid.
        /// </summary>
        public bool IsValid { get { return ((Caster != null) && (SkillData != null) && (Skill != null)); } }
        /// <summary>
        /// Gets current information about this PSUData.
        /// </summary>
        /// <returns>String with information.</returns>
        public override string ToString()
        {
            return String.Format("Skill: {0}, Myself: {1}, Target is a {2}",
                Skill.dwNameID, Caster.c_data.strPlayerName, (Target.IsClient ? "Player (" + Target.AsClient.c_data.strPlayerName + ")" : "Monster (" + Target.AsMonster.Data.mobName + ")"));
        }
        /// <summary>
        /// Say if it come from an actionslot activation or not.
        /// </summary>
        public bool IsFromActionSlot = false;
        /// <summary>
        /// Give order of the action slot
        /// </summary>
        public int actionslotorder = 0;
        /// <summary>
        /// Gets current information about this PSUData.
        /// </summary>
        /// <returns>String with information.</returns>
    }
    #endregion

    #region Public Partial Class Client -> ProcessSkillUsage(CPSUData PSUData)
    public partial class Client
    {
        public bool ProcessSkillUsage(CPSUData PSUData)
        {

            if (!PSUData.IsValid)
            {
                Log.Write(Log.MessageType.debug, "PSUData is not valid");
                return false;
            }
            SkillGroups sg = PSUData.Type;
            Log.Write(Log.MessageType.debug,"Actual skill is in {0} group",sg);
            switch (sg)
            {
                case SkillGroups.AntiBuff: return UseSkill.AntiBuff(PSUData, false);
                case SkillGroups.AoE: return UseSkill.AoE(PSUData);
                case SkillGroups.Attack: return UseSkill.Attack(PSUData);
                case SkillGroups.Buff: return UseSkill.Buffs(PSUData);
                case SkillGroups.BuffWithEffect: return UseSkill.BuffWithEffect(PSUData);
                case SkillGroups.Cure: return UseSkill.Cure(PSUData);
                case SkillGroups.Debuff: return UseSkill.Debuff(PSUData);
                case SkillGroups.Heal: return UseSkill.Heal(PSUData);
                case SkillGroups.PartySkill: return UseSkill.SkillPartySkills(PSUData);
                case SkillGroups.Ressurect: return UseSkill.Resurect(PSUData);
                case SkillGroups.Teleport: return UseSkill.Teleport(PSUData);
                case SkillGroups.MagicalAttack: return UseSkill.MagicalAttack(PSUData);
            }
            return false;
        }
        public int NumberOfAttack(CPSUData PSUData)
        {
            int numberofattack = 1;
            switch (PSUData.SkillName)
            {
                case SkillNames.SkillList.Slash: return 2; break;
                case SkillNames.SkillList.Keenwheel: return 3; break;
                case SkillNames.SkillList.JunkArrow:
                    int skilllvl = PSUData.SkillData.dwSkillLvl;
                    return skilllvl / 2; break;
                case SkillNames.SkillList.TripleShot: return 3; break;
            }
            return numberofattack;



        }
        public void ManageBuffEffectonMover(Mover mover, Client client)
        {
            if (DiceRoller.Roll(c_attributes[FlyFF.DST_CHR_CHANCESTUN])) //if player has stonehand and it work
            {
                bool alreadystuned = false;
                if (mover.antibuffs.Count > 0)
                {
                    for (int i = 0; i < mover.antibuffs.Count; i++)
                    {
                        if (mover.antibuffs[i]._skill.dwNameID == 227)
                            alreadystuned = true;
                    }
                }
                if (!alreadystuned || mover.antibuffs.Count == 0)
                {

                    Skills skill = new Skills();
                    skill.dwSkillLvl = 2;
                    skill.dwNameID = 227;
                    skill.dwDestParam1 = 64;
                    skill.dwAdjParamVal1 = 8;
                    skill.dwSkillTime = 4000;
                    Buff antibuff = new Buff();
                    antibuff._skill = skill;
                    antibuff.dwTime = skill.dwSkillTime;
                    

                    mover.SendMoverBuff(antibuff, 1);
                    mover.bIsBlocked = true;
                    mover.bIsFighting = false;
                    mover.SendChangeState(antibuff.dwTime);
                    SendPlayerTargetStateInfos(mover.dwMoverID, antibuff);

                    mover.antibuffs.Add(antibuff);

                    //ok now we send to remove antibuff                                    
                    Log.Write(Log.MessageType.debug, "We send a delayed remove buff to act in {0} milliseconde", antibuff._skill.dwSkillTime);
                    ulong qwID = Delayer.RegisterDelayedAction(delegate() { DelayedActions.Delayed_RemoveBuffonMover(mover, client, antibuff, true, false); }, antibuff._skill.dwSkillTime);
                    /* [Divinepunition] Note : if this antibuff is send on a Client.we add the delayed action to the list of this client
                    * cause someone can cure him and remove the effect. So no need to keep this in delayer list. In this case we could wrote this :
                    DelayedActions.Delaycontent delay = new DelayedActions.Delaycontent(qwID, "Delayed_RemoveBuffonMover", skill.dwNameID);
                    clienttarget.Delayedactionslist.Add(delay);*/
                }
                
            }
            if (c_attributes[FlyFF.DST_CHRSTATE] == 1024) //if player is underdarkillusion and attack remove it
            {
                Buff buff = new Buff();
                Skill skill = GetSkillByID(193);
                buff._skill = Skills.getSkillByNameIDAndLevel(skill.dwSkillID, skill.dwSkillLevel);
                buff.dwTime = 0;
                SendPlayerBuff(buff);
                for (int i = 0; i < c_data.buffs.Count; i++)
                {
                    if (c_data.buffs[i]._skill.dwNameID == 193)
                        c_data.buffs.Remove(c_data.buffs[i]);
                }
            }
            if (DiceRoller.Roll(c_attributes[FlyFF.DST_CHR_CHANCEBLEEDING])) //if player has bleeding buff and it work
            {
                bool alreadybleed = false;
                if (mover.antibuffs.Count > 0)
                {
                    for (int i = 0; i < mover.antibuffs.Count; i++)
                    {
                        if (mover.antibuffs[i]._skill.dwNameID == 228) //bleeding
                            alreadybleed = true;
                    }
                }
                if (!alreadybleed || mover.antibuffs.Count == 0)
                {

                    Skills skill = new Skills();
                    skill.dwSkillLvl = 3;
                    skill.dwNameID = 228;
                    skill.dwDestParam1 = 64;
                    skill.dwAdjParamVal1 = 8;
                    skill.dwdestData1 = 24;
                    skill.dwPainTime = 3000;
                    skill.dwSkillTime = 6000;
                    Buff antibuff = new Buff();
                    antibuff._skill = skill;
                    antibuff.dwTime = skill.dwSkillTime;
                    

                    mover.SendMoverBuff(antibuff, 1);                    
                    mover.SendChangeState(antibuff.dwTime);
                    SendPlayerTargetStateInfos(mover.dwMoverID, antibuff);
                    mover.antibuffs.Add(antibuff);

                    //ok now we send to remove antibuff                                    
                    Log.Write(Log.MessageType.debug, "We send a delayed remove buff to act in {0} milliseconde", antibuff._skill.dwSkillTime);
                    ulong qwID = Delayer.RegisterDelayedAction(delegate() { DelayedActions.Delayed_RemoveBuffonMover(mover, client, antibuff, true, false); }, antibuff._skill.dwSkillTime);
                    /* [Divinepunition] Note : if this antibuff is send on a Client.we add the delayed action to the list of this client
                    * cause someone can cure him and remove the effect. So no need to keep this in delayer list. In this case we could wrote this :
                    DelayedActions.Delaycontent delay = new DelayedActions.Delaycontent(qwID, "Delayed_RemoveBuffonMover", skill.dwNameID);
                    clienttarget.Delayedactionslist.Add(delay);*/
                    //Send damage with delay
                    int numberofdmg = 1 + skill.dwPainTime / skill.dwSkillTime;
                    
                    DelayedActions.str_delayedSendDamage senddamage = new DelayedActions.str_delayedSendDamage();
                    senddamage.caster = client as Mover;
                    senddamage.target = mover;
                    int dmg = skill.dwdestData1;
                    senddamage.damage = dmg;
                    senddamage.attackFlag = AttackFlags.NORMAL;
                    int paintime = skill.dwPainTime;
                    for (int i = 0; i < numberofdmg; i++)
                    {
                        
                        Delayer.RegisterDelayedAction(delegate() { DelayedActions.Delayed_senddamage(senddamage); }, paintime * i);

                    }
                }
                
            }
            if (DiceRoller.Roll(c_attributes[FlyFF.DST_CHR_CHANCESTEALHP])) //if player has absorb buff and it work
            {
               //[Divinepunition] I need to sniff packet to know more. For the moment i consider that player use skill 229 lvl3 so 20HP
                    if (client.c_attributes[FlyFF.DST_HP] < client.c_data.f_MaxHP) //no need to add hp if max
                    {
                        client.c_attributes[FlyFF.DST_HP] += 20;
                        if (client.c_attributes[FlyFF.DST_HP] > client.c_data.f_MaxHP) //can't have more
                            client.c_attributes[FlyFF.DST_HP] = client.c_data.f_MaxHP;
                        client.SendPlayerAttribSet(FlyFF.DST_HP,client.c_attributes[FlyFF.DST_HP]);
                    }
            }
                

            return;

        }
    }

    

    
    #endregion

    #region Use Skill Class ~ Function for each group
    public class UseSkill
    {
        /// <summary>
        /// This function manage Damage done by all the skills
        /// </summary>
        /// <param name="Monster mob">Actually this function work only for monster, mob is the target</param>
        /// <param name="Client me">The caster</param>
        /// <param name="int dmg">damage calculated in the skill's function</param>
        /// <param name="CPSUData PSUData">Structure tthat contain information about the skill casted</param>
        /// <param name="int AttackFlag">Flag corresponding to attack type</param>
        /// <returns></returns>
        public static void SetDamageEffect(Monster mob, Client me, int dmg, CPSUData PSUData, int AttackFlag)
        {
            int real_damage = mob.c_attributes[FlyFFAttributes.DST_HP] - dmg < 0 ? mob.c_attributes[FlyFFAttributes.DST_HP] : dmg;
            mob.c_attributes[FlyFFAttributes.DST_HP] -= real_damage;
            Mover mover = mob as Mover;
            if (PSUData.Skill.dwNameID == 131 || PSUData.Skill.dwNameID == 222) //power swing||triple shot
                mover.SendMoverDamaged(PSUData.Caster.dwMoverID, dmg, AttackFlags.KNOCKBACK, mover.c_position, 0);
            else
                mover.SendMoverDamaged(PSUData.Caster.dwMoverID, dmg, AttackFlags.NORMAL, mover.c_position, 0);
            mover.c_target = me as Mover;
            mover.bIsFighting = true;//like this monster will attack me
            mover.bIsFollowing = true;
            if (mob.c_attributes[FlyFFAttributes.DST_HP] <= 0)
            {
                int xprate = 1;
                if (me.c_data.dwLevel < 60) xprate = Server.exp_rate;
                else if (me.c_data.dwLevel >= 60 && me.c_data.dwLevel < 90) xprate = Server.exp_rate60;
                else if (me.c_data.dwLevel >= 90) xprate = Server.exp_rate90;
                me.c_data.qwExperience += (int)((double)(mob.Data.mobExpPoints) * (double)xprate * me.c_data.f_RateModifierEXP);
                me.OnCheckLevelGroup();
                me.SendPlayerCombatInfo();
                mob.SendMoverDeath();
                mob.c_target = null;
                mob.mob_OnDeath();
                me.DropMobItems(mob, me, me.c_data.dwMapID);
            }
        }
        /// <summary>
        /// This function manage effect of the attacking skills. For exemple if an attacking skill stealhp we will manage here the effect
        /// </summary>
        /// <param name="CPSUData PSUData">Structure tthat contain information about the skill casted</param>
        /// <returns></returns>
        public static void DoSkillEffect(CPSUData PSUData, int dmg)
        {
            Mover mover = PSUData.Target.AsMover;
            Client client = PSUData.Caster;
            Buff antibuff = new Buff();
            bool bantibuffalreadyonhim = false;

            switch (PSUData.Skill.dwNameID)
            {
                #region Skill 65|70|112
                case 65://hot air
                    Log.Write(Log.MessageType.debug, "I'm in DoSkillEffect for skill id 65");
                    Skills skill = PSUData.Skill;
                    antibuff = new Buff();
                    antibuff._skill = skill;
                    antibuff.dwTime = skill.dwSkillTime;
                    for (int j = 0; j < mover.antibuffs.Count; j++) //we check if the mover has already the antibuff
                    {
                        if (mover.antibuffs[j]._skill.dwNameID == PSUData.Skill.dwNameID)
                            bantibuffalreadyonhim = true;
                    }
                    if (!bantibuffalreadyonhim) //if mof has already the effect we don't need to send it another time
                    {
                        if (!AntiBuff(PSUData, true))
                            Log.Write(Log.MessageType.error, "Antibuff function doesn't work for this beeding effect skill...");
                    }
                    int HPLost = dmg / 10;
                    int timedelay = skill.dwPainTime;
                    int timeduration = skill.dwSkillTime;
                    int numberofpain = 1 + timeduration / timedelay;
                    Mover caster = PSUData.Caster as Mover;
                    int calltime = timedelay;
                    DelayedActions.str_delayedSendDamage delayeddamage = new DelayedActions.str_delayedSendDamage();
                    delayeddamage.caster = caster;
                    delayeddamage.attackFlag = AttackFlags.MAGIC;
                    delayeddamage.damage = HPLost;
                    delayeddamage.target = PSUData.Target.AsMover;
                    
                    for (int i = 0; i < numberofpain; i++)
                    {

                        ulong qwID = Delayer.RegisterDelayedAction(delegate() { DelayedActions.Delayed_senddamage(delayeddamage); }, timedelay * (i + 1));
                        DelayedActions.Delaycontent delayedcontent = new DelayedActions.Delaycontent();
                        delayedcontent.Index = qwID;
                        delayedcontent.Refvalue = HPLost;
                        delayedcontent.MethodName = DelayedActions.METHOD_SENDAMAGE;
                        PSUData.Target.AsMover.list_delayedprocess.Add(delayedcontent);
                    }

                    break;
                case 70: //strong wind
                    Log.Write(Log.MessageType.debug, "I'm in DoSkillEffect for skill id 70");
                    PSUData.Target.AsMover.followMover(PSUData.Caster.dwMoverID, (int)PacketCommands.RANGE_AVERAGE);
                    break;
                case 112: //Bloody strike
                    Log.Write(Log.MessageType.debug, "I'm in DoSkillEffect for skill id 112");
                    int stealHP = (dmg * PSUData.Skill.dwAdjParamVal1) / 100;
                    PSUData.Caster.c_attributes[FlyFFAttributes.DST_HP] += stealHP;
                    if (PSUData.Caster.c_attributes[FlyFFAttributes.DST_HP] > PSUData.Caster.c_data.f_MaxHP)
                        PSUData.Caster.c_attributes[FlyFFAttributes.DST_HP] = PSUData.Caster.c_data.f_MaxHP;
                    PSUData.Caster.SendPlayerAttribSet(FlyFFAttributes.DST_HP, PSUData.Caster.c_attributes[FlyFFAttributes.DST_HP]);
                    break;
                #endregion
                #region Send antibuff without probability
                case 30: //firestrike
                case 33: //spring water
                case 134://Paindealer  [Divinepunition] Need to sniff packet from a knight to check this cause i'm not sure it send a buff packet
                case 220: //silent shot send a silence on a target
                    Log.Write(Log.MessageType.debug, "I'm in DoSkillEffect for skill id 30/33/134/220");
                    for (int j = 0; j < mover.antibuffs.Count; j++) //we check if the mover has already the antibuff
                    {
                        if (mover.antibuffs[j]._skill.dwNameID == PSUData.Skill.dwNameID)
                            bantibuffalreadyonhim = true;
                    }
                    if (!bantibuffalreadyonhim) //if mof has already the effect we don't need to send it another time
                    {
                        if (!AntiBuff(PSUData, true))
                            Log.Write(Log.MessageType.error, "Antibuff function doesn't wok for Pain dealer skill...");

                    }
                    break;
                #endregion
                #region send antibuff with probability
                case 120: //spike stone
                case 201: //crossline
                case 211:
                case 135: //Power Stump [Divinepunition] actually i do it like stonehand but i nee to sniff packet with a knight to know more
                    Log.Write(Log.MessageType.debug, "I'm in DoSkillEffect for skill id 120/201/211/135");
                    antibuff._skill = PSUData.Skill;
                    antibuff.dwTime = PSUData.Skill.dwSkillTime;


                    for (int j = 0; j < mover.antibuffs.Count; j++) //we check if the mover has already the antibuff
                    {
                        if (mover.antibuffs[j]._skill.dwNameID == antibuff._skill.dwNameID)
                            bantibuffalreadyonhim = true;
                    }
                    if (!bantibuffalreadyonhim) //if mof has already the effect we don't need to send it another time
                    {
                        if (DiceRoller.Roll(antibuff._skill.dwProbability))
                        {
                            Log.Write(Log.MessageType.debug, "I'm in DoSkillEffect for skill id 65 - Probability success");
                            mover.SendMoverBuff(antibuff, 1);
                            mover.bIsBlocked = true;
                            mover.SendChangeState(antibuff.dwTime);
                            client.SendPlayerTargetStateInfos(mover.dwMoverID, antibuff);
                            mover.c_attributes[antibuff._skill.dwDestParam1] += antibuff._skill.dwAdjParamVal1;
                            mover.SendMoverAttribRaise(antibuff._skill.dwDestParam1, antibuff._skill.dwAdjParamVal1, -1);

                            mover.antibuffs.Add(antibuff);

                            //ok now we send to remove antibuff                                    
                            ulong qwID = Delayer.RegisterDelayedAction(delegate() { DelayedActions.Delayed_RemoveBuffonMover(mover, client, antibuff, true, true); }, antibuff._skill.dwSkillTime);
                            /* [Divinepunition] Note : if this antibuff is send on a Client.we add the delayed action to the list of this client
                            * cause someone can cure him and remove the effect. So no need to keep this in delayer list. In this case we could wrote this :
                            DelayedActions.Delaycontent delay = new DelayedActions.Delaycontent(qwID, "Delayed_RemoveBuffonMover", skill.dwNameID);
                            clienttarget.Delayedactionslist.Add(delay);*/
                            DelayedActions.Delaycontent delayedcontent = new DelayedActions.Delaycontent();
                            delayedcontent.Index = qwID;
                            delayedcontent.Refvalue = PSUData.Skill.dwNameID;
                            delayedcontent.MethodName = DelayedActions.METHOD_REMOVEBUFF;
                            PSUData.Target.AsMover.list_delayedprocess.Add(delayedcontent);
                        }
                    }

                    break;
                #endregion
                #region Send damage with time and antibuff without probability
                case 154: //Blood fist
                case 179: //Poison cloud
                case 205: //deadly swing
                case 219: //poison arrow
                    if (DiceRoller.Roll(PSUData.Skill.dwProbability))
                    {
                        Log.Write(Log.MessageType.debug, "I'm in DoSkillEffect for skill id 154/179/205/219 - probability success");
                        skill = PSUData.Skill;
                        
                        for (int j = 0; j < mover.antibuffs.Count; j++) //we check if the mover has already the antibuff
                        {
                            if (mover.antibuffs[j]._skill.dwNameID == PSUData.Skill.dwNameID)
                                bantibuffalreadyonhim = true;
                        }
                        if (!bantibuffalreadyonhim) //if mof has already the effect we don't need to send it another time
                        {
                            if (!AntiBuff(PSUData, false))
                                Log.Write(Log.MessageType.error, "Antibuff function doesn't work for this skill...");
                        }
                        HPLost = skill.dwdestData1;
                        timedelay = skill.dwPainTime;
                        timeduration = skill.dwSkillTime;
                        numberofpain = timeduration / timedelay;
                        caster = PSUData.Caster as Mover;
                        calltime = timedelay;
                        delayeddamage = new DelayedActions.str_delayedSendDamage();
                        delayeddamage.caster = caster;
                        delayeddamage.attackFlag = AttackFlags.NORMAL;
                        delayeddamage.damage = HPLost;
                        delayeddamage.target = PSUData.Target.AsMover;
                        
                        for (int i = 0; i < numberofpain; i++)
                        {

                            ulong qwID = Delayer.RegisterDelayedAction(delegate() { DelayedActions.Delayed_senddamage(delayeddamage); }, timedelay * (i + 1));
                            DelayedActions.Delaycontent delayedcontent = new DelayedActions.Delaycontent();
                            delayedcontent.Index = qwID;
                            delayedcontent.Refvalue = HPLost;
                            delayedcontent.MethodName = DelayedActions.METHOD_SENDAMAGE;
                            PSUData.Target.AsMover.list_delayedprocess.Add(delayedcontent);
                        }

                    }
                    break;
                #endregion
                #region Psychic bomb
                case 162://PsychicBomb  
                    Log.Write(Log.MessageType.debug, "I'm in DoSkillEffect for skill id 162");
                    if (!AntiBuff(PSUData, true))
                        Log.Write(Log.MessageType.error, "Antibuff function doesn't wok for Pain dealer skill...");
                    break;
                #endregion
                #region Psychic wall
                case 163: //Psychic wall
                    Log.Write(Log.MessageType.debug, "I'm in DoSkillEffect for skill id 163");
                    //calculate new position temporary value but need to find a formula for this
                    Point position = DiceRoller.RandomPointInCircle(mover.c_position, 5.5f);
                    mover.SendMoverInstantMove(position);
                    break;
                #endregion
                #region send antibuff without probability only if monster don't have already this antibuff
                case 197: //slowstep
                case 173: //electric shock
                case 202: //auto shot
                    Log.Write(Log.MessageType.debug, "I'm in DoSkillEffect for skill id 197/173/202");
                    for (int j = 0; j < mover.antibuffs.Count; j++) //we check if the mover has already the antibuff
                    {
                        if (mover.antibuffs[j]._skill.dwNameID == PSUData.Skill.dwNameID)
                            bantibuffalreadyonhim = true;
                    }
                    if (!bantibuffalreadyonhim) //if mof has already the effect we don't need to send it another time
                    {
                        if (!AntiBuff(PSUData, true))
                            Log.Write(Log.MessageType.error, "Antibuff function doesn't wok for Pain dealer skill...");

                    }
                    break;
                #endregion
                #region only for snitch skill
                case 203://snith
                    Log.Write(Log.MessageType.debug, "I'm in DoSkillEffect for skill id 203");
                    //we take some money
                    Client target = mover as Client;

                    int penya = DiceRoller.RandomNumber(10, 10 * PSUData.Skill.dwSkillLvl);
                    if (target != null)
                    {
                        if (target.c_data.dwPenya > penya)
                            target.c_data.dwPenya -= penya;
                        else
                        {
                            penya = target.c_data.dwPenya;
                            target.c_data.dwPenya = 0;
                        }
                        target.SendPlayerPenya();
                    }
                    client.c_data.dwPenya += penya;
                    client.SendPlayerPenya();
                    client.SendMessageInfoNotice("You have stolen {0} penya", penya);
                    //now we must delete dark illusion buff
                    Buff buff = new Buff();
                    Skill skilldark = client.GetSkillByID(193);
                    buff._skill = Skills.getSkillByNameIDAndLevel(skilldark.dwSkillID, skilldark.dwSkillLevel);
                    buff.dwTime = 0;
                    client.SendPlayerBuff(buff);
                    for (int i = 0; i < client.c_data.buffs.Count; i++)
                    {
                        if (client.c_data.buffs[i]._skill.dwNameID == 193)
                            client.c_data.buffs.Remove(client.c_data.buffs[i]);
                    }
                    break;
                #endregion
                #region Make damage with the time
                case 200: //arrow rain, make damage in the time
                    //if we are here, the first damage is already done, we calculate the number of remaining damage
                    Log.Write(Log.MessageType.debug, "I'm in DoSkillEffect for skill id 200");
                    int numberofdmg = PSUData.Skill.dwSkillTime / PSUData.Skill.dwPainTime;
                    Monster mob = PSUData.Target.AsMonster;
                    DelayedActions.str_delayedSendDamage senddamage = new DelayedActions.str_delayedSendDamage();
                    senddamage.caster = PSUData.Caster as Mover;
                    senddamage.target = PSUData.Target.AsMover;
                    int paintime = PSUData.Skill.dwPainTime;
                    for (int i = 1; i <= numberofdmg; i++)
                    {
                        dmg = PSUData.Caster.c_data.f_CalculateDamageAgainstMob(mob);
                        if (!SkillFormulas.GetDamageFromSkillFormula(PSUData, ref dmg))
                        {
                            Log.Write(Log.MessageType.info, "No skill formula were found for skill: {0}",
                                PSUData.SkillName.ToString());
                        }

                        senddamage.damage = dmg;
                        AttackFlags typeflag = AttackFlags.MAGIC;
                        if (PSUData.Skill.dwNameID == 200) //arrow rain
                            typeflag = AttackFlags.BOW;
                        senddamage.attackFlag = typeflag;
                        ulong qwID = Delayer.RegisterDelayedAction(delegate() { DelayedActions.Delayed_senddamage(senddamage); }, paintime * i);
                        DelayedActions.Delaycontent delayedcontent = new DelayedActions.Delaycontent();
                        delayedcontent.Index = qwID;
                        delayedcontent.Refvalue = dmg;
                        delayedcontent.MethodName = DelayedActions.METHOD_SENDAMAGE;
                        PSUData.Target.AsMover.list_delayedprocess.Add(delayedcontent);
                    }
                    break;
                #endregion
                #region Send Antibuff if probability succ with skillID if this buff is not already on the mover
                case 216: //Ice arrow
                case 118: //ice missible
                case 182: //windfield
                    if (DiceRoller.Roll(PSUData.Skill.dwProbability))
                    {
                        Log.Write(Log.MessageType.debug, "I'm in DoSkillEffect for skill id 118/182/216 - prbability success");
                        for (int j = 0; j < mover.antibuffs.Count; j++) //we check if the mover has already the antibuff
                        {
                            if (mover.antibuffs[j]._skill.dwNameID == PSUData.Skill.dwNameID)
                                bantibuffalreadyonhim = true;
                        }
                        if (!bantibuffalreadyonhim) //if mof has already the effect we don't need to send it another time
                        {
                            if (!AntiBuff(PSUData, true))
                                Log.Write(Log.MessageType.error, "Antibuff function doesn't work ice arrow...");

                        }
                    }
                    break;
                #endregion
                #region Send Antibuff with a different ID and make damage with time
                case 170: //burning fieal
                case 217: //flame arrow
                    if (DiceRoller.Roll(PSUData.Skill.dwProbability))
                    {
                        Log.Write(Log.MessageType.debug, "I'm in DoSkillEffect for skill id 170/217 - probability success");
                        skill = PSUData.Skill;
                        antibuff = new Buff();
                        antibuff._skill = Skills.getSkillByNameIDAndLevel(225, 1);
                        antibuff.dwTime = antibuff._skill.dwSkillTime;
                        for (int j = 0; j < mover.antibuffs.Count; j++) //we check if the mover has already the antibuff
                        {
                            if (mover.antibuffs[j]._skill.dwNameID == PSUData.Skill.dwNameID)
                                bantibuffalreadyonhim = true;
                        }
                        CPSUData PSUData2 = new CPSUData();
                        PSUData2.Skill = antibuff._skill;
                        PSUData2.SkillData = antibuff._skill;
                        PSUData2.Target = PSUData.Target;
                        PSUData2.Caster = PSUData.Caster;
                        PSUData2.actionslotorder = PSUData.actionslotorder;
                        if (!bantibuffalreadyonhim) //if mob has already the effect we don't need to send it another time
                        {
                            if (!AntiBuff(PSUData2, false))
                                Log.Write(Log.MessageType.error, "Antibuff function doesn't work for this beeding effect skill...");
                        }
                        HPLost = 0;
                        if (!SkillFormulas.GetDamageFromSkillFormula(PSUData2, ref HPLost))
                        {
                            Log.Write(Log.MessageType.info, "No skill formula were found for skill: {0}",
                                PSUData.SkillName.ToString());
                        }
                        timedelay = antibuff._skill.dwdestData2;
                        timeduration = antibuff._skill.dwSkillTime;
                        numberofpain = timeduration / timedelay;
                        caster = PSUData.Caster as Mover;
                        calltime = timedelay;
                        delayeddamage = new DelayedActions.str_delayedSendDamage();
                        delayeddamage.caster = caster;
                        delayeddamage.attackFlag = AttackFlags.MAGIC;
                        delayeddamage.damage = HPLost;
                        delayeddamage.target = PSUData.Target.AsMover;
                        
                        for (int i = 0; i < numberofpain; i++)
                        {

                            ulong qwID = Delayer.RegisterDelayedAction(delegate() { DelayedActions.Delayed_senddamage(delayeddamage); }, timedelay * (i + 1));
                            DelayedActions.Delaycontent delayedcontent = new DelayedActions.Delaycontent();
                            delayedcontent.Index = qwID;
                            delayedcontent.Refvalue = HPLost;
                            delayedcontent.MethodName = DelayedActions.METHOD_SENDAMAGE;
                            PSUData.Target.AsMover.list_delayedprocess.Add(delayedcontent);
                        }

                    }
                    break;
                #endregion
            }
        }
        /// <summary>
        /// This function manage all the Buffs skills
        /// </summary>       
        /// <param name="CPSUData PSUData">Structure tthat contain information about the skill casted</param>
        /// <returns></returns>
        public static bool Buffs(CPSUData PSUData)
        {
            if (!PSUData.IsTargetSet)
                return false;
            Buff buff = new Buff();
            Skills skill = new Skills();
            skill = PSUData.Skill;
            buff._skill = skill;
            int msg = SkillConsuming.Consume(skill, PSUData.Caster);
            if (msg != 0)
            {
                PSUData.Caster.SendMessageInfo(msg);
                return false;
            }
            buff.dwTime = Skills.getBuffInterval(skill, PSUData.Caster.c_attributes[FlyFF.DST_INT]);
            if (PSUData.IsFromActionSlot)
            {
                if (PSUData.actionslotorder == 1)
                    PSUData.Caster.SendPlayerSkillMotion(PSUData.Skill, PSUData.Target.AsMover, PacketCommands.ACTIONSLOT_ORDER_1, PacketCommands.UNKNOWN_MOTIONVALUE3);
                else
                    PSUData.Caster.SendPlayerSkillMotion(PSUData.Skill, PSUData.Target.AsMover, PacketCommands.ACTIONSLOT_ORDER_OTHER, PacketCommands.UNKNOWN_MOTIONVALUE0);
            }
            else
                PSUData.Caster.SendPlayerSkillMotion(PSUData.Skill, PSUData.Target.AsMover, PacketCommands.ACTIONSLOT_ORDER_NO, PacketCommands.UNKNOWN_MOTIONVALUE3);
            PSUData.Caster.SendPlayerSkillUnknowpacket();
            Skills.setBuffEffect(buff, PSUData.Target.AsClient);
            return true;
        }
        /// <summary>
        /// This function manage Attack skills
        /// </summary>       
        /// <param name="CPSUData PSUData">Structure tthat contain information about the skill casted</param>
        /// <returns></returns>
        public static bool Attack(CPSUData PSUData)
        {
            Log.Write(Log.MessageType.debug, "I'm on attack");
            if (!PSUData.Target.IsMonster)
                return false;
            Monster mob = PSUData.Target.AsMonster;
            Client me = PSUData.Caster;
            Skills skill = PSUData.Skill;
            //sneach | backstab |vitalstab need the player is buffed with darkillusion
            if (skill.dwNameID == 203 || skill.dwNameID == 211 || skill.dwNameID == 214)
            {
                bool haveDI = false;
                for (int i = 0; i < me.c_data.buffs.Count; i++)
                {
                    if (me.c_data.buffs[i]._skill.dwNameID == 203)
                        haveDI = true;
                }
                if (!haveDI)
                {
                    me.SendMessageInfoNotice("You are not under dark illusion, you can't use this skill");
                    me.SendPlayerSkillEnd(); // Very important, else client will freeze!
                    return true;
                }
            }
            int msg = SkillConsuming.Consume(skill, PSUData.Caster);
            if (msg != 0)
            {
                PSUData.Caster.SendMessageInfo(msg);
                return false;
            }
            int numberattack = me.NumberOfAttack(PSUData);
            //we must go to monster to attack him
            Mover me2 = me as Mover;
            //we must determin type of range for this attack
            int attacktyperange = 29;
            if (me.c_data.dwClass == 2 || me.c_data.dwClass == 8 || me.c_data.dwClass == 9 || me.c_data.dwClass == 18 || me.c_data.dwClass == 19 || me.c_data.dwClass == 26 || me.c_data.dwClass == 27)
                attacktyperange = 30;
            if (PSUData.IsFromActionSlot)
            {
                if (PSUData.actionslotorder == 1)
                {
                    //me.SendPlayerAttackMotion(attacktyperange, mob.dwMoverID); ? error i thing :p
                    PSUData.Caster.SendPlayerSkillMotion(PSUData.Skill, PSUData.Target.AsMover, PacketCommands.ACTIONSLOT_ORDER_1, PacketCommands.UNKNOWN_MOTIONVALUE3);
                }
                else
                    PSUData.Caster.SendPlayerSkillMotion(PSUData.Skill, PSUData.Target.AsMover, PacketCommands.ACTIONSLOT_ORDER_OTHER, PacketCommands.UNKNOWN_MOTIONVALUE0);
            }
            else
            {
                me.SendPlayerAttackMotion(attacktyperange, mob.dwMoverID);
                PSUData.Caster.SendPlayerSkillMotion(PSUData.Skill, PSUData.Target.AsMover, PacketCommands.ACTIONSLOT_ORDER_NO, PacketCommands.UNKNOWN_MOTIONVALUE1);
            }
            PSUData.Caster.SendPlayerSkillUnknowpacket();
            for (int i = 1; i <= numberattack; i++)
            {
                int dmg = me.c_data.f_CalculateDamageAgainstMob(mob);
                if (!SkillFormulas.GetDamageFromSkillFormula(PSUData, ref dmg))
                {
                    Log.Write(Log.MessageType.info, "No skill formula were found for skill: {0}",
                        PSUData.SkillName.ToString());
                }
                if (PSUData.Skill.dwNameID == 212) //if the skill is hitofpenya we must check penya needed
                {
                    int numberpenya = DiceRoller.RandomNumber(100 * PSUData.Skill.dwSkillLvl, 200 * PSUData.Skill.dwSkillLvl); //number of penya needed
                    if (me.c_data.dwPenya < numberpenya)
                        dmg = dmg / 2; //if you don't have penya dammage greatly decrease
                    else
                    {
                        me.c_data.dwPenya -= numberpenya;
                        me.SendPlayerPenya();
                        me.SendMessageInfoNotice("{0} penya used", numberpenya);
                    }
                }
                //[Divinepunition] we send damage after skill motion
                DelayedActions.str_delayedSendDamage senddamage = new DelayedActions.str_delayedSendDamage();
                senddamage.caster = PSUData.Caster;
                senddamage.target = PSUData.Target.AsMover;
                senddamage.damage = dmg;
                senddamage.attackFlag = AttackFlags.NORMAL;
                
                int calltime = skill.dwComboSkillTime;
                if (i > 1)
                    calltime += skill.dwComboSkillTime;
                Log.Write(Log.MessageType.debug, "we had created a delayed send damage with dmg = {0} send in {1} millisecond",dmg,calltime);
                ulong qwID = Delayer.RegisterDelayedAction(delegate() { DelayedActions.Delayed_senddamage(senddamage); }, calltime);
                if (qwID != null) Log.Write(Log.MessageType.debug, "seem that the delayed action have been send");
                DelayedActions.Delaycontent delayedcontent = new DelayedActions.Delaycontent();
                delayedcontent.Index = qwID;
                delayedcontent.Refvalue = dmg;
                delayedcontent.MethodName = DelayedActions.METHOD_SENDAMAGE;
                PSUData.Target.AsMover.list_delayedprocess.Add(delayedcontent);
                DoSkillEffect(PSUData, dmg); //to manage effect of the skills

            }

            return true;
        }
        /// <summary>
        /// This function manage all AOE skills
        /// </summary>       
        /// <param name="CPSUData PSUData">Structure tthat contain information about the skill casted</param>
        /// <returns></returns>
        public static bool AoE(CPSUData PSUData)
        {
            /*we must determin type of AOE : aroung player/in front of player or around target...
             * 1 : one target
             * 2 : arround target at range (near target 100%, arround 100% damage)
             * 3: arround target at range (near target 100%, arount minimum damage)
             * 4: arround player (arround 100% damage)
             * 5: ?
             * 6: All ennemi in front of player on a line on a certain long value
             * 7: arround one target (target 0% damage, arround 100%)
             * 8: party of the target*/
            Log.Write(Log.MessageType.debug, "I'm on AOE");

            Mover mover = PSUData.Target.AsMover;
            Client me = PSUData.Caster;
            Skills skill = PSUData.Skill;

            int msg = SkillConsuming.Consume(skill, PSUData.Caster);

            if (msg != 0)
            {
                PSUData.Caster.SendMessageInfo(msg);
                return false;
            }
            int attacktyperange = 29;
            if (me.c_data.dwClass == 2 || me.c_data.dwClass == 8 || me.c_data.dwClass == 9 || me.c_data.dwClass == 18 || me.c_data.dwClass == 19 || me.c_data.dwClass == 26 || me.c_data.dwClass == 27)
                attacktyperange = 30;
            if (PSUData.IsFromActionSlot)
            {
                if (PSUData.actionslotorder == 1)
                {
                    //me.SendPlayerAttackMotion(attacktyperange, mob.dwMoverID); ? error i thing :p
                    PSUData.Caster.SendPlayerSkillMotion(PSUData.Skill, PSUData.Target.AsMover, PacketCommands.ACTIONSLOT_ORDER_1, PacketCommands.UNKNOWN_MOTIONVALUE3);
                }
                else
                    PSUData.Caster.SendPlayerSkillMotion(PSUData.Skill, PSUData.Target.AsMover, PacketCommands.ACTIONSLOT_ORDER_OTHER, PacketCommands.UNKNOWN_MOTIONVALUE0);
            }
            else
            {
                me.SendPlayerAttackMotion(attacktyperange, mover.dwMoverID);
                PSUData.Caster.SendPlayerSkillMotion(PSUData.Skill, PSUData.Target.AsMover, PacketCommands.ACTIONSLOT_ORDER_NO, PacketCommands.UNKNOWN_MOTIONVALUE1);
            }
            PSUData.Caster.SendPlayerSkillUnknowpacket();

            //k now we need to send damage to all monster in the area
            bool magical = Skills.isMagicAttackSkill(PSUData.Skill.dwNameID);

            SearchMonsterAndApplyDamage(PSUData, me, mover, magical);


            return true;
        }
        /// <summary>
        /// This function manage all healing skills
        /// </summary>       
        /// <param name="CPSUData PSUData">Structure tthat contain information about the skill casted</param>
        /// <returns></returns>
        public static bool Heal(CPSUData PSUData)
        {
            Skills curSkill = new Skills();
            curSkill = PSUData.Skill;
            Skills.healTarget(curSkill, PSUData.Target.AsClient, PSUData.Caster);
            int msg = SkillConsuming.Consume(curSkill, PSUData.Caster);
            if (msg != 0)
            {
                PSUData.Caster.SendMessageInfo(msg);
                return false;
            }
            if (PSUData.IsFromActionSlot)
            {
                if (PSUData.actionslotorder == 1)
                    PSUData.Caster.SendPlayerSkillMotion(PSUData.Skill, PSUData.Target.AsMover, PacketCommands.ACTIONSLOT_ORDER_1, PacketCommands.UNKNOWN_MOTIONVALUE1);
                else
                    PSUData.Caster.SendPlayerSkillMotion(PSUData.Skill, PSUData.Target.AsMover, PacketCommands.ACTIONSLOT_ORDER_OTHER, PacketCommands.UNKNOWN_MOTIONVALUE0);
            }
            else
                PSUData.Caster.SendPlayerSkillMotion(PSUData.Skill, PSUData.Target.AsMover, PacketCommands.ACTIONSLOT_ORDER_NO, PacketCommands.UNKNOWN_MOTIONVALUE0);
            PSUData.Caster.SendPlayerSkillUnknowpacket();
            return true;
        }
        /// <summary>
        /// This function manage resurrection skills
        /// </summary>       
        /// <param name="CPSUData PSUData">Structure tthat contain information about the skill casted</param>
        /// <returns></returns>
        public static bool Resurect(CPSUData PSUData)
        {
            Skills curSkill = new Skills();
            curSkill = PSUData.Skill;
            Skills.resurrectPlayer(PSUData.Skill, PSUData.Target.AsClient, PSUData.Caster);
            int msg = SkillConsuming.Consume(curSkill, PSUData.Caster);
            if (msg != 0)
            {
                PSUData.Caster.SendMessageInfo(msg);
                return false;
            }
            PSUData.Caster.SendPlayerSkillMotion(curSkill, PSUData.Target.AsMover, 0, 1);
            PSUData.Caster.SendPlayerSkillUnknowpacket();
            return true;
        }
        /// <summary>
        /// This function manage all the skills that have an effect on party's member
        /// </summary>       
        /// <param name="CPSUData PSUData">Structure tthat contain information about the skill casted</param>
        /// <returns></returns>
        public static bool SkillPartySkills(CPSUData PSUData)
        {
            Skills curSkill = new Skills();
            Client caster = PSUData.Caster;
            curSkill = PSUData.Skill;

            int msg = SkillConsuming.Consume(curSkill, PSUData.Caster);
            if (msg != 0)
            {
                PSUData.Caster.SendMessageInfo(msg);
                return false;
            }
            if (PSUData.IsFromActionSlot)
            {
                if (PSUData.actionslotorder == 1)
                    PSUData.Caster.SendPlayerSkillMotion(PSUData.Skill, PSUData.Target.AsMover, PacketCommands.ACTIONSLOT_ORDER_1, PacketCommands.UNKNOWN_MOTIONVALUE3);
                else
                    PSUData.Caster.SendPlayerSkillMotion(PSUData.Skill, PSUData.Target.AsMover, PacketCommands.ACTIONSLOT_ORDER_OTHER, PacketCommands.UNKNOWN_MOTIONVALUE0);
            }
            else
                PSUData.Caster.SendPlayerSkillMotion(PSUData.Skill, PSUData.Target.AsMover, PacketCommands.ACTIONSLOT_ORDER_NO, PacketCommands.UNKNOWN_MOTIONVALUE3);
            PSUData.Caster.SendPlayerSkillUnknowpacket();
            Skills.SkillPartySkills(curSkill, PSUData.Caster);
            return true;
        }
        /// <summary>
        /// This function manage the use of Blink pool
        /// </summary>       
        /// <param name="CPSUData PSUData">Structure tthat contain information about the skill casted</param>
        /// <returns></returns>
        public static bool Teleport(CPSUData PSUData)
        {
            Skills skill = PSUData.Skill;
            int msg = SkillConsuming.Consume(skill, PSUData.Caster);
            if (msg != 0)
            {
                PSUData.Caster.SendMessageInfo(msg);
                return false;
            }
            PSUData.Caster.SendPlayerSkillMotion(PSUData.Skill, PSUData.Target.AsMover, 0, 1);
            PSUData.Caster.SendPlayerSkillUnknowpacket();
            switch (skill.dwNameID)
            {
                case 206: //pulling
                case 238: //drawing
                    //temporary random value in circle near the player for the good we should calculate
                    Point position = DiceRoller.RandomPointInCircle(PSUData.Caster.c_position, 1.5f);

                    PSUData.Target.AsMover.SendMoverInstantMove2(position);
                    PSUData.Target.AsMover.SendMoverInstantMove(position);
                    break;



            }
            return true;

        }
        /// <summary>
        /// This function manage all the debuffs skills
        /// </summary>       
        /// <param name="CPSUData PSUData">Structure that contain information about the skill casted</param>
        /// <returns></returns>
        public static bool Debuff(CPSUData PSUData)
        {
            Client target = PSUData.Target.AsClient;
            Client caster = PSUData.Caster;
            Skills skill = PSUData.Skill;
            int msg = SkillConsuming.Consume(skill, PSUData.Caster);
            if (msg != 0)
            {
                PSUData.Caster.SendMessageInfo(msg);
                return false;
            }
            //motion packet depend if it's from action slot or not
            if (PSUData.IsFromActionSlot)
            {
                if (PSUData.actionslotorder == 1)
                    caster.SendPlayerSkillMotion(skill, target, PacketCommands.ACTIONSLOT_ORDER_1, PacketCommands.UNKNOWN_MOTIONVALUE7);
                else
                    caster.SendPlayerSkillMotion(skill, target, PacketCommands.ACTIONSLOT_ORDER_OTHER, PacketCommands.UNKNOWN_MOTIONVALUE0);
            }
            else
                PSUData.Caster.SendPlayerSkillMotion(skill, target, PacketCommands.ACTIONSLOT_ORDER_NO, PacketCommands.UNKNOWN_MOTIONVALUE1);
            caster.SendPlayerSkillUnknowpacket();
            if (skill.dwNameID == 213) //escape - jester
            {
                if (caster.antibuffs.Count > 0)
                {
                    for (int i = 0; i < caster.antibuffs.Count; i++)
                    {
                        switch (caster.antibuffs[i]._skill.dwNameID)
                        {

                            case 201:
                            case 211:
                            case 227: //stun effect
                            case 235:
                            case 242: //hero skill petrification
                                Buff antibuff = caster.antibuffs[i];
                                DelayedActions.Delayed_RemoveBuffonMover(PSUData.Target.AsMover, PSUData.Caster, antibuff, true, true);
                                break;
                        }
                    }
                }
            }
            if (skill.dwNameID == 243) //escape - jester
            {
                Buff.ClearAllBuffs(target);
            }
            if (skill.dwNameID == 244) //teleport - ringmaster
            {
                // New revival regions code.
                RevivalRegion region = null;
                for (int i = 0; i < WorldServer.data_resrgns.Count; i++)
                {
                    if (WorldServer.data_resrgns[i].IsInRegion(caster.c_position, caster.c_data.dwMapID))
                    {
                        region = WorldServer.data_resrgns[i];
                        break;
                    }
                }
                if (region == null)
                    region = WorldHelper.DefaultRevivalRegion;
                // set stuff o_O
                caster.c_position = new Point(region.c_destiny.x, caster.c_position.y, region.c_destiny.z, caster.c_position.angle);
                if (region.dwDestMap != caster.c_data.dwMapID)
                {
                    caster.c_data.dwMapID = region.dwDestMap;
                    caster.SendPlayerMapTransfer();
                }
                else
                {
                    caster.SendMoverNewPosition();
                }
            }
            return true;
        }
        /// <summary>
        /// This function manage all cure skills
        /// </summary>       
        /// <param name="CPSUData PSUData">Structure that contain information about the skill casted</param>
        /// <returns></returns>
        public static bool Cure(CPSUData PSUData)
        {

            if (!PSUData.IsTargetSet)
                return false;
            Skills skill = new Skills();

            skill = PSUData.Skill;

            int msg = SkillConsuming.Consume(skill, PSUData.Caster);
            if (msg != 0)
            {
                PSUData.Caster.SendMessageInfo(msg);
                return false;
            }
            Mover target = PSUData.Target.AsMover;
            Client caster = PSUData.Caster;

            //ok now we will find antibuff list of the mover and send a remove antibuff for each
            if (target.antibuffs.Count == 0)
                return true;
            for (int i = 0; i < target.antibuffs.Count; i++)
            {
                Buff curantibuff = target.antibuffs[i];

                DelayedActions.Delayed_RemoveBuffonMover(target, caster, curantibuff, true, true);
                target.antibuffs.Remove(target.antibuffs[i]);
            }

            return true;
        }
        /// <summary>
        /// This function manage all the buffs skills that make some effect on the other
        /// </summary>       
        /// <param name="CPSUData PSUData">Structure that contain information about the skill casted</param>
        /// <returns></returns>
        public static bool BuffWithEffect(CPSUData PSUData)
        {
            Log.Write(Log.MessageType.debug, "Enter into buffwitheffect");
            if (!PSUData.IsTargetSet)
                return false;
            Buff buff = new Buff();
            Skills skill = new Skills();

            skill = PSUData.Skill;
            buff._skill = skill;
            int msg = SkillConsuming.Consume(skill, PSUData.Caster);
            if (msg != 0)
            {
                PSUData.Caster.SendMessageInfo(msg);
                return false;
            }
            buff.dwTime = Skills.getBuffInterval(skill, PSUData.Caster.c_attributes[FlyFF.DST_INT]);
            PSUData.Caster.SendPlayerSkillMotion(skill, PSUData.Target.AsMover, 0, 3);
            PSUData.Caster.SendPlayerSkillUnknowpacket();
            //we send buff and skill motion
            PSUData.Caster.SendPlayerBuff(buff);
            PSUData.Caster.c_data.buffs.Add(buff);
            //we need to put a timer :
            PSUData.Caster.isBuffed = true;
            return true;
        }
        /// <summary>
        /// This function manage all the skills that change target status
        /// </summary>       
        /// <param name="CPSUData PSUData">Structure that contain information about the skill casted</param>
        /// <returns></returns>
        public static bool AntiBuff(CPSUData PSUData, bool changestate)
        {
            if (PSUData.Skill.dwNameID == 239 || PSUData.Skill.dwNameID == 242) //if skill silence hero jst or petrification
                if (!DiceRoller.Roll(PSUData.Skill.dwProbability)) //if we do more than probability
                    return true;

            Mover target = PSUData.Target.AsMover; //for the moment i will only code for player vs monster so
            Monster mob = null;
            if (PSUData.Target.AsMonster != null)
                mob = PSUData.Target.AsMonster;
            if (mob == null)
                return false; //this is player vs player
            Client caster = PSUData.Caster;
            Skills skill = PSUData.Skill;
            Buff antibuff = new Buff();
            antibuff._skill = skill;
            antibuff.dwTime = skill.dwSkillTime;

            #region sendmotion
            int msg = SkillConsuming.Consume(skill, PSUData.Caster);
            if (msg != 0)
            {
                PSUData.Caster.SendMessageInfo(msg);
                return false;
            }

            //motion packet depend if it's from action slot or not
            if (PSUData.IsFromActionSlot)
            {
                if (PSUData.actionslotorder == 1)
                    caster.SendPlayerSkillMotion(skill, target, PacketCommands.ACTIONSLOT_ORDER_1, PacketCommands.UNKNOWN_MOTIONVALUE7);
                else
                    caster.SendPlayerSkillMotion(skill, target, PacketCommands.ACTIONSLOT_ORDER_OTHER, PacketCommands.UNKNOWN_MOTIONVALUE0);
            }
            else
                PSUData.Caster.SendPlayerSkillMotion(skill, target, PacketCommands.ACTIONSLOT_ORDER_NO, PacketCommands.UNKNOWN_MOTIONVALUE1);
            caster.SendPlayerSkillUnknowpacket();

            #endregion
            mob.SendMoverBuff(antibuff, 1);
            if (changestate)
            {
                mob.SendChangeState(skill.dwSkillTime);
            }
            mob.c_attributes[skill.dwDestParam1] += skill.dwAdjParamVal1;
            if (skill.dwDestParam1 < 0) //if we decrease something
                mob.SendMoverAttribDecrease(skill.dwDestParam1, Math.Abs(skill.dwAdjParamVal1));
            else
                mob.SendMoverAttribRaise(skill.dwDestParam1, skill.dwAdjParamVal1, 0x7FFFFFFF);
            if (skill.dwDestParam2 != 0)//if there is a second effect
            {
                mob.c_attributes[skill.dwDestParam2] += skill.dwAdjParamVal2;
                if (skill.dwDestParam2 < 0) //if we decrease something
                    mob.SendMoverAttribDecrease(skill.dwDestParam2, Math.Abs(skill.dwAdjParamVal2));
                else
                    mob.SendMoverAttribRaise(skill.dwDestParam2, skill.dwAdjParamVal2, 0x7FFFFFFF);
            }

            switch (skill.dwNameID)
            {

                case 13:
                    mob.bIsBlocked = true;
                    break;
                case 37: //looting
                case 161: //satanology
                case 162: //psychicbomb
                    mob.c_attributes[FlyFF.DST_SPEED] = 0;
                    mob.bIsBlocked = true;
                    break;
                default: break;
            }
            //ok now we send to remove antibuff
            ulong qwID = Delayer.RegisterDelayedAction(delegate() { DelayedActions.Delayed_RemoveBuffonMover(PSUData.Target.AsMover, PSUData.Caster, antibuff, changestate, true); }, skill.dwSkillTime);
            DelayedActions.Delaycontent delayedcontent = new DelayedActions.Delaycontent();
            delayedcontent.Index = qwID;
            delayedcontent.Refvalue = antibuff._skill.dwNameID;
            delayedcontent.MethodName = DelayedActions.METHOD_REMOVEANTIBUFF;
            PSUData.Target.AsMover.list_delayedprocess.Add(delayedcontent);
            return true;
        }
        /// <summary>
        /// This function manage all Magical attack skills unless the aoe skills
        /// </summary>       
        /// <param name="CPSUData PSUData">Structure that contain information about the skill casted</param>
        /// <returns></returns>
        public static bool MagicalAttack(CPSUData PSUData)
        {
            if (!PSUData.Target.IsMonster)
            {
                Log.Write(Log.MessageType.debug, "Target seem to not be a monster");
                return false;
            }

            Monster mob = PSUData.Target.AsMonster;
            Client me = PSUData.Caster;
            Skills skill = PSUData.Skill;
            int numberofattack = skill.dwSkillCount;
            if (numberofattack == 0)
                numberofattack = 1;
            int msg = SkillConsuming.Consume(skill, PSUData.Caster);
            if (msg != 0)
            {
                PSUData.Caster.SendMessageInfo(msg);
                return false;
            }
            //motion packet depend if it's from action slot or not
            if (PSUData.IsFromActionSlot)
            {
                if (PSUData.actionslotorder == 1)
                    PSUData.Caster.SendPlayerSkillMotion(PSUData.Skill, PSUData.Target.AsMover, PacketCommands.ACTIONSLOT_ORDER_1, PacketCommands.UNKNOWN_MOTIONVALUE7);
                else
                    PSUData.Caster.SendPlayerSkillMotion(PSUData.Skill, PSUData.Target.AsMover, PacketCommands.ACTIONSLOT_ORDER_OTHER, PacketCommands.UNKNOWN_MOTIONVALUE0);
            }
            else
                PSUData.Caster.SendPlayerSkillMotion(PSUData.Skill, PSUData.Target.AsMover, PacketCommands.ACTIONSLOT_ORDER_NO, PacketCommands.UNKNOWN_MOTIONVALUE1);
            PSUData.Caster.SendPlayerSkillUnknowpacket();
            int dmg = 0;
            for (int i = 1; i <= numberofattack; i++)
            {
                dmg = me.c_data.f_CalculateMagicalDamageVsMonster(mob, skill);
                if (!SkillFormulas.GetDamageFromSkillFormula(PSUData, ref dmg))
                {
                    Log.Write(Log.MessageType.info, "No skill formula were found for skill: {0}",
                        PSUData.SkillName.ToString());
                }
                //SetDamageEffect(mob, me, dmg, PSUData, (int)AttackFlags.MAGIC);
                //[Divinepunition] we send damage after skill motion
                DelayedActions.str_delayedSendDamage senddamage = new DelayedActions.str_delayedSendDamage();
                senddamage.caster = PSUData.Caster as Mover;
                senddamage.target = PSUData.Target.AsMover;                
                senddamage.damage = dmg;
                senddamage.attackFlag = AttackFlags.MAGIC;
                ulong qwID = Delayer.RegisterDelayedAction(delegate() { DelayedActions.Delayed_senddamage(senddamage); }, skill.dwComboSkillTime * i);
                DelayedActions.Delaycontent delayedcontent = new DelayedActions.Delaycontent();
                delayedcontent.Index = qwID;
                delayedcontent.Refvalue = dmg;
                delayedcontent.MethodName = DelayedActions.METHOD_SENDAMAGE;
                PSUData.Target.AsMover.list_delayedprocess.Add(delayedcontent);
            }
            DoSkillEffect(PSUData, dmg); //for effect of the skills

            return true;
        }
        /// <summary>
        /// This function for AOE skills will search all monster in the area to apply to them damage. Monster have been put in region list to avoid to test 10000 mob each time an aoe is launch
        /// </summary>       
        /// <param name="CPSUData PSUData">Structure that contain information about the skill casted</param>
        /// <param name="Client me">caster</param>
        /// <param name="Monster mob">target mob</param>
        /// <param name="bool magical">allow to know if an aoe is done by magical skills (for flag)</param>
        /// <returns></returns>
        public static void SearchMonsterAndApplyDamage(CPSUData PSUData, Client me, Mover mover, bool magical)
        {
            Log.Write(Log.MessageType.debug, "I'm in Searchmonsterandapplydamage function");
            int mobnumber = 0;
            int dmg = 0;
            Skills skill = PSUData.Skill;
            for (int i = 0; i < me.c_spawns.Count; i++)
            {
                if (me.c_spawns[i] is Monster)
                {
                    Monster curMonster = me.c_spawns[i] as Monster;
                    if (curMonster == null)
                        continue;
                    switch (PSUData.Skill.dwNameID)
                    {
                        #region Around Player normal damage
                        case 105:
                        case 135:
                        case 139:
                        case 140:
                        case 141: //skills around player
                        case 158:

                            if (curMonster.c_position.IsInCircle(me.c_position, PSUData.Skill.dwSkillRange))
                            {
                                //while based target is the player himself we must change PSUData target to actual mob
                                
                                dmg = me.c_data.f_CalculateDamageAgainstMob(curMonster);
                                if (!SkillFormulas.GetDamageFromSkillFormula(PSUData, ref dmg))
                                {
                                    Log.Write(Log.MessageType.info, "No skill formula were found for skill: {0}",
                                        PSUData.SkillName.ToString());
                                }
                                //SetDamageEffect(curMonster, me, dmg, PSUData, (int)AttackFlags.NORMAL_SKILL);  
                                //[Divinepunition] we send damage after skill motion
                                DelayedActions.str_delayedSendDamage senddamage = new DelayedActions.str_delayedSendDamage();
                                senddamage.caster = PSUData.Caster as Mover;
                                senddamage.target = curMonster;
                                senddamage.damage = dmg;
                                senddamage.attackFlag = AttackFlags.NORMAL_SKILL;
                                Log.Write(Log.MessageType.debug, "We will send a senddamage it will be applyed in {0} miliseconde", skill.dwComboSkillTime + mobnumber);
                                ulong qwID = Delayer.RegisterDelayedAction(delegate() { DelayedActions.Delayed_senddamage(senddamage); }, skill.dwComboSkillTime + mobnumber);
                                DelayedActions.Delaycontent delayedcontent = new DelayedActions.Delaycontent();
                                delayedcontent.Index = qwID;
                                delayedcontent.Refvalue = dmg;
                                delayedcontent.MethodName = DelayedActions.METHOD_SENDAMAGE;
                                curMonster.list_delayedprocess.Add(delayedcontent);
                                PSUData.Target.SetMover(curMonster);
                                DoSkillEffect(PSUData, dmg);
                                mobnumber++;
                            }
                            break;
                        #endregion
                        #region Magical damage around caster
                        
                        case 164:
                        case 166://magical damage around caster
                        case 167:
                            if (curMonster.c_position.IsInCircle(me.c_position, PSUData.Skill.dwSkillRange))
                            {

                                dmg = me.c_data.f_CalculateMagicalDamageVsMonster(curMonster, PSUData.Skill);
                                if (!SkillFormulas.GetDamageFromSkillFormula(PSUData, ref dmg))
                                {
                                    Log.Write(Log.MessageType.info, "No skill formula were found for skill: {0}",
                                        PSUData.SkillName.ToString());
                                }
                                //SetDamageEffect(curMonster, me, dmg, PSUData, (int)AttackFlags.MAGIC);
                                //[Divinepunition] we send damage after skill motion
                                DelayedActions.str_delayedSendDamage senddamage = new DelayedActions.str_delayedSendDamage();
                                senddamage.caster = PSUData.Caster as Mover;
                                senddamage.target = curMonster;
                                senddamage.damage = dmg;
                                senddamage.attackFlag = AttackFlags.MAGIC;
                                Log.Write(Log.MessageType.debug, "We will send a senddamage it will be applyed in {0} miliseconde", skill.dwComboSkillTime + mobnumber);
                                ulong qwID = Delayer.RegisterDelayedAction(delegate() { DelayedActions.Delayed_senddamage(senddamage); }, skill.dwComboSkillTime + mobnumber);
                                DelayedActions.Delaycontent delayedcontent = new DelayedActions.Delaycontent();
                                delayedcontent.Index = qwID;
                                delayedcontent.Refvalue = dmg;
                                delayedcontent.MethodName = DelayedActions.METHOD_SENDAMAGE;
                                curMonster.list_delayedprocess.Add(delayedcontent);
                                PSUData.Target.SetMover(curMonster);
                                DoSkillEffect(PSUData, dmg);
                                mobnumber++;
                            }

                            break;
                        #endregion
                        #region magical damage around caster with time effect
                        case 151: //merkaba magical aoe around player

                            if (curMonster.c_position.IsInCircle(me.c_position, PSUData.Skill.dwSkillRange))
                            {

                                dmg = me.c_data.f_CalculateMagicalDamageVsMonster(curMonster, PSUData.Skill);
                                if (!SkillFormulas.GetDamageFromSkillFormula(PSUData, ref dmg))
                                {
                                    Log.Write(Log.MessageType.info, "No skill formula were found for skill: {0}",
                                        PSUData.SkillName.ToString());
                                }
                                //SetDamageEffect(curMonster, me, dmg, PSUData, (int)AttackFlags.MAGIC);
                                //[Divinepunition] we send damage after skill motion
                                DelayedActions.str_delayedSendDamage senddamage = new DelayedActions.str_delayedSendDamage();
                                senddamage.caster = PSUData.Caster as Mover;
                                senddamage.target = curMonster;
                                senddamage.damage = dmg;
                                senddamage.attackFlag = AttackFlags.MAGIC;
                                Log.Write(Log.MessageType.debug, "We will send a senddamage it will be applyed in {0} miliseconde", skill.dwComboSkillTime + mobnumber);
                                ulong qwID = Delayer.RegisterDelayedAction(delegate() { DelayedActions.Delayed_senddamage(senddamage); }, skill.dwCastingTime+mobnumber);
                                DelayedActions.Delaycontent delayedcontent = new DelayedActions.Delaycontent();
                                delayedcontent.Index = qwID;
                                delayedcontent.Refvalue = dmg;
                                delayedcontent.MethodName = DelayedActions.METHOD_SENDAMAGE;
                                curMonster.list_delayedprocess.Add(delayedcontent);
                                PSUData.Target.SetMover(curMonster);
                                DoSkillEffect(PSUData, dmg);
                                mobnumber++;
                            }

                            break;


                        #endregion
                        #region AOE in line
                        //[Divinepunition] : AOE in line, need to be tested
                        case 218: //percing arrow straight line attack
                        case 153: //percing serpent straight line attack
                            //ok we need to know with monster are in range

                            if (curMonster.c_position.IsInCircle(me.c_position, PSUData.Skill.dwSkillRange))
                            {
                                Log.Write(Log.MessageType.debug, "Just before seee in mob is in line");
                                //now we need to know if this monster is in a line between him and target
                                if (curMonster.c_position.IsInLine(me.c_position, mover.c_position))
                                {
                                    dmg = me.c_data.f_CalculateDamageAgainstMob(curMonster);
                                    if (!SkillFormulas.GetDamageFromSkillFormula(PSUData, ref dmg))
                                    {
                                        Log.Write(Log.MessageType.info, "No skill formula were found for skill: {0}",
                                            PSUData.SkillName.ToString());
                                    }
                                    //SetDamageEffect(curMonster, me, dmg, PSUData, (int)AttackFlags.NORMAL_SKILL);
                                    //[Divinepunition] we send damage after skill motion
                                    DelayedActions.str_delayedSendDamage senddamage = new DelayedActions.str_delayedSendDamage();
                                    senddamage.caster = PSUData.Caster as Mover;
                                    senddamage.target = curMonster;
                                    
                                    senddamage.damage = dmg;
                                    senddamage.attackFlag = AttackFlags.NORMAL_SKILL;
                                    ulong qwID = Delayer.RegisterDelayedAction(delegate() { DelayedActions.Delayed_senddamage(senddamage); }, skill.dwComboSkillTime + mobnumber);
                                    DelayedActions.Delaycontent delayedcontent = new DelayedActions.Delaycontent();
                                    delayedcontent.Index = qwID;
                                    delayedcontent.Refvalue = dmg;
                                    delayedcontent.MethodName = DelayedActions.METHOD_SENDAMAGE;
                                    curMonster.list_delayedprocess.Add(delayedcontent);
                                    PSUData.Target.SetMover(curMonster);
                                    DoSkillEffect(PSUData, dmg);
                                    mobnumber++;
                                }
                            }
                            ;
                            break;
                        #endregion
                        #region AOE around a mob
                        //AOE Arround a mod
                        case 132:
                        case 170: //burningfield
                        case 173: //electricshock
                        case 176: //earthquake
                        case 179: //AOE with effect on time
                        case 183: //Meteoshower
                        case 184: //lighningstorm
                        case 185: //sandstorm
                        case 186: //avalanche
                        case 217: //flame arrow
                            if (curMonster.c_position.IsInCircle(mover.c_position, PSUData.Skill.dwSkillRange))
                            {
                                if (PSUData.Skill.dwNameID == 217)
                                    dmg = me.c_data.f_CalculateDamageAgainstMob(curMonster);
                                else
                                    dmg = me.c_data.f_CalculateMagicalDamageVsMonster(curMonster, PSUData.Skill);
                                if (!SkillFormulas.GetDamageFromSkillFormula(PSUData, ref dmg))
                                {
                                    Log.Write(Log.MessageType.info, "No skill formula were found for skill: {0}",
                                        PSUData.SkillName.ToString());
                                }
                                //SetDamageEffect(curMonster, me, dmg, PSUData, (int)AttackFlags.MAGIC);
                                //[Divinepunition] we send damage after skill motion
                                DelayedActions.str_delayedSendDamage senddamage = new DelayedActions.str_delayedSendDamage();
                                senddamage.caster = PSUData.Caster as Mover;
                                senddamage.target = curMonster;
                                
                                senddamage.damage = dmg;
                                if (PSUData.Skill.dwNameID == 217)
                                    senddamage.attackFlag = AttackFlags.BOW;
                                if (PSUData.Skill.dwNameID == 132)
                                    senddamage.attackFlag = AttackFlags.NORMAL_SKILL;
                                else
                                    senddamage.attackFlag = AttackFlags.MAGIC;
                                Log.Write(Log.MessageType.debug, "We will send a senddamage it will be applyed in {0} miliseconde", skill.dwComboSkillTime + mobnumber);
                                ulong qwID = Delayer.RegisterDelayedAction(delegate() { DelayedActions.Delayed_senddamage(senddamage); }, skill.dwComboSkillTime + mobnumber);
                                DelayedActions.Delaycontent delayedcontent = new DelayedActions.Delaycontent();
                                delayedcontent.Index = qwID;
                                delayedcontent.Refvalue = dmg;
                                delayedcontent.MethodName = DelayedActions.METHOD_SENDAMAGE;
                                curMonster.list_delayedprocess.Add(delayedcontent);
                                PSUData.Target.SetMover(curMonster);
                                DoSkillEffect(PSUData, dmg);
                                mobnumber++;
                            }


                            break;
                        #endregion
                        default:
                            me.SendMessageHud("Skill not implemented"); break;
                    }




                    
                }
            }
        }
    }

                    

    #region Skill Requirements Consuming Class
    public class SkillConsuming
    {
        /// <summary>
        /// Decreases MP or FP depending on skill requirements.
        /// </summary>
        /// <param name="data">Skill data of used skill.</param>
        /// <param name="user">Player that cast the skill.</param>
        /// <returns>Message ID if fail, otherwise zero.</returns>
        public static int Consume(Skills data, Client user)
        {
            
            if (data.dwReqMp > 0)
            {
                if (user.c_attributes[FlyFF.DST_MP] < data.dwReqMp)
                    return FlyFF.TID_GAME_REQMP;
                user.c_attributes[FlyFF.DST_MP] -= data.dwReqMp;
                user.SendPlayerAttribSet(FlyFF.DST_MP, user.c_attributes[FlyFF.DST_MP]);
            }
            if (data.dwReqFP > 0)
            {
                if (user.c_attributes[FlyFF.DST_FP] < data.dwReqFP)
                    return FlyFF.TID_GAME_REQFP;
                user.c_attributes[FlyFF.DST_FP] -= data.dwReqFP;
                user.SendPlayerAttribSet(FlyFF.DST_FP, user.c_attributes[FlyFF.DST_FP]);
            }
            return 0;
        }
        /// <summary>
        /// Search all monster in Aoe region to send them damage.
        /// </summary>
        /// <param name="data">Skill data of used skill.</param>
        /// <param name="user">Player that cast the skill.</param>
        /// <returns>Message ID if fail, otherwise zero.</returns>
        
    }
    #endregion
    #endregion
}
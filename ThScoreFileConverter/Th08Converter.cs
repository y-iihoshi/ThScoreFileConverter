//-----------------------------------------------------------------------
// <copyright file="Th08Converter.cs" company="None">
//     (c) 2013-2014 IIHOSHI Yoshinori
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable 1591

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage(
    "StyleCop.CSharp.DocumentationRules", "*", Justification = "Reviewed.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage(
    "StyleCop.CSharp.LayoutRules", "*", Justification = "Reviewed.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage(
    "StyleCop.CSharp.LayoutRules",
    "SA1503:CurlyBracketsMustNotBeOmitted",
    Justification = "Reviewed.")]

namespace ThScoreFileConverter
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using CardInfo = SpellCardInfo<Th08Converter.StagePractice, Th08Converter.LevelPractice>;

    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "StyleCop.CSharp.OrderingRules",
        "SA1201:ElementsMustAppearInTheCorrectOrder",
        Justification = "Reviewed.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "StyleCop.CSharp.SpacingRules",
        "SA1025:CodeMustNotContainMultipleWhitespaceInARow",
        Justification = "Reviewed.")]
    internal class Th08Converter : ThConverter
    {
        public enum Level
        {
            [EnumAltName("E")] Easy,
            [EnumAltName("N")] Normal,
            [EnumAltName("H")] Hard,
            [EnumAltName("L")] Lunatic,
            [EnumAltName("X")] Extra
        }
        public enum LevelWithTotal
        {
            [EnumAltName("E")] Easy,
            [EnumAltName("N")] Normal,
            [EnumAltName("H")] Hard,
            [EnumAltName("L")] Lunatic,
            [EnumAltName("X")] Extra,
            [EnumAltName("T")] Total
        }
        public enum LevelPractice
        {
            [EnumAltName("E")] Easy,
            [EnumAltName("N")] Normal,
            [EnumAltName("H")] Hard,
            [EnumAltName("L")] Lunatic,
            [EnumAltName("X")] Extra,
            [EnumAltName("W", LongName = "Last Word")] LastWord
        }
        public enum LevelPracticeWithTotal
        {
            [EnumAltName("E")] Easy,
            [EnumAltName("N")] Normal,
            [EnumAltName("H")] Hard,
            [EnumAltName("L")] Lunatic,
            [EnumAltName("X")] Extra,
            [EnumAltName("W", LongName = "Last Word")] LastWord,
            [EnumAltName("T")] Total
        }

        public enum Chara
        {
            [EnumAltName("RY")] ReimuYukari,
            [EnumAltName("MA")] MarisaAlice,
            [EnumAltName("SR")] SakuyaRemilia,
            [EnumAltName("YY")] YoumuYuyuko,
            [EnumAltName("RM")] Reimu,
            [EnumAltName("YK")] Yukari,
            [EnumAltName("MR")] Marisa,
            [EnumAltName("AL")] Alice,
            [EnumAltName("SK")] Sakuya,
            [EnumAltName("RL")] Remilia,
            [EnumAltName("YM")] Youmu,
            [EnumAltName("YU")] Yuyuko
        }
        public enum CharaWithTotal
        {
            [EnumAltName("RY")] ReimuYukari,
            [EnumAltName("MA")] MarisaAlice,
            [EnumAltName("SR")] SakuyaRemilia,
            [EnumAltName("YY")] YoumuYuyuko,
            [EnumAltName("RM")] Reimu,
            [EnumAltName("YK")] Yukari,
            [EnumAltName("MR")] Marisa,
            [EnumAltName("AL")] Alice,
            [EnumAltName("SK")] Sakuya,
            [EnumAltName("RL")] Remilia,
            [EnumAltName("YM")] Youmu,
            [EnumAltName("YU")] Yuyuko,
            [EnumAltName("TL")] Total
        }

        public enum Stage
        {
            [EnumAltName("1A")] St1,
            [EnumAltName("2A")] St2,
            [EnumAltName("3A")] St3,
            [EnumAltName("4A")] St4A,
            [EnumAltName("4B")] St4B,
            [EnumAltName("5A")] St5,
            [EnumAltName("6A")] St6A,
            [EnumAltName("6B")] St6B,
            [EnumAltName("EX")] Extra
        }
        public enum StageWithTotal
        {
            [EnumAltName("1A")] St1,
            [EnumAltName("2A")] St2,
            [EnumAltName("3A")] St3,
            [EnumAltName("4A")] St4A,
            [EnumAltName("4B")] St4B,
            [EnumAltName("5A")] St5,
            [EnumAltName("6A")] St6A,
            [EnumAltName("6B")] St6B,
            [EnumAltName("EX")] Extra,
            [EnumAltName("00")] Total
        }
        public enum StagePractice
        {
            [EnumAltName("1A")] St1,
            [EnumAltName("2A")] St2,
            [EnumAltName("3A")] St3,
            [EnumAltName("4A")] St4A,
            [EnumAltName("4B")] St4B,
            [EnumAltName("5A")] St5,
            [EnumAltName("6A")] St6A,
            [EnumAltName("6B")] St6B,
            [EnumAltName("EX")] Extra,
            [EnumAltName("LW", LongName = "Last Word")] LastWord
        }

        public enum StageProgress
        {
            [EnumAltName("Stage 1")]          St1,
            [EnumAltName("Stage 2")]          St2,
            [EnumAltName("Stage 3")]          St3,
            [EnumAltName("Stage 4-uncanny")]  St4A,
            [EnumAltName("Stage 4-powerful")] St4B,
            [EnumAltName("Stage 5")]          St5,
            [EnumAltName("Stage 6-Eirin")]    St6A,
            [EnumAltName("Stage 6-Kaguya")]   St6B,
            [EnumAltName("Extra Stage")]      Extra,
            [EnumAltName("All Clear")]        Clear = 99
        }

        [Flags]
        private enum PlayableStages
        {
            Stage1   = 0x0001,
            Stage2   = 0x0002,
            Stage3   = 0x0004,
            Stage4A  = 0x0008,
            Stage4B  = 0x0010,
            Stage5   = 0x0020,
            Stage6A  = 0x0040,
            Stage6B  = 0x0080,
            Extra    = 0x0100,
            Unknown  = 0x4000,
            AllClear = 0x8000
        }

        private class CharaLevelPair : Pair<Chara, Level>
        {
            [System.Diagnostics.CodeAnalysis.SuppressMessage(
                "Microsoft.Performance",
                "CA1811:AvoidUncalledPrivateCode",
                Justification = "For future use.")]
            public Chara Chara { get { return this.First; } }

            [System.Diagnostics.CodeAnalysis.SuppressMessage(
                "Microsoft.Performance",
                "CA1811:AvoidUncalledPrivateCode",
                Justification = "For future use.")]
            public Level Level { get { return this.Second; } }

            public CharaLevelPair(Chara chara, Level level) : base(chara, level) { }
        }

        private class StageLevelPair : Pair<Stage, Level>
        {
            [System.Diagnostics.CodeAnalysis.SuppressMessage(
                "Microsoft.Performance",
                "CA1811:AvoidUncalledPrivateCode",
                Justification = "For future use.")]
            public Stage Stage { get { return this.First; } }

            [System.Diagnostics.CodeAnalysis.SuppressMessage(
                "Microsoft.Performance",
                "CA1811:AvoidUncalledPrivateCode",
                Justification = "For future use.")]
            public Level Level { get { return this.Second; } }

            public StageLevelPair(Stage stage, Level level) : base(stage, level) { }
        }

        private class AllScoreData
        {
            public Header Header { get; set; }
            public Dictionary<CharaLevelPair, List<HighScore>> Rankings { get; set; }
            public Dictionary<CharaWithTotal, ClearData> ClearData { get; set; }
            public Dictionary<int, CardAttack> CardAttacks { get; set; }
            public Dictionary<Chara, PracticeScore> PracticeScores { get; set; }
            public FLSP Flsp { get; set; }
            public PlayStatus PlayStatus { get; set; }
            public LastName LastName { get; set; }
            public VersionInfo VersionInfo { get; set; }

            public AllScoreData()
            {
                var numCharas = Enum.GetValues(typeof(Chara)).Length;
                var numPairs = numCharas * Enum.GetValues(typeof(Level)).Length;
                this.Rankings = new Dictionary<CharaLevelPair, List<HighScore>>(numPairs);
                this.ClearData =
                    new Dictionary<CharaWithTotal, ClearData>(Enum.GetValues(typeof(CharaWithTotal)).Length);
                this.CardAttacks = new Dictionary<int, CardAttack>(CardTable.Count);
                this.PracticeScores = new Dictionary<Chara, PracticeScore>(numCharas);
            }
        }

        private class Chapter : IBinaryReadable
        {
            public string Signature { get; private set; }   // .Length = 4
            public short Size1 { get; private set; }
            public short Size2 { get; private set; }        // always equal to size1?

            public Chapter() { }
            public Chapter(Chapter ch)
            {
                this.Signature = ch.Signature;
                this.Size1 = ch.Size1;
                this.Size2 = ch.Size2;
            }

            public virtual void ReadFrom(BinaryReader reader)
            {
                this.Signature = new string(reader.ReadChars(4));
                this.Size1 = reader.ReadInt16();
                this.Size2 = reader.ReadInt16();
            }
        }

        private class Header : Chapter
        {
            public Header(Chapter ch)
                : base(ch)
            {
                if (this.Signature != "TH8K")
                    throw new InvalidDataException("Signature");
                if (this.Size1 != 0x000C)
                    throw new InvalidDataException("Size1");
                // if (this.Size2 != 0x000C)
                //     throw new InvalidDataException("Size2");
            }

            public override void ReadFrom(BinaryReader reader)
            {
                reader.ReadUInt32();    // always 0x00000001?
            }
        }

        private class HighScore : Chapter   // per character, level, rank
        {
            public uint Score { get; private set; }                     // * 10
            public float SlowRate { get; private set; }
            public Chara Chara { get; private set; }                    // size: 1Byte
            public Level Level { get; private set; }                    // size: 1Byte
            public StageProgress StageProgress { get; private set; }    // size: 1Byte
            public byte[] Name { get; private set; }                    // .Length = 9, null-terminated
            public byte[] Date { get; private set; }                    // .Length = 6, "mm/dd\0"
            public ushort ContinueCount { get; private set; }
            public byte PlayerNum { get; private set; }                 // 0-based
            public uint PlayTime { get; private set; }                  // = seconds * 60fps
            public int PointItem { get; private set; }
            public int MissCount { get; private set; }
            public int BombCount { get; private set; }
            public int LastSpellCount { get; private set; }
            public int PauseCount { get; private set; }
            public int TimePoint { get; private set; }
            public int HumanRate { get; private set; }                  // / 100
            public Dictionary<int, byte> CardFlags { get; private set; }

            public HighScore(Chapter ch)
                : base(ch)
            {
                if (this.Signature != "HSCR")
                    throw new InvalidDataException("Signature");
                if (this.Size1 != 0x0168)
                    throw new InvalidDataException("Size1");
                // if (this.Size2 != 0x0168)
                //     throw new InvalidDataException("Size2");

                this.CardFlags = new Dictionary<int, byte>(CardTable.Count);
            }

            public HighScore(uint score)    // for InitialRanking only
                : base()
            {
                this.Score = score;
                this.Name = Encoding.Default.GetBytes("--------\0");
                this.Date = Encoding.Default.GetBytes("--/--\0");
                this.CardFlags = new Dictionary<int, byte>(CardTable.Count);
            }

            public override void ReadFrom(BinaryReader reader)
            {
                reader.ReadUInt32();    // always 0x00000004?
                this.Score = reader.ReadUInt32();
                this.SlowRate = reader.ReadSingle();
                this.Chara = (Chara)reader.ReadByte();
                this.Level = (Level)reader.ReadByte();
                this.StageProgress = (StageProgress)reader.ReadByte();
                this.Name = reader.ReadBytes(9);
                this.Date = reader.ReadBytes(6);
                this.ContinueCount = reader.ReadUInt16();
                // 01 00 00 00 04 00 09 00 FF FF FF FF FF FF FF FF
                // 05 00 00 00 01 00 08 00 58 02 58 02
                reader.ReadBytes(0x1C);
                this.PlayerNum = reader.ReadByte();
                // NN 03 00 01 01 LL 01 00 02 00 00 ** ** 00 00 00
                // 00 00 00 00 00 00 00 00 00 00 00 00 01 40 00 00
                // where NN: PlayerNum, LL: level, **: unknown (0x64 or 0x0A; 0x50 or 0x0A)
                reader.ReadBytes(0x1F);
                this.PlayTime = reader.ReadUInt32();
                this.PointItem = reader.ReadInt32();
                reader.ReadUInt32();    // always 0x00000000?
                this.MissCount = reader.ReadInt32();
                this.BombCount = reader.ReadInt32();
                this.LastSpellCount = reader.ReadInt32();
                this.PauseCount = reader.ReadInt32();
                this.TimePoint = reader.ReadInt32();
                this.HumanRate = reader.ReadInt32();
                foreach (var key in CardTable.Keys)
                    this.CardFlags.Add(key, reader.ReadByte());
                reader.ReadBytes(2);
            }
        }

        private class ClearData : Chapter   // per character-with-total
        {
            public Dictionary<Level, PlayableStages> StoryFlags { get; private set; }       // really...?
            public Dictionary<Level, PlayableStages> PracticeFlags { get; private set; }    // really...?
            public CharaWithTotal Chara { get; private set; }                               // size: 1Byte

            public ClearData(Chapter ch)
                : base(ch)
            {
                if (this.Signature != "CLRD")
                    throw new InvalidDataException("Signature");
                if (this.Size1 != 0x0024)
                    throw new InvalidDataException("Size1");
                // if (this.Size2 != 0x0024)
                //     throw new InvalidDataException("Size2");

                var numLevels = Enum.GetValues(typeof(Level)).Length;
                this.StoryFlags = new Dictionary<Level, PlayableStages>(numLevels);
                this.PracticeFlags = new Dictionary<Level, PlayableStages>(numLevels);
            }

            public override void ReadFrom(BinaryReader reader)
            {
                var levels = Utils.GetEnumerator<Level>();

                reader.ReadUInt32();    // always 0x00000004?
                foreach (var level in levels)
                    this.StoryFlags.Add(level, (PlayableStages)reader.ReadUInt16());
                foreach (var level in levels)
                    this.PracticeFlags.Add(level, (PlayableStages)reader.ReadUInt16());
                reader.ReadByte();      // always 0x00?
                this.Chara = (CharaWithTotal)reader.ReadByte();
                reader.ReadUInt16();    // always 0x0000?
            }
        }

        private class CardAttack : Chapter      // per card
        {
            public short Number { get; private set; }       // 1-based
            public LevelPracticeWithTotal Level { get; private set; }

            [System.Diagnostics.CodeAnalysis.SuppressMessage(
                "Microsoft.Performance",
                "CA1811:AvoidUncalledPrivateCode",
                Justification = "For future use.")]
            public byte[] CardName { get; private set; }    // .Length = 0x30

            [System.Diagnostics.CodeAnalysis.SuppressMessage(
                "Microsoft.Performance",
                "CA1811:AvoidUncalledPrivateCode",
                Justification = "For future use.")]
            public byte[] EnemyName { get; private set; }   // .Length = 0x30

            [System.Diagnostics.CodeAnalysis.SuppressMessage(
                "Microsoft.Performance",
                "CA1811:AvoidUncalledPrivateCode",
                Justification = "For future use.")]
            public byte[] Comment { get; private set; }     // .Length = 0x80, should split by '\0'

            public CardAttackCareer StoryCareer { get; private set; }
            public CardAttackCareer PracticeCareer { get; private set; }

            public CardAttack(Chapter ch)
                : base(ch)
            {
                if (this.Signature != "CATK")
                    throw new InvalidDataException("Signature");
                if (this.Size1 != 0x022C)
                    throw new InvalidDataException("Size1");
                // if (this.Size2 != 0x022C)
                //     throw new InvalidDataException("Size2");

                this.StoryCareer = new CardAttackCareer();
                this.PracticeCareer = new CardAttackCareer();
            }

            public override void ReadFrom(BinaryReader reader)
            {
                reader.ReadUInt32();    // always 0x00000003?
                this.Number = (short)(reader.ReadInt16() + 1);
                reader.ReadByte();
                this.Level = (LevelPracticeWithTotal)reader.ReadByte();     // Last Word == Normal...
                this.CardName = reader.ReadBytes(0x30);
                this.EnemyName = reader.ReadBytes(0x30);
                this.Comment = reader.ReadBytes(0x80);
                this.StoryCareer.ReadFrom(reader);
                this.PracticeCareer.ReadFrom(reader);
                reader.ReadUInt32();    // always 0x00000000?
            }

            public bool HasTried()
            {
                return (this.StoryCareer.TrialCounts[CharaWithTotal.Total] > 0)
                    || (this.PracticeCareer.TrialCounts[CharaWithTotal.Total] > 0);
            }
        }

        private class CardAttackCareer : IBinaryReadable      // per story or practice
        {
            public Dictionary<CharaWithTotal, uint> MaxBonuses { get; private set; }
            public Dictionary<CharaWithTotal, int> TrialCounts { get; private set; }
            public Dictionary<CharaWithTotal, int> ClearCounts { get; private set; }

            public CardAttackCareer()
            {
                var numCharas = Enum.GetValues(typeof(CharaWithTotal)).Length;
                this.MaxBonuses = new Dictionary<CharaWithTotal, uint>(numCharas);
                this.TrialCounts = new Dictionary<CharaWithTotal, int>(numCharas);
                this.ClearCounts = new Dictionary<CharaWithTotal, int>(numCharas);
            }

            public void ReadFrom(BinaryReader reader)
            {
                var charas = Utils.GetEnumerator<CharaWithTotal>();
                foreach (var chara in charas)
                    this.MaxBonuses.Add(chara, reader.ReadUInt32());
                foreach (var chara in charas)
                    this.TrialCounts.Add(chara, reader.ReadInt32());
                foreach (var chara in charas)
                    this.ClearCounts.Add(chara, reader.ReadInt32());
            }
        }

        private class PracticeScore : Chapter   // per character
        {
            public Dictionary<StageLevelPair, int> PlayCounts { get; private set; }
            public Dictionary<StageLevelPair, int> HighScores { get; private set; }     // * 10
            public Chara Chara { get; private set; }        // size: 1Byte

            public PracticeScore(Chapter ch)
                : base(ch)
            {
                if (this.Signature != "PSCR")
                    throw new InvalidDataException("Signature");
                if (this.Size1 != 0x0178)
                    throw new InvalidDataException("Size1");
                // if (this.Size2 != 0x0178)
                //     throw new InvalidDataException("Size2");

                var numPairs = Enum.GetValues(typeof(Stage)).Length * Enum.GetValues(typeof(Level)).Length;
                this.PlayCounts = new Dictionary<StageLevelPair, int>(numPairs);
                this.HighScores = new Dictionary<StageLevelPair, int>(numPairs);
            }

            public override void ReadFrom(BinaryReader reader)
            {
                // The fields for Stage.Extra and Level.Extra actually exist...

                var stages = Utils.GetEnumerator<Stage>();
                var levels = Utils.GetEnumerator<Level>();
                reader.ReadUInt32();    // always 0x00000002?
                foreach (var stage in stages)
                    foreach (var level in levels)
                    {
                        var key = new StageLevelPair(stage, level);
                        if (!this.PlayCounts.ContainsKey(key))
                            this.PlayCounts.Add(key, 0);
                        this.PlayCounts[key] = reader.ReadInt32();
                    }
                foreach (var stage in stages)
                    foreach (var level in levels)
                    {
                        var key = new StageLevelPair(stage, level);
                        if (!this.HighScores.ContainsKey(key))
                            this.HighScores.Add(key, 0);
                        this.HighScores[key] = reader.ReadInt32();
                    }
                this.Chara = (Chara)reader.ReadByte();
                reader.ReadBytes(3);    // always 0x000001?
            }
        }

        private class FLSP : Chapter    // FIXME
        {
            public FLSP(Chapter ch)
                : base(ch)
            {
                if (this.Signature != "FLSP")
                    throw new InvalidDataException("Signature");
                if (this.Size1 != 0x0020)
                    throw new InvalidDataException("Size1");
                // if (this.Size2 != 0x0020)
                //     throw new InvalidDataException("Size2");
            }

            public override void ReadFrom(BinaryReader reader)
            {
                reader.ReadBytes(0x18);
            }
        }

        private class PlayStatus : Chapter
        {
            public Time TotalRunningTime { get; private set; }
            public Time TotalPlayTime { get; private set; }
            public Dictionary<Level, PlayCount> PlayCounts { get; private set; }
            public PlayCount TotalPlayCount { get; private set; }

            [System.Diagnostics.CodeAnalysis.SuppressMessage(
                "Microsoft.Performance",
                "CA1811:AvoidUncalledPrivateCode",
                Justification = "For future use.")]
            public byte[] BgmFlags { get; private set; }            // .Length = 21

            public PlayStatus(Chapter ch)
                : base(ch)
            {
                if (this.Signature != "PLST")
                    throw new InvalidDataException("Signature");
                if (this.Size1 != 0x0228)
                    throw new InvalidDataException("Size1");
                // if (this.Size2 != 0x0228)
                //     throw new InvalidDataException("Size2");

                this.PlayCounts = new Dictionary<Level, PlayCount>(Enum.GetValues(typeof(Level)).Length);
                this.TotalPlayCount = new PlayCount();
            }

            public override void ReadFrom(BinaryReader reader)
            {
                reader.ReadUInt32();    // always 0x00000002?
                this.TotalRunningTime = new Time(
                    reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), false);
                this.TotalPlayTime = new Time(
                    reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), false);
                foreach (var level in Utils.GetEnumerator<Level>())
                {
                    var playCount = new PlayCount();
                    playCount.ReadFrom(reader);
                    if (!this.PlayCounts.ContainsKey(level))
                        this.PlayCounts.Add(level, playCount);
                }
                new PlayCount().ReadFrom(reader);   // always all 0?
                this.TotalPlayCount.ReadFrom(reader);
                this.BgmFlags = reader.ReadBytes(21);
                reader.ReadBytes(11);
            }
        }

        private class PlayCount : IBinaryReadable   // per level-with-total
        {
            public int TotalTrial { get; private set; }
            public Dictionary<Chara, int> Trials { get; private set; }
            public int TotalClear { get; private set; }
            public int TotalContinue { get; private set; }
            public int TotalPractice { get; private set; }

            public PlayCount()
            {
                this.Trials = new Dictionary<Chara, int>(Enum.GetValues(typeof(Chara)).Length);
            }

            public void ReadFrom(BinaryReader reader)
            {
                this.TotalTrial = reader.ReadInt32();
                foreach (var chara in Utils.GetEnumerator<Chara>())
                    this.Trials.Add(chara, reader.ReadInt32());
                reader.ReadUInt32();    // always 0x00000000?
                this.TotalClear = reader.ReadInt32();
                this.TotalContinue = reader.ReadInt32();
                this.TotalPractice = reader.ReadInt32();
            }
        }

        private class LastName : Chapter
        {
            [System.Diagnostics.CodeAnalysis.SuppressMessage(
                "Microsoft.Performance",
                "CA1811:AvoidUncalledPrivateCode",
                Justification = "For future use.")]
            public byte[] Name { get; private set; }    // .Length = 12, null-terminated

            public LastName(Chapter ch)
                : base(ch)
            {
                if (this.Signature != "LSNM")
                    throw new InvalidDataException("Signature");
                if (this.Size1 != 0x0018)
                    throw new InvalidDataException("Size1");
                // if (this.Size2 != 0x0018)
                //     throw new InvalidDataException("Size2");
            }

            public override void ReadFrom(BinaryReader reader)
            {
                reader.ReadUInt32();    // always 0x00000001?
                this.Name = reader.ReadBytes(12);
            }
        }

        private class VersionInfo : Chapter
        {
            [System.Diagnostics.CodeAnalysis.SuppressMessage(
                "Microsoft.Performance",
                "CA1811:AvoidUncalledPrivateCode",
                Justification = "For future use.")]
            public byte[] Version { get; private set; }     // .Length = 6, null-terminated

            public VersionInfo(Chapter ch)
                : base(ch)
            {
                if (this.Signature != "VRSM")
                    throw new InvalidDataException("Signature");
                if (this.Size1 != 0x001C)
                    throw new InvalidDataException("Size1");
                // if (this.Size2 != 0x001C)
                //     throw new InvalidDataException("Size2");
            }

            public override void ReadFrom(BinaryReader reader)
            {
                reader.ReadUInt32();    // always 0x00000001?
                this.Version = reader.ReadBytes(6);
                reader.ReadBytes(3);
                reader.ReadBytes(3);
                reader.ReadUInt32();
            }
        }

        private AllScoreData allScoreData = null;

        public override string SupportedVersions
        {
            get { return "1.00d"; }
        }

        private static readonly Dictionary<int, CardInfo> CardTable;
        private static readonly List<HighScore> InitialRanking;

        private static readonly string LevelPattern;
        private static readonly string LevelWithTotalPattern;
        private static readonly string LevelPracticeWithTotalPattern;
        private static readonly string CharaPattern;
        private static readonly string CharaWithTotalPattern;
        private static readonly string StagePattern;
        private static readonly string StageWithTotalPattern;

        private static readonly Func<string, Level> ToLevel;
        private static readonly Func<string, LevelWithTotal> ToLevelWithTotal;
        private static readonly Func<string, LevelPracticeWithTotal> ToLevelPracticeWithTotal;
        private static readonly Func<string, Chara> ToChara;
        private static readonly Func<string, CharaWithTotal> ToCharaWithTotal;
        private static readonly Func<string, Stage> ToStage;
        private static readonly Func<string, StageWithTotal> ToStageWithTotal;

        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Performance",
            "CA1810:InitializeReferenceTypeStaticFieldsInline",
            Justification = "Reviewed.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "StyleCop.CSharp.MaintainabilityRules",
            "SA1119:StatementMustNotUseUnnecessaryParenthesis",
            Justification = "Reviewed.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "StyleCop.CSharp.SpacingRules",
            "SA1008:OpeningParenthesisMustBeSpacedCorrectly",
            Justification = "Reviewed.")]
        static Th08Converter()
        {
            // Thanks to thwiki.info
            var cardList = new List<CardInfo>()
            {
                new CardInfo(  1, "蛍符「地上の流星」",                       StagePractice.St1,      LevelPractice.Hard),
                new CardInfo(  2, "蛍符「地上の彗星」",                       StagePractice.St1,      LevelPractice.Lunatic),
                new CardInfo(  3, "灯符「ファイヤフライフェノメノン」",       StagePractice.St1,      LevelPractice.Easy),
                new CardInfo(  4, "灯符「ファイヤフライフェノメノン」",       StagePractice.St1,      LevelPractice.Normal),
                new CardInfo(  5, "灯符「ファイヤフライフェノメノン」",       StagePractice.St1,      LevelPractice.Hard),
                new CardInfo(  6, "灯符「ファイヤフライフェノメノン」",       StagePractice.St1,      LevelPractice.Lunatic),
                new CardInfo(  7, "蠢符「リトルバグ」",                       StagePractice.St1,      LevelPractice.Easy),
                new CardInfo(  8, "蠢符「リトルバグストーム」",               StagePractice.St1,      LevelPractice.Normal),
                new CardInfo(  9, "蠢符「ナイトバグストーム」",               StagePractice.St1,      LevelPractice.Hard),
                new CardInfo( 10, "蠢符「ナイトバグトルネード」",             StagePractice.St1,      LevelPractice.Lunatic),
                new CardInfo( 11, "隠蟲「永夜蟄居」",                         StagePractice.St1,      LevelPractice.Normal),
                new CardInfo( 12, "隠蟲「永夜蟄居」",                         StagePractice.St1,      LevelPractice.Hard),
                new CardInfo( 13, "隠蟲「永夜蟄居」",                         StagePractice.St1,      LevelPractice.Lunatic),
                new CardInfo( 14, "声符「梟の夜鳴声」",                       StagePractice.St2,      LevelPractice.Easy),
                new CardInfo( 15, "声符「梟の夜鳴声」",                       StagePractice.St2,      LevelPractice.Normal),
                new CardInfo( 16, "声符「木菟咆哮」",                         StagePractice.St2,      LevelPractice.Hard),
                new CardInfo( 17, "声符「木菟咆哮」",                         StagePractice.St2,      LevelPractice.Lunatic),
                new CardInfo( 18, "蛾符「天蛾の蠱道」",                       StagePractice.St2,      LevelPractice.Easy),
                new CardInfo( 19, "蛾符「天蛾の蠱道」",                       StagePractice.St2,      LevelPractice.Normal),
                new CardInfo( 20, "毒符「毒蛾の鱗粉」",                       StagePractice.St2,      LevelPractice.Hard),
                new CardInfo( 21, "猛毒「毒蛾の暗闇演舞」",                   StagePractice.St2,      LevelPractice.Lunatic),
                new CardInfo( 22, "鷹符「イルスタードダイブ」",               StagePractice.St2,      LevelPractice.Easy),
                new CardInfo( 23, "鷹符「イルスタードダイブ」",               StagePractice.St2,      LevelPractice.Normal),
                new CardInfo( 24, "鷹符「イルスタードダイブ」",               StagePractice.St2,      LevelPractice.Hard),
                new CardInfo( 25, "鷹符「イルスタードダイブ」",               StagePractice.St2,      LevelPractice.Lunatic),
                new CardInfo( 26, "夜盲「夜雀の歌」",                         StagePractice.St2,      LevelPractice.Easy),
                new CardInfo( 27, "夜盲「夜雀の歌」",                         StagePractice.St2,      LevelPractice.Normal),
                new CardInfo( 28, "夜盲「夜雀の歌」",                         StagePractice.St2,      LevelPractice.Hard),
                new CardInfo( 29, "夜盲「夜雀の歌」",                         StagePractice.St2,      LevelPractice.Lunatic),
                new CardInfo( 30, "夜雀「真夜中のコーラスマスター」",         StagePractice.St2,      LevelPractice.Normal),
                new CardInfo( 31, "夜雀「真夜中のコーラスマスター」",         StagePractice.St2,      LevelPractice.Hard),
                new CardInfo( 32, "夜雀「真夜中のコーラスマスター」",         StagePractice.St2,      LevelPractice.Lunatic),
                new CardInfo( 33, "産霊「ファーストピラミッド」",             StagePractice.St3,      LevelPractice.Easy),
                new CardInfo( 34, "産霊「ファーストピラミッド」",             StagePractice.St3,      LevelPractice.Normal),
                new CardInfo( 35, "産霊「ファーストピラミッド」",             StagePractice.St3,      LevelPractice.Hard),
                new CardInfo( 36, "産霊「ファーストピラミッド」",             StagePractice.St3,      LevelPractice.Lunatic),
                new CardInfo( 37, "始符「エフェメラリティ137」",              StagePractice.St3,      LevelPractice.Normal),
                new CardInfo( 38, "始符「エフェメラリティ137」",              StagePractice.St3,      LevelPractice.Hard),
                new CardInfo( 39, "始符「エフェメラリティ137」",              StagePractice.St3,      LevelPractice.Lunatic),
                new CardInfo( 40, "野符「武烈クライシス」",                   StagePractice.St3,      LevelPractice.Easy),
                new CardInfo( 41, "野符「将門クライシス」",                   StagePractice.St3,      LevelPractice.Normal),
                new CardInfo( 42, "野符「義満クライシス」",                   StagePractice.St3,      LevelPractice.Hard),
                new CardInfo( 43, "野符「GHQクライシス」",                    StagePractice.St3,      LevelPractice.Lunatic),
                new CardInfo( 44, "国符「三種の神器　剣」",                   StagePractice.St3,      LevelPractice.Easy),
                new CardInfo( 45, "国符「三種の神器　玉」",                   StagePractice.St3,      LevelPractice.Normal),
                new CardInfo( 46, "国符「三種の神器　鏡」",                   StagePractice.St3,      LevelPractice.Hard),
                new CardInfo( 47, "国体「三種の神器　郷」",                   StagePractice.St3,      LevelPractice.Lunatic),
                new CardInfo( 48, "終符「幻想天皇」",                         StagePractice.St3,      LevelPractice.Easy),
                new CardInfo( 49, "終符「幻想天皇」",                         StagePractice.St3,      LevelPractice.Normal),
                new CardInfo( 50, "虚史「幻想郷伝説」",                       StagePractice.St3,      LevelPractice.Hard),
                new CardInfo( 51, "虚史「幻想郷伝説」",                       StagePractice.St3,      LevelPractice.Lunatic),
                new CardInfo( 52, "未来「高天原」",                           StagePractice.St3,      LevelPractice.Normal),
                new CardInfo( 53, "未来「高天原」",                           StagePractice.St3,      LevelPractice.Hard),
                new CardInfo( 54, "未来「高天原」",                           StagePractice.St3,      LevelPractice.Lunatic),
                new CardInfo( 55, "夢符「二重結界」",                         StagePractice.St4A,     LevelPractice.Easy),
                new CardInfo( 56, "夢符「二重結界」",                         StagePractice.St4A,     LevelPractice.Normal),
                new CardInfo( 57, "夢境「二重大結界」",                       StagePractice.St4A,     LevelPractice.Hard),
                new CardInfo( 58, "夢境「二重大結界」",                       StagePractice.St4A,     LevelPractice.Lunatic),
                new CardInfo( 59, "霊符「夢想封印　散」",                     StagePractice.St4A,     LevelPractice.Easy),
                new CardInfo( 60, "霊符「夢想封印　散」",                     StagePractice.St4A,     LevelPractice.Normal),
                new CardInfo( 61, "散霊「夢想封印　寂」",                     StagePractice.St4A,     LevelPractice.Hard),
                new CardInfo( 62, "散霊「夢想封印　寂」",                     StagePractice.St4A,     LevelPractice.Lunatic),
                new CardInfo( 63, "夢符「封魔陣」",                           StagePractice.St4A,     LevelPractice.Easy),
                new CardInfo( 64, "夢符「封魔陣」",                           StagePractice.St4A,     LevelPractice.Normal),
                new CardInfo( 65, "神技「八方鬼縛陣」",                       StagePractice.St4A,     LevelPractice.Hard),
                new CardInfo( 66, "神技「八方龍殺陣」",                       StagePractice.St4A,     LevelPractice.Lunatic),
                new CardInfo( 67, "霊符「夢想封印　集」",                     StagePractice.St4A,     LevelPractice.Easy),
                new CardInfo( 68, "霊符「夢想封印　集」",                     StagePractice.St4A,     LevelPractice.Normal),
                new CardInfo( 69, "回霊「夢想封印　侘」",                     StagePractice.St4A,     LevelPractice.Hard),
                new CardInfo( 70, "回霊「夢想封印　侘」",                     StagePractice.St4A,     LevelPractice.Lunatic),
                new CardInfo( 71, "境界「二重弾幕結界」",                     StagePractice.St4A,     LevelPractice.Easy),
                new CardInfo( 72, "境界「二重弾幕結界」",                     StagePractice.St4A,     LevelPractice.Normal),
                new CardInfo( 73, "大結界「博麗弾幕結界」",                   StagePractice.St4A,     LevelPractice.Hard),
                new CardInfo( 74, "大結界「博麗弾幕結界」",                   StagePractice.St4A,     LevelPractice.Lunatic),
                new CardInfo( 75, "神霊「夢想封印　瞬」",                     StagePractice.St4A,     LevelPractice.Normal),
                new CardInfo( 76, "神霊「夢想封印　瞬」",                     StagePractice.St4A,     LevelPractice.Hard),
                new CardInfo( 77, "神霊「夢想封印　瞬」",                     StagePractice.St4A,     LevelPractice.Lunatic),
                new CardInfo( 78, "魔符「ミルキーウェイ」",                   StagePractice.St4B,     LevelPractice.Easy),
                new CardInfo( 79, "魔符「ミルキーウェイ」",                   StagePractice.St4B,     LevelPractice.Normal),
                new CardInfo( 80, "魔空「アステロイドベルト」",               StagePractice.St4B,     LevelPractice.Hard),
                new CardInfo( 81, "魔空「アステロイドベルト」",               StagePractice.St4B,     LevelPractice.Lunatic),
                new CardInfo( 82, "魔符「スターダストレヴァリエ」",           StagePractice.St4B,     LevelPractice.Easy),
                new CardInfo( 83, "魔符「スターダストレヴァリエ」",           StagePractice.St4B,     LevelPractice.Normal),
                new CardInfo( 84, "黒魔「イベントホライズン」",               StagePractice.St4B,     LevelPractice.Hard),
                new CardInfo( 85, "黒魔「イベントホライズン」",               StagePractice.St4B,     LevelPractice.Lunatic),
                new CardInfo( 86, "恋符「ノンディレクショナルレーザー」",     StagePractice.St4B,     LevelPractice.Easy),
                new CardInfo( 87, "恋符「ノンディレクショナルレーザー」",     StagePractice.St4B,     LevelPractice.Normal),
                new CardInfo( 88, "恋風「スターライトタイフーン」",           StagePractice.St4B,     LevelPractice.Hard),
                new CardInfo( 89, "恋風「スターライトタイフーン」",           StagePractice.St4B,     LevelPractice.Lunatic),
                new CardInfo( 90, "恋符「マスタースパーク」",                 StagePractice.St4B,     LevelPractice.Easy),
                new CardInfo( 91, "恋符「マスタースパーク」",                 StagePractice.St4B,     LevelPractice.Normal),
                new CardInfo( 92, "恋心「ダブルスパーク」",                   StagePractice.St4B,     LevelPractice.Hard),
                new CardInfo( 93, "恋心「ダブルスパーク」",                   StagePractice.St4B,     LevelPractice.Lunatic),
                new CardInfo( 94, "光符「アースライトレイ」",                 StagePractice.St4B,     LevelPractice.Easy),
                new CardInfo( 95, "光符「アースライトレイ」",                 StagePractice.St4B,     LevelPractice.Normal),
                new CardInfo( 96, "光撃「シュート・ザ・ムーン」",             StagePractice.St4B,     LevelPractice.Hard),
                new CardInfo( 97, "光撃「シュート・ザ・ムーン」",             StagePractice.St4B,     LevelPractice.Lunatic),
                new CardInfo( 98, "魔砲「ファイナルスパーク」",               StagePractice.St4B,     LevelPractice.Normal),
                new CardInfo( 99, "魔砲「ファイナルスパーク」",               StagePractice.St4B,     LevelPractice.Hard),
                new CardInfo(100, "魔砲「ファイナルマスタースパーク」",       StagePractice.St4B,     LevelPractice.Lunatic),
                new CardInfo(101, "波符「赤眼催眠(マインドシェイカー)」",     StagePractice.St5,      LevelPractice.Easy),
                new CardInfo(102, "波符「赤眼催眠(マインドシェイカー)」",     StagePractice.St5,      LevelPractice.Normal),
                new CardInfo(103, "幻波「赤眼催眠(マインドブローイング)」",   StagePractice.St5,      LevelPractice.Hard),
                new CardInfo(104, "幻波「赤眼催眠(マインドブローイング)」",   StagePractice.St5,      LevelPractice.Lunatic),
                new CardInfo(105, "狂符「幻視調律(ビジョナリチューニング)」", StagePractice.St5,      LevelPractice.Easy),
                new CardInfo(106, "狂符「幻視調律(ビジョナリチューニング)」", StagePractice.St5,      LevelPractice.Normal),
                new CardInfo(107, "狂視「狂視調律(イリュージョンシーカー)」", StagePractice.St5,      LevelPractice.Hard),
                new CardInfo(108, "狂視「狂視調律(イリュージョンシーカー)」", StagePractice.St5,      LevelPractice.Lunatic),
                new CardInfo(109, "懶符「生神停止(アイドリングウェーブ)」",   StagePractice.St5,      LevelPractice.Easy),
                new CardInfo(110, "懶符「生神停止(アイドリングウェーブ)」",   StagePractice.St5,      LevelPractice.Normal),
                new CardInfo(111, "懶惰「生神停止(マインドストッパー)」",     StagePractice.St5,      LevelPractice.Hard),
                new CardInfo(112, "懶惰「生神停止(マインドストッパー)」",     StagePractice.St5,      LevelPractice.Lunatic),
                new CardInfo(113, "散符「真実の月(インビジブルフルムーン)」", StagePractice.St5,      LevelPractice.Easy),
                new CardInfo(114, "散符「真実の月(インビジブルフルムーン)」", StagePractice.St5,      LevelPractice.Normal),
                new CardInfo(115, "散符「真実の月(インビジブルフルムーン)」", StagePractice.St5,      LevelPractice.Hard),
                new CardInfo(116, "散符「真実の月(インビジブルフルムーン)」", StagePractice.St5,      LevelPractice.Lunatic),
                new CardInfo(117, "月眼「月兎遠隔催眠術(テレメスメリズム)」", StagePractice.St5,      LevelPractice.Normal),
                new CardInfo(118, "月眼「月兎遠隔催眠術(テレメスメリズム)」", StagePractice.St5,      LevelPractice.Hard),
                new CardInfo(119, "月眼「月兎遠隔催眠術(テレメスメリズム)」", StagePractice.St5,      LevelPractice.Lunatic),
                new CardInfo(120, "天丸「壺中の天地」",                       StagePractice.St6A,     LevelPractice.Easy),
                new CardInfo(121, "天丸「壺中の天地」",                       StagePractice.St6A,     LevelPractice.Normal),
                new CardInfo(122, "天丸「壺中の天地」",                       StagePractice.St6A,     LevelPractice.Hard),
                new CardInfo(123, "天丸「壺中の天地」",                       StagePractice.St6A,     LevelPractice.Lunatic),
                new CardInfo(124, "覚神「神代の記憶」",                       StagePractice.St6A,     LevelPractice.Easy),
                new CardInfo(125, "覚神「神代の記憶」",                       StagePractice.St6A,     LevelPractice.Normal),
                new CardInfo(126, "神符「天人の系譜」",                       StagePractice.St6A,     LevelPractice.Hard),
                new CardInfo(127, "神符「天人の系譜」",                       StagePractice.St6A,     LevelPractice.Lunatic),
                new CardInfo(128, "蘇活「生命遊戯　-ライフゲーム-」",         StagePractice.St6A,     LevelPractice.Easy),
                new CardInfo(129, "蘇活「生命遊戯　-ライフゲーム-」",         StagePractice.St6A,     LevelPractice.Normal),
                new CardInfo(130, "蘇生「ライジングゲーム」",                 StagePractice.St6A,     LevelPractice.Hard),
                new CardInfo(131, "蘇生「ライジングゲーム」",                 StagePractice.St6A,     LevelPractice.Lunatic),
                new CardInfo(132, "操神「オモイカネディバイス」",             StagePractice.St6A,     LevelPractice.Easy),
                new CardInfo(133, "操神「オモイカネディバイス」",             StagePractice.St6A,     LevelPractice.Normal),
                new CardInfo(134, "神脳「オモイカネブレイン」",               StagePractice.St6A,     LevelPractice.Hard),
                new CardInfo(135, "神脳「オモイカネブレイン」",               StagePractice.St6A,     LevelPractice.Lunatic),
                new CardInfo(136, "天呪「アポロ１３」",                       StagePractice.St6A,     LevelPractice.Easy),
                new CardInfo(137, "天呪「アポロ１３」",                       StagePractice.St6A,     LevelPractice.Normal),
                new CardInfo(138, "天呪「アポロ１３」",                       StagePractice.St6A,     LevelPractice.Hard),
                new CardInfo(139, "天呪「アポロ１３」",                       StagePractice.St6A,     LevelPractice.Lunatic),
                new CardInfo(140, "秘術「天文密葬法」",                       StagePractice.St6A,     LevelPractice.Easy),
                new CardInfo(141, "秘術「天文密葬法」",                       StagePractice.St6A,     LevelPractice.Normal),
                new CardInfo(142, "秘術「天文密葬法」",                       StagePractice.St6A,     LevelPractice.Hard),
                new CardInfo(143, "秘術「天文密葬法」",                       StagePractice.St6A,     LevelPractice.Lunatic),
                new CardInfo(144, "禁薬「蓬莱の薬」",                         StagePractice.St6A,     LevelPractice.Easy),
                new CardInfo(145, "禁薬「蓬莱の薬」",                         StagePractice.St6A,     LevelPractice.Normal),
                new CardInfo(146, "禁薬「蓬莱の薬」",                         StagePractice.St6A,     LevelPractice.Hard),
                new CardInfo(147, "禁薬「蓬莱の薬」",                         StagePractice.St6A,     LevelPractice.Lunatic),
                new CardInfo(148, "薬符「壺中の大銀河」",                     StagePractice.St6B,     LevelPractice.Easy),
                new CardInfo(149, "薬符「壺中の大銀河」",                     StagePractice.St6B,     LevelPractice.Normal),
                new CardInfo(150, "薬符「壺中の大銀河」",                     StagePractice.St6B,     LevelPractice.Hard),
                new CardInfo(151, "薬符「壺中の大銀河」",                     StagePractice.St6B,     LevelPractice.Lunatic),
                new CardInfo(152, "難題「龍の頸の玉　-五色の弾丸-」",         StagePractice.St6B,     LevelPractice.Easy),
                new CardInfo(153, "難題「龍の頸の玉　-五色の弾丸-」",         StagePractice.St6B,     LevelPractice.Normal),
                new CardInfo(154, "神宝「ブリリアントドラゴンバレッタ」",     StagePractice.St6B,     LevelPractice.Hard),
                new CardInfo(155, "神宝「ブリリアントドラゴンバレッタ」",     StagePractice.St6B,     LevelPractice.Lunatic),
                new CardInfo(156, "難題「仏の御石の鉢　-砕けぬ意思-」",       StagePractice.St6B,     LevelPractice.Easy),
                new CardInfo(157, "難題「仏の御石の鉢　-砕けぬ意思-」",       StagePractice.St6B,     LevelPractice.Normal),
                new CardInfo(158, "神宝「ブディストダイアモンド」",           StagePractice.St6B,     LevelPractice.Hard),
                new CardInfo(159, "神宝「ブディストダイアモンド」",           StagePractice.St6B,     LevelPractice.Lunatic),
                new CardInfo(160, "難題「火鼠の皮衣　-焦れぬ心-」",           StagePractice.St6B,     LevelPractice.Easy),
                new CardInfo(161, "難題「火鼠の皮衣　-焦れぬ心-」",           StagePractice.St6B,     LevelPractice.Normal),
                new CardInfo(162, "神宝「サラマンダーシールド」",             StagePractice.St6B,     LevelPractice.Hard),
                new CardInfo(163, "神宝「サラマンダーシールド」",             StagePractice.St6B,     LevelPractice.Lunatic),
                new CardInfo(164, "難題「燕の子安貝　-永命線-」",             StagePractice.St6B,     LevelPractice.Easy),
                new CardInfo(165, "難題「燕の子安貝　-永命線-」",             StagePractice.St6B,     LevelPractice.Normal),
                new CardInfo(166, "神宝「ライフスプリングインフィニティ」",   StagePractice.St6B,     LevelPractice.Hard),
                new CardInfo(167, "神宝「ライフスプリングインフィニティ」",   StagePractice.St6B,     LevelPractice.Lunatic),
                new CardInfo(168, "難題「蓬莱の弾の枝　-虹色の弾幕-」",       StagePractice.St6B,     LevelPractice.Easy),
                new CardInfo(169, "難題「蓬莱の弾の枝　-虹色の弾幕-」",       StagePractice.St6B,     LevelPractice.Normal),
                new CardInfo(170, "神宝「蓬莱の玉の枝　-夢色の郷-」",         StagePractice.St6B,     LevelPractice.Hard),
                new CardInfo(171, "神宝「蓬莱の玉の枝　-夢色の郷-」",         StagePractice.St6B,     LevelPractice.Lunatic),
                new CardInfo(172, "「永夜返し　-初月-」",                     StagePractice.St6B,     LevelPractice.Easy),
                new CardInfo(173, "「永夜返し　-三日月-」",                   StagePractice.St6B,     LevelPractice.Normal),
                new CardInfo(174, "「永夜返し　-上つ弓張-」",                 StagePractice.St6B,     LevelPractice.Hard),
                new CardInfo(175, "「永夜返し　-待宵-」",                     StagePractice.St6B,     LevelPractice.Lunatic),
                new CardInfo(176, "「永夜返し　-子の刻-」",                   StagePractice.St6B,     LevelPractice.Easy),
                new CardInfo(177, "「永夜返し　-子の二つ-」",                 StagePractice.St6B,     LevelPractice.Normal),
                new CardInfo(178, "「永夜返し　-子の三つ-」",                 StagePractice.St6B,     LevelPractice.Hard),
                new CardInfo(179, "「永夜返し　-子の四つ-」",                 StagePractice.St6B,     LevelPractice.Lunatic),
                new CardInfo(180, "「永夜返し　-丑の刻-」",                   StagePractice.St6B,     LevelPractice.Easy),
                new CardInfo(181, "「永夜返し　-丑の二つ-」",                 StagePractice.St6B,     LevelPractice.Normal),
                new CardInfo(182, "「永夜返し　-丑三つ時-」",                 StagePractice.St6B,     LevelPractice.Hard),
                new CardInfo(183, "「永夜返し　-丑の四つ-」",                 StagePractice.St6B,     LevelPractice.Lunatic),
                new CardInfo(184, "「永夜返し　-寅の刻-」",                   StagePractice.St6B,     LevelPractice.Easy),
                new CardInfo(185, "「永夜返し　-寅の二つ-」",                 StagePractice.St6B,     LevelPractice.Normal),
                new CardInfo(186, "「永夜返し　-寅の三つ-」",                 StagePractice.St6B,     LevelPractice.Hard),
                new CardInfo(187, "「永夜返し　-寅の四つ-」",                 StagePractice.St6B,     LevelPractice.Lunatic),
                new CardInfo(188, "「永夜返し　-朝靄-」",                     StagePractice.St6B,     LevelPractice.Easy),
                new CardInfo(189, "「永夜返し　-夜明け-」",                   StagePractice.St6B,     LevelPractice.Normal),
                new CardInfo(190, "「永夜返し　-明けの明星-」",               StagePractice.St6B,     LevelPractice.Hard),
                new CardInfo(191, "「永夜返し　-世明け-」",                   StagePractice.St6B,     LevelPractice.Lunatic),
                new CardInfo(192, "旧史「旧秘境史　-オールドヒストリー-」",   StagePractice.Extra,    LevelPractice.Extra),
                new CardInfo(193, "転世「一条戻り橋」",                       StagePractice.Extra,    LevelPractice.Extra),
                new CardInfo(194, "新史「新幻想史　-ネクストヒストリー-」",   StagePractice.Extra,    LevelPractice.Extra),
                new CardInfo(195, "時効「月のいはかさの呪い」",               StagePractice.Extra,    LevelPractice.Extra),
                new CardInfo(196, "不死「火の鳥　-鳳翼天翔-」",               StagePractice.Extra,    LevelPractice.Extra),
                new CardInfo(197, "藤原「滅罪寺院傷」",                       StagePractice.Extra,    LevelPractice.Extra),
                new CardInfo(198, "不死「徐福時空」",                         StagePractice.Extra,    LevelPractice.Extra),
                new CardInfo(199, "滅罪「正直者の死」",                       StagePractice.Extra,    LevelPractice.Extra),
                new CardInfo(200, "虚人「ウー」",                             StagePractice.Extra,    LevelPractice.Extra),
                new CardInfo(201, "不滅「フェニックスの尾」",                 StagePractice.Extra,    LevelPractice.Extra),
                new CardInfo(202, "蓬莱「凱風快晴　-フジヤマヴォルケイノ-」", StagePractice.Extra,    LevelPractice.Extra),
                new CardInfo(203, "「パゼストバイフェニックス」",             StagePractice.Extra,    LevelPractice.Extra),
                new CardInfo(204, "「蓬莱人形」",                             StagePractice.Extra,    LevelPractice.Extra),
                new CardInfo(205, "「インペリシャブルシューティング」",       StagePractice.Extra,    LevelPractice.Extra),
                new CardInfo(206, "「季節外れのバタフライストーム」",         StagePractice.LastWord, LevelPractice.LastWord),
                new CardInfo(207, "「ブラインドナイトバード」",               StagePractice.LastWord, LevelPractice.LastWord),
                new CardInfo(208, "「日出づる国の天子」",                     StagePractice.LastWord, LevelPractice.LastWord),
                new CardInfo(209, "「幻朧月睨(ルナティックレッドアイズ)」",   StagePractice.LastWord, LevelPractice.LastWord),
                new CardInfo(210, "「天網蜘網捕蝶の法」",                     StagePractice.LastWord, LevelPractice.LastWord),
                new CardInfo(211, "「蓬莱の樹海」",                           StagePractice.LastWord, LevelPractice.LastWord),
                new CardInfo(212, "「フェニックス再誕」",                     StagePractice.LastWord, LevelPractice.LastWord),
                new CardInfo(213, "「エンシェントデューパー」",               StagePractice.LastWord, LevelPractice.LastWord),
                new CardInfo(214, "「無何有浄化」",                           StagePractice.LastWord, LevelPractice.LastWord),
                new CardInfo(215, "「夢想天生」",                             StagePractice.LastWord, LevelPractice.LastWord),
                new CardInfo(216, "「ブレイジングスター」",                   StagePractice.LastWord, LevelPractice.LastWord),
                new CardInfo(217, "「デフレーションワールド」",               StagePractice.LastWord, LevelPractice.LastWord),
                new CardInfo(218, "「待宵反射衛星斬」",                       StagePractice.LastWord, LevelPractice.LastWord),
                new CardInfo(219, "「グランギニョル座の怪人」",               StagePractice.LastWord, LevelPractice.LastWord),
                new CardInfo(220, "「スカーレットディスティニー」",           StagePractice.LastWord, LevelPractice.LastWord),
                new CardInfo(221, "「西行寺無余涅槃」",                       StagePractice.LastWord, LevelPractice.LastWord),
                new CardInfo(222, "「深弾幕結界　-夢幻泡影-」",               StagePractice.LastWord, LevelPractice.LastWord)
            };
            CardTable = cardList.ToDictionary(card => card.Number);

            InitialRanking = new List<HighScore>()
            {
                new HighScore(100000),
                new HighScore( 90000),
                new HighScore( 80000),
                new HighScore( 70000),
                new HighScore( 60000),
                new HighScore( 50000),
                new HighScore( 40000),
                new HighScore( 30000),
                new HighScore( 20000),
                new HighScore( 10000)
            };

            var levels = Utils.GetEnumerator<Level>();
            var levelsWithTotal = Utils.GetEnumerator<LevelWithTotal>();
            var levelsPracticeWithTotal = Utils.GetEnumerator<LevelPracticeWithTotal>();
            var charas = Utils.GetEnumerator<Chara>();
            var charasWithTotal = Utils.GetEnumerator<CharaWithTotal>();
            var stages = Utils.GetEnumerator<Stage>();
            var stagesWithTotal = Utils.GetEnumerator<StageWithTotal>();

            LevelPattern = string.Join(
                string.Empty, levels.Select(lv => lv.ToShortName()).ToArray());
            LevelWithTotalPattern = string.Join(
                string.Empty, levelsWithTotal.Select(lv => lv.ToShortName()).ToArray());
            LevelPracticeWithTotalPattern = string.Join(
                string.Empty, levelsPracticeWithTotal.Select(lv => lv.ToShortName()).ToArray());
            CharaPattern = string.Join(
                "|", charas.Select(ch => ch.ToShortName()).ToArray());
            CharaWithTotalPattern = string.Join(
                "|", charasWithTotal.Select(ch => ch.ToShortName()).ToArray());
            StagePattern = string.Join(
                "|", stages.Select(st => st.ToShortName()).ToArray());
            StageWithTotalPattern = string.Join(
                "|", stagesWithTotal.Select(st => st.ToShortName()).ToArray());

            var comparisonType = StringComparison.OrdinalIgnoreCase;

            ToLevel = (shortName =>
                levels.First(lv => lv.ToShortName().Equals(shortName, comparisonType)));
            ToLevelWithTotal = (shortName =>
                levelsWithTotal.First(lv => lv.ToShortName().Equals(shortName, comparisonType)));
            ToLevelPracticeWithTotal = (shortName =>
                levelsPracticeWithTotal.First(lv => lv.ToShortName().Equals(shortName, comparisonType)));
            ToChara = (shortName =>
                charas.First(ch => ch.ToShortName().Equals(shortName, comparisonType)));
            ToCharaWithTotal = (shortName =>
                charasWithTotal.First(ch => ch.ToShortName().Equals(shortName, comparisonType)));
            ToStage = (shortName =>
                stages.First(st => st.ToShortName().Equals(shortName, comparisonType)));
            ToStageWithTotal = (shortName =>
                stagesWithTotal.First(st => st.ToShortName().Equals(shortName, comparisonType)));
        }

        public Th08Converter()
        {
        }

        protected override bool ReadScoreFile(Stream input)
        {
            using (var decrypted = new MemoryStream())
#if DEBUG
            using (var decoded = new FileStream("th08decoded.dat", FileMode.Create, FileAccess.ReadWrite))
#else
            using (var decoded = new MemoryStream())
#endif
            {
                if (!Decrypt(input, decrypted))
                    return false;

                decrypted.Seek(0, SeekOrigin.Begin);
                if (!Extract(decrypted, decoded))
                    return false;

                decoded.Seek(0, SeekOrigin.Begin);
                if (!Validate(decoded))
                    return false;

                decoded.Seek(0, SeekOrigin.Begin);
                this.allScoreData = Read(decoded);

                return this.allScoreData != null;
            }
        }

        private static bool Decrypt(Stream input, Stream output)
        {
            var size = (int)input.Length;
            ThCrypt.Decrypt(input, output, size, 0x59, 0x79, 0x0100, 0x0C00);

            var data = new byte[size];
            output.Seek(0, SeekOrigin.Begin);
            output.Read(data, 0, size);

            uint checksum = 0;
            byte temp = 0;
            for (var index = 2; index < size; index++)
            {
                temp += data[index - 1];
                temp = (byte)((temp >> 5) | (temp << 3));
                data[index] ^= temp;
                if (index > 3)
                    checksum += data[index];
            }

            output.Seek(0, SeekOrigin.Begin);
            output.Write(data, 0, size);

            return (ushort)checksum == BitConverter.ToUInt16(data, 2);
        }

        private static bool Extract(Stream input, Stream output)
        {
            var reader = new BinaryReader(input);

            reader.ReadUInt16();    // Unknown1
            reader.ReadUInt16();    // Checksum; already checked by this.Decrypt()
            var version = reader.ReadInt16();
            if (version != 1)
                return false;

            reader.ReadUInt16();    // Unknown2
            var headerSize = reader.ReadInt32();
            if (headerSize != 0x1C)
                return false;

            reader.ReadUInt32();    // Unknown3
            var decodedSize = reader.ReadInt32();
            reader.ReadInt32();     // DecodedBodySize
            var encodedBodySize = reader.ReadInt32();
            if (encodedBodySize != (input.Length - headerSize))
                return false;

            var header = new byte[headerSize];
            input.Seek(0, SeekOrigin.Begin);
            input.Read(header, 0, headerSize);
            output.Write(header, 0, headerSize);

            Lzss.Extract(input, output);
            output.Flush();
            output.SetLength(output.Position);

            return output.Position == decodedSize;
        }

        private static bool Validate(Stream input)
        {
            var reader = new BinaryReader(input);
            var chapter = new Chapter();

            reader.ReadBytes(8);
            var headerSize = reader.ReadInt32();
            reader.ReadBytes(4);
            var decodedSize = reader.ReadInt32();
            var remainSize = decodedSize - headerSize;
            reader.ReadBytes(8);
            if (remainSize <= 0)
                return false;

            try
            {
                while (remainSize > 0)
                {
                    chapter.ReadFrom(reader);
                    if (chapter.Size1 == 0)
                        return false;

                    byte temp = 0;
                    switch (chapter.Signature)
                    {
                        case "TH8K":
                            temp = reader.ReadByte();
                            // 8 means the total size of Signature, Size1, and Size2.
                            reader.ReadBytes(chapter.Size1 - 8 - 1);
                            if (temp != 0x01)
                                return false;
                            break;
                        case "VRSM":
                            temp = reader.ReadByte();
                            reader.ReadBytes(chapter.Size1 - 8 - 1);    // 8 means the same above
                            if (temp != 0x01)
                                return false;
                            // th08.exe does something more things here...
                            break;
                        default:
                            reader.ReadBytes(chapter.Size1 - 8);        // 8 means the same above
                            break;
                    }
                    remainSize -= chapter.Size1;
                }
            }
            catch (EndOfStreamException)
            {
                // It's OK, do nothing.
            }

            return remainSize == 0;
        }

        private static AllScoreData Read(Stream input)
        {
            var reader = new BinaryReader(input);
            var allScoreData = new AllScoreData();
            var chapter = new Chapter();

            reader.ReadBytes(0x1C);

            try
            {
                while (true)
                {
                    chapter.ReadFrom(reader);
                    switch (chapter.Signature)
                    {
                        case "TH8K":
                            {
                                var header = new Header(chapter);
                                header.ReadFrom(reader);
                                allScoreData.Header = header;
                            }
                            break;

                        case "HSCR":
                            {
                                var score = new HighScore(chapter);
                                score.ReadFrom(reader);
                                var key = new CharaLevelPair(score.Chara, score.Level);
                                if (!allScoreData.Rankings.ContainsKey(key))
                                    allScoreData.Rankings.Add(key, new List<HighScore>(InitialRanking));
                                var ranking = allScoreData.Rankings[key];
                                ranking.Add(score);
                                ranking.Sort((lhs, rhs) => rhs.Score.CompareTo(lhs.Score));
                                ranking.RemoveAt(ranking.Count - 1);
                            }
                            break;

                        case "CLRD":
                            {
                                var clearData = new ClearData(chapter);
                                clearData.ReadFrom(reader);
                                if (!allScoreData.ClearData.ContainsKey(clearData.Chara))
                                    allScoreData.ClearData.Add(clearData.Chara, clearData);
                            }
                            break;

                        case "CATK":
                            {
                                var attack = new CardAttack(chapter);
                                attack.ReadFrom(reader);
                                if (!allScoreData.CardAttacks.ContainsKey(attack.Number))
                                    allScoreData.CardAttacks.Add(attack.Number, attack);
                            }
                            break;

                        case "PSCR":
                            {
                                var score = new PracticeScore(chapter);
                                score.ReadFrom(reader);
                                if (!allScoreData.PracticeScores.ContainsKey(score.Chara))
                                    allScoreData.PracticeScores.Add(score.Chara, score);
                            }
                            break;

                        case "PLST":
                            {
                                var status = new PlayStatus(chapter);
                                status.ReadFrom(reader);
                                allScoreData.PlayStatus = status;
                            }
                            break;

                        case "LSNM":
                            {
                                var lastName = new LastName(chapter);
                                lastName.ReadFrom(reader);
                                allScoreData.LastName = lastName;
                            }
                            break;

                        case "FLSP":
                            {
                                var flsp = new FLSP(chapter);
                                flsp.ReadFrom(reader);
                                allScoreData.Flsp = flsp;
                            }
                            break;

                        case "VRSM":
                            {
                                var versionInfo = new VersionInfo(chapter);
                                versionInfo.ReadFrom(reader);
                                allScoreData.VersionInfo = versionInfo;
                            }
                            break;

                        default:
                            // 8 means the total size of Signature, Size1, and Size2.
                            reader.ReadBytes(chapter.Size1 - 8);
                            break;
                    }
                }
            }
            catch (EndOfStreamException)
            {
                // It's OK, do nothing.
            }

            if ((allScoreData.Header != null) &&
                // (allScoreData.rankings.Count >= 0) &&
                (allScoreData.ClearData.Count == Enum.GetValues(typeof(CharaWithTotal)).Length) &&
                // (allScoreData.cardAttacks.Length == NumCards) &&
                // (allScoreData.practiceScores.Count >= 0) &&
                (allScoreData.Flsp != null) &&
                (allScoreData.PlayStatus != null) &&
                (allScoreData.LastName != null) &&
                (allScoreData.VersionInfo != null))
                return allScoreData;
            else
                return null;
        }

        protected override void Convert(Stream input, Stream output)
        {
            var reader = new StreamReader(input, Encoding.GetEncoding("shift_jis"));
            var writer = new StreamWriter(output, Encoding.GetEncoding("shift_jis"));

            var allLine = reader.ReadToEnd();
            allLine = this.ReplaceScore(allLine);
            allLine = this.ReplaceCareer(allLine);
            allLine = this.ReplaceCard(allLine);
            allLine = this.ReplaceCollectRate(allLine);
            allLine = this.ReplaceClear(allLine);
            allLine = this.ReplacePlay(allLine);
            allLine = this.ReplaceTime(allLine);
            allLine = this.ReplacePractice(allLine);
            writer.Write(allLine);

            writer.Flush();
            writer.BaseStream.SetLength(writer.BaseStream.Position);
        }

        // %T08SCR[w][xx][y][z]
        private string ReplaceScore(string input)
        {
            var pattern = Utils.Format(@"%T08SCR([{0}])({1})(\d)([\dA-G])", LevelPattern, CharaPattern);
            var evaluator = new MatchEvaluator(match =>
            {
                var level = ToLevel(match.Groups[1].Value);
                var chara = ToChara(match.Groups[2].Value);
                var rank = Utils.ToZeroBased(int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture));
                var type = match.Groups[4].Value.ToUpperInvariant();

                var key = new CharaLevelPair(chara, level);
                var score = this.allScoreData.Rankings.ContainsKey(key)
                    ? this.allScoreData.Rankings[key][rank] : InitialRanking[rank];

                switch (type)
                {
                    case "1":   // name
                        return Encoding.Default.GetString(score.Name).Split('\0')[0];
                    case "2":   // score
                        return this.ToNumberString((score.Score * 10) + score.ContinueCount);
                    case "3":   // stage
                        if ((level == Level.Extra) &&
                            (Encoding.Default.GetString(score.Date).TrimEnd('\0') == "--/--"))
                            return StageProgress.Extra.ToShortName();
                        else
                            return score.StageProgress.ToShortName();
                    case "4":   // date
                        return Encoding.Default.GetString(score.Date).TrimEnd('\0');
                    case "5":   // slow rate
                        return Utils.Format("{0:F3}%", score.SlowRate);
                    case "6":   // play time
                        return new Time(score.PlayTime).ToString();
                    case "7":   // initial number of players
                        return (score.PlayerNum + 1).ToString(CultureInfo.CurrentCulture);
                    case "8":   // point items
                        return this.ToNumberString(score.PointItem);
                    case "9":   // time point
                        return this.ToNumberString(score.TimePoint);
                    case "0":   // miss count
                        return score.MissCount.ToString(CultureInfo.CurrentCulture);
                    case "A":   // bomb count
                        return score.BombCount.ToString(CultureInfo.CurrentCulture);
                    case "B":   // last spell count
                        return score.LastSpellCount.ToString(CultureInfo.CurrentCulture);
                    case "C":   // pause count
                        return this.ToNumberString(score.PauseCount);
                    case "D":   // continue count
                        return score.ContinueCount.ToString(CultureInfo.CurrentCulture);
                    case "E":   // human rate
                        return Utils.Format("{0:F2}%", score.HumanRate / 100.0);
                    case "F":   // got spell cards
                        {
                            var list = CardTable.Values
                                .Where(card => score.CardFlags[card.Number] > 0)
                                .Select(card => Utils.Format("No.{0:D3} {1}", card.Number, card.Name));
                            return string.Join("\n", list.ToArray());
                        }
                    case "G":   // number of got spell cards
                        return score.CardFlags.Values.Count(flag => flag > 0)
                            .ToString(CultureInfo.CurrentCulture);
                    default:    // unreachable
                        return match.ToString();
                }
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T08C[w][xxx][yy][z]
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "StyleCop.CSharp.MaintainabilityRules",
            "SA1119:StatementMustNotUseUnnecessaryParenthesis",
            Justification = "Reviewed.")]
        private string ReplaceCareer(string input)
        {
            var pattern = Utils.Format(@"%T08C([SP])(\d{{3}})({0})([1-3])", CharaWithTotalPattern);
            var evaluator = new MatchEvaluator(match =>
            {
                var kind = match.Groups[1].Value.ToUpperInvariant();
                var number = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);
                var chara = ToCharaWithTotal(match.Groups[3].Value);
                var type = int.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture);

                Func<CardAttack, CardAttackCareer> getCareer = (attack => null);
                if (kind == "S")
                    getCareer = (attack => attack.StoryCareer);
                else
                    getCareer = (attack => attack.PracticeCareer);

                Func<CardAttack, long> getValue = (attack => 0L);
                if (type == 1)
                    getValue = (attack => getCareer(attack).MaxBonuses[chara]);
                else if (type == 2)
                    getValue = (attack => getCareer(attack).ClearCounts[chara]);
                else
                    getValue = (attack => getCareer(attack).TrialCounts[chara]);

                if (number == 0)
                    return this.ToNumberString(this.allScoreData.CardAttacks.Values.Sum(getValue));
                else if (CardTable.ContainsKey(number))
                {
                    CardAttack attack;
                    if (this.allScoreData.CardAttacks.TryGetValue(number, out attack))
                        return this.ToNumberString(getValue(attack));
                    else
                        return "0";
                }
                else
                    return match.ToString();
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T08CARD[xxx][y]
        private string ReplaceCard(string input)
        {
            var pattern = @"%T08CARD(\d{3})([NR])";
            var evaluator = new MatchEvaluator(match =>
            {
                var number = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
                var type = match.Groups[2].Value.ToUpperInvariant();

                if (CardTable.ContainsKey(number))
                {
                    CardAttack attack;
                    if (this.allScoreData.CardAttacks.TryGetValue(number, out attack) && attack.HasTried())
                    {
                        if (type == "N")
                            return CardTable[number].Name;
                        else
                        {
                            var level = CardTable[number].Level;
                            var levelName = level.ToLongName();
                            return (levelName.Length > 0) ? levelName : level.ToString();
                        }
                    }
                    else
                        return (type == "N") ? "??????????" : "?????";
                }
                else
                    return match.ToString();
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T08CRG[v][w][xx][yy][z]
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "StyleCop.CSharp.MaintainabilityRules",
            "SA1119:StatementMustNotUseUnnecessaryParenthesis",
            Justification = "Reviewed.")]
        private string ReplaceCollectRate(string input)
        {
            var pattern = Utils.Format(
                @"%T08CRG([SP])([{0}])({1})({2})([12])",
                LevelPracticeWithTotalPattern,
                CharaWithTotalPattern,
                StageWithTotalPattern);
            var evaluator = new MatchEvaluator(match =>
            {
                var kind = match.Groups[1].Value.ToUpperInvariant();
                var level = ToLevelPracticeWithTotal(match.Groups[2].Value);
                var chara = ToCharaWithTotal(match.Groups[3].Value);
                var stage = ToStageWithTotal(match.Groups[4].Value);
                var type = int.Parse(match.Groups[5].Value, CultureInfo.InvariantCulture);

                if (stage == StageWithTotal.Extra)
                    return match.ToString();

                Func<CardAttack, bool> findByKindType = (attack => true);
                Func<CardAttack, bool> findByLevel = (attack => true);
                Func<CardAttack, bool> findByStage = (attack => true);

                if (kind == "S")
                {
                    if (type == 1)
                        findByKindType = (attack => attack.StoryCareer.ClearCounts[chara] > 0);
                    else
                        findByKindType = (attack => attack.StoryCareer.TrialCounts[chara] > 0);
                }
                else
                {
                    if (type == 1)
                        findByKindType = (attack => attack.PracticeCareer.ClearCounts[chara] > 0);
                    else
                        findByKindType = (attack => attack.PracticeCareer.TrialCounts[chara] > 0);
                }

                if (stage == StageWithTotal.Total)
                {
                    // Do nothing
                }
                else
                    findByStage = (attack => CardTable[attack.Number].Stage == (StagePractice)stage);

                switch (level)
                {
                    case LevelPracticeWithTotal.Total:
                        // Do nothing
                        break;
                    case LevelPracticeWithTotal.Extra:
                        findByStage =
                            (attack => CardTable[attack.Number].Stage == StagePractice.Extra);
                        break;
                    case LevelPracticeWithTotal.LastWord:
                        findByStage =
                            (attack => CardTable[attack.Number].Stage == StagePractice.LastWord);
                        break;
                    default:
                        findByLevel = (attack => attack.Level == level);
                        break;
                }

                var and = Utils.MakeAndPredicate(findByKindType, findByLevel, findByStage);
                return this.allScoreData.CardAttacks.Values.Count(and).ToString(CultureInfo.CurrentCulture);
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T08CLEAR[x][yy]
        private string ReplaceClear(string input)
        {
            var pattern = Utils.Format(@"%T08CLEAR([{0}])({1})", LevelPattern, CharaPattern);
            var evaluator = new MatchEvaluator(match =>
            {
                var level = ToLevel(match.Groups[1].Value);
                var chara = ToChara(match.Groups[2].Value);

                var key = new CharaLevelPair(chara, level);
                if (this.allScoreData.Rankings.ContainsKey(key))
                {
                    var stageProgress = this.allScoreData.Rankings[key].Max(rank => rank.StageProgress);
                    if ((stageProgress == StageProgress.St4A) || (stageProgress == StageProgress.St4B))
                        return "Stage 4";
                    else if (stageProgress == StageProgress.Extra)
                        return "Not Clear";
                    else if (stageProgress == StageProgress.Clear)
                    {
                        if ((level != Level.Extra) &&
                            ((this.allScoreData.ClearData[(CharaWithTotal)chara].StoryFlags[level]
                                & PlayableStages.Stage6B) != PlayableStages.Stage6B))
                            return "FinalA Clear";
                        else
                            return stageProgress.ToShortName();
                    }
                    else
                        return stageProgress.ToShortName();
                }
                else
                    return "-------";
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T08PLAY[x][yy]
        private string ReplacePlay(string input)
        {
            var pattern = Utils.Format(
                @"%T08PLAY([{0}])({1}|CL|CN|PR)", LevelWithTotalPattern, CharaWithTotalPattern);
            var evaluator = new MatchEvaluator(match =>
            {
                var level = ToLevelWithTotal(match.Groups[1].Value);
                var charaAndMore = match.Groups[2].Value.ToUpperInvariant();

                var playCount = (level == LevelWithTotal.Total)
                    ? this.allScoreData.PlayStatus.TotalPlayCount
                    : this.allScoreData.PlayStatus.PlayCounts[(Level)level];

                switch (charaAndMore)
                {
                    case "CL":  // clear count
                        return this.ToNumberString(playCount.TotalClear);
                    case "CN":  // continue count
                        return this.ToNumberString(playCount.TotalContinue);
                    case "PR":  // practice count
                        return this.ToNumberString(playCount.TotalPractice);
                    default:
                        {
                            var chara = ToCharaWithTotal(match.Groups[2].Value);
                            return this.ToNumberString((chara == CharaWithTotal.Total)
                                ? playCount.TotalTrial : playCount.Trials[(Chara)chara]);
                        }
                }
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T08TIME(ALL|PLY)
        private string ReplaceTime(string input)
        {
            var pattern = @"%T08TIME(ALL|PLY)";
            var evaluator = new MatchEvaluator(match =>
            {
                var kind = match.Groups[1].Value.ToUpperInvariant();

                return (kind == "ALL")
                    ? this.allScoreData.PlayStatus.TotalRunningTime.ToLongString()
                    : this.allScoreData.PlayStatus.TotalPlayTime.ToLongString();
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T08PRAC[w][xx][yy][z]
        private string ReplacePractice(string input)
        {
            var pattern = Utils.Format(
                @"%T08PRAC([{0}])({1})({2})([12])", LevelPattern, CharaPattern, StagePattern);
            var evaluator = new MatchEvaluator(match =>
            {
                var level = ToLevel(match.Groups[1].Value);
                var chara = ToChara(match.Groups[2].Value);
                var stage = ToStage(match.Groups[3].Value);
                var type = int.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture);

                if (level == Level.Extra)
                    return match.ToString();
                if (stage == Stage.Extra)
                    return match.ToString();

                if (this.allScoreData.PracticeScores.ContainsKey(chara))
                {
                    var scores = this.allScoreData.PracticeScores[chara];
                    var key = new StageLevelPair(stage, level);
                    if (type == 1)
                        return scores.HighScores.ContainsKey(key)
                            ? this.ToNumberString(scores.HighScores[key] * 10) : "0";
                    else
                        return scores.PlayCounts.ContainsKey(key)
                            ? this.ToNumberString(scores.PlayCounts[key]) : "0";
                }
                else
                    return "0";
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }
    }
}

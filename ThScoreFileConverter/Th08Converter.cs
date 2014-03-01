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

    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "StyleCop.CSharp.OrderingRules",
        "SA1201:ElementsMustAppearInTheCorrectOrder",
        Justification = "Reviewed.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "StyleCop.CSharp.SpacingRules",
        "SA1025:CodeMustNotContainMultipleWhitespaceInARow",
        Justification = "Reviewed.")]
    public class Th08Converter : ThConverter
    {
        private enum Level
        {
            [EnumAltName("E")] Easy,
            [EnumAltName("N")] Normal,
            [EnumAltName("H")] Hard,
            [EnumAltName("L")] Lunatic,
            [EnumAltName("X")] Extra
        }
        private enum LevelWithTotal
        {
            [EnumAltName("E")] Easy,
            [EnumAltName("N")] Normal,
            [EnumAltName("H")] Hard,
            [EnumAltName("L")] Lunatic,
            [EnumAltName("X")] Extra,
            [EnumAltName("T")] Total
        }
        private enum LevelPractice
        {
            [EnumAltName("E")] Easy,
            [EnumAltName("N")] Normal,
            [EnumAltName("H")] Hard,
            [EnumAltName("L")] Lunatic,
            [EnumAltName("X")] Extra,
            [EnumAltName("W", LongName = "Last Word")] LastWord,
            [EnumAltName("T")] Total
        }

        private enum Chara
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
        private enum CharaWithTotal
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

        private enum Stage
        {
            [EnumAltName("1A")] Stage1,
            [EnumAltName("2A")] Stage2,
            [EnumAltName("3A")] Stage3,
            [EnumAltName("4A")] Stage4A,
            [EnumAltName("4B")] Stage4B,
            [EnumAltName("5A")] Stage5,
            [EnumAltName("6A")] Stage6A,
            [EnumAltName("6B")] Stage6B,
            [EnumAltName("EX")] Extra
        }
        private enum StageWithTotal
        {
            [EnumAltName("1A")] Stage1,
            [EnumAltName("2A")] Stage2,
            [EnumAltName("3A")] Stage3,
            [EnumAltName("4A")] Stage4A,
            [EnumAltName("4B")] Stage4B,
            [EnumAltName("5A")] Stage5,
            [EnumAltName("6A")] Stage6A,
            [EnumAltName("6B")] Stage6B,
            [EnumAltName("EX")] Extra,
            [EnumAltName("00")] Total
        }
        private enum StagePractice
        {
            [EnumAltName("1A")] Stage1,
            [EnumAltName("2A")] Stage2,
            [EnumAltName("3A")] Stage3,
            [EnumAltName("4A")] Stage4A,
            [EnumAltName("4B")] Stage4B,
            [EnumAltName("5A")] Stage5,
            [EnumAltName("6A")] Stage6A,
            [EnumAltName("6B")] Stage6B,
            [EnumAltName("EX")] Extra,
            [EnumAltName("LW", LongName = "Last Word")] LastWord
        }

        private enum StageProgress
        {
            [EnumAltName("Stage 1")]          Stage1,
            [EnumAltName("Stage 2")]          Stage2,
            [EnumAltName("Stage 3")]          Stage3,
            [EnumAltName("Stage 4-uncanny")]  Stage4A,
            [EnumAltName("Stage 4-powerful")] Stage4B,
            [EnumAltName("Stage 5")]          Stage5,
            [EnumAltName("Stage 6-Eirin")]    Stage6A,
            [EnumAltName("Stage 6-Kaguya")]   Stage6B,
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "StyleCop.CSharp.SpacingRules",
            "SA1008:OpeningParenthesisMustBeSpacedCorrectly",
            Justification = "Reviewed.")]
        private static readonly List<HighScore> InitialRanking = new List<HighScore>()
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

        private const int NumCards = 222;

        // Thanks to thwiki.info
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "StyleCop.CSharp.SpacingRules",
            "SA1008:OpeningParenthesisMustBeSpacedCorrectly",
            Justification = "Reviewed.")]
        private static readonly Dictionary<StagePractice, Range<int>> StageCardTable =
            new Dictionary<StagePractice, Range<int>>()
            {
                { StagePractice.Stage1,   new Range<int>(  0,  12) },
                { StagePractice.Stage2,   new Range<int>( 13,  31) },
                { StagePractice.Stage3,   new Range<int>( 32,  53) },
                { StagePractice.Stage4A,  new Range<int>( 54,  76) },
                { StagePractice.Stage4B,  new Range<int>( 77,  99) },
                { StagePractice.Stage5,   new Range<int>(100, 118) },
                { StagePractice.Stage6A,  new Range<int>(119, 146) },
                { StagePractice.Stage6B,  new Range<int>(147, 190) },
                { StagePractice.Extra,    new Range<int>(191, 204) },
                { StagePractice.LastWord, new Range<int>(205, 221) }
            };

        private class CharaLevelPair : Pair<Chara, Level>
        {
            public Chara Chara { get { return this.First; } }
            public Level Level { get { return this.Second; } }

            public CharaLevelPair(Chara chara, Level level) : base(chara, level) { }
        }

        private class StageLevelPair : Pair<Stage, Level>
        {
            public Stage Stage { get { return this.First; } }
            public Level Level { get { return this.Second; } }

            public StageLevelPair(Stage stage, Level level) : base(stage, level) { }
        }

        private class AllScoreData
        {
            public Header Header { get; set; }
            public Dictionary<CharaLevelPair, List<HighScore>> Rankings { get; set; }
            public Dictionary<CharaWithTotal, ClearData> ClearData { get; set; }
            public CardAttack[] CardAttacks { get; set; }
            public Dictionary<Chara, PracticeScore> PracticeScores { get; set; }
            public FLSP Flsp { get; set; }
            public PlayStatus PlayStatus { get; set; }
            public LastName LastName { get; set; }
            public VersionInfo VersionInfo { get; set; }

            public AllScoreData()
            {
                this.Rankings = new Dictionary<CharaLevelPair, List<HighScore>>();
                this.ClearData =
                    new Dictionary<CharaWithTotal, ClearData>(Enum.GetValues(typeof(CharaWithTotal)).Length);
                this.CardAttacks = new CardAttack[NumCards];
                this.PracticeScores =
                    new Dictionary<Chara, PracticeScore>(Enum.GetValues(typeof(Chara)).Length);
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
            private uint unknown;   // always 0x00000001?

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
                this.unknown = reader.ReadUInt32();
            }
        }

        private class HighScore : Chapter   // per character, level, rank
        {
            private uint unknown1;      // always 0x00000004?
            private byte[] unknown2;    // .Length = 0x1C
            // 01 00 00 00 04 00 09 00 FF FF FF FF FF FF FF FF
            // 05 00 00 00 01 00 08 00 58 02 58 02
            private byte[] unknown3;    // .Length = 0x1F
            // NN 03 00 01 01 LL 01 00 02 00 00 ** ** 00 00 00
            // 00 00 00 00 00 00 00 00 00 00 00 00 01 40 00 00
            // where NN: PlayerNum, LL: level, **: unknown (0x64 or 0x0A; 0x50 or 0x0A)
            private uint unknown4;      // always 0x00000000?

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
            public int HumanRate { get; private set; }      // / 100
            public byte[] CardFlags { get; private set; }   // .Length = 222
            public byte[] Padding { get; private set; }     // .Length = 2

            public HighScore(Chapter ch)
                : base(ch)
            {
                if (this.Signature != "HSCR")
                    throw new InvalidDataException("Signature");
                if (this.Size1 != 0x0168)
                    throw new InvalidDataException("Size1");
                // if (this.Size2 != 0x0168)
                //     throw new InvalidDataException("Size2");
            }

            public HighScore(uint score)    // for InitialRanking only
                : base()
            {
                this.Score = score;
                this.Name = Encoding.Default.GetBytes("--------\0");
                this.Date = Encoding.Default.GetBytes("--/--\0");
                this.CardFlags = new byte[NumCards];
            }

            public override void ReadFrom(BinaryReader reader)
            {
                this.unknown1 = reader.ReadUInt32();
                this.Score = reader.ReadUInt32();
                this.SlowRate = reader.ReadSingle();
                this.Chara = (Chara)reader.ReadByte();
                this.Level = (Level)reader.ReadByte();
                this.StageProgress = (StageProgress)reader.ReadByte();
                this.Name = reader.ReadBytes(9);
                this.Date = reader.ReadBytes(6);
                this.ContinueCount = reader.ReadUInt16();
                this.unknown2 = reader.ReadBytes(0x1C);
                this.PlayerNum = reader.ReadByte();
                this.unknown3 = reader.ReadBytes(0x1F);
                this.PlayTime = reader.ReadUInt32();
                this.PointItem = reader.ReadInt32();
                this.unknown4 = reader.ReadUInt32();
                this.MissCount = reader.ReadInt32();
                this.BombCount = reader.ReadInt32();
                this.LastSpellCount = reader.ReadInt32();
                this.PauseCount = reader.ReadInt32();
                this.TimePoint = reader.ReadInt32();
                this.HumanRate = reader.ReadInt32();
                this.CardFlags = reader.ReadBytes(Th08Converter.NumCards);
                this.Padding = reader.ReadBytes(2);
            }
        }

        private class ClearData : Chapter   // per character-with-total
        {
            private uint unknown1;      // always 0x00000004?
            private byte unknown2;      // always 0x00?
            private ushort unknown3;    // always 0x0000?

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
                this.unknown1 = reader.ReadUInt32();
                foreach (var level in levels)
                    this.StoryFlags[level] = (PlayableStages)reader.ReadUInt16();
                foreach (var level in levels)
                    this.PracticeFlags[level] = (PlayableStages)reader.ReadUInt16();
                this.unknown2 = reader.ReadByte();
                this.Chara = (CharaWithTotal)reader.ReadByte();
                this.unknown3 = reader.ReadUInt16();
            }
        }

        private class CardAttack : Chapter      // per card
        {
            private uint unknown1;  // always 0x00000003?
            private byte unknown2;  // variable...
            private uint unknown3;  // always 0x00000000?

            public short Number { get; private set; }       // 0-based
            public LevelPractice Level { get; private set; }
            public byte[] CardName { get; private set; }    // .Length = 0x30
            public byte[] EnemyName { get; private set; }   // .Length = 0x30
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
                this.unknown1 = reader.ReadUInt32();
                this.Number = reader.ReadInt16();
                this.unknown2 = reader.ReadByte();
                this.Level = (LevelPractice)reader.ReadByte();
                this.CardName = reader.ReadBytes(0x30);
                this.EnemyName = reader.ReadBytes(0x30);
                this.Comment = reader.ReadBytes(0x80);
                this.StoryCareer.ReadFrom(reader);
                this.PracticeCareer.ReadFrom(reader);
                this.unknown3 = reader.ReadUInt32();
            }

            public bool HasTried()
            {
                return Encoding.Default.GetString(this.CardName).TrimEnd('\0').Length > 0;
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
            private uint unknown1;      // always 0x00000002?
            private byte[] unknown2;    // .Length = 3, always 0x000001?

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

                this.PlayCounts = new Dictionary<StageLevelPair, int>();
                this.HighScores = new Dictionary<StageLevelPair, int>();
            }

            public override void ReadFrom(BinaryReader reader)
            {
                // The fields for Stage.Extra and Level.Extra actually exist...

                var stages = Utils.GetEnumerator<Stage>();
                var levels = Utils.GetEnumerator<Level>();
                this.unknown1 = reader.ReadUInt32();
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
                this.unknown2 = reader.ReadBytes(3);
            }
        }

        private class FLSP : Chapter    // FIXME
        {
            private byte[] unknown;     // .Length = 0x18

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
                this.unknown = reader.ReadBytes(0x18);
            }
        }

        private class PlayStatus : Chapter
        {
            private uint unknown1;      // always 0x00000002?
            private PlayCount unknown2; // always all 0?

            public Time TotalRunningTime { get; private set; }
            public Time TotalPlayTime { get; private set; }
            public Dictionary<Level, PlayCount> PlayCounts { get; private set; }
            public PlayCount TotalPlayCount { get; private set; }
            public byte[] BgmFlags { get; private set; }            // .Length = 21
            public byte[] Padding { get; private set; }             // .Length = 11

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
                this.unknown2 = new PlayCount();
                this.TotalPlayCount = new PlayCount();
            }

            public override void ReadFrom(BinaryReader reader)
            {
                this.unknown1 = reader.ReadUInt32();
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
                this.unknown2.ReadFrom(reader);
                this.TotalPlayCount.ReadFrom(reader);
                this.BgmFlags = reader.ReadBytes(21);
                this.Padding = reader.ReadBytes(11);
            }
        }

        private class PlayCount : IBinaryReadable     // per level-with-total
        {
            private uint unknown;   // always 0x00000000?

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
                this.unknown = reader.ReadUInt32();
                this.TotalClear = reader.ReadInt32();
                this.TotalContinue = reader.ReadInt32();
                this.TotalPractice = reader.ReadInt32();
            }
        }

        private class LastName : Chapter
        {
            private uint unknown;   // always 0x00000001?

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
                this.unknown = reader.ReadUInt32();
                this.Name = reader.ReadBytes(12);
            }
        }

        private class VersionInfo : Chapter
        {
            private uint unknown1;      // always 0x00000001?
            private byte[] unknown2;    // .Length = 3
            private byte[] unknown3;    // .Length = 3
            private uint unknown4;      // variable...

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
                this.unknown1 = reader.ReadUInt32();
                this.Version = reader.ReadBytes(6);
                this.unknown2 = reader.ReadBytes(3);
                this.unknown3 = reader.ReadBytes(3);
                this.unknown4 = reader.ReadUInt32();
            }
        }

        private AllScoreData allScoreData = null;

        public override string SupportedVersions
        {
            get { return "1.00d"; }
        }

        private static readonly string LevelPattern;
        private static readonly string LevelExceptExtraPattern;
        private static readonly string LevelWithTotalPattern;
        private static readonly string LevelPracticePattern;
        private static readonly string CharaPattern;
        private static readonly string CharaWithTotalPattern;
        private static readonly string StageExceptExtraPattern;
        private static readonly string StageWithTotalExceptExtraPattern;

        private static readonly Func<string, StringComparison, Level> ToLevel;
        private static readonly Func<string, StringComparison, LevelWithTotal> ToLevelWithTotal;
        private static readonly Func<string, StringComparison, LevelPractice> ToLevelPractice;
        private static readonly Func<string, StringComparison, Chara> ToChara;
        private static readonly Func<string, StringComparison, CharaWithTotal> ToCharaWithTotal;
        private static readonly Func<string, StringComparison, Stage> ToStage;
        private static readonly Func<string, StringComparison, StageWithTotal> ToStageWithTotal;

        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Performance",
            "CA1810:InitializeReferenceTypeStaticFieldsInline",
            Justification = "Reviewed.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "StyleCop.CSharp.MaintainabilityRules",
            "SA1119:StatementMustNotUseUnnecessaryParenthesis",
            Justification = "Reviewed.")]
        static Th08Converter()
        {
            var levels = Utils.GetEnumerator<Level>();
            var levelsWithTotal = Utils.GetEnumerator<LevelWithTotal>();
            var levelsPractice = Utils.GetEnumerator<LevelPractice>();
            var charas = Utils.GetEnumerator<Chara>();
            var charasWithTotal = Utils.GetEnumerator<CharaWithTotal>();
            var stages = Utils.GetEnumerator<Stage>();
            var stagesWithTotal = Utils.GetEnumerator<StageWithTotal>();

            // To avoid SA1118
            var levelsExceptExtra = levels.Where(lv => lv != Level.Extra);
            var stagesExceptExtra = stages.Where(st => st != Stage.Extra);
            var stagesWithTotalExceptExtra = stagesWithTotal.Where(st => st != StageWithTotal.Extra);

            LevelPattern = string.Join(
                string.Empty, levels.Select(lv => lv.ToShortName()).ToArray());
            LevelExceptExtraPattern = string.Join(
                string.Empty, levelsExceptExtra.Select(lv => lv.ToShortName()).ToArray());
            LevelWithTotalPattern = string.Join(
                string.Empty, levelsWithTotal.Select(lv => lv.ToShortName()).ToArray());
            LevelPracticePattern = string.Join(
                string.Empty, levelsPractice.Select(lv => lv.ToShortName()).ToArray());
            CharaPattern = string.Join(
                "|", charas.Select(ch => ch.ToShortName()).ToArray());
            CharaWithTotalPattern = string.Join(
                "|", charasWithTotal.Select(ch => ch.ToShortName()).ToArray());
            StageExceptExtraPattern = string.Join(
                "|", stagesExceptExtra.Select(st => st.ToShortName()).ToArray());
            StageWithTotalExceptExtraPattern = string.Join(
                "|", stagesWithTotalExceptExtra.Select(st => st.ToShortName()).ToArray());

            ToLevel = ((shortName, comparisonType) =>
                levels.First(lv => lv.ToShortName().Equals(shortName, comparisonType)));
            ToLevelWithTotal = ((shortName, comparisonType) =>
                levelsWithTotal.First(lv => lv.ToShortName().Equals(shortName, comparisonType)));
            ToLevelPractice = ((shortName, comparisonType) =>
                levelsPractice.First(lv => lv.ToShortName().Equals(shortName, comparisonType)));
            ToChara = ((shortName, comparisonType) =>
                charas.First(ch => ch.ToShortName().Equals(shortName, comparisonType)));
            ToCharaWithTotal = ((shortName, comparisonType) =>
                charasWithTotal.First(ch => ch.ToShortName().Equals(shortName, comparisonType)));
            ToStage = ((shortName, comparisonType) =>
                stages.First(st => st.ToShortName().Equals(shortName, comparisonType)));
            ToStageWithTotal = ((shortName, comparisonType) =>
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

            allScoreData.Rankings = new Dictionary<CharaLevelPair, List<HighScore>>();
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
                                allScoreData.CardAttacks[attack.Number] = attack;
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
                var level = ToLevel(match.Groups[1].Value, StringComparison.OrdinalIgnoreCase);
                var chara = ToChara(match.Groups[2].Value, StringComparison.OrdinalIgnoreCase);
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
                            var list = new List<string>();
                            for (var index = 0; index < NumCards; index++)
                            {
                                if (score.CardFlags[index] > 0)
                                {
                                    var attack = this.allScoreData.CardAttacks[index];
                                    var str = Utils.Format(
                                        "No.{0:D3} {1}",
                                        attack.Number + 1,
                                        Encoding.Default.GetString(attack.CardName).TrimEnd('\0'));
                                    list.Add(str);
                                }
                            }
                            return string.Join("\n", list.ToArray());
                        }
                    case "G":   // number of got spell cards
                        return score.CardFlags.Count(flag => flag > 0).ToString(CultureInfo.CurrentCulture);
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
                var chara = ToCharaWithTotal(match.Groups[3].Value, StringComparison.OrdinalIgnoreCase);
                var type = int.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture);

                Func<CardAttack, CardAttackCareer> getCareer = (attack => null);
                if (kind == "S")
                    getCareer = (attack => attack.StoryCareer);
                else
                    getCareer = (attack => attack.PracticeCareer);

                Func<CardAttackCareer, long> getValue = (career => 0L);
                if (type == 1)
                    getValue = (career => career.MaxBonuses[chara]);
                else if (type == 2)
                    getValue = (career => career.ClearCounts[chara]);
                else
                    getValue = (career => career.TrialCounts[chara]);

                Func<CardAttack, long> getValueWithNullCheck =
                    (attack => (attack != null) ? getValue(getCareer(attack)) : 0L);

                if (number == 0)
                    return this.ToNumberString(
                        this.allScoreData.CardAttacks.Sum(getValueWithNullCheck));
                else if (new Range<int>(1, NumCards).Contains(number))
                    return this.ToNumberString(
                        getValueWithNullCheck(this.allScoreData.CardAttacks[number - 1]));
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

                if (new Range<int>(1, NumCards).Contains(number))
                {
                    var attack = this.allScoreData.CardAttacks[number - 1];
                    if (type == "N")
                        return ((attack != null) && attack.HasTried())
                            ? Encoding.Default.GetString(attack.CardName).TrimEnd('\0') : "??????????";
                    else
                    {
                        if ((attack != null) && attack.HasTried())
                        {
                            var levelName = attack.Level.ToLongName();
                            return (levelName.Length > 0) ? levelName : attack.Level.ToString();
                        }
                        else
                            return "?????";
                    }
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
                LevelPracticePattern,
                CharaWithTotalPattern,
                StageWithTotalExceptExtraPattern);
            var evaluator = new MatchEvaluator(match =>
            {
                var kind = match.Groups[1].Value.ToUpperInvariant();
                var level = ToLevelPractice(match.Groups[2].Value, StringComparison.OrdinalIgnoreCase);
                var chara = ToCharaWithTotal(match.Groups[3].Value, StringComparison.OrdinalIgnoreCase);
                var stage = ToStageWithTotal(match.Groups[4].Value, StringComparison.OrdinalIgnoreCase);
                var type = int.Parse(match.Groups[5].Value, CultureInfo.InvariantCulture);

                Func<CardAttack, bool> checkNotNull = (attack => attack != null);
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
                    findByStage = (attack => StageCardTable[(StagePractice)stage].Contains(attack.Number));

                switch (level)
                {
                    case LevelPractice.Total:
                        // Do nothing
                        break;
                    case LevelPractice.Extra:
                        findByStage =
                            (attack => StageCardTable[StagePractice.Extra].Contains(attack.Number));
                        break;
                    case LevelPractice.LastWord:
                        findByStage =
                            (attack => StageCardTable[StagePractice.LastWord].Contains(attack.Number));
                        break;
                    default:
                        findByLevel = (attack => attack.Level == level);
                        break;
                }

                var and = Utils.MakeAndPredicate(checkNotNull, findByKindType, findByLevel, findByStage);
                return this.allScoreData.CardAttacks.Count(and).ToString(CultureInfo.CurrentCulture);
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T08CLEAR[x][yy]
        private string ReplaceClear(string input)
        {
            var pattern = Utils.Format(@"%T08CLEAR([{0}])({1})", LevelPattern, CharaPattern);
            var evaluator = new MatchEvaluator(match =>
            {
                var level = ToLevel(match.Groups[1].Value, StringComparison.OrdinalIgnoreCase);
                var chara = ToChara(match.Groups[2].Value, StringComparison.OrdinalIgnoreCase);

                var key = new CharaLevelPair(chara, level);
                if (this.allScoreData.Rankings.ContainsKey(key))
                {
                    var stageProgress = this.allScoreData.Rankings[key].Max(rank => rank.StageProgress);
                    if ((stageProgress == StageProgress.Stage4A) || (stageProgress == StageProgress.Stage4B))
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
                var level = ToLevelWithTotal(match.Groups[1].Value, StringComparison.OrdinalIgnoreCase);
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
                            var chara = ToCharaWithTotal(
                                match.Groups[2].Value, StringComparison.OrdinalIgnoreCase);
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
                @"%T08PRAC([{0}])({1})({2})([12])",
                LevelExceptExtraPattern,
                CharaPattern,
                StageExceptExtraPattern);
            var evaluator = new MatchEvaluator(match =>
            {
                var level = ToLevel(match.Groups[1].Value, StringComparison.OrdinalIgnoreCase);
                var chara = ToChara(match.Groups[2].Value, StringComparison.OrdinalIgnoreCase);
                var stage = ToStage(match.Groups[3].Value, StringComparison.OrdinalIgnoreCase);
                var type = int.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture);

                if (level == Level.Extra)
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

//-----------------------------------------------------------------------
// <copyright file="Th07Converter.cs" company="None">
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
    internal class Th07Converter : ThConverter
    {
        public enum Level
        {
            [EnumAltName("E")] Easy,
            [EnumAltName("N")] Normal,
            [EnumAltName("H")] Hard,
            [EnumAltName("L")] Lunatic,
            [EnumAltName("X")] Extra,
            [EnumAltName("P")] Phantasm
        }
        public enum LevelWithTotal
        {
            [EnumAltName("E")] Easy,
            [EnumAltName("N")] Normal,
            [EnumAltName("H")] Hard,
            [EnumAltName("L")] Lunatic,
            [EnumAltName("X")] Extra,
            [EnumAltName("P")] Phantasm,
            [EnumAltName("T")] Total
        }

        public enum Chara
        {
            [EnumAltName("RA")] ReimuA,
            [EnumAltName("RB")] ReimuB,
            [EnumAltName("MA")] MarisaA,
            [EnumAltName("MB")] MarisaB,
            [EnumAltName("SA")] SakuyaA,
            [EnumAltName("SB")] SakuyaB
        }
        public enum CharaWithTotal
        {
            [EnumAltName("RA")] ReimuA,
            [EnumAltName("RB")] ReimuB,
            [EnumAltName("MA")] MarisaA,
            [EnumAltName("MB")] MarisaB,
            [EnumAltName("SA")] SakuyaA,
            [EnumAltName("SB")] SakuyaB,
            [EnumAltName("TL")] Total
        }

        public enum Stage
        {
            [EnumAltName("1")] Stage1,
            [EnumAltName("2")] Stage2,
            [EnumAltName("3")] Stage3,
            [EnumAltName("4")] Stage4,
            [EnumAltName("5")] Stage5,
            [EnumAltName("6")] Stage6,
            [EnumAltName("X")] Extra,
            [EnumAltName("P")] Phantasm
        }
        public enum StageWithTotal
        {
            [EnumAltName("1")] Stage1,
            [EnumAltName("2")] Stage2,
            [EnumAltName("3")] Stage3,
            [EnumAltName("4")] Stage4,
            [EnumAltName("5")] Stage5,
            [EnumAltName("6")] Stage6,
            [EnumAltName("X")] Extra,
            [EnumAltName("P")] Phantasm,
            [EnumAltName("0")] Total
        }

        public enum StageProgress
        {
            [EnumAltName("-------")]        None,
            [EnumAltName("Stage 1")]        Stage1,
            [EnumAltName("Stage 2")]        Stage2,
            [EnumAltName("Stage 3")]        Stage3,
            [EnumAltName("Stage 4")]        Stage4,
            [EnumAltName("Stage 5")]        Stage5,
            [EnumAltName("Stage 6")]        Stage6,
            [EnumAltName("Extra Stage")]    Extra,
            [EnumAltName("Phantasm Stage")] Phantasm,
            [EnumAltName("All Clear")]      Clear = 99
        }

        private const int NumCards = 141;

        // Thanks to thwiki.info
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "StyleCop.CSharp.SpacingRules",
            "SA1008:OpeningParenthesisMustBeSpacedCorrectly",
            Justification = "Reviewed.")]
        private static readonly Dictionary<Stage, Range<int>> StageCardTable =
            new Dictionary<Stage, Range<int>>()
            {
                { Stage.Stage1,   new Range<int>(  0,   9) },
                { Stage.Stage2,   new Range<int>( 10,  25) },
                { Stage.Stage3,   new Range<int>( 26,  43) },
                { Stage.Stage4,   new Range<int>( 44,  67) },
                { Stage.Stage5,   new Range<int>( 68,  87) },
                { Stage.Stage6,   new Range<int>( 88, 115) },
                { Stage.Extra,    new Range<int>(116, 127) },
                { Stage.Phantasm, new Range<int>(128, 140) }
            };

        // Thanks to www57.atwiki.jp/2touhoukouryaku
        private static readonly Level[] CardLevelTable =
        {
            Level.Hard, Level.Lunatic, Level.Easy, Level.Normal, Level.Hard,
            Level.Lunatic, Level.Easy, Level.Normal, Level.Hard, Level.Lunatic,
            Level.Easy, Level.Normal, Level.Hard, Level.Lunatic, Level.Easy,
            Level.Normal, Level.Hard, Level.Lunatic, Level.Easy, Level.Normal,
            Level.Hard, Level.Lunatic, Level.Easy, Level.Normal, Level.Hard,
            Level.Lunatic, Level.Hard, Level.Lunatic, Level.Easy, Level.Normal,
            Level.Hard, Level.Lunatic, Level.Easy, Level.Normal, Level.Hard,
            Level.Lunatic, Level.Easy, Level.Normal, Level.Hard, Level.Lunatic,
            Level.Easy, Level.Normal, Level.Hard, Level.Lunatic, Level.Easy,
            Level.Normal, Level.Hard, Level.Lunatic, Level.Easy, Level.Normal,

            Level.Hard, Level.Lunatic, Level.Easy, Level.Normal, Level.Hard,
            Level.Lunatic, Level.Easy, Level.Normal, Level.Hard, Level.Lunatic,
            Level.Easy, Level.Normal, Level.Hard, Level.Lunatic, Level.Easy,
            Level.Normal, Level.Hard, Level.Lunatic, Level.Easy, Level.Normal,
            Level.Hard, Level.Lunatic, Level.Easy, Level.Normal, Level.Hard,
            Level.Lunatic, Level.Easy, Level.Normal, Level.Hard, Level.Lunatic,
            Level.Easy, Level.Normal, Level.Hard, Level.Lunatic, Level.Easy,
            Level.Normal, Level.Hard, Level.Lunatic, Level.Easy, Level.Normal,
            Level.Hard, Level.Lunatic, Level.Easy, Level.Normal, Level.Hard,
            Level.Lunatic, Level.Easy, Level.Normal, Level.Hard, Level.Lunatic,

            Level.Easy, Level.Normal, Level.Hard, Level.Lunatic, Level.Easy,
            Level.Normal, Level.Hard, Level.Lunatic, Level.Easy, Level.Normal,
            Level.Hard, Level.Lunatic, Level.Easy, Level.Normal, Level.Hard,
            Level.Lunatic, Level.Extra, Level.Extra, Level.Extra, Level.Extra,
            Level.Extra, Level.Extra, Level.Extra, Level.Extra, Level.Extra,
            Level.Extra, Level.Extra, Level.Extra, Level.Phantasm, Level.Phantasm,
            Level.Phantasm, Level.Phantasm, Level.Phantasm, Level.Phantasm, Level.Phantasm,
            Level.Phantasm, Level.Phantasm, Level.Phantasm, Level.Phantasm, Level.Phantasm,
            Level.Phantasm
        };

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

        private class AllScoreData
        {
            public Header Header { get; set; }
            public Dictionary<CharaLevelPair, List<HighScore>> Rankings { get; set; }
            public Dictionary<Chara, ClearData> ClearData { get; set; }
            public CardAttack[] CardAttacks { get; set; }
            public Dictionary<CharaLevelPair, Dictionary<Stage, PracticeScore>> PracticeScores { get; set; }
            public PlayStatus PlayStatus { get; set; }
            public LastName LastName { get; set; }
            public VersionInfo VersionInfo { get; set; }

            public AllScoreData()
            {
                var numCharas = Enum.GetValues(typeof(Chara)).Length;
                this.Rankings = new Dictionary<CharaLevelPair, List<HighScore>>();
                this.ClearData = new Dictionary<Chara, ClearData>(numCharas);
                this.CardAttacks = new CardAttack[NumCards];
                this.PracticeScores = new Dictionary<CharaLevelPair, Dictionary<Stage, PracticeScore>>();
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
                if (this.Signature != "TH7K")
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

            public HighScore(Chapter ch)
                : base(ch)
            {
                if (this.Signature != "HSCR")
                    throw new InvalidDataException("Signature");
                if (this.Size1 != 0x0028)
                    throw new InvalidDataException("Size1");
                // if (this.Size2 != 0x0028)
                //     throw new InvalidDataException("Size2");
            }

            public HighScore(uint score)    // for InitialRanking only
                : base()
            {
                this.Score = score;
                this.Name = Encoding.Default.GetBytes("--------\0");
                this.Date = Encoding.Default.GetBytes("--/--\0");
            }

            public override void ReadFrom(BinaryReader reader)
            {
                reader.ReadUInt32();    // always 0x00000001?
                this.Score = reader.ReadUInt32();
                this.SlowRate = reader.ReadSingle();
                this.Chara = (Chara)reader.ReadByte();
                this.Level = (Level)reader.ReadByte();
                this.StageProgress = (StageProgress)reader.ReadByte();
                this.Name = reader.ReadBytes(9);
                this.Date = reader.ReadBytes(6);
                this.ContinueCount = reader.ReadUInt16();
            }
        }

        private class ClearData : Chapter   // per character
        {
            [System.Diagnostics.CodeAnalysis.SuppressMessage(
                "Microsoft.Performance",
                "CA1811:AvoidUncalledPrivateCode",
                Justification = "For future use.")]
            public byte[] StoryFlags { get; private set; }      // [level]; really...?

            [System.Diagnostics.CodeAnalysis.SuppressMessage(
                "Microsoft.Performance",
                "CA1811:AvoidUncalledPrivateCode",
                Justification = "For future use.")]
            public byte[] PracticeFlags { get; private set; }   // [level]; really...?

            public Chara Chara { get; private set; }            // size: 4Bytes

            public ClearData(Chapter ch)
                : base(ch)
            {
                if (this.Signature != "CLRD")
                    throw new InvalidDataException("Signature");
                if (this.Size1 != 0x001C)
                    throw new InvalidDataException("Size1");
                // if (this.Size2 != 0x001C)
                //     throw new InvalidDataException("Size2");
            }

            public override void ReadFrom(BinaryReader reader)
            {
                var numLevels = Enum.GetValues(typeof(Level)).Length;
                reader.ReadUInt32();    // always 0x00000001?
                this.StoryFlags = reader.ReadBytes(numLevels);
                this.PracticeFlags = reader.ReadBytes(numLevels);
                this.Chara = (Chara)reader.ReadInt32();
            }
        }

        private class CardAttack : Chapter      // per card
        {
            public Dictionary<CharaWithTotal, uint> MaxBonuses { get; private set; }
            public short Number { get; private set; }       // 0-based
            public byte[] CardName { get; private set; }    // .Length = 0x30
            public Dictionary<CharaWithTotal, ushort> TrialCounts { get; private set; }
            public Dictionary<CharaWithTotal, ushort> ClearCounts { get; private set; }

            public CardAttack(Chapter ch)
                : base(ch)
            {
                if (this.Signature != "CATK")
                    throw new InvalidDataException("Signature");
                if (this.Size1 != 0x0078)
                    throw new InvalidDataException("Size1");
                // if (this.Size2 != 0x0078)
                //     throw new InvalidDataException("Size2");

                var numCharas = Enum.GetValues(typeof(CharaWithTotal)).Length;
                this.MaxBonuses = new Dictionary<CharaWithTotal, uint>(numCharas);
                this.TrialCounts = new Dictionary<CharaWithTotal, ushort>(numCharas);
                this.ClearCounts = new Dictionary<CharaWithTotal, ushort>(numCharas);
            }

            public override void ReadFrom(BinaryReader reader)
            {
                var charas = Utils.GetEnumerator<CharaWithTotal>();
                reader.ReadUInt32();    // always 0x00000001?
                foreach (var chara in charas)
                    this.MaxBonuses.Add(chara, reader.ReadUInt32());
                this.Number = reader.ReadInt16();
                reader.ReadByte();
                this.CardName = reader.ReadBytes(0x30);
                reader.ReadByte();      // always 0x00?
                foreach (var chara in charas)
                    this.TrialCounts.Add(chara, reader.ReadUInt16());
                foreach (var chara in charas)
                    this.ClearCounts.Add(chara, reader.ReadUInt16());
            }

            public bool HasTried()
            {
                return this.TrialCounts[CharaWithTotal.Total] > 0;
            }
        }

        private class PracticeScore : Chapter   // per character, level, stage
        {
            public int TrialCount { get; private set; }     // really...?
            public int HighScore { get; private set; }
            public Chara Chara { get; private set; }        // size: 1Byte
            public Level Level { get; private set; }        // size: 1Byte
            public Stage Stage { get; private set; }        // size: 1Byte

            public PracticeScore(Chapter ch)
                : base(ch)
            {
                if (this.Signature != "PSCR")
                    throw new InvalidDataException("Signature");
                if (this.Size1 != 0x0018)
                    throw new InvalidDataException("Size1");
                // if (this.Size2 != 0x0018)
                //     throw new InvalidDataException("Size2");
            }

            public override void ReadFrom(BinaryReader reader)
            {
                reader.ReadUInt32();    // always 0x00000001?
                this.TrialCount = reader.ReadInt32();
                this.HighScore = reader.ReadInt32();
                this.Chara = (Chara)reader.ReadByte();
                this.Level = (Level)reader.ReadByte();
                this.Stage = (Stage)reader.ReadByte();
                reader.ReadByte();      // always 0x00?
            }
        }

        private class PlayStatus : Chapter
        {
            public Time TotalRunningTime { get; private set; }
            public Time TotalPlayTime { get; private set; }
            public Dictionary<LevelWithTotal, PlayCount> PlayCounts { get; private set; }

            public PlayStatus(Chapter ch)
                : base(ch)
            {
                if (this.Signature != "PLST")
                    throw new InvalidDataException("Signature");
                if (this.Size1 != 0x0160)
                    throw new InvalidDataException("Size1");
                // if (this.Size2 != 0x0160)
                //     throw new InvalidDataException("Size2");

                this.PlayCounts =
                    new Dictionary<LevelWithTotal, PlayCount>(Enum.GetValues(typeof(LevelWithTotal)).Length);
            }

            public override void ReadFrom(BinaryReader reader)
            {
                reader.ReadUInt32();    // always 0x00000001?
                this.TotalRunningTime = new Time(
                    reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), false);
                this.TotalPlayTime = new Time(
                    reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), false);
                foreach (var level in Utils.GetEnumerator<LevelWithTotal>())
                {
                    var playCount = new PlayCount();
                    playCount.ReadFrom(reader);
                    if (!this.PlayCounts.ContainsKey(level))
                        this.PlayCounts.Add(level, playCount);
                }
            }
        }

        private class PlayCount : IBinaryReadable     // per level-with-total
        {
            public int TotalTrial { get; private set; }
            public Dictionary<Chara, int> Trials { get; private set; }
            public int TotalRetry { get; private set; }
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
                this.TotalRetry = reader.ReadInt32();
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
                reader.ReadUInt16();    // always 0x0001?
                reader.ReadUInt16();
                this.Version = reader.ReadBytes(6);
                reader.ReadBytes(3);
                reader.ReadBytes(3);
                reader.ReadUInt32();
            }
        }

        private AllScoreData allScoreData = null;

        public override string SupportedVersions
        {
            get { return "1.00b"; }
        }

        private static readonly List<HighScore> InitialRanking;

        private static readonly string LevelPattern;
        private static readonly string LevelWithTotalPattern;
        private static readonly string CharaPattern;
        private static readonly string CharaWithTotalPattern;
        private static readonly string StagePattern;
        private static readonly string StageWithTotalPattern;

        private static readonly Func<string, Level> ToLevel;
        private static readonly Func<string, LevelWithTotal> ToLevelWithTotal;
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
        static Th07Converter()
        {
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
            var charas = Utils.GetEnumerator<Chara>();
            var charasWithTotal = Utils.GetEnumerator<CharaWithTotal>();
            var stages = Utils.GetEnumerator<Stage>();
            var stagesWithTotal = Utils.GetEnumerator<StageWithTotal>();

            LevelPattern = string.Join(
                string.Empty, levels.Select(lv => lv.ToShortName()).ToArray());
            LevelWithTotalPattern = string.Join(
                string.Empty, levelsWithTotal.Select(lv => lv.ToShortName()).ToArray());
            CharaPattern = string.Join(
                "|", charas.Select(ch => ch.ToShortName()).ToArray());
            CharaWithTotalPattern = string.Join(
                "|", charasWithTotal.Select(ch => ch.ToShortName()).ToArray());
            StagePattern = string.Join(
                string.Empty, stages.Select(st => st.ToShortName()).ToArray());
            StageWithTotalPattern = string.Join(
                string.Empty, stagesWithTotal.Select(st => st.ToShortName()).ToArray());

            var comparisonType = StringComparison.OrdinalIgnoreCase;

            ToLevel = (shortName =>
                levels.First(lv => lv.ToShortName().Equals(shortName, comparisonType)));
            ToLevelWithTotal = (shortName =>
                levelsWithTotal.First(lv => lv.ToShortName().Equals(shortName, comparisonType)));
            ToChara = (shortName =>
                charas.First(ch => ch.ToShortName().Equals(shortName, comparisonType)));
            ToCharaWithTotal = (shortName =>
                charasWithTotal.First(ch => ch.ToShortName().Equals(shortName, comparisonType)));
            ToStage = (shortName =>
                stages.First(st => st.ToShortName().Equals(shortName, comparisonType)));
            ToStageWithTotal = (shortName =>
                stagesWithTotal.First(st => st.ToShortName().Equals(shortName, comparisonType)));
        }

        public Th07Converter()
        {
        }

        protected override bool ReadScoreFile(Stream input)
        {
            using (var decrypted = new MemoryStream())
#if DEBUG
            using (var decoded = new FileStream("th07decoded.dat", FileMode.Create, FileAccess.ReadWrite))
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
            var data = new byte[size];
            input.Read(data, 0, size);

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

            output.Write(data, 0, size);

            return (ushort)checksum == BitConverter.ToUInt16(data, 2);
        }

        private static bool Extract(Stream input, Stream output)
        {
            var reader = new BinaryReader(input);

            reader.ReadUInt16();    // Unknown1
            reader.ReadUInt16();    // Checksum; already checked by this.Decrypt()
            var version = reader.ReadInt16();
            if (version != 0x0B)
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
                        case "TH7K":
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
                            // th07.exe does something more things here...
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
                        case "TH7K":
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
                                if ((score.Level != Level.Extra) && (score.Level != Level.Phantasm) &&
                                    (score.Stage != Stage.Extra) && (score.Stage != Stage.Phantasm))
                                {
                                    var key = new CharaLevelPair(score.Chara, score.Level);
                                    if (!allScoreData.PracticeScores.ContainsKey(key))
                                        allScoreData.PracticeScores.Add(
                                            key, new Dictionary<Stage, PracticeScore>());
                                    var scores = allScoreData.PracticeScores[key];
                                    if (!scores.ContainsKey(score.Stage))
                                        scores.Add(score.Stage, score);
                                }
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
                (allScoreData.ClearData.Count == Enum.GetValues(typeof(Chara)).Length) &&
                // (allScoreData.cardAttacks.Length == NumCards) &&
                // (allScoreData.practiceScores.Count >= 0) &&
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

        // %T07SCR[w][xx][y][z]
        private string ReplaceScore(string input)
        {
            var pattern = Utils.Format(@"%T07SCR([{0}])({1})(\d)([1-5])", LevelPattern, CharaPattern);
            var evaluator = new MatchEvaluator(match =>
            {
                var level = ToLevel(match.Groups[1].Value);
                var chara = ToChara(match.Groups[2].Value);
                var rank = Utils.ToZeroBased(int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture));
                var type = int.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture);

                var key = new CharaLevelPair(chara, level);
                var score = this.allScoreData.Rankings.ContainsKey(key)
                    ? this.allScoreData.Rankings[key][rank] : InitialRanking[rank];

                switch (type)
                {
                    case 1:     // name
                        return Encoding.Default.GetString(score.Name).Split('\0')[0];
                    case 2:     // score
                        return this.ToNumberString((score.Score * 10) + score.ContinueCount);
                    case 3:     // stage
                        return score.StageProgress.ToShortName();
                    case 4:     // date
                        return Encoding.Default.GetString(score.Date).TrimEnd('\0');
                    case 5:     // slow rate
                        return Utils.Format("{0:F3}%", score.SlowRate);
                    default:    // unreachable
                        return match.ToString();
                }
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T07C[xxx][yy][z]
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "StyleCop.CSharp.MaintainabilityRules",
            "SA1119:StatementMustNotUseUnnecessaryParenthesis",
            Justification = "Reviewed.")]
        private string ReplaceCareer(string input)
        {
            var pattern = Utils.Format(@"%T07C(\d{{3}})({0})([1-3])", CharaWithTotalPattern);
            var evaluator = new MatchEvaluator(match =>
            {
                var number = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
                var chara = ToCharaWithTotal(match.Groups[2].Value);
                var type = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

                Func<CardAttack, long> getValue = (attack => 0L);
                if (type == 1)
                    getValue = (attack => attack.MaxBonuses[chara]);
                else if (type == 2)
                    getValue = (attack => attack.ClearCounts[chara]);
                else
                    getValue = (attack => attack.TrialCounts[chara]);

                Func<CardAttack, long> getValueWithNullCheck =
                    (attack => (attack != null) ? getValue(attack) : 0L);

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

        // %T07CARD[xxx][y]
        private string ReplaceCard(string input)
        {
            var pattern = @"%T07CARD(\d{3})([NR])";
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
                        return ((attack != null) && attack.HasTried())
                            ? CardLevelTable[attack.Number].ToString() : "?????";
                }
                else
                    return match.ToString();
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T07CRG[w][xx][yy][z]
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "StyleCop.CSharp.MaintainabilityRules",
            "SA1119:StatementMustNotUseUnnecessaryParenthesis",
            Justification = "Reviewed.")]
        private string ReplaceCollectRate(string input)
        {
            var pattern = Utils.Format(
                @"%T07CRG([{0}])({1})([{2}])([12])",
                LevelWithTotalPattern,
                CharaWithTotalPattern,
                StageWithTotalPattern);
            var evaluator = new MatchEvaluator(match =>
            {
                var level = ToLevelWithTotal(match.Groups[1].Value);
                var chara = ToCharaWithTotal(match.Groups[2].Value);
                var stage = ToStageWithTotal(match.Groups[3].Value);
                var type = int.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture);

                if ((stage == StageWithTotal.Extra) || (stage == StageWithTotal.Phantasm))
                    return match.ToString();

                Func<CardAttack, bool> checkNotNull = (attack => attack != null);
                Func<CardAttack, bool> findByLevel = (attack => true);
                Func<CardAttack, bool> findByStage = (attack => true);
                Func<CardAttack, bool> findByType = (attack => true);

                if (stage == StageWithTotal.Total)
                {
                    // Do nothing
                }
                else
                    findByStage = (attack => StageCardTable[(Stage)stage].Contains(attack.Number));

                switch (level)
                {
                    case LevelWithTotal.Total:
                        // Do nothing
                        break;
                    case LevelWithTotal.Extra:
                        findByStage = (attack => StageCardTable[Stage.Extra].Contains(attack.Number));
                        break;
                    case LevelWithTotal.Phantasm:
                        findByStage = (attack => StageCardTable[Stage.Phantasm].Contains(attack.Number));
                        break;
                    default:
                        findByLevel = (attack => CardLevelTable[attack.Number] == (Level)level);
                        break;
                }

                if (type == 1)
                    findByType = (attack => attack.ClearCounts[chara] > 0);
                else
                    findByType = (attack => attack.TrialCounts[chara] > 0);

                var and = Utils.MakeAndPredicate(checkNotNull, findByLevel, findByStage, findByType);
                return this.allScoreData.CardAttacks.Count(and).ToString(CultureInfo.CurrentCulture);
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T07CLEAR[x][yy]
        private string ReplaceClear(string input)
        {
            var pattern = Utils.Format(@"%T07CLEAR([{0}])({1})", LevelPattern, CharaPattern);
            var evaluator = new MatchEvaluator(match =>
            {
                var level = ToLevel(match.Groups[1].Value);
                var chara = ToChara(match.Groups[2].Value);

                var key = new CharaLevelPair(chara, level);
                if (this.allScoreData.Rankings.ContainsKey(key))
                {
                    var stageProgress = this.allScoreData.Rankings[key].Max(rank => rank.StageProgress);
                    if ((stageProgress == StageProgress.Extra) || (stageProgress == StageProgress.Phantasm))
                        return "Not Clear";
                    else
                        return stageProgress.ToShortName();
                }
                else
                    return StageProgress.None.ToShortName();
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T07PLAY[x][yy]
        private string ReplacePlay(string input)
        {
            var pattern = Utils.Format(
                @"%T07PLAY([{0}])({1}|CL|CN|PR|RT)", LevelWithTotalPattern, CharaWithTotalPattern);
            var evaluator = new MatchEvaluator(match =>
            {
                var level = ToLevelWithTotal(match.Groups[1].Value);
                var charaAndMore = match.Groups[2].Value.ToUpperInvariant();

                var playCount = this.allScoreData.PlayStatus.PlayCounts[(LevelWithTotal)level];
                switch (charaAndMore)
                {
                    case "CL":  // clear count
                        return this.ToNumberString(playCount.TotalClear);
                    case "CN":  // continue count
                        return this.ToNumberString(playCount.TotalContinue);
                    case "PR":  // practice count
                        return this.ToNumberString(playCount.TotalPractice);
                    case "RT":  // retry count
                        return this.ToNumberString(playCount.TotalRetry);
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

        // %T07TIME(ALL|PLY)
        private string ReplaceTime(string input)
        {
            var pattern = @"%T07TIME(ALL|PLY)";
            var evaluator = new MatchEvaluator(match =>
            {
                var kind = match.Groups[1].Value.ToUpperInvariant();

                return (kind == "ALL")
                    ? this.allScoreData.PlayStatus.TotalRunningTime.ToLongString()
                    : this.allScoreData.PlayStatus.TotalPlayTime.ToLongString();
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T07PRAC[w][xx][y][z]
        private string ReplacePractice(string input)
        {
            var pattern = Utils.Format(
                @"%T07PRAC([{0}])({1})([{2}])([12])", LevelPattern, CharaPattern, StagePattern);
            var evaluator = new MatchEvaluator(match =>
            {
                var level = ToLevel(match.Groups[1].Value);
                var chara = ToChara(match.Groups[2].Value);
                var stage = ToStage(match.Groups[3].Value);
                var type = int.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture);

                if ((level == Level.Extra) || (level == Level.Phantasm))
                    return match.ToString();
                if ((stage == Stage.Extra) || (stage == Stage.Phantasm))
                    return match.ToString();

                var key = new CharaLevelPair(chara, level);
                if (this.allScoreData.PracticeScores.ContainsKey(key))
                {
                    var scores = this.allScoreData.PracticeScores[key];
                    if (type == 1)
                        return scores.ContainsKey(stage)
                            ? this.ToNumberString(scores[stage].HighScore * 10) : "0";
                    else
                        return scores.ContainsKey(stage)
                            ? this.ToNumberString(scores[stage].TrialCount) : "0";
                }
                else
                    return "0";
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }
    }
}

//-----------------------------------------------------------------------
// <copyright file="Th14Converter.cs" company="None">
//     (c) 2014 IIHOSHI Yoshinori
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
    internal class Th14Converter : ThConverter
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
            [EnumAltName("-")] NotUsed
        }
        public enum LevelPracticeWithTotal
        {
            [EnumAltName("E")] Easy,
            [EnumAltName("N")] Normal,
            [EnumAltName("H")] Hard,
            [EnumAltName("L")] Lunatic,
            [EnumAltName("X")] Extra,
            [EnumAltName("-")] NotUsed,
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
            [EnumAltName("X")] Extra
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
            [EnumAltName("0")] Total
        }
        public enum StagePractice
        {
            [EnumAltName("1")] Stage1,
            [EnumAltName("2")] Stage2,
            [EnumAltName("3")] Stage3,
            [EnumAltName("4")] Stage4,
            [EnumAltName("5")] Stage5,
            [EnumAltName("6")] Stage6,
            [EnumAltName("X")] Extra,
            [EnumAltName("-")] NotUsed
        }

        public enum StageProgress
        {
            [EnumAltName("-------")]     None,
            [EnumAltName("Stage 1")]     Stage1,
            [EnumAltName("Stage 2")]     Stage2,
            [EnumAltName("Stage 3")]     Stage3,
            [EnumAltName("Stage 4")]     Stage4,
            [EnumAltName("Stage 5")]     Stage5,
            [EnumAltName("Stage 6")]     Stage6,
            [EnumAltName("Extra Stage")] Extra,
            [EnumAltName("All Clear")]   Clear,
            [EnumAltName("Extra Clear")] ExtraClear
        }

        private const int NumCards = 120;

        // Thanks to thwiki.info
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "StyleCop.CSharp.SpacingRules",
            "SA1008:OpeningParenthesisMustBeSpacedCorrectly",
            Justification = "Reviewed.")]
        private static readonly Dictionary<Stage, Range<int>> StageCardTable =
            new Dictionary<Stage, Range<int>>()
            {
                { Stage.Stage1,     new Range<int>(  0,   9) },
                { Stage.Stage2,     new Range<int>( 10,  25) },
                { Stage.Stage3,     new Range<int>( 26,  39) },
#if false
                { Stage.Stage4A,    new Range<int>( 40,  51) },
                { Stage.Stage4B,    new Range<int>( 52,  63) },
#else
                { Stage.Stage4,     new Range<int>( 40,  63) },
#endif
                { Stage.Stage5,     new Range<int>( 64,  83) },
                { Stage.Stage6,     new Range<int>( 84, 107) },
                { Stage.Extra,      new Range<int>(108, 119) }
            };

        private class LevelStagePair : Pair<LevelPractice, StagePractice>
        {
            [System.Diagnostics.CodeAnalysis.SuppressMessage(
                "Microsoft.Performance",
                "CA1811:AvoidUncalledPrivateCode",
                Justification = "For future use.")]
            public LevelPractice Level { get { return this.First; } }

            [System.Diagnostics.CodeAnalysis.SuppressMessage(
                "Microsoft.Performance",
                "CA1811:AvoidUncalledPrivateCode",
                Justification = "For future use.")]
            public StagePractice Stage { get { return this.Second; } }

            public LevelStagePair(LevelPractice level, StagePractice stage) : base(level, stage) { }
            public LevelStagePair(Level level, Stage stage)
                : base((LevelPractice)level, (StagePractice)stage) { }
        }

        private class AllScoreData
        {
            public Header Header { get; set; }
            public Dictionary<CharaWithTotal, ClearData> ClearData { get; set; }
            public Status Status { get; set; }
        }

        private class Header : IBinaryReadable
        {
            private uint unknown1;
            private uint unknown2;

            public string Signature { get; private set; }
            public int EncodedAllSize { get; private set; }
            public int EncodedBodySize { get; private set; }
            public int DecodedBodySize { get; private set; }

            public void ReadFrom(BinaryReader reader)
            {
                this.Signature = new string(reader.ReadChars(4));
                this.EncodedAllSize = reader.ReadInt32();
                this.unknown1 = reader.ReadUInt32();
                this.unknown2 = reader.ReadUInt32();
                this.EncodedBodySize = reader.ReadInt32();
                this.DecodedBodySize = reader.ReadInt32();
            }

            public void WriteTo(BinaryWriter writer)
            {
                writer.Write(this.Signature.ToCharArray());
                writer.Write(this.EncodedAllSize);
                writer.Write(this.unknown1);
                writer.Write(this.unknown2);
                writer.Write(this.EncodedBodySize);
                writer.Write(this.DecodedBodySize);
            }
        }

        private class Chapter : IBinaryReadable
        {
            public string Signature { get; private set; }
            public ushort Version { get; private set; }
            public uint Checksum { get; private set; }
            public int Size { get; private set; }

            public Chapter() { }
            public Chapter(Chapter ch)
            {
                this.Signature = ch.Signature;
                this.Version = ch.Version;
                this.Checksum = ch.Checksum;
                this.Size = ch.Size;
            }

            public virtual void ReadFrom(BinaryReader reader)
            {
                this.Signature = Encoding.Default.GetString(reader.ReadBytes(2));
                this.Version = reader.ReadUInt16();
                this.Checksum = reader.ReadUInt32();
                this.Size = reader.ReadInt32();
            }
        }

        private class ClearData : Chapter   // per character
        {
            public CharaWithTotal Chara { get; private set; }   // size: 4Bytes
            public Dictionary<LevelPracticeWithTotal, ScoreData[]> Rankings { get; private set; }
            public int TotalPlayCount { get; private set; }
            public int PlayTime { get; private set; }           // = seconds * 60fps
            public Dictionary<LevelPracticeWithTotal, int> ClearCounts { get; private set; }
            public Dictionary<LevelPracticeWithTotal, int> ClearFlags { get; private set; }     // Really...?
            public Dictionary<LevelStagePair, Practice> Practices { get; private set; }
            public SpellCard[] Cards { get; private set; }      // [0..119]

            public ClearData(Chapter ch)
                : base(ch)
            {
                if (this.Signature != "CR")
                    throw new InvalidDataException("Signature");
                if (this.Version != 0x0001)
                    throw new InvalidDataException("Version");
                if (this.Size != 0x00005298)
                    throw new InvalidDataException("Size");

                var numLevels = Enum.GetValues(typeof(LevelPracticeWithTotal)).Length;
                this.Rankings = new Dictionary<LevelPracticeWithTotal, ScoreData[]>(numLevels);
                this.ClearCounts = new Dictionary<LevelPracticeWithTotal, int>(numLevels);
                this.ClearFlags = new Dictionary<LevelPracticeWithTotal, int>(numLevels);
                this.Practices = new Dictionary<LevelStagePair, Practice>();
                this.Cards = new SpellCard[NumCards];
            }

            public override void ReadFrom(BinaryReader reader)
            {
                var levelsPracticeWithTotal = Utils.GetEnumerator<LevelPracticeWithTotal>();
                var stages = Utils.GetEnumerator<StagePractice>();
                this.Chara = (CharaWithTotal)reader.ReadInt32();
                foreach (var level in levelsPracticeWithTotal)
                {
                    if (!this.Rankings.ContainsKey(level))
                        this.Rankings.Add(level, new ScoreData[10]);
                    for (var rank = 0; rank < 10; rank++)
                    {
                        var score = new ScoreData();
                        score.ReadFrom(reader);
                        this.Rankings[level][rank] = score;
                    }
                }
                this.TotalPlayCount = reader.ReadInt32();
                this.PlayTime = reader.ReadInt32();
                foreach (var level in levelsPracticeWithTotal)
                {
                    var clearCount = reader.ReadInt32();
                    if (!this.ClearCounts.ContainsKey(level))
                        this.ClearCounts.Add(level, clearCount);
                }
                foreach (var level in levelsPracticeWithTotal)
                {
                    var clearFlag = reader.ReadInt32();
                    if (!this.ClearFlags.ContainsKey(level))
                        this.ClearFlags.Add(level, clearFlag);
                }
                foreach (var level in Utils.GetEnumerator<LevelPractice>())
                    foreach (var stage in stages)
                    {
                        var practice = new Practice();
                        practice.ReadFrom(reader);
                        var key = new LevelStagePair(level, stage);
                        if (!this.Practices.ContainsKey(key))
                            this.Practices.Add(key, practice);
                    }
                for (var number = 0; number < Th14Converter.NumCards; number++)
                {
                    var card = new SpellCard();
                    card.ReadFrom(reader);
                    this.Cards[number] = card;
                }
            }
        }

        private class Status : Chapter
        {
            [System.Diagnostics.CodeAnalysis.SuppressMessage(
                "Microsoft.Performance",
                "CA1811:AvoidUncalledPrivateCode",
                Justification = "For future use.")]
            public byte[] LastName { get; private set; }    // .Length = 10 (The last 2 bytes are always 0x00 ?)

            [System.Diagnostics.CodeAnalysis.SuppressMessage(
                "Microsoft.Performance",
                "CA1811:AvoidUncalledPrivateCode",
                Justification = "For future use.")]
            public byte[] BgmFlags { get; private set; }    // .Length = 17

            [System.Diagnostics.CodeAnalysis.SuppressMessage(
                "Microsoft.Performance",
                "CA1811:AvoidUncalledPrivateCode",
                Justification = "For future use.")]
            public int TotalPlayTime { get; private set; }  // unit: 10ms

            public Status(Chapter ch)
                : base(ch)
            {
                if (this.Signature != "ST")
                    throw new InvalidDataException("Signature");
                if (this.Version != 0x0001)
                    throw new InvalidDataException("Version");
                if (this.Size != 0x0000042C)
                    throw new InvalidDataException("Size");
            }

            public override void ReadFrom(BinaryReader reader)
            {
                this.LastName = reader.ReadBytes(10);
                reader.ReadBytes(0x10);
                this.BgmFlags = reader.ReadBytes(17);
                reader.ReadBytes(0x11);
                this.TotalPlayTime = reader.ReadInt32();
                reader.ReadBytes(0x03E0);
            }
        }

        private class ScoreData : IBinaryReadable
        {
            public uint Score { get; private set; }         // * 10
            public StageProgress StageProgress { get; private set; }    // size: 1Byte
            public byte ContinueCount { get; private set; }
            public byte[] Name { get; private set; }        // .Length = 10 (The last 2 bytes are always 0x00 ?)
            public uint DateTime { get; private set; }      // UNIX time (unit: [s])
            public float SlowRate { get; private set; }     // really...?

            public void ReadFrom(BinaryReader reader)
            {
                this.Score = reader.ReadUInt32();
                this.StageProgress = (StageProgress)reader.ReadByte();
                this.ContinueCount = reader.ReadByte();
                this.Name = reader.ReadBytes(10);
                this.DateTime = reader.ReadUInt32();
                this.SlowRate = reader.ReadSingle();
                reader.ReadUInt32();
            }
        }

        private class SpellCard : IBinaryReadable
        {
            public byte[] Name { get; private set; }            // .Length = 0x80
            public int ClearCount { get; private set; }
            public int PracticeClearCount { get; private set; }
            public int TrialCount { get; private set; }
            public int PracticeTrialCount { get; private set; }
            public int Number { get; private set; }             // 0-based
            public Level Level { get; private set; }

            [System.Diagnostics.CodeAnalysis.SuppressMessage(
                "Microsoft.Performance",
                "CA1811:AvoidUncalledPrivateCode",
                Justification = "For future use.")]
            public int PracticeScore { get; private set; }

            public void ReadFrom(BinaryReader reader)
            {
                this.Name = reader.ReadBytes(0x80);
                this.ClearCount = reader.ReadInt32();
                this.PracticeClearCount = reader.ReadInt32();
                this.TrialCount = reader.ReadInt32();
                this.PracticeTrialCount = reader.ReadInt32();
                this.Number = reader.ReadInt32();
                this.Level = (Level)reader.ReadInt32();
                this.PracticeScore = reader.ReadInt32();
            }
        }

        private class Practice : IBinaryReadable
        {
            public uint Score { get; private set; }         // * 10

            [System.Diagnostics.CodeAnalysis.SuppressMessage(
                "Microsoft.Performance",
                "CA1811:AvoidUncalledPrivateCode",
                Justification = "For future use.")]
            public byte ClearFlag { get; private set; }     // 0x00: Not clear, 0x01: Cleared

            [System.Diagnostics.CodeAnalysis.SuppressMessage(
                "Microsoft.Performance",
                "CA1811:AvoidUncalledPrivateCode",
                Justification = "For future use.")]
            public byte EnableFlag { get; private set; }    // 0x00: Disable, 0x01: Enable

            public void ReadFrom(BinaryReader reader)
            {
                this.Score = reader.ReadUInt32();
                this.ClearFlag = reader.ReadByte();
                this.EnableFlag = reader.ReadByte();
                reader.ReadUInt16();    // always 0x0000?
            }
        }

        private AllScoreData allScoreData = null;

        public override string SupportedVersions
        {
            get { return "1.00b"; }
        }

        private static readonly string LevelPattern;
        private static readonly string LevelWithTotalPattern;
        private static readonly string CharaPattern;
        private static readonly string CharaWithTotalPattern;
        private static readonly string StagePattern;
        private static readonly string StageWithTotalPattern;

        private static readonly Func<string, StringComparison, Level> ToLevel;
        private static readonly Func<string, StringComparison, LevelWithTotal> ToLevelWithTotal;
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
        static Th14Converter()
        {
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

            ToLevel = ((shortName, comparisonType) =>
                levels.First(lv => lv.ToShortName().Equals(shortName, comparisonType)));
            ToLevelWithTotal = ((shortName, comparisonType) =>
                levelsWithTotal.First(lv => lv.ToShortName().Equals(shortName, comparisonType)));
            ToChara = ((shortName, comparisonType) =>
                charas.First(ch => ch.ToShortName().Equals(shortName, comparisonType)));
            ToCharaWithTotal = ((shortName, comparisonType) =>
                charasWithTotal.First(ch => ch.ToShortName().Equals(shortName, comparisonType)));
            ToStage = ((shortName, comparisonType) =>
                stages.First(st => st.ToShortName().Equals(shortName, comparisonType)));
            ToStageWithTotal = ((shortName, comparisonType) =>
                stagesWithTotal.First(st => st.ToShortName().Equals(shortName, comparisonType)));
        }

        public Th14Converter()
        {
        }

        protected override bool ReadScoreFile(Stream input)
        {
            using (var decrypted = new MemoryStream())
#if DEBUG
            using (var decoded = new FileStream("th14decoded.dat", FileMode.Create, FileAccess.ReadWrite))
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
            var reader = new BinaryReader(input);
            var writer = new BinaryWriter(output);

            var header = new Header();
            header.ReadFrom(reader);
            if (header.Signature != "TH41")
                return false;
            if (header.EncodedAllSize != reader.BaseStream.Length)
                return false;

            header.WriteTo(writer);
            ThCrypt.Decrypt(input, output, header.EncodedBodySize, 0xAC, 0x35, 0x10, header.EncodedBodySize);

            return true;
        }

        private static bool Extract(Stream input, Stream output)
        {
            var reader = new BinaryReader(input);
            var writer = new BinaryWriter(output);

            var header = new Header();
            header.ReadFrom(reader);
            header.WriteTo(writer);

            var bodyBeginPos = output.Position;
            Lzss.Extract(input, output);
            output.Flush();
            output.SetLength(output.Position);

            return header.DecodedBodySize == (output.Position - bodyBeginPos);
        }

        private static bool Validate(Stream input)
        {
            var reader = new BinaryReader(input);

            var header = new Header();
            header.ReadFrom(reader);
            var remainSize = header.DecodedBodySize;
            var chapter = new Chapter();

            try
            {
                while (true)
                {
                    chapter.ReadFrom(reader);

                    if (!((chapter.Signature == "CR") && (chapter.Version == 0x0001)) &&
                        !((chapter.Signature == "ST") && (chapter.Version == 0x0001)))
                        return false;

                    // -4 means the size of Size.
                    reader.BaseStream.Seek(-4, SeekOrigin.Current);
                    // 8 means the total size of Signature, Version, and Checksum.
                    var body = reader.ReadBytes(chapter.Size - 8);
                    var sum = body.Sum(elem => (int)elem);
                    if (sum != chapter.Checksum)
                        return false;

                    remainSize -= chapter.Size;
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
            var numCharas = Enum.GetValues(typeof(CharaWithTotal)).Length;

            allScoreData.ClearData = new Dictionary<CharaWithTotal, ClearData>(numCharas);
            allScoreData.Header = new Header();
            allScoreData.Header.ReadFrom(reader);

            try
            {
                while (true)
                {
                    chapter.ReadFrom(reader);
                    switch (chapter.Signature)
                    {
                        case "CR":
                            {
                                var clearData = new ClearData(chapter);
                                clearData.ReadFrom(reader);
                                if (!allScoreData.ClearData.ContainsKey(clearData.Chara))
                                    allScoreData.ClearData.Add(clearData.Chara, clearData);
                            }
                            break;
                        case "ST":
                            {
                                var status = new Status(chapter);
                                status.ReadFrom(reader);
                                allScoreData.Status = status;
                            }
                            break;
                        default:
                            // 12 means the total size of Signature, Version, Checksum, and Size.
                            reader.ReadBytes(chapter.Size - 12);
                            break;
                    }
                }
            }
            catch (EndOfStreamException)
            {
                // It's OK, do nothing.
            }

            if ((allScoreData.Header != null) &&
                (allScoreData.ClearData.Count == numCharas) &&
                (allScoreData.Status != null))
                return allScoreData;
            else
                return null;
        }

        protected override void Convert(Stream input, Stream output)
        {
            var reader = new StreamReader(input, Encoding.GetEncoding("shift_jis"));
            var writer = new StreamWriter(output, Encoding.GetEncoding("shift_jis"));

            var allLines = reader.ReadToEnd();
            allLines = this.ReplaceScore(allLines);
            allLines = this.ReplaceCareer(allLines);
            allLines = this.ReplaceCard(allLines);
            allLines = this.ReplaceCollectRate(allLines);
            allLines = this.ReplaceClear(allLines);
            allLines = this.ReplaceChara(allLines);
            allLines = this.ReplaceCharaEx(allLines);
            allLines = this.ReplacePractice(allLines);
            writer.Write(allLines);

            writer.Flush();
            writer.BaseStream.SetLength(writer.BaseStream.Position);
        }

        // %T14SCR[w][xx][y][z]
        private string ReplaceScore(string input)
        {
            var pattern = Utils.Format(@"%T14SCR([{0}])({1})(\d)([1-5])", LevelPattern, CharaPattern);
            var evaluator = new MatchEvaluator(match =>
            {
                var level = (LevelPracticeWithTotal)ToLevel(
                    match.Groups[1].Value, StringComparison.OrdinalIgnoreCase);
                var chara = (CharaWithTotal)ToChara(
                    match.Groups[2].Value, StringComparison.OrdinalIgnoreCase);
                var rank = Utils.ToZeroBased(int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture));
                var type = int.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture);

                var ranking = this.allScoreData.ClearData[chara].Rankings[level][rank];
                switch (type)
                {
                    case 1:     // name
                        return Encoding.Default.GetString(ranking.Name).Split('\0')[0];
                    case 2:     // score
                        return this.ToNumberString((ranking.Score * 10) + ranking.ContinueCount);
                    case 3:     // stage
                        if (ranking.DateTime > 0)
                        {
                            if (ranking.StageProgress == StageProgress.Extra)
                                return "Not Clear";
                            else if (ranking.StageProgress == StageProgress.ExtraClear)
                                return StageProgress.Clear.ToShortName();
                            else
                                return ranking.StageProgress.ToShortName();
                        }
                        else
                            return StageProgress.None.ToShortName();
                    case 4:     // date & time
                        if (ranking.DateTime > 0)
                            return new DateTime(1970, 1, 1).AddSeconds(ranking.DateTime)
                                .ToLocalTime().ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.CurrentCulture);
                        else
                            return "----/--/-- --:--:--";
                    case 5:     // slow
                        if (ranking.DateTime > 0)
                            return Utils.Format("{0:F3}%", ranking.SlowRate);
                        else
                            return "-----%";
                    default:    // unreachable
                        return match.ToString();
                }
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T14C[w][xxx][yy][z]
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "StyleCop.CSharp.MaintainabilityRules",
            "SA1119:StatementMustNotUseUnnecessaryParenthesis",
            Justification = "Reviewed.")]
        private string ReplaceCareer(string input)
        {
            var pattern = Utils.Format(@"%T14C([SP])(\d{{3}})({0})([12])", CharaWithTotalPattern);
            var evaluator = new MatchEvaluator(match =>
            {
                var kind = match.Groups[1].Value.ToUpperInvariant();
                var number = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);
                var chara = ToCharaWithTotal(match.Groups[3].Value, StringComparison.OrdinalIgnoreCase);
                var type = int.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture);

                Func<SpellCard, int> getCount = (card => 0);
                if (kind == "S")
                {
                    if (type == 1)
                        getCount = (card => card.ClearCount);
                    else
                        getCount = (card => card.TrialCount);
                }
                else
                {
                    if (type == 1)
                        getCount = (card => card.PracticeClearCount);
                    else
                        getCount = (card => card.PracticeTrialCount);
                }

                var cards = this.allScoreData.ClearData[chara].Cards;
                if (number == 0)
                    return this.ToNumberString(cards.Sum(getCount));
                else if (new Range<int>(1, NumCards).Contains(number))
                    return this.ToNumberString(getCount(cards[number - 1]));
                else
                    return match.ToString();
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T14CARD[xxx][y]
        private string ReplaceCard(string input)
        {
            var pattern = @"%T14CARD(\d{3})([NR])";
            var evaluator = new MatchEvaluator(match =>
            {
                var number = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
                var type = match.Groups[2].Value.ToUpperInvariant();

                if (new Range<int>(1, NumCards).Contains(number))
                {
                    var card = this.allScoreData.ClearData[CharaWithTotal.Total].Cards[number - 1];
                    if (type == "N")
                    {
                        var name = Encoding.Default.GetString(card.Name).TrimEnd('\0');
                        return (name.Length > 0) ? name : "??????????";
                    }
                    else
                        return card.Level.ToString();
                }
                else
                    return match.ToString();
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T14CRG[v][w][xx][y][z]
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "StyleCop.CSharp.MaintainabilityRules",
            "SA1119:StatementMustNotUseUnnecessaryParenthesis",
            Justification = "Reviewed.")]
        private string ReplaceCollectRate(string input)
        {
            var pattern = Utils.Format(
                @"%T14CRG([SP])([{0}])({1})([{2}])([12])",
                LevelWithTotalPattern,
                CharaWithTotalPattern,
                StageWithTotalPattern);
            var evaluator = new MatchEvaluator(match =>
            {
                var kind = match.Groups[1].Value.ToUpperInvariant();
                var level = ToLevelWithTotal(match.Groups[2].Value, StringComparison.OrdinalIgnoreCase);
                var chara = ToCharaWithTotal(match.Groups[3].Value, StringComparison.OrdinalIgnoreCase);
                var stage = ToStageWithTotal(match.Groups[4].Value, StringComparison.OrdinalIgnoreCase);
                var type = int.Parse(match.Groups[5].Value, CultureInfo.InvariantCulture);

                if (stage == StageWithTotal.Extra)
                    return match.ToString();

                Func<SpellCard, bool> checkNotNull = (card => card != null);
                Func<SpellCard, bool> findByKindType = (card => true);
                Func<SpellCard, bool> findByLevel = (card => true);
                Func<SpellCard, bool> findByStage = (card => true);

                if (kind == "S")
                {
                    if (type == 1)
                        findByKindType = (card => card.ClearCount > 0);
                    else
                        findByKindType = (card => card.TrialCount > 0);
                }
                else
                {
                    if (type == 1)
                        findByKindType = (card => card.PracticeClearCount > 0);
                    else
                        findByKindType = (card => card.PracticeTrialCount > 0);
                }

                if (stage == StageWithTotal.Total)
                {
                    // Do nothing
                }
                else
                    findByStage = (card => StageCardTable[(Stage)stage].Contains(card.Number));

                switch (level)
                {
                    case LevelWithTotal.Total:
                        // Do nothing
                        break;
                    case LevelWithTotal.Extra:
                        findByStage = (card => StageCardTable[Stage.Extra].Contains(card.Number));
                        break;
                    default:
                        findByLevel = (card => card.Level == (Level)level);
                        break;
                }

                var and = Utils.MakeAndPredicate(checkNotNull, findByKindType, findByLevel, findByStage);
                return this.allScoreData.ClearData[chara].Cards.Count(and)
                    .ToString(CultureInfo.CurrentCulture);
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T14CLEAR[x][yy]
        private string ReplaceClear(string input)
        {
            var pattern = Utils.Format(@"%T14CLEAR([{0}])({1})", LevelPattern, CharaPattern);
            var evaluator = new MatchEvaluator(match =>
            {
                var level = (LevelPracticeWithTotal)ToLevel(
                    match.Groups[1].Value, StringComparison.OrdinalIgnoreCase);
                var chara = (CharaWithTotal)ToChara(
                    match.Groups[2].Value, StringComparison.OrdinalIgnoreCase);

                var rankings = this.allScoreData.ClearData[chara].Rankings[level]
                    .Where(ranking => ranking.DateTime > 0);
                var stageProgress = (rankings.Count() > 0)
                    ? rankings.Max(ranking => ranking.StageProgress) : StageProgress.None;

                if (stageProgress == StageProgress.Extra)
                    return "Not Clear";
                else if (stageProgress == StageProgress.ExtraClear)
                    return StageProgress.Clear.ToShortName();
                else
                    return stageProgress.ToShortName();
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T14CHARA[xx][y]
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "StyleCop.CSharp.MaintainabilityRules",
            "SA1119:StatementMustNotUseUnnecessaryParenthesis",
            Justification = "Reviewed.")]
        private string ReplaceChara(string input)
        {
            var pattern = Utils.Format(@"%T14CHARA({0})([1-3])", CharaWithTotalPattern);
            var evaluator = new MatchEvaluator(match =>
            {
                var chara = ToCharaWithTotal(match.Groups[1].Value, StringComparison.OrdinalIgnoreCase);
                var type = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);

                Func<ClearData, long> getValueByType = (data => 0L);
                Func<long, string> toString = (value => string.Empty);
                if (type == 1)
                {
                    getValueByType = (data => data.TotalPlayCount);
                    toString = (value => this.ToNumberString(value));
                }
                else if (type == 2)
                {
                    getValueByType = (data => data.PlayTime);
                    toString = (value => new Time(value).ToString());
                }
                else
                {
                    getValueByType = (data => data.ClearCounts.Values.Sum());
                    toString = (value => this.ToNumberString(value));
                }

                Func<AllScoreData, long> getValueByChara = (allData => 0L);
                if (chara == CharaWithTotal.Total)
                    getValueByChara = (allData => allData.ClearData.Values.Sum(
                        data => (data.Chara != chara) ? getValueByType(data) : 0L));
                else
                    getValueByChara = (allData => getValueByType(allData.ClearData[chara]));

                return toString(getValueByChara(this.allScoreData));
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T14CHARAEX[x][yy][z]
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "StyleCop.CSharp.MaintainabilityRules",
            "SA1119:StatementMustNotUseUnnecessaryParenthesis",
            Justification = "Reviewed.")]
        private string ReplaceCharaEx(string input)
        {
            var pattern = Utils.Format(
                @"%T14CHARAEX([{0}])({1})([1-3])", LevelWithTotalPattern, CharaWithTotalPattern);
            var evaluator = new MatchEvaluator(match =>
            {
                var level = ToLevelWithTotal(match.Groups[1].Value, StringComparison.OrdinalIgnoreCase);
                var chara = ToCharaWithTotal(match.Groups[2].Value, StringComparison.OrdinalIgnoreCase);
                var type = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

                Func<ClearData, long> getValueByType = (data => 0L);
                Func<long, string> toString = (value => string.Empty);
                if (type == 1)
                {
                    getValueByType = (data => data.TotalPlayCount);
                    toString = (value => this.ToNumberString(value));
                }
                else if (type == 2)
                {
                    getValueByType = (data => data.PlayTime);
                    toString = (value => new Time(value).ToString());
                }
                else
                {
                    if (level == LevelWithTotal.Total)
                        getValueByType = (data => data.ClearCounts.Values.Sum());
                    else
                        getValueByType = (data => data.ClearCounts[(LevelPracticeWithTotal)level]);
                    toString = (value => this.ToNumberString(value));
                }

                Func<AllScoreData, long> getValueByChara = (allData => 0L);
                if (chara == CharaWithTotal.Total)
                    getValueByChara = (allData => allData.ClearData.Values.Sum(
                        data => (data.Chara != chara) ? getValueByType(data) : 0L));
                else
                    getValueByChara = (allData => getValueByType(allData.ClearData[chara]));

                return toString(getValueByChara(this.allScoreData));
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T14PRAC[x][yy][z]
        private string ReplacePractice(string input)
        {
            var pattern = Utils.Format(
                @"%T14PRAC([{0}])({1})([{2}])", LevelPattern, CharaPattern, StagePattern);
            var evaluator = new MatchEvaluator(match =>
            {
                var level = ToLevel(match.Groups[1].Value, StringComparison.OrdinalIgnoreCase);
                var chara = (CharaWithTotal)ToChara(
                    match.Groups[2].Value, StringComparison.OrdinalIgnoreCase);
                var stage = ToStage(match.Groups[3].Value, StringComparison.OrdinalIgnoreCase);

                if (level == Level.Extra)
                    return match.ToString();
                if (stage == Stage.Extra)
                    return match.ToString();

                if (this.allScoreData.ClearData.ContainsKey(chara))
                {
                    var key = new LevelStagePair(level, stage);
                    var practices = this.allScoreData.ClearData[chara].Practices;
                    return practices.ContainsKey(key)
                        ? this.ToNumberString(practices[key].Score * 10) : "0";
                }
                else
                    return "0";
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }
    }
}

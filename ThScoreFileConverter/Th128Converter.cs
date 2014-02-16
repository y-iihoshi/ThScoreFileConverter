//-----------------------------------------------------------------------
// <copyright file="Th128Converter.cs" company="None">
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
    public class Th128Converter : ThConverter
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

        private enum Route
        {
            [EnumAltName("A1")] RouteA1,
            [EnumAltName("A2")] RouteA2,
            [EnumAltName("B1")] RouteB1,
            [EnumAltName("B2")] RouteB2,
            [EnumAltName("C1")] RouteC1,
            [EnumAltName("C2")] RouteC2,
            [EnumAltName("EX")] Extra
        }
        private enum RouteWithTotal
        {
            [EnumAltName("A1")] RouteA1,
            [EnumAltName("A2")] RouteA2,
            [EnumAltName("B1")] RouteB1,
            [EnumAltName("B2")] RouteB2,
            [EnumAltName("C1")] RouteC1,
            [EnumAltName("C2")] RouteC2,
            [EnumAltName("EX")] Extra,
            [EnumAltName("TL")] Total
        }

        private enum Stage
        {
            [EnumAltName("A11")] StageA_1,
            [EnumAltName("A12")] StageA1_2,
            [EnumAltName("A13")] StageA1_3,
            [EnumAltName("A22")] StageA2_2,
            [EnumAltName("A23")] StageA2_3,
            [EnumAltName("B11")] StageB_1,
            [EnumAltName("B12")] StageB1_2,
            [EnumAltName("B13")] StageB1_3,
            [EnumAltName("B22")] StageB2_2,
            [EnumAltName("B23")] StageB2_3,
            [EnumAltName("C11")] StageC_1,
            [EnumAltName("C12")] StageC1_2,
            [EnumAltName("C13")] StageC1_3,
            [EnumAltName("C22")] StageC2_2,
            [EnumAltName("C23")] StageC2_3,
            [EnumAltName("EXT")] Extra
        }
        private enum StageWithTotal
        {
            [EnumAltName("A11")] StageA_1,
            [EnumAltName("A12")] StageA1_2,
            [EnumAltName("A13")] StageA1_3,
            [EnumAltName("A22")] StageA2_2,
            [EnumAltName("A23")] StageA2_3,
            [EnumAltName("B11")] StageB_1,
            [EnumAltName("B12")] StageB1_2,
            [EnumAltName("B13")] StageB1_3,
            [EnumAltName("B22")] StageB2_2,
            [EnumAltName("B23")] StageB2_3,
            [EnumAltName("C11")] StageC_1,
            [EnumAltName("C12")] StageC1_2,
            [EnumAltName("C13")] StageC1_3,
            [EnumAltName("C22")] StageC2_2,
            [EnumAltName("C23")] StageC2_3,
            [EnumAltName("EXT")] Extra,
            [EnumAltName("TTL")] Total
        }

        private enum StageProgress
        {
            [EnumAltName("-------")]     None,
            [EnumAltName("Stage A-1")]   StageA_1,
            [EnumAltName("Stage A1-2")]  StageA1_2,
            [EnumAltName("Stage A1-3")]  StageA1_3,
            [EnumAltName("Stage A2-2")]  StageA2_2,
            [EnumAltName("Stage A2-3")]  StageA2_3,
            [EnumAltName("Stage B-1")]   StageB_1,
            [EnumAltName("Stage B1-2")]  StageB1_2,
            [EnumAltName("Stage B1-3")]  StageB1_3,
            [EnumAltName("Stage B2-2")]  StageB2_2,
            [EnumAltName("Stage B2-3")]  StageB2_3,
            [EnumAltName("Stage C-1")]   StageC_1,
            [EnumAltName("Stage C1-2")]  StageC1_2,
            [EnumAltName("Stage C1-3")]  StageC1_3,
            [EnumAltName("Stage C2-2")]  StageC2_2,
            [EnumAltName("Stage C2-3")]  StageC2_3,
            [EnumAltName("Extra Stage")] Extra,
            [EnumAltName("A1 Clear")]    A1Clear,
            [EnumAltName("A2 Clear")]    A2Clear,
            [EnumAltName("B1 Clear")]    B1Clear,
            [EnumAltName("B2 Clear")]    B2Clear,
            [EnumAltName("C1 Clear")]    C1Clear,
            [EnumAltName("C2 Clear")]    C2Clear,
            [EnumAltName("Extra Clear")] ExtraClear
        }

        private const int NumCards = 250;

        // Thanks to thwiki.info
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "StyleCop.CSharp.SpacingRules",
            "SA1008:OpeningParenthesisMustBeSpacedCorrectly",
            Justification = "Reviewed.")]
        private static readonly Dictionary<Stage, Range<int>> StageCardTable =
            new Dictionary<Stage, Range<int>>()
            {
                { Stage.StageA_1,  new Range<int>(  0,   7) },
                { Stage.StageA1_2, new Range<int>(  8,  19) },
                { Stage.StageA1_3, new Range<int>( 20,  43) },
                { Stage.StageA2_2, new Range<int>( 44,  55) },
                { Stage.StageA2_3, new Range<int>( 56,  79) },
                { Stage.StageB_1,  new Range<int>( 80,  87) },
                { Stage.StageB1_2, new Range<int>( 88,  99) },
                { Stage.StageB1_3, new Range<int>(100, 123) },
                { Stage.StageB2_2, new Range<int>(124, 135) },
                { Stage.StageB2_3, new Range<int>(136, 159) },
                { Stage.StageC_1,  new Range<int>(160, 167) },
                { Stage.StageC1_2, new Range<int>(168, 179) },
                { Stage.StageC1_3, new Range<int>(180, 203) },
                { Stage.StageC2_2, new Range<int>(204, 215) },
                { Stage.StageC2_3, new Range<int>(216, 239) },
                { Stage.Extra,     new Range<int>(240, 249) }
            };

        private class AllScoreData
        {
            public Header Header { get; set; }
            public Dictionary<RouteWithTotal, ClearData> ClearData { get; set; }
            public CardData CardData { get; set; }
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

        private class ClearData : Chapter   // per route
        {
            public RouteWithTotal Route { get; private set; }   // size: 4Bytes
            public Dictionary<Level, ScoreData[]> Rankings { get; private set; }
            public int TotalPlayCount { get; private set; }
            public int PlayTime { get; private set; }           // = seconds * 60fps
            public Dictionary<Level, int> ClearCounts { get; private set; }

            public ClearData(Chapter ch)
                : base(ch)
            {
                if (this.Signature != "CR")
                    throw new InvalidDataException("Signature");
                if (this.Version != 0x0003)
                    throw new InvalidDataException("Version");
                if (this.Size != 0x0000066C)
                    throw new InvalidDataException("Size");

                var numLevels = Enum.GetValues(typeof(Level)).Length;
                this.Rankings = new Dictionary<Level, ScoreData[]>(numLevels);
                this.ClearCounts = new Dictionary<Level, int>(numLevels);
            }

            public override void ReadFrom(BinaryReader reader)
            {
                var levels = Utils.GetEnumerator<Level>();
                this.Route = (RouteWithTotal)reader.ReadInt32();
                foreach (var level in levels)
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
                foreach (var level in levels)
                {
                    var clearCount = reader.ReadInt32();
                    if (!this.ClearCounts.ContainsKey(level))
                        this.ClearCounts.Add(level, clearCount);
                }
            }
        }

        private class CardData : Chapter
        {
            public SpellCard[] Cards { get; private set; }  // [0..249]

            public CardData(Chapter ch)
                : base(ch)
            {
                if (this.Signature != "CD")
                    throw new InvalidDataException("Signature");
                if (this.Version != 0x0001)
                    throw new InvalidDataException("Version");
                if (this.Size != 0x0000947C)
                    throw new InvalidDataException("Size");

                this.Cards = new SpellCard[NumCards];
            }

            public override void ReadFrom(BinaryReader reader)
            {
                for (var number = 0; number < Th128Converter.NumCards; number++)
                {
                    var card = new SpellCard();
                    card.ReadFrom(reader);
                    this.Cards[number] = card;
                }
            }
        }

        private class Status : Chapter
        {
            private byte[] unknown1;    // .Length = 0x10
            private byte[] unknown2;    // .Length = 0x18
            private byte[] unknown3;    // .Length = 0x03E0

            public byte[] LastName { get; private set; }    // .Length = 10 (The last 2 bytes are always 0x00 ?)
            public byte[] BgmFlags { get; private set; }    // .Length = 10
            public int TotalPlayTime { get; private set; }  // unit: 10ms

            public Status(Chapter ch)
                : base(ch)
            {
                if (this.Signature != "ST")
                    throw new InvalidDataException("Signature");
                if (this.Version != 0x0002)
                    throw new InvalidDataException("Version");
                if (this.Size != 0x0000042C)
                    throw new InvalidDataException("Size");
            }

            public override void ReadFrom(BinaryReader reader)
            {
                this.LastName = reader.ReadBytes(10);
                this.unknown1 = reader.ReadBytes(0x10);
                this.BgmFlags = reader.ReadBytes(10);
                this.unknown2 = reader.ReadBytes(0x18);
                this.TotalPlayTime = reader.ReadInt32();
                this.unknown3 = reader.ReadBytes(0x03E0);
            }
        }

        private class ScoreData : IBinaryReadable
        {
            private byte[] unknown1;    // .Length = 0x08

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
                this.unknown1 = reader.ReadBytes(0x08);
            }
        }

        private class SpellCard : IBinaryReadable
        {
            private uint unknown1;

            public byte[] Name { get; private set; }        // .Length = 0x80
            public int NoMissCount { get; private set; }
            public int NoIceCount { get; private set; }
            public int TrialCount { get; private set; }
            public int Number { get; private set; }         // 0-based
            public Level Level { get; private set; }

            public void ReadFrom(BinaryReader reader)
            {
                this.Name = reader.ReadBytes(0x80);
                this.NoMissCount = reader.ReadInt32();
                this.NoIceCount = reader.ReadInt32();
                this.unknown1 = reader.ReadUInt32();
                this.TrialCount = reader.ReadInt32();
                this.Number = reader.ReadInt32();
                this.Level = (Level)reader.ReadInt32();
            }
        }

        private AllScoreData allScoreData = null;

        public override string SupportedVersions
        {
            get { return "1.00a"; }
        }

        private static readonly string LevelPattern;
        private static readonly string LevelWithTotalPattern;
        private static readonly string RoutePattern;
        private static readonly string RouteWithTotalPattern;
        private static readonly string StageWithTotalExceptExtraPattern;

        private static readonly Func<string, StringComparison, Level> ToLevel;
        private static readonly Func<string, StringComparison, LevelWithTotal> ToLevelWithTotal;
        private static readonly Func<string, StringComparison, Route> ToRoute;
        private static readonly Func<string, StringComparison, RouteWithTotal> ToRouteWithTotal;
        private static readonly Func<string, StringComparison, StageWithTotal> ToStageWithTotal;

        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Performance",
            "CA1810:InitializeReferenceTypeStaticFieldsInline",
            Justification = "Reviewed.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "StyleCop.CSharp.MaintainabilityRules",
            "SA1119:StatementMustNotUseUnnecessaryParenthesis",
            Justification = "Reviewed.")]
        static Th128Converter()
        {
            var levels = Utils.GetEnumerator<Level>();
            var levelsWithTotal = Utils.GetEnumerator<LevelWithTotal>();
            var routes = Utils.GetEnumerator<Route>();
            var routesWithTotal = Utils.GetEnumerator<RouteWithTotal>();
            var stagesWithTotal = Utils.GetEnumerator<StageWithTotal>();

            // To avoid SA1118
            var stagesWithTotalExceptExtra = stagesWithTotal.Where(st => st != StageWithTotal.Extra);

            LevelPattern = string.Join(
                string.Empty, levels.Select(lv => lv.ToShortName()).ToArray());
            LevelWithTotalPattern = string.Join(
                string.Empty, levelsWithTotal.Select(lv => lv.ToShortName()).ToArray());
            RoutePattern = string.Join(
                "|", routes.Select(rt => rt.ToShortName()).ToArray());
            RouteWithTotalPattern = string.Join(
                "|", routesWithTotal.Select(rt => rt.ToShortName()).ToArray());
            StageWithTotalExceptExtraPattern = string.Join(
                "|", stagesWithTotalExceptExtra.Select(st => st.ToShortName()).ToArray());

            ToLevel = ((shortName, comparisonType) =>
                levels.First(lv => lv.ToShortName().Equals(shortName, comparisonType)));
            ToLevelWithTotal = ((shortName, comparisonType) =>
                levelsWithTotal.First(lv => lv.ToShortName().Equals(shortName, comparisonType)));
            ToRoute = ((shortName, comparisonType) =>
                routes.First(rt => rt.ToShortName().Equals(shortName, comparisonType)));
            ToRouteWithTotal = ((shortName, comparisonType) =>
                routesWithTotal.First(rt => rt.ToShortName().Equals(shortName, comparisonType)));
            ToStageWithTotal = ((shortName, comparisonType) =>
                stagesWithTotal.First(st => st.ToShortName().Equals(shortName, comparisonType)));
        }

        public Th128Converter()
        {
        }

        protected override bool ReadScoreFile(Stream input)
        {
            using (var decrypted = new MemoryStream())
#if DEBUG
            using (var decoded = new FileStream("th128decoded.dat", FileMode.Create, FileAccess.ReadWrite))
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
            if (header.Signature != "T821")
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

                    if (!((chapter.Signature == "CR") && (chapter.Version == 0x0003)) &&
                        !((chapter.Signature == "CD") && (chapter.Version == 0x0001)) &&
                        !((chapter.Signature == "ST") && (chapter.Version == 0x0002)))
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
            var numRoutes = Enum.GetValues(typeof(RouteWithTotal)).Length;

            allScoreData.ClearData = new Dictionary<RouteWithTotal, ClearData>(numRoutes);
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
                                if (!allScoreData.ClearData.ContainsKey(clearData.Route))
                                    allScoreData.ClearData.Add(clearData.Route, clearData);
                            }
                            break;
                        case "CD":
                            {
                                var cardData = new CardData(chapter);
                                cardData.ReadFrom(reader);
                                allScoreData.CardData = cardData;
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
                (allScoreData.ClearData.Count == numRoutes) &&
                (allScoreData.CardData != null) &&
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
            allLines = this.ReplaceRoute(allLines);
            allLines = this.ReplaceRouteEx(allLines);
            allLines = this.ReplaceTime(allLines);
            writer.Write(allLines);

            writer.Flush();
            writer.BaseStream.SetLength(writer.BaseStream.Position);
        }

        // %T128SCR[w][xx][y][z]
        private string ReplaceScore(string input)
        {
            var pattern = Utils.Format(@"%T128SCR([{0}])({1})(\d)([1-5])", LevelPattern, RoutePattern);
            var evaluator = new MatchEvaluator(match =>
            {
                var level = ToLevel(match.Groups[1].Value, StringComparison.OrdinalIgnoreCase);
                var route = (RouteWithTotal)ToRoute(
                    match.Groups[2].Value, StringComparison.OrdinalIgnoreCase);
                var rank = Utils.ToZeroBased(int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture));
                var type = int.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture);

                if ((level == Level.Extra) && (route != RouteWithTotal.Extra))
                    return match.ToString();
                if ((route == RouteWithTotal.Extra) && (level != Level.Extra))
                    return match.ToString();

                var ranking = this.allScoreData.ClearData[route].Rankings[level][rank];
                switch (type)
                {
                    case 1:     // name
                        return Encoding.Default.GetString(ranking.Name).Split('\0')[0];
                    case 2:     // score
                        return this.ToNumberString((ranking.Score * 10) + ranking.ContinueCount);
                    case 3:     // stage
                        if (ranking.DateTime > 0)
                            return ranking.StageProgress.ToShortName();
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

        // %T128C[xxx][z]
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "StyleCop.CSharp.MaintainabilityRules",
            "SA1119:StatementMustNotUseUnnecessaryParenthesis",
            Justification = "Reviewed.")]
        private string ReplaceCareer(string input)
        {
            var pattern = @"%T128C(\d{3})([1-3])";
            var evaluator = new MatchEvaluator(match =>
            {
                var number = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
                var type = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);

                Func<SpellCard, int> getCount = (card => 0);
                if (type == 1)
                    getCount = (card => card.NoIceCount);
                else if (type == 2)
                    getCount = (card => card.NoMissCount);
                else
                    getCount = (card => card.TrialCount);

                if (number == 0)
                    return this.ToNumberString(this.allScoreData.CardData.Cards.Sum(getCount));
                else if (new Range<int>(1, NumCards).Contains(number))
                    return this.ToNumberString(getCount(this.allScoreData.CardData.Cards[number - 1]));
                else
                    return match.ToString();
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T128CARD[xxx][y]
        private string ReplaceCard(string input)
        {
            var pattern = @"%T128CARD(\d{3})([NR])";
            var evaluator = new MatchEvaluator(match =>
            {
                var number = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
                var type = match.Groups[2].Value.ToUpperInvariant();

                if (new Range<int>(1, NumCards).Contains(number))
                {
                    var card = this.allScoreData.CardData.Cards[number - 1];
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

        // %T128CRG[x][yyy][z]
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "StyleCop.CSharp.MaintainabilityRules",
            "SA1119:StatementMustNotUseUnnecessaryParenthesis",
            Justification = "Reviewed.")]
        private string ReplaceCollectRate(string input)
        {
            var pattern = Utils.Format(
                @"%T128CRG([{0}])({1})([1-3])", LevelWithTotalPattern, StageWithTotalExceptExtraPattern);
            var evaluator = new MatchEvaluator(match =>
            {
                var level = ToLevelWithTotal(match.Groups[1].Value, StringComparison.OrdinalIgnoreCase);
                var stage = ToStageWithTotal(match.Groups[2].Value, StringComparison.OrdinalIgnoreCase);
                var type = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

                Func<SpellCard, bool> checkNotNull = (card => card != null);
                Func<SpellCard, bool> findByLevel = (card => true);
                Func<SpellCard, bool> findByStage = (card => true);
                Func<SpellCard, bool> findByType = (card => true);

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

                if (type == 1)
                    findByType = (card => card.NoIceCount > 0);
                else if (type == 2)
                    findByType = (card => card.NoMissCount > 0);
                else
                    findByType = (card => card.TrialCount > 0);

                var and = Utils.MakeAndPredicate(checkNotNull, findByLevel, findByStage, findByType);
                return this.allScoreData.CardData.Cards.Count(and).ToString(CultureInfo.CurrentCulture);
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T128CLEAR[x][yy]
        private string ReplaceClear(string input)
        {
            var pattern = Utils.Format(@"%T128CLEAR([{0}])({1})", LevelPattern, RoutePattern);
            var evaluator = new MatchEvaluator(match =>
            {
                var level = ToLevel(match.Groups[1].Value, StringComparison.OrdinalIgnoreCase);
                var route = (RouteWithTotal)ToRoute(
                    match.Groups[2].Value, StringComparison.OrdinalIgnoreCase);

                if ((level == Level.Extra) && (route != RouteWithTotal.Extra))
                    return match.ToString();
                if ((route == RouteWithTotal.Extra) && (level != Level.Extra))
                    return match.ToString();

                var rankings = this.allScoreData.ClearData[route].Rankings[level]
                    .Where(ranking => ranking.DateTime > 0);
                var stageProgress = (rankings.Count() > 0)
                    ? rankings.Max(ranking => ranking.StageProgress) : StageProgress.None;

                return stageProgress.ToShortName();
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T128ROUTE[xx][y]
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "StyleCop.CSharp.MaintainabilityRules",
            "SA1119:StatementMustNotUseUnnecessaryParenthesis",
            Justification = "Reviewed.")]
        private string ReplaceRoute(string input)
        {
            var pattern = Utils.Format(@"%T128ROUTE({0})([1-3])", RouteWithTotalPattern);
            var evaluator = new MatchEvaluator(match =>
            {
                var route = ToRouteWithTotal(match.Groups[1].Value, StringComparison.OrdinalIgnoreCase);
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

                Func<AllScoreData, long> getValueByRoute = (allData => 0L);
                if (route == RouteWithTotal.Total)
                    getValueByRoute = (allData => allData.ClearData.Values.Sum(
                        data => (data.Route != route) ? getValueByType(data) : 0L));
                else
                    getValueByRoute = (allData => getValueByType(allData.ClearData[route]));

                return toString(getValueByRoute(this.allScoreData));
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T128ROUTEEX[x][yy][z]
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "StyleCop.CSharp.MaintainabilityRules",
            "SA1119:StatementMustNotUseUnnecessaryParenthesis",
            Justification = "Reviewed.")]
        private string ReplaceRouteEx(string input)
        {
            var pattern = Utils.Format(
                @"%T128ROUTEEX([{0}])({1})([1-3])", LevelWithTotalPattern, RouteWithTotalPattern);
            var evaluator = new MatchEvaluator(match =>
            {
                var level = ToLevelWithTotal(match.Groups[1].Value, StringComparison.OrdinalIgnoreCase);
                var route = ToRouteWithTotal(match.Groups[2].Value, StringComparison.OrdinalIgnoreCase);
                var type = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

                if ((level == LevelWithTotal.Extra) &&
                    ((route != RouteWithTotal.Extra) && (route != RouteWithTotal.Total)))
                    return match.ToString();
                if ((route == RouteWithTotal.Extra) &&
                    ((level != LevelWithTotal.Extra) && (level != LevelWithTotal.Total)))
                    return match.ToString();

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
                        getValueByType = (data => data.ClearCounts[(Level)level]);
                    toString = (value => this.ToNumberString(value));
                }

                Func<AllScoreData, long> getValueByRoute = (allData => 0L);
                if (route == RouteWithTotal.Total)
                    getValueByRoute = (allData => allData.ClearData.Values.Sum(
                        data => (data.Route != route) ? getValueByType(data) : 0L));
                else
                    getValueByRoute = (allData => getValueByType(allData.ClearData[route]));

                return toString(getValueByRoute(this.allScoreData));
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T128TIMEPLY
        private string ReplaceTime(string input)
        {
            var pattern = @"%T128TIMEPLY";
            var evaluator = new MatchEvaluator(match =>
            {
                return new Time(this.allScoreData.Status.TotalPlayTime * 10, false).ToLongString();
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }
    }
}

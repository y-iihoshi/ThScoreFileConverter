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
        private enum Level                  { Easy, Normal, Hard, Lunatic, Extra }
        private enum LevelWithTotal         { Easy, Normal, Hard, Lunatic, Extra, Total }
        private enum LevelShort             { E, N, H, L, X }
        private enum LevelShortWithTotal    { E, N, H, L, X, T }

        private enum Route
        {
            RouteA1, RouteA2, RouteB1, RouteB2, RouteC1, RouteC2, Extra
        }
        private enum RouteWithTotal
        {
            RouteA1, RouteA2, RouteB1, RouteB2, RouteC1, RouteC2, Extra, Total
        }
        private enum RouteShort             { A1, A2, B1, B2, C1, C2, EX }
        private enum RouteShortWithTotal    { A1, A2, B1, B2, C1, C2, EX, TL }

        private enum Stage
        {
            StageA_1, StageA1_2, StageA1_3, StageA2_2, StageA2_3,
            StageB_1, StageB1_2, StageB1_3, StageB2_2, StageB2_3,
            StageC_1, StageC1_2, StageC1_3, StageC2_2, StageC2_3, Extra
        }
        private enum StageWithTotal
        {
            StageA_1, StageA1_2, StageA1_3, StageA2_2, StageA2_3,
            StageB_1, StageB1_2, StageB1_3, StageB2_2, StageB2_3,
            StageC_1, StageC1_2, StageC1_3, StageC2_2, StageC2_3, Extra, Total
        }
        private enum StageShort
        {
            A11, A12, A13, A22, A23, B11, B12, B13, B22, B23, C11, C12, C13, C22, C23, EXT
        }
        private enum StageShortWithTotal
        {
            A11, A12, A13, A22, A23, B11, B12, B13, B22, B23, C11, C12, C13, C22, C23, EXT, TTL
        }

        // FIXME
        private static readonly string[] StageProgressArray =
        {
            "-------",
            "Stage A-1", "Stage A1-2", "Stage A1-3", "Stage A2-2", "Stage A2-3",
            "Stage B-1", "Stage B1-2", "Stage B1-3", "Stage B2-2", "Stage B2-3",
            "Stage C-1", "Stage C1-2", "Stage C1-3", "Stage C2-2", "Stage C2-3", "Extra Stage",
            "A1 Clear", "A2 Clear", "B1 Clear", "B2 Clear", "C1 Clear", "C2 Clear", "Extra Clear"
        };

        private const int NumCards = 250;

        // Thanks to thwiki.info
        private static readonly Dictionary<Stage, Range<int>> StageCardTable =
            new Dictionary<Stage, Range<int>>()
            {
                { Stage.StageA_1,  new Range<int> { Min = 0,   Max = 7   } },
                { Stage.StageA1_2, new Range<int> { Min = 8,   Max = 19  } },
                { Stage.StageA1_3, new Range<int> { Min = 20,  Max = 43  } },
                { Stage.StageA2_2, new Range<int> { Min = 44,  Max = 55  } },
                { Stage.StageA2_3, new Range<int> { Min = 56,  Max = 79  } },
                { Stage.StageB_1,  new Range<int> { Min = 80,  Max = 87  } },
                { Stage.StageB1_2, new Range<int> { Min = 88,  Max = 99  } },
                { Stage.StageB1_3, new Range<int> { Min = 100, Max = 123 } },
                { Stage.StageB2_2, new Range<int> { Min = 124, Max = 135 } },
                { Stage.StageB2_3, new Range<int> { Min = 136, Max = 159 } },
                { Stage.StageC_1,  new Range<int> { Min = 160, Max = 167 } },
                { Stage.StageC1_2, new Range<int> { Min = 168, Max = 179 } },
                { Stage.StageC1_3, new Range<int> { Min = 180, Max = 203 } },
                { Stage.StageC2_2, new Range<int> { Min = 204, Max = 215 } },
                { Stage.StageC2_3, new Range<int> { Min = 216, Max = 239 } },
                { Stage.Extra,     new Range<int> { Min = 240, Max = 249 } }
            };

        private class LevelStagePair : Pair<Level, Stage>
        {
            public Level Level { get { return this.First; } }
            public Stage Stage { get { return this.Second; } }

            public LevelStagePair(Level level, Stage stage) : base(level, stage) { }
        }

        private class AllScoreData
        {
            public Header Header { get; set; }
            public Dictionary<RouteWithTotal, ClearData> ClearData { get; set; }
            public CardData CardData { get; set; }
            public Status Status { get; set; }
        }

        private class Header : Utils.IBinaryReadable
        {
            public string Signature { get; private set; }
            public int EncodedAllSize { get; private set; }
            public uint Unknown1 { get; private set; }
            public uint Unknown2 { get; private set; }
            public int EncodedBodySize { get; private set; }
            public int DecodedBodySize { get; private set; }

            public void ReadFrom(BinaryReader reader)
            {
                this.Signature = new string(reader.ReadChars(4));
                this.EncodedAllSize = reader.ReadInt32();
                this.Unknown1 = reader.ReadUInt32();
                this.Unknown2 = reader.ReadUInt32();
                this.EncodedBodySize = reader.ReadInt32();
                this.DecodedBodySize = reader.ReadInt32();
            }

            public void WriteTo(BinaryWriter writer)
            {
                writer.Write(this.Signature.ToCharArray());
                writer.Write(this.EncodedAllSize);
                writer.Write(this.Unknown1);
                writer.Write(this.Unknown2);
                writer.Write(this.EncodedBodySize);
                writer.Write(this.DecodedBodySize);
            }
        }

        private class Chapter : Utils.IBinaryReadable
        {
            public string Signature { get; private set; }
            public ushort Unknown { get; private set; }
            public uint Checksum { get; private set; }
            public int Size { get; private set; }

            public Chapter() { }
            public Chapter(Chapter ch)
            {
                this.Signature = ch.Signature;
                this.Unknown = ch.Unknown;
                this.Checksum = ch.Checksum;
                this.Size = ch.Size;
            }

            public virtual void ReadFrom(BinaryReader reader)
            {
                this.Signature = Encoding.Default.GetString(reader.ReadBytes(2));
                this.Unknown = reader.ReadUInt16();
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
                if (this.Unknown != 0x0003)
                    throw new InvalidDataException("Unknown");
                if (this.Size != 0x0000066C)
                    throw new InvalidDataException("Size");

                var numLevels = Enum.GetValues(typeof(Level)).Length;
                this.Rankings = new Dictionary<Level, ScoreData[]>(numLevels);
                this.ClearCounts = new Dictionary<Level, int>(numLevels);
            }

            public override void ReadFrom(BinaryReader reader)
            {
                var levels = Enum.GetValues(typeof(Level));
                this.Route = (RouteWithTotal)reader.ReadInt32();
                foreach (Level level in levels)
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
                foreach (Level level in levels)
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
                if (this.Unknown != 0x0001)
                    throw new InvalidDataException("Unknown");
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
            public byte[] LastName { get; private set; }    // .Length = 10 (The last 2 bytes are always 0x00 ?)
            public byte[] Unknown1 { get; private set; }    // .Length = 0x10
            public byte[] BgmFlags { get; private set; }    // .Length = 10
            public byte[] Unknown2 { get; private set; }    // .Length = 0x18
            public int TotalPlayTime { get; private set; }  // unit: 10ms
            public byte[] Unknown3 { get; private set; }    // .Length = 0x03E0

            public Status(Chapter ch)
                : base(ch)
            {
                if (this.Signature != "ST")
                    throw new InvalidDataException("Signature");
                if (this.Unknown != 0x0002)
                    throw new InvalidDataException("Unknown");
                if (this.Size != 0x0000042C)
                    throw new InvalidDataException("Size");
            }

            public override void ReadFrom(BinaryReader reader)
            {
                this.LastName = reader.ReadBytes(10);
                this.Unknown1 = reader.ReadBytes(0x10);
                this.BgmFlags = reader.ReadBytes(10);
                this.Unknown2 = reader.ReadBytes(0x18);
                this.TotalPlayTime = reader.ReadInt32();
                this.Unknown3 = reader.ReadBytes(0x03E0);
            }
        }

        private class ScoreData : Utils.IBinaryReadable
        {
            public uint Score { get; private set; }         // * 10
            public byte StageProgress { get; private set; }
            public byte ContinueCount { get; private set; }
            public byte[] Name { get; private set; }        // .Length = 10 (The last 2 bytes are always 0x00 ?)
            public uint DateTime { get; private set; }      // UNIX time (unit: [s])
            public float SlowRate { get; private set; }     // really...?
            public byte[] Unknown1 { get; private set; }    // .Length = 0x08

            public void ReadFrom(BinaryReader reader)
            {
                this.Score = reader.ReadUInt32();
                this.StageProgress = reader.ReadByte();
                this.ContinueCount = reader.ReadByte();
                this.Name = reader.ReadBytes(10);
                this.DateTime = reader.ReadUInt32();
                this.SlowRate = reader.ReadSingle();
                this.Unknown1 = reader.ReadBytes(0x08);
            }
        }

        private class SpellCard : Utils.IBinaryReadable
        {
            public byte[] Name { get; private set; }        // .Length = 0x80
            public int NoMissCount { get; private set; }
            public int NoIceCount { get; private set; }
            public uint Unknown1 { get; private set; }
            public int TrialCount { get; private set; }
            public int Number { get; private set; }         // 0-origin
            public Level Level { get; private set; }        // Easy .. Extra

            public void ReadFrom(BinaryReader reader)
            {
                this.Name = reader.ReadBytes(0x80);
                this.NoMissCount = reader.ReadInt32();
                this.NoIceCount = reader.ReadInt32();
                this.Unknown1 = reader.ReadUInt32();
                this.TrialCount = reader.ReadInt32();
                this.Number = reader.ReadInt32();
                this.Level = (Level)reader.ReadInt32();
            }
        }

        private class Practice : Utils.IBinaryReadable
        {
            public uint Score { get; private set; }     // * 10
            public uint StageFlag { get; private set; } // 0x00000000: disable, 0x00000101: enable ?

            public void ReadFrom(BinaryReader reader)
            {
                this.Score = reader.ReadUInt32();
                this.StageFlag = reader.ReadUInt32();
            }
        }

        private AllScoreData allScoreData = null;

        public override string SupportedVersions
        {
            get { return "1.00a"; }
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
                if (!this.Decrypt(input, decrypted))
                    return false;

                decrypted.Seek(0, SeekOrigin.Begin);
                if (!this.Extract(decrypted, decoded))
                    return false;

                decoded.Seek(0, SeekOrigin.Begin);
                if (!this.Validate(decoded))
                    return false;

                decoded.Seek(0, SeekOrigin.Begin);
                this.allScoreData = this.Read(decoded);

                return this.allScoreData != null;
            }
        }

        private bool Decrypt(Stream input, Stream output)
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

        private bool Extract(Stream input, Stream output)
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

        private bool Validate(Stream input)
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

                    if (!((chapter.Signature == "CR") && (chapter.Unknown == 0x0003)) &&
                        !((chapter.Signature == "CD") && (chapter.Unknown == 0x0001)) &&
                        !((chapter.Signature == "ST") && (chapter.Unknown == 0x0002)))
                        return false;

                    // -4 means the size of Size.
                    reader.BaseStream.Seek(-4, SeekOrigin.Current);
                    // 8 means the total size of Signature, Unknown, and Checksum.
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

        private AllScoreData Read(Stream input)
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
                            var clearData = new ClearData(chapter);
                            clearData.ReadFrom(reader);
                            if (!allScoreData.ClearData.ContainsKey(clearData.Route))
                                allScoreData.ClearData.Add(clearData.Route, clearData);
                            break;
                        case "CD":
                            var cardData = new CardData(chapter);
                            cardData.ReadFrom(reader);
                            allScoreData.CardData = cardData;
                            break;
                        case "ST":
                            var status = new Status(chapter);
                            status.ReadFrom(reader);
                            allScoreData.Status = status;
                            break;
                        default:
                            // 12 means the total size of Signature, Unknown, Checksum, and Size.
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
            var pattern = string.Format(
                @"%T128SCR([{0}])({1})(\d)([1-5])",
                Utils.JoinEnumNames<LevelShort>(string.Empty),
                Utils.JoinEnumNames<RouteShort>("|"));
            var evaluator = Utils.ToNothrowEvaluator(match =>
            {
                var level = (Level)Utils.ParseEnum<LevelShort>(match.Groups[1].Value, true);
                var route = (RouteWithTotal)Utils.ParseEnum<RouteShort>(match.Groups[2].Value, true);
                var rank = (int.Parse(match.Groups[3].Value) + 9) % 10;     // from [1..9, 0] to [0..9]
                var type = int.Parse(match.Groups[4].Value);

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
                            return (ranking.StageProgress < StageProgressArray.Length)
                                ? StageProgressArray[ranking.StageProgress]
                                : StageProgressArray[0];
                        else
                            return StageProgressArray[0];
                    case 4:     // date & time
                        if (ranking.DateTime > 0)
                            return new DateTime(1970, 1, 1).AddSeconds(ranking.DateTime)
                                .ToLocalTime().ToString("yyyy/MM/dd HH:mm:ss");
                        else
                            return "----/--/-- --:--:--";
                    case 5:     // slow
                        if (ranking.DateTime > 0)
                            return ranking.SlowRate.ToString("F3") + "%";
                        else
                            return "-----%";
                    default:    // unreachable
                        return match.ToString();
                }
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T128C[xxx][z]
        private string ReplaceCareer(string input)
        {
            var pattern = @"%T128C(\d{3})([1-3])";
            var evaluator = Utils.ToNothrowEvaluator(match =>
            {
                var number = int.Parse(match.Groups[1].Value);
                var type = int.Parse(match.Groups[2].Value);

                var cards = this.allScoreData.CardData.Cards;
                if (number == 0)
                    switch (type)
                    {
                        case 1:
                            return this.ToNumberString(cards.Sum(card => card.NoIceCount));
                        case 2:
                            return this.ToNumberString(cards.Sum(card => card.NoMissCount));
                        case 3:
                            return this.ToNumberString(cards.Sum(card => card.TrialCount));
                        default:
                            return match.ToString();
                    }
                else if ((0 < number) && (number <= NumCards))
                    switch (type)
                    {
                        case 1:
                            return this.ToNumberString(cards[number - 1].NoIceCount);
                        case 2:
                            return this.ToNumberString(cards[number - 1].NoMissCount);
                        case 3:
                            return this.ToNumberString(cards[number - 1].TrialCount);
                        default:
                            return match.ToString();
                    }
                else
                    return match.ToString();
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T128CARD[xxx][y]
        private string ReplaceCard(string input)
        {
            var pattern = @"%T128CARD(\d{3})([NR])";
            var evaluator = Utils.ToNothrowEvaluator(match =>
            {
                var number = int.Parse(match.Groups[1].Value);
                var type = match.Groups[2].Value.ToUpper();

                if ((0 < number) && (number <= NumCards))
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
            var pattern = string.Format(
                @"%T128CRG([{0}])({1})([1-3])",
                Utils.JoinEnumNames<LevelShortWithTotal>(string.Empty),
                Utils.JoinEnumNames<StageShortWithTotal>("|"));
            var evaluator = Utils.ToNothrowEvaluator(match =>
            {
                var level = Utils.ParseEnum<LevelShortWithTotal>(match.Groups[1].Value, true);
                var stage = Utils.ParseEnum<StageShortWithTotal>(match.Groups[2].Value, true);
                var type = int.Parse(match.Groups[3].Value);

                Func<SpellCard, bool> findCard = (card => false);

                if (level == LevelShortWithTotal.T)
                {
                    if (stage == StageShortWithTotal.TTL)
                        switch (type)
                        {
                            case 1:
                                findCard = (card => card.NoIceCount > 0);
                                break;
                            case 2:
                                findCard = (card => card.NoMissCount > 0);
                                break;
                            case 3:
                                findCard = (card => card.TrialCount > 0);
                                break;
                            default:    // unreachable
                                break;
                        }
                    else
                    {
                        var st = (Stage)stage;
                        switch (type)
                        {
                            case 1:
                                findCard = (card =>
                                    StageCardTable[st].Contains(card.Number) && (card.NoIceCount > 0));
                                break;
                            case 2:
                                findCard = (card =>
                                    StageCardTable[st].Contains(card.Number) && (card.NoMissCount > 0));
                                break;
                            case 3:
                                findCard = (card =>
                                    StageCardTable[st].Contains(card.Number) && (card.TrialCount > 0));
                                break;
                            default:    // unreachable
                                break;
                        }
                    }
                }
                else if (level == LevelShortWithTotal.X)
                    switch (type)
                    {
                        case 1:
                            findCard = (card =>
                                StageCardTable[Stage.Extra].Contains(card.Number) && (card.NoIceCount > 0));
                            break;
                        case 2:
                            findCard = (card =>
                                StageCardTable[Stage.Extra].Contains(card.Number) && (card.NoMissCount > 0));
                            break;
                        case 3:
                            findCard = (card =>
                                StageCardTable[Stage.Extra].Contains(card.Number) && (card.TrialCount > 0));
                            break;
                        default:    // unreachable
                            break;
                    }
                else
                {
                    var lv = (Level)level;
                    if (stage == StageShortWithTotal.TTL)
                        switch (type)
                        {
                            case 1:
                                findCard = (card => (card.Level == lv) && (card.NoIceCount > 0));
                                break;
                            case 2:
                                findCard = (card => (card.Level == lv) && (card.NoMissCount > 0));
                                break;
                            case 3:
                                findCard = (card => (card.Level == lv) && (card.TrialCount > 0));
                                break;
                            default:    // unreachable
                                break;
                        }
                    else
                    {
                        var st = (Stage)stage;
                        switch (type)
                        {
                            case 1:
                                findCard = (card =>
                                    StageCardTable[st].Contains(card.Number) &&
                                    (card.Level == lv) && (card.NoIceCount > 0));
                                break;
                            case 2:
                                findCard = (card =>
                                    StageCardTable[st].Contains(card.Number) &&
                                    (card.Level == lv) && (card.NoMissCount > 0));
                                break;
                            case 3:
                                findCard = (card =>
                                    StageCardTable[st].Contains(card.Number) &&
                                    (card.Level == lv) && (card.TrialCount > 0));
                                break;
                            default:    // unreachable
                                break;
                        }
                    }
                }

                return this.allScoreData.CardData.Cards.Count(findCard).ToString();
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T128CLEAR[x][yy]
        private string ReplaceClear(string input)
        {
            var pattern = string.Format(
                @"%T128CLEAR([{0}])({1})",
                Utils.JoinEnumNames<LevelShort>(string.Empty),
                Utils.JoinEnumNames<RouteShort>("|"));
            var evaluator = Utils.ToNothrowEvaluator(match =>
            {
                var level = (Level)Utils.ParseEnum<LevelShort>(match.Groups[1].Value, true);
                var route = (RouteWithTotal)Utils.ParseEnum<RouteShort>(match.Groups[2].Value, true);

                if ((level == Level.Extra) && (route != RouteWithTotal.Extra))
                    return match.ToString();
                if ((route == RouteWithTotal.Extra) && (level != Level.Extra))
                    return match.ToString();

                var stageProgress = 0;
                for (var rank = 0; rank < 10; rank++)
                {
                    var ranking = this.allScoreData.ClearData[route].Rankings[level][rank];
                    if (ranking.DateTime > 0)
                        stageProgress = Math.Max(stageProgress, ranking.StageProgress);
                }

                return (stageProgress < StageProgressArray.Length)
                    ? StageProgressArray[stageProgress] : StageProgressArray[0];
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T128ROUTE[xx][y]
        private string ReplaceRoute(string input)
        {
            var pattern = string.Format(
                @"%T128ROUTE({0})([1-3])",
                Utils.JoinEnumNames<RouteShortWithTotal>("|"));
            var evaluator = Utils.ToNothrowEvaluator(match =>
            {
                var route =
                    (RouteWithTotal)Utils.ParseEnum<RouteShortWithTotal>(match.Groups[1].Value, true);
                var type = int.Parse(match.Groups[2].Value);

                switch (type)
                {
                    case 1:     // total play count
                        if (route == RouteWithTotal.Total)
                            return this.ToNumberString(
                                this.allScoreData.ClearData.Values.Sum(
                                    data => (data.Route != route) ? data.TotalPlayCount : 0));
                        else
                            return this.ToNumberString(
                                this.allScoreData.ClearData[route].TotalPlayCount);
                    case 2:     // play times
                        {
                            var frames = (route == RouteWithTotal.Total)
                                ? this.allScoreData.ClearData.Values.Sum(
                                    data => (data.Route != route) ? (long)data.PlayTime : 0L)
                                : (long)this.allScoreData.ClearData[route].PlayTime;
                            return new Time(frames).ToString();
                        }
                    case 3:     // clear count
                        if (route == RouteWithTotal.Total)
                            return this.ToNumberString(
                                this.allScoreData.ClearData.Values.Sum(
                                    data => (data.Route != route)
                                        ? data.ClearCounts.Values.Sum() : 0));
                        else
                            return this.ToNumberString(
                                this.allScoreData.ClearData[route].ClearCounts.Values.Sum());
                    default:    // unreachable
                        return match.ToString();
                }
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T128ROUTEEX[x][yy][z]
        private string ReplaceRouteEx(string input)
        {
            var pattern = string.Format(
                @"%T128ROUTEEX([{0}])({1})([1-3])",
                Utils.JoinEnumNames<LevelShortWithTotal>(string.Empty),
                Utils.JoinEnumNames<RouteShortWithTotal>("|"));
            var evaluator = Utils.ToNothrowEvaluator(match =>
            {
                var level =
                    (LevelWithTotal)Utils.ParseEnum<LevelShortWithTotal>(match.Groups[1].Value, true);
                var route =
                    (RouteWithTotal)Utils.ParseEnum<RouteShortWithTotal>(match.Groups[2].Value, true);
                var type = int.Parse(match.Groups[3].Value);

                if ((level == LevelWithTotal.Extra) &&
                    ((route != RouteWithTotal.Extra) && (route != RouteWithTotal.Total)))
                    return match.ToString();
                if ((route == RouteWithTotal.Extra) &&
                    ((level != LevelWithTotal.Extra) && (level != LevelWithTotal.Total)))
                    return match.ToString();

                switch (type)
                {
                    case 1:     // total play count
                        if (route == RouteWithTotal.Total)
                            return this.ToNumberString(
                                this.allScoreData.ClearData.Values.Sum(
                                    data => (data.Route != route) ? data.TotalPlayCount : 0));
                        else
                            return this.allScoreData.ClearData[route].TotalPlayCount.ToString();
                    case 2:     // play times
                        {
                            var frames = (route == RouteWithTotal.Total)
                                ? this.allScoreData.ClearData.Values.Sum(
                                    data => (data.Route != route) ? (long)data.PlayTime : 0L)
                                : (long)this.allScoreData.ClearData[route].PlayTime;
                            return new Time(frames).ToString();
                        }
                    case 3:     // clear count
                        if (route == RouteWithTotal.Total)
                        {
                            if (level == LevelWithTotal.Total)
                                return this.ToNumberString(
                                    this.allScoreData.ClearData.Values.Sum(
                                        data => (data.Route != route)
                                            ? data.ClearCounts.Values.Sum() : 0));
                            else
                                return this.ToNumberString(
                                    this.allScoreData.ClearData.Values.Sum(
                                        data => (data.Route != route)
                                            ? data.ClearCounts[(Level)level] : 0));
                        }
                        else
                        {
                            if (level == LevelWithTotal.Total)
                                return this.ToNumberString(
                                    this.allScoreData.ClearData[route].ClearCounts.Values.Sum());
                            else
                                return this.ToNumberString(
                                    this.allScoreData.ClearData[route].ClearCounts[(Level)level]);
                        }
                    default:    // unreachable
                        return match.ToString();
                }
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T128TIMEPLY
        private string ReplaceTime(string input)
        {
            var pattern = @"%T128TIMEPLY";
            var evaluator = Utils.ToNothrowEvaluator(match =>
            {
                return new Time(this.allScoreData.Status.TotalPlayTime * 10, false).ToLongString();
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }
    }
}

//-----------------------------------------------------------------------
// <copyright file="Th11Converter.cs" company="None">
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
    public class Th11Converter : ThConverter
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

        private enum Chara
        {
            [EnumAltName("RY")] ReimuYukari,
            [EnumAltName("RS")] ReimuSuika,
            [EnumAltName("RA")] ReimuAya,
            [EnumAltName("MA")] MarisaAlice,
            [EnumAltName("MP")] MarisaPatchouli,
            [EnumAltName("MN")] MarisaNitori
        }
        private enum CharaWithTotal
        {
            [EnumAltName("RY")] ReimuYukari,
            [EnumAltName("RS")] ReimuSuika,
            [EnumAltName("RA")] ReimuAya,
            [EnumAltName("MA")] MarisaAlice,
            [EnumAltName("MP")] MarisaPatchouli,
            [EnumAltName("MN")] MarisaNitori,
            [EnumAltName("TL")] Total
        }

        private enum Stage { Stage1, Stage2, Stage3, Stage4, Stage5, Stage6, Extra }

        private static readonly string[] StageProgressArray =
        {
            "-------", "Stage 1", "Stage 2", "Stage 3", "Stage 4", "Stage 5", "Stage 6",
            "Not Clear", "All Clear"
        };

        private const int NumCards = 175;

        // Thanks to thwiki.info
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "StyleCop.CSharp.SpacingRules",
            "SA1008:OpeningParenthesisMustBeSpacedCorrectly",
            Justification = "Reviewed.")]
        private static readonly Dictionary<Stage, Range<int>> StageCardTable =
            new Dictionary<Stage, Range<int>>()
            {
                { Stage.Stage1, new Range<int>(  0,   9) },
                { Stage.Stage2, new Range<int>( 10,  25) },
                { Stage.Stage3, new Range<int>( 26,  41) },
                { Stage.Stage4, new Range<int>( 42, 117) },
                { Stage.Stage5, new Range<int>(118, 137) },
                { Stage.Stage6, new Range<int>(138, 161) },
                { Stage.Extra,  new Range<int>(162, 174) }
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
            public Dictionary<Level, ScoreData[]> Rankings { get; private set; }
            public int TotalPlayCount { get; private set; }
            public int PlayTime { get; private set; }           // = seconds * 60fps
            public Dictionary<Level, int> ClearCounts { get; private set; }
            public Dictionary<LevelStagePair, Practice> Practices { get; private set; }
            public SpellCard[] Cards { get; private set; }      // [0..174]

            public ClearData(Chapter ch)
                : base(ch)
            {
                if (this.Signature != "CR")
                    throw new InvalidDataException("Signature");
                if (this.Version != 0x0000)
                    throw new InvalidDataException("Version");
                if (this.Size != 0x000068D4)
                    throw new InvalidDataException("Size");

                var numLevels = Enum.GetValues(typeof(Level)).Length;
                this.Rankings = new Dictionary<Level, ScoreData[]>(numLevels);
                this.ClearCounts = new Dictionary<Level, int>(numLevels);
                this.Practices = new Dictionary<LevelStagePair, Practice>();
                this.Cards = new SpellCard[NumCards];
            }

            public override void ReadFrom(BinaryReader reader)
            {
                var levels = Utils.GetEnumerator<Level>();
                var stages = Utils.GetEnumerator<Stage>();
                this.Chara = (CharaWithTotal)reader.ReadInt32();
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
                foreach (var level in levels.Where(lv => lv != Level.Extra))
                    foreach (var stage in stages.Where(st => st != Stage.Extra))
                    {
                        var practice = new Practice();
                        practice.ReadFrom(reader);
                        var key = new LevelStagePair(level, stage);
                        if (!this.Practices.ContainsKey(key))
                            this.Practices.Add(key, practice);
                    }
                for (var number = 0; number < Th11Converter.NumCards; number++)
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
            private byte[] unknown2;    // .Length = 0x0411

            public byte[] LastName { get; private set; }    // .Length = 10 (The last 2 bytes are always 0x00 ?)
            public byte[] BgmFlags { get; private set; }    // .Length = 17

            public Status(Chapter ch)
                : base(ch)
            {
                if (this.Signature != "ST")
                    throw new InvalidDataException("Signature");
                if (this.Version != 0x0000)
                    throw new InvalidDataException("Version");
                if (this.Size != 0x00000448)
                    throw new InvalidDataException("Size");
            }

            public override void ReadFrom(BinaryReader reader)
            {
                this.LastName = reader.ReadBytes(10);
                this.unknown1 = reader.ReadBytes(0x10);
                this.BgmFlags = reader.ReadBytes(17);
                this.unknown2 = reader.ReadBytes(0x0411);
            }
        }

        private class ScoreData : IBinaryReadable
        {
            private uint unknown1;

            public uint Score { get; private set; }         // * 10
            public byte StageProgress { get; private set; }
            public byte ContinueCount { get; private set; }
            public byte[] Name { get; private set; }        // .Length = 10 (The last 2 bytes are always 0x00 ?)
            public uint DateTime { get; private set; }      // UNIX time (unit: [s])
            public float SlowRate { get; private set; }     // really...?

            public void ReadFrom(BinaryReader reader)
            {
                this.Score = reader.ReadUInt32();
                this.StageProgress = reader.ReadByte();
                this.ContinueCount = reader.ReadByte();
                this.Name = reader.ReadBytes(10);
                this.DateTime = reader.ReadUInt32();
                this.SlowRate = reader.ReadSingle();
                this.unknown1 = reader.ReadUInt32();
            }
        }

        private class SpellCard : IBinaryReadable
        {
            public byte[] Name { get; private set; }    // .Length = 0x80
            public int ClearCount { get; private set; }
            public int TrialCount { get; private set; }
            public int Number { get; private set; }     // 0-origin
            public Level Level { get; private set; }

            public void ReadFrom(BinaryReader reader)
            {
                this.Name = reader.ReadBytes(0x80);
                this.ClearCount = reader.ReadInt32();
                this.TrialCount = reader.ReadInt32();
                this.Number = reader.ReadInt32();
                this.Level = (Level)reader.ReadInt32();
            }
        }

        private class Practice : IBinaryReadable
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

        public Th11Converter()
        {
        }

        protected override bool ReadScoreFile(Stream input)
        {
            using (var decrypted = new MemoryStream())
#if DEBUG
            using (var decoded = new FileStream("th11decoded.dat", FileMode.Create, FileAccess.ReadWrite))
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
            if (header.Signature != "TH11")
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

                    if (!((chapter.Signature == "CR") && (chapter.Version == 0x0000)) &&
                        !((chapter.Signature == "ST") && (chapter.Version == 0x0000)))
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

        // %T11SCR[w][xx][y][z]
        private string ReplaceScore(string input)
        {
            var levels = Utils.GetEnumerator<Level>();
            var charas = Utils.GetEnumerator<Chara>();
            var pattern = Utils.Format(
                @"%T11SCR([{0}])({1})(\d)([1-5])",
                string.Join(string.Empty, levels.Select(lv => lv.ToShortName()).ToArray()),
                string.Join("|", charas.Select(ch => ch.ToShortName()).ToArray()));
            var evaluator = new MatchEvaluator(match =>
            {
                var level = levels.First(
                    lv => lv.ToShortName()
                        .Equals(match.Groups[1].Value, StringComparison.InvariantCultureIgnoreCase));
                var chara = (CharaWithTotal)charas.First(
                    ch => ch.ToShortName()
                        .Equals(match.Groups[2].Value, StringComparison.InvariantCultureIgnoreCase));
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
                            return (ranking.StageProgress < StageProgressArray.Length)
                                ? StageProgressArray[ranking.StageProgress] : StageProgressArray[0];
                        else
                            return StageProgressArray[0];
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

        // %T11C[xxx][yy][z]
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "StyleCop.CSharp.MaintainabilityRules",
            "SA1119:StatementMustNotUseUnnecessaryParenthesis",
            Justification = "Reviewed.")]
        private string ReplaceCareer(string input)
        {
            var charasWithTotal = Utils.GetEnumerator<CharaWithTotal>();
            var pattern = Utils.Format(
                @"%T11C(\d{{3}})({0})([12])",
                string.Join("|", charasWithTotal.Select(ch => ch.ToShortName()).ToArray()));
            var evaluator = new MatchEvaluator(match =>
            {
                var number = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
                var chara = charasWithTotal.First(
                    ch => ch.ToShortName()
                        .Equals(match.Groups[2].Value, StringComparison.InvariantCultureIgnoreCase));
                var type = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

                Func<SpellCard, int> getCount = (card => 0);
                if (type == 1)
                    getCount = (card => card.ClearCount);
                else
                    getCount = (card => card.TrialCount);

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

        // %T11CARD[xxx][y]
        private string ReplaceCard(string input)
        {
            var pattern = @"%T11CARD(\d{3})([NR])";
            var evaluator = new MatchEvaluator(match =>
            {
                var number = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
                var type = match.Groups[2].Value.ToUpper(CultureInfo.InvariantCulture);

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

        // %T11CRG[w][xx][y][z]
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "StyleCop.CSharp.MaintainabilityRules",
            "SA1119:StatementMustNotUseUnnecessaryParenthesis",
            Justification = "Reviewed.")]
        private string ReplaceCollectRate(string input)
        {
            var levelsWithTotal = Utils.GetEnumerator<LevelWithTotal>();
            var charasWithTotal = Utils.GetEnumerator<CharaWithTotal>();
            var pattern = Utils.Format(
                @"%T11CRG([{0}])({1})([0-6])([12])",
                string.Join(string.Empty, levelsWithTotal.Select(lv => lv.ToShortName()).ToArray()),
                string.Join("|", charasWithTotal.Select(ch => ch.ToShortName()).ToArray()));
            var evaluator = new MatchEvaluator(match =>
            {
                var level = levelsWithTotal.First(
                    lv => lv.ToShortName()
                        .Equals(match.Groups[1].Value, StringComparison.InvariantCultureIgnoreCase));
                var chara = charasWithTotal.First(
                    ch => ch.ToShortName()
                        .Equals(match.Groups[2].Value, StringComparison.InvariantCultureIgnoreCase));
                var stage = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);
                var type = int.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture);

                Func<SpellCard, bool> checkNotNull = (card => card != null);
                Func<SpellCard, bool> findByLevel = (card => true);
                Func<SpellCard, bool> findByStage = (card => true);
                Func<SpellCard, bool> findByType = (card => true);

                if (stage == 0)
                {
                    // Do nothing (total of all stages)
                }
                else
                    findByStage = (card => StageCardTable[(Stage)(stage - 1)].Contains(card.Number));

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
                    findByType = (card => card.ClearCount > 0);
                else
                    findByType = (card => card.TrialCount > 0);

                var and = Utils.MakeAndPredicate(checkNotNull, findByLevel, findByStage, findByType);
                return this.allScoreData.ClearData[chara].Cards.Count(and)
                    .ToString(CultureInfo.CurrentCulture);
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T11CLEAR[x][yy]
        private string ReplaceClear(string input)
        {
            var levels = Utils.GetEnumerator<Level>();
            var charas = Utils.GetEnumerator<Chara>();
            var pattern = Utils.Format(
                @"%T11CLEAR([{0}])({1})",
                string.Join(string.Empty, levels.Select(lv => lv.ToShortName()).ToArray()),
                string.Join("|", charas.Select(ch => ch.ToShortName()).ToArray()));
            var evaluator = new MatchEvaluator(match =>
            {
                var level = levels.First(
                    lv => lv.ToShortName()
                        .Equals(match.Groups[1].Value, StringComparison.InvariantCultureIgnoreCase));
                var chara = (CharaWithTotal)charas.First(
                    ch => ch.ToShortName()
                        .Equals(match.Groups[2].Value, StringComparison.InvariantCultureIgnoreCase));

                var rankings = this.allScoreData.ClearData[chara].Rankings[level]
                    .Where(ranking => ranking.DateTime > 0);
                var stageProgress = (rankings.Count() > 0)
                    ? rankings.Max(ranking => ranking.StageProgress) : 0;

                if (stageProgress < StageProgressArray.Length)
                {
                    if (level != Level.Extra)
                        return (stageProgress != 7)
                            ? StageProgressArray[stageProgress] : StageProgressArray[0];
                    else
                        return (stageProgress >= 7)
                            ? StageProgressArray[stageProgress] : StageProgressArray[0];
                }
                else
                    return StageProgressArray[0];
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T11CHARA[xx][y]
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "StyleCop.CSharp.MaintainabilityRules",
            "SA1119:StatementMustNotUseUnnecessaryParenthesis",
            Justification = "Reviewed.")]
        private string ReplaceChara(string input)
        {
            var charasWithTotal = Utils.GetEnumerator<CharaWithTotal>();
            var pattern = Utils.Format(
                @"%T11CHARA({0})([1-3])",
                string.Join("|", charasWithTotal.Select(ch => ch.ToShortName()).ToArray()));
            var evaluator = new MatchEvaluator(match =>
            {
                var chara = charasWithTotal.First(
                    ch => ch.ToShortName()
                        .Equals(match.Groups[1].Value, StringComparison.InvariantCultureIgnoreCase));
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

        // %T11CHARAEX[x][yy][z]
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "StyleCop.CSharp.MaintainabilityRules",
            "SA1119:StatementMustNotUseUnnecessaryParenthesis",
            Justification = "Reviewed.")]
        private string ReplaceCharaEx(string input)
        {
            var levelsWithTotal = Utils.GetEnumerator<LevelWithTotal>();
            var charasWithTotal = Utils.GetEnumerator<CharaWithTotal>();
            var pattern = Utils.Format(
                @"%T11CHARAEX([{0}])({1})([1-3])",
                string.Join(string.Empty, levelsWithTotal.Select(lv => lv.ToShortName()).ToArray()),
                string.Join("|", charasWithTotal.Select(ch => ch.ToShortName()).ToArray()));
            var evaluator = new MatchEvaluator(match =>
            {
                var level = levelsWithTotal.First(
                    lv => lv.ToShortName()
                        .Equals(match.Groups[1].Value, StringComparison.InvariantCultureIgnoreCase));
                var chara = charasWithTotal.First(
                    ch => ch.ToShortName()
                        .Equals(match.Groups[2].Value, StringComparison.InvariantCultureIgnoreCase));
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
                        getValueByType = (data => data.ClearCounts[(Level)level]);
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

        // %T11PRAC[x][yy][z]
        private string ReplacePractice(string input)
        {
            var levels = Utils.GetEnumerator<Level>();
            var charas = Utils.GetEnumerator<Chara>();
            var pattern = Utils.Format(
                @"%T11PRAC([{0}])({1})([1-6])",
                string.Join(string.Empty, levels.Select(lv => lv.ToShortName()).ToArray()),
                string.Join("|", charas.Select(ch => ch.ToShortName()).ToArray()));
            var evaluator = new MatchEvaluator(match =>
            {
                var level = levels.First(
                    lv => lv.ToShortName()
                        .Equals(match.Groups[1].Value, StringComparison.InvariantCultureIgnoreCase));
                var chara = (CharaWithTotal)charas.First(
                    ch => ch.ToShortName()
                        .Equals(match.Groups[2].Value, StringComparison.InvariantCultureIgnoreCase));
                var stage = (Stage)Utils.ToZeroBased(
                    int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture));

                if (level == Level.Extra)
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

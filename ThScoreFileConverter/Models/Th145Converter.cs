//-----------------------------------------------------------------------
// <copyright file="Th145Converter.cs" company="None">
//     (c) 2017 IIHOSHI Yoshinori
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable 1591

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "*", Justification = "Reviewed.")]

namespace ThScoreFileConverter.Models
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    internal class Th145Converter : ThConverter
    {
        private static readonly new EnumShortNameParser<Level> LevelParser =
            new EnumShortNameParser<Level>();

        private static readonly new EnumShortNameParser<LevelWithTotal> LevelWithTotalParser =
            new EnumShortNameParser<LevelWithTotal>();

        private static readonly EnumShortNameParser<Chara> CharaParser =
            new EnumShortNameParser<Chara>();

        private static readonly EnumShortNameParser<CharaWithTotal> CharaWithTotalParser =
            new EnumShortNameParser<CharaWithTotal>();

        private AllScoreData allScoreData = null;

        public new enum Level
        {
            [EnumAltName("E")] Easy,
            [EnumAltName("N")] Normal,
            [EnumAltName("H")] Hard,
            [EnumAltName("L")] Lunatic
        }

        public new enum LevelWithTotal
        {
            [EnumAltName("E")] Easy,
            [EnumAltName("N")] Normal,
            [EnumAltName("H")] Hard,
            [EnumAltName("L")] Lunatic,
            [EnumAltName("T")] Total
        }

        [Flags]
        public enum LevelFlag
        {
            None = 0,
            Easy = 1,
            Normal = 2,
            Hard = 4,
            Lunatic = 8
        }

        public enum Chara
        {
            [EnumAltName("RA")] ReimuA,
            [EnumAltName("MR")] Marisa,
            [EnumAltName("IU")] IchirinUnzan,
            [EnumAltName("BY")] Byakuren,
            [EnumAltName("FT")] Futo,
            [EnumAltName("MK")] Miko,
            [EnumAltName("NT")] Nitori,
            [EnumAltName("KO")] Koishi,
            [EnumAltName("MM")] Mamizou,
            [EnumAltName("KK")] Kokoro,
            [EnumAltName("KS")] Kasen,
            [EnumAltName("MO")] Mokou,
            [EnumAltName("SN")] Shinmyoumaru,
            [EnumAltName("SM")] Sumireko,
            [EnumAltName("RB")] ReimuB
        }

        public enum CharaWithTotal
        {
            [EnumAltName("RA")] ReimuA,
            [EnumAltName("MR")] Marisa,
            [EnumAltName("IU")] IchirinUnzan,
            [EnumAltName("BY")] Byakuren,
            [EnumAltName("FT")] Futo,
            [EnumAltName("MK")] Miko,
            [EnumAltName("NT")] Nitori,
            [EnumAltName("KO")] Koishi,
            [EnumAltName("MM")] Mamizou,
            [EnumAltName("KK")] Kokoro,
            [EnumAltName("KS")] Kasen,
            [EnumAltName("MO")] Mokou,
            [EnumAltName("SN")] Shinmyoumaru,
            [EnumAltName("SM")] Sumireko,
            [EnumAltName("RB")] ReimuB,
            [EnumAltName("TL")] Total
        }

        public override string SupportedVersions
        {
            get { return "1.41"; }
        }

        public override bool HasCardReplacer
        {
            get { return false; }
        }

        protected override bool ReadScoreFile(Stream input)
        {
#if DEBUG
            using (var decoded = new FileStream("th145decoded.dat", FileMode.Create, FileAccess.ReadWrite))
#else
            using (var decoded = new MemoryStream())
#endif
            {
                if (!Extract(input, decoded))
                    return false;

                decoded.Seek(0, SeekOrigin.Begin);
                this.allScoreData = Read(decoded);

                return this.allScoreData != null;
            }
        }

        protected override IEnumerable<IStringReplaceable> CreateReplacers(
            bool hideUntriedCards, string outputFilePath)
        {
            return new List<IStringReplaceable>
            {
                new ClearRankReplacer(this),
                new ClearTimeReplacer(this)
            };
        }

        private static bool Extract(Stream input, Stream output)
        {
            var succeeded = false;

            // See section 2.2 of RFC 1950
            var validHeader = new byte[] { 0x78, 0x9C };

            if (input.Length >= sizeof(int) + validHeader.Length)
            {
                var size = new byte[sizeof(int)];
                var header = new byte[validHeader.Length];

                input.Seek(0, SeekOrigin.Begin);
                input.Read(size, 0, size.Length);
                input.Read(header, 0, header.Length);

                if (Enumerable.SequenceEqual(header, validHeader))
                {
                    var extracted = new byte[0x80000];
                    var extractedSize = 0;

                    using (var deflate = new DeflateStream(input, CompressionMode.Decompress, true))
                        extractedSize = deflate.Read(extracted, 0, extracted.Length);

                    output.Seek(0, SeekOrigin.Begin);
                    output.Write(extracted, 0, extractedSize);

                    succeeded = true;
                }
                else
                {
                    // Invalid header
                }
            }
            else
            {
                // The input stream is too short
            }

            return succeeded;
        }

        private static AllScoreData Read(Stream input)
        {
            var reader = new BinaryReader(input);
            var allScoreData = new AllScoreData();

            try
            {
                allScoreData.ReadFrom(reader);
            }
            catch (EndOfStreamException)
            {
            }

            return allScoreData;
        }

        // %T145CLEAR[x][yy]
        private class ClearRankReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T145CLEAR({0})({1})", LevelParser.Pattern, CharaParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public ClearRankReplacer(Th145Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var level = LevelParser.Parse(match.Groups[1].Value);
                    var chara = CharaParser.Parse(match.Groups[2].Value);

                    // FIXME
                    switch (parent.allScoreData.ClearRanks[level][chara])
                    {
                        case 1:
                            return "Bronze";
                        case 2:
                            return "Silver";
                        case 3:
                            return "Gold";
                        default:
                            return "Not Clear";
                    }
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        // %T145TIMECLR[x][yy]
        private class ClearTimeReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T145TIMECLR({0})({1})", LevelWithTotalParser.Pattern, CharaWithTotalParser.Pattern);

            private readonly MatchEvaluator evaluator;

            [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
            public ClearTimeReplacer(Th145Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var level = LevelWithTotalParser.Parse(match.Groups[1].Value);
                    var chara = CharaWithTotalParser.Parse(match.Groups[2].Value);

                    Func<Dictionary<Chara, int>, int> getValueByChara;
                    if (chara == CharaWithTotal.Total)
                        getValueByChara = (dict => dict.Values.Sum());
                    else
                        getValueByChara = (dict => dict[(Chara)chara]);

                    Func<Dictionary<Level, Dictionary<Chara, int>>, int> getValueByLevel;
                    if (level == LevelWithTotal.Total)
                        getValueByLevel = (dict => dict.Values.Sum(getValueByChara));
                    else
                        getValueByLevel = (dict => getValueByChara(dict[(Level)level]));

                    return new Time(getValueByLevel(parent.allScoreData.ClearTimes)).ToString();
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        private class AllScoreData : IBinaryReadable
        {
            private static readonly Func<BinaryReader, object> StringReader =
                reader =>
                {
                    var size = reader.ReadInt32();
                    return (size > 0) ? Encoding.Default.GetString(reader.ReadBytes(size)) : string.Empty;
                };

            private static readonly Func<BinaryReader, object> ArrayReader =
                reader =>
                {
                    var num = reader.ReadInt32();
                    if (num > 0)
                    {
                        var array = new object[num];
                        for (var count = 0; count < num; count++)
                        {
                            object index;
                            if (ReadObject(reader, out index))
                            {
                                object value;
                                if (ReadObject(reader, out value))
                                {
                                    if ((index is int) && ((int)index < num))
                                        array[(int)index] = value;
                                }
                            }
                        }

                        object endmark;
                        if (ReadObject(reader, out endmark) && (endmark is EndMark))
                            return array;
                    }

                    return new object[] { };
                };

            private static readonly Func<BinaryReader, object> DictionaryReader =
                reader =>
                {
                    var dictionary = new Dictionary<object, object>();
                    while (true)
                    {
                        object key;
                        if (ReadObject(reader, out key))
                        {
                            if (key is EndMark)
                                break;

                            object value;
                            if (ReadObject(reader, out value))
                                dictionary.Add(key, value);
                        }
                        else
                            break;
                    }

                    return dictionary;
                };

            private static readonly Dictionary<uint, Func<BinaryReader, object>> ObjectReaders =
                new Dictionary<uint, Func<BinaryReader, object>>
                {
                    { 0x01000001, reader => new EndMark() },
                    { 0x01000008, reader => reader.ReadByte() == 0x01 },
                    { 0x05000002, reader => reader.ReadInt32() },
                    { 0x05000004, reader => reader.ReadSingle() },
                    { 0x08000010, StringReader },
                    { 0x08000040, ArrayReader },
                    { 0x0A000020, DictionaryReader },
                    { 0x0A008000, reader => new object() }  // unknown (appears in gauge_1p/2p...)
                };

            private Dictionary<string, object> allData;

            public AllScoreData()
            {
                this.allData = null;
                this.StoryClearFlags = null;
                this.BgmFlags = null;
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public int StoryProgress
            {
                get { return this.GetValue<int>("story_progress"); }
            }

            public Dictionary<Chara, LevelFlag> StoryClearFlags { get; private set; }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public int EndingCount
            {
                get { return this.GetValue<int>("ed_count"); }
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public int Ending2Count
            {
                get { return this.GetValue<int>("ed2_count"); }
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public bool IsEnabledStageTanuki1
            {
                get { return this.GetValue<bool>("enable_stage_tanuki1"); }
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public bool IsEnabledStageTanuki2
            {
                get { return this.GetValue<bool>("enable_stage_tanuki2"); }
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public bool IsEnabledStageKokoro
            {
                get { return this.GetValue<bool>("enable_stage_kokoro"); }
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public bool IsEnabledSt27
            {
                get { return this.GetValue<bool>("enable_st27"); }
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public bool IsEnabledSt28
            {
                get { return this.GetValue<bool>("enable_st28"); }
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public bool IsPlayableMamizou
            {
                get { return this.GetValue<bool>("enable_mamizou"); }
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public bool IsPlayableKokoro
            {
                get { return this.GetValue<bool>("enable_kokoro"); }
            }

            public Dictionary<int, bool> BgmFlags { get; private set; }

            public Dictionary<Level, Dictionary<Chara, int>> ClearRanks { get; private set; }

            public Dictionary<Level, Dictionary<Chara, int>> ClearTimes { get; private set; }

            public static bool ReadObject(BinaryReader reader, out object obj)
            {
                var type = reader.ReadUInt32();

                Func<BinaryReader, object> objectReader;
                obj = ObjectReaders.TryGetValue(type, out objectReader) ? objectReader(reader) : null;

                return obj != null;
            }

            public void ReadFrom(BinaryReader reader)
            {
                var dictionary = DictionaryReader(reader) as Dictionary<object, object>;
                if (dictionary != null)
                {
                    this.allData = dictionary
                        .Where(pair => pair.Key is string)
                        .ToDictionary(pair => pair.Key as string, pair => pair.Value);

                    object counts;
                    if (this.allData.TryGetValue("story_clear", out counts))
                    {
                        var storyClearFlags = counts as object[];
                        if (storyClearFlags != null)
                        {
                            this.StoryClearFlags =
                                new Dictionary<Chara, LevelFlag>(Enum.GetValues(typeof(Chara)).Length);
                            for (var index = 0; index < storyClearFlags.Length; index++)
                            {
                                if (storyClearFlags[index] is int)
                                    this.StoryClearFlags[(Chara)index] = (LevelFlag)storyClearFlags[index];
                            }
                        }
                    }

                    object flags;
                    if (this.allData.TryGetValue("enable_bgm", out flags))
                    {
                        var bgmFlags = flags as Dictionary<object, object>;
                        if (bgmFlags != null)
                        {
                            this.BgmFlags = bgmFlags
                                .Where(pair => (pair.Key is int) && (pair.Value is bool))
                                .ToDictionary(pair => (int)pair.Key, pair => (bool)pair.Value);
                        }
                    }

                    object ranks;
                    if (this.allData.TryGetValue("clear_rank", out ranks))
                    {
                        var clearRanks = ranks as object[];
                        if (clearRanks != null)
                        {
                            this.ClearRanks =
                                new Dictionary<Level, Dictionary<Chara, int>>(Enum.GetValues(typeof(Level)).Length);
                            for (var index = 0; index < clearRanks.Length; index++)
                            {
                                var clearRanksPerChara = clearRanks[index] as object[];
                                if (clearRanksPerChara != null)
                                {
                                    this.ClearRanks[(Level)index] =
                                        new Dictionary<Chara, int>(Enum.GetValues(typeof(Chara)).Length);
                                    for (var charaIndex = 0; charaIndex < clearRanksPerChara.Length; charaIndex++)
                                    {
                                        if (clearRanksPerChara[charaIndex] is int)
                                        {
                                            this.ClearRanks[(Level)index][(Chara)charaIndex] = (int)clearRanksPerChara[charaIndex];
                                        }
                                    }
                                }
                            }
                        }
                    }

                    object times;
                    if (this.allData.TryGetValue("clear_time", out times))
                    {
                        var clearTimes = times as object[];
                        if (clearTimes != null)
                        {
                            this.ClearTimes =
                                new Dictionary<Level, Dictionary<Chara, int>>(Enum.GetValues(typeof(Level)).Length);
                            for (var index = 0; index < clearTimes.Length; index++)
                            {
                                var clearTimesPerChara = clearTimes[index] as object[];
                                if (clearTimesPerChara != null)
                                {
                                    this.ClearTimes[(Level)index] =
                                        new Dictionary<Chara, int>(Enum.GetValues(typeof(Chara)).Length);
                                    for (var charaIndex = 0; charaIndex < clearTimesPerChara.Length; charaIndex++)
                                    {
                                        if (clearTimesPerChara[charaIndex] is int)
                                        {
                                            this.ClearTimes[(Level)index][(Chara)charaIndex] = (int)clearTimesPerChara[charaIndex];
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            private T GetValue<T>(string key)
                where T : struct
            {
                object value;
                if (this.allData.TryGetValue(key, out value) && (value is T))
                    return (T)value;
                else
                    return default(T);
            }

            private class EndMark
            {
            }
        }
    }
}

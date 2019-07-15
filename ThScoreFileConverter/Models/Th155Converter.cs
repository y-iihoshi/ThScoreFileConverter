//-----------------------------------------------------------------------
// <copyright file="Th155Converter.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable 1591
#pragma warning disable SA1600 // ElementsMustBeDocumented
#pragma warning disable SA1602 // EnumerationItemsMustBeDocumented

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "*", Justification = "Reviewed.")]

namespace ThScoreFileConverter.Models
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Text.RegularExpressions;
    using ThScoreFileConverter.Properties;

    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Reviewed.")]
    internal class Th155Converter : ThConverter
    {
        private static new readonly EnumShortNameParser<Level> LevelParser =
            new EnumShortNameParser<Level>();

        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "For future use.")]
#pragma warning disable IDE0052 // Remove unread private members
        private static new readonly EnumShortNameParser<LevelWithTotal> LevelWithTotalParser =
            new EnumShortNameParser<LevelWithTotal>();
#pragma warning restore IDE0052 // Remove unread private members

        private static readonly EnumShortNameParser<StoryChara> StoryCharaParser =
            new EnumShortNameParser<StoryChara>();

        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "For future use.")]
#pragma warning disable IDE0052 // Remove unread private members
        private static readonly EnumShortNameParser<StoryCharaWithTotal> StoryCharaWithTotalParser =
            new EnumShortNameParser<StoryCharaWithTotal>();
#pragma warning restore IDE0052 // Remove unread private members

        private AllScoreData allScoreData = null;

        public new enum Level
        {
#pragma warning disable SA1134 // Attributes should not share line
            [EnumAltName("E")] Easy,
            [EnumAltName("N")] Normal,
            [EnumAltName("H")] Hard,
            [EnumAltName("L")] Lunatic,
            [EnumAltName("D")] OverDrive,
#pragma warning restore SA1134 // Attributes should not share line
        }

        public new enum LevelWithTotal
        {
#pragma warning disable SA1134 // Attributes should not share line
            [EnumAltName("E")] Easy,
            [EnumAltName("N")] Normal,
            [EnumAltName("H")] Hard,
            [EnumAltName("L")] Lunatic,
            [EnumAltName("D")] OverDrive,
            [EnumAltName("T")] Total,
#pragma warning restore SA1134 // Attributes should not share line
        }

        [Flags]
        public enum LevelFlag
        {
            None = 0,
            Easy = 1,
            Normal = 2,
            Hard = 4,
            Lunatic = 8,
            OverDrive = 16,
        }

        public enum StoryChara
        {
#pragma warning disable SA1134 // Attributes should not share line
            [EnumAltName("RK")] ReimuKasen,
            [EnumAltName("MK")] MarisaKoishi,
            [EnumAltName("NK")] NitoriKokoro,
            [EnumAltName("MM")] MamizouMokou,
            [EnumAltName("MB")] MikoByakuren,
            [EnumAltName("FI")] FutoIchirin,
            [EnumAltName("RD")] ReisenDoremy,
            [EnumAltName("SD")] SumirekoDoremy,
            [EnumAltName("TS")] TenshiShinmyoumaru,
            [EnumAltName("YR")] YukariReimu,
            [EnumAltName("JS")] JoonShion,
#pragma warning restore SA1134 // Attributes should not share line
        }

        public enum StoryCharaWithTotal
        {
#pragma warning disable SA1134 // Attributes should not share line
            [EnumAltName("RK")] ReimuKasen,
            [EnumAltName("MK")] MarisaKoishi,
            [EnumAltName("NK")] NitoriKokoro,
            [EnumAltName("MM")] MamizouMokou,
            [EnumAltName("MB")] MikoByakuren,
            [EnumAltName("FI")] FutoIchirin,
            [EnumAltName("RD")] ReisenDoremy,
            [EnumAltName("SD")] SumirekoDoremy,
            [EnumAltName("TS")] TenshiShinmyoumaru,
            [EnumAltName("YR")] YukariReimu,
            [EnumAltName("JS")] JoonShion,
            [EnumAltName("TL")] Total,
#pragma warning restore SA1134 // Attributes should not share line
        }

        public override string SupportedVersions
        {
            get { return "1.10c"; }
        }

        public override bool HasCardReplacer
        {
            get { return false; }
        }

        protected override bool ReadScoreFile(Stream input)
        {
#if DEBUG
            using (var decoded = new FileStream("th155decoded.dat", FileMode.Create, FileAccess.ReadWrite))
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
            using (var reader = new BinaryReader(input, Encoding.UTF8, true))
            {
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
        }

        // %T155CLEAR[x][yy]
        private class ClearRankReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T155CLEAR({0})({1})", LevelParser.Pattern, StoryCharaParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public ClearRankReplacer(Th155Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var level = LevelParser.Parse(match.Groups[1].Value);
                    var chara = StoryCharaParser.Parse(match.Groups[2].Value);

                    LevelFlag ToLevelFlag(Level lv)
                    {
                        switch (lv)
                        {
                            case Level.Easy:
                                return LevelFlag.Easy;
                            case Level.Normal:
                                return LevelFlag.Normal;
                            case Level.Hard:
                                return LevelFlag.Hard;
                            case Level.Lunatic:
                                return LevelFlag.Lunatic;
                            case Level.OverDrive:
                                return LevelFlag.OverDrive;
                            default:
                                return LevelFlag.None;
                        }
                    }

                    if (parent.allScoreData.StoryDictionary.TryGetValue(chara, out AllScoreData.Story story)
                        && story.Available
                        && ((story.Ed & ToLevelFlag(level)) != LevelFlag.None))
                        return "Clear";
                    else
                        return "Not Clear";
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
                    return (size > 0) ? Encoding.CP932.GetString(reader.ReadExactBytes(size)) : string.Empty;
                };

            private static readonly Func<BinaryReader, object> ArrayReader =
                reader =>
                {
                    var num = reader.ReadInt32();
                    if (num < 0)
                        throw new InvalidDataException("number of elements must not be negative");

                    var array = new object[num];
                    for (var count = 0; count < num; count++)
                    {
                        if (!ReadObject(reader, out object index))
                            throw new InvalidDataException("failed to read index");
                        if (!ReadObject(reader, out object value))
                            throw new InvalidDataException("failed to read value");

                        if (!(index is int i))
                            throw new InvalidDataException("index is not an integer");
                        if (i >= num)
                            throw new InvalidDataException("index is out of range");

                        array[i] = value;
                    }

                    if (!ReadObject(reader, out object endmark))
                        throw new InvalidDataException("failed to read sentinel");

                    if (endmark is EndMark)
                        return array;
                    else
                        throw new InvalidDataException("sentinel is wrong");
                };

            private static readonly Func<BinaryReader, object> DictionaryReader =
                reader =>
                {
                    var dictionary = new Dictionary<object, object>();
                    while (true)
                    {
                        if (!ReadObject(reader, out object key))
                            throw new InvalidDataException("failed to read key");
                        if (key is EndMark)
                            break;
                        if (!ReadObject(reader, out object value))
                            throw new InvalidDataException("failed to read value");
                        dictionary.Add(key, value);
                    }

                    return dictionary;
                };

            private static readonly Dictionary<uint, Func<BinaryReader, object>> ObjectReaders =
                new Dictionary<uint, Func<BinaryReader, object>>
                {
                    { (uint)Squirrel.OTNull,     reader => new EndMark() },
                    { (uint)Squirrel.OTBool,     reader => reader.ReadByte() != 0x00 },
                    { (uint)Squirrel.OTInteger,  reader => reader.ReadInt32() },
                    { (uint)Squirrel.OTFloat,    reader => reader.ReadSingle() },
                    { (uint)Squirrel.OTString,   StringReader },
                    { (uint)Squirrel.OTArray,    ArrayReader },
                    { (uint)Squirrel.OTClosure,  reader => new OptionalMark() },
                    { (uint)Squirrel.OTTable,    DictionaryReader },
                    { (uint)Squirrel.OTInstance, reader => new object() }, // unknown (appears in gauge_1p/2p...)
                };

            private Dictionary<string, object> allData;

            public AllScoreData()
            {
                this.allData = null;
                this.StoryDictionary = null;
                this.BgmDictionary = null;
                this.EndingDictionary = null;
                this.StageDictionary = null;
            }

            public Dictionary<StoryChara, Story> StoryDictionary { get; private set; }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public Dictionary<string, int> CharacterDictionary { get; private set; }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public Dictionary<int, bool> BgmDictionary { get; private set; }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public Dictionary<string, int> EndingDictionary { get; private set; }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public Dictionary<int, int> StageDictionary { get; private set; }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public int Version { get; private set; }

            public static bool ReadObject(BinaryReader reader, out object obj)
            {
                if (reader is null)
                    throw new ArgumentNullException(nameof(reader));

                var type = reader.ReadUInt32();

                if (ObjectReaders.TryGetValue(type, out Func<BinaryReader, object> objectReader))
                    obj = objectReader(reader);
                else
                    throw new InvalidDataException(Resources.InvalidDataExceptionWrongType);

                return obj != null;
            }

            public void ReadFrom(BinaryReader reader)
            {
                if (DictionaryReader(reader) is Dictionary<object, object> dictionary)
                {
                    this.allData = dictionary
                        .Where(pair => pair.Key is string)
                        .ToDictionary(pair => pair.Key as string, pair => pair.Value);

                    this.ParseStoryDictionary();
                    this.ParseCharacterDictionary();
                    this.ParseBgmDictionary();
                    this.ParseEndingDictionary();
                    this.ParseStageDictionary();
                    this.ParseVersion();
                }
            }

            private static StoryChara? ParseStoryChara(object obj)
            {
                if (obj is string str)
                {
                    switch (str)
                    {
                        case "reimu":
                            return StoryChara.ReimuKasen;
                        case "marisa":
                            return StoryChara.MarisaKoishi;
                        case "nitori":
                            return StoryChara.NitoriKokoro;
                        case "usami":
                            return StoryChara.SumirekoDoremy;
                        case "tenshi":
                            return StoryChara.TenshiShinmyoumaru;
                        case "miko":
                            return StoryChara.MikoByakuren;
                        case "yukari":
                            return StoryChara.YukariReimu;
                        case "mamizou":
                            return StoryChara.MamizouMokou;
                        case "udonge":
                            return StoryChara.ReisenDoremy;
                        case "futo":
                            return StoryChara.FutoIchirin;
                        case "jyoon":
                            return StoryChara.JoonShion;
                        default:
                            return null;
                    }
                }
                else
                {
                    return null;
                }
            }

            private static Story ParseStory(object obj)
            {
                var story = default(Story);

                if (obj is Dictionary<object, object> dict)
                {
                    foreach (var pair in dict)
                    {
                        if (pair.Key is string key)
                        {
                            if ((key == "stage") && (pair.Value is int stage))
                                story.Stage = stage;
                            if ((key == "ed") && (pair.Value is int ed))
                                story.Ed = (LevelFlag)ed;
                            if ((key == "available") && (pair.Value is bool available))
                                story.Available = available;
                            if ((key == "overdrive") && (pair.Value is int overDrive))
                                story.OverDrive = overDrive;
                            if ((key == "stage_overdrive") && (pair.Value is int stageOverDrive))
                                story.StageOverDrive = stageOverDrive;
                        }
                    }
                }

                return story;
            }

            private void ParseStoryDictionary()
            {
                if (this.allData.TryGetValue("story", out object story))
                {
                    if (story is Dictionary<object, object> dict)
                    {
                        this.StoryDictionary = dict
                            .Where(pair => ParseStoryChara(pair.Key) != null)
                            .ToDictionary(
                                pair => ParseStoryChara(pair.Key).Value,
                                pair => ParseStory(pair.Value));
                    }
                }
            }

            private void ParseCharacterDictionary()
            {
                if (this.allData.TryGetValue("character", out object character))
                {
                    if (character is Dictionary<object, object> dict)
                    {
                        this.CharacterDictionary = dict
                            .Where(pair => (pair.Key is string) && (pair.Value is int))
                            .ToDictionary(pair => (string)pair.Key, pair => (int)pair.Value);
                    }
                }
            }

            private void ParseBgmDictionary()
            {
                if (this.allData.TryGetValue("bgm", out object bgm))
                {
                    if (bgm is Dictionary<object, object> dict)
                    {
                        this.BgmDictionary = dict
                            .Where(pair => (pair.Key is int) && (pair.Value is bool))
                            .ToDictionary(pair => (int)pair.Key, pair => (bool)pair.Value);
                    }
                }
            }

            private void ParseEndingDictionary()
            {
                if (this.allData.TryGetValue("ed", out object ed))
                {
                    if (ed is Dictionary<object, object> dict)
                    {
                        this.EndingDictionary = dict
                            .Where(pair => (pair.Key is string) && (pair.Value is int))
                            .ToDictionary(pair => (string)pair.Key, pair => (int)pair.Value);
                    }
                }
            }

            private void ParseStageDictionary()
            {
                if (this.allData.TryGetValue("stage", out object stage))
                {
                    if (stage is Dictionary<object, object> dict)
                    {
                        this.StageDictionary = dict
                            .Where(pair => (pair.Key is int) && (pair.Value is int))
                            .ToDictionary(pair => (int)pair.Key, pair => (int)pair.Value);
                    }
                }
            }

            private void ParseVersion()
            {
                this.Version = this.GetValue<int>("version");
            }

            private T GetValue<T>(string key)
                where T : struct
                => (this.allData.TryGetValue(key, out object value) && (value is T)) ? (T)value : default;

            public struct Story
            {
                public int Stage;
                public LevelFlag Ed;
                public bool Available;
                public int OverDrive;
                public int StageOverDrive;
            }

            private class OptionalMark
            {
            }

            private class EndMark
            {
            }
        }
    }
}

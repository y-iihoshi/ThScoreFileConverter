﻿//-----------------------------------------------------------------------
// <copyright file="Th135Converter.cs" company="None">
//     (c) 2014-2019 IIHOSHI Yoshinori
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable 1591

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

    internal class Th135Converter : ThConverter
    {
        private static readonly new EnumShortNameParser<Level> LevelParser =
            new EnumShortNameParser<Level>();

        private static readonly EnumShortNameParser<Chara> CharaParser =
            new EnumShortNameParser<Chara>();

        private AllScoreData allScoreData = null;

        public new enum Level
        {
            [EnumAltName("E")] Easy,
            [EnumAltName("N")] Normal,
            [EnumAltName("H")] Hard,
            [EnumAltName("L")] Lunatic
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
            [EnumAltName("RM")] Reimu,
            [EnumAltName("MR")] Marisa,
            [EnumAltName("IU")] IchirinUnzan,
            [EnumAltName("BY")] Byakuren,
            [EnumAltName("FT")] Futo,
            [EnumAltName("MK")] Miko,
            [EnumAltName("NT")] Nitori,
            [EnumAltName("KO")] Koishi,
            [EnumAltName("MM")] Mamizou,
            [EnumAltName("KK")] Kokoro
        }

        public override string SupportedVersions
        {
            get { return "1.34b"; }
        }

        public override bool HasCardReplacer
        {
            get { return false; }
        }

        protected override bool ReadScoreFile(Stream input)
        {
#if DEBUG
            using (var decoded = new FileStream("th135decoded.dat", FileMode.Create, FileAccess.ReadWrite))
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
                new ClearReplacer(this)
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

        // %T135CLEAR[x][yy]
        private class ClearReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T135CLEAR({0})({1})", LevelParser.Pattern, CharaParser.Pattern);

            private readonly MatchEvaluator evaluator;

            [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
            public ClearReplacer(Th135Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var level = LevelParser.Parse(match.Groups[1].Value);
                    var chara = CharaParser.Parse(match.Groups[2].Value);

                    var cleared = false;
                    var flags = LevelFlag.None;
                    if (parent.allScoreData.StoryClearFlags.TryGetValue((Chara)chara, out flags))
                    {
                        switch (level)
                        {
                            case Level.Easy:
                                cleared = (flags & LevelFlag.Easy) == LevelFlag.Easy;
                                break;
                            case Level.Normal:
                                cleared = (flags & LevelFlag.Normal) == LevelFlag.Normal;
                                break;
                            case Level.Hard:
                                cleared = (flags & LevelFlag.Hard) == LevelFlag.Hard;
                                break;
                            case Level.Lunatic:
                                cleared = (flags & LevelFlag.Lunatic) == LevelFlag.Lunatic;
                                break;
                            default:    // unreachable
                                break;
                        }
                    }

                    return cleared ? "Clear" : "Not Clear";
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
                    { 0x01000001, reader => new EndMark() },
                    { 0x01000008, reader => reader.ReadByte() != 0x00 },
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
                this.allData = new Dictionary<string, object>();
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
            public bool IsPlayableMamizou
            {
                get { return this.GetValue<bool>("enable_mamizou"); }
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public bool IsPlayableKokoro
            {
                get { return this.GetValue<bool>("enable_kokoro"); }
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public Dictionary<int, bool> BgmFlags { get; private set; }

            public static bool ReadObject(BinaryReader reader, out object obj)
            {
                if (reader is null)
                    throw new ArgumentNullException(nameof(reader));

                var type = reader.ReadUInt32();

                if (ObjectReaders.TryGetValue(type, out Func<BinaryReader, object> objectReader))
                    obj = objectReader(reader);
                else
                    throw new InvalidDataException("wrong type");

                return obj != null;
            }

            public void ReadFrom(BinaryReader reader)
            {
                if (DictionaryReader(reader) is Dictionary<object, object> dictionary)
                {
                    this.allData = dictionary
                        .Where(pair => pair.Key is string)
                        .ToDictionary(pair => pair.Key as string, pair => pair.Value);

                    if (this.allData.TryGetValue("story_clear", out object counts))
                    {
                        if (counts is object[] storyClearFlags)
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

                    if (this.allData.TryGetValue("enable_bgm", out object flags))
                    {
                        if (flags is Dictionary<object, object> bgmFlags)
                        {
                            this.BgmFlags = bgmFlags
                                .Where(pair => (pair.Key is int) && (pair.Value is bool))
                                .ToDictionary(pair => (int)pair.Key, pair => (bool)pair.Value);
                        }
                    }
                }
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            private T GetValue<T>(string key)
                where T : struct
            {
                if (this.allData.TryGetValue(key, out object value) && (value is T))
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

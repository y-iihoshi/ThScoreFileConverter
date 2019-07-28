//-----------------------------------------------------------------------
// <copyright file="Th135Converter.cs" company="None">
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
    using ThScoreFileConverter.Squirrel;

    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Reviewed.")]
    internal class Th135Converter : ThConverter
    {
        private static new readonly EnumShortNameParser<Level> LevelParser =
            new EnumShortNameParser<Level>();

        private static readonly EnumShortNameParser<Chara> CharaParser =
            new EnumShortNameParser<Chara>();

        private AllScoreData allScoreData = null;

        public new enum Level
        {
#pragma warning disable SA1134 // Attributes should not share line
            [EnumAltName("E")] Easy,
            [EnumAltName("N")] Normal,
            [EnumAltName("H")] Hard,
            [EnumAltName("L")] Lunatic,
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
        }

        public enum Chara
        {
#pragma warning disable SA1134 // Attributes should not share line
            [EnumAltName("RM")] Reimu,
            [EnumAltName("MR")] Marisa,
            [EnumAltName("IU")] IchirinUnzan,
            [EnumAltName("BY")] Byakuren,
            [EnumAltName("FT")] Futo,
            [EnumAltName("MK")] Miko,
            [EnumAltName("NT")] Nitori,
            [EnumAltName("KO")] Koishi,
            [EnumAltName("MM")] Mamizou,
            [EnumAltName("KK")] Kokoro,
#pragma warning restore SA1134 // Attributes should not share line
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
                new ClearReplacer(this),
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
            private SQTable allData;

            public AllScoreData()
            {
                this.allData = new SQTable();
                this.StoryClearFlags = null;
                this.BgmFlags = null;
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public int StoryProgress => this.GetValue<int>("story_progress");

            public Dictionary<Chara, LevelFlag> StoryClearFlags { get; private set; }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public int EndingCount => this.GetValue<int>("ed_count");

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public int Ending2Count => this.GetValue<int>("ed2_count");

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public bool IsEnabledStageTanuki1 => this.GetValue<bool>("enable_stage_tanuki1");

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public bool IsEnabledStageTanuki2 => this.GetValue<bool>("enable_stage_tanuki2");

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public bool IsEnabledStageKokoro => this.GetValue<bool>("enable_stage_kokoro");

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public bool IsPlayableMamizou => this.GetValue<bool>("enable_mamizou");

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public bool IsPlayableKokoro => this.GetValue<bool>("enable_kokoro");

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public Dictionary<int, bool> BgmFlags { get; private set; }

            public void ReadFrom(BinaryReader reader)
            {
                this.allData = SQTable.Create(reader, true);

                this.ParseStoryClear();
                this.ParseEnableBgm();
            }

            private void ParseStoryClear()
            {
                if (this.allData.Value.TryGetValue(new SQString("story_clear"), out var counts))
                {
                    if (counts is SQArray storyClearFlags)
                    {
                        this.StoryClearFlags = storyClearFlags.Value
                            .Select((flag, index) => (flag, index))
                            .Where(pair => pair.flag is SQInteger)
                            .ToDictionary(pair => (Chara)pair.index, pair => (LevelFlag)(int)(pair.flag as SQInteger));
                    }
                }
            }

            private void ParseEnableBgm()
            {
                if (this.allData.Value.TryGetValue(new SQString("enable_bgm"), out var flags))
                {
                    if (flags is SQTable bgmFlags)
                    {
                        this.BgmFlags = bgmFlags.Value
                            .Where(pair => (pair.Key is SQInteger) && (pair.Value is SQBool))
                            .ToDictionary(pair => (int)(pair.Key as SQInteger), pair => (bool)(pair.Value as SQBool));
                    }
                }
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            private T GetValue<T>(string key)
                where T : struct
            {
                T result = default;

                if (this.allData.Value.TryGetValue(new SQString(key), out var value))
                {
                    switch (value)
                    {
                        case SQBool sqbool:
                            if (result is bool)
                                result = (T)(object)(bool)sqbool;
                            break;
                        case SQInteger sqinteger:
                            if (result is int)
                                result = (T)(object)(int)sqinteger;
                            break;
                    }
                }

                return result;
            }
        }
    }
}

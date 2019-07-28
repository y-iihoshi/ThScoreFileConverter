//-----------------------------------------------------------------------
// <copyright file="Th155Converter.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable 1591
#pragma warning disable SA1600 // ElementsMustBeDocumented
#pragma warning disable SA1602 // EnumerationItemsMustBeDocumented

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;
using ThScoreFileConverter.Squirrel;

namespace ThScoreFileConverter.Models
{
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
            private SQTable allData;

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

            public void ReadFrom(BinaryReader reader)
            {
                this.allData = SQTable.Create(reader, true);

                this.ParseStoryDictionary();
                this.ParseCharacterDictionary();
                this.ParseBgmDictionary();
                this.ParseEndingDictionary();
                this.ParseStageDictionary();
                this.ParseVersion();
            }

            private static StoryChara? ParseStoryChara(SQObject obj)
            {
                if (obj is SQString str)
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

            private static Story ParseStory(SQObject obj)
            {
                var story = default(Story);

                if (obj is SQTable dict)
                {
                    foreach (var pair in dict.Value)
                    {
                        if (pair.Key is SQString key)
                        {
                            if ((key == "stage") && (pair.Value is SQInteger stage))
                                story.Stage = stage;
                            if ((key == "ed") && (pair.Value is SQInteger ed))
                                story.Ed = (LevelFlag)(int)ed;
                            if ((key == "available") && (pair.Value is SQBool available))
                                story.Available = available;
                            if ((key == "overdrive") && (pair.Value is SQInteger overDrive))
                                story.OverDrive = overDrive;
                            if ((key == "stage_overdrive") && (pair.Value is SQInteger stageOverDrive))
                                story.StageOverDrive = stageOverDrive;
                        }
                    }
                }

                return story;
            }

            private void ParseStoryDictionary()
            {
                if (this.allData.Value.TryGetValue(new SQString("story"), out var story))
                {
                    if (story is SQTable table)
                    {
                        this.StoryDictionary = table.Value
                            .Where(pair => ParseStoryChara(pair.Key) != null)
                            .ToDictionary(pair => ParseStoryChara(pair.Key).Value, pair => ParseStory(pair.Value));
                    }
                }
            }

            private void ParseCharacterDictionary()
            {
                if (this.allData.Value.TryGetValue(new SQString("character"), out var character))
                {
                    if (character is SQTable table)
                    {
                        this.CharacterDictionary = table.Value
                            .Where(pair => (pair.Key is SQString) && (pair.Value is SQInteger))
                            .ToDictionary(
                                pair => (string)(pair.Key as SQString),
                                pair => (int)(pair.Value as SQInteger));
                    }
                }
            }

            private void ParseBgmDictionary()
            {
                if (this.allData.Value.TryGetValue(new SQString("bgm"), out var bgm))
                {
                    if (bgm is SQTable table)
                    {
                        this.BgmDictionary = table.Value
                            .Where(pair => (pair.Key is SQInteger) && (pair.Value is SQBool))
                            .ToDictionary(pair => (int)(pair.Key as SQInteger), pair => (bool)(pair.Value as SQBool));
                    }
                }
            }

            private void ParseEndingDictionary()
            {
                if (this.allData.Value.TryGetValue(new SQString("ed"), out var ed))
                {
                    if (ed is SQTable table)
                    {
                        this.EndingDictionary = table.Value
                            .Where(pair => (pair.Key is SQString) && (pair.Value is SQInteger))
                            .ToDictionary(
                                pair => (string)(pair.Key as SQString),
                                pair => (int)(pair.Value as SQInteger));
                    }
                }
            }

            private void ParseStageDictionary()
            {
                if (this.allData.Value.TryGetValue(new SQString("stage"), out var stage))
                {
                    if (stage is SQTable table)
                    {
                        this.StageDictionary = table.Value
                            .Where(pair => (pair.Key is SQInteger) && (pair.Value is SQInteger))
                            .ToDictionary(pair => (int)(pair.Key as SQInteger), pair => (int)(pair.Value as SQInteger));
                    }
                }
            }

            private void ParseVersion() => this.Version = this.GetValue<int>("version");

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

            public struct Story
            {
                public int Stage;
                public LevelFlag Ed;
                public bool Available;
                public int OverDrive;
                public int StageOverDrive;
            }
        }
    }
}

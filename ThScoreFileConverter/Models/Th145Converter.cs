//-----------------------------------------------------------------------
// <copyright file="Th145Converter.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable 1591
#pragma warning disable SA1600 // ElementsMustBeDocumented

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;
using ThScoreFileConverter.Models.Th145;
using ThScoreFileConverter.Squirrel;

namespace ThScoreFileConverter.Models
{
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Reviewed.")]
    internal class Th145Converter : ThConverter
    {
        private AllScoreData allScoreData = null;

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
                new ClearTimeReplacer(this),
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

        // %T145CLEAR[x][yy]
        private class ClearRankReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T145CLEAR({0})({1})", Parsers.LevelParser.Pattern, Parsers.CharaParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public ClearRankReplacer(Th145Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var level = Parsers.LevelParser.Parse(match.Groups[1].Value);
                    var chara = Parsers.CharaParser.Parse(match.Groups[2].Value);

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
                @"%T145TIMECLR({0})({1})", Parsers.LevelWithTotalParser.Pattern, Parsers.CharaWithTotalParser.Pattern);

            private readonly MatchEvaluator evaluator;

            [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
            public ClearTimeReplacer(Th145Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var level = Parsers.LevelWithTotalParser.Parse(match.Groups[1].Value);
                    var chara = Parsers.CharaWithTotalParser.Parse(match.Groups[2].Value);

                    Func<IReadOnlyDictionary<Chara, int>, int> getValueByChara;
                    if (chara == CharaWithTotal.Total)
                        getValueByChara = (dict => dict.Values.Sum());
                    else
                        getValueByChara = (dict => dict[(Chara)chara]);

                    Func<IReadOnlyDictionary<Th145.Level, IReadOnlyDictionary<Chara, int>>, int> getValueByLevel;
                    if (level == Th145.LevelWithTotal.Total)
                        getValueByLevel = (dict => dict.Values.Sum(getValueByChara));
                    else
                        getValueByLevel = (dict => getValueByChara(dict[(Th145.Level)level]));

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
            private SQTable allData;

            public AllScoreData()
            {
                this.allData = new SQTable();
                this.StoryClearFlags = null;
                this.BgmFlags = null;
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public int StoryProgress => this.GetValue<int>("story_progress");

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public IReadOnlyDictionary<Chara, LevelFlags> StoryClearFlags { get; private set; }

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
            public bool IsEnabledSt27 => this.GetValue<bool>("enable_st27");

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public bool IsEnabledSt28 => this.GetValue<bool>("enable_st28");

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public bool IsPlayableMamizou => this.GetValue<bool>("enable_mamizou");

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public bool IsPlayableKokoro => this.GetValue<bool>("enable_kokoro");

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public IReadOnlyDictionary<int, bool> BgmFlags { get; private set; }

            public IReadOnlyDictionary<Th145.Level, IReadOnlyDictionary<Chara, int>> ClearRanks { get; private set; }

            public IReadOnlyDictionary<Th145.Level, IReadOnlyDictionary<Chara, int>> ClearTimes { get; private set; }

            public void ReadFrom(BinaryReader reader)
            {
                this.allData = SQTable.Create(reader, true);

                this.ParseStoryClear();
                this.ParseEnableBgm();
                this.ParseClearRank();
                this.ParseClearTime();
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
                            .ToDictionary(pair => (Chara)pair.index, pair => (LevelFlags)(int)(pair.flag as SQInteger));
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

            private void ParseClearRank()
            {
                if (this.allData.Value.TryGetValue(new SQString("clear_rank"), out var ranks))
                {
                    if (ranks is SQArray clearRanks)
                    {
                        this.ClearRanks = clearRanks.Value
                            .Select((ranksPerChara, index) => (ranksPerChara, index))
                            .Where(levelPair => levelPair.ranksPerChara is SQArray)
                            .ToDictionary(
                                levelPair => (Th145.Level)levelPair.index,
                                levelPair => (levelPair.ranksPerChara as SQArray).Value
                                    .Select((rank, index) => (rank, index))
                                    .Where(charaPair => charaPair.rank is SQInteger)
                                    .ToDictionary(
                                        charaPair => (Chara)charaPair.index,
                                        charaPair => (int)(charaPair.rank as SQInteger))
                                    as IReadOnlyDictionary<Chara, int>);
                    }
                }
            }

            private void ParseClearTime()
            {
                if (this.allData.Value.TryGetValue(new SQString("clear_time"), out var times))
                {
                    if (times is SQArray clearTimes)
                    {
                        this.ClearTimes = clearTimes.Value
                            .Select((timesPerChara, index) => (timesPerChara, index))
                            .Where(levelPair => levelPair.timesPerChara is SQArray)
                            .ToDictionary(
                                levelPair => (Th145.Level)levelPair.index,
                                levelPair => (levelPair.timesPerChara as SQArray).Value
                                    .Select((time, index) => (time, index))
                                    .Where(charaPair => charaPair.time is SQInteger)
                                    .ToDictionary(
                                        charaPair => (Chara)charaPair.index,
                                        charaPair => (int)(charaPair.time as SQInteger))
                                    as IReadOnlyDictionary<Chara, int>);
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

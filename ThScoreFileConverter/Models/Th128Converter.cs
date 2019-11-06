//-----------------------------------------------------------------------
// <copyright file="Th128Converter.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable 1591
#pragma warning disable SA1600 // ElementsMustBeDocumented

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Models.Th128;

namespace ThScoreFileConverter.Models
{
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Reviewed.")]
    internal class Th128Converter : ThConverter
    {
        private AllScoreData allScoreData = null;

        public override string SupportedVersions
        {
            get { return "1.00a"; }
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

        protected override IEnumerable<IStringReplaceable> CreateReplacers(
            bool hideUntriedCards, string outputFilePath)
        {
            return new List<IStringReplaceable>
            {
                new ScoreReplacer(this),
                new CareerReplacer(this),
                new CardReplacer(this, hideUntriedCards),
                new CollectRateReplacer(this),
                new ClearReplacer(this),
                new RouteReplacer(this),
                new RouteExReplacer(this),
                new TimeReplacer(this),
            };
        }

        private static bool Decrypt(Stream input, Stream output)
        {
            using (var reader = new BinaryReader(input, Encoding.UTF8, true))
            using (var writer = new BinaryWriter(output, Encoding.UTF8, true))
            {
                var header = new Header();
                header.ReadFrom(reader);
                if (!header.IsValid)
                    return false;
                if (header.EncodedAllSize != reader.BaseStream.Length)
                    return false;

                header.WriteTo(writer);
                ThCrypt.Decrypt(input, output, header.EncodedBodySize, 0xAC, 0x35, 0x10, header.EncodedBodySize);

                return true;
            }
        }

        private static bool Extract(Stream input, Stream output)
        {
            using (var reader = new BinaryReader(input, Encoding.UTF8, true))
            using (var writer = new BinaryWriter(output, Encoding.UTF8, true))
            {
                var header = new Header();
                header.ReadFrom(reader);
                header.WriteTo(writer);

                var bodyBeginPos = output.Position;
                Lzss.Extract(input, output);
                output.Flush();
                output.SetLength(output.Position);

                return header.DecodedBodySize == (output.Position - bodyBeginPos);
            }
        }

        private static bool Validate(Stream input)
        {
            using (var reader = new BinaryReader(input, Encoding.UTF8, true))
            {
                var header = new Header();
                header.ReadFrom(reader);
                var remainSize = header.DecodedBodySize;
                var chapter = new Th10.Chapter();

                try
                {
                    while (remainSize > 0)
                    {
                        chapter.ReadFrom(reader);
                        if (!chapter.IsValid)
                            return false;
                        if (!ClearData.CanInitialize(chapter) &&
                            !CardData.CanInitialize(chapter) &&
                            !Status.CanInitialize(chapter))
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
        }

        private static AllScoreData Read(Stream input)
        {
            var dictionary = new Dictionary<string, Action<AllScoreData, Th10.Chapter>>
            {
                { ClearData.ValidSignature, (data, ch) => data.Set(new ClearData(ch)) },
                { CardData.ValidSignature,  (data, ch) => data.Set(new CardData(ch))  },
                { Status.ValidSignature,    (data, ch) => data.Set(new Status(ch))    },
            };

            using (var reader = new BinaryReader(input, Encoding.UTF8, true))
            {
                var allScoreData = new AllScoreData();
                var chapter = new Th10.Chapter();

                var header = new Header();
                header.ReadFrom(reader);
                allScoreData.Set(header);

                try
                {
                    while (true)
                    {
                        chapter.ReadFrom(reader);
                        if (dictionary.TryGetValue(chapter.Signature, out var setChapter))
                            setChapter(allScoreData, chapter);
                    }
                }
                catch (EndOfStreamException)
                {
                    // It's OK, do nothing.
                }

                if ((allScoreData.Header != null) &&
                    (allScoreData.ClearData.Count == Enum.GetValues(typeof(RouteWithTotal)).Length) &&
                    (allScoreData.CardData != null) &&
                    (allScoreData.Status != null))
                    return allScoreData;
                else
                    return null;
            }
        }

        // %T128SCR[w][xx][y][z]
        private class ScoreReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T128SCR({0})({1})(\d)([1-5])", Parsers.LevelParser.Pattern, Parsers.RouteParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public ScoreReplacer(Th128Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var level = Parsers.LevelParser.Parse(match.Groups[1].Value);
                    var route = (RouteWithTotal)Parsers.RouteParser.Parse(match.Groups[2].Value);
                    var rank = Utils.ToZeroBased(
                        int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture));
                    var type = int.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture);

                    if ((level == Level.Extra) && (route != RouteWithTotal.Extra))
                        return match.ToString();
                    if ((route == RouteWithTotal.Extra) && (level != Level.Extra))
                        return match.ToString();

                    var ranking = parent.allScoreData.ClearData[route].Rankings[level][rank];
                    switch (type)
                    {
                        case 1:     // name
                            return Encoding.Default.GetString(ranking.Name.ToArray()).Split('\0')[0];
                        case 2:     // score
                            return Utils.ToNumberString((ranking.Score * 10) + ranking.ContinueCount);
                        case 3:     // stage
                            if (ranking.DateTime == 0)
                                return StageProgress.None.ToShortName();
                            return ranking.StageProgress.ToShortName();
                        case 4:     // date & time
                            if (ranking.DateTime == 0)
                                return "----/--/-- --:--:--";
                            return new DateTime(1970, 1, 1).AddSeconds(ranking.DateTime).ToLocalTime()
                                .ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.CurrentCulture);
                        case 5:     // slow
                            if (ranking.DateTime == 0)
                                return "-----%";
                            return Utils.Format("{0:F3}%", ranking.SlowRate);
                        default:    // unreachable
                            return match.ToString();
                    }
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        // %T128C[xxx][z]
        private class CareerReplacer : IStringReplaceable
        {
            private const string Pattern = @"%T128C(\d{3})([1-3])";

            private readonly MatchEvaluator evaluator;

            [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
            public CareerReplacer(Th128Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var number = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
                    var type = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);

                    Func<ISpellCard, int> getCount;
                    if (type == 1)
                        getCount = (card => card.NoIceCount);
                    else if (type == 2)
                        getCount = (card => card.NoMissCount);
                    else
                        getCount = (card => card.TrialCount);

                    if (number == 0)
                    {
                        return Utils.ToNumberString(
                            parent.allScoreData.CardData.Cards.Values.Sum(getCount));
                    }
                    else if (Definitions.CardTable.ContainsKey(number))
                    {
                        if (parent.allScoreData.CardData.Cards.TryGetValue(number, out var card))
                            return Utils.ToNumberString(getCount(card));
                        else
                            return "0";
                    }
                    else
                    {
                        return match.ToString();
                    }
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        // %T128CARD[xxx][y]
        private class CardReplacer : IStringReplaceable
        {
            private const string Pattern = @"%T128CARD(\d{3})([NR])";

            private readonly MatchEvaluator evaluator;

            public CardReplacer(Th128Converter parent, bool hideUntriedCards)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var number = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
                    var type = match.Groups[2].Value.ToUpperInvariant();

                    if (Definitions.CardTable.ContainsKey(number))
                    {
                        if (type == "N")
                        {
                            if (hideUntriedCards)
                            {
                                var cards = parent.allScoreData.CardData.Cards;
                                if (!cards.TryGetValue(number, out var card) || !card.HasTried())
                                    return "??????????";
                            }

                            return Definitions.CardTable[number].Name;
                        }
                        else
                        {
                            return Definitions.CardTable[number].Level.ToString();
                        }
                    }
                    else
                    {
                        return match.ToString();
                    }
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        // %T128CRG[x][yyy][z]
        private class CollectRateReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T128CRG({0})({1})([1-3])",
                Parsers.LevelWithTotalParser.Pattern,
                Parsers.StageWithTotalParser.Pattern);

            private readonly MatchEvaluator evaluator;

            [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
            public CollectRateReplacer(Th128Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var level = Parsers.LevelWithTotalParser.Parse(match.Groups[1].Value);
                    var stage = Parsers.StageWithTotalParser.Parse(match.Groups[2].Value);
                    var type = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

                    if (stage == Th128.StageWithTotal.Extra)
                        return match.ToString();

                    Func<ISpellCard, bool> findByStage;
                    if (stage == Th128.StageWithTotal.Total)
                        findByStage = (card => true);
                    else
                        findByStage = (card => Definitions.CardTable[card.Id].Stage == (Th128.Stage)stage);

                    Func<ISpellCard, bool> findByLevel = (card => true);
                    switch (level)
                    {
                        case LevelWithTotal.Total:
                            // Do nothing
                            break;
                        case LevelWithTotal.Extra:
                            findByStage = (card => Definitions.CardTable[card.Id].Stage == Th128.Stage.Extra);
                            break;
                        default:
                            findByLevel = (card => card.Level == (Level)level);
                            break;
                    }

                    Func<ISpellCard, bool> findByType;
                    if (type == 1)
                        findByType = (card => card.NoIceCount > 0);
                    else if (type == 2)
                        findByType = (card => card.NoMissCount > 0);
                    else
                        findByType = (card => card.TrialCount > 0);

                    return parent.allScoreData.CardData.Cards.Values
                        .Count(Utils.MakeAndPredicate(findByLevel, findByStage, findByType))
                        .ToString(CultureInfo.CurrentCulture);
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        // %T128CLEAR[x][yy]
        private class ClearReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T128CLEAR({0})({1})", Parsers.LevelParser.Pattern, Parsers.RouteParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public ClearReplacer(Th128Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var level = Parsers.LevelParser.Parse(match.Groups[1].Value);
                    var route = (RouteWithTotal)Parsers.RouteParser.Parse(match.Groups[2].Value);

                    if ((level == Level.Extra) && (route != RouteWithTotal.Extra))
                        return match.ToString();
                    if ((route == RouteWithTotal.Extra) && (level != Level.Extra))
                        return match.ToString();

                    var rankings = parent.allScoreData.ClearData[route].Rankings[level]
                        .Where(ranking => ranking.DateTime > 0);
                    var stageProgress = rankings.Any()
                        ? rankings.Max(ranking => ranking.StageProgress) : StageProgress.None;

                    return stageProgress.ToShortName();
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        // %T128ROUTE[xx][y]
        private class RouteReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T128ROUTE({0})([1-3])", Parsers.RouteWithTotalParser.Pattern);

            private readonly MatchEvaluator evaluator;

            [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
            public RouteReplacer(Th128Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var route = Parsers.RouteWithTotalParser.Parse(match.Groups[1].Value);
                    var type = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);

                    Func<IClearData, long> getValueByType;
                    Func<long, string> toString;
                    if (type == 1)
                    {
                        getValueByType = (data => data.TotalPlayCount);
                        toString = Utils.ToNumberString;
                    }
                    else if (type == 2)
                    {
                        getValueByType = (data => data.PlayTime);
                        toString = (value => new Time(value).ToString());
                    }
                    else
                    {
                        getValueByType = (data => data.ClearCounts.Values.Sum());
                        toString = Utils.ToNumberString;
                    }

                    Func<AllScoreData, long> getValueByRoute;
                    if (route == RouteWithTotal.Total)
                    {
                        getValueByRoute = (allData => allData.ClearData.Values
                            .Where(data => data.Route != route).Sum(getValueByType));
                    }
                    else
                    {
                        getValueByRoute = (allData => getValueByType(allData.ClearData[route]));
                    }

                    return toString(getValueByRoute(parent.allScoreData));
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        // %T128ROUTEEX[x][yy][z]
        private class RouteExReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T128ROUTEEX({0})({1})([1-3])",
                Parsers.LevelWithTotalParser.Pattern,
                Parsers.RouteWithTotalParser.Pattern);

            private readonly MatchEvaluator evaluator;

            [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
            public RouteExReplacer(Th128Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var level = Parsers.LevelWithTotalParser.Parse(match.Groups[1].Value);
                    var route = Parsers.RouteWithTotalParser.Parse(match.Groups[2].Value);
                    var type = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

                    if ((level == LevelWithTotal.Extra) &&
                        ((route != RouteWithTotal.Extra) && (route != RouteWithTotal.Total)))
                        return match.ToString();
                    if ((route == RouteWithTotal.Extra) &&
                        ((level != LevelWithTotal.Extra) && (level != LevelWithTotal.Total)))
                        return match.ToString();

                    Func<IClearData, long> getValueByType;
                    Func<long, string> toString;
                    if (type == 1)
                    {
                        getValueByType = (data => data.TotalPlayCount);
                        toString = Utils.ToNumberString;
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
                        toString = Utils.ToNumberString;
                    }

                    Func<AllScoreData, long> getValueByRoute;
                    if (route == RouteWithTotal.Total)
                    {
                        getValueByRoute = (allData => allData.ClearData.Values
                            .Where(data => data.Route != route).Sum(getValueByType));
                    }
                    else
                    {
                        getValueByRoute = (allData => getValueByType(allData.ClearData[route]));
                    }

                    return toString(getValueByRoute(parent.allScoreData));
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        // %T128TIMEPLY
        private class TimeReplacer : IStringReplaceable
        {
            private const string Pattern = @"%T128TIMEPLY";

            private readonly MatchEvaluator evaluator;

            public TimeReplacer(Th128Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    return new Time(parent.allScoreData.Status.TotalPlayTime * 10, false).ToLongString();
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        private class AllScoreData
        {
            private readonly Dictionary<RouteWithTotal, IClearData> clearData;

            public AllScoreData()
            {
                this.clearData =
                    new Dictionary<RouteWithTotal, IClearData>(Enum.GetValues(typeof(RouteWithTotal)).Length);
            }

            public Th095.HeaderBase Header { get; private set; }

            public IReadOnlyDictionary<RouteWithTotal, IClearData> ClearData => this.clearData;

            public ICardData CardData { get; private set; }

            public Th125.IStatus Status { get; private set; }

            public void Set(Th095.HeaderBase header) => this.Header = header;

            public void Set(IClearData data)
            {
                if (!this.clearData.ContainsKey(data.Route))
                    this.clearData.Add(data.Route, data);
            }

            public void Set(ICardData data) => this.CardData = data;

            public void Set(Th125.IStatus status) => this.Status = status;
        }

        private class Status : StatusBase
        {
            public const ushort ValidVersion = 0x0002;

            public Status(Th10.Chapter chapter)
                : base(chapter, ValidVersion, 10, 0x18)
            {
            }

            public static new bool CanInitialize(Th10.Chapter chapter)
                => StatusBase.CanInitialize(chapter) && (chapter.Version == ValidVersion);
        }
    }
}

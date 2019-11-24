//-----------------------------------------------------------------------
// <copyright file="Th15Converter.cs" company="None">
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
using ThScoreFileConverter.Models.Th15;

namespace ThScoreFileConverter.Models
{
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Reviewed.")]
    internal class Th15Converter : ThConverter
    {
        private AllScoreData allScoreData = null;

        public override string SupportedVersions
        {
            get { return "1.00b"; }
        }

        protected override bool ReadScoreFile(Stream input)
        {
            using (var decrypted = new MemoryStream())
#if DEBUG
            using (var decoded = new FileStream("th15decoded.dat", FileMode.Create, FileAccess.ReadWrite))
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
                new ScoreReplacer(this.allScoreData.ClearData),
                new CareerReplacer(this.allScoreData.ClearData),
                new CardReplacer(this.allScoreData.ClearData, hideUntriedCards),
                new CollectRateReplacer(this.allScoreData.ClearData),
                new ClearReplacer(this.allScoreData.ClearData),
                new CharaReplacer(this.allScoreData.ClearData),
                new CharaExReplacer(this.allScoreData.ClearData),
                new PracticeReplacer(this.allScoreData.ClearData),
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
                        if (!ClearData.CanInitialize(chapter) && !Th13.Status.CanInitialize(chapter))
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
                { ClearData.ValidSignature,   (data, ch) => data.Set(new ClearData(ch))   },
                { Th13.Status.ValidSignature, (data, ch) => data.Set(new Th13.Status(ch)) },
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
                    (allScoreData.ClearData.Count == Enum.GetValues(typeof(CharaWithTotal)).Length) &&
                    (allScoreData.Status != null))
                    return allScoreData;
                else
                    return null;
            }
        }

        // %T15SCR[v][w][xx][y][z]
        private class ScoreReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T15SCR({0})({1})({2})(\d)([1-6])",
                Parsers.GameModeParser.Pattern,
                Parsers.LevelParser.Pattern,
                Parsers.CharaParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public ScoreReplacer(IReadOnlyDictionary<CharaWithTotal, IClearData> clearDataDictionary)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var mode = Parsers.GameModeParser.Parse(match.Groups[1].Value);
                    var level = (LevelWithTotal)Parsers.LevelParser.Parse(match.Groups[2].Value);
                    var chara = (CharaWithTotal)Parsers.CharaParser.Parse(match.Groups[3].Value);
                    var rank = Utils.ToZeroBased(
                        int.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture));
                    var type = int.Parse(match.Groups[5].Value, CultureInfo.InvariantCulture);

#if false   // FIXME
                    if (level == LevelWithTotal.Extra)
                        mode = GameMode.Pointdevice;
#endif

                    var ranking = clearDataDictionary[chara].GameModeData[mode].Rankings[level][rank];
                    switch (type)
                    {
                        case 1:     // name
                            return Encoding.Default.GetString(ranking.Name.ToArray()).Split('\0')[0];
                        case 2:     // score
                            return Utils.ToNumberString((ranking.Score * 10) + ranking.ContinueCount);
                        case 3:     // stage
                            if (ranking.DateTime == 0)
                                return Th13.StageProgress.None.ToShortName();
                            if (ranking.StageProgress == Th13.StageProgress.Extra)
                                return "Not Clear";
                            if (ranking.StageProgress == Th13.StageProgress.ExtraClear)
                                return Th13.StageProgress.Clear.ToShortName();
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
                        case 6:     // retry
                            if (ranking.DateTime == 0)
                                return "-----";
                            return Utils.ToNumberString(ranking.RetryCount);
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

        // %T15C[w][xxx][yy][z]
        private class CareerReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T15C({0})(\d{{3}})({1})([12])",
                Parsers.GameModeParser.Pattern,
                Parsers.CharaWithTotalParser.Pattern);

            private readonly MatchEvaluator evaluator;

            [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
            public CareerReplacer(IReadOnlyDictionary<CharaWithTotal, IClearData> clearDataDictionary)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var mode = Parsers.GameModeParser.Parse(match.Groups[1].Value);
                    var number = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);
                    var chara = Parsers.CharaWithTotalParser.Parse(match.Groups[3].Value);
                    var type = int.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture);

                    Func<Th13.ISpellCard<Level>, int> getCount;
                    if (type == 1)
                        getCount = (card => card.ClearCount);
                    else
                        getCount = (card => card.TrialCount);

                    var cards = clearDataDictionary[chara].GameModeData[mode].Cards;
                    if (number == 0)
                    {
                        return Utils.ToNumberString(cards.Values.Sum(getCount));
                    }
                    else if (Definitions.CardTable.ContainsKey(number))
                    {
                        if (cards.TryGetValue(number, out var card))
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

        // %T15CARD[xxx][y]
        private class CardReplacer : IStringReplaceable
        {
            private const string Pattern = @"%T15CARD(\d{3})([NR])";

            private readonly MatchEvaluator evaluator;

            public CardReplacer(
                IReadOnlyDictionary<CharaWithTotal, IClearData> clearDataDictionary, bool hideUntriedCards)
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
                                var tried = clearDataDictionary[CharaWithTotal.Total].GameModeData.Any(
                                    data => data.Value.Cards.TryGetValue(number, out var card) && card.HasTried);
                                if (!tried)
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

        // %T15CRG[v][w][xx][y][z]
        private class CollectRateReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T15CRG({0})({1})({2})({3})([12])",
                Parsers.GameModeParser.Pattern,
                Parsers.LevelWithTotalParser.Pattern,
                Parsers.CharaWithTotalParser.Pattern,
                Parsers.StageWithTotalParser.Pattern);

            private readonly MatchEvaluator evaluator;

            [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
            public CollectRateReplacer(IReadOnlyDictionary<CharaWithTotal, IClearData> clearDataDictionary)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var mode = Parsers.GameModeParser.Parse(match.Groups[1].Value);
                    var level = Parsers.LevelWithTotalParser.Parse(match.Groups[2].Value);
                    var chara = Parsers.CharaWithTotalParser.Parse(match.Groups[3].Value);
                    var stage = Parsers.StageWithTotalParser.Parse(match.Groups[4].Value);
                    var type = int.Parse(match.Groups[5].Value, CultureInfo.InvariantCulture);

                    if (stage == StageWithTotal.Extra)
                        return match.ToString();

#if false   // FIXME
                    if (level == LevelWithTotal.Extra)
                        mode = GameMode.Pointdevice;
#endif

                    Func<Th13.ISpellCard<Level>, bool> findByType;
                    if (type == 1)
                        findByType = (card => card.ClearCount > 0);
                    else
                        findByType = (card => card.TrialCount > 0);

                    Func<Th13.ISpellCard<Level>, bool> findByStage;
                    if (stage == StageWithTotal.Total)
                        findByStage = (card => true);
                    else
                        findByStage = (card => Definitions.CardTable[card.Id].Stage == (Stage)stage);

                    Func<Th13.ISpellCard<Level>, bool> findByLevel = (card => true);
                    switch (level)
                    {
                        case LevelWithTotal.Total:
                            // Do nothing
                            break;
                        case LevelWithTotal.Extra:
                            findByStage = (card => Definitions.CardTable[card.Id].Stage == Stage.Extra);
                            break;
                        default:
                            findByLevel = (card => card.Level == (Level)level);
                            break;
                    }

                    return clearDataDictionary[chara].GameModeData[mode].Cards.Values
                        .Count(Utils.MakeAndPredicate(findByType, findByLevel, findByStage))
                        .ToString(CultureInfo.CurrentCulture);
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        // %T15CLEAR[x][y][zz]
        private class ClearReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T15CLEAR({0})({1})({2})",
                Parsers.GameModeParser.Pattern,
                Parsers.LevelParser.Pattern,
                Parsers.CharaParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public ClearReplacer(IReadOnlyDictionary<CharaWithTotal, IClearData> clearDataDictionary)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var mode = Parsers.GameModeParser.Parse(match.Groups[1].Value);
                    var level = (LevelWithTotal)Parsers.LevelParser.Parse(match.Groups[2].Value);
                    var chara = (CharaWithTotal)Parsers.CharaParser.Parse(match.Groups[3].Value);

#if false   // FIXME
                    if (level == LevelWithTotal.Extra)
                        mode = GameMode.Pointdevice;
#endif

                    var rankings = clearDataDictionary[chara].GameModeData[mode].Rankings[level]
                        .Where(ranking => ranking.DateTime > 0);
                    var stageProgress = rankings.Any()
                        ? rankings.Max(ranking => ranking.StageProgress) : Th13.StageProgress.None;

                    if (stageProgress == Th13.StageProgress.Extra)
                        return "Not Clear";
                    else if (stageProgress == Th13.StageProgress.ExtraClear)
                        return Th13.StageProgress.Clear.ToShortName();
                    else
                        return stageProgress.ToShortName();
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        // %T15CHARA[x][yy][z]
        private class CharaReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T15CHARA({0})({1})([1-3])", Parsers.GameModeParser.Pattern, Parsers.CharaWithTotalParser.Pattern);

            private readonly MatchEvaluator evaluator;

            [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
            public CharaReplacer(IReadOnlyDictionary<CharaWithTotal, IClearData> clearDataDictionary)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var mode = Parsers.GameModeParser.Parse(match.Groups[1].Value);
                    var chara = Parsers.CharaWithTotalParser.Parse(match.Groups[2].Value);
                    var type = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

                    Func<IClearData, long> getValueByType;
                    Func<long, string> toString;
                    if (type == 1)
                    {
                        getValueByType = (clearData => clearData.GameModeData[mode].TotalPlayCount);
                        toString = Utils.ToNumberString;
                    }
                    else if (type == 2)
                    {
                        getValueByType = (clearData => clearData.GameModeData[mode].PlayTime);
                        toString = (value => new Time(value * 10, false).ToString());
                    }
                    else
                    {
                        getValueByType = (clearData => clearData.GameModeData[mode].ClearCounts.Values.Sum());
                        toString = Utils.ToNumberString;
                    }

                    Func<IReadOnlyDictionary<CharaWithTotal, IClearData>, long> getValueByChara;
                    if (chara == CharaWithTotal.Total)
                    {
                        getValueByChara = (dictionary => dictionary.Values
                            .Where(clearData => clearData.Chara != chara).Sum(getValueByType));
                    }
                    else
                    {
                        getValueByChara = (dictionary => getValueByType(dictionary[chara]));
                    }

                    return toString(getValueByChara(clearDataDictionary));
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        // %T15CHARAEX[w][x][yy][z]
        private class CharaExReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T15CHARAEX({0})({1})({2})([1-3])",
                Parsers.GameModeParser.Pattern,
                Parsers.LevelWithTotalParser.Pattern,
                Parsers.CharaWithTotalParser.Pattern);

            private readonly MatchEvaluator evaluator;

            [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
            public CharaExReplacer(IReadOnlyDictionary<CharaWithTotal, IClearData> clearDataDictionary)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var mode = Parsers.GameModeParser.Parse(match.Groups[1].Value);
                    var level = Parsers.LevelWithTotalParser.Parse(match.Groups[2].Value);
                    var chara = Parsers.CharaWithTotalParser.Parse(match.Groups[3].Value);
                    var type = int.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture);

                    Func<IClearData, long> getValueByType;
                    Func<long, string> toString;
                    if (type == 1)
                    {
                        getValueByType = (clearData => clearData.GameModeData[mode].TotalPlayCount);
                        toString = Utils.ToNumberString;
                    }
                    else if (type == 2)
                    {
                        getValueByType = (clearData => clearData.GameModeData[mode].PlayTime);
                        toString = (value => new Time(value * 10, false).ToString());
                    }
                    else
                    {
                        if (level == LevelWithTotal.Total)
                            getValueByType = (clearData => clearData.GameModeData[mode].ClearCounts.Values.Sum());
                        else
                            getValueByType = (clearData => clearData.GameModeData[mode].ClearCounts[level]);
                        toString = Utils.ToNumberString;
                    }

                    Func<IReadOnlyDictionary<CharaWithTotal, IClearData>, long> getValueByChara;
                    if (chara == CharaWithTotal.Total)
                    {
                        getValueByChara = (dictionary => dictionary.Values
                            .Where(clearData => clearData.Chara != chara).Sum(getValueByType));
                    }
                    else
                    {
                        getValueByChara = (dictionary => getValueByType(dictionary[chara]));
                    }

                    return toString(getValueByChara(clearDataDictionary));
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        // %T15PRAC[x][yy][z]
        private class PracticeReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T15PRAC({0})({1})({2})",
                Parsers.LevelParser.Pattern,
                Parsers.CharaParser.Pattern,
                Parsers.StageParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public PracticeReplacer(IReadOnlyDictionary<CharaWithTotal, IClearData> clearDataDictionary)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var level = Parsers.LevelParser.Parse(match.Groups[1].Value);
                    var chara = (CharaWithTotal)Parsers.CharaParser.Parse(match.Groups[2].Value);
                    var stage = Parsers.StageParser.Parse(match.Groups[3].Value);

                    if (level == Level.Extra)
                        return match.ToString();
                    if (stage == Stage.Extra)
                        return match.ToString();

                    if (clearDataDictionary.ContainsKey(chara))
                    {
                        var key = (level, (StagePractice)stage);
                        var practices = clearDataDictionary[chara].Practices;
                        return practices.ContainsKey(key)
                            ? Utils.ToNumberString(practices[key].Score * 10) : "0";
                    }
                    else
                    {
                        return "0";
                    }
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }
    }
}

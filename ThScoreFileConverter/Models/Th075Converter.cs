//-----------------------------------------------------------------------
// <copyright file="Th075Converter.cs" company="None">
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
using ThScoreFileConverter.Models.Th075;

namespace ThScoreFileConverter.Models
{
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Reviewed.")]
    internal class Th075Converter : ThConverter
    {
        private AllScoreData allScoreData = null;

        public override string SupportedVersions
        {
            get { return "1.11"; }
        }

        protected override bool ReadScoreFile(Stream input)
        {
#if DEBUG
            using (var decoded = new FileStream("th075decoded.dat", FileMode.Create, FileAccess.ReadWrite))
            {
                var size = (int)input.Length;
                var data = new byte[size];
                input.Read(data, 0, size);
                decoded.Write(data, 0, size);
                decoded.Flush();
                decoded.SetLength(decoded.Position);
            }
#endif

            input.Seek(0, SeekOrigin.Begin);
            this.allScoreData = Read(input);

            return this.allScoreData != null;
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
                new CharaReplacer(this),
            };
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

                var numPairs = Enum.GetValues(typeof(Chara)).Length * Enum.GetValues(typeof(Th075.Level)).Length;
                if ((allScoreData.ClearData.Count == numPairs) &&
                    (allScoreData.Status != null))
                    return allScoreData;
                else
                    return null;
            }
        }

        // %T75SCR[w][xx][y][z]
        private class ScoreReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T75SCR({0})({1})(\d)([1-3])", Parsers.LevelParser.Pattern, Parsers.CharaParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public ScoreReplacer(Th075Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var level = Parsers.LevelParser.Parse(match.Groups[1].Value);
                    var chara = Parsers.CharaParser.Parse(match.Groups[2].Value);
                    var rank = Utils.ToZeroBased(
                        int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture));
                    var type = int.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture);

                    if (chara == Chara.Meiling)
                        return match.ToString();

                    var score = parent.allScoreData.ClearData[(chara, level)].Ranking[rank];

                    switch (type)
                    {
                        case 1:     // name
                            return score.Name;
                        case 2:     // score
                            return Utils.ToNumberString(score.Score);
                        case 3:     // date
                            return Utils.Format("{0:D2}/{1:D2}", score.Month, score.Day);
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

        // %T75C[xxx][yy][z]
        private class CareerReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T75C(\d{{3}})({0})([1-4])", Parsers.CharaParser.Pattern);

            private readonly MatchEvaluator evaluator;

            [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
            public CareerReplacer(Th075Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var number = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
                    var chara = Parsers.CharaParser.Parse(match.Groups[2].Value);
                    var type = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

                    if (chara == Chara.Meiling)
                        return match.ToString();

                    if (type == 4)
                    {
                        if ((number > 0) && (number <= Definitions.CardIdTable[chara].Count()))
                        {
                            return parent.allScoreData.ClearData.Where(pair => pair.Key.chara == chara)
                                .Any(pair => pair.Value.CardTrulyGot[number - 1] != 0x00) ? "★" : string.Empty;
                        }
                        else
                        {
                            return match.ToString();
                        }
                    }

                    Func<short, int> toInteger = (value => (int)value);
                    Func<ClearData, IEnumerable<int>> getValues;
                    if (type == 1)
                        getValues = (data => data.MaxBonuses);
                    else if (type == 2)
                        getValues = (data => data.CardGotCount.Select(toInteger));
                    else
                        getValues = (data => data.CardTrialCount.Select(toInteger));

                    if (number == 0)
                    {
                        return Utils.ToNumberString(
                            parent.allScoreData.ClearData.Where(pair => pair.Key.chara == chara)
                                .Sum(pair => getValues(pair.Value).Sum()));
                    }
                    else if (number <= Definitions.CardIdTable[chara].Count())
                    {
                        return Utils.ToNumberString(
                            parent.allScoreData.ClearData.Where(pair => pair.Key.chara == chara)
                                .Sum(pair => getValues(pair.Value).ElementAt(number - 1)));
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

        // %T75CARD[xxx][yy][z]
        private class CardReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T75CARD(\d{{3}})({0})([NR])", Parsers.CharaParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public CardReplacer(Th075Converter parent, bool hideUntriedCards)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var number = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
                    var chara = Parsers.CharaParser.Parse(match.Groups[2].Value);
                    var type = match.Groups[3].Value.ToUpperInvariant();

                    if (chara == Chara.Meiling)
                        return match.ToString();

                    if ((number > 0) && (number <= Definitions.CardIdTable[chara].Count()))
                    {
                        if (hideUntriedCards)
                        {
                            var dataList = parent.allScoreData.ClearData
                                .Where(pair => pair.Key.chara == chara).Select(pair => pair.Value);
                            if (dataList.All(data => data.CardTrialCount[number - 1] <= 0))
                                return (type == "N") ? "??????????" : "?????";
                        }

                        var cardId = Definitions.CardIdTable[chara].ElementAt(number - 1);
                        return (type == "N")
                            ? Definitions.CardTable[cardId].Name : Definitions.CardTable[cardId].Level.ToString();
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

        // %T75CRG[x][yy][z]
        private class CollectRateReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T75CRG({0})({1})([1-3])", Parsers.LevelWithTotalParser.Pattern, Parsers.CharaParser.Pattern);

            private readonly MatchEvaluator evaluator;

            [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
            public CollectRateReplacer(Th075Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var level = Parsers.LevelWithTotalParser.Parse(match.Groups[1].Value);
                    var chara = Parsers.CharaParser.Parse(match.Groups[2].Value);
                    var type = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

                    if (chara == Chara.Meiling)
                        return match.ToString();

                    Func<ClearData, IEnumerable<short>> getValues;
                    if (type == 1)
                        getValues = (data => data.CardGotCount);
                    else if (type == 2)
                        getValues = (data => data.CardTrialCount);
                    else
                        getValues = (data => data.CardTrulyGot.Select(got => (short)got));

                    Func<short, bool> isPositive = (value => value > 0);

                    if (level == Th075.LevelWithTotal.Total)
                    {
                        return Utils.ToNumberString(
                            parent.allScoreData.ClearData.Where(pair => pair.Key.chara == chara)
                                .Sum(pair => getValues(pair.Value).Count(isPositive)));
                    }
                    else
                    {
                        var cardIndexIdPairs = Definitions.CardIdTable[chara]
                            .Select((id, index) => new KeyValuePair<int, int>(index, id))
                            .Where(pair => Definitions.CardTable[pair.Value].Level == (Th075.Level)level);
                        return Utils.ToNumberString(
                            getValues(parent.allScoreData.ClearData[(chara, Th075.Level.Easy)])
                                .Where((value, index) => cardIndexIdPairs.Any(pair => pair.Key == index))
                                .Count(isPositive));
                    }
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        // %T75CHR[x][yy][z]
        private class CharaReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T75CHR({0})({1})([1-4])", Parsers.LevelParser.Pattern, Parsers.CharaParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public CharaReplacer(Th075Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var level = Parsers.LevelParser.Parse(match.Groups[1].Value);
                    var chara = Parsers.CharaParser.Parse(match.Groups[2].Value);
                    var type = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

                    if (chara == Chara.Meiling)
                        return match.ToString();

                    var data = parent.allScoreData.ClearData[(chara, level)];
                    switch (type)
                    {
                        case 1:
                            return Utils.ToNumberString(data.UseCount);
                        case 2:
                            return Utils.ToNumberString(data.ClearCount);
                        case 3:
                            return Utils.ToNumberString(data.MaxCombo);
                        case 4:
                            return Utils.ToNumberString(data.MaxDamage);
                        default:
                            return match.ToString();
                    }
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        private class AllScoreData : IBinaryReadable
        {
            public AllScoreData()
            {
                var numCharas = Enum.GetValues(typeof(Chara)).Length;
                var numLevels = Enum.GetValues(typeof(Th075.Level)).Length;
                this.ClearData = new Dictionary<(Chara, Th075.Level), ClearData>(numCharas * numLevels);
            }

            public IReadOnlyDictionary<(Chara chara, Th075.Level level), ClearData> ClearData { get; private set; }

            public Status Status { get; private set; }

            public void ReadFrom(BinaryReader reader)
            {
                var levels = Utils.GetEnumerator<Th075.Level>();

                this.ClearData = Utils.GetEnumerator<Chara>()
                    .SelectMany(chara => levels.Select(level => (chara, level)))
                    .ToDictionary(pair => pair, pair =>
                    {
                        var clearData = new ClearData();
                        clearData.ReadFrom(reader);
                        return clearData;
                    });

                _ = Enumerable.Range(1, 4).SelectMany(unknownChara => levels.Select(level =>
                {
                    var clearData = new ClearData();
                    clearData.ReadFrom(reader);
                    return clearData;
                })).ToList();

                var status = new Status();
                status.ReadFrom(reader);
                this.Status = status;
            }
        }
    }
}

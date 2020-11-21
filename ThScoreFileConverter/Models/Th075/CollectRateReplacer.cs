//-----------------------------------------------------------------------
// <copyright file="CollectRateReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace ThScoreFileConverter.Models.Th075
{
    // %T75CRG[x][yy][z]
    internal class CollectRateReplacer : IStringReplaceable
    {
        private static readonly string Pattern = Utils.Format(
            @"%T75CRG({0})({1})([1-3])", Parsers.LevelWithTotalParser.Pattern, Parsers.CharaParser.Pattern);

        private readonly MatchEvaluator evaluator;

        public CollectRateReplacer(IReadOnlyDictionary<(CharaWithReserved chara, Level level), IClearData> clearData)
        {
            if (clearData is null)
                throw new ArgumentNullException(nameof(clearData));

            this.evaluator = new MatchEvaluator(match => EvaluatorImpl(match, clearData));

            static string EvaluatorImpl(
                Match match, IReadOnlyDictionary<(CharaWithReserved chara, Level level), IClearData> clearData)
            {
                var level = Parsers.LevelWithTotalParser.Parse(match.Groups[1].Value);
                var chara = Parsers.CharaParser.Parse(match.Groups[2].Value);
                var type = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

                if (chara == Chara.Meiling)
                    return match.ToString();

                Func<IClearData, IEnumerable<short>> getValues = type switch
                {
                    1 => data => data.CardGotCount,
                    2 => data => data.CardTrialCount,
                    _ => data => data.CardTrulyGot.Select(got => (short)got),
                };

                IEnumerable<(int cardId, int cardIndex)> MakeCardIdIndexPairs(Level lv)
                {
                    return Definitions.CardIdTable[chara]
                        .Select((id, index) => (id, index))
                        .Where(pair => Definitions.CardTable[pair.id].Level == lv);
                }

                static bool IsPositive(short value)
                {
                    return value > 0;
                }

                if (level == LevelWithTotal.Total)
                {
                    return Utils.ToNumberString(clearData
                        .Where(dataPair => dataPair.Key.chara == (CharaWithReserved)chara)
                        .Sum(dataPair => getValues(dataPair.Value)
                            .Where((value, index) =>
                                MakeCardIdIndexPairs(dataPair.Key.level).Any(pair => pair.cardIndex == index))
                            .Count(IsPositive)));
                }
                else
                {
                    return Utils.ToNumberString(clearData
                        .Where(dataPair => dataPair.Key == ((CharaWithReserved)chara, (Level)level))
                        .Sum(dataPair => getValues(dataPair.Value)
                            .Where((value, index) =>
                                MakeCardIdIndexPairs((Level)level).Any(pair => pair.cardIndex == index))
                            .Count(IsPositive)));
                }
            }
        }

        public string Replace(string input)
        {
            return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
        }
    }
}

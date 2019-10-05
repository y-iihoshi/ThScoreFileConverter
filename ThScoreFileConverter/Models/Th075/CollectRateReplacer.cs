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
            this.evaluator = new MatchEvaluator(match =>
            {
                var level = Parsers.LevelWithTotalParser.Parse(match.Groups[1].Value);
                var chara = Parsers.CharaParser.Parse(match.Groups[2].Value);
                var type = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

                if (chara == Chara.Meiling)
                    return match.ToString();

                Func<IClearData, IEnumerable<short>> getValues;
                if (type == 1)
                    getValues = data => data.CardGotCount;
                else if (type == 2)
                    getValues = data => data.CardTrialCount;
                else
                    getValues = data => data.CardTrulyGot.Select(got => (short)got);

                bool IsPositive(short value) => value > 0;

                if (level == LevelWithTotal.Total)
                {
                    return Utils.ToNumberString(clearData
                        .Where(pair => pair.Key.chara == (CharaWithReserved)chara)
                        .Sum(pair => getValues(pair.Value).Count(IsPositive)));
                }
                else
                {
                    var cardIndexIdPairs = Definitions.CardIdTable[chara]
                        .Select((id, index) => new KeyValuePair<int, int>(index, id))
                        .Where(pair => Definitions.CardTable[pair.Value].Level == (Level)level);
                    return Utils.ToNumberString(
                        getValues(clearData[((CharaWithReserved)chara, Level.Easy)])
                            .Where((value, index) => cardIndexIdPairs.Any(pair => pair.Key == index))
                            .Count(IsPositive));
                }
            });
        }

        public string Replace(string input) => Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
    }
}

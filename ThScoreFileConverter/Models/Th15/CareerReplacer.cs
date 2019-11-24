//-----------------------------------------------------------------------
// <copyright file="CareerReplacer.cs" company="None">
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

namespace ThScoreFileConverter.Models.Th15
{
    // %T15C[w][xxx][yy][z]
    internal class CareerReplacer : IStringReplaceable
    {
        private static readonly string Pattern = Utils.Format(
            @"%T15C({0})(\d{{3}})({1})([12])", Parsers.GameModeParser.Pattern, Parsers.CharaWithTotalParser.Pattern);

        private readonly MatchEvaluator evaluator;

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
                    getCount = card => card.ClearCount;
                else
                    getCount = card => card.TrialCount;

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

        public string Replace(string input) => Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
    }
}

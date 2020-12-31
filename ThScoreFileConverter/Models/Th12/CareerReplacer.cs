//-----------------------------------------------------------------------
// <copyright file="CareerReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using ThScoreFileConverter.Helpers;
using IClearData = ThScoreFileConverter.Models.Th10.IClearData<
    ThScoreFileConverter.Models.Th12.CharaWithTotal, ThScoreFileConverter.Models.Th10.StageProgress>;

namespace ThScoreFileConverter.Models.Th12
{
    // %T12C[xxx][yy][z]
    internal class CareerReplacer : IStringReplaceable
    {
        private static readonly string Pattern = Utils.Format(
            @"%T12C(\d{{3}})({0})([12])", Parsers.CharaWithTotalParser.Pattern);

        private readonly MatchEvaluator evaluator;

        public CareerReplacer(
            IReadOnlyDictionary<CharaWithTotal, IClearData> clearDataDictionary, INumberFormatter formatter)
        {
            if (clearDataDictionary is null)
                throw new ArgumentNullException(nameof(clearDataDictionary));

            this.evaluator = new MatchEvaluator(match =>
            {
                var number = IntegerHelper.Parse(match.Groups[1].Value);
                var chara = Parsers.CharaWithTotalParser.Parse(match.Groups[2].Value);
                var type = IntegerHelper.Parse(match.Groups[3].Value);

                Func<Th10.ISpellCard<Level>, int> getCount = type switch
                {
                    1 => card => card.ClearCount,
                    _ => card => card.TrialCount,
                };

                var cards = clearDataDictionary.TryGetValue(chara, out var clearData)
                    ? clearData.Cards : ImmutableDictionary<int, Th10.ISpellCard<Level>>.Empty;
                if (number == 0)
                {
                    return formatter.FormatNumber(cards.Values.Sum(getCount));
                }
                else if (Definitions.CardTable.ContainsKey(number))
                {
                    return formatter.FormatNumber(cards.TryGetValue(number, out var card) ? getCount(card) : default);
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
}

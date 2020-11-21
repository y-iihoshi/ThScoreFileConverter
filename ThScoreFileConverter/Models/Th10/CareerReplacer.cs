﻿//-----------------------------------------------------------------------
// <copyright file="CareerReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace ThScoreFileConverter.Models.Th10
{
    // %T10C[xxx][yy][z]
    internal class CareerReplacer : IStringReplaceable
    {
        private static readonly string Pattern = Utils.Format(
            @"%T10C(\d{{3}})({0})([12])", Parsers.CharaWithTotalParser.Pattern);

        private readonly MatchEvaluator evaluator;

        public CareerReplacer(
            IReadOnlyDictionary<CharaWithTotal, IClearData<CharaWithTotal, StageProgress>> clearDataDictionary)
        {
            if (clearDataDictionary is null)
                throw new ArgumentNullException(nameof(clearDataDictionary));

            this.evaluator = new MatchEvaluator(match =>
            {
                var number = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
                var chara = Parsers.CharaWithTotalParser.Parse(match.Groups[2].Value);
                var type = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

                Func<ISpellCard<Level>, int> getCount = type switch
                {
                    1 => card => card.ClearCount,
                    _ => card => card.TrialCount,
                };

                var cards = clearDataDictionary.TryGetValue(chara, out var clearData)
                    ? clearData.Cards : ImmutableDictionary<int, ISpellCard<Level>>.Empty;
                if (number == 0)
                {
                    return Utils.ToNumberString(cards.Values.Sum(getCount));
                }
                else if (Definitions.CardTable.ContainsKey(number))
                {
                    return Utils.ToNumberString(cards.TryGetValue(number, out var card) ? getCount(card) : default);
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

﻿//-----------------------------------------------------------------------
// <copyright file="CareerReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th128
{
    // %T128C[xxx][z]
    internal class CareerReplacer : IStringReplaceable
    {
        private const string Pattern = @"%T128C(\d{3})([1-3])";

        private readonly MatchEvaluator evaluator;

        public CareerReplacer(IReadOnlyDictionary<int, ISpellCard> spellCards)
        {
            if (spellCards is null)
                throw new ArgumentNullException(nameof(spellCards));

            this.evaluator = new MatchEvaluator(match =>
            {
                var number = IntegerHelper.Parse(match.Groups[1].Value);
                var type = IntegerHelper.Parse(match.Groups[2].Value);

                Func<ISpellCard, int> getCount = type switch
                {
                    1 => card => card.NoIceCount,
                    2 => card => card.NoMissCount,
                    _ => card => card.TrialCount,
                };

                if (number == 0)
                {
                    return Utils.ToNumberString(spellCards.Values.Sum(getCount));
                }
                else if (Definitions.CardTable.ContainsKey(number))
                {
                    return Utils.ToNumberString(
                        spellCards.TryGetValue(number, out var card) ? getCount(card) : default);
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

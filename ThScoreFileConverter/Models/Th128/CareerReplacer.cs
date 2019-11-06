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

namespace ThScoreFileConverter.Models.Th128
{
    // %T128C[xxx][z]
    internal class CareerReplacer : IStringReplaceable
    {
        private const string Pattern = @"%T128C(\d{3})([1-3])";

        private readonly MatchEvaluator evaluator;

        public CareerReplacer(IReadOnlyDictionary<int, ISpellCard> spellCards)
        {
            this.evaluator = new MatchEvaluator(match =>
            {
                var number = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
                var type = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);

                Func<ISpellCard, int> getCount;
                if (type == 1)
                    getCount = card => card.NoIceCount;
                else if (type == 2)
                    getCount = card => card.NoMissCount;
                else
                    getCount = card => card.TrialCount;

                if (number == 0)
                {
                    return Utils.ToNumberString(spellCards.Values.Sum(getCount));
                }
                else if (Definitions.CardTable.ContainsKey(number))
                {
                    if (spellCards.TryGetValue(number, out var card))
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

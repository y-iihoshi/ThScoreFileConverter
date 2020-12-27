//-----------------------------------------------------------------------
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

namespace ThScoreFileConverter.Models.Th06
{
    // %T06C[xx][y]
    internal class CareerReplacer : IStringReplaceable
    {
        private const string Pattern = @"%T06C(\d{2})([12])";

        private readonly MatchEvaluator evaluator;

        public CareerReplacer(IReadOnlyDictionary<int, ICardAttack> cardAttacks)
        {
            if (cardAttacks is null)
                throw new ArgumentNullException(nameof(cardAttacks));

            this.evaluator = new MatchEvaluator(match =>
            {
                var number = IntegerHelper.Parse(match.Groups[1].Value);
                var type = IntegerHelper.Parse(match.Groups[2].Value);

                Func<ICardAttack, int> getCount = type switch
                {
                    1 => attack => attack.ClearCount,
                    _ => attack => attack.TrialCount,
                };

                if (number == 0)
                {
                    return Utils.ToNumberString(cardAttacks.Values.Sum(getCount));
                }
                else if (Definitions.CardTable.ContainsKey(number))
                {
                    return Utils.ToNumberString(
                        cardAttacks.TryGetValue(number, out var attack) ? getCount(attack) : default);
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

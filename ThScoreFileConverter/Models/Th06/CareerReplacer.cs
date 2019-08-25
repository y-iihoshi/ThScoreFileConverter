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

namespace ThScoreFileConverter.Models.Th06
{
    // %T06C[xx][y]
    internal class CareerReplacer : IStringReplaceable
    {
        private const string Pattern = @"%T06C(\d{2})([12])";

        private readonly MatchEvaluator evaluator;

        public CareerReplacer(IReadOnlyDictionary<int, CardAttack> cardAttacks)
        {
            if (cardAttacks is null)
                throw new ArgumentNullException(nameof(cardAttacks));

            this.evaluator = new MatchEvaluator(match =>
            {
                var number = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
                var type = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);

                Func<CardAttack, int> getCount;
                if (type == 1)
                    getCount = attack => attack.ClearCount;
                else
                    getCount = attack => attack.TrialCount;

                if (number == 0)
                {
                    return Utils.ToNumberString(cardAttacks.Values.Sum(getCount));
                }
                else if (Definitions.CardTable.ContainsKey(number))
                {
                    return cardAttacks.TryGetValue(number, out var attack)
                        ? Utils.ToNumberString(getCount(attack)) : "0";
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

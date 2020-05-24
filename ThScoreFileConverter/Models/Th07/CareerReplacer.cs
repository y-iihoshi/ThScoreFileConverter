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
using static ThScoreFileConverter.Models.Th07.Parsers;

namespace ThScoreFileConverter.Models.Th07
{
    // %T07C[xxx][yy][z]
    internal class CareerReplacer : IStringReplaceable
    {
        private static readonly string Pattern = Utils.Format(
            @"%T07C(\d{{3}})({0})([1-3])", CharaWithTotalParser.Pattern);

        private readonly MatchEvaluator evaluator;

        public CareerReplacer(IReadOnlyDictionary<int, ICardAttack> cardAttacks)
        {
            if (cardAttacks is null)
                throw new ArgumentNullException(nameof(cardAttacks));

            this.evaluator = new MatchEvaluator(match =>
            {
                var number = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
                var chara = CharaWithTotalParser.Parse(match.Groups[2].Value);
                var type = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

#pragma warning disable IDE0007 // Use implicit type
                Func<ICardAttack, long> getValue = type switch
                {
                    1 => attack => attack.MaxBonuses[chara],
                    2 => attack => attack.ClearCounts[chara],
                    _ => attack => attack.TrialCounts[chara],
                };
#pragma warning restore IDE0007 // Use implicit type

                if (number == 0)
                {
                    return Utils.ToNumberString(cardAttacks.Values.Sum(getValue));
                }
                else if (Definitions.CardTable.ContainsKey(number))
                {
                    return Utils.ToNumberString(
                        cardAttacks.TryGetValue(number, out var attack) ? getValue(attack) : default);
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

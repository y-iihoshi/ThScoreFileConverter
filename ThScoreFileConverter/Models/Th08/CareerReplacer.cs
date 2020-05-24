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
using static ThScoreFileConverter.Models.Th08.Parsers;

namespace ThScoreFileConverter.Models.Th08
{
    // %T08C[w][xxx][yy][z]
    internal class CareerReplacer : IStringReplaceable
    {
        private static readonly string Pattern = Utils.Format(
            @"%T08C([SP])(\d{{3}})({0})([1-3])", CharaWithTotalParser.Pattern);

        private readonly MatchEvaluator evaluator;

        public CareerReplacer(IReadOnlyDictionary<int, ICardAttack> cardAttacks)
        {
            if (cardAttacks is null)
                throw new ArgumentNullException(nameof(cardAttacks));

            this.evaluator = new MatchEvaluator(match =>
            {
                var kind = match.Groups[1].Value.ToUpperInvariant();
                var number = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);
                var chara = CharaWithTotalParser.Parse(match.Groups[3].Value);
                var type = int.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture);

#pragma warning disable IDE0007 // Use implicit type
                Func<ICardAttack, bool> isValidLevel = kind switch
                {
                    "S" => attack => Definitions.CardTable[attack.CardId].Level != LevelPractice.LastWord,
                    _ => Utils.True,
                };

                Func<ICardAttack, ICardAttackCareer> getCareer = kind switch
                {
                    "S" => attack => attack.StoryCareer,
                    _ => attack => attack.PracticeCareer,
                };

                Func<ICardAttack, long> getValue = type switch
                {
                    1 => attack => getCareer(attack).MaxBonuses[chara],
                    2 => attack => getCareer(attack).ClearCounts[chara],
                    _ => attack => getCareer(attack).TrialCounts[chara],
                };
#pragma warning restore IDE0007 // Use implicit type

                if (number == 0)
                {
                    return Utils.ToNumberString(cardAttacks.Values.Where(isValidLevel).Sum(getValue));
                }
                else if (Definitions.CardTable.ContainsKey(number))
                {
                    if (cardAttacks.TryGetValue(number, out var attack))
                    {
                        return isValidLevel(attack) ? Utils.ToNumberString(getValue(attack)) : match.ToString();
                    }
                    else
                    {
                        return Utils.ToNumberString(default(long));
                    }
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

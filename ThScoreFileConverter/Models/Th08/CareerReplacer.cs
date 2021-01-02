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
using static ThScoreFileConverter.Models.Th08.Parsers;

namespace ThScoreFileConverter.Models.Th08
{
    // %T08C[w][xxx][yy][z]
    internal class CareerReplacer : IStringReplaceable
    {
        private static readonly string Pattern = Utils.Format(
            @"%T08C([SP])(\d{{3}})({0})([1-3])", CharaWithTotalParser.Pattern);

        private readonly MatchEvaluator evaluator;

        public CareerReplacer(IReadOnlyDictionary<int, ICardAttack> cardAttacks, INumberFormatter formatter)
        {
            this.evaluator = new MatchEvaluator(match =>
            {
                var kind = match.Groups[1].Value.ToUpperInvariant();
                var number = IntegerHelper.Parse(match.Groups[2].Value);
                var chara = CharaWithTotalParser.Parse(match.Groups[3].Value);
                var type = IntegerHelper.Parse(match.Groups[4].Value);

                Func<ICardAttack, bool> isValidLevel = kind switch
                {
                    "S" => attack => Definitions.CardTable[attack.CardId].Level != LevelPractice.LastWord,
                    _ => FuncHelper.True,
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

                if (number == 0)
                {
                    return formatter.FormatNumber(cardAttacks.Values.Where(isValidLevel).Sum(getValue));
                }
                else if (Definitions.CardTable.ContainsKey(number))
                {
                    if (cardAttacks.TryGetValue(number, out var attack))
                    {
                        return isValidLevel(attack) ? formatter.FormatNumber(getValue(attack)) : match.ToString();
                    }
                    else
                    {
                        return formatter.FormatNumber(default(long));
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

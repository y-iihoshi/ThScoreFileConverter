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
using IClearData = ThScoreFileConverter.Models.Th13.IClearData<
    ThScoreFileConverter.Models.Th13.CharaWithTotal,
    ThScoreFileConverter.Models.Th13.LevelPractice,
    ThScoreFileConverter.Models.Th13.LevelPractice,
    ThScoreFileConverter.Models.Th13.LevelPracticeWithTotal,
    ThScoreFileConverter.Models.Th13.StagePractice>;

namespace ThScoreFileConverter.Models.Th13
{
    // %T13C[w][xxx][yy][z]
    internal class CareerReplacer : IStringReplaceable
    {
        private static readonly string Pattern = Utils.Format(
            @"%T13C([SP])(\d{{3}})({0})([12])", Parsers.CharaWithTotalParser.Pattern);

        private readonly MatchEvaluator evaluator;

        public CareerReplacer(
            IReadOnlyDictionary<CharaWithTotal, IClearData> clearDataDictionary, INumberFormatter formatter)
        {
            this.evaluator = new MatchEvaluator(match =>
            {
                var kind = match.Groups[1].Value.ToUpperInvariant();
                var number = IntegerHelper.Parse(match.Groups[2].Value);
                var chara = Parsers.CharaWithTotalParser.Parse(match.Groups[3].Value);
                var type = IntegerHelper.Parse(match.Groups[4].Value);

                Func<ISpellCard<LevelPractice>, bool> isValidLevel = kind switch
                {
                    "S" => card => card.Level != LevelPractice.OverDrive,
                    _ => FuncHelper.True,
                };

                Func<ISpellCard<LevelPractice>, int> getCount = (kind, type) switch
                {
                    ("S", 1) => card => card.ClearCount,
                    ("S", _) => card => card.TrialCount,
                    (_, 1) => card => card.PracticeClearCount,
                    _ => card => card.PracticeTrialCount,
                };

                var cards = clearDataDictionary.TryGetValue(chara, out var clearData)
                    ? clearData.Cards : ImmutableDictionary<int, ISpellCard<LevelPractice>>.Empty;
                if (number == 0)
                {
                    return formatter.FormatNumber(cards.Values.Where(isValidLevel).Sum(getCount));
                }
                else if (Definitions.CardTable.ContainsKey(number))
                {
                    if (cards.TryGetValue(number, out var card))
                    {
                        return isValidLevel(card) ? formatter.FormatNumber(getCount(card)) : match.ToString();
                    }
                    else
                    {
                        return formatter.FormatNumber(default(int));
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

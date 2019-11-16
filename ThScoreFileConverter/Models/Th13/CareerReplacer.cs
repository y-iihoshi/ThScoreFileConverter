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

        public CareerReplacer(IReadOnlyDictionary<CharaWithTotal, IClearData> clearDataDictionary)
        {
            if (clearDataDictionary is null)
                throw new ArgumentNullException(nameof(clearDataDictionary));

            this.evaluator = new MatchEvaluator(match =>
            {
                var kind = match.Groups[1].Value.ToUpperInvariant();
                var number = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);
                var chara = Parsers.CharaWithTotalParser.Parse(match.Groups[3].Value);
                var type = int.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture);

                Func<ISpellCard<LevelPractice>, bool> isValidLevel;
                Func<ISpellCard<LevelPractice>, int> getCount;
                if (kind == "S")
                {
                    isValidLevel = card => card.Level != LevelPractice.OverDrive;
                    if (type == 1)
                        getCount = card => card.ClearCount;
                    else
                        getCount = card => card.TrialCount;
                }
                else
                {
                    isValidLevel = card => true;
                    if (type == 1)
                        getCount = card => card.PracticeClearCount;
                    else
                        getCount = card => card.PracticeTrialCount;
                }

                var cards = clearDataDictionary.TryGetValue(chara, out var clearData)
                    ? clearData.Cards : new Dictionary<int, ISpellCard<LevelPractice>>();
                if (number == 0)
                {
                    return Utils.ToNumberString(cards.Values.Where(isValidLevel).Sum(getCount));
                }
                else if (Definitions.CardTable.ContainsKey(number))
                {
                    if (cards.TryGetValue(number, out var card))
                    {
                        return isValidLevel(card) ? Utils.ToNumberString(getCount(card)) : match.ToString();
                    }
                    else
                    {
                        return "0";
                    }
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

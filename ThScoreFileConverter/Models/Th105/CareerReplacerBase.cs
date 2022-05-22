//-----------------------------------------------------------------------
// <copyright file="CareerReplacerBase.cs" company="None">
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
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models.Th105;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th105;

internal class CareerReplacerBase<TChara> : IStringReplaceable
    where TChara : struct, Enum
{
    private readonly string pattern;
    private readonly MatchEvaluator evaluator;

    protected CareerReplacerBase(
        string formatPrefix,
        EnumShortNameParser<TChara> charaParser,
        Func<int, TChara, int, bool> canReplace,
        IReadOnlyDictionary<TChara, IEnumerable<(TChara Enemy, int CardId)>> enemyCardIdTable,
        IReadOnlyDictionary<TChara, IClearData<TChara>> clearDataDictionary,
        INumberFormatter formatter)
    {
        var numLevels = EnumHelper<Level>.NumValues;

        this.pattern = Utils.Format(
            @"{0}C(\d{{{1}}})({2})([1-3])",
            formatPrefix,
            IntegerHelper.GetNumDigits(enemyCardIdTable.Max(pair => pair.Value.Count()) * numLevels),
            charaParser.Pattern);
        this.evaluator = new MatchEvaluator(match =>
        {
            var number = IntegerHelper.Parse(match.Groups[1].Value);
            var chara = charaParser.Parse(match.Groups[2].Value);
            var type = IntegerHelper.Parse(match.Groups[3].Value);

            if (!canReplace(number, chara, type))
                return match.ToString();

            Func<ISpellCardResult<TChara>, long> getValue = type switch
            {
                1 => result => result.GotCount,
                2 => result => result.TrialCount,
                _ => result => result.Frames,
            };

            Func<long, string> toString = type switch
            {
                3 => value =>
                {
                    var time = new Time(value);
                    return Utils.Format(
                        "{0:D2}:{1:D2}.{2:D3}",
                        (time.Hours * 60) + time.Minutes,
                        time.Seconds,
                        time.Frames * 1000 / 60);
                },
                _ => formatter.FormatNumber,
            };

            var spellCardResults = clearDataDictionary.TryGetValue(chara, out var clearData)
                ? clearData.SpellCardResults : ImmutableDictionary<(TChara, int), ISpellCardResult<TChara>>.Empty;
            if (number == 0)
            {
                return toString(spellCardResults.Values.Sum(getValue));
            }
            else
            {
                var index = (number - 1) / numLevels;
                if (enemyCardIdTable.TryGetValue(chara, out var enemyCardIdPairs)
                    && (index < enemyCardIdPairs.Count()))
                {
                    var (enemy, cardId) = enemyCardIdPairs.ElementAt(index);
                    var key = (enemy, (cardId * numLevels) + ((number - 1) % numLevels));
                    return toString(spellCardResults.TryGetValue(key, out var result) ? getValue(result) : default);
                }
                else
                {
                    return match.ToString();
                }
            }
        });
    }

    public string Replace(string input)
    {
        return Regex.Replace(input, this.pattern, this.evaluator, RegexOptions.IgnoreCase);
    }
}

//-----------------------------------------------------------------------
// <copyright file="CareerReplacerBase.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Immutable;
using System.Text.RegularExpressions;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Helpers;
using Level = ThScoreFileConverter.Core.Models.Th105.Level;

namespace ThScoreFileConverter.Models.Th105;

internal class CareerReplacerBase<TChara> : IStringReplaceable
    where TChara : struct, Enum
{
    private static readonly IntegerParser TypeParser = new(@"[1-3]");

    private readonly string pattern;
    private readonly MatchEvaluator evaluator;

    protected CareerReplacerBase(
        string formatPrefix,
        IRegexParser<TChara> charaParser,
        Func<int, TChara, int, bool> canReplace,
        IReadOnlyDictionary<TChara, IEnumerable<(TChara Enemy, int CardId)>> enemyCardIdTable,
        IReadOnlyDictionary<TChara, IClearData<TChara>> clearDataDictionary,
        INumberFormatter formatter)
    {
        var numLevels = EnumHelper<Level>.NumValues;
        var numDigits = IntegerHelper.GetNumDigits(enemyCardIdTable.Max(pair => pair.Value.Count()) * numLevels);
        var cardNumberParser = new IntegerParser($@"\d{{{numDigits}}}");

        this.pattern = StringHelper.Create($"{formatPrefix}C({cardNumberParser.Pattern})({charaParser.Pattern})({TypeParser.Pattern})");
        this.evaluator = new MatchEvaluator(match =>
        {
            var number = cardNumberParser.Parse(match.Groups[1]);
            var chara = charaParser.Parse(match.Groups[2]);
            var type = TypeParser.Parse(match.Groups[3]);

            if (!canReplace(number, chara, type))
                return match.ToString();

            static string FormatTime(long value)
            {
                var time = new Time(value);
                var minutes = (time.Hours * 60) + time.Minutes;
                var milliseconds = time.Frames * 1000 / 60;
                return StringHelper.Create($"{minutes:D2}:{time.Seconds:D2}.{milliseconds:D3}");
            }

            Func<ISpellCardResult<TChara>, long> getValue = type switch
            {
                1 => result => result.GotCount,
                2 => result => result.TrialCount,
                _ => result => result.Frames,
            };

            Func<long, string> toString = type switch
            {
                3 => FormatTime,
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

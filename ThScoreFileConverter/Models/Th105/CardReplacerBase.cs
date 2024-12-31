//-----------------------------------------------------------------------
// <copyright file="CardReplacerBase.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Helpers;
using Level = ThScoreFileConverter.Core.Models.Th105.Level;

#pragma warning disable SA1600 // Elements should be documented

namespace ThScoreFileConverter.Models.Th105;

internal class CardReplacerBase<TChara> : IStringReplaceable
    where TChara : struct, Enum
{
    private readonly string pattern;
    private readonly MatchEvaluator evaluator;

    protected CardReplacerBase(
        string formatPrefix,
        IRegexParser<TChara> charaParser,
        Func<TChara, bool> charaHasStory,
        IReadOnlyDictionary<TChara, IEnumerable<(TChara Enemy, int CardId)>> enemyCardIdTable,
        IReadOnlyDictionary<(TChara Chara, int CardId), string> cardNameTable,
        IReadOnlyDictionary<TChara, IClearData<TChara>> clearDataDictionary,
        bool hideUntriedCards)
    {
        var numLevels = EnumHelper<Level>.NumValues;
        var numDigits = IntegerHelper.GetNumDigits(enemyCardIdTable.Max(pair => pair.Value.Count()) * numLevels);

        this.pattern = StringHelper.Create($@"{formatPrefix}CARD(\d{{{numDigits}}})({charaParser.Pattern})([NR])");
        this.evaluator = new MatchEvaluator(match =>
        {
            var number = IntegerHelper.Parse(match.Groups[1].Value);
            var chara = charaParser.Parse(match.Groups[2]);
            var type = match.Groups[3].Value.ToUpperInvariant();

            if (number <= 0)
                return match.ToString();
            if (!charaHasStory(chara))
                return match.ToString();

            var index = (number - 1) / numLevels;
            if (enemyCardIdTable.TryGetValue(chara, out var enemyCardIdPairs)
                && (index < enemyCardIdPairs.Count()))
            {
                var level = (Level)((number - 1) % numLevels);
                var enemyCardIdPair = enemyCardIdPairs.ElementAt(index);
                if (hideUntriedCards)
                {
                    var spellCardResults = clearDataDictionary.TryGetValue(chara, out var clearData)
                        ? clearData.SpellCardResults
                        : ImmutableDictionary<(TChara, int), ISpellCardResult<TChara>>.Empty;
                    var key = (enemyCardIdPair.Enemy, (enemyCardIdPair.CardId * numLevels) + (int)level);
                    if (!spellCardResults.TryGetValue(key, out var result) || (result.TrialCount <= 0))
                        return (type == "N") ? "??????????" : "?????";
                }

                return (type == "N") ? cardNameTable[enemyCardIdPair] : level.ToString();
            }
            else
            {
                return match.ToString();
            }
        });
    }

    public string Replace(string input)
    {
        return Regex.Replace(input, this.pattern, this.evaluator, RegexOptions.IgnoreCase);
    }
}

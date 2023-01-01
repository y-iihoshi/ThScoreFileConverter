//-----------------------------------------------------------------------
// <copyright file="CollectRateReplacerBase.cs" company="None">
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
using ThScoreFileConverter.Core.Models.Th105;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th105;

internal class CollectRateReplacerBase<TChara> : IStringReplaceable
    where TChara : struct, Enum
{
    private readonly string pattern;
    private readonly MatchEvaluator evaluator;

    protected CollectRateReplacerBase(
        string formatPrefix,
        EnumShortNameParser<LevelWithTotal> levelWithTotalParser,
        EnumShortNameParser<TChara> charaParser,
        Func<LevelWithTotal, TChara, int, bool> canReplace,
        IReadOnlyDictionary<TChara, IClearData<TChara>> clearDataDictionary,
        INumberFormatter formatter)
    {
        this.pattern = Utils.Format(
            @"{0}CRG({1})({2})([1-2])", formatPrefix, levelWithTotalParser.Pattern, charaParser.Pattern);
        this.evaluator = new MatchEvaluator(match =>
        {
            var level = levelWithTotalParser.Parse(match.Groups[1].Value);
            var chara = charaParser.Parse(match.Groups[2].Value);
            var type = IntegerHelper.Parse(match.Groups[3].Value);

            if (!canReplace(level, chara, type))
                return match.ToString();

#pragma warning disable IDE0072 // Add missing cases to switch expression
            Func<KeyValuePair<(TChara, int), ISpellCardResult<TChara>>, bool> findByLevel = level switch
            {
                LevelWithTotal.Total => FuncHelper.True,
                _ => pair => pair.Value.Level == (Level)level,
            };
#pragma warning restore IDE0072 // Add missing cases to switch expression

            Func<KeyValuePair<(TChara, int), ISpellCardResult<TChara>>, bool> countByType = type switch
            {
                1 => pair => pair.Value.GotCount > 0,
                _ => pair => pair.Value.TrialCount > 0,
            };

            var spellCardResults = clearDataDictionary.TryGetValue(chara, out var clearData)
                ? clearData.SpellCardResults : ImmutableDictionary<(TChara, int), ISpellCardResult<TChara>>.Empty;
            return formatter.FormatNumber(spellCardResults.Where(findByLevel).Count(countByType));
        });
    }

    public string Replace(string input)
    {
        return Regex.Replace(input, this.pattern, this.evaluator, RegexOptions.IgnoreCase);
    }
}

//-----------------------------------------------------------------------
// <copyright file="CollectRateReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Helpers;
using Stage = ThScoreFileConverter.Core.Models.Th128.Stage;
using StageWithTotal = ThScoreFileConverter.Core.Models.Th128.StageWithTotal;

namespace ThScoreFileConverter.Models.Th128;

// %T128CRG[x][yyy][z]
internal class CollectRateReplacer : IStringReplaceable
{
    private static readonly string Pattern = StringHelper.Create(
        $"{Definitions.FormatPrefix}CRG({Parsers.LevelWithTotalParser.Pattern})({Parsers.StageWithTotalParser.Pattern})([1-3])");

    private readonly MatchEvaluator evaluator;

    public CollectRateReplacer(IReadOnlyDictionary<int, ISpellCard> spellCards, INumberFormatter formatter)
    {
        this.evaluator = new MatchEvaluator(match =>
        {
            var level = Parsers.LevelWithTotalParser.Parse(match.Groups[1].Value);
            var stage = Parsers.StageWithTotalParser.Parse(match.Groups[2].Value);
            var type = IntegerHelper.Parse(match.Groups[3].Value);

            if (stage == StageWithTotal.Extra)
                return match.ToString();

#pragma warning disable IDE0072 // Add missing cases to switch expression
            Func<ISpellCard, bool> findByLevel = level switch
            {
                LevelWithTotal.Total => FuncHelper.True,
                LevelWithTotal.Extra => FuncHelper.True,
                _ => card => card.Level == (Level)level,
            };
#pragma warning restore IDE0072 // Add missing cases to switch expression

            Func<ISpellCard, bool> findByStage = (level, stage) switch
            {
                (LevelWithTotal.Extra, _) => card => Definitions.CardTable[card.Id].Stage == Stage.Extra,
                (_, StageWithTotal.Total) => FuncHelper.True,
                _ => card => Definitions.CardTable[card.Id].Stage == (Stage)stage,
            };

            Func<ISpellCard, bool> findByType = type switch
            {
                1 => card => card.NoIceCount > 0,
                2 => card => card.NoMissCount > 0,
                _ => card => card.TrialCount > 0,
            };

            return formatter.FormatNumber(
                spellCards.Values.Count(FuncHelper.MakeAndPredicate(findByLevel, findByStage, findByType)));
        });
    }

    public string Replace(string input)
    {
        return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
    }
}

﻿//-----------------------------------------------------------------------
// <copyright file="CollectRateReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Text.RegularExpressions;
using ThScoreFileConverter.Core.Models.Th07;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th07;

// %T07CRG[w][xx][yy][z]
internal sealed class CollectRateReplacer : IStringReplaceable
{
    private static readonly Core.Models.IntegerParser TypeParser = new(@"[12]");
    private static readonly string Pattern = StringHelper.Create(
        $"{Definitions.FormatPrefix}CRG({Parsers.LevelWithTotalParser.Pattern})({Parsers.CharaWithTotalParser.Pattern})({Parsers.StageWithTotalParser.Pattern})({TypeParser.Pattern})");

    private readonly MatchEvaluator evaluator;

    public CollectRateReplacer(IReadOnlyDictionary<int, ICardAttack> cardAttacks, INumberFormatter formatter)
    {
        this.evaluator = new MatchEvaluator(match =>
        {
            var level = Parsers.LevelWithTotalParser.Parse(match.Groups[1]);
            var chara = Parsers.CharaWithTotalParser.Parse(match.Groups[2]);
            var stage = Parsers.StageWithTotalParser.Parse(match.Groups[3]);
            var type = TypeParser.Parse(match.Groups[4]);

            if (stage is StageWithTotal.Extra or StageWithTotal.Phantasm)
                return match.ToString();

#pragma warning disable IDE0072 // Add missing cases to switch expression
            Func<ICardAttack, bool> findByLevel = level switch
            {
                LevelWithTotal.Total => FuncHelper.True,
                LevelWithTotal.Extra => FuncHelper.True,
                LevelWithTotal.Phantasm => FuncHelper.True,
                _ => attack => Definitions.CardTable.Any(
                    pair => (pair.Key == attack.CardId) && (pair.Value.Level == (Level)level)),
            };
#pragma warning restore IDE0072 // Add missing cases to switch expression

            Func<ICardAttack, bool> findByStage = (level, stage) switch
            {
                (LevelWithTotal.Extra, _) => attack => Definitions.CardTable.Any(
                    pair => (pair.Key == attack.CardId) && (pair.Value.Stage == Stage.Extra)),
                (LevelWithTotal.Phantasm, _) => attack => Definitions.CardTable.Any(
                    pair => (pair.Key == attack.CardId) && (pair.Value.Stage == Stage.Phantasm)),
                (_, StageWithTotal.Total) => FuncHelper.True,
                _ => attack => Definitions.CardTable.Any(
                    pair => (pair.Key == attack.CardId) && (pair.Value.Stage == (Stage)stage)),
            };

            Func<ICardAttack, bool> findByType = type switch
            {
                1 => attack => attack.ClearCounts[chara] > 0,
                _ => attack => attack.TrialCounts[chara] > 0,
            };

            return formatter.FormatNumber(
                cardAttacks.Values.Count(FuncHelper.MakeAndPredicate(findByLevel, findByStage, findByType)));
        });
    }

    public string Replace(string input)
    {
        return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
    }
}

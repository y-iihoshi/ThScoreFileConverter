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
using ThScoreFileConverter.Core.Models.Th08;
using ThScoreFileConverter.Helpers;
using GameMode = ThScoreFileConverter.Core.Models.GameMode;

namespace ThScoreFileConverter.Models.Th08;

// %T08CRG[v][w][xx][yy][z]
internal sealed class CollectRateReplacer : IStringReplaceable
{
    private static readonly string Pattern = StringHelper.Create(
        $"{Definitions.FormatPrefix}CRG({Parsers.GameModeParser.Pattern})({Parsers.LevelPracticeWithTotalParser.Pattern})({Parsers.CharaWithTotalParser.Pattern})({Parsers.StageWithTotalParser.Pattern})([12])");

    private readonly MatchEvaluator evaluator;

    public CollectRateReplacer(IReadOnlyDictionary<int, ICardAttack> cardAttacks, INumberFormatter formatter)
    {
        this.evaluator = new MatchEvaluator(match => EvaluatorImpl(match, cardAttacks, formatter));

        static string EvaluatorImpl(
            Match match, IReadOnlyDictionary<int, ICardAttack> cardAttacks, INumberFormatter formatter)
        {
            var mode = Parsers.GameModeParser.Parse(match.Groups[1]);
            var level = Parsers.LevelPracticeWithTotalParser.Parse(match.Groups[2]);
            var chara = Parsers.CharaWithTotalParser.Parse(match.Groups[3].Value);
            var stage = Parsers.StageWithTotalParser.Parse(match.Groups[4]);
            var type = IntegerHelper.Parse(match.Groups[5].Value);

            if (stage == StageWithTotal.Extra)
                return match.ToString();
            if ((mode == GameMode.Story) && (level == LevelPracticeWithTotal.LastWord))
                return match.ToString();

            Func<ICardAttackCareer, bool> findByType = type switch
            {
                1 => career => career.ClearCounts[chara] > 0,
                _ => career => career.TrialCounts[chara] > 0,
            };

#pragma warning disable IDE0072 // Add missing cases to switch expression
            Func<ICardAttack, bool> findByMode = mode switch
            {
                GameMode.Story => attack => Definitions.CardTable.Any(
                    pair => (pair.Key == attack.CardId) && (pair.Value.Level != LevelPractice.LastWord))
                    && findByType(attack.StoryCareer),
                _ => attack => findByType(attack.PracticeCareer),
            };

            Func<ICardAttack, bool> findByLevel = level switch
            {
                LevelPracticeWithTotal.Total => FuncHelper.True,
                LevelPracticeWithTotal.Extra => FuncHelper.True,
                LevelPracticeWithTotal.LastWord => FuncHelper.True,
                _ => attack => attack.Level == level,
            };
#pragma warning restore IDE0072 // Add missing cases to switch expression

            Func<ICardAttack, bool> findByStage = (level, stage) switch
            {
                (LevelPracticeWithTotal.Extra, _) => attack => Definitions.CardTable.Any(
                    pair => (pair.Key == attack.CardId) && (pair.Value.Stage == StagePractice.Extra)),
                (LevelPracticeWithTotal.LastWord, _) => attack => Definitions.CardTable.Any(
                    pair => (pair.Key == attack.CardId) && (pair.Value.Stage == StagePractice.LastWord)),
                (_, StageWithTotal.Total) => FuncHelper.True,
                _ => attack => Definitions.CardTable.Any(
                    pair => (pair.Key == attack.CardId) && (pair.Value.Stage == (StagePractice)stage)),
            };

            return formatter.FormatNumber(
                cardAttacks.Values.Count(FuncHelper.MakeAndPredicate(findByMode, findByLevel, findByStage)));
        }
    }

    public string Replace(string input)
    {
        return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
    }
}

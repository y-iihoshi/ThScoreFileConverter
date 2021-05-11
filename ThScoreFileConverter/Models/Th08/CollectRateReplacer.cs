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
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th08
{
    // %T08CRG[v][w][xx][yy][z]
    internal class CollectRateReplacer : IStringReplaceable
    {
        private static readonly string Pattern = Utils.Format(
            @"{0}CRG([SP])({1})({2})({3})([12])",
            Definitions.FormatPrefix,
            Parsers.LevelPracticeWithTotalParser.Pattern,
            Parsers.CharaWithTotalParser.Pattern,
            Parsers.StageWithTotalParser.Pattern);

        private readonly MatchEvaluator evaluator;

        public CollectRateReplacer(IReadOnlyDictionary<int, ICardAttack> cardAttacks, INumberFormatter formatter)
        {
            this.evaluator = new MatchEvaluator(match => EvaluatorImpl(match, cardAttacks, formatter));

            static string EvaluatorImpl(
                Match match, IReadOnlyDictionary<int, ICardAttack> cardAttacks, INumberFormatter formatter)
            {
                var kind = match.Groups[1].Value.ToUpperInvariant();
                var level = Parsers.LevelPracticeWithTotalParser.Parse(match.Groups[2].Value);
                var chara = Parsers.CharaWithTotalParser.Parse(match.Groups[3].Value);
                var stage = Parsers.StageWithTotalParser.Parse(match.Groups[4].Value);
                var type = IntegerHelper.Parse(match.Groups[5].Value);

                if (stage == StageWithTotal.Extra)
                    return match.ToString();
                if ((kind == "S") && (level == LevelPracticeWithTotal.LastWord))
                    return match.ToString();

                Func<ICardAttackCareer, bool> findByType = type switch
                {
                    1 => career => career.ClearCounts[chara] > 0,
                    _ => career => career.TrialCounts[chara] > 0,
                };

                Func<ICardAttack, bool> findByKind = kind switch
                {
                    "S" => attack => Definitions.CardTable.Any(
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
                    cardAttacks.Values.Count(FuncHelper.MakeAndPredicate(findByKind, findByLevel, findByStage)));
            }
        }

        public string Replace(string input)
        {
            return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
        }
    }
}

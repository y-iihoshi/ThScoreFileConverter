//-----------------------------------------------------------------------
// <copyright file="CollectRateReplacer.cs" company="None">
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
using static ThScoreFileConverter.Models.Th08.Parsers;

namespace ThScoreFileConverter.Models.Th08
{
    // %T08CRG[v][w][xx][yy][z]
    internal class CollectRateReplacer : IStringReplaceable
    {
        private static readonly string Pattern = Utils.Format(
            @"%T08CRG([SP])({0})({1})({2})([12])",
            LevelPracticeWithTotalParser.Pattern,
            CharaWithTotalParser.Pattern,
            StageWithTotalParser.Pattern);

        private readonly MatchEvaluator evaluator;

        public CollectRateReplacer(IReadOnlyDictionary<int, ICardAttack> cardAttacks)
        {
            if (cardAttacks is null)
                throw new ArgumentNullException(nameof(cardAttacks));

            this.evaluator = new MatchEvaluator(match => EvaluatorImpl(match, cardAttacks));

            static string EvaluatorImpl(Match match, IReadOnlyDictionary<int, ICardAttack> cardAttacks)
            {
                var kind = match.Groups[1].Value.ToUpperInvariant();
                var level = LevelPracticeWithTotalParser.Parse(match.Groups[2].Value);
                var chara = CharaWithTotalParser.Parse(match.Groups[3].Value);
                var stage = StageWithTotalParser.Parse(match.Groups[4].Value);
                var type = int.Parse(match.Groups[5].Value, CultureInfo.InvariantCulture);

                if (stage == StageWithTotal.Extra)
                    return match.ToString();
                if ((kind == "S") && (level == LevelPracticeWithTotal.LastWord))
                    return match.ToString();

#pragma warning disable IDE0007 // Use implicit type
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
                    LevelPracticeWithTotal.Total => Utils.True,
                    LevelPracticeWithTotal.Extra => Utils.True,
                    LevelPracticeWithTotal.LastWord => Utils.True,
                    _ => attack => attack.Level == level,
                };

                Func<ICardAttack, bool> findByStage = (level, stage) switch
                {
                    (LevelPracticeWithTotal.Extra, _) => attack => Definitions.CardTable.Any(
                        pair => (pair.Key == attack.CardId) && (pair.Value.Stage == StagePractice.Extra)),
                    (LevelPracticeWithTotal.LastWord, _) => attack => Definitions.CardTable.Any(
                        pair => (pair.Key == attack.CardId) && (pair.Value.Stage == StagePractice.LastWord)),
                    (_, StageWithTotal.Total) => Utils.True,
                    _ => attack => Definitions.CardTable.Any(
                        pair => (pair.Key == attack.CardId) && (pair.Value.Stage == (StagePractice)stage)),
                };
#pragma warning restore IDE0007 // Use implicit type

                return Utils.ToNumberString(
                    cardAttacks.Values.Count(Utils.MakeAndPredicate(findByKind, findByLevel, findByStage)));
            }
        }

        public string Replace(string input)
        {
            return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
        }
    }
}

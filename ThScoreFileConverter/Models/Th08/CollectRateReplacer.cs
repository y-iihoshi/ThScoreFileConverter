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

        private static readonly Func<ICardAttack, CharaWithTotal, string, int, bool> FindByKindTypeImpl =
            (attack, chara, kind, type) =>
            {
                Func<ICardAttackCareer, int> getCount;
                if (type == 1)
                    getCount = career => career.ClearCounts[chara];
                else
                    getCount = career => career.TrialCounts[chara];

                if (kind == "S")
                {
                    return Definitions.CardTable.Any(
                        pair => (pair.Key == attack.CardId) && (pair.Value.Level != LevelPractice.LastWord))
                        && (getCount(attack.StoryCareer) > 0);
                }
                else
                {
                    return getCount(attack.PracticeCareer) > 0;
                }
            };

        private readonly MatchEvaluator evaluator;

        public CollectRateReplacer(IReadOnlyDictionary<int, ICardAttack> cardAttacks)
        {
            if (cardAttacks is null)
                throw new ArgumentNullException(nameof(cardAttacks));

            this.evaluator = new MatchEvaluator(match =>
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

                bool FindByKindType(ICardAttack attack) => FindByKindTypeImpl(attack, chara, kind, type);

                Func<ICardAttack, bool> findByStage;
                if (stage == StageWithTotal.Total)
                {
                    findByStage = attack => true;
                }
                else
                {
                    findByStage = attack => Definitions.CardTable.Any(
                        pair => (pair.Key == attack.CardId) && (pair.Value.Stage == (StagePractice)stage));
                }

                Func<ICardAttack, bool> findByLevel = attack => true;
                switch (level)
                {
                    case LevelPracticeWithTotal.Total:
                        // Do nothing
                        break;
                    case LevelPracticeWithTotal.Extra:
                        findByStage = attack => Definitions.CardTable.Any(
                            pair => (pair.Key == attack.CardId) && (pair.Value.Stage == StagePractice.Extra));
                        break;
                    case LevelPracticeWithTotal.LastWord:
                        findByStage = attack => Definitions.CardTable.Any(
                            pair => (pair.Key == attack.CardId) && (pair.Value.Stage == StagePractice.LastWord));
                        break;
                    default:
                        findByLevel = attack => attack.Level == level;
                        break;
                }

                return cardAttacks.Values
                    .Count(Utils.MakeAndPredicate(FindByKindType, findByLevel, findByStage))
                    .ToString(CultureInfo.CurrentCulture);
            });
        }

        public string Replace(string input) => Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
    }
}

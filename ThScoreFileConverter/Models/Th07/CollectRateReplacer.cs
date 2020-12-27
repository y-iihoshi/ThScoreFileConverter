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
using static ThScoreFileConverter.Models.Th07.Parsers;

namespace ThScoreFileConverter.Models.Th07
{
    // %T07CRG[w][xx][yy][z]
    internal class CollectRateReplacer : IStringReplaceable
    {
        private static readonly string Pattern = Utils.Format(
            @"%T07CRG({0})({1})({2})([12])",
            LevelWithTotalParser.Pattern,
            CharaWithTotalParser.Pattern,
            StageWithTotalParser.Pattern);

        private readonly MatchEvaluator evaluator;

        public CollectRateReplacer(IReadOnlyDictionary<int, ICardAttack> cardAttacks)
        {
            if (cardAttacks is null)
                throw new ArgumentNullException(nameof(cardAttacks));

            this.evaluator = new MatchEvaluator(match =>
            {
                var level = LevelWithTotalParser.Parse(match.Groups[1].Value);
                var chara = CharaWithTotalParser.Parse(match.Groups[2].Value);
                var stage = StageWithTotalParser.Parse(match.Groups[3].Value);
                var type = IntegerHelper.Parse(match.Groups[4].Value);

                if ((stage == StageWithTotal.Extra) || (stage == StageWithTotal.Phantasm))
                    return match.ToString();

                Func<ICardAttack, bool> findByLevel = level switch
                {
                    LevelWithTotal.Total => FuncHelper.True,
                    LevelWithTotal.Extra => FuncHelper.True,
                    LevelWithTotal.Phantasm => FuncHelper.True,
                    _ => attack => Definitions.CardTable.Any(
                        pair => (pair.Key == attack.CardId) && (pair.Value.Level == (Level)level)),
                };

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

                return Utils.ToNumberString(
                    cardAttacks.Values.Count(FuncHelper.MakeAndPredicate(findByLevel, findByStage, findByType)));
            });
        }

        public string Replace(string input)
        {
            return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
        }
    }
}

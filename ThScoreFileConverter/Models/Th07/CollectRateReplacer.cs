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
                var type = int.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture);

                if ((stage == StageWithTotal.Extra) || (stage == StageWithTotal.Phantasm))
                    return match.ToString();

#pragma warning disable IDE0007 // Use implicit type
                Func<ICardAttack, bool> findByLevel = level switch
                {
                    LevelWithTotal.Total => Utils.True,
                    LevelWithTotal.Extra => Utils.True,
                    LevelWithTotal.Phantasm => Utils.True,
                    _ => attack => Definitions.CardTable.Any(
                        pair => (pair.Key == attack.CardId) && (pair.Value.Level == (Level)level)),
                };

                Func<ICardAttack, bool> findByStage = (level, stage) switch
                {
                    (LevelWithTotal.Extra, _) => attack => Definitions.CardTable.Any(
                        pair => (pair.Key == attack.CardId) && (pair.Value.Stage == Stage.Extra)),
                    (LevelWithTotal.Phantasm, _) => attack => Definitions.CardTable.Any(
                        pair => (pair.Key == attack.CardId) && (pair.Value.Stage == Stage.Phantasm)),
                    (_, StageWithTotal.Total) => Utils.True,
                    _ => attack => Definitions.CardTable.Any(
                        pair => (pair.Key == attack.CardId) && (pair.Value.Stage == (Stage)stage)),
                };

                Func<ICardAttack, bool> findByType = type switch
                {
                    1 => attack => attack.ClearCounts[chara] > 0,
                    _ => attack => attack.TrialCounts[chara] > 0,
                };
#pragma warning restore IDE0007 // Use implicit type

                return Utils.ToNumberString(
                    cardAttacks.Values.Count(Utils.MakeAndPredicate(findByLevel, findByStage, findByType)));
            });
        }

        public string Replace(string input)
        {
            return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
        }
    }
}

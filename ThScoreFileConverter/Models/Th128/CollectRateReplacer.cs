//-----------------------------------------------------------------------
// <copyright file="CollectRateReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning restore SA1600 // Elements should be documented

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace ThScoreFileConverter.Models.Th128
{
    // %T128CRG[x][yyy][z]
#pragma warning disable SA1600 // Elements should be documented
    internal class CollectRateReplacer : IStringReplaceable
    {
        private static readonly string Pattern = Utils.Format(
            @"%T128CRG({0})({1})([1-3])",
            Parsers.LevelWithTotalParser.Pattern,
            Parsers.StageWithTotalParser.Pattern);

        private readonly MatchEvaluator evaluator;

        public CollectRateReplacer(IReadOnlyDictionary<int, ISpellCard> spellCards)
        {
            if (spellCards is null)
                throw new ArgumentNullException(nameof(spellCards));

            this.evaluator = new MatchEvaluator(match =>
            {
                var level = Parsers.LevelWithTotalParser.Parse(match.Groups[1].Value);
                var stage = Parsers.StageWithTotalParser.Parse(match.Groups[2].Value);
                var type = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

                if (stage == StageWithTotal.Extra)
                    return match.ToString();

#pragma warning disable IDE0007 // Use implicit type
                Func<ISpellCard, bool> findByLevel = level switch
                {
                    LevelWithTotal.Total => Utils.True,
                    LevelWithTotal.Extra => Utils.True,
                    _ => card => card.Level == (Level)level,
                };

                Func<ISpellCard, bool> findByStage = (level, stage) switch
                {
                    (LevelWithTotal.Extra, _) => card => Definitions.CardTable[card.Id].Stage == Stage.Extra,
                    (_, StageWithTotal.Total) => Utils.True,
                    _ => card => Definitions.CardTable[card.Id].Stage == (Stage)stage,
                };

                Func<ISpellCard, bool> findByType = type switch
                {
                    1 => card => card.NoIceCount > 0,
                    2 => card => card.NoMissCount > 0,
                    _ => card => card.TrialCount > 0,
                };
#pragma warning restore IDE0007 // Use implicit type

                return Utils.ToNumberString(
                    spellCards.Values.Count(Utils.MakeAndPredicate(findByLevel, findByStage, findByType)));
            });
        }

        public string Replace(string input)
        {
            return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
        }
    }
}

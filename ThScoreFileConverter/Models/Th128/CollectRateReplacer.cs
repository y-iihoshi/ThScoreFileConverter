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

                Func<ISpellCard, bool> findByStage;
                if (stage == StageWithTotal.Total)
                    findByStage = card => true;
                else
                    findByStage = card => Definitions.CardTable[card.Id].Stage == (Stage)stage;

                Func<ISpellCard, bool> findByLevel = card => true;
                switch (level)
                {
                    case LevelWithTotal.Total:
                        // Do nothing
                        break;
                    case LevelWithTotal.Extra:
                        findByStage = card => Definitions.CardTable[card.Id].Stage == Stage.Extra;
                        break;
                    default:
                        findByLevel = card => card.Level == (Level)level;
                        break;
                }

                Func<ISpellCard, bool> findByType;
                if (type == 1)
                    findByType = card => card.NoIceCount > 0;
                else if (type == 2)
                    findByType = card => card.NoMissCount > 0;
                else
                    findByType = card => card.TrialCount > 0;

                return spellCards.Values
                    .Count(Utils.MakeAndPredicate(findByLevel, findByStage, findByType))
                    .ToString(CultureInfo.CurrentCulture);
            });
        }

        public string Replace(string input) => Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
    }
}

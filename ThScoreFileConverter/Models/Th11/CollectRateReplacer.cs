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

namespace ThScoreFileConverter.Models.Th11
{
    // %T11CRG[w][xx][y][z]
    internal class CollectRateReplacer : IStringReplaceable
    {
        private static readonly string Pattern = Utils.Format(
            @"%T11CRG({0})({1})({2})([12])",
            Parsers.LevelWithTotalParser.Pattern,
            Parsers.CharaWithTotalParser.Pattern,
            Parsers.StageWithTotalParser.Pattern);

        private readonly MatchEvaluator evaluator;

        public CollectRateReplacer(
            IReadOnlyDictionary<CharaWithTotal, Th10.IClearData<CharaWithTotal, StageProgress>> clearDataDictionary)
        {
            this.evaluator = new MatchEvaluator(match =>
            {
                var level = Parsers.LevelWithTotalParser.Parse(match.Groups[1].Value);
                var chara = Parsers.CharaWithTotalParser.Parse(match.Groups[2].Value);
                var stage = Parsers.StageWithTotalParser.Parse(match.Groups[3].Value);
                var type = int.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture);

                if (stage == StageWithTotal.Extra)
                    return match.ToString();

                Func<Th10.ISpellCard<Level>, bool> findByStage;
                if (stage == StageWithTotal.Total)
                    findByStage = card => true;
                else
                    findByStage = card => Definitions.CardTable[card.Id].Stage == (Stage)stage;

                Func<Th10.ISpellCard<Level>, bool> findByLevel = card => true;
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

                Func<Th10.ISpellCard<Level>, bool> findByType;
                if (type == 1)
                    findByType = card => card.ClearCount > 0;
                else
                    findByType = card => card.TrialCount > 0;

                return clearDataDictionary[chara].Cards.Values
                    .Count(Utils.MakeAndPredicate(findByLevel, findByStage, findByType))
                    .ToString(CultureInfo.CurrentCulture);
            });
        }

        public string Replace(string input) => Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
    }
}

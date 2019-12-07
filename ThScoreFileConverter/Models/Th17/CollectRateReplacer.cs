﻿//-----------------------------------------------------------------------
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
using static ThScoreFileConverter.Models.Th17.Parsers;
using ISpellCard = ThScoreFileConverter.Models.Th13.ISpellCard<ThScoreFileConverter.Models.Level>;

namespace ThScoreFileConverter.Models.Th17
{
    // %T17CRG[v][w][xx][y][z]
    internal class CollectRateReplacer : IStringReplaceable
    {
        private static readonly string Pattern = Utils.Format(
            @"%T17CRG([SP])({0})({1})({2})([12])",
            LevelWithTotalParser.Pattern,
            CharaWithTotalParser.Pattern,
            StageWithTotalParser.Pattern);

        private static readonly Func<ISpellCard, string, int, bool> FindByKindTypeImpl =
            (card, kind, type) =>
            {
                if (kind == "S")
                {
                    if (type == 1)
                        return card.ClearCount > 0;
                    else
                        return card.TrialCount > 0;
                }
                else
                {
                    if (type == 1)
                        return card.PracticeClearCount > 0;
                    else
                        return card.PracticeTrialCount > 0;
                }
            };

        private readonly MatchEvaluator evaluator;

        public CollectRateReplacer(IReadOnlyDictionary<CharaWithTotal, IClearData> clearDataDictionary)
        {
            if (clearDataDictionary is null)
                throw new ArgumentNullException(nameof(clearDataDictionary));

            this.evaluator = new MatchEvaluator(match =>
            {
                var kind = match.Groups[1].Value.ToUpperInvariant();
                var level = LevelWithTotalParser.Parse(match.Groups[2].Value);
                var chara = CharaWithTotalParser.Parse(match.Groups[3].Value);
                var stage = StageWithTotalParser.Parse(match.Groups[4].Value);
                var type = int.Parse(match.Groups[5].Value, CultureInfo.InvariantCulture);

                if (stage == StageWithTotal.Extra)
                    return match.ToString();

                bool FindByKindType(ISpellCard card) => FindByKindTypeImpl(card, kind, type);

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

                var cards = clearDataDictionary.TryGetValue(chara, out var clearData)
                    ? clearData.Cards : new Dictionary<int, ISpellCard>();
                return cards.Values
                    .Count(Utils.MakeAndPredicate(FindByKindType, findByLevel, findByStage))
                    .ToString(CultureInfo.CurrentCulture);
            });
        }

        public string Replace(string input) => Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
    }
}
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

namespace ThScoreFileConverter.Models.Th15
{
    // %T15CRG[v][w][xx][y][z]
    internal class CollectRateReplacer : IStringReplaceable
    {
        private static readonly string Pattern = Utils.Format(
            @"%T15CRG({0})({1})({2})({3})([12])",
            Parsers.GameModeParser.Pattern,
            Parsers.LevelWithTotalParser.Pattern,
            Parsers.CharaWithTotalParser.Pattern,
            Parsers.StageWithTotalParser.Pattern);

        private readonly MatchEvaluator evaluator;

        public CollectRateReplacer(IReadOnlyDictionary<CharaWithTotal, IClearData> clearDataDictionary)
        {
            if (clearDataDictionary is null)
                throw new ArgumentNullException(nameof(clearDataDictionary));

            this.evaluator = new MatchEvaluator(match =>
            {
                var mode = Parsers.GameModeParser.Parse(match.Groups[1].Value);
                var level = Parsers.LevelWithTotalParser.Parse(match.Groups[2].Value);
                var chara = Parsers.CharaWithTotalParser.Parse(match.Groups[3].Value);
                var stage = Parsers.StageWithTotalParser.Parse(match.Groups[4].Value);
                var type = int.Parse(match.Groups[5].Value, CultureInfo.InvariantCulture);

                if (stage == StageWithTotal.Extra)
                    return match.ToString();

#if false   // FIXME
                if (level == LevelWithTotal.Extra)
                    mode = GameMode.Pointdevice;
#endif

                Func<Th13.ISpellCard<Level>, bool> findByType;
                if (type == 1)
                    findByType = card => card.ClearCount > 0;
                else
                    findByType = card => card.TrialCount > 0;

                Func<Th13.ISpellCard<Level>, bool> findByStage;
                if (stage == StageWithTotal.Total)
                    findByStage = card => true;
                else
                    findByStage = card => Definitions.CardTable[card.Id].Stage == (Stage)stage;

                Func<Th13.ISpellCard<Level>, bool> findByLevel = card => true;
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

                return clearDataDictionary.TryGetValue(chara, out var clearData)
                    && clearData.GameModeData.TryGetValue(mode, out var clearDataPerGameMode)
                    ? clearDataPerGameMode.Cards.Values
                        .Count(Utils.MakeAndPredicate(findByType, findByLevel, findByStage))
                        .ToString(CultureInfo.CurrentCulture)
                    : "0";
            });
        }

        public string Replace(string input) => Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
    }
}

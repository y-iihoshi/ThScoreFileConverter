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
using static ThScoreFileConverter.Models.Th18.Parsers;
using IClearData = ThScoreFileConverter.Models.Th13.IClearData<
    ThScoreFileConverter.Models.Th18.CharaWithTotal,
    ThScoreFileConverter.Models.Level,
    ThScoreFileConverter.Models.Level,
    ThScoreFileConverter.Models.Th14.LevelPracticeWithTotal,
    ThScoreFileConverter.Models.Stage,
    ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th13.StageProgress>>;

namespace ThScoreFileConverter.Models.Th18
{
    // %T18CRG[v][w][xx][y][z]
    internal class CollectRateReplacer : IStringReplaceable
    {
        private static readonly string Pattern = Utils.Format(
            @"%T18CRG([SP])({0})({1})({2})([12])",
            LevelWithTotalParser.Pattern,
            CharaWithTotalParser.Pattern,
            StageWithTotalParser.Pattern);

        private readonly MatchEvaluator evaluator;

        public CollectRateReplacer(
            IReadOnlyDictionary<CharaWithTotal, IClearData> clearDataDictionary, INumberFormatter formatter)
        {
            this.evaluator = new MatchEvaluator(match => EvaluatorImpl(match, clearDataDictionary, formatter));

            static string EvaluatorImpl(
                Match match,
                IReadOnlyDictionary<CharaWithTotal, IClearData> clearDataDictionary,
                INumberFormatter formatter)
            {
                var kind = match.Groups[1].Value.ToUpperInvariant();
                var level = LevelWithTotalParser.Parse(match.Groups[2].Value);
                var chara = CharaWithTotalParser.Parse(match.Groups[3].Value);
                var stage = StageWithTotalParser.Parse(match.Groups[4].Value);
                var type = IntegerHelper.Parse(match.Groups[5].Value);

                if (stage == StageWithTotal.Extra)
                    return match.ToString();

                Func<Th13.ISpellCard<Level>, bool> findByKindType = (kind, type) switch
                {
                    ("S", 1) => card => card.ClearCount > 0,
                    ("S", _) => card => card.TrialCount > 0,
                    (_, 1) => card => card.PracticeClearCount > 0,
                    _ => card => card.PracticeTrialCount > 0,
                };

                Func<Th13.ISpellCard<Level>, bool> findByLevel = level switch
                {
                    LevelWithTotal.Total => FuncHelper.True,
                    LevelWithTotal.Extra => FuncHelper.True,
                    _ => card => card.Level == (Level)level,
                };

                Func<Th13.ISpellCard<Level>, bool> findByStage = (level, stage) switch
                {
                    (LevelWithTotal.Extra, _) => card => Definitions.CardTable[card.Id].Stage == Stage.Extra,
                    (_, StageWithTotal.Total) => FuncHelper.True,
                    _ => card => Definitions.CardTable[card.Id].Stage == (Stage)stage,
                };

                return formatter.FormatNumber(
                    clearDataDictionary.TryGetValue(chara, out var clearData)
                    ? clearData.Cards.Values
                        .Count(FuncHelper.MakeAndPredicate(findByKindType, findByLevel, findByStage))
                    : default);
            }
        }

        public string Replace(string input)
        {
            return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
        }
    }
}

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
using IClearData = ThScoreFileConverter.Models.Th13.IClearData<
    ThScoreFileConverter.Models.Th13.CharaWithTotal,
    ThScoreFileConverter.Models.Th13.LevelPractice,
    ThScoreFileConverter.Models.Th13.LevelPractice,
    ThScoreFileConverter.Models.Th13.LevelPracticeWithTotal,
    ThScoreFileConverter.Models.Th13.StagePractice>;

namespace ThScoreFileConverter.Models.Th13
{
    // %T13CRG[v][w][xx][y][z]
    internal class CollectRateReplacer : IStringReplaceable
    {
        private static readonly string Pattern = Utils.Format(
            @"%T13CRG([SP])({0})({1})({2})([12])",
            Parsers.LevelPracticeWithTotalParser.Pattern,
            Parsers.CharaWithTotalParser.Pattern,
            Parsers.StageWithTotalParser.Pattern);

        private readonly MatchEvaluator evaluator;

        public CollectRateReplacer(IReadOnlyDictionary<CharaWithTotal, IClearData> clearDataDictionary)
        {
            if (clearDataDictionary is null)
                throw new ArgumentNullException(nameof(clearDataDictionary));

            this.evaluator = new MatchEvaluator(match => EvaluatorImpl(match, clearDataDictionary));

            static string EvaluatorImpl(
                Match match, IReadOnlyDictionary<CharaWithTotal, IClearData> clearDataDictionary)
            {
                var kind = match.Groups[1].Value.ToUpperInvariant();
                var level = Parsers.LevelPracticeWithTotalParser.Parse(match.Groups[2].Value);
                var chara = Parsers.CharaWithTotalParser.Parse(match.Groups[3].Value);
                var stage = Parsers.StageWithTotalParser.Parse(match.Groups[4].Value);
                var type = int.Parse(match.Groups[5].Value, CultureInfo.InvariantCulture);

                if (stage == StageWithTotal.Extra)
                    return match.ToString();
                if ((kind == "S") && (level == LevelPracticeWithTotal.OverDrive))
                    return match.ToString();

#pragma warning disable IDE0007 // Use implicit type
                Func<ISpellCard<LevelPractice>, bool> findByKindType = (kind, type) switch
                {
                    ("S", 1) => card => (card.Level != LevelPractice.OverDrive) && (card.ClearCount > 0),
                    ("S", _) => card => (card.Level != LevelPractice.OverDrive) && (card.TrialCount > 0),
                    (_, 1) => card => card.PracticeClearCount > 0,
                    _ => card => card.PracticeTrialCount > 0,
                };

                Func<ISpellCard<LevelPractice>, bool> findByLevel = level switch
                {
                    LevelPracticeWithTotal.Total => Utils.True,
                    LevelPracticeWithTotal.Extra => Utils.True,
                    LevelPracticeWithTotal.OverDrive => Utils.True,
                    _ => card => card.Level == (LevelPractice)level,
                };

                Func<ISpellCard<LevelPractice>, bool> findByStage = (level, stage) switch
                {
                    (LevelPracticeWithTotal.Extra, _) =>
                        card => Definitions.CardTable[card.Id].Stage == StagePractice.Extra,
                    (LevelPracticeWithTotal.OverDrive, _) =>
                        card => Definitions.CardTable[card.Id].Stage == StagePractice.OverDrive,
                    (_, StageWithTotal.Total) => Utils.True,
                    _ => card => Definitions.CardTable[card.Id].Stage == (StagePractice)stage,
                };
#pragma warning restore IDE0007 // Use implicit type

                return Utils.ToNumberString(
                    clearDataDictionary.TryGetValue(chara, out var clearData)
                    ? clearData.Cards.Values.Count(Utils.MakeAndPredicate(findByKindType, findByLevel, findByStage))
                    : default);
            }
        }

        public string Replace(string input)
        {
            return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
        }
    }
}

//-----------------------------------------------------------------------
// <copyright file="CollectRateReplacerBase.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Text.RegularExpressions;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th10;

internal class CollectRateReplacerBase<TCharaWithTotal> : IStringReplaceable
    where TCharaWithTotal : struct, Enum
{
    private static readonly IntegerParser TypeParser = new(@"[12]");

    private readonly string pattern;
    private readonly MatchEvaluator evaluator;

    protected CollectRateReplacerBase(
        string formatPrefix,
        IRegexParser<LevelWithTotal> levelWithTotalParser,
        IRegexParser<TCharaWithTotal> charaWithTotalParser,
        IRegexParser<StageWithTotal> stageWithTotalParser,
        IReadOnlyDictionary<int, SpellCardInfo<Stage, Level>> cardTable,
        IReadOnlyDictionary<TCharaWithTotal, IClearData<TCharaWithTotal>> clearDataDictionary,
        INumberFormatter formatter)
    {
        this.pattern = StringHelper.Create(
            $"{formatPrefix}CRG({levelWithTotalParser.Pattern})({charaWithTotalParser.Pattern})({stageWithTotalParser.Pattern})({TypeParser.Pattern})");
        this.evaluator = new MatchEvaluator(match =>
        {
            var level = levelWithTotalParser.Parse(match.Groups[1]);
            var chara = charaWithTotalParser.Parse(match.Groups[2]);
            var stage = stageWithTotalParser.Parse(match.Groups[3]);
            var type = TypeParser.Parse(match.Groups[4]);

            if (stage == StageWithTotal.Extra)
                return match.ToString();

#pragma warning disable IDE0072 // Add missing cases to switch expression
            Func<ISpellCard<Level>, bool> findByLevel = level switch
            {
                LevelWithTotal.Total => FuncHelper.True,
                LevelWithTotal.Extra => FuncHelper.True,
                _ => card => card.Level == (Level)level,
            };
#pragma warning restore IDE0072 // Add missing cases to switch expression

            Func<ISpellCard<Level>, bool> findByStage = (level, stage) switch
            {
                (LevelWithTotal.Extra, _) => card => cardTable[card.Id].Stage == Stage.Extra,
                (_, StageWithTotal.Total) => FuncHelper.True,
                _ => card => cardTable[card.Id].Stage == (Stage)stage,
            };

            Func<ISpellCard<Level>, bool> findByType = type switch
            {
                1 => card => card.ClearCount > 0,
                _ => card => card.TrialCount > 0,
            };

            return formatter.FormatNumber(
                clearDataDictionary.TryGetValue(chara, out var clearData)
                ? clearData.Cards.Values.Count(FuncHelper.MakeAndPredicate(findByLevel, findByStage, findByType))
                : default);
        });
    }

    public string Replace(string input)
    {
        return Regex.Replace(input, this.pattern, this.evaluator, RegexOptions.IgnoreCase);
    }
}

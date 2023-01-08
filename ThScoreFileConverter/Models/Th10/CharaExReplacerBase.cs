//-----------------------------------------------------------------------
// <copyright file="CharaExReplacerBase.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th10;

internal class CharaExReplacerBase<TCharaWithTotal> : IStringReplaceable
    where TCharaWithTotal : struct, Enum
{
    private readonly string pattern;
    private readonly MatchEvaluator evaluator;

    protected CharaExReplacerBase(
        string formatPrefix,
        EnumShortNameParser<LevelWithTotal> levelWithTotalParser,
        EnumShortNameParser<TCharaWithTotal> charaWithTotalParser,
        Func<LevelWithTotal, bool> levelIsTotal,
        Func<TCharaWithTotal, bool> charaIsTotal,
        IReadOnlyDictionary<TCharaWithTotal, IClearData<TCharaWithTotal>> clearDataDictionary,
        INumberFormatter formatter)
    {
        this.pattern = StringHelper.Create(
            $"{formatPrefix}CHARAEX({levelWithTotalParser.Pattern})({charaWithTotalParser.Pattern})([1-3])");
        this.evaluator = new MatchEvaluator(match =>
        {
            var level = levelWithTotalParser.Parse(match.Groups[1].Value);
            var chara = charaWithTotalParser.Parse(match.Groups[2].Value);
            var type = IntegerHelper.Parse(match.Groups[3].Value);

            Func<IClearData<TCharaWithTotal>, long> getValueByType = (level, type) switch
            {
                (_, 1) => clearData => clearData.TotalPlayCount,
                (_, 2) => clearData => clearData.PlayTime,
                _ when levelIsTotal(level) => clearData => clearData.ClearCounts.Values.Sum(),
                _ => clearData => clearData.ClearCounts.TryGetValue((Level)level, out var count) ? count : default,
            };

            Func<IReadOnlyDictionary<TCharaWithTotal, IClearData<TCharaWithTotal>>, long> getValueByChara = chara switch
            {
                _ when charaIsTotal(chara) => dictionary => dictionary.Values
                    .Where(clearData => !charaIsTotal(clearData.Chara)).Sum(getValueByType),
                _ => dictionary => dictionary.TryGetValue(chara, out var clearData)
                    ? getValueByType(clearData) : default,
            };

            Func<long, string> toString = type switch
            {
                2 => value => new Time(value).ToString(),
                _ => formatter.FormatNumber,
            };

            return toString(getValueByChara(clearDataDictionary));
        });
    }

    public string Replace(string input)
    {
        return Regex.Replace(input, this.pattern, this.evaluator, RegexOptions.IgnoreCase);
    }
}

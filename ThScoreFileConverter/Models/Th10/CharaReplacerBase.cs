//-----------------------------------------------------------------------
// <copyright file="CharaReplacerBase.cs" company="None">
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

internal class CharaReplacerBase<TCharaWithTotal> : IStringReplaceable
    where TCharaWithTotal : struct, Enum
{
    private readonly string pattern;
    private readonly MatchEvaluator evaluator;

    protected CharaReplacerBase(
        string formatPrefix,
        IRegexParser<TCharaWithTotal> charaWithTotalParser,
        Func<TCharaWithTotal, bool> isTotal,
        IReadOnlyDictionary<TCharaWithTotal, IClearData<TCharaWithTotal>> clearDataDictionary,
        INumberFormatter formatter)
    {
        this.pattern = StringHelper.Create($"{formatPrefix}CHARA({charaWithTotalParser.Pattern})([1-3])");
        this.evaluator = new MatchEvaluator(match =>
        {
            var chara = charaWithTotalParser.Parse(match.Groups[1]);
            var type = IntegerHelper.Parse(match.Groups[2].Value);

            Func<IClearData<TCharaWithTotal>, long> getValueByType = type switch
            {
                1 => clearData => clearData.TotalPlayCount,
                2 => clearData => clearData.PlayTime,
                _ => clearData => clearData.ClearCounts.Values.Sum(),
            };

            Func<IReadOnlyDictionary<TCharaWithTotal, IClearData<TCharaWithTotal>>, long> getValueByChara = chara switch
            {
                _ when isTotal(chara) => dictionary => dictionary.Values
                    .Where(clearData => !isTotal(clearData.Chara)).Sum(getValueByType),
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

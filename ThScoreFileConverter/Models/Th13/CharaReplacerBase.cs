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

namespace ThScoreFileConverter.Models.Th13;

internal class CharaReplacerBase<TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TScoreData>
    : IStringReplaceable
    where TChWithT : struct, Enum
    where TLv : struct, Enum
    where TLvPrac : struct, Enum
    where TLvPracWithT : struct, Enum
    where TStPrac : struct, Enum
    where TScoreData : Th10.IScoreData<StageProgress>
{
    private readonly string pattern;
    private readonly MatchEvaluator evaluator;

    protected CharaReplacerBase(
        string formatPrefix,
        IRegexParser<TChWithT> charaWithTotalParser,
        Func<TChWithT, bool> charaIsTotal,
        Func<TLvPracWithT, bool> levelIsToBeSummed,
        Func<long, Time> createTime,
        IReadOnlyDictionary<TChWithT, IClearData<
            TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TScoreData>> clearDataDictionary,
        INumberFormatter formatter)
    {
        this.pattern = StringHelper.Create($"{formatPrefix}CHARA({charaWithTotalParser.Pattern})([1-3])");
        this.evaluator = new MatchEvaluator(match =>
        {
            var chara = charaWithTotalParser.Parse(match.Groups[1]);
            var type = IntegerHelper.Parse(match.Groups[2].Value);

            Func<IClearData<TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TScoreData>, long> getValueByType = type switch
            {
                1 => clearData => clearData.TotalPlayCount,
                2 => clearData => clearData.PlayTime,
                _ => clearData => clearData.ClearCounts
                    .Where(pair => levelIsToBeSummed(pair.Key)).Sum(pair => pair.Value),
            };

            Func<IReadOnlyDictionary<TChWithT, IClearData<TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TScoreData>>, long> getValueByChara = chara switch
            {
                _ when charaIsTotal(chara) => dictionary => dictionary.Values
                    .Where(clearData => !charaIsTotal(clearData.Chara)).Sum(getValueByType),
                _ => dictionary => dictionary.TryGetValue(chara, out var clearData)
                    ? getValueByType(clearData) : default,
            };

            Func<long, string> toString = type switch
            {
                2 => value => createTime(value).ToString(),
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

//-----------------------------------------------------------------------
// <copyright file="CharaExReplacerBase.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Text.RegularExpressions;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th13;

internal class CharaExReplacerBase<TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TScoreData> : IStringReplaceable
    where TChWithT : struct, Enum
    where TLv : struct, Enum
    where TLvPrac : struct, Enum
    where TLvPracWithT : struct, Enum
    where TStPrac : struct, Enum
    where TScoreData : Th10.IScoreData<StageProgress>
{
    private static readonly IntegerParser TypeParser = new(@"[1-3]");

    private readonly string pattern;
    private readonly MatchEvaluator evaluator;

    protected CharaExReplacerBase(
        string formatPrefix,
        IRegexParser<LevelWithTotal> levelWithTotalParser,
        IRegexParser<TChWithT> charaWithTotalParser,
        Func<LevelWithTotal, bool> levelIsTotal,
        Func<TChWithT, bool> charaIsTotal,
        Func<TLvPracWithT, bool> levelIsToBeSummed,
        Func<long, Time> createTime,
        IReadOnlyDictionary<TChWithT, IClearData<
            TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TScoreData>> clearDataDictionary,
        INumberFormatter formatter)
    {
        this.pattern = StringHelper.Create(
            $"{formatPrefix}CHARAEX({levelWithTotalParser.Pattern})({charaWithTotalParser.Pattern})({TypeParser.Pattern})");
        this.evaluator = new MatchEvaluator(match =>
        {
            var level = levelWithTotalParser.Parse(match.Groups[1]);
            var chara = charaWithTotalParser.Parse(match.Groups[2]);
            var type = TypeParser.Parse(match.Groups[3]);

            Func<IClearData<TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TScoreData>, long> getValueByType = (level, type) switch
            {
                (_, 1) => clearData => clearData.TotalPlayCount,
                (_, 2) => clearData => clearData.PlayTime,
                _ when levelIsTotal(level) => clearData => clearData.ClearCounts
                    .Where(pair => levelIsToBeSummed(pair.Key)).Sum(pair => pair.Value),
                _ => clearData => clearData.ClearCounts
                    .TryGetValue(EnumHelper.To<TLvPracWithT>(level), out var count)
                    ? count : default,
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

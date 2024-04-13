//-----------------------------------------------------------------------
// <copyright file="ClearReplacerBase.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ThScoreFileConverter.Core.Extensions;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Helpers;
using IScoreData = ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th13.StageProgress>;

namespace ThScoreFileConverter.Models.Th13;

internal class ClearReplacerBase<
    TCh, TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TScoreData> : IStringReplaceable
    where TCh : struct, Enum
    where TChWithT : struct, Enum
    where TLv : struct, Enum
    where TLvPrac : struct, Enum
    where TLvPracWithT : struct, Enum
    where TStPrac : struct, Enum
    where TScoreData : IScoreData
{
    private readonly string pattern;
    private readonly MatchEvaluator evaluator;

    protected ClearReplacerBase(
        string formatPrefix,
        EnumShortNameParser<Level> levelParser,
        EnumShortNameParser<TCh> charaParser,
        IReadOnlyDictionary<
            TChWithT, IClearData<TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, IScoreData>> clearDataDictionary)
        : this(
              formatPrefix,
              levelParser,
              charaParser,
              (level, chara) => GetRanking(clearDataDictionary, level, chara))
    {
    }

    protected ClearReplacerBase(
        string formatPrefix,
        EnumShortNameParser<Level> levelParser,
        EnumShortNameParser<TCh> charaParser,
        Func<Level, TCh, IReadOnlyList<IScoreData>> getRanking)
    {
        this.pattern = StringHelper.Create($"{formatPrefix}CLEAR({levelParser.Pattern})({charaParser.Pattern})");
        this.evaluator = new MatchEvaluator(match =>
        {
            var level = levelParser.Parse(match.Groups[1].Value);
            var chara = charaParser.Parse(match.Groups[2].Value);

            var scores = getRanking(level, chara).Where(static score => score.DateTime > 0);
            var stageProgress = scores.Any() ? scores.Max(static score => score.StageProgress) : StageProgress.None;

            if (stageProgress == StageProgress.Extra)
                return "Not Clear";
            else if (stageProgress == StageProgress.ExtraClear)
                return StageProgress.Clear.ToShortName();
            else
                return stageProgress.ToShortName();
        });
    }

    public string Replace(string input)
    {
        return Regex.Replace(input, this.pattern, this.evaluator, RegexOptions.IgnoreCase);
    }

    private static IReadOnlyList<IScoreData> GetRanking(
        IReadOnlyDictionary<
            TChWithT, IClearData<TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, IScoreData>> dictionary,
        Level level,
        TCh chara)
    {
        return dictionary.TryGetValue(EnumHelper.To<TChWithT>(chara), out var clearData)
            && clearData.Rankings.TryGetValue(EnumHelper.To<TLvPracWithT>(level), out var ranking)
            ? ranking : [];
    }
}

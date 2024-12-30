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
using IScoreData = ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th10.StageProgress>;

namespace ThScoreFileConverter.Models.Th10;

internal class ClearReplacerBase<TChara, TCharaWithTotal> : IStringReplaceable
    where TChara : struct, Enum
    where TCharaWithTotal : struct, Enum
{
    private readonly string pattern;
    private readonly MatchEvaluator evaluator;

    protected ClearReplacerBase(
        string formatPrefix,
        IRegexParser<Level> levelParser,
        IRegexParser<TChara> charaParser,
        IReadOnlyDictionary<TCharaWithTotal, IClearData<TCharaWithTotal>> clearDataDictionary)
    {
        this.pattern = StringHelper.Create($"{formatPrefix}CLEAR({levelParser.Pattern})({charaParser.Pattern})");
        this.evaluator = new MatchEvaluator(match =>
        {
            var level = levelParser.Parse(match.Groups[1]);
            var chara = charaParser.Parse(match.Groups[2]);

            var scores = GetRanking(clearDataDictionary, level, chara).Where(static score => score.DateTime > 0);
            var stageProgress = scores.Any() ? scores.Max(static score => score.StageProgress) : StageProgress.None;

            return (stageProgress == StageProgress.Extra) ? "Not Clear" : stageProgress.ToShortName();
        });
    }

    public string Replace(string input)
    {
        return Regex.Replace(input, this.pattern, this.evaluator, RegexOptions.IgnoreCase);
    }

    private static IReadOnlyList<IScoreData> GetRanking(
        IReadOnlyDictionary<TCharaWithTotal, IClearData<TCharaWithTotal>> dictionary, Level level, TChara chara)
    {
        return dictionary.TryGetValue(EnumHelper.To<TCharaWithTotal>(chara), out var clearData)
            && clearData.Rankings.TryGetValue(level, out var ranking)
            ? ranking : [];
    }
}

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
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th06;

internal class ClearReplacerBase<TLevel, TChara, TStageProgress> : IStringReplaceable
    where TLevel : struct, Enum
    where TChara : struct, Enum
    where TStageProgress : struct, Enum
{
    private readonly string pattern;
    private readonly MatchEvaluator evaluator;

    protected ClearReplacerBase(
        string formatPrefix,
        IRegexParser<TLevel> levelParser,
        IRegexParser<TChara> charaParser,
        Func<TLevel, TChara, IReadOnlyList<IHighScore<TChara, TLevel, TStageProgress>>?> getRanking,
        Func<TStageProgress, bool> isAdditionalStage)
    {
        this.pattern = StringHelper.Create($"{formatPrefix}CLEAR({levelParser.Pattern})({charaParser.Pattern})");
        this.evaluator = new MatchEvaluator(match =>
        {
            var level = levelParser.Parse(match.Groups[1]);
            var chara = charaParser.Parse(match.Groups[2]);

            if (getRanking(level, chara) is { } ranking)
            {
                var stageProgress = ranking.Count > 0 ? ranking.Max(static score => score.StageProgress) : default;
                return isAdditionalStage(stageProgress) ? "Not Clear" : stageProgress.ToDisplayName();
            }
            else
            {
                return StageProgress.None.ToDisplayName();
            }
        });
    }

    public string Replace(string input)
    {
        return Regex.Replace(input, this.pattern, this.evaluator, RegexOptions.IgnoreCase);
    }
}

﻿//-----------------------------------------------------------------------
// <copyright file="ClearReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Text.RegularExpressions;
using ThScoreFileConverter.Core.Extensions;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th08;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th08;

// %T08CLEAR[x][yy]
internal sealed class ClearReplacer(
    IReadOnlyDictionary<(Chara Chara, Level Level), IReadOnlyList<IHighScore>> rankings,
    IReadOnlyDictionary<CharaWithTotal, IClearData> clearData)
    : IStringReplaceable
{
    private static readonly string Pattern = StringHelper.Create(
        $"{Definitions.FormatPrefix}CLEAR({Parsers.LevelParser.Pattern})({Parsers.CharaParser.Pattern})");

    private readonly MatchEvaluator evaluator = new(match =>
    {
        var level = Parsers.LevelParser.Parse(match.Groups[1]);
        var chara = Parsers.CharaParser.Parse(match.Groups[2]);

        var key = (chara, level);
        if (rankings.TryGetValue(key, out var ranking) && ranking.Any())
        {
            var stageProgress = ranking.Max(rank => rank.StageProgress);
            if (stageProgress is StageProgress.FourUncanny or StageProgress.FourPowerful)
            {
                return "Stage 4";
            }
            else if (stageProgress == StageProgress.Extra)
            {
                return "Not Clear";
            }
            else if (stageProgress == StageProgress.Clear)
            {
                if ((level != Level.Extra) &&
                    ((clearData[(CharaWithTotal)chara].StoryFlags[level]
                        & PlayableStages.Stage6B) != PlayableStages.Stage6B))
                    return "FinalA Clear";
                else
                    return stageProgress.ToDisplayName();
            }
            else
            {
                return stageProgress.ToDisplayName();
            }
        }
        else
        {
            return "-------";
        }
    });

    public string Replace(string input)
    {
        return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
    }
}

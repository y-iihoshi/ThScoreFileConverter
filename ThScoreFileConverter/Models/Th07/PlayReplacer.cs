﻿//-----------------------------------------------------------------------
// <copyright file="PlayReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Text.RegularExpressions;
using ThScoreFileConverter.Core.Models.Th07;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th07;

// %T07PLAY[x][yy]
internal sealed class PlayReplacer(PlayStatus playStatus, INumberFormatter formatter) : IStringReplaceable
{
    private static readonly string Pattern = StringHelper.Create(
        $"{Definitions.FormatPrefix}PLAY({Parsers.LevelWithTotalParser.Pattern})({Parsers.CharaWithTotalParser.Pattern}|CL|CN|PR|RT)");

    private readonly MatchEvaluator evaluator = new(match =>
    {
        var level = Parsers.LevelWithTotalParser.Parse(match.Groups[1]);
        var charaAndMore = match.Groups[2].Value.ToUpperInvariant();

        var playCount = playStatus.PlayCounts[level];
        switch (charaAndMore)
        {
            case "CL":  // clear count
                return formatter.FormatNumber(playCount.TotalClear);
            case "CN":  // continue count
                return formatter.FormatNumber(playCount.TotalContinue);
            case "PR":  // practice count
                return formatter.FormatNumber(playCount.TotalPractice);
            case "RT":  // retry count
                return formatter.FormatNumber(playCount.TotalRetry);
            default:
                {
                    var chara = Parsers.CharaWithTotalParser.Parse(match.Groups[2]);
                    return formatter.FormatNumber((chara == CharaWithTotal.Total)
                        ? playCount.TotalTrial : playCount.Trials[(Chara)chara]);
                }
        }
    });

    public string Replace(string input)
    {
        return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
    }
}

﻿//-----------------------------------------------------------------------
// <copyright file="ScoreReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Text.RegularExpressions;
using ThScoreFileConverter.Core.Extensions;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th06;
using ThScoreFileConverter.Helpers;
using IHighScore = ThScoreFileConverter.Models.Th06.IHighScore<
    ThScoreFileConverter.Core.Models.Th06.Chara,
    ThScoreFileConverter.Core.Models.Level,
    ThScoreFileConverter.Models.Th06.StageProgress>;

namespace ThScoreFileConverter.Models.Th06;

// %T06SCR[w][xx][y][z]
internal sealed class ScoreReplacer(
    IReadOnlyDictionary<(Chara Chara, Level Level), IReadOnlyList<IHighScore>> rankings,
    INumberFormatter formatter)
    : IStringReplaceable
{
    private static readonly IntegerParser RankParser = new(@"\d");
    private static readonly IntegerParser TypeParser = new(@"[1-3]");
    private static readonly string Pattern = StringHelper.Create(
        $"{Definitions.FormatPrefix}SCR({Parsers.LevelParser.Pattern})({Parsers.CharaParser.Pattern})({RankParser.Pattern})({TypeParser.Pattern})");

    private readonly MatchEvaluator evaluator = new(match =>
    {
        var level = Parsers.LevelParser.Parse(match.Groups[1]);
        var chara = Parsers.CharaParser.Parse(match.Groups[2]);
        var rank = IntegerHelper.ToZeroBased(RankParser.Parse(match.Groups[3]));
        var type = TypeParser.Parse(match.Groups[4]);

        var key = (chara, level);
        var score = (rankings.TryGetValue(key, out var ranking) && (rank < ranking.Count))
            ? ranking[rank] : Definitions.InitialRanking[rank];

        return type switch
        {
            1 => EncodingHelper.Default.GetString([.. score.Name]).Split('\0')[0],
            2 => formatter.FormatNumber(score.Score),
            3 => score.StageProgress.ToDisplayName(),
            _ => match.ToString(),  // unreachable
        };
    });

    public string Replace(string input)
    {
        return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
    }
}

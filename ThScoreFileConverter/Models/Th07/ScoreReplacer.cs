//-----------------------------------------------------------------------
// <copyright file="ScoreReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ThScoreFileConverter.Core.Extensions;
using ThScoreFileConverter.Core.Models.Th07;
using ThScoreFileConverter.Helpers;
using IHighScore = ThScoreFileConverter.Models.Th07.IHighScore<
    ThScoreFileConverter.Core.Models.Th07.Chara,
    ThScoreFileConverter.Core.Models.Th07.Level,
    ThScoreFileConverter.Models.Th07.StageProgress>;

namespace ThScoreFileConverter.Models.Th07;

// %T07SCR[w][xx][y][z]
internal sealed class ScoreReplacer(
    IReadOnlyDictionary<(Chara Chara, Level Level), IReadOnlyList<IHighScore>> rankings,
    INumberFormatter formatter)
    : IStringReplaceable
{
    private static readonly Core.Models.IntegerParser RankParser = new(@"\d");
    private static readonly Core.Models.IntegerParser TypeParser = new(@"[1-5]");
    private static readonly string Pattern = StringHelper.Create(
        $@"{Definitions.FormatPrefix}SCR({Parsers.LevelParser.Pattern})({Parsers.CharaParser.Pattern})({RankParser.Pattern})({TypeParser.Pattern})");

    private readonly MatchEvaluator evaluator = new(match =>
    {
        var level = Parsers.LevelParser.Parse(match.Groups[1]);
        var chara = Parsers.CharaParser.Parse(match.Groups[2].Value);
        var rank = IntegerHelper.ToZeroBased(RankParser.Parse(match.Groups[3]));
        var type = TypeParser.Parse(match.Groups[4]);

        var key = (chara, level);
        var score = (rankings.TryGetValue(key, out var ranking) && (rank < ranking.Count))
            ? ranking[rank] : Definitions.InitialRanking[rank];

        return type switch
        {
            1 => EncodingHelper.Default.GetString(score.Name.ToArray()).Split('\0')[0],
            2 => formatter.FormatNumber((score.Score * 10) + score.ContinueCount),
            3 => score.StageProgress.ToDisplayName(),
            4 => EncodingHelper.Default.GetString(score.Date.ToArray()).TrimEnd('\0'),
            5 => formatter.FormatPercent(score.SlowRate, 3),
            _ => match.ToString(),  // unreachable
        };
    });

    public string Replace(string input)
    {
        return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
    }
}

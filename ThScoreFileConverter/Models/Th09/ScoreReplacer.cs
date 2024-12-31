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
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th09;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th09;

// %T09SCR[w][xx][y][z]
internal sealed class ScoreReplacer(
    IReadOnlyDictionary<(Chara Chara, Level Level), IReadOnlyList<IHighScore>> rankings,
    INumberFormatter formatter)
    : IStringReplaceable
{
    private static readonly string Pattern = StringHelper.Create(
        $"{Definitions.FormatPrefix}SCR({Parsers.LevelParser.Pattern})({Parsers.CharaParser.Pattern})([1-5])([1-3])");

    private readonly MatchEvaluator evaluator = new(match =>
    {
        var level = Parsers.LevelParser.Parse(match.Groups[1]);
        var chara = Parsers.CharaParser.Parse(match.Groups[2].Value);
        var rank = IntegerHelper.ToZeroBased(IntegerHelper.Parse(match.Groups[3].Value));
        var type = IntegerHelper.Parse(match.Groups[4].Value);

        var score = rankings.TryGetValue((chara, level), out var highScores) && (rank < highScores.Count)
            ? highScores[rank] : null;
        var date = string.Empty;

        switch (type)
        {
            case 1:     // name
                return (score is not null)
                    ? EncodingHelper.Default.GetString(score.Name.ToArray()).Split('\0')[0] : "--------";
            case 2:     // score
                return formatter.FormatNumber(
                    (score is not null) ? ((score.Score * 10) + score.ContinueCount) : default);
            case 3:     // date
                date = (score is not null)
                    ? EncodingHelper.Default.GetString(score.Date.ToArray()).Split('\0')[0] : "--/--";
                return (date != "--/--") ? date : "--/--/--";
            default:    // unreachable
                return match.ToString();
        }
    });

    public string Replace(string input)
    {
        return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
    }
}

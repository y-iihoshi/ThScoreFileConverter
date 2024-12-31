//-----------------------------------------------------------------------
// <copyright file="PracticeReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using System.Text.RegularExpressions;
using ThScoreFileConverter.Core.Models.Th08;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th08;

// %T08PRAC[w][xx][yy][z]
internal sealed class PracticeReplacer(
    IReadOnlyDictionary<Chara, IPracticeScore> practiceScores, INumberFormatter formatter)
    : IStringReplaceable
{
    private static readonly string Pattern = StringHelper.Create(
        $"{Definitions.FormatPrefix}PRAC({Parsers.LevelParser.Pattern})({Parsers.CharaParser.Pattern})({Parsers.StageParser.Pattern})([12])");

    private readonly MatchEvaluator evaluator = new(match =>
    {
        var level = Parsers.LevelParser.Parse(match.Groups[1]);
        var chara = Parsers.CharaParser.Parse(match.Groups[2].Value);
        var stage = Parsers.StageParser.Parse(match.Groups[3]);
        var type = IntegerHelper.Parse(match.Groups[4].Value);

        int GetValue(IPracticeScore score)
        {
            var key = (stage, level);
            return (type == 1)
                ? (score.HighScores.TryGetValue(key, out var highScore) ? (highScore * 10) : default)
                : (score.PlayCounts.TryGetValue(key, out var playCount) ? playCount : default);
        }

        return Core.Models.Definitions.CanPractice(level) && Core.Models.Th08.Definitions.CanPractice(stage)
            ? formatter.FormatNumber(
                practiceScores.TryGetValue(chara, out var score) ? GetValue(score) : default)
            : match.ToString();
    });

    public string Replace(string input)
    {
        return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
    }
}

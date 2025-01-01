//-----------------------------------------------------------------------
// <copyright file="PracticeReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using System.Text.RegularExpressions;
using ThScoreFileConverter.Core.Models.Th07;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th07;

// %T07PRAC[w][xx][y][z]
internal sealed class PracticeReplacer(
    IReadOnlyDictionary<(Chara Chara, Level Level, Stage Stage), IPracticeScore> practiceScores,
    INumberFormatter formatter)
    : IStringReplaceable
{
    private static readonly Core.Models.IntegerParser TypeParser = new(@"[12]");
    private static readonly string Pattern = StringHelper.Create(
        $"{Definitions.FormatPrefix}PRAC({Parsers.LevelParser.Pattern})({Parsers.CharaParser.Pattern})({Parsers.StageParser.Pattern})({TypeParser.Pattern})");

    private readonly MatchEvaluator evaluator = new(match =>
    {
        var level = Parsers.LevelParser.Parse(match.Groups[1]);
        var chara = Parsers.CharaParser.Parse(match.Groups[2].Value);
        var stage = Parsers.StageParser.Parse(match.Groups[3]);
        var type = TypeParser.Parse(match.Groups[4]);

        int GetValue(IPracticeScore score)
        {
            return (type == 1) ? score.HighScore * 10 : score.TrialCount;
        }

        return Core.Models.Th07.Definitions.CanPractice(level) && Core.Models.Th07.Definitions.CanPractice(stage)
            ? formatter.FormatNumber(
                practiceScores.TryGetValue((chara, level, stage), out var score) ? GetValue(score) : default)
            : match.ToString();
    });

    public string Replace(string input)
    {
        return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
    }
}

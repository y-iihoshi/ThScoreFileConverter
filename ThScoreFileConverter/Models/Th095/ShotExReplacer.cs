//-----------------------------------------------------------------------
// <copyright file="ShotExReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using ThScoreFileConverter.Core.Models.Th095;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th095;

// %T95SHOTEX[x][y][z]
internal sealed class ShotExReplacer(
    IReadOnlyDictionary<(Level Level, int Scene), (string Path, IBestShotHeader<Level> Header)> bestshots,
    IReadOnlyList<IScore> scores,
    INumberFormatter formatter,
    string outputFilePath)
    : IStringReplaceable
{
    private static readonly string Pattern = StringHelper.Create(
        $"{Definitions.FormatPrefix}SHOTEX({Parsers.LevelParser.Pattern})([1-9])([1-6])");

    private readonly MatchEvaluator evaluator = new(match =>
    {
        var level = Parsers.LevelParser.Parse(match.Groups[1]);
        var scene = IntegerHelper.Parse(match.Groups[2].Value);
        var type = IntegerHelper.Parse(match.Groups[3].Value);

        var key = (level, scene);
        if (!Definitions.SpellCards.ContainsKey(key))
            return match.ToString();

        if (bestshots.TryGetValue(key, out var bestshot))
        {
            return type switch
            {
                1 => UriHelper.GetRelativePath(outputFilePath, bestshot.Path),
                2 => bestshot.Header.Width.ToString(CultureInfo.InvariantCulture),
                3 => bestshot.Header.Height.ToString(CultureInfo.InvariantCulture),
                4 => formatter.FormatNumber(bestshot.Header.ResultScore),
                5 => formatter.FormatPercent(bestshot.Header.SlowRate, 6),
                6 => DateTimeHelper.GetString(scores.FirstOrDefault(s => (s is not null) && s.LevelScene.Equals(key))?.DateTime),
                _ => match.ToString(),
            };
        }
        else
        {
            return type switch
            {
                1 => string.Empty,
                2 => "0",
                3 => "0",
                4 => "--------",
                5 => "-----%",
                6 => DateTimeHelper.GetString(null),
                _ => match.ToString(),
            };
        }
    });

    public string Replace(string input)
    {
        return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
    }
}

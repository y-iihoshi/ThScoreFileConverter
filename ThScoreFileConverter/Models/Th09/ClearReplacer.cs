﻿//-----------------------------------------------------------------------
// <copyright file="ClearReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Text.RegularExpressions;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th09;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th09;

// %T09CLEAR[x][yy][z]
internal sealed class ClearReplacer(
    IReadOnlyDictionary<(Chara Chara, Level Level), IReadOnlyList<IHighScore>> rankings,
    IReadOnlyDictionary<Chara, IClearCount> clearCounts,
    INumberFormatter formatter)
    : IStringReplaceable
{
    private static readonly IntegerParser TypeParser = new(@"[12]");
    private static readonly string Pattern = StringHelper.Create(
        $"{Definitions.FormatPrefix}CLEAR({Parsers.LevelParser.Pattern})({Parsers.CharaParser.Pattern})({TypeParser.Pattern})");

    private readonly MatchEvaluator evaluator = new(match =>
    {
        var level = Parsers.LevelParser.Parse(match.Groups[1]);
        var chara = Parsers.CharaParser.Parse(match.Groups[2]);
        var type = TypeParser.Parse(match.Groups[3]);

        var count = clearCounts.TryGetValue(chara, out var clearCount)
            && clearCount.Counts.TryGetValue(level, out var c) ? c : default;

        if (type == 1)
        {
            return formatter.FormatNumber(count);
        }
        else
        {
            if (count > 0)
            {
                return "Cleared";
            }
            else
            {
                var score = rankings.TryGetValue((chara, level), out var ranking) && (ranking.Count > 0)
                    ? ranking[0] : null;
                var date = (score is not null)
                    ? EncodingHelper.Default.GetString([.. score.Date]).TrimEnd('\0') : "--/--";
                return (date != "--/--") ? "Not Cleared" : "-------";
            }
        }
    });

    public string Replace(string input)
    {
        return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
    }
}

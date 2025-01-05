//-----------------------------------------------------------------------
// <copyright file="ClearRankReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using System.Text.RegularExpressions;
using ThScoreFileConverter.Core.Models.Th145;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th145;

// %T145CLEAR[x][yy]
internal sealed class ClearRankReplacer(IReadOnlyDictionary<Level, IReadOnlyDictionary<Chara, int>> clearRanks)
    : IStringReplaceable
{
    private static readonly string Pattern = StringHelper.Create(
        $"{Definitions.FormatPrefix}CLEAR({Parsers.LevelParser.Pattern})({Parsers.CharaParser.Pattern})");

    private readonly MatchEvaluator evaluator = new(match =>
    {
        var level = Parsers.LevelParser.Parse(match.Groups[1]);
        var chara = Parsers.CharaParser.Parse(match.Groups[2]);

        if (clearRanks.TryGetValue(level, out var ranks) && ranks.TryGetValue(chara, out var rank))
        {
            // FIXME
            return rank switch
            {
                1 => "Bronze",
                2 => "Silver",
                3 => "Gold",
                _ => "Not Clear",
            };
        }
        else
        {
            return "Not Clear";
        }
    });

    public string Replace(string input)
    {
        return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
    }
}

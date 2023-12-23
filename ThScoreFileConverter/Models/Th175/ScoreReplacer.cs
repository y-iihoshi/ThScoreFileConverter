//-----------------------------------------------------------------------
// <copyright file="ScoreReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ThScoreFileConverter.Core.Models.Th175;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th175;

// %T175SCR[w][xx][y][z]
internal sealed class ScoreReplacer(
    IReadOnlyDictionary<(Level Level, Chara Chara), IEnumerable<int>> scoreDictionary,
    IReadOnlyDictionary<(Level Level, Chara Chara), IEnumerable<int>> timeDictionary,
    INumberFormatter formatter)
    : IStringReplaceable
{
    private static readonly string Pattern = StringHelper.Create(
        $@"{Definitions.FormatPrefix}SCR({Parsers.LevelParser.Pattern})({Parsers.CharaParser.Pattern})(\d)([12])");

    private readonly MatchEvaluator evaluator = new(match =>
    {
        static IEnumerable<int> GetRanking(
            IReadOnlyDictionary<(Level, Chara), IEnumerable<int>> dictionary, Level level, Chara chara)
        {
            return dictionary.TryGetValue((level, chara), out var ranking) ? ranking : Array.Empty<int>();
        }

        var level = Parsers.LevelParser.Parse(match.Groups[1].Value);
        var chara = Parsers.CharaParser.Parse(match.Groups[2].Value);
        var rank = IntegerHelper.ToZeroBased(IntegerHelper.Parse(match.Groups[3].Value));
        var type = IntegerHelper.Parse(match.Groups[4].Value);

        return type switch
        {
            1 => formatter.FormatNumber(GetRanking(scoreDictionary, level, chara).ElementAtOrDefault(rank)),
            2 => new Time(GetRanking(timeDictionary, level, chara).ElementAtOrDefault(rank)).ToString(),
            _ => match.ToString(),  // unreachable
        };
    });

    public string Replace(string input)
    {
        return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
    }
}

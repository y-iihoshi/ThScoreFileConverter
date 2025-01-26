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
using ThScoreFileConverter.Core.Models.Th128;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th128;

// %T128SCR[w][xx][y][z]
internal sealed class ScoreReplacer(
    IReadOnlyDictionary<RouteWithTotal, IClearData> clearDataDictionary, INumberFormatter formatter)
    : IStringReplaceable
{
    private static readonly IntegerParser RankParser = new(@"\d");
    private static readonly IntegerParser TypeParser = new(@"[1-5]");
    private static readonly string Pattern = StringHelper.Create(
        $"{Definitions.FormatPrefix}SCR({Parsers.LevelParser.Pattern})({Parsers.RouteParser.Pattern})({RankParser.Pattern})({TypeParser.Pattern})");

    private readonly MatchEvaluator evaluator = new(match =>
    {
        var level = Parsers.LevelParser.Parse(match.Groups[1]);
        var route = (RouteWithTotal)Parsers.RouteParser.Parse(match.Groups[2]);
        var rank = IntegerHelper.ToZeroBased(RankParser.Parse(match.Groups[3]));
        var type = TypeParser.Parse(match.Groups[4]);

        if ((level == Level.Extra) && (route != RouteWithTotal.Extra))
            return match.ToString();
        if ((route == RouteWithTotal.Extra) && (level != Level.Extra))
            return match.ToString();

        var ranking = clearDataDictionary.TryGetValue(route, out var clearData)
            && clearData.Rankings.TryGetValue(level, out var rankings)
            && (rank < rankings.Count)
            ? rankings[rank] : new ScoreData();
        switch (type)
        {
            case 1:     // name
                return ranking.Name.Any()
                    ? EncodingHelper.Default.GetString(ranking.Name.ToArray()).Split('\0')[0] : "--------";
            case 2:     // score
                return formatter.FormatNumber((ranking.Score * 10) + ranking.ContinueCount);
            case 3:     // stage
                if (ranking.DateTime == 0)
                    return StageProgress.None.ToDisplayName();
                return ranking.StageProgress.ToDisplayName();
            case 4:     // date & time
                return DateTimeHelper.GetString(ranking.DateTime == 0 ? null : ranking.DateTime);
            case 5:     // slow
                if (ranking.DateTime == 0)
                    return "-----%";
                return formatter.FormatPercent(ranking.SlowRate, 3);
            default:    // unreachable
                return match.ToString();
        }
    });

    public string Replace(string input)
    {
        return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
    }
}

//-----------------------------------------------------------------------
// <copyright file="ClearReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ThScoreFileConverter.Core.Extensions;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th128;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th128;

// %T128CLEAR[x][yy]
internal sealed class ClearReplacer(IReadOnlyDictionary<RouteWithTotal, IClearData> clearDataDictionary)
    : IStringReplaceable
{
    private static readonly string Pattern = StringHelper.Create(
        $"{Definitions.FormatPrefix}CLEAR({Parsers.LevelParser.Pattern})({Parsers.RouteParser.Pattern})");

    private readonly MatchEvaluator evaluator = new(match =>
    {
        var level = Parsers.LevelParser.Parse(match.Groups[1]);
        var route = (RouteWithTotal)Parsers.RouteParser.Parse(match.Groups[2]);

        if ((level == Level.Extra) && (route != RouteWithTotal.Extra))
            return match.ToString();
        if ((route == RouteWithTotal.Extra) && (level != Level.Extra))
            return match.ToString();

        var scores = clearDataDictionary.TryGetValue(route, out var clearData)
            && clearData.Rankings.TryGetValue(level, out var ranking)
            ? ranking.Where(score => score.DateTime > 0) : [];
        var stageProgress = scores.Any()
            ? scores.Max(score => score.StageProgress) : StageProgress.None;

        return stageProgress.ToShortName();
    });

    public string Replace(string input)
    {
        return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
    }
}

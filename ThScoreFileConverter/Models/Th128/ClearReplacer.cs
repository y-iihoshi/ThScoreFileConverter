//-----------------------------------------------------------------------
// <copyright file="ClearReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using ThScoreFileConverter.Core.Extensions;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th128;

namespace ThScoreFileConverter.Models.Th128;

// %T128CLEAR[x][yy]
internal class ClearReplacer : IStringReplaceable
{
    private static readonly string Pattern = Utils.Format(
        @"{0}CLEAR({1})({2})", Definitions.FormatPrefix, Parsers.LevelParser.Pattern, Parsers.RouteParser.Pattern);

    private readonly MatchEvaluator evaluator;

    public ClearReplacer(IReadOnlyDictionary<RouteWithTotal, IClearData> clearDataDictionary)
    {
        this.evaluator = new MatchEvaluator(match =>
        {
            var level = Parsers.LevelParser.Parse(match.Groups[1].Value);
            var route = (RouteWithTotal)Parsers.RouteParser.Parse(match.Groups[2].Value);

            if ((level == Level.Extra) && (route != RouteWithTotal.Extra))
                return match.ToString();
            if ((route == RouteWithTotal.Extra) && (level != Level.Extra))
                return match.ToString();

            var scores = clearDataDictionary.TryGetValue(route, out var clearData)
                && clearData.Rankings.TryGetValue(level, out var ranking)
                ? ranking.Where(score => score.DateTime > 0)
                : ImmutableList<Th10.IScoreData<StageProgress>>.Empty;
            var stageProgress = scores.Any()
                ? scores.Max(score => score.StageProgress) : StageProgress.None;

            return stageProgress.ToShortName();
        });
    }

    public string Replace(string input)
    {
        return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
    }
}

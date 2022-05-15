//-----------------------------------------------------------------------
// <copyright file="RouteExReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th128;

// %T128ROUTEEX[x][yy][z]
internal class RouteExReplacer : IStringReplaceable
{
    private static readonly string Pattern = Utils.Format(
        @"{0}ROUTEEX({1})({2})([1-3])",
        Definitions.FormatPrefix,
        Parsers.LevelWithTotalParser.Pattern,
        Parsers.RouteWithTotalParser.Pattern);

    private readonly MatchEvaluator evaluator;

    public RouteExReplacer(
        IReadOnlyDictionary<RouteWithTotal, IClearData> clearDataDictionary, INumberFormatter formatter)
    {
        this.evaluator = new MatchEvaluator(match =>
        {
            var level = Parsers.LevelWithTotalParser.Parse(match.Groups[1].Value);
            var route = Parsers.RouteWithTotalParser.Parse(match.Groups[2].Value);
            var type = IntegerHelper.Parse(match.Groups[3].Value);

            if ((level == LevelWithTotal.Extra) &&
                (route != RouteWithTotal.Extra) && (route != RouteWithTotal.Total))
                return match.ToString();
            if ((route == RouteWithTotal.Extra) &&
                (level != LevelWithTotal.Extra) && (level != LevelWithTotal.Total))
                return match.ToString();

            Func<IClearData, long> getValueByType = (level, type) switch
            {
                (_, 1) => clearData => clearData.TotalPlayCount,
                (_, 2) => clearData => clearData.PlayTime,
                (LevelWithTotal.Total, _) => clearData => clearData.ClearCounts.Values.Sum(),
                _ => clearData => clearData.ClearCounts.TryGetValue((Level)level, out var count) ? count : default,
            };

            Func<IReadOnlyDictionary<RouteWithTotal, IClearData>, long> getValueByRoute = route switch
            {
                RouteWithTotal.Total => dictionary => dictionary.Values
                    .Where(clearData => clearData.Route != route).Sum(getValueByType),
                _ => dictionary => dictionary.TryGetValue(route, out var clearData)
                    ? getValueByType(clearData) : default,
            };

            Func<long, string> toString = type switch
            {
                2 => value => new Time(value).ToString(),
                _ => formatter.FormatNumber,
            };

            return toString(getValueByRoute(clearDataDictionary));
        });
    }

    public string Replace(string input)
    {
        return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
    }
}

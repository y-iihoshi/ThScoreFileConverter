//-----------------------------------------------------------------------
// <copyright file="RouteReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th128;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th128;

// %T128ROUTE[xx][y]
internal sealed class RouteReplacer : IStringReplaceable
{
    private static readonly IntegerParser TypeParser = new(@"[1-3]");
    private static readonly string Pattern = StringHelper.Create(
        $"{Definitions.FormatPrefix}ROUTE({Parsers.RouteWithTotalParser.Pattern})({TypeParser.Pattern})");

    private readonly MatchEvaluator evaluator;

    public RouteReplacer(
        IReadOnlyDictionary<RouteWithTotal, IClearData> clearDataDictionary, INumberFormatter formatter)
    {
        this.evaluator = new MatchEvaluator(match =>
        {
            var route = Parsers.RouteWithTotalParser.Parse(match.Groups[1]);
            var type = TypeParser.Parse(match.Groups[2]);

            Func<IClearData, long> getValueByType = type switch
            {
                1 => clearData => clearData.TotalPlayCount,
                2 => clearData => clearData.PlayTime,
                _ => clearData => clearData.ClearCounts.Values.Sum(),
            };

#pragma warning disable IDE0072 // Add missing cases to switch expression
            Func<IReadOnlyDictionary<RouteWithTotal, IClearData>, long> getValueByRoute = route switch
            {
                RouteWithTotal.Total => dictionary => dictionary.Values
                    .Where(clearData => clearData.Route != route).Sum(getValueByType),
                _ => dictionary => dictionary.TryGetValue(route, out var clearData)
                    ? getValueByType(clearData) : default,
            };
#pragma warning restore IDE0072 // Add missing cases to switch expression

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

//-----------------------------------------------------------------------
// <copyright file="RouteReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace ThScoreFileConverter.Models.Th128
{
    // %T128ROUTE[xx][y]
    internal class RouteReplacer : IStringReplaceable
    {
        private static readonly string Pattern = Utils.Format(
            @"%T128ROUTE({0})([1-3])", Parsers.RouteWithTotalParser.Pattern);

        private readonly MatchEvaluator evaluator;

        public RouteReplacer(IReadOnlyDictionary<RouteWithTotal, IClearData> clearDataDictionary)
        {
            if (clearDataDictionary is null)
                throw new ArgumentNullException(nameof(clearDataDictionary));

            this.evaluator = new MatchEvaluator(match =>
            {
                var route = Parsers.RouteWithTotalParser.Parse(match.Groups[1].Value);
                var type = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);

                Func<IClearData, long> getValueByType = type switch
                {
                    1 => clearData => clearData.TotalPlayCount,
                    2 => clearData => clearData.PlayTime,
                    _ => clearData => clearData.ClearCounts.Values.Sum(),
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
                    _ => Utils.ToNumberString,
                };

                return toString(getValueByRoute(clearDataDictionary));
            });
        }

        public string Replace(string input)
        {
            return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
        }
    }
}

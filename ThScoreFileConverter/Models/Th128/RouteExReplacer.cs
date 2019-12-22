//-----------------------------------------------------------------------
// <copyright file="RouteExReplacer.cs" company="None">
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
    // %T128ROUTEEX[x][yy][z]
    internal class RouteExReplacer : IStringReplaceable
    {
        private static readonly string Pattern = Utils.Format(
            @"%T128ROUTEEX({0})({1})([1-3])",
            Parsers.LevelWithTotalParser.Pattern,
            Parsers.RouteWithTotalParser.Pattern);

        private readonly MatchEvaluator evaluator;

        public RouteExReplacer(IReadOnlyDictionary<RouteWithTotal, IClearData> clearDataDictionary)
        {
            if (clearDataDictionary is null)
                throw new ArgumentNullException(nameof(clearDataDictionary));

            this.evaluator = new MatchEvaluator(match =>
            {
                var level = Parsers.LevelWithTotalParser.Parse(match.Groups[1].Value);
                var route = Parsers.RouteWithTotalParser.Parse(match.Groups[2].Value);
                var type = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

                if ((level == LevelWithTotal.Extra) &&
                    (route != RouteWithTotal.Extra) && (route != RouteWithTotal.Total))
                    return match.ToString();
                if ((route == RouteWithTotal.Extra) &&
                    (level != LevelWithTotal.Extra) && (level != LevelWithTotal.Total))
                    return match.ToString();

                Func<IClearData, long> getValueByType;
                Func<long, string> toString;
                if (type == 1)
                {
                    getValueByType = clearData => clearData.TotalPlayCount;
                    toString = Utils.ToNumberString;
                }
                else if (type == 2)
                {
                    getValueByType = clearData => clearData.PlayTime;
                    toString = value => new Time(value).ToString();
                }
                else
                {
                    if (level == LevelWithTotal.Total)
                    {
                        getValueByType = clearData => clearData.ClearCounts.Values.Sum();
                    }
                    else
                    {
                        getValueByType =
                            clearData => clearData.ClearCounts.TryGetValue((Level)level, out var count) ? count : 0;
                    }

                    toString = Utils.ToNumberString;
                }

                Func<IReadOnlyDictionary<RouteWithTotal, IClearData>, long> getValueByRoute;
                if (route == RouteWithTotal.Total)
                {
                    getValueByRoute = dictionary => dictionary.Values
                        .Where(clearData => clearData.Route != route).Sum(getValueByType);
                }
                else
                {
                    getValueByRoute = dictionary => dictionary.TryGetValue(route, out var clearData)
                        ? getValueByType(clearData) : 0;
                }

                return toString(getValueByRoute(clearDataDictionary));
            });
        }

        public string Replace(string input)
        {
            return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
        }
    }
}

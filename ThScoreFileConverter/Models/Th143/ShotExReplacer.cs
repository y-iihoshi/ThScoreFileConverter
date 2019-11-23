//-----------------------------------------------------------------------
// <copyright file="ShotExReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace ThScoreFileConverter.Models.Th143
{
    // %T143SHOTEX[w][x][y]
    internal class ShotExReplacer : IStringReplaceable
    {
        private static readonly string Pattern = Utils.Format(
            @"%T143SHOTEX({0})([0-9])([1-4])", Parsers.DayParser.Pattern);

        private readonly MatchEvaluator evaluator;

        public ShotExReplacer(
            IReadOnlyDictionary<(Day Day, int Scene), (string Path, IBestShotHeader Header)> bestshots,
            string outputFilePath)
        {
            this.evaluator = new MatchEvaluator(match =>
            {
                var day = Parsers.DayParser.Parse(match.Groups[1].Value);
                var scene = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);
                scene = (scene == 0) ? 10 : scene;
                var type = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

                var key = (day, scene);
                if (!Definitions.SpellCards.ContainsKey(key))
                    return match.ToString();

                if (!string.IsNullOrEmpty(outputFilePath) && bestshots.TryGetValue(key, out var bestshot))
                {
                    switch (type)
                    {
                        case 1:     // relative path to the bestshot file
                            return new Uri(outputFilePath).MakeRelativeUri(new Uri(bestshot.Path)).OriginalString;
                        case 2:     // width
                            return bestshot.Header.Width.ToString(CultureInfo.InvariantCulture);
                        case 3:     // height
                            return bestshot.Header.Height.ToString(CultureInfo.InvariantCulture);
                        case 4:     // date & time
                            return new DateTime(1970, 1, 1)
                                .AddSeconds(bestshot.Header.DateTime).ToLocalTime()
                                .ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.CurrentCulture);
                        default:    // unreachable
                            return match.ToString();
                    }
                }
                else
                {
                    switch (type)
                    {
                        case 1: return string.Empty;
                        case 2: return "0";
                        case 3: return "0";
                        case 4: return "----/--/-- --:--:--";
                        default: return match.ToString();
                    }
                }
            });
        }

        public string Replace(string input) => Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
    }
}

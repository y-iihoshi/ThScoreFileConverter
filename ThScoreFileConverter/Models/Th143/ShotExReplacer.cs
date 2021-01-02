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
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th143
{
    // %T143SHOTEX[w][x][y]
    internal class ShotExReplacer : IStringReplaceable
    {
        private static readonly string Pattern = Utils.Format(
            @"%T143SHOTEX({0})([0-9])([1-4])", Parsers.DayParser.Pattern);

        private readonly MatchEvaluator evaluator;

        public ShotExReplacer(
            IReadOnlyDictionary<(Day, int), (string Path, IBestShotHeader Header)> bestshots, string outputFilePath)
        {
            this.evaluator = new MatchEvaluator(match =>
            {
                var day = Parsers.DayParser.Parse(match.Groups[1].Value);
                var scene = IntegerHelper.Parse(match.Groups[2].Value);
                scene = (scene == 0) ? 10 : scene;
                var type = IntegerHelper.Parse(match.Groups[3].Value);

                var key = (day, scene);
                if (!Definitions.SpellCards.ContainsKey(key))
                    return match.ToString();

                if (bestshots.TryGetValue(key, out var bestshot))
                {
                    switch (type)
                    {
                        case 1:     // relative path to the bestshot file
                            if (Uri.TryCreate(outputFilePath, UriKind.Absolute, out var outputFileUri) &&
                                Uri.TryCreate(bestshot.Path, UriKind.Absolute, out var bestshotUri))
                            {
                                return outputFileUri.MakeRelativeUri(bestshotUri).OriginalString;
                            }
                            else
                            {
                                return string.Empty;
                            }

                        case 2:     // width
                            return bestshot.Header.Width.ToString(CultureInfo.InvariantCulture);
                        case 3:     // height
                            return bestshot.Header.Height.ToString(CultureInfo.InvariantCulture);
                        case 4:     // date & time
                            return DateTimeHelper.GetString(bestshot.Header.DateTime);
                        default:    // unreachable
                            return match.ToString();
                    }
                }
                else
                {
                    return type switch
                    {
                        1 => string.Empty,
                        2 => "0",
                        3 => "0",
                        4 => DateTimeHelper.GetString(null),
                        _ => match.ToString(),
                    };
                }
            });
        }

        public string Replace(string input)
        {
            return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
        }
    }
}

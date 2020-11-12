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
using System.Linq;
using System.Text.RegularExpressions;

namespace ThScoreFileConverter.Models.Th095
{
    // %T95SHOTEX[x][y][z]
    internal class ShotExReplacer : IStringReplaceable
    {
        private static readonly string Pattern = Utils.Format(
            @"%T95SHOTEX({0})([1-9])([1-6])", Parsers.LevelParser.Pattern);

        private readonly MatchEvaluator evaluator;

        public ShotExReplacer(
            IReadOnlyDictionary<(Level, int), (string Path, IBestShotHeader Header)> bestshots,
            IReadOnlyList<IScore> scores,
            string outputFilePath)
        {
            if (bestshots is null)
                throw new ArgumentNullException(nameof(bestshots));
            if (scores is null)
                throw new ArgumentNullException(nameof(scores));

            this.evaluator = new MatchEvaluator(match =>
            {
                var level = Parsers.LevelParser.Parse(match.Groups[1].Value);
                var scene = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);
                var type = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

                var key = (level, scene);
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
                        case 4:     // score
                            return Utils.ToNumberString(bestshot.Header.ResultScore);
                        case 5:     // slow rate
                            return Utils.Format("{0:F6}%", bestshot.Header.SlowRate);
                        case 6:     // date & time
                            {
                                var score = scores.FirstOrDefault(
                                    elem => (elem is not null) && elem.LevelScene.Equals(key));
                                if (score is not null)
                                {
                                    return new DateTime(1970, 1, 1)
                                        .AddSeconds(score.DateTime).ToLocalTime()
                                        .ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.CurrentCulture);
                                }
                                else
                                {
                                    return "----/--/-- --:--:--";
                                }
                            }

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
                        4 => "--------",
                        5 => "-----%",
                        6 => "----/--/-- --:--:--",
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

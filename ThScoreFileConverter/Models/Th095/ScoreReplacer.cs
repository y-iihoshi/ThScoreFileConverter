//-----------------------------------------------------------------------
// <copyright file="ScoreReplacer.cs" company="None">
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
    // %T95SCR[x][y][z]
    internal class ScoreReplacer : IStringReplaceable
    {
        private static readonly string Pattern = Utils.Format(
            @"%T95SCR({0})([1-9])([1-4])", Parsers.LevelParser.Pattern);

        private readonly MatchEvaluator evaluator;

        public ScoreReplacer(IReadOnlyList<IScore> scores)
        {
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

                var score = scores.FirstOrDefault(elem => (elem is not null) && elem.LevelScene.Equals(key));

                return type switch
                {
                    1 => Utils.ToNumberString(score?.HighScore ?? default),
                    2 => Utils.ToNumberString(score?.BestshotScore ?? default),
                    3 => Utils.ToNumberString(score?.TrialCount ?? default),
                    4 => (score is not null) ? Utils.Format("{0:F3}%", score.SlowRate2) : "-----%",
                    _ => match.ToString(),  // unreachable
                };
            });
        }

        public string Replace(string input)
        {
            return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
        }
    }
}

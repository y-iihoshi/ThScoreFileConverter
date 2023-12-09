//-----------------------------------------------------------------------
// <copyright file="ScoreReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th095;

// %T95SCR[x][y][z]
internal sealed class ScoreReplacer : IStringReplaceable
{
    private static readonly string Pattern = StringHelper.Create(
        $"{Definitions.FormatPrefix}SCR({Parsers.LevelParser.Pattern})([1-9])([1-4])");

    private readonly MatchEvaluator evaluator;

    public ScoreReplacer(IReadOnlyList<IScore> scores, INumberFormatter formatter)
    {
        this.evaluator = new MatchEvaluator(match =>
        {
            var level = Parsers.LevelParser.Parse(match.Groups[1].Value);
            var scene = IntegerHelper.Parse(match.Groups[2].Value);
            var type = IntegerHelper.Parse(match.Groups[3].Value);

            var key = (level, scene);
            if (!Definitions.SpellCards.ContainsKey(key))
                return match.ToString();

            var score = scores.FirstOrDefault(elem => (elem is not null) && elem.LevelScene.Equals(key));

            return type switch
            {
                1 => formatter.FormatNumber(score?.HighScore ?? default),
                2 => formatter.FormatNumber(score?.BestshotScore ?? default),
                3 => formatter.FormatNumber(score?.TrialCount ?? default),
                4 => (score is not null) ? formatter.FormatPercent(score.SlowRate2, 3) : "-----%",
                _ => match.ToString(),  // unreachable
            };
        });
    }

    public string Replace(string input)
    {
        return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
    }
}

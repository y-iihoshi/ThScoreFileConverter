﻿//-----------------------------------------------------------------------
// <copyright file="ScoreReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ThScoreFileConverter.Core.Models.Th143;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th143;

// %T143SCR[w][x][y][z]
internal class ScoreReplacer : IStringReplaceable
{
    private static readonly string Pattern = Utils.Format(
        @"{0}SCR({1})([0-9])({2})([1-3])",
        Definitions.FormatPrefix,
        Parsers.DayParser.Pattern,
        Parsers.ItemWithTotalParser.Pattern);

    private readonly MatchEvaluator evaluator;

    public ScoreReplacer(IReadOnlyList<IScore> scores, INumberFormatter formatter)
    {
        this.evaluator = new MatchEvaluator(match =>
        {
            var day = Parsers.DayParser.Parse(match.Groups[1].Value);
            var scene = IntegerHelper.Parse(match.Groups[2].Value);
            scene = (scene == 0) ? 10 : scene;
            var item = Parsers.ItemWithTotalParser.Parse(match.Groups[3].Value);
            var type = IntegerHelper.Parse(match.Groups[4].Value);

            var key = (day, scene);
            if (!Definitions.SpellCards.ContainsKey(key))
                return match.ToString();

            var score = scores.FirstOrDefault(elem =>
                (elem?.Number > 0) &&
                (elem.Number <= Definitions.SpellCards.Count) &&
                Definitions.SpellCards.ElementAt(elem.Number - 1).Key.Equals(key));

            switch (type)
            {
                case 1:     // high score
                    return formatter.FormatNumber((score?.HighScore * 10) ?? default);
                case 2:     // challenge count
                    if (item == ItemWithTotal.NoItem)
                    {
                        return "-";
                    }
                    else
                    {
                        return formatter.FormatNumber(
                            (score is not null) && score.ChallengeCounts.TryGetValue(item, out var challengeCount)
                            ? challengeCount : default);
                    }

                case 3:     // cleared count
                    return formatter.FormatNumber(
                        (score is not null) && score.ClearCounts.TryGetValue(item, out var clearCount)
                        ? clearCount : default);
                default:    // unreachable
                    return match.ToString();
            }
        });
    }

    public string Replace(string input)
    {
        return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
    }
}

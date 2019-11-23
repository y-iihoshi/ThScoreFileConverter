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

namespace ThScoreFileConverter.Models.Th143
{
    // %T143SCR[w][x][y][z]
    internal class ScoreReplacer : IStringReplaceable
    {
        private static readonly string Pattern = Utils.Format(
            @"%T143SCR({0})([0-9])({1})([1-3])", Parsers.DayParser.Pattern, Parsers.ItemWithTotalParser.Pattern);

        private readonly MatchEvaluator evaluator;

        public ScoreReplacer(IReadOnlyList<IScore> scores)
        {
            if (scores is null)
                throw new ArgumentNullException(nameof(scores));

            this.evaluator = new MatchEvaluator(match =>
            {
                var day = Parsers.DayParser.Parse(match.Groups[1].Value);
                var scene = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);
                scene = (scene == 0) ? 10 : scene;
                var item = Parsers.ItemWithTotalParser.Parse(match.Groups[3].Value);
                var type = int.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture);

                var key = (day, scene);
                if (!Definitions.SpellCards.ContainsKey(key))
                    return match.ToString();

                var score = scores.FirstOrDefault(elem =>
                    (elem != null) &&
                    (elem.Number > 0) &&
                    (elem.Number <= Definitions.SpellCards.Count) &&
                    Definitions.SpellCards.ElementAt(elem.Number - 1).Key.Equals(key));

                switch (type)
                {
                    case 1:     // high score
                        return (score != null) ? Utils.ToNumberString(score.HighScore * 10) : "0";
                    case 2:     // challenge count
                        if (item == ItemWithTotal.NoItem)
                        {
                            return "-";
                        }
                        else
                        {
                            return (score != null) && score.ChallengeCounts.TryGetValue(item, out var challengeCount)
                                ? Utils.ToNumberString(challengeCount) : "0";
                        }

                    case 3:     // cleared count
                        return (score != null) && score.ClearCounts.TryGetValue(item, out var clearCount)
                            ? Utils.ToNumberString(clearCount) : "0";
                    default:    // unreachable
                        return match.ToString();
                }
            });
        }

        public string Replace(string input) => Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
    }
}

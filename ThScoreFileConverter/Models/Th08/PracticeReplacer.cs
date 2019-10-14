//-----------------------------------------------------------------------
// <copyright file="PracticeReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using static ThScoreFileConverter.Models.Th08.Parsers;

namespace ThScoreFileConverter.Models.Th08
{
    // %T08PRAC[w][xx][yy][z]
    internal class PracticeReplacer : IStringReplaceable
    {
        private static readonly string Pattern = Utils.Format(
            @"%T08PRAC({0})({1})({2})([12])", LevelParser.Pattern, CharaParser.Pattern, StageParser.Pattern);

        private readonly MatchEvaluator evaluator;

        public PracticeReplacer(IReadOnlyDictionary<Chara, IPracticeScore> practiceScores)
        {
            if (practiceScores is null)
                throw new ArgumentNullException(nameof(practiceScores));

            this.evaluator = new MatchEvaluator(match =>
            {
                var level = LevelParser.Parse(match.Groups[1].Value);
                var chara = CharaParser.Parse(match.Groups[2].Value);
                var stage = StageParser.Parse(match.Groups[3].Value);
                var type = int.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture);

                if (level == Level.Extra)
                    return match.ToString();
                if (stage == Stage.Extra)
                    return match.ToString();

                if (practiceScores.TryGetValue(chara, out var practiceScore))
                {
                    var key = (stage, level);
                    if (type == 1)
                    {
                        return practiceScore.HighScores.TryGetValue(key, out var highScore)
                            ? Utils.ToNumberString(highScore * 10) : "0";
                    }
                    else
                    {
                        return practiceScore.PlayCounts.TryGetValue(key, out var playCount)
                            ? Utils.ToNumberString(playCount) : "0";
                    }
                }
                else
                {
                    return "0";
                }
            });
        }

        public string Replace(string input) => Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
    }
}

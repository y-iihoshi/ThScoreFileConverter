//-----------------------------------------------------------------------
// <copyright file="PracticeReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using System.Text.RegularExpressions;
using ThScoreFileConverter.Helpers;
using static ThScoreFileConverter.Models.Th08.Parsers;

namespace ThScoreFileConverter.Models.Th08
{
    // %T08PRAC[w][xx][yy][z]
    internal class PracticeReplacer : IStringReplaceable
    {
        private static readonly string Pattern = Utils.Format(
            @"{0}PRAC({1})({2})({3})([12])",
            Definitions.FormatPrefix,
            LevelParser.Pattern,
            CharaParser.Pattern,
            StageParser.Pattern);

        private readonly MatchEvaluator evaluator;

        public PracticeReplacer(IReadOnlyDictionary<Chara, IPracticeScore> practiceScores, INumberFormatter formatter)
        {
            this.evaluator = new MatchEvaluator(match =>
            {
                var level = LevelParser.Parse(match.Groups[1].Value);
                var chara = CharaParser.Parse(match.Groups[2].Value);
                var stage = StageParser.Parse(match.Groups[3].Value);
                var type = IntegerHelper.Parse(match.Groups[4].Value);

                int GetValue(IPracticeScore score)
                {
                    var key = (stage, level);
                    return (type == 1)
                        ? (score.HighScores.TryGetValue(key, out var highScore) ? (highScore * 10) : default)
                        : (score.PlayCounts.TryGetValue(key, out var playCount) ? playCount : default);
                }

                return Models.Definitions.CanPractice(level) && Definitions.CanPractice(stage)
                    ? formatter.FormatNumber(
                        practiceScores.TryGetValue(chara, out var score) ? GetValue(score) : default)
                    : match.ToString();
            });
        }

        public string Replace(string input)
        {
            return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
        }
    }
}

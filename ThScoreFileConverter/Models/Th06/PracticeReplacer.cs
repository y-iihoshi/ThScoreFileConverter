//-----------------------------------------------------------------------
// <copyright file="PracticeReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ThScoreFileConverter.Models.Th06
{
    // %T06PRAC[x][yy][z]
    internal class PracticeReplacer : IStringReplaceable
    {
        private static readonly string Pattern = Utils.Format(
            @"{0}PRAC({1})({2})({3})",
            Definitions.FormatPrefix,
            Parsers.LevelParser.Pattern,
            Parsers.CharaParser.Pattern,
            Parsers.StageParser.Pattern);

        private readonly MatchEvaluator evaluator;

        public PracticeReplacer(
            IReadOnlyDictionary<(Chara Chara, Level Level, Stage Stage), IPracticeScore> practiceScores,
            INumberFormatter formatter)
        {
            this.evaluator = new MatchEvaluator(match =>
            {
                var level = Parsers.LevelParser.Parse(match.Groups[1].Value);
                var chara = Parsers.CharaParser.Parse(match.Groups[2].Value);
                var stage = Parsers.StageParser.Parse(match.Groups[3].Value);

                return Models.Definitions.CanPractice(level) && Models.Definitions.CanPractice(stage)
                    ? formatter.FormatNumber(
                        practiceScores.TryGetValue((chara, level, stage), out var score) ? score.HighScore : default)
                    : match.ToString();
            });
        }

        public string Replace(string input)
        {
            return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
        }
    }
}

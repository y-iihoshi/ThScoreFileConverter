//-----------------------------------------------------------------------
// <copyright file="PracticeReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using static ThScoreFileConverter.Models.Th07.Parsers;

namespace ThScoreFileConverter.Models.Th07
{
    // %T07PRAC[w][xx][y][z]
    internal class PracticeReplacer : IStringReplaceable
    {
        private static readonly string Pattern = Utils.Format(
            @"%T07PRAC({0})({1})({2})([12])", LevelParser.Pattern, CharaParser.Pattern, StageParser.Pattern);

        private readonly MatchEvaluator evaluator;

        public PracticeReplacer(IReadOnlyDictionary<(Chara, Level, Stage), PracticeScore> practiceScores)
        {
            this.evaluator = new MatchEvaluator(match =>
            {
                var level = LevelParser.Parse(match.Groups[1].Value);
                var chara = CharaParser.Parse(match.Groups[2].Value);
                var stage = StageParser.Parse(match.Groups[3].Value);
                var type = int.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture);

                if ((level == Level.Extra) || (level == Level.Phantasm))
                    return match.ToString();
                if ((stage == Stage.Extra) || (stage == Stage.Phantasm))
                    return match.ToString();

                var key = (chara, level, stage);
                if (practiceScores.TryGetValue(key, out var score))
                {
                    return (type == 1)
                        ? Utils.ToNumberString(score.HighScore * 10) : Utils.ToNumberString(score.TrialCount);
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

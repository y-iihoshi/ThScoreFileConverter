﻿//-----------------------------------------------------------------------
// <copyright file="PracticeReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using System.Text.RegularExpressions;
using ThScoreFileConverter.Helpers;
using static ThScoreFileConverter.Models.Th07.Parsers;

namespace ThScoreFileConverter.Models.Th07
{
    // %T07PRAC[w][xx][y][z]
    internal class PracticeReplacer : IStringReplaceable
    {
        private static readonly string Pattern = Utils.Format(
            @"{0}PRAC({1})({2})({3})([12])",
            Definitions.FormatPrefix,
            LevelParser.Pattern,
            CharaParser.Pattern,
            StageParser.Pattern);

        private readonly MatchEvaluator evaluator;

        public PracticeReplacer(
            IReadOnlyDictionary<(Chara Chara, Level Level, Stage Stage), IPracticeScore> practiceScores,
            INumberFormatter formatter)
        {
            this.evaluator = new MatchEvaluator(match =>
            {
                var level = LevelParser.Parse(match.Groups[1].Value);
                var chara = CharaParser.Parse(match.Groups[2].Value);
                var stage = StageParser.Parse(match.Groups[3].Value);
                var type = IntegerHelper.Parse(match.Groups[4].Value);

                if ((level == Level.Extra) || (level == Level.Phantasm))
                    return match.ToString();
                if ((stage == Stage.Extra) || (stage == Stage.Phantasm))
                    return match.ToString();

                var key = (chara, level, stage);
                if (type == 1)
                {
                    return formatter.FormatNumber(
                        practiceScores.TryGetValue(key, out var score) ? (score.HighScore * 10) : default);
                }
                else
                {
                    return formatter.FormatNumber(
                        practiceScores.TryGetValue(key, out var score) ? score.TrialCount : default);
                }
            });
        }

        public string Replace(string input)
        {
            return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
        }
    }
}

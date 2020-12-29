//-----------------------------------------------------------------------
// <copyright file="ScoreReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th125
{
    // %T125SCR[w][x][y][z]
    internal class ScoreReplacer : IStringReplaceable
    {
        private static readonly string Pattern = Utils.Format(
            @"%T125SCR({0})({1})([1-9])([1-5])", Parsers.CharaParser.Pattern, Parsers.LevelParser.Pattern);

        private readonly MatchEvaluator evaluator;

        public ScoreReplacer(IReadOnlyList<IScore> scores)
        {
            if (scores is null)
                throw new ArgumentNullException(nameof(scores));

            this.evaluator = new MatchEvaluator(match =>
            {
                var chara = Parsers.CharaParser.Parse(match.Groups[1].Value);
                var level = Parsers.LevelParser.Parse(match.Groups[2].Value);
                var scene = IntegerHelper.Parse(match.Groups[3].Value);
                var type = IntegerHelper.Parse(match.Groups[4].Value);

                var key = (level, scene);
                if (!Definitions.SpellCards.ContainsKey(key))
                    return match.ToString();

                var score = scores.FirstOrDefault(elem => (elem?.Chara == chara) && elem.LevelScene.Equals(key));

                return type switch
                {
                    1 => Utils.ToNumberString(score?.HighScore ?? default),
                    2 => Utils.ToNumberString(score?.BestshotScore ?? default),
                    3 => Utils.ToNumberString(score?.TrialCount ?? default),
                    4 => Utils.ToNumberString(score?.FirstSuccess ?? default),
                    5 => DateTimeHelper.GetString(score?.DateTime),
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

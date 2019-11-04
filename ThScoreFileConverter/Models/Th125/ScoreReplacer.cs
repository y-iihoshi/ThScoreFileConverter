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
                var scene = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);
                var type = int.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture);

                var key = (level, scene);
                if (!Definitions.SpellCards.ContainsKey(key))
                    return match.ToString();

                var score = scores.FirstOrDefault(elem =>
                    (elem != null) && (elem.Chara == chara) && elem.LevelScene.Equals(key));

                switch (type)
                {
                    case 1:     // high score
                        return (score != null) ? Utils.ToNumberString(score.HighScore) : "0";
                    case 2:     // bestshot score
                        return (score != null) ? Utils.ToNumberString(score.BestshotScore) : "0";
                    case 3:     // num of shots
                        return (score != null) ? Utils.ToNumberString(score.TrialCount) : "0";
                    case 4:     // num of shots for the first success
                        return (score != null) ? Utils.ToNumberString(score.FirstSuccess) : "0";
                    case 5:     // date & time
                        return (score != null)
                            ? new DateTime(1970, 1, 1).AddSeconds(score.DateTime).ToLocalTime()
                                .ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.CurrentCulture)
                            : "----/--/-- --:--:--";
                    default:    // unreachable
                        return match.ToString();
                }
            });
        }

        public string Replace(string input) => Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
    }
}

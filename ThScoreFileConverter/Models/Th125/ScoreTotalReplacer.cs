//-----------------------------------------------------------------------
// <copyright file="ScoreTotalReplacer.cs" company="None">
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
    // %T125SCRTL[x][y][z]
    internal class ScoreTotalReplacer : IStringReplaceable
    {
        private static readonly string Pattern = Utils.Format(
            @"%T125SCRTL({0})([12])([1-5])", Parsers.CharaParser.Pattern);

        private readonly MatchEvaluator evaluator;

        public ScoreTotalReplacer(IReadOnlyList<IScore> scores)
        {
            if (scores is null)
                throw new ArgumentNullException(nameof(scores));

            this.evaluator = new MatchEvaluator(match => EvaluatorImpl(match, scores));

            static string EvaluatorImpl(Match match, IReadOnlyList<IScore> scores)
            {
                var chara = Parsers.CharaParser.Parse(match.Groups[1].Value);
                var method = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);
                var type = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

                bool IsTarget(IScore score)
                {
                    return IsTargetImpl(score, chara, method);
                }

                bool TriedAndSucceeded(IScore score)
                {
                    return IsTarget(score) && (score.TrialCount > 0) && (score.FirstSuccess > 0);
                }

                return type switch
                {
                    1 => Utils.ToNumberString(scores.Sum(score => TriedAndSucceeded(score) ? score.HighScore : 0L)),
                    2 => Utils.ToNumberString(scores.Sum(score => IsTarget(score) ? score.BestshotScore : 0L)),
                    3 => Utils.ToNumberString(scores.Sum(score => IsTarget(score) ? score.TrialCount : 0)),
                    4 => Utils.ToNumberString(scores.Sum(score => TriedAndSucceeded(score) ? score.FirstSuccess : 0L)),
                    5 => scores.Count(TriedAndSucceeded).ToString(CultureInfo.CurrentCulture),
                    _ => match.ToString(),  // unreachable
                };
            }
        }

        public string Replace(string input)
        {
            return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
        }

        private static bool IsTargetImpl(IScore score, Chara chara, int method)
        {
            if (score is null)
                return false;

            if (method == 2)
                return score.Chara == chara;

            if (score.LevelScene.Level != Level.Spoiler)
                return score.Chara == chara;

            if (chara == Chara.Hatate)
                return false;

            return score.Chara == (score.LevelScene.Scene <= 4 ? Chara.Aya : Chara.Hatate);
        }
    }
}

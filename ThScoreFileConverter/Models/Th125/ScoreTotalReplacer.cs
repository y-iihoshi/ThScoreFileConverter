//-----------------------------------------------------------------------
// <copyright file="ScoreTotalReplacer.cs" company="None">
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
    // %T125SCRTL[x][y][z]
    internal class ScoreTotalReplacer : IStringReplaceable
    {
        private static readonly string Pattern = Utils.Format(
            @"%T125SCRTL({0})([12])([1-5])", Parsers.CharaParser.Pattern);

        private readonly MatchEvaluator evaluator;

        public ScoreTotalReplacer(IReadOnlyList<IScore> scores, INumberFormatter formatter)
        {
            if (scores is null)
                throw new ArgumentNullException(nameof(scores));

            this.evaluator = new MatchEvaluator(match => EvaluatorImpl(match, scores, formatter));

            static string EvaluatorImpl(Match match, IReadOnlyList<IScore> scores, INumberFormatter formatter)
            {
                var chara = Parsers.CharaParser.Parse(match.Groups[1].Value);
                var method = IntegerHelper.Parse(match.Groups[2].Value);
                var type = IntegerHelper.Parse(match.Groups[3].Value);

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
                    1 => formatter.FormatNumber(
                        scores.Sum(score => (long)(TriedAndSucceeded(score) ? score.HighScore : default))),
                    2 => formatter.FormatNumber(
                        scores.Sum(score => (long)(IsTarget(score) ? score.BestshotScore : default))),
                    3 => formatter.FormatNumber(
                        scores.Sum(score => (long)(IsTarget(score) ? score.TrialCount : default))),
                    4 => formatter.FormatNumber(
                        scores.Sum(score => (long)(TriedAndSucceeded(score) ? score.FirstSuccess : default))),
                    5 => formatter.FormatNumber(scores.Count(TriedAndSucceeded)),
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

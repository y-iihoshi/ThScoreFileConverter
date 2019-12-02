//-----------------------------------------------------------------------
// <copyright file="ScoreTotalReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace ThScoreFileConverter.Models.Th165
{
    // %T165SCRTL[x]
    internal class ScoreTotalReplacer : IStringReplaceable
    {
        private static readonly string Pattern = Utils.Format(@"%T165SCRTL([1-6])");

        private readonly MatchEvaluator evaluator;

        public ScoreTotalReplacer(IReadOnlyList<IScore> scores, IStatus status)
        {
            this.evaluator = new MatchEvaluator(match =>
            {
                var type = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);

                switch (type)
                {
                    case 1:     // total score
                        return Utils.ToNumberString(scores.Sum(score => score.HighScore));
                    case 2:     // total of challenge counts
                        return Utils.ToNumberString(scores.Sum(score => score.ChallengeCount));
                    case 3:     // total of cleared counts
                        return Utils.ToNumberString(scores.Sum(score => score.ClearCount));
                    case 4:     // num of cleared scenes
                        return Utils.ToNumberString(scores.Count(score => score.ClearCount > 0));
                    case 5:     // num of photos
                        return Utils.ToNumberString(scores.Sum(score => score.NumPhotos));
                    case 6:     // num of nicknames
                        return Utils.ToNumberString(status.NicknameFlags.Count(flag => flag > 0));
                    default:    // unreachable
                        return match.ToString();
                }
            });
        }

        public string Replace(string input) => Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
    }
}

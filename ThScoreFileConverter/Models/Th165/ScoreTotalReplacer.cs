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

namespace ThScoreFileConverter.Models.Th165
{
    // %T165SCRTL[x]
    internal class ScoreTotalReplacer : IStringReplaceable
    {
        private static readonly string Pattern = Utils.Format(@"%T165SCRTL([1-6])");

        private readonly MatchEvaluator evaluator;

        public ScoreTotalReplacer(IReadOnlyList<IScore> scores, IStatus status)
        {
            if (scores is null)
                throw new ArgumentNullException(nameof(scores));
            if (status is null)
                throw new ArgumentNullException(nameof(status));

            this.evaluator = new MatchEvaluator(match =>
            {
                var type = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);

                return type switch
                {
                    1 => Utils.ToNumberString(scores.Sum(score => score.HighScore)),
                    2 => Utils.ToNumberString(scores.Sum(score => score.ChallengeCount)),
                    3 => Utils.ToNumberString(scores.Sum(score => score.ClearCount)),
                    4 => Utils.ToNumberString(scores.Count(score => score.ClearCount > 0)),
                    5 => Utils.ToNumberString(scores.Sum(score => score.NumPhotos)),
                    6 => Utils.ToNumberString(status.NicknameFlags.Count(flag => flag > 0)),
                    _ => match.ToString(),  // unreachable
                };
            });
        }

        public string Replace(string input) => Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
    }
}

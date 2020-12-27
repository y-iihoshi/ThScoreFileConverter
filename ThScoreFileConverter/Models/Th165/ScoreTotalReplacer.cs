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
                var type = IntegerHelper.Parse(match.Groups[1].Value);

                return type switch
                {
                    1 => Utils.ToNumberString(scores.Sum(score => (long)(score?.HighScore ?? default))),
                    2 => Utils.ToNumberString(scores.Sum(score => (long)(score?.ChallengeCount ?? default))),
                    3 => Utils.ToNumberString(scores.Sum(score => (long)(score?.ClearCount ?? default))),
                    4 => Utils.ToNumberString(scores.Count(score => score?.ClearCount > 0)),
                    5 => Utils.ToNumberString(scores.Sum(score => (long)(score?.NumPhotos ?? default))),
                    6 => Utils.ToNumberString(status.NicknameFlags.Count(flag => flag > 0)),
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

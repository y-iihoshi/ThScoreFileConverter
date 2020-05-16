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

namespace ThScoreFileConverter.Models.Th095
{
    // %T95SCRTL[x]
    internal class ScoreTotalReplacer : IStringReplaceable
    {
        private const string Pattern = @"%T95SCRTL([1-4])";

        private readonly MatchEvaluator evaluator;

        public ScoreTotalReplacer(IReadOnlyList<IScore> scores)
        {
            if (scores is null)
                throw new ArgumentNullException(nameof(scores));

            this.evaluator = new MatchEvaluator(match =>
            {
                var type = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);

                return type switch
                {
                    1 => Utils.ToNumberString(scores.Sum(score => (long)(score?.HighScore ?? default))),
                    2 => Utils.ToNumberString(scores.Sum(score => (long)(score?.BestshotScore ?? default))),
                    3 => Utils.ToNumberString(scores.Sum(score => (long)(score?.TrialCount ?? default))),
                    4 => scores.Count(score => score?.HighScore > 0).ToString(CultureInfo.CurrentCulture),
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

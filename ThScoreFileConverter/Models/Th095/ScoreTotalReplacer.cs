//-----------------------------------------------------------------------
// <copyright file="ScoreTotalReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th095;

// %T95SCRTL[x]
internal class ScoreTotalReplacer : IStringReplaceable
{
    private static readonly string Pattern = Utils.Format(@"{0}SCRTL([1-4])", Definitions.FormatPrefix);

    private readonly MatchEvaluator evaluator;

    public ScoreTotalReplacer(IReadOnlyList<IScore> scores, INumberFormatter formatter)
    {
        this.evaluator = new MatchEvaluator(match =>
        {
            var type = IntegerHelper.Parse(match.Groups[1].Value);

            return type switch
            {
                1 => formatter.FormatNumber(scores.Sum(score => (long)(score?.HighScore ?? default))),
                2 => formatter.FormatNumber(scores.Sum(score => (long)(score?.BestshotScore ?? default))),
                3 => formatter.FormatNumber(scores.Sum(score => (long)(score?.TrialCount ?? default))),
                4 => formatter.FormatNumber(scores.Count(score => score?.HighScore > 0)),
                _ => match.ToString(),  // unreachable
            };
        });
    }

    public string Replace(string input)
    {
        return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
    }
}

//-----------------------------------------------------------------------
// <copyright file="ScoreTotalReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Text.RegularExpressions;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th165;

// %T165SCRTL[x]
internal sealed class ScoreTotalReplacer(IReadOnlyList<IScore> scores, IStatus status, INumberFormatter formatter)
    : IStringReplaceable
{
    private static readonly IntegerParser TypeParser = new(@"[1-6]");
    private static readonly string Pattern = StringHelper.Create($"{Definitions.FormatPrefix}SCRTL({TypeParser.Pattern})");

    private readonly MatchEvaluator evaluator = new(match =>
    {
        var type = TypeParser.Parse(match.Groups[1]);

        return type switch
        {
            1 => formatter.FormatNumber(scores.Sum(score => (long)(score?.HighScore ?? default))),
            2 => formatter.FormatNumber(scores.Sum(score => (long)(score?.ChallengeCount ?? default))),
            3 => formatter.FormatNumber(scores.Sum(score => (long)(score?.ClearCount ?? default))),
            4 => formatter.FormatNumber(scores.Count(score => score?.ClearCount > 0)),
            5 => formatter.FormatNumber(scores.Sum(score => (long)(score?.NumPhotos ?? default))),
            6 => formatter.FormatNumber(status.NicknameFlags.Count(flag => flag > 0)),
            _ => match.ToString(),  // unreachable
        };
    });

    public string Replace(string input)
    {
        return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
    }
}

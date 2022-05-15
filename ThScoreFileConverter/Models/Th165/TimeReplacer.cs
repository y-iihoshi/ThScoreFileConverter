//-----------------------------------------------------------------------
// <copyright file="TimeReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Text.RegularExpressions;

namespace ThScoreFileConverter.Models.Th165;

// %T165TIMEPLY
internal class TimeReplacer : IStringReplaceable
{
    private static readonly string Pattern = Utils.Format(@"{0}TIMEPLY", Definitions.FormatPrefix);

    private readonly MatchEvaluator evaluator;

    public TimeReplacer(IStatus status)
    {
        this.evaluator = new MatchEvaluator(match => new Time(status.TotalPlayTime * 10, false).ToLongString());
    }

    public string Replace(string input)
    {
        return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
    }
}

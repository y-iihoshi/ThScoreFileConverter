//-----------------------------------------------------------------------
// <copyright file="TimeReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Text.RegularExpressions;

namespace ThScoreFileConverter.Models.Th09
{
    // %T09TIMEALL
    internal class TimeReplacer : IStringReplaceable
    {
        private const string Pattern = @"%T09TIMEALL";

        private readonly MatchEvaluator evaluator;

        public TimeReplacer(IPlayStatus playStatus)
        {
            this.evaluator = new MatchEvaluator(match => playStatus.TotalRunningTime.ToLongString());
        }

        public string Replace(string input)
        {
            return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
        }
    }
}

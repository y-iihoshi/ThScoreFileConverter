//-----------------------------------------------------------------------
// <copyright file="TimeReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Text.RegularExpressions;

namespace ThScoreFileConverter.Models.Th143
{
    // %T143TIMEPLY
    internal class TimeReplacer : IStringReplaceable
    {
        private const string Pattern = @"%T143TIMEPLY";

        private readonly MatchEvaluator evaluator;

        public TimeReplacer(IStatus status)
        {
            if (status is null)
                throw new ArgumentNullException(nameof(status));

            this.evaluator = new MatchEvaluator(match => new Time(status.TotalPlayTime * 10, false).ToLongString());
        }

        public string Replace(string input)
        {
            return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
        }
    }
}

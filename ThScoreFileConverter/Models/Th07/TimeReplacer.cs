//-----------------------------------------------------------------------
// <copyright file="TimeReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Text.RegularExpressions;

namespace ThScoreFileConverter.Models.Th07
{
    // %T07TIME(ALL|PLY)
    internal class TimeReplacer : IStringReplaceable
    {
        private const string Pattern = @"%T07TIME(ALL|PLY)";

        private readonly MatchEvaluator evaluator;

        public TimeReplacer(PlayStatus playStatus)
        {
            if (playStatus is null)
                throw new ArgumentNullException(nameof(playStatus));

            this.evaluator = new MatchEvaluator(match =>
            {
                var kind = match.Groups[1].Value.ToUpperInvariant();

                return (kind == "ALL")
                    ? playStatus.TotalRunningTime.ToLongString() : playStatus.TotalPlayTime.ToLongString();
            });
        }

        public string Replace(string input) => Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
    }
}

﻿//-----------------------------------------------------------------------
// <copyright file="TimeReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Text.RegularExpressions;

namespace ThScoreFileConverter.Models.Th08
{
    // %T08TIME(ALL|PLY)
    internal class TimeReplacer : IStringReplaceable
    {
        private static readonly string Pattern = Utils.Format(@"{0}TIME(ALL|PLY)", Definitions.FormatPrefix);

        private readonly MatchEvaluator evaluator;

        public TimeReplacer(IPlayStatus playStatus)
        {
            this.evaluator = new MatchEvaluator(match =>
            {
                var kind = match.Groups[1].Value.ToUpperInvariant();

                return (kind == "ALL")
                    ? playStatus.TotalRunningTime.ToLongString() : playStatus.TotalPlayTime.ToLongString();
            });
        }

        public string Replace(string input)
        {
            return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
        }
    }
}

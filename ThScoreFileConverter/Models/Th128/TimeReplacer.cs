﻿//-----------------------------------------------------------------------
// <copyright file="TimeReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Text.RegularExpressions;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th128;

// %T128TIMEPLY
internal sealed class TimeReplacer(Th125.IStatus status) : IStringReplaceable
{
    private static readonly string Pattern = StringHelper.Create($"{Definitions.FormatPrefix}TIMEPLY");

    private readonly MatchEvaluator evaluator = new(match => new Time(status.TotalPlayTime * 10, false).ToLongString());

    public string Replace(string input)
    {
        return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
    }
}

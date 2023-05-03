﻿//-----------------------------------------------------------------------
// <copyright file="Parsers.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System.Linq;
using ThScoreFileConverter.Core.Extensions;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models.Th143;

namespace ThScoreFileConverter.Models.Th143;

/// <summary>
/// Provides the parsers used for ISC.
/// </summary>
internal static class Parsers
{
    /// <summary>
    /// Gets the parser of <see cref="Day"/>.
    /// </summary>
    public static EnumShortNameParser<Day> DayParser { get; } =
        new EnumShortNameParser<Day>();

    /// <summary>
    /// Gets the parser of <see cref="ItemWithTotal"/>.
    /// </summary>
    public static EnumShortNameParser<ItemWithTotal> ItemWithTotalParser { get; } =
        new EnumShortNameParser<ItemWithTotal>();

    /// <summary>
    /// Gets the pattern used for parsing as a long name of a <see cref="Day"/> enumerator.
    /// </summary>
    public static string DayLongPattern { get; } =
        string.Join("|", EnumHelper<Day>.Enumerable.Select(day => day.ToLongName()));
}

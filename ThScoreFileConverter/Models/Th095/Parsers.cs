//-----------------------------------------------------------------------
// <copyright file="Parsers.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System.Linq;
using ThScoreFileConverter.Core.Extensions;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models.Th095;

namespace ThScoreFileConverter.Models.Th095;

/// <summary>
/// Provides the parsers used for StB.
/// </summary>
internal static class Parsers
{
    /// <summary>
    /// Gets the parser of <see cref="Level"/>.
    /// </summary>
    public static EnumShortNameParser<Level> LevelParser { get; } =
        new EnumShortNameParser<Level>();

    /// <summary>
    /// Gets the pattern used for parsing as a long name of a <see cref="Level"/> enumerator.
    /// </summary>
    public static string LevelLongPattern { get; } =
        string.Join("|", EnumHelper<Level>.Enumerable.Select(level => level.ToLongName()));
}

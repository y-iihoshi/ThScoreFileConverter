//-----------------------------------------------------------------------
// <copyright file="Parsers.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using ThScoreFileConverter.Core.Models.Th175;

namespace ThScoreFileConverter.Models.Th175;

/// <summary>
/// Provides the parsers used for AoCF.
/// </summary>
internal static class Parsers
{
    /// <summary>
    /// Gets the parser of <see cref="Level"/>.
    /// </summary>
    public static Core.Models.EnumPatternParser<Level> LevelParser { get; } = new();

    /// <summary>
    /// Gets the parser of <see cref="Chara"/>.
    /// </summary>
    public static Core.Models.EnumPatternParser<Chara> CharaParser { get; } = new();

    /// <summary>
    /// Gets the parser of <see cref="CharaWithTotal"/>.
    /// </summary>
    public static Core.Models.EnumPatternParser<CharaWithTotal> CharaWithTotalParser { get; } = new();
}

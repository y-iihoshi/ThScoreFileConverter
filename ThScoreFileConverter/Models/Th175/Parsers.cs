//-----------------------------------------------------------------------
// <copyright file="Parsers.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Models.Th175;

/// <summary>
/// Provides the parsers used for AoCF.
/// </summary>
internal static class Parsers
{
    /// <summary>
    /// Gets the parser of <see cref="Level"/>.
    /// </summary>
    public static EnumShortNameParser<Level> LevelParser { get; }
        = new EnumShortNameParser<Level>();

    /// <summary>
    /// Gets the parser of <see cref="Chara"/>.
    /// </summary>
    public static EnumShortNameParser<Chara> CharaParser { get; }
        = new EnumShortNameParser<Chara>();

    /// <summary>
    /// Gets the parser of <see cref="CharaWithTotal"/>.
    /// </summary>
    public static EnumShortNameParser<CharaWithTotal> CharaWithTotalParser { get; }
        = new EnumShortNameParser<CharaWithTotal>();
}

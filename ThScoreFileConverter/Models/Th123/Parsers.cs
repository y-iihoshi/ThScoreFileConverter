//-----------------------------------------------------------------------
// <copyright file="Parsers.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using ThScoreFileConverter.Core.Models.Th105;

namespace ThScoreFileConverter.Models.Th123;

/// <summary>
/// Provides the parsers used for Hisoutensoku.
/// </summary>
internal static class Parsers
{
    /// <summary>
    /// Gets the parser of <see cref="Th105.LevelWithTotal"/>.
    /// </summary>
    public static EnumShortNameParser<Th105.LevelWithTotal> LevelWithTotalParser { get; } =
        new EnumShortNameParser<Th105.LevelWithTotal>();

    /// <summary>
    /// Gets the parser of <see cref="Chara"/>.
    /// </summary>
    public static EnumShortNameParser<Chara> CharaParser { get; } =
        new EnumShortNameParser<Chara>();

    /// <summary>
    /// Gets the parser of <see cref="Core.Models.Th105.CardType"/>.
    /// </summary>
    public static EnumShortNameParser<CardType> CardTypeParser { get; } =
        new EnumShortNameParser<CardType>();
}

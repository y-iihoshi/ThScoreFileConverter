//-----------------------------------------------------------------------
// <copyright file="Parsers.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using ThScoreFileConverter.Core.Models.Th123;
using CardType = ThScoreFileConverter.Core.Models.Th105.CardType;
using LevelWithTotal = ThScoreFileConverter.Core.Models.Th105.LevelWithTotal;

namespace ThScoreFileConverter.Models.Th123;

/// <summary>
/// Provides the parsers used for Hisoutensoku.
/// </summary>
internal static class Parsers
{
    /// <summary>
    /// Gets the parser of <see cref="LevelWithTotal"/>.
    /// </summary>
    public static Core.Models.EnumPatternParser<LevelWithTotal> LevelWithTotalParser { get; } = new();

    /// <summary>
    /// Gets the parser of <see cref="Chara"/>.
    /// </summary>
    public static EnumShortNameParser<Chara> CharaParser { get; } =
        new EnumShortNameParser<Chara>();

    /// <summary>
    /// Gets the parser of <see cref="CardType"/>.
    /// </summary>
    public static Core.Models.EnumPatternParser<CardType> CardTypeParser { get; } = new();
}

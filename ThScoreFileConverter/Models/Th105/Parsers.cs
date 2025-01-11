//-----------------------------------------------------------------------
// <copyright file="Parsers.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using ThScoreFileConverter.Core.Models.Th105;

namespace ThScoreFileConverter.Models.Th105;

/// <summary>
/// Provides the parsers used for SWR.
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
    public static Core.Models.EnumPatternParser<Chara> CharaParser { get; } = new();

    /// <summary>
    /// Gets the parser of <see cref="CardType"/>.
    /// </summary>
    public static Core.Models.EnumPatternParser<CardType> CardTypeParser { get; } = new();
}

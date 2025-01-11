//-----------------------------------------------------------------------
// <copyright file="Parsers.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th18;

namespace ThScoreFileConverter.Models.Th18;

/// <summary>
/// Provides the parsers used for WBWC.
/// </summary>
internal static class Parsers
{
    /// <summary>
    /// Gets the parser of <see cref="GameMode"/>.
    /// </summary>
    public static EnumPatternParser<GameMode> GameModeParser { get; } = new();

    /// <summary>
    /// Gets the parser of <see cref="Level"/>.
    /// </summary>
    public static EnumPatternParser<Level> LevelParser { get; } = new();

    /// <summary>
    /// Gets the parser of <see cref="LevelWithTotal"/>.
    /// </summary>
    public static EnumPatternParser<LevelWithTotal> LevelWithTotalParser { get; } = new();

    /// <summary>
    /// Gets the parser of <see cref="Chara"/>.
    /// </summary>
    public static EnumPatternParser<Chara> CharaParser { get; } = new();

    /// <summary>
    /// Gets the parser of <see cref="CharaWithTotal"/>.
    /// </summary>
    public static EnumPatternParser<CharaWithTotal> CharaWithTotalParser { get; } = new();

    /// <summary>
    /// Gets the parser of <see cref="Stage"/>.
    /// </summary>
    public static EnumPatternParser<Stage> StageParser { get; } = new();

    /// <summary>
    /// Gets the parser of <see cref="StageWithTotal"/>.
    /// </summary>
    public static EnumPatternParser<StageWithTotal> StageWithTotalParser { get; } = new();
}

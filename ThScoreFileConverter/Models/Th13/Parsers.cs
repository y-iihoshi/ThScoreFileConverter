//-----------------------------------------------------------------------
// <copyright file="Parsers.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th13;

namespace ThScoreFileConverter.Models.Th13;

/// <summary>
/// Provides the parsers used for TD.
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
    /// Gets the parser of <see cref="LevelPracticeWithTotal"/>.
    /// </summary>
    public static EnumPatternParser<LevelPracticeWithTotal> LevelPracticeWithTotalParser { get; } = new();

    /// <summary>
    /// Gets the parser of <see cref="Chara"/>.
    /// </summary>
    public static EnumShortNameParser<Chara> CharaParser { get; } =
        new EnumShortNameParser<Chara>();

    /// <summary>
    /// Gets the parser of <see cref="CharaWithTotal"/>.
    /// </summary>
    public static EnumShortNameParser<CharaWithTotal> CharaWithTotalParser { get; } =
        new EnumShortNameParser<CharaWithTotal>();

    /// <summary>
    /// Gets the parser of <see cref="Stage"/>.
    /// </summary>
    public static EnumShortNameParser<Stage> StageParser { get; } =
        new EnumShortNameParser<Stage>();

    /// <summary>
    /// Gets the parser of <see cref="StageWithTotal"/>.
    /// </summary>
    public static EnumShortNameParser<StageWithTotal> StageWithTotalParser { get; } =
        new EnumShortNameParser<StageWithTotal>();
}

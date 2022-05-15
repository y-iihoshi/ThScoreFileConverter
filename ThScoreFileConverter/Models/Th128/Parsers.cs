//-----------------------------------------------------------------------
// <copyright file="Parsers.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Models.Th128;

/// <summary>
/// Provides the parsers used for FW.
/// </summary>
internal static class Parsers
{
    /// <summary>
    /// Gets the parser of <see cref="Level"/>.
    /// </summary>
    public static EnumShortNameParser<Level> LevelParser { get; } =
        new EnumShortNameParser<Level>();

    /// <summary>
    /// Gets the parser of <see cref="LevelWithTotal"/>.
    /// </summary>
    public static EnumShortNameParser<LevelWithTotal> LevelWithTotalParser { get; } =
        new EnumShortNameParser<LevelWithTotal>();

    /// <summary>
    /// Gets the parser of <see cref="Route"/>.
    /// </summary>
    public static EnumShortNameParser<Route> RouteParser { get; } =
        new EnumShortNameParser<Route>();

    /// <summary>
    /// Gets the parser of <see cref="RouteWithTotal"/>.
    /// </summary>
    public static EnumShortNameParser<RouteWithTotal> RouteWithTotalParser { get; } =
        new EnumShortNameParser<RouteWithTotal>();

    /// <summary>
    /// Gets the parser of <see cref="StageWithTotal"/>.
    /// </summary>
    public static EnumShortNameParser<StageWithTotal> StageWithTotalParser { get; } =
        new EnumShortNameParser<StageWithTotal>();
}

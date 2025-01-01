//-----------------------------------------------------------------------
// <copyright file="Parsers.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using ThScoreFileConverter.Core.Models.Th128;
using Level = ThScoreFileConverter.Core.Models.Level;
using LevelWithTotal = ThScoreFileConverter.Core.Models.LevelWithTotal;

namespace ThScoreFileConverter.Models.Th128;

/// <summary>
/// Provides the parsers used for FW.
/// </summary>
internal static class Parsers
{
    /// <summary>
    /// Gets the parser of <see cref="Level"/>.
    /// </summary>
    public static Core.Models.EnumPatternParser<Level> LevelParser { get; } = new();

    /// <summary>
    /// Gets the parser of <see cref="LevelWithTotal"/>.
    /// </summary>
    public static Core.Models.EnumPatternParser<LevelWithTotal> LevelWithTotalParser { get; } = new();

    /// <summary>
    /// Gets the parser of <see cref="Route"/>.
    /// </summary>
    public static Core.Models.EnumPatternParser<Route> RouteParser { get; } = new();

    /// <summary>
    /// Gets the parser of <see cref="RouteWithTotal"/>.
    /// </summary>
    public static Core.Models.EnumPatternParser<RouteWithTotal> RouteWithTotalParser { get; } = new();

    /// <summary>
    /// Gets the parser of <see cref="StageWithTotal"/>.
    /// </summary>
    public static Core.Models.EnumPatternParser<StageWithTotal> StageWithTotalParser { get; } = new();
}

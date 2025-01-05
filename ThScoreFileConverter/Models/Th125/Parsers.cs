//-----------------------------------------------------------------------
// <copyright file="Parsers.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using ThScoreFileConverter.Core.Models.Th125;

namespace ThScoreFileConverter.Models.Th125;

/// <summary>
/// Provides the parsers used for DS.
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
    /// Gets the parser of scenes.
    /// </summary>
    public static Core.Models.IntegerParser SceneParser { get; } = new(@"[1-9]");
}

//-----------------------------------------------------------------------
// <copyright file="Parsers.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using ThScoreFileConverter.Core.Models.Th165;

namespace ThScoreFileConverter.Models.Th165;

/// <summary>
/// Provides the parsers used for VD.
/// </summary>
internal static class Parsers
{
    /// <summary>
    /// Gets the parser of <see cref="Day"/>.
    /// </summary>
    public static Core.Models.EnumPatternParser<Day> DayParser { get; } = new();

    /// <summary>
    /// Gets the parser of scenes.
    /// </summary>
    public static Core.Models.IntegerParser SceneParser { get; } = new(@"[1-7]");
}

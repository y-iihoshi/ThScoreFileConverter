﻿//-----------------------------------------------------------------------
// <copyright file="Parsers.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using ThScoreFileConverter.Core.Models.Th075;

namespace ThScoreFileConverter.Models.Th075;

/// <summary>
/// Provides the parsers used for IMP.
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
    /// Gets the parser of <see cref="Chara"/>.
    /// </summary>
    public static Core.Models.EnumPatternParser<Chara> CharaParser { get; } = new();

    /// <summary>
    /// Gets the parser for card numbers.
    /// </summary>
    public static Core.Models.IntegerParser CardNumberParser { get; } = new(@"\d{3}");
}

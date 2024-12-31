﻿//-----------------------------------------------------------------------
// <copyright file="LevelPracticeWithTotal.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Core.Models.Th08;

/// <summary>
/// Represents level of IN spell card practice and total.
/// </summary>
public enum LevelPracticeWithTotal
{
    /// <summary>
    /// Represents level Easy.
    /// </summary>
    [EnumAltName("E")]
    [Pattern("E")]
    Easy,

    /// <summary>
    /// Represents level Normal.
    /// </summary>
    [EnumAltName("N")]
    [Pattern("N")]
    Normal,

    /// <summary>
    /// Represents level Hard.
    /// </summary>
    [EnumAltName("H")]
    [Pattern("H")]
    Hard,

    /// <summary>
    /// Represents level Lunatic.
    /// </summary>
    [EnumAltName("L")]
    [Pattern("L")]
    Lunatic,

    /// <summary>
    /// Represents level Extra.
    /// </summary>
    [EnumAltName("X")]
    [Pattern("X")]
    Extra,

    /// <summary>
    /// Represents level Last Word.
    /// </summary>
    [EnumAltName("W", LongName = "Last Word")]
    [Pattern("W")]
    LastWord,

    /// <summary>
    /// Represents total across levels.
    /// </summary>
    [EnumAltName("T")]
    [Pattern("T")]
    Total,
}

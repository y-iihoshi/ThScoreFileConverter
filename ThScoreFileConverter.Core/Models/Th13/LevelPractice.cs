//-----------------------------------------------------------------------
// <copyright file="LevelPractice.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Core.Models.Th13;

/// <summary>
/// Represents levels of TD spell practice.
/// </summary>
public enum LevelPractice
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
    /// Represents Over Drive.
    /// </summary>
    [EnumAltName("D", LongName = "Over Drive")]
    [Pattern("D")]
    OverDrive,
}

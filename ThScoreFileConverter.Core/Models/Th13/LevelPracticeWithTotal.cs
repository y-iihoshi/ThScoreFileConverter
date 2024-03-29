﻿//-----------------------------------------------------------------------
// <copyright file="LevelPracticeWithTotal.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Core.Models.Th13;

/// <summary>
/// Represents levels of TD spell practice and total.
/// </summary>
public enum LevelPracticeWithTotal
{
    /// <summary>
    /// Represents level Easy.
    /// </summary>
    [EnumAltName("E")]
    Easy,

    /// <summary>
    /// Represents level Normal.
    /// </summary>
    [EnumAltName("N")]
    Normal,

    /// <summary>
    /// Represents level Hard.
    /// </summary>
    [EnumAltName("H")]
    Hard,

    /// <summary>
    /// Represents level Lunatic.
    /// </summary>
    [EnumAltName("L")]
    Lunatic,

    /// <summary>
    /// Represents level Extra.
    /// </summary>
    [EnumAltName("X")]
    Extra,

    /// <summary>
    /// Represents Over Drive.
    /// </summary>
    [EnumAltName("D", LongName = "Over Drive")]
    OverDrive,

    /// <summary>
    /// Represents total across levels.
    /// </summary>
    [EnumAltName("T")]
    Total,
}

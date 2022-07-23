//-----------------------------------------------------------------------
// <copyright file="LevelWithTotal.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Core.Models.Th155;

/// <summary>
/// Represents levels of AoCF and total.
/// </summary>
public enum LevelWithTotal
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
    /// Represents level OverDrive.
    /// </summary>
    [EnumAltName("D")]
    OverDrive,

    /// <summary>
    /// Represents total across levels.
    /// </summary>
    [EnumAltName("T")]
    Total,
}

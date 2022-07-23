//-----------------------------------------------------------------------
// <copyright file="LevelPracticeWithTotal.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using ThScoreFileConverter.Core.Models;

namespace ThScoreFileConverter.Models.Th14;

/// <summary>
/// Representing levels of DDC and total.
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
    /// Not used.
    /// </summary>
    [EnumAltName("-")]
    NotUsed,

    /// <summary>
    /// Represents total across levels.
    /// </summary>
    [EnumAltName("T")]
    Total,
}

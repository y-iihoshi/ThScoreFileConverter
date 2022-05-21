//-----------------------------------------------------------------------
// <copyright file="StageWithTotal.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using ThScoreFileConverter.Core.Models;

namespace ThScoreFileConverter.Models.Th08;

/// <summary>
/// Represents stages of IN and total.
/// </summary>
public enum StageWithTotal
{
    /// <summary>
    /// Represents stage 1.
    /// </summary>
    [EnumAltName("1A")]
    One,

    /// <summary>
    /// Represents stage 2.
    /// </summary>
    [EnumAltName("2A")]
    Two,

    /// <summary>
    /// Represents stage 3.
    /// </summary>
    [EnumAltName("3A")]
    Three,

    /// <summary>
    /// Represents stage 4 Uncanny.
    /// </summary>
    [EnumAltName("4A")]
    FourUncanny,

    /// <summary>
    /// Represents stage 4 Powerful.
    /// </summary>
    [EnumAltName("4B")]
    FourPowerful,

    /// <summary>
    /// Represents stage 5.
    /// </summary>
    [EnumAltName("5A")]
    Five,

    /// <summary>
    /// Represents stage Final A.
    /// </summary>
    [EnumAltName("6A")]
    FinalA,

    /// <summary>
    /// Represents stage Final B.
    /// </summary>
    [EnumAltName("6B")]
    FinalB,

    /// <summary>
    /// Represents Extra stage.
    /// </summary>
    [EnumAltName("EX")]
    Extra,

    /// <summary>
    /// Represents total across stages.
    /// </summary>
    [EnumAltName("00")]
    Total,
}

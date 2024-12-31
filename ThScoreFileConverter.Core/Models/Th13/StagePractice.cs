//-----------------------------------------------------------------------
// <copyright file="StagePractice.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Core.Models.Th13;

/// <summary>
/// Represents stages of TD spell practice.
/// </summary>
public enum StagePractice
{
    /// <summary>
    /// Represents stage 1.
    /// </summary>
    [EnumAltName("1")]
    [Pattern("1")]
    One,

    /// <summary>
    /// Represents stage 2.
    /// </summary>
    [EnumAltName("2")]
    [Pattern("2")]
    Two,

    /// <summary>
    /// Represents stage 3.
    /// </summary>
    [EnumAltName("3")]
    [Pattern("3")]
    Three,

    /// <summary>
    /// Represents stage 4.
    /// </summary>
    [EnumAltName("4")]
    [Pattern("4")]
    Four,

    /// <summary>
    /// Represents stage 5.
    /// </summary>
    [EnumAltName("5")]
    [Pattern("5")]
    Five,

    /// <summary>
    /// Represents stage 6.
    /// </summary>
    [EnumAltName("6")]
    [Pattern("6")]
    Six,

    /// <summary>
    /// Represents Extra stage.
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

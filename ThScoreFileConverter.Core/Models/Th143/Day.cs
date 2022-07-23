//-----------------------------------------------------------------------
// <copyright file="Day.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Core.Models.Th143;

/// <summary>
/// Represents days of ISC.
/// </summary>
public enum Day
{
    /// <summary>
    /// The 1st Day.
    /// </summary>
    [EnumAltName("1", LongName = "01")]
    First,

    /// <summary>
    /// The 2nd Day.
    /// </summary>
    [EnumAltName("2", LongName = "02")]
    Second,

    /// <summary>
    /// The 3rd Day.
    /// </summary>
    [EnumAltName("3", LongName = "03")]
    Third,

    /// <summary>
    /// The 4th Day.
    /// </summary>
    [EnumAltName("4", LongName = "04")]
    Fourth,

    /// <summary>
    /// The 5th Day.
    /// </summary>
    [EnumAltName("5", LongName = "05")]
    Fifth,

    /// <summary>
    /// The 6th Day.
    /// </summary>
    [EnumAltName("6", LongName = "06")]
    Sixth,

    /// <summary>
    /// The 7th Day.
    /// </summary>
    [EnumAltName("7", LongName = "07")]
    Seventh,

    /// <summary>
    /// The 8th Day.
    /// </summary>
    [EnumAltName("8", LongName = "08")]
    Eighth,

    /// <summary>
    /// The 9th Day.
    /// </summary>
    [EnumAltName("9", LongName = "09")]
    Ninth,

    /// <summary>
    /// The Last Day.
    /// </summary>
    [EnumAltName("L", LongName = "10")]
    Last,
}

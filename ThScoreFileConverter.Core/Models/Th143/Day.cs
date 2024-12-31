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
    [Pattern("1")]
    First,

    /// <summary>
    /// The 2nd Day.
    /// </summary>
    [EnumAltName("2", LongName = "02")]
    [Pattern("2")]
    Second,

    /// <summary>
    /// The 3rd Day.
    /// </summary>
    [EnumAltName("3", LongName = "03")]
    [Pattern("3")]
    Third,

    /// <summary>
    /// The 4th Day.
    /// </summary>
    [EnumAltName("4", LongName = "04")]
    [Pattern("4")]
    Fourth,

    /// <summary>
    /// The 5th Day.
    /// </summary>
    [EnumAltName("5", LongName = "05")]
    [Pattern("5")]
    Fifth,

    /// <summary>
    /// The 6th Day.
    /// </summary>
    [EnumAltName("6", LongName = "06")]
    [Pattern("6")]
    Sixth,

    /// <summary>
    /// The 7th Day.
    /// </summary>
    [EnumAltName("7", LongName = "07")]
    [Pattern("7")]
    Seventh,

    /// <summary>
    /// The 8th Day.
    /// </summary>
    [EnumAltName("8", LongName = "08")]
    [Pattern("8")]
    Eighth,

    /// <summary>
    /// The 9th Day.
    /// </summary>
    [EnumAltName("9", LongName = "09")]
    [Pattern("9")]
    Ninth,

    /// <summary>
    /// The Last Day.
    /// </summary>
    [EnumAltName("L", LongName = "10")]
    [Pattern("L")]
    Last,
}

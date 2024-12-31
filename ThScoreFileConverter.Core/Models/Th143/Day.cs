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
    [Pattern("1")]
    First,

    /// <summary>
    /// The 2nd Day.
    /// </summary>
    [Pattern("2")]
    Second,

    /// <summary>
    /// The 3rd Day.
    /// </summary>
    [Pattern("3")]
    Third,

    /// <summary>
    /// The 4th Day.
    /// </summary>
    [Pattern("4")]
    Fourth,

    /// <summary>
    /// The 5th Day.
    /// </summary>
    [Pattern("5")]
    Fifth,

    /// <summary>
    /// The 6th Day.
    /// </summary>
    [Pattern("6")]
    Sixth,

    /// <summary>
    /// The 7th Day.
    /// </summary>
    [Pattern("7")]
    Seventh,

    /// <summary>
    /// The 8th Day.
    /// </summary>
    [Pattern("8")]
    Eighth,

    /// <summary>
    /// The 9th Day.
    /// </summary>
    [Pattern("9")]
    Ninth,

    /// <summary>
    /// The Last Day.
    /// </summary>
    [Pattern("L")]
    Last,
}

//-----------------------------------------------------------------------
// <copyright file="Day.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace ThScoreFileConverter.Core.Models.Th143;

/// <summary>
/// Represents days of ISC.
/// </summary>
public enum Day
{
    /// <summary>
    /// The 1st Day.
    /// </summary>
    [Display(Name = "一日目")]
    [Pattern("1")]
    First,

    /// <summary>
    /// The 2nd Day.
    /// </summary>
    [Display(Name = "二日目")]
    [Pattern("2")]
    Second,

    /// <summary>
    /// The 3rd Day.
    /// </summary>
    [Display(Name = "三日目")]
    [Pattern("3")]
    Third,

    /// <summary>
    /// The 4th Day.
    /// </summary>
    [Display(Name = "四日目")]
    [Pattern("4")]
    Fourth,

    /// <summary>
    /// The 5th Day.
    /// </summary>
    [Display(Name = "五日目")]
    [Pattern("5")]
    Fifth,

    /// <summary>
    /// The 6th Day.
    /// </summary>
    [Display(Name = "六日目")]
    [Pattern("6")]
    Sixth,

    /// <summary>
    /// The 7th Day.
    /// </summary>
    [Display(Name = "七日目")]
    [Pattern("7")]
    Seventh,

    /// <summary>
    /// The 8th Day.
    /// </summary>
    [Display(Name = "八日目")]
    [Pattern("8")]
    Eighth,

    /// <summary>
    /// The 9th Day.
    /// </summary>
    [Display(Name = "九日目")]
    [Pattern("9")]
    Ninth,

    /// <summary>
    /// The Last Day.
    /// </summary>
    [Display(Name = "最終日")]
    [Pattern("L")]
    Last,
}

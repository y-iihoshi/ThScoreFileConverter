//-----------------------------------------------------------------------
// <copyright file="Level.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace ThScoreFileConverter.Core.Models.Th095;

/// <summary>
/// Represents levels of StB.
/// </summary>
public enum Level
{
    /// <summary>
    /// Represents level 1.
    /// </summary>
    [Display(Name = "Level 1")]
    [Pattern("1")]
    One,

    /// <summary>
    /// Represents level 2.
    /// </summary>
    [Display(Name = "Level 2")]
    [Pattern("2")]
    Two,

    /// <summary>
    /// Represents level 3.
    /// </summary>
    [Display(Name = "Level 3")]
    [Pattern("3")]
    Three,

    /// <summary>
    /// Represents level 4.
    /// </summary>
    [Display(Name = "Level 4")]
    [Pattern("4")]
    Four,

    /// <summary>
    /// Represents level 5.
    /// </summary>
    [Display(Name = "Level 5")]
    [Pattern("5")]
    Five,

    /// <summary>
    /// Represents level 6.
    /// </summary>
    [Display(Name = "Level 6")]
    [Pattern("6")]
    Six,

    /// <summary>
    /// Represents level 7.
    /// </summary>
    [Display(Name = "Level 7")]
    [Pattern("7")]
    Seven,

    /// <summary>
    /// Represents level 8.
    /// </summary>
    [Display(Name = "Level 8")]
    [Pattern("8")]
    Eight,

    /// <summary>
    /// Represents level 9.
    /// </summary>
    [Display(Name = "Level 9")]
    [Pattern("9")]
    Nine,

    /// <summary>
    /// Represents level 10.
    /// </summary>
    [Display(Name = "Level 10")]
    [Pattern("0")]
    Ten,

    /// <summary>
    /// Represents level Extra.
    /// </summary>
    [Display(Name = "Level Extra")]
    [Pattern("X")]
    Extra,
}

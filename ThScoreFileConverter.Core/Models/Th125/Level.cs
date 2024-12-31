//-----------------------------------------------------------------------
// <copyright file="Level.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace ThScoreFileConverter.Core.Models.Th125;

/// <summary>
/// Represents levels of DS.
/// </summary>
public enum Level
{
    /// <summary>
    /// Represents level 1.
    /// </summary>
    [Display(Name = "Level 1", ShortName = "1")]
    [Pattern("1")]
    One,

    /// <summary>
    /// Represents level 2.
    /// </summary>
    [Display(Name = "Level 2", ShortName = "2")]
    [Pattern("2")]
    Two,

    /// <summary>
    /// Represents level 3.
    /// </summary>
    [Display(Name = "Level 3", ShortName = "3")]
    [Pattern("3")]
    Three,

    /// <summary>
    /// Represents level 4.
    /// </summary>
    [Display(Name = "Level 4", ShortName = "4")]
    [Pattern("4")]
    Four,

    /// <summary>
    /// Represents level 5.
    /// </summary>
    [Display(Name = "Level 5", ShortName = "5")]
    [Pattern("5")]
    Five,

    /// <summary>
    /// Represents level 6.
    /// </summary>
    [Display(Name = "Level 6", ShortName = "6")]
    [Pattern("6")]
    Six,

    /// <summary>
    /// Represents level 7.
    /// </summary>
    [Display(Name = "Level 7", ShortName = "7")]
    [Pattern("7")]
    Seven,

    /// <summary>
    /// Represents level 8.
    /// </summary>
    [Display(Name = "Level 8", ShortName = "8")]
    [Pattern("8")]
    Eight,

    /// <summary>
    /// Represents level 9.
    /// </summary>
    [Display(Name = "Level 9", ShortName = "9")]
    [Pattern("9")]
    Nine,

    /// <summary>
    /// Represents level 10.
    /// </summary>
    [Display(Name = "Level 10", ShortName = "10")]
    [Pattern("A")]
    Ten,

    /// <summary>
    /// Represents level 11.
    /// </summary>
    [Display(Name = "Level 11", ShortName = "11")]
    [Pattern("B")]
    Eleven,

    /// <summary>
    /// Represents level 12.
    /// </summary>
    [Display(Name = "Level 12", ShortName = "12")]
    [Pattern("C")]
    Twelve,

    /// <summary>
    /// Represents level Extra.
    /// </summary>
    [Display(Name = "Level EX", ShortName = "EX")]
    [Pattern("X")]
    Extra,

    /// <summary>
    /// Represents level Spoiler.
    /// </summary>
    [Display(Name = "SPOILER", ShortName = "??")]
    [Pattern("S")]
    Spoiler,
}

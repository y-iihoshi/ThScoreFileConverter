//-----------------------------------------------------------------------
// <copyright file="StageWithTotal.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace ThScoreFileConverter.Core.Models.Th07;

/// <summary>
/// Represents stages of PCB and total.
/// </summary>
public enum StageWithTotal
{
    /// <summary>
    /// Represents stage 1.
    /// </summary>
    [Display(Name = "Stage 1")]
    [Pattern("1")]
    One,

    /// <summary>
    /// Represents stage 2.
    /// </summary>
    [Display(Name = "Stage 2")]
    [Pattern("2")]
    Two,

    /// <summary>
    /// Represents stage 3.
    /// </summary>
    [Display(Name = "Stage 3")]
    [Pattern("3")]
    Three,

    /// <summary>
    /// Represents stage 4.
    /// </summary>
    [Display(Name = "Stage 4")]
    [Pattern("4")]
    Four,

    /// <summary>
    /// Represents stage 5.
    /// </summary>
    [Display(Name = "Stage 5")]
    [Pattern("5")]
    Five,

    /// <summary>
    /// Represents stage 6.
    /// </summary>
    [Display(Name = "Stage 6")]
    [Pattern("6")]
    Six,

    /// <summary>
    /// Represents Extra stage.
    /// </summary>
    [Pattern("X")]
    Extra,

    /// <summary>
    /// Represents Phantasm stage.
    /// </summary>
    [Pattern("P")]
    Phantasm,

    /// <summary>
    /// Represents total across stages.
    /// </summary>
    [Pattern("0")]
    Total,
}

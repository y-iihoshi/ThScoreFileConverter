//-----------------------------------------------------------------------
// <copyright file="StageProgress.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace ThScoreFileConverter.Models.Th08;

/// <summary>
/// Represents a stage progress of a gameplay.
/// </summary>
public enum StageProgress
{
    /// <summary>
    /// Lost at stage 1.
    /// </summary>
    [Display(Name = "Stage 1")]
    One,

    /// <summary>
    /// Lost at stage 2.
    /// </summary>
    [Display(Name = "Stage 2")]
    Two,

    /// <summary>
    /// Lost at stage 3.
    /// </summary>
    [Display(Name = "Stage 3")]
    Three,

    /// <summary>
    /// Lost at stage 4 Uncanny.
    /// </summary>
    [Display(Name = "Stage 4-uncanny")]
    FourUncanny,

    /// <summary>
    /// Lost at stage 4 Powerful.
    /// </summary>
    [Display(Name = "Stage 4-powerful")]
    FourPowerful,

    /// <summary>
    /// Lost at stage 5.
    /// </summary>
    [Display(Name = "Stage 5")]
    Five,

    /// <summary>
    /// Lost at stage Final A.
    /// </summary>
    [Display(Name = "Stage 6-Eirin")]
    FinalA,

    /// <summary>
    /// Lost at stage Final B.
    /// </summary>
    [Display(Name = "Stage 6-Kaguya")]
    FinalB,

    /// <summary>
    /// Lost at Extra stage.
    /// </summary>
    [Display(Name = "Extra Stage")]
    Extra,

    /// <summary>
    /// All cleared.
    /// </summary>
    [Display(Name = "All Clear")]
    Clear = 99,
}

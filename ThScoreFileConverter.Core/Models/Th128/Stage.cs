//-----------------------------------------------------------------------
// <copyright file="Stage.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable CA1707 // Identifiers should not contain underscores

using System.ComponentModel.DataAnnotations;

namespace ThScoreFileConverter.Core.Models.Th128;

/// <summary>
/// Represents stages of FW.
/// </summary>
public enum Stage
{
    /// <summary>
    /// Stage A-1.
    /// </summary>
    [Display(Name = "Stage A-1")]
    [Pattern("A11")]
    A_1,

    /// <summary>
    /// Stage A1-2.
    /// </summary>
    [Display(Name = "Stage A1-2")]
    [Pattern("A12")]
    A1_2,

    /// <summary>
    /// Stage A1-3.
    /// </summary>
    [Display(Name = "Stage A1-3")]
    [Pattern("A13")]
    A1_3,

    /// <summary>
    /// Stage A2-2.
    /// </summary>
    [Display(Name = "Stage A2-2")]
    [Pattern("A22")]
    A2_2,

    /// <summary>
    /// Stage A2-3.
    /// </summary>
    [Display(Name = "Stage A2-3")]
    [Pattern("A23")]
    A2_3,

    /// <summary>
    /// Stage B-1.
    /// </summary>
    [Display(Name = "Stage B-1")]
    [Pattern("B11")]
    B_1,

    /// <summary>
    /// Stage B1-2.
    /// </summary>
    [Display(Name = "Stage B1-2")]
    [Pattern("B12")]
    B1_2,

    /// <summary>
    /// Stage B1-3.
    /// </summary>
    [Display(Name = "Stage B1-3")]
    [Pattern("B13")]
    B1_3,

    /// <summary>
    /// Stage B2-2.
    /// </summary>
    [Display(Name = "Stage B2-2")]
    [Pattern("B22")]
    B2_2,

    /// <summary>
    /// Stage B2-3.
    /// </summary>
    [Display(Name = "Stage B2-3")]
    [Pattern("B23")]
    B2_3,

    /// <summary>
    /// Stage C-1.
    /// </summary>
    [Display(Name = "Stage C-1")]
    [Pattern("C11")]
    C_1,

    /// <summary>
    /// Stage C1-2.
    /// </summary>
    [Display(Name = "Stage C1-2")]
    [Pattern("C12")]
    C1_2,

    /// <summary>
    /// Stage C1-3.
    /// </summary>
    [Display(Name = "Stage C1-3")]
    [Pattern("C13")]
    C1_3,

    /// <summary>
    /// Stage C2-2.
    /// </summary>
    [Display(Name = "Stage C2-2")]
    [Pattern("C22")]
    C2_2,

    /// <summary>
    /// Stage C2-3.
    /// </summary>
    [Display(Name = "Stage C2-3")]
    [Pattern("C23")]
    C2_3,

    /// <summary>
    /// Stage Extra.
    /// </summary>
    [Pattern("EXT")]
    Extra,
}

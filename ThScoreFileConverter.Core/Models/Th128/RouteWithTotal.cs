//-----------------------------------------------------------------------
// <copyright file="RouteWithTotal.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace ThScoreFileConverter.Core.Models.Th128;

/// <summary>
/// Represents routes of FW and total.
/// </summary>
public enum RouteWithTotal
{
    /// <summary>
    /// Route A1.
    /// </summary>
    [Display(Name = "ルート A1")]
    [Pattern("A1")]
    A1,

    /// <summary>
    /// Route A2.
    /// </summary>
    [Display(Name = "ルート A2")]
    [Pattern("A2")]
    A2,

    /// <summary>
    /// Route B1.
    /// </summary>
    [Display(Name = "ルート B1")]
    [Pattern("B1")]
    B1,

    /// <summary>
    /// Route B2.
    /// </summary>
    [Display(Name = "ルート B2")]
    [Pattern("B2")]
    B2,

    /// <summary>
    /// Route C1.
    /// </summary>
    [Display(Name = "ルート C1")]
    [Pattern("C1")]
    C1,

    /// <summary>
    /// Route C2.
    /// </summary>
    [Display(Name = "ルート C2")]
    [Pattern("C2")]
    C2,

    /// <summary>
    /// Route EX.
    /// </summary>
    [Display(Name = "ルート EX")]
    [Pattern("EX")]
    Extra,

    /// <summary>
    /// Represents total across routes.
    /// </summary>
    [Display(Name = "全ルート合計")]
    [Pattern("TL")]
    Total,
}

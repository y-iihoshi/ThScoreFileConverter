//-----------------------------------------------------------------------
// <copyright file="RouteWithTotal.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Core.Models.Th128;

/// <summary>
/// Represents routes of FW and total.
/// </summary>
public enum RouteWithTotal
{
    /// <summary>
    /// Route A1.
    /// </summary>
    [Pattern("A1")]
    A1,

    /// <summary>
    /// Route A2.
    /// </summary>
    [Pattern("A2")]
    A2,

    /// <summary>
    /// Route B1.
    /// </summary>
    [Pattern("B1")]
    B1,

    /// <summary>
    /// Route B2.
    /// </summary>
    [Pattern("B2")]
    B2,

    /// <summary>
    /// Route C1.
    /// </summary>
    [Pattern("C1")]
    C1,

    /// <summary>
    /// Route C2.
    /// </summary>
    [Pattern("C2")]
    C2,

    /// <summary>
    /// Route EX.
    /// </summary>
    [Pattern("EX")]
    Extra,

    /// <summary>
    /// Represents total across routes.
    /// </summary>
    [Pattern("TL")]
    Total,
}

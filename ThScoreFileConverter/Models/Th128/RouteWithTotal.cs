//-----------------------------------------------------------------------
// <copyright file="RouteWithTotal.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Models.Th128;

/// <summary>
/// Represents routes of FW and total.
/// </summary>
public enum RouteWithTotal
{
    /// <summary>
    /// Route A1.
    /// </summary>
    [EnumAltName("A1")]
    A1,

    /// <summary>
    /// Route A2.
    /// </summary>
    [EnumAltName("A2")]
    A2,

    /// <summary>
    /// Route B1.
    /// </summary>
    [EnumAltName("B1")]
    B1,

    /// <summary>
    /// Route B2.
    /// </summary>
    [EnumAltName("B2")]
    B2,

    /// <summary>
    /// Route C1.
    /// </summary>
    [EnumAltName("C1")]
    C1,

    /// <summary>
    /// Route C2.
    /// </summary>
    [EnumAltName("C2")]
    C2,

    /// <summary>
    /// Route EX.
    /// </summary>
    [EnumAltName("EX")]
    Extra,

    /// <summary>
    /// Represents total across routes.
    /// </summary>
    [EnumAltName("TL")]
    Total,
}

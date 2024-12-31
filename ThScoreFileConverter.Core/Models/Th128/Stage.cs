//-----------------------------------------------------------------------
// <copyright file="Stage.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable CA1707 // Identifiers should not contain underscores

namespace ThScoreFileConverter.Core.Models.Th128;

/// <summary>
/// Represents stages of FW.
/// </summary>
public enum Stage
{
    /// <summary>
    /// Stage A-1.
    /// </summary>
    [Pattern("A11")]
    A_1,

    /// <summary>
    /// Stage A1-2.
    /// </summary>
    [Pattern("A12")]
    A1_2,

    /// <summary>
    /// Stage A1-3.
    /// </summary>
    [Pattern("A13")]
    A1_3,

    /// <summary>
    /// Stage A2-2.
    /// </summary>
    [Pattern("A22")]
    A2_2,

    /// <summary>
    /// Stage A2-3.
    /// </summary>
    [Pattern("A23")]
    A2_3,

    /// <summary>
    /// Stage B-1.
    /// </summary>
    [Pattern("B11")]
    B_1,

    /// <summary>
    /// Stage B1-2.
    /// </summary>
    [Pattern("B12")]
    B1_2,

    /// <summary>
    /// Stage B1-3.
    /// </summary>
    [Pattern("B13")]
    B1_3,

    /// <summary>
    /// Stage B2-2.
    /// </summary>
    [Pattern("B22")]
    B2_2,

    /// <summary>
    /// Stage B2-3.
    /// </summary>
    [Pattern("B23")]
    B2_3,

    /// <summary>
    /// Stage C-1.
    /// </summary>
    [Pattern("C11")]
    C_1,

    /// <summary>
    /// Stage C1-2.
    /// </summary>
    [Pattern("C12")]
    C1_2,

    /// <summary>
    /// Stage C1-3.
    /// </summary>
    [Pattern("C13")]
    C1_3,

    /// <summary>
    /// Stage C2-2.
    /// </summary>
    [Pattern("C22")]
    C2_2,

    /// <summary>
    /// Stage C2-3.
    /// </summary>
    [Pattern("C23")]
    C2_3,

    /// <summary>
    /// Stage Extra.
    /// </summary>
    [Pattern("EXT")]
    Extra,
}

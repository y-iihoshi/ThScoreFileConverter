//-----------------------------------------------------------------------
// <copyright file="StageWithTotal.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Core.Models;

/// <summary>
/// Represents stage and total.
/// </summary>
public enum StageWithTotal
{
    /// <summary>
    /// Represents stage 1.
    /// </summary>
    [Pattern("1")]
    One,

    /// <summary>
    /// Represents stage 2.
    /// </summary>
    [Pattern("2")]
    Two,

    /// <summary>
    /// Represents stage 3.
    /// </summary>
    [Pattern("3")]
    Three,

    /// <summary>
    /// Represents stage 4.
    /// </summary>
    [Pattern("4")]
    Four,

    /// <summary>
    /// Represents stage 5.
    /// </summary>
    [Pattern("5")]
    Five,

    /// <summary>
    /// Represents stage 6.
    /// </summary>
    [Pattern("6")]
    Six,

    /// <summary>
    /// Represents Extra stage.
    /// </summary>
    [Pattern("X")]
    Extra,

    /// <summary>
    /// Represents total across stages.
    /// </summary>
    [Pattern("0")]
    Total,
}

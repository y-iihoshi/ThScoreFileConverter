﻿//-----------------------------------------------------------------------
// <copyright file="StageWithTotal.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Core.Models.Th08;

/// <summary>
/// Represents stages of IN and total.
/// </summary>
public enum StageWithTotal
{
    /// <summary>
    /// Represents stage 1.
    /// </summary>
    [Pattern("1A")]
    One,

    /// <summary>
    /// Represents stage 2.
    /// </summary>
    [Pattern("2A")]
    Two,

    /// <summary>
    /// Represents stage 3.
    /// </summary>
    [Pattern("3A")]
    Three,

    /// <summary>
    /// Represents stage 4 Uncanny.
    /// </summary>
    [Pattern("4A")]
    FourUncanny,

    /// <summary>
    /// Represents stage 4 Powerful.
    /// </summary>
    [Pattern("4B")]
    FourPowerful,

    /// <summary>
    /// Represents stage 5.
    /// </summary>
    [Pattern("5A")]
    Five,

    /// <summary>
    /// Represents stage Final A.
    /// </summary>
    [Pattern("6A")]
    FinalA,

    /// <summary>
    /// Represents stage Final B.
    /// </summary>
    [Pattern("6B")]
    FinalB,

    /// <summary>
    /// Represents Extra stage.
    /// </summary>
    [Pattern("EX")]
    Extra,

    /// <summary>
    /// Represents total across stages.
    /// </summary>
    [Pattern("00")]
    Total,
}

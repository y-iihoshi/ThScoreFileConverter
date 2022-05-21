﻿//-----------------------------------------------------------------------
// <copyright file="StageWithTotal.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using ThScoreFileConverter.Core.Models;

namespace ThScoreFileConverter.Models;

/// <summary>
/// Represents stage and total.
/// </summary>
public enum StageWithTotal
{
    /// <summary>
    /// Represents stage 1.
    /// </summary>
    [EnumAltName("1")]
    One,

    /// <summary>
    /// Represents stage 2.
    /// </summary>
    [EnumAltName("2")]
    Two,

    /// <summary>
    /// Represents stage 3.
    /// </summary>
    [EnumAltName("3")]
    Three,

    /// <summary>
    /// Represents stage 4.
    /// </summary>
    [EnumAltName("4")]
    Four,

    /// <summary>
    /// Represents stage 5.
    /// </summary>
    [EnumAltName("5")]
    Five,

    /// <summary>
    /// Represents stage 6.
    /// </summary>
    [EnumAltName("6")]
    Six,

    /// <summary>
    /// Represents Extra stage.
    /// </summary>
    [EnumAltName("X")]
    Extra,

    /// <summary>
    /// Represents total across stages.
    /// </summary>
    [EnumAltName("0")]
    Total,
}

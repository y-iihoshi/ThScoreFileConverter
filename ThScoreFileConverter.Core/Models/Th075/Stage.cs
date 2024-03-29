﻿//-----------------------------------------------------------------------
// <copyright file="Stage.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Core.Models.Th075;

/// <summary>
/// Represents stages of IMP.
/// </summary>
public enum Stage
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
    /// Represents stage 7.
    /// </summary>
    [EnumAltName("7")]
    Seven,
}

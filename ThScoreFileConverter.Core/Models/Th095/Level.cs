//-----------------------------------------------------------------------
// <copyright file="Level.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Core.Models.Th095;

/// <summary>
/// Represents levels of StB.
/// </summary>
public enum Level
{
    /// <summary>
    /// Represents level 1.
    /// </summary>
    [Pattern("1")]
    One,

    /// <summary>
    /// Represents level 2.
    /// </summary>
    [Pattern("2")]
    Two,

    /// <summary>
    /// Represents level 3.
    /// </summary>
    [Pattern("3")]
    Three,

    /// <summary>
    /// Represents level 4.
    /// </summary>
    [Pattern("4")]
    Four,

    /// <summary>
    /// Represents level 5.
    /// </summary>
    [Pattern("5")]
    Five,

    /// <summary>
    /// Represents level 6.
    /// </summary>
    [Pattern("6")]
    Six,

    /// <summary>
    /// Represents level 7.
    /// </summary>
    [Pattern("7")]
    Seven,

    /// <summary>
    /// Represents level 8.
    /// </summary>
    [Pattern("8")]
    Eight,

    /// <summary>
    /// Represents level 9.
    /// </summary>
    [Pattern("9")]
    Nine,

    /// <summary>
    /// Represents level 10.
    /// </summary>
    [Pattern("0")]
    Ten,

    /// <summary>
    /// Represents level Extra.
    /// </summary>
    [Pattern("X")]
    Extra,
}

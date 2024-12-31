//-----------------------------------------------------------------------
// <copyright file="LevelWithTotal.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Core.Models;

/// <summary>
/// Represents level and total.
/// </summary>
public enum LevelWithTotal
{
    /// <summary>
    /// Represents level Easy.
    /// </summary>
    [Pattern("E")]
    Easy,

    /// <summary>
    /// Represents level Normal.
    /// </summary>
    [Pattern("N")]
    Normal,

    /// <summary>
    /// Represents level Hard.
    /// </summary>
    [Pattern("H")]
    Hard,

    /// <summary>
    /// Represents level Lunatic.
    /// </summary>
    [Pattern("L")]
    Lunatic,

    /// <summary>
    /// Represents level Extra.
    /// </summary>
    [Pattern("X")]
    Extra,

    /// <summary>
    /// Represents total across levels.
    /// </summary>
    [Pattern("T")]
    Total,
}

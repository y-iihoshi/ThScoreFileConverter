//-----------------------------------------------------------------------
// <copyright file="GameMode.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Models;

/// <summary>
/// Represents game modes.
/// </summary>
public enum GameMode
{
    /// <summary>
    /// Story Mode.
    /// </summary>
    [EnumAltName("S")]
    Story,

    /// <summary>
    /// Spell Practice Mode.
    /// </summary>
    [EnumAltName("P")]
    SpellPractice,
}

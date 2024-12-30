//-----------------------------------------------------------------------
// <copyright file="GameMode.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Core.Models;

/// <summary>
/// Represents game modes.
/// </summary>
public enum GameMode
{
    /// <summary>
    /// Story Mode.
    /// </summary>
    [Pattern("S")]
    Story,

    /// <summary>
    /// Spell Practice Mode.
    /// </summary>
    [Pattern("P")]
    SpellPractice,
}

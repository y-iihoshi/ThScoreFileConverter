//-----------------------------------------------------------------------
// <copyright file="Chara.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Core.Models.Th175;

/// <summary>
/// Represents player characters of SFW.
/// </summary>
public enum Chara
{
    /// <summary>
    /// Hakurei Reimu.
    /// </summary>
    [Pattern("RM")]
    [Character(nameof(Reimu))]
    Reimu,

    /// <summary>
    /// Kirisame Marisa.
    /// </summary>
    [Pattern("MR")]
    [Character(nameof(Marisa))]
    Marisa,

    /// <summary>
    /// Yasaka Kanako.
    /// </summary>
    [Pattern("KN")]
    [Character(nameof(Kanako))]
    Kanako,

    /// <summary>
    /// Murasa Minamitsu.
    /// </summary>
    [Pattern("MI")]
    [Character(nameof(Minamitsu))]
    Minamitsu,

    /// <summary>
    /// Yorigami Joon and Yorigami Shion.
    /// </summary>
    [Pattern("JS")]
    [Character("Joon")]
    [Character("Shion", 1)]
    JoonShion,

    /// <summary>
    /// Flandre Scarlet.
    /// </summary>
    [Pattern("FL")]
    [Character(nameof(Flandre))]
    Flandre,
}

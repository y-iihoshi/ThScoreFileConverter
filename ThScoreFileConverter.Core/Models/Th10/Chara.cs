//-----------------------------------------------------------------------
// <copyright file="Chara.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Core.Models.Th10;

/// <summary>
/// Represents playable characters of MoF.
/// </summary>
public enum Chara
{
    /// <summary>
    /// Hakurei Reimu (Homing type).
    /// </summary>
    [Pattern("RA")]
    [Character("Reimu")]
    [ShotType<Chara>(ReimuA)]
    ReimuA,

    /// <summary>
    /// Hakurei Reimu (Forward focus type).
    /// </summary>
    [Pattern("RB")]
    [Character("Reimu")]
    [ShotType<Chara>(ReimuB)]
    ReimuB,

    /// <summary>
    /// Hakurei Reimu (Sealing type).
    /// </summary>
    [Pattern("RC")]
    [Character("Reimu")]
    [ShotType<Chara>(ReimuC)]
    ReimuC,

    /// <summary>
    /// Kirisame Marisa (High-power type).
    /// </summary>
    [Pattern("MA")]
    [Character("Marisa")]
    [ShotType<Chara>(MarisaA)]
    MarisaA,

    /// <summary>
    /// Kirisame Marisa (Piercing type).
    /// </summary>
    [Pattern("MB")]
    [Character("Marisa")]
    [ShotType<Chara>(MarisaB)]
    MarisaB,

    /// <summary>
    /// Kirisame Marisa (Magic-user type).
    /// </summary>
    [Pattern("MC")]
    [Character("Marisa")]
    [ShotType<Chara>(MarisaC)]
    MarisaC,
}

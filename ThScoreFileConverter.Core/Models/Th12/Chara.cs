//-----------------------------------------------------------------------
// <copyright file="Chara.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Core.Models.Th12;

/// <summary>
/// Represents playable characters of UFO.
/// </summary>
public enum Chara
{
    /// <summary>
    /// Hakurei Reimu (Dream Sign).
    /// </summary>
    [Pattern("RA")]
    [Character("Reimu")]
    [ShotType<Chara>(ReimuA)]
    ReimuA,

    /// <summary>
    /// Hakurei Reimu (Spirit Sign).
    /// </summary>
    [Pattern("RB")]
    [Character("Reimu")]
    [ShotType<Chara>(ReimuB)]
    ReimuB,

    /// <summary>
    /// Kirisame Marisa (Love Sign).
    /// </summary>
    [Pattern("MA")]
    [Character("Marisa")]
    [ShotType<Chara>(MarisaA)]
    MarisaA,

    /// <summary>
    /// Kirisame Marisa (Magic Sign).
    /// </summary>
    [Pattern("MB")]
    [Character("Marisa")]
    [ShotType<Chara>(MarisaB)]
    MarisaB,

    /// <summary>
    /// Kochiya Sanae (Snake Sign).
    /// </summary>
    [Pattern("SA")]
    [Character("Sanae")]
    [ShotType<Chara>(SanaeA)]
    SanaeA,

    /// <summary>
    /// Kochiya Sanae (Frog Sign).
    /// </summary>
    [Pattern("SB")]
    [Character("Sanae")]
    [ShotType<Chara>(SanaeB)]
    SanaeB,
}

//-----------------------------------------------------------------------
// <copyright file="CharaWithTotal.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Core.Models.Th10;

/// <summary>
/// Represents playable characters of MoF and total.
/// </summary>
public enum CharaWithTotal
{
    /// <summary>
    /// Hakurei Reimu (Homing type).
    /// </summary>
    [Pattern("RA")]
    [Character("Reimu")]
    [ShotType<CharaWithTotal>(ReimuA)]
    ReimuA,

    /// <summary>
    /// Hakurei Reimu (Forward focus type).
    /// </summary>
    [Pattern("RB")]
    [Character("Reimu")]
    [ShotType<CharaWithTotal>(ReimuB)]
    ReimuB,

    /// <summary>
    /// Hakurei Reimu (Sealing type).
    /// </summary>
    [Pattern("RC")]
    [Character("Reimu")]
    [ShotType<CharaWithTotal>(ReimuC)]
    ReimuC,

    /// <summary>
    /// Kirisame Marisa (High-power type).
    /// </summary>
    [Pattern("MA")]
    [Character("Marisa")]
    [ShotType<CharaWithTotal>(MarisaA)]
    MarisaA,

    /// <summary>
    /// Kirisame Marisa (Piercing type).
    /// </summary>
    [Pattern("MB")]
    [Character("Marisa")]
    [ShotType<CharaWithTotal>(MarisaB)]
    MarisaB,

    /// <summary>
    /// Kirisame Marisa (Magic-user type).
    /// </summary>
    [Pattern("MC")]
    [Character("Marisa")]
    [ShotType<CharaWithTotal>(MarisaC)]
    MarisaC,

    /// <summary>
    /// Represents total across characters.
    /// </summary>
    [Pattern("TL")]
    Total,
}

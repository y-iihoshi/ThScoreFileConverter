//-----------------------------------------------------------------------
// <copyright file="CharaWithTotal.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Core.Models.Th07;

/// <summary>
/// Represents player characters of PCB and total.
/// </summary>
public enum CharaWithTotal
{
    /// <summary>
    /// Hakurei Reimu (Spirit Sign).
    /// </summary>
    [Pattern("RA")]
    [Character("Reimu")]
    [ShotType<CharaWithTotal>(ReimuA)]
    ReimuA,

    /// <summary>
    /// Hakurei Reimu (Dream Sign).
    /// </summary>
    [Pattern("RB")]
    [Character("Reimu")]
    [ShotType<CharaWithTotal>(ReimuB)]
    ReimuB,

    /// <summary>
    /// Kirisame Marisa (Magic Sign).
    /// </summary>
    [Pattern("MA")]
    [Character("Marisa")]
    [ShotType<CharaWithTotal>(MarisaA)]
    MarisaA,

    /// <summary>
    /// Kirisame Marisa (Love Sign).
    /// </summary>
    [Pattern("MB")]
    [Character("Marisa")]
    [ShotType<CharaWithTotal>(MarisaB)]
    MarisaB,

    /// <summary>
    /// Izayoi Sakuya (Illusion Sign).
    /// </summary>
    [Pattern("SA")]
    [Character("Sakuya")]
    [ShotType<CharaWithTotal>(SakuyaA)]
    SakuyaA,

    /// <summary>
    /// Izayoi Sakuya (Time Sign).
    /// </summary>
    [Pattern("SB")]
    [Character("Sakuya")]
    [ShotType<CharaWithTotal>(SakuyaB)]
    SakuyaB,

    /// <summary>
    /// Represents total across characters.
    /// </summary>
    [Pattern("TL")]
    Total,
}

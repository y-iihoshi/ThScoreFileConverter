﻿//-----------------------------------------------------------------------
// <copyright file="Chara.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Core.Models.Th14;

/// <summary>
/// Represents playable characters of DDC.
/// </summary>
public enum Chara
{
    /// <summary>
    /// Hakurei Reimu with the bewitched weapon.
    /// </summary>
    [Pattern("RA")]
    [Character("Reimu")]
    [ShotType<Chara>(ReimuA)]
    ReimuA,

    /// <summary>
    /// Hakurei Reimu without the bewitched weapon.
    /// </summary>
    [Pattern("RB")]
    [Character("Reimu")]
    [ShotType<Chara>(ReimuB)]
    ReimuB,

    /// <summary>
    /// Kirisame Marisa with the bewitched weapon.
    /// </summary>
    [Pattern("MA")]
    [Character("Marisa")]
    [ShotType<Chara>(MarisaA)]
    MarisaA,

    /// <summary>
    /// Kirisame Marisa without the bewitched weapon.
    /// </summary>
    [Pattern("MB")]
    [Character("Marisa")]
    [ShotType<Chara>(MarisaB)]
    MarisaB,

    /// <summary>
    /// Izayoi Sakuya with the bewitched weapon.
    /// </summary>
    [Pattern("SA")]
    [Character("Sakuya")]
    [ShotType<Chara>(SakuyaA)]
    SakuyaA,

    /// <summary>
    /// Izayoi Sakuya without the bewitched weapon.
    /// </summary>
    [Pattern("SB")]
    [Character("Sakuya")]
    [ShotType<Chara>(SakuyaB)]
    SakuyaB,
}

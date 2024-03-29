﻿//-----------------------------------------------------------------------
// <copyright file="CharaWithTotal.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Core.Models.Th14;

/// <summary>
/// Represents playable characters of DDC and total.
/// </summary>
public enum CharaWithTotal
{
    /// <summary>
    /// Hakurei Reimu with the bewitched weapon.
    /// </summary>
    [EnumAltName("RA")]
    ReimuA,

    /// <summary>
    /// Hakurei Reimu without the bewitched weapon.
    /// </summary>
    [EnumAltName("RB")]
    ReimuB,

    /// <summary>
    /// Kirisame Marisa with the bewitched weapon.
    /// </summary>
    [EnumAltName("MA")]
    MarisaA,

    /// <summary>
    /// Kirisame Marisa without the bewitched weapon.
    /// </summary>
    [EnumAltName("MB")]
    MarisaB,

    /// <summary>
    /// Izayoi Sakuya with the bewitched weapon.
    /// </summary>
    [EnumAltName("SA")]
    SakuyaA,

    /// <summary>
    /// Izayoi Sakuya without the bewitched weapon.
    /// </summary>
    [EnumAltName("SB")]
    SakuyaB,

    /// <summary>
    /// Represents total across characters.
    /// </summary>
    [EnumAltName("TL")]
    Total,
}

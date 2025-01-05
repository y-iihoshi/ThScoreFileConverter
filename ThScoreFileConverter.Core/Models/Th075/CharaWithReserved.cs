﻿//-----------------------------------------------------------------------
// <copyright file="CharaWithReserved.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Core.Models.Th075;

/// <summary>
/// Represents playable characters of IMP including reserved.
/// </summary>
public enum CharaWithReserved
{
    /// <summary>
    /// Hakurei Reimu.
    /// </summary>
    [EnumAltName("RM")]
    [Character(nameof(Reimu))]
    Reimu,

    /// <summary>
    /// Kirisame Marisa.
    /// </summary>
    [EnumAltName("MR")]
    [Character(nameof(Marisa))]
    Marisa,

    /// <summary>
    /// Izayoi Sakuya.
    /// </summary>
    [EnumAltName("SK")]
    [Character(nameof(Sakuya))]
    Sakuya,

    /// <summary>
    /// Alice Margatroid.
    /// </summary>
    [EnumAltName("AL")]
    [Character(nameof(Alice))]
    Alice,

    /// <summary>
    /// Patchouli Knowledge.
    /// </summary>
    [EnumAltName("PC")]
    [Character(nameof(Patchouli))]
    Patchouli,

    /// <summary>
    /// Konpaku Youmu.
    /// </summary>
    [EnumAltName("YM")]
    [Character(nameof(Youmu))]
    Youmu,

    /// <summary>
    /// Remilia Scarlet.
    /// </summary>
    [EnumAltName("RL")]
    [Character(nameof(Remilia))]
    Remilia,

    /// <summary>
    /// Saigyouji Yuyuko.
    /// </summary>
    [EnumAltName("YU")]
    [Character(nameof(Yuyuko))]
    Yuyuko,

    /// <summary>
    /// Yakumo Yukari.
    /// </summary>
    [EnumAltName("YK")]
    [Character(nameof(Yukari))]
    Yukari,

    /// <summary>
    /// Ibuki Suika.
    /// </summary>
    [EnumAltName("SU")]
    [Character(nameof(Suika))]
    Suika,

    /// <summary>
    /// Hong Meiling.
    /// </summary>
    [EnumAltName("ML")]
    [Character(nameof(Meiling))]
    Meiling,

#pragma warning disable CA1700 // Do not name enum values 'Reserved'
    /// <summary>
    /// Reserved character #12.
    /// </summary>
    [EnumAltName("12")]
    Reserved12,

    /// <summary>
    /// Reserved character #13.
    /// </summary>
    [EnumAltName("13")]
    Reserved13,

    /// <summary>
    /// Reserved character #14.
    /// </summary>
    [EnumAltName("14")]
    Reserved14,

    /// <summary>
    /// Reserved character #15.
    /// </summary>
    [EnumAltName("15")]
    Reserved15,
#pragma warning restore CA1700 // Do not name enum values 'Reserved'
}

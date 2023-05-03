//-----------------------------------------------------------------------
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
    Reimu,

    /// <summary>
    /// Kirisame Marisa.
    /// </summary>
    [EnumAltName("MR")]
    Marisa,

    /// <summary>
    /// Izayoi Sakuya.
    /// </summary>
    [EnumAltName("SK")]
    Sakuya,

    /// <summary>
    /// Alice Margatroid.
    /// </summary>
    [EnumAltName("AL")]
    Alice,

    /// <summary>
    /// Patchouli Knowledge.
    /// </summary>
    [EnumAltName("PC")]
    Patchouli,

    /// <summary>
    /// Konpaku Youmu.
    /// </summary>
    [EnumAltName("YM")]
    Youmu,

    /// <summary>
    /// Remilia Scarlet.
    /// </summary>
    [EnumAltName("RL")]
    Remilia,

    /// <summary>
    /// Saigyouji Yuyuko.
    /// </summary>
    [EnumAltName("YU")]
    Yuyuko,

    /// <summary>
    /// Yakumo Yukari.
    /// </summary>
    [EnumAltName("YK")]
    Yukari,

    /// <summary>
    /// Ibuki Suika.
    /// </summary>
    [EnumAltName("SU")]
    Suika,

    /// <summary>
    /// Hong Meiling.
    /// </summary>
    [EnumAltName("ML")]
    Meiling,

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
}

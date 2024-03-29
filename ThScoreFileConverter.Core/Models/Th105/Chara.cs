﻿//-----------------------------------------------------------------------
// <copyright file="Chara.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Core.Models.Th105;

/// <summary>
/// Represents playable characters of SWR.
/// </summary>
public enum Chara
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
    /// Reisen Udongein Inaba.
    /// </summary>
    [EnumAltName("RS")]
    Reisen,

    /// <summary>
    /// Shameimaru Aya.
    /// </summary>
    [EnumAltName("AY")]
    Aya,

    /// <summary>
    /// Onozuka Komachi.
    /// </summary>
    [EnumAltName("KM")]
    Komachi,

    /// <summary>
    /// Nagae Iku.
    /// </summary>
    [EnumAltName("IK")]
    Iku,

    /// <summary>
    /// Hinanawi Tenshi.
    /// </summary>
    [EnumAltName("TN")]
    Tenshi,
}

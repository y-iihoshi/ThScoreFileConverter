//-----------------------------------------------------------------------
// <copyright file="Chara.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Models.Th075;

/// <summary>
/// Represents playable characters of IMP.
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
    /// Hong Meiling.
    /// </summary>
    [EnumAltName("ML")]
    Meiling,
}

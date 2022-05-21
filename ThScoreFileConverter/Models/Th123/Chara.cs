//-----------------------------------------------------------------------
// <copyright file="Chara.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using ThScoreFileConverter.Core.Models;

namespace ThScoreFileConverter.Models.Th123;

/// <summary>
/// Represents characters of Hisoutensoku.
/// </summary>
#pragma warning disable CA1027 // Mark enums with FlagsAttribute
public enum Chara
#pragma warning restore CA1027 // Mark enums with FlagsAttribute
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

    /// <summary>
    /// Kochiya Sanae.
    /// </summary>
    [EnumAltName("SN")]
    Sanae,

    /// <summary>
    /// Cirno.
    /// </summary>
    [EnumAltName("CI")]
    Cirno,

    /// <summary>
    /// Hong Meiling.
    /// </summary>
    [EnumAltName("ML")]
    Meiling,

    /// <summary>
    /// Reiuji Utsuho.
    /// </summary>
    [EnumAltName("UT")]
    Utsuho,

    /// <summary>
    /// Moriya Suwako.
    /// </summary>
    [EnumAltName("SW")]
    Suwako,

    /// <summary>
    /// Oonamazu.
    /// </summary>
    [EnumAltName("NM")]
    Oonamazu = 0x15,
}

//-----------------------------------------------------------------------
// <copyright file="Chara.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Models.Th09;

/// <summary>
/// Represents playable characters of PoFV.
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
    /// Konpaku Youmu.
    /// </summary>
    [EnumAltName("YM")]
    Youmu,

    /// <summary>
    /// Reisen Udongein Inaba.
    /// </summary>
    [EnumAltName("RS")]
    Reisen,

    /// <summary>
    /// Cirno.
    /// </summary>
    [EnumAltName("CI")]
    Cirno,

    /// <summary>
    /// Lyrica Prismriver.
    /// </summary>
    [EnumAltName("LY")]
    Lyrica,

    /// <summary>
    /// Mystia Lorelei.
    /// </summary>
    [EnumAltName("MY")]
    Mystia,

    /// <summary>
    /// Inaba Tewi.
    /// </summary>
    [EnumAltName("TW")]
    Tewi,

    /// <summary>
    /// Kazami Yuuka.
    /// </summary>
    [EnumAltName("YU")]
    Yuuka,

    /// <summary>
    /// Shameimaru Aya.
    /// </summary>
    [EnumAltName("AY")]
    Aya,

    /// <summary>
    /// Medicine Melancholy.
    /// </summary>
    [EnumAltName("MD")]
    Medicine,

    /// <summary>
    /// Onozuka Komachi.
    /// </summary>
    [EnumAltName("KM")]
    Komachi,

    /// <summary>
    /// Shiki Eiki, Yamaxanadu.
    /// </summary>
    [EnumAltName("SI")]
    Shikieiki,

    /// <summary>
    /// Merlin Prismriver.
    /// </summary>
    [EnumAltName("ML")]
    Merlin,

    /// <summary>
    /// Lunasa Prismriver.
    /// </summary>
    [EnumAltName("LN")]
    Lunasa,
}

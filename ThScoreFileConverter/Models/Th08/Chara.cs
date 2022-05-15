//-----------------------------------------------------------------------
// <copyright file="Chara.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Models.Th08;

/// <summary>
/// Represents player characters of IN.
/// </summary>
public enum Chara
{
    /// <summary>
    /// Illusionary Barrier Team.
    /// </summary>
    [EnumAltName("RY")]
    ReimuYukari,

    /// <summary>
    /// Aria of Forbidden Magic Team.
    /// </summary>
    [EnumAltName("MA")]
    MarisaAlice,

    /// <summary>
    /// Visionary Scarlet Devil Team.
    /// </summary>
    [EnumAltName("SR")]
    SakuyaRemilia,

    /// <summary>
    /// Netherworld Dwellers' Team.
    /// </summary>
    [EnumAltName("YY")]
    YoumuYuyuko,

    /// <summary>
    /// Hakurei Reimu.
    /// </summary>
    [EnumAltName("RM")]
    Reimu,

    /// <summary>
    /// Yakumo Yukari.
    /// </summary>
    [EnumAltName("YK")]
    Yukari,

    /// <summary>
    /// Kirisame Marisa.
    /// </summary>
    [EnumAltName("MR")]
    Marisa,

    /// <summary>
    /// Alice Margatroid.
    /// </summary>
    [EnumAltName("AL")]
    Alice,

    /// <summary>
    /// Izayoi Sakuya.
    /// </summary>
    [EnumAltName("SK")]
    Sakuya,

    /// <summary>
    /// Remilia Scarlet.
    /// </summary>
    [EnumAltName("RL")]
    Remilia,

    /// <summary>
    /// Konpaku Youmu.
    /// </summary>
    [EnumAltName("YM")]
    Youmu,

    /// <summary>
    /// Saigyouji Yuyuko.
    /// </summary>
    [EnumAltName("YU")]
    Yuyuko,
}

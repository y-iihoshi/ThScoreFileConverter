//-----------------------------------------------------------------------
// <copyright file="Chara.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Core.Models.Th075;

/// <summary>
/// Represents playable characters of IMP.
/// </summary>
public enum Chara
{
    /// <summary>
    /// Hakurei Reimu.
    /// </summary>
    [Pattern("RM")]
    [Character(nameof(Reimu))]
    Reimu,

    /// <summary>
    /// Kirisame Marisa.
    /// </summary>
    [Pattern("MR")]
    [Character(nameof(Marisa))]
    Marisa,

    /// <summary>
    /// Izayoi Sakuya.
    /// </summary>
    [Pattern("SK")]
    [Character(nameof(Sakuya))]
    Sakuya,

    /// <summary>
    /// Alice Margatroid.
    /// </summary>
    [Pattern("AL")]
    [Character(nameof(Alice))]
    Alice,

    /// <summary>
    /// Patchouli Knowledge.
    /// </summary>
    [Pattern("PC")]
    [Character(nameof(Patchouli))]
    Patchouli,

    /// <summary>
    /// Konpaku Youmu.
    /// </summary>
    [Pattern("YM")]
    [Character(nameof(Youmu))]
    Youmu,

    /// <summary>
    /// Remilia Scarlet.
    /// </summary>
    [Pattern("RL")]
    [Character(nameof(Remilia))]
    Remilia,

    /// <summary>
    /// Saigyouji Yuyuko.
    /// </summary>
    [Pattern("YU")]
    [Character(nameof(Yuyuko))]
    Yuyuko,

    /// <summary>
    /// Yakumo Yukari.
    /// </summary>
    [Pattern("YK")]
    [Character(nameof(Yukari))]
    Yukari,

    /// <summary>
    /// Ibuki Suika.
    /// </summary>
    [Pattern("SU")]
    [Character(nameof(Suika))]
    Suika,

    /// <summary>
    /// Hong Meiling.
    /// </summary>
    [Pattern("ML")]
    [Character(nameof(Meiling))]
    Meiling,
}

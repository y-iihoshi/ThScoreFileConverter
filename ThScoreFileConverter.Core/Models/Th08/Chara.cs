//-----------------------------------------------------------------------
// <copyright file="Chara.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Core.Models.Th08;

/// <summary>
/// Represents player characters of IN.
/// </summary>
public enum Chara
{
    /// <summary>
    /// Illusionary Barrier Team.
    /// </summary>
    [Pattern("RY")]
    [Character(nameof(Reimu))]
    [Character(nameof(Yukari), 1)]
    ReimuYukari,

    /// <summary>
    /// Aria of Forbidden Magic Team.
    /// </summary>
    [Pattern("MA")]
    [Character(nameof(Marisa))]
    [Character(nameof(Alice), 1)]
    MarisaAlice,

    /// <summary>
    /// Visionary Scarlet Devil Team.
    /// </summary>
    [Pattern("SR")]
    [Character(nameof(Sakuya))]
    [Character(nameof(Remilia), 1)]
    SakuyaRemilia,

    /// <summary>
    /// Netherworld Dwellers' Team.
    /// </summary>
    [Pattern("YY")]
    [Character(nameof(Youmu))]
    [Character(nameof(Yuyuko), 1)]
    YoumuYuyuko,

    /// <summary>
    /// Hakurei Reimu.
    /// </summary>
    [Pattern("RM")]
    [Character(nameof(Reimu))]
    Reimu,

    /// <summary>
    /// Yakumo Yukari.
    /// </summary>
    [Pattern("YK")]
    [Character(nameof(Yukari))]
    Yukari,

    /// <summary>
    /// Kirisame Marisa.
    /// </summary>
    [Pattern("MR")]
    [Character(nameof(Marisa))]
    Marisa,

    /// <summary>
    /// Alice Margatroid.
    /// </summary>
    [Pattern("AL")]
    [Character(nameof(Alice))]
    Alice,

    /// <summary>
    /// Izayoi Sakuya.
    /// </summary>
    [Pattern("SK")]
    [Character(nameof(Sakuya))]
    Sakuya,

    /// <summary>
    /// Remilia Scarlet.
    /// </summary>
    [Pattern("RL")]
    [Character(nameof(Remilia))]
    Remilia,

    /// <summary>
    /// Konpaku Youmu.
    /// </summary>
    [Pattern("YM")]
    [Character(nameof(Youmu))]
    Youmu,

    /// <summary>
    /// Saigyouji Yuyuko.
    /// </summary>
    [Pattern("YU")]
    [Character(nameof(Yuyuko))]
    Yuyuko,
}

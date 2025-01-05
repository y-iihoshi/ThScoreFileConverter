//-----------------------------------------------------------------------
// <copyright file="Chara.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Core.Models.Th09;

/// <summary>
/// Represents playable characters of PoFV.
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
    /// Konpaku Youmu.
    /// </summary>
    [Pattern("YM")]
    [Character(nameof(Youmu))]
    Youmu,

    /// <summary>
    /// Reisen Udongein Inaba.
    /// </summary>
    [Pattern("RS")]
    [Character(nameof(Reisen))]
    Reisen,

    /// <summary>
    /// Cirno.
    /// </summary>
    [Pattern("CI")]
    [Character(nameof(Cirno))]
    Cirno,

    /// <summary>
    /// Lyrica Prismriver.
    /// </summary>
    [Pattern("LY")]
    [Character(nameof(Lyrica))]
    Lyrica,

    /// <summary>
    /// Mystia Lorelei.
    /// </summary>
    [Pattern("MY")]
    [Character(nameof(Mystia))]
    Mystia,

    /// <summary>
    /// Inaba Tewi.
    /// </summary>
    [Pattern("TW")]
    [Character(nameof(Tewi))]
    Tewi,

    /// <summary>
    /// Kazami Yuuka.
    /// </summary>
    [Pattern("YU")]
    [Character(nameof(Yuuka))]
    Yuuka,

    /// <summary>
    /// Shameimaru Aya.
    /// </summary>
    [Pattern("AY")]
    [Character(nameof(Aya))]
    Aya,

    /// <summary>
    /// Medicine Melancholy.
    /// </summary>
    [Pattern("MD")]
    [Character(nameof(Medicine))]
    Medicine,

    /// <summary>
    /// Onozuka Komachi.
    /// </summary>
    [Pattern("KM")]
    [Character(nameof(Komachi))]
    Komachi,

    /// <summary>
    /// Shiki Eiki, Yamaxanadu.
    /// </summary>
    [Pattern("SI")]
    [Character(nameof(Eiki))]
    Eiki,

    /// <summary>
    /// Merlin Prismriver.
    /// </summary>
    [Pattern("ML")]
    [Character(nameof(Merlin))]
    Merlin,

    /// <summary>
    /// Lunasa Prismriver.
    /// </summary>
    [Pattern("LN")]
    [Character(nameof(Lunasa))]
    Lunasa,
}

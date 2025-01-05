//-----------------------------------------------------------------------
// <copyright file="Chara.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Core.Models.Th17;

/// <summary>
/// Represents player characters of WBWC.
/// </summary>
public enum Chara
{
    /// <summary>
    /// Hakurei Reimu (Wolf spirit).
    /// </summary>
    [Pattern("RA")]
    [Character("Reimu")]
    [ShotType<Chara>(ReimuA)]
    ReimuA,

    /// <summary>
    /// Hakurei Reimu (Otter spirit).
    /// </summary>
    [Pattern("RB")]
    [Character("Reimu")]
    [ShotType<Chara>(ReimuB)]
    ReimuB,

    /// <summary>
    /// Hakurei Reimu (Eagle spirit).
    /// </summary>
    [Pattern("RC")]
    [Character("Reimu")]
    [ShotType<Chara>(ReimuC)]
    ReimuC,

    /// <summary>
    /// Kirisame Reimu (Wolf spirit).
    /// </summary>
    [Pattern("MA")]
    [Character("Marisa")]
    [ShotType<Chara>(MarisaA)]
    MarisaA,

    /// <summary>
    /// Kirisame Marisa (Otter spirit).
    /// </summary>
    [Pattern("MB")]
    [Character("Marisa")]
    [ShotType<Chara>(MarisaB)]
    MarisaB,

    /// <summary>
    /// Kirisame Marisa (Eagle spirit).
    /// </summary>
    [Pattern("MC")]
    [Character("Marisa")]
    [ShotType<Chara>(MarisaC)]
    MarisaC,

    /// <summary>
    /// Konpaku Youmu (Wolf spirit).
    /// </summary>
    [Pattern("YA")]
    [Character("Youmu")]
    [ShotType<Chara>(YoumuA)]
    YoumuA,

    /// <summary>
    /// Konpaku Youmu (Otter spirit).
    /// </summary>
    [Pattern("YB")]
    [Character("Youmu")]
    [ShotType<Chara>(YoumuB)]
    YoumuB,

    /// <summary>
    /// Konpaku Youmu (Eagle spirit).
    /// </summary>
    [Pattern("YC")]
    [Character("Youmu")]
    [ShotType<Chara>(YoumuC)]
    YoumuC,
}

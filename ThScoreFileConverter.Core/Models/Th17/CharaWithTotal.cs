//-----------------------------------------------------------------------
// <copyright file="CharaWithTotal.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Core.Models.Th17;

/// <summary>
/// Represents player characters of WBWC and total.
/// </summary>
public enum CharaWithTotal
{
    /// <summary>
    /// Hakurei Reimu (Wolf spirit).
    /// </summary>
    [Pattern("RA")]
    [Character("Reimu")]
    [ShotType<CharaWithTotal>(ReimuA)]
    ReimuA,

    /// <summary>
    /// Hakurei Reimu (Otter spirit).
    /// </summary>
    [Pattern("RB")]
    [Character("Reimu")]
    [ShotType<CharaWithTotal>(ReimuB)]
    ReimuB,

    /// <summary>
    /// Hakurei Reimu (Eagle spirit).
    /// </summary>
    [Pattern("RC")]
    [Character("Reimu")]
    [ShotType<CharaWithTotal>(ReimuC)]
    ReimuC,

    /// <summary>
    /// Kirisame Reimu (Wolf spirit).
    /// </summary>
    [Pattern("MA")]
    [Character("Marisa")]
    [ShotType<CharaWithTotal>(MarisaA)]
    MarisaA,

    /// <summary>
    /// Kirisame Marisa (Otter spirit).
    /// </summary>
    [Pattern("MB")]
    [Character("Marisa")]
    [ShotType<CharaWithTotal>(MarisaB)]
    MarisaB,

    /// <summary>
    /// Kirisame Marisa (Eagle spirit).
    /// </summary>
    [Pattern("MC")]
    [Character("Marisa")]
    [ShotType<CharaWithTotal>(MarisaC)]
    MarisaC,

    /// <summary>
    /// Konpaku Youmu (Wolf spirit).
    /// </summary>
    [Pattern("YA")]
    [Character("Youmu")]
    [ShotType<CharaWithTotal>(YoumuA)]
    YoumuA,

    /// <summary>
    /// Konpaku Youmu (Otter spirit).
    /// </summary>
    [Pattern("YB")]
    [Character("Youmu")]
    [ShotType<CharaWithTotal>(YoumuB)]
    YoumuB,

    /// <summary>
    /// Konpaku Youmu (Eagle spirit).
    /// </summary>
    [Pattern("YC")]
    [Character("Youmu")]
    [ShotType<CharaWithTotal>(YoumuC)]
    YoumuC,

    /// <summary>
    /// Represents total across characters.
    /// </summary>
    [Pattern("TL")]
    Total,
}

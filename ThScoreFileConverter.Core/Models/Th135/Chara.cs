//-----------------------------------------------------------------------
// <copyright file="Chara.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Core.Models.Th135;

/// <summary>
/// Represents playable characters of HM.
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
    /// Kumoi Ichirin and Unzan.
    /// </summary>
    [Pattern("IU")]
    [Character("Ichirin")]
    [Character("Unzan", 1)]
    IchirinUnzan,

    /// <summary>
    /// Hijiri Byakuren.
    /// </summary>
    [Pattern("BY")]
    [Character(nameof(Byakuren))]
    Byakuren,

    /// <summary>
    /// Mononobe no Futo.
    /// </summary>
    [Pattern("FT")]
    [Character(nameof(Futo))]
    Futo,

    /// <summary>
    /// Toyosatomimi no Miko.
    /// </summary>
    [Pattern("MK")]
    [Character(nameof(Miko))]
    Miko,

    /// <summary>
    /// Kawashiro Nitori.
    /// </summary>
    [Pattern("NT")]
    [Character(nameof(Nitori))]
    Nitori,

    /// <summary>
    /// Komeiji Koishi.
    /// </summary>
    [Pattern("KO")]
    [Character(nameof(Koishi))]
    Koishi,

    /// <summary>
    /// Futatsuiwa Mamizou.
    /// </summary>
    [Pattern("MM")]
    [Character(nameof(Mamizou))]
    Mamizou,

    /// <summary>
    /// Hata no Kokoro.
    /// </summary>
    [Pattern("KK")]
    [Character(nameof(Kokoro))]
    Kokoro,
}

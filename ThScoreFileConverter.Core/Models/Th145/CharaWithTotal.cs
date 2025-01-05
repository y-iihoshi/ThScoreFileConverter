//-----------------------------------------------------------------------
// <copyright file="CharaWithTotal.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Core.Models.Th145;

/// <summary>
/// Represents playable characters of ULiL and total.
/// </summary>
public enum CharaWithTotal
{
    /// <summary>
    /// Hakurei Reimu (Prologue).
    /// </summary>
    [Pattern("RA")]
    [Character($"Th145.{nameof(ReimuA)}")]  // FIXME
    ReimuA,

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

    /// <summary>
    /// Ibaraki Kasen.
    /// </summary>
    [Pattern("KS")]
    [Character(nameof(Kasen))]
    Kasen,

    /// <summary>
    /// Fujiwara no Mokou.
    /// </summary>
    [Pattern("MO")]
    [Character(nameof(Mokou))]
    Mokou,

    /// <summary>
    /// Sukuna Shinmyoumaru.
    /// </summary>
    [Pattern("SN")]
    [Character(nameof(Shinmyoumaru))]
    Shinmyoumaru,

    /// <summary>
    /// Usami Sumireko.
    /// </summary>
    [Pattern("SM")]
    [Character(nameof(Sumireko))]
    Sumireko,

    /// <summary>
    /// Hakurei Reimu (Final).
    /// </summary>
    [Pattern("RB")]
    [Character($"Th145.{nameof(ReimuB)}")]  // FIXME
    ReimuB,

    /// <summary>
    /// Represents total across characters.
    /// </summary>
    [Pattern("TL")]
    Total,
}

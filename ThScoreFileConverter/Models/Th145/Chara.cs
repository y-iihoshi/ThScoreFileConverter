//-----------------------------------------------------------------------
// <copyright file="Chara.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using ThScoreFileConverter.Core.Models;

namespace ThScoreFileConverter.Models.Th145;

/// <summary>
/// Represents playable characters of ULiL.
/// </summary>
public enum Chara
{
    /// <summary>
    /// Hakurei Reimu (Prologue).
    /// </summary>
    [EnumAltName("RA")]
    ReimuA,

    /// <summary>
    /// Kirisame Marisa.
    /// </summary>
    [EnumAltName("MR")]
    Marisa,

    /// <summary>
    /// Kumoi Ichirin and Unzan.
    /// </summary>
    [EnumAltName("IU")]
    IchirinUnzan,

    /// <summary>
    /// Hijiri Byakuren.
    /// </summary>
    [EnumAltName("BY")]
    Byakuren,

    /// <summary>
    /// Mononobe no Futo.
    /// </summary>
    [EnumAltName("FT")]
    Futo,

    /// <summary>
    /// Toyosatomimi no Miko.
    /// </summary>
    [EnumAltName("MK")]
    Miko,

    /// <summary>
    /// Kawashiro Nitori.
    /// </summary>
    [EnumAltName("NT")]
    Nitori,

    /// <summary>
    /// Komeiji Koishi.
    /// </summary>
    [EnumAltName("KO")]
    Koishi,

    /// <summary>
    /// Futatsuiwa Mamizou.
    /// </summary>
    [EnumAltName("MM")]
    Mamizou,

    /// <summary>
    /// Hata no Kokoro.
    /// </summary>
    [EnumAltName("KK")]
    Kokoro,

    /// <summary>
    /// Ibaraki Kasen.
    /// </summary>
    [EnumAltName("KS")]
    Kasen,

    /// <summary>
    /// Fujiwara no Mokou.
    /// </summary>
    [EnumAltName("MO")]
    Mokou,

    /// <summary>
    /// Sukuna Shinmyoumaru.
    /// </summary>
    [EnumAltName("SN")]
    Shinmyoumaru,

    /// <summary>
    /// Usami Sumireko.
    /// </summary>
    [EnumAltName("SM")]
    Sumireko,

    /// <summary>
    /// Hakurei Reimu (Final).
    /// </summary>
    [EnumAltName("RB")]
    ReimuB,
}

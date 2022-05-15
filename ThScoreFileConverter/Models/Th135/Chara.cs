//-----------------------------------------------------------------------
// <copyright file="Chara.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Models.Th135;

/// <summary>
/// Represents playable characters of HM.
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
}

//-----------------------------------------------------------------------
// <copyright file="StoryChara.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using ThScoreFileConverter.Core.Models;

namespace ThScoreFileConverter.Models.Th155;

/// <summary>
/// Represents story characters of AoCF.
/// </summary>
public enum StoryChara
{
    /// <summary>
    /// Hakurei Reimu and Ibaraki Kasen.
    /// </summary>
    [EnumAltName("RK")]
    ReimuKasen,

    /// <summary>
    /// Kirisame Marisa and Komeiji Koishi.
    /// </summary>
    [EnumAltName("MK")]
    MarisaKoishi,

    /// <summary>
    /// Kawashiro Nitori and Hata no Kokoro.
    /// </summary>
    [EnumAltName("NK")]
    NitoriKokoro,

    /// <summary>
    /// Futatsuiwa Mamizou and Fujiwara no Mokou.
    /// </summary>
    [EnumAltName("MM")]
    MamizouMokou,

    /// <summary>
    /// Toyosatomimi no Miko and Hijiri Byakuren.
    /// </summary>
    [EnumAltName("MB")]
    MikoByakuren,

    /// <summary>
    /// Mononobe no Futo and Kumoi Ichirin.
    /// </summary>
    [EnumAltName("FI")]
    FutoIchirin,

    /// <summary>
    /// Reisen Udongein Inaba and Doremy Sweet.
    /// </summary>
    [EnumAltName("RD")]
    ReisenDoremy,

    /// <summary>
    /// Usami Sumireko and Doremy Sweet.
    /// </summary>
    [EnumAltName("SD")]
    SumirekoDoremy,

    /// <summary>
    /// Hinanawi Tenshi and Sukuna Shinmyoumaru.
    /// </summary>
    [EnumAltName("TS")]
    TenshiShinmyoumaru,

    /// <summary>
    /// Yakumo Yukari and Hakurei Reimu.
    /// </summary>
    [EnumAltName("YR")]
    YukariReimu,

    /// <summary>
    /// Yorigami Joon and Yorigami Shion.
    /// </summary>
    [EnumAltName("JS")]
    JoonShion,
}

//-----------------------------------------------------------------------
// <copyright file="StoryCharaWithTotal.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Core.Models.Th155;

/// <summary>
/// Represents story characters of AoCF.
/// </summary>
public enum StoryCharaWithTotal
{
    /// <summary>
    /// Hakurei Reimu and Ibaraki Kasen.
    /// </summary>
    [Pattern("RK")]
    ReimuKasen,

    /// <summary>
    /// Kirisame Marisa and Komeiji Koishi.
    /// </summary>
    [Pattern("MK")]
    MarisaKoishi,

    /// <summary>
    /// Kawashiro Nitori and Hata no Kokoro.
    /// </summary>
    [Pattern("NK")]
    NitoriKokoro,

    /// <summary>
    /// Futatsuiwa Mamizou and Fujiwara no Mokou.
    /// </summary>
    [Pattern("MM")]
    MamizouMokou,

    /// <summary>
    /// Toyosatomimi no Miko and Hijiri Byakuren.
    /// </summary>
    [Pattern("MB")]
    MikoByakuren,

    /// <summary>
    /// Mononobe no Futo and Kumoi Ichirin.
    /// </summary>
    [Pattern("FI")]
    FutoIchirin,

    /// <summary>
    /// Reisen Udongein Inaba and Doremy Sweet.
    /// </summary>
    [Pattern("RD")]
    ReisenDoremy,

    /// <summary>
    /// Usami Sumireko and Doremy Sweet.
    /// </summary>
    [Pattern("SD")]
    SumirekoDoremy,

    /// <summary>
    /// Hinanawi Tenshi and Sukuna Shinmyoumaru.
    /// </summary>
    [Pattern("TS")]
    TenshiShinmyoumaru,

    /// <summary>
    /// Yakumo Yukari and Hakurei Reimu.
    /// </summary>
    [Pattern("YR")]
    YukariReimu,

    /// <summary>
    /// Yorigami Joon and Yorigami Shion.
    /// </summary>
    [Pattern("JS")]
    JoonShion,

    /// <summary>
    /// Represents total across characters.
    /// </summary>
    [Pattern("TL")]
    Total,
}

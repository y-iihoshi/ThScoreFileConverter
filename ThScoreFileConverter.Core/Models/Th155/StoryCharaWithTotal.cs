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
    [Character("Reimu")]
    [Character("Kasen", 1)]
    ReimuKasen,

    /// <summary>
    /// Kirisame Marisa and Komeiji Koishi.
    /// </summary>
    [Pattern("MK")]
    [Character("Marisa")]
    [Character("Koishi", 1)]
    MarisaKoishi,

    /// <summary>
    /// Kawashiro Nitori and Hata no Kokoro.
    /// </summary>
    [Pattern("NK")]
    [Character("Nitori")]
    [Character("Kokoro", 1)]
    NitoriKokoro,

    /// <summary>
    /// Futatsuiwa Mamizou and Fujiwara no Mokou.
    /// </summary>
    [Pattern("MM")]
    [Character("Mamizou")]
    [Character("Mokou", 1)]
    MamizouMokou,

    /// <summary>
    /// Toyosatomimi no Miko and Hijiri Byakuren.
    /// </summary>
    [Pattern("MB")]
    [Character("Miko")]
    [Character("Byakuren", 1)]
    MikoByakuren,

    /// <summary>
    /// Mononobe no Futo and Kumoi Ichirin.
    /// </summary>
    [Pattern("FI")]
    [Character("Futo")]
    [Character("Ichirin", 1)]
    FutoIchirin,

    /// <summary>
    /// Reisen Udongein Inaba and Doremy Sweet.
    /// </summary>
    [Pattern("RD")]
    [Character("Reisen")]
    [Character("Doremy", 1)]
    ReisenDoremy,

    /// <summary>
    /// Usami Sumireko and Doremy Sweet.
    /// </summary>
    [Pattern("SD")]
    [Character("Sumireko")]
    [Character("Doremy", 1)]
    SumirekoDoremy,

    /// <summary>
    /// Hinanawi Tenshi and Sukuna Shinmyoumaru.
    /// </summary>
    [Pattern("TS")]
    [Character("Tenshi")]
    [Character("Shinmyoumaru", 1)]
    TenshiShinmyoumaru,

    /// <summary>
    /// Yakumo Yukari and Hakurei Reimu.
    /// </summary>
    [Pattern("YR")]
    [Character("Yukari")]
    [Character("Reimu", 1)]
    YukariReimu,

    /// <summary>
    /// Yorigami Joon and Yorigami Shion.
    /// </summary>
    [Pattern("JS")]
    [Character("Joon")]
    [Character("Shion", 1)]
    JoonShion,

    /// <summary>
    /// Represents total across characters.
    /// </summary>
    [Pattern("TL")]
    Total,
}

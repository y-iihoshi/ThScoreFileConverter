//-----------------------------------------------------------------------
// <copyright file="CharaWithTotal.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Core.Models.Th123;

/// <summary>
/// Represents characters of Hisoutensoku and total.
/// </summary>
#pragma warning disable CA1027 // Mark enums with FlagsAttribute
public enum CharaWithTotal
#pragma warning restore CA1027 // Mark enums with FlagsAttribute
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
    /// Alice Margatroid.
    /// </summary>
    [Pattern("AL")]
    [Character(nameof(Alice))]
    Alice,

    /// <summary>
    /// Patchouli Knowledge.
    /// </summary>
    [Pattern("PC")]
    [Character(nameof(Patchouli))]
    Patchouli,

    /// <summary>
    /// Konpaku Youmu.
    /// </summary>
    [Pattern("YM")]
    [Character(nameof(Youmu))]
    Youmu,

    /// <summary>
    /// Remilia Scarlet.
    /// </summary>
    [Pattern("RL")]
    [Character(nameof(Remilia))]
    Remilia,

    /// <summary>
    /// Saigyouji Yuyuko.
    /// </summary>
    [Pattern("YU")]
    [Character(nameof(Yuyuko))]
    Yuyuko,

    /// <summary>
    /// Yakumo Yukari.
    /// </summary>
    [Pattern("YK")]
    [Character(nameof(Yukari))]
    Yukari,

    /// <summary>
    /// Ibuki Suika.
    /// </summary>
    [Pattern("SU")]
    [Character(nameof(Suika))]
    Suika,

    /// <summary>
    /// Reisen Udongein Inaba.
    /// </summary>
    [Pattern("RS")]
    [Character(nameof(Reisen))]
    Reisen,

    /// <summary>
    /// Shameimaru Aya.
    /// </summary>
    [Pattern("AY")]
    [Character(nameof(Aya))]
    Aya,

    /// <summary>
    /// Onozuka Komachi.
    /// </summary>
    [Pattern("KM")]
    [Character(nameof(Komachi))]
    Komachi,

    /// <summary>
    /// Nagae Iku.
    /// </summary>
    [Pattern("IK")]
    [Character(nameof(Iku))]
    Iku,

    /// <summary>
    /// Hinanawi Tenshi.
    /// </summary>
    [Pattern("TN")]
    [Character(nameof(Tenshi))]
    Tenshi,

    /// <summary>
    /// Kochiya Sanae.
    /// </summary>
    [Pattern("SN")]
    [Character(nameof(Sanae))]
    Sanae,

    /// <summary>
    /// Cirno.
    /// </summary>
    [Pattern("CI")]
    [Character(nameof(Cirno))]
    Cirno,

    /// <summary>
    /// Hong Meiling.
    /// </summary>
    [Pattern("ML")]
    [Character(nameof(Meiling))]
    Meiling,

    /// <summary>
    /// Reiuji Utsuho.
    /// </summary>
    [Pattern("UT")]
    [Character(nameof(Utsuho))]
    Utsuho,

    /// <summary>
    /// Moriya Suwako.
    /// </summary>
    [Pattern("SW")]
    [Character(nameof(Suwako))]
    Suwako,

    /// <summary>
    /// Giant Catfish; Oonamazu.
    /// </summary>
    [Pattern("NM")]
    [Character(nameof(Catfish))]
    Catfish = 0x15,

    /// <summary>
    /// Represents total across characters.
    /// </summary>
    [Pattern("TL")]
    Total,
}

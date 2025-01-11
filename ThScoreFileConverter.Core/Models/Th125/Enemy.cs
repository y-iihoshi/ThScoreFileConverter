//-----------------------------------------------------------------------
// <copyright file="Enemy.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Core.Models.Th125;

/// <summary>
/// Represents enemy characters of DS.
/// </summary>
public enum Enemy
{
    /// <summary>
    /// Aki Shizuha.
    /// </summary>
    [Character(nameof(Shizuha))]
    Shizuha,

    /// <summary>
    /// Aki Minoriko.
    /// </summary>
    [Character(nameof(Minoriko))]
    Minoriko,

    /// <summary>
    /// Mizuhashi Parsee.
    /// </summary>
    [Character(nameof(Parsee))]
    Parsee,

    /// <summary>
    /// Kagiyama Hina.
    /// </summary>
    [Character(nameof(Hina))]
    Hina,

    /// <summary>
    /// Tatara Kogasa.
    /// </summary>
    [Character(nameof(Kogasa))]
    Kogasa,

    /// <summary>
    /// Kisume.
    /// </summary>
    [Character(nameof(Kisume))]
    Kisume,

    /// <summary>
    /// Kurodani Yamame.
    /// </summary>
    [Character(nameof(Yamame))]
    Yamame,

    /// <summary>
    /// Kawashiro Nitori.
    /// </summary>
    [Character(nameof(Nitori))]
    Nitori,

    /// <summary>
    /// Inubashiri Momiji.
    /// </summary>
    [Character(nameof(Momiji))]
    Momiji,

    /// <summary>
    /// Kumoi Ichirin (and Unzan).
    /// </summary>
    [Character(nameof(Ichirin))]
    Ichirin,

    /// <summary>
    /// Murasa Minamitsu.
    /// </summary>
    [Character(nameof(Minamitsu))]
    Minamitsu,

    /// <summary>
    /// Hoshiguma Yuugi.
    /// </summary>
    [Character(nameof(Yuugi))]
    Yuugi,

    /// <summary>
    /// Ibuki Suika.
    /// </summary>
    [Character(nameof(Suika))]
    Suika,

    /// <summary>
    /// Toramaru Shou.
    /// </summary>
    [Character(nameof(Shou))]
    Shou,

    /// <summary>
    /// Nazrin.
    /// </summary>
    [Character(nameof(Nazrin))]
    Nazrin,

    /// <summary>
    /// Reiuji Utsuho.
    /// </summary>
    [Character(nameof(Utsuho))]
    Utsuho,

    /// <summary>
    /// Kaenbyou Rin.
    /// </summary>
    [Character(nameof(Rin))]
    Rin,

    /// <summary>
    /// Komeiji Koishi.
    /// </summary>
    [Character(nameof(Koishi))]
    Koishi,

    /// <summary>
    /// Komeiji Satori.
    /// </summary>
    [Character(nameof(Satori))]
    Satori,

    /// <summary>
    /// Hinanawi Tenshi.
    /// </summary>
    [Character(nameof(Tenshi))]
    Tenshi,

    /// <summary>
    /// Nagae Iku.
    /// </summary>
    [Character(nameof(Iku))]
    Iku,

    /// <summary>
    /// Moriya Suwako.
    /// </summary>
    [Character(nameof(Suwako))]
    Suwako,

    /// <summary>
    /// Yasaka Kanako.
    /// </summary>
    [Character(nameof(Kanako))]
    Kanako,

    /// <summary>
    /// Houjuu Nue.
    /// </summary>
    [Character(nameof(Nue))]
    Nue,

    /// <summary>
    /// Hijiri Byakurin.
    /// </summary>
    [Character(nameof(Byakuren))]
    Byakuren,

    /// <summary>
    /// Hakurei Reimu.
    /// </summary>
    [Character(nameof(Reimu))]
    Reimu,

    /// <summary>
    /// Kirisame Marisa.
    /// </summary>
    [Character(nameof(Marisa))]
    Marisa,

    /// <summary>
    /// Kochiya Sanae.
    /// </summary>
    [Character(nameof(Sanae))]
    Sanae,

    /// <summary>
    /// Himekaidou Hatate.
    /// </summary>
    [Character(nameof(Hatate))]
    Hatate,

    /// <summary>
    /// Shameimaru Aya.
    /// </summary>
    [Character(nameof(Aya))]
    Aya,
}

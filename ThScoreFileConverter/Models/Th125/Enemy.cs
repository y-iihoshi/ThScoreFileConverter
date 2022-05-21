//-----------------------------------------------------------------------
// <copyright file="Enemy.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using ThScoreFileConverter.Core.Models;

namespace ThScoreFileConverter.Models.Th125;

/// <summary>
/// Represents enemy characters of DS.
/// </summary>
public enum Enemy
{
    /// <summary>
    /// Aki Shizuha.
    /// </summary>
    [EnumAltName("静葉", LongName = "秋 静葉")]
    Shizuha,

    /// <summary>
    /// Aki Minoriko.
    /// </summary>
    [EnumAltName("穣子", LongName = "秋 穣子")]
    Minoriko,

    /// <summary>
    /// Mizuhashi Parsee.
    /// </summary>
    [EnumAltName("パルスィ", LongName = "水橋 パルスィ")]
    Parsee,

    /// <summary>
    /// Kagiyama Hina.
    /// </summary>
    [EnumAltName("雛", LongName = "鍵山 雛")]
    Hina,

    /// <summary>
    /// Tatara Kogasa.
    /// </summary>
    [EnumAltName("小傘", LongName = "多々良 小傘")]
    Kogasa,

    /// <summary>
    /// Kisume.
    /// </summary>
    [EnumAltName("キスメ", LongName = "キスメ")]
    Kisume,

    /// <summary>
    /// Kurodani Yamame.
    /// </summary>
    [EnumAltName("ヤマメ", LongName = "黒谷 ヤマメ")]
    Yamame,

    /// <summary>
    /// Kawashiro Nitori.
    /// </summary>
    [EnumAltName("にとり", LongName = "河城 にとり")]
    Nitori,

    /// <summary>
    /// Inubashiri Momiji.
    /// </summary>
    [EnumAltName("椛", LongName = "犬走 椛")]
    Momiji,

    /// <summary>
    /// Kumoi Ichirin and Unzan.
    /// </summary>
    [EnumAltName("一輪 & 雲山", LongName = "雲居 一輪 & 雲山")]
    Ichirin,

    /// <summary>
    /// Murasa Minamitsu.
    /// </summary>
    [EnumAltName("水蜜", LongName = "村紗 水蜜")]
    Minamitsu,

    /// <summary>
    /// Hoshiguma Yuugi.
    /// </summary>
    [EnumAltName("勇儀", LongName = "星熊 勇儀")]
    Yuugi,

    /// <summary>
    /// Ibuki Suika.
    /// </summary>
    [EnumAltName("萃香", LongName = "伊吹 萃香")]
    Suika,

    /// <summary>
    /// Toramaru Shou.
    /// </summary>
    [EnumAltName("星", LongName = "寅丸 星")]
    Shou,

    /// <summary>
    /// Nazrin.
    /// </summary>
    [EnumAltName("ナズーリン", LongName = "ナズーリン")]
    Nazrin,

    /// <summary>
    /// Reiuji Utsuho.
    /// </summary>
    [EnumAltName("お空", LongName = "霊烏路 空")]
    Utsuho,

    /// <summary>
    /// Kaenbyou Rin.
    /// </summary>
    [EnumAltName("お燐", LongName = "火焔猫 燐")]
    Rin,

    /// <summary>
    /// Komeiji Koishi.
    /// </summary>
    [EnumAltName("こいし", LongName = "古明地 こいし")]
    Koishi,

    /// <summary>
    /// Komeiji Satori.
    /// </summary>
    [EnumAltName("さとり", LongName = "古明地 さとり")]
    Satori,

    /// <summary>
    /// Hinanawi Tenshi.
    /// </summary>
    [EnumAltName("天子", LongName = "比那名居 天子")]
    Tenshi,

    /// <summary>
    /// Nagae Iku.
    /// </summary>
    [EnumAltName("衣玖", LongName = "永江 衣玖")]
    Iku,

    /// <summary>
    /// Moriya Suwako.
    /// </summary>
    [EnumAltName("諏訪子", LongName = "洩矢 諏訪子")]
    Suwako,

    /// <summary>
    /// Yasaka Kanako.
    /// </summary>
    [EnumAltName("神奈子", LongName = "八坂 神奈子")]
    Kanako,

    /// <summary>
    /// Houjuu Nue.
    /// </summary>
    [EnumAltName("ぬえ", LongName = "封獣 ぬえ")]
    Nue,

    /// <summary>
    /// Hijiri Byakurin.
    /// </summary>
    [EnumAltName("白蓮", LongName = "聖 白蓮")]
    Byakuren,

    /// <summary>
    /// Hakurei Reimu.
    /// </summary>
    [EnumAltName("霊夢", LongName = "博麗 霊夢")]
    Reimu,

    /// <summary>
    /// Kirisame Marisa.
    /// </summary>
    [EnumAltName("魔理沙", LongName = "霧雨 魔理沙")]
    Marisa,

    /// <summary>
    /// Kochiya Sanae.
    /// </summary>
    [EnumAltName("早苗", LongName = "東風谷 早苗")]
    Sanae,

    /// <summary>
    /// Himekaidou Hatate.
    /// </summary>
    [EnumAltName("はたて", LongName = "姫海棠 はたて")]
    Hatate,

    /// <summary>
    /// Shameimaru Aya.
    /// </summary>
    [EnumAltName("文", LongName = "射命丸 文")]
    Aya,
}

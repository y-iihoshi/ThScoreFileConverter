//-----------------------------------------------------------------------
// <copyright file="Enemy.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using ThScoreFileConverter.Core.Models;

namespace ThScoreFileConverter.Models.Th143;

/// <summary>
/// Represents enemy characters of ISC.
/// </summary>
public enum Enemy
{
    /// <summary>
    /// Wakasagihime.
    /// </summary>
    [EnumAltName("わかさぎ姫", LongName = "わかさぎ姫")]
    Wakasagihime,

    /// <summary>
    /// Cirno.
    /// </summary>
    [EnumAltName("チルノ", LongName = "チルノ")]
    Cirno,

    /// <summary>
    /// Kasodani Kyouko.
    /// </summary>
    [EnumAltName("響子", LongName = "幽谷 響子")]
    Kyouko,

    /// <summary>
    /// Sekibanki.
    /// </summary>
    [EnumAltName("赤蛮奇", LongName = "赤蛮奇")]
    Sekibanki,

    /// <summary>
    /// Imaizumi Kagerou.
    /// </summary>
    [EnumAltName("影狼", LongName = "今泉 影狼")]
    Kagerou,

    /// <summary>
    /// Kamishirasawa Keine.
    /// </summary>
    [EnumAltName("慧音", LongName = "上白沢 慧音")]
    Keine,

    /// <summary>
    /// Fujiwara no Mokou.
    /// </summary>
    [EnumAltName("妹紅", LongName = "藤原 妹紅")]
    Mokou,

    /// <summary>
    /// Miyako Yoshika.
    /// </summary>
    [EnumAltName("芳香", LongName = "宮古 芳香")]
    Yoshika,

    /// <summary>
    /// Kaku Seiga.
    /// </summary>
    [EnumAltName("青娥", LongName = "霍 青娥")]
    Seiga,

    /// <summary>
    /// Saigyouji Yuyuko.
    /// </summary>
    [EnumAltName("幽々子", LongName = "西行寺 幽々子")]
    Yuyuko,

    /// <summary>
    /// Tsukumo Yatsuhashi.
    /// </summary>
    [EnumAltName("八橋", LongName = "九十九 八橋")]
    Yatsuhashi,

    /// <summary>
    /// Tsukumo Benben.
    /// </summary>
    [EnumAltName("弁々", LongName = "九十九 弁々")]
    Benben,

    /// <summary>
    /// Horikawa Raiko.
    /// </summary>
    [EnumAltName("雷鼓", LongName = "堀川 雷鼓")]
    Raiko,

    /// <summary>
    /// Shameimaru Aya.
    /// </summary>
    [EnumAltName("文", LongName = "射命丸 文")]
    Aya,

    /// <summary>
    /// Himekaidou Hatate.
    /// </summary>
    [EnumAltName("はたて", LongName = "姫海棠 はたて")]
    Hatate,

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
    /// Kirisame Marisa.
    /// </summary>
    [EnumAltName("魔理沙", LongName = "霧雨 魔理沙")]
    Marisa,

    /// <summary>
    /// Izayoi Sakuya.
    /// </summary>
    [EnumAltName("咲夜", LongName = "十六夜 咲夜")]
    Sakuya,

    /// <summary>
    /// Konpaku Youmu.
    /// </summary>
    [EnumAltName("妖夢", LongName = "魂魄 妖夢")]
    Youmu,

    /// <summary>
    /// Kochiya Sanae.
    /// </summary>
    [EnumAltName("早苗", LongName = "東風谷 早苗")]
    Sanae,

    /// <summary>
    /// Hakurei Reimu.
    /// </summary>
    [EnumAltName("霊夢", LongName = "博麗 霊夢")]
    Reimu,

    /// <summary>
    /// Futatsuiwa Mamizou.
    /// </summary>
    [EnumAltName("マミゾウ", LongName = "二ッ岩 マミゾウ")]
    Mamizou,

    /// <summary>
    /// Sukuna Shinmyoumaru.
    /// </summary>
    [EnumAltName("針妙丸", LongName = "少名 針妙丸")]
    Shinmyoumaru,

    /// <summary>
    /// Yasaka Kanako.
    /// </summary>
    [EnumAltName("神奈子", LongName = "八坂 神奈子")]
    Kanako,

    /// <summary>
    /// Moriya Suwako.
    /// </summary>
    [EnumAltName("諏訪子", LongName = "洩矢 諏訪子")]
    Suwako,

    /// <summary>
    /// Mononobe no Futo.
    /// </summary>
    [EnumAltName("布都", LongName = "物部 布都")]
    Futo,

    /// <summary>
    /// Ibuki Suika.
    /// </summary>
    [EnumAltName("萃香", LongName = "伊吹 萃香")]
    Suika,

    /// <summary>
    /// Hijiri Byakuren.
    /// </summary>
    [EnumAltName("白蓮", LongName = "聖 白蓮")]
    Byakuren,

    /// <summary>
    /// Toyosatomimi no Miko.
    /// </summary>
    [EnumAltName("神子", LongName = "豊聡耳 神子")]
    Miko,

    /// <summary>
    /// Hinanawi Tenshi.
    /// </summary>
    [EnumAltName("天子", LongName = "比那名居 天子")]
    Tenshi,

    /// <summary>
    /// Remilia Scarlet.
    /// </summary>
    [EnumAltName("レミリア", LongName = "レミリア・スカーレット")]
    Remilia,

    /// <summary>
    /// Yakumo Yukari.
    /// </summary>
    [EnumAltName("紫", LongName = "八雲 紫")]
    Yukari,
}

//-----------------------------------------------------------------------
// <copyright file="Enemy.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using ThScoreFileConverter.Core.Models;

namespace ThScoreFileConverter.Models.Th165;

/// <summary>
/// Represents enemy characters of VD.
/// </summary>
public enum Enemy
{
    /// <summary>
    /// Hakurei Reimu.
    /// </summary>
    [EnumAltName("霊夢", LongName = "博麗 霊夢")]
    Reimu,

    /// <summary>
    /// Seiran.
    /// </summary>
    [EnumAltName("清蘭", LongName = "清蘭")]
    Seiran,

    /// <summary>
    /// Ringo.
    /// </summary>
    [EnumAltName("鈴瑚", LongName = "鈴瑚")]
    Ringo,

    /// <summary>
    /// Eternity Larva.
    /// </summary>
    [EnumAltName("ラルバ", LongName = "エタニティラルバ")]
    Larva,

    /// <summary>
    /// Kirisame Marisa.
    /// </summary>
    [EnumAltName("魔理沙", LongName = "霧雨 魔理沙")]
    Marisa,

    /// <summary>
    /// Yatadera Narumi.
    /// </summary>
    [EnumAltName("成美", LongName = "矢田寺 成美")]
    Narumi,

    /// <summary>
    /// Sakata Nemuno.
    /// </summary>
    [EnumAltName("ネムノ", LongName = "坂田 ネムノ")]
    Nemuno,

    /// <summary>
    /// Komano Aunn.
    /// </summary>
    [EnumAltName("あうん", LongName = "高麗野 あうん")]
    Aunn,

    /// <summary>
    /// Doremy Sweet.
    /// </summary>
    [EnumAltName("ドレミー", LongName = "ドレミー・スイート")]
    Doremy,

    /// <summary>
    /// Clownpiece.
    /// </summary>
    [EnumAltName("クラウンピース", LongName = "クラウンピース")]
    Clownpiece,

    /// <summary>
    /// Kishin Sagume.
    /// </summary>
    [EnumAltName("サグメ", LongName = "稀神 サグメ")]
    Sagume,

    /// <summary>
    /// Teireida Mai.
    /// </summary>
    [EnumAltName("舞", LongName = "丁礼田 舞")]
    Mai,

    /// <summary>
    /// Nishida Satono.
    /// </summary>
    [EnumAltName("里乃", LongName = "爾子田 里乃")]
    Satono,

    /// <summary>
    /// Hecatia Lapislazuli.
    /// </summary>
    [EnumAltName("ヘカーティア", LongName = "ヘカーティア・ラピスラズリ")]
    Hecatia,

    /// <summary>
    /// Junko.
    /// </summary>
    [EnumAltName("純狐", LongName = "純狐")]
    Junko,

    /// <summary>
    /// Matara Okina.
    /// </summary>
    [EnumAltName("隠岐奈", LongName = "摩多羅 隠岐奈")]
    Okina,

    /// <summary>
    /// Remilia Scarlet.
    /// </summary>
    [EnumAltName("レミリア", LongName = "レミリア・スカーレット")]
    Remilia,

    /// <summary>
    /// Flandre Scarlet.
    /// </summary>
    [EnumAltName("フランドール", LongName = "フランドール・スカーレット")]
    Flandre,

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
    /// Saigyouji Yuyuko.
    /// </summary>
    [EnumAltName("幽々子", LongName = "西行寺 幽々子")]
    Yuyuko,

    /// <summary>
    /// Shiki Eiki, Yamaxanadu.
    /// </summary>
    [EnumAltName("映姫", LongName = "四季映姫・ヤマザナドゥ")]
    Eiki,

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
    /// Yagokoro Eirin.
    /// </summary>
    [EnumAltName("永琳", LongName = "八意 永琳")]
    Eirin,

    /// <summary>
    /// Houraisan Kaguya.
    /// </summary>
    [EnumAltName("輝夜", LongName = "蓬莱山 輝夜")]
    Kaguya,

    /// <summary>
    /// Hinanawi Tenshi.
    /// </summary>
    [EnumAltName("天子", LongName = "比那名居 天子")]
    Tenshi,

    /// <summary>
    /// Sukuna Shinmyoumaru.
    /// </summary>
    [EnumAltName("針妙丸", LongName = "少名 針妙丸")]
    Shinmyoumaru,

    /// <summary>
    /// Komeiji Satori.
    /// </summary>
    [EnumAltName("さとり", LongName = "古明地 さとり")]
    Satori,

    /// <summary>
    /// Reiuji Utsuho.
    /// </summary>
    [EnumAltName("空", LongName = "霊烏路 空")]
    Utsuho,

    /// <summary>
    /// Yakumo Ran.
    /// </summary>
    [EnumAltName("藍", LongName = "八雲 藍")]
    Ran,

    /// <summary>
    /// Komeiji Koishi.
    /// </summary>
    [EnumAltName("こいし", LongName = "古明地 こいし")]
    Koishi,

    /// <summary>
    /// Houjuu Nue.
    /// </summary>
    [EnumAltName("ぬえ", LongName = "封獣 ぬえ")]
    Nue,

    /// <summary>
    /// Futatsuiwa Mamizou.
    /// </summary>
    [EnumAltName("マミゾウ", LongName = "二ッ岩 マミゾウ")]
    Mamizou,

    /// <summary>
    /// Nagae Iku.
    /// </summary>
    [EnumAltName("衣玖", LongName = "永江 衣玖")]
    Iku,

    /// <summary>
    /// Horikawa Raiko.
    /// </summary>
    [EnumAltName("雷鼓", LongName = "堀川 雷鼓")]
    Raiko,

    /// <summary>
    /// Ibuki Suika.
    /// </summary>
    [EnumAltName("萃香", LongName = "伊吹 萃香")]
    Suika,

    /// <summary>
    /// Fujiwara no Mokou.
    /// </summary>
    [EnumAltName("妹紅", LongName = "藤原 妹紅")]
    Mokou,

    /// <summary>
    /// Yakumo Yukari.
    /// </summary>
    [EnumAltName("紫", LongName = "八雲 紫")]
    Yukari,

    /// <summary>
    /// Usami Sumireko.
    /// </summary>
    [EnumAltName("菫子", LongName = "宇佐見 菫子")]
    Sumireko,

    /// <summary>
    /// Usami Sumireko (Dream World).
    /// </summary>
    [EnumAltName("菫子(夢)", LongName = "宇佐見 菫子 (夢)")]
    DreamSumireko,
}

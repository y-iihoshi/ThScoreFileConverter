//-----------------------------------------------------------------------
// <copyright file="Enemy.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Models.Th095;

/// <summary>
/// Represents enemy characters of StB.
/// </summary>
public enum Enemy
{
    /// <summary>
    /// Wriggle Nightbug.
    /// </summary>
    [EnumAltName("リグル", LongName = "リグル・ナイトバグ")]
    Wriggle,

    /// <summary>
    /// Rumia.
    /// </summary>
    [EnumAltName("ルーミア", LongName = "ルーミア")]
    Rumia,

    /// <summary>
    /// Cirno.
    /// </summary>
    [EnumAltName("チルノ", LongName = "チルノ")]
    Cirno,

    /// <summary>
    /// Letty Whiterock.
    /// </summary>
    [EnumAltName("レティ", LongName = "レティ・ホワイトロック")]
    Letty,

    /// <summary>
    /// Alice Margatroid.
    /// </summary>
    [EnumAltName("アリス", LongName = "アリス・マーガトロイド")]
    Alice,

    /// <summary>
    /// Kamishirasawa Keine.
    /// </summary>
    [EnumAltName("慧音", LongName = "上白沢 慧音")]
    Keine,

    /// <summary>
    /// Medicine Melancholy.
    /// </summary>
    [EnumAltName("メディスン", LongName = "メディスン・メランコリー")]
    Medicine,

    /// <summary>
    /// Inaba Tewi.
    /// </summary>
    [EnumAltName("てゐ", LongName = "因幡 てゐ")]
    Tewi,

    /// <summary>
    /// Reisen Udongein Inaba.
    /// </summary>
    [EnumAltName("鈴仙", LongName = "鈴仙・優曇華院・イナバ")]
    Reisen,

    /// <summary>
    /// Hon Meirin.
    /// </summary>
    [EnumAltName("美鈴", LongName = "紅 美鈴")]
    Meirin,

    /// <summary>
    /// Patchouli Knowledge.
    /// </summary>
    [EnumAltName("パチュリー", LongName = "パチュリー・ノーレッジ")]
    Patchouli,

    /// <summary>
    /// Chen.
    /// </summary>
    [EnumAltName("橙", LongName = "橙")]
    Chen,

    /// <summary>
    /// Konpaku Youmu.
    /// </summary>
    [EnumAltName("妖夢", LongName = "魂魄 妖夢")]
    Youmu,

    /// <summary>
    /// Izayoi Sakuya.
    /// </summary>
    [EnumAltName("咲夜", LongName = "十六夜 咲夜")]
    Sakuya,

    /// <summary>
    /// Remilia Scarlet.
    /// </summary>
    [EnumAltName("レミリア", LongName = "レミリア・スカーレット")]
    Remilia,

    /// <summary>
    /// Yakumo Ran.
    /// </summary>
    [EnumAltName("藍", LongName = "八雲 藍")]
    Ran,

    /// <summary>
    /// Saigyouji Yuyuko.
    /// </summary>
    [EnumAltName("幽々子", LongName = "西行寺 幽々子")]
    Yuyuko,

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
    /// Onozuka Komachi.
    /// </summary>
    [EnumAltName("小町", LongName = "小野塚 小町")]
    Komachi,

    /// <summary>
    /// Shiki Eiki, Yamaxanadu.
    /// </summary>
    [EnumAltName("映姫", LongName = "四季映姫・ヤマザナドゥ")]
    Shikieiki,

    /// <summary>
    /// Flandre Scarlet.
    /// </summary>
    [EnumAltName("フラン", LongName = "フランドール・スカーレット")]
    Flandre,

    /// <summary>
    /// Yakumo Yukari.
    /// </summary>
    [EnumAltName("紫", LongName = "八雲 紫")]
    Yukari,

    /// <summary>
    /// Fujiwara no Mokou.
    /// </summary>
    [EnumAltName("妹紅", LongName = "藤原 妹紅")]
    Mokou,

    /// <summary>
    /// Ibuki Suika.
    /// </summary>
    [EnumAltName("萃香", LongName = "伊吹 萃香")]
    Suika,
}

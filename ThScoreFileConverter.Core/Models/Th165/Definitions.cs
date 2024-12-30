﻿//-----------------------------------------------------------------------
// <copyright file="Definitions.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System.Collections.Generic;

namespace ThScoreFileConverter.Core.Models.Th165;

/// <summary>
/// Provides several VD specific definitions.
/// </summary>
public static class Definitions
{
    /// <summary>
    /// Gets the dictionary of VD spell cards.
    /// Thanks to thwiki.info.
    /// </summary>
    public static IReadOnlyDictionary<(Day Day, int Scene), (Enemy[] Enemies, string Card)> SpellCards { get; } =
        new Dictionary<(Day, int), (Enemy[], string)>
        {
            { (Day.Sunday,             1), ([Enemy.Reimu],      string.Empty) },
            { (Day.Sunday,             2), ([Enemy.Reimu],      string.Empty) },
            { (Day.Monday,             1), ([Enemy.Seiran],     "弾符「イーグルシューティング」") },
            { (Day.Monday,             2), ([Enemy.Ringo],      "兎符「ストロベリー大ダンゴ」") },
            { (Day.Monday,             3), ([Enemy.Seiran],     "弾符「ラビットファルコナー」") },
            { (Day.Monday,             4), ([Enemy.Ringo],      "兎符「ダンゴ三姉妹」") },
            { (Day.Tuesday,            1), ([Enemy.Larva],      string.Empty) },
            { (Day.Tuesday,            2), ([Enemy.Larva],      "蝶符「バタフライドリーム」") },
            { (Day.Tuesday,            3), ([Enemy.Larva],      "蝶符「纏わり付く鱗粉」") },
            { (Day.Wednesday,          1), ([Enemy.Marisa],     string.Empty) },
            { (Day.Wednesday,          2), ([Enemy.Narumi],     "魔符「慈愛の地蔵」") },
            { (Day.Wednesday,          3), ([Enemy.Narumi],     "地蔵「菩薩ストンプ」") },
            { (Day.Wednesday,          4), ([Enemy.Narumi],     "地蔵「活きの良いバレットゴーレム」") },
            { (Day.Thursday,           1), ([Enemy.Nemuno],     string.Empty) },
            { (Day.Thursday,           2), ([Enemy.Nemuno],     "研符「狂い輝く鬼包丁」") },
            { (Day.Thursday,           3), ([Enemy.Nemuno],     "殺符「窮僻の山姥」") },
            { (Day.Friday,             1), ([Enemy.Aunn],       string.Empty) },
            { (Day.Friday,             2), ([Enemy.Aunn],       "独楽「コマ犬大回転」") },
            { (Day.Friday,             3), ([Enemy.Aunn],       "独楽「阿吽の閃光」") },
            { (Day.Saturday,           1), ([Enemy.Doremy],     string.Empty) },
            { (Day.WrongSunday,        1), ([Enemy.Reimu],      string.Empty) },
            { (Day.WrongSunday,        2), ([Enemy.Seiran],     "夢弾「ルナティックドリームショット」") },
            { (Day.WrongSunday,        3), ([Enemy.Ringo],      "団子「ダンゴフラワー」") },
            { (Day.WrongSunday,        4), ([Enemy.Larva],      "夢蝶「クレージーバタフライ」") },
            { (Day.WrongSunday,        5), ([Enemy.Narumi],     "夢地蔵「劫火の希望」") },
            { (Day.WrongSunday,        6), ([Enemy.Nemuno],     "夢尽「殺人鬼の懐」") },
            { (Day.WrongSunday,        7), ([Enemy.Aunn],       "夢犬「１０１匹の野良犬」") },
            { (Day.WrongMonday,        1), ([Enemy.Clownpiece], string.Empty) },
            { (Day.WrongMonday,        2), ([Enemy.Clownpiece], "獄符「バースティンググラッジ」") },
            { (Day.WrongMonday,        3), ([Enemy.Clownpiece], "獄符「ダブルストライプ」") },
            { (Day.WrongMonday,        4), ([Enemy.Clownpiece], "月夢「エクリプスナイトメア」") },
            { (Day.WrongTuesday,       1), ([Enemy.Sagume],     string.Empty) },
            { (Day.WrongTuesday,       2), ([Enemy.Sagume],     "玉符「金城鉄壁の陰陽玉」") },
            { (Day.WrongTuesday,       3), ([Enemy.Sagume],     "玉符「神々の写し難い弾冠」") },
            { (Day.WrongTuesday,       4), ([Enemy.Sagume],     "夢鷺「片翼の夢鷺」") },
            { (Day.WrongWednesday,     1), ([Enemy.Doremy],     string.Empty) },
            { (Day.WrongWednesday,     2), ([Enemy.Mai],        "竹符「バンブーラビリンス」") },
            { (Day.WrongWednesday,     3), ([Enemy.Satono],     "茗荷「メスメリズムダンス」") },
            { (Day.WrongWednesday,     4), ([Enemy.Mai],        "笹符「タナバタスタードリーム」") },
            { (Day.WrongWednesday,     5), ([Enemy.Satono],     "冥加「ビハインドナイトメア」") },
            { (Day.WrongWednesday,     6), ([Enemy.Mai, Enemy.Satono], string.Empty) },
            { (Day.WrongThursday,      1), ([Enemy.Hecatia],    "異界「ディストーテッドファイア」") },
            { (Day.WrongThursday,      2), ([Enemy.Hecatia],    "異界「恨みがましい地獄の雨」") },
            { (Day.WrongThursday,      3), ([Enemy.Hecatia],    "月「コズミックレディエーション」") },
            { (Day.WrongThursday,      4), ([Enemy.Hecatia],    "異界「逢魔ガ刻　夢」") },
            { (Day.WrongThursday,      5), ([Enemy.Hecatia],    "「月が堕ちてくる！」") },
            { (Day.WrongFriday,        1), ([Enemy.Junko],      string.Empty) },
            { (Day.WrongFriday,        2), ([Enemy.Junko],      "「震え凍える悪夢」") },
            { (Day.WrongFriday,        3), ([Enemy.Junko],      "「サイケデリックマンダラ」") },
            { (Day.WrongFriday,        4), ([Enemy.Junko],      "「極めて威厳のある純光」") },
            { (Day.WrongFriday,        5), ([Enemy.Junko],      "「確実に悪夢で殺す為の弾幕」") },
            { (Day.WrongSaturday,      1), ([Enemy.Okina],      "秘儀「マターラスッカ」") },
            { (Day.WrongSaturday,      2), ([Enemy.Okina],      "秘儀「背面の邪炎」") },
            { (Day.WrongSaturday,      3), ([Enemy.Okina],      "後符「絶対秘神の後光」") },
            { (Day.WrongSaturday,      4), ([Enemy.Okina],      "秘儀「秘神の暗曜弾幕」") },
            { (Day.WrongSaturday,      5), ([Enemy.Okina],      "秘儀「神秘の玉繭」") },
            { (Day.WrongSaturday,      6), ([Enemy.Okina],      string.Empty) },
            { (Day.NightmareSunday,    1), ([Enemy.Remilia,  Enemy.Flandre],      "紅魔符「ブラッディカタストロフ」") },
            { (Day.NightmareSunday,    2), ([Enemy.Byakuren, Enemy.Miko],         "星神符「十七条の超人」") },
            { (Day.NightmareSunday,    3), ([Enemy.Remilia,  Enemy.Byakuren],     "紅星符「超人ブラッディナイフ」") },
            { (Day.NightmareSunday,    4), ([Enemy.Flandre,  Enemy.Miko],         "紅神符「十七条のカタストロフ」") },
            { (Day.NightmareSunday,    5), ([Enemy.Remilia,  Enemy.Miko],         "神紅符「ブラッディ十七条のレーザー」") },
            { (Day.NightmareSunday,    6), ([Enemy.Flandre,  Enemy.Byakuren],     "紅星符「超人カタストロフ行脚」") },
            { (Day.NightmareMonday,    1), ([Enemy.Yuyuko,   Enemy.Eiki],         "妖花符「バタフライストーム閻魔笏」") },
            { (Day.NightmareMonday,    2), ([Enemy.Kanako,   Enemy.Suwako],       "風神符「ミシャバシラ」") },
            { (Day.NightmareMonday,    3), ([Enemy.Yuyuko,   Enemy.Kanako],       "風妖符「死蝶オンバシラ」") },
            { (Day.NightmareMonday,    4), ([Enemy.Eiki,     Enemy.Suwako],       "風花符「ミシャグジ様の是非」") },
            { (Day.NightmareMonday,    5), ([Enemy.Yuyuko,   Enemy.Suwako],       "妖風符「土着蝶ストーム」") },
            { (Day.NightmareMonday,    6), ([Enemy.Eiki,     Enemy.Kanako],       "風花符「オンバシラ裁判」") },
            { (Day.NightmareTuesday,   1), ([Enemy.Eirin,    Enemy.Kaguya],       "永夜符「蓬莱壺中の弾の枝」") },
            { (Day.NightmareTuesday,   2), ([Enemy.Tenshi,   Enemy.Shinmyoumaru], "緋針符「要石も大きくなあれ」") },
            { (Day.NightmareTuesday,   3), ([Enemy.Eirin,    Enemy.Tenshi],       "永緋符「墜落する壺中の有頂天」") },
            { (Day.NightmareTuesday,   4), ([Enemy.Kaguya,   Enemy.Shinmyoumaru], "輝夜符「蓬莱の大きな弾の枝」") },
            { (Day.NightmareTuesday,   5), ([Enemy.Eirin,    Enemy.Shinmyoumaru], "永輝符「大きくなる壺」") },
            { (Day.NightmareTuesday,   6), ([Enemy.Kaguya,   Enemy.Tenshi],       "緋夜符「蓬莱の弾の要石」") },
            { (Day.NightmareWednesday, 1), ([Enemy.Satori,   Enemy.Utsuho],       "地霊符「マインドステラスチール」") },
            { (Day.NightmareWednesday, 2), ([Enemy.Ran,      Enemy.Koishi],       "地妖符「イドの式神」") },
            { (Day.NightmareWednesday, 3), ([Enemy.Satori,   Enemy.Koishi],       "「パーフェクトマインドコントロール」") },
            { (Day.NightmareWednesday, 4), ([Enemy.Ran,      Enemy.Utsuho],       "地妖符「式神大星」") },
            { (Day.NightmareWednesday, 5), ([Enemy.Ran,      Enemy.Satori],       "地妖符「エゴの式神」") },
            { (Day.NightmareWednesday, 6), ([Enemy.Utsuho,   Enemy.Koishi],       "地霊符「マインドステラリリーフ」") },
            { (Day.NightmareThursday,  1), ([Enemy.Nue,      Enemy.Mamizou],      "神星符「正体不明の怪光人だかり」") },
            { (Day.NightmareThursday,  2), ([Enemy.Iku,      Enemy.Raiko],        "輝天符「迅雷のドンドコ太鼓」") },
            { (Day.NightmareThursday,  3), ([Enemy.Mamizou,  Enemy.Raiko],        "輝神符「謎のドンドコ人だかり」") },
            { (Day.NightmareThursday,  4), ([Enemy.Iku,      Enemy.Nue],          "緋星符「正体不明の落雷」") },
            { (Day.NightmareThursday,  5), ([Enemy.Iku,      Enemy.Mamizou],      "神緋符「雷雨の中のストーカー」") },
            { (Day.NightmareThursday,  6), ([Enemy.Nue,      Enemy.Raiko],        "輝星符「正体不明のドンドコ太鼓」") },
            { (Day.NightmareFriday,    1), ([Enemy.Suika,    Enemy.Mokou],        "萃夜符「身命霧散」") },
            { (Day.NightmareFriday,    2), ([Enemy.Junko,    Enemy.Hecatia],      "紺珠符「純粋と不純の弾幕」") },
            { (Day.NightmareFriday,    3), ([Enemy.Suika,    Enemy.Junko],        "萃珠符「純粋な五里霧中」") },
            { (Day.NightmareFriday,    4), ([Enemy.Mokou,    Enemy.Hecatia],      "永珠符「捨て身のリフレクション」") },
            { (Day.NightmareFriday,    5), ([Enemy.Suika,    Enemy.Hecatia],      "萃珠符「ミストレイ」") },
            { (Day.NightmareFriday,    6), ([Enemy.Mokou,    Enemy.Junko],        "永珠符「穢れ無き珠と穢れ多き霊」") },
            { (Day.NightmareSaturday,  1), ([Enemy.Yukari,   Enemy.Okina],        "「秘神結界」") },
            { (Day.NightmareSaturday,  2), ([Enemy.Reimu,    Enemy.Marisa],       "「盗撮者調伏マスタースパーク」") },
            { (Day.NightmareSaturday,  3), ([Enemy.Reimu,    Enemy.Okina],        "「背後からの盗撮者調伏」") },
            { (Day.NightmareSaturday,  4), ([Enemy.Marisa,   Enemy.Yukari],       "「弾幕結界を撃ち抜け！」") },
            { (Day.NightmareSaturday,  5), ([Enemy.Marisa,   Enemy.Okina],        "「卑怯者マスタースパーク」") },
            { (Day.NightmareSaturday,  6), ([Enemy.Reimu,    Enemy.Yukari],       "「許可無く弾幕は撮影禁止です」") },
            { (Day.NightmareDiary,     1), ([Enemy.Doremy],                       "「最後の日曜日に見る悪夢」") },
            { (Day.NightmareDiary,     2), ([Enemy.Sumireko],                     "紙符「ＥＳＰカード手裏剣」") },
            { (Day.NightmareDiary,     3), ([Enemy.Sumireko, Enemy.Yukari],       "紙符「結界中のＥＳＰカード手裏剣」") },
            { (Day.NightmareDiary,     4), ([Enemy.DreamSumireko],                string.Empty) },
        };

    /// <summary>
    /// Gets the list of nicknames.
    /// </summary>
    public static IReadOnlyList<string> Nicknames { get; } =
    [
        "秘封倶楽部　伝説の会長",
        "現実を取り戻した会長",
        "弱小同好会",
        "注目の新規同好会",
        "噂のオカルト同好会",
        "一目置かれるオカルト部",
        "格上のオカルト部",
        "名の知れたオカルト部",
        "至極崇高なオカルト部",
        "信仰を集めるオカルト部",
        "神秘的なオカルト部",
        "ムーが食い付くオカルト部",
        "初めての弾幕写真",
        "終わらない悪夢",
        "真の悪夢の始まり",
        "夢の世界の終わり",
        "覚醒超能力者　菫子",
        "夢の支配者",
        "悪夢の支配者",
        "正夢の支配者",
        "ＳＮＳ始めました",
        "映え写真ガール",
        "枚数だけカメラマン",
        "瞬撮投稿ガール",
        "秘封イ○スタグラマー",
        "駆け出しバレスタグラマー",
        "人気バレスタグラマー",
        "超絶バレスタグラマー",
        "カリスマバレスタグラマー",
        "秘封グラマー",
        "会心の一枚",
        "奇跡の一枚",
        "究極の一枚",
        "神懸かった写真",
        "秘封を曝く写真",
        "うたた寝女子高生",
        "レム睡眠女子高生",
        "ショートスリーパー",
        "睡眠不足女子高生",
        "夢遊病女子高生",
        "ボロボロ会長",
        "ゾンビ会長",
        "被弾大好き会長",
        "ヒュンヒュン人間",
        "秘封テレポーター",
        "なんちゃって不死身ちゃん",
        "夢オチ不死身ちゃん",
        "スーパードリーマー",
        "パーフェクトドリーマー",
        "バイオレットドリーマー",
    ];
}

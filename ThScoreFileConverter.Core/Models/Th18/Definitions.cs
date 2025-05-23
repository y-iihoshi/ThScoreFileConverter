﻿//-----------------------------------------------------------------------
// <copyright file="Definitions.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using CardInfo = ThScoreFileConverter.Core.Models.SpellCardInfo<
    ThScoreFileConverter.Core.Models.Stage, ThScoreFileConverter.Core.Models.Level>;

namespace ThScoreFileConverter.Core.Models.Th18;

/// <summary>
/// Provides several UM specific definitions.
/// </summary>
public static class Definitions
{
    /// <summary>
    /// Gets the dictionary of UM spell cards.
    /// Thanks to wikiwiki.jp/thk and en.touhouwiki.net.
    /// </summary>
    public static IReadOnlyDictionary<int, CardInfo> CardTable { get; } = new CardInfo[]
    {
#pragma warning disable SA1008 // Opening parenthesis should be spaced correctly
        new( 1, "招符「弾幕万来」",                       Stage.One,   Level.Easy),
        new( 2, "招符「弾幕万来」",                       Stage.One,   Level.Normal),
        new( 3, "招符「弾幕万来」",                       Stage.One,   Level.Hard),
        new( 4, "招符「弾幕万来」",                       Stage.One,   Level.Lunatic),
        new( 5, "招符「弾災招福」",                       Stage.One,   Level.Easy),
        new( 6, "招符「弾災招福」",                       Stage.One,   Level.Normal),
        new( 7, "招符「弾災招福」",                       Stage.One,   Level.Hard),
        new( 8, "招符「弾災招福」",                       Stage.One,   Level.Lunatic),
        new( 9, "森符「木隠れの技術」",                   Stage.Two,   Level.Easy),
        new(10, "森符「木隠れの技術」",                   Stage.Two,   Level.Normal),
        new(11, "森符「極・木隠れの技術」",               Stage.Two,   Level.Hard),
        new(12, "森符「真・木隠れの技術」",               Stage.Two,   Level.Lunatic),
        new(13, "森符「最奥の森域」",                     Stage.Two,   Level.Easy),
        new(14, "森符「最奥の森域」",                     Stage.Two,   Level.Normal),
        new(15, "森符「極・最奥の森域」",                 Stage.Two,   Level.Hard),
        new(16, "森符「真・最奥の森域」",                 Stage.Two,   Level.Lunatic),
        new(17, "葉技「グリーンスパイラル」",             Stage.Two,   Level.Easy),
        new(18, "葉技「グリーンスパイラル」",             Stage.Two,   Level.Normal),
        new(19, "葉技「グリーンサイクロン」",             Stage.Two,   Level.Hard),
        new(20, "葉技「グリーントルネード」",             Stage.Two,   Level.Lunatic),
        new(21, "山符「動天の雲間草」",                   Stage.Three, Level.Easy),
        new(22, "山符「動天の雲間草」",                   Stage.Three, Level.Normal),
        new(23, "山怪「驚愕の雲間草」",                   Stage.Three, Level.Hard),
        new(24, "山怪「驚愕の雲間草」",                   Stage.Three, Level.Lunatic),
        new(25, "山符「妖光輝く薄雪草」",                 Stage.Three, Level.Easy),
        new(26, "山符「妖光輝く薄雪草」",                 Stage.Three, Level.Normal),
        new(27, "山怪「妖魔犇めく薄雪草」",               Stage.Three, Level.Hard),
        new(28, "山怪「妖魔犇めく薄雪草」",               Stage.Three, Level.Lunatic),
        new(29, "山花「殺戮の駒草」",                     Stage.Three, Level.Easy),
        new(30, "山花「殺戮の駒草」",                     Stage.Three, Level.Normal),
        new(31, "山花「殺戮の山の女王」",                 Stage.Three, Level.Hard),
        new(32, "山花「殺戮の山の女王」",                 Stage.Three, Level.Lunatic),
        new(33, "玉符「虹龍陰陽玉」",                     Stage.Four,  Level.Easy),
        new(34, "玉符「虹龍陰陽玉」",                     Stage.Four,  Level.Normal),
        new(35, "玉符「虹龍陰陽玉」",                     Stage.Four,  Level.Hard),
        new(36, "玉符「陰陽神玉」",                       Stage.Four,  Level.Lunatic),
        new(37, "玉将「クイーンオブインヤンスフィア」",   Stage.Four,  Level.Easy),
        new(38, "玉将「クイーンオブインヤンスフィア」",   Stage.Four,  Level.Normal),
        new(39, "女王珠「虹の扉の向こうに」",             Stage.Four,  Level.Hard),
        new(40, "女王珠「虹の扉の向こうに」",             Stage.Four,  Level.Lunatic),
        new(41, "「陰陽サフォケイション」",               Stage.Four,  Level.Easy),
        new(42, "「陰陽サフォケイション」",               Stage.Four,  Level.Normal),
        new(43, "「陰陽サフォケイション」",               Stage.Four,  Level.Hard),
        new(44, "「陰陽サフォケイション」",               Stage.Four,  Level.Lunatic),
        new(45, "禍星「星火燎原の舞」",                   Stage.Five,  Level.Easy),
        new(46, "禍星「星火燎原の舞」",                   Stage.Five,  Level.Normal),
        new(47, "禍星「星火燎原乱舞」",                   Stage.Five,  Level.Hard),
        new(48, "禍星「星火燎原乱舞」",                   Stage.Five,  Level.Lunatic),
        new(49, "星風「虹彩陸離の舞」",                   Stage.Five,  Level.Easy),
        new(50, "星風「虹彩陸離の舞」",                   Stage.Five,  Level.Normal),
        new(51, "星風「虹彩陸離乱舞」",                   Stage.Five,  Level.Hard),
        new(52, "星風「虹彩陸離乱舞」",                   Stage.Five,  Level.Lunatic),
        new(53, "光馬「天馬行空の舞」",                   Stage.Five,  Level.Easy),
        new(54, "光馬「天馬行空の舞」",                   Stage.Five,  Level.Normal),
        new(55, "光馬「天馬行空乱舞」",                   Stage.Five,  Level.Hard),
        new(56, "光馬「天馬行空乱舞」",                   Stage.Five,  Level.Lunatic),
        new(57, "虹光「光風霽月」",                       Stage.Five,  Level.Easy),
        new(58, "虹光「光風霽月」",                       Stage.Five,  Level.Normal),
        new(59, "虹光「光風霽月」",                       Stage.Five,  Level.Hard),
        new(60, "虹光「光風霽月」",                       Stage.Five,  Level.Lunatic),
        new(61, "「無主への供物」",                       Stage.Six,   Level.Easy),
        new(62, "「無主への供物」",                       Stage.Six,   Level.Normal),
        new(63, "「無主への供物」",                       Stage.Six,   Level.Hard),
        new(64, "「無主への供物」",                       Stage.Six,   Level.Lunatic),
        new(65, "「弾幕狂蒐家の妄執」",                   Stage.Six,   Level.Easy),
        new(66, "「弾幕狂蒐家の妄執」",                   Stage.Six,   Level.Normal),
        new(67, "「弾幕狂蒐家の妄執」",                   Stage.Six,   Level.Hard),
        new(68, "「弾幕狂蒐家の妄執」",                   Stage.Six,   Level.Lunatic),
        new(69, "「バレットマーケット」",                 Stage.Six,   Level.Easy),
        new(70, "「バレットマーケット」",                 Stage.Six,   Level.Normal),
        new(71, "「密度の高いバレットマーケット」",       Stage.Six,   Level.Hard),
        new(72, "「弾幕自由市場」",                       Stage.Six,   Level.Lunatic),
        new(73, "「虹人環」",                             Stage.Six,   Level.Easy),
        new(74, "「虹人環」",                             Stage.Six,   Level.Normal),
        new(75, "「虹人環」",                             Stage.Six,   Level.Hard),
        new(76, "「虹人環」",                             Stage.Six,   Level.Lunatic),
        new(77, "「バレットドミニオン」",                 Stage.Six,   Level.Easy),
        new(78, "「バレットドミニオン」",                 Stage.Six,   Level.Normal),
        new(79, "「暴虐のバレットドミニオン」",           Stage.Six,   Level.Hard),
        new(80, "「無動のバレットドミニオン」",           Stage.Six,   Level.Lunatic),
        new(81, "「弾幕のアジール」",                     Stage.Six,   Level.Easy),
        new(82, "「弾幕のアジール」",                     Stage.Six,   Level.Normal),
        new(83, "「弾幕のアジール」",                     Stage.Six,   Level.Hard),
        new(84, "「弾幕のアジール」",                     Stage.Six,   Level.Lunatic),
        new(85, "狐符「フォックスワインダー」",           Stage.Extra, Level.Extra),
        new(86, "管狐「シリンダーフォックス」",           Stage.Extra, Level.Extra),
        new(87, "星狐「天狐龍星の舞",                     Stage.Extra, Level.Extra),
        new(88, "蠱毒「カニバリスティックインセクト」",   Stage.Extra, Level.Extra),
        new(89, "蠱毒「ケイブスウォーマー」",             Stage.Extra, Level.Extra),
        new(90, "蠱毒「スカイペンドラ」",                 Stage.Extra, Level.Extra),
        new(91, "採掘「積もり続けるマインダンプ」",       Stage.Extra, Level.Extra),
        new(92, "採掘「マインブラスト」",                 Stage.Extra, Level.Extra),
        new(93, "採掘「妖怪達のシールドメソッド」",       Stage.Extra, Level.Extra),
        new(94, "大蜈蚣「スネークイーター」",             Stage.Extra, Level.Extra),
        new(95, "大蜈蚣「ドラゴンイーター」",             Stage.Extra, Level.Extra),
        new(96, "「蠱毒のグルメ」",                       Stage.Extra, Level.Extra),
        new(97, "「蟲姫さまの輝かしく落ち着かない毎日」", Stage.Extra, Level.Extra),
#pragma warning restore SA1008 // Opening parenthesis should be spaced correctly
    }.ToDictionary(static card => card.Id);

    /// <summary>
    /// Gets the dictionary of UM ability cards.
    /// NOTE: The key means the display order in game.
    /// </summary>
    public static IReadOnlyDictionary<int, AbilityCard> AbilityCardTable { get; } = new Dictionary<int, AbilityCard>
    {
#pragma warning disable SA1008 // Opening parenthesis should be spaced correctly
        {  0, new( 0, "空白のカード",           AbilityCardType.Passive) },
        {  1, new( 1, "命のカード",             AbilityCardType.Item) },
        {  2, new( 2, "スペルのカード",         AbilityCardType.Item) },
        {  3, new( 3, "欠けた命のカード",       AbilityCardType.Item) },
        {  4, new( 4, "欠けたスペルのカード",   AbilityCardType.Item) },
        {  5, new( 5, "勝手に天下の回り物",     AbilityCardType.Item) },
        {  6, new( 6, "鈴瑚印の団子",           AbilityCardType.Item) },
        {  7, new( 7, "不死鳥の尾",             AbilityCardType.Item) },
        {  8, new( 8, "陰陽玉",                 AbilityCardType.Equipment) },
        {  9, new( 9, "陰陽玉（針）",           AbilityCardType.Equipment) },
        { 10, new(10, "ミニ八卦炉",             AbilityCardType.Equipment) },
        { 11, new(11, "ミニ八卦炉（ミサイル）", AbilityCardType.Equipment) },
        { 12, new(12, "メイドナイフ",           AbilityCardType.Equipment) },
        { 13, new(13, "メイドナイフ（跳弾）",   AbilityCardType.Equipment) },
        { 14, new(14, "無事かえるお守り",       AbilityCardType.Equipment) },
        { 15, new(15, "蛇の抜け殻入りお守り",   AbilityCardType.Equipment) },
        { 16, new(16, "半霊の半分",             AbilityCardType.Equipment) },
        { 17, new(17, "上海人形",               AbilityCardType.Equipment) },
        { 18, new(18, "アイスフェアリー",       AbilityCardType.Equipment) },
        { 19, new(19, "背中の扉",               AbilityCardType.Equipment) },
        { 20, new(20, "鬱陶しいＵＦＯ",         AbilityCardType.Equipment) },
        { 21, new(51, "太古の勾玉",             AbilityCardType.Equipment) },
        { 22, new(21, "守銭奴の教訓",           AbilityCardType.Passive) },
        { 23, new(22, "神山への供物",           AbilityCardType.Passive) },
        { 24, new(23, "死穢回避の薬",           AbilityCardType.Passive) },
        { 25, new(24, "幸運うさぎの足",         AbilityCardType.Passive) },
        { 26, new(25, "弱肉強食の理",           AbilityCardType.Passive) },
        { 27, new(26, "法力経典",               AbilityCardType.Passive) },
        { 28, new(27, "小石ころ帽子",           AbilityCardType.Passive) },
        { 29, new(28, "はじける赤蛙",           AbilityCardType.Passive) },
        { 30, new(29, "疾風の下駄",             AbilityCardType.Passive) },
        { 31, new(30, "偶像防衛隊",             AbilityCardType.Passive) },
        { 32, new(31, "かぐや姫の隠し箱",       AbilityCardType.Passive) },
        { 33, new(32, "頼りになる弟子狸",       AbilityCardType.Passive) },
        { 34, new(33, "弾幕の亡霊",             AbilityCardType.Passive) },
        { 35, new(34, "鬼傑組長の脅嚇",         AbilityCardType.Passive) },
        { 36, new(35, "地獄の沙汰も金次第",     AbilityCardType.Passive) },
        { 37, new(36, "肉体強化地蔵",           AbilityCardType.Passive) },
        { 38, new(37, "転ばぬ先のスペル",       AbilityCardType.Passive) },
        { 39, new(38, "商売上手な招き猫",       AbilityCardType.Passive) },
        { 40, new(39, "山童的買い物術",         AbilityCardType.Passive) },
        { 41, new(40, "ドラゴンキセル",         AbilityCardType.Passive) },
        { 42, new(54, "暴食のムカデ",           AbilityCardType.Passive) },
        { 43, new(41, "画面の境界",             AbilityCardType.Active) },
        { 44, new(42, "打ち出の小槌",           AbilityCardType.Active) },
        { 45, new(43, "忍耐の要石",             AbilityCardType.Active) },
        { 46, new(44, "狂気の月",               AbilityCardType.Active) },
        { 47, new(45, "やんごとなき威光",       AbilityCardType.Active) },
        { 48, new(46, "ヴァンパイアファング",   AbilityCardType.Active) },
        { 49, new(47, "地底の太陽",             AbilityCardType.Active) },
        { 50, new(48, "アイテムの季節",         AbilityCardType.Active) },
        { 51, new(49, "重低音バスドラム",       AbilityCardType.Active) },
        { 52, new(50, "サイコキネシス",         AbilityCardType.Active) },
        { 53, new(52, "霊力の標本瓶",           AbilityCardType.Active) },
        { 54, new(53, "大天狗の麦飯",           AbilityCardType.Active) },
        { 55, new(55, "空色の勾玉",             AbilityCardType.Passive) },
#if false
        { 56, new(56, "(なし)",                 AbilityCardType.Unknown) },
#endif
#pragma warning restore SA1008 // Opening parenthesis should be spaced correctly
    };

    /// <summary>
    /// Gets the list of achievements.
    /// Thanks to en.touhouwiki.net.
    /// </summary>
    public static IReadOnlyList<string> Achievements { get; } =
    [
        "霊夢アナザークリア",
        "霊夢でクリア",
        "魔理沙アナザークリア",
        "魔理沙でクリア",
        "咲夜アナザークリア",
        "咲夜でクリア",
        "早苗アナザークリア",
        "早苗でクリア",
        "霊夢バッドエンド",
        "魔理沙BADクリア",
        "咲夜BADクリア",
        "早苗BADクリア",
        "霊夢EXクリア",
        "魔理沙EXクリア",
        "咲夜EXクリア",
        "早苗EXクリア",
        "イージークリア",
        "ノーマルクリア",
        "ハードクリア",
        "ルナティッククリア",
        "イージーノーコンクリア",
        "ノーマルノーコンクリア",
        "ハードNCクリア",
        "ルナティックNCクリア",
        "売り物のない市場",
        "パーフェクトクリア",
        "無主の存在",
        "蠱毒王",
        "スペルカードコレクター",
        "カード売人",
    ];
}

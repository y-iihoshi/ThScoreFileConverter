//-----------------------------------------------------------------------
// <copyright file="Definitions.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using ThScoreFileConverter.Core.Models.Th18;
using CardInfo = ThScoreFileConverter.Core.Models.SpellCardInfo<
    ThScoreFileConverter.Core.Models.Stage, ThScoreFileConverter.Core.Models.Level>;

namespace ThScoreFileConverter.Models.Th18;

internal static class Definitions
{
    public static IReadOnlyDictionary<int, CardInfo> CardTable { get; } = Core.Models.Th18.Definitions.CardTable;

    // NOTE: The key means the display order in game.
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

    public static IReadOnlyList<string> Achievements { get; } = Core.Models.Th18.Definitions.Achievements;

    public static string FormatPrefix { get; } = "%T18";

    public static bool IsTotal(CharaWithTotal chara)
    {
        return chara is CharaWithTotal.Total;
    }
}

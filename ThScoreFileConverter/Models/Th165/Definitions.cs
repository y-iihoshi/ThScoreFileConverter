//-----------------------------------------------------------------------
// <copyright file="Definitions.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using ThScoreFileConverter.Core.Models.Th165;

namespace ThScoreFileConverter.Models.Th165;

internal static class Definitions
{
    public static IReadOnlyDictionary<(Day Day, int Scene), (Enemy[] Enemies, string Card)> SpellCards { get; } =
        Core.Models.Th165.Definitions.SpellCards;

    public static IReadOnlyList<string> Nicknames { get; } = new List<string>
    {
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
    };

    public static string FormatPrefix { get; } = "%T165";
}

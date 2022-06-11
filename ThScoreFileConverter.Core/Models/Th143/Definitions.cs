//-----------------------------------------------------------------------
// <copyright file="Definitions.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System.Collections.Generic;

namespace ThScoreFileConverter.Core.Models.Th143;

/// <summary>
/// Provides several ISC specific definitions.
/// </summary>
public static class Definitions
{
    /// <summary>
    /// Gets the dictionary of ISC spell cards.
    /// Thanks to thwiki.info.
    /// </summary>
    public static IReadOnlyDictionary<(Day Day, int Scene), (Enemy[] Enemies, string Card)> SpellCards { get; } =
        new Dictionary<(Day, int), (Enemy[], string)>
        {
            { (Day.First,   1), (new[] { Enemy.Yatsuhashi },   string.Empty) },
            { (Day.First,   2), (new[] { Enemy.Wakasagihime }, "水符「ルナティックレッドスラップ」") },
            { (Day.First,   3), (new[] { Enemy.Cirno },        "氷符「パーフェクトグレーシェリスト」") },
            { (Day.First,   4), (new[] { Enemy.Wakasagihime }, "潮符「湖のタイダルウェイブ」") },
            { (Day.First,   5), (new[] { Enemy.Cirno },        "氷王「フロストキング」") },
            { (Day.First,   6), (new[] { Enemy.Wakasagihime }, "魚符「スクールオブフィッシュ」") },
            { (Day.Second,  1), (new[] { Enemy.Kyouko },       "叫喚「プライマルスクリーム」") },
            { (Day.Second,  2), (new[] { Enemy.Sekibanki },    "飛首「エクストリームロングネック」") },
            { (Day.Second,  3), (new[] { Enemy.Kyouko },       "劈音「ピアッシングサークル」") },
            { (Day.Second,  4), (new[] { Enemy.Sekibanki },    "眼光「ヘルズレイ」") },
            { (Day.Second,  5), (new[] { Enemy.Kyouko },       "御経「無限念仏」") },
            { (Day.Second,  6), (new[] { Enemy.Sekibanki },    "飛首「ツインロクロヘッド」") },
            { (Day.Third,   1), (new[] { Enemy.Kagerou },      string.Empty) },
            { (Day.Third,   2), (new[] { Enemy.Kagerou },      "満月「フルムーンロア」") },
            { (Day.Third,   3), (new[] { Enemy.Keine },        "「２０ＸＸ年　死後の旅」") },
            { (Day.Third,   4), (new[] { Enemy.Mokou },        "惜命「不死身の捨て身」") },
            { (Day.Third,   5), (new[] { Enemy.Kagerou },      "狼牙「血に餓えたウルフファング」") },
            { (Day.Third,   6), (new[] { Enemy.Keine },        "大火「江戸のフラワー」") },
            { (Day.Third,   7), (new[] { Enemy.Mokou },        "「火の鳥 ―不死伝説―」") },
            { (Day.Fourth,  1), (new[] { Enemy.Yuyuko },       string.Empty) },
            { (Day.Fourth,  2), (new[] { Enemy.Seiga, Enemy.Yoshika }, "入魔「過剰ゾウフォルゥモォ」") },
            { (Day.Fourth,  3), (new[] { Enemy.Yuyuko },       "蝶符「花蝶風月」") },
            { (Day.Fourth,  4), (new[] { Enemy.Yoshika },      "毒爪「ゾンビクロー」") },
            { (Day.Fourth,  5), (new[] { Enemy.Seiga },        "仙術「ウォールランナー」") },
            { (Day.Fourth,  6), (new[] { Enemy.Yuyuko },       "桜花「桜吹雪花小町」") },
            { (Day.Fourth,  7), (new[] { Enemy.Seiga },        "仙術「壁抜けワームホール」") },
            { (Day.Fifth,   1), (new[] { Enemy.Raiko },        string.Empty) },
            { (Day.Fifth,   2), (new[] { Enemy.Yatsuhashi },   "琴符「天の詔琴」") },
            { (Day.Fifth,   3), (new[] { Enemy.Benben },       "音符「大熱唱琵琶」") },
            { (Day.Fifth,   4), (new[] { Enemy.Raiko },        "雷符「怒りのデンデン太鼓」") },
            { (Day.Fifth,   5), (new[] { Enemy.Yatsuhashi },   "哀歌「人琴ともに亡ぶ」") },
            { (Day.Fifth,   6), (new[] { Enemy.Benben },       "楽譜「スコアウェブ」") },
            { (Day.Fifth,   7), (new[] { Enemy.Raiko },        "太鼓「ファンタジックウーファー」") },
            { (Day.Fifth,   8), (new[] { Enemy.Benben, Enemy.Yatsuhashi }, "両吟「星降る唄」") },
            { (Day.Sixth,   1), (new[] { Enemy.Mamizou },      string.Empty) },
            { (Day.Sixth,   2), (new[] { Enemy.Aya },          "写真「激撮テングスクープ」") },
            { (Day.Sixth,   3), (new[] { Enemy.Hatate },       "写真「フルパノラマショット」") },
            { (Day.Sixth,   4), (new[] { Enemy.Nitori },       "瀑符「シライトフォール」") },
            { (Day.Sixth,   5), (new[] { Enemy.Momiji },       "牙符「咀嚼玩味」") },
            { (Day.Sixth,   6), (new[] { Enemy.Nitori },       "瀑符「ケゴンガン」") },
            { (Day.Sixth,   7), (new[] { Enemy.Hatate },       "写真「籠もりパパラッチ」") },
            { (Day.Sixth,   8), (new[] { Enemy.Aya },          "「瞬撮ジャーナリスト」") },
            { (Day.Seventh, 1), (new[] { Enemy.Marisa },       "恋符「ワイドマスター」") },
            { (Day.Seventh, 2), (new[] { Enemy.Sakuya },       "時符「タイムストッパー咲夜」") },
            { (Day.Seventh, 3), (new[] { Enemy.Youmu },        "光符「冥府光芒一閃」") },
            { (Day.Seventh, 4), (new[] { Enemy.Sanae },        "蛇符「バインドスネークカモン」") },
            { (Day.Seventh, 5), (new[] { Enemy.Marisa },       "恋符「マシンガンスパーク」") },
            { (Day.Seventh, 6), (new[] { Enemy.Sakuya },       "時符「チェンジリングマジック」") },
            { (Day.Seventh, 7), (new[] { Enemy.Youmu },        "彼岸剣「地獄極楽滅多斬り」") },
            { (Day.Seventh, 8), (new[] { Enemy.Sanae },        "蛇符「グリーンスネークカモン」") },
            { (Day.Eighth,  1), (new[] { Enemy.Shinmyoumaru }, string.Empty) },
            { (Day.Eighth,  2), (new[] { Enemy.Reimu },        "神籤「反則結界」") },
            { (Day.Eighth,  3), (new[] { Enemy.Mamizou },      "「鳴かぬなら泣くまで待とう時鳥」") },
            { (Day.Eighth,  4), (new[] { Enemy.Shinmyoumaru }, "「小人の地獄」") },
            { (Day.Eighth,  5), (new[] { Enemy.Reimu },        "「パスウェイジョンニードル」") },
            { (Day.Eighth,  6), (new[] { Enemy.Mamizou },      "「にんげんって良いな」") },
            { (Day.Eighth,  7), (new[] { Enemy.Shinmyoumaru }, "輝針「鬼ごろし両目突きの針」") },
            { (Day.Ninth,   1), (new[] { Enemy.Kanako },       "御柱「ライジングオンバシラ」") },
            { (Day.Ninth,   2), (new[] { Enemy.Suwako },       "緑石「ジェイドブレイク」") },
            { (Day.Ninth,   3), (new[] { Enemy.Futo },         "古舟「エンシェントシップ」") },
            { (Day.Ninth,   4), (new[] { Enemy.Suika },        "鬼群「インプスウォーム」") },
            { (Day.Ninth,   5), (new[] { Enemy.Kanako },       "「神の御威光」") },
            { (Day.Ninth,   6), (new[] { Enemy.Suwako },       "蛙符「血塗られた赤蛙塚」") },
            { (Day.Ninth,   7), (new[] { Enemy.Futo },         "熱龍「火焔龍脈」") },
            { (Day.Ninth,   8), (new[] { Enemy.Suika },        "鬼群「百鬼禿童」") },
            { (Day.Last,    1), (new[] { Enemy.Byakuren },     "「ハリの制縛」") },
            { (Day.Last,    2), (new[] { Enemy.Miko },         "「我こそが天道なり」") },
            { (Day.Last,    3), (new[] { Enemy.Tenshi },       "「全妖怪の緋想天」") },
            { (Day.Last,    4), (new[] { Enemy.Remilia },      "「フィットフルナイトメア」") },
            { (Day.Last,    5), (new[] { Enemy.Yukari },       "「不可能弾幕結界」") },
            { (Day.Last,    6), (new[] { Enemy.Byakuren },     "「ブラフマーの瞳」") },
            { (Day.Last,    7), (new[] { Enemy.Miko },         "「十七条の憲法爆弾」") },
            { (Day.Last,    8), (new[] { Enemy.Tenshi },       "「鹿島鎮護」") },
            { (Day.Last,    9), (new[] { Enemy.Remilia },      "「きゅうけつ鬼ごっこ」") },
            { (Day.Last,   10), (new[] { Enemy.Yukari },       "「運鈍根の捕物帖」") },
        };
}

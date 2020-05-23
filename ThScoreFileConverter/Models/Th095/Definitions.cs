//-----------------------------------------------------------------------
// <copyright file="Definitions.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;

namespace ThScoreFileConverter.Models.Th095
{
    internal static class Definitions
    {
        // Thanks to thwiki.info
        public static IReadOnlyDictionary<(Level Level, int Scene), (Enemy Enemy, string Card)> SpellCards { get; } =
            new Dictionary<(Level, int), (Enemy, string)>
            {
                { (Level.One,   1), (Enemy.Wriggle,   string.Empty) },
                { (Level.One,   2), (Enemy.Rumia,     string.Empty) },
                { (Level.One,   3), (Enemy.Wriggle,   "蛍符「地上の恒星」") },
                { (Level.One,   4), (Enemy.Rumia,     "闇符「ダークサイドオブザムーン」") },
                { (Level.One,   5), (Enemy.Wriggle,   "蝶符「バタフライストーム」") },
                { (Level.One,   6), (Enemy.Rumia,     "夜符「ミッドナイトバード」") },
                { (Level.Two,   1), (Enemy.Cirno,     string.Empty) },
                { (Level.Two,   2), (Enemy.Letty,     string.Empty) },
                { (Level.Two,   3), (Enemy.Cirno,     "雪符「ダイアモンドブリザード」") },
                { (Level.Two,   4), (Enemy.Letty,     "寒符「コールドスナップ」") },
                { (Level.Two,   5), (Enemy.Cirno,     "凍符「マイナスＫ」") },
                { (Level.Two,   6), (Enemy.Letty,     "冬符「ノーザンウイナー」") },
                { (Level.Three, 1), (Enemy.Alice,     string.Empty) },
                { (Level.Three, 2), (Enemy.Keine,     "光符「アマテラス」") },
                { (Level.Three, 3), (Enemy.Alice,     "操符「ドールズインシー」") },
                { (Level.Three, 4), (Enemy.Keine,     "包符「昭和の雨」") },
                { (Level.Three, 5), (Enemy.Alice,     "呪符「ストロードールカミカゼ」") },
                { (Level.Three, 6), (Enemy.Keine,     "葵符「水戸の光圀」") },
                { (Level.Three, 7), (Enemy.Alice,     "赤符「ドールミラセティ」") },
                { (Level.Three, 8), (Enemy.Keine,     "倭符「邪馬台の国」") },
                { (Level.Four,  1), (Enemy.Reisen,    string.Empty) },
                { (Level.Four,  2), (Enemy.Medicine,  "霧符「ガシングガーデン」") },
                { (Level.Four,  3), (Enemy.Tewi,      "脱兎「フラスターエスケープ」") },
                { (Level.Four,  4), (Enemy.Reisen,    "散符「朧月花栞（ロケット・イン・ミスト）」") },
                { (Level.Four,  5), (Enemy.Medicine,  "毒符「ポイズンブレス」") },
                { (Level.Four,  6), (Enemy.Reisen,    "波符「幻の月（インビジブルハーフムーン）」") },
                { (Level.Four,  7), (Enemy.Medicine,  "譫妄「イントゥデリリウム」") },
                { (Level.Four,  8), (Enemy.Tewi,      "借符「大穴牟遅様の薬」") },
                { (Level.Four,  9), (Enemy.Reisen,    "狂夢「風狂の夢（ドリームワールド）」") },
                { (Level.Five,  1), (Enemy.Meirin,    string.Empty) },
                { (Level.Five,  2), (Enemy.Patchouli, "日＆水符「ハイドロジェナスプロミネンス」") },
                { (Level.Five,  3), (Enemy.Meirin,    "華符「彩光蓮華掌」") },
                { (Level.Five,  4), (Enemy.Patchouli, "水＆火符「フロギスティックレイン」") },
                { (Level.Five,  5), (Enemy.Meirin,    "彩翔「飛花落葉」") },
                { (Level.Five,  6), (Enemy.Patchouli, "月＆木符「サテライトヒマワリ」") },
                { (Level.Five,  7), (Enemy.Meirin,    "彩華「虹色太極拳」") },
                { (Level.Five,  8), (Enemy.Patchouli, "日＆月符「ロイヤルダイアモンドリング」") },
                { (Level.Six,   1), (Enemy.Chen,      string.Empty) },
                { (Level.Six,   2), (Enemy.Youmu,     "人智剣「天女返し」") },
                { (Level.Six,   3), (Enemy.Chen,      "星符「飛び重ね鱗」") },
                { (Level.Six,   4), (Enemy.Youmu,     "妄執剣「修羅の血」") },
                { (Level.Six,   5), (Enemy.Chen,      "鬼神「鳴動持国天」") },
                { (Level.Six,   6), (Enemy.Youmu,     "天星剣「涅槃寂静の如し」") },
                { (Level.Six,   7), (Enemy.Chen,      "化猫「橙」") },
                { (Level.Six,   8), (Enemy.Youmu,     "四生剣「衆生無情の響き」") },
                { (Level.Seven, 1), (Enemy.Sakuya,    string.Empty) },
                { (Level.Seven, 2), (Enemy.Remilia,   "魔符「全世界ナイトメア」") },
                { (Level.Seven, 3), (Enemy.Sakuya,    "時符「トンネルエフェクト」") },
                { (Level.Seven, 4), (Enemy.Remilia,   "紅符「ブラッディマジックスクウェア」") },
                { (Level.Seven, 5), (Enemy.Sakuya,    "空虚「インフレーションスクウェア」") },
                { (Level.Seven, 6), (Enemy.Remilia,   "紅蝙蝠「ヴァンピリッシュナイト」") },
                { (Level.Seven, 7), (Enemy.Sakuya,    "銀符「パーフェクトメイド」") },
                { (Level.Seven, 8), (Enemy.Remilia,   "神鬼「レミリアストーカー」") },
                { (Level.Eight, 1), (Enemy.Ran,       string.Empty) },
                { (Level.Eight, 2), (Enemy.Yuyuko,    "幽雅「死出の誘蛾灯」") },
                { (Level.Eight, 3), (Enemy.Ran,       "密符「御大師様の秘鍵」") },
                { (Level.Eight, 4), (Enemy.Yuyuko,    "蝶符「鳳蝶紋の死槍」") },
                { (Level.Eight, 5), (Enemy.Ran,       "行符「八千万枚護摩」") },
                { (Level.Eight, 6), (Enemy.Yuyuko,    "死符「酔人の生、死の夢幻」") },
                { (Level.Eight, 7), (Enemy.Ran,       "超人「飛翔役小角」") },
                { (Level.Eight, 8), (Enemy.Yuyuko,    "「死蝶浮月」") },
                { (Level.Nine,  1), (Enemy.Eirin,     string.Empty) },
                { (Level.Nine,  2), (Enemy.Kaguya,    "新難題「月のイルメナイト」") },
                { (Level.Nine,  3), (Enemy.Eirin,     "薬符「胡蝶夢丸ナイトメア」") },
                { (Level.Nine,  4), (Enemy.Kaguya,    "新難題「エイジャの赤石」") },
                { (Level.Nine,  5), (Enemy.Eirin,     "錬丹「水銀の海」") },
                { (Level.Nine,  6), (Enemy.Kaguya,    "新難題「金閣寺の一枚天井」") },
                { (Level.Nine,  7), (Enemy.Eirin,     "秘薬「仙香玉兎」") },
                { (Level.Nine,  8), (Enemy.Kaguya,    "新難題「ミステリウム」") },
                { (Level.Ten,   1), (Enemy.Komachi,   string.Empty) },
                { (Level.Ten,   2), (Enemy.Shikieiki, "嘘言「タン・オブ・ウルフ」") },
                { (Level.Ten,   3), (Enemy.Komachi,   "死歌「八重霧の渡し」") },
                { (Level.Ten,   4), (Enemy.Shikieiki, "審判「十王裁判」") },
                { (Level.Ten,   5), (Enemy.Komachi,   "古雨「黄泉中有の旅の雨」") },
                { (Level.Ten,   6), (Enemy.Shikieiki, "審判「ギルティ・オワ・ノットギルティ」") },
                { (Level.Ten,   7), (Enemy.Komachi,   "死価「プライス・オブ・ライフ」") },
                { (Level.Ten,   8), (Enemy.Shikieiki, "審判「浄頗梨審判 -射命丸文-」") },
                { (Level.Extra, 1), (Enemy.Flandre,   "禁忌「フォービドゥンフルーツ」") },
                { (Level.Extra, 2), (Enemy.Flandre,   "禁忌「禁じられた遊び」") },
                { (Level.Extra, 3), (Enemy.Yukari,    "境符「色と空の境界」") },
                { (Level.Extra, 4), (Enemy.Yukari,    "境符「波と粒の境界」") },
                { (Level.Extra, 5), (Enemy.Mokou,     "貴人「サンジェルマンの忠告」") },
                { (Level.Extra, 6), (Enemy.Mokou,     "蓬莱「瑞江浦嶋子と五色の瑞亀」") },
                { (Level.Extra, 7), (Enemy.Suika,     "鬼気「濛々迷霧」") },
                { (Level.Extra, 8), (Enemy.Suika,     "「百万鬼夜行」") },
            };
    }
}

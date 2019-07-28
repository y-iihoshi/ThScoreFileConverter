//-----------------------------------------------------------------------
// <copyright file="Th105Converter.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable 1591
#pragma warning disable SA1600 // ElementsMustBeDocumented
#pragma warning disable SA1602 // EnumerationItemsMustBeDocumented

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;
using ClearData = ThScoreFileConverter.Models.Th105.ClearData<
    ThScoreFileConverter.Models.Th105Converter.Chara, ThScoreFileConverter.Models.Th105Converter.Level>;
using SpellCardResult = ThScoreFileConverter.Models.Th105.SpellCardResult<
    ThScoreFileConverter.Models.Th105Converter.Chara, ThScoreFileConverter.Models.Th105Converter.Level>;
using StageInfo = ThScoreFileConverter.Models.Th105.StageInfo<
    ThScoreFileConverter.Models.Th105Converter.Stage, ThScoreFileConverter.Models.Th105Converter.Chara>;

namespace ThScoreFileConverter.Models
{
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Reviewed.")]
    internal class Th105Converter : ThConverter
    {
        // Thanks to en.touhouwiki.net
        private static readonly Dictionary<int, string> SystemCardNameTable =
            new Dictionary<int, string>
            {
                { 0, "「気質発現」" },
                { 1, "「霊撃」" },
                { 2, "「ガード反撃」" },
                { 3, "「スペル増幅」" },
                { 4, "「体力回復」" },
                { 5, "「霊力回復」" },
            };

        // Thanks to th3_5154.txt, www.toho-motoneta.net and en.touhouwiki.net
        private static readonly Dictionary<(Chara Chara, int CardId), string> CardNameTable =
            new Dictionary<(Chara, int), string>
            {
                { (Chara.Reimu,   0), "祈願「厄除け祈願」" },
                { (Chara.Reimu,   1), "宝符「躍る陰陽玉」" },
                { (Chara.Reimu,   2), "夢境「二重大結界」" },
                { (Chara.Reimu,   3), "光霊「神霊宝珠」" },
                { (Chara.Reimu,   4), "「境界の内側に潜む霊と不思議な巫女」" },
                { (Chara.Reimu, 100), "博麗アミュレット" },
                { (Chara.Reimu, 101), "警醒陣" },
                { (Chara.Reimu, 102), "亜空穴" },
                { (Chara.Reimu, 103), "昇天脚" },
                { (Chara.Reimu, 104), "妖怪バスター" },
                { (Chara.Reimu, 105), "繋縛陣" },
                { (Chara.Reimu, 106), "封魔亜空穴" },
                { (Chara.Reimu, 107), "抄地昇天脚" },
                { (Chara.Reimu, 108), "拡散アミュレット" },
                { (Chara.Reimu, 109), "常置陣" },
                { (Chara.Reimu, 110), "刹那亜空穴" },
                { (Chara.Reimu, 200), "霊符「夢想妙珠」" },
                { (Chara.Reimu, 201), "神霊「夢想封印」" },
                { (Chara.Reimu, 206), "神技「八方鬼縛陣」" },
                { (Chara.Reimu, 208), "珠符「明珠暗投」" },
                { (Chara.Reimu, 209), "宝符「陰陽宝玉」" },
                { (Chara.Reimu, 210), "宝具「陰陽鬼神玉」" },
                { (Chara.Reimu, 214), "神技「天覇風神脚」" },
                { (Chara.Reimu, 219), "「夢想天生」" },
                { (Chara.Marisa,   0), "星符「ポラリスユニーク」" },
                { (Chara.Marisa,   1), "天儀「オーレリーズユニバース」" },
                { (Chara.Marisa,   2), "邪恋「実りやすいマスタースパーク」" },
                { (Chara.Marisa, 100), "ウィッチレイライン" },
                { (Chara.Marisa, 101), "ミアズマスウィープ" },
                { (Chara.Marisa, 102), "グラウンドスターダスト" },
                { (Chara.Marisa, 103), "メテオニックデブリ" },
                { (Chara.Marisa, 104), "ラジアルストライク" },
                { (Chara.Marisa, 105), "バスキースウィーパー" },
                { (Chara.Marisa, 106), "デビルダムトーチ" },
                { (Chara.Marisa, 107), "ナロースパーク" },
                { (Chara.Marisa, 108), "アップスウィープ" },
                { (Chara.Marisa, 109), "ステラミサイル" },
                { (Chara.Marisa, 111), "グリーンスプレッド" },
                { (Chara.Marisa, 200), "恋符「マスタースパーク」" },
                { (Chara.Marisa, 202), "魔砲「ファイナルスパーク」" },
                { (Chara.Marisa, 203), "星符「ドラゴンメテオ」" },
                { (Chara.Marisa, 205), "魔符「スターダストレヴァリエ」" },
                { (Chara.Marisa, 206), "星符「エスケープベロシティ」" },
                { (Chara.Marisa, 207), "彗星「ブレイジングスター」" },
                { (Chara.Marisa, 208), "星符「メテオニックシャワー」" },
                { (Chara.Marisa, 211), "光符「ルミネスストライク」" },
                { (Chara.Marisa, 215), "儀符「オーレリーズサン」" },
                { (Chara.Marisa, 219), "邪恋「実りやすいマスタースパーク」" },
                { (Chara.Sakuya,   0), "銀符「シルバーバウンド」" },
                { (Chara.Sakuya,   1), "時符「咲夜特製ストップウォッチ」" },
                { (Chara.Sakuya,   2), "幻術「マイナイフリカージョン」" },
                { (Chara.Sakuya,   3), "時符「シルバーアキュート３６０」" },
                { (Chara.Sakuya, 100), "クロースアップマジック" },
                { (Chara.Sakuya, 101), "バウンスノーバウンス" },
                { (Chara.Sakuya, 102), "マジックスターソード" },
                { (Chara.Sakuya, 103), "バニシングエブリシング" },
                { (Chara.Sakuya, 104), "プロペリングシルバー" },
                { (Chara.Sakuya, 105), "スクウェアリコシェ" },
                { (Chara.Sakuya, 106), "離剣の見" },
                { (Chara.Sakuya, 107), "パーフェクトメイド" },
                { (Chara.Sakuya, 200), "幻符「殺人ドール」" },
                { (Chara.Sakuya, 201), "時符「プライベートスクウェア」" },
                { (Chara.Sakuya, 202), "傷符「インスクライブレッドソウル」" },
                { (Chara.Sakuya, 203), "幻葬「夜霧の幻影殺人鬼」" },
                { (Chara.Sakuya, 204), "「咲夜の世界」" },
                { (Chara.Sakuya, 205), "傷魂「ソウルスカルプチュア」" },
                { (Chara.Sakuya, 206), "銀符「シルバーバウンド」" },
                { (Chara.Sakuya, 207), "奇術「エターナルミーク」" },
                { (Chara.Sakuya, 208), "速符「ルミネスリコシェ」" },
                { (Chara.Sakuya, 209), "時符「咲夜特製ストップウォッチ」" },
                { (Chara.Alice,   0), "足軽「スーサイドスクワッド」" },
                { (Chara.Alice,   1), "剣符「ソルジャーオブクロス」" },
                { (Chara.Alice,   2), "人形「魂のないフォークダンス」" },
                { (Chara.Alice, 100), "人形操創" },
                { (Chara.Alice, 101), "人形無操" },
                { (Chara.Alice, 102), "人形置操" },
                { (Chara.Alice, 103), "人形振起" },
                { (Chara.Alice, 104), "人形帰巣" },
                { (Chara.Alice, 105), "人形火葬" },
                { (Chara.Alice, 106), "人形千槍" },
                { (Chara.Alice, 107), "人形ＳＰ" },
                { (Chara.Alice, 109), "大江戸爆薬からくり人形" },
                { (Chara.Alice, 111), "シーカーワイヤー" },
                { (Chara.Alice, 200), "魔符「アーティフルサクリファイス」" },
                { (Chara.Alice, 201), "戦符「リトルレギオン」" },
                { (Chara.Alice, 202), "咒符「上海人形」" },
                { (Chara.Alice, 203), "魔操「リターンイナニメトネス」" },
                { (Chara.Alice, 204), "戦操「ドールズウォー」" },
                { (Chara.Alice, 205), "咒詛「蓬莱人形」" },
                { (Chara.Alice, 206), "偵符「シーカードールズ」" },
                { (Chara.Alice, 207), "紅符「和蘭人形」" },
                { (Chara.Alice, 208), "人形「未来文楽」" },
                { (Chara.Alice, 209), "注力「トリップワイヤー」" },
                { (Chara.Patchouli,   0), "火符「アキバサマー」" },
                { (Chara.Patchouli,   1), "水符「ジェリーフィッシュプリンセス」" },
                { (Chara.Patchouli,   2), "月金符「サンシャインリフレクター」" },
                { (Chara.Patchouli, 100), "サマーレッド" },
                { (Chara.Patchouli, 101), "ウィンターエレメント" },
                { (Chara.Patchouli, 102), "スプリングウィンド" },
                { (Chara.Patchouli, 103), "オータムエッジ" },
                { (Chara.Patchouli, 104), "ドヨースピア" },
                { (Chara.Patchouli, 105), "サマーフレイム" },
                { (Chara.Patchouli, 106), "コンデンスドバブル" },
                { (Chara.Patchouli, 107), "フラッシュオブスプリング" },
                { (Chara.Patchouli, 108), "オータムブレード" },
                { (Chara.Patchouli, 109), "エメラルドシティ" },
                { (Chara.Patchouli, 200), "火金符「セントエルモピラー」" },
                { (Chara.Patchouli, 201), "土水符「ノエキアンデリュージュ」" },
                { (Chara.Patchouli, 202), "金木符「エレメンタルハーベスター」" },
                { (Chara.Patchouli, 203), "日符「ロイヤルフレア」" },
                { (Chara.Patchouli, 204), "月符「サイレントセレナ」" },
                { (Chara.Patchouli, 205), "火水木金土符「賢者の石」" },
                { (Chara.Patchouli, 206), "水符「ジェリーフィッシュプリンセス」" },
                { (Chara.Patchouli, 207), "月木符「サテライトヒマワリ」" },
                { (Chara.Patchouli, 210), "日木符「フォトシンセシス」" },
                { (Chara.Youmu,   0), "人智剣「天女返し」" },
                { (Chara.Youmu,   1), "桜花剣「閃々散華」" },
                { (Chara.Youmu,   2), "断想剣「草木成仏斬」" },
                { (Chara.Youmu,   3), "瞑斬「楼観から弾をも断つ心の眼」" },
                { (Chara.Youmu, 100), "反射下界斬" },
                { (Chara.Youmu, 101), "弦月斬" },
                { (Chara.Youmu, 102), "生死流転斬" },
                { (Chara.Youmu, 103), "憑坐の縛" },
                { (Chara.Youmu, 104), "結跏趺斬" },
                { (Chara.Youmu, 105), "折伏無間" },
                { (Chara.Youmu, 106), "心抄斬" },
                { (Chara.Youmu, 107), "悪し魂" },
                { (Chara.Youmu, 109), "炯眼剣" },
                { (Chara.Youmu, 111), "奇び半身" },
                { (Chara.Youmu, 200), "人符「現世斬」" },
                { (Chara.Youmu, 201), "断命剣「冥想斬」" },
                { (Chara.Youmu, 202), "魂符「幽明の苦輪」" },
                { (Chara.Youmu, 203), "人鬼「未来永劫斬」" },
                { (Chara.Youmu, 204), "断迷剣「迷津慈航斬」" },
                { (Chara.Youmu, 205), "魂魄「幽明求聞持聡明の法」" },
                { (Chara.Youmu, 206), "剣伎「桜花閃々」" },
                { (Chara.Youmu, 207), "断霊剣「成仏得脱斬」" },
                { (Chara.Remilia,   0), "運命「ミゼラブルフェイト」" },
                { (Chara.Remilia,   1), "夜符「ボンバードナイト」" },
                { (Chara.Remilia,   2), "蝙蝠「ヴァンパイアスウィープ」" },
                { (Chara.Remilia,   3), "神鬼「レミリアストーカー」" },
                { (Chara.Remilia, 100), "デーモンロードウォーク" },
                { (Chara.Remilia, 101), "サーヴァントフライヤー" },
                { (Chara.Remilia, 102), "デーモンロードクレイドル" },
                { (Chara.Remilia, 103), "デーモンロードアロー" },
                { (Chara.Remilia, 104), "ヴァンパイアクロウ" },
                { (Chara.Remilia, 105), "チェーンギャング" },
                { (Chara.Remilia, 106), "ロケットキックアップ" },
                { (Chara.Remilia, 107), "シーリングフィア" },
                { (Chara.Remilia, 109), "デモンズディナーフォーク" },
                { (Chara.Remilia, 200), "紅符「不夜城レッド」" },
                { (Chara.Remilia, 201), "必殺「ハートブレイク」" },
                { (Chara.Remilia, 202), "夜符「デーモンキングクレイドル」" },
                { (Chara.Remilia, 203), "紅魔「スカーレットデビル」" },
                { (Chara.Remilia, 204), "神槍「スピア・ザ・グングニル」" },
                { (Chara.Remilia, 205), "夜王「ドラキュラクレイドル」" },
                { (Chara.Remilia, 206), "夜符「バッドレディスクランブル」" },
                { (Chara.Remilia, 207), "運命「ミゼラブルフェイト」" },
                { (Chara.Yuyuko,   0), "幽符「冥界ミステリースポット」" },
                { (Chara.Yuyuko,   1), "霊蝶「蝶の羽風生に暫く」" },
                { (Chara.Yuyuko,   2), "宴会「死して全て大団円」" },
                { (Chara.Yuyuko, 100), "幽胡蝶" },
                { (Chara.Yuyuko, 101), "未生の光" },
                { (Chara.Yuyuko, 102), "悉皆彷徨" },
                { (Chara.Yuyuko, 103), "胡蝶夢の舞" },
                { (Chara.Yuyuko, 104), "好死の霊" },
                { (Chara.Yuyuko, 105), "鳳蝶紋の槍" },
                { (Chara.Yuyuko, 106), "誘霊の甘蜜" },
                { (Chara.Yuyuko, 107), "逆さ屏風" },
                { (Chara.Yuyuko, 108), "スフィアブルーム" },
                { (Chara.Yuyuko, 200), "死符「ギャストリドリーム」" },
                { (Chara.Yuyuko, 201), "冥符「黄泉平坂行路」" },
                { (Chara.Yuyuko, 202), "霊符「无寿の夢」" },
                { (Chara.Yuyuko, 203), "死蝶「華胥の永眠」" },
                { (Chara.Yuyuko, 204), "再迷「幻想郷の黄泉還り」" },
                { (Chara.Yuyuko, 205), "寿命「无寿国への約束手形」" },
                { (Chara.Yuyuko, 206), "霊蝶「蝶の羽風生に暫く」" },
                { (Chara.Yuyuko, 207), "蝶符「鳳蝶紋の死槍」" },
                { (Chara.Yuyuko, 208), "幽雅「死出の誘蛾灯」" },
                { (Chara.Yukari,   0), "光弾「ドップラーエフェクト」" },
                { (Chara.Yukari,   1), "捌器「全てを二つに別ける物」" },
                { (Chara.Yukari,   2), "幻巣「飛光虫ネスト」" },
                { (Chara.Yukari,   3), "空餌「狂躁高速飛行物体」" },
                { (Chara.Yukari,   4), "「八雲の巣」" },
                { (Chara.Yukari, 100), "開けて悔しき玉手箱" },
                { (Chara.Yukari, 101), "禅寺に潜む妖蝶" },
                { (Chara.Yukari, 102), "枕石漱流" },
                { (Chara.Yukari, 103), "幻想狂想穴" },
                { (Chara.Yukari, 104), "至る処に青山あり" },
                { (Chara.Yukari, 105), "幻想卍傘" },
                { (Chara.Yukari, 106), "物質と反物質の宇宙" },
                { (Chara.Yukari, 107), "肉体分解機" },
                { (Chara.Yukari, 108), "魅惑のエサ" },
                { (Chara.Yukari, 200), "境符「四重結界」" },
                { (Chara.Yukari, 201), "式神「八雲藍」" },
                { (Chara.Yukari, 202), "境符「二次元と三次元の境界」" },
                { (Chara.Yukari, 203), "結界「魅力的な四重結界」" },
                { (Chara.Yukari, 204), "式神「橙」" },
                { (Chara.Yukari, 205), "結界「客観結界」" },
                { (Chara.Yukari, 206), "幻巣「飛光虫ネスト」" },
                { (Chara.Yukari, 207), "空餌「中毒性のあるエサ」" },
                { (Chara.Yukari, 215), "廃線「ぶらり廃駅下車の旅」" },
                { (Chara.Suika,   0), "吐息「小鬼の深呼吸」" },
                { (Chara.Suika,   1), "火弾「地霊活性弾」" },
                { (Chara.Suika,   2), "鬼神「ミッシングパープルパワー」" },
                { (Chara.Suika,   3), "鬼気「濛々迷霧」" },
                { (Chara.Suika,   4), "「百万同一鬼」" },
                { (Chara.Suika, 100), "妖鬼-密-" },
                { (Chara.Suika, 101), "地霊-密-" },
                { (Chara.Suika, 102), "妖鬼-疎-" },
                { (Chara.Suika, 103), "萃鬼" },
                { (Chara.Suika, 104), "元鬼玉" },
                { (Chara.Suika, 105), "地霊-疎-" },
                { (Chara.Suika, 106), "厭霧" },
                { (Chara.Suika, 107), "疎鬼" },
                { (Chara.Suika, 109), "火鬼" },
                { (Chara.Suika, 200), "萃符「戸隠山投げ」" },
                { (Chara.Suika, 201), "酔神「鬼縛りの術」" },
                { (Chara.Suika, 202), "鬼符「ミッシングパワー」" },
                { (Chara.Suika, 203), "萃鬼「天手力男投げ」" },
                { (Chara.Suika, 204), "酔夢「施餓鬼縛りの術」" },
                { (Chara.Suika, 205), "鬼神「ミッシングパープルパワー」" },
                { (Chara.Suika, 206), "霧符「雲集霧散」" },
                { (Chara.Suika, 207), "鬼火「超高密度燐禍術」" },
                { (Chara.Suika, 208), "鬼符「大江山悉皆殺し」" },
                { (Chara.Reisen,   0), "迫符「脅迫幻覚(オブセッショナー)」" },
                { (Chara.Reisen,   1), "幻弾「幻想視差(ブラフバラージ)」" },
                { (Chara.Reisen,   2), "幻兎「平行交差(パラレルクロス)」" },
                { (Chara.Reisen, 100), "マインドエクスプロージョン" },
                { (Chara.Reisen, 101), "イリュージョナリィブラスト" },
                { (Chara.Reisen, 102), "フィールドウルトラレッド" },
                { (Chara.Reisen, 103), "ディスビリーフアスペクト" },
                { (Chara.Reisen, 104), "マインドベンディング" },
                { (Chara.Reisen, 105), "アイサイトクリーニング" },
                { (Chara.Reisen, 106), "フィールドウルトラバイオレット" },
                { (Chara.Reisen, 107), "ディスオーダーアイ" },
                { (Chara.Reisen, 108), "マインドドロッピング" },
                { (Chara.Reisen, 109), "リップルヴィジョン" },
                { (Chara.Reisen, 110), "アンダーセンスブレイク" },
                { (Chara.Reisen, 200), "惑視「離円花冠(カローラヴィジョン)」" },
                { (Chara.Reisen, 202), "幻爆「近眼花火(マインドスターマイン)」" },
                { (Chara.Reisen, 203), "幻惑「花冠視線(クラウンヴィジョン)」" },
                { (Chara.Reisen, 205), "「幻朧月睨(ルナティックレッドアイズ)」" },
                { (Chara.Reisen, 206), "弱心「喪心喪意(ディモチヴェイション)」" },
                { (Chara.Reisen, 207), "喪心「喪心創痍(ディスカーダー)」" },
                { (Chara.Reisen, 208), "毒煙幕「瓦斯織物の玉」" },
                { (Chara.Reisen, 209), "生薬「国士無双の薬」" },
                { (Chara.Aya,   0), "突風「猿田彦の先導」" },
                { (Chara.Aya,   1), "旋符「飄妖扇」" },
                { (Chara.Aya,   2), "旋風「鳥居つむじ風」" },
                { (Chara.Aya,   3), "「幻想風靡」" },
                { (Chara.Aya, 100), "疾風扇" },
                { (Chara.Aya, 101), "疾走風靡" },
                { (Chara.Aya, 102), "天狗の立風露" },
                { (Chara.Aya, 103), "暗夜の礫" },
                { (Chara.Aya, 104), "烈風扇" },
                { (Chara.Aya, 105), "疾走優美" },
                { (Chara.Aya, 106), "天狗のダウンバースト" },
                { (Chara.Aya, 107), "鎌風ベーリング" },
                { (Chara.Aya, 109), "天狗ナメシ" },
                { (Chara.Aya, 110), "天狗の太鼓" },
                { (Chara.Aya, 200), "旋符「紅葉扇風」" },
                { (Chara.Aya, 201), "竜巻「天孫降臨の道しるべ」" },
                { (Chara.Aya, 202), "逆風「人間禁制の道」" },
                { (Chara.Aya, 203), "突符「天狗のマクロバースト」" },
                { (Chara.Aya, 205), "風符「天狗道の開風」" },
                { (Chara.Aya, 206), "「幻想風靡」" },
                { (Chara.Aya, 211), "魔獣「鎌鼬ベーリング」" },
                { (Chara.Aya, 212), "突風「猿田彦の先導」" },
                { (Chara.Komachi,   0), "霊符「古き地縛霊の目覚め」" },
                { (Chara.Komachi,   1), "死符「死者選別の鎌」" },
                { (Chara.Komachi,   2), "魂符「魂の遊戯」" },
                { (Chara.Komachi, 100), "迷わず生きた人霊" },
                { (Chara.Komachi, 101), "浮かばれない地縛霊" },
                { (Chara.Komachi, 102), "脱魂の儀" },
                { (Chara.Komachi, 103), "怠惰に生きた浮遊霊" },
                { (Chara.Komachi, 104), "死神の大鎌" },
                { (Chara.Komachi, 105), "死出の風" },
                { (Chara.Komachi, 106), "無間の道" },
                { (Chara.Komachi, 107), "寂しがり屋の緊縛霊" },
                { (Chara.Komachi, 108), "通りすがりの人霊" },
                { (Chara.Komachi, 110), "お迎え体験版" },
                { (Chara.Komachi, 200), "舟符「河の流れのように」" },
                { (Chara.Komachi, 201), "薄命「余命幾許も無し」" },
                { (Chara.Komachi, 202), "霊符「何処にでもいる浮遊霊」" },
                { (Chara.Komachi, 203), "死歌「八重霧の渡し」" },
                { (Chara.Komachi, 204), "換命「不惜身命、可惜身命」" },
                { (Chara.Komachi, 205), "恨符「未練がましい緊縛霊」" },
                { (Chara.Komachi, 206), "死符「死者選別の鎌」" },
                { (Chara.Komachi, 211), "地獄「無間の狭間」" },
                { (Chara.Iku,   0), "光珠「龍の光る眼」" },
                { (Chara.Iku,   1), "雷符「神鳴り様の住処」" },
                { (Chara.Iku,   2), "棘符「雷雲棘魚」" },
                { (Chara.Iku,   3), "雲界「玄雲海の雷庭」" },
                { (Chara.Iku, 100), "龍魚の一撃" },
                { (Chara.Iku, 101), "羽衣は水の如く" },
                { (Chara.Iku, 102), "龍魚の怒り" },
                { (Chara.Iku, 103), "静電誘導弾" },
                { (Chara.Iku, 104), "龍神の一撃" },
                { (Chara.Iku, 105), "羽衣は風の如く" },
                { (Chara.Iku, 106), "龍神の怒り" },
                { (Chara.Iku, 107), "龍神の稲光り" },
                { (Chara.Iku, 111), "龍の眼" },
                { (Chara.Iku, 200), "電符「雷鼓弾」" },
                { (Chara.Iku, 201), "魚符「龍魚ドリル」" },
                { (Chara.Iku, 202), "雷符「エレキテルの龍宮」" },
                { (Chara.Iku, 203), "光星「光龍の吐息」" },
                { (Chara.Iku, 206), "雷魚「雷雲魚遊泳弾」" },
                { (Chara.Iku, 207), "羽衣「羽衣は空の如く」" },
                { (Chara.Iku, 209), "棘符「雷雲棘魚」" },
                { (Chara.Iku, 210), "龍魚「龍宮の使い遊泳弾」" },
                { (Chara.Tenshi,   0), "要石「天空の霊石」" },
                { (Chara.Tenshi,   1), "乾坤「荒々しくも母なる大地よ」" },
                { (Chara.Tenshi,   2), "霊想「大地を鎮める石」" },
                { (Chara.Tenshi,   3), "天地「世界を見下ろす遥かなる大地よ」" },
                { (Chara.Tenshi,   4), "「全人類の緋想天」" },
                { (Chara.Tenshi, 100), "坤儀の剣" },
                { (Chara.Tenshi, 101), "天罰の石柱" },
                { (Chara.Tenshi, 102), "非想の威光" },
                { (Chara.Tenshi, 103), "非想の剣" },
                { (Chara.Tenshi, 104), "六震-相-" },
                { (Chara.Tenshi, 105), "守りの要" },
                { (Chara.Tenshi, 106), "天地プレス" },
                { (Chara.Tenshi, 107), "緋想の剣" },
                { (Chara.Tenshi, 200), "地符「不譲土壌の剣」" },
                { (Chara.Tenshi, 201), "非想「非想非非想の剣」" },
                { (Chara.Tenshi, 202), "天符「天道是非の剣」" },
                { (Chara.Tenshi, 203), "地震「先憂後楽の剣」" },
                { (Chara.Tenshi, 204), "気符「天啓気象の剣」" },
                { (Chara.Tenshi, 205), "要石「天地開闢プレス」" },
                { (Chara.Tenshi, 206), "気符「無念無想の境地」" },
                { (Chara.Tenshi, 207), "「全人類の緋想天」" },
            };

        // Thanks to en.touhouwiki.net
        private static readonly Dictionary<Chara, List<StageInfo>> StageInfoTable =
            new Dictionary<Chara, List<StageInfo>>
            {
                {
                    Chara.Reimu,
                    new List<StageInfo>
                    {
                        new StageInfo(Stage.St1, Chara.Marisa,  Enumerable.Range(0, 2)),
                        new StageInfo(Stage.St2, Chara.Alice,   Enumerable.Range(0, 2)),
                        new StageInfo(Stage.St3, Chara.Komachi, Enumerable.Range(0, 3)),
                        new StageInfo(Stage.St4, Chara.Aya,     Enumerable.Range(0, 3)),
                        new StageInfo(Stage.St5, Chara.Iku,     Enumerable.Range(0, 4)),
                        new StageInfo(Stage.St6, Chara.Tenshi,  Enumerable.Range(0, 5)),
                    }
                },
                {
                    Chara.Marisa,
                    new List<StageInfo>
                    {
                        new StageInfo(Stage.St1, Chara.Alice,  Enumerable.Range(0, 2)),
                        new StageInfo(Stage.St2, Chara.Reimu,  Enumerable.Range(0, 2)),
                        new StageInfo(Stage.St3, Chara.Reisen, Enumerable.Range(0, 3)),
                        new StageInfo(Stage.St4, Chara.Aya,    Enumerable.Range(0, 3)),
                        new StageInfo(Stage.St5, Chara.Iku,    Enumerable.Range(0, 4)),
                        new StageInfo(Stage.St6, Chara.Tenshi, Enumerable.Range(0, 5)),
                    }
                },
                {
                    Chara.Sakuya,
                    new List<StageInfo>
                    {
                        new StageInfo(Stage.St1, Chara.Marisa, Enumerable.Range(0, 2)),
                        new StageInfo(Stage.St2, Chara.Youmu,  Enumerable.Range(0, 2)),
                        new StageInfo(Stage.St3, Chara.Yuyuko, Enumerable.Range(0, 3)),
                        new StageInfo(Stage.St4, Chara.Aya,    Enumerable.Range(0, 3)),
                        new StageInfo(Stage.St5, Chara.Iku,    Enumerable.Range(0, 4)),
                        new StageInfo(Stage.St6, Chara.Tenshi, Enumerable.Range(0, 5)),
                    }
                },
                {
                    Chara.Alice,
                    new List<StageInfo>
                    {
                        new StageInfo(Stage.St1, Chara.Reimu,     Enumerable.Range(0, 2)),
                        new StageInfo(Stage.St2, Chara.Youmu,     Enumerable.Range(0, 2)),
                        new StageInfo(Stage.St3, Chara.Patchouli, Enumerable.Range(0, 3)),
                        new StageInfo(Stage.St4, Chara.Sakuya,    Enumerable.Range(0, 3)),
                        new StageInfo(Stage.St5, Chara.Iku,       Enumerable.Range(0, 4)),
                        new StageInfo(Stage.St6, Chara.Tenshi,    Enumerable.Range(0, 5)),
                    }
                },
                {
                    Chara.Patchouli,
                    new List<StageInfo>
                    {
                        new StageInfo(Stage.St1, Chara.Sakuya, Enumerable.Range(0, 2)),
                        new StageInfo(Stage.St2, Chara.Reimu,  Enumerable.Range(0, 2)),
                        new StageInfo(Stage.St3, Chara.Marisa, Enumerable.Range(0, 3)),
                        new StageInfo(Stage.St4, Chara.Iku,    Enumerable.Range(0, 3)),
                        new StageInfo(Stage.St5, Chara.Tenshi, Enumerable.Range(0, 4)),
                        new StageInfo(Stage.St6, Chara.Suika,  Enumerable.Range(0, 5)),
                    }
                },
                {
                    Chara.Youmu,
                    new List<StageInfo>
                    {
                        new StageInfo(Stage.St1, Chara.Yuyuko,  Enumerable.Range(0, 2)),
                        new StageInfo(Stage.St2, Chara.Reimu,   Enumerable.Range(0, 2)),
                        new StageInfo(Stage.St3, Chara.Komachi, Enumerable.Range(0, 3)),
                        new StageInfo(Stage.St4, Chara.Yukari,  Enumerable.Range(0, 3)),
                        new StageInfo(Stage.St5, Chara.Iku,     Enumerable.Range(0, 4)),
                        new StageInfo(Stage.St6, Chara.Tenshi,  Enumerable.Range(0, 5)),
                    }
                },
                {
                    Chara.Remilia,
                    new List<StageInfo>
                    {
                        new StageInfo(Stage.St1, Chara.Sakuya,    Enumerable.Range(0, 2)),
                        new StageInfo(Stage.St2, Chara.Patchouli, Enumerable.Range(0, 2)),
                        new StageInfo(Stage.St3, Chara.Alice,     Enumerable.Range(0, 2)),
                        new StageInfo(Stage.St4, Chara.Reisen,    Enumerable.Range(0, 3)),
                        new StageInfo(Stage.St5, Chara.Youmu,     Enumerable.Range(0, 3)),
                        new StageInfo(Stage.St6, Chara.Komachi,   Enumerable.Range(0, 3)),
                        new StageInfo(Stage.St7, Chara.Reimu,     Enumerable.Range(0, 4)),
                        new StageInfo(Stage.St8, Chara.Yukari,    Enumerable.Range(0, 5)),
                    }
                },
                {
                    Chara.Yuyuko,
                    new List<StageInfo>
                    {
                        new StageInfo(Stage.St1, Chara.Reimu,   Enumerable.Range(0, 2)),
                        new StageInfo(Stage.St2, Chara.Marisa,  Enumerable.Range(0, 2)),
                        new StageInfo(Stage.St3, Chara.Komachi, Enumerable.Range(0, 3)),
                        new StageInfo(Stage.St4, Chara.Aya,     Enumerable.Range(0, 3)),
                        new StageInfo(Stage.St5, Chara.Iku,     Enumerable.Range(0, 4)),
                        new StageInfo(Stage.St6, Chara.Tenshi,  Enumerable.Range(0, 5)),
                    }
                },
                {
                    Chara.Yukari,
                    new List<StageInfo>
                    {
                        new StageInfo(Stage.St1, Chara.Reimu,   Enumerable.Range(0, 2)),
                        new StageInfo(Stage.St2, Chara.Komachi, Enumerable.Range(0, 2)),
                        new StageInfo(Stage.St3, Chara.Marisa,  Enumerable.Range(0, 3)),
                        new StageInfo(Stage.St4, Chara.Iku,     Enumerable.Range(0, 3)),
                        new StageInfo(Stage.St5, Chara.Suika,   Enumerable.Range(0, 4)),
                        new StageInfo(Stage.St6, Chara.Tenshi,  Enumerable.Range(0, 5)),
                    }
                },
                {
                    Chara.Suika,
                    new List<StageInfo>
                    {
                        new StageInfo(Stage.St1, Chara.Aya,    Enumerable.Range(0, 4)),
                        new StageInfo(Stage.St2, Chara.Iku,    Enumerable.Range(0, 4)),
                        new StageInfo(Stage.St3, Chara.Tenshi, Enumerable.Range(0, 5)),
                    }
                },
                {
                    Chara.Reisen,
                    new List<StageInfo>
                    {
                        new StageInfo(Stage.St1, Chara.Remilia, Enumerable.Range(0, 3)),
                        new StageInfo(Stage.St2, Chara.Yuyuko,  Enumerable.Range(0, 3)),
                        new StageInfo(Stage.St3, Chara.Yukari,  Enumerable.Range(0, 4)),
                        new StageInfo(Stage.St4, Chara.Tenshi,  Enumerable.Range(0, 5)),
                    }
                },
                {
                    Chara.Aya,
                    new List<StageInfo>
                    {
                        new StageInfo(Stage.St1, Chara.Reimu,   Enumerable.Range(0, 2)),
                        new StageInfo(Stage.St2, Chara.Marisa,  Enumerable.Range(0, 3)),
                        new StageInfo(Stage.St3, Chara.Sakuya,  Enumerable.Range(0, 3)),
                        new StageInfo(Stage.St4, Chara.Remilia, Enumerable.Range(0, 4)),
                        new StageInfo(Stage.St5, Chara.Reimu,   Enumerable.Range(2, 3)),    // FIXME
                    }
                },
                {
                    Chara.Komachi,
                    new List<StageInfo>
                    {
                        new StageInfo(Stage.St1, Chara.Marisa, Enumerable.Range(0, 2)),
                        new StageInfo(Stage.St2, Chara.Sakuya, Enumerable.Range(0, 2)),
                        new StageInfo(Stage.St3, Chara.Youmu,  Enumerable.Range(0, 3)),
                        new StageInfo(Stage.St4, Chara.Reisen, Enumerable.Range(0, 3)),
                        new StageInfo(Stage.St5, Chara.Reimu,  Enumerable.Range(0, 4)),
                        new StageInfo(Stage.St6, Chara.Tenshi, Enumerable.Range(0, 5)),
                    }
                },
                {
                    Chara.Iku,
                    new List<StageInfo>
                    {
                        new StageInfo(Stage.St1, Chara.Suika,   Enumerable.Range(0, 2)),
                        new StageInfo(Stage.St2, Chara.Aya,     Enumerable.Range(0, 2)),
                        new StageInfo(Stage.St3, Chara.Remilia, Enumerable.Range(0, 3)),
                        new StageInfo(Stage.St4, Chara.Yuyuko,  Enumerable.Range(0, 3)),
                        new StageInfo(Stage.St5, Chara.Yukari,  Enumerable.Range(0, 4)),
                        new StageInfo(Stage.St6, Chara.Tenshi,  Enumerable.Range(0, 5)),
                    }
                },
                {
                    Chara.Tenshi,
                    new List<StageInfo>
                    {
                        new StageInfo(Stage.St1, Chara.Yuyuko,    Enumerable.Range(0, 2)),
                        new StageInfo(Stage.St2, Chara.Suika,     Enumerable.Range(0, 2)),
                        new StageInfo(Stage.St3, Chara.Patchouli, Enumerable.Range(0, 2)),
                        new StageInfo(Stage.St4, Chara.Alice,     Enumerable.Range(0, 3)),
                        new StageInfo(Stage.St5, Chara.Marisa,    Enumerable.Range(0, 3)),
                        new StageInfo(Stage.St6, Chara.Sakuya,    Enumerable.Range(0, 4)),
                        new StageInfo(Stage.St7, Chara.Youmu,     Enumerable.Range(0, 4)),
                        new StageInfo(Stage.St8, Chara.Reimu,     Enumerable.Range(0, 5)),
                    }
                },
            };

        private static readonly Dictionary<Chara, IEnumerable<(Chara Enemy, int CardId)>> EnemyCardIdTable =
            StageInfoTable.ToDictionary(
                stageInfoPair => stageInfoPair.Key,
                stageInfoPair => stageInfoPair.Value.SelectMany(
                    stageInfo => stageInfo.CardIds.Select(id => (stageInfo.Enemy, id))));

        private static new readonly EnumShortNameParser<LevelWithTotal> LevelWithTotalParser =
            new EnumShortNameParser<LevelWithTotal>();

        private static readonly EnumShortNameParser<Chara> CharaParser =
            new EnumShortNameParser<Chara>();

        private static readonly EnumShortNameParser<CardType> CardTypeParser =
            new EnumShortNameParser<CardType>();

        private AllScoreData allScoreData = null;

        public new enum Level
        {
#pragma warning disable SA1134 // Attributes should not share line
            [EnumAltName("E")] Easy,
            [EnumAltName("N")] Normal,
            [EnumAltName("H")] Hard,
            [EnumAltName("L")] Lunatic,
#pragma warning restore SA1134 // Attributes should not share line
        }

        public new enum LevelWithTotal
        {
#pragma warning disable SA1134 // Attributes should not share line
            [EnumAltName("E")] Easy,
            [EnumAltName("N")] Normal,
            [EnumAltName("H")] Hard,
            [EnumAltName("L")] Lunatic,
            [EnumAltName("T")] Total,
#pragma warning restore SA1134 // Attributes should not share line
        }

        public enum Chara
        {
#pragma warning disable SA1134 // Attributes should not share line
            [EnumAltName("RM")] Reimu,
            [EnumAltName("MR")] Marisa,
            [EnumAltName("SK")] Sakuya,
            [EnumAltName("AL")] Alice,
            [EnumAltName("PC")] Patchouli,
            [EnumAltName("YM")] Youmu,
            [EnumAltName("RL")] Remilia,
            [EnumAltName("YU")] Yuyuko,
            [EnumAltName("YK")] Yukari,
            [EnumAltName("SU")] Suika,
            [EnumAltName("RS")] Reisen,
            [EnumAltName("AY")] Aya,
            [EnumAltName("KM")] Komachi,
            [EnumAltName("IK")] Iku,
            [EnumAltName("TN")] Tenshi,
#pragma warning restore SA1134 // Attributes should not share line
        }

        public enum CharaWithTotal
        {
#pragma warning disable SA1134 // Attributes should not share line
            [EnumAltName("RM")] Reimu,
            [EnumAltName("MR")] Marisa,
            [EnumAltName("SK")] Sakuya,
            [EnumAltName("AL")] Alice,
            [EnumAltName("PC")] Patchouli,
            [EnumAltName("YM")] Youmu,
            [EnumAltName("RL")] Remilia,
            [EnumAltName("YU")] Yuyuko,
            [EnumAltName("YK")] Yukari,
            [EnumAltName("SU")] Suika,
            [EnumAltName("RS")] Reisen,
            [EnumAltName("AY")] Aya,
            [EnumAltName("KM")] Komachi,
            [EnumAltName("IK")] Iku,
            [EnumAltName("TN")] Tenshi,
            [EnumAltName("TL")] Total,
#pragma warning restore SA1134 // Attributes should not share line
        }

        public new enum Stage
        {
#pragma warning disable SA1134 // Attributes should not share line
            [EnumAltName("1")] St1,
            [EnumAltName("2")] St2,
            [EnumAltName("3")] St3,
            [EnumAltName("4")] St4,
            [EnumAltName("5")] St5,
            [EnumAltName("6")] St6,
            [EnumAltName("7")] St7,
            [EnumAltName("8")] St8,
#pragma warning restore SA1134 // Attributes should not share line
        }

        public enum CardType
        {
#pragma warning disable SA1134 // Attributes should not share line
            [EnumAltName("Y")] System,
            [EnumAltName("K")] Skill,
            [EnumAltName("P")] Spell,
#pragma warning restore SA1134 // Attributes should not share line
        }

        public override string SupportedVersions
        {
            get { return "1.06a"; }
        }

        protected override bool ReadScoreFile(Stream input)
        {
            using (var decrypted = new MemoryStream())
#if DEBUG
            using (var decoded = new FileStream("th105decoded.dat", FileMode.Create, FileAccess.ReadWrite))
#else
            using (var decoded = new MemoryStream())
#endif
            {
                if (!Decrypt(input, decrypted))
                    return false;

                decrypted.Seek(0, SeekOrigin.Begin);
                if (!Extract(decrypted, decoded))
                    return false;

                decoded.Seek(0, SeekOrigin.Begin);
                this.allScoreData = Read(decoded);

                return this.allScoreData != null;
            }
        }

        protected override IEnumerable<IStringReplaceable> CreateReplacers(
            bool hideUntriedCards, string outputFilePath)
        {
            return new List<IStringReplaceable>
            {
                new CareerReplacer(this),
                new CardReplacer(this, hideUntriedCards),
                new CollectRateReplacer(this),
                new CardForDeckReplacer(this, hideUntriedCards),
            };
        }

        private static bool Decrypt(Stream input, Stream output)
        {
            var size = (int)input.Length;
            var inData = new byte[size];
            var outData = new byte[size];

            input.Seek(0, SeekOrigin.Begin);
            input.Read(inData, 0, size);

            for (var index = 0; index < size; index++)
                outData[index] = (byte)((index * 7) ^ inData[size - index - 1]);

            output.Seek(0, SeekOrigin.Begin);
            output.Write(outData, 0, size);

            // See section 2.2 of RFC 1950
            return (outData[0] == 0x78) && (outData[1] == 0x9C);
        }

        private static bool Extract(Stream input, Stream output)
        {
            var extracted = new byte[0x80000];
            var extractedSize = 0;

            // Skip the header bytes of a zlib stream
            input.Seek(2, SeekOrigin.Begin);

            using (var deflate = new DeflateStream(input, CompressionMode.Decompress, true))
                extractedSize = deflate.Read(extracted, 0, extracted.Length);

            output.Seek(0, SeekOrigin.Begin);
            output.Write(extracted, 0, extractedSize);

            return true;
        }

        private static AllScoreData Read(Stream input)
        {
            using (var reader = new BinaryReader(input, Encoding.UTF8, true))
            {
                var allScoreData = new AllScoreData();

                try
                {
                    allScoreData.ReadFrom(reader);
                }
                catch (EndOfStreamException)
                {
                }

                var numCharas = Enum.GetValues(typeof(Chara)).Length;
                if (allScoreData.ClearData.Count == numCharas)
                    return allScoreData;
                else
                    return null;
            }
        }

        // serialNumber: 0-based
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
        private static (Chara Chara, int CardId)? GetCharaCardIdPair(Chara chara, CardType type, int serialNumber)
        {
            if (type == CardType.System)
                return null;

            Func<(Chara Chara, int CardId), bool> matchesCharaAndType;
            if (type == CardType.Skill)
                matchesCharaAndType = (pair => (pair.Chara == chara) && (pair.CardId / 100 == 1));
            else
                matchesCharaAndType = (pair => (pair.Chara == chara) && (pair.CardId / 100 == 2));

            return CardNameTable.Keys.Where(matchesCharaAndType).ElementAtOrDefault(serialNumber);
        }

        // %T105C[xxx][yy][z]
        private class CareerReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T105C(\d{{3}})({0})([1-3])", CharaParser.Pattern);

            private readonly MatchEvaluator evaluator;

            [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
            public CareerReplacer(Th105Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var number = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
                    var chara = CharaParser.Parse(match.Groups[2].Value);
                    var type = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

                    Func<SpellCardResult, long> getValue;
                    Func<long, string> toString;
                    if (type == 1)
                    {
                        getValue = (result => result.GotCount);
                        toString = Utils.ToNumberString;
                    }
                    else if (type == 2)
                    {
                        getValue = (result => result.TrialCount);
                        toString = Utils.ToNumberString;
                    }
                    else
                    {
                        getValue = (result => result.Frames);
                        toString = (value =>
                        {
                            var time = new Time(value);
                            return Utils.Format(
                                "{0:D2}:{1:D2}.{2:D3}",
                                (time.Hours * 60) + time.Minutes,
                                time.Seconds,
                                (time.Frames * 1000) / 60);
                        });
                    }

                    var clearData = parent.allScoreData.ClearData[chara];
                    if (number == 0)
                    {
                        return toString(clearData.SpellCardResults.Values.Sum(getValue));
                    }
                    else
                    {
                        var numLevels = Enum.GetValues(typeof(Level)).Length;
                        var index = (number - 1) / numLevels;
                        if ((index >= 0) && (index < EnemyCardIdTable[chara].Count()))
                        {
                            var (enemy, cardId) = EnemyCardIdTable[chara].ElementAt(index);
                            var key = (enemy, (cardId * numLevels) + ((number - 1) % numLevels));
                            return toString(getValue(clearData.SpellCardResults[key]));
                        }
                        else
                        {
                            return match.ToString();
                        }
                    }
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        // %T105CARD[xxx][yy][z]
        private class CardReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T105CARD(\d{{3}})({0})([NR])", CharaParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public CardReplacer(Th105Converter parent, bool hideUntriedCards)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var number = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
                    var chara = CharaParser.Parse(match.Groups[2].Value);
                    var type = match.Groups[3].Value.ToUpperInvariant();

                    if (number <= 0)
                        return match.ToString();

                    var numLevels = Enum.GetValues(typeof(Level)).Length;
                    var index = (number - 1) / numLevels;
                    if ((index >= 0) && (index < EnemyCardIdTable[chara].Count()))
                    {
                        var level = (Level)((number - 1) % numLevels);
                        var enemyCardIdPair = EnemyCardIdTable[chara].ElementAt(index);
                        if (hideUntriedCards)
                        {
                            var clearData = parent.allScoreData.ClearData[chara];
                            var key = (
                                enemyCardIdPair.Enemy,
                                (enemyCardIdPair.CardId * numLevels) + (int)level);
                            if (clearData.SpellCardResults[key].TrialCount <= 0)
                                return (type == "N") ? "??????????" : "?????";
                        }

                        return (type == "N") ? CardNameTable[enemyCardIdPair] : level.ToString();
                    }
                    else
                    {
                        return match.ToString();
                    }
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        // %T105CRG[x][yy][z]
        private class CollectRateReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T105CRG({0})({1})([1-2])",
                LevelWithTotalParser.Pattern,
                CharaParser.Pattern);

            private readonly MatchEvaluator evaluator;

            [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
            public CollectRateReplacer(Th105Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var level = LevelWithTotalParser.Parse(match.Groups[1].Value);
                    var chara = CharaParser.Parse(match.Groups[2].Value);
                    var type = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

                    Func<KeyValuePair<(Chara, int), SpellCardResult>, bool> findByLevel;
                    if (level == LevelWithTotal.Total)
                        findByLevel = (pair => true);
                    else
                        findByLevel = (pair => pair.Value.Level == (Level)level);

                    Func<KeyValuePair<(Chara, int), SpellCardResult>, bool> countByType;
                    if (type == 1)
                        countByType = (pair => pair.Value.GotCount > 0);
                    else
                        countByType = (pair => pair.Value.TrialCount > 0);

                    return Utils.ToNumberString(parent.allScoreData.ClearData[chara]
                        .SpellCardResults.Where(findByLevel).Count(countByType));
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        // %T105DC[ww][x][yy][z]
        private class CardForDeckReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T105DC({0})({1})(\d{{2}})([NC])", CharaParser.Pattern, CardTypeParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public CardForDeckReplacer(Th105Converter parent, bool hideUntriedCards)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var chara = CharaParser.Parse(match.Groups[1].Value);
                    var cardType = CardTypeParser.Parse(match.Groups[2].Value);
                    var number = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);
                    var type = match.Groups[4].Value.ToUpperInvariant();

                    if (cardType == CardType.System)
                    {
                        if (SystemCardNameTable.ContainsKey(number - 1))
                        {
                            var card = parent.allScoreData.SystemCards[number - 1];
                            if (type == "N")
                            {
                                if (hideUntriedCards)
                                {
                                    if (card.MaxNumber <= 0)
                                        return "??????????";
                                }

                                return SystemCardNameTable[number - 1];
                            }
                            else
                            {
                                return Utils.ToNumberString(card.MaxNumber);
                            }
                        }
                        else
                        {
                            return match.ToString();
                        }
                    }
                    else
                    {
                        var key = GetCharaCardIdPair(chara, cardType, number - 1);
                        if (key != null)
                        {
                            var card = parent.allScoreData.ClearData[key.Value.Chara].CardsForDeck[key.Value.CardId];
                            if (type == "N")
                            {
                                if (hideUntriedCards)
                                {
                                    if (card.MaxNumber <= 0)
                                        return "??????????";
                                }

                                return CardNameTable[key.Value];
                            }
                            else
                            {
                                return Utils.ToNumberString(card.MaxNumber);
                            }
                        }
                        else
                        {
                            return match.ToString();
                        }
                    }
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        private class AllScoreData : IBinaryReadable
        {
            public AllScoreData()
            {
                this.StoryClearCounts = new Dictionary<Chara, byte>(Enum.GetValues(typeof(Chara)).Length);
                this.SystemCards = null;
                this.ClearData = null;
            }

            public Dictionary<Chara, byte> StoryClearCounts { get; private set; }

            public Dictionary<int, Th105.CardForDeck> SystemCards { get; private set; }

            public Dictionary<Chara, ClearData> ClearData { get; private set; }

            public void ReadFrom(BinaryReader reader)
            {
                if (reader == null)
                    throw new ArgumentNullException(nameof(reader));

                var validNumCharas = Enum.GetValues(typeof(Chara)).Length;

                reader.ReadUInt32();            // version? (0x6A == 106 --> ver.1.06?)
                reader.ReadUInt32();

                for (var index = 0; index < 0x14; index++)
                {
                    var count = reader.ReadByte();
                    if (index < validNumCharas)
                    {
                        var chara = (Chara)index;
                        if (!this.StoryClearCounts.ContainsKey(chara))
                            this.StoryClearCounts.Add(chara, count);    // really...?
                    }
                }

                reader.ReadExactBytes(0x14);    // flags of story playable characters?
                reader.ReadExactBytes(0x14);    // flags of versus/arcade playable characters?

                var numBgmFlags = reader.ReadInt32();
                for (var index = 0; index < numBgmFlags; index++)
                    reader.ReadUInt32();        // signature of an unlocked bgm?

                var num = reader.ReadInt32();   // always 2?
                for (var index = 0; index < num; index++)
                    reader.ReadUInt32();        // always 0x0000000A and 0x0000000B?

                var numSystemCards = reader.ReadInt32();
                this.SystemCards = new Dictionary<int, Th105.CardForDeck>(numSystemCards);
                for (var index = 0; index < numSystemCards; index++)
                {
                    var card = new Th105.CardForDeck();
                    card.ReadFrom(reader);
                    if (!this.SystemCards.ContainsKey(card.Id))
                        this.SystemCards.Add(card.Id, card);
                }

                this.ClearData = new Dictionary<Chara, ClearData>(validNumCharas);
                var numCharas = reader.ReadInt32();
                for (var index = 0; index < numCharas; index++)
                {
                    var data = new ClearData();
                    data.ReadFrom(reader);
                    if (index < validNumCharas)
                    {
                        var chara = (Chara)index;
                        if (!this.ClearData.ContainsKey(chara))
                            this.ClearData.Add(chara, data);
                    }
                }
            }
        }
    }
}

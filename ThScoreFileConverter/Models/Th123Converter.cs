//-----------------------------------------------------------------------
// <copyright file="Th123Converter.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable 1591
#pragma warning disable SA1600 // ElementsMustBeDocumented
#pragma warning disable SA1602 // EnumerationItemsMustBeDocumented

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "*", Justification = "Reviewed.")]

namespace ThScoreFileConverter.Models
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Text.RegularExpressions;

    internal class Th123Converter : ThConverter
    {
        // Thanks to en.touhouwiki.net
        [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1025:CodeMustNotContainMultipleWhitespaceInARow", Justification = "Reviewed.")]
        private static readonly Dictionary<int, string> SystemCardNameTable =
            new Dictionary<int, string>
            {
                {  0, "「霊撃札」" },
                {  1, "「マジックポーション」" },
                {  2, "「ストップウォッチ」" },
                {  3, "「白楼剣」" },
                {  4, "「身代わり人形」" },
                {  5, "「グリモワール」" },
                {  6, "「特注の日傘」" },
                {  7, "「人魂灯」" },
                {  8, "「左扇」" },
                {  9, "「伊吹瓢」" },
                { 10, "「天狗団扇」" },
                { 11, "「符蝕薬」" },
                { 12, "「宵越しの銭」" },
                { 13, "「龍魚の羽衣」" },
                { 14, "「緋想の剣」" },
                { 15, "「病気平癒守」" },
                { 16, "「冷凍カエル」" },
                { 17, "「龍星」" },
                { 18, "「制御棒」" },
                { 19, "「三粒の天滴」" },
                { 20, "「ナマズの大地震」" },
            };

        // Thanks to th3_5154.txt, th123_ai, www.toho-motoneta.net and en.touhouwiki.net
        [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1025:CodeMustNotContainMultipleWhitespaceInARow", Justification = "Reviewed.")]
        private static readonly Dictionary<CharaCardIdPair, string> CardNameTable =
            new Dictionary<CharaCardIdPair, string>
            {
                { new CharaCardIdPair(Chara.Reimu,   0), "御守「妖怪足止めお守り」" },
                { new CharaCardIdPair(Chara.Reimu,   1), "霊符「夢想封印　円」" },
                { new CharaCardIdPair(Chara.Reimu,   2), "神霊「夢想封印　瞬」" },
                { new CharaCardIdPair(Chara.Reimu,   3), "「最も凶悪なびっくり巫女玉」" },
                { new CharaCardIdPair(Chara.Reimu, 100), "博麗アミュレット" },
                { new CharaCardIdPair(Chara.Reimu, 101), "警醒陣" },
                { new CharaCardIdPair(Chara.Reimu, 102), "亜空穴" },
                { new CharaCardIdPair(Chara.Reimu, 103), "昇天脚" },
                { new CharaCardIdPair(Chara.Reimu, 104), "妖怪バスター" },
                { new CharaCardIdPair(Chara.Reimu, 105), "繋縛陣" },
                { new CharaCardIdPair(Chara.Reimu, 106), "封魔亜空穴" },
                { new CharaCardIdPair(Chara.Reimu, 107), "抄地昇天脚" },
                { new CharaCardIdPair(Chara.Reimu, 108), "拡散アミュレット" },
                { new CharaCardIdPair(Chara.Reimu, 109), "常置陣" },
                { new CharaCardIdPair(Chara.Reimu, 110), "刹那亜空穴" },
                { new CharaCardIdPair(Chara.Reimu, 111), "雨乞祈り" },
                { new CharaCardIdPair(Chara.Reimu, 200), "霊符「夢想妙珠」" },
                { new CharaCardIdPair(Chara.Reimu, 201), "神霊「夢想封印」" },
                { new CharaCardIdPair(Chara.Reimu, 204), "夢符「封魔陣」" },
                { new CharaCardIdPair(Chara.Reimu, 206), "神技「八方鬼縛陣」" },
                { new CharaCardIdPair(Chara.Reimu, 207), "結界「拡散結界」" },
                { new CharaCardIdPair(Chara.Reimu, 208), "珠符「明珠暗投」" },
                { new CharaCardIdPair(Chara.Reimu, 209), "宝符「陰陽宝玉」" },
                { new CharaCardIdPair(Chara.Reimu, 210), "宝具「陰陽鬼神玉」" },
                { new CharaCardIdPair(Chara.Reimu, 214), "神技「天覇風神脚」" },
                { new CharaCardIdPair(Chara.Reimu, 219), "「夢想天生」" },
                { new CharaCardIdPair(Chara.Marisa,   0), "星符「エキセントリックアステロイド」" },
                { new CharaCardIdPair(Chara.Marisa,   1), "流光「シューティングエコー」" },
                { new CharaCardIdPair(Chara.Marisa,   2), "魔符「マジカルＲ３６０」" },
                { new CharaCardIdPair(Chara.Marisa, 100), "ウィッチレイライン" },
                { new CharaCardIdPair(Chara.Marisa, 101), "ミアズマスウィープ" },
                { new CharaCardIdPair(Chara.Marisa, 102), "グラウンドスターダスト" },
                { new CharaCardIdPair(Chara.Marisa, 103), "メテオニックデブリ" },
                { new CharaCardIdPair(Chara.Marisa, 104), "ラジアルストライク" },
                { new CharaCardIdPair(Chara.Marisa, 105), "バスキースウィーパー" },
                { new CharaCardIdPair(Chara.Marisa, 106), "デビルダムトーチ" },
                { new CharaCardIdPair(Chara.Marisa, 107), "ナロースパーク" },
                { new CharaCardIdPair(Chara.Marisa, 108), "アップスウィープ" },
                { new CharaCardIdPair(Chara.Marisa, 109), "ステラミサイル" },
                { new CharaCardIdPair(Chara.Marisa, 110), "マジカル産廃再利用ボム" },
                { new CharaCardIdPair(Chara.Marisa, 111), "グリーンスプレッド" },
                { new CharaCardIdPair(Chara.Marisa, 200), "恋符「マスタースパーク」" },
                { new CharaCardIdPair(Chara.Marisa, 202), "魔砲「ファイナルスパーク」" },
                { new CharaCardIdPair(Chara.Marisa, 203), "星符「ドラゴンメテオ」" },
                { new CharaCardIdPair(Chara.Marisa, 204), "恋符「ノンディレクショナルレーザー」" },
                { new CharaCardIdPair(Chara.Marisa, 205), "魔符「スターダストレヴァリエ」" },
                { new CharaCardIdPair(Chara.Marisa, 206), "星符「エスケープベロシティ」" },
                { new CharaCardIdPair(Chara.Marisa, 207), "彗星「ブレイジングスター」" },
                { new CharaCardIdPair(Chara.Marisa, 208), "星符「メテオニックシャワー」" },
                { new CharaCardIdPair(Chara.Marisa, 209), "星符「グラビティビート」" },
                { new CharaCardIdPair(Chara.Marisa, 211), "光符「ルミネスストライク」" },
                { new CharaCardIdPair(Chara.Marisa, 212), "光符「アースライトレイ」" },
                { new CharaCardIdPair(Chara.Marisa, 214), "魔廃「ディープエコロジカルボム」" },
                { new CharaCardIdPair(Chara.Marisa, 215), "儀符「オーレリーズサン」" },
                { new CharaCardIdPair(Chara.Marisa, 219), "邪恋「実りやすいマスタースパーク」" },
                { new CharaCardIdPair(Chara.Sakuya,   0), "銀符「シルバーバウンド」" },
                { new CharaCardIdPair(Chara.Sakuya,   1), "時符「咲夜特製ストップウォッチ」" },
                { new CharaCardIdPair(Chara.Sakuya,   2), "幻術「マイナイフリカージョン」" },
                { new CharaCardIdPair(Chara.Sakuya,   3), "時符「シルバーアキュート３６０」" },
                { new CharaCardIdPair(Chara.Sakuya, 100), "クロースアップマジック" },
                { new CharaCardIdPair(Chara.Sakuya, 101), "バウンスノーバウンス" },
                { new CharaCardIdPair(Chara.Sakuya, 102), "マジックスターソード" },
                { new CharaCardIdPair(Chara.Sakuya, 103), "バニシングエブリシング" },
                { new CharaCardIdPair(Chara.Sakuya, 104), "プロペリングシルバー" },
                { new CharaCardIdPair(Chara.Sakuya, 105), "スクウェアリコシェ" },
                { new CharaCardIdPair(Chara.Sakuya, 106), "離剣の見" },
                { new CharaCardIdPair(Chara.Sakuya, 107), "パーフェクトメイド" },
                { new CharaCardIdPair(Chara.Sakuya, 108), "ダンシングスターソード" },
                { new CharaCardIdPair(Chara.Sakuya, 109), "ミスディレクション" },
                { new CharaCardIdPair(Chara.Sakuya, 110), "パラレルブレーン" },
                { new CharaCardIdPair(Chara.Sakuya, 111), "タイムパラドックス" },
                { new CharaCardIdPair(Chara.Sakuya, 200), "幻符「殺人ドール」" },
                { new CharaCardIdPair(Chara.Sakuya, 201), "時符「プライベートスクウェア」" },
                { new CharaCardIdPair(Chara.Sakuya, 202), "傷符「インスクライブレッドソウル」" },
                { new CharaCardIdPair(Chara.Sakuya, 203), "幻葬「夜霧の幻影殺人鬼」" },
                { new CharaCardIdPair(Chara.Sakuya, 204), "「咲夜の世界」" },
                { new CharaCardIdPair(Chara.Sakuya, 205), "傷魂「ソウルスカルプチュア」" },
                { new CharaCardIdPair(Chara.Sakuya, 206), "銀符「シルバーバウンド」" },
                { new CharaCardIdPair(Chara.Sakuya, 207), "奇術「エターナルミーク」" },
                { new CharaCardIdPair(Chara.Sakuya, 208), "速符「ルミネスリコシェ」" },
                { new CharaCardIdPair(Chara.Sakuya, 209), "時符「咲夜特製ストップウォッチ」" },
                { new CharaCardIdPair(Chara.Sakuya, 210), "光速「Ｃ．リコシェ」" },
                { new CharaCardIdPair(Chara.Sakuya, 211), "時符「イマジナリバーチカルタイム」" },
                { new CharaCardIdPair(Chara.Sakuya, 212), "時計「ルナダイアル」" },
                { new CharaCardIdPair(Chara.Alice,   0), "人形「セミオートマトン」" },
                { new CharaCardIdPair(Chara.Alice,   1), "騎士「ドールオブラウンドテーブル」" },
                { new CharaCardIdPair(Chara.Alice,   2), "犠牲「スーサイドパクト」" },
                { new CharaCardIdPair(Chara.Alice,   3), "試験中「レベルティターニア」" },
                { new CharaCardIdPair(Chara.Alice,   4), "試験中「ゴリアテ人形」" },
                { new CharaCardIdPair(Chara.Alice, 100), "人形操創" },
                { new CharaCardIdPair(Chara.Alice, 101), "人形無操" },
                { new CharaCardIdPair(Chara.Alice, 102), "人形置操" },
                { new CharaCardIdPair(Chara.Alice, 103), "人形振起" },
                { new CharaCardIdPair(Chara.Alice, 104), "人形帰巣" },
                { new CharaCardIdPair(Chara.Alice, 105), "人形火葬" },
                { new CharaCardIdPair(Chara.Alice, 106), "人形千槍" },
                { new CharaCardIdPair(Chara.Alice, 107), "人形ＳＰ" },
                { new CharaCardIdPair(Chara.Alice, 108), "人形伏兵" },
                { new CharaCardIdPair(Chara.Alice, 109), "大江戸爆薬からくり人形" },
                { new CharaCardIdPair(Chara.Alice, 110), "人形弓兵" },
                { new CharaCardIdPair(Chara.Alice, 111), "シーカーワイヤー" },
                { new CharaCardIdPair(Chara.Alice, 200), "魔符「アーティフルサクリファイス」" },
                { new CharaCardIdPair(Chara.Alice, 201), "戦符「リトルレギオン」" },
                { new CharaCardIdPair(Chara.Alice, 202), "咒符「上海人形」" },
                { new CharaCardIdPair(Chara.Alice, 203), "魔操「リターンイナニメトネス」" },
                { new CharaCardIdPair(Chara.Alice, 204), "戦操「ドールズウォー」" },
                { new CharaCardIdPair(Chara.Alice, 205), "咒詛「蓬莱人形」" },
                { new CharaCardIdPair(Chara.Alice, 206), "偵符「シーカードールズ」" },
                { new CharaCardIdPair(Chara.Alice, 207), "紅符「和蘭人形」" },
                { new CharaCardIdPair(Chara.Alice, 208), "人形「未来文楽」" },
                { new CharaCardIdPair(Chara.Alice, 209), "注力「トリップワイヤー」" },
                { new CharaCardIdPair(Chara.Alice, 210), "槍符「キューティ大千槍」" },
                { new CharaCardIdPair(Chara.Alice, 211), "人形「レミングスパレード」" },
                { new CharaCardIdPair(Chara.Patchouli,   0), "火符「リングオブアグニ」" },
                { new CharaCardIdPair(Chara.Patchouli,   1), "水符「プリンセスウンディネ」" },
                { new CharaCardIdPair(Chara.Patchouli, 100), "サマーレッド" },
                { new CharaCardIdPair(Chara.Patchouli, 101), "ウィンターエレメント" },
                { new CharaCardIdPair(Chara.Patchouli, 102), "スプリングウィンド" },
                { new CharaCardIdPair(Chara.Patchouli, 103), "オータムエッジ" },
                { new CharaCardIdPair(Chara.Patchouli, 104), "ドヨースピア" },
                { new CharaCardIdPair(Chara.Patchouli, 105), "サマーフレイム" },
                { new CharaCardIdPair(Chara.Patchouli, 106), "コンデンスドバブル" },
                { new CharaCardIdPair(Chara.Patchouli, 107), "フラッシュオブスプリング" },
                { new CharaCardIdPair(Chara.Patchouli, 108), "オータムブレード" },
                { new CharaCardIdPair(Chara.Patchouli, 109), "エメラルドシティ" },
                { new CharaCardIdPair(Chara.Patchouli, 110), "ワイプモイスチャー" },
                { new CharaCardIdPair(Chara.Patchouli, 111), "スティッキーバブル" },
                { new CharaCardIdPair(Chara.Patchouli, 112), "スタティックグリーン" },
                { new CharaCardIdPair(Chara.Patchouli, 113), "フォールスラッシャー" },
                { new CharaCardIdPair(Chara.Patchouli, 114), "ダイアモンドハードネス" },
                { new CharaCardIdPair(Chara.Patchouli, 200), "火金符「セントエルモピラー」" },
                { new CharaCardIdPair(Chara.Patchouli, 201), "土水符「ノエキアンデリュージュ」" },
                { new CharaCardIdPair(Chara.Patchouli, 202), "金木符「エレメンタルハーベスター」" },
                { new CharaCardIdPair(Chara.Patchouli, 203), "日符「ロイヤルフレア」" },
                { new CharaCardIdPair(Chara.Patchouli, 204), "月符「サイレントセレナ」" },
                { new CharaCardIdPair(Chara.Patchouli, 205), "火水木金土符「賢者の石」" },
                { new CharaCardIdPair(Chara.Patchouli, 206), "水符「ジェリーフィッシュプリンセス」" },
                { new CharaCardIdPair(Chara.Patchouli, 207), "月木符「サテライトヒマワリ」" },
                { new CharaCardIdPair(Chara.Patchouli, 210), "日木符「フォトシンセシス」" },
                { new CharaCardIdPair(Chara.Patchouli, 211), "火水符「フロギスティックピラー」" },
                { new CharaCardIdPair(Chara.Patchouli, 212), "土金符「エメラルドメガロポリス」" },
                { new CharaCardIdPair(Chara.Patchouli, 213), "日月符「ロイヤルダイアモンドリング」" },
                { new CharaCardIdPair(Chara.Youmu,   0), "人智剣「天女返し」" },
                { new CharaCardIdPair(Chara.Youmu,   1), "桜花剣「閃々散華」" },
                { new CharaCardIdPair(Chara.Youmu,   2), "断想剣「草木成仏斬」" },
                { new CharaCardIdPair(Chara.Youmu,   3), "瞑斬「楼観から弾をも断つ心の眼」" },
                { new CharaCardIdPair(Chara.Youmu, 100), "反射下界斬" },
                { new CharaCardIdPair(Chara.Youmu, 101), "弦月斬" },
                { new CharaCardIdPair(Chara.Youmu, 102), "生死流転斬" },
                { new CharaCardIdPair(Chara.Youmu, 103), "憑坐の縛" },
                { new CharaCardIdPair(Chara.Youmu, 104), "結跏趺斬" },
                { new CharaCardIdPair(Chara.Youmu, 105), "折伏無間" },
                { new CharaCardIdPair(Chara.Youmu, 106), "心抄斬" },
                { new CharaCardIdPair(Chara.Youmu, 107), "悪し魂" },
                { new CharaCardIdPair(Chara.Youmu, 108), "燐気斬" },
                { new CharaCardIdPair(Chara.Youmu, 109), "炯眼剣" },
                { new CharaCardIdPair(Chara.Youmu, 110), "頭上花剪斬" },
                { new CharaCardIdPair(Chara.Youmu, 111), "奇び半身" },
                { new CharaCardIdPair(Chara.Youmu, 200), "人符「現世斬」" },
                { new CharaCardIdPair(Chara.Youmu, 201), "断命剣「冥想斬」" },
                { new CharaCardIdPair(Chara.Youmu, 202), "魂符「幽明の苦輪」" },
                { new CharaCardIdPair(Chara.Youmu, 203), "人鬼「未来永劫斬」" },
                { new CharaCardIdPair(Chara.Youmu, 204), "断迷剣「迷津慈航斬」" },
                { new CharaCardIdPair(Chara.Youmu, 205), "魂魄「幽明求聞持聡明の法」" },
                { new CharaCardIdPair(Chara.Youmu, 206), "剣伎「桜花閃々」" },
                { new CharaCardIdPair(Chara.Youmu, 207), "断霊剣「成仏得脱斬」" },
                { new CharaCardIdPair(Chara.Youmu, 208), "空観剣「六根清浄斬」" },
                { new CharaCardIdPair(Chara.Youmu, 212), "転生剣「円心流転斬」" },
                { new CharaCardIdPair(Chara.Remilia,   0), "運命「ミゼラブルフェイト」" },
                { new CharaCardIdPair(Chara.Remilia,   1), "夜符「ボンバードナイト」" },
                { new CharaCardIdPair(Chara.Remilia,   2), "蝙蝠「ヴァンパイアスウィープ」" },
                { new CharaCardIdPair(Chara.Remilia,   3), "神鬼「レミリアストーカー」" },
                { new CharaCardIdPair(Chara.Remilia, 100), "デーモンロードウォーク" },
                { new CharaCardIdPair(Chara.Remilia, 101), "サーヴァントフライヤー" },
                { new CharaCardIdPair(Chara.Remilia, 102), "デーモンロードクレイドル" },
                { new CharaCardIdPair(Chara.Remilia, 103), "デーモンロードアロー" },
                { new CharaCardIdPair(Chara.Remilia, 104), "ヴァンパイアクロウ" },
                { new CharaCardIdPair(Chara.Remilia, 105), "チェーンギャング" },
                { new CharaCardIdPair(Chara.Remilia, 106), "ロケットキックアップ" },
                { new CharaCardIdPair(Chara.Remilia, 107), "シーリングフィア" },
                { new CharaCardIdPair(Chara.Remilia, 108), "トリックスターデビル" },
                { new CharaCardIdPair(Chara.Remilia, 109), "デモンズディナーフォーク" },
                { new CharaCardIdPair(Chara.Remilia, 110), "バンパイアキス" },
                { new CharaCardIdPair(Chara.Remilia, 111), "スティグマナイザー" },
                { new CharaCardIdPair(Chara.Remilia, 200), "紅符「不夜城レッド」" },
                { new CharaCardIdPair(Chara.Remilia, 201), "必殺「ハートブレイク」" },
                { new CharaCardIdPair(Chara.Remilia, 202), "夜符「デーモンキングクレイドル」" },
                { new CharaCardIdPair(Chara.Remilia, 203), "紅魔「スカーレットデビル」" },
                { new CharaCardIdPair(Chara.Remilia, 204), "神槍「スピア・ザ・グングニル」" },
                { new CharaCardIdPair(Chara.Remilia, 205), "夜王「ドラキュラクレイドル」" },
                { new CharaCardIdPair(Chara.Remilia, 206), "夜符「バッドレディスクランブル」" },
                { new CharaCardIdPair(Chara.Remilia, 207), "運命「ミゼラブルフェイト」" },
                { new CharaCardIdPair(Chara.Remilia, 208), "「ミレニアムの吸血鬼」" },
                { new CharaCardIdPair(Chara.Remilia, 209), "悪魔「レミリアストレッチ」" },
                { new CharaCardIdPair(Chara.Yuyuko,   0), "幽符「冥界ミステリースポット」" },
                { new CharaCardIdPair(Chara.Yuyuko,   1), "霊蝶「蝶の羽風生に暫く」" },
                { new CharaCardIdPair(Chara.Yuyuko,   2), "宴会「死して全て大団円」" },
                { new CharaCardIdPair(Chara.Yuyuko, 100), "幽胡蝶" },
                { new CharaCardIdPair(Chara.Yuyuko, 101), "未生の光" },
                { new CharaCardIdPair(Chara.Yuyuko, 102), "悉皆彷徨" },
                { new CharaCardIdPair(Chara.Yuyuko, 103), "胡蝶夢の舞" },
                { new CharaCardIdPair(Chara.Yuyuko, 104), "好死の霊" },
                { new CharaCardIdPair(Chara.Yuyuko, 105), "鳳蝶紋の槍" },
                { new CharaCardIdPair(Chara.Yuyuko, 106), "誘霊の甘蜜" },
                { new CharaCardIdPair(Chara.Yuyuko, 107), "逆さ屏風" },
                { new CharaCardIdPair(Chara.Yuyuko, 108), "スフィアブルーム" },
                { new CharaCardIdPair(Chara.Yuyuko, 109), "死還の大地" },
                { new CharaCardIdPair(Chara.Yuyuko, 110), "故人のお届け物" },
                { new CharaCardIdPair(Chara.Yuyuko, 111), "センスオブエレガンス" },
                { new CharaCardIdPair(Chara.Yuyuko, 200), "死符「ギャストリドリーム」" },
                { new CharaCardIdPair(Chara.Yuyuko, 201), "冥符「黄泉平坂行路」" },
                { new CharaCardIdPair(Chara.Yuyuko, 202), "霊符「无寿の夢」" },
                { new CharaCardIdPair(Chara.Yuyuko, 203), "死蝶「華胥の永眠」" },
                { new CharaCardIdPair(Chara.Yuyuko, 204), "再迷「幻想郷の黄泉還り」" },
                { new CharaCardIdPair(Chara.Yuyuko, 205), "寿命「无寿国への約束手形」" },
                { new CharaCardIdPair(Chara.Yuyuko, 206), "霊蝶「蝶の羽風生に暫く」" },
                { new CharaCardIdPair(Chara.Yuyuko, 207), "蝶符「鳳蝶紋の死槍」" },
                { new CharaCardIdPair(Chara.Yuyuko, 208), "幽雅「死出の誘蛾灯」" },
                { new CharaCardIdPair(Chara.Yuyuko, 209), "桜符「センスオブチェリーブロッサム」" },
                { new CharaCardIdPair(Chara.Yuyuko, 219), "「反魂蝶」" },
                { new CharaCardIdPair(Chara.Yukari,   0), "光弾「ドップラーエフェクト」" },
                { new CharaCardIdPair(Chara.Yukari,   1), "捌器「全てを二つに別ける物」" },
                { new CharaCardIdPair(Chara.Yukari,   2), "幻巣「飛光虫ネスト」" },
                { new CharaCardIdPair(Chara.Yukari,   3), "空餌「狂躁高速飛行物体」" },
                { new CharaCardIdPair(Chara.Yukari,   4), "「八雲の巣」" },
                { new CharaCardIdPair(Chara.Yukari, 100), "開けて悔しき玉手箱" },
                { new CharaCardIdPair(Chara.Yukari, 101), "禅寺に潜む妖蝶" },
                { new CharaCardIdPair(Chara.Yukari, 102), "枕石漱流" },
                { new CharaCardIdPair(Chara.Yukari, 103), "幻想狂想穴" },
                { new CharaCardIdPair(Chara.Yukari, 104), "至る処に青山あり" },
                { new CharaCardIdPair(Chara.Yukari, 105), "幻想卍傘" },
                { new CharaCardIdPair(Chara.Yukari, 106), "物質と反物質の宇宙" },
                { new CharaCardIdPair(Chara.Yukari, 107), "肉体分解機" },
                { new CharaCardIdPair(Chara.Yukari, 108), "魅惑のエサ" },
                { new CharaCardIdPair(Chara.Yukari, 109), "知能と脚の境界" },
                { new CharaCardIdPair(Chara.Yukari, 110), "変容を見る眼" },
                { new CharaCardIdPair(Chara.Yukari, 111), "キマイラの翼" },
                { new CharaCardIdPair(Chara.Yukari, 200), "境符「四重結界」" },
                { new CharaCardIdPair(Chara.Yukari, 201), "式神「八雲藍」" },
                { new CharaCardIdPair(Chara.Yukari, 202), "境符「二次元と三次元の境界」" },
                { new CharaCardIdPair(Chara.Yukari, 203), "結界「魅力的な四重結界」" },
                { new CharaCardIdPair(Chara.Yukari, 204), "式神「橙」" },
                { new CharaCardIdPair(Chara.Yukari, 205), "結界「客観結界」" },
                { new CharaCardIdPair(Chara.Yukari, 206), "幻巣「飛光虫ネスト」" },
                { new CharaCardIdPair(Chara.Yukari, 207), "空餌「中毒性のあるエサ」" },
                { new CharaCardIdPair(Chara.Yukari, 208), "魔眼「ラプラスの魔」" },
                { new CharaCardIdPair(Chara.Yukari, 215), "廃線「ぶらり廃駅下車の旅」" },
                { new CharaCardIdPair(Chara.Suika,   0), "吐息「小鬼の深呼吸」" },
                { new CharaCardIdPair(Chara.Suika,   1), "火弾「地霊活性弾」" },
                { new CharaCardIdPair(Chara.Suika,   2), "鬼神「ミッシングパープルパワー」" },
                { new CharaCardIdPair(Chara.Suika,   3), "鬼気「濛々迷霧」" },
                { new CharaCardIdPair(Chara.Suika,   4), "「百万同一鬼」" },
                { new CharaCardIdPair(Chara.Suika, 100), "妖鬼-密-" },
                { new CharaCardIdPair(Chara.Suika, 101), "地霊-密-" },
                { new CharaCardIdPair(Chara.Suika, 102), "妖鬼-疎-" },
                { new CharaCardIdPair(Chara.Suika, 103), "萃鬼" },
                { new CharaCardIdPair(Chara.Suika, 104), "元鬼玉" },
                { new CharaCardIdPair(Chara.Suika, 105), "地霊-疎-" },
                { new CharaCardIdPair(Chara.Suika, 106), "厭霧" },
                { new CharaCardIdPair(Chara.Suika, 107), "疎鬼" },
                { new CharaCardIdPair(Chara.Suika, 108), "踏鞴" },
                { new CharaCardIdPair(Chara.Suika, 109), "火鬼" },
                { new CharaCardIdPair(Chara.Suika, 110), "鬼神燐火術" },
                { new CharaCardIdPair(Chara.Suika, 111), "攫鬼" },
                { new CharaCardIdPair(Chara.Suika, 200), "萃符「戸隠山投げ」" },
                { new CharaCardIdPair(Chara.Suika, 201), "酔神「鬼縛りの術」" },
                { new CharaCardIdPair(Chara.Suika, 202), "鬼符「ミッシングパワー」" },
                { new CharaCardIdPair(Chara.Suika, 203), "萃鬼「天手力男投げ」" },
                { new CharaCardIdPair(Chara.Suika, 204), "酔夢「施餓鬼縛りの術」" },
                { new CharaCardIdPair(Chara.Suika, 205), "鬼神「ミッシングパープルパワー」" },
                { new CharaCardIdPair(Chara.Suika, 206), "霧符「雲集霧散」" },
                { new CharaCardIdPair(Chara.Suika, 207), "鬼火「超高密度燐禍術」" },
                { new CharaCardIdPair(Chara.Suika, 208), "鬼符「大江山悉皆殺し」" },
                { new CharaCardIdPair(Chara.Suika, 212), "四天王奥義「三歩壊廃」" },
                { new CharaCardIdPair(Chara.Reisen,   0), "迫符「脅迫幻覚(オブセッショナー)」" },
                { new CharaCardIdPair(Chara.Reisen,   1), "幻弾「幻想視差(ブラフバラージ)」" },
                { new CharaCardIdPair(Chara.Reisen,   2), "幻兎「平行交差(パラレルクロス)」" },
                { new CharaCardIdPair(Chara.Reisen, 100), "マインドエクスプロージョン" },
                { new CharaCardIdPair(Chara.Reisen, 101), "イリュージョナリィブラスト" },
                { new CharaCardIdPair(Chara.Reisen, 102), "フィールドウルトラレッド" },
                { new CharaCardIdPair(Chara.Reisen, 103), "ディスビリーフアスペクト" },
                { new CharaCardIdPair(Chara.Reisen, 104), "マインドベンディング" },
                { new CharaCardIdPair(Chara.Reisen, 105), "アイサイトクリーニング" },
                { new CharaCardIdPair(Chara.Reisen, 106), "フィールドウルトラバイオレット" },
                { new CharaCardIdPair(Chara.Reisen, 107), "ディスオーダーアイ" },
                { new CharaCardIdPair(Chara.Reisen, 108), "マインドドロッピング" },
                { new CharaCardIdPair(Chara.Reisen, 109), "リップルヴィジョン" },
                { new CharaCardIdPair(Chara.Reisen, 110), "アンダーセンスブレイク" },
                { new CharaCardIdPair(Chara.Reisen, 111), "アキュラースペクトル" },
                { new CharaCardIdPair(Chara.Reisen, 200), "惑視「離円花冠(カローラヴィジョン)」" },
                { new CharaCardIdPair(Chara.Reisen, 202), "幻爆「近眼花火(マインドスターマイン)」" },
                { new CharaCardIdPair(Chara.Reisen, 203), "幻惑「花冠視線(クラウンヴィジョン)」" },
                { new CharaCardIdPair(Chara.Reisen, 204), "赤眼「望見円月(ルナティックブラスト)」" },
                { new CharaCardIdPair(Chara.Reisen, 205), "「幻朧月睨(ルナティックレッドアイズ)」" },
                { new CharaCardIdPair(Chara.Reisen, 206), "弱心「喪心喪意(ディモチヴェイション)」" },
                { new CharaCardIdPair(Chara.Reisen, 207), "喪心「喪心創痍(ディスカーダー)」" },
                { new CharaCardIdPair(Chara.Reisen, 208), "毒煙幕「瓦斯織物の玉」" },
                { new CharaCardIdPair(Chara.Reisen, 209), "生薬「国士無双の薬」" },
                { new CharaCardIdPair(Chara.Reisen, 210), "短視「超短脳波(エックスウェイブ)」" },
                { new CharaCardIdPair(Chara.Reisen, 211), "長視「赤月下(インフレアドムーン)」" },
                { new CharaCardIdPair(Chara.Aya,   0), "突風「猿田彦の先導」" },
                { new CharaCardIdPair(Chara.Aya,   1), "旋符「飄妖扇」" },
                { new CharaCardIdPair(Chara.Aya,   2), "旋風「鳥居つむじ風」" },
                { new CharaCardIdPair(Chara.Aya,   3), "「幻想風靡」" },
                { new CharaCardIdPair(Chara.Aya, 100), "疾風扇" },
                { new CharaCardIdPair(Chara.Aya, 101), "疾走風靡" },
                { new CharaCardIdPair(Chara.Aya, 102), "天狗の立風露" },
                { new CharaCardIdPair(Chara.Aya, 103), "暗夜の礫" },
                { new CharaCardIdPair(Chara.Aya, 104), "烈風扇" },
                { new CharaCardIdPair(Chara.Aya, 105), "疾走優美" },
                { new CharaCardIdPair(Chara.Aya, 106), "天狗のダウンバースト" },
                { new CharaCardIdPair(Chara.Aya, 107), "鎌風ベーリング" },
                { new CharaCardIdPair(Chara.Aya, 108), "楓扇風" },
                { new CharaCardIdPair(Chara.Aya, 109), "天狗ナメシ" },
                { new CharaCardIdPair(Chara.Aya, 110), "天狗の太鼓" },
                { new CharaCardIdPair(Chara.Aya, 111), "天狗礫" },
                { new CharaCardIdPair(Chara.Aya, 200), "旋符「紅葉扇風」" },
                { new CharaCardIdPair(Chara.Aya, 201), "竜巻「天孫降臨の道しるべ」" },
                { new CharaCardIdPair(Chara.Aya, 202), "逆風「人間禁制の道」" },
                { new CharaCardIdPair(Chara.Aya, 203), "突符「天狗のマクロバースト」" },
                { new CharaCardIdPair(Chara.Aya, 205), "風符「天狗道の開風」" },
                { new CharaCardIdPair(Chara.Aya, 206), "「幻想風靡」" },
                { new CharaCardIdPair(Chara.Aya, 207), "風符「天狗報即日限」" },
                { new CharaCardIdPair(Chara.Aya, 208), "鴉符「暗夜のデイメア」" },
                { new CharaCardIdPair(Chara.Aya, 211), "魔獣「鎌鼬ベーリング」" },
                { new CharaCardIdPair(Chara.Aya, 212), "突風「猿田彦の先導」" },
                { new CharaCardIdPair(Chara.Komachi,   0), "霊符「古き地縛霊の目覚め」" },
                { new CharaCardIdPair(Chara.Komachi,   1), "死符「死者選別の鎌」" },
                { new CharaCardIdPair(Chara.Komachi,   2), "魂符「魂の遊戯」" },
                { new CharaCardIdPair(Chara.Komachi, 100), "迷わず生きた人霊" },
                { new CharaCardIdPair(Chara.Komachi, 101), "浮かばれない地縛霊" },
                { new CharaCardIdPair(Chara.Komachi, 102), "脱魂の儀" },
                { new CharaCardIdPair(Chara.Komachi, 103), "怠惰に生きた浮遊霊" },
                { new CharaCardIdPair(Chara.Komachi, 104), "死神の大鎌" },
                { new CharaCardIdPair(Chara.Komachi, 105), "死出の風" },
                { new CharaCardIdPair(Chara.Komachi, 106), "無間の道" },
                { new CharaCardIdPair(Chara.Komachi, 107), "寂しがり屋の緊縛霊" },
                { new CharaCardIdPair(Chara.Komachi, 108), "通りすがりの人霊" },
                { new CharaCardIdPair(Chara.Komachi, 109), "三途の舟" },
                { new CharaCardIdPair(Chara.Komachi, 110), "お迎え体験版" },
                { new CharaCardIdPair(Chara.Komachi, 111), "離魂の鎌" },
                { new CharaCardIdPair(Chara.Komachi, 200), "舟符「河の流れのように」" },
                { new CharaCardIdPair(Chara.Komachi, 201), "薄命「余命幾許も無し」" },
                { new CharaCardIdPair(Chara.Komachi, 202), "霊符「何処にでもいる浮遊霊」" },
                { new CharaCardIdPair(Chara.Komachi, 203), "死歌「八重霧の渡し」" },
                { new CharaCardIdPair(Chara.Komachi, 204), "換命「不惜身命、可惜身命」" },
                { new CharaCardIdPair(Chara.Komachi, 205), "恨符「未練がましい緊縛霊」" },
                { new CharaCardIdPair(Chara.Komachi, 206), "死符「死者選別の鎌」" },
                { new CharaCardIdPair(Chara.Komachi, 207), "魂符「生魂流離の鎌」" },
                { new CharaCardIdPair(Chara.Komachi, 211), "地獄「無間の狭間」" },
                { new CharaCardIdPair(Chara.Iku,   0), "光珠「龍の光る眼」" },
                { new CharaCardIdPair(Chara.Iku,   1), "雷符「神鳴り様の住処」" },
                { new CharaCardIdPair(Chara.Iku,   2), "棘符「雷雲棘魚」" },
                { new CharaCardIdPair(Chara.Iku,   3), "雲界「玄雲海の雷庭」" },
                { new CharaCardIdPair(Chara.Iku, 100), "龍魚の一撃" },
                { new CharaCardIdPair(Chara.Iku, 101), "羽衣は水の如く" },
                { new CharaCardIdPair(Chara.Iku, 102), "龍魚の怒り" },
                { new CharaCardIdPair(Chara.Iku, 103), "静電誘導弾" },
                { new CharaCardIdPair(Chara.Iku, 104), "龍神の一撃" },
                { new CharaCardIdPair(Chara.Iku, 105), "羽衣は風の如く" },
                { new CharaCardIdPair(Chara.Iku, 106), "龍神の怒り" },
                { new CharaCardIdPair(Chara.Iku, 107), "龍神の稲光り" },
                { new CharaCardIdPair(Chara.Iku, 108), "水得の龍魚" },
                { new CharaCardIdPair(Chara.Iku, 109), "天女の一撃" },
                { new CharaCardIdPair(Chara.Iku, 110), "龍神の髭" },
                { new CharaCardIdPair(Chara.Iku, 111), "龍の眼" },
                { new CharaCardIdPair(Chara.Iku, 200), "電符「雷鼓弾」" },
                { new CharaCardIdPair(Chara.Iku, 201), "魚符「龍魚ドリル」" },
                { new CharaCardIdPair(Chara.Iku, 202), "雷符「エレキテルの龍宮」" },
                { new CharaCardIdPair(Chara.Iku, 203), "光星「光龍の吐息」" },
                { new CharaCardIdPair(Chara.Iku, 206), "雷魚「雷雲魚遊泳弾」" },
                { new CharaCardIdPair(Chara.Iku, 207), "羽衣「羽衣は空の如く」" },
                { new CharaCardIdPair(Chara.Iku, 208), "羽衣「羽衣は時の如く」" },
                { new CharaCardIdPair(Chara.Iku, 209), "棘符「雷雲棘魚」" },
                { new CharaCardIdPair(Chara.Iku, 210), "龍魚「龍宮の使い遊泳弾」" },
                { new CharaCardIdPair(Chara.Iku, 211), "珠符「五爪龍の珠」" },
                { new CharaCardIdPair(Chara.Tenshi,   0), "要石「天空の霊石」" },
                { new CharaCardIdPair(Chara.Tenshi,   1), "乾坤「荒々しくも母なる大地よ」" },
                { new CharaCardIdPair(Chara.Tenshi,   2), "霊想「大地を鎮める石」" },
                { new CharaCardIdPair(Chara.Tenshi,   3), "天地「世界を見下ろす遥かなる大地よ」" },
                { new CharaCardIdPair(Chara.Tenshi,   4), "「全人類の緋想天」" },
                { new CharaCardIdPair(Chara.Tenshi, 100), "坤儀の剣" },
                { new CharaCardIdPair(Chara.Tenshi, 101), "天罰の石柱" },
                { new CharaCardIdPair(Chara.Tenshi, 102), "非想の威光" },
                { new CharaCardIdPair(Chara.Tenshi, 103), "非想の剣" },
                { new CharaCardIdPair(Chara.Tenshi, 104), "六震-相-" },
                { new CharaCardIdPair(Chara.Tenshi, 105), "守りの要" },
                { new CharaCardIdPair(Chara.Tenshi, 106), "天地プレス" },
                { new CharaCardIdPair(Chara.Tenshi, 107), "緋想の剣" },
                { new CharaCardIdPair(Chara.Tenshi, 108), "因果の剣" },
                { new CharaCardIdPair(Chara.Tenshi, 109), "地精の起床" },
                { new CharaCardIdPair(Chara.Tenshi, 110), "緋想の剣気" },
                { new CharaCardIdPair(Chara.Tenshi, 111), "昇天突" },
                { new CharaCardIdPair(Chara.Tenshi, 200), "地符「不譲土壌の剣」" },
                { new CharaCardIdPair(Chara.Tenshi, 201), "非想「非想非非想の剣」" },
                { new CharaCardIdPair(Chara.Tenshi, 202), "天符「天道是非の剣」" },
                { new CharaCardIdPair(Chara.Tenshi, 203), "地震「先憂後楽の剣」" },
                { new CharaCardIdPair(Chara.Tenshi, 204), "気符「天啓気象の剣」" },
                { new CharaCardIdPair(Chara.Tenshi, 205), "要石「天地開闢プレス」" },
                { new CharaCardIdPair(Chara.Tenshi, 206), "気符「無念無想の境地」" },
                { new CharaCardIdPair(Chara.Tenshi, 207), "「全人類の緋想天」" },
                { new CharaCardIdPair(Chara.Tenshi, 208), "剣技「気炎万丈の剣」" },
                { new CharaCardIdPair(Chara.Tenshi, 209), "天気「緋想天促」" },
                { new CharaCardIdPair(Chara.Sanae,   0), "蛇符「雲を泳ぐ大蛇」" },
                { new CharaCardIdPair(Chara.Sanae,   1), "奇跡「ファフロッキーズの奇跡」" },
                { new CharaCardIdPair(Chara.Sanae, 100), "風起こし" },
                { new CharaCardIdPair(Chara.Sanae, 101), "おみくじ爆弾" },
                { new CharaCardIdPair(Chara.Sanae, 102), "乾神招来　突" },
                { new CharaCardIdPair(Chara.Sanae, 103), "坤神招来　盾" },
                { new CharaCardIdPair(Chara.Sanae, 104), "波起こし" },
                { new CharaCardIdPair(Chara.Sanae, 105), "スカイサーペント" },
                { new CharaCardIdPair(Chara.Sanae, 106), "乾神招来　風" },
                { new CharaCardIdPair(Chara.Sanae, 107), "坤神招来　鉄輪" },
                { new CharaCardIdPair(Chara.Sanae, 108), "星落とし" },
                { new CharaCardIdPair(Chara.Sanae, 109), "コバルトスプレッド" },
                { new CharaCardIdPair(Chara.Sanae, 110), "乾神招来　御柱" },
                { new CharaCardIdPair(Chara.Sanae, 111), "坤神招来　罠" },
                { new CharaCardIdPair(Chara.Sanae, 200), "祈願「商売繁盛守り」" },
                { new CharaCardIdPair(Chara.Sanae, 201), "秘術「グレイソーマタージ」" },
                { new CharaCardIdPair(Chara.Sanae, 202), "秘術「忘却の祭儀」" },
                { new CharaCardIdPair(Chara.Sanae, 203), "神籤「乱れおみくじ連続引き」" },
                { new CharaCardIdPair(Chara.Sanae, 204), "開海「海が割れる日」" },
                { new CharaCardIdPair(Chara.Sanae, 205), "開海「モーゼの奇跡」" },
                { new CharaCardIdPair(Chara.Sanae, 206), "奇跡「白昼の客星」" },
                { new CharaCardIdPair(Chara.Sanae, 207), "奇跡「客星の明るすぎる夜」" },
                { new CharaCardIdPair(Chara.Sanae, 210), "秘法「九字刺し」" },
                { new CharaCardIdPair(Chara.Cirno,   0), "氷符「アイシクルフォール」" },
                { new CharaCardIdPair(Chara.Cirno,   1), "凍符「マイナスＫ」" },
                { new CharaCardIdPair(Chara.Cirno, 100), "アイシクルシュート" },
                { new CharaCardIdPair(Chara.Cirno, 101), "真夏のスノーマン" },
                { new CharaCardIdPair(Chara.Cirno, 102), "リトルアイスバーグ" },
                { new CharaCardIdPair(Chara.Cirno, 103), "フリーズタッチミー" },
                { new CharaCardIdPair(Chara.Cirno, 104), "フロストピラーズ" },
                { new CharaCardIdPair(Chara.Cirno, 105), "アイスチャージ" },
                { new CharaCardIdPair(Chara.Cirno, 106), "アイシクルボム" },
                { new CharaCardIdPair(Chara.Cirno, 107), "アイシクルライズ" },
                { new CharaCardIdPair(Chara.Cirno, 108), "冷凍光線" },
                { new CharaCardIdPair(Chara.Cirno, 109), "アイスキック" },
                { new CharaCardIdPair(Chara.Cirno, 110), "フローズン冷凍法" },
                { new CharaCardIdPair(Chara.Cirno, 111), "アイシクルソード" },
                { new CharaCardIdPair(Chara.Cirno, 200), "氷符「アイシクルマシンガン」" },
                { new CharaCardIdPair(Chara.Cirno, 201), "霜符「フロストコラムス」" },
                { new CharaCardIdPair(Chara.Cirno, 202), "氷塊「コールドスプリンクラー」" },
                { new CharaCardIdPair(Chara.Cirno, 203), "冷体「スーパーアイスキック」" },
                { new CharaCardIdPair(Chara.Cirno, 204), "凍符「パーフェクトフリーズ」" },
                { new CharaCardIdPair(Chara.Cirno, 205), "氷符「フェアリースピン」" },
                { new CharaCardIdPair(Chara.Cirno, 206), "吹氷「アイストルネード」" },
                { new CharaCardIdPair(Chara.Cirno, 207), "氷符「ソードフリーザー」" },
                { new CharaCardIdPair(Chara.Cirno, 208), "氷塊「グレートクラッシャー」" },
                { new CharaCardIdPair(Chara.Cirno, 210), "凍符「フリーズアトモスフェア」" },
                { new CharaCardIdPair(Chara.Cirno, 213), "冷符「瞬間冷凍ビーム」" },
                { new CharaCardIdPair(Chara.Meiling,   0), "彩翔「飛花落葉」" },
                { new CharaCardIdPair(Chara.Meiling,   1), "彩符「極彩沛雨」" },
                { new CharaCardIdPair(Chara.Meiling, 100), "螺光歩" },
                { new CharaCardIdPair(Chara.Meiling, 101), "紅砲" },
                { new CharaCardIdPair(Chara.Meiling, 102), "黄震脚" },
                { new CharaCardIdPair(Chara.Meiling, 103), "芳波" },
                { new CharaCardIdPair(Chara.Meiling, 104), "烈虹拳" },
                { new CharaCardIdPair(Chara.Meiling, 105), "紅寸剄" },
                { new CharaCardIdPair(Chara.Meiling, 106), "地龍波" },
                { new CharaCardIdPair(Chara.Meiling, 107), "水形太極拳" },
                { new CharaCardIdPair(Chara.Meiling, 108), "降華蹴" },
                { new CharaCardIdPair(Chara.Meiling, 109), "虎剄" },
                { new CharaCardIdPair(Chara.Meiling, 110), "天龍脚" },
                { new CharaCardIdPair(Chara.Meiling, 111), "彩雨" },
                { new CharaCardIdPair(Chara.Meiling, 200), "彩符「彩光風鈴」" },
                { new CharaCardIdPair(Chara.Meiling, 201), "極彩「彩光乱舞」" },
                { new CharaCardIdPair(Chara.Meiling, 202), "気符「星脈弾」" },
                { new CharaCardIdPair(Chara.Meiling, 203), "星気「星脈地転弾」" },
                { new CharaCardIdPair(Chara.Meiling, 204), "撃符「大鵬拳」" },
                { new CharaCardIdPair(Chara.Meiling, 205), "熾撃「大鵬墜撃拳」" },
                { new CharaCardIdPair(Chara.Meiling, 206), "虹符「烈虹真拳」" },
                { new CharaCardIdPair(Chara.Meiling, 207), "気符「地龍天龍脚」" },
                { new CharaCardIdPair(Chara.Meiling, 208), "彩華「虹色太極拳」" },
                { new CharaCardIdPair(Chara.Meiling, 209), "華符「彩光蓮華掌」" },
                { new CharaCardIdPair(Chara.Meiling, 211), "気符「猛虎内剄」" },
                { new CharaCardIdPair(Chara.Utsuho,   0), "熱符「ブレイクプロミネンス」" },
                { new CharaCardIdPair(Chara.Utsuho,   1), "核熱「核反応制御不能ダイブ」" },
                { new CharaCardIdPair(Chara.Utsuho,   2), "「ホットジュピター落下モデル」" },
                { new CharaCardIdPair(Chara.Utsuho,   3), "核熱「人工太陽の黒点」" },
                { new CharaCardIdPair(Chara.Utsuho, 100), "フレアアップ" },
                { new CharaCardIdPair(Chara.Utsuho, 101), "グラウンドメルト" },
                { new CharaCardIdPair(Chara.Utsuho, 102), "ブレイクサン" },
                { new CharaCardIdPair(Chara.Utsuho, 103), "シューティングスター" },
                { new CharaCardIdPair(Chara.Utsuho, 104), "ロケットダイブ" },
                { new CharaCardIdPair(Chara.Utsuho, 105), "ヘルゲイザー" },
                { new CharaCardIdPair(Chara.Utsuho, 106), "シューティングサン" },
                { new CharaCardIdPair(Chara.Utsuho, 107), "レトロ原子核モデル" },
                { new CharaCardIdPair(Chara.Utsuho, 108), "メルティング浴びせ蹴り" },
                { new CharaCardIdPair(Chara.Utsuho, 109), "レイディアントブレード" },
                { new CharaCardIdPair(Chara.Utsuho, 110), "地獄波動砲" },
                { new CharaCardIdPair(Chara.Utsuho, 111), "核熱の怨霊" },
                { new CharaCardIdPair(Chara.Utsuho, 200), "爆符「メガフレア」" },
                { new CharaCardIdPair(Chara.Utsuho, 201), "爆符「ギガフレア」" },
                { new CharaCardIdPair(Chara.Utsuho, 203), "焔星「フィクストスター」" },
                { new CharaCardIdPair(Chara.Utsuho, 204), "焔星「十凶星」" },
                { new CharaCardIdPair(Chara.Utsuho, 205), "核符「クリーピングサン」" },
                { new CharaCardIdPair(Chara.Utsuho, 206), "「地獄の人工太陽」" },
                { new CharaCardIdPair(Chara.Utsuho, 207), "地熱「核ブレイズゲイザー」" },
                { new CharaCardIdPair(Chara.Utsuho, 208), "光熱「ハイテンションブレード」" },
                { new CharaCardIdPair(Chara.Utsuho, 209), "鴉符「八咫烏ダイブ」" },
                { new CharaCardIdPair(Chara.Utsuho, 210), "核熱「核反応制御不能ダイブ」" },
                { new CharaCardIdPair(Chara.Utsuho, 211), "制御「セルフトカマク」" },
                { new CharaCardIdPair(Chara.Utsuho, 212), "「サブタレイニアンサン」" },
                { new CharaCardIdPair(Chara.Utsuho, 213), "遮光「核熱バイザー」" },
                { new CharaCardIdPair(Chara.Utsuho, 214), "「アビスノヴァ」" },
                { new CharaCardIdPair(Chara.Suwako,   0), "合掌「だいだらぼっちの参拝」" },
                { new CharaCardIdPair(Chara.Suwako,   1), "神具「洩矢の鉄の輪」" },
                { new CharaCardIdPair(Chara.Suwako,   2), "「獄熱の間欠泉」" },
                { new CharaCardIdPair(Chara.Suwako,   3), "「マグマの両生類」" },
                { new CharaCardIdPair(Chara.Suwako,   4), "「幻想郷空中神戦」" },
                { new CharaCardIdPair(Chara.Suwako, 100), "古の間欠泉" },
                { new CharaCardIdPair(Chara.Suwako, 101), "大蝦蟇神" },
                { new CharaCardIdPair(Chara.Suwako, 102), "大地の湖" },
                { new CharaCardIdPair(Chara.Suwako, 103), "土着神の祟り" },
                { new CharaCardIdPair(Chara.Suwako, 104), "水蛙神" },
                { new CharaCardIdPair(Chara.Suwako, 105), "古の鉄輪" },
                { new CharaCardIdPair(Chara.Suwako, 106), "古代翡翠" },
                { new CharaCardIdPair(Chara.Suwako, 107), "祟られた大地" },
                { new CharaCardIdPair(Chara.Suwako, 108), "雨を呼ぶ雨蛙" },
                { new CharaCardIdPair(Chara.Suwako, 109), "手長足長さん" },
                { new CharaCardIdPair(Chara.Suwako, 110), "蛙石神" },
                { new CharaCardIdPair(Chara.Suwako, 111), "ミシャグジさまの祟り" },
                { new CharaCardIdPair(Chara.Suwako, 200), "土着神「洩矢神」" },
                { new CharaCardIdPair(Chara.Suwako, 201), "源符「諏訪清水」" },
                { new CharaCardIdPair(Chara.Suwako, 202), "開宴「二拝二拍一拝」" },
                { new CharaCardIdPair(Chara.Suwako, 203), "土着神「ケロちゃん風雨に負けず」" },
                { new CharaCardIdPair(Chara.Suwako, 204), "神具「洩矢の鉄の輪」" },
                { new CharaCardIdPair(Chara.Suwako, 205), "源符「厭い川の翡翠」" },
                { new CharaCardIdPair(Chara.Suwako, 206), "蛙狩「蛙は口ゆえ蛇に呑まるる」" },
                { new CharaCardIdPair(Chara.Suwako, 207), "土着神「手長足長さま」" },
                { new CharaCardIdPair(Chara.Suwako, 208), "祟り神「赤口（ミシャグチ）さま」" },
                { new CharaCardIdPair(Chara.Suwako, 209), "土着神「宝永四年の赤蛙」" },
                { new CharaCardIdPair(Chara.Suwako, 212), "蛙休「オールウェイズ冬眠できます」" },
                { new CharaCardIdPair(Chara.Oonamazu, 0), "ナマズ「ほらほら世界が震えるぞ？」" },
                { new CharaCardIdPair(Chara.Oonamazu, 1), "ナマズ「液状化現象で大地も泥のようじゃ！」" },
                { new CharaCardIdPair(Chara.Oonamazu, 2), "ナマズ「発電だって頑張っちゃうぞ？」" },
                { new CharaCardIdPair(Chara.Oonamazu, 3), "ナマズ「オール電化でエコロジーじゃ！」" },
                { new CharaCardIdPair(Chara.Oonamazu, 4), "大ナマズ「これで浮き世もおしまいじゃあ！」" },
            };

        private static readonly Dictionary<Chara, Dictionary<CardType, int[]>> CardOrderTable =
            new Dictionary<Chara, Dictionary<CardType, int[]>>
            {
                {
                    Chara.Reimu,
                    new Dictionary<CardType, int[]>
                    {
                        {
                            CardType.Skill,
                            new int[] { 100, 104, 108, 103, 107, 111, 101, 105, 109, 102, 106, 110 }
                        },
                        {
                            CardType.Spell,
                            new int[] { 208, 200, 204, 209, 207, 214, 206, 210, 201, 219 }
                        },
                    }
                },
                {
                    Chara.Marisa,
                    new Dictionary<CardType, int[]>
                    {
                        {
                            CardType.Skill,
                            new int[] { 103, 107, 111, 101, 105, 109, 100, 104, 108, 102, 106, 110 }
                        },
                        {
                            CardType.Spell,
                            new int[] { 208, 205, 211, 215, 200, 206, 209, 212, 204, 214, 202, 203, 207, 219 }
                        },
                    }
                },
                {
                    Chara.Sakuya,
                    new Dictionary<CardType, int[]>
                    {
                        {
                            CardType.Skill,
                            new int[] { 102, 106, 110, 100, 104, 108, 101, 105, 109, 103, 107, 111 }
                        },
                        {
                            CardType.Spell,
                            new int[] { 200, 206, 207, 201, 202, 208, 211, 212, 203, 205, 209, 210, 204 }
                        },
                    }
                },
                {
                    Chara.Alice,
                    new Dictionary<CardType, int[]>
                    {
                        {
                            CardType.Skill,
                            new int[] { 100, 104, 108, 101, 105, 109, 102, 106, 110, 103, 107, 111 }
                        },
                        {
                            CardType.Spell,
                            new int[] { 200, 201, 202, 206, 209, 203, 208, 204, 205, 207, 210, 211 }
                        },
                    }
                },
                {
                    Chara.Patchouli,
                    new Dictionary<CardType, int[]>
                    {
                        {
                            CardType.Skill,
                            new int[] { 100, 105, 110, 102, 107, 112, 103, 108, 113, 104, 109, 114, 101, 106, 111 }
                        },
                        {
                            CardType.Spell,
                            new int[] { 201, 202, 200, 206, 207, 210, 211, 204, 212, 203, 205, 213 }
                        },
                    }
                },
                {
                    Chara.Youmu,
                    new Dictionary<CardType, int[]>
                    {
                        {
                            CardType.Skill,
                            new int[] { 100, 104, 108, 101, 105, 109, 102, 106, 110, 103, 107, 111 }
                        },
                        {
                            CardType.Spell,
                            new int[] { 200, 202, 201, 206, 212, 204, 207, 203, 205, 208 }
                        },
                    }
                },
                {
                    Chara.Remilia,
                    new Dictionary<CardType, int[]>
                    {
                        {
                            CardType.Skill,
                            new int[] { 100, 104, 108, 102, 106, 110, 101, 105, 109, 103, 107, 111 }
                        },
                        {
                            CardType.Spell,
                            new int[] { 201, 202, 200, 206, 207, 204, 208, 209, 203, 205 }
                        },
                    }
                },
                {
                    Chara.Yuyuko,
                    new Dictionary<CardType, int[]>
                    {
                        {
                            CardType.Skill,
                            new int[] { 101, 105, 109, 103, 107, 111, 100, 104, 108, 102, 106, 110 }
                        },
                        {
                            CardType.Spell,
                            new int[] { 201, 200, 202, 206, 208, 219, 203, 205, 207, 204, 209 }
                        },
                    }
                },
                {
                    Chara.Yukari,
                    new Dictionary<CardType, int[]>
                    {
                        {
                            CardType.Skill,
                            new int[] { 100, 104, 108, 101, 105, 109, 102, 106, 110, 103, 107, 111 }
                        },
                        {
                            CardType.Spell,
                            new int[] { 202, 204, 207, 200, 201, 205, 206, 203, 208, 215 }
                        },
                    }
                },
                {
                    Chara.Suika,
                    new Dictionary<CardType, int[]>
                    {
                        {
                            CardType.Skill,
                            new int[] { 100, 104, 108, 101, 105, 109, 102, 106, 110, 103, 107, 111 }
                        },
                        {
                            CardType.Spell,
                            new int[] { 206, 200, 201, 202, 212, 204, 205, 207, 203, 208 }
                        },
                    }
                },
                {
                    Chara.Reisen,
                    new Dictionary<CardType, int[]>
                    {
                        {
                            CardType.Skill,
                            new int[] { 100, 104, 108, 102, 106, 110, 101, 105, 109, 103, 107, 111 }
                        },
                        {
                            CardType.Spell,
                            new int[] { 200, 206, 208, 211, 202, 203, 207, 209, 210, 204, 205 }
                        },
                    }
                },
                {
                    Chara.Aya,
                    new Dictionary<CardType, int[]>
                    {
                        {
                            CardType.Skill,
                            new int[] { 100, 104, 108, 101, 105, 109, 103, 107, 111, 102, 106, 110 }
                        },
                        {
                            CardType.Spell,
                            new int[] { 211, 205, 207, 200, 203, 212, 202, 208, 201, 206 }
                        },
                    }
                },
                {
                    Chara.Komachi,
                    new Dictionary<CardType, int[]>
                    {
                        {
                            CardType.Skill,
                            new int[] { 100, 104, 108, 101, 105, 109, 103, 107, 111, 102, 106, 110 }
                        },
                        {
                            CardType.Spell,
                            new int[] { 200, 202, 203, 205, 211, 206, 207, 201, 204 }
                        },
                    }
                },
                {
                    Chara.Iku,
                    new Dictionary<CardType, int[]>
                    {
                        {
                            CardType.Skill,
                            new int[] { 100, 104, 108, 101, 105, 109, 103, 107, 111, 102, 106, 110 }
                        },
                        {
                            CardType.Spell,
                            new int[] { 200, 208, 206, 201, 202, 203, 209, 207, 211, 210 }
                        },
                    }
                },
                {
                    Chara.Tenshi,
                    new Dictionary<CardType, int[]>
                    {
                        {
                            CardType.Skill,
                            new int[] { 102, 106, 110, 103, 107, 111, 100, 104, 108, 101, 105, 109 }
                        },
                        {
                            CardType.Spell,
                            new int[] { 200, 201, 202, 208, 209, 204, 206, 203, 205, 207 }
                        },
                    }
                },
                {
                    Chara.Sanae,
                    new Dictionary<CardType, int[]>
                    {
                        {
                            CardType.Skill,
                            new int[] { 100, 104, 108, 102, 106, 110, 103, 107, 111, 101, 105, 109 }
                        },
                        {
                            CardType.Spell,
                            new int[] { 200, 201, 203, 204, 206, 202, 207, 205, 210 }
                        },
                    }
                },
                {
                    Chara.Cirno,
                    new Dictionary<CardType, int[]>
                    {
                        {
                            CardType.Skill,
                            new int[] { 100, 104, 108, 103, 107, 111, 101, 105, 109, 102, 106, 110 }
                        },
                        {
                            CardType.Spell,
                            new int[] { 200, 205, 202, 203, 207, 210, 213, 201, 206, 204, 208 }
                        },
                    }
                },
                {
                    Chara.Meiling,
                    new Dictionary<CardType, int[]>
                    {
                        {
                            CardType.Skill,
                            new int[] { 103, 107, 111, 101, 105, 109, 100, 104, 108, 102, 106, 110 }
                        },
                        {
                            CardType.Spell,
                            new int[] { 200, 206, 202, 204, 207, 201, 208, 211, 203, 205, 209 }
                        },
                    }
                },
                {
                    Chara.Utsuho,
                    new Dictionary<CardType, int[]>
                    {
                        {
                            CardType.Skill,
                            new int[] { 101, 105, 109, 100, 104, 108, 103, 107, 111, 102, 106, 110 }
                        },
                        {
                            CardType.Spell,
                            new int[] { 206, 211, 213, 200, 203, 205, 208, 204, 207, 209, 210, 212, 201, 214 }
                        },
                    }
                },
                {
                    Chara.Suwako,
                    new Dictionary<CardType, int[]>
                    {
                        {
                            CardType.Skill,
                            new int[] { 102, 106, 110, 101, 105, 109, 100, 104, 108, 103, 107, 111 }
                        },
                        {
                            CardType.Spell,
                            new int[] { 201, 204, 200, 202, 203, 205, 206, 207, 209, 212, 208 }
                        },
                    }
                },
            };

        // Thanks to en.touhouwiki.net
        private static readonly Dictionary<Chara, List<StageInfo>> StageInfoTable =
            new Dictionary<Chara, List<StageInfo>>
            {
                {
                    Chara.Sanae,
                    new List<StageInfo>
                    {
                        new StageInfo(Stage.St1, Chara.Cirno,   Enumerable.Range(0, 2)),
                        new StageInfo(Stage.St2, Chara.Meiling, Enumerable.Range(0, 2)),
                        new StageInfo(Stage.St3, Chara.Reimu,   Enumerable.Range(0, 3)),
                        new StageInfo(Stage.St4, Chara.Utsuho,  Enumerable.Range(0, 4)),
                        new StageInfo(Stage.St5, Chara.Suwako,  Enumerable.Range(0, 5)),
                    }
                },
                {
                    Chara.Cirno,
                    new List<StageInfo>
                    {
                        new StageInfo(Stage.St1, Chara.Sanae,   Enumerable.Range(0, 2)),
                        new StageInfo(Stage.St2, Chara.Meiling, Enumerable.Range(0, 2)),
                        new StageInfo(Stage.St3, Chara.Marisa,  Enumerable.Range(0, 3)),
                        new StageInfo(Stage.St4, Chara.Utsuho,  Enumerable.Range(0, 4)),
                        new StageInfo(Stage.St5, Chara.Alice,   Enumerable.Range(0, 5)),
                    }
                },
                {
                    Chara.Meiling,
                    new List<StageInfo>
                    {
                        new StageInfo(Stage.St1, Chara.Patchouli, Enumerable.Range(0, 2)),
                        new StageInfo(Stage.St2, Chara.Alice,     Enumerable.Range(0, 2)),
                        new StageInfo(Stage.St3, Chara.Marisa,    Enumerable.Range(0, 3)),
                        new StageInfo(Stage.St4, Chara.Reimu,     Enumerable.Range(0, 4)),
                        new StageInfo(Stage.St5, Chara.Oonamazu,  Enumerable.Range(0, 5)),
                    }
                },
            };

        private static readonly Dictionary<Chara, IEnumerable<CharaCardIdPair>> EnemyCardIdTable =
            StageInfoTable.ToDictionary(
                stageInfoPair => stageInfoPair.Key,
                stageInfoPair => stageInfoPair.Value.SelectMany(
                    stageInfo => stageInfo.CardIds.Select(id => new CharaCardIdPair(stageInfo.Enemy, id))));

        private static readonly new EnumShortNameParser<LevelWithTotal> LevelWithTotalParser =
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
            [EnumAltName("SN")] Sanae,
            [EnumAltName("CI")] Cirno,
            [EnumAltName("ML")] Meiling,
            [EnumAltName("UT")] Utsuho,
            [EnumAltName("SW")] Suwako,
            [EnumAltName("NM")] Oonamazu = 0x15,
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
            [EnumAltName("SN")] Sanae,
            [EnumAltName("CI")] Cirno,
            [EnumAltName("ML")] Meiling,
            [EnumAltName("UT")] Utsuho,
            [EnumAltName("SW")] Suwako,
            [EnumAltName("NM")] Oonamazu = 0x15,
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
            get { return "1.10a"; }
        }

        protected override bool ReadScoreFile(Stream input)
        {
            using (var decrypted = new MemoryStream())
#if DEBUG
            using (var decoded = new FileStream("th123decoded.dat", FileMode.Create, FileAccess.ReadWrite))
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
            var reader = new BinaryReader(input);
            var allScoreData = new AllScoreData();

            try
            {
                allScoreData.ReadFrom(reader);
            }
            catch (EndOfStreamException)
            {
            }

            var numCharas = Enum.GetValues(typeof(Chara)).Length - 1;   // Except Oonamazu
            if (allScoreData.ClearData.Count == numCharas)
                return allScoreData;
            else
                return null;
        }

        // serialNumber: 0-based
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
        private static CharaCardIdPair GetCharaCardIdPair(Chara chara, CardType type, int serialNumber)
        {
#if false
            if (type == CardType.System)
                return null;

            Func<CharaCardIdPair, bool> matchesCharaAndType;
            if (type == CardType.Skill)
                matchesCharaAndType = (pair => (pair.Chara == chara) && (pair.CardId / 100 == 1));
            else
                matchesCharaAndType = (pair => (pair.Chara == chara) && (pair.CardId / 100 == 2));

            return CardNameTable.Keys.Where(matchesCharaAndType).ElementAtOrDefault(serialNumber);
#else
            if (CardOrderTable.TryGetValue(chara, out Dictionary<CardType, int[]> cardTypeIdDict))
            {
                if (cardTypeIdDict.TryGetValue(type, out int[] cardIds))
                {
                    if (serialNumber < cardIds.Length)
                        return new CharaCardIdPair(chara, cardIds[serialNumber]);
                    else
                        return null;
                }
                else
                    return null;
            }
            else
                return null;
#endif
        }

        // %T123C[xx][yy][z]
        private class CareerReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T123C(\d{{2}})({0})([1-3])", CharaParser.Pattern);

            private readonly MatchEvaluator evaluator;

            [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
            public CareerReplacer(Th123Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var number = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
                    var chara = CharaParser.Parse(match.Groups[2].Value);
                    var type = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

                    if ((chara != Chara.Sanae) && (chara != Chara.Cirno) && (chara != Chara.Meiling))
                        return match.ToString();

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
                        return toString(clearData.SpellCardResults.Values.Sum(getValue));
                    else
                    {
                        var numLevels = Enum.GetValues(typeof(Level)).Length;
                        var index = (number - 1) / numLevels;
                        if ((index >= 0) && (index < EnemyCardIdTable[chara].Count()))
                        {
                            var enemyCardIdPair = EnemyCardIdTable[chara].ElementAt(index);
                            var key = new CharaCardIdPair(
                                enemyCardIdPair.Chara,
                                (enemyCardIdPair.CardId * numLevels) + ((number - 1) % numLevels));
                            return toString(getValue(clearData.SpellCardResults[key]));
                        }
                        else
                            return match.ToString();
                    }
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        // %T123CARD[xx][yy][z]
        private class CardReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T123CARD(\d{{2}})({0})([NR])", CharaParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public CardReplacer(Th123Converter parent, bool hideUntriedCards)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var number = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
                    var chara = CharaParser.Parse(match.Groups[2].Value);
                    var type = match.Groups[3].Value.ToUpperInvariant();

                    if (number <= 0)
                        return match.ToString();
                    if ((chara != Chara.Sanae) && (chara != Chara.Cirno) && (chara != Chara.Meiling))
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
                            var key = new CharaCardIdPair(
                                enemyCardIdPair.Chara,
                                (enemyCardIdPair.CardId * numLevels) + (int)level);
                            if (clearData.SpellCardResults[key].TrialCount <= 0)
                                return (type == "N") ? "??????????" : "?????";
                        }

                        return (type == "N") ? CardNameTable[enemyCardIdPair] : level.ToString();
                    }
                    else
                        return match.ToString();
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        // %T123CRG[x][yy][z]
        private class CollectRateReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T123CRG({0})({1})([1-2])",
                LevelWithTotalParser.Pattern,
                CharaParser.Pattern);

            private readonly MatchEvaluator evaluator;

            [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
            public CollectRateReplacer(Th123Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var level = LevelWithTotalParser.Parse(match.Groups[1].Value);
                    var chara = CharaParser.Parse(match.Groups[2].Value);
                    var type = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

                    if ((chara != Chara.Sanae) && (chara != Chara.Cirno) && (chara != Chara.Meiling))
                        return match.ToString();

                    Func<KeyValuePair<CharaCardIdPair, SpellCardResult>, bool> findByLevel;
                    if (level == LevelWithTotal.Total)
                        findByLevel = (pair => true);
                    else
                        findByLevel = (pair => pair.Value.Level == (Level)level);

                    Func<KeyValuePair<CharaCardIdPair, SpellCardResult>, bool> countByType;
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

        // %T123DC[ww][x][yy][z]
        private class CardForDeckReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T123DC({0})({1})(\d{{2}})([NC])", CharaParser.Pattern, CardTypeParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public CardForDeckReplacer(Th123Converter parent, bool hideUntriedCards)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var chara = CharaParser.Parse(match.Groups[1].Value);
                    var cardType = CardTypeParser.Parse(match.Groups[2].Value);
                    var number = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);
                    var type = match.Groups[4].Value.ToUpperInvariant();

                    if (chara == Chara.Oonamazu)
                        return match.ToString();

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
                                return Utils.ToNumberString(card.MaxNumber);
                        }
                        else
                            return match.ToString();
                    }
                    else
                    {
                        var key = GetCharaCardIdPair(chara, cardType, number - 1);
                        if (key != null)
                        {
                            var card = parent.allScoreData.ClearData[key.Chara].CardsForDeck[key.CardId];
                            if (type == "N")
                            {
                                if (hideUntriedCards)
                                {
                                    if (card.MaxNumber <= 0)
                                        return "??????????";
                                }

                                return CardNameTable[key];
                            }
                            else
                                return Utils.ToNumberString(card.MaxNumber);
                        }
                        else
                            return match.ToString();
                    }
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        private class CharaCardIdPair : Pair<Chara, int>
        {
            public CharaCardIdPair(Chara chara, int cardId)
                : base(chara, cardId)
            {
            }

            public Chara Chara
            {
                get { return this.First; }
            }

            public int CardId
            {
                get { return this.Second; }     // 0-based
            }
        }

        private class StageInfo
        {
            public StageInfo(Stage stage, Chara enemy, IEnumerable<int> cardIds)
            {
                this.Stage = stage;
                this.Enemy = enemy;
                this.CardIds = cardIds.ToList();
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public Stage Stage { get; private set; }

            public Chara Enemy { get; private set; }

            public List<int> CardIds { get; private set; }  // 0-based
        }

        private class AllScoreData : IBinaryReadable
        {
            public AllScoreData()
            {
                this.StoryClearCounts = new Dictionary<Chara, byte>(
                    Enum.GetValues(typeof(Chara)).Length - 1);  // Except Oonamazu
                this.SystemCards = null;
                this.ClearData = null;
            }

            public Dictionary<Chara, byte> StoryClearCounts { get; private set; }

            public Dictionary<int, CardForDeck> SystemCards { get; private set; }

            public Dictionary<Chara, ClearData> ClearData { get; private set; }

            public void ReadFrom(BinaryReader reader)
            {
                if (reader == null)
                    throw new ArgumentNullException("reader");

                var validNumCharas = Enum.GetValues(typeof(Chara)).Length - 1;  // Except Oonamazu

                reader.ReadUInt32();            // version? (0xD2 == 210 --> ver.1.10?)
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

                var num = reader.ReadInt32();
                for (var index = 0; index < num; index++)
                    reader.ReadUInt32();

                var numSystemCards = reader.ReadInt32();
                this.SystemCards = new Dictionary<int, CardForDeck>(numSystemCards);
                for (var index = 0; index < numSystemCards; index++)
                {
                    var card = new CardForDeck();
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

        private class ClearData : IBinaryReadable   // per character
        {
            public ClearData()
            {
                this.CardsForDeck = null;
                this.SpellCardResults = null;
            }

            public Dictionary<int, CardForDeck> CardsForDeck { get; private set; }

            public Dictionary<CharaCardIdPair, SpellCardResult> SpellCardResults { get; private set; }

            public void ReadFrom(BinaryReader reader)
            {
                if (reader == null)
                    throw new ArgumentNullException("reader");

                var numCards = reader.ReadInt32();
                this.CardsForDeck = new Dictionary<int, CardForDeck>(numCards);
                for (var index = 0; index < numCards; index++)
                {
                    var card = new CardForDeck();
                    card.ReadFrom(reader);
                    if (!this.CardsForDeck.ContainsKey(card.Id))
                        this.CardsForDeck.Add(card.Id, card);
                }

                var numResults = reader.ReadInt32();
                this.SpellCardResults = new Dictionary<CharaCardIdPair, SpellCardResult>(numResults);
                for (var index = 0; index < numResults; index++)
                {
                    var result = new SpellCardResult();
                    result.ReadFrom(reader);
                    var key = new CharaCardIdPair(result.Enemy, result.Id);
                    if (!this.SpellCardResults.ContainsKey(key))
                        this.SpellCardResults.Add(key, result);
                }
            }
        }

        private class CardForDeck : IBinaryReadable
        {
            public CardForDeck()
            {
            }

            public int Id { get; private set; }     // 0-based

            public int MaxNumber { get; private set; }

            public void ReadFrom(BinaryReader reader)
            {
                if (reader == null)
                    throw new ArgumentNullException("reader");

                this.Id = reader.ReadInt32();
                this.MaxNumber = reader.ReadInt32();
            }
        }

        private class SpellCardResult : IBinaryReadable
        {
            public SpellCardResult()
            {
            }

            public Chara Enemy { get; private set; }

            public Level Level { get; private set; }

            public int Id { get; private set; }     // 0-based

            public int TrialCount { get; private set; }

            public int GotCount { get; private set; }

            public uint Frames { get; private set; }

            public void ReadFrom(BinaryReader reader)
            {
                if (reader == null)
                    throw new ArgumentNullException("reader");

                this.Enemy = (Chara)reader.ReadInt32();
                this.Level = (Level)reader.ReadInt32();
                this.Id = reader.ReadInt32();
                this.TrialCount = reader.ReadInt32();
                this.GotCount = reader.ReadInt32();
                this.Frames = reader.ReadUInt32();
            }
        }
    }
}

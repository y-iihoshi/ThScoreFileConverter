//-----------------------------------------------------------------------
// <copyright file="Definitions.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using System.Linq;
using CardType = ThScoreFileConverter.Models.Th105.CardType;
using StageInfo = ThScoreFileConverter.Models.Th105.StageInfo<ThScoreFileConverter.Models.Th123.Chara>;

namespace ThScoreFileConverter.Models.Th123
{
    internal static class Definitions
    {
        // Thanks to en.touhouwiki.net
        public static IReadOnlyDictionary<int, string> SystemCardNameTable { get; } =
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
        public static IReadOnlyDictionary<(Chara Chara, int CardId), string> CardNameTable { get; } =
            new Dictionary<(Chara, int), string>
            {
                { (Chara.Reimu,   0), "御守「妖怪足止めお守り」" },
                { (Chara.Reimu,   1), "霊符「夢想封印　円」" },
                { (Chara.Reimu,   2), "神霊「夢想封印　瞬」" },
                { (Chara.Reimu,   3), "「最も凶悪なびっくり巫女玉」" },
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
                { (Chara.Reimu, 111), "雨乞祈り" },
                { (Chara.Reimu, 200), "霊符「夢想妙珠」" },
                { (Chara.Reimu, 201), "神霊「夢想封印」" },
                { (Chara.Reimu, 204), "夢符「封魔陣」" },
                { (Chara.Reimu, 206), "神技「八方鬼縛陣」" },
                { (Chara.Reimu, 207), "結界「拡散結界」" },
                { (Chara.Reimu, 208), "珠符「明珠暗投」" },
                { (Chara.Reimu, 209), "宝符「陰陽宝玉」" },
                { (Chara.Reimu, 210), "宝具「陰陽鬼神玉」" },
                { (Chara.Reimu, 214), "神技「天覇風神脚」" },
                { (Chara.Reimu, 219), "「夢想天生」" },
                { (Chara.Marisa,   0), "星符「エキセントリックアステロイド」" },
                { (Chara.Marisa,   1), "流光「シューティングエコー」" },
                { (Chara.Marisa,   2), "魔符「マジカルＲ３６０」" },
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
                { (Chara.Marisa, 110), "マジカル産廃再利用ボム" },
                { (Chara.Marisa, 111), "グリーンスプレッド" },
                { (Chara.Marisa, 200), "恋符「マスタースパーク」" },
                { (Chara.Marisa, 202), "魔砲「ファイナルスパーク」" },
                { (Chara.Marisa, 203), "星符「ドラゴンメテオ」" },
                { (Chara.Marisa, 204), "恋符「ノンディレクショナルレーザー」" },
                { (Chara.Marisa, 205), "魔符「スターダストレヴァリエ」" },
                { (Chara.Marisa, 206), "星符「エスケープベロシティ」" },
                { (Chara.Marisa, 207), "彗星「ブレイジングスター」" },
                { (Chara.Marisa, 208), "星符「メテオニックシャワー」" },
                { (Chara.Marisa, 209), "星符「グラビティビート」" },
                { (Chara.Marisa, 211), "光符「ルミネスストライク」" },
                { (Chara.Marisa, 212), "光符「アースライトレイ」" },
                { (Chara.Marisa, 214), "魔廃「ディープエコロジカルボム」" },
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
                { (Chara.Sakuya, 108), "ダンシングスターソード" },
                { (Chara.Sakuya, 109), "ミスディレクション" },
                { (Chara.Sakuya, 110), "パラレルブレーン" },
                { (Chara.Sakuya, 111), "タイムパラドックス" },
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
                { (Chara.Sakuya, 210), "光速「Ｃ．リコシェ」" },
                { (Chara.Sakuya, 211), "時符「イマジナリバーチカルタイム」" },
                { (Chara.Sakuya, 212), "時計「ルナダイアル」" },
                { (Chara.Alice,   0), "人形「セミオートマトン」" },
                { (Chara.Alice,   1), "騎士「ドールオブラウンドテーブル」" },
                { (Chara.Alice,   2), "犠牲「スーサイドパクト」" },
                { (Chara.Alice,   3), "試験中「レベルティターニア」" },
                { (Chara.Alice,   4), "試験中「ゴリアテ人形」" },
                { (Chara.Alice, 100), "人形操創" },
                { (Chara.Alice, 101), "人形無操" },
                { (Chara.Alice, 102), "人形置操" },
                { (Chara.Alice, 103), "人形振起" },
                { (Chara.Alice, 104), "人形帰巣" },
                { (Chara.Alice, 105), "人形火葬" },
                { (Chara.Alice, 106), "人形千槍" },
                { (Chara.Alice, 107), "人形ＳＰ" },
                { (Chara.Alice, 108), "人形伏兵" },
                { (Chara.Alice, 109), "大江戸爆薬からくり人形" },
                { (Chara.Alice, 110), "人形弓兵" },
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
                { (Chara.Alice, 210), "槍符「キューティ大千槍」" },
                { (Chara.Alice, 211), "人形「レミングスパレード」" },
                { (Chara.Patchouli,   0), "火符「リングオブアグニ」" },
                { (Chara.Patchouli,   1), "水符「プリンセスウンディネ」" },
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
                { (Chara.Patchouli, 110), "ワイプモイスチャー" },
                { (Chara.Patchouli, 111), "スティッキーバブル" },
                { (Chara.Patchouli, 112), "スタティックグリーン" },
                { (Chara.Patchouli, 113), "フォールスラッシャー" },
                { (Chara.Patchouli, 114), "ダイアモンドハードネス" },
                { (Chara.Patchouli, 200), "火金符「セントエルモピラー」" },
                { (Chara.Patchouli, 201), "土水符「ノエキアンデリュージュ」" },
                { (Chara.Patchouli, 202), "金木符「エレメンタルハーベスター」" },
                { (Chara.Patchouli, 203), "日符「ロイヤルフレア」" },
                { (Chara.Patchouli, 204), "月符「サイレントセレナ」" },
                { (Chara.Patchouli, 205), "火水木金土符「賢者の石」" },
                { (Chara.Patchouli, 206), "水符「ジェリーフィッシュプリンセス」" },
                { (Chara.Patchouli, 207), "月木符「サテライトヒマワリ」" },
                { (Chara.Patchouli, 210), "日木符「フォトシンセシス」" },
                { (Chara.Patchouli, 211), "火水符「フロギスティックピラー」" },
                { (Chara.Patchouli, 212), "土金符「エメラルドメガロポリス」" },
                { (Chara.Patchouli, 213), "日月符「ロイヤルダイアモンドリング」" },
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
                { (Chara.Youmu, 108), "燐気斬" },
                { (Chara.Youmu, 109), "炯眼剣" },
                { (Chara.Youmu, 110), "頭上花剪斬" },
                { (Chara.Youmu, 111), "奇び半身" },
                { (Chara.Youmu, 200), "人符「現世斬」" },
                { (Chara.Youmu, 201), "断命剣「冥想斬」" },
                { (Chara.Youmu, 202), "魂符「幽明の苦輪」" },
                { (Chara.Youmu, 203), "人鬼「未来永劫斬」" },
                { (Chara.Youmu, 204), "断迷剣「迷津慈航斬」" },
                { (Chara.Youmu, 205), "魂魄「幽明求聞持聡明の法」" },
                { (Chara.Youmu, 206), "剣伎「桜花閃々」" },
                { (Chara.Youmu, 207), "断霊剣「成仏得脱斬」" },
                { (Chara.Youmu, 208), "空観剣「六根清浄斬」" },
                { (Chara.Youmu, 212), "転生剣「円心流転斬」" },
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
                { (Chara.Remilia, 108), "トリックスターデビル" },
                { (Chara.Remilia, 109), "デモンズディナーフォーク" },
                { (Chara.Remilia, 110), "バンパイアキス" },
                { (Chara.Remilia, 111), "スティグマナイザー" },
                { (Chara.Remilia, 200), "紅符「不夜城レッド」" },
                { (Chara.Remilia, 201), "必殺「ハートブレイク」" },
                { (Chara.Remilia, 202), "夜符「デーモンキングクレイドル」" },
                { (Chara.Remilia, 203), "紅魔「スカーレットデビル」" },
                { (Chara.Remilia, 204), "神槍「スピア・ザ・グングニル」" },
                { (Chara.Remilia, 205), "夜王「ドラキュラクレイドル」" },
                { (Chara.Remilia, 206), "夜符「バッドレディスクランブル」" },
                { (Chara.Remilia, 207), "運命「ミゼラブルフェイト」" },
                { (Chara.Remilia, 208), "「ミレニアムの吸血鬼」" },
                { (Chara.Remilia, 209), "悪魔「レミリアストレッチ」" },
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
                { (Chara.Yuyuko, 109), "死還の大地" },
                { (Chara.Yuyuko, 110), "故人のお届け物" },
                { (Chara.Yuyuko, 111), "センスオブエレガンス" },
                { (Chara.Yuyuko, 200), "死符「ギャストリドリーム」" },
                { (Chara.Yuyuko, 201), "冥符「黄泉平坂行路」" },
                { (Chara.Yuyuko, 202), "霊符「无寿の夢」" },
                { (Chara.Yuyuko, 203), "死蝶「華胥の永眠」" },
                { (Chara.Yuyuko, 204), "再迷「幻想郷の黄泉還り」" },
                { (Chara.Yuyuko, 205), "寿命「无寿国への約束手形」" },
                { (Chara.Yuyuko, 206), "霊蝶「蝶の羽風生に暫く」" },
                { (Chara.Yuyuko, 207), "蝶符「鳳蝶紋の死槍」" },
                { (Chara.Yuyuko, 208), "幽雅「死出の誘蛾灯」" },
                { (Chara.Yuyuko, 209), "桜符「センスオブチェリーブロッサム」" },
                { (Chara.Yuyuko, 219), "「反魂蝶」" },
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
                { (Chara.Yukari, 109), "知能と脚の境界" },
                { (Chara.Yukari, 110), "変容を見る眼" },
                { (Chara.Yukari, 111), "キマイラの翼" },
                { (Chara.Yukari, 200), "境符「四重結界」" },
                { (Chara.Yukari, 201), "式神「八雲藍」" },
                { (Chara.Yukari, 202), "境符「二次元と三次元の境界」" },
                { (Chara.Yukari, 203), "結界「魅力的な四重結界」" },
                { (Chara.Yukari, 204), "式神「橙」" },
                { (Chara.Yukari, 205), "結界「客観結界」" },
                { (Chara.Yukari, 206), "幻巣「飛光虫ネスト」" },
                { (Chara.Yukari, 207), "空餌「中毒性のあるエサ」" },
                { (Chara.Yukari, 208), "魔眼「ラプラスの魔」" },
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
                { (Chara.Suika, 108), "踏鞴" },
                { (Chara.Suika, 109), "火鬼" },
                { (Chara.Suika, 110), "鬼神燐火術" },
                { (Chara.Suika, 111), "攫鬼" },
                { (Chara.Suika, 200), "萃符「戸隠山投げ」" },
                { (Chara.Suika, 201), "酔神「鬼縛りの術」" },
                { (Chara.Suika, 202), "鬼符「ミッシングパワー」" },
                { (Chara.Suika, 203), "萃鬼「天手力男投げ」" },
                { (Chara.Suika, 204), "酔夢「施餓鬼縛りの術」" },
                { (Chara.Suika, 205), "鬼神「ミッシングパープルパワー」" },
                { (Chara.Suika, 206), "霧符「雲集霧散」" },
                { (Chara.Suika, 207), "鬼火「超高密度燐禍術」" },
                { (Chara.Suika, 208), "鬼符「大江山悉皆殺し」" },
                { (Chara.Suika, 212), "四天王奥義「三歩壊廃」" },
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
                { (Chara.Reisen, 111), "アキュラースペクトル" },
                { (Chara.Reisen, 200), "惑視「離円花冠(カローラヴィジョン)」" },
                { (Chara.Reisen, 202), "幻爆「近眼花火(マインドスターマイン)」" },
                { (Chara.Reisen, 203), "幻惑「花冠視線(クラウンヴィジョン)」" },
                { (Chara.Reisen, 204), "赤眼「望見円月(ルナティックブラスト)」" },
                { (Chara.Reisen, 205), "「幻朧月睨(ルナティックレッドアイズ)」" },
                { (Chara.Reisen, 206), "弱心「喪心喪意(ディモチヴェイション)」" },
                { (Chara.Reisen, 207), "喪心「喪心創痍(ディスカーダー)」" },
                { (Chara.Reisen, 208), "毒煙幕「瓦斯織物の玉」" },
                { (Chara.Reisen, 209), "生薬「国士無双の薬」" },
                { (Chara.Reisen, 210), "短視「超短脳波(エックスウェイブ)」" },
                { (Chara.Reisen, 211), "長視「赤月下(インフレアドムーン)」" },
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
                { (Chara.Aya, 108), "楓扇風" },
                { (Chara.Aya, 109), "天狗ナメシ" },
                { (Chara.Aya, 110), "天狗の太鼓" },
                { (Chara.Aya, 111), "天狗礫" },
                { (Chara.Aya, 200), "旋符「紅葉扇風」" },
                { (Chara.Aya, 201), "竜巻「天孫降臨の道しるべ」" },
                { (Chara.Aya, 202), "逆風「人間禁制の道」" },
                { (Chara.Aya, 203), "突符「天狗のマクロバースト」" },
                { (Chara.Aya, 205), "風符「天狗道の開風」" },
                { (Chara.Aya, 206), "「幻想風靡」" },
                { (Chara.Aya, 207), "風符「天狗報即日限」" },
                { (Chara.Aya, 208), "鴉符「暗夜のデイメア」" },
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
                { (Chara.Komachi, 109), "三途の舟" },
                { (Chara.Komachi, 110), "お迎え体験版" },
                { (Chara.Komachi, 111), "離魂の鎌" },
                { (Chara.Komachi, 200), "舟符「河の流れのように」" },
                { (Chara.Komachi, 201), "薄命「余命幾許も無し」" },
                { (Chara.Komachi, 202), "霊符「何処にでもいる浮遊霊」" },
                { (Chara.Komachi, 203), "死歌「八重霧の渡し」" },
                { (Chara.Komachi, 204), "換命「不惜身命、可惜身命」" },
                { (Chara.Komachi, 205), "恨符「未練がましい緊縛霊」" },
                { (Chara.Komachi, 206), "死符「死者選別の鎌」" },
                { (Chara.Komachi, 207), "魂符「生魂流離の鎌」" },
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
                { (Chara.Iku, 108), "水得の龍魚" },
                { (Chara.Iku, 109), "天女の一撃" },
                { (Chara.Iku, 110), "龍神の髭" },
                { (Chara.Iku, 111), "龍の眼" },
                { (Chara.Iku, 200), "電符「雷鼓弾」" },
                { (Chara.Iku, 201), "魚符「龍魚ドリル」" },
                { (Chara.Iku, 202), "雷符「エレキテルの龍宮」" },
                { (Chara.Iku, 203), "光星「光龍の吐息」" },
                { (Chara.Iku, 206), "雷魚「雷雲魚遊泳弾」" },
                { (Chara.Iku, 207), "羽衣「羽衣は空の如く」" },
                { (Chara.Iku, 208), "羽衣「羽衣は時の如く」" },
                { (Chara.Iku, 209), "棘符「雷雲棘魚」" },
                { (Chara.Iku, 210), "龍魚「龍宮の使い遊泳弾」" },
                { (Chara.Iku, 211), "珠符「五爪龍の珠」" },
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
                { (Chara.Tenshi, 108), "因果の剣" },
                { (Chara.Tenshi, 109), "地精の起床" },
                { (Chara.Tenshi, 110), "緋想の剣気" },
                { (Chara.Tenshi, 111), "昇天突" },
                { (Chara.Tenshi, 200), "地符「不譲土壌の剣」" },
                { (Chara.Tenshi, 201), "非想「非想非非想の剣」" },
                { (Chara.Tenshi, 202), "天符「天道是非の剣」" },
                { (Chara.Tenshi, 203), "地震「先憂後楽の剣」" },
                { (Chara.Tenshi, 204), "気符「天啓気象の剣」" },
                { (Chara.Tenshi, 205), "要石「天地開闢プレス」" },
                { (Chara.Tenshi, 206), "気符「無念無想の境地」" },
                { (Chara.Tenshi, 207), "「全人類の緋想天」" },
                { (Chara.Tenshi, 208), "剣技「気炎万丈の剣」" },
                { (Chara.Tenshi, 209), "天気「緋想天促」" },
                { (Chara.Sanae,   0), "蛇符「雲を泳ぐ大蛇」" },
                { (Chara.Sanae,   1), "奇跡「ファフロッキーズの奇跡」" },
                { (Chara.Sanae, 100), "風起こし" },
                { (Chara.Sanae, 101), "おみくじ爆弾" },
                { (Chara.Sanae, 102), "乾神招来　突" },
                { (Chara.Sanae, 103), "坤神招来　盾" },
                { (Chara.Sanae, 104), "波起こし" },
                { (Chara.Sanae, 105), "スカイサーペント" },
                { (Chara.Sanae, 106), "乾神招来　風" },
                { (Chara.Sanae, 107), "坤神招来　鉄輪" },
                { (Chara.Sanae, 108), "星落とし" },
                { (Chara.Sanae, 109), "コバルトスプレッド" },
                { (Chara.Sanae, 110), "乾神招来　御柱" },
                { (Chara.Sanae, 111), "坤神招来　罠" },
                { (Chara.Sanae, 200), "祈願「商売繁盛守り」" },
                { (Chara.Sanae, 201), "秘術「グレイソーマタージ」" },
                { (Chara.Sanae, 202), "秘術「忘却の祭儀」" },
                { (Chara.Sanae, 203), "神籤「乱れおみくじ連続引き」" },
                { (Chara.Sanae, 204), "開海「海が割れる日」" },
                { (Chara.Sanae, 205), "開海「モーゼの奇跡」" },
                { (Chara.Sanae, 206), "奇跡「白昼の客星」" },
                { (Chara.Sanae, 207), "奇跡「客星の明るすぎる夜」" },
                { (Chara.Sanae, 210), "秘法「九字刺し」" },
                { (Chara.Cirno,   0), "氷符「アイシクルフォール」" },
                { (Chara.Cirno,   1), "凍符「マイナスＫ」" },
                { (Chara.Cirno, 100), "アイシクルシュート" },
                { (Chara.Cirno, 101), "真夏のスノーマン" },
                { (Chara.Cirno, 102), "リトルアイスバーグ" },
                { (Chara.Cirno, 103), "フリーズタッチミー" },
                { (Chara.Cirno, 104), "フロストピラーズ" },
                { (Chara.Cirno, 105), "アイスチャージ" },
                { (Chara.Cirno, 106), "アイシクルボム" },
                { (Chara.Cirno, 107), "アイシクルライズ" },
                { (Chara.Cirno, 108), "冷凍光線" },
                { (Chara.Cirno, 109), "アイスキック" },
                { (Chara.Cirno, 110), "フローズン冷凍法" },
                { (Chara.Cirno, 111), "アイシクルソード" },
                { (Chara.Cirno, 200), "氷符「アイシクルマシンガン」" },
                { (Chara.Cirno, 201), "霜符「フロストコラムス」" },
                { (Chara.Cirno, 202), "氷塊「コールドスプリンクラー」" },
                { (Chara.Cirno, 203), "冷体「スーパーアイスキック」" },
                { (Chara.Cirno, 204), "凍符「パーフェクトフリーズ」" },
                { (Chara.Cirno, 205), "氷符「フェアリースピン」" },
                { (Chara.Cirno, 206), "吹氷「アイストルネード」" },
                { (Chara.Cirno, 207), "氷符「ソードフリーザー」" },
                { (Chara.Cirno, 208), "氷塊「グレートクラッシャー」" },
                { (Chara.Cirno, 210), "凍符「フリーズアトモスフェア」" },
                { (Chara.Cirno, 213), "冷符「瞬間冷凍ビーム」" },
                { (Chara.Meiling,   0), "彩翔「飛花落葉」" },
                { (Chara.Meiling,   1), "彩符「極彩沛雨」" },
                { (Chara.Meiling, 100), "螺光歩" },
                { (Chara.Meiling, 101), "紅砲" },
                { (Chara.Meiling, 102), "黄震脚" },
                { (Chara.Meiling, 103), "芳波" },
                { (Chara.Meiling, 104), "烈虹拳" },
                { (Chara.Meiling, 105), "紅寸剄" },
                { (Chara.Meiling, 106), "地龍波" },
                { (Chara.Meiling, 107), "水形太極拳" },
                { (Chara.Meiling, 108), "降華蹴" },
                { (Chara.Meiling, 109), "虎剄" },
                { (Chara.Meiling, 110), "天龍脚" },
                { (Chara.Meiling, 111), "彩雨" },
                { (Chara.Meiling, 200), "彩符「彩光風鈴」" },
                { (Chara.Meiling, 201), "極彩「彩光乱舞」" },
                { (Chara.Meiling, 202), "気符「星脈弾」" },
                { (Chara.Meiling, 203), "星気「星脈地転弾」" },
                { (Chara.Meiling, 204), "撃符「大鵬拳」" },
                { (Chara.Meiling, 205), "熾撃「大鵬墜撃拳」" },
                { (Chara.Meiling, 206), "虹符「烈虹真拳」" },
                { (Chara.Meiling, 207), "気符「地龍天龍脚」" },
                { (Chara.Meiling, 208), "彩華「虹色太極拳」" },
                { (Chara.Meiling, 209), "華符「彩光蓮華掌」" },
                { (Chara.Meiling, 211), "気符「猛虎内剄」" },
                { (Chara.Utsuho,   0), "熱符「ブレイクプロミネンス」" },
                { (Chara.Utsuho,   1), "核熱「核反応制御不能ダイブ」" },
                { (Chara.Utsuho,   2), "「ホットジュピター落下モデル」" },
                { (Chara.Utsuho,   3), "核熱「人工太陽の黒点」" },
                { (Chara.Utsuho, 100), "フレアアップ" },
                { (Chara.Utsuho, 101), "グラウンドメルト" },
                { (Chara.Utsuho, 102), "ブレイクサン" },
                { (Chara.Utsuho, 103), "シューティングスター" },
                { (Chara.Utsuho, 104), "ロケットダイブ" },
                { (Chara.Utsuho, 105), "ヘルゲイザー" },
                { (Chara.Utsuho, 106), "シューティングサン" },
                { (Chara.Utsuho, 107), "レトロ原子核モデル" },
                { (Chara.Utsuho, 108), "メルティング浴びせ蹴り" },
                { (Chara.Utsuho, 109), "レイディアントブレード" },
                { (Chara.Utsuho, 110), "地獄波動砲" },
                { (Chara.Utsuho, 111), "核熱の怨霊" },
                { (Chara.Utsuho, 200), "爆符「メガフレア」" },
                { (Chara.Utsuho, 201), "爆符「ギガフレア」" },
                { (Chara.Utsuho, 203), "焔星「フィクストスター」" },
                { (Chara.Utsuho, 204), "焔星「十凶星」" },
                { (Chara.Utsuho, 205), "核符「クリーピングサン」" },
                { (Chara.Utsuho, 206), "「地獄の人工太陽」" },
                { (Chara.Utsuho, 207), "地熱「核ブレイズゲイザー」" },
                { (Chara.Utsuho, 208), "光熱「ハイテンションブレード」" },
                { (Chara.Utsuho, 209), "鴉符「八咫烏ダイブ」" },
                { (Chara.Utsuho, 210), "核熱「核反応制御不能ダイブ」" },
                { (Chara.Utsuho, 211), "制御「セルフトカマク」" },
                { (Chara.Utsuho, 212), "「サブタレイニアンサン」" },
                { (Chara.Utsuho, 213), "遮光「核熱バイザー」" },
                { (Chara.Utsuho, 214), "「アビスノヴァ」" },
                { (Chara.Suwako,   0), "合掌「だいだらぼっちの参拝」" },
                { (Chara.Suwako,   1), "神具「洩矢の鉄の輪」" },
                { (Chara.Suwako,   2), "「獄熱の間欠泉」" },
                { (Chara.Suwako,   3), "「マグマの両生類」" },
                { (Chara.Suwako,   4), "「幻想郷空中神戦」" },
                { (Chara.Suwako, 100), "古の間欠泉" },
                { (Chara.Suwako, 101), "大蝦蟇神" },
                { (Chara.Suwako, 102), "大地の湖" },
                { (Chara.Suwako, 103), "土着神の祟り" },
                { (Chara.Suwako, 104), "水蛙神" },
                { (Chara.Suwako, 105), "古の鉄輪" },
                { (Chara.Suwako, 106), "古代翡翠" },
                { (Chara.Suwako, 107), "祟られた大地" },
                { (Chara.Suwako, 108), "雨を呼ぶ雨蛙" },
                { (Chara.Suwako, 109), "手長足長さん" },
                { (Chara.Suwako, 110), "蛙石神" },
                { (Chara.Suwako, 111), "ミシャグジさまの祟り" },
                { (Chara.Suwako, 200), "土着神「洩矢神」" },
                { (Chara.Suwako, 201), "源符「諏訪清水」" },
                { (Chara.Suwako, 202), "開宴「二拝二拍一拝」" },
                { (Chara.Suwako, 203), "土着神「ケロちゃん風雨に負けず」" },
                { (Chara.Suwako, 204), "神具「洩矢の鉄の輪」" },
                { (Chara.Suwako, 205), "源符「厭い川の翡翠」" },
                { (Chara.Suwako, 206), "蛙狩「蛙は口ゆえ蛇に呑まるる」" },
                { (Chara.Suwako, 207), "土着神「手長足長さま」" },
                { (Chara.Suwako, 208), "祟り神「赤口（ミシャグチ）さま」" },
                { (Chara.Suwako, 209), "土着神「宝永四年の赤蛙」" },
                { (Chara.Suwako, 212), "蛙休「オールウェイズ冬眠できます」" },
                { (Chara.Oonamazu, 0), "ナマズ「ほらほら世界が震えるぞ？」" },
                { (Chara.Oonamazu, 1), "ナマズ「液状化現象で大地も泥のようじゃ！」" },
                { (Chara.Oonamazu, 2), "ナマズ「発電だって頑張っちゃうぞ？」" },
                { (Chara.Oonamazu, 3), "ナマズ「オール電化でエコロジーじゃ！」" },
                { (Chara.Oonamazu, 4), "大ナマズ「これで浮き世もおしまいじゃあ！」" },
            };

        public static IReadOnlyDictionary<Chara, IReadOnlyDictionary<CardType, IReadOnlyList<int>>> CardOrderTable { get; } =
            new Dictionary<Chara, IReadOnlyDictionary<CardType, IReadOnlyList<int>>>
            {
                {
                    Chara.Reimu,
                    new Dictionary<CardType, IReadOnlyList<int>>
                    {
                        {
                            CardType.Skill,
                            new[] { 100, 104, 108, 103, 107, 111, 101, 105, 109, 102, 106, 110 }
                        },
                        {
                            CardType.Spell,
                            new[] { 208, 200, 204, 209, 207, 214, 206, 210, 201, 219 }
                        },
                    }
                },
                {
                    Chara.Marisa,
                    new Dictionary<CardType, IReadOnlyList<int>>
                    {
                        {
                            CardType.Skill,
                            new[] { 103, 107, 111, 101, 105, 109, 100, 104, 108, 102, 106, 110 }
                        },
                        {
                            CardType.Spell,
                            new[] { 208, 205, 211, 215, 200, 206, 209, 212, 204, 214, 202, 203, 207, 219 }
                        },
                    }
                },
                {
                    Chara.Sakuya,
                    new Dictionary<CardType, IReadOnlyList<int>>
                    {
                        {
                            CardType.Skill,
                            new[] { 102, 106, 110, 100, 104, 108, 101, 105, 109, 103, 107, 111 }
                        },
                        {
                            CardType.Spell,
                            new[] { 200, 206, 207, 201, 202, 208, 211, 212, 203, 205, 209, 210, 204 }
                        },
                    }
                },
                {
                    Chara.Alice,
                    new Dictionary<CardType, IReadOnlyList<int>>
                    {
                        {
                            CardType.Skill,
                            new[] { 100, 104, 108, 101, 105, 109, 102, 106, 110, 103, 107, 111 }
                        },
                        {
                            CardType.Spell,
                            new[] { 200, 201, 202, 206, 209, 203, 208, 204, 205, 207, 210, 211 }
                        },
                    }
                },
                {
                    Chara.Patchouli,
                    new Dictionary<CardType, IReadOnlyList<int>>
                    {
                        {
                            CardType.Skill,
                            new[] { 100, 105, 110, 102, 107, 112, 103, 108, 113, 104, 109, 114, 101, 106, 111 }
                        },
                        {
                            CardType.Spell,
                            new[] { 201, 202, 200, 206, 207, 210, 211, 204, 212, 203, 205, 213 }
                        },
                    }
                },
                {
                    Chara.Youmu,
                    new Dictionary<CardType, IReadOnlyList<int>>
                    {
                        {
                            CardType.Skill,
                            new[] { 100, 104, 108, 101, 105, 109, 102, 106, 110, 103, 107, 111 }
                        },
                        {
                            CardType.Spell,
                            new[] { 200, 202, 201, 206, 212, 204, 207, 203, 205, 208 }
                        },
                    }
                },
                {
                    Chara.Remilia,
                    new Dictionary<CardType, IReadOnlyList<int>>
                    {
                        {
                            CardType.Skill,
                            new[] { 100, 104, 108, 102, 106, 110, 101, 105, 109, 103, 107, 111 }
                        },
                        {
                            CardType.Spell,
                            new[] { 201, 202, 200, 206, 207, 204, 208, 209, 203, 205 }
                        },
                    }
                },
                {
                    Chara.Yuyuko,
                    new Dictionary<CardType, IReadOnlyList<int>>
                    {
                        {
                            CardType.Skill,
                            new[] { 101, 105, 109, 103, 107, 111, 100, 104, 108, 102, 106, 110 }
                        },
                        {
                            CardType.Spell,
                            new[] { 201, 200, 202, 206, 208, 219, 203, 205, 207, 204, 209 }
                        },
                    }
                },
                {
                    Chara.Yukari,
                    new Dictionary<CardType, IReadOnlyList<int>>
                    {
                        {
                            CardType.Skill,
                            new[] { 100, 104, 108, 101, 105, 109, 102, 106, 110, 103, 107, 111 }
                        },
                        {
                            CardType.Spell,
                            new[] { 202, 204, 207, 200, 201, 205, 206, 203, 208, 215 }
                        },
                    }
                },
                {
                    Chara.Suika,
                    new Dictionary<CardType, IReadOnlyList<int>>
                    {
                        {
                            CardType.Skill,
                            new[] { 100, 104, 108, 101, 105, 109, 102, 106, 110, 103, 107, 111 }
                        },
                        {
                            CardType.Spell,
                            new[] { 206, 200, 201, 202, 212, 204, 205, 207, 203, 208 }
                        },
                    }
                },
                {
                    Chara.Reisen,
                    new Dictionary<CardType, IReadOnlyList<int>>
                    {
                        {
                            CardType.Skill,
                            new[] { 100, 104, 108, 102, 106, 110, 101, 105, 109, 103, 107, 111 }
                        },
                        {
                            CardType.Spell,
                            new[] { 200, 206, 208, 211, 202, 203, 207, 209, 210, 204, 205 }
                        },
                    }
                },
                {
                    Chara.Aya,
                    new Dictionary<CardType, IReadOnlyList<int>>
                    {
                        {
                            CardType.Skill,
                            new[] { 100, 104, 108, 101, 105, 109, 103, 107, 111, 102, 106, 110 }
                        },
                        {
                            CardType.Spell,
                            new[] { 211, 205, 207, 200, 203, 212, 202, 208, 201, 206 }
                        },
                    }
                },
                {
                    Chara.Komachi,
                    new Dictionary<CardType, IReadOnlyList<int>>
                    {
                        {
                            CardType.Skill,
                            new[] { 100, 104, 108, 101, 105, 109, 103, 107, 111, 102, 106, 110 }
                        },
                        {
                            CardType.Spell,
                            new[] { 200, 202, 203, 205, 211, 206, 207, 201, 204 }
                        },
                    }
                },
                {
                    Chara.Iku,
                    new Dictionary<CardType, IReadOnlyList<int>>
                    {
                        {
                            CardType.Skill,
                            new[] { 100, 104, 108, 101, 105, 109, 103, 107, 111, 102, 106, 110 }
                        },
                        {
                            CardType.Spell,
                            new[] { 200, 208, 206, 201, 202, 203, 209, 207, 211, 210 }
                        },
                    }
                },
                {
                    Chara.Tenshi,
                    new Dictionary<CardType, IReadOnlyList<int>>
                    {
                        {
                            CardType.Skill,
                            new[] { 102, 106, 110, 103, 107, 111, 100, 104, 108, 101, 105, 109 }
                        },
                        {
                            CardType.Spell,
                            new[] { 200, 201, 202, 208, 209, 204, 206, 203, 205, 207 }
                        },
                    }
                },
                {
                    Chara.Sanae,
                    new Dictionary<CardType, IReadOnlyList<int>>
                    {
                        {
                            CardType.Skill,
                            new[] { 100, 104, 108, 102, 106, 110, 103, 107, 111, 101, 105, 109 }
                        },
                        {
                            CardType.Spell,
                            new[] { 200, 201, 203, 204, 206, 202, 207, 205, 210 }
                        },
                    }
                },
                {
                    Chara.Cirno,
                    new Dictionary<CardType, IReadOnlyList<int>>
                    {
                        {
                            CardType.Skill,
                            new[] { 100, 104, 108, 103, 107, 111, 101, 105, 109, 102, 106, 110 }
                        },
                        {
                            CardType.Spell,
                            new[] { 200, 205, 202, 203, 207, 210, 213, 201, 206, 204, 208 }
                        },
                    }
                },
                {
                    Chara.Meiling,
                    new Dictionary<CardType, IReadOnlyList<int>>
                    {
                        {
                            CardType.Skill,
                            new[] { 103, 107, 111, 101, 105, 109, 100, 104, 108, 102, 106, 110 }
                        },
                        {
                            CardType.Spell,
                            new[] { 200, 206, 202, 204, 207, 201, 208, 211, 203, 205, 209 }
                        },
                    }
                },
                {
                    Chara.Utsuho,
                    new Dictionary<CardType, IReadOnlyList<int>>
                    {
                        {
                            CardType.Skill,
                            new[] { 101, 105, 109, 100, 104, 108, 103, 107, 111, 102, 106, 110 }
                        },
                        {
                            CardType.Spell,
                            new[] { 206, 211, 213, 200, 203, 205, 208, 204, 207, 209, 210, 212, 201, 214 }
                        },
                    }
                },
                {
                    Chara.Suwako,
                    new Dictionary<CardType, IReadOnlyList<int>>
                    {
                        {
                            CardType.Skill,
                            new[] { 102, 106, 110, 101, 105, 109, 100, 104, 108, 103, 107, 111 }
                        },
                        {
                            CardType.Spell,
                            new[] { 201, 204, 200, 202, 203, 205, 206, 207, 209, 212, 208 }
                        },
                    }
                },
            };

        // Thanks to en.touhouwiki.net
        public static IReadOnlyDictionary<Chara, IReadOnlyList<StageInfo>> StageInfoTable { get; } =
            new Dictionary<Chara, IReadOnlyList<StageInfo>>
            {
                {
                    Chara.Sanae,
                    new StageInfo[]
                    {
                        new(Th105.Stage.One,   Chara.Cirno,   Enumerable.Range(0, 2)),
                        new(Th105.Stage.Two,   Chara.Meiling, Enumerable.Range(0, 2)),
                        new(Th105.Stage.Three, Chara.Reimu,   Enumerable.Range(0, 3)),
                        new(Th105.Stage.Four,  Chara.Utsuho,  Enumerable.Range(0, 4)),
                        new(Th105.Stage.Five,  Chara.Suwako,  Enumerable.Range(0, 5)),
                    }
                },
                {
                    Chara.Cirno,
                    new StageInfo[]
                    {
                        new(Th105.Stage.One,   Chara.Sanae,   Enumerable.Range(0, 2)),
                        new(Th105.Stage.Two,   Chara.Meiling, Enumerable.Range(0, 2)),
                        new(Th105.Stage.Three, Chara.Marisa,  Enumerable.Range(0, 3)),
                        new(Th105.Stage.Four,  Chara.Utsuho,  Enumerable.Range(0, 4)),
                        new(Th105.Stage.Five,  Chara.Alice,   Enumerable.Range(0, 5)),
                    }
                },
                {
                    Chara.Meiling,
                    new StageInfo[]
                    {
                        new(Th105.Stage.One,   Chara.Patchouli, Enumerable.Range(0, 2)),
                        new(Th105.Stage.Two,   Chara.Alice,     Enumerable.Range(0, 2)),
                        new(Th105.Stage.Three, Chara.Marisa,    Enumerable.Range(0, 3)),
                        new(Th105.Stage.Four,  Chara.Reimu,     Enumerable.Range(0, 4)),
                        new(Th105.Stage.Five,  Chara.Oonamazu,  Enumerable.Range(0, 5)),
                    }
                },
            };

        public static IReadOnlyDictionary<Chara, IEnumerable<(Chara Enemy, int CardId)>> EnemyCardIdTable { get; } =
            StageInfoTable.ToDictionary(
                stageInfoPair => stageInfoPair.Key,
                stageInfoPair => stageInfoPair.Value.SelectMany(
                    stageInfo => stageInfo.CardIds.Select(id => (stageInfo.Enemy, id))));

        public static string FormatPrefix { get; } = "%T123";
    }
}

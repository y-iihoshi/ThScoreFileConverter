//-----------------------------------------------------------------------
// <copyright file="Definitions.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using System.Linq;
using CardInfo = ThScoreFileConverter.Models.SpellCardInfo<
    ThScoreFileConverter.Models.Stage, ThScoreFileConverter.Models.Level>;

namespace ThScoreFileConverter.Models.Th06
{
    internal static class Definitions
    {
        public static IReadOnlyDictionary<int, CardInfo> CardTable { get; } = new List<CardInfo>
        {
#pragma warning disable SA1008 // Opening parenthesis should be spaced correctly
            new CardInfo( 1, "月符「ムーンライトレイ」",         Stage.St1,   Level.Hard, Level.Lunatic),
            new CardInfo( 2, "夜符「ナイトバード」",             Stage.St1,   Level.Easy, Level.Normal, Level.Hard, Level.Lunatic),
            new CardInfo( 3, "闇符「ディマーケイション」",       Stage.St1,   Level.Easy, Level.Normal, Level.Hard, Level.Lunatic),
            new CardInfo( 4, "氷符「アイシクルフォール」",       Stage.St2,   Level.Easy, Level.Normal),
            new CardInfo( 5, "雹符「ヘイルストーム」",           Stage.St2,   Level.Hard, Level.Lunatic),
            new CardInfo( 6, "凍符「パーフェクトフリーズ」",     Stage.St2,   Level.Easy, Level.Normal, Level.Hard, Level.Lunatic),
            new CardInfo( 7, "雪符「ダイアモンドブリザード」",   Stage.St2,   Level.Normal, Level.Hard, Level.Lunatic),
            new CardInfo( 8, "華符「芳華絢爛」",                 Stage.St3,   Level.Easy, Level.Normal),
            new CardInfo( 9, "華符「セラギネラ９」",             Stage.St3,   Level.Hard, Level.Lunatic),
            new CardInfo(10, "虹符「彩虹の風鈴」",               Stage.St3,   Level.Easy, Level.Normal, Level.Hard, Level.Lunatic),
            new CardInfo(11, "幻符「華想夢葛」",                 Stage.St3,   Level.Hard, Level.Lunatic),
            new CardInfo(12, "彩符「彩雨」",                     Stage.St3,   Level.Easy, Level.Normal),
            new CardInfo(13, "彩符「彩光乱舞」",                 Stage.St3,   Level.Hard, Level.Lunatic),
            new CardInfo(14, "彩符「極彩颱風」",                 Stage.St3,   Level.Normal, Level.Hard, Level.Lunatic),
            new CardInfo(15, "火符「アグニシャイン」",           Stage.St4,   Level.Easy, Level.Normal),
            new CardInfo(16, "水符「プリンセスウンディネ」",     Stage.St4,   Level.Easy, Level.Normal),
            new CardInfo(17, "木符「シルフィホルン」",           Stage.St4,   Level.Easy, Level.Normal),
            new CardInfo(18, "土符「レイジィトリリトン」",       Stage.St4,   Level.Easy, Level.Normal),
            new CardInfo(19, "金符「メタルファティーグ」",       Stage.St4,   Level.Normal),
            new CardInfo(20, "火符「アグニシャイン上級」",       Stage.St4,   Level.Normal, Level.Hard, Level.Lunatic),
            new CardInfo(21, "木符「シルフィホルン上級」",       Stage.St4,   Level.Normal, Level.Hard, Level.Lunatic),
            new CardInfo(22, "土符「レイジィトリリトン上級」",   Stage.St4,   Level.Normal, Level.Hard, Level.Lunatic),
            new CardInfo(23, "火符「アグニレイディアンス」",     Stage.St4,   Level.Hard, Level.Lunatic),
            new CardInfo(24, "水符「ベリーインレイク」",         Stage.St4,   Level.Hard, Level.Lunatic),
            new CardInfo(25, "木符「グリーンストーム」",         Stage.St4,   Level.Hard, Level.Lunatic),
            new CardInfo(26, "土符「トリリトンシェイク」",       Stage.St4,   Level.Hard, Level.Lunatic),
            new CardInfo(27, "金符「シルバードラゴン」",         Stage.St4,   Level.Hard, Level.Lunatic),
            new CardInfo(28, "火＆土符「ラーヴァクロムレク」",   Stage.St4,   Level.Easy, Level.Normal, Level.Hard, Level.Lunatic),
            new CardInfo(29, "木＆火符「フォレストブレイズ」",   Stage.St4,   Level.Easy, Level.Normal, Level.Hard, Level.Lunatic),
            new CardInfo(30, "水＆木符「ウォーターエルフ」",     Stage.St4,   Level.Easy, Level.Normal, Level.Hard, Level.Lunatic),
            new CardInfo(31, "金＆水符「マーキュリポイズン」",   Stage.St4,   Level.Normal, Level.Hard, Level.Lunatic),
            new CardInfo(32, "土＆金符「エメラルドメガリス」",   Stage.St4,   Level.Easy, Level.Normal, Level.Hard, Level.Lunatic),
            new CardInfo(33, "奇術「ミスディレクション」",       Stage.St5,   Level.Easy, Level.Normal),
            new CardInfo(34, "奇術「幻惑ミスディレクション」",   Stage.St5,   Level.Hard, Level.Lunatic),
            new CardInfo(35, "幻在「クロックコープス」",         Stage.St5,   Level.Easy, Level.Normal),
            new CardInfo(36, "幻象「ルナクロック」",             Stage.St5,   Level.Easy, Level.Normal),
            new CardInfo(37, "メイド秘技「操りドール」",         Stage.St5,   Level.Easy, Level.Normal),
            new CardInfo(38, "幻幽「ジャック・ザ・ルドビレ」",   Stage.St5,   Level.Hard, Level.Lunatic),
            new CardInfo(39, "幻世「ザ・ワールド」",             Stage.St5,   Level.Hard, Level.Lunatic),
            new CardInfo(40, "メイド秘技「殺人ドール」",         Stage.St5,   Level.Hard, Level.Lunatic),
            new CardInfo(41, "奇術「エターナルミーク」",         Stage.St6,   Level.Normal, Level.Hard, Level.Lunatic),
            new CardInfo(42, "天罰「スターオブダビデ」",         Stage.St6,   Level.Normal),
            new CardInfo(43, "冥符「紅色の冥界」",               Stage.St6,   Level.Normal),
            new CardInfo(44, "呪詛「ブラド・ツェペシュの呪い」", Stage.St6,   Level.Normal),
            new CardInfo(45, "紅符「スカーレットシュート」",     Stage.St6,   Level.Normal),
            new CardInfo(46, "「レッドマジック」",               Stage.St6,   Level.Normal),
            new CardInfo(47, "神罰「幼きデーモンロード」",       Stage.St6,   Level.Hard, Level.Lunatic),
            new CardInfo(48, "獄符「千本の針の山」",             Stage.St6,   Level.Hard, Level.Lunatic),
            new CardInfo(49, "神術「吸血鬼幻想」",               Stage.St6,   Level.Hard, Level.Lunatic),
            new CardInfo(50, "紅符「スカーレットマイスタ」",     Stage.St6,   Level.Hard, Level.Lunatic),
            new CardInfo(51, "「紅色の幻想郷」",                 Stage.St6,   Level.Hard, Level.Lunatic),
            new CardInfo(52, "月符「サイレントセレナ」",         Stage.Extra, Level.Extra),
            new CardInfo(53, "日符「ロイヤルフレア」",           Stage.Extra, Level.Extra),
            new CardInfo(54, "火水木金土符「賢者の石」",         Stage.Extra, Level.Extra),
            new CardInfo(55, "禁忌「クランベリートラップ」",     Stage.Extra, Level.Extra),
            new CardInfo(56, "禁忌「レーヴァテイン」",           Stage.Extra, Level.Extra),
            new CardInfo(57, "禁忌「フォーオブアカインド」",     Stage.Extra, Level.Extra),
            new CardInfo(58, "禁忌「カゴメカゴメ」",             Stage.Extra, Level.Extra),
            new CardInfo(59, "禁忌「恋の迷路」",                 Stage.Extra, Level.Extra),
            new CardInfo(60, "禁弾「スターボウブレイク」",       Stage.Extra, Level.Extra),
            new CardInfo(61, "禁弾「カタディオプトリック」",     Stage.Extra, Level.Extra),
            new CardInfo(62, "禁弾「過去を刻む時計」",           Stage.Extra, Level.Extra),
            new CardInfo(63, "秘弾「そして誰もいなくなるか？」", Stage.Extra, Level.Extra),
            new CardInfo(64, "ＱＥＤ「４９５年の波紋」",         Stage.Extra, Level.Extra),
#pragma warning restore SA1008 // Opening parenthesis should be spaced correctly
        }.ToDictionary(card => card.Id);

        public static IReadOnlyList<HighScore> InitialRanking { get; } =
            Enumerable.Range(1, 10).Reverse().Select(index => new HighScore((uint)index * 100000)).ToList();
    }
}

//-----------------------------------------------------------------------
// <copyright file="Definitions.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using System.Linq;
using ThScoreFileConverter.Core.Models;
using CardInfo = ThScoreFileConverter.Models.SpellCardInfo<
    ThScoreFileConverter.Core.Models.Stage, ThScoreFileConverter.Core.Models.Level>;
using IHighScore = ThScoreFileConverter.Models.Th06.IHighScore<
    ThScoreFileConverter.Models.Th06.Chara,
    ThScoreFileConverter.Core.Models.Level,
    ThScoreFileConverter.Models.Th06.StageProgress>;

namespace ThScoreFileConverter.Models.Th06;

internal static class Definitions
{
    public static IReadOnlyDictionary<int, CardInfo> CardTable { get; } = new CardInfo[]
    {
#pragma warning disable SA1008 // Opening parenthesis should be spaced correctly
        new( 1, "月符「ムーンライトレイ」",         Stage.One,   Level.Hard, Level.Lunatic),
        new( 2, "夜符「ナイトバード」",             Stage.One,   Level.Easy, Level.Normal, Level.Hard, Level.Lunatic),
        new( 3, "闇符「ディマーケイション」",       Stage.One,   Level.Easy, Level.Normal, Level.Hard, Level.Lunatic),
        new( 4, "氷符「アイシクルフォール」",       Stage.Two,   Level.Easy, Level.Normal),
        new( 5, "雹符「ヘイルストーム」",           Stage.Two,   Level.Hard, Level.Lunatic),
        new( 6, "凍符「パーフェクトフリーズ」",     Stage.Two,   Level.Easy, Level.Normal, Level.Hard, Level.Lunatic),
        new( 7, "雪符「ダイアモンドブリザード」",   Stage.Two,   Level.Normal, Level.Hard, Level.Lunatic),
        new( 8, "華符「芳華絢爛」",                 Stage.Three, Level.Easy, Level.Normal),
        new( 9, "華符「セラギネラ９」",             Stage.Three, Level.Hard, Level.Lunatic),
        new(10, "虹符「彩虹の風鈴」",               Stage.Three, Level.Easy, Level.Normal, Level.Hard, Level.Lunatic),
        new(11, "幻符「華想夢葛」",                 Stage.Three, Level.Hard, Level.Lunatic),
        new(12, "彩符「彩雨」",                     Stage.Three, Level.Easy, Level.Normal),
        new(13, "彩符「彩光乱舞」",                 Stage.Three, Level.Hard, Level.Lunatic),
        new(14, "彩符「極彩颱風」",                 Stage.Three, Level.Normal, Level.Hard, Level.Lunatic),
        new(15, "火符「アグニシャイン」",           Stage.Four,  Level.Easy, Level.Normal),
        new(16, "水符「プリンセスウンディネ」",     Stage.Four,  Level.Easy, Level.Normal),
        new(17, "木符「シルフィホルン」",           Stage.Four,  Level.Easy, Level.Normal),
        new(18, "土符「レイジィトリリトン」",       Stage.Four,  Level.Easy, Level.Normal),
        new(19, "金符「メタルファティーグ」",       Stage.Four,  Level.Normal),
        new(20, "火符「アグニシャイン上級」",       Stage.Four,  Level.Normal, Level.Hard, Level.Lunatic),
        new(21, "木符「シルフィホルン上級」",       Stage.Four,  Level.Normal, Level.Hard, Level.Lunatic),
        new(22, "土符「レイジィトリリトン上級」",   Stage.Four,  Level.Normal, Level.Hard, Level.Lunatic),
        new(23, "火符「アグニレイディアンス」",     Stage.Four,  Level.Hard, Level.Lunatic),
        new(24, "水符「ベリーインレイク」",         Stage.Four,  Level.Hard, Level.Lunatic),
        new(25, "木符「グリーンストーム」",         Stage.Four,  Level.Hard, Level.Lunatic),
        new(26, "土符「トリリトンシェイク」",       Stage.Four,  Level.Hard, Level.Lunatic),
        new(27, "金符「シルバードラゴン」",         Stage.Four,  Level.Hard, Level.Lunatic),
        new(28, "火＆土符「ラーヴァクロムレク」",   Stage.Four,  Level.Easy, Level.Normal, Level.Hard, Level.Lunatic),
        new(29, "木＆火符「フォレストブレイズ」",   Stage.Four,  Level.Easy, Level.Normal, Level.Hard, Level.Lunatic),
        new(30, "水＆木符「ウォーターエルフ」",     Stage.Four,  Level.Easy, Level.Normal, Level.Hard, Level.Lunatic),
        new(31, "金＆水符「マーキュリポイズン」",   Stage.Four,  Level.Normal, Level.Hard, Level.Lunatic),
        new(32, "土＆金符「エメラルドメガリス」",   Stage.Four,  Level.Easy, Level.Normal, Level.Hard, Level.Lunatic),
        new(33, "奇術「ミスディレクション」",       Stage.Five,  Level.Easy, Level.Normal),
        new(34, "奇術「幻惑ミスディレクション」",   Stage.Five,  Level.Hard, Level.Lunatic),
        new(35, "幻在「クロックコープス」",         Stage.Five,  Level.Easy, Level.Normal),
        new(36, "幻象「ルナクロック」",             Stage.Five,  Level.Easy, Level.Normal),
        new(37, "メイド秘技「操りドール」",         Stage.Five,  Level.Easy, Level.Normal),
        new(38, "幻幽「ジャック・ザ・ルドビレ」",   Stage.Five,  Level.Hard, Level.Lunatic),
        new(39, "幻世「ザ・ワールド」",             Stage.Five,  Level.Hard, Level.Lunatic),
        new(40, "メイド秘技「殺人ドール」",         Stage.Five,  Level.Hard, Level.Lunatic),
        new(41, "奇術「エターナルミーク」",         Stage.Six,   Level.Normal, Level.Hard, Level.Lunatic),
        new(42, "天罰「スターオブダビデ」",         Stage.Six,   Level.Normal),
        new(43, "冥符「紅色の冥界」",               Stage.Six,   Level.Normal),
        new(44, "呪詛「ブラド・ツェペシュの呪い」", Stage.Six,   Level.Normal),
        new(45, "紅符「スカーレットシュート」",     Stage.Six,   Level.Normal),
        new(46, "「レッドマジック」",               Stage.Six,   Level.Normal),
        new(47, "神罰「幼きデーモンロード」",       Stage.Six,   Level.Hard, Level.Lunatic),
        new(48, "獄符「千本の針の山」",             Stage.Six,   Level.Hard, Level.Lunatic),
        new(49, "神術「吸血鬼幻想」",               Stage.Six,   Level.Hard, Level.Lunatic),
        new(50, "紅符「スカーレットマイスタ」",     Stage.Six,   Level.Hard, Level.Lunatic),
        new(51, "「紅色の幻想郷」",                 Stage.Six,   Level.Hard, Level.Lunatic),
        new(52, "月符「サイレントセレナ」",         Stage.Extra, Level.Extra),
        new(53, "日符「ロイヤルフレア」",           Stage.Extra, Level.Extra),
        new(54, "火水木金土符「賢者の石」",         Stage.Extra, Level.Extra),
        new(55, "禁忌「クランベリートラップ」",     Stage.Extra, Level.Extra),
        new(56, "禁忌「レーヴァテイン」",           Stage.Extra, Level.Extra),
        new(57, "禁忌「フォーオブアカインド」",     Stage.Extra, Level.Extra),
        new(58, "禁忌「カゴメカゴメ」",             Stage.Extra, Level.Extra),
        new(59, "禁忌「恋の迷路」",                 Stage.Extra, Level.Extra),
        new(60, "禁弾「スターボウブレイク」",       Stage.Extra, Level.Extra),
        new(61, "禁弾「カタディオプトリック」",     Stage.Extra, Level.Extra),
        new(62, "禁弾「過去を刻む時計」",           Stage.Extra, Level.Extra),
        new(63, "秘弾「そして誰もいなくなるか？」", Stage.Extra, Level.Extra),
        new(64, "ＱＥＤ「４９５年の波紋」",         Stage.Extra, Level.Extra),
#pragma warning restore SA1008 // Opening parenthesis should be spaced correctly
    }.ToDictionary(card => card.Id);

    public static IReadOnlyList<IHighScore> InitialRanking { get; } =
        Enumerable.Range(1, 10).Reverse().Select(index => new HighScore((uint)index * 100000)).ToList();

    public static string FormatPrefix { get; } = "%T06";
}

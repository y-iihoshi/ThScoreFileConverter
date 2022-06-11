//-----------------------------------------------------------------------
// <copyright file="Definitions.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
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
    /// Gets the list of achievements.
    /// Thanks to en.touhouwiki.net.
    /// </summary>
    public static IReadOnlyList<string> Achievements { get; } = new List<string>
    {
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
    };
}

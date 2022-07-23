﻿//-----------------------------------------------------------------------
// <copyright file="Definitions.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using CardInfo = ThScoreFileConverter.Core.Models.SpellCardInfo<
    ThScoreFileConverter.Core.Models.Stage, ThScoreFileConverter.Core.Models.Level>;

namespace ThScoreFileConverter.Core.Models.Th11;

/// <summary>
/// Provides several SA specific definitions.
/// </summary>
public static class Definitions
{
    /// <summary>
    /// Gets the dictionary of SA spell cards.
    /// Thanks to thwiki.info.
    /// </summary>
    public static IReadOnlyDictionary<int, CardInfo> CardTable { get; } = new CardInfo[]
    {
#pragma warning disable SA1008 // Opening parenthesis should be spaced correctly
        new(  1, "怪奇「釣瓶落としの怪」",               Stage.One,   Level.Hard),
        new(  2, "怪奇「釣瓶落としの怪」",               Stage.One,   Level.Lunatic),
        new(  3, "罠符「キャプチャーウェブ」",           Stage.One,   Level.Easy),
        new(  4, "罠符「キャプチャーウェブ」",           Stage.One,   Level.Normal),
        new(  5, "蜘蛛「石窟の蜘蛛の巣」",               Stage.One,   Level.Hard),
        new(  6, "蜘蛛「石窟の蜘蛛の巣」",               Stage.One,   Level.Lunatic),
        new(  7, "瘴符「フィルドミアズマ」",             Stage.One,   Level.Easy),
        new(  8, "瘴符「フィルドミアズマ」",             Stage.One,   Level.Normal),
        new(  9, "瘴気「原因不明の熱病」",               Stage.One,   Level.Hard),
        new( 10, "瘴気「原因不明の熱病」",               Stage.One,   Level.Lunatic),
        new( 11, "妬符「グリーンアイドモンスター」",     Stage.Two,   Level.Easy),
        new( 12, "妬符「グリーンアイドモンスター」",     Stage.Two,   Level.Normal),
        new( 13, "嫉妬「緑色の目をした見えない怪物」",   Stage.Two,   Level.Hard),
        new( 14, "嫉妬「緑色の目をした見えない怪物」",   Stage.Two,   Level.Lunatic),
        new( 15, "花咲爺「華やかなる仁者への嫉妬」",     Stage.Two,   Level.Easy),
        new( 16, "花咲爺「華やかなる仁者への嫉妬」",     Stage.Two,   Level.Normal),
        new( 17, "花咲爺「シロの灰」",                   Stage.Two,   Level.Hard),
        new( 18, "花咲爺「シロの灰」",                   Stage.Two,   Level.Lunatic),
        new( 19, "舌切雀「謙虚なる富者への片恨」",       Stage.Two,   Level.Easy),
        new( 20, "舌切雀「謙虚なる富者への片恨」",       Stage.Two,   Level.Normal),
        new( 21, "舌切雀「大きな葛籠と小さな葛籠」",     Stage.Two,   Level.Hard),
        new( 22, "舌切雀「大きな葛籠と小さな葛籠」",     Stage.Two,   Level.Lunatic),
        new( 23, "恨符「丑の刻参り」",                   Stage.Two,   Level.Easy),
        new( 24, "恨符「丑の刻参り」",                   Stage.Two,   Level.Normal),
        new( 25, "恨符「丑の刻参り七日目」",             Stage.Two,   Level.Hard),
        new( 26, "恨符「丑の刻参り七日目」",             Stage.Two,   Level.Lunatic),
        new( 27, "鬼符「怪力乱神」",                     Stage.Three, Level.Easy),
        new( 28, "鬼符「怪力乱神」",                     Stage.Three, Level.Normal),
        new( 29, "鬼符「怪力乱神」",                     Stage.Three, Level.Hard),
        new( 30, "鬼符「怪力乱神」",                     Stage.Three, Level.Lunatic),
        new( 31, "怪輪「地獄の苦輪」",                   Stage.Three, Level.Easy),
        new( 32, "怪輪「地獄の苦輪」",                   Stage.Three, Level.Normal),
        new( 33, "枷符「咎人の外さぬ枷」",               Stage.Three, Level.Hard),
        new( 34, "枷符「咎人の外さぬ枷」",               Stage.Three, Level.Lunatic),
        new( 35, "力業「大江山嵐」",                     Stage.Three, Level.Easy),
        new( 36, "力業「大江山嵐」",                     Stage.Three, Level.Normal),
        new( 37, "力業「大江山颪」",                     Stage.Three, Level.Hard),
        new( 38, "力業「大江山颪」",                     Stage.Three, Level.Lunatic),
        new( 39, "四天王奥義「三歩必殺」",               Stage.Three, Level.Easy),
        new( 40, "四天王奥義「三歩必殺」",               Stage.Three, Level.Normal),
        new( 41, "四天王奥義「三歩必殺」",               Stage.Three, Level.Hard),
        new( 42, "四天王奥義「三歩必殺」",               Stage.Three, Level.Lunatic),
        new( 43, "想起「テリブルスーヴニール」",         Stage.Four,  Level.Easy),
        new( 44, "想起「テリブルスーヴニール」",         Stage.Four,  Level.Normal),
        new( 45, "想起「恐怖催眠術」",                   Stage.Four,  Level.Hard),
        new( 46, "想起「恐怖催眠術」",                   Stage.Four,  Level.Lunatic),
        new( 47, "想起「二重黒死蝶」",                   Stage.Four,  Level.Easy),
        new( 48, "想起「二重黒死蝶」",                   Stage.Four,  Level.Normal),
        new( 49, "想起「二重黒死蝶」",                   Stage.Four,  Level.Hard),
        new( 50, "想起「二重黒死蝶」",                   Stage.Four,  Level.Lunatic),
        new( 51, "想起「飛行虫ネスト」",                 Stage.Four,  Level.Easy),
        new( 52, "想起「飛行虫ネスト」",                 Stage.Four,  Level.Normal),
        new( 53, "想起「飛行虫ネスト」",                 Stage.Four,  Level.Hard),
        new( 54, "想起「飛行虫ネスト」",                 Stage.Four,  Level.Lunatic),
        new( 55, "想起「波と粒の境界」",                 Stage.Four,  Level.Easy),
        new( 56, "想起「波と粒の境界」",                 Stage.Four,  Level.Normal),
        new( 57, "想起「波と粒の境界」",                 Stage.Four,  Level.Hard),
        new( 58, "想起「波と粒の境界」",                 Stage.Four,  Level.Lunatic),
        new( 59, "想起「戸隠山投げ」",                   Stage.Four,  Level.Easy),
        new( 60, "想起「戸隠山投げ」",                   Stage.Four,  Level.Normal),
        new( 61, "想起「戸隠山投げ」",                   Stage.Four,  Level.Hard),
        new( 62, "想起「戸隠山投げ」",                   Stage.Four,  Level.Lunatic),
        new( 63, "想起「百万鬼夜行」",                   Stage.Four,  Level.Easy),
        new( 64, "想起「百万鬼夜行」",                   Stage.Four,  Level.Normal),
        new( 65, "想起「百万鬼夜行」",                   Stage.Four,  Level.Hard),
        new( 66, "想起「百万鬼夜行」",                   Stage.Four,  Level.Lunatic),
        new( 67, "想起「濛々迷霧」",                     Stage.Four,  Level.Easy),
        new( 68, "想起「濛々迷霧」",                     Stage.Four,  Level.Normal),
        new( 69, "想起「濛々迷霧」",                     Stage.Four,  Level.Hard),
        new( 70, "想起「濛々迷霧」",                     Stage.Four,  Level.Lunatic),
        new( 71, "想起「風神木の葉隠れ」",               Stage.Four,  Level.Easy),
        new( 72, "想起「風神木の葉隠れ」",               Stage.Four,  Level.Normal),
        new( 73, "想起「風神木の葉隠れ」",               Stage.Four,  Level.Hard),
        new( 74, "想起「風神木の葉隠れ」",               Stage.Four,  Level.Lunatic),
        new( 75, "想起「天狗のマクロバースト」",         Stage.Four,  Level.Easy),
        new( 76, "想起「天狗のマクロバースト」",         Stage.Four,  Level.Normal),
        new( 77, "想起「天狗のマクロバースト」",         Stage.Four,  Level.Hard),
        new( 78, "想起「天狗のマクロバースト」",         Stage.Four,  Level.Lunatic),
        new( 79, "想起「鳥居つむじ風」",                 Stage.Four,  Level.Easy),
        new( 80, "想起「鳥居つむじ風」",                 Stage.Four,  Level.Normal),
        new( 81, "想起「鳥居つむじ風」",                 Stage.Four,  Level.Hard),
        new( 82, "想起「鳥居つむじ風」",                 Stage.Four,  Level.Lunatic),
        new( 83, "想起「春の京人形」",                   Stage.Four,  Level.Easy),
        new( 84, "想起「春の京人形」",                   Stage.Four,  Level.Normal),
        new( 85, "想起「春の京人形」",                   Stage.Four,  Level.Hard),
        new( 86, "想起「春の京人形」",                   Stage.Four,  Level.Lunatic),
        new( 87, "想起「ストロードールカミカゼ」",       Stage.Four,  Level.Easy),
        new( 88, "想起「ストロードールカミカゼ」",       Stage.Four,  Level.Normal),
        new( 89, "想起「ストロードールカミカゼ」",       Stage.Four,  Level.Hard),
        new( 90, "想起「ストロードールカミカゼ」",       Stage.Four,  Level.Lunatic),
        new( 91, "想起「リターンイナニメトネス」",       Stage.Four,  Level.Easy),
        new( 92, "想起「リターンイナニメトネス」",       Stage.Four,  Level.Normal),
        new( 93, "想起「リターンイナニメトネス」",       Stage.Four,  Level.Hard),
        new( 94, "想起「リターンイナニメトネス」",       Stage.Four,  Level.Lunatic),
        new( 95, "想起「マーキュリポイズン」",           Stage.Four,  Level.Easy),
        new( 96, "想起「マーキュリポイズン」",           Stage.Four,  Level.Normal),
        new( 97, "想起「マーキュリポイズン」",           Stage.Four,  Level.Hard),
        new( 98, "想起「マーキュリポイズン」",           Stage.Four,  Level.Lunatic),
        new( 99, "想起「プリンセスウンディネ」",         Stage.Four,  Level.Easy),
        new(100, "想起「プリンセスウンディネ」",         Stage.Four,  Level.Normal),
        new(101, "想起「プリンセスウンディネ」",         Stage.Four,  Level.Hard),
        new(102, "想起「プリンセスウンディネ」",         Stage.Four,  Level.Lunatic),
        new(103, "想起「賢者の石」",                     Stage.Four,  Level.Easy),
        new(104, "想起「賢者の石」",                     Stage.Four,  Level.Normal),
        new(105, "想起「賢者の石」",                     Stage.Four,  Level.Hard),
        new(106, "想起「賢者の石」",                     Stage.Four,  Level.Lunatic),
        new(107, "想起「のびーるアーム」",               Stage.Four,  Level.Easy),
        new(108, "想起「のびーるアーム」",               Stage.Four,  Level.Normal),
        new(109, "想起「のびーるアーム」",               Stage.Four,  Level.Hard),
        new(110, "想起「のびーるアーム」",               Stage.Four,  Level.Lunatic),
        new(111, "想起「河童のポロロッカ」",             Stage.Four,  Level.Easy),
        new(112, "想起「河童のポロロッカ」",             Stage.Four,  Level.Normal),
        new(113, "想起「河童のポロロッカ」",             Stage.Four,  Level.Hard),
        new(114, "想起「河童のポロロッカ」",             Stage.Four,  Level.Lunatic),
        new(115, "想起「光り輝く水底のトラウマ」",       Stage.Four,  Level.Easy),
        new(116, "想起「光り輝く水底のトラウマ」",       Stage.Four,  Level.Normal),
        new(117, "想起「光り輝く水底のトラウマ」",       Stage.Four,  Level.Hard),
        new(118, "想起「光り輝く水底のトラウマ」",       Stage.Four,  Level.Lunatic),
        new(119, "猫符「キャッツウォーク」",             Stage.Five,  Level.Easy),
        new(120, "猫符「キャッツウォーク」",             Stage.Five,  Level.Normal),
        new(121, "猫符「怨霊猫乱歩」",                   Stage.Five,  Level.Hard),
        new(122, "猫符「怨霊猫乱歩」",                   Stage.Five,  Level.Lunatic),
        new(123, "呪精「ゾンビフェアリー」",             Stage.Five,  Level.Easy),
        new(124, "呪精「ゾンビフェアリー」",             Stage.Five,  Level.Normal),
        new(125, "呪精「怨霊憑依妖精」",                 Stage.Five,  Level.Hard),
        new(126, "呪精「怨霊憑依妖精」",                 Stage.Five,  Level.Lunatic),
        new(127, "恨霊「スプリーンイーター」",           Stage.Five,  Level.Easy),
        new(128, "恨霊「スプリーンイーター」",           Stage.Five,  Level.Normal),
        new(129, "屍霊「食人怨霊」",                     Stage.Five,  Level.Hard),
        new(130, "屍霊「食人怨霊」",                     Stage.Five,  Level.Lunatic),
        new(131, "贖罪「旧地獄の針山」",                 Stage.Five,  Level.Easy),
        new(132, "贖罪「旧地獄の針山」",                 Stage.Five,  Level.Normal),
        new(133, "贖罪「昔時の針と痛がる怨霊」",         Stage.Five,  Level.Hard),
        new(134, "贖罪「昔時の針と痛がる怨霊」",         Stage.Five,  Level.Lunatic),
        new(135, "「死灰復燃」",                         Stage.Five,  Level.Easy),
        new(136, "「死灰復燃」",                         Stage.Five,  Level.Normal),
        new(137, "「小悪霊復活せし」",                   Stage.Five,  Level.Hard),
        new(138, "「小悪霊復活せし」",                   Stage.Five,  Level.Lunatic),
        new(139, "妖怪「火焔の車輪」",                   Stage.Six,   Level.Easy),
        new(140, "妖怪「火焔の車輪」",                   Stage.Six,   Level.Normal),
        new(141, "妖怪「火焔の車輪」",                   Stage.Six,   Level.Hard),
        new(142, "妖怪「火焔の車輪」",                   Stage.Six,   Level.Lunatic),
        new(143, "核熱「ニュークリアフュージョン」",     Stage.Six,   Level.Easy),
        new(144, "核熱「ニュークリアフュージョン」",     Stage.Six,   Level.Normal),
        new(145, "核熱「ニュークリアエクスカーション」", Stage.Six,   Level.Hard),
        new(146, "核熱「核反応制御不能」",               Stage.Six,   Level.Lunatic),
        new(147, "爆符「プチフレア」",                   Stage.Six,   Level.Easy),
        new(148, "爆符「メガフレア」",                   Stage.Six,   Level.Normal),
        new(149, "爆符「ギガフレア」",                   Stage.Six,   Level.Hard),
        new(150, "爆符「ペタフレア」",                   Stage.Six,   Level.Lunatic),
        new(151, "焔星「フィクストスター」",             Stage.Six,   Level.Easy),
        new(152, "焔星「フィクストスター」",             Stage.Six,   Level.Normal),
        new(153, "焔星「プラネタリーレボリューション」", Stage.Six,   Level.Hard),
        new(154, "焔星「十凶星」",                       Stage.Six,   Level.Lunatic),
        new(155, "「地獄極楽メルトダウン」",             Stage.Six,   Level.Easy),
        new(156, "「地獄極楽メルトダウン」",             Stage.Six,   Level.Normal),
        new(157, "「ヘルズトカマク」",                   Stage.Six,   Level.Hard),
        new(158, "「ヘルズトカマク」",                   Stage.Six,   Level.Lunatic),
        new(159, "「地獄の人工太陽」",                   Stage.Six,   Level.Easy),
        new(160, "「地獄の人工太陽」",                   Stage.Six,   Level.Normal),
        new(161, "「サブタレイニアンサン」",             Stage.Six,   Level.Hard),
        new(162, "「サブタレイニアンサン」",             Stage.Six,   Level.Lunatic),
        new(163, "秘法「九字刺し」",                     Stage.Extra, Level.Extra),
        new(164, "奇跡「ミラクルフルーツ」",             Stage.Extra, Level.Extra),
        new(165, "神徳「五穀豊穣ライスシャワー」",       Stage.Extra, Level.Extra),
        new(166, "表象「夢枕にご先祖総立ち」",           Stage.Extra, Level.Extra),
        new(167, "表象「弾幕パラノイア」",               Stage.Extra, Level.Extra),
        new(168, "本能「イドの解放」",                   Stage.Extra, Level.Extra),
        new(169, "抑制「スーパーエゴ」",                 Stage.Extra, Level.Extra),
        new(170, "反応「妖怪ポリグラフ」",               Stage.Extra, Level.Extra),
        new(171, "無意識「弾幕のロールシャッハ」",       Stage.Extra, Level.Extra),
        new(172, "復燃「恋の埋火」",                     Stage.Extra, Level.Extra),
        new(173, "深層「無意識の遺伝子」",               Stage.Extra, Level.Extra),
        new(174, "「嫌われ者のフィロソフィ」",           Stage.Extra, Level.Extra),
        new(175, "「サブタレイニアンローズ」",           Stage.Extra, Level.Extra),
#pragma warning restore SA1008 // Opening parenthesis should be spaced correctly
    }.ToDictionary(static card => card.Id);
}

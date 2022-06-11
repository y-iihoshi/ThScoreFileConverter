﻿//-----------------------------------------------------------------------
// <copyright file="Definitions.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using CardInfo = ThScoreFileConverter.Core.Models.SpellCardInfo<
    ThScoreFileConverter.Core.Models.Th128.Stage, ThScoreFileConverter.Core.Models.Level>;

namespace ThScoreFileConverter.Core.Models.Th128;

/// <summary>
/// Provides several FW definitions.
/// </summary>
public static class Definitions
{
    /// <summary>
    /// Gets the dictionary of FW spell cards.
    /// Thanks to thwiki.info.
    /// </summary>
    public static IReadOnlyDictionary<int, CardInfo> CardTable { get; } = new CardInfo[]
    {
#pragma warning disable SA1008 // Opening parenthesis should be spaced correctly
        new(  1, "月符「ルナティックレイン」",               Stage.A_1,   Level.Easy),
        new(  2, "月符「ルナティックレイン」",               Stage.A_1,   Level.Normal),
        new(  3, "月符「ルナティックレイン」",               Stage.A_1,   Level.Hard),
        new(  4, "月符「ルナティックレイン」",               Stage.A_1,   Level.Lunatic),
        new(  5, "月符「ルナサイクロン」",                   Stage.A_1,   Level.Easy),
        new(  6, "月符「ルナサイクロン」",                   Stage.A_1,   Level.Normal),
        new(  7, "月符「ルナサイクロン」",                   Stage.A_1,   Level.Hard),
        new(  8, "月符「ルナサイクロン」",                   Stage.A_1,   Level.Lunatic),
        new(  9, "流星「プチコメット」",                     Stage.A1_2,  Level.Easy),
        new( 10, "流星「プチコメット」",                     Stage.A1_2,  Level.Normal),
        new( 11, "流星「プチコメット」",                     Stage.A1_2,  Level.Hard),
        new( 12, "流星「プチコメット」",                     Stage.A1_2,  Level.Lunatic),
        new( 13, "星粒「スプリンクルピース」",               Stage.A1_2,  Level.Easy),
        new( 14, "星粒「スプリンクルピース」",               Stage.A1_2,  Level.Normal),
        new( 15, "星粒「スプリンクルピース」",               Stage.A1_2,  Level.Hard),
        new( 16, "星粒「スプリンクルピース」",               Stage.A1_2,  Level.Lunatic),
        new( 17, "星符「トゥインクルサファイア」",           Stage.A1_2,  Level.Easy),
        new( 18, "星符「トゥインクルサファイア」",           Stage.A1_2,  Level.Normal),
        new( 19, "星符「トゥインクルサファイア」",           Stage.A1_2,  Level.Hard),
        new( 20, "星符「トゥインクルサファイア」",           Stage.A1_2,  Level.Lunatic),
        new( 21, "陽光「サンシャインブラスト」",             Stage.A1_3,  Level.Easy),
        new( 22, "陽光「サンシャインブラスト」",             Stage.A1_3,  Level.Normal),
        new( 23, "陽光「サンシャインブラスト」",             Stage.A1_3,  Level.Hard),
        new( 24, "陽光「サンシャインブラスト」",             Stage.A1_3,  Level.Lunatic),
        new( 25, "光符「ルチルフレクション」",               Stage.A1_3,  Level.Easy),
        new( 26, "光符「ルチルフレクション」",               Stage.A1_3,  Level.Normal),
        new( 27, "光符「ルチルフレクション」",               Stage.A1_3,  Level.Hard),
        new( 28, "光符「ルチルフレクション」",               Stage.A1_3,  Level.Lunatic),
        new( 29, "日熱「アイスディゾルバー」",               Stage.A1_3,  Level.Easy),
        new( 30, "日熱「アイスディゾルバー」",               Stage.A1_3,  Level.Normal),
        new( 31, "日熱「アイスディゾルバー」",               Stage.A1_3,  Level.Hard),
        new( 32, "日熱「アイスディゾルバー」",               Stage.A1_3,  Level.Lunatic),
        new( 33, "空符「エルフィンキャノピー」",             Stage.A1_3,  Level.Easy),
        new( 34, "空符「エルフィンキャノピー」",             Stage.A1_3,  Level.Normal),
        new( 35, "空符「エルフィンキャノピー」",             Stage.A1_3,  Level.Hard),
        new( 36, "空符「エルフィンキャノピー」",             Stage.A1_3,  Level.Lunatic),
        new( 37, "協力技「フェアリーオーバードライブ」",     Stage.A1_3,  Level.Easy),
        new( 38, "協力技「フェアリーオーバードライブ」",     Stage.A1_3,  Level.Normal),
        new( 39, "協力技「フェアリーオーバードライブ」",     Stage.A1_3,  Level.Hard),
        new( 40, "協力技「フェアリーオーバードライブ」",     Stage.A1_3,  Level.Lunatic),
        new( 41, "「スリーフェアリーズ」",                   Stage.A1_3,  Level.Easy),
        new( 42, "「スリーフェアリーズ」",                   Stage.A1_3,  Level.Normal),
        new( 43, "「スリーフェアリーズ」",                   Stage.A1_3,  Level.Hard),
        new( 44, "「スリーフェアリーズ」",                   Stage.A1_3,  Level.Lunatic),
        new( 45, "虹光「プリズムフラッシュ」",               Stage.A2_2,  Level.Easy),
        new( 46, "虹光「プリズムフラッシュ」",               Stage.A2_2,  Level.Normal),
        new( 47, "虹光「プリズムフラッシュ」",               Stage.A2_2,  Level.Hard),
        new( 48, "虹光「プリズムフラッシュ」",               Stage.A2_2,  Level.Lunatic),
        new( 49, "光精「ダイアモンドリング」",               Stage.A2_2,  Level.Easy),
        new( 50, "光精「ダイアモンドリング」",               Stage.A2_2,  Level.Normal),
        new( 51, "光精「ダイアモンドリング」",               Stage.A2_2,  Level.Hard),
        new( 52, "光精「ダイアモンドリング」",               Stage.A2_2,  Level.Lunatic),
        new( 53, "光符「ブルーディフレクション」",           Stage.A2_2,  Level.Easy),
        new( 54, "光符「ブルーディフレクション」",           Stage.A2_2,  Level.Normal),
        new( 55, "光符「ブルーディフレクション」",           Stage.A2_2,  Level.Hard),
        new( 56, "光符「ブルーディフレクション」",           Stage.A2_2,  Level.Lunatic),
        new( 57, "星光「スターレーザー」",                   Stage.A2_3,  Level.Easy),
        new( 58, "星光「スターレーザー」",                   Stage.A2_3,  Level.Normal),
        new( 59, "星光「スターレーザー」",                   Stage.A2_3,  Level.Hard),
        new( 60, "星光「スターレーザー」",                   Stage.A2_3,  Level.Lunatic),
        new( 61, "光符「トリプルメテオ」",                   Stage.A2_3,  Level.Easy),
        new( 62, "光符「トリプルメテオ」",                   Stage.A2_3,  Level.Normal),
        new( 63, "光符「トリプルメテオ」",                   Stage.A2_3,  Level.Hard),
        new( 64, "光符「トリプルメテオ」",                   Stage.A2_3,  Level.Lunatic),
        new( 65, "星熱「アイスディゾルバー」",               Stage.A2_3,  Level.Easy),
        new( 66, "星熱「アイスディゾルバー」",               Stage.A2_3,  Level.Normal),
        new( 67, "星熱「アイスディゾルバー」",               Stage.A2_3,  Level.Hard),
        new( 68, "星熱「アイスディゾルバー」",               Stage.A2_3,  Level.Lunatic),
        new( 69, "光星「オリオンベルト」",                   Stage.A2_3,  Level.Easy),
        new( 70, "光星「オリオンベルト」",                   Stage.A2_3,  Level.Normal),
        new( 71, "光星「オリオンベルト」",                   Stage.A2_3,  Level.Hard),
        new( 72, "光星「オリオンベルト」",                   Stage.A2_3,  Level.Lunatic),
        new( 73, "協力技「フェアリーオーバードライブ」",     Stage.A2_3,  Level.Easy),
        new( 74, "協力技「フェアリーオーバードライブ」",     Stage.A2_3,  Level.Normal),
        new( 75, "協力技「フェアリーオーバードライブ」",     Stage.A2_3,  Level.Hard),
        new( 76, "協力技「フェアリーオーバードライブ」",     Stage.A2_3,  Level.Lunatic),
        new( 77, "「スリーフェアリーズ」",                   Stage.A2_3,  Level.Easy),
        new( 78, "「スリーフェアリーズ」",                   Stage.A2_3,  Level.Normal),
        new( 79, "「スリーフェアリーズ」",                   Stage.A2_3,  Level.Hard),
        new( 80, "「スリーフェアリーズ」",                   Stage.A2_3,  Level.Lunatic),
        new( 81, "日符「アグレッシブライト」",               Stage.B_1,   Level.Easy),
        new( 82, "日符「アグレッシブライト」",               Stage.B_1,   Level.Normal),
        new( 83, "日符「アグレッシブライト」",               Stage.B_1,   Level.Hard),
        new( 84, "日符「アグレッシブライト」",               Stage.B_1,   Level.Lunatic),
        new( 85, "日符「ダイレクトサンライト」",             Stage.B_1,   Level.Easy),
        new( 86, "日符「ダイレクトサンライト」",             Stage.B_1,   Level.Normal),
        new( 87, "日符「ダイレクトサンライト」",             Stage.B_1,   Level.Hard),
        new( 88, "日符「ダイレクトサンライト」",             Stage.B_1,   Level.Lunatic),
        new( 89, "流星「コメットストリーム」",               Stage.B1_2,  Level.Easy),
        new( 90, "流星「コメットストリーム」",               Stage.B1_2,  Level.Normal),
        new( 91, "流星「コメットストリーム」",               Stage.B1_2,  Level.Hard),
        new( 92, "流星「コメットストリーム」",               Stage.B1_2,  Level.Lunatic),
        new( 93, "星粒「スターピースシャワー」",             Stage.B1_2,  Level.Easy),
        new( 94, "星粒「スターピースシャワー」",             Stage.B1_2,  Level.Normal),
        new( 95, "星粒「スターピースシャワー」",             Stage.B1_2,  Level.Hard),
        new( 96, "星粒「スターピースシャワー」",             Stage.B1_2,  Level.Lunatic),
        new( 97, "星符「シューティングサファイア」",         Stage.B1_2,  Level.Easy),
        new( 98, "星符「シューティングサファイア」",         Stage.B1_2,  Level.Normal),
        new( 99, "星符「シューティングサファイア」",         Stage.B1_2,  Level.Hard),
        new(100, "星符「シューティングサファイア」",         Stage.B1_2,  Level.Lunatic),
        new(101, "月光「サイレントストーム」",               Stage.B1_3,  Level.Easy),
        new(102, "月光「サイレントストーム」",               Stage.B1_3,  Level.Normal),
        new(103, "月光「サイレントストーム」",               Stage.B1_3,  Level.Hard),
        new(104, "月光「サイレントストーム」",               Stage.B1_3,  Level.Lunatic),
        new(105, "光符「ブライトナイト」",                   Stage.B1_3,  Level.Easy),
        new(106, "光符「ブライトナイト」",                   Stage.B1_3,  Level.Normal),
        new(107, "光符「ブライトナイト」",                   Stage.B1_3,  Level.Hard),
        new(108, "光符「ブライトナイト」",                   Stage.B1_3,  Level.Lunatic),
        new(109, "月熱「アイスディゾルバー」",               Stage.B1_3,  Level.Easy),
        new(110, "月熱「アイスディゾルバー」",               Stage.B1_3,  Level.Normal),
        new(111, "月熱「アイスディゾルバー」",               Stage.B1_3,  Level.Hard),
        new(112, "月熱「アイスディゾルバー」",               Stage.B1_3,  Level.Lunatic),
        new(113, "空符「ブレイクキャノピー」",               Stage.B1_3,  Level.Easy),
        new(114, "空符「ブレイクキャノピー」",               Stage.B1_3,  Level.Normal),
        new(115, "空符「ブレイクキャノピー」",               Stage.B1_3,  Level.Hard),
        new(116, "空符「ブレイクキャノピー」",               Stage.B1_3,  Level.Lunatic),
        new(117, "協力技「フェアリーオーバードライブ」",     Stage.B1_3,  Level.Easy),
        new(118, "協力技「フェアリーオーバードライブ」",     Stage.B1_3,  Level.Normal),
        new(119, "協力技「フェアリーオーバードライブ」",     Stage.B1_3,  Level.Hard),
        new(120, "協力技「フェアリーオーバードライブ」",     Stage.B1_3,  Level.Lunatic),
        new(121, "「スリーフェアリーズ」",                   Stage.B1_3,  Level.Easy),
        new(122, "「スリーフェアリーズ」",                   Stage.B1_3,  Level.Normal),
        new(123, "「スリーフェアリーズ」",                   Stage.B1_3,  Level.Hard),
        new(124, "「スリーフェアリーズ」",                   Stage.B1_3,  Level.Lunatic),
        new(125, "月光「ダークスティルネス」",               Stage.B2_2,  Level.Easy),
        new(126, "月光「ダークスティルネス」",               Stage.B2_2,  Level.Normal),
        new(127, "月光「ダークスティルネス」",               Stage.B2_2,  Level.Hard),
        new(128, "月光「ダークスティルネス」",               Stage.B2_2,  Level.Lunatic),
        new(129, "障光「ムーンライトウォール」",             Stage.B2_2,  Level.Easy),
        new(130, "障光「ムーンライトウォール」",             Stage.B2_2,  Level.Normal),
        new(131, "障光「ムーンライトウォール」",             Stage.B2_2,  Level.Hard),
        new(132, "障光「ムーンライトウォール」",             Stage.B2_2,  Level.Lunatic),
        new(133, "夜符「ナイトフェアリーズ」",               Stage.B2_2,  Level.Easy),
        new(134, "夜符「ナイトフェアリーズ」",               Stage.B2_2,  Level.Normal),
        new(135, "夜符「ナイトフェアリーズ」",               Stage.B2_2,  Level.Hard),
        new(136, "夜符「ナイトフェアリーズ」",               Stage.B2_2,  Level.Lunatic),
        new(137, "星光「スターストーム」",                   Stage.B2_3,  Level.Easy),
        new(138, "星光「スターストーム」",                   Stage.B2_3,  Level.Normal),
        new(139, "星光「スターストーム」",                   Stage.B2_3,  Level.Hard),
        new(140, "星光「スターストーム」",                   Stage.B2_3,  Level.Lunatic),
        new(141, "光符「エクステンシブメテオ」",             Stage.B2_3,  Level.Easy),
        new(142, "光符「エクステンシブメテオ」",             Stage.B2_3,  Level.Normal),
        new(143, "光符「エクステンシブメテオ」",             Stage.B2_3,  Level.Hard),
        new(144, "光符「エクステンシブメテオ」",             Stage.B2_3,  Level.Lunatic),
        new(145, "星熱「アイスディゾルバー」",               Stage.B2_3,  Level.Easy),
        new(146, "星熱「アイスディゾルバー」",               Stage.B2_3,  Level.Normal),
        new(147, "星熱「アイスディゾルバー」",               Stage.B2_3,  Level.Hard),
        new(148, "星熱「アイスディゾルバー」",               Stage.B2_3,  Level.Lunatic),
        new(149, "光星「グレートトライアングル」",           Stage.B2_3,  Level.Easy),
        new(150, "光星「グレートトライアングル」",           Stage.B2_3,  Level.Normal),
        new(151, "光星「グレートトライアングル」",           Stage.B2_3,  Level.Hard),
        new(152, "光星「グレートトライアングル」",           Stage.B2_3,  Level.Lunatic),
        new(153, "協力技「フェアリーオーバードライブ」",     Stage.B2_3,  Level.Easy),
        new(154, "協力技「フェアリーオーバードライブ」",     Stage.B2_3,  Level.Normal),
        new(155, "協力技「フェアリーオーバードライブ」",     Stage.B2_3,  Level.Hard),
        new(156, "協力技「フェアリーオーバードライブ」",     Stage.B2_3,  Level.Lunatic),
        new(157, "「スリーフェアリーズ」",                   Stage.B2_3,  Level.Easy),
        new(158, "「スリーフェアリーズ」",                   Stage.B2_3,  Level.Normal),
        new(159, "「スリーフェアリーズ」",                   Stage.B2_3,  Level.Hard),
        new(160, "「スリーフェアリーズ」",                   Stage.B2_3,  Level.Lunatic),
        new(161, "星符「スターライトレイン」",               Stage.C_1,   Level.Easy),
        new(162, "星符「スターライトレイン」",               Stage.C_1,   Level.Normal),
        new(163, "星符「スターライトレイン」",               Stage.C_1,   Level.Hard),
        new(164, "星符「スターライトレイン」",               Stage.C_1,   Level.Lunatic),
        new(165, "星符「レッドスター」",                     Stage.C_1,   Level.Easy),
        new(166, "星符「レッドスター」",                     Stage.C_1,   Level.Normal),
        new(167, "星符「レッドスター」",                     Stage.C_1,   Level.Hard),
        new(168, "星符「レッドスター」",                     Stage.C_1,   Level.Lunatic),
        new(169, "瞬光「フェイタルフラッシュ」",             Stage.C1_2,  Level.Easy),
        new(170, "瞬光「フェイタルフラッシュ」",             Stage.C1_2,  Level.Normal),
        new(171, "瞬光「フェイタルフラッシュ」",             Stage.C1_2,  Level.Hard),
        new(172, "瞬光「フェイタルフラッシュ」",             Stage.C1_2,  Level.Lunatic),
        new(173, "光精「クロスディフュージョン」",           Stage.C1_2,  Level.Easy),
        new(174, "光精「クロスディフュージョン」",           Stage.C1_2,  Level.Normal),
        new(175, "光精「クロスディフュージョン」",           Stage.C1_2,  Level.Hard),
        new(176, "光精「クロスディフュージョン」",           Stage.C1_2,  Level.Lunatic),
        new(177, "光符「イエローディフレクション」",         Stage.C1_2,  Level.Easy),
        new(178, "光符「イエローディフレクション」",         Stage.C1_2,  Level.Normal),
        new(179, "光符「イエローディフレクション」",         Stage.C1_2,  Level.Hard),
        new(180, "光符「イエローディフレクション」",         Stage.C1_2,  Level.Lunatic),
        new(181, "月光「サイレントフラワー」",               Stage.C1_3,  Level.Easy),
        new(182, "月光「サイレントフラワー」",               Stage.C1_3,  Level.Normal),
        new(183, "月光「サイレントフラワー」",               Stage.C1_3,  Level.Hard),
        new(184, "月光「サイレントフラワー」",               Stage.C1_3,  Level.Lunatic),
        new(185, "光符「フルムーンナイト」",                 Stage.C1_3,  Level.Easy),
        new(186, "光符「フルムーンナイト」",                 Stage.C1_3,  Level.Normal),
        new(187, "光符「フルムーンナイト」",                 Stage.C1_3,  Level.Hard),
        new(188, "光符「フルムーンナイト」",                 Stage.C1_3,  Level.Lunatic),
        new(189, "月熱「アイスディゾルバー」",               Stage.C1_3,  Level.Easy),
        new(190, "月熱「アイスディゾルバー」",               Stage.C1_3,  Level.Normal),
        new(191, "月熱「アイスディゾルバー」",               Stage.C1_3,  Level.Hard),
        new(192, "月熱「アイスディゾルバー」",               Stage.C1_3,  Level.Lunatic),
        new(193, "降光「トリプルライト」",                   Stage.C1_3,  Level.Easy),
        new(194, "降光「トリプルライト」",                   Stage.C1_3,  Level.Normal),
        new(195, "降光「トリプルライト」",                   Stage.C1_3,  Level.Hard),
        new(196, "降光「トリプルライト」",                   Stage.C1_3,  Level.Lunatic),
        new(197, "協力技「フェアリーオーバードライブ」",     Stage.C1_3,  Level.Easy),
        new(198, "協力技「フェアリーオーバードライブ」",     Stage.C1_3,  Level.Normal),
        new(199, "協力技「フェアリーオーバードライブ」",     Stage.C1_3,  Level.Hard),
        new(200, "協力技「フェアリーオーバードライブ」",     Stage.C1_3,  Level.Lunatic),
        new(201, "「スリーフェアリーズ」",                   Stage.C1_3,  Level.Easy),
        new(202, "「スリーフェアリーズ」",                   Stage.C1_3,  Level.Normal),
        new(203, "「スリーフェアリーズ」",                   Stage.C1_3,  Level.Hard),
        new(204, "「スリーフェアリーズ」",                   Stage.C1_3,  Level.Lunatic),
        new(205, "月光「ムーンスティルネス」",               Stage.C2_2,  Level.Easy),
        new(206, "月光「ムーンスティルネス」",               Stage.C2_2,  Level.Normal),
        new(207, "月光「ムーンスティルネス」",               Stage.C2_2,  Level.Hard),
        new(208, "月光「ムーンスティルネス」",               Stage.C2_2,  Level.Lunatic),
        new(209, "光壁「ウォールブレイク」",                 Stage.C2_2,  Level.Easy),
        new(210, "光壁「ウォールブレイク」",                 Stage.C2_2,  Level.Normal),
        new(211, "光壁「ウォールブレイク」",                 Stage.C2_2,  Level.Hard),
        new(212, "光壁「ウォールブレイク」",                 Stage.C2_2,  Level.Lunatic),
        new(213, "月光「サイレントサイクロン」",             Stage.C2_2,  Level.Easy),
        new(214, "月光「サイレントサイクロン」",             Stage.C2_2,  Level.Normal),
        new(215, "月光「サイレントサイクロン」",             Stage.C2_2,  Level.Hard),
        new(216, "月光「サイレントサイクロン」",             Stage.C2_2,  Level.Lunatic),
        new(217, "陽光「サンシャインニードル」",             Stage.C2_3,  Level.Easy),
        new(218, "陽光「サンシャインニードル」",             Stage.C2_3,  Level.Normal),
        new(219, "陽光「サンシャインニードル」",             Stage.C2_3,  Level.Hard),
        new(220, "陽光「サンシャインニードル」",             Stage.C2_3,  Level.Lunatic),
        new(221, "光符「ハイパーインクレクション」",         Stage.C2_3,  Level.Easy),
        new(222, "光符「ハイパーインクレクション」",         Stage.C2_3,  Level.Normal),
        new(223, "光符「ハイパーインクレクション」",         Stage.C2_3,  Level.Hard),
        new(224, "光符「ハイパーインクレクション」",         Stage.C2_3,  Level.Lunatic),
        new(225, "日熱「アイスディゾルバー」",               Stage.C2_3,  Level.Easy),
        new(226, "日熱「アイスディゾルバー」",               Stage.C2_3,  Level.Normal),
        new(227, "日熱「アイスディゾルバー」",               Stage.C2_3,  Level.Hard),
        new(228, "日熱「アイスディゾルバー」",               Stage.C2_3,  Level.Lunatic),
        new(229, "激光「サンバースト」",                     Stage.C2_3,  Level.Easy),
        new(230, "激光「サンバースト」",                     Stage.C2_3,  Level.Normal),
        new(231, "激光「サンバースト」",                     Stage.C2_3,  Level.Hard),
        new(232, "激光「サンバースト」",                     Stage.C2_3,  Level.Lunatic),
        new(233, "協力技「フェアリーオーバードライブ」",     Stage.C2_3,  Level.Easy),
        new(234, "協力技「フェアリーオーバードライブ」",     Stage.C2_3,  Level.Normal),
        new(235, "協力技「フェアリーオーバードライブ」",     Stage.C2_3,  Level.Hard),
        new(236, "協力技「フェアリーオーバードライブ」",     Stage.C2_3,  Level.Lunatic),
        new(237, "「スリーフェアリーズ」",                   Stage.C2_3,  Level.Easy),
        new(238, "「スリーフェアリーズ」",                   Stage.C2_3,  Level.Normal),
        new(239, "「スリーフェアリーズ」",                   Stage.C2_3,  Level.Hard),
        new(240, "「スリーフェアリーズ」",                   Stage.C2_3,  Level.Lunatic),
        new(241, "光符「ミステリアスビーム」",               Stage.Extra, Level.Extra),
        new(242, "光撃「シュート・ザ・リトルムーン」",       Stage.Extra, Level.Extra),
        new(243, "魔弾「テストスレイブ」",                   Stage.Extra, Level.Extra),
        new(244, "閉符「ビッグクランチ」",                   Stage.Extra, Level.Extra),
        new(245, "恋符「マスタースパークのような懐中電灯」", Stage.Extra, Level.Extra),
        new(246, "魔開「オープンユニバース」",               Stage.Extra, Level.Extra),
        new(247, "魔十字「グランドクロス」",                 Stage.Extra, Level.Extra),
        new(248, "流星「スーパーペルセイド」",               Stage.Extra, Level.Extra),
        new(249, "「ブレイジングスターのような鬼ごっこ」",   Stage.Extra, Level.Extra),
        new(250, "「妖精尽滅光」",                           Stage.Extra, Level.Extra),
#pragma warning restore SA1008 // Opening parenthesis should be spaced correctly
    }.ToDictionary(static card => card.Id);
}
﻿//-----------------------------------------------------------------------
// <copyright file="Definitions.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using System.Linq;
using CardInfo = ThScoreFileConverter.Models.SpellCardInfo<
    ThScoreFileConverter.Models.Th128.Stage, ThScoreFileConverter.Models.Level>;

namespace ThScoreFileConverter.Models.Th128
{
    internal static class Definitions
    {
        // Thanks to thwiki.info
        public static IReadOnlyDictionary<int, CardInfo> CardTable { get; } = new List<CardInfo>()
        {
#pragma warning disable SA1008 // Opening parenthesis should be spaced correctly
            new CardInfo(  1, "月符「ルナティックレイン」",               Stage.A_1,   Level.Easy),
            new CardInfo(  2, "月符「ルナティックレイン」",               Stage.A_1,   Level.Normal),
            new CardInfo(  3, "月符「ルナティックレイン」",               Stage.A_1,   Level.Hard),
            new CardInfo(  4, "月符「ルナティックレイン」",               Stage.A_1,   Level.Lunatic),
            new CardInfo(  5, "月符「ルナサイクロン」",                   Stage.A_1,   Level.Easy),
            new CardInfo(  6, "月符「ルナサイクロン」",                   Stage.A_1,   Level.Normal),
            new CardInfo(  7, "月符「ルナサイクロン」",                   Stage.A_1,   Level.Hard),
            new CardInfo(  8, "月符「ルナサイクロン」",                   Stage.A_1,   Level.Lunatic),
            new CardInfo(  9, "流星「プチコメット」",                     Stage.A1_2,  Level.Easy),
            new CardInfo( 10, "流星「プチコメット」",                     Stage.A1_2,  Level.Normal),
            new CardInfo( 11, "流星「プチコメット」",                     Stage.A1_2,  Level.Hard),
            new CardInfo( 12, "流星「プチコメット」",                     Stage.A1_2,  Level.Lunatic),
            new CardInfo( 13, "星粒「スプリンクルピース」",               Stage.A1_2,  Level.Easy),
            new CardInfo( 14, "星粒「スプリンクルピース」",               Stage.A1_2,  Level.Normal),
            new CardInfo( 15, "星粒「スプリンクルピース」",               Stage.A1_2,  Level.Hard),
            new CardInfo( 16, "星粒「スプリンクルピース」",               Stage.A1_2,  Level.Lunatic),
            new CardInfo( 17, "星符「トゥインクルサファイア」",           Stage.A1_2,  Level.Easy),
            new CardInfo( 18, "星符「トゥインクルサファイア」",           Stage.A1_2,  Level.Normal),
            new CardInfo( 19, "星符「トゥインクルサファイア」",           Stage.A1_2,  Level.Hard),
            new CardInfo( 20, "星符「トゥインクルサファイア」",           Stage.A1_2,  Level.Lunatic),
            new CardInfo( 21, "陽光「サンシャインブラスト」",             Stage.A1_3,  Level.Easy),
            new CardInfo( 22, "陽光「サンシャインブラスト」",             Stage.A1_3,  Level.Normal),
            new CardInfo( 23, "陽光「サンシャインブラスト」",             Stage.A1_3,  Level.Hard),
            new CardInfo( 24, "陽光「サンシャインブラスト」",             Stage.A1_3,  Level.Lunatic),
            new CardInfo( 25, "光符「ルチルフレクション」",               Stage.A1_3,  Level.Easy),
            new CardInfo( 26, "光符「ルチルフレクション」",               Stage.A1_3,  Level.Normal),
            new CardInfo( 27, "光符「ルチルフレクション」",               Stage.A1_3,  Level.Hard),
            new CardInfo( 28, "光符「ルチルフレクション」",               Stage.A1_3,  Level.Lunatic),
            new CardInfo( 29, "日熱「アイスディゾルバー」",               Stage.A1_3,  Level.Easy),
            new CardInfo( 30, "日熱「アイスディゾルバー」",               Stage.A1_3,  Level.Normal),
            new CardInfo( 31, "日熱「アイスディゾルバー」",               Stage.A1_3,  Level.Hard),
            new CardInfo( 32, "日熱「アイスディゾルバー」",               Stage.A1_3,  Level.Lunatic),
            new CardInfo( 33, "空符「エルフィンキャノピー」",             Stage.A1_3,  Level.Easy),
            new CardInfo( 34, "空符「エルフィンキャノピー」",             Stage.A1_3,  Level.Normal),
            new CardInfo( 35, "空符「エルフィンキャノピー」",             Stage.A1_3,  Level.Hard),
            new CardInfo( 36, "空符「エルフィンキャノピー」",             Stage.A1_3,  Level.Lunatic),
            new CardInfo( 37, "協力技「フェアリーオーバードライブ」",     Stage.A1_3,  Level.Easy),
            new CardInfo( 38, "協力技「フェアリーオーバードライブ」",     Stage.A1_3,  Level.Normal),
            new CardInfo( 39, "協力技「フェアリーオーバードライブ」",     Stage.A1_3,  Level.Hard),
            new CardInfo( 40, "協力技「フェアリーオーバードライブ」",     Stage.A1_3,  Level.Lunatic),
            new CardInfo( 41, "「スリーフェアリーズ」",                   Stage.A1_3,  Level.Easy),
            new CardInfo( 42, "「スリーフェアリーズ」",                   Stage.A1_3,  Level.Normal),
            new CardInfo( 43, "「スリーフェアリーズ」",                   Stage.A1_3,  Level.Hard),
            new CardInfo( 44, "「スリーフェアリーズ」",                   Stage.A1_3,  Level.Lunatic),
            new CardInfo( 45, "虹光「プリズムフラッシュ」",               Stage.A2_2,  Level.Easy),
            new CardInfo( 46, "虹光「プリズムフラッシュ」",               Stage.A2_2,  Level.Normal),
            new CardInfo( 47, "虹光「プリズムフラッシュ」",               Stage.A2_2,  Level.Hard),
            new CardInfo( 48, "虹光「プリズムフラッシュ」",               Stage.A2_2,  Level.Lunatic),
            new CardInfo( 49, "光精「ダイアモンドリング」",               Stage.A2_2,  Level.Easy),
            new CardInfo( 50, "光精「ダイアモンドリング」",               Stage.A2_2,  Level.Normal),
            new CardInfo( 51, "光精「ダイアモンドリング」",               Stage.A2_2,  Level.Hard),
            new CardInfo( 52, "光精「ダイアモンドリング」",               Stage.A2_2,  Level.Lunatic),
            new CardInfo( 53, "光符「ブルーディフレクション」",           Stage.A2_2,  Level.Easy),
            new CardInfo( 54, "光符「ブルーディフレクション」",           Stage.A2_2,  Level.Normal),
            new CardInfo( 55, "光符「ブルーディフレクション」",           Stage.A2_2,  Level.Hard),
            new CardInfo( 56, "光符「ブルーディフレクション」",           Stage.A2_2,  Level.Lunatic),
            new CardInfo( 57, "星光「スターレーザー」",                   Stage.A2_3,  Level.Easy),
            new CardInfo( 58, "星光「スターレーザー」",                   Stage.A2_3,  Level.Normal),
            new CardInfo( 59, "星光「スターレーザー」",                   Stage.A2_3,  Level.Hard),
            new CardInfo( 60, "星光「スターレーザー」",                   Stage.A2_3,  Level.Lunatic),
            new CardInfo( 61, "光符「トリプルメテオ」",                   Stage.A2_3,  Level.Easy),
            new CardInfo( 62, "光符「トリプルメテオ」",                   Stage.A2_3,  Level.Normal),
            new CardInfo( 63, "光符「トリプルメテオ」",                   Stage.A2_3,  Level.Hard),
            new CardInfo( 64, "光符「トリプルメテオ」",                   Stage.A2_3,  Level.Lunatic),
            new CardInfo( 65, "星熱「アイスディゾルバー」",               Stage.A2_3,  Level.Easy),
            new CardInfo( 66, "星熱「アイスディゾルバー」",               Stage.A2_3,  Level.Normal),
            new CardInfo( 67, "星熱「アイスディゾルバー」",               Stage.A2_3,  Level.Hard),
            new CardInfo( 68, "星熱「アイスディゾルバー」",               Stage.A2_3,  Level.Lunatic),
            new CardInfo( 69, "光星「オリオンベルト」",                   Stage.A2_3,  Level.Easy),
            new CardInfo( 70, "光星「オリオンベルト」",                   Stage.A2_3,  Level.Normal),
            new CardInfo( 71, "光星「オリオンベルト」",                   Stage.A2_3,  Level.Hard),
            new CardInfo( 72, "光星「オリオンベルト」",                   Stage.A2_3,  Level.Lunatic),
            new CardInfo( 73, "協力技「フェアリーオーバードライブ」",     Stage.A2_3,  Level.Easy),
            new CardInfo( 74, "協力技「フェアリーオーバードライブ」",     Stage.A2_3,  Level.Normal),
            new CardInfo( 75, "協力技「フェアリーオーバードライブ」",     Stage.A2_3,  Level.Hard),
            new CardInfo( 76, "協力技「フェアリーオーバードライブ」",     Stage.A2_3,  Level.Lunatic),
            new CardInfo( 77, "「スリーフェアリーズ」",                   Stage.A2_3,  Level.Easy),
            new CardInfo( 78, "「スリーフェアリーズ」",                   Stage.A2_3,  Level.Normal),
            new CardInfo( 79, "「スリーフェアリーズ」",                   Stage.A2_3,  Level.Hard),
            new CardInfo( 80, "「スリーフェアリーズ」",                   Stage.A2_3,  Level.Lunatic),
            new CardInfo( 81, "日符「アグレッシブライト」",               Stage.B_1,   Level.Easy),
            new CardInfo( 82, "日符「アグレッシブライト」",               Stage.B_1,   Level.Normal),
            new CardInfo( 83, "日符「アグレッシブライト」",               Stage.B_1,   Level.Hard),
            new CardInfo( 84, "日符「アグレッシブライト」",               Stage.B_1,   Level.Lunatic),
            new CardInfo( 85, "日符「ダイレクトサンライト」",             Stage.B_1,   Level.Easy),
            new CardInfo( 86, "日符「ダイレクトサンライト」",             Stage.B_1,   Level.Normal),
            new CardInfo( 87, "日符「ダイレクトサンライト」",             Stage.B_1,   Level.Hard),
            new CardInfo( 88, "日符「ダイレクトサンライト」",             Stage.B_1,   Level.Lunatic),
            new CardInfo( 89, "流星「コメットストリーム」",               Stage.B1_2,  Level.Easy),
            new CardInfo( 90, "流星「コメットストリーム」",               Stage.B1_2,  Level.Normal),
            new CardInfo( 91, "流星「コメットストリーム」",               Stage.B1_2,  Level.Hard),
            new CardInfo( 92, "流星「コメットストリーム」",               Stage.B1_2,  Level.Lunatic),
            new CardInfo( 93, "星粒「スターピースシャワー」",             Stage.B1_2,  Level.Easy),
            new CardInfo( 94, "星粒「スターピースシャワー」",             Stage.B1_2,  Level.Normal),
            new CardInfo( 95, "星粒「スターピースシャワー」",             Stage.B1_2,  Level.Hard),
            new CardInfo( 96, "星粒「スターピースシャワー」",             Stage.B1_2,  Level.Lunatic),
            new CardInfo( 97, "星符「シューティングサファイア」",         Stage.B1_2,  Level.Easy),
            new CardInfo( 98, "星符「シューティングサファイア」",         Stage.B1_2,  Level.Normal),
            new CardInfo( 99, "星符「シューティングサファイア」",         Stage.B1_2,  Level.Hard),
            new CardInfo(100, "星符「シューティングサファイア」",         Stage.B1_2,  Level.Lunatic),
            new CardInfo(101, "月光「サイレントストーム」",               Stage.B1_3,  Level.Easy),
            new CardInfo(102, "月光「サイレントストーム」",               Stage.B1_3,  Level.Normal),
            new CardInfo(103, "月光「サイレントストーム」",               Stage.B1_3,  Level.Hard),
            new CardInfo(104, "月光「サイレントストーム」",               Stage.B1_3,  Level.Lunatic),
            new CardInfo(105, "光符「ブライトナイト」",                   Stage.B1_3,  Level.Easy),
            new CardInfo(106, "光符「ブライトナイト」",                   Stage.B1_3,  Level.Normal),
            new CardInfo(107, "光符「ブライトナイト」",                   Stage.B1_3,  Level.Hard),
            new CardInfo(108, "光符「ブライトナイト」",                   Stage.B1_3,  Level.Lunatic),
            new CardInfo(109, "月熱「アイスディゾルバー」",               Stage.B1_3,  Level.Easy),
            new CardInfo(110, "月熱「アイスディゾルバー」",               Stage.B1_3,  Level.Normal),
            new CardInfo(111, "月熱「アイスディゾルバー」",               Stage.B1_3,  Level.Hard),
            new CardInfo(112, "月熱「アイスディゾルバー」",               Stage.B1_3,  Level.Lunatic),
            new CardInfo(113, "空符「ブレイクキャノピー」",               Stage.B1_3,  Level.Easy),
            new CardInfo(114, "空符「ブレイクキャノピー」",               Stage.B1_3,  Level.Normal),
            new CardInfo(115, "空符「ブレイクキャノピー」",               Stage.B1_3,  Level.Hard),
            new CardInfo(116, "空符「ブレイクキャノピー」",               Stage.B1_3,  Level.Lunatic),
            new CardInfo(117, "協力技「フェアリーオーバードライブ」",     Stage.B1_3,  Level.Easy),
            new CardInfo(118, "協力技「フェアリーオーバードライブ」",     Stage.B1_3,  Level.Normal),
            new CardInfo(119, "協力技「フェアリーオーバードライブ」",     Stage.B1_3,  Level.Hard),
            new CardInfo(120, "協力技「フェアリーオーバードライブ」",     Stage.B1_3,  Level.Lunatic),
            new CardInfo(121, "「スリーフェアリーズ」",                   Stage.B1_3,  Level.Easy),
            new CardInfo(122, "「スリーフェアリーズ」",                   Stage.B1_3,  Level.Normal),
            new CardInfo(123, "「スリーフェアリーズ」",                   Stage.B1_3,  Level.Hard),
            new CardInfo(124, "「スリーフェアリーズ」",                   Stage.B1_3,  Level.Lunatic),
            new CardInfo(125, "月光「ダークスティルネス」",               Stage.B2_2,  Level.Easy),
            new CardInfo(126, "月光「ダークスティルネス」",               Stage.B2_2,  Level.Normal),
            new CardInfo(127, "月光「ダークスティルネス」",               Stage.B2_2,  Level.Hard),
            new CardInfo(128, "月光「ダークスティルネス」",               Stage.B2_2,  Level.Lunatic),
            new CardInfo(129, "障光「ムーンライトウォール」",             Stage.B2_2,  Level.Easy),
            new CardInfo(130, "障光「ムーンライトウォール」",             Stage.B2_2,  Level.Normal),
            new CardInfo(131, "障光「ムーンライトウォール」",             Stage.B2_2,  Level.Hard),
            new CardInfo(132, "障光「ムーンライトウォール」",             Stage.B2_2,  Level.Lunatic),
            new CardInfo(133, "夜符「ナイトフェアリーズ」",               Stage.B2_2,  Level.Easy),
            new CardInfo(134, "夜符「ナイトフェアリーズ」",               Stage.B2_2,  Level.Normal),
            new CardInfo(135, "夜符「ナイトフェアリーズ」",               Stage.B2_2,  Level.Hard),
            new CardInfo(136, "夜符「ナイトフェアリーズ」",               Stage.B2_2,  Level.Lunatic),
            new CardInfo(137, "星光「スターストーム」",                   Stage.B2_3,  Level.Easy),
            new CardInfo(138, "星光「スターストーム」",                   Stage.B2_3,  Level.Normal),
            new CardInfo(139, "星光「スターストーム」",                   Stage.B2_3,  Level.Hard),
            new CardInfo(140, "星光「スターストーム」",                   Stage.B2_3,  Level.Lunatic),
            new CardInfo(141, "光符「エクステンシブメテオ」",             Stage.B2_3,  Level.Easy),
            new CardInfo(142, "光符「エクステンシブメテオ」",             Stage.B2_3,  Level.Normal),
            new CardInfo(143, "光符「エクステンシブメテオ」",             Stage.B2_3,  Level.Hard),
            new CardInfo(144, "光符「エクステンシブメテオ」",             Stage.B2_3,  Level.Lunatic),
            new CardInfo(145, "星熱「アイスディゾルバー」",               Stage.B2_3,  Level.Easy),
            new CardInfo(146, "星熱「アイスディゾルバー」",               Stage.B2_3,  Level.Normal),
            new CardInfo(147, "星熱「アイスディゾルバー」",               Stage.B2_3,  Level.Hard),
            new CardInfo(148, "星熱「アイスディゾルバー」",               Stage.B2_3,  Level.Lunatic),
            new CardInfo(149, "光星「グレートトライアングル」",           Stage.B2_3,  Level.Easy),
            new CardInfo(150, "光星「グレートトライアングル」",           Stage.B2_3,  Level.Normal),
            new CardInfo(151, "光星「グレートトライアングル」",           Stage.B2_3,  Level.Hard),
            new CardInfo(152, "光星「グレートトライアングル」",           Stage.B2_3,  Level.Lunatic),
            new CardInfo(153, "協力技「フェアリーオーバードライブ」",     Stage.B2_3,  Level.Easy),
            new CardInfo(154, "協力技「フェアリーオーバードライブ」",     Stage.B2_3,  Level.Normal),
            new CardInfo(155, "協力技「フェアリーオーバードライブ」",     Stage.B2_3,  Level.Hard),
            new CardInfo(156, "協力技「フェアリーオーバードライブ」",     Stage.B2_3,  Level.Lunatic),
            new CardInfo(157, "「スリーフェアリーズ」",                   Stage.B2_3,  Level.Easy),
            new CardInfo(158, "「スリーフェアリーズ」",                   Stage.B2_3,  Level.Normal),
            new CardInfo(159, "「スリーフェアリーズ」",                   Stage.B2_3,  Level.Hard),
            new CardInfo(160, "「スリーフェアリーズ」",                   Stage.B2_3,  Level.Lunatic),
            new CardInfo(161, "星符「スターライトレイン」",               Stage.C_1,   Level.Easy),
            new CardInfo(162, "星符「スターライトレイン」",               Stage.C_1,   Level.Normal),
            new CardInfo(163, "星符「スターライトレイン」",               Stage.C_1,   Level.Hard),
            new CardInfo(164, "星符「スターライトレイン」",               Stage.C_1,   Level.Lunatic),
            new CardInfo(165, "星符「レッドスター」",                     Stage.C_1,   Level.Easy),
            new CardInfo(166, "星符「レッドスター」",                     Stage.C_1,   Level.Normal),
            new CardInfo(167, "星符「レッドスター」",                     Stage.C_1,   Level.Hard),
            new CardInfo(168, "星符「レッドスター」",                     Stage.C_1,   Level.Lunatic),
            new CardInfo(169, "瞬光「フェイタルフラッシュ」",             Stage.C1_2,  Level.Easy),
            new CardInfo(170, "瞬光「フェイタルフラッシュ」",             Stage.C1_2,  Level.Normal),
            new CardInfo(171, "瞬光「フェイタルフラッシュ」",             Stage.C1_2,  Level.Hard),
            new CardInfo(172, "瞬光「フェイタルフラッシュ」",             Stage.C1_2,  Level.Lunatic),
            new CardInfo(173, "光精「クロスディフュージョン」",           Stage.C1_2,  Level.Easy),
            new CardInfo(174, "光精「クロスディフュージョン」",           Stage.C1_2,  Level.Normal),
            new CardInfo(175, "光精「クロスディフュージョン」",           Stage.C1_2,  Level.Hard),
            new CardInfo(176, "光精「クロスディフュージョン」",           Stage.C1_2,  Level.Lunatic),
            new CardInfo(177, "光符「イエローディフレクション」",         Stage.C1_2,  Level.Easy),
            new CardInfo(178, "光符「イエローディフレクション」",         Stage.C1_2,  Level.Normal),
            new CardInfo(179, "光符「イエローディフレクション」",         Stage.C1_2,  Level.Hard),
            new CardInfo(180, "光符「イエローディフレクション」",         Stage.C1_2,  Level.Lunatic),
            new CardInfo(181, "月光「サイレントフラワー」",               Stage.C1_3,  Level.Easy),
            new CardInfo(182, "月光「サイレントフラワー」",               Stage.C1_3,  Level.Normal),
            new CardInfo(183, "月光「サイレントフラワー」",               Stage.C1_3,  Level.Hard),
            new CardInfo(184, "月光「サイレントフラワー」",               Stage.C1_3,  Level.Lunatic),
            new CardInfo(185, "光符「フルムーンナイト」",                 Stage.C1_3,  Level.Easy),
            new CardInfo(186, "光符「フルムーンナイト」",                 Stage.C1_3,  Level.Normal),
            new CardInfo(187, "光符「フルムーンナイト」",                 Stage.C1_3,  Level.Hard),
            new CardInfo(188, "光符「フルムーンナイト」",                 Stage.C1_3,  Level.Lunatic),
            new CardInfo(189, "月熱「アイスディゾルバー」",               Stage.C1_3,  Level.Easy),
            new CardInfo(190, "月熱「アイスディゾルバー」",               Stage.C1_3,  Level.Normal),
            new CardInfo(191, "月熱「アイスディゾルバー」",               Stage.C1_3,  Level.Hard),
            new CardInfo(192, "月熱「アイスディゾルバー」",               Stage.C1_3,  Level.Lunatic),
            new CardInfo(193, "降光「トリプルライト」",                   Stage.C1_3,  Level.Easy),
            new CardInfo(194, "降光「トリプルライト」",                   Stage.C1_3,  Level.Normal),
            new CardInfo(195, "降光「トリプルライト」",                   Stage.C1_3,  Level.Hard),
            new CardInfo(196, "降光「トリプルライト」",                   Stage.C1_3,  Level.Lunatic),
            new CardInfo(197, "協力技「フェアリーオーバードライブ」",     Stage.C1_3,  Level.Easy),
            new CardInfo(198, "協力技「フェアリーオーバードライブ」",     Stage.C1_3,  Level.Normal),
            new CardInfo(199, "協力技「フェアリーオーバードライブ」",     Stage.C1_3,  Level.Hard),
            new CardInfo(200, "協力技「フェアリーオーバードライブ」",     Stage.C1_3,  Level.Lunatic),
            new CardInfo(201, "「スリーフェアリーズ」",                   Stage.C1_3,  Level.Easy),
            new CardInfo(202, "「スリーフェアリーズ」",                   Stage.C1_3,  Level.Normal),
            new CardInfo(203, "「スリーフェアリーズ」",                   Stage.C1_3,  Level.Hard),
            new CardInfo(204, "「スリーフェアリーズ」",                   Stage.C1_3,  Level.Lunatic),
            new CardInfo(205, "月光「ムーンスティルネス」",               Stage.C2_2,  Level.Easy),
            new CardInfo(206, "月光「ムーンスティルネス」",               Stage.C2_2,  Level.Normal),
            new CardInfo(207, "月光「ムーンスティルネス」",               Stage.C2_2,  Level.Hard),
            new CardInfo(208, "月光「ムーンスティルネス」",               Stage.C2_2,  Level.Lunatic),
            new CardInfo(209, "光壁「ウォールブレイク」",                 Stage.C2_2,  Level.Easy),
            new CardInfo(210, "光壁「ウォールブレイク」",                 Stage.C2_2,  Level.Normal),
            new CardInfo(211, "光壁「ウォールブレイク」",                 Stage.C2_2,  Level.Hard),
            new CardInfo(212, "光壁「ウォールブレイク」",                 Stage.C2_2,  Level.Lunatic),
            new CardInfo(213, "月光「サイレントサイクロン」",             Stage.C2_2,  Level.Easy),
            new CardInfo(214, "月光「サイレントサイクロン」",             Stage.C2_2,  Level.Normal),
            new CardInfo(215, "月光「サイレントサイクロン」",             Stage.C2_2,  Level.Hard),
            new CardInfo(216, "月光「サイレントサイクロン」",             Stage.C2_2,  Level.Lunatic),
            new CardInfo(217, "陽光「サンシャインニードル」",             Stage.C2_3,  Level.Easy),
            new CardInfo(218, "陽光「サンシャインニードル」",             Stage.C2_3,  Level.Normal),
            new CardInfo(219, "陽光「サンシャインニードル」",             Stage.C2_3,  Level.Hard),
            new CardInfo(220, "陽光「サンシャインニードル」",             Stage.C2_3,  Level.Lunatic),
            new CardInfo(221, "光符「ハイパーインクレクション」",         Stage.C2_3,  Level.Easy),
            new CardInfo(222, "光符「ハイパーインクレクション」",         Stage.C2_3,  Level.Normal),
            new CardInfo(223, "光符「ハイパーインクレクション」",         Stage.C2_3,  Level.Hard),
            new CardInfo(224, "光符「ハイパーインクレクション」",         Stage.C2_3,  Level.Lunatic),
            new CardInfo(225, "日熱「アイスディゾルバー」",               Stage.C2_3,  Level.Easy),
            new CardInfo(226, "日熱「アイスディゾルバー」",               Stage.C2_3,  Level.Normal),
            new CardInfo(227, "日熱「アイスディゾルバー」",               Stage.C2_3,  Level.Hard),
            new CardInfo(228, "日熱「アイスディゾルバー」",               Stage.C2_3,  Level.Lunatic),
            new CardInfo(229, "激光「サンバースト」",                     Stage.C2_3,  Level.Easy),
            new CardInfo(230, "激光「サンバースト」",                     Stage.C2_3,  Level.Normal),
            new CardInfo(231, "激光「サンバースト」",                     Stage.C2_3,  Level.Hard),
            new CardInfo(232, "激光「サンバースト」",                     Stage.C2_3,  Level.Lunatic),
            new CardInfo(233, "協力技「フェアリーオーバードライブ」",     Stage.C2_3,  Level.Easy),
            new CardInfo(234, "協力技「フェアリーオーバードライブ」",     Stage.C2_3,  Level.Normal),
            new CardInfo(235, "協力技「フェアリーオーバードライブ」",     Stage.C2_3,  Level.Hard),
            new CardInfo(236, "協力技「フェアリーオーバードライブ」",     Stage.C2_3,  Level.Lunatic),
            new CardInfo(237, "「スリーフェアリーズ」",                   Stage.C2_3,  Level.Easy),
            new CardInfo(238, "「スリーフェアリーズ」",                   Stage.C2_3,  Level.Normal),
            new CardInfo(239, "「スリーフェアリーズ」",                   Stage.C2_3,  Level.Hard),
            new CardInfo(240, "「スリーフェアリーズ」",                   Stage.C2_3,  Level.Lunatic),
            new CardInfo(241, "光符「ミステリアスビーム」",               Stage.Extra, Level.Extra),
            new CardInfo(242, "光撃「シュート・ザ・リトルムーン」",       Stage.Extra, Level.Extra),
            new CardInfo(243, "魔弾「テストスレイブ」",                   Stage.Extra, Level.Extra),
            new CardInfo(244, "閉符「ビッグクランチ」",                   Stage.Extra, Level.Extra),
            new CardInfo(245, "恋符「マスタースパークのような懐中電灯」", Stage.Extra, Level.Extra),
            new CardInfo(246, "魔開「オープンユニバース」",               Stage.Extra, Level.Extra),
            new CardInfo(247, "魔十字「グランドクロス」",                 Stage.Extra, Level.Extra),
            new CardInfo(248, "流星「スーパーペルセイド」",               Stage.Extra, Level.Extra),
            new CardInfo(249, "「ブレイジングスターのような鬼ごっこ」",   Stage.Extra, Level.Extra),
            new CardInfo(250, "「妖精尽滅光」",                           Stage.Extra, Level.Extra),
#pragma warning restore SA1008 // Opening parenthesis should be spaced correctly
        }.ToDictionary(card => card.Id);
    }
}

//-----------------------------------------------------------------------
// <copyright file="Th128Converter.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable 1591
#pragma warning disable SA1600 // ElementsMustBeDocumented

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Models.Th128;
using CardInfo = ThScoreFileConverter.Models.SpellCardInfo<
    ThScoreFileConverter.Models.Th128.Stage, ThScoreFileConverter.Models.Level>;

namespace ThScoreFileConverter.Models
{
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Reviewed.")]
    internal class Th128Converter : ThConverter
    {
        // Thanks to thwiki.info
        [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1008:OpeningParenthesisMustBeSpacedCorrectly", Justification = "Reviewed.")]
        private static readonly Dictionary<int, CardInfo> CardTable =
            new List<CardInfo>()
            {
                new CardInfo(  1, "月符「ルナティックレイン」",               Th128.Stage.A_1,   Level.Easy),
                new CardInfo(  2, "月符「ルナティックレイン」",               Th128.Stage.A_1,   Level.Normal),
                new CardInfo(  3, "月符「ルナティックレイン」",               Th128.Stage.A_1,   Level.Hard),
                new CardInfo(  4, "月符「ルナティックレイン」",               Th128.Stage.A_1,   Level.Lunatic),
                new CardInfo(  5, "月符「ルナサイクロン」",                   Th128.Stage.A_1,   Level.Easy),
                new CardInfo(  6, "月符「ルナサイクロン」",                   Th128.Stage.A_1,   Level.Normal),
                new CardInfo(  7, "月符「ルナサイクロン」",                   Th128.Stage.A_1,   Level.Hard),
                new CardInfo(  8, "月符「ルナサイクロン」",                   Th128.Stage.A_1,   Level.Lunatic),
                new CardInfo(  9, "流星「プチコメット」",                     Th128.Stage.A1_2,  Level.Easy),
                new CardInfo( 10, "流星「プチコメット」",                     Th128.Stage.A1_2,  Level.Normal),
                new CardInfo( 11, "流星「プチコメット」",                     Th128.Stage.A1_2,  Level.Hard),
                new CardInfo( 12, "流星「プチコメット」",                     Th128.Stage.A1_2,  Level.Lunatic),
                new CardInfo( 13, "星粒「スプリンクルピース」",               Th128.Stage.A1_2,  Level.Easy),
                new CardInfo( 14, "星粒「スプリンクルピース」",               Th128.Stage.A1_2,  Level.Normal),
                new CardInfo( 15, "星粒「スプリンクルピース」",               Th128.Stage.A1_2,  Level.Hard),
                new CardInfo( 16, "星粒「スプリンクルピース」",               Th128.Stage.A1_2,  Level.Lunatic),
                new CardInfo( 17, "星符「トゥインクルサファイア」",           Th128.Stage.A1_2,  Level.Easy),
                new CardInfo( 18, "星符「トゥインクルサファイア」",           Th128.Stage.A1_2,  Level.Normal),
                new CardInfo( 19, "星符「トゥインクルサファイア」",           Th128.Stage.A1_2,  Level.Hard),
                new CardInfo( 20, "星符「トゥインクルサファイア」",           Th128.Stage.A1_2,  Level.Lunatic),
                new CardInfo( 21, "陽光「サンシャインブラスト」",             Th128.Stage.A1_3,  Level.Easy),
                new CardInfo( 22, "陽光「サンシャインブラスト」",             Th128.Stage.A1_3,  Level.Normal),
                new CardInfo( 23, "陽光「サンシャインブラスト」",             Th128.Stage.A1_3,  Level.Hard),
                new CardInfo( 24, "陽光「サンシャインブラスト」",             Th128.Stage.A1_3,  Level.Lunatic),
                new CardInfo( 25, "光符「ルチルフレクション」",               Th128.Stage.A1_3,  Level.Easy),
                new CardInfo( 26, "光符「ルチルフレクション」",               Th128.Stage.A1_3,  Level.Normal),
                new CardInfo( 27, "光符「ルチルフレクション」",               Th128.Stage.A1_3,  Level.Hard),
                new CardInfo( 28, "光符「ルチルフレクション」",               Th128.Stage.A1_3,  Level.Lunatic),
                new CardInfo( 29, "日熱「アイスディゾルバー」",               Th128.Stage.A1_3,  Level.Easy),
                new CardInfo( 30, "日熱「アイスディゾルバー」",               Th128.Stage.A1_3,  Level.Normal),
                new CardInfo( 31, "日熱「アイスディゾルバー」",               Th128.Stage.A1_3,  Level.Hard),
                new CardInfo( 32, "日熱「アイスディゾルバー」",               Th128.Stage.A1_3,  Level.Lunatic),
                new CardInfo( 33, "空符「エルフィンキャノピー」",             Th128.Stage.A1_3,  Level.Easy),
                new CardInfo( 34, "空符「エルフィンキャノピー」",             Th128.Stage.A1_3,  Level.Normal),
                new CardInfo( 35, "空符「エルフィンキャノピー」",             Th128.Stage.A1_3,  Level.Hard),
                new CardInfo( 36, "空符「エルフィンキャノピー」",             Th128.Stage.A1_3,  Level.Lunatic),
                new CardInfo( 37, "協力技「フェアリーオーバードライブ」",     Th128.Stage.A1_3,  Level.Easy),
                new CardInfo( 38, "協力技「フェアリーオーバードライブ」",     Th128.Stage.A1_3,  Level.Normal),
                new CardInfo( 39, "協力技「フェアリーオーバードライブ」",     Th128.Stage.A1_3,  Level.Hard),
                new CardInfo( 40, "協力技「フェアリーオーバードライブ」",     Th128.Stage.A1_3,  Level.Lunatic),
                new CardInfo( 41, "「スリーフェアリーズ」",                   Th128.Stage.A1_3,  Level.Easy),
                new CardInfo( 42, "「スリーフェアリーズ」",                   Th128.Stage.A1_3,  Level.Normal),
                new CardInfo( 43, "「スリーフェアリーズ」",                   Th128.Stage.A1_3,  Level.Hard),
                new CardInfo( 44, "「スリーフェアリーズ」",                   Th128.Stage.A1_3,  Level.Lunatic),
                new CardInfo( 45, "虹光「プリズムフラッシュ」",               Th128.Stage.A2_2,  Level.Easy),
                new CardInfo( 46, "虹光「プリズムフラッシュ」",               Th128.Stage.A2_2,  Level.Normal),
                new CardInfo( 47, "虹光「プリズムフラッシュ」",               Th128.Stage.A2_2,  Level.Hard),
                new CardInfo( 48, "虹光「プリズムフラッシュ」",               Th128.Stage.A2_2,  Level.Lunatic),
                new CardInfo( 49, "光精「ダイアモンドリング」",               Th128.Stage.A2_2,  Level.Easy),
                new CardInfo( 50, "光精「ダイアモンドリング」",               Th128.Stage.A2_2,  Level.Normal),
                new CardInfo( 51, "光精「ダイアモンドリング」",               Th128.Stage.A2_2,  Level.Hard),
                new CardInfo( 52, "光精「ダイアモンドリング」",               Th128.Stage.A2_2,  Level.Lunatic),
                new CardInfo( 53, "光符「ブルーディフレクション」",           Th128.Stage.A2_2,  Level.Easy),
                new CardInfo( 54, "光符「ブルーディフレクション」",           Th128.Stage.A2_2,  Level.Normal),
                new CardInfo( 55, "光符「ブルーディフレクション」",           Th128.Stage.A2_2,  Level.Hard),
                new CardInfo( 56, "光符「ブルーディフレクション」",           Th128.Stage.A2_2,  Level.Lunatic),
                new CardInfo( 57, "星光「スターレーザー」",                   Th128.Stage.A2_3,  Level.Easy),
                new CardInfo( 58, "星光「スターレーザー」",                   Th128.Stage.A2_3,  Level.Normal),
                new CardInfo( 59, "星光「スターレーザー」",                   Th128.Stage.A2_3,  Level.Hard),
                new CardInfo( 60, "星光「スターレーザー」",                   Th128.Stage.A2_3,  Level.Lunatic),
                new CardInfo( 61, "光符「トリプルメテオ」",                   Th128.Stage.A2_3,  Level.Easy),
                new CardInfo( 62, "光符「トリプルメテオ」",                   Th128.Stage.A2_3,  Level.Normal),
                new CardInfo( 63, "光符「トリプルメテオ」",                   Th128.Stage.A2_3,  Level.Hard),
                new CardInfo( 64, "光符「トリプルメテオ」",                   Th128.Stage.A2_3,  Level.Lunatic),
                new CardInfo( 65, "星熱「アイスディゾルバー」",               Th128.Stage.A2_3,  Level.Easy),
                new CardInfo( 66, "星熱「アイスディゾルバー」",               Th128.Stage.A2_3,  Level.Normal),
                new CardInfo( 67, "星熱「アイスディゾルバー」",               Th128.Stage.A2_3,  Level.Hard),
                new CardInfo( 68, "星熱「アイスディゾルバー」",               Th128.Stage.A2_3,  Level.Lunatic),
                new CardInfo( 69, "光星「オリオンベルト」",                   Th128.Stage.A2_3,  Level.Easy),
                new CardInfo( 70, "光星「オリオンベルト」",                   Th128.Stage.A2_3,  Level.Normal),
                new CardInfo( 71, "光星「オリオンベルト」",                   Th128.Stage.A2_3,  Level.Hard),
                new CardInfo( 72, "光星「オリオンベルト」",                   Th128.Stage.A2_3,  Level.Lunatic),
                new CardInfo( 73, "協力技「フェアリーオーバードライブ」",     Th128.Stage.A2_3,  Level.Easy),
                new CardInfo( 74, "協力技「フェアリーオーバードライブ」",     Th128.Stage.A2_3,  Level.Normal),
                new CardInfo( 75, "協力技「フェアリーオーバードライブ」",     Th128.Stage.A2_3,  Level.Hard),
                new CardInfo( 76, "協力技「フェアリーオーバードライブ」",     Th128.Stage.A2_3,  Level.Lunatic),
                new CardInfo( 77, "「スリーフェアリーズ」",                   Th128.Stage.A2_3,  Level.Easy),
                new CardInfo( 78, "「スリーフェアリーズ」",                   Th128.Stage.A2_3,  Level.Normal),
                new CardInfo( 79, "「スリーフェアリーズ」",                   Th128.Stage.A2_3,  Level.Hard),
                new CardInfo( 80, "「スリーフェアリーズ」",                   Th128.Stage.A2_3,  Level.Lunatic),
                new CardInfo( 81, "日符「アグレッシブライト」",               Th128.Stage.B_1,   Level.Easy),
                new CardInfo( 82, "日符「アグレッシブライト」",               Th128.Stage.B_1,   Level.Normal),
                new CardInfo( 83, "日符「アグレッシブライト」",               Th128.Stage.B_1,   Level.Hard),
                new CardInfo( 84, "日符「アグレッシブライト」",               Th128.Stage.B_1,   Level.Lunatic),
                new CardInfo( 85, "日符「ダイレクトサンライト」",             Th128.Stage.B_1,   Level.Easy),
                new CardInfo( 86, "日符「ダイレクトサンライト」",             Th128.Stage.B_1,   Level.Normal),
                new CardInfo( 87, "日符「ダイレクトサンライト」",             Th128.Stage.B_1,   Level.Hard),
                new CardInfo( 88, "日符「ダイレクトサンライト」",             Th128.Stage.B_1,   Level.Lunatic),
                new CardInfo( 89, "流星「コメットストリーム」",               Th128.Stage.B1_2,  Level.Easy),
                new CardInfo( 90, "流星「コメットストリーム」",               Th128.Stage.B1_2,  Level.Normal),
                new CardInfo( 91, "流星「コメットストリーム」",               Th128.Stage.B1_2,  Level.Hard),
                new CardInfo( 92, "流星「コメットストリーム」",               Th128.Stage.B1_2,  Level.Lunatic),
                new CardInfo( 93, "星粒「スターピースシャワー」",             Th128.Stage.B1_2,  Level.Easy),
                new CardInfo( 94, "星粒「スターピースシャワー」",             Th128.Stage.B1_2,  Level.Normal),
                new CardInfo( 95, "星粒「スターピースシャワー」",             Th128.Stage.B1_2,  Level.Hard),
                new CardInfo( 96, "星粒「スターピースシャワー」",             Th128.Stage.B1_2,  Level.Lunatic),
                new CardInfo( 97, "星符「シューティングサファイア」",         Th128.Stage.B1_2,  Level.Easy),
                new CardInfo( 98, "星符「シューティングサファイア」",         Th128.Stage.B1_2,  Level.Normal),
                new CardInfo( 99, "星符「シューティングサファイア」",         Th128.Stage.B1_2,  Level.Hard),
                new CardInfo(100, "星符「シューティングサファイア」",         Th128.Stage.B1_2,  Level.Lunatic),
                new CardInfo(101, "月光「サイレントストーム」",               Th128.Stage.B1_3,  Level.Easy),
                new CardInfo(102, "月光「サイレントストーム」",               Th128.Stage.B1_3,  Level.Normal),
                new CardInfo(103, "月光「サイレントストーム」",               Th128.Stage.B1_3,  Level.Hard),
                new CardInfo(104, "月光「サイレントストーム」",               Th128.Stage.B1_3,  Level.Lunatic),
                new CardInfo(105, "光符「ブライトナイト」",                   Th128.Stage.B1_3,  Level.Easy),
                new CardInfo(106, "光符「ブライトナイト」",                   Th128.Stage.B1_3,  Level.Normal),
                new CardInfo(107, "光符「ブライトナイト」",                   Th128.Stage.B1_3,  Level.Hard),
                new CardInfo(108, "光符「ブライトナイト」",                   Th128.Stage.B1_3,  Level.Lunatic),
                new CardInfo(109, "月熱「アイスディゾルバー」",               Th128.Stage.B1_3,  Level.Easy),
                new CardInfo(110, "月熱「アイスディゾルバー」",               Th128.Stage.B1_3,  Level.Normal),
                new CardInfo(111, "月熱「アイスディゾルバー」",               Th128.Stage.B1_3,  Level.Hard),
                new CardInfo(112, "月熱「アイスディゾルバー」",               Th128.Stage.B1_3,  Level.Lunatic),
                new CardInfo(113, "空符「ブレイクキャノピー」",               Th128.Stage.B1_3,  Level.Easy),
                new CardInfo(114, "空符「ブレイクキャノピー」",               Th128.Stage.B1_3,  Level.Normal),
                new CardInfo(115, "空符「ブレイクキャノピー」",               Th128.Stage.B1_3,  Level.Hard),
                new CardInfo(116, "空符「ブレイクキャノピー」",               Th128.Stage.B1_3,  Level.Lunatic),
                new CardInfo(117, "協力技「フェアリーオーバードライブ」",     Th128.Stage.B1_3,  Level.Easy),
                new CardInfo(118, "協力技「フェアリーオーバードライブ」",     Th128.Stage.B1_3,  Level.Normal),
                new CardInfo(119, "協力技「フェアリーオーバードライブ」",     Th128.Stage.B1_3,  Level.Hard),
                new CardInfo(120, "協力技「フェアリーオーバードライブ」",     Th128.Stage.B1_3,  Level.Lunatic),
                new CardInfo(121, "「スリーフェアリーズ」",                   Th128.Stage.B1_3,  Level.Easy),
                new CardInfo(122, "「スリーフェアリーズ」",                   Th128.Stage.B1_3,  Level.Normal),
                new CardInfo(123, "「スリーフェアリーズ」",                   Th128.Stage.B1_3,  Level.Hard),
                new CardInfo(124, "「スリーフェアリーズ」",                   Th128.Stage.B1_3,  Level.Lunatic),
                new CardInfo(125, "月光「ダークスティルネス」",               Th128.Stage.B2_2,  Level.Easy),
                new CardInfo(126, "月光「ダークスティルネス」",               Th128.Stage.B2_2,  Level.Normal),
                new CardInfo(127, "月光「ダークスティルネス」",               Th128.Stage.B2_2,  Level.Hard),
                new CardInfo(128, "月光「ダークスティルネス」",               Th128.Stage.B2_2,  Level.Lunatic),
                new CardInfo(129, "障光「ムーンライトウォール」",             Th128.Stage.B2_2,  Level.Easy),
                new CardInfo(130, "障光「ムーンライトウォール」",             Th128.Stage.B2_2,  Level.Normal),
                new CardInfo(131, "障光「ムーンライトウォール」",             Th128.Stage.B2_2,  Level.Hard),
                new CardInfo(132, "障光「ムーンライトウォール」",             Th128.Stage.B2_2,  Level.Lunatic),
                new CardInfo(133, "夜符「ナイトフェアリーズ」",               Th128.Stage.B2_2,  Level.Easy),
                new CardInfo(134, "夜符「ナイトフェアリーズ」",               Th128.Stage.B2_2,  Level.Normal),
                new CardInfo(135, "夜符「ナイトフェアリーズ」",               Th128.Stage.B2_2,  Level.Hard),
                new CardInfo(136, "夜符「ナイトフェアリーズ」",               Th128.Stage.B2_2,  Level.Lunatic),
                new CardInfo(137, "星光「スターストーム」",                   Th128.Stage.B2_3,  Level.Easy),
                new CardInfo(138, "星光「スターストーム」",                   Th128.Stage.B2_3,  Level.Normal),
                new CardInfo(139, "星光「スターストーム」",                   Th128.Stage.B2_3,  Level.Hard),
                new CardInfo(140, "星光「スターストーム」",                   Th128.Stage.B2_3,  Level.Lunatic),
                new CardInfo(141, "光符「エクステンシブメテオ」",             Th128.Stage.B2_3,  Level.Easy),
                new CardInfo(142, "光符「エクステンシブメテオ」",             Th128.Stage.B2_3,  Level.Normal),
                new CardInfo(143, "光符「エクステンシブメテオ」",             Th128.Stage.B2_3,  Level.Hard),
                new CardInfo(144, "光符「エクステンシブメテオ」",             Th128.Stage.B2_3,  Level.Lunatic),
                new CardInfo(145, "星熱「アイスディゾルバー」",               Th128.Stage.B2_3,  Level.Easy),
                new CardInfo(146, "星熱「アイスディゾルバー」",               Th128.Stage.B2_3,  Level.Normal),
                new CardInfo(147, "星熱「アイスディゾルバー」",               Th128.Stage.B2_3,  Level.Hard),
                new CardInfo(148, "星熱「アイスディゾルバー」",               Th128.Stage.B2_3,  Level.Lunatic),
                new CardInfo(149, "光星「グレートトライアングル」",           Th128.Stage.B2_3,  Level.Easy),
                new CardInfo(150, "光星「グレートトライアングル」",           Th128.Stage.B2_3,  Level.Normal),
                new CardInfo(151, "光星「グレートトライアングル」",           Th128.Stage.B2_3,  Level.Hard),
                new CardInfo(152, "光星「グレートトライアングル」",           Th128.Stage.B2_3,  Level.Lunatic),
                new CardInfo(153, "協力技「フェアリーオーバードライブ」",     Th128.Stage.B2_3,  Level.Easy),
                new CardInfo(154, "協力技「フェアリーオーバードライブ」",     Th128.Stage.B2_3,  Level.Normal),
                new CardInfo(155, "協力技「フェアリーオーバードライブ」",     Th128.Stage.B2_3,  Level.Hard),
                new CardInfo(156, "協力技「フェアリーオーバードライブ」",     Th128.Stage.B2_3,  Level.Lunatic),
                new CardInfo(157, "「スリーフェアリーズ」",                   Th128.Stage.B2_3,  Level.Easy),
                new CardInfo(158, "「スリーフェアリーズ」",                   Th128.Stage.B2_3,  Level.Normal),
                new CardInfo(159, "「スリーフェアリーズ」",                   Th128.Stage.B2_3,  Level.Hard),
                new CardInfo(160, "「スリーフェアリーズ」",                   Th128.Stage.B2_3,  Level.Lunatic),
                new CardInfo(161, "星符「スターライトレイン」",               Th128.Stage.C_1,   Level.Easy),
                new CardInfo(162, "星符「スターライトレイン」",               Th128.Stage.C_1,   Level.Normal),
                new CardInfo(163, "星符「スターライトレイン」",               Th128.Stage.C_1,   Level.Hard),
                new CardInfo(164, "星符「スターライトレイン」",               Th128.Stage.C_1,   Level.Lunatic),
                new CardInfo(165, "星符「レッドスター」",                     Th128.Stage.C_1,   Level.Easy),
                new CardInfo(166, "星符「レッドスター」",                     Th128.Stage.C_1,   Level.Normal),
                new CardInfo(167, "星符「レッドスター」",                     Th128.Stage.C_1,   Level.Hard),
                new CardInfo(168, "星符「レッドスター」",                     Th128.Stage.C_1,   Level.Lunatic),
                new CardInfo(169, "瞬光「フェイタルフラッシュ」",             Th128.Stage.C1_2,  Level.Easy),
                new CardInfo(170, "瞬光「フェイタルフラッシュ」",             Th128.Stage.C1_2,  Level.Normal),
                new CardInfo(171, "瞬光「フェイタルフラッシュ」",             Th128.Stage.C1_2,  Level.Hard),
                new CardInfo(172, "瞬光「フェイタルフラッシュ」",             Th128.Stage.C1_2,  Level.Lunatic),
                new CardInfo(173, "光精「クロスディフュージョン」",           Th128.Stage.C1_2,  Level.Easy),
                new CardInfo(174, "光精「クロスディフュージョン」",           Th128.Stage.C1_2,  Level.Normal),
                new CardInfo(175, "光精「クロスディフュージョン」",           Th128.Stage.C1_2,  Level.Hard),
                new CardInfo(176, "光精「クロスディフュージョン」",           Th128.Stage.C1_2,  Level.Lunatic),
                new CardInfo(177, "光符「イエローディフレクション」",         Th128.Stage.C1_2,  Level.Easy),
                new CardInfo(178, "光符「イエローディフレクション」",         Th128.Stage.C1_2,  Level.Normal),
                new CardInfo(179, "光符「イエローディフレクション」",         Th128.Stage.C1_2,  Level.Hard),
                new CardInfo(180, "光符「イエローディフレクション」",         Th128.Stage.C1_2,  Level.Lunatic),
                new CardInfo(181, "月光「サイレントフラワー」",               Th128.Stage.C1_3,  Level.Easy),
                new CardInfo(182, "月光「サイレントフラワー」",               Th128.Stage.C1_3,  Level.Normal),
                new CardInfo(183, "月光「サイレントフラワー」",               Th128.Stage.C1_3,  Level.Hard),
                new CardInfo(184, "月光「サイレントフラワー」",               Th128.Stage.C1_3,  Level.Lunatic),
                new CardInfo(185, "光符「フルムーンナイト」",                 Th128.Stage.C1_3,  Level.Easy),
                new CardInfo(186, "光符「フルムーンナイト」",                 Th128.Stage.C1_3,  Level.Normal),
                new CardInfo(187, "光符「フルムーンナイト」",                 Th128.Stage.C1_3,  Level.Hard),
                new CardInfo(188, "光符「フルムーンナイト」",                 Th128.Stage.C1_3,  Level.Lunatic),
                new CardInfo(189, "月熱「アイスディゾルバー」",               Th128.Stage.C1_3,  Level.Easy),
                new CardInfo(190, "月熱「アイスディゾルバー」",               Th128.Stage.C1_3,  Level.Normal),
                new CardInfo(191, "月熱「アイスディゾルバー」",               Th128.Stage.C1_3,  Level.Hard),
                new CardInfo(192, "月熱「アイスディゾルバー」",               Th128.Stage.C1_3,  Level.Lunatic),
                new CardInfo(193, "降光「トリプルライト」",                   Th128.Stage.C1_3,  Level.Easy),
                new CardInfo(194, "降光「トリプルライト」",                   Th128.Stage.C1_3,  Level.Normal),
                new CardInfo(195, "降光「トリプルライト」",                   Th128.Stage.C1_3,  Level.Hard),
                new CardInfo(196, "降光「トリプルライト」",                   Th128.Stage.C1_3,  Level.Lunatic),
                new CardInfo(197, "協力技「フェアリーオーバードライブ」",     Th128.Stage.C1_3,  Level.Easy),
                new CardInfo(198, "協力技「フェアリーオーバードライブ」",     Th128.Stage.C1_3,  Level.Normal),
                new CardInfo(199, "協力技「フェアリーオーバードライブ」",     Th128.Stage.C1_3,  Level.Hard),
                new CardInfo(200, "協力技「フェアリーオーバードライブ」",     Th128.Stage.C1_3,  Level.Lunatic),
                new CardInfo(201, "「スリーフェアリーズ」",                   Th128.Stage.C1_3,  Level.Easy),
                new CardInfo(202, "「スリーフェアリーズ」",                   Th128.Stage.C1_3,  Level.Normal),
                new CardInfo(203, "「スリーフェアリーズ」",                   Th128.Stage.C1_3,  Level.Hard),
                new CardInfo(204, "「スリーフェアリーズ」",                   Th128.Stage.C1_3,  Level.Lunatic),
                new CardInfo(205, "月光「ムーンスティルネス」",               Th128.Stage.C2_2,  Level.Easy),
                new CardInfo(206, "月光「ムーンスティルネス」",               Th128.Stage.C2_2,  Level.Normal),
                new CardInfo(207, "月光「ムーンスティルネス」",               Th128.Stage.C2_2,  Level.Hard),
                new CardInfo(208, "月光「ムーンスティルネス」",               Th128.Stage.C2_2,  Level.Lunatic),
                new CardInfo(209, "光壁「ウォールブレイク」",                 Th128.Stage.C2_2,  Level.Easy),
                new CardInfo(210, "光壁「ウォールブレイク」",                 Th128.Stage.C2_2,  Level.Normal),
                new CardInfo(211, "光壁「ウォールブレイク」",                 Th128.Stage.C2_2,  Level.Hard),
                new CardInfo(212, "光壁「ウォールブレイク」",                 Th128.Stage.C2_2,  Level.Lunatic),
                new CardInfo(213, "月光「サイレントサイクロン」",             Th128.Stage.C2_2,  Level.Easy),
                new CardInfo(214, "月光「サイレントサイクロン」",             Th128.Stage.C2_2,  Level.Normal),
                new CardInfo(215, "月光「サイレントサイクロン」",             Th128.Stage.C2_2,  Level.Hard),
                new CardInfo(216, "月光「サイレントサイクロン」",             Th128.Stage.C2_2,  Level.Lunatic),
                new CardInfo(217, "陽光「サンシャインニードル」",             Th128.Stage.C2_3,  Level.Easy),
                new CardInfo(218, "陽光「サンシャインニードル」",             Th128.Stage.C2_3,  Level.Normal),
                new CardInfo(219, "陽光「サンシャインニードル」",             Th128.Stage.C2_3,  Level.Hard),
                new CardInfo(220, "陽光「サンシャインニードル」",             Th128.Stage.C2_3,  Level.Lunatic),
                new CardInfo(221, "光符「ハイパーインクレクション」",         Th128.Stage.C2_3,  Level.Easy),
                new CardInfo(222, "光符「ハイパーインクレクション」",         Th128.Stage.C2_3,  Level.Normal),
                new CardInfo(223, "光符「ハイパーインクレクション」",         Th128.Stage.C2_3,  Level.Hard),
                new CardInfo(224, "光符「ハイパーインクレクション」",         Th128.Stage.C2_3,  Level.Lunatic),
                new CardInfo(225, "日熱「アイスディゾルバー」",               Th128.Stage.C2_3,  Level.Easy),
                new CardInfo(226, "日熱「アイスディゾルバー」",               Th128.Stage.C2_3,  Level.Normal),
                new CardInfo(227, "日熱「アイスディゾルバー」",               Th128.Stage.C2_3,  Level.Hard),
                new CardInfo(228, "日熱「アイスディゾルバー」",               Th128.Stage.C2_3,  Level.Lunatic),
                new CardInfo(229, "激光「サンバースト」",                     Th128.Stage.C2_3,  Level.Easy),
                new CardInfo(230, "激光「サンバースト」",                     Th128.Stage.C2_3,  Level.Normal),
                new CardInfo(231, "激光「サンバースト」",                     Th128.Stage.C2_3,  Level.Hard),
                new CardInfo(232, "激光「サンバースト」",                     Th128.Stage.C2_3,  Level.Lunatic),
                new CardInfo(233, "協力技「フェアリーオーバードライブ」",     Th128.Stage.C2_3,  Level.Easy),
                new CardInfo(234, "協力技「フェアリーオーバードライブ」",     Th128.Stage.C2_3,  Level.Normal),
                new CardInfo(235, "協力技「フェアリーオーバードライブ」",     Th128.Stage.C2_3,  Level.Hard),
                new CardInfo(236, "協力技「フェアリーオーバードライブ」",     Th128.Stage.C2_3,  Level.Lunatic),
                new CardInfo(237, "「スリーフェアリーズ」",                   Th128.Stage.C2_3,  Level.Easy),
                new CardInfo(238, "「スリーフェアリーズ」",                   Th128.Stage.C2_3,  Level.Normal),
                new CardInfo(239, "「スリーフェアリーズ」",                   Th128.Stage.C2_3,  Level.Hard),
                new CardInfo(240, "「スリーフェアリーズ」",                   Th128.Stage.C2_3,  Level.Lunatic),
                new CardInfo(241, "光符「ミステリアスビーム」",               Th128.Stage.Extra, Level.Extra),
                new CardInfo(242, "光撃「シュート・ザ・リトルムーン」",       Th128.Stage.Extra, Level.Extra),
                new CardInfo(243, "魔弾「テストスレイブ」",                   Th128.Stage.Extra, Level.Extra),
                new CardInfo(244, "閉符「ビッグクランチ」",                   Th128.Stage.Extra, Level.Extra),
                new CardInfo(245, "恋符「マスタースパークのような懐中電灯」", Th128.Stage.Extra, Level.Extra),
                new CardInfo(246, "魔開「オープンユニバース」",               Th128.Stage.Extra, Level.Extra),
                new CardInfo(247, "魔十字「グランドクロス」",                 Th128.Stage.Extra, Level.Extra),
                new CardInfo(248, "流星「スーパーペルセイド」",               Th128.Stage.Extra, Level.Extra),
                new CardInfo(249, "「ブレイジングスターのような鬼ごっこ」",   Th128.Stage.Extra, Level.Extra),
                new CardInfo(250, "「妖精尽滅光」",                           Th128.Stage.Extra, Level.Extra),
            }.ToDictionary(card => card.Id);

        private static readonly EnumShortNameParser<Route> RouteParser =
            new EnumShortNameParser<Route>();

        private static readonly EnumShortNameParser<RouteWithTotal> RouteWithTotalParser =
            new EnumShortNameParser<RouteWithTotal>();

        private static new readonly EnumShortNameParser<StageWithTotal> StageWithTotalParser =
            new EnumShortNameParser<StageWithTotal>();

        private AllScoreData allScoreData = null;

        public override string SupportedVersions
        {
            get { return "1.00a"; }
        }

        protected override bool ReadScoreFile(Stream input)
        {
            using (var decrypted = new MemoryStream())
#if DEBUG
            using (var decoded = new FileStream("th128decoded.dat", FileMode.Create, FileAccess.ReadWrite))
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
                if (!Validate(decoded))
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
                new ScoreReplacer(this),
                new CareerReplacer(this),
                new CardReplacer(this, hideUntriedCards),
                new CollectRateReplacer(this),
                new ClearReplacer(this),
                new RouteReplacer(this),
                new RouteExReplacer(this),
                new TimeReplacer(this),
            };
        }

        private static bool Decrypt(Stream input, Stream output)
        {
            using (var reader = new BinaryReader(input, Encoding.UTF8, true))
            using (var writer = new BinaryWriter(output, Encoding.UTF8, true))
            {
                var header = new Header();
                header.ReadFrom(reader);
                if (!header.IsValid)
                    return false;
                if (header.EncodedAllSize != reader.BaseStream.Length)
                    return false;

                header.WriteTo(writer);
                ThCrypt.Decrypt(input, output, header.EncodedBodySize, 0xAC, 0x35, 0x10, header.EncodedBodySize);

                return true;
            }
        }

        private static bool Extract(Stream input, Stream output)
        {
            using (var reader = new BinaryReader(input, Encoding.UTF8, true))
            using (var writer = new BinaryWriter(output, Encoding.UTF8, true))
            {
                var header = new Header();
                header.ReadFrom(reader);
                header.WriteTo(writer);

                var bodyBeginPos = output.Position;
                Lzss.Extract(input, output);
                output.Flush();
                output.SetLength(output.Position);

                return header.DecodedBodySize == (output.Position - bodyBeginPos);
            }
        }

        private static bool Validate(Stream input)
        {
            using (var reader = new BinaryReader(input, Encoding.UTF8, true))
            {
                var header = new Header();
                header.ReadFrom(reader);
                var remainSize = header.DecodedBodySize;
                var chapter = new Th10.Chapter();

                try
                {
                    while (remainSize > 0)
                    {
                        chapter.ReadFrom(reader);
                        if (!chapter.IsValid)
                            return false;
                        if (!ClearData.CanInitialize(chapter) &&
                            !CardData.CanInitialize(chapter) &&
                            !Status.CanInitialize(chapter))
                            return false;

                        remainSize -= chapter.Size;
                    }
                }
                catch (EndOfStreamException)
                {
                    // It's OK, do nothing.
                }

                return remainSize == 0;
            }
        }

        private static AllScoreData Read(Stream input)
        {
            var dictionary = new Dictionary<string, Action<AllScoreData, Th10.Chapter>>
            {
                { ClearData.ValidSignature, (data, ch) => data.Set(new ClearData(ch)) },
                { CardData.ValidSignature,  (data, ch) => data.Set(new CardData(ch))  },
                { Status.ValidSignature,    (data, ch) => data.Set(new Status(ch))    },
            };

            using (var reader = new BinaryReader(input, Encoding.UTF8, true))
            {
                var allScoreData = new AllScoreData();
                var chapter = new Th10.Chapter();

                var header = new Header();
                header.ReadFrom(reader);
                allScoreData.Set(header);

                try
                {
                    while (true)
                    {
                        chapter.ReadFrom(reader);
                        if (dictionary.TryGetValue(chapter.Signature, out var setChapter))
                            setChapter(allScoreData, chapter);
                    }
                }
                catch (EndOfStreamException)
                {
                    // It's OK, do nothing.
                }

                if ((allScoreData.Header != null) &&
                    (allScoreData.ClearData.Count == Enum.GetValues(typeof(RouteWithTotal)).Length) &&
                    (allScoreData.CardData != null) &&
                    (allScoreData.Status != null))
                    return allScoreData;
                else
                    return null;
            }
        }

        // %T128SCR[w][xx][y][z]
        private class ScoreReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T128SCR({0})({1})(\d)([1-5])", LevelParser.Pattern, RouteParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public ScoreReplacer(Th128Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var level = LevelParser.Parse(match.Groups[1].Value);
                    var route = (RouteWithTotal)RouteParser.Parse(match.Groups[2].Value);
                    var rank = Utils.ToZeroBased(
                        int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture));
                    var type = int.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture);

                    if ((level == Level.Extra) && (route != RouteWithTotal.Extra))
                        return match.ToString();
                    if ((route == RouteWithTotal.Extra) && (level != Level.Extra))
                        return match.ToString();

                    var ranking = parent.allScoreData.ClearData[route].Rankings[level][rank];
                    switch (type)
                    {
                        case 1:     // name
                            return Encoding.Default.GetString(ranking.Name.ToArray()).Split('\0')[0];
                        case 2:     // score
                            return Utils.ToNumberString((ranking.Score * 10) + ranking.ContinueCount);
                        case 3:     // stage
                            if (ranking.DateTime == 0)
                                return StageProgress.None.ToShortName();
                            return ranking.StageProgress.ToShortName();
                        case 4:     // date & time
                            if (ranking.DateTime == 0)
                                return "----/--/-- --:--:--";
                            return new DateTime(1970, 1, 1).AddSeconds(ranking.DateTime).ToLocalTime()
                                .ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.CurrentCulture);
                        case 5:     // slow
                            if (ranking.DateTime == 0)
                                return "-----%";
                            return Utils.Format("{0:F3}%", ranking.SlowRate);
                        default:    // unreachable
                            return match.ToString();
                    }
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        // %T128C[xxx][z]
        private class CareerReplacer : IStringReplaceable
        {
            private const string Pattern = @"%T128C(\d{3})([1-3])";

            private readonly MatchEvaluator evaluator;

            [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
            public CareerReplacer(Th128Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var number = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
                    var type = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);

                    Func<ISpellCard, int> getCount;
                    if (type == 1)
                        getCount = (card => card.NoIceCount);
                    else if (type == 2)
                        getCount = (card => card.NoMissCount);
                    else
                        getCount = (card => card.TrialCount);

                    if (number == 0)
                    {
                        return Utils.ToNumberString(
                            parent.allScoreData.CardData.Cards.Values.Sum(getCount));
                    }
                    else if (CardTable.ContainsKey(number))
                    {
                        if (parent.allScoreData.CardData.Cards.TryGetValue(number, out var card))
                            return Utils.ToNumberString(getCount(card));
                        else
                            return "0";
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

        // %T128CARD[xxx][y]
        private class CardReplacer : IStringReplaceable
        {
            private const string Pattern = @"%T128CARD(\d{3})([NR])";

            private readonly MatchEvaluator evaluator;

            public CardReplacer(Th128Converter parent, bool hideUntriedCards)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var number = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
                    var type = match.Groups[2].Value.ToUpperInvariant();

                    if (CardTable.ContainsKey(number))
                    {
                        if (type == "N")
                        {
                            if (hideUntriedCards)
                            {
                                var cards = parent.allScoreData.CardData.Cards;
                                if (!cards.TryGetValue(number, out var card) || !card.HasTried())
                                    return "??????????";
                            }

                            return CardTable[number].Name;
                        }
                        else
                        {
                            return CardTable[number].Level.ToString();
                        }
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

        // %T128CRG[x][yyy][z]
        private class CollectRateReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T128CRG({0})({1})([1-3])", LevelWithTotalParser.Pattern, StageWithTotalParser.Pattern);

            private readonly MatchEvaluator evaluator;

            [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
            public CollectRateReplacer(Th128Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var level = LevelWithTotalParser.Parse(match.Groups[1].Value);
                    var stage = StageWithTotalParser.Parse(match.Groups[2].Value);
                    var type = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

                    if (stage == StageWithTotal.Extra)
                        return match.ToString();

                    Func<ISpellCard, bool> findByStage;
                    if (stage == StageWithTotal.Total)
                        findByStage = (card => true);
                    else
                        findByStage = (card => CardTable[card.Id].Stage == (Th128.Stage)stage);

                    Func<ISpellCard, bool> findByLevel = (card => true);
                    switch (level)
                    {
                        case LevelWithTotal.Total:
                            // Do nothing
                            break;
                        case LevelWithTotal.Extra:
                            findByStage = (card => CardTable[card.Id].Stage == Th128.Stage.Extra);
                            break;
                        default:
                            findByLevel = (card => card.Level == (Level)level);
                            break;
                    }

                    Func<ISpellCard, bool> findByType;
                    if (type == 1)
                        findByType = (card => card.NoIceCount > 0);
                    else if (type == 2)
                        findByType = (card => card.NoMissCount > 0);
                    else
                        findByType = (card => card.TrialCount > 0);

                    return parent.allScoreData.CardData.Cards.Values
                        .Count(Utils.MakeAndPredicate(findByLevel, findByStage, findByType))
                        .ToString(CultureInfo.CurrentCulture);
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        // %T128CLEAR[x][yy]
        private class ClearReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T128CLEAR({0})({1})", LevelParser.Pattern, RouteParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public ClearReplacer(Th128Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var level = LevelParser.Parse(match.Groups[1].Value);
                    var route = (RouteWithTotal)RouteParser.Parse(match.Groups[2].Value);

                    if ((level == Level.Extra) && (route != RouteWithTotal.Extra))
                        return match.ToString();
                    if ((route == RouteWithTotal.Extra) && (level != Level.Extra))
                        return match.ToString();

                    var rankings = parent.allScoreData.ClearData[route].Rankings[level]
                        .Where(ranking => ranking.DateTime > 0);
                    var stageProgress = rankings.Any()
                        ? rankings.Max(ranking => ranking.StageProgress) : StageProgress.None;

                    return stageProgress.ToShortName();
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        // %T128ROUTE[xx][y]
        private class RouteReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T128ROUTE({0})([1-3])", RouteWithTotalParser.Pattern);

            private readonly MatchEvaluator evaluator;

            [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
            public RouteReplacer(Th128Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var route = RouteWithTotalParser.Parse(match.Groups[1].Value);
                    var type = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);

                    Func<IClearData, long> getValueByType;
                    Func<long, string> toString;
                    if (type == 1)
                    {
                        getValueByType = (data => data.TotalPlayCount);
                        toString = Utils.ToNumberString;
                    }
                    else if (type == 2)
                    {
                        getValueByType = (data => data.PlayTime);
                        toString = (value => new Time(value).ToString());
                    }
                    else
                    {
                        getValueByType = (data => data.ClearCounts.Values.Sum());
                        toString = Utils.ToNumberString;
                    }

                    Func<AllScoreData, long> getValueByRoute;
                    if (route == RouteWithTotal.Total)
                    {
                        getValueByRoute = (allData => allData.ClearData.Values
                            .Where(data => data.Route != route).Sum(getValueByType));
                    }
                    else
                    {
                        getValueByRoute = (allData => getValueByType(allData.ClearData[route]));
                    }

                    return toString(getValueByRoute(parent.allScoreData));
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        // %T128ROUTEEX[x][yy][z]
        private class RouteExReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T128ROUTEEX({0})({1})([1-3])",
                LevelWithTotalParser.Pattern,
                RouteWithTotalParser.Pattern);

            private readonly MatchEvaluator evaluator;

            [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
            public RouteExReplacer(Th128Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var level = LevelWithTotalParser.Parse(match.Groups[1].Value);
                    var route = RouteWithTotalParser.Parse(match.Groups[2].Value);
                    var type = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

                    if ((level == LevelWithTotal.Extra) &&
                        ((route != RouteWithTotal.Extra) && (route != RouteWithTotal.Total)))
                        return match.ToString();
                    if ((route == RouteWithTotal.Extra) &&
                        ((level != LevelWithTotal.Extra) && (level != LevelWithTotal.Total)))
                        return match.ToString();

                    Func<IClearData, long> getValueByType;
                    Func<long, string> toString;
                    if (type == 1)
                    {
                        getValueByType = (data => data.TotalPlayCount);
                        toString = Utils.ToNumberString;
                    }
                    else if (type == 2)
                    {
                        getValueByType = (data => data.PlayTime);
                        toString = (value => new Time(value).ToString());
                    }
                    else
                    {
                        if (level == LevelWithTotal.Total)
                            getValueByType = (data => data.ClearCounts.Values.Sum());
                        else
                            getValueByType = (data => data.ClearCounts[(Level)level]);
                        toString = Utils.ToNumberString;
                    }

                    Func<AllScoreData, long> getValueByRoute;
                    if (route == RouteWithTotal.Total)
                    {
                        getValueByRoute = (allData => allData.ClearData.Values
                            .Where(data => data.Route != route).Sum(getValueByType));
                    }
                    else
                    {
                        getValueByRoute = (allData => getValueByType(allData.ClearData[route]));
                    }

                    return toString(getValueByRoute(parent.allScoreData));
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        // %T128TIMEPLY
        private class TimeReplacer : IStringReplaceable
        {
            private const string Pattern = @"%T128TIMEPLY";

            private readonly MatchEvaluator evaluator;

            public TimeReplacer(Th128Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    return new Time(parent.allScoreData.Status.TotalPlayTime * 10, false).ToLongString();
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        private class AllScoreData
        {
            private readonly Dictionary<RouteWithTotal, IClearData> clearData;

            public AllScoreData()
            {
                this.clearData =
                    new Dictionary<RouteWithTotal, IClearData>(Enum.GetValues(typeof(RouteWithTotal)).Length);
            }

            public Th095.HeaderBase Header { get; private set; }

            public IReadOnlyDictionary<RouteWithTotal, IClearData> ClearData => this.clearData;

            public ICardData CardData { get; private set; }

            public Th125.IStatus Status { get; private set; }

            public void Set(Th095.HeaderBase header) => this.Header = header;

            public void Set(IClearData data)
            {
                if (!this.clearData.ContainsKey(data.Route))
                    this.clearData.Add(data.Route, data);
            }

            public void Set(ICardData data) => this.CardData = data;

            public void Set(Th125.IStatus status) => this.Status = status;
        }

        private class ClearData : Th10.Chapter, IClearData  // per route
        {
            public const string ValidSignature = "CR";
            public const ushort ValidVersion = 0x0003;
            public const int ValidSize = 0x0000066C;

            public ClearData(Th10.Chapter chapter)
                : base(chapter, ValidSignature, ValidVersion, ValidSize)
            {
                var levels = Utils.GetEnumerator<Level>();

                using (var reader = new BinaryReader(new MemoryStream(this.Data, false)))
                {
                    this.Route = (RouteWithTotal)reader.ReadInt32();

                    this.Rankings = levels.ToDictionary(
                        level => level,
                        _ => Enumerable.Range(0, 10).Select(rank =>
                        {
                            var score = new ScoreData();
                            score.ReadFrom(reader);
                            return score;
                        }).ToList() as IReadOnlyList<Th10.IScoreData<StageProgress>>);

                    this.TotalPlayCount = reader.ReadInt32();
                    this.PlayTime = reader.ReadInt32();
                    this.ClearCounts = levels.ToDictionary(level => level, _ => reader.ReadInt32());
                }
            }

            public RouteWithTotal Route { get; }

            public IReadOnlyDictionary<Level, IReadOnlyList<Th10.IScoreData<StageProgress>>> Rankings { get; }

            public int TotalPlayCount { get; }

            public int PlayTime { get; }    // = seconds * 60fps

            public IReadOnlyDictionary<Level, int> ClearCounts { get; }

            public static bool CanInitialize(Th10.Chapter chapter)
            {
                return chapter.Signature.Equals(ValidSignature, StringComparison.Ordinal)
                    && (chapter.Version == ValidVersion)
                    && (chapter.Size == ValidSize);
            }
        }

        private class CardData : Th10.Chapter, ICardData
        {
            public const string ValidSignature = "CD";
            public const ushort ValidVersion = 0x0001;
            public const int ValidSize = 0x0000947C;

            public CardData(Th10.Chapter chapter)
                : base(chapter, ValidSignature, ValidVersion, ValidSize)
            {
                using (var reader = new BinaryReader(new MemoryStream(this.Data, false)))
                {
                    this.Cards = Enumerable.Range(0, CardTable.Count).Select(_ =>
                    {
                        var card = new SpellCard();
                        card.ReadFrom(reader);
                        return card as ISpellCard;
                    }).ToDictionary(card => card.Id);
                }
            }

            public IReadOnlyDictionary<int, ISpellCard> Cards { get; }

            public static bool CanInitialize(Th10.Chapter chapter)
            {
                return chapter.Signature.Equals(ValidSignature, StringComparison.Ordinal)
                    && (chapter.Version == ValidVersion)
                    && (chapter.Size == ValidSize);
            }
        }

        private class Status : Th10.Chapter, Th125.IStatus
        {
            public const string ValidSignature = "ST";
            public const ushort ValidVersion = 0x0002;
            public const int ValidSize = 0x0000042C;

            public Status(Th10.Chapter chapter)
                : base(chapter, ValidSignature, ValidVersion, ValidSize)
            {
                using (var reader = new BinaryReader(new MemoryStream(this.Data, false)))
                {
                    this.LastName = reader.ReadExactBytes(10);
                    reader.ReadExactBytes(0x10);
                    this.BgmFlags = reader.ReadExactBytes(10);
                    reader.ReadExactBytes(0x18);
                    this.TotalPlayTime = reader.ReadInt32();
                    reader.ReadExactBytes(0x03E0);
                }
            }

            public IEnumerable<byte> LastName { get; }  // The last 2 bytes are always 0x00 ?

            public IEnumerable<byte> BgmFlags { get; }

            public int TotalPlayTime { get; }   // unit: 10ms

            public static bool CanInitialize(Th10.Chapter chapter)
            {
                return chapter.Signature.Equals(ValidSignature, StringComparison.Ordinal)
                    && (chapter.Version == ValidVersion)
                    && (chapter.Size == ValidSize);
            }
        }

        private class SpellCard : IBinaryReadable, ISpellCard
        {
            public IEnumerable<byte> Name { get; private set; }

            public int NoMissCount { get; private set; }

            public int NoIceCount { get; private set; }

            public int TrialCount { get; private set; }

            public int Id { get; private set; } // 1-based

            public Level Level { get; private set; }

            public void ReadFrom(BinaryReader reader)
            {
                if (reader is null)
                    throw new ArgumentNullException(nameof(reader));

                this.Name = reader.ReadExactBytes(0x80);
                this.NoMissCount = reader.ReadInt32();
                this.NoIceCount = reader.ReadInt32();
                reader.ReadUInt32();
                this.TrialCount = reader.ReadInt32();
                this.Id = reader.ReadInt32() + 1;
                this.Level = Utils.ToEnum<Level>(reader.ReadInt32());
            }

            public bool HasTried() => this.TrialCount > 0;
        }
    }
}

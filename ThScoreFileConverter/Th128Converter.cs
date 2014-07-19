//-----------------------------------------------------------------------
// <copyright file="Th128Converter.cs" company="None">
//     (c) 2013-2014 IIHOSHI Yoshinori
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable 1591

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "*", Justification = "Reviewed.")]

namespace ThScoreFileConverter
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using CardInfo = SpellCardInfo<Th128Converter.Stage, ThConverter.Level>;

    internal class Th128Converter : ThConverter
    {
        // Thanks to thwiki.info
        [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1025:CodeMustNotContainMultipleWhitespaceInARow", Justification = "Reviewed.")]
        [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1008:OpeningParenthesisMustBeSpacedCorrectly", Justification = "Reviewed.")]
        private static readonly Dictionary<int, CardInfo> CardTable =
            new List<CardInfo>()
            {
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
                new CardInfo(250, "「妖精尽滅光」",                           Stage.Extra, Level.Extra)
            }.ToDictionary(card => card.Id);

        private static readonly EnumShortNameParser<Route> RouteParser =
            new EnumShortNameParser<Route>();

        private static readonly EnumShortNameParser<RouteWithTotal> RouteWithTotalParser =
            new EnumShortNameParser<RouteWithTotal>();

        private static readonly new EnumShortNameParser<StageWithTotal> StageWithTotalParser =
            new EnumShortNameParser<StageWithTotal>();

        private AllScoreData allScoreData = null;

        public Th128Converter()
        {
        }

        public enum Route
        {
            [EnumAltName("A1")] A1,
            [EnumAltName("A2")] A2,
            [EnumAltName("B1")] B1,
            [EnumAltName("B2")] B2,
            [EnumAltName("C1")] C1,
            [EnumAltName("C2")] C2,
            [EnumAltName("EX")] Extra
        }

        public enum RouteWithTotal
        {
            [EnumAltName("A1")] A1,
            [EnumAltName("A2")] A2,
            [EnumAltName("B1")] B1,
            [EnumAltName("B2")] B2,
            [EnumAltName("C1")] C1,
            [EnumAltName("C2")] C2,
            [EnumAltName("EX")] Extra,
            [EnumAltName("TL")] Total
        }

        public new enum Stage
        {
            [EnumAltName("A11")] A_1,
            [EnumAltName("A12")] A1_2,
            [EnumAltName("A13")] A1_3,
            [EnumAltName("A22")] A2_2,
            [EnumAltName("A23")] A2_3,
            [EnumAltName("B11")] B_1,
            [EnumAltName("B12")] B1_2,
            [EnumAltName("B13")] B1_3,
            [EnumAltName("B22")] B2_2,
            [EnumAltName("B23")] B2_3,
            [EnumAltName("C11")] C_1,
            [EnumAltName("C12")] C1_2,
            [EnumAltName("C13")] C1_3,
            [EnumAltName("C22")] C2_2,
            [EnumAltName("C23")] C2_3,
            [EnumAltName("EXT")] Extra
        }

        public new enum StageWithTotal
        {
            [EnumAltName("A11")] A_1,
            [EnumAltName("A12")] A1_2,
            [EnumAltName("A13")] A1_3,
            [EnumAltName("A22")] A2_2,
            [EnumAltName("A23")] A2_3,
            [EnumAltName("B11")] B_1,
            [EnumAltName("B12")] B1_2,
            [EnumAltName("B13")] B1_3,
            [EnumAltName("B22")] B2_2,
            [EnumAltName("B23")] B2_3,
            [EnumAltName("C11")] C_1,
            [EnumAltName("C12")] C1_2,
            [EnumAltName("C13")] C1_3,
            [EnumAltName("C22")] C2_2,
            [EnumAltName("C23")] C2_3,
            [EnumAltName("EXT")] Extra,
            [EnumAltName("TTL")] Total
        }

        [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1025:CodeMustNotContainMultipleWhitespaceInARow", Justification = "Reviewed.")]
        public enum StageProgress
        {
            [EnumAltName("-------")]     None,
            [EnumAltName("Stage A-1")]   A_1,
            [EnumAltName("Stage A1-2")]  A1_2,
            [EnumAltName("Stage A1-3")]  A1_3,
            [EnumAltName("Stage A2-2")]  A2_2,
            [EnumAltName("Stage A2-3")]  A2_3,
            [EnumAltName("Stage B-1")]   B_1,
            [EnumAltName("Stage B1-2")]  B1_2,
            [EnumAltName("Stage B1-3")]  B1_3,
            [EnumAltName("Stage B2-2")]  B2_2,
            [EnumAltName("Stage B2-3")]  B2_3,
            [EnumAltName("Stage C-1")]   C_1,
            [EnumAltName("Stage C1-2")]  C1_2,
            [EnumAltName("Stage C1-3")]  C1_3,
            [EnumAltName("Stage C2-2")]  C2_2,
            [EnumAltName("Stage C2-3")]  C2_3,
            [EnumAltName("Extra Stage")] Extra,
            [EnumAltName("A1 Clear")]    A1Clear,
            [EnumAltName("A2 Clear")]    A2Clear,
            [EnumAltName("B1 Clear")]    B1Clear,
            [EnumAltName("B2 Clear")]    B2Clear,
            [EnumAltName("C1 Clear")]    C1Clear,
            [EnumAltName("C2 Clear")]    C2Clear,
            [EnumAltName("Extra Clear")] ExtraClear
        }

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
                new TimeReplacer(this)
            };
        }

        private static bool Decrypt(Stream input, Stream output)
        {
            var reader = new BinaryReader(input);
            var writer = new BinaryWriter(output);

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

        private static bool Extract(Stream input, Stream output)
        {
            var reader = new BinaryReader(input);
            var writer = new BinaryWriter(output);

            var header = new Header();
            header.ReadFrom(reader);
            header.WriteTo(writer);

            var bodyBeginPos = output.Position;
            Lzss.Extract(input, output);
            output.Flush();
            output.SetLength(output.Position);

            return header.DecodedBodySize == (output.Position - bodyBeginPos);
        }

        private static bool Validate(Stream input)
        {
            var reader = new BinaryReader(input);

            var header = new Header();
            header.ReadFrom(reader);
            var remainSize = header.DecodedBodySize;
            var chapter = new Chapter();

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

        [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1025:CodeMustNotContainMultipleWhitespaceInARow", Justification = "Reviewed.")]
        private static AllScoreData Read(Stream input)
        {
            var dictionary = new Dictionary<string, Action<AllScoreData, Chapter>>
            {
                { ClearData.ValidSignature, (data, ch) => data.Set(new ClearData(ch)) },
                { CardData.ValidSignature,  (data, ch) => data.Set(new CardData(ch))  },
                { Status.ValidSignature,    (data, ch) => data.Set(new Status(ch))    }
            };

            var reader = new BinaryReader(input);
            var allScoreData = new AllScoreData();
            var chapter = new Chapter();

            var header = new Header();
            header.ReadFrom(reader);
            allScoreData.Set(header);

            try
            {
                Action<AllScoreData, Chapter> setChapter;
                while (true)
                {
                    chapter.ReadFrom(reader);
                    if (dictionary.TryGetValue(chapter.Signature, out setChapter))
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
                            return Encoding.Default.GetString(ranking.Name).Split('\0')[0];
                        case 2:     // score
                            return Utils.ToNumberString((ranking.Score * 10) + ranking.ContinueCount);
                        case 3:     // stage
                            if (ranking.DateTime > 0)
                                return ranking.StageProgress.ToShortName();
                            else
                                return StageProgress.None.ToShortName();
                        case 4:     // date & time
                            if (ranking.DateTime > 0)
                                return new DateTime(1970, 1, 1).AddSeconds(ranking.DateTime).ToLocalTime()
                                    .ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.CurrentCulture);
                            else
                                return "----/--/-- --:--:--";
                        case 5:     // slow
                            if (ranking.DateTime > 0)
                                return Utils.Format("{0:F3}%", ranking.SlowRate);
                            else
                                return "-----%";
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

                    Func<SpellCard, int> getCount = (card => 0);
                    if (type == 1)
                        getCount = (card => card.NoIceCount);
                    else if (type == 2)
                        getCount = (card => card.NoMissCount);
                    else
                        getCount = (card => card.TrialCount);

                    if (number == 0)
                        return Utils.ToNumberString(
                            parent.allScoreData.CardData.Cards.Values.Sum(getCount));
                    else if (CardTable.ContainsKey(number))
                    {
                        SpellCard card;
                        if (parent.allScoreData.CardData.Cards.TryGetValue(number, out card))
                            return Utils.ToNumberString(getCount(card));
                        else
                            return "0";
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
                                SpellCard card;
                                if (!cards.TryGetValue(number, out card) || !card.HasTried())
                                    return "??????????";
                            }

                            return CardTable[number].Name;
                        }
                        else
                            return CardTable[number].Level.ToString();
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

                    Func<SpellCard, bool> findByLevel = (card => true);
                    Func<SpellCard, bool> findByStage = (card => true);
                    Func<SpellCard, bool> findByType = (card => true);

                    if (stage == StageWithTotal.Total)
                    {
                        // Do nothing
                    }
                    else
                        findByStage = (card => CardTable[card.Id].Stage == (Stage)stage);

                    switch (level)
                    {
                        case LevelWithTotal.Total:
                            // Do nothing
                            break;
                        case LevelWithTotal.Extra:
                            findByStage = (card => CardTable[card.Id].Stage == Stage.Extra);
                            break;
                        default:
                            findByLevel = (card => card.Level == (Level)level);
                            break;
                    }

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
                    var stageProgress = (rankings.Count() > 0)
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

                    Func<ClearData, long> getValueByType = (data => 0L);
                    Func<long, string> toString = (value => string.Empty);
                    if (type == 1)
                    {
                        getValueByType = (data => data.TotalPlayCount);
                        toString = (value => Utils.ToNumberString(value));
                    }
                    else if (type == 2)
                    {
                        getValueByType = (data => data.PlayTime);
                        toString = (value => new Time(value).ToString());
                    }
                    else
                    {
                        getValueByType = (data => data.ClearCounts.Values.Sum());
                        toString = (value => Utils.ToNumberString(value));
                    }

                    Func<AllScoreData, long> getValueByRoute = (allData => 0L);
                    if (route == RouteWithTotal.Total)
                        getValueByRoute = (allData => allData.ClearData.Values.Sum(
                            data => (data.Route != route) ? getValueByType(data) : 0L));
                    else
                        getValueByRoute = (allData => getValueByType(allData.ClearData[route]));

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

                    Func<ClearData, long> getValueByType = (data => 0L);
                    Func<long, string> toString = (value => string.Empty);
                    if (type == 1)
                    {
                        getValueByType = (data => data.TotalPlayCount);
                        toString = (value => Utils.ToNumberString(value));
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
                        toString = (value => Utils.ToNumberString(value));
                    }

                    Func<AllScoreData, long> getValueByRoute = (allData => 0L);
                    if (route == RouteWithTotal.Total)
                        getValueByRoute = (allData => allData.ClearData.Values.Sum(
                            data => (data.Route != route) ? getValueByType(data) : 0L));
                    else
                        getValueByRoute = (allData => getValueByType(allData.ClearData[route]));

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
            public AllScoreData()
            {
                this.ClearData =
                    new Dictionary<RouteWithTotal, ClearData>(Enum.GetValues(typeof(RouteWithTotal)).Length);
            }

            public Header Header { get; private set; }

            public Dictionary<RouteWithTotal, ClearData> ClearData { get; private set; }

            public CardData CardData { get; private set; }

            public Status Status { get; private set; }

            public void Set(Header header)
            {
                this.Header = header;
            }

            public void Set(ClearData data)
            {
                if (!this.ClearData.ContainsKey(data.Route))
                    this.ClearData.Add(data.Route, data);
            }

            public void Set(CardData data)
            {
                this.CardData = data;
            }

            public void Set(Status status)
            {
                this.Status = status;
            }
        }

        private class Header : IBinaryReadable, IBinaryWritable
        {
            public const string ValidSignature = "T821";
            public const int SignatureSize = 4;
            public const int Size = SignatureSize + (sizeof(int) * 3) + (sizeof(uint) * 2);

            private uint unknown1;
            private uint unknown2;

            public string Signature { get; private set; }

            public int EncodedAllSize { get; private set; }

            public int EncodedBodySize { get; private set; }

            public int DecodedBodySize { get; private set; }

            public bool IsValid
            {
                get
                {
                    return this.Signature.Equals(ValidSignature, StringComparison.Ordinal)
                        && (this.EncodedAllSize - this.EncodedBodySize == Size);
                }
            }

            public void ReadFrom(BinaryReader reader)
            {
                this.Signature = Encoding.Default.GetString(reader.ReadBytes(SignatureSize));
                this.EncodedAllSize = reader.ReadInt32();
                this.unknown1 = reader.ReadUInt32();
                this.unknown2 = reader.ReadUInt32();
                this.EncodedBodySize = reader.ReadInt32();
                this.DecodedBodySize = reader.ReadInt32();
            }

            public void WriteTo(BinaryWriter writer)
            {
                writer.Write(Encoding.Default.GetBytes(this.Signature));
                writer.Write(this.EncodedAllSize);
                writer.Write(this.unknown1);
                writer.Write(this.unknown2);
                writer.Write(this.EncodedBodySize);
                writer.Write(this.DecodedBodySize);
            }
        }

        private class Chapter : IBinaryReadable
        {
            public const int SignatureSize = 2;

            public Chapter()
            {
                this.Signature = string.Empty;
                this.Version = 0;
                this.Checksum = 0;
                this.Size = 0;
                this.Data = new byte[] { };
            }

            protected Chapter(Chapter chapter)
            {
                this.Signature = chapter.Signature;
                this.Version = chapter.Version;
                this.Checksum = chapter.Checksum;
                this.Size = chapter.Size;
                this.Data = new byte[chapter.Data.Length];
                chapter.Data.CopyTo(this.Data, 0);
            }

            public string Signature { get; private set; }

            public ushort Version { get; private set; }

            public uint Checksum { get; private set; }

            public int Size { get; private set; }

            public bool IsValid
            {
                get
                {
                    var sum = BitConverter.GetBytes(this.Size).Concat(this.Data).Sum(elem => (uint)elem);
                    return sum == this.Checksum;
                }
            }

            protected byte[] Data { get; private set; }

            public void ReadFrom(BinaryReader reader)
            {
                this.Signature = Encoding.Default.GetString(reader.ReadBytes(SignatureSize));
                this.Version = reader.ReadUInt16();
                this.Checksum = reader.ReadUInt32();
                this.Size = reader.ReadInt32();
                this.Data = reader.ReadBytes(
                    this.Size - SignatureSize - sizeof(ushort) - sizeof(uint) - sizeof(int));
            }
        }

        private class ClearData : Chapter   // per route
        {
            public const string ValidSignature = "CR";
            public const ushort ValidVersion = 0x0003;
            public const int ValidSize = 0x0000066C;

            public ClearData(Chapter chapter)
                : base(chapter)
            {
                if (!this.Signature.Equals(ValidSignature, StringComparison.Ordinal))
                    throw new InvalidDataException("Signature");
                if (this.Version != ValidVersion)
                    throw new InvalidDataException("Version");
                if (this.Size != ValidSize)
                    throw new InvalidDataException("Size");

                var levels = Utils.GetEnumerator<Level>();
                var numLevels = levels.Count();
                this.Rankings = new Dictionary<Level, ScoreData[]>(numLevels);
                this.ClearCounts = new Dictionary<Level, int>(numLevels);

                using (var stream = new MemoryStream(this.Data, false))
                using (var reader = new BinaryReader(stream))
                {
                    this.Route = (RouteWithTotal)reader.ReadInt32();

                    foreach (var level in levels)
                    {
                        if (!this.Rankings.ContainsKey(level))
                            this.Rankings.Add(level, new ScoreData[10]);
                        for (var rank = 0; rank < 10; rank++)
                        {
                            var score = new ScoreData();
                            score.ReadFrom(reader);
                            this.Rankings[level][rank] = score;
                        }
                    }

                    this.TotalPlayCount = reader.ReadInt32();
                    this.PlayTime = reader.ReadInt32();

                    foreach (var level in levels)
                    {
                        var clearCount = reader.ReadInt32();
                        if (!this.ClearCounts.ContainsKey(level))
                            this.ClearCounts.Add(level, clearCount);
                    }
                }
            }

            public RouteWithTotal Route { get; private set; }   // size: 4Bytes

            public Dictionary<Level, ScoreData[]> Rankings { get; private set; }

            public int TotalPlayCount { get; private set; }

            public int PlayTime { get; private set; }           // = seconds * 60fps

            public Dictionary<Level, int> ClearCounts { get; private set; }

            public static bool CanInitialize(Chapter chapter)
            {
                return chapter.Signature.Equals(ValidSignature, StringComparison.Ordinal)
                    && (chapter.Version == ValidVersion)
                    && (chapter.Size == ValidSize);
            }
        }

        private class CardData : Chapter
        {
            public const string ValidSignature = "CD";
            public const ushort ValidVersion = 0x0001;
            public const int ValidSize = 0x0000947C;

            public CardData(Chapter chapter)
                : base(chapter)
            {
                if (!this.Signature.Equals(ValidSignature, StringComparison.Ordinal))
                    throw new InvalidDataException("Signature");
                if (this.Version != ValidVersion)
                    throw new InvalidDataException("Version");
                if (this.Size != ValidSize)
                    throw new InvalidDataException("Size");

                this.Cards = new Dictionary<int, SpellCard>(CardTable.Count);

                using (var stream = new MemoryStream(this.Data, false))
                using (var reader = new BinaryReader(stream))
                {
                    for (var number = 0; number < CardTable.Count; number++)
                    {
                        var card = new SpellCard();
                        card.ReadFrom(reader);
                        if (!this.Cards.ContainsKey(card.Id))
                            this.Cards.Add(card.Id, card);
                    }
                }
            }

            public Dictionary<int, SpellCard> Cards { get; private set; }

            public static bool CanInitialize(Chapter chapter)
            {
                return chapter.Signature.Equals(ValidSignature, StringComparison.Ordinal)
                    && (chapter.Version == ValidVersion)
                    && (chapter.Size == ValidSize);
            }
        }

        private class Status : Chapter
        {
            public const string ValidSignature = "ST";
            public const ushort ValidVersion = 0x0002;
            public const int ValidSize = 0x0000042C;

            public Status(Chapter chapter)
                : base(chapter)
            {
                if (!this.Signature.Equals(ValidSignature, StringComparison.Ordinal))
                    throw new InvalidDataException("Signature");
                if (this.Version != ValidVersion)
                    throw new InvalidDataException("Version");
                if (this.Size != ValidSize)
                    throw new InvalidDataException("Size");

                using (var stream = new MemoryStream(this.Data, false))
                using (var reader = new BinaryReader(stream))
                {
                    this.LastName = reader.ReadBytes(10);
                    reader.ReadBytes(0x10);
                    this.BgmFlags = reader.ReadBytes(10);
                    reader.ReadBytes(0x18);
                    this.TotalPlayTime = reader.ReadInt32();
                    reader.ReadBytes(0x03E0);
                }
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public byte[] LastName { get; private set; }    // .Length = 10 (The last 2 bytes are always 0x00 ?)

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public byte[] BgmFlags { get; private set; }    // .Length = 10

            public int TotalPlayTime { get; private set; }  // unit: 10ms

            public static bool CanInitialize(Chapter chapter)
            {
                return chapter.Signature.Equals(ValidSignature, StringComparison.Ordinal)
                    && (chapter.Version == ValidVersion)
                    && (chapter.Size == ValidSize);
            }
        }

        private class ScoreData : IBinaryReadable
        {
            public uint Score { get; private set; }     // * 10

            public StageProgress StageProgress { get; private set; }    // size: 1Byte

            public byte ContinueCount { get; private set; }

            public byte[] Name { get; private set; }    // .Length = 10 (The last 2 bytes are always 0x00 ?)

            public uint DateTime { get; private set; }  // UNIX time (unit: [s])

            public float SlowRate { get; private set; } // really...?

            public void ReadFrom(BinaryReader reader)
            {
                this.Score = reader.ReadUInt32();
                this.StageProgress = (StageProgress)reader.ReadByte();
                this.ContinueCount = reader.ReadByte();
                this.Name = reader.ReadBytes(10);
                this.DateTime = reader.ReadUInt32();
                this.SlowRate = reader.ReadSingle();
                reader.ReadBytes(0x08);
            }
        }

        private class SpellCard : IBinaryReadable
        {
            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public byte[] Name { get; private set; }        // .Length = 0x80

            public int NoMissCount { get; private set; }

            public int NoIceCount { get; private set; }

            public int TrialCount { get; private set; }

            public int Id { get; private set; }             // 1-based

            public Level Level { get; private set; }

            public void ReadFrom(BinaryReader reader)
            {
                this.Name = reader.ReadBytes(0x80);
                this.NoMissCount = reader.ReadInt32();
                this.NoIceCount = reader.ReadInt32();
                reader.ReadUInt32();
                this.TrialCount = reader.ReadInt32();
                this.Id = reader.ReadInt32() + 1;
                this.Level = (Level)reader.ReadInt32();
            }

            public bool HasTried()
            {
                return this.TrialCount > 0;
            }
        }
    }
}

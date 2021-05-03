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

namespace ThScoreFileConverter.Models.Th10
{
    internal static class Definitions
    {
        // Thanks to thwiki.info
        public static IReadOnlyDictionary<int, CardInfo> CardTable { get; } = new List<CardInfo>()
        {
#pragma warning disable SA1008 // Opening parenthesis should be spaced correctly
            new CardInfo(  1, "葉符「狂いの落葉」",                       Stage.One,   Level.Hard),
            new CardInfo(  2, "葉符「狂いの落葉」",                       Stage.One,   Level.Lunatic),
            new CardInfo(  3, "秋符「オータムスカイ」",                   Stage.One,   Level.Easy),
            new CardInfo(  4, "秋符「オータムスカイ」",                   Stage.One,   Level.Normal),
            new CardInfo(  5, "秋符「秋の空と乙女の心」",                 Stage.One,   Level.Hard),
            new CardInfo(  6, "秋符「秋の空と乙女の心」",                 Stage.One,   Level.Lunatic),
            new CardInfo(  7, "豊符「オヲトシハーベスター」",             Stage.One,   Level.Easy),
            new CardInfo(  8, "豊符「オヲトシハーベスター」",             Stage.One,   Level.Normal),
            new CardInfo(  9, "豊作「穀物神の約束」",                     Stage.One,   Level.Hard),
            new CardInfo( 10, "豊作「穀物神の約束」",                     Stage.One,   Level.Lunatic),
            new CardInfo( 11, "厄符「バッドフォーチュン」",               Stage.Two,   Level.Easy),
            new CardInfo( 12, "厄符「バッドフォーチュン」",               Stage.Two,   Level.Normal),
            new CardInfo( 13, "厄符「厄神様のバイオリズム」",             Stage.Two,   Level.Hard),
            new CardInfo( 14, "厄符「厄神様のバイオリズム」",             Stage.Two,   Level.Lunatic),
            new CardInfo( 15, "疵符「ブロークンアミュレット」",           Stage.Two,   Level.Easy),
            new CardInfo( 16, "疵符「ブロークンアミュレット」",           Stage.Two,   Level.Normal),
            new CardInfo( 17, "疵痕「壊されたお守り」",                   Stage.Two,   Level.Hard),
            new CardInfo( 18, "疵痕「壊されたお守り」",                   Stage.Two,   Level.Lunatic),
            new CardInfo( 19, "悪霊「ミスフォーチュンズホイール」",       Stage.Two,   Level.Easy),
            new CardInfo( 20, "悪霊「ミスフォーチュンズホイール」",       Stage.Two,   Level.Normal),
            new CardInfo( 21, "悲運「大鐘婆の火」",                       Stage.Two,   Level.Hard),
            new CardInfo( 22, "悲運「大鐘婆の火」",                       Stage.Two,   Level.Lunatic),
            new CardInfo( 23, "創符「ペインフロー」",                     Stage.Two,   Level.Easy),
            new CardInfo( 24, "創符「ペインフロー」",                     Stage.Two,   Level.Normal),
            new CardInfo( 25, "創符「流刑人形」",                         Stage.Two,   Level.Hard),
            new CardInfo( 26, "創符「流刑人形」",                         Stage.Two,   Level.Lunatic),
            new CardInfo( 27, "光学「オプティカルカモフラージュ」",       Stage.Three, Level.Easy),
            new CardInfo( 28, "光学「オプティカルカモフラージュ」",       Stage.Three, Level.Normal),
            new CardInfo( 29, "光学「ハイドロカモフラージュ」",           Stage.Three, Level.Hard),
            new CardInfo( 30, "光学「ハイドロカモフラージュ」",           Stage.Three, Level.Lunatic),
            new CardInfo( 31, "洪水「ウーズフラッディング」",             Stage.Three, Level.Easy),
            new CardInfo( 32, "洪水「ウーズフラッディング」",             Stage.Three, Level.Normal),
            new CardInfo( 33, "洪水「デリューヴィアルメア」",             Stage.Three, Level.Hard),
            new CardInfo( 34, "漂溺「光り輝く水底のトラウマ」",           Stage.Three, Level.Lunatic),
            new CardInfo( 35, "水符「河童のポロロッカ」",                 Stage.Three, Level.Easy),
            new CardInfo( 36, "水符「河童のポロロッカ」",                 Stage.Three, Level.Normal),
            new CardInfo( 37, "水符「河童のフラッシュフラッド」",         Stage.Three, Level.Hard),
            new CardInfo( 38, "水符「河童の幻想大瀑布」",                 Stage.Three, Level.Lunatic),
            new CardInfo( 39, "河童「お化けキューカンバー」",             Stage.Three, Level.Easy),
            new CardInfo( 40, "河童「お化けキューカンバー」",             Stage.Three, Level.Normal),
            new CardInfo( 41, "河童「のびーるアーム」",                   Stage.Three, Level.Hard),
            new CardInfo( 42, "河童「スピン・ザ・セファリックプレート」", Stage.Three, Level.Lunatic),
            new CardInfo( 43, "岐符「天の八衢」",                         Stage.Four,  Level.Easy),
            new CardInfo( 44, "岐符「天の八衢」",                         Stage.Four,  Level.Normal),
            new CardInfo( 45, "岐符「サルタクロス」",                     Stage.Four,  Level.Hard),
            new CardInfo( 46, "岐符「サルタクロス」",                     Stage.Four,  Level.Lunatic),
            new CardInfo( 47, "風神「風神木の葉隠れ」",                   Stage.Four,  Level.Easy),
            new CardInfo( 48, "風神「風神木の葉隠れ」",                   Stage.Four,  Level.Normal),
            new CardInfo( 49, "風神「天狗颪」",                           Stage.Four,  Level.Hard),
            new CardInfo( 50, "風神「二百十日」",                         Stage.Four,  Level.Lunatic),
            new CardInfo( 51, "「幻想風靡」",                             Stage.Four,  Level.Normal),
            new CardInfo( 52, "「幻想風靡」",                             Stage.Four,  Level.Hard),
            new CardInfo( 53, "「無双風神」",                             Stage.Four,  Level.Lunatic),
            new CardInfo( 54, "塞符「山神渡御」",                         Stage.Four,  Level.Easy),
            new CardInfo( 55, "塞符「山神渡御」",                         Stage.Four,  Level.Normal),
            new CardInfo( 56, "塞符「天孫降臨」",                         Stage.Four,  Level.Hard),
            new CardInfo( 57, "塞符「天上天下の照國」",                   Stage.Four,  Level.Lunatic),
            new CardInfo( 58, "秘術「グレイソーマタージ」",               Stage.Five,  Level.Easy),
            new CardInfo( 59, "秘術「グレイソーマタージ」",               Stage.Five,  Level.Normal),
            new CardInfo( 60, "秘術「忘却の祭儀」",                       Stage.Five,  Level.Hard),
            new CardInfo( 61, "秘術「一子相伝の弾幕」",                   Stage.Five,  Level.Lunatic),
            new CardInfo( 62, "奇跡「白昼の客星」",                       Stage.Five,  Level.Easy),
            new CardInfo( 63, "奇跡「白昼の客星」",                       Stage.Five,  Level.Normal),
            new CardInfo( 64, "奇跡「客星の明るい夜」",                   Stage.Five,  Level.Hard),
            new CardInfo( 65, "奇跡「客星の明るすぎる夜」",               Stage.Five,  Level.Lunatic),
            new CardInfo( 66, "開海「海が割れる日」",                     Stage.Five,  Level.Easy),
            new CardInfo( 67, "開海「海が割れる日」",                     Stage.Five,  Level.Normal),
            new CardInfo( 68, "開海「モーゼの奇跡」",                     Stage.Five,  Level.Hard),
            new CardInfo( 69, "開海「モーゼの奇跡」",                     Stage.Five,  Level.Lunatic),
            new CardInfo( 70, "準備「神風を喚ぶ星の儀式」",               Stage.Five,  Level.Easy),
            new CardInfo( 71, "準備「神風を喚ぶ星の儀式」",               Stage.Five,  Level.Normal),
            new CardInfo( 72, "準備「サモンタケミナカタ」",               Stage.Five,  Level.Hard),
            new CardInfo( 73, "準備「サモンタケミナカタ」",               Stage.Five,  Level.Lunatic),
            new CardInfo( 74, "奇跡「神の風」",                           Stage.Five,  Level.Easy),
            new CardInfo( 75, "奇跡「神の風」",                           Stage.Five,  Level.Normal),
            new CardInfo( 76, "大奇跡「八坂の神風」",                     Stage.Five,  Level.Hard),
            new CardInfo( 77, "大奇跡「八坂の神風」",                     Stage.Five,  Level.Lunatic),
            new CardInfo( 78, "神祭「エクスパンデッド・オンバシラ」",     Stage.Six,   Level.Easy),
            new CardInfo( 79, "神祭「エクスパンデッド・オンバシラ」",     Stage.Six,   Level.Normal),
            new CardInfo( 80, "奇祭「目処梃子乱舞」",                     Stage.Six,   Level.Hard),
            new CardInfo( 81, "奇祭「目処梃子乱舞」",                     Stage.Six,   Level.Lunatic),
            new CardInfo( 82, "筒粥「神の粥」",                           Stage.Six,   Level.Easy),
            new CardInfo( 83, "筒粥「神の粥」",                           Stage.Six,   Level.Normal),
            new CardInfo( 84, "忘穀「アンリメンバードクロップ」",         Stage.Six,   Level.Hard),
            new CardInfo( 85, "神穀「ディバイニングクロップ」",           Stage.Six,   Level.Lunatic),
            new CardInfo( 86, "贄符「御射山御狩神事」",                   Stage.Six,   Level.Easy),
            new CardInfo( 87, "贄符「御射山御狩神事」",                   Stage.Six,   Level.Normal),
            new CardInfo( 88, "神秘「葛井の清水」",                       Stage.Six,   Level.Hard),
            new CardInfo( 89, "神秘「ヤマトトーラス」",                   Stage.Six,   Level.Lunatic),
            new CardInfo( 90, "天流「お天水の奇跡」",                     Stage.Six,   Level.Easy),
            new CardInfo( 91, "天流「お天水の奇跡」",                     Stage.Six,   Level.Normal),
            new CardInfo( 92, "天竜「雨の源泉」",                         Stage.Six,   Level.Hard),
            new CardInfo( 93, "天竜「雨の源泉」",                         Stage.Six,   Level.Lunatic),
            new CardInfo( 94, "「マウンテン・オブ・フェイス」",           Stage.Six,   Level.Easy),
            new CardInfo( 95, "「マウンテン・オブ・フェイス」",           Stage.Six,   Level.Normal),
            new CardInfo( 96, "「風神様の神徳」",                         Stage.Six,   Level.Hard),
            new CardInfo( 97, "「風神様の神徳」",                         Stage.Six,   Level.Lunatic),
            new CardInfo( 98, "神符「水眼の如き美しき源泉」",             Stage.Extra, Level.Extra),
            new CardInfo( 99, "神符「杉で結ぶ古き縁」",                   Stage.Extra, Level.Extra),
            new CardInfo(100, "神符「神が歩かれた御神渡り」",             Stage.Extra, Level.Extra),
            new CardInfo(101, "開宴「二拝二拍一拝」",                     Stage.Extra, Level.Extra),
            new CardInfo(102, "土着神「手長足長さま」",                   Stage.Extra, Level.Extra),
            new CardInfo(103, "神具「洩矢の鉄の輪」",                     Stage.Extra, Level.Extra),
            new CardInfo(104, "源符「厭い川の翡翠」",                     Stage.Extra, Level.Extra),
            new CardInfo(105, "蛙狩「蛙は口ゆえ蛇に呑まるる」",           Stage.Extra, Level.Extra),
            new CardInfo(106, "土着神「七つの石と七つの木」",             Stage.Extra, Level.Extra),
            new CardInfo(107, "土着神「ケロちゃん風雨に負けず」",         Stage.Extra, Level.Extra),
            new CardInfo(108, "土着神「宝永四年の赤蛙」",                 Stage.Extra, Level.Extra),
            new CardInfo(109, "「諏訪大戦　～ 土着神話 vs 中央神話」",    Stage.Extra, Level.Extra),
            new CardInfo(110, "祟符「ミシャグジさま」",                   Stage.Extra, Level.Extra),
#pragma warning restore SA1008 // Opening parenthesis should be spaced correctly
        }.ToDictionary(card => card.Id);

        public static string FormatPrefix { get; } = "%T10";
    }
}

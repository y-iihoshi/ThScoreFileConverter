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
    ThScoreFileConverter.Models.Stage, ThScoreFileConverter.Models.Level>;

namespace ThScoreFileConverter.Models.Th16
{
    internal static class Definitions
    {
        // Thanks to thwiki.info
        public static IReadOnlyDictionary<int, CardInfo> CardTable { get; } = new List<CardInfo>()
        {
#pragma warning disable SA1008 // Opening parenthesis should be spaced correctly
            new CardInfo(  1, "蝶符「ミニットスケールス」",           Stage.One,   Level.Easy),
            new CardInfo(  2, "蝶符「ミニットスケールス」",           Stage.One,   Level.Normal),
            new CardInfo(  3, "蝶符「アゲハの鱗粉」",                 Stage.One,   Level.Hard),
            new CardInfo(  4, "蝶符「アゲハの鱗粉」",                 Stage.One,   Level.Lunatic),
            new CardInfo(  5, "蝶符「フラッタリングサマー」",         Stage.One,   Level.Easy),
            new CardInfo(  6, "蝶符「フラッタリングサマー」",         Stage.One,   Level.Normal),
            new CardInfo(  7, "蝶符「真夏の羽ばたき」",               Stage.One,   Level.Hard),
            new CardInfo(  8, "蝶符「真夏の羽ばたき」",               Stage.One,   Level.Lunatic),
            new CardInfo(  9, "雨符「囚われの秋雨」",                 Stage.Two,   Level.Easy),
            new CardInfo( 10, "雨符「囚われの秋雨」",                 Stage.Two,   Level.Normal),
            new CardInfo( 11, "雨符「呪われた柴榑雨」",               Stage.Two,   Level.Hard),
            new CardInfo( 12, "雨符「呪われた柴榑雨」",               Stage.Two,   Level.Lunatic),
            new CardInfo( 13, "刃符「山姥の包丁研ぎ」",               Stage.Two,   Level.Easy),
            new CardInfo( 14, "刃符「山姥の包丁研ぎ」",               Stage.Two,   Level.Normal),
            new CardInfo( 15, "刃符「山姥の鬼包丁研ぎ」",             Stage.Two,   Level.Hard),
            new CardInfo( 16, "刃符「山姥の鬼包丁研ぎ」",             Stage.Two,   Level.Lunatic),
            new CardInfo( 17, "尽符「マウンテンマーダー」",           Stage.Two,   Level.Easy),
            new CardInfo( 18, "尽符「マウンテンマーダー」",           Stage.Two,   Level.Normal),
            new CardInfo( 19, "尽符「ブラッディマウンテンマーダー」", Stage.Two,   Level.Hard),
            new CardInfo( 20, "尽符「ブラッディマウンテンマーダー」", Stage.Two,   Level.Lunatic),
            new CardInfo( 21, "春符「サプライズスプリング」",         Stage.Three, Level.Hard),
            new CardInfo( 22, "春符「サプライズスプリング」",         Stage.Three, Level.Lunatic),
            new CardInfo( 23, "犬符「野良犬の散歩」",                 Stage.Three, Level.Easy),
            new CardInfo( 24, "犬符「野良犬の散歩」",                 Stage.Three, Level.Normal),
            new CardInfo( 25, "狗符「山狗の散歩」",                   Stage.Three, Level.Hard),
            new CardInfo( 26, "狗符「山狗の散歩」",                   Stage.Three, Level.Lunatic),
            new CardInfo( 27, "独楽「コマ犬回し」",                   Stage.Three, Level.Easy),
            new CardInfo( 28, "独楽「コマ犬回し」",                   Stage.Three, Level.Normal),
            new CardInfo( 29, "独楽「コマ犬回し」",                   Stage.Three, Level.Hard),
            new CardInfo( 30, "独楽「カールアップアンドダイ」",       Stage.Three, Level.Lunatic),
            new CardInfo( 31, "狗符「独り阿吽の呼吸」",               Stage.Three, Level.Easy),
            new CardInfo( 32, "狗符「独り阿吽の呼吸」",               Stage.Three, Level.Normal),
            new CardInfo( 33, "狗符「独り阿吽の呼吸」",               Stage.Three, Level.Hard),
            new CardInfo( 34, "狗符「独り阿吽の呼吸」",               Stage.Three, Level.Lunatic),
            new CardInfo( 35, "魔符「インスタントボーディ」",         Stage.Four,  Level.Easy),
            new CardInfo( 36, "魔符「インスタントボーディ」",         Stage.Four,  Level.Normal),
            new CardInfo( 37, "魔符「即席菩提」",                     Stage.Four,  Level.Hard),
            new CardInfo( 38, "魔符「即席菩提」",                     Stage.Four,  Level.Lunatic),
            new CardInfo( 39, "魔符「バレットゴーレム」",             Stage.Four,  Level.Easy),
            new CardInfo( 40, "魔符「バレットゴーレム」",             Stage.Four,  Level.Normal),
            new CardInfo( 41, "魔符「ペットの巨大弾生命体」",         Stage.Four,  Level.Hard),
            new CardInfo( 42, "魔符「ペットの巨大弾生命体」",         Stage.Four,  Level.Lunatic),
            new CardInfo( 43, "地蔵「クリミナルサルヴェイション」",   Stage.Four,  Level.Easy),
            new CardInfo( 44, "地蔵「クリミナルサルヴェイション」",   Stage.Four,  Level.Normal),
            new CardInfo( 45, "地蔵「業火救済」",                     Stage.Four,  Level.Hard),
            new CardInfo( 46, "地蔵「業火救済」",                     Stage.Four,  Level.Lunatic),
            new CardInfo( 47, "竹符「バンブースピアダンス」",         Stage.Five,  Level.Easy),
            new CardInfo( 48, "竹符「バンブースピアダンス」",         Stage.Five,  Level.Normal),
            new CardInfo( 49, "竹符「バンブークレイジーダンス」",     Stage.Five,  Level.Hard),
            new CardInfo( 50, "竹符「バンブークレイジーダンス」",     Stage.Five,  Level.Lunatic),
            new CardInfo( 51, "茗荷「フォゲットユアネーム」",         Stage.Five,  Level.Easy),
            new CardInfo( 52, "茗荷「フォゲットユアネーム」",         Stage.Five,  Level.Normal),
            new CardInfo( 53, "茗荷「フォゲットユアネーム」",         Stage.Five,  Level.Hard),
            new CardInfo( 54, "茗荷「フォゲットユアネーム」",         Stage.Five,  Level.Lunatic),
            new CardInfo( 55, "笹符「タナバタスターフェスティバル」", Stage.Five,  Level.Easy),
            new CardInfo( 56, "笹符「タナバタスターフェスティバル」", Stage.Five,  Level.Normal),
            new CardInfo( 57, "笹符「タナバタスターフェスティバル」", Stage.Five,  Level.Hard),
            new CardInfo( 58, "笹符「タナバタスターフェスティバル」", Stage.Five,  Level.Lunatic),
            new CardInfo( 59, "冥加「ビハインドユー」",               Stage.Five,  Level.Easy),
            new CardInfo( 60, "冥加「ビハインドユー」",               Stage.Five,  Level.Normal),
            new CardInfo( 61, "冥加「ビハインドユー」",               Stage.Five,  Level.Hard),
            new CardInfo( 62, "冥加「ビハインドユー」",               Stage.Five,  Level.Lunatic),
            new CardInfo( 63, "舞符「ビハインドフェスティバル」",     Stage.Five,  Level.Easy),
            new CardInfo( 64, "舞符「ビハインドフェスティバル」",     Stage.Five,  Level.Normal),
            new CardInfo( 65, "舞符「ビハインドフェスティバル」",     Stage.Five,  Level.Hard),
            new CardInfo( 66, "舞符「ビハインドフェスティバル」",     Stage.Five,  Level.Lunatic),
            new CardInfo( 67, "狂舞「テングオドシ」",                 Stage.Five,  Level.Easy),
            new CardInfo( 68, "狂舞「テングオドシ」",                 Stage.Five,  Level.Normal),
            new CardInfo( 69, "狂舞「狂乱天狗怖し」",                 Stage.Five,  Level.Hard),
            new CardInfo( 70, "狂舞「狂乱天狗怖し」",                 Stage.Five,  Level.Lunatic),
            new CardInfo( 71, "後符「秘神の後光」",                   Stage.Six,   Level.Easy),
            new CardInfo( 72, "後符「秘神の後光」",                   Stage.Six,   Level.Normal),
            new CardInfo( 73, "後符「秘神の後光」",                   Stage.Six,   Level.Hard),
            new CardInfo( 74, "後符「絶対秘神の後光」",               Stage.Six,   Level.Lunatic),
            new CardInfo( 75, "裏夏「スコーチ・バイ・ホットサマー」", Stage.Six,   Level.Easy),
            new CardInfo( 76, "裏夏「スコーチ・バイ・ホットサマー」", Stage.Six,   Level.Normal),
            new CardInfo( 77, "裏夏「異常猛暑の焦土」",               Stage.Six,   Level.Hard),
            new CardInfo( 78, "裏夏「異常猛暑の焦土」",               Stage.Six,   Level.Lunatic),
            new CardInfo( 79, "裏秋「ダイ・オブ・ファミン」",         Stage.Six,   Level.Easy),
            new CardInfo( 80, "裏秋「ダイ・オブ・ファミン」",         Stage.Six,   Level.Normal),
            new CardInfo( 81, "裏秋「異常枯死の餓鬼」",               Stage.Six,   Level.Hard),
            new CardInfo( 82, "裏秋「異常枯死の餓鬼」",               Stage.Six,   Level.Lunatic),
            new CardInfo( 83, "裏冬「ブラックスノーマン」",           Stage.Six,   Level.Easy),
            new CardInfo( 84, "裏冬「ブラックスノーマン」",           Stage.Six,   Level.Normal),
            new CardInfo( 85, "裏冬「異常降雪の雪だるま」",           Stage.Six,   Level.Hard),
            new CardInfo( 86, "裏冬「異常降雪の雪だるま」",           Stage.Six,   Level.Lunatic),
            new CardInfo( 87, "裏春「エイプリルウィザード」",         Stage.Six,   Level.Easy),
            new CardInfo( 88, "裏春「エイプリルウィザード」",         Stage.Six,   Level.Normal),
            new CardInfo( 89, "裏春「異常落花の魔術使い」",           Stage.Six,   Level.Hard),
            new CardInfo( 90, "裏春「異常落花の魔術使い」",           Stage.Six,   Level.Lunatic),
            new CardInfo( 91, "「裏・ブリージーチェリーブロッサム」", Stage.Six,   Level.Easy),
            new CardInfo( 92, "「裏・ブリージーチェリーブロッサム」", Stage.Six,   Level.Normal),
            new CardInfo( 93, "「裏・ブリージーチェリーブロッサム」", Stage.Six,   Level.Hard),
            new CardInfo( 94, "「裏・ブリージーチェリーブロッサム」", Stage.Six,   Level.Lunatic),
            new CardInfo( 95, "「裏・パーフェクトサマーアイス」",     Stage.Six,   Level.Easy),
            new CardInfo( 96, "「裏・パーフェクトサマーアイス」",     Stage.Six,   Level.Normal),
            new CardInfo( 97, "「裏・パーフェクトサマーアイス」",     Stage.Six,   Level.Hard),
            new CardInfo( 98, "「裏・パーフェクトサマーアイス」",     Stage.Six,   Level.Lunatic),
            new CardInfo( 99, "「裏・クレイジーフォールウィンド」",   Stage.Six,   Level.Easy),
            new CardInfo(100, "「裏・クレイジーフォールウィンド」",   Stage.Six,   Level.Normal),
            new CardInfo(101, "「裏・クレイジーフォールウィンド」",   Stage.Six,   Level.Hard),
            new CardInfo(102, "「裏・クレイジーフォールウィンド」",   Stage.Six,   Level.Lunatic),
            new CardInfo(103, "「裏・エクストリームウィンター」",     Stage.Six,   Level.Easy),
            new CardInfo(104, "「裏・エクストリームウィンター」",     Stage.Six,   Level.Normal),
            new CardInfo(105, "「裏・エクストリームウィンター」",     Stage.Six,   Level.Hard),
            new CardInfo(106, "「裏・エクストリームウィンター」",     Stage.Six,   Level.Lunatic),
            new CardInfo(107, "鼓舞「パワフルチアーズ」",             Stage.Extra, Level.Extra),
            new CardInfo(108, "狂舞「クレイジーバックダンス」",       Stage.Extra, Level.Extra),
            new CardInfo(109, "弾舞「二つ目の台風」",                 Stage.Extra, Level.Extra),
            new CardInfo(110, "秘儀「リバースインヴォーカー」",       Stage.Extra, Level.Extra),
            new CardInfo(111, "秘儀「裏切りの後方射撃」",             Stage.Extra, Level.Extra),
            new CardInfo(112, "秘儀「弾幕の玉繭」",                   Stage.Extra, Level.Extra),
            new CardInfo(113, "秘儀「穢那の火」",                     Stage.Extra, Level.Extra),
            new CardInfo(114, "秘儀「後戸の狂言」",                   Stage.Extra, Level.Extra),
            new CardInfo(115, "秘儀「マターラドゥッカ」",             Stage.Extra, Level.Extra),
            new CardInfo(116, "秘儀「七星の剣」",                     Stage.Extra, Level.Extra),
            new CardInfo(117, "秘儀「無縁の芸能者」",                 Stage.Extra, Level.Extra),
            new CardInfo(118, "「背面の暗黒猿楽」",                   Stage.Extra, Level.Extra),
            new CardInfo(119, "「アナーキーバレットヘル」",           Stage.Extra, Level.Extra),
#pragma warning restore SA1008 // Opening parenthesis should be spaced correctly
        }.ToDictionary(card => card.Id);

        public static string FormatPrefix { get; } = "%T16";
    }
}

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
using ThScoreFileConverter.Core.Models.Th16;
using CardInfo = ThScoreFileConverter.Core.Models.SpellCardInfo<
    ThScoreFileConverter.Core.Models.Stage, ThScoreFileConverter.Core.Models.Level>;

namespace ThScoreFileConverter.Models.Th16;

internal static class Definitions
{
    // Thanks to thwiki.info
    public static IReadOnlyDictionary<int, CardInfo> CardTable { get; } = new CardInfo[]
    {
#pragma warning disable SA1008 // Opening parenthesis should be spaced correctly
        new(  1, "蝶符「ミニットスケールス」",           Stage.One,   Level.Easy),
        new(  2, "蝶符「ミニットスケールス」",           Stage.One,   Level.Normal),
        new(  3, "蝶符「アゲハの鱗粉」",                 Stage.One,   Level.Hard),
        new(  4, "蝶符「アゲハの鱗粉」",                 Stage.One,   Level.Lunatic),
        new(  5, "蝶符「フラッタリングサマー」",         Stage.One,   Level.Easy),
        new(  6, "蝶符「フラッタリングサマー」",         Stage.One,   Level.Normal),
        new(  7, "蝶符「真夏の羽ばたき」",               Stage.One,   Level.Hard),
        new(  8, "蝶符「真夏の羽ばたき」",               Stage.One,   Level.Lunatic),
        new(  9, "雨符「囚われの秋雨」",                 Stage.Two,   Level.Easy),
        new( 10, "雨符「囚われの秋雨」",                 Stage.Two,   Level.Normal),
        new( 11, "雨符「呪われた柴榑雨」",               Stage.Two,   Level.Hard),
        new( 12, "雨符「呪われた柴榑雨」",               Stage.Two,   Level.Lunatic),
        new( 13, "刃符「山姥の包丁研ぎ」",               Stage.Two,   Level.Easy),
        new( 14, "刃符「山姥の包丁研ぎ」",               Stage.Two,   Level.Normal),
        new( 15, "刃符「山姥の鬼包丁研ぎ」",             Stage.Two,   Level.Hard),
        new( 16, "刃符「山姥の鬼包丁研ぎ」",             Stage.Two,   Level.Lunatic),
        new( 17, "尽符「マウンテンマーダー」",           Stage.Two,   Level.Easy),
        new( 18, "尽符「マウンテンマーダー」",           Stage.Two,   Level.Normal),
        new( 19, "尽符「ブラッディマウンテンマーダー」", Stage.Two,   Level.Hard),
        new( 20, "尽符「ブラッディマウンテンマーダー」", Stage.Two,   Level.Lunatic),
        new( 21, "春符「サプライズスプリング」",         Stage.Three, Level.Hard),
        new( 22, "春符「サプライズスプリング」",         Stage.Three, Level.Lunatic),
        new( 23, "犬符「野良犬の散歩」",                 Stage.Three, Level.Easy),
        new( 24, "犬符「野良犬の散歩」",                 Stage.Three, Level.Normal),
        new( 25, "狗符「山狗の散歩」",                   Stage.Three, Level.Hard),
        new( 26, "狗符「山狗の散歩」",                   Stage.Three, Level.Lunatic),
        new( 27, "独楽「コマ犬回し」",                   Stage.Three, Level.Easy),
        new( 28, "独楽「コマ犬回し」",                   Stage.Three, Level.Normal),
        new( 29, "独楽「コマ犬回し」",                   Stage.Three, Level.Hard),
        new( 30, "独楽「カールアップアンドダイ」",       Stage.Three, Level.Lunatic),
        new( 31, "狗符「独り阿吽の呼吸」",               Stage.Three, Level.Easy),
        new( 32, "狗符「独り阿吽の呼吸」",               Stage.Three, Level.Normal),
        new( 33, "狗符「独り阿吽の呼吸」",               Stage.Three, Level.Hard),
        new( 34, "狗符「独り阿吽の呼吸」",               Stage.Three, Level.Lunatic),
        new( 35, "魔符「インスタントボーディ」",         Stage.Four,  Level.Easy),
        new( 36, "魔符「インスタントボーディ」",         Stage.Four,  Level.Normal),
        new( 37, "魔符「即席菩提」",                     Stage.Four,  Level.Hard),
        new( 38, "魔符「即席菩提」",                     Stage.Four,  Level.Lunatic),
        new( 39, "魔符「バレットゴーレム」",             Stage.Four,  Level.Easy),
        new( 40, "魔符「バレットゴーレム」",             Stage.Four,  Level.Normal),
        new( 41, "魔符「ペットの巨大弾生命体」",         Stage.Four,  Level.Hard),
        new( 42, "魔符「ペットの巨大弾生命体」",         Stage.Four,  Level.Lunatic),
        new( 43, "地蔵「クリミナルサルヴェイション」",   Stage.Four,  Level.Easy),
        new( 44, "地蔵「クリミナルサルヴェイション」",   Stage.Four,  Level.Normal),
        new( 45, "地蔵「業火救済」",                     Stage.Four,  Level.Hard),
        new( 46, "地蔵「業火救済」",                     Stage.Four,  Level.Lunatic),
        new( 47, "竹符「バンブースピアダンス」",         Stage.Five,  Level.Easy),
        new( 48, "竹符「バンブースピアダンス」",         Stage.Five,  Level.Normal),
        new( 49, "竹符「バンブークレイジーダンス」",     Stage.Five,  Level.Hard),
        new( 50, "竹符「バンブークレイジーダンス」",     Stage.Five,  Level.Lunatic),
        new( 51, "茗荷「フォゲットユアネーム」",         Stage.Five,  Level.Easy),
        new( 52, "茗荷「フォゲットユアネーム」",         Stage.Five,  Level.Normal),
        new( 53, "茗荷「フォゲットユアネーム」",         Stage.Five,  Level.Hard),
        new( 54, "茗荷「フォゲットユアネーム」",         Stage.Five,  Level.Lunatic),
        new( 55, "笹符「タナバタスターフェスティバル」", Stage.Five,  Level.Easy),
        new( 56, "笹符「タナバタスターフェスティバル」", Stage.Five,  Level.Normal),
        new( 57, "笹符「タナバタスターフェスティバル」", Stage.Five,  Level.Hard),
        new( 58, "笹符「タナバタスターフェスティバル」", Stage.Five,  Level.Lunatic),
        new( 59, "冥加「ビハインドユー」",               Stage.Five,  Level.Easy),
        new( 60, "冥加「ビハインドユー」",               Stage.Five,  Level.Normal),
        new( 61, "冥加「ビハインドユー」",               Stage.Five,  Level.Hard),
        new( 62, "冥加「ビハインドユー」",               Stage.Five,  Level.Lunatic),
        new( 63, "舞符「ビハインドフェスティバル」",     Stage.Five,  Level.Easy),
        new( 64, "舞符「ビハインドフェスティバル」",     Stage.Five,  Level.Normal),
        new( 65, "舞符「ビハインドフェスティバル」",     Stage.Five,  Level.Hard),
        new( 66, "舞符「ビハインドフェスティバル」",     Stage.Five,  Level.Lunatic),
        new( 67, "狂舞「テングオドシ」",                 Stage.Five,  Level.Easy),
        new( 68, "狂舞「テングオドシ」",                 Stage.Five,  Level.Normal),
        new( 69, "狂舞「狂乱天狗怖し」",                 Stage.Five,  Level.Hard),
        new( 70, "狂舞「狂乱天狗怖し」",                 Stage.Five,  Level.Lunatic),
        new( 71, "後符「秘神の後光」",                   Stage.Six,   Level.Easy),
        new( 72, "後符「秘神の後光」",                   Stage.Six,   Level.Normal),
        new( 73, "後符「秘神の後光」",                   Stage.Six,   Level.Hard),
        new( 74, "後符「絶対秘神の後光」",               Stage.Six,   Level.Lunatic),
        new( 75, "裏夏「スコーチ・バイ・ホットサマー」", Stage.Six,   Level.Easy),
        new( 76, "裏夏「スコーチ・バイ・ホットサマー」", Stage.Six,   Level.Normal),
        new( 77, "裏夏「異常猛暑の焦土」",               Stage.Six,   Level.Hard),
        new( 78, "裏夏「異常猛暑の焦土」",               Stage.Six,   Level.Lunatic),
        new( 79, "裏秋「ダイ・オブ・ファミン」",         Stage.Six,   Level.Easy),
        new( 80, "裏秋「ダイ・オブ・ファミン」",         Stage.Six,   Level.Normal),
        new( 81, "裏秋「異常枯死の餓鬼」",               Stage.Six,   Level.Hard),
        new( 82, "裏秋「異常枯死の餓鬼」",               Stage.Six,   Level.Lunatic),
        new( 83, "裏冬「ブラックスノーマン」",           Stage.Six,   Level.Easy),
        new( 84, "裏冬「ブラックスノーマン」",           Stage.Six,   Level.Normal),
        new( 85, "裏冬「異常降雪の雪だるま」",           Stage.Six,   Level.Hard),
        new( 86, "裏冬「異常降雪の雪だるま」",           Stage.Six,   Level.Lunatic),
        new( 87, "裏春「エイプリルウィザード」",         Stage.Six,   Level.Easy),
        new( 88, "裏春「エイプリルウィザード」",         Stage.Six,   Level.Normal),
        new( 89, "裏春「異常落花の魔術使い」",           Stage.Six,   Level.Hard),
        new( 90, "裏春「異常落花の魔術使い」",           Stage.Six,   Level.Lunatic),
        new( 91, "「裏・ブリージーチェリーブロッサム」", Stage.Six,   Level.Easy),
        new( 92, "「裏・ブリージーチェリーブロッサム」", Stage.Six,   Level.Normal),
        new( 93, "「裏・ブリージーチェリーブロッサム」", Stage.Six,   Level.Hard),
        new( 94, "「裏・ブリージーチェリーブロッサム」", Stage.Six,   Level.Lunatic),
        new( 95, "「裏・パーフェクトサマーアイス」",     Stage.Six,   Level.Easy),
        new( 96, "「裏・パーフェクトサマーアイス」",     Stage.Six,   Level.Normal),
        new( 97, "「裏・パーフェクトサマーアイス」",     Stage.Six,   Level.Hard),
        new( 98, "「裏・パーフェクトサマーアイス」",     Stage.Six,   Level.Lunatic),
        new( 99, "「裏・クレイジーフォールウィンド」",   Stage.Six,   Level.Easy),
        new(100, "「裏・クレイジーフォールウィンド」",   Stage.Six,   Level.Normal),
        new(101, "「裏・クレイジーフォールウィンド」",   Stage.Six,   Level.Hard),
        new(102, "「裏・クレイジーフォールウィンド」",   Stage.Six,   Level.Lunatic),
        new(103, "「裏・エクストリームウィンター」",     Stage.Six,   Level.Easy),
        new(104, "「裏・エクストリームウィンター」",     Stage.Six,   Level.Normal),
        new(105, "「裏・エクストリームウィンター」",     Stage.Six,   Level.Hard),
        new(106, "「裏・エクストリームウィンター」",     Stage.Six,   Level.Lunatic),
        new(107, "鼓舞「パワフルチアーズ」",             Stage.Extra, Level.Extra),
        new(108, "狂舞「クレイジーバックダンス」",       Stage.Extra, Level.Extra),
        new(109, "弾舞「二つ目の台風」",                 Stage.Extra, Level.Extra),
        new(110, "秘儀「リバースインヴォーカー」",       Stage.Extra, Level.Extra),
        new(111, "秘儀「裏切りの後方射撃」",             Stage.Extra, Level.Extra),
        new(112, "秘儀「弾幕の玉繭」",                   Stage.Extra, Level.Extra),
        new(113, "秘儀「穢那の火」",                     Stage.Extra, Level.Extra),
        new(114, "秘儀「後戸の狂言」",                   Stage.Extra, Level.Extra),
        new(115, "秘儀「マターラドゥッカ」",             Stage.Extra, Level.Extra),
        new(116, "秘儀「七星の剣」",                     Stage.Extra, Level.Extra),
        new(117, "秘儀「無縁の芸能者」",                 Stage.Extra, Level.Extra),
        new(118, "「背面の暗黒猿楽」",                   Stage.Extra, Level.Extra),
        new(119, "「アナーキーバレットヘル」",           Stage.Extra, Level.Extra),
#pragma warning restore SA1008 // Opening parenthesis should be spaced correctly
    }.ToDictionary(card => card.Id);

    public static string FormatPrefix { get; } = "%T16";

    public static bool IsTotal(CharaWithTotal chara)
    {
        return chara is CharaWithTotal.Total;
    }
}

﻿//-----------------------------------------------------------------------
// <copyright file="Definitions.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using CardInfo = ThScoreFileConverter.Core.Models.SpellCardInfo<
    ThScoreFileConverter.Core.Models.Stage, ThScoreFileConverter.Core.Models.Level>;

namespace ThScoreFileConverter.Core.Models.Th17;

/// <summary>
/// Provides several WBWC specific definitions.
/// </summary>
public static class Definitions
{
    /// <summary>
    /// Gets the dictionary of WBWC spell cards.
    /// Thanks to thwiki.info and en.touhouwiki.net.
    /// </summary>
    public static IReadOnlyDictionary<int, CardInfo> CardTable { get; } = new CardInfo[]
    {
#pragma warning disable SA1008 // Opening parenthesis should be spaced correctly
        new(  1, "石符「ストーンウッズ」",               Stage.One,   Level.Easy),
        new(  2, "石符「ストーンウッズ」",               Stage.One,   Level.Normal),
        new(  3, "石符「ストーンコニファー」",           Stage.One,   Level.Hard),
        new(  4, "石符「ストーンコニファー」",           Stage.One,   Level.Lunatic),
        new(  5, "石符「チルドレンズリンボ」",           Stage.One,   Level.Easy),
        new(  6, "石符「チルドレンズリンボ」",           Stage.One,   Level.Normal),
        new(  7, "石符「アダルトチルドレンズリンボ」",   Stage.One,   Level.Hard),
        new(  8, "石符「アダルトチルドレンズリンボ」",   Stage.One,   Level.Lunatic),
        new(  9, "石符「ストーンベイビー」",             Stage.Two,   Level.Easy),
        new( 10, "石符「ストーンベイビー」",             Stage.Two,   Level.Normal),
        new( 11, "石符「ヘビーストーンベイビー」",       Stage.Two,   Level.Hard),
        new( 12, "石符「ヘビーストーンベイビー」",       Stage.Two,   Level.Lunatic),
        new( 13, "溺符「三途の淪溺」",                   Stage.Two,   Level.Easy),
        new( 14, "溺符「三途の淪溺」",                   Stage.Two,   Level.Normal),
        new( 15, "溺符「三途の淪溺」",                   Stage.Two,   Level.Hard),
        new( 16, "溺符「三途の淪溺」",                   Stage.Two,   Level.Lunatic),
        new( 17, "鬼符「デーモンシージ」",               Stage.Two,   Level.Easy),
        new( 18, "鬼符「デーモンシージ」",               Stage.Two,   Level.Normal),
        new( 19, "鬼符「ハングリーデーモンシージ」",     Stage.Two,   Level.Hard),
        new( 20, "鬼符「ハングリーデーモンシージ」",     Stage.Two,   Level.Lunatic),
        new( 21, "水符「水配りの試練」",                 Stage.Three, Level.Easy),
        new( 22, "水符「水配りの試練」",                 Stage.Three, Level.Normal),
        new( 23, "水符「水配りの上級試煉」",             Stage.Three, Level.Hard),
        new( 24, "水符「水配りの極級試煉」",             Stage.Three, Level.Lunatic),
        new( 25, "光符「見渡しの試練」",                 Stage.Three, Level.Easy),
        new( 26, "光符「見渡しの試練」",                 Stage.Three, Level.Normal),
        new( 27, "光符「見渡しの上級試煉」",             Stage.Three, Level.Hard),
        new( 28, "光符「見渡しの極級試煉」",             Stage.Three, Level.Lunatic),
        new( 29, "鬼符「鬼渡の試練」",                   Stage.Three, Level.Easy),
        new( 30, "鬼符「鬼渡の試練」",                   Stage.Three, Level.Normal),
        new( 31, "鬼符「鬼渡の上級試煉」",               Stage.Three, Level.Hard),
        new( 32, "鬼符「鬼渡の獄級試煉」",               Stage.Three, Level.Lunatic),
        new( 33, "亀符「亀甲地獄」",                     Stage.Four,  Level.Easy),
        new( 34, "亀符「亀甲地獄」",                     Stage.Four,  Level.Normal),
        new( 35, "亀符「亀甲地獄」",                     Stage.Four,  Level.Hard),
        new( 36, "亀符「亀甲地獄」",                     Stage.Four,  Level.Lunatic),
        new( 37, "鬼符「搦手の畜生」",                   Stage.Four,  Level.Easy),
        new( 38, "鬼符「搦手の畜生」",                   Stage.Four,  Level.Normal),
        new( 39, "鬼符「搦手の犬畜生」",                 Stage.Four,  Level.Hard),
        new( 40, "鬼符「搦手の鬼畜生」",                 Stage.Four,  Level.Lunatic),
        new( 41, "龍符「龍紋弾」",                       Stage.Four,  Level.Easy),
        new( 42, "龍符「龍紋弾」",                       Stage.Four,  Level.Normal),
        new( 43, "龍符「龍紋弾」",                       Stage.Four,  Level.Hard),
        new( 44, "龍符「龍紋弾」",                       Stage.Four,  Level.Lunatic),
        new( 45, "埴輪「弓兵埴輪」",                     Stage.Five,  Level.Easy),
        new( 46, "埴輪「弓兵埴輪」",                     Stage.Five,  Level.Normal),
        new( 47, "埴輪「熟練弓兵埴輪」",                 Stage.Five,  Level.Hard),
        new( 48, "埴輪「熟練弓兵埴輪」",                 Stage.Five,  Level.Lunatic),
        new( 49, "埴輪「剣士埴輪」",                     Stage.Five,  Level.Easy),
        new( 50, "埴輪「剣士埴輪」",                     Stage.Five,  Level.Normal),
        new( 51, "埴輪「熟練剣士埴輪」",                 Stage.Five,  Level.Hard),
        new( 52, "埴輪「熟練剣士埴輪」",                 Stage.Five,  Level.Lunatic),
        new( 53, "埴輪「騎馬兵埴輪」",                   Stage.Five,  Level.Easy),
        new( 54, "埴輪「騎馬兵埴輪」",                   Stage.Five,  Level.Normal),
        new( 55, "埴輪「熟練騎馬兵埴輪」",               Stage.Five,  Level.Hard),
        new( 56, "埴輪「熟練騎馬兵埴輪」",               Stage.Five,  Level.Lunatic),
        new( 57, "埴輪「がらんどうの無尽兵団」",         Stage.Five,  Level.Easy),
        new( 58, "埴輪「がらんどうの無尽兵団」",         Stage.Five,  Level.Normal),
        new( 59, "埴輪「不敗の無尽兵団」",               Stage.Five,  Level.Hard),
        new( 60, "埴輪「不敗の無尽兵団」",               Stage.Five,  Level.Lunatic),
        new( 61, "方形「方形造形術」",                   Stage.Six,   Level.Easy),
        new( 62, "方形「方形造形術」",                   Stage.Six,   Level.Normal),
        new( 63, "方形「スクエアクリーチャー」",         Stage.Six,   Level.Hard),
        new( 64, "方形「スクエアクリーチャー」",         Stage.Six,   Level.Lunatic),
        new( 65, "円形「真円造形術」",                   Stage.Six,   Level.Easy),
        new( 66, "円形「真円造形術」",                   Stage.Six,   Level.Normal),
        new( 67, "円形「サークルクリーチャー」",         Stage.Six,   Level.Hard),
        new( 68, "円形「サークルクリーチャー」",         Stage.Six,   Level.Lunatic),
        new( 69, "線形「線形造形術」",                   Stage.Six,   Level.Easy),
        new( 70, "線形「線形造形術」",                   Stage.Six,   Level.Normal),
        new( 71, "線形「リニアクリーチャー」",           Stage.Six,   Level.Hard),
        new( 72, "線形「リニアクリーチャー」",           Stage.Six,   Level.Lunatic),
        new( 73, "埴輪「偶像人馬造形術」",               Stage.Six,   Level.Easy),
        new( 74, "埴輪「偶像人馬造形術」",               Stage.Six,   Level.Normal),
        new( 75, "埴輪「アイドルクリーチャー」",         Stage.Six,   Level.Hard),
        new( 76, "埴輪「アイドルクリーチャー」",         Stage.Six,   Level.Lunatic),
        new( 77, "「鬼形造形術」",                       Stage.Six,   Level.Easy),
        new( 78, "「鬼形造形術」",                       Stage.Six,   Level.Normal),
        new( 79, "「鬼形造形術」",                       Stage.Six,   Level.Hard),
        new( 80, "「鬼形造形術」",                       Stage.Six,   Level.Lunatic),
        new( 81, "「ジオメトリッククリーチャー」",       Stage.Six,   Level.Easy),
        new( 82, "「ジオメトリッククリーチャー」",       Stage.Six,   Level.Normal),
        new( 83, "「ジオメトリッククリーチャー」",       Stage.Six,   Level.Hard),
        new( 84, "「ジオメトリッククリーチャー」",       Stage.Six,   Level.Lunatic),
        new( 85, "「イドラディアボルス」",               Stage.Six,   Level.Easy),
        new( 86, "「イドラディアボルス」",               Stage.Six,   Level.Normal),
        new( 87, "「イドラディアボルス」",               Stage.Six,   Level.Hard),
        new( 88, "「イドラディアボルス」",               Stage.Six,   Level.Lunatic),
        new( 89, "血戦「血の分水嶺」",                   Stage.Extra, Level.Extra),
        new( 90, "血戦「獄界視線」",                     Stage.Extra, Level.Extra),
        new( 91, "血戦「全霊鬼渡り」",                   Stage.Extra, Level.Extra),
        new( 92, "勁疾技「スリリングショット」",         Stage.Extra, Level.Extra),
        new( 93, "勁疾技「ライトニングネイ」",           Stage.Extra, Level.Extra),
        new( 94, "勁疾技「デンスクラウド」",             Stage.Extra, Level.Extra),
        new( 95, "勁疾技「ビーストエピデミシティ」",     Stage.Extra, Level.Extra),
        new( 96, "勁疾技「トライアングルチェイス」",     Stage.Extra, Level.Extra),
        new( 97, "勁疾技「ブラックペガサス流星弾」",     Stage.Extra, Level.Extra),
        new( 98, "勁疾技「マッスルエクスプロージョン」", Stage.Extra, Level.Extra),
        new( 99, "「フォロミーアンアフライド」",         Stage.Extra, Level.Extra),
        new(100, "「鬼形のホイポロイ」",                 Stage.Extra, Level.Extra),
        new(101, "「鬼畜生の所業」",                     Stage.Extra, Level.Extra),
#pragma warning restore SA1008 // Opening parenthesis should be spaced correctly
    }.ToDictionary(static card => card.Id);

    /// <summary>
    /// Gets the list of achievements.
    /// Thanks to en.touhouwiki.net.
    /// </summary>
    public static IReadOnlyList<string> Achievements { get; } =
    [
        "霊夢（オオカミ霊）クリア",
        "霊夢（カワウソ霊）クリア",
        "霊夢（オオワシ霊）クリア",
        "魔理沙（オオカミ霊）クリア",
        "魔理沙（カワウソ霊）クリア",
        "魔理沙（オオワシ霊）クリア",
        "妖夢（オオカミ霊）クリア",
        "妖夢（カワウソ霊）クリア",
        "妖夢（オオワシ霊）クリア",
        "霊夢バッドエンド",
        "魔理沙BADクリア",
        "妖夢BADクリア",
        "霊夢（オオカミ霊）EXクリア",
        "霊夢（カワウソ霊）EXクリア",
        "霊夢（オオワシ霊）EXクリア",
        "魔理沙（オオカミ霊）EXクリア",
        "魔理沙（カワウソ霊）EXクリア",
        "魔理沙（オオワシ霊）EXクリア",
        "妖夢（オオカミ霊）EXクリア",
        "妖夢（カワウソ霊）EXクリア",
        "妖夢（オオワシ霊）EXクリア",
        "イージークリア",
        "ノーマルクリア",
        "ハードクリア",
        "ルナティッククリア",
        "イージーノーコンクリア",
        "ノーマルノーコンクリア",
        "ハードNCクリア",
        "ルナティックNCクリア",
        "パーフェクトクリア",
        "畜生界の破壊神",
        "畜生を蹂躙する者",
        "スペルカードコレクター",
        "クラゲ大好き",
        "そうだ牛乳を飲もう",
        "生まれたてのピヨちゃん",
        "のんびりコツコツと",
        "おーいハニマルさん",
        "土の馬",
        "ひよこが三匹",
    ];
}

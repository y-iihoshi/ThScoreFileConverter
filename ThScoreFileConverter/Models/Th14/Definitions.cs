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
using ThScoreFileConverter.Core.Models.Th14;
using CardInfo = ThScoreFileConverter.Models.SpellCardInfo<
    ThScoreFileConverter.Core.Models.Stage, ThScoreFileConverter.Core.Models.Level>;

namespace ThScoreFileConverter.Models.Th14;

internal static class Definitions
{
    // Thanks to thwiki.info
    public static IReadOnlyDictionary<int, CardInfo> CardTable { get; } = new CardInfo[]
    {
#pragma warning disable SA1008 // Opening parenthesis should be spaced correctly
        new(  1, "氷符「アルティメットブリザード」",   Stage.One,   Level.Hard),
        new(  2, "氷符「アルティメットブリザード」",   Stage.One,   Level.Lunatic),
        new(  3, "水符「テイルフィンスラップ」",       Stage.One,   Level.Easy),
        new(  4, "水符「テイルフィンスラップ」",       Stage.One,   Level.Normal),
        new(  5, "水符「テイルフィンスラップ」",       Stage.One,   Level.Hard),
        new(  6, "水符「テイルフィンスラップ」",       Stage.One,   Level.Lunatic),
        new(  7, "鱗符「スケールウェイブ」",           Stage.One,   Level.Easy),
        new(  8, "鱗符「スケールウェイブ」",           Stage.One,   Level.Normal),
        new(  9, "鱗符「逆鱗の荒波」",                 Stage.One,   Level.Hard),
        new( 10, "鱗符「逆鱗の大荒波」",               Stage.One,   Level.Lunatic),
        new( 11, "飛符「フライングヘッド」",           Stage.Two,   Level.Easy),
        new( 12, "飛符「フライングヘッド」",           Stage.Two,   Level.Normal),
        new( 13, "飛符「フライングヘッド」",           Stage.Two,   Level.Hard),
        new( 14, "飛符「フライングヘッド」",           Stage.Two,   Level.Lunatic),
        new( 15, "首符「クローズアイショット」",       Stage.Two,   Level.Easy),
        new( 16, "首符「クローズアイショット」",       Stage.Two,   Level.Normal),
        new( 17, "首符「ろくろ首飛来」",               Stage.Two,   Level.Hard),
        new( 18, "首符「ろくろ首飛来」",               Stage.Two,   Level.Lunatic),
        new( 19, "飛頭「マルチプリケイティブヘッド」", Stage.Two,   Level.Easy),
        new( 20, "飛頭「マルチプリケイティブヘッド」", Stage.Two,   Level.Normal),
        new( 21, "飛頭「セブンズヘッド」",             Stage.Two,   Level.Hard),
        new( 22, "飛頭「ナインズヘッド」",             Stage.Two,   Level.Lunatic),
        new( 23, "飛頭「デュラハンナイト」",           Stage.Two,   Level.Easy),
        new( 24, "飛頭「デュラハンナイト」",           Stage.Two,   Level.Normal),
        new( 25, "飛頭「デュラハンナイト」",           Stage.Two,   Level.Hard),
        new( 26, "飛頭「デュラハンナイト」",           Stage.Two,   Level.Lunatic),
        new( 27, "牙符「月下の犬歯」",                 Stage.Three, Level.Hard),
        new( 28, "牙符「月下の犬歯」",                 Stage.Three, Level.Lunatic),
        new( 29, "変身「トライアングルファング」",     Stage.Three, Level.Easy),
        new( 30, "変身「トライアングルファング」",     Stage.Three, Level.Normal),
        new( 31, "変身「スターファング」",             Stage.Three, Level.Hard),
        new( 32, "変身「スターファング」",             Stage.Three, Level.Lunatic),
        new( 33, "咆哮「ストレンジロア」",             Stage.Three, Level.Easy),
        new( 34, "咆哮「ストレンジロア」",             Stage.Three, Level.Normal),
        new( 35, "咆哮「満月の遠吠え」",               Stage.Three, Level.Hard),
        new( 36, "咆哮「満月の遠吠え」",               Stage.Three, Level.Lunatic),
        new( 37, "狼符「スターリングパウンス」",       Stage.Three, Level.Easy),
        new( 38, "狼符「スターリングパウンス」",       Stage.Three, Level.Normal),
        new( 39, "天狼「ハイスピードパウンス」",       Stage.Three, Level.Hard),
        new( 40, "天狼「ハイスピードパウンス」",       Stage.Three, Level.Lunatic),
        new( 41, "平曲「祇園精舎の鐘の音」",           Stage.Four,  Level.Easy),
        new( 42, "平曲「祇園精舎の鐘の音」",           Stage.Four,  Level.Normal),
        new( 43, "平曲「祇園精舎の鐘の音」",           Stage.Four,  Level.Hard),
        new( 44, "平曲「祇園精舎の鐘の音」",           Stage.Four,  Level.Lunatic),
        new( 45, "怨霊「耳無し芳一」",                 Stage.Four,  Level.Easy),
        new( 46, "怨霊「耳無し芳一」",                 Stage.Four,  Level.Normal),
        new( 47, "怨霊「平家の大怨霊」",               Stage.Four,  Level.Hard),
        new( 48, "怨霊「平家の大怨霊」",               Stage.Four,  Level.Lunatic),
        new( 49, "楽符「邪悪な五線譜」",               Stage.Four,  Level.Easy),
        new( 50, "楽符「邪悪な五線譜」",               Stage.Four,  Level.Normal),
        new( 51, "楽符「凶悪な五線譜」",               Stage.Four,  Level.Hard),
        new( 52, "楽符「ダブルスコア」",               Stage.Four,  Level.Lunatic),
        new( 53, "琴符「諸行無常の琴の音」",           Stage.Four,  Level.Easy),
        new( 54, "琴符「諸行無常の琴の音」",           Stage.Four,  Level.Normal),
        new( 55, "琴符「諸行無常の琴の音」",           Stage.Four,  Level.Hard),
        new( 56, "琴符「諸行無常の琴の音」",           Stage.Four,  Level.Lunatic),
        new( 57, "響符「平安の残響」",                 Stage.Four,  Level.Easy),
        new( 58, "響符「平安の残響」",                 Stage.Four,  Level.Normal),
        new( 59, "響符「エコーチェンバー」",           Stage.Four,  Level.Hard),
        new( 60, "響符「エコーチェンバー」",           Stage.Four,  Level.Lunatic),
        new( 61, "箏曲「下克上送箏曲」",               Stage.Four,  Level.Easy),
        new( 62, "箏曲「下克上送箏曲」",               Stage.Four,  Level.Normal),
        new( 63, "筝曲「下克上レクイエム」",           Stage.Four,  Level.Hard),
        new( 64, "筝曲「下克上レクイエム」",           Stage.Four,  Level.Lunatic),
        new( 65, "欺符「逆針撃」",                     Stage.Five,  Level.Easy),
        new( 66, "欺符「逆針撃」",                     Stage.Five,  Level.Normal),
        new( 67, "欺符「逆針撃」",                     Stage.Five,  Level.Hard),
        new( 68, "欺符「逆針撃」",                     Stage.Five,  Level.Lunatic),
        new( 69, "逆符「鏡の国の弾幕」",               Stage.Five,  Level.Easy),
        new( 70, "逆符「鏡の国の弾幕」",               Stage.Five,  Level.Normal),
        new( 71, "逆符「イビルインザミラー」",         Stage.Five,  Level.Hard),
        new( 72, "逆符「イビルインザミラー」",         Stage.Five,  Level.Lunatic),
        new( 73, "逆符「天地有用」",                   Stage.Five,  Level.Easy),
        new( 74, "逆符「天地有用」",                   Stage.Five,  Level.Normal),
        new( 75, "逆符「天下転覆」",                   Stage.Five,  Level.Hard),
        new( 76, "逆符「天下転覆」",                   Stage.Five,  Level.Lunatic),
        new( 77, "逆弓「天壌夢弓」",                   Stage.Five,  Level.Easy),
        new( 78, "逆弓「天壌夢弓」",                   Stage.Five,  Level.Normal),
        new( 79, "逆弓「天壌夢弓の詔勅」",             Stage.Five,  Level.Hard),
        new( 80, "逆弓「天壌夢弓の詔勅」",             Stage.Five,  Level.Lunatic),
        new( 81, "逆転「リバースヒエラルキー」",       Stage.Five,  Level.Easy),
        new( 82, "逆転「リバースヒエラルキー」",       Stage.Five,  Level.Normal),
        new( 83, "逆転「チェンジエアブレイブ」",       Stage.Five,  Level.Hard),
        new( 84, "逆転「チェンジエアブレイブ」",       Stage.Five,  Level.Lunatic),
        new( 85, "小弾「小人の道」",                   Stage.Six,   Level.Easy),
        new( 86, "小弾「小人の道」",                   Stage.Six,   Level.Normal),
        new( 87, "小弾「小人の茨道」",                 Stage.Six,   Level.Hard),
        new( 88, "小弾「小人の茨道」",                 Stage.Six,   Level.Lunatic),
        new( 89, "小槌「大きくなあれ」",               Stage.Six,   Level.Easy),
        new( 90, "小槌「大きくなあれ」",               Stage.Six,   Level.Normal),
        new( 91, "小槌「もっと大きくなあれ」",         Stage.Six,   Level.Hard),
        new( 92, "小槌「もっと大きくなあれ」",         Stage.Six,   Level.Lunatic),
        new( 93, "妖剣「輝針剣」",                     Stage.Six,   Level.Easy),
        new( 94, "妖剣「輝針剣」",                     Stage.Six,   Level.Normal),
        new( 95, "妖剣「輝針剣」",                     Stage.Six,   Level.Hard),
        new( 96, "妖剣「輝針剣」",                     Stage.Six,   Level.Lunatic),
        new( 97, "小槌「お前が大きくなあれ」",         Stage.Six,   Level.Easy),
        new( 98, "小槌「お前が大きくなあれ」",         Stage.Six,   Level.Normal),
        new( 99, "小槌「お前が大きくなあれ」",         Stage.Six,   Level.Hard),
        new(100, "小槌「お前が大きくなあれ」",         Stage.Six,   Level.Lunatic),
        new(101, "「進撃の小人」",                     Stage.Six,   Level.Easy),
        new(102, "「進撃の小人」",                     Stage.Six,   Level.Normal),
        new(103, "「ウォールオブイッスン」",           Stage.Six,   Level.Hard),
        new(104, "「ウォールオブイッスン」",           Stage.Six,   Level.Lunatic),
        new(105, "「ホップオマイサムセブン」",         Stage.Six,   Level.Easy),
        new(106, "「ホップオマイサムセブン」",         Stage.Six,   Level.Normal),
        new(107, "「七人の一寸法師」",                 Stage.Six,   Level.Hard),
        new(108, "「七人の一寸法師」",                 Stage.Six,   Level.Lunatic),
        new(109, "弦楽「嵐のアンサンブル」",           Stage.Extra, Level.Extra),
        new(110, "弦楽「浄瑠璃世界」",                 Stage.Extra, Level.Extra),
        new(111, "一鼓「暴れ宮太鼓」",                 Stage.Extra, Level.Extra),
        new(112, "二鼓「怨霊アヤノツヅミ」",           Stage.Extra, Level.Extra),
        new(113, "三鼓「午前零時のスリーストライク」", Stage.Extra, Level.Extra),
        new(114, "死鼓「ランドパーカス」",             Stage.Extra, Level.Extra),
        new(115, "五鼓「デンデン太鼓」",               Stage.Extra, Level.Extra),
        new(116, "六鼓「オルタネイトスティッキング」", Stage.Extra, Level.Extra),
        new(117, "七鼓「高速和太鼓ロケット」",         Stage.Extra, Level.Extra),
        new(118, "八鼓「雷神の怒り」",                 Stage.Extra, Level.Extra),
        new(119, "「ブルーレディショー」",             Stage.Extra, Level.Extra),
        new(120, "「プリスティンビート」",             Stage.Extra, Level.Extra),
#pragma warning restore SA1008 // Opening parenthesis should be spaced correctly
    }.ToDictionary(card => card.Id);

    public static string FormatPrefix { get; } = "%T14";

    public static bool IsTotal(CharaWithTotal chara)
    {
        return chara is CharaWithTotal.Total;
    }

    public static bool IsToBeSummed(LevelPracticeWithTotal level)
    {
        return level is not (LevelPracticeWithTotal.Total or LevelPracticeWithTotal.NotUsed);
    }
}

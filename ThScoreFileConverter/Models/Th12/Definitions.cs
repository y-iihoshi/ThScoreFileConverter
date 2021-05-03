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

namespace ThScoreFileConverter.Models.Th12
{
    internal static class Definitions
    {
        // Thanks to thwiki.info
        public static IReadOnlyDictionary<int, CardInfo> CardTable { get; } = new List<CardInfo>()
        {
#pragma warning disable SA1008 // Opening parenthesis should be spaced correctly
            new CardInfo(  1, "棒符「ビジーロッド」",                 Stage.One,   Level.Hard),
            new CardInfo(  2, "棒符「ビジーロッド」",                 Stage.One,   Level.Lunatic),
            new CardInfo(  3, "捜符「レアメタルディテクター」",       Stage.One,   Level.Easy),
            new CardInfo(  4, "捜符「レアメタルディテクター」",       Stage.One,   Level.Normal),
            new CardInfo(  5, "捜符「ゴールドディテクター」",         Stage.One,   Level.Hard),
            new CardInfo(  6, "捜符「ゴールドディテクター」",         Stage.One,   Level.Lunatic),
            new CardInfo(  7, "視符「ナズーリンペンデュラム」",       Stage.One,   Level.Easy),
            new CardInfo(  8, "視符「ナズーリンペンデュラム」",       Stage.One,   Level.Normal),
            new CardInfo(  9, "視符「高感度ナズーリンペンデュラム」", Stage.One,   Level.Hard),
            new CardInfo( 10, "守符「ペンデュラムガード」",           Stage.One,   Level.Lunatic),
            new CardInfo( 11, "大輪「からかさ後光」",                 Stage.Two,   Level.Easy),
            new CardInfo( 12, "大輪「からかさ後光」",                 Stage.Two,   Level.Normal),
            new CardInfo( 13, "大輪「ハロウフォゴットンワールド」",   Stage.Two,   Level.Hard),
            new CardInfo( 14, "大輪「ハロウフォゴットンワールド」",   Stage.Two,   Level.Lunatic),
            new CardInfo( 15, "傘符「パラソルスターシンフォニー」",   Stage.Two,   Level.Easy),
            new CardInfo( 16, "傘符「パラソルスターシンフォニー」",   Stage.Two,   Level.Normal),
            new CardInfo( 17, "傘符「パラソルスターメモリーズ」",     Stage.Two,   Level.Hard),
            new CardInfo( 18, "傘符「パラソルスターメモリーズ」",     Stage.Two,   Level.Lunatic),
            new CardInfo( 19, "雨符「雨夜の怪談」",                   Stage.Two,   Level.Easy),
            new CardInfo( 20, "雨符「雨夜の怪談」",                   Stage.Two,   Level.Normal),
            new CardInfo( 21, "雨傘「超撥水かさかさお化け」",         Stage.Two,   Level.Hard),
            new CardInfo( 22, "雨傘「超撥水かさかさお化け」",         Stage.Two,   Level.Lunatic),
            new CardInfo( 23, "化符「忘れ傘の夜行列車」",             Stage.Two,   Level.Easy),
            new CardInfo( 24, "化符「忘れ傘の夜行列車」",             Stage.Two,   Level.Normal),
            new CardInfo( 25, "化鉄「置き傘特急ナイトカーニバル」",   Stage.Two,   Level.Hard),
            new CardInfo( 26, "化鉄「置き傘特急ナイトカーニバル」",   Stage.Two,   Level.Lunatic),
            new CardInfo( 27, "鉄拳「問答無用の妖怪拳」",             Stage.Three, Level.Easy),
            new CardInfo( 28, "鉄拳「問答無用の妖怪拳」",             Stage.Three, Level.Normal),
            new CardInfo( 29, "神拳「雲上地獄突き」",                 Stage.Three, Level.Hard),
            new CardInfo( 30, "神拳「天海地獄突き」",                 Stage.Three, Level.Lunatic),
            new CardInfo( 31, "拳符「天網サンドバッグ」",             Stage.Three, Level.Easy),
            new CardInfo( 32, "拳符「天網サンドバッグ」",             Stage.Three, Level.Normal),
            new CardInfo( 33, "連打「雲界クラーケン殴り」",           Stage.Three, Level.Hard),
            new CardInfo( 34, "連打「キングクラーケン殴り」",         Stage.Three, Level.Lunatic),
            new CardInfo( 35, "拳打「げんこつスマッシュ」",           Stage.Three, Level.Easy),
            new CardInfo( 36, "拳打「げんこつスマッシュ」",           Stage.Three, Level.Normal),
            new CardInfo( 37, "潰滅「天上天下連続フック」",           Stage.Three, Level.Hard),
            new CardInfo( 38, "潰滅「天上天下連続フック」",           Stage.Three, Level.Lunatic),
            new CardInfo( 39, "大喝「時代親父大目玉」",               Stage.Three, Level.Easy),
            new CardInfo( 40, "大喝「時代親父大目玉」",               Stage.Three, Level.Normal),
            new CardInfo( 41, "忿怒「天変大目玉焼き」",               Stage.Three, Level.Hard),
            new CardInfo( 42, "忿怒「空前絶後大目玉焼き」",           Stage.Three, Level.Lunatic),
            new CardInfo( 43, "転覆「道連れアンカー」",               Stage.Four,  Level.Easy),
            new CardInfo( 44, "転覆「道連れアンカー」",               Stage.Four,  Level.Normal),
            new CardInfo( 45, "転覆「沈没アンカー」",                 Stage.Four,  Level.Hard),
            new CardInfo( 46, "転覆「撃沈アンカー」",                 Stage.Four,  Level.Lunatic),
            new CardInfo( 47, "溺符「ディープヴォーテックス」",       Stage.Four,  Level.Easy),
            new CardInfo( 48, "溺符「ディープヴォーテックス」",       Stage.Four,  Level.Normal),
            new CardInfo( 49, "溺符「シンカブルヴォーテックス」",     Stage.Four,  Level.Hard),
            new CardInfo( 50, "溺符「シンカブルヴォーテックス」",     Stage.Four,  Level.Lunatic),
            new CardInfo( 51, "湊符「ファントムシップハーバー」",     Stage.Four,  Level.Easy),
            new CardInfo( 52, "湊符「ファントムシップハーバー」",     Stage.Four,  Level.Normal),
            new CardInfo( 53, "湊符「幽霊船の港」",                   Stage.Four,  Level.Hard),
            new CardInfo( 54, "湊符「幽霊船永久停泊」",               Stage.Four,  Level.Lunatic),
            new CardInfo( 55, "幽霊「シンカーゴースト」",             Stage.Four,  Level.Normal),
            new CardInfo( 56, "幽霊「忍び寄る柄杓」",                 Stage.Four,  Level.Hard),
            new CardInfo( 57, "幽霊「忍び寄る柄杓」",                 Stage.Four,  Level.Lunatic),
            new CardInfo( 58, "宝塔「グレイテストトレジャー」",       Stage.Five,  Level.Easy),
            new CardInfo( 59, "宝塔「グレイテストトレジャー」",       Stage.Five,  Level.Normal),
            new CardInfo( 60, "宝塔「グレイテストトレジャー」",       Stage.Five,  Level.Hard),
            new CardInfo( 61, "宝塔「グレイテストトレジャー」",       Stage.Five,  Level.Lunatic),
            new CardInfo( 62, "宝塔「レイディアントトレジャー」",     Stage.Five,  Level.Easy),
            new CardInfo( 63, "宝塔「レイディアントトレジャー」",     Stage.Five,  Level.Normal),
            new CardInfo( 64, "宝塔「レイディアントトレジャーガン」", Stage.Five,  Level.Hard),
            new CardInfo( 65, "宝塔「レイディアントトレジャーガン」", Stage.Five,  Level.Lunatic),
            new CardInfo( 66, "光符「アブソリュートジャスティス」",   Stage.Five,  Level.Easy),
            new CardInfo( 67, "光符「アブソリュートジャスティス」",   Stage.Five,  Level.Normal),
            new CardInfo( 68, "光符「正義の威光」",                   Stage.Five,  Level.Hard),
            new CardInfo( 69, "光符「正義の威光」",                   Stage.Five,  Level.Lunatic),
            new CardInfo( 70, "法力「至宝の独鈷杵」",                 Stage.Five,  Level.Easy),
            new CardInfo( 71, "法力「至宝の独鈷杵」",                 Stage.Five,  Level.Normal),
            new CardInfo( 72, "法灯「隙間無い法の独鈷杵」",           Stage.Five,  Level.Hard),
            new CardInfo( 73, "法灯「隙間無い法の独鈷杵」",           Stage.Five,  Level.Lunatic),
            new CardInfo( 74, "光符「浄化の魔」",                     Stage.Five,  Level.Easy),
            new CardInfo( 75, "光符「浄化の魔」",                     Stage.Five,  Level.Normal),
            new CardInfo( 76, "光符「浄化の魔」",                     Stage.Five,  Level.Hard),
            new CardInfo( 77, "「コンプリートクラリフィケイション」", Stage.Five,  Level.Lunatic),
            new CardInfo( 78, "魔法「紫雲のオーメン」",               Stage.Six,   Level.Easy),
            new CardInfo( 79, "魔法「紫雲のオーメン」",               Stage.Six,   Level.Normal),
            new CardInfo( 80, "吉兆「紫の雲路」",                     Stage.Six,   Level.Hard),
            new CardInfo( 81, "吉兆「極楽の紫の雲路」",               Stage.Six,   Level.Lunatic),
            new CardInfo( 82, "魔法「魔界蝶の妖香」",                 Stage.Six,   Level.Easy),
            new CardInfo( 83, "魔法「魔界蝶の妖香」",                 Stage.Six,   Level.Normal),
            new CardInfo( 84, "魔法「マジックバタフライ」",           Stage.Six,   Level.Hard),
            new CardInfo( 85, "魔法「マジックバタフライ」",           Stage.Six,   Level.Lunatic),
            new CardInfo( 86, "光魔「スターメイルシュトロム」",       Stage.Six,   Level.Easy),
            new CardInfo( 87, "光魔「スターメイルシュトロム」",       Stage.Six,   Level.Normal),
            new CardInfo( 88, "光魔「魔法銀河系」",                   Stage.Six,   Level.Hard),
            new CardInfo( 89, "光魔「魔法銀河系」",                   Stage.Six,   Level.Lunatic),
            new CardInfo( 90, "大魔法「魔神復誦」",                   Stage.Six,   Level.Easy),
            new CardInfo( 91, "大魔法「魔神復誦」",                   Stage.Six,   Level.Normal),
            new CardInfo( 92, "大魔法「魔神復誦」",                   Stage.Six,   Level.Hard),
            new CardInfo( 93, "大魔法「魔神復誦」",                   Stage.Six,   Level.Lunatic),
            new CardInfo( 94, "「聖尼公のエア巻物」",                 Stage.Six,   Level.Normal),
            new CardInfo( 95, "「聖尼公のエア巻物」",                 Stage.Six,   Level.Hard),
            new CardInfo( 96, "超人「聖白蓮」",                       Stage.Six,   Level.Lunatic),
            new CardInfo( 97, "飛鉢「フライングファンタスティカ」",   Stage.Six,   Level.Easy),
            new CardInfo( 98, "飛鉢「フライングファンタスティカ」",   Stage.Six,   Level.Normal),
            new CardInfo( 99, "飛鉢「伝説の飛空円盤」",               Stage.Six,   Level.Hard),
            new CardInfo(100, "飛鉢「伝説の飛空円盤」",               Stage.Six,   Level.Lunatic),
            new CardInfo(101, "傘符「大粒の涙雨」",                   Stage.Extra, Level.Extra),
            new CardInfo(102, "驚雨「ゲリラ台風」",                   Stage.Extra, Level.Extra),
            new CardInfo(103, "後光「からかさ驚きフラッシュ」",       Stage.Extra, Level.Extra),
            new CardInfo(104, "妖雲「平安のダーククラウド」",         Stage.Extra, Level.Extra),
            new CardInfo(105, "正体不明「忿怒のレッドＵＦＯ襲来」",   Stage.Extra, Level.Extra),
            new CardInfo(106, "鵺符「鵺的スネークショー」",           Stage.Extra, Level.Extra),
            new CardInfo(107, "正体不明「哀愁のブルーＵＦＯ襲来」",   Stage.Extra, Level.Extra),
            new CardInfo(108, "鵺符「弾幕キメラ」",                   Stage.Extra, Level.Extra),
            new CardInfo(109, "正体不明「義心のグリーンＵＦＯ襲来」", Stage.Extra, Level.Extra),
            new CardInfo(110, "鵺符「アンディファインドダークネス」", Stage.Extra, Level.Extra),
            new CardInfo(111, "正体不明「恐怖の虹色ＵＦＯ襲来」",     Stage.Extra, Level.Extra),
            new CardInfo(112, "「平安京の悪夢」",                     Stage.Extra, Level.Extra),
            new CardInfo(113, "恨弓「源三位頼政の弓」",               Stage.Extra, Level.Extra),
#pragma warning restore SA1008 // Opening parenthesis should be spaced correctly
        }.ToDictionary(card => card.Id);

        public static string FormatPrefix { get; } = "%T12";
    }
}

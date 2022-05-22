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
using ThScoreFileConverter.Core.Models.Th12;
using CardInfo = ThScoreFileConverter.Models.SpellCardInfo<
    ThScoreFileConverter.Core.Models.Stage, ThScoreFileConverter.Core.Models.Level>;

namespace ThScoreFileConverter.Models.Th12;

internal static class Definitions
{
    // Thanks to thwiki.info
    public static IReadOnlyDictionary<int, CardInfo> CardTable { get; } = new CardInfo[]
    {
#pragma warning disable SA1008 // Opening parenthesis should be spaced correctly
        new(  1, "棒符「ビジーロッド」",                 Stage.One,   Level.Hard),
        new(  2, "棒符「ビジーロッド」",                 Stage.One,   Level.Lunatic),
        new(  3, "捜符「レアメタルディテクター」",       Stage.One,   Level.Easy),
        new(  4, "捜符「レアメタルディテクター」",       Stage.One,   Level.Normal),
        new(  5, "捜符「ゴールドディテクター」",         Stage.One,   Level.Hard),
        new(  6, "捜符「ゴールドディテクター」",         Stage.One,   Level.Lunatic),
        new(  7, "視符「ナズーリンペンデュラム」",       Stage.One,   Level.Easy),
        new(  8, "視符「ナズーリンペンデュラム」",       Stage.One,   Level.Normal),
        new(  9, "視符「高感度ナズーリンペンデュラム」", Stage.One,   Level.Hard),
        new( 10, "守符「ペンデュラムガード」",           Stage.One,   Level.Lunatic),
        new( 11, "大輪「からかさ後光」",                 Stage.Two,   Level.Easy),
        new( 12, "大輪「からかさ後光」",                 Stage.Two,   Level.Normal),
        new( 13, "大輪「ハロウフォゴットンワールド」",   Stage.Two,   Level.Hard),
        new( 14, "大輪「ハロウフォゴットンワールド」",   Stage.Two,   Level.Lunatic),
        new( 15, "傘符「パラソルスターシンフォニー」",   Stage.Two,   Level.Easy),
        new( 16, "傘符「パラソルスターシンフォニー」",   Stage.Two,   Level.Normal),
        new( 17, "傘符「パラソルスターメモリーズ」",     Stage.Two,   Level.Hard),
        new( 18, "傘符「パラソルスターメモリーズ」",     Stage.Two,   Level.Lunatic),
        new( 19, "雨符「雨夜の怪談」",                   Stage.Two,   Level.Easy),
        new( 20, "雨符「雨夜の怪談」",                   Stage.Two,   Level.Normal),
        new( 21, "雨傘「超撥水かさかさお化け」",         Stage.Two,   Level.Hard),
        new( 22, "雨傘「超撥水かさかさお化け」",         Stage.Two,   Level.Lunatic),
        new( 23, "化符「忘れ傘の夜行列車」",             Stage.Two,   Level.Easy),
        new( 24, "化符「忘れ傘の夜行列車」",             Stage.Two,   Level.Normal),
        new( 25, "化鉄「置き傘特急ナイトカーニバル」",   Stage.Two,   Level.Hard),
        new( 26, "化鉄「置き傘特急ナイトカーニバル」",   Stage.Two,   Level.Lunatic),
        new( 27, "鉄拳「問答無用の妖怪拳」",             Stage.Three, Level.Easy),
        new( 28, "鉄拳「問答無用の妖怪拳」",             Stage.Three, Level.Normal),
        new( 29, "神拳「雲上地獄突き」",                 Stage.Three, Level.Hard),
        new( 30, "神拳「天海地獄突き」",                 Stage.Three, Level.Lunatic),
        new( 31, "拳符「天網サンドバッグ」",             Stage.Three, Level.Easy),
        new( 32, "拳符「天網サンドバッグ」",             Stage.Three, Level.Normal),
        new( 33, "連打「雲界クラーケン殴り」",           Stage.Three, Level.Hard),
        new( 34, "連打「キングクラーケン殴り」",         Stage.Three, Level.Lunatic),
        new( 35, "拳打「げんこつスマッシュ」",           Stage.Three, Level.Easy),
        new( 36, "拳打「げんこつスマッシュ」",           Stage.Three, Level.Normal),
        new( 37, "潰滅「天上天下連続フック」",           Stage.Three, Level.Hard),
        new( 38, "潰滅「天上天下連続フック」",           Stage.Three, Level.Lunatic),
        new( 39, "大喝「時代親父大目玉」",               Stage.Three, Level.Easy),
        new( 40, "大喝「時代親父大目玉」",               Stage.Three, Level.Normal),
        new( 41, "忿怒「天変大目玉焼き」",               Stage.Three, Level.Hard),
        new( 42, "忿怒「空前絶後大目玉焼き」",           Stage.Three, Level.Lunatic),
        new( 43, "転覆「道連れアンカー」",               Stage.Four,  Level.Easy),
        new( 44, "転覆「道連れアンカー」",               Stage.Four,  Level.Normal),
        new( 45, "転覆「沈没アンカー」",                 Stage.Four,  Level.Hard),
        new( 46, "転覆「撃沈アンカー」",                 Stage.Four,  Level.Lunatic),
        new( 47, "溺符「ディープヴォーテックス」",       Stage.Four,  Level.Easy),
        new( 48, "溺符「ディープヴォーテックス」",       Stage.Four,  Level.Normal),
        new( 49, "溺符「シンカブルヴォーテックス」",     Stage.Four,  Level.Hard),
        new( 50, "溺符「シンカブルヴォーテックス」",     Stage.Four,  Level.Lunatic),
        new( 51, "湊符「ファントムシップハーバー」",     Stage.Four,  Level.Easy),
        new( 52, "湊符「ファントムシップハーバー」",     Stage.Four,  Level.Normal),
        new( 53, "湊符「幽霊船の港」",                   Stage.Four,  Level.Hard),
        new( 54, "湊符「幽霊船永久停泊」",               Stage.Four,  Level.Lunatic),
        new( 55, "幽霊「シンカーゴースト」",             Stage.Four,  Level.Normal),
        new( 56, "幽霊「忍び寄る柄杓」",                 Stage.Four,  Level.Hard),
        new( 57, "幽霊「忍び寄る柄杓」",                 Stage.Four,  Level.Lunatic),
        new( 58, "宝塔「グレイテストトレジャー」",       Stage.Five,  Level.Easy),
        new( 59, "宝塔「グレイテストトレジャー」",       Stage.Five,  Level.Normal),
        new( 60, "宝塔「グレイテストトレジャー」",       Stage.Five,  Level.Hard),
        new( 61, "宝塔「グレイテストトレジャー」",       Stage.Five,  Level.Lunatic),
        new( 62, "宝塔「レイディアントトレジャー」",     Stage.Five,  Level.Easy),
        new( 63, "宝塔「レイディアントトレジャー」",     Stage.Five,  Level.Normal),
        new( 64, "宝塔「レイディアントトレジャーガン」", Stage.Five,  Level.Hard),
        new( 65, "宝塔「レイディアントトレジャーガン」", Stage.Five,  Level.Lunatic),
        new( 66, "光符「アブソリュートジャスティス」",   Stage.Five,  Level.Easy),
        new( 67, "光符「アブソリュートジャスティス」",   Stage.Five,  Level.Normal),
        new( 68, "光符「正義の威光」",                   Stage.Five,  Level.Hard),
        new( 69, "光符「正義の威光」",                   Stage.Five,  Level.Lunatic),
        new( 70, "法力「至宝の独鈷杵」",                 Stage.Five,  Level.Easy),
        new( 71, "法力「至宝の独鈷杵」",                 Stage.Five,  Level.Normal),
        new( 72, "法灯「隙間無い法の独鈷杵」",           Stage.Five,  Level.Hard),
        new( 73, "法灯「隙間無い法の独鈷杵」",           Stage.Five,  Level.Lunatic),
        new( 74, "光符「浄化の魔」",                     Stage.Five,  Level.Easy),
        new( 75, "光符「浄化の魔」",                     Stage.Five,  Level.Normal),
        new( 76, "光符「浄化の魔」",                     Stage.Five,  Level.Hard),
        new( 77, "「コンプリートクラリフィケイション」", Stage.Five,  Level.Lunatic),
        new( 78, "魔法「紫雲のオーメン」",               Stage.Six,   Level.Easy),
        new( 79, "魔法「紫雲のオーメン」",               Stage.Six,   Level.Normal),
        new( 80, "吉兆「紫の雲路」",                     Stage.Six,   Level.Hard),
        new( 81, "吉兆「極楽の紫の雲路」",               Stage.Six,   Level.Lunatic),
        new( 82, "魔法「魔界蝶の妖香」",                 Stage.Six,   Level.Easy),
        new( 83, "魔法「魔界蝶の妖香」",                 Stage.Six,   Level.Normal),
        new( 84, "魔法「マジックバタフライ」",           Stage.Six,   Level.Hard),
        new( 85, "魔法「マジックバタフライ」",           Stage.Six,   Level.Lunatic),
        new( 86, "光魔「スターメイルシュトロム」",       Stage.Six,   Level.Easy),
        new( 87, "光魔「スターメイルシュトロム」",       Stage.Six,   Level.Normal),
        new( 88, "光魔「魔法銀河系」",                   Stage.Six,   Level.Hard),
        new( 89, "光魔「魔法銀河系」",                   Stage.Six,   Level.Lunatic),
        new( 90, "大魔法「魔神復誦」",                   Stage.Six,   Level.Easy),
        new( 91, "大魔法「魔神復誦」",                   Stage.Six,   Level.Normal),
        new( 92, "大魔法「魔神復誦」",                   Stage.Six,   Level.Hard),
        new( 93, "大魔法「魔神復誦」",                   Stage.Six,   Level.Lunatic),
        new( 94, "「聖尼公のエア巻物」",                 Stage.Six,   Level.Normal),
        new( 95, "「聖尼公のエア巻物」",                 Stage.Six,   Level.Hard),
        new( 96, "超人「聖白蓮」",                       Stage.Six,   Level.Lunatic),
        new( 97, "飛鉢「フライングファンタスティカ」",   Stage.Six,   Level.Easy),
        new( 98, "飛鉢「フライングファンタスティカ」",   Stage.Six,   Level.Normal),
        new( 99, "飛鉢「伝説の飛空円盤」",               Stage.Six,   Level.Hard),
        new(100, "飛鉢「伝説の飛空円盤」",               Stage.Six,   Level.Lunatic),
        new(101, "傘符「大粒の涙雨」",                   Stage.Extra, Level.Extra),
        new(102, "驚雨「ゲリラ台風」",                   Stage.Extra, Level.Extra),
        new(103, "後光「からかさ驚きフラッシュ」",       Stage.Extra, Level.Extra),
        new(104, "妖雲「平安のダーククラウド」",         Stage.Extra, Level.Extra),
        new(105, "正体不明「忿怒のレッドＵＦＯ襲来」",   Stage.Extra, Level.Extra),
        new(106, "鵺符「鵺的スネークショー」",           Stage.Extra, Level.Extra),
        new(107, "正体不明「哀愁のブルーＵＦＯ襲来」",   Stage.Extra, Level.Extra),
        new(108, "鵺符「弾幕キメラ」",                   Stage.Extra, Level.Extra),
        new(109, "正体不明「義心のグリーンＵＦＯ襲来」", Stage.Extra, Level.Extra),
        new(110, "鵺符「アンディファインドダークネス」", Stage.Extra, Level.Extra),
        new(111, "正体不明「恐怖の虹色ＵＦＯ襲来」",     Stage.Extra, Level.Extra),
        new(112, "「平安京の悪夢」",                     Stage.Extra, Level.Extra),
        new(113, "恨弓「源三位頼政の弓」",               Stage.Extra, Level.Extra),
#pragma warning restore SA1008 // Opening parenthesis should be spaced correctly
    }.ToDictionary(card => card.Id);

    public static string FormatPrefix { get; } = "%T12";

    public static bool IsTotal(CharaWithTotal chara)
    {
        return chara is CharaWithTotal.Total;
    }
}

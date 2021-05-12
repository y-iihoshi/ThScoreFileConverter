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

namespace ThScoreFileConverter.Models.Th15
{
    internal static class Definitions
    {
        // Thanks to thwiki.info
        public static IReadOnlyDictionary<int, CardInfo> CardTable { get; } = new CardInfo[]
        {
#pragma warning disable SA1008 // Opening parenthesis should be spaced correctly
            new(  1, "凶弾「スピードストライク」",           Stage.One,   Level.Hard),
            new(  2, "凶弾「スピードストライク」",           Stage.One,   Level.Lunatic),
            new(  3, "弾符「イーグルシューティング」",       Stage.One,   Level.Easy),
            new(  4, "弾符「イーグルシューティング」",       Stage.One,   Level.Normal),
            new(  5, "弾符「イーグルシューティング」",       Stage.One,   Level.Hard),
            new(  6, "弾符「鷹は撃ち抜いた」",               Stage.One,   Level.Lunatic),
            new(  7, "銃符「ルナティックガン」",             Stage.One,   Level.Easy),
            new(  8, "銃符「ルナティックガン」",             Stage.One,   Level.Normal),
            new(  9, "銃符「ルナティックガン」",             Stage.One,   Level.Hard),
            new( 10, "銃符「ルナティックガン」",             Stage.One,   Level.Lunatic),
            new( 11, "兎符「ストロベリーダンゴ」",           Stage.Two,   Level.Easy),
            new( 12, "兎符「ストロベリーダンゴ」",           Stage.Two,   Level.Normal),
            new( 13, "兎符「ベリーベリーダンゴ」",           Stage.Two,   Level.Hard),
            new( 14, "兎符「ベリーベリーダンゴ」",           Stage.Two,   Level.Lunatic),
            new( 15, "兎符「ダンゴインフリューエンス」",     Stage.Two,   Level.Easy),
            new( 16, "兎符「ダンゴインフリューエンス」",     Stage.Two,   Level.Normal),
            new( 17, "兎符「ダンゴインフリューエンス」",     Stage.Two,   Level.Hard),
            new( 18, "兎符「ダンゴインフリューエンス」",     Stage.Two,   Level.Lunatic),
            new( 19, "月見「セプテンバーフルムーン」",       Stage.Two,   Level.Easy),
            new( 20, "月見「セプテンバーフルムーン」",       Stage.Two,   Level.Normal),
            new( 21, "月見「セプテンバーフルムーン」",       Stage.Two,   Level.Hard),
            new( 22, "月見酒「ルナティックセプテンバー」",   Stage.Two,   Level.Lunatic),
            new( 23, "夢符「緋色の悪夢」",                   Stage.Three, Level.Easy),
            new( 24, "夢符「緋色の悪夢」",                   Stage.Three, Level.Normal),
            new( 25, "夢符「緋色の圧迫悪夢」",               Stage.Three, Level.Hard),
            new( 26, "夢符「緋色の圧迫悪夢」",               Stage.Three, Level.Lunatic),
            new( 27, "夢符「藍色の愁夢」",                   Stage.Three, Level.Easy),
            new( 28, "夢符「藍色の愁夢」",                   Stage.Three, Level.Normal),
            new( 29, "夢符「藍色の愁三重夢」",               Stage.Three, Level.Hard),
            new( 30, "夢符「愁永遠の夢」",                   Stage.Three, Level.Lunatic),
            new( 31, "夢符「刈安色の迷夢」",                 Stage.Three, Level.Easy),
            new( 32, "夢符「刈安色の迷夢」",                 Stage.Three, Level.Normal),
            new( 33, "夢符「刈安色の錯綜迷夢」",             Stage.Three, Level.Hard),
            new( 34, "夢符「刈安色の錯綜迷夢」",             Stage.Three, Level.Lunatic),
            new( 35, "夢符「ドリームキャッチャー」",         Stage.Three, Level.Easy),
            new( 36, "夢符「ドリームキャッチャー」",         Stage.Three, Level.Normal),
            new( 37, "夢符「蒼色のドリームキャッチャー」",   Stage.Three, Level.Hard),
            new( 38, "夢符「夢我夢中」",                     Stage.Three, Level.Lunatic),
            new( 39, "月符「紺色の狂夢」",                   Stage.Three, Level.Easy),
            new( 40, "月符「紺色の狂夢」",                   Stage.Three, Level.Normal),
            new( 41, "月符「紺色の狂夢」",                   Stage.Three, Level.Hard),
            new( 42, "月符「紺色の狂夢」",                   Stage.Three, Level.Lunatic),
            new( 43, "玉符「烏合の呪」",                     Stage.Four,  Level.Easy),
            new( 44, "玉符「烏合の呪」",                     Stage.Four,  Level.Normal),
            new( 45, "玉符「烏合の逆呪」",                   Stage.Four,  Level.Hard),
            new( 46, "玉符「烏合の二重呪」",                 Stage.Four,  Level.Lunatic),
            new( 47, "玉符「穢身探知型機雷」",               Stage.Four,  Level.Easy),
            new( 48, "玉符「穢身探知型機雷」",               Stage.Four,  Level.Normal),
            new( 49, "玉符「穢身探知型機雷 改」",            Stage.Four,  Level.Hard),
            new( 50, "玉符「穢身探知型機雷 改」",            Stage.Four,  Level.Lunatic),
            new( 51, "玉符「神々の弾冠」",                   Stage.Four,  Level.Easy),
            new( 52, "玉符「神々の弾冠」",                   Stage.Four,  Level.Normal),
            new( 53, "玉符「神々の光り輝く弾冠」",           Stage.Four,  Level.Hard),
            new( 54, "玉符「神々の光り輝く弾冠」",           Stage.Four,  Level.Lunatic),
            new( 55, "「片翼の白鷺」",                       Stage.Four,  Level.Easy),
            new( 56, "「片翼の白鷺」",                       Stage.Four,  Level.Normal),
            new( 57, "「片翼の白鷺」",                       Stage.Four,  Level.Hard),
            new( 58, "「片翼の白鷺」",                       Stage.Four,  Level.Lunatic),
            new( 59, "獄符「ヘルエクリプス」",               Stage.Five,  Level.Easy),
            new( 60, "獄符「ヘルエクリプス」",               Stage.Five,  Level.Normal),
            new( 61, "獄符「地獄の蝕」",                     Stage.Five,  Level.Hard),
            new( 62, "獄符「地獄の蝕」",                     Stage.Five,  Level.Lunatic),
            new( 63, "獄符「フラッシュアンドストライプ」",   Stage.Five,  Level.Easy),
            new( 64, "獄符「フラッシュアンドストライプ」",   Stage.Five,  Level.Normal),
            new( 65, "獄符「スターアンドストライプ」",       Stage.Five,  Level.Hard),
            new( 66, "獄符「スターアンドストライプ」",       Stage.Five,  Level.Lunatic),
            new( 67, "獄炎「グレイズインフェルノ」",         Stage.Five,  Level.Easy),
            new( 68, "獄炎「グレイズインフェルノ」",         Stage.Five,  Level.Normal),
            new( 69, "獄炎「グレイズインフェルノ」",         Stage.Five,  Level.Hard),
            new( 70, "獄炎「かすりの獄意」",                 Stage.Five,  Level.Lunatic),
            new( 71, "地獄「ストライプドアビス」",           Stage.Five,  Level.Easy),
            new( 72, "地獄「ストライプドアビス」",           Stage.Five,  Level.Normal),
            new( 73, "地獄「ストライプドアビス」",           Stage.Five,  Level.Hard),
            new( 74, "地獄「ストライプドアビス」",           Stage.Five,  Level.Lunatic),
            new( 75, "「フェイクアポロ」",                   Stage.Five,  Level.Easy),
            new( 76, "「フェイクアポロ」",                   Stage.Five,  Level.Normal),
            new( 77, "「アポロ捏造説」",                     Stage.Five,  Level.Hard),
            new( 78, "「アポロ捏造説」",                     Stage.Five,  Level.Lunatic),
            new( 79, "「掌の純光」",                         Stage.Six,   Level.Easy),
            new( 80, "「掌の純光」",                         Stage.Six,   Level.Normal),
            new( 81, "「掌の純光」",                         Stage.Six,   Level.Hard),
            new( 82, "「掌の純光」",                         Stage.Six,   Level.Lunatic),
            new( 83, "「殺意の百合」",                       Stage.Six,   Level.Easy),
            new( 84, "「殺意の百合」",                       Stage.Six,   Level.Normal),
            new( 85, "「殺意の百合」",                       Stage.Six,   Level.Hard),
            new( 86, "「殺意の百合」",                       Stage.Six,   Level.Lunatic),
            new( 87, "「原始の神霊界」",                     Stage.Six,   Level.Easy),
            new( 88, "「原始の神霊界」",                     Stage.Six,   Level.Normal),
            new( 89, "「現代の神霊界」",                     Stage.Six,   Level.Hard),
            new( 90, "「現代の神霊界」",                     Stage.Six,   Level.Lunatic),
            new( 91, "「震え凍える星」",                     Stage.Six,   Level.Easy),
            new( 92, "「震え凍える星」",                     Stage.Six,   Level.Normal),
            new( 93, "「震え凍える星」",                     Stage.Six,   Level.Hard),
            new( 94, "「震え凍える星」",                     Stage.Six,   Level.Lunatic),
            new( 95, "「純粋なる狂気」",                     Stage.Six,   Level.Easy),
            new( 96, "「純粋なる狂気」",                     Stage.Six,   Level.Normal),
            new( 97, "「純粋なる狂気」",                     Stage.Six,   Level.Hard),
            new( 98, "「純粋なる狂気」",                     Stage.Six,   Level.Lunatic),
            new( 99, "「溢れ出る瑕穢」",                     Stage.Six,   Level.Easy),
            new(100, "「溢れ出る瑕穢」",                     Stage.Six,   Level.Normal),
            new(101, "「溢れ出る瑕穢」",                     Stage.Six,   Level.Hard),
            new(102, "「地上穢の純化」",                     Stage.Six,   Level.Lunatic),
            new(103, "純符「ピュアリーバレットヘル」",       Stage.Six,   Level.Easy),
            new(104, "純符「ピュアリーバレットヘル」",       Stage.Six,   Level.Normal),
            new(105, "純符「純粋な弾幕地獄」",               Stage.Six,   Level.Hard),
            new(106, "純符「純粋な弾幕地獄」",               Stage.Six,   Level.Lunatic),
            new(107, "胡蝶「バタフライサプランテーション」", Stage.Extra, Level.Extra),
            new(108, "超特急「ドリームエクスプレス」",       Stage.Extra, Level.Extra),
            new(109, "這夢「クリーピングバレット」",         Stage.Extra, Level.Extra),
            new(110, "異界「逢魔ガ刻」",                     Stage.Extra, Level.Extra),
            new(111, "地球「邪穢在身」",                     Stage.Extra, Level.Extra),
            new(112, "月「アポロ反射鏡」",                   Stage.Extra, Level.Extra),
            new(113, "「袋の鼠を追い詰める為の単純な弾幕」", Stage.Extra, Level.Extra),
            new(114, "異界「地獄のノンイデアル弾幕」",       Stage.Extra, Level.Extra),
            new(115, "地球「地獄に降る雨」",                 Stage.Extra, Level.Extra),
            new(116, "月「ルナティックインパクト」",         Stage.Extra, Level.Extra),
            new(117, "「人を殺める為の純粋な弾幕」",         Stage.Extra, Level.Extra),
            new(118, "「トリニタリアンラプソディ」",         Stage.Extra, Level.Extra),
            new(119, "「最初で最後の無名の弾幕」",           Stage.Extra, Level.Extra),
#pragma warning restore SA1008 // Opening parenthesis should be spaced correctly
        }.ToDictionary(card => card.Id);

        public static string FormatPrefix { get; } = "%T15";

        public static bool IsTotal(CharaWithTotal chara)
        {
            return chara is CharaWithTotal.Total;
        }
    }
}

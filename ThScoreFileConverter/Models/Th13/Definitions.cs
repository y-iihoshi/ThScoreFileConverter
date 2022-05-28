﻿//-----------------------------------------------------------------------
// <copyright file="Definitions.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using System.Linq;
using ThScoreFileConverter.Core.Models.Th13;
using CardInfo = ThScoreFileConverter.Models.SpellCardInfo<
    ThScoreFileConverter.Core.Models.Th13.StagePractice, ThScoreFileConverter.Core.Models.Th13.LevelPractice>;

namespace ThScoreFileConverter.Models.Th13;

internal static class Definitions
{
    // Thanks to thwiki.info
    public static IReadOnlyDictionary<int, CardInfo> CardTable { get; } = new CardInfo[]
    {
#pragma warning disable SA1008 // Opening parenthesis should be spaced correctly
        new(  1, "符牒「死蝶の舞」",                     StagePractice.One,       LevelPractice.Easy),
        new(  2, "符牒「死蝶の舞」",                     StagePractice.One,       LevelPractice.Normal),
        new(  3, "符牒「死蝶の舞　- 桜花 -」",           StagePractice.One,       LevelPractice.Hard),
        new(  4, "符牒「死蝶の舞　- 桜花 -」",           StagePractice.One,       LevelPractice.Lunatic),
        new(  5, "幽蝶「ゴーストスポット」",             StagePractice.One,       LevelPractice.Easy),
        new(  6, "幽蝶「ゴーストスポット」",             StagePractice.One,       LevelPractice.Normal),
        new(  7, "幽蝶「ゴーストスポット　- 桜花 -」",   StagePractice.One,       LevelPractice.Hard),
        new(  8, "幽蝶「ゴーストスポット　- 桜花 -」",   StagePractice.One,       LevelPractice.Lunatic),
        new(  9, "冥符「常夜桜」",                       StagePractice.One,       LevelPractice.Easy),
        new( 10, "冥符「常夜桜」",                       StagePractice.One,       LevelPractice.Normal),
        new( 11, "冥符「常夜桜」",                       StagePractice.One,       LevelPractice.Hard),
        new( 12, "冥符「常夜桜」",                       StagePractice.One,       LevelPractice.Lunatic),
        new( 13, "桜符「西行桜吹雪」",                   StagePractice.One,       LevelPractice.Hard),
        new( 14, "桜符「西行桜吹雪」",                   StagePractice.One,       LevelPractice.Lunatic),
        new( 15, "響符「マウンテンエコー」",             StagePractice.Two,       LevelPractice.Easy),
        new( 16, "響符「マウンテンエコー」",             StagePractice.Two,       LevelPractice.Normal),
        new( 17, "響符「マウンテンエコースクランブル」", StagePractice.Two,       LevelPractice.Hard),
        new( 18, "響符「マウンテンエコースクランブル」", StagePractice.Two,       LevelPractice.Lunatic),
        new( 19, "響符「パワーレゾナンス」",             StagePractice.Two,       LevelPractice.Easy),
        new( 20, "響符「パワーレゾナンス」",             StagePractice.Two,       LevelPractice.Normal),
        new( 21, "響符「パワーレゾナンス」",             StagePractice.Two,       LevelPractice.Hard),
        new( 22, "響符「パワーレゾナンス」",             StagePractice.Two,       LevelPractice.Lunatic),
        new( 23, "山彦「ロングレンジエコー」",           StagePractice.Two,       LevelPractice.Easy),
        new( 24, "山彦「ロングレンジエコー」",           StagePractice.Two,       LevelPractice.Normal),
        new( 25, "山彦「アンプリファイエコー」",         StagePractice.Two,       LevelPractice.Hard),
        new( 26, "山彦「アンプリファイエコー」",         StagePractice.Two,       LevelPractice.Lunatic),
        new( 27, "大声「チャージドクライ」",             StagePractice.Two,       LevelPractice.Easy),
        new( 28, "大声「チャージドクライ」",             StagePractice.Two,       LevelPractice.Normal),
        new( 29, "大声「チャージドヤッホー」",           StagePractice.Two,       LevelPractice.Hard),
        new( 30, "大声「チャージドヤッホー」",           StagePractice.Two,       LevelPractice.Lunatic),
        new( 31, "虹符「アンブレラサイクロン」",         StagePractice.Three,     LevelPractice.Hard),
        new( 32, "虹符「アンブレラサイクロン」",         StagePractice.Three,     LevelPractice.Lunatic),
        new( 33, "回復「ヒールバイデザイア」",           StagePractice.Three,     LevelPractice.Easy),
        new( 34, "回復「ヒールバイデザイア」",           StagePractice.Three,     LevelPractice.Normal),
        new( 35, "回復「ヒールバイデザイア」",           StagePractice.Three,     LevelPractice.Hard),
        new( 36, "回復「ヒールバイデザイア」",           StagePractice.Three,     LevelPractice.Lunatic),
        new( 37, "毒爪「ポイズンレイズ」",               StagePractice.Three,     LevelPractice.Easy),
        new( 38, "毒爪「ポイズンレイズ」",               StagePractice.Three,     LevelPractice.Normal),
        new( 39, "毒爪「ポイズンマーダー」",             StagePractice.Three,     LevelPractice.Hard),
        new( 40, "毒爪「ポイズンマーダー」",             StagePractice.Three,     LevelPractice.Lunatic),
        new( 41, "欲符「稼欲霊招来」",                   StagePractice.Three,     LevelPractice.Easy),
        new( 42, "欲符「稼欲霊招来」",                   StagePractice.Three,     LevelPractice.Normal),
        new( 43, "欲霊「スコアデザイアイーター」",       StagePractice.Three,     LevelPractice.Hard),
        new( 44, "欲霊「スコアデザイアイーター」",       StagePractice.Three,     LevelPractice.Lunatic),
        new( 45, "邪符「ヤンシャオグイ」",               StagePractice.Four,      LevelPractice.Normal),
        new( 46, "邪符「グーフンイエグイ」",             StagePractice.Four,      LevelPractice.Hard),
        new( 47, "邪符「グーフンイエグイ」",             StagePractice.Four,      LevelPractice.Lunatic),
        new( 48, "入魔「ゾウフォルゥモォ」",             StagePractice.Four,      LevelPractice.Easy),
        new( 49, "入魔「ゾウフォルゥモォ」",             StagePractice.Four,      LevelPractice.Normal),
        new( 50, "入魔「ゾウフォルゥモォ」",             StagePractice.Four,      LevelPractice.Hard),
        new( 51, "入魔「ゾウフォルゥモォ」",             StagePractice.Four,      LevelPractice.Lunatic),
        new( 52, "降霊「死人タンキー」",                 StagePractice.Four,      LevelPractice.Easy),
        new( 53, "降霊「死人タンキー」",                 StagePractice.Four,      LevelPractice.Normal),
        new( 54, "通霊「トンリン芳香」",                 StagePractice.Four,      LevelPractice.Hard),
        new( 55, "通霊「トンリン芳香」",                 StagePractice.Four,      LevelPractice.Lunatic),
        new( 56, "道符「タオ胎動」",                     StagePractice.Four,      LevelPractice.Easy),
        new( 57, "道符「タオ胎動」",                     StagePractice.Four,      LevelPractice.Normal),
        new( 58, "道符「タオ胎動」",                     StagePractice.Four,      LevelPractice.Hard),
        new( 59, "道符「タオ胎動」",                     StagePractice.Four,      LevelPractice.Lunatic),
        new( 60, "雷矢「ガゴウジサイクロン」",           StagePractice.Five,      LevelPractice.Normal),
        new( 61, "雷矢「ガゴウジサイクロン」",           StagePractice.Five,      LevelPractice.Hard),
        new( 62, "雷矢「ガゴウジトルネード」",           StagePractice.Five,      LevelPractice.Lunatic),
        new( 63, "天符「雨の磐舟」",                     StagePractice.Five,      LevelPractice.Easy),
        new( 64, "天符「雨の磐舟」",                     StagePractice.Five,      LevelPractice.Normal),
        new( 65, "天符「天の磐舟よ天へ昇れ」",           StagePractice.Five,      LevelPractice.Hard),
        new( 66, "天符「天の磐舟よ天へ昇れ」",           StagePractice.Five,      LevelPractice.Lunatic),
        new( 67, "投皿「物部の八十平瓮」",               StagePractice.Five,      LevelPractice.Easy),
        new( 68, "投皿「物部の八十平瓮」",               StagePractice.Five,      LevelPractice.Normal),
        new( 69, "投皿「物部の八十平瓮」",               StagePractice.Five,      LevelPractice.Hard),
        new( 70, "投皿「物部の八十平瓮」",               StagePractice.Five,      LevelPractice.Lunatic),
        new( 71, "炎符「廃仏の炎風」",                   StagePractice.Five,      LevelPractice.Easy),
        new( 72, "炎符「廃仏の炎風」",                   StagePractice.Five,      LevelPractice.Normal),
        new( 73, "炎符「桜井寺炎上」",                   StagePractice.Five,      LevelPractice.Hard),
        new( 74, "炎符「桜井寺炎上」",                   StagePractice.Five,      LevelPractice.Lunatic),
        new( 75, "聖童女「大物忌正餐」",                 StagePractice.Five,      LevelPractice.Easy),
        new( 76, "聖童女「大物忌正餐」",                 StagePractice.Five,      LevelPractice.Normal),
        new( 77, "聖童女「大物忌正餐」",                 StagePractice.Five,      LevelPractice.Hard),
        new( 78, "聖童女「大物忌正餐」",                 StagePractice.Five,      LevelPractice.Lunatic),
        new( 79, "名誉「十二階の色彩」",                 StagePractice.Six,       LevelPractice.Easy),
        new( 80, "名誉「十二階の色彩」",                 StagePractice.Six,       LevelPractice.Normal),
        new( 81, "名誉「十二階の冠位」",                 StagePractice.Six,       LevelPractice.Hard),
        new( 82, "名誉「十二階の冠位」",                 StagePractice.Six,       LevelPractice.Lunatic),
        new( 83, "仙符「日出ずる処の道士」",             StagePractice.Six,       LevelPractice.Easy),
        new( 84, "仙符「日出ずる処の道士」",             StagePractice.Six,       LevelPractice.Normal),
        new( 85, "仙符「日出ずる処の天子」",             StagePractice.Six,       LevelPractice.Hard),
        new( 86, "仙符「日出ずる処の天子」",             StagePractice.Six,       LevelPractice.Lunatic),
        new( 87, "召喚「豪族乱舞」",                     StagePractice.Six,       LevelPractice.Easy),
        new( 88, "召喚「豪族乱舞」",                     StagePractice.Six,       LevelPractice.Normal),
        new( 89, "召喚「豪族乱舞」",                     StagePractice.Six,       LevelPractice.Hard),
        new( 90, "召喚「豪族乱舞」",                     StagePractice.Six,       LevelPractice.Lunatic),
        new( 91, "秘宝「斑鳩寺の天球儀」",               StagePractice.Six,       LevelPractice.Easy),
        new( 92, "秘宝「斑鳩寺の天球儀」",               StagePractice.Six,       LevelPractice.Normal),
        new( 93, "秘宝「斑鳩寺の天球儀」",               StagePractice.Six,       LevelPractice.Hard),
        new( 94, "秘宝「聖徳太子のオーパーツ」",         StagePractice.Six,       LevelPractice.Lunatic),
        new( 95, "光符「救世観音の光後光」",             StagePractice.Six,       LevelPractice.Easy),
        new( 96, "光符「救世観音の光後光」",             StagePractice.Six,       LevelPractice.Normal),
        new( 97, "光符「グセフラッシュ」",               StagePractice.Six,       LevelPractice.Hard),
        new( 98, "光符「グセフラッシュ」",               StagePractice.Six,       LevelPractice.Lunatic),
        new( 99, "眼光「十七条のレーザー」",             StagePractice.Six,       LevelPractice.Easy),
        new(100, "眼光「十七条のレーザー」",             StagePractice.Six,       LevelPractice.Normal),
        new(101, "神光「逆らう事なきを宗とせよ」",       StagePractice.Six,       LevelPractice.Hard),
        new(102, "神光「逆らう事なきを宗とせよ」",       StagePractice.Six,       LevelPractice.Lunatic),
        new(103, "「星降る神霊廟」",                     StagePractice.Six,       LevelPractice.Easy),
        new(104, "「星降る神霊廟」",                     StagePractice.Six,       LevelPractice.Normal),
        new(105, "「生まれたての神霊」",                 StagePractice.Six,       LevelPractice.Hard),
        new(106, "「生まれたての神霊」",                 StagePractice.Six,       LevelPractice.Lunatic),
        new(107, "アンノウン「軌道不明の鬼火」",         StagePractice.Extra,     LevelPractice.Extra),
        new(108, "アンノウン「姿態不明の空魚」",         StagePractice.Extra,     LevelPractice.Extra),
        new(109, "アンノウン「原理不明の妖怪玉」",       StagePractice.Extra,     LevelPractice.Extra),
        new(110, "壱番勝負「霊長化弾幕変化」",           StagePractice.Extra,     LevelPractice.Extra),
        new(111, "弐番勝負「肉食化弾幕変化」",           StagePractice.Extra,     LevelPractice.Extra),
        new(112, "参番勝負「延羽化弾幕変化」",           StagePractice.Extra,     LevelPractice.Extra),
        new(113, "四番勝負「両生化弾幕変化」",           StagePractice.Extra,     LevelPractice.Extra),
        new(114, "伍番勝負「鳥獣戯画」",                 StagePractice.Extra,     LevelPractice.Extra),
        new(115, "六番勝負「狸の化け学校」",             StagePractice.Extra,     LevelPractice.Extra),
        new(116, "七番勝負「野生の離島」",               StagePractice.Extra,     LevelPractice.Extra),
        new(117, "変化「まぬけ巫女の偽調伏」",           StagePractice.Extra,     LevelPractice.Extra),
        new(118, "「マミゾウ化弾幕十変化」",             StagePractice.Extra,     LevelPractice.Extra),
        new(119, "狢符「満月のポンポコリン」",           StagePractice.Extra,     LevelPractice.Extra),
        new(120, "桜符「桜吹雪地獄」",                   StagePractice.OverDrive, LevelPractice.OverDrive),
        new(121, "山彦「ヤマビコの本領発揮エコー」",     StagePractice.OverDrive, LevelPractice.OverDrive),
        new(122, "毒爪「死なない殺人鬼」",               StagePractice.OverDrive, LevelPractice.OverDrive),
        new(123, "道符「ＴＡＯ胎動　～道～」",           StagePractice.OverDrive, LevelPractice.OverDrive),
        new(124, "怨霊「入鹿の雷」",                     StagePractice.OverDrive, LevelPractice.OverDrive),
        new(125, "聖童女「太陽神の贄」",                 StagePractice.OverDrive, LevelPractice.OverDrive),
        new(126, "「神霊大宇宙」",                       StagePractice.OverDrive, LevelPractice.OverDrive),
        new(127, "「ワイルドカーペット」",               StagePractice.OverDrive, LevelPractice.OverDrive),
#pragma warning restore SA1008 // Opening parenthesis should be spaced correctly
    }.ToDictionary(card => card.Id);

    public static string FormatPrefix { get; } = "%T13";

    public static bool IsTotal(CharaWithTotal chara)
    {
        return chara is CharaWithTotal.Total;
    }

    public static bool IsToBeSummed(LevelPracticeWithTotal level)
    {
        return level is not LevelPracticeWithTotal.Total;
    }
}

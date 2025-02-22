﻿//-----------------------------------------------------------------------
// <copyright file="Definitions.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using ThScoreFileConverter.Core.Helpers;
using CardInfo = ThScoreFileConverter.Core.Models.SpellCardInfo<
    ThScoreFileConverter.Core.Models.Th08.StagePractice, ThScoreFileConverter.Core.Models.Th08.LevelPractice>;

namespace ThScoreFileConverter.Core.Models.Th08;

/// <summary>
/// Provides several IN specific definitions.
/// </summary>
public static class Definitions
{
    /// <summary>
    /// Gets the dictionary of IN spell cards.
    /// Thanks to thwiki.info.
    /// </summary>
    public static IReadOnlyDictionary<int, CardInfo> CardTable { get; } = new CardInfo[]
    {
#pragma warning disable SA1008 // Opening parenthesis should be spaced correctly
        new(  1, "蛍符「地上の流星」",                       StagePractice.One,          LevelPractice.Hard),
        new(  2, "蛍符「地上の彗星」",                       StagePractice.One,          LevelPractice.Lunatic),
        new(  3, "灯符「ファイヤフライフェノメノン」",       StagePractice.One,          LevelPractice.Easy),
        new(  4, "灯符「ファイヤフライフェノメノン」",       StagePractice.One,          LevelPractice.Normal),
        new(  5, "灯符「ファイヤフライフェノメノン」",       StagePractice.One,          LevelPractice.Hard),
        new(  6, "灯符「ファイヤフライフェノメノン」",       StagePractice.One,          LevelPractice.Lunatic),
        new(  7, "蠢符「リトルバグ」",                       StagePractice.One,          LevelPractice.Easy),
        new(  8, "蠢符「リトルバグストーム」",               StagePractice.One,          LevelPractice.Normal),
        new(  9, "蠢符「ナイトバグストーム」",               StagePractice.One,          LevelPractice.Hard),
        new( 10, "蠢符「ナイトバグトルネード」",             StagePractice.One,          LevelPractice.Lunatic),
        new( 11, "隠蟲「永夜蟄居」",                         StagePractice.One,          LevelPractice.Normal),
        new( 12, "隠蟲「永夜蟄居」",                         StagePractice.One,          LevelPractice.Hard),
        new( 13, "隠蟲「永夜蟄居」",                         StagePractice.One,          LevelPractice.Lunatic),
        new( 14, "声符「梟の夜鳴声」",                       StagePractice.Two,          LevelPractice.Easy),
        new( 15, "声符「梟の夜鳴声」",                       StagePractice.Two,          LevelPractice.Normal),
        new( 16, "声符「木菟咆哮」",                         StagePractice.Two,          LevelPractice.Hard),
        new( 17, "声符「木菟咆哮」",                         StagePractice.Two,          LevelPractice.Lunatic),
        new( 18, "蛾符「天蛾の蠱道」",                       StagePractice.Two,          LevelPractice.Easy),
        new( 19, "蛾符「天蛾の蠱道」",                       StagePractice.Two,          LevelPractice.Normal),
        new( 20, "毒符「毒蛾の鱗粉」",                       StagePractice.Two,          LevelPractice.Hard),
        new( 21, "猛毒「毒蛾の暗闇演舞」",                   StagePractice.Two,          LevelPractice.Lunatic),
        new( 22, "鷹符「イルスタードダイブ」",               StagePractice.Two,          LevelPractice.Easy),
        new( 23, "鷹符「イルスタードダイブ」",               StagePractice.Two,          LevelPractice.Normal),
        new( 24, "鷹符「イルスタードダイブ」",               StagePractice.Two,          LevelPractice.Hard),
        new( 25, "鷹符「イルスタードダイブ」",               StagePractice.Two,          LevelPractice.Lunatic),
        new( 26, "夜盲「夜雀の歌」",                         StagePractice.Two,          LevelPractice.Easy),
        new( 27, "夜盲「夜雀の歌」",                         StagePractice.Two,          LevelPractice.Normal),
        new( 28, "夜盲「夜雀の歌」",                         StagePractice.Two,          LevelPractice.Hard),
        new( 29, "夜盲「夜雀の歌」",                         StagePractice.Two,          LevelPractice.Lunatic),
        new( 30, "夜雀「真夜中のコーラスマスター」",         StagePractice.Two,          LevelPractice.Normal),
        new( 31, "夜雀「真夜中のコーラスマスター」",         StagePractice.Two,          LevelPractice.Hard),
        new( 32, "夜雀「真夜中のコーラスマスター」",         StagePractice.Two,          LevelPractice.Lunatic),
        new( 33, "産霊「ファーストピラミッド」",             StagePractice.Three,        LevelPractice.Easy),
        new( 34, "産霊「ファーストピラミッド」",             StagePractice.Three,        LevelPractice.Normal),
        new( 35, "産霊「ファーストピラミッド」",             StagePractice.Three,        LevelPractice.Hard),
        new( 36, "産霊「ファーストピラミッド」",             StagePractice.Three,        LevelPractice.Lunatic),
        new( 37, "始符「エフェメラリティ137」",              StagePractice.Three,        LevelPractice.Normal),
        new( 38, "始符「エフェメラリティ137」",              StagePractice.Three,        LevelPractice.Hard),
        new( 39, "始符「エフェメラリティ137」",              StagePractice.Three,        LevelPractice.Lunatic),
        new( 40, "野符「武烈クライシス」",                   StagePractice.Three,        LevelPractice.Easy),
        new( 41, "野符「将門クライシス」",                   StagePractice.Three,        LevelPractice.Normal),
        new( 42, "野符「義満クライシス」",                   StagePractice.Three,        LevelPractice.Hard),
        new( 43, "野符「GHQクライシス」",                    StagePractice.Three,        LevelPractice.Lunatic),
        new( 44, "国符「三種の神器　剣」",                   StagePractice.Three,        LevelPractice.Easy),
        new( 45, "国符「三種の神器　玉」",                   StagePractice.Three,        LevelPractice.Normal),
        new( 46, "国符「三種の神器　鏡」",                   StagePractice.Three,        LevelPractice.Hard),
        new( 47, "国体「三種の神器　郷」",                   StagePractice.Three,        LevelPractice.Lunatic),
        new( 48, "終符「幻想天皇」",                         StagePractice.Three,        LevelPractice.Easy),
        new( 49, "終符「幻想天皇」",                         StagePractice.Three,        LevelPractice.Normal),
        new( 50, "虚史「幻想郷伝説」",                       StagePractice.Three,        LevelPractice.Hard),
        new( 51, "虚史「幻想郷伝説」",                       StagePractice.Three,        LevelPractice.Lunatic),
        new( 52, "未来「高天原」",                           StagePractice.Three,        LevelPractice.Normal),
        new( 53, "未来「高天原」",                           StagePractice.Three,        LevelPractice.Hard),
        new( 54, "未来「高天原」",                           StagePractice.Three,        LevelPractice.Lunatic),
        new( 55, "夢符「二重結界」",                         StagePractice.FourUncanny,  LevelPractice.Easy),
        new( 56, "夢符「二重結界」",                         StagePractice.FourUncanny,  LevelPractice.Normal),
        new( 57, "夢境「二重大結界」",                       StagePractice.FourUncanny,  LevelPractice.Hard),
        new( 58, "夢境「二重大結界」",                       StagePractice.FourUncanny,  LevelPractice.Lunatic),
        new( 59, "霊符「夢想封印　散」",                     StagePractice.FourUncanny,  LevelPractice.Easy),
        new( 60, "霊符「夢想封印　散」",                     StagePractice.FourUncanny,  LevelPractice.Normal),
        new( 61, "散霊「夢想封印　寂」",                     StagePractice.FourUncanny,  LevelPractice.Hard),
        new( 62, "散霊「夢想封印　寂」",                     StagePractice.FourUncanny,  LevelPractice.Lunatic),
        new( 63, "夢符「封魔陣」",                           StagePractice.FourUncanny,  LevelPractice.Easy),
        new( 64, "夢符「封魔陣」",                           StagePractice.FourUncanny,  LevelPractice.Normal),
        new( 65, "神技「八方鬼縛陣」",                       StagePractice.FourUncanny,  LevelPractice.Hard),
        new( 66, "神技「八方龍殺陣」",                       StagePractice.FourUncanny,  LevelPractice.Lunatic),
        new( 67, "霊符「夢想封印　集」",                     StagePractice.FourUncanny,  LevelPractice.Easy),
        new( 68, "霊符「夢想封印　集」",                     StagePractice.FourUncanny,  LevelPractice.Normal),
        new( 69, "回霊「夢想封印　侘」",                     StagePractice.FourUncanny,  LevelPractice.Hard),
        new( 70, "回霊「夢想封印　侘」",                     StagePractice.FourUncanny,  LevelPractice.Lunatic),
        new( 71, "境界「二重弾幕結界」",                     StagePractice.FourUncanny,  LevelPractice.Easy),
        new( 72, "境界「二重弾幕結界」",                     StagePractice.FourUncanny,  LevelPractice.Normal),
        new( 73, "大結界「博麗弾幕結界」",                   StagePractice.FourUncanny,  LevelPractice.Hard),
        new( 74, "大結界「博麗弾幕結界」",                   StagePractice.FourUncanny,  LevelPractice.Lunatic),
        new( 75, "神霊「夢想封印　瞬」",                     StagePractice.FourUncanny,  LevelPractice.Normal),
        new( 76, "神霊「夢想封印　瞬」",                     StagePractice.FourUncanny,  LevelPractice.Hard),
        new( 77, "神霊「夢想封印　瞬」",                     StagePractice.FourUncanny,  LevelPractice.Lunatic),
        new( 78, "魔符「ミルキーウェイ」",                   StagePractice.FourPowerful, LevelPractice.Easy),
        new( 79, "魔符「ミルキーウェイ」",                   StagePractice.FourPowerful, LevelPractice.Normal),
        new( 80, "魔空「アステロイドベルト」",               StagePractice.FourPowerful, LevelPractice.Hard),
        new( 81, "魔空「アステロイドベルト」",               StagePractice.FourPowerful, LevelPractice.Lunatic),
        new( 82, "魔符「スターダストレヴァリエ」",           StagePractice.FourPowerful, LevelPractice.Easy),
        new( 83, "魔符「スターダストレヴァリエ」",           StagePractice.FourPowerful, LevelPractice.Normal),
        new( 84, "黒魔「イベントホライズン」",               StagePractice.FourPowerful, LevelPractice.Hard),
        new( 85, "黒魔「イベントホライズン」",               StagePractice.FourPowerful, LevelPractice.Lunatic),
        new( 86, "恋符「ノンディレクショナルレーザー」",     StagePractice.FourPowerful, LevelPractice.Easy),
        new( 87, "恋符「ノンディレクショナルレーザー」",     StagePractice.FourPowerful, LevelPractice.Normal),
        new( 88, "恋風「スターライトタイフーン」",           StagePractice.FourPowerful, LevelPractice.Hard),
        new( 89, "恋風「スターライトタイフーン」",           StagePractice.FourPowerful, LevelPractice.Lunatic),
        new( 90, "恋符「マスタースパーク」",                 StagePractice.FourPowerful, LevelPractice.Easy),
        new( 91, "恋符「マスタースパーク」",                 StagePractice.FourPowerful, LevelPractice.Normal),
        new( 92, "恋心「ダブルスパーク」",                   StagePractice.FourPowerful, LevelPractice.Hard),
        new( 93, "恋心「ダブルスパーク」",                   StagePractice.FourPowerful, LevelPractice.Lunatic),
        new( 94, "光符「アースライトレイ」",                 StagePractice.FourPowerful, LevelPractice.Easy),
        new( 95, "光符「アースライトレイ」",                 StagePractice.FourPowerful, LevelPractice.Normal),
        new( 96, "光撃「シュート・ザ・ムーン」",             StagePractice.FourPowerful, LevelPractice.Hard),
        new( 97, "光撃「シュート・ザ・ムーン」",             StagePractice.FourPowerful, LevelPractice.Lunatic),
        new( 98, "魔砲「ファイナルスパーク」",               StagePractice.FourPowerful, LevelPractice.Normal),
        new( 99, "魔砲「ファイナルスパーク」",               StagePractice.FourPowerful, LevelPractice.Hard),
        new(100, "魔砲「ファイナルマスタースパーク」",       StagePractice.FourPowerful, LevelPractice.Lunatic),
        new(101, "波符「赤眼催眠(マインドシェイカー)」",     StagePractice.Five,         LevelPractice.Easy),
        new(102, "波符「赤眼催眠(マインドシェイカー)」",     StagePractice.Five,         LevelPractice.Normal),
        new(103, "幻波「赤眼催眠(マインドブローイング)」",   StagePractice.Five,         LevelPractice.Hard),
        new(104, "幻波「赤眼催眠(マインドブローイング)」",   StagePractice.Five,         LevelPractice.Lunatic),
        new(105, "狂符「幻視調律(ビジョナリチューニング)」", StagePractice.Five,         LevelPractice.Easy),
        new(106, "狂符「幻視調律(ビジョナリチューニング)」", StagePractice.Five,         LevelPractice.Normal),
        new(107, "狂視「狂視調律(イリュージョンシーカー)」", StagePractice.Five,         LevelPractice.Hard),
        new(108, "狂視「狂視調律(イリュージョンシーカー)」", StagePractice.Five,         LevelPractice.Lunatic),
        new(109, "懶符「生神停止(アイドリングウェーブ)」",   StagePractice.Five,         LevelPractice.Easy),
        new(110, "懶符「生神停止(アイドリングウェーブ)」",   StagePractice.Five,         LevelPractice.Normal),
        new(111, "懶惰「生神停止(マインドストッパー)」",     StagePractice.Five,         LevelPractice.Hard),
        new(112, "懶惰「生神停止(マインドストッパー)」",     StagePractice.Five,         LevelPractice.Lunatic),
        new(113, "散符「真実の月(インビジブルフルムーン)」", StagePractice.Five,         LevelPractice.Easy),
        new(114, "散符「真実の月(インビジブルフルムーン)」", StagePractice.Five,         LevelPractice.Normal),
        new(115, "散符「真実の月(インビジブルフルムーン)」", StagePractice.Five,         LevelPractice.Hard),
        new(116, "散符「真実の月(インビジブルフルムーン)」", StagePractice.Five,         LevelPractice.Lunatic),
        new(117, "月眼「月兎遠隔催眠術(テレメスメリズム)」", StagePractice.Five,         LevelPractice.Normal),
        new(118, "月眼「月兎遠隔催眠術(テレメスメリズム)」", StagePractice.Five,         LevelPractice.Hard),
        new(119, "月眼「月兎遠隔催眠術(テレメスメリズム)」", StagePractice.Five,         LevelPractice.Lunatic),
        new(120, "天丸「壺中の天地」",                       StagePractice.FinalA,       LevelPractice.Easy),
        new(121, "天丸「壺中の天地」",                       StagePractice.FinalA,       LevelPractice.Normal),
        new(122, "天丸「壺中の天地」",                       StagePractice.FinalA,       LevelPractice.Hard),
        new(123, "天丸「壺中の天地」",                       StagePractice.FinalA,       LevelPractice.Lunatic),
        new(124, "覚神「神代の記憶」",                       StagePractice.FinalA,       LevelPractice.Easy),
        new(125, "覚神「神代の記憶」",                       StagePractice.FinalA,       LevelPractice.Normal),
        new(126, "神符「天人の系譜」",                       StagePractice.FinalA,       LevelPractice.Hard),
        new(127, "神符「天人の系譜」",                       StagePractice.FinalA,       LevelPractice.Lunatic),
        new(128, "蘇活「生命遊戯　-ライフゲーム-」",         StagePractice.FinalA,       LevelPractice.Easy),
        new(129, "蘇活「生命遊戯　-ライフゲーム-」",         StagePractice.FinalA,       LevelPractice.Normal),
        new(130, "蘇生「ライジングゲーム」",                 StagePractice.FinalA,       LevelPractice.Hard),
        new(131, "蘇生「ライジングゲーム」",                 StagePractice.FinalA,       LevelPractice.Lunatic),
        new(132, "操神「オモイカネディバイス」",             StagePractice.FinalA,       LevelPractice.Easy),
        new(133, "操神「オモイカネディバイス」",             StagePractice.FinalA,       LevelPractice.Normal),
        new(134, "神脳「オモイカネブレイン」",               StagePractice.FinalA,       LevelPractice.Hard),
        new(135, "神脳「オモイカネブレイン」",               StagePractice.FinalA,       LevelPractice.Lunatic),
        new(136, "天呪「アポロ１３」",                       StagePractice.FinalA,       LevelPractice.Easy),
        new(137, "天呪「アポロ１３」",                       StagePractice.FinalA,       LevelPractice.Normal),
        new(138, "天呪「アポロ１３」",                       StagePractice.FinalA,       LevelPractice.Hard),
        new(139, "天呪「アポロ１３」",                       StagePractice.FinalA,       LevelPractice.Lunatic),
        new(140, "秘術「天文密葬法」",                       StagePractice.FinalA,       LevelPractice.Easy),
        new(141, "秘術「天文密葬法」",                       StagePractice.FinalA,       LevelPractice.Normal),
        new(142, "秘術「天文密葬法」",                       StagePractice.FinalA,       LevelPractice.Hard),
        new(143, "秘術「天文密葬法」",                       StagePractice.FinalA,       LevelPractice.Lunatic),
        new(144, "禁薬「蓬莱の薬」",                         StagePractice.FinalA,       LevelPractice.Easy),
        new(145, "禁薬「蓬莱の薬」",                         StagePractice.FinalA,       LevelPractice.Normal),
        new(146, "禁薬「蓬莱の薬」",                         StagePractice.FinalA,       LevelPractice.Hard),
        new(147, "禁薬「蓬莱の薬」",                         StagePractice.FinalA,       LevelPractice.Lunatic),
        new(148, "薬符「壺中の大銀河」",                     StagePractice.FinalB,       LevelPractice.Easy),
        new(149, "薬符「壺中の大銀河」",                     StagePractice.FinalB,       LevelPractice.Normal),
        new(150, "薬符「壺中の大銀河」",                     StagePractice.FinalB,       LevelPractice.Hard),
        new(151, "薬符「壺中の大銀河」",                     StagePractice.FinalB,       LevelPractice.Lunatic),
        new(152, "難題「龍の頸の玉　-五色の弾丸-」",         StagePractice.FinalB,       LevelPractice.Easy),
        new(153, "難題「龍の頸の玉　-五色の弾丸-」",         StagePractice.FinalB,       LevelPractice.Normal),
        new(154, "神宝「ブリリアントドラゴンバレッタ」",     StagePractice.FinalB,       LevelPractice.Hard),
        new(155, "神宝「ブリリアントドラゴンバレッタ」",     StagePractice.FinalB,       LevelPractice.Lunatic),
        new(156, "難題「仏の御石の鉢　-砕けぬ意思-」",       StagePractice.FinalB,       LevelPractice.Easy),
        new(157, "難題「仏の御石の鉢　-砕けぬ意思-」",       StagePractice.FinalB,       LevelPractice.Normal),
        new(158, "神宝「ブディストダイアモンド」",           StagePractice.FinalB,       LevelPractice.Hard),
        new(159, "神宝「ブディストダイアモンド」",           StagePractice.FinalB,       LevelPractice.Lunatic),
        new(160, "難題「火鼠の皮衣　-焦れぬ心-」",           StagePractice.FinalB,       LevelPractice.Easy),
        new(161, "難題「火鼠の皮衣　-焦れぬ心-」",           StagePractice.FinalB,       LevelPractice.Normal),
        new(162, "神宝「サラマンダーシールド」",             StagePractice.FinalB,       LevelPractice.Hard),
        new(163, "神宝「サラマンダーシールド」",             StagePractice.FinalB,       LevelPractice.Lunatic),
        new(164, "難題「燕の子安貝　-永命線-」",             StagePractice.FinalB,       LevelPractice.Easy),
        new(165, "難題「燕の子安貝　-永命線-」",             StagePractice.FinalB,       LevelPractice.Normal),
        new(166, "神宝「ライフスプリングインフィニティ」",   StagePractice.FinalB,       LevelPractice.Hard),
        new(167, "神宝「ライフスプリングインフィニティ」",   StagePractice.FinalB,       LevelPractice.Lunatic),
        new(168, "難題「蓬莱の弾の枝　-虹色の弾幕-」",       StagePractice.FinalB,       LevelPractice.Easy),
        new(169, "難題「蓬莱の弾の枝　-虹色の弾幕-」",       StagePractice.FinalB,       LevelPractice.Normal),
        new(170, "神宝「蓬莱の玉の枝　-夢色の郷-」",         StagePractice.FinalB,       LevelPractice.Hard),
        new(171, "神宝「蓬莱の玉の枝　-夢色の郷-」",         StagePractice.FinalB,       LevelPractice.Lunatic),
        new(172, "「永夜返し　-初月-」",                     StagePractice.FinalB,       LevelPractice.Easy),
        new(173, "「永夜返し　-三日月-」",                   StagePractice.FinalB,       LevelPractice.Normal),
        new(174, "「永夜返し　-上つ弓張-」",                 StagePractice.FinalB,       LevelPractice.Hard),
        new(175, "「永夜返し　-待宵-」",                     StagePractice.FinalB,       LevelPractice.Lunatic),
        new(176, "「永夜返し　-子の刻-」",                   StagePractice.FinalB,       LevelPractice.Easy),
        new(177, "「永夜返し　-子の二つ-」",                 StagePractice.FinalB,       LevelPractice.Normal),
        new(178, "「永夜返し　-子の三つ-」",                 StagePractice.FinalB,       LevelPractice.Hard),
        new(179, "「永夜返し　-子の四つ-」",                 StagePractice.FinalB,       LevelPractice.Lunatic),
        new(180, "「永夜返し　-丑の刻-」",                   StagePractice.FinalB,       LevelPractice.Easy),
        new(181, "「永夜返し　-丑の二つ-」",                 StagePractice.FinalB,       LevelPractice.Normal),
        new(182, "「永夜返し　-丑三つ時-」",                 StagePractice.FinalB,       LevelPractice.Hard),
        new(183, "「永夜返し　-丑の四つ-」",                 StagePractice.FinalB,       LevelPractice.Lunatic),
        new(184, "「永夜返し　-寅の刻-」",                   StagePractice.FinalB,       LevelPractice.Easy),
        new(185, "「永夜返し　-寅の二つ-」",                 StagePractice.FinalB,       LevelPractice.Normal),
        new(186, "「永夜返し　-寅の三つ-」",                 StagePractice.FinalB,       LevelPractice.Hard),
        new(187, "「永夜返し　-寅の四つ-」",                 StagePractice.FinalB,       LevelPractice.Lunatic),
        new(188, "「永夜返し　-朝靄-」",                     StagePractice.FinalB,       LevelPractice.Easy),
        new(189, "「永夜返し　-夜明け-」",                   StagePractice.FinalB,       LevelPractice.Normal),
        new(190, "「永夜返し　-明けの明星-」",               StagePractice.FinalB,       LevelPractice.Hard),
        new(191, "「永夜返し　-世明け-」",                   StagePractice.FinalB,       LevelPractice.Lunatic),
        new(192, "旧史「旧秘境史　-オールドヒストリー-」",   StagePractice.Extra,        LevelPractice.Extra),
        new(193, "転世「一条戻り橋」",                       StagePractice.Extra,        LevelPractice.Extra),
        new(194, "新史「新幻想史　-ネクストヒストリー-」",   StagePractice.Extra,        LevelPractice.Extra),
        new(195, "時効「月のいはかさの呪い」",               StagePractice.Extra,        LevelPractice.Extra),
        new(196, "不死「火の鳥　-鳳翼天翔-」",               StagePractice.Extra,        LevelPractice.Extra),
        new(197, "藤原「滅罪寺院傷」",                       StagePractice.Extra,        LevelPractice.Extra),
        new(198, "不死「徐福時空」",                         StagePractice.Extra,        LevelPractice.Extra),
        new(199, "滅罪「正直者の死」",                       StagePractice.Extra,        LevelPractice.Extra),
        new(200, "虚人「ウー」",                             StagePractice.Extra,        LevelPractice.Extra),
        new(201, "不滅「フェニックスの尾」",                 StagePractice.Extra,        LevelPractice.Extra),
        new(202, "蓬莱「凱風快晴　-フジヤマヴォルケイノ-」", StagePractice.Extra,        LevelPractice.Extra),
        new(203, "「パゼストバイフェニックス」",             StagePractice.Extra,        LevelPractice.Extra),
        new(204, "「蓬莱人形」",                             StagePractice.Extra,        LevelPractice.Extra),
        new(205, "「インペリシャブルシューティング」",       StagePractice.Extra,        LevelPractice.Extra),
        new(206, "「季節外れのバタフライストーム」",         StagePractice.LastWord,     LevelPractice.LastWord),
        new(207, "「ブラインドナイトバード」",               StagePractice.LastWord,     LevelPractice.LastWord),
        new(208, "「日出づる国の天子」",                     StagePractice.LastWord,     LevelPractice.LastWord),
        new(209, "「幻朧月睨(ルナティックレッドアイズ)」",   StagePractice.LastWord,     LevelPractice.LastWord),
        new(210, "「天網蜘網捕蝶の法」",                     StagePractice.LastWord,     LevelPractice.LastWord),
        new(211, "「蓬莱の樹海」",                           StagePractice.LastWord,     LevelPractice.LastWord),
        new(212, "「フェニックス再誕」",                     StagePractice.LastWord,     LevelPractice.LastWord),
        new(213, "「エンシェントデューパー」",               StagePractice.LastWord,     LevelPractice.LastWord),
        new(214, "「無何有浄化」",                           StagePractice.LastWord,     LevelPractice.LastWord),
        new(215, "「夢想天生」",                             StagePractice.LastWord,     LevelPractice.LastWord),
        new(216, "「ブレイジングスター」",                   StagePractice.LastWord,     LevelPractice.LastWord),
        new(217, "「デフレーションワールド」",               StagePractice.LastWord,     LevelPractice.LastWord),
        new(218, "「待宵反射衛星斬」",                       StagePractice.LastWord,     LevelPractice.LastWord),
        new(219, "「グランギニョル座の怪人」",               StagePractice.LastWord,     LevelPractice.LastWord),
        new(220, "「スカーレットディスティニー」",           StagePractice.LastWord,     LevelPractice.LastWord),
        new(221, "「西行寺無余涅槃」",                       StagePractice.LastWord,     LevelPractice.LastWord),
        new(222, "「深弾幕結界　-夢幻泡影-」",               StagePractice.LastWord,     LevelPractice.LastWord),
#pragma warning restore SA1008 // Opening parenthesis should be spaced correctly
    }.ToDictionary(static card => card.Id);

    /// <summary>
    /// Gets wheter you can practice the specified stage or not.
    /// </summary>
    /// <param name="stage">A stage.</param>
    /// <returns><see langword="true"/> if it can be practiced, otherwize <see langword="false"/>.</returns>
    public static bool CanPractice(Stage stage)
    {
        return EnumHelper.IsDefined(stage) && (stage != Stage.Extra);
    }
}

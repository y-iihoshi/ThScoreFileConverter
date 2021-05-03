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
    ThScoreFileConverter.Models.Th08.StagePractice, ThScoreFileConverter.Models.Th08.LevelPractice>;
using IHighScore = ThScoreFileConverter.Models.Th08.IHighScore<
    ThScoreFileConverter.Models.Th08.Chara,
    ThScoreFileConverter.Models.Level,
    ThScoreFileConverter.Models.Th08.StageProgress>;

namespace ThScoreFileConverter.Models.Th08
{
    internal static class Definitions
    {
        // Thanks to thwiki.info
        public static IReadOnlyDictionary<int, CardInfo> CardTable { get; } = new List<CardInfo>()
        {
#pragma warning disable SA1008 // Opening parenthesis should be spaced correctly
            new CardInfo(  1, "蛍符「地上の流星」",                       StagePractice.One,          LevelPractice.Hard),
            new CardInfo(  2, "蛍符「地上の彗星」",                       StagePractice.One,          LevelPractice.Lunatic),
            new CardInfo(  3, "灯符「ファイヤフライフェノメノン」",       StagePractice.One,          LevelPractice.Easy),
            new CardInfo(  4, "灯符「ファイヤフライフェノメノン」",       StagePractice.One,          LevelPractice.Normal),
            new CardInfo(  5, "灯符「ファイヤフライフェノメノン」",       StagePractice.One,          LevelPractice.Hard),
            new CardInfo(  6, "灯符「ファイヤフライフェノメノン」",       StagePractice.One,          LevelPractice.Lunatic),
            new CardInfo(  7, "蠢符「リトルバグ」",                       StagePractice.One,          LevelPractice.Easy),
            new CardInfo(  8, "蠢符「リトルバグストーム」",               StagePractice.One,          LevelPractice.Normal),
            new CardInfo(  9, "蠢符「ナイトバグストーム」",               StagePractice.One,          LevelPractice.Hard),
            new CardInfo( 10, "蠢符「ナイトバグトルネード」",             StagePractice.One,          LevelPractice.Lunatic),
            new CardInfo( 11, "隠蟲「永夜蟄居」",                         StagePractice.One,          LevelPractice.Normal),
            new CardInfo( 12, "隠蟲「永夜蟄居」",                         StagePractice.One,          LevelPractice.Hard),
            new CardInfo( 13, "隠蟲「永夜蟄居」",                         StagePractice.One,          LevelPractice.Lunatic),
            new CardInfo( 14, "声符「梟の夜鳴声」",                       StagePractice.Two,          LevelPractice.Easy),
            new CardInfo( 15, "声符「梟の夜鳴声」",                       StagePractice.Two,          LevelPractice.Normal),
            new CardInfo( 16, "声符「木菟咆哮」",                         StagePractice.Two,          LevelPractice.Hard),
            new CardInfo( 17, "声符「木菟咆哮」",                         StagePractice.Two,          LevelPractice.Lunatic),
            new CardInfo( 18, "蛾符「天蛾の蠱道」",                       StagePractice.Two,          LevelPractice.Easy),
            new CardInfo( 19, "蛾符「天蛾の蠱道」",                       StagePractice.Two,          LevelPractice.Normal),
            new CardInfo( 20, "毒符「毒蛾の鱗粉」",                       StagePractice.Two,          LevelPractice.Hard),
            new CardInfo( 21, "猛毒「毒蛾の暗闇演舞」",                   StagePractice.Two,          LevelPractice.Lunatic),
            new CardInfo( 22, "鷹符「イルスタードダイブ」",               StagePractice.Two,          LevelPractice.Easy),
            new CardInfo( 23, "鷹符「イルスタードダイブ」",               StagePractice.Two,          LevelPractice.Normal),
            new CardInfo( 24, "鷹符「イルスタードダイブ」",               StagePractice.Two,          LevelPractice.Hard),
            new CardInfo( 25, "鷹符「イルスタードダイブ」",               StagePractice.Two,          LevelPractice.Lunatic),
            new CardInfo( 26, "夜盲「夜雀の歌」",                         StagePractice.Two,          LevelPractice.Easy),
            new CardInfo( 27, "夜盲「夜雀の歌」",                         StagePractice.Two,          LevelPractice.Normal),
            new CardInfo( 28, "夜盲「夜雀の歌」",                         StagePractice.Two,          LevelPractice.Hard),
            new CardInfo( 29, "夜盲「夜雀の歌」",                         StagePractice.Two,          LevelPractice.Lunatic),
            new CardInfo( 30, "夜雀「真夜中のコーラスマスター」",         StagePractice.Two,          LevelPractice.Normal),
            new CardInfo( 31, "夜雀「真夜中のコーラスマスター」",         StagePractice.Two,          LevelPractice.Hard),
            new CardInfo( 32, "夜雀「真夜中のコーラスマスター」",         StagePractice.Two,          LevelPractice.Lunatic),
            new CardInfo( 33, "産霊「ファーストピラミッド」",             StagePractice.Three,        LevelPractice.Easy),
            new CardInfo( 34, "産霊「ファーストピラミッド」",             StagePractice.Three,        LevelPractice.Normal),
            new CardInfo( 35, "産霊「ファーストピラミッド」",             StagePractice.Three,        LevelPractice.Hard),
            new CardInfo( 36, "産霊「ファーストピラミッド」",             StagePractice.Three,        LevelPractice.Lunatic),
            new CardInfo( 37, "始符「エフェメラリティ137」",              StagePractice.Three,        LevelPractice.Normal),
            new CardInfo( 38, "始符「エフェメラリティ137」",              StagePractice.Three,        LevelPractice.Hard),
            new CardInfo( 39, "始符「エフェメラリティ137」",              StagePractice.Three,        LevelPractice.Lunatic),
            new CardInfo( 40, "野符「武烈クライシス」",                   StagePractice.Three,        LevelPractice.Easy),
            new CardInfo( 41, "野符「将門クライシス」",                   StagePractice.Three,        LevelPractice.Normal),
            new CardInfo( 42, "野符「義満クライシス」",                   StagePractice.Three,        LevelPractice.Hard),
            new CardInfo( 43, "野符「GHQクライシス」",                    StagePractice.Three,        LevelPractice.Lunatic),
            new CardInfo( 44, "国符「三種の神器　剣」",                   StagePractice.Three,        LevelPractice.Easy),
            new CardInfo( 45, "国符「三種の神器　玉」",                   StagePractice.Three,        LevelPractice.Normal),
            new CardInfo( 46, "国符「三種の神器　鏡」",                   StagePractice.Three,        LevelPractice.Hard),
            new CardInfo( 47, "国体「三種の神器　郷」",                   StagePractice.Three,        LevelPractice.Lunatic),
            new CardInfo( 48, "終符「幻想天皇」",                         StagePractice.Three,        LevelPractice.Easy),
            new CardInfo( 49, "終符「幻想天皇」",                         StagePractice.Three,        LevelPractice.Normal),
            new CardInfo( 50, "虚史「幻想郷伝説」",                       StagePractice.Three,        LevelPractice.Hard),
            new CardInfo( 51, "虚史「幻想郷伝説」",                       StagePractice.Three,        LevelPractice.Lunatic),
            new CardInfo( 52, "未来「高天原」",                           StagePractice.Three,        LevelPractice.Normal),
            new CardInfo( 53, "未来「高天原」",                           StagePractice.Three,        LevelPractice.Hard),
            new CardInfo( 54, "未来「高天原」",                           StagePractice.Three,        LevelPractice.Lunatic),
            new CardInfo( 55, "夢符「二重結界」",                         StagePractice.FourUncanny,  LevelPractice.Easy),
            new CardInfo( 56, "夢符「二重結界」",                         StagePractice.FourUncanny,  LevelPractice.Normal),
            new CardInfo( 57, "夢境「二重大結界」",                       StagePractice.FourUncanny,  LevelPractice.Hard),
            new CardInfo( 58, "夢境「二重大結界」",                       StagePractice.FourUncanny,  LevelPractice.Lunatic),
            new CardInfo( 59, "霊符「夢想封印　散」",                     StagePractice.FourUncanny,  LevelPractice.Easy),
            new CardInfo( 60, "霊符「夢想封印　散」",                     StagePractice.FourUncanny,  LevelPractice.Normal),
            new CardInfo( 61, "散霊「夢想封印　寂」",                     StagePractice.FourUncanny,  LevelPractice.Hard),
            new CardInfo( 62, "散霊「夢想封印　寂」",                     StagePractice.FourUncanny,  LevelPractice.Lunatic),
            new CardInfo( 63, "夢符「封魔陣」",                           StagePractice.FourUncanny,  LevelPractice.Easy),
            new CardInfo( 64, "夢符「封魔陣」",                           StagePractice.FourUncanny,  LevelPractice.Normal),
            new CardInfo( 65, "神技「八方鬼縛陣」",                       StagePractice.FourUncanny,  LevelPractice.Hard),
            new CardInfo( 66, "神技「八方龍殺陣」",                       StagePractice.FourUncanny,  LevelPractice.Lunatic),
            new CardInfo( 67, "霊符「夢想封印　集」",                     StagePractice.FourUncanny,  LevelPractice.Easy),
            new CardInfo( 68, "霊符「夢想封印　集」",                     StagePractice.FourUncanny,  LevelPractice.Normal),
            new CardInfo( 69, "回霊「夢想封印　侘」",                     StagePractice.FourUncanny,  LevelPractice.Hard),
            new CardInfo( 70, "回霊「夢想封印　侘」",                     StagePractice.FourUncanny,  LevelPractice.Lunatic),
            new CardInfo( 71, "境界「二重弾幕結界」",                     StagePractice.FourUncanny,  LevelPractice.Easy),
            new CardInfo( 72, "境界「二重弾幕結界」",                     StagePractice.FourUncanny,  LevelPractice.Normal),
            new CardInfo( 73, "大結界「博麗弾幕結界」",                   StagePractice.FourUncanny,  LevelPractice.Hard),
            new CardInfo( 74, "大結界「博麗弾幕結界」",                   StagePractice.FourUncanny,  LevelPractice.Lunatic),
            new CardInfo( 75, "神霊「夢想封印　瞬」",                     StagePractice.FourUncanny,  LevelPractice.Normal),
            new CardInfo( 76, "神霊「夢想封印　瞬」",                     StagePractice.FourUncanny,  LevelPractice.Hard),
            new CardInfo( 77, "神霊「夢想封印　瞬」",                     StagePractice.FourUncanny,  LevelPractice.Lunatic),
            new CardInfo( 78, "魔符「ミルキーウェイ」",                   StagePractice.FourPowerful, LevelPractice.Easy),
            new CardInfo( 79, "魔符「ミルキーウェイ」",                   StagePractice.FourPowerful, LevelPractice.Normal),
            new CardInfo( 80, "魔空「アステロイドベルト」",               StagePractice.FourPowerful, LevelPractice.Hard),
            new CardInfo( 81, "魔空「アステロイドベルト」",               StagePractice.FourPowerful, LevelPractice.Lunatic),
            new CardInfo( 82, "魔符「スターダストレヴァリエ」",           StagePractice.FourPowerful, LevelPractice.Easy),
            new CardInfo( 83, "魔符「スターダストレヴァリエ」",           StagePractice.FourPowerful, LevelPractice.Normal),
            new CardInfo( 84, "黒魔「イベントホライズン」",               StagePractice.FourPowerful, LevelPractice.Hard),
            new CardInfo( 85, "黒魔「イベントホライズン」",               StagePractice.FourPowerful, LevelPractice.Lunatic),
            new CardInfo( 86, "恋符「ノンディレクショナルレーザー」",     StagePractice.FourPowerful, LevelPractice.Easy),
            new CardInfo( 87, "恋符「ノンディレクショナルレーザー」",     StagePractice.FourPowerful, LevelPractice.Normal),
            new CardInfo( 88, "恋風「スターライトタイフーン」",           StagePractice.FourPowerful, LevelPractice.Hard),
            new CardInfo( 89, "恋風「スターライトタイフーン」",           StagePractice.FourPowerful, LevelPractice.Lunatic),
            new CardInfo( 90, "恋符「マスタースパーク」",                 StagePractice.FourPowerful, LevelPractice.Easy),
            new CardInfo( 91, "恋符「マスタースパーク」",                 StagePractice.FourPowerful, LevelPractice.Normal),
            new CardInfo( 92, "恋心「ダブルスパーク」",                   StagePractice.FourPowerful, LevelPractice.Hard),
            new CardInfo( 93, "恋心「ダブルスパーク」",                   StagePractice.FourPowerful, LevelPractice.Lunatic),
            new CardInfo( 94, "光符「アースライトレイ」",                 StagePractice.FourPowerful, LevelPractice.Easy),
            new CardInfo( 95, "光符「アースライトレイ」",                 StagePractice.FourPowerful, LevelPractice.Normal),
            new CardInfo( 96, "光撃「シュート・ザ・ムーン」",             StagePractice.FourPowerful, LevelPractice.Hard),
            new CardInfo( 97, "光撃「シュート・ザ・ムーン」",             StagePractice.FourPowerful, LevelPractice.Lunatic),
            new CardInfo( 98, "魔砲「ファイナルスパーク」",               StagePractice.FourPowerful, LevelPractice.Normal),
            new CardInfo( 99, "魔砲「ファイナルスパーク」",               StagePractice.FourPowerful, LevelPractice.Hard),
            new CardInfo(100, "魔砲「ファイナルマスタースパーク」",       StagePractice.FourPowerful, LevelPractice.Lunatic),
            new CardInfo(101, "波符「赤眼催眠(マインドシェイカー)」",     StagePractice.Five,         LevelPractice.Easy),
            new CardInfo(102, "波符「赤眼催眠(マインドシェイカー)」",     StagePractice.Five,         LevelPractice.Normal),
            new CardInfo(103, "幻波「赤眼催眠(マインドブローイング)」",   StagePractice.Five,         LevelPractice.Hard),
            new CardInfo(104, "幻波「赤眼催眠(マインドブローイング)」",   StagePractice.Five,         LevelPractice.Lunatic),
            new CardInfo(105, "狂符「幻視調律(ビジョナリチューニング)」", StagePractice.Five,         LevelPractice.Easy),
            new CardInfo(106, "狂符「幻視調律(ビジョナリチューニング)」", StagePractice.Five,         LevelPractice.Normal),
            new CardInfo(107, "狂視「狂視調律(イリュージョンシーカー)」", StagePractice.Five,         LevelPractice.Hard),
            new CardInfo(108, "狂視「狂視調律(イリュージョンシーカー)」", StagePractice.Five,         LevelPractice.Lunatic),
            new CardInfo(109, "懶符「生神停止(アイドリングウェーブ)」",   StagePractice.Five,         LevelPractice.Easy),
            new CardInfo(110, "懶符「生神停止(アイドリングウェーブ)」",   StagePractice.Five,         LevelPractice.Normal),
            new CardInfo(111, "懶惰「生神停止(マインドストッパー)」",     StagePractice.Five,         LevelPractice.Hard),
            new CardInfo(112, "懶惰「生神停止(マインドストッパー)」",     StagePractice.Five,         LevelPractice.Lunatic),
            new CardInfo(113, "散符「真実の月(インビジブルフルムーン)」", StagePractice.Five,         LevelPractice.Easy),
            new CardInfo(114, "散符「真実の月(インビジブルフルムーン)」", StagePractice.Five,         LevelPractice.Normal),
            new CardInfo(115, "散符「真実の月(インビジブルフルムーン)」", StagePractice.Five,         LevelPractice.Hard),
            new CardInfo(116, "散符「真実の月(インビジブルフルムーン)」", StagePractice.Five,         LevelPractice.Lunatic),
            new CardInfo(117, "月眼「月兎遠隔催眠術(テレメスメリズム)」", StagePractice.Five,         LevelPractice.Normal),
            new CardInfo(118, "月眼「月兎遠隔催眠術(テレメスメリズム)」", StagePractice.Five,         LevelPractice.Hard),
            new CardInfo(119, "月眼「月兎遠隔催眠術(テレメスメリズム)」", StagePractice.Five,         LevelPractice.Lunatic),
            new CardInfo(120, "天丸「壺中の天地」",                       StagePractice.FinalA,       LevelPractice.Easy),
            new CardInfo(121, "天丸「壺中の天地」",                       StagePractice.FinalA,       LevelPractice.Normal),
            new CardInfo(122, "天丸「壺中の天地」",                       StagePractice.FinalA,       LevelPractice.Hard),
            new CardInfo(123, "天丸「壺中の天地」",                       StagePractice.FinalA,       LevelPractice.Lunatic),
            new CardInfo(124, "覚神「神代の記憶」",                       StagePractice.FinalA,       LevelPractice.Easy),
            new CardInfo(125, "覚神「神代の記憶」",                       StagePractice.FinalA,       LevelPractice.Normal),
            new CardInfo(126, "神符「天人の系譜」",                       StagePractice.FinalA,       LevelPractice.Hard),
            new CardInfo(127, "神符「天人の系譜」",                       StagePractice.FinalA,       LevelPractice.Lunatic),
            new CardInfo(128, "蘇活「生命遊戯　-ライフゲーム-」",         StagePractice.FinalA,       LevelPractice.Easy),
            new CardInfo(129, "蘇活「生命遊戯　-ライフゲーム-」",         StagePractice.FinalA,       LevelPractice.Normal),
            new CardInfo(130, "蘇生「ライジングゲーム」",                 StagePractice.FinalA,       LevelPractice.Hard),
            new CardInfo(131, "蘇生「ライジングゲーム」",                 StagePractice.FinalA,       LevelPractice.Lunatic),
            new CardInfo(132, "操神「オモイカネディバイス」",             StagePractice.FinalA,       LevelPractice.Easy),
            new CardInfo(133, "操神「オモイカネディバイス」",             StagePractice.FinalA,       LevelPractice.Normal),
            new CardInfo(134, "神脳「オモイカネブレイン」",               StagePractice.FinalA,       LevelPractice.Hard),
            new CardInfo(135, "神脳「オモイカネブレイン」",               StagePractice.FinalA,       LevelPractice.Lunatic),
            new CardInfo(136, "天呪「アポロ１３」",                       StagePractice.FinalA,       LevelPractice.Easy),
            new CardInfo(137, "天呪「アポロ１３」",                       StagePractice.FinalA,       LevelPractice.Normal),
            new CardInfo(138, "天呪「アポロ１３」",                       StagePractice.FinalA,       LevelPractice.Hard),
            new CardInfo(139, "天呪「アポロ１３」",                       StagePractice.FinalA,       LevelPractice.Lunatic),
            new CardInfo(140, "秘術「天文密葬法」",                       StagePractice.FinalA,       LevelPractice.Easy),
            new CardInfo(141, "秘術「天文密葬法」",                       StagePractice.FinalA,       LevelPractice.Normal),
            new CardInfo(142, "秘術「天文密葬法」",                       StagePractice.FinalA,       LevelPractice.Hard),
            new CardInfo(143, "秘術「天文密葬法」",                       StagePractice.FinalA,       LevelPractice.Lunatic),
            new CardInfo(144, "禁薬「蓬莱の薬」",                         StagePractice.FinalA,       LevelPractice.Easy),
            new CardInfo(145, "禁薬「蓬莱の薬」",                         StagePractice.FinalA,       LevelPractice.Normal),
            new CardInfo(146, "禁薬「蓬莱の薬」",                         StagePractice.FinalA,       LevelPractice.Hard),
            new CardInfo(147, "禁薬「蓬莱の薬」",                         StagePractice.FinalA,       LevelPractice.Lunatic),
            new CardInfo(148, "薬符「壺中の大銀河」",                     StagePractice.FinalB,       LevelPractice.Easy),
            new CardInfo(149, "薬符「壺中の大銀河」",                     StagePractice.FinalB,       LevelPractice.Normal),
            new CardInfo(150, "薬符「壺中の大銀河」",                     StagePractice.FinalB,       LevelPractice.Hard),
            new CardInfo(151, "薬符「壺中の大銀河」",                     StagePractice.FinalB,       LevelPractice.Lunatic),
            new CardInfo(152, "難題「龍の頸の玉　-五色の弾丸-」",         StagePractice.FinalB,       LevelPractice.Easy),
            new CardInfo(153, "難題「龍の頸の玉　-五色の弾丸-」",         StagePractice.FinalB,       LevelPractice.Normal),
            new CardInfo(154, "神宝「ブリリアントドラゴンバレッタ」",     StagePractice.FinalB,       LevelPractice.Hard),
            new CardInfo(155, "神宝「ブリリアントドラゴンバレッタ」",     StagePractice.FinalB,       LevelPractice.Lunatic),
            new CardInfo(156, "難題「仏の御石の鉢　-砕けぬ意思-」",       StagePractice.FinalB,       LevelPractice.Easy),
            new CardInfo(157, "難題「仏の御石の鉢　-砕けぬ意思-」",       StagePractice.FinalB,       LevelPractice.Normal),
            new CardInfo(158, "神宝「ブディストダイアモンド」",           StagePractice.FinalB,       LevelPractice.Hard),
            new CardInfo(159, "神宝「ブディストダイアモンド」",           StagePractice.FinalB,       LevelPractice.Lunatic),
            new CardInfo(160, "難題「火鼠の皮衣　-焦れぬ心-」",           StagePractice.FinalB,       LevelPractice.Easy),
            new CardInfo(161, "難題「火鼠の皮衣　-焦れぬ心-」",           StagePractice.FinalB,       LevelPractice.Normal),
            new CardInfo(162, "神宝「サラマンダーシールド」",             StagePractice.FinalB,       LevelPractice.Hard),
            new CardInfo(163, "神宝「サラマンダーシールド」",             StagePractice.FinalB,       LevelPractice.Lunatic),
            new CardInfo(164, "難題「燕の子安貝　-永命線-」",             StagePractice.FinalB,       LevelPractice.Easy),
            new CardInfo(165, "難題「燕の子安貝　-永命線-」",             StagePractice.FinalB,       LevelPractice.Normal),
            new CardInfo(166, "神宝「ライフスプリングインフィニティ」",   StagePractice.FinalB,       LevelPractice.Hard),
            new CardInfo(167, "神宝「ライフスプリングインフィニティ」",   StagePractice.FinalB,       LevelPractice.Lunatic),
            new CardInfo(168, "難題「蓬莱の弾の枝　-虹色の弾幕-」",       StagePractice.FinalB,       LevelPractice.Easy),
            new CardInfo(169, "難題「蓬莱の弾の枝　-虹色の弾幕-」",       StagePractice.FinalB,       LevelPractice.Normal),
            new CardInfo(170, "神宝「蓬莱の玉の枝　-夢色の郷-」",         StagePractice.FinalB,       LevelPractice.Hard),
            new CardInfo(171, "神宝「蓬莱の玉の枝　-夢色の郷-」",         StagePractice.FinalB,       LevelPractice.Lunatic),
            new CardInfo(172, "「永夜返し　-初月-」",                     StagePractice.FinalB,       LevelPractice.Easy),
            new CardInfo(173, "「永夜返し　-三日月-」",                   StagePractice.FinalB,       LevelPractice.Normal),
            new CardInfo(174, "「永夜返し　-上つ弓張-」",                 StagePractice.FinalB,       LevelPractice.Hard),
            new CardInfo(175, "「永夜返し　-待宵-」",                     StagePractice.FinalB,       LevelPractice.Lunatic),
            new CardInfo(176, "「永夜返し　-子の刻-」",                   StagePractice.FinalB,       LevelPractice.Easy),
            new CardInfo(177, "「永夜返し　-子の二つ-」",                 StagePractice.FinalB,       LevelPractice.Normal),
            new CardInfo(178, "「永夜返し　-子の三つ-」",                 StagePractice.FinalB,       LevelPractice.Hard),
            new CardInfo(179, "「永夜返し　-子の四つ-」",                 StagePractice.FinalB,       LevelPractice.Lunatic),
            new CardInfo(180, "「永夜返し　-丑の刻-」",                   StagePractice.FinalB,       LevelPractice.Easy),
            new CardInfo(181, "「永夜返し　-丑の二つ-」",                 StagePractice.FinalB,       LevelPractice.Normal),
            new CardInfo(182, "「永夜返し　-丑三つ時-」",                 StagePractice.FinalB,       LevelPractice.Hard),
            new CardInfo(183, "「永夜返し　-丑の四つ-」",                 StagePractice.FinalB,       LevelPractice.Lunatic),
            new CardInfo(184, "「永夜返し　-寅の刻-」",                   StagePractice.FinalB,       LevelPractice.Easy),
            new CardInfo(185, "「永夜返し　-寅の二つ-」",                 StagePractice.FinalB,       LevelPractice.Normal),
            new CardInfo(186, "「永夜返し　-寅の三つ-」",                 StagePractice.FinalB,       LevelPractice.Hard),
            new CardInfo(187, "「永夜返し　-寅の四つ-」",                 StagePractice.FinalB,       LevelPractice.Lunatic),
            new CardInfo(188, "「永夜返し　-朝靄-」",                     StagePractice.FinalB,       LevelPractice.Easy),
            new CardInfo(189, "「永夜返し　-夜明け-」",                   StagePractice.FinalB,       LevelPractice.Normal),
            new CardInfo(190, "「永夜返し　-明けの明星-」",               StagePractice.FinalB,       LevelPractice.Hard),
            new CardInfo(191, "「永夜返し　-世明け-」",                   StagePractice.FinalB,       LevelPractice.Lunatic),
            new CardInfo(192, "旧史「旧秘境史　-オールドヒストリー-」",   StagePractice.Extra,        LevelPractice.Extra),
            new CardInfo(193, "転世「一条戻り橋」",                       StagePractice.Extra,        LevelPractice.Extra),
            new CardInfo(194, "新史「新幻想史　-ネクストヒストリー-」",   StagePractice.Extra,        LevelPractice.Extra),
            new CardInfo(195, "時効「月のいはかさの呪い」",               StagePractice.Extra,        LevelPractice.Extra),
            new CardInfo(196, "不死「火の鳥　-鳳翼天翔-」",               StagePractice.Extra,        LevelPractice.Extra),
            new CardInfo(197, "藤原「滅罪寺院傷」",                       StagePractice.Extra,        LevelPractice.Extra),
            new CardInfo(198, "不死「徐福時空」",                         StagePractice.Extra,        LevelPractice.Extra),
            new CardInfo(199, "滅罪「正直者の死」",                       StagePractice.Extra,        LevelPractice.Extra),
            new CardInfo(200, "虚人「ウー」",                             StagePractice.Extra,        LevelPractice.Extra),
            new CardInfo(201, "不滅「フェニックスの尾」",                 StagePractice.Extra,        LevelPractice.Extra),
            new CardInfo(202, "蓬莱「凱風快晴　-フジヤマヴォルケイノ-」", StagePractice.Extra,        LevelPractice.Extra),
            new CardInfo(203, "「パゼストバイフェニックス」",             StagePractice.Extra,        LevelPractice.Extra),
            new CardInfo(204, "「蓬莱人形」",                             StagePractice.Extra,        LevelPractice.Extra),
            new CardInfo(205, "「インペリシャブルシューティング」",       StagePractice.Extra,        LevelPractice.Extra),
            new CardInfo(206, "「季節外れのバタフライストーム」",         StagePractice.LastWord,     LevelPractice.LastWord),
            new CardInfo(207, "「ブラインドナイトバード」",               StagePractice.LastWord,     LevelPractice.LastWord),
            new CardInfo(208, "「日出づる国の天子」",                     StagePractice.LastWord,     LevelPractice.LastWord),
            new CardInfo(209, "「幻朧月睨(ルナティックレッドアイズ)」",   StagePractice.LastWord,     LevelPractice.LastWord),
            new CardInfo(210, "「天網蜘網捕蝶の法」",                     StagePractice.LastWord,     LevelPractice.LastWord),
            new CardInfo(211, "「蓬莱の樹海」",                           StagePractice.LastWord,     LevelPractice.LastWord),
            new CardInfo(212, "「フェニックス再誕」",                     StagePractice.LastWord,     LevelPractice.LastWord),
            new CardInfo(213, "「エンシェントデューパー」",               StagePractice.LastWord,     LevelPractice.LastWord),
            new CardInfo(214, "「無何有浄化」",                           StagePractice.LastWord,     LevelPractice.LastWord),
            new CardInfo(215, "「夢想天生」",                             StagePractice.LastWord,     LevelPractice.LastWord),
            new CardInfo(216, "「ブレイジングスター」",                   StagePractice.LastWord,     LevelPractice.LastWord),
            new CardInfo(217, "「デフレーションワールド」",               StagePractice.LastWord,     LevelPractice.LastWord),
            new CardInfo(218, "「待宵反射衛星斬」",                       StagePractice.LastWord,     LevelPractice.LastWord),
            new CardInfo(219, "「グランギニョル座の怪人」",               StagePractice.LastWord,     LevelPractice.LastWord),
            new CardInfo(220, "「スカーレットディスティニー」",           StagePractice.LastWord,     LevelPractice.LastWord),
            new CardInfo(221, "「西行寺無余涅槃」",                       StagePractice.LastWord,     LevelPractice.LastWord),
            new CardInfo(222, "「深弾幕結界　-夢幻泡影-」",               StagePractice.LastWord,     LevelPractice.LastWord),
#pragma warning restore SA1008 // Opening parenthesis should be spaced correctly
        }.ToDictionary(card => card.Id);

        public static IReadOnlyList<IHighScore> InitialRanking { get; } =
            Enumerable.Range(1, 10).Reverse().Select(index => new HighScore((uint)index * 10000)).ToList();

        public static string FormatPrefix { get; } = "%T08";
    }
}

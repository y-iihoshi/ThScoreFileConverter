//-----------------------------------------------------------------------
// <copyright file="Th08Converter.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable 1591
#pragma warning disable SA1600 // ElementsMustBeDocumented
#pragma warning disable SA1602 // EnumerationItemsMustBeDocumented

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "*", Justification = "Reviewed.")]

namespace ThScoreFileConverter.Models
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using CardInfo = SpellCardInfo<Th08Converter.StagePractice, Th08Converter.LevelPractice>;

    internal class Th08Converter : ThConverter
    {
        // Thanks to thwiki.info
        [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1025:CodeMustNotContainMultipleWhitespaceInARow", Justification = "Reviewed.")]
        [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1008:OpeningParenthesisMustBeSpacedCorrectly", Justification = "Reviewed.")]
        private static readonly Dictionary<int, CardInfo> CardTable =
            new List<CardInfo>()
            {
                new CardInfo(  1, "蛍符「地上の流星」",                       StagePractice.St1,      LevelPractice.Hard),
                new CardInfo(  2, "蛍符「地上の彗星」",                       StagePractice.St1,      LevelPractice.Lunatic),
                new CardInfo(  3, "灯符「ファイヤフライフェノメノン」",       StagePractice.St1,      LevelPractice.Easy),
                new CardInfo(  4, "灯符「ファイヤフライフェノメノン」",       StagePractice.St1,      LevelPractice.Normal),
                new CardInfo(  5, "灯符「ファイヤフライフェノメノン」",       StagePractice.St1,      LevelPractice.Hard),
                new CardInfo(  6, "灯符「ファイヤフライフェノメノン」",       StagePractice.St1,      LevelPractice.Lunatic),
                new CardInfo(  7, "蠢符「リトルバグ」",                       StagePractice.St1,      LevelPractice.Easy),
                new CardInfo(  8, "蠢符「リトルバグストーム」",               StagePractice.St1,      LevelPractice.Normal),
                new CardInfo(  9, "蠢符「ナイトバグストーム」",               StagePractice.St1,      LevelPractice.Hard),
                new CardInfo( 10, "蠢符「ナイトバグトルネード」",             StagePractice.St1,      LevelPractice.Lunatic),
                new CardInfo( 11, "隠蟲「永夜蟄居」",                         StagePractice.St1,      LevelPractice.Normal),
                new CardInfo( 12, "隠蟲「永夜蟄居」",                         StagePractice.St1,      LevelPractice.Hard),
                new CardInfo( 13, "隠蟲「永夜蟄居」",                         StagePractice.St1,      LevelPractice.Lunatic),
                new CardInfo( 14, "声符「梟の夜鳴声」",                       StagePractice.St2,      LevelPractice.Easy),
                new CardInfo( 15, "声符「梟の夜鳴声」",                       StagePractice.St2,      LevelPractice.Normal),
                new CardInfo( 16, "声符「木菟咆哮」",                         StagePractice.St2,      LevelPractice.Hard),
                new CardInfo( 17, "声符「木菟咆哮」",                         StagePractice.St2,      LevelPractice.Lunatic),
                new CardInfo( 18, "蛾符「天蛾の蠱道」",                       StagePractice.St2,      LevelPractice.Easy),
                new CardInfo( 19, "蛾符「天蛾の蠱道」",                       StagePractice.St2,      LevelPractice.Normal),
                new CardInfo( 20, "毒符「毒蛾の鱗粉」",                       StagePractice.St2,      LevelPractice.Hard),
                new CardInfo( 21, "猛毒「毒蛾の暗闇演舞」",                   StagePractice.St2,      LevelPractice.Lunatic),
                new CardInfo( 22, "鷹符「イルスタードダイブ」",               StagePractice.St2,      LevelPractice.Easy),
                new CardInfo( 23, "鷹符「イルスタードダイブ」",               StagePractice.St2,      LevelPractice.Normal),
                new CardInfo( 24, "鷹符「イルスタードダイブ」",               StagePractice.St2,      LevelPractice.Hard),
                new CardInfo( 25, "鷹符「イルスタードダイブ」",               StagePractice.St2,      LevelPractice.Lunatic),
                new CardInfo( 26, "夜盲「夜雀の歌」",                         StagePractice.St2,      LevelPractice.Easy),
                new CardInfo( 27, "夜盲「夜雀の歌」",                         StagePractice.St2,      LevelPractice.Normal),
                new CardInfo( 28, "夜盲「夜雀の歌」",                         StagePractice.St2,      LevelPractice.Hard),
                new CardInfo( 29, "夜盲「夜雀の歌」",                         StagePractice.St2,      LevelPractice.Lunatic),
                new CardInfo( 30, "夜雀「真夜中のコーラスマスター」",         StagePractice.St2,      LevelPractice.Normal),
                new CardInfo( 31, "夜雀「真夜中のコーラスマスター」",         StagePractice.St2,      LevelPractice.Hard),
                new CardInfo( 32, "夜雀「真夜中のコーラスマスター」",         StagePractice.St2,      LevelPractice.Lunatic),
                new CardInfo( 33, "産霊「ファーストピラミッド」",             StagePractice.St3,      LevelPractice.Easy),
                new CardInfo( 34, "産霊「ファーストピラミッド」",             StagePractice.St3,      LevelPractice.Normal),
                new CardInfo( 35, "産霊「ファーストピラミッド」",             StagePractice.St3,      LevelPractice.Hard),
                new CardInfo( 36, "産霊「ファーストピラミッド」",             StagePractice.St3,      LevelPractice.Lunatic),
                new CardInfo( 37, "始符「エフェメラリティ137」",              StagePractice.St3,      LevelPractice.Normal),
                new CardInfo( 38, "始符「エフェメラリティ137」",              StagePractice.St3,      LevelPractice.Hard),
                new CardInfo( 39, "始符「エフェメラリティ137」",              StagePractice.St3,      LevelPractice.Lunatic),
                new CardInfo( 40, "野符「武烈クライシス」",                   StagePractice.St3,      LevelPractice.Easy),
                new CardInfo( 41, "野符「将門クライシス」",                   StagePractice.St3,      LevelPractice.Normal),
                new CardInfo( 42, "野符「義満クライシス」",                   StagePractice.St3,      LevelPractice.Hard),
                new CardInfo( 43, "野符「GHQクライシス」",                    StagePractice.St3,      LevelPractice.Lunatic),
                new CardInfo( 44, "国符「三種の神器　剣」",                   StagePractice.St3,      LevelPractice.Easy),
                new CardInfo( 45, "国符「三種の神器　玉」",                   StagePractice.St3,      LevelPractice.Normal),
                new CardInfo( 46, "国符「三種の神器　鏡」",                   StagePractice.St3,      LevelPractice.Hard),
                new CardInfo( 47, "国体「三種の神器　郷」",                   StagePractice.St3,      LevelPractice.Lunatic),
                new CardInfo( 48, "終符「幻想天皇」",                         StagePractice.St3,      LevelPractice.Easy),
                new CardInfo( 49, "終符「幻想天皇」",                         StagePractice.St3,      LevelPractice.Normal),
                new CardInfo( 50, "虚史「幻想郷伝説」",                       StagePractice.St3,      LevelPractice.Hard),
                new CardInfo( 51, "虚史「幻想郷伝説」",                       StagePractice.St3,      LevelPractice.Lunatic),
                new CardInfo( 52, "未来「高天原」",                           StagePractice.St3,      LevelPractice.Normal),
                new CardInfo( 53, "未来「高天原」",                           StagePractice.St3,      LevelPractice.Hard),
                new CardInfo( 54, "未来「高天原」",                           StagePractice.St3,      LevelPractice.Lunatic),
                new CardInfo( 55, "夢符「二重結界」",                         StagePractice.St4A,     LevelPractice.Easy),
                new CardInfo( 56, "夢符「二重結界」",                         StagePractice.St4A,     LevelPractice.Normal),
                new CardInfo( 57, "夢境「二重大結界」",                       StagePractice.St4A,     LevelPractice.Hard),
                new CardInfo( 58, "夢境「二重大結界」",                       StagePractice.St4A,     LevelPractice.Lunatic),
                new CardInfo( 59, "霊符「夢想封印　散」",                     StagePractice.St4A,     LevelPractice.Easy),
                new CardInfo( 60, "霊符「夢想封印　散」",                     StagePractice.St4A,     LevelPractice.Normal),
                new CardInfo( 61, "散霊「夢想封印　寂」",                     StagePractice.St4A,     LevelPractice.Hard),
                new CardInfo( 62, "散霊「夢想封印　寂」",                     StagePractice.St4A,     LevelPractice.Lunatic),
                new CardInfo( 63, "夢符「封魔陣」",                           StagePractice.St4A,     LevelPractice.Easy),
                new CardInfo( 64, "夢符「封魔陣」",                           StagePractice.St4A,     LevelPractice.Normal),
                new CardInfo( 65, "神技「八方鬼縛陣」",                       StagePractice.St4A,     LevelPractice.Hard),
                new CardInfo( 66, "神技「八方龍殺陣」",                       StagePractice.St4A,     LevelPractice.Lunatic),
                new CardInfo( 67, "霊符「夢想封印　集」",                     StagePractice.St4A,     LevelPractice.Easy),
                new CardInfo( 68, "霊符「夢想封印　集」",                     StagePractice.St4A,     LevelPractice.Normal),
                new CardInfo( 69, "回霊「夢想封印　侘」",                     StagePractice.St4A,     LevelPractice.Hard),
                new CardInfo( 70, "回霊「夢想封印　侘」",                     StagePractice.St4A,     LevelPractice.Lunatic),
                new CardInfo( 71, "境界「二重弾幕結界」",                     StagePractice.St4A,     LevelPractice.Easy),
                new CardInfo( 72, "境界「二重弾幕結界」",                     StagePractice.St4A,     LevelPractice.Normal),
                new CardInfo( 73, "大結界「博麗弾幕結界」",                   StagePractice.St4A,     LevelPractice.Hard),
                new CardInfo( 74, "大結界「博麗弾幕結界」",                   StagePractice.St4A,     LevelPractice.Lunatic),
                new CardInfo( 75, "神霊「夢想封印　瞬」",                     StagePractice.St4A,     LevelPractice.Normal),
                new CardInfo( 76, "神霊「夢想封印　瞬」",                     StagePractice.St4A,     LevelPractice.Hard),
                new CardInfo( 77, "神霊「夢想封印　瞬」",                     StagePractice.St4A,     LevelPractice.Lunatic),
                new CardInfo( 78, "魔符「ミルキーウェイ」",                   StagePractice.St4B,     LevelPractice.Easy),
                new CardInfo( 79, "魔符「ミルキーウェイ」",                   StagePractice.St4B,     LevelPractice.Normal),
                new CardInfo( 80, "魔空「アステロイドベルト」",               StagePractice.St4B,     LevelPractice.Hard),
                new CardInfo( 81, "魔空「アステロイドベルト」",               StagePractice.St4B,     LevelPractice.Lunatic),
                new CardInfo( 82, "魔符「スターダストレヴァリエ」",           StagePractice.St4B,     LevelPractice.Easy),
                new CardInfo( 83, "魔符「スターダストレヴァリエ」",           StagePractice.St4B,     LevelPractice.Normal),
                new CardInfo( 84, "黒魔「イベントホライズン」",               StagePractice.St4B,     LevelPractice.Hard),
                new CardInfo( 85, "黒魔「イベントホライズン」",               StagePractice.St4B,     LevelPractice.Lunatic),
                new CardInfo( 86, "恋符「ノンディレクショナルレーザー」",     StagePractice.St4B,     LevelPractice.Easy),
                new CardInfo( 87, "恋符「ノンディレクショナルレーザー」",     StagePractice.St4B,     LevelPractice.Normal),
                new CardInfo( 88, "恋風「スターライトタイフーン」",           StagePractice.St4B,     LevelPractice.Hard),
                new CardInfo( 89, "恋風「スターライトタイフーン」",           StagePractice.St4B,     LevelPractice.Lunatic),
                new CardInfo( 90, "恋符「マスタースパーク」",                 StagePractice.St4B,     LevelPractice.Easy),
                new CardInfo( 91, "恋符「マスタースパーク」",                 StagePractice.St4B,     LevelPractice.Normal),
                new CardInfo( 92, "恋心「ダブルスパーク」",                   StagePractice.St4B,     LevelPractice.Hard),
                new CardInfo( 93, "恋心「ダブルスパーク」",                   StagePractice.St4B,     LevelPractice.Lunatic),
                new CardInfo( 94, "光符「アースライトレイ」",                 StagePractice.St4B,     LevelPractice.Easy),
                new CardInfo( 95, "光符「アースライトレイ」",                 StagePractice.St4B,     LevelPractice.Normal),
                new CardInfo( 96, "光撃「シュート・ザ・ムーン」",             StagePractice.St4B,     LevelPractice.Hard),
                new CardInfo( 97, "光撃「シュート・ザ・ムーン」",             StagePractice.St4B,     LevelPractice.Lunatic),
                new CardInfo( 98, "魔砲「ファイナルスパーク」",               StagePractice.St4B,     LevelPractice.Normal),
                new CardInfo( 99, "魔砲「ファイナルスパーク」",               StagePractice.St4B,     LevelPractice.Hard),
                new CardInfo(100, "魔砲「ファイナルマスタースパーク」",       StagePractice.St4B,     LevelPractice.Lunatic),
                new CardInfo(101, "波符「赤眼催眠(マインドシェイカー)」",     StagePractice.St5,      LevelPractice.Easy),
                new CardInfo(102, "波符「赤眼催眠(マインドシェイカー)」",     StagePractice.St5,      LevelPractice.Normal),
                new CardInfo(103, "幻波「赤眼催眠(マインドブローイング)」",   StagePractice.St5,      LevelPractice.Hard),
                new CardInfo(104, "幻波「赤眼催眠(マインドブローイング)」",   StagePractice.St5,      LevelPractice.Lunatic),
                new CardInfo(105, "狂符「幻視調律(ビジョナリチューニング)」", StagePractice.St5,      LevelPractice.Easy),
                new CardInfo(106, "狂符「幻視調律(ビジョナリチューニング)」", StagePractice.St5,      LevelPractice.Normal),
                new CardInfo(107, "狂視「狂視調律(イリュージョンシーカー)」", StagePractice.St5,      LevelPractice.Hard),
                new CardInfo(108, "狂視「狂視調律(イリュージョンシーカー)」", StagePractice.St5,      LevelPractice.Lunatic),
                new CardInfo(109, "懶符「生神停止(アイドリングウェーブ)」",   StagePractice.St5,      LevelPractice.Easy),
                new CardInfo(110, "懶符「生神停止(アイドリングウェーブ)」",   StagePractice.St5,      LevelPractice.Normal),
                new CardInfo(111, "懶惰「生神停止(マインドストッパー)」",     StagePractice.St5,      LevelPractice.Hard),
                new CardInfo(112, "懶惰「生神停止(マインドストッパー)」",     StagePractice.St5,      LevelPractice.Lunatic),
                new CardInfo(113, "散符「真実の月(インビジブルフルムーン)」", StagePractice.St5,      LevelPractice.Easy),
                new CardInfo(114, "散符「真実の月(インビジブルフルムーン)」", StagePractice.St5,      LevelPractice.Normal),
                new CardInfo(115, "散符「真実の月(インビジブルフルムーン)」", StagePractice.St5,      LevelPractice.Hard),
                new CardInfo(116, "散符「真実の月(インビジブルフルムーン)」", StagePractice.St5,      LevelPractice.Lunatic),
                new CardInfo(117, "月眼「月兎遠隔催眠術(テレメスメリズム)」", StagePractice.St5,      LevelPractice.Normal),
                new CardInfo(118, "月眼「月兎遠隔催眠術(テレメスメリズム)」", StagePractice.St5,      LevelPractice.Hard),
                new CardInfo(119, "月眼「月兎遠隔催眠術(テレメスメリズム)」", StagePractice.St5,      LevelPractice.Lunatic),
                new CardInfo(120, "天丸「壺中の天地」",                       StagePractice.St6A,     LevelPractice.Easy),
                new CardInfo(121, "天丸「壺中の天地」",                       StagePractice.St6A,     LevelPractice.Normal),
                new CardInfo(122, "天丸「壺中の天地」",                       StagePractice.St6A,     LevelPractice.Hard),
                new CardInfo(123, "天丸「壺中の天地」",                       StagePractice.St6A,     LevelPractice.Lunatic),
                new CardInfo(124, "覚神「神代の記憶」",                       StagePractice.St6A,     LevelPractice.Easy),
                new CardInfo(125, "覚神「神代の記憶」",                       StagePractice.St6A,     LevelPractice.Normal),
                new CardInfo(126, "神符「天人の系譜」",                       StagePractice.St6A,     LevelPractice.Hard),
                new CardInfo(127, "神符「天人の系譜」",                       StagePractice.St6A,     LevelPractice.Lunatic),
                new CardInfo(128, "蘇活「生命遊戯　-ライフゲーム-」",         StagePractice.St6A,     LevelPractice.Easy),
                new CardInfo(129, "蘇活「生命遊戯　-ライフゲーム-」",         StagePractice.St6A,     LevelPractice.Normal),
                new CardInfo(130, "蘇生「ライジングゲーム」",                 StagePractice.St6A,     LevelPractice.Hard),
                new CardInfo(131, "蘇生「ライジングゲーム」",                 StagePractice.St6A,     LevelPractice.Lunatic),
                new CardInfo(132, "操神「オモイカネディバイス」",             StagePractice.St6A,     LevelPractice.Easy),
                new CardInfo(133, "操神「オモイカネディバイス」",             StagePractice.St6A,     LevelPractice.Normal),
                new CardInfo(134, "神脳「オモイカネブレイン」",               StagePractice.St6A,     LevelPractice.Hard),
                new CardInfo(135, "神脳「オモイカネブレイン」",               StagePractice.St6A,     LevelPractice.Lunatic),
                new CardInfo(136, "天呪「アポロ１３」",                       StagePractice.St6A,     LevelPractice.Easy),
                new CardInfo(137, "天呪「アポロ１３」",                       StagePractice.St6A,     LevelPractice.Normal),
                new CardInfo(138, "天呪「アポロ１３」",                       StagePractice.St6A,     LevelPractice.Hard),
                new CardInfo(139, "天呪「アポロ１３」",                       StagePractice.St6A,     LevelPractice.Lunatic),
                new CardInfo(140, "秘術「天文密葬法」",                       StagePractice.St6A,     LevelPractice.Easy),
                new CardInfo(141, "秘術「天文密葬法」",                       StagePractice.St6A,     LevelPractice.Normal),
                new CardInfo(142, "秘術「天文密葬法」",                       StagePractice.St6A,     LevelPractice.Hard),
                new CardInfo(143, "秘術「天文密葬法」",                       StagePractice.St6A,     LevelPractice.Lunatic),
                new CardInfo(144, "禁薬「蓬莱の薬」",                         StagePractice.St6A,     LevelPractice.Easy),
                new CardInfo(145, "禁薬「蓬莱の薬」",                         StagePractice.St6A,     LevelPractice.Normal),
                new CardInfo(146, "禁薬「蓬莱の薬」",                         StagePractice.St6A,     LevelPractice.Hard),
                new CardInfo(147, "禁薬「蓬莱の薬」",                         StagePractice.St6A,     LevelPractice.Lunatic),
                new CardInfo(148, "薬符「壺中の大銀河」",                     StagePractice.St6B,     LevelPractice.Easy),
                new CardInfo(149, "薬符「壺中の大銀河」",                     StagePractice.St6B,     LevelPractice.Normal),
                new CardInfo(150, "薬符「壺中の大銀河」",                     StagePractice.St6B,     LevelPractice.Hard),
                new CardInfo(151, "薬符「壺中の大銀河」",                     StagePractice.St6B,     LevelPractice.Lunatic),
                new CardInfo(152, "難題「龍の頸の玉　-五色の弾丸-」",         StagePractice.St6B,     LevelPractice.Easy),
                new CardInfo(153, "難題「龍の頸の玉　-五色の弾丸-」",         StagePractice.St6B,     LevelPractice.Normal),
                new CardInfo(154, "神宝「ブリリアントドラゴンバレッタ」",     StagePractice.St6B,     LevelPractice.Hard),
                new CardInfo(155, "神宝「ブリリアントドラゴンバレッタ」",     StagePractice.St6B,     LevelPractice.Lunatic),
                new CardInfo(156, "難題「仏の御石の鉢　-砕けぬ意思-」",       StagePractice.St6B,     LevelPractice.Easy),
                new CardInfo(157, "難題「仏の御石の鉢　-砕けぬ意思-」",       StagePractice.St6B,     LevelPractice.Normal),
                new CardInfo(158, "神宝「ブディストダイアモンド」",           StagePractice.St6B,     LevelPractice.Hard),
                new CardInfo(159, "神宝「ブディストダイアモンド」",           StagePractice.St6B,     LevelPractice.Lunatic),
                new CardInfo(160, "難題「火鼠の皮衣　-焦れぬ心-」",           StagePractice.St6B,     LevelPractice.Easy),
                new CardInfo(161, "難題「火鼠の皮衣　-焦れぬ心-」",           StagePractice.St6B,     LevelPractice.Normal),
                new CardInfo(162, "神宝「サラマンダーシールド」",             StagePractice.St6B,     LevelPractice.Hard),
                new CardInfo(163, "神宝「サラマンダーシールド」",             StagePractice.St6B,     LevelPractice.Lunatic),
                new CardInfo(164, "難題「燕の子安貝　-永命線-」",             StagePractice.St6B,     LevelPractice.Easy),
                new CardInfo(165, "難題「燕の子安貝　-永命線-」",             StagePractice.St6B,     LevelPractice.Normal),
                new CardInfo(166, "神宝「ライフスプリングインフィニティ」",   StagePractice.St6B,     LevelPractice.Hard),
                new CardInfo(167, "神宝「ライフスプリングインフィニティ」",   StagePractice.St6B,     LevelPractice.Lunatic),
                new CardInfo(168, "難題「蓬莱の弾の枝　-虹色の弾幕-」",       StagePractice.St6B,     LevelPractice.Easy),
                new CardInfo(169, "難題「蓬莱の弾の枝　-虹色の弾幕-」",       StagePractice.St6B,     LevelPractice.Normal),
                new CardInfo(170, "神宝「蓬莱の玉の枝　-夢色の郷-」",         StagePractice.St6B,     LevelPractice.Hard),
                new CardInfo(171, "神宝「蓬莱の玉の枝　-夢色の郷-」",         StagePractice.St6B,     LevelPractice.Lunatic),
                new CardInfo(172, "「永夜返し　-初月-」",                     StagePractice.St6B,     LevelPractice.Easy),
                new CardInfo(173, "「永夜返し　-三日月-」",                   StagePractice.St6B,     LevelPractice.Normal),
                new CardInfo(174, "「永夜返し　-上つ弓張-」",                 StagePractice.St6B,     LevelPractice.Hard),
                new CardInfo(175, "「永夜返し　-待宵-」",                     StagePractice.St6B,     LevelPractice.Lunatic),
                new CardInfo(176, "「永夜返し　-子の刻-」",                   StagePractice.St6B,     LevelPractice.Easy),
                new CardInfo(177, "「永夜返し　-子の二つ-」",                 StagePractice.St6B,     LevelPractice.Normal),
                new CardInfo(178, "「永夜返し　-子の三つ-」",                 StagePractice.St6B,     LevelPractice.Hard),
                new CardInfo(179, "「永夜返し　-子の四つ-」",                 StagePractice.St6B,     LevelPractice.Lunatic),
                new CardInfo(180, "「永夜返し　-丑の刻-」",                   StagePractice.St6B,     LevelPractice.Easy),
                new CardInfo(181, "「永夜返し　-丑の二つ-」",                 StagePractice.St6B,     LevelPractice.Normal),
                new CardInfo(182, "「永夜返し　-丑三つ時-」",                 StagePractice.St6B,     LevelPractice.Hard),
                new CardInfo(183, "「永夜返し　-丑の四つ-」",                 StagePractice.St6B,     LevelPractice.Lunatic),
                new CardInfo(184, "「永夜返し　-寅の刻-」",                   StagePractice.St6B,     LevelPractice.Easy),
                new CardInfo(185, "「永夜返し　-寅の二つ-」",                 StagePractice.St6B,     LevelPractice.Normal),
                new CardInfo(186, "「永夜返し　-寅の三つ-」",                 StagePractice.St6B,     LevelPractice.Hard),
                new CardInfo(187, "「永夜返し　-寅の四つ-」",                 StagePractice.St6B,     LevelPractice.Lunatic),
                new CardInfo(188, "「永夜返し　-朝靄-」",                     StagePractice.St6B,     LevelPractice.Easy),
                new CardInfo(189, "「永夜返し　-夜明け-」",                   StagePractice.St6B,     LevelPractice.Normal),
                new CardInfo(190, "「永夜返し　-明けの明星-」",               StagePractice.St6B,     LevelPractice.Hard),
                new CardInfo(191, "「永夜返し　-世明け-」",                   StagePractice.St6B,     LevelPractice.Lunatic),
                new CardInfo(192, "旧史「旧秘境史　-オールドヒストリー-」",   StagePractice.Extra,    LevelPractice.Extra),
                new CardInfo(193, "転世「一条戻り橋」",                       StagePractice.Extra,    LevelPractice.Extra),
                new CardInfo(194, "新史「新幻想史　-ネクストヒストリー-」",   StagePractice.Extra,    LevelPractice.Extra),
                new CardInfo(195, "時効「月のいはかさの呪い」",               StagePractice.Extra,    LevelPractice.Extra),
                new CardInfo(196, "不死「火の鳥　-鳳翼天翔-」",               StagePractice.Extra,    LevelPractice.Extra),
                new CardInfo(197, "藤原「滅罪寺院傷」",                       StagePractice.Extra,    LevelPractice.Extra),
                new CardInfo(198, "不死「徐福時空」",                         StagePractice.Extra,    LevelPractice.Extra),
                new CardInfo(199, "滅罪「正直者の死」",                       StagePractice.Extra,    LevelPractice.Extra),
                new CardInfo(200, "虚人「ウー」",                             StagePractice.Extra,    LevelPractice.Extra),
                new CardInfo(201, "不滅「フェニックスの尾」",                 StagePractice.Extra,    LevelPractice.Extra),
                new CardInfo(202, "蓬莱「凱風快晴　-フジヤマヴォルケイノ-」", StagePractice.Extra,    LevelPractice.Extra),
                new CardInfo(203, "「パゼストバイフェニックス」",             StagePractice.Extra,    LevelPractice.Extra),
                new CardInfo(204, "「蓬莱人形」",                             StagePractice.Extra,    LevelPractice.Extra),
                new CardInfo(205, "「インペリシャブルシューティング」",       StagePractice.Extra,    LevelPractice.Extra),
                new CardInfo(206, "「季節外れのバタフライストーム」",         StagePractice.LastWord, LevelPractice.LastWord),
                new CardInfo(207, "「ブラインドナイトバード」",               StagePractice.LastWord, LevelPractice.LastWord),
                new CardInfo(208, "「日出づる国の天子」",                     StagePractice.LastWord, LevelPractice.LastWord),
                new CardInfo(209, "「幻朧月睨(ルナティックレッドアイズ)」",   StagePractice.LastWord, LevelPractice.LastWord),
                new CardInfo(210, "「天網蜘網捕蝶の法」",                     StagePractice.LastWord, LevelPractice.LastWord),
                new CardInfo(211, "「蓬莱の樹海」",                           StagePractice.LastWord, LevelPractice.LastWord),
                new CardInfo(212, "「フェニックス再誕」",                     StagePractice.LastWord, LevelPractice.LastWord),
                new CardInfo(213, "「エンシェントデューパー」",               StagePractice.LastWord, LevelPractice.LastWord),
                new CardInfo(214, "「無何有浄化」",                           StagePractice.LastWord, LevelPractice.LastWord),
                new CardInfo(215, "「夢想天生」",                             StagePractice.LastWord, LevelPractice.LastWord),
                new CardInfo(216, "「ブレイジングスター」",                   StagePractice.LastWord, LevelPractice.LastWord),
                new CardInfo(217, "「デフレーションワールド」",               StagePractice.LastWord, LevelPractice.LastWord),
                new CardInfo(218, "「待宵反射衛星斬」",                       StagePractice.LastWord, LevelPractice.LastWord),
                new CardInfo(219, "「グランギニョル座の怪人」",               StagePractice.LastWord, LevelPractice.LastWord),
                new CardInfo(220, "「スカーレットディスティニー」",           StagePractice.LastWord, LevelPractice.LastWord),
                new CardInfo(221, "「西行寺無余涅槃」",                       StagePractice.LastWord, LevelPractice.LastWord),
                new CardInfo(222, "「深弾幕結界　-夢幻泡影-」",               StagePractice.LastWord, LevelPractice.LastWord),
            }.ToDictionary(card => card.Id);

        [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1008:OpeningParenthesisMustBeSpacedCorrectly", Justification = "Reviewed.")]
        private static readonly List<HighScore> InitialRanking =
            new List<HighScore>()
            {
                new HighScore(100000),
                new HighScore( 90000),
                new HighScore( 80000),
                new HighScore( 70000),
                new HighScore( 60000),
                new HighScore( 50000),
                new HighScore( 40000),
                new HighScore( 30000),
                new HighScore( 20000),
                new HighScore( 10000),
            };

        private static readonly EnumShortNameParser<LevelPracticeWithTotal> LevelPracticeWithTotalParser =
            new EnumShortNameParser<LevelPracticeWithTotal>();

        private static readonly EnumShortNameParser<Chara> CharaParser =
            new EnumShortNameParser<Chara>();

        private static readonly EnumShortNameParser<CharaWithTotal> CharaWithTotalParser =
            new EnumShortNameParser<CharaWithTotal>();

        private static new readonly EnumShortNameParser<Stage> StageParser =
            new EnumShortNameParser<Stage>();

        private static new readonly EnumShortNameParser<StageWithTotal> StageWithTotalParser =
            new EnumShortNameParser<StageWithTotal>();

        private AllScoreData allScoreData = null;

        public enum LevelPractice
        {
#pragma warning disable SA1134 // Attributes should not share line
            [EnumAltName("E")] Easy,
            [EnumAltName("N")] Normal,
            [EnumAltName("H")] Hard,
            [EnumAltName("L")] Lunatic,
            [EnumAltName("X")] Extra,
            [EnumAltName("W", LongName = "Last Word")] LastWord,
#pragma warning restore SA1134 // Attributes should not share line
        }

        public enum LevelPracticeWithTotal
        {
#pragma warning disable SA1134 // Attributes should not share line
            [EnumAltName("E")] Easy,
            [EnumAltName("N")] Normal,
            [EnumAltName("H")] Hard,
            [EnumAltName("L")] Lunatic,
            [EnumAltName("X")] Extra,
            [EnumAltName("W", LongName = "Last Word")] LastWord,
            [EnumAltName("T")] Total,
#pragma warning restore SA1134 // Attributes should not share line
        }

        public enum Chara
        {
#pragma warning disable SA1134 // Attributes should not share line
            [EnumAltName("RY")] ReimuYukari,
            [EnumAltName("MA")] MarisaAlice,
            [EnumAltName("SR")] SakuyaRemilia,
            [EnumAltName("YY")] YoumuYuyuko,
            [EnumAltName("RM")] Reimu,
            [EnumAltName("YK")] Yukari,
            [EnumAltName("MR")] Marisa,
            [EnumAltName("AL")] Alice,
            [EnumAltName("SK")] Sakuya,
            [EnumAltName("RL")] Remilia,
            [EnumAltName("YM")] Youmu,
            [EnumAltName("YU")] Yuyuko,
#pragma warning restore SA1134 // Attributes should not share line
        }

        public enum CharaWithTotal
        {
#pragma warning disable SA1134 // Attributes should not share line
            [EnumAltName("RY")] ReimuYukari,
            [EnumAltName("MA")] MarisaAlice,
            [EnumAltName("SR")] SakuyaRemilia,
            [EnumAltName("YY")] YoumuYuyuko,
            [EnumAltName("RM")] Reimu,
            [EnumAltName("YK")] Yukari,
            [EnumAltName("MR")] Marisa,
            [EnumAltName("AL")] Alice,
            [EnumAltName("SK")] Sakuya,
            [EnumAltName("RL")] Remilia,
            [EnumAltName("YM")] Youmu,
            [EnumAltName("YU")] Yuyuko,
            [EnumAltName("TL")] Total,
#pragma warning restore SA1134 // Attributes should not share line
        }

        public new enum Stage
        {
#pragma warning disable SA1134 // Attributes should not share line
            [EnumAltName("1A")] St1,
            [EnumAltName("2A")] St2,
            [EnumAltName("3A")] St3,
            [EnumAltName("4A")] St4A,
            [EnumAltName("4B")] St4B,
            [EnumAltName("5A")] St5,
            [EnumAltName("6A")] St6A,
            [EnumAltName("6B")] St6B,
            [EnumAltName("EX")] Extra,
#pragma warning restore SA1134 // Attributes should not share line
        }

        public new enum StageWithTotal
        {
#pragma warning disable SA1134 // Attributes should not share line
            [EnumAltName("1A")] St1,
            [EnumAltName("2A")] St2,
            [EnumAltName("3A")] St3,
            [EnumAltName("4A")] St4A,
            [EnumAltName("4B")] St4B,
            [EnumAltName("5A")] St5,
            [EnumAltName("6A")] St6A,
            [EnumAltName("6B")] St6B,
            [EnumAltName("EX")] Extra,
            [EnumAltName("00")] Total,
#pragma warning restore SA1134 // Attributes should not share line
        }

        public enum StagePractice
        {
#pragma warning disable SA1134 // Attributes should not share line
            [EnumAltName("1A")] St1,
            [EnumAltName("2A")] St2,
            [EnumAltName("3A")] St3,
            [EnumAltName("4A")] St4A,
            [EnumAltName("4B")] St4B,
            [EnumAltName("5A")] St5,
            [EnumAltName("6A")] St6A,
            [EnumAltName("6B")] St6B,
            [EnumAltName("EX")] Extra,
            [EnumAltName("LW", LongName = "Last Word")] LastWord,
#pragma warning restore SA1134 // Attributes should not share line
        }

        [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1025:CodeMustNotContainMultipleWhitespaceInARow", Justification = "Reviewed.")]
        public enum StageProgress
        {
#pragma warning disable SA1134 // Attributes should not share line
            [EnumAltName("Stage 1")]          St1,
            [EnumAltName("Stage 2")]          St2,
            [EnumAltName("Stage 3")]          St3,
            [EnumAltName("Stage 4-uncanny")]  St4A,
            [EnumAltName("Stage 4-powerful")] St4B,
            [EnumAltName("Stage 5")]          St5,
            [EnumAltName("Stage 6-Eirin")]    St6A,
            [EnumAltName("Stage 6-Kaguya")]   St6B,
            [EnumAltName("Extra Stage")]      Extra,
            [EnumAltName("All Clear")]        Clear = 99,
#pragma warning restore SA1134 // Attributes should not share line
        }

        [Flags]
        private enum PlayableStages
        {
            Stage1   = 0x0001,
            Stage2   = 0x0002,
            Stage3   = 0x0004,
            Stage4A  = 0x0008,
            Stage4B  = 0x0010,
            Stage5   = 0x0020,
            Stage6A  = 0x0040,
            Stage6B  = 0x0080,
            Extra    = 0x0100,
            Unknown  = 0x4000,
            AllClear = 0x8000,
        }

        public override string SupportedVersions
        {
            get { return "1.00d"; }
        }

        protected override bool ReadScoreFile(Stream input)
        {
            using (var decrypted = new MemoryStream())
#if DEBUG
            using (var decoded = new FileStream("th08decoded.dat", FileMode.Create, FileAccess.ReadWrite))
#else
            using (var decoded = new MemoryStream())
#endif
            {
                if (!Decrypt(input, decrypted))
                    return false;

                decrypted.Seek(0, SeekOrigin.Begin);
                if (!Extract(decrypted, decoded))
                    return false;

                decoded.Seek(0, SeekOrigin.Begin);
                if (!Validate(decoded))
                    return false;

                decoded.Seek(0, SeekOrigin.Begin);
                this.allScoreData = Read(decoded);

                return this.allScoreData != null;
            }
        }

        protected override IEnumerable<IStringReplaceable> CreateReplacers(
            bool hideUntriedCards, string outputFilePath)
        {
            return new List<IStringReplaceable>
            {
                new ScoreReplacer(this),
                new CareerReplacer(this),
                new CardReplacer(this, hideUntriedCards),
                new CollectRateReplacer(this),
                new ClearReplacer(this),
                new PlayReplacer(this),
                new TimeReplacer(this),
                new PracticeReplacer(this),
            };
        }

        private static bool Decrypt(Stream input, Stream output)
        {
            var size = (int)input.Length;
            ThCrypt.Decrypt(input, output, size, 0x59, 0x79, 0x0100, 0x0C00);

            var data = new byte[size];
            output.Seek(0, SeekOrigin.Begin);
            output.Read(data, 0, size);

            uint checksum = 0;
            byte temp = 0;
            for (var index = 2; index < size; index++)
            {
                temp += data[index - 1];
                temp = (byte)((temp >> 5) | (temp << 3));
                data[index] ^= temp;
                if (index > 3)
                    checksum += data[index];
            }

            output.Seek(0, SeekOrigin.Begin);
            output.Write(data, 0, size);

            return (ushort)checksum == BitConverter.ToUInt16(data, 2);
        }

        private static bool Extract(Stream input, Stream output)
        {
            using (var reader = new BinaryReader(input, Encoding.UTF8, true))
            using (var writer = new BinaryWriter(output, Encoding.UTF8, true))
            {
                var header = new FileHeader();

                header.ReadFrom(reader);
                if (!header.IsValid)
                    return false;
                if (header.Size + header.EncodedBodySize != input.Length)
                    return false;

                header.WriteTo(writer);

                Lzss.Extract(input, output);
                output.Flush();
                output.SetLength(output.Position);

                return output.Position == header.DecodedAllSize;
            }
        }

        private static bool Validate(Stream input)
        {
            using (var reader = new BinaryReader(input, Encoding.UTF8, true))
            {
                var header = new FileHeader();
                var chapter = new Th06.Chapter();

                header.ReadFrom(reader);
                var remainSize = header.DecodedAllSize - header.Size;
                if (remainSize <= 0)
                    return false;

                try
                {
                    while (remainSize > 0)
                    {
                        chapter.ReadFrom(reader);
                        if (chapter.Size1 == 0)
                            return false;

                        switch (chapter.Signature)
                        {
                            case Header.ValidSignature:
                                if (chapter.FirstByteOfData != 0x01)
                                    return false;
                                break;
                            case Th07.VersionInfo.ValidSignature:
                                if (chapter.FirstByteOfData != 0x01)
                                    return false;
                                //// th08.exe does something more things here...
                                break;
                            default:
                                break;
                        }

                        remainSize -= chapter.Size1;
                    }
                }
                catch (EndOfStreamException)
                {
                    // It's OK, do nothing.
                }

                return remainSize == 0;
            }
        }

        [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1025:CodeMustNotContainMultipleWhitespaceInARow", Justification = "Reviewed.")]
        private static AllScoreData Read(Stream input)
        {
            var dictionary = new Dictionary<string, Action<AllScoreData, Th06.Chapter>>
            {
                { Header.ValidSignature,           (data, ch) => data.Set(new Header(ch))           },
                { HighScore.ValidSignature,        (data, ch) => data.Set(new HighScore(ch))        },
                { ClearData.ValidSignature,        (data, ch) => data.Set(new ClearData(ch))        },
                { CardAttack.ValidSignature,       (data, ch) => data.Set(new CardAttack(ch))       },
                { PracticeScore.ValidSignature,    (data, ch) => data.Set(new PracticeScore(ch))    },
                { FLSP.ValidSignature,             (data, ch) => data.Set(new FLSP(ch))             },
                { PlayStatus.ValidSignature,       (data, ch) => data.Set(new PlayStatus(ch))       },
                { Th07.LastName.ValidSignature,    (data, ch) => data.Set(new Th07.LastName(ch))    },
                { Th07.VersionInfo.ValidSignature, (data, ch) => data.Set(new Th07.VersionInfo(ch)) },
            };

            using (var reader = new BinaryReader(input, Encoding.UTF8, true))
            {
                var allScoreData = new AllScoreData();
                var chapter = new Th06.Chapter();

                reader.ReadExactBytes(FileHeader.ValidSize);

                try
                {
                    while (true)
                    {
                        chapter.ReadFrom(reader);
                        if (dictionary.TryGetValue(chapter.Signature, out Action<AllScoreData, Th06.Chapter> setChapter))
                            setChapter(allScoreData, chapter);
                    }
                }
                catch (EndOfStreamException)
                {
                    // It's OK, do nothing.
                }

                if ((allScoreData.Header != null) &&
                    //// (allScoreData.rankings.Count >= 0) &&
                    (allScoreData.ClearData.Count == Enum.GetValues(typeof(CharaWithTotal)).Length) &&
                    //// (allScoreData.cardAttacks.Length == NumCards) &&
                    //// (allScoreData.practiceScores.Count >= 0) &&
                    (allScoreData.Flsp != null) &&
                    (allScoreData.PlayStatus != null) &&
                    (allScoreData.LastName != null) &&
                    (allScoreData.VersionInfo != null))
                    return allScoreData;
                else
                    return null;
            }
        }

        // %T08SCR[w][xx][y][z]
        private class ScoreReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T08SCR({0})({1})(\d)([\dA-G])", LevelParser.Pattern, CharaParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public ScoreReplacer(Th08Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var level = LevelParser.Parse(match.Groups[1].Value);
                    var chara = CharaParser.Parse(match.Groups[2].Value);
                    var rank = Utils.ToZeroBased(
                        int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture));
                    var type = match.Groups[4].Value.ToUpperInvariant();

                    var key = new CharaLevelPair(chara, level);
                    var score = parent.allScoreData.Rankings.ContainsKey(key)
                        ? parent.allScoreData.Rankings[key][rank] : InitialRanking[rank];
                    IEnumerable<string> cardStrings;

                    switch (type)
                    {
                        case "1":   // name
                            return Encoding.Default.GetString(score.Name).Split('\0')[0];
                        case "2":   // score
                            return Utils.ToNumberString((score.Score * 10) + score.ContinueCount);
                        case "3":   // stage
                            if ((level == Level.Extra) &&
                                (Encoding.Default.GetString(score.Date).TrimEnd('\0') == "--/--"))
                                return StageProgress.Extra.ToShortName();
                            else
                                return score.StageProgress.ToShortName();
                        case "4":   // date
                            return Encoding.Default.GetString(score.Date).TrimEnd('\0');
                        case "5":   // slow rate
                            return Utils.Format("{0:F3}%", score.SlowRate);
                        case "6":   // play time
                            return new Time(score.PlayTime).ToString();
                        case "7":   // initial number of players
                            return (score.PlayerNum + 1).ToString(CultureInfo.CurrentCulture);
                        case "8":   // point items
                            return Utils.ToNumberString(score.PointItem);
                        case "9":   // time point
                            return Utils.ToNumberString(score.TimePoint);
                        case "0":   // miss count
                            return score.MissCount.ToString(CultureInfo.CurrentCulture);
                        case "A":   // bomb count
                            return score.BombCount.ToString(CultureInfo.CurrentCulture);
                        case "B":   // last spell count
                            return score.LastSpellCount.ToString(CultureInfo.CurrentCulture);
                        case "C":   // pause count
                            return Utils.ToNumberString(score.PauseCount);
                        case "D":   // continue count
                            return score.ContinueCount.ToString(CultureInfo.CurrentCulture);
                        case "E":   // human rate
                            return Utils.Format("{0:F2}%", score.HumanRate / 100.0);
                        case "F":   // got spell cards
                            cardStrings = score.CardFlags
                                .Where(pair => pair.Value > 0)
                                .Select(pair =>
                                {
                                    return CardTable.TryGetValue(pair.Key, out CardInfo card)
                                        ? Utils.Format("No.{0:D3} {1}", card.Id, card.Name) : string.Empty;
                                });
                            return string.Join(Environment.NewLine, cardStrings.ToArray());
                        case "G":   // number of got spell cards
                            return score.CardFlags.Values.Count(flag => flag > 0)
                                .ToString(CultureInfo.CurrentCulture);
                        default:    // unreachable
                            return match.ToString();
                    }
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        // %T08C[w][xxx][yy][z]
        private class CareerReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T08C([SP])(\d{{3}})({0})([1-3])", CharaWithTotalParser.Pattern);

            private readonly MatchEvaluator evaluator;

            [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
            public CareerReplacer(Th08Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var kind = match.Groups[1].Value.ToUpperInvariant();
                    var number = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);
                    var chara = CharaWithTotalParser.Parse(match.Groups[3].Value);
                    var type = int.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture);

                    Func<CardAttack, bool> isValidLevel;
                    Func<CardAttack, CardAttackCareer> getCareer;
                    if (kind == "S")
                    {
                        isValidLevel = (attack => CardTable[attack.CardId].Level != LevelPractice.LastWord);
                        getCareer = (attack => attack.StoryCareer);
                    }
                    else
                    {
                        isValidLevel = (attack => true);
                        getCareer = (attack => attack.PracticeCareer);
                    }

                    Func<CardAttack, long> getValue;
                    if (type == 1)
                        getValue = (attack => getCareer(attack).MaxBonuses[chara]);
                    else if (type == 2)
                        getValue = (attack => getCareer(attack).ClearCounts[chara]);
                    else
                        getValue = (attack => getCareer(attack).TrialCounts[chara]);

                    if (number == 0)
                    {
                        return Utils.ToNumberString(
                            parent.allScoreData.CardAttacks.Values.Where(isValidLevel).Sum(getValue));
                    }
                    else if (CardTable.ContainsKey(number))
                    {
                        if (parent.allScoreData.CardAttacks.TryGetValue(number, out CardAttack attack))
                        {
                            return isValidLevel(attack)
                                ? Utils.ToNumberString(getValue(attack)) : match.ToString();
                        }
                        else
                        {
                            return "0";
                        }
                    }
                    else
                    {
                        return match.ToString();
                    }
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        // %T08CARD[xxx][y]
        private class CardReplacer : IStringReplaceable
        {
            private const string Pattern = @"%T08CARD(\d{3})([NR])";

            private readonly MatchEvaluator evaluator;

            public CardReplacer(Th08Converter parent, bool hideUntriedCards)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var number = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
                    var type = match.Groups[2].Value.ToUpperInvariant();

                    if (CardTable.ContainsKey(number))
                    {
                        if (hideUntriedCards)
                        {
                            if (!parent.allScoreData.CardAttacks.TryGetValue(number, out CardAttack attack) ||
                                !attack.HasTried())
                                return (type == "N") ? "??????????" : "?????";
                        }

                        if (type == "N")
                        {
                            return CardTable[number].Name;
                        }
                        else
                        {
                            var level = CardTable[number].Level;
                            var levelName = level.ToLongName();
                            return (levelName.Length > 0) ? levelName : level.ToString();
                        }
                    }
                    else
                    {
                        return match.ToString();
                    }
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        // %T08CRG[v][w][xx][yy][z]
        private class CollectRateReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T08CRG([SP])({0})({1})({2})([12])",
                LevelPracticeWithTotalParser.Pattern,
                CharaWithTotalParser.Pattern,
                StageWithTotalParser.Pattern);

            [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
            private static readonly Func<CardAttack, CharaWithTotal, string, int, bool> FindByKindTypeImpl =
                (attack, chara, kind, type) =>
                {
                    Func<CardAttackCareer, int> getCount;
                    if (type == 1)
                        getCount = (career => career.ClearCounts[chara]);
                    else
                        getCount = (career => career.TrialCounts[chara]);

                    if (kind == "S")
                    {
                        return (CardTable[attack.CardId].Level != LevelPractice.LastWord)
                            && (getCount(attack.StoryCareer) > 0);
                    }
                    else
                    {
                        return getCount(attack.PracticeCareer) > 0;
                    }
                };

            private readonly MatchEvaluator evaluator;

            [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
            public CollectRateReplacer(Th08Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var kind = match.Groups[1].Value.ToUpperInvariant();
                    var level = LevelPracticeWithTotalParser.Parse(match.Groups[2].Value);
                    var chara = CharaWithTotalParser.Parse(match.Groups[3].Value);
                    var stage = StageWithTotalParser.Parse(match.Groups[4].Value);
                    var type = int.Parse(match.Groups[5].Value, CultureInfo.InvariantCulture);

                    if (stage == StageWithTotal.Extra)
                        return match.ToString();
                    if ((kind == "S") && (level == LevelPracticeWithTotal.LastWord))
                        return match.ToString();

                    Func<CardAttack, bool> findByKindType =
                        (attack => FindByKindTypeImpl(attack, chara, kind, type));

                    Func<CardAttack, bool> findByStage;
                    if (stage == StageWithTotal.Total)
                        findByStage = (attack => true);
                    else
                        findByStage = (attack => CardTable[attack.CardId].Stage == (StagePractice)stage);

                    Func<CardAttack, bool> findByLevel = (attack => true);
                    switch (level)
                    {
                        case LevelPracticeWithTotal.Total:
                            // Do nothing
                            break;
                        case LevelPracticeWithTotal.Extra:
                            findByStage =
                                (attack => CardTable[attack.CardId].Stage == StagePractice.Extra);
                            break;
                        case LevelPracticeWithTotal.LastWord:
                            findByStage =
                                (attack => CardTable[attack.CardId].Stage == StagePractice.LastWord);
                            break;
                        default:
                            findByLevel = (attack => attack.Level == level);
                            break;
                    }

                    return parent.allScoreData.CardAttacks.Values
                        .Count(Utils.MakeAndPredicate(findByKindType, findByLevel, findByStage))
                        .ToString(CultureInfo.CurrentCulture);
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        // %T08CLEAR[x][yy]
        private class ClearReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T08CLEAR({0})({1})", LevelParser.Pattern, CharaParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public ClearReplacer(Th08Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var level = LevelParser.Parse(match.Groups[1].Value);
                    var chara = CharaParser.Parse(match.Groups[2].Value);

                    var key = new CharaLevelPair(chara, level);
                    if (parent.allScoreData.Rankings.ContainsKey(key))
                    {
                        var stageProgress =
                            parent.allScoreData.Rankings[key].Max(rank => rank.StageProgress);
                        if ((stageProgress == StageProgress.St4A) || (stageProgress == StageProgress.St4B))
                        {
                            return "Stage 4";
                        }
                        else if (stageProgress == StageProgress.Extra)
                        {
                            return "Not Clear";
                        }
                        else if (stageProgress == StageProgress.Clear)
                        {
                            if ((level != Level.Extra) &&
                                ((parent.allScoreData.ClearData[(CharaWithTotal)chara].StoryFlags[level]
                                    & PlayableStages.Stage6B) != PlayableStages.Stage6B))
                                return "FinalA Clear";
                            else
                                return stageProgress.ToShortName();
                        }
                        else
                        {
                            return stageProgress.ToShortName();
                        }
                    }
                    else
                    {
                        return "-------";
                    }
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        // %T08PLAY[x][yy]
        private class PlayReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T08PLAY({0})({1}|CL|CN|PR)", LevelWithTotalParser.Pattern, CharaWithTotalParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public PlayReplacer(Th08Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var level = LevelWithTotalParser.Parse(match.Groups[1].Value);
                    var charaAndMore = match.Groups[2].Value.ToUpperInvariant();

                    var playCount = (level == LevelWithTotal.Total)
                        ? parent.allScoreData.PlayStatus.TotalPlayCount
                        : parent.allScoreData.PlayStatus.PlayCounts[(Level)level];

                    switch (charaAndMore)
                    {
                        case "CL":  // clear count
                            return Utils.ToNumberString(playCount.TotalClear);
                        case "CN":  // continue count
                            return Utils.ToNumberString(playCount.TotalContinue);
                        case "PR":  // practice count
                            return Utils.ToNumberString(playCount.TotalPractice);
                        default:
                            {
                                var chara = CharaWithTotalParser.Parse(match.Groups[2].Value);
                                return Utils.ToNumberString((chara == CharaWithTotal.Total)
                                    ? playCount.TotalTrial : playCount.Trials[(Chara)chara]);
                            }
                    }
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        // %T08TIME(ALL|PLY)
        private class TimeReplacer : IStringReplaceable
        {
            private const string Pattern = @"%T08TIME(ALL|PLY)";

            private readonly MatchEvaluator evaluator;

            public TimeReplacer(Th08Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var kind = match.Groups[1].Value.ToUpperInvariant();

                    return (kind == "ALL")
                        ? parent.allScoreData.PlayStatus.TotalRunningTime.ToLongString()
                        : parent.allScoreData.PlayStatus.TotalPlayTime.ToLongString();
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        // %T08PRAC[w][xx][yy][z]
        private class PracticeReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T08PRAC({0})({1})({2})([12])",
                LevelParser.Pattern,
                CharaParser.Pattern,
                StageParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public PracticeReplacer(Th08Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var level = LevelParser.Parse(match.Groups[1].Value);
                    var chara = CharaParser.Parse(match.Groups[2].Value);
                    var stage = StageParser.Parse(match.Groups[3].Value);
                    var type = int.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture);

                    if (level == Level.Extra)
                        return match.ToString();
                    if (stage == Stage.Extra)
                        return match.ToString();

                    if (parent.allScoreData.PracticeScores.ContainsKey(chara))
                    {
                        var scores = parent.allScoreData.PracticeScores[chara];
                        var key = new StageLevelPair(stage, level);
                        if (type == 1)
                        {
                            return scores.HighScores.ContainsKey(key)
                                ? Utils.ToNumberString(scores.HighScores[key] * 10) : "0";
                        }
                        else
                        {
                            return scores.PlayCounts.ContainsKey(key)
                                ? Utils.ToNumberString(scores.PlayCounts[key]) : "0";
                        }
                    }
                    else
                    {
                        return "0";
                    }
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        private class CharaLevelPair : Pair<Chara, Level>
        {
            public CharaLevelPair(Chara chara, Level level)
                : base(chara, level)
            {
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public Chara Chara
            {
                get { return this.First; }
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public Level Level
            {
                get { return this.Second; }
            }
        }

        private class StageLevelPair : Pair<Stage, Level>
        {
            public StageLevelPair(Stage stage, Level level)
                : base(stage, level)
            {
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public Stage Stage
            {
                get { return this.First; }
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public Level Level
            {
                get { return this.Second; }
            }
        }

        private class FileHeader : IBinaryReadable, IBinaryWritable
        {
            public const short ValidVersion = 0x0001;
            public const int ValidSize = 0x0000001C;

            private ushort unknown1;
            private ushort unknown2;
            private uint unknown3;

            public FileHeader()
            {
            }

            public ushort Checksum { get; private set; }

            public short Version { get; private set; }

            public int Size { get; private set; }

            public int DecodedAllSize { get; private set; }

            public int DecodedBodySize { get; private set; }

            public int EncodedBodySize { get; private set; }

            public bool IsValid
            {
                get
                {
                    return (this.Version == ValidVersion)
                        && (this.Size == ValidSize)
                        && (this.DecodedAllSize == this.Size + this.DecodedBodySize);
                }
            }

            public void ReadFrom(BinaryReader reader)
            {
                if (reader == null)
                    throw new ArgumentNullException(nameof(reader));

                this.unknown1 = reader.ReadUInt16();
                this.Checksum = reader.ReadUInt16();
                this.Version = reader.ReadInt16();
                this.unknown2 = reader.ReadUInt16();
                this.Size = reader.ReadInt32();
                this.unknown3 = reader.ReadUInt32();
                this.DecodedAllSize = reader.ReadInt32();
                this.DecodedBodySize = reader.ReadInt32();
                this.EncodedBodySize = reader.ReadInt32();
            }

            public void WriteTo(BinaryWriter writer)
            {
                if (writer == null)
                    throw new ArgumentNullException(nameof(writer));

                writer.Write(this.unknown1);
                writer.Write(this.Checksum);
                writer.Write(this.Version);
                writer.Write(this.unknown2);
                writer.Write(this.Size);
                writer.Write(this.unknown3);
                writer.Write(this.DecodedAllSize);
                writer.Write(this.DecodedBodySize);
                writer.Write(this.EncodedBodySize);
            }
        }

        private class AllScoreData
        {
            public AllScoreData()
            {
                var numCharas = Enum.GetValues(typeof(Chara)).Length;
                var numPairs = numCharas * Enum.GetValues(typeof(Level)).Length;
                this.Rankings = new Dictionary<CharaLevelPair, List<HighScore>>(numPairs);
                this.ClearData =
                    new Dictionary<CharaWithTotal, ClearData>(Enum.GetValues(typeof(CharaWithTotal)).Length);
                this.CardAttacks = new Dictionary<int, CardAttack>(CardTable.Count);
                this.PracticeScores = new Dictionary<Chara, PracticeScore>(numCharas);
            }

            public Header Header { get; private set; }

            public Dictionary<CharaLevelPair, List<HighScore>> Rankings { get; private set; }

            public Dictionary<CharaWithTotal, ClearData> ClearData { get; private set; }

            public Dictionary<int, CardAttack> CardAttacks { get; private set; }

            public Dictionary<Chara, PracticeScore> PracticeScores { get; private set; }

            public FLSP Flsp { get; private set; }

            public PlayStatus PlayStatus { get; private set; }

            public Th07.LastName LastName { get; private set; }

            public Th07.VersionInfo VersionInfo { get; private set; }

            public void Set(Header header)
            {
                this.Header = header;
            }

            public void Set(HighScore score)
            {
                var key = new CharaLevelPair(score.Chara, score.Level);
                if (!this.Rankings.ContainsKey(key))
                    this.Rankings.Add(key, new List<HighScore>(InitialRanking));
                var ranking = this.Rankings[key];
                ranking.Add(score);
                ranking.Sort((lhs, rhs) => rhs.Score.CompareTo(lhs.Score));
                ranking.RemoveAt(ranking.Count - 1);
            }

            public void Set(ClearData data)
            {
                if (!this.ClearData.ContainsKey(data.Chara))
                    this.ClearData.Add(data.Chara, data);
            }

            public void Set(CardAttack attack)
            {
                if (!this.CardAttacks.ContainsKey(attack.CardId))
                    this.CardAttacks.Add(attack.CardId, attack);
            }

            public void Set(PracticeScore score)
            {
                if (!this.PracticeScores.ContainsKey(score.Chara))
                    this.PracticeScores.Add(score.Chara, score);
            }

            public void Set(FLSP flsp)
            {
                this.Flsp = flsp;
            }

            public void Set(PlayStatus status)
            {
                this.PlayStatus = status;
            }

            public void Set(Th07.LastName name)
            {
                this.LastName = name;
            }

            public void Set(Th07.VersionInfo info)
            {
                this.VersionInfo = info;
            }
        }

        private class Header : Th06.Chapter
        {
            public const string ValidSignature = "TH8K";
            public const short ValidSize = 0x000C;

            public Header(Th06.Chapter chapter)
                : base(chapter)
            {
                if (!this.Signature.Equals(ValidSignature, StringComparison.Ordinal))
                    throw new InvalidDataException("Signature");
                if (this.Size1 != ValidSize)
                    throw new InvalidDataException("Size1");

                using (var reader = new BinaryReader(new MemoryStream(this.Data, false)))
                {
                    reader.ReadUInt32();    // always 0x00000001?
                }
            }
        }

        private class HighScore : Th06.Chapter   // per character, level, rank
        {
            public const string ValidSignature = "HSCR";
            public const short ValidSize = 0x0168;

            public HighScore(Th06.Chapter chapter)
                : base(chapter)
            {
                if (!this.Signature.Equals(ValidSignature, StringComparison.Ordinal))
                    throw new InvalidDataException("Signature");
                if (this.Size1 != ValidSize)
                    throw new InvalidDataException("Size1");

                this.CardFlags = new Dictionary<int, byte>();

                using (var reader = new BinaryReader(new MemoryStream(this.Data, false)))
                {
                    reader.ReadUInt32();    // always 0x00000004?
                    this.Score = reader.ReadUInt32();
                    this.SlowRate = reader.ReadSingle();
                    this.Chara = Utils.ToEnum<Chara>(reader.ReadByte());
                    this.Level = Utils.ToEnum<Level>(reader.ReadByte());
                    this.StageProgress = Utils.ToEnum<StageProgress>(reader.ReadByte());
                    this.Name = reader.ReadExactBytes(9);
                    this.Date = reader.ReadExactBytes(6);
                    this.ContinueCount = reader.ReadUInt16();

                    // 01 00 00 00 04 00 09 00 FF FF FF FF FF FF FF FF
                    // 05 00 00 00 01 00 08 00 58 02 58 02
                    reader.ReadExactBytes(0x1C);

                    this.PlayerNum = reader.ReadByte();

                    // NN 03 00 01 01 LL 01 00 02 00 00 ** ** 00 00 00
                    // 00 00 00 00 00 00 00 00 00 00 00 00 01 40 00 00
                    // where NN: PlayerNum, LL: level, **: unknown (0x64 or 0x0A; 0x50 or 0x0A)
                    reader.ReadExactBytes(0x1F);

                    this.PlayTime = reader.ReadUInt32();
                    this.PointItem = reader.ReadInt32();
                    reader.ReadUInt32();    // always 0x00000000?
                    this.MissCount = reader.ReadInt32();
                    this.BombCount = reader.ReadInt32();
                    this.LastSpellCount = reader.ReadInt32();
                    this.PauseCount = reader.ReadInt32();
                    this.TimePoint = reader.ReadInt32();
                    this.HumanRate = reader.ReadInt32();
                    foreach (var key in CardTable.Keys)
                        this.CardFlags.Add(key, reader.ReadByte());
                    reader.ReadExactBytes(2);
                }
            }

            public HighScore(uint score)    // for InitialRanking only
                : base()
            {
                this.Score = score;
                this.Name = Encoding.Default.GetBytes("--------\0");
                this.Date = Encoding.Default.GetBytes("--/--\0");
                this.CardFlags = new Dictionary<int, byte>();
            }

            public uint Score { get; private set; }                     // * 10

            public float SlowRate { get; private set; }

            public Chara Chara { get; private set; }                    // size: 1Byte

            public Level Level { get; private set; }                    // size: 1Byte

            public StageProgress StageProgress { get; private set; }    // size: 1Byte

            public byte[] Name { get; private set; }                    // .Length = 9, null-terminated

            public byte[] Date { get; private set; }                    // .Length = 6, "mm/dd\0"

            public ushort ContinueCount { get; private set; }

            public byte PlayerNum { get; private set; }                 // 0-based

            public uint PlayTime { get; private set; }                  // = seconds * 60fps

            public int PointItem { get; private set; }

            public int MissCount { get; private set; }

            public int BombCount { get; private set; }

            public int LastSpellCount { get; private set; }

            public int PauseCount { get; private set; }

            public int TimePoint { get; private set; }

            public int HumanRate { get; private set; }                  // / 100

            public Dictionary<int, byte> CardFlags { get; private set; }
        }

        private class ClearData : Th06.Chapter   // per character-with-total
        {
            public const string ValidSignature = "CLRD";
            public const short ValidSize = 0x0024;

            public ClearData(Th06.Chapter chapter)
                : base(chapter)
            {
                if (!this.Signature.Equals(ValidSignature, StringComparison.Ordinal))
                    throw new InvalidDataException("Signature");
                if (this.Size1 != ValidSize)
                    throw new InvalidDataException("Size1");

                var levels = Utils.GetEnumerator<Level>();
                var numLevels = levels.Count();
                this.StoryFlags = new Dictionary<Level, PlayableStages>(numLevels);
                this.PracticeFlags = new Dictionary<Level, PlayableStages>(numLevels);

                using (var reader = new BinaryReader(new MemoryStream(this.Data, false)))
                {
                    reader.ReadUInt32();    // always 0x00000004?
                    foreach (var level in levels)
                        this.StoryFlags.Add(level, (PlayableStages)reader.ReadUInt16());
                    foreach (var level in levels)
                        this.PracticeFlags.Add(level, (PlayableStages)reader.ReadUInt16());
                    reader.ReadByte();      // always 0x00?
                    this.Chara = Utils.ToEnum<CharaWithTotal>(reader.ReadByte());
                    reader.ReadUInt16();    // always 0x0000?
                }
            }

            public Dictionary<Level, PlayableStages> StoryFlags { get; private set; }       // really...?

            public Dictionary<Level, PlayableStages> PracticeFlags { get; private set; }    // really...?

            public CharaWithTotal Chara { get; private set; }                               // size: 1Byte
        }

        private class CardAttack : Th06.Chapter      // per card
        {
            public const string ValidSignature = "CATK";
            public const short ValidSize = 0x022C;

            public CardAttack(Th06.Chapter chapter)
                : base(chapter)
            {
                if (!this.Signature.Equals(ValidSignature, StringComparison.Ordinal))
                    throw new InvalidDataException("Signature");
                if (this.Size1 != ValidSize)
                    throw new InvalidDataException("Size1");

                this.StoryCareer = new CardAttackCareer();
                this.PracticeCareer = new CardAttackCareer();

                using (var reader = new BinaryReader(new MemoryStream(this.Data, false)))
                {
                    reader.ReadUInt32();    // always 0x00000003?
                    this.CardId = (short)(reader.ReadInt16() + 1);
                    reader.ReadByte();
                    this.Level = Utils.ToEnum<LevelPracticeWithTotal>(reader.ReadByte());   // Last Word == Normal...
                    this.CardName = reader.ReadExactBytes(0x30);
                    this.EnemyName = reader.ReadExactBytes(0x30);
                    this.Comment = reader.ReadExactBytes(0x80);
                    this.StoryCareer.ReadFrom(reader);
                    this.PracticeCareer.ReadFrom(reader);
                    reader.ReadUInt32();    // always 0x00000000?
                }
            }

            public short CardId { get; private set; }       // 1-based

            public LevelPracticeWithTotal Level { get; private set; }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public byte[] CardName { get; private set; }    // .Length = 0x30

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public byte[] EnemyName { get; private set; }   // .Length = 0x30

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public byte[] Comment { get; private set; }     // .Length = 0x80, should split by '\0'

            public CardAttackCareer StoryCareer { get; private set; }

            public CardAttackCareer PracticeCareer { get; private set; }

            public bool HasTried()
            {
                return (this.StoryCareer.TrialCounts[CharaWithTotal.Total] > 0)
                    || (this.PracticeCareer.TrialCounts[CharaWithTotal.Total] > 0);
            }
        }

        private class CardAttackCareer : IBinaryReadable    // per story or practice
        {
            public CardAttackCareer()
            {
                var numCharas = Enum.GetValues(typeof(CharaWithTotal)).Length;
                this.MaxBonuses = new Dictionary<CharaWithTotal, uint>(numCharas);
                this.TrialCounts = new Dictionary<CharaWithTotal, int>(numCharas);
                this.ClearCounts = new Dictionary<CharaWithTotal, int>(numCharas);
            }

            public Dictionary<CharaWithTotal, uint> MaxBonuses { get; private set; }

            public Dictionary<CharaWithTotal, int> TrialCounts { get; private set; }

            public Dictionary<CharaWithTotal, int> ClearCounts { get; private set; }

            public void ReadFrom(BinaryReader reader)
            {
                if (reader == null)
                    throw new ArgumentNullException(nameof(reader));

                var charas = Utils.GetEnumerator<CharaWithTotal>();
                foreach (var chara in charas)
                    this.MaxBonuses.Add(chara, reader.ReadUInt32());
                foreach (var chara in charas)
                    this.TrialCounts.Add(chara, reader.ReadInt32());
                foreach (var chara in charas)
                    this.ClearCounts.Add(chara, reader.ReadInt32());
            }
        }

        private class PracticeScore : Th06.Chapter   // per character
        {
            public const string ValidSignature = "PSCR";
            public const short ValidSize = 0x0178;

            public PracticeScore(Th06.Chapter chapter)
                : base(chapter)
            {
                if (!this.Signature.Equals(ValidSignature, StringComparison.Ordinal))
                    throw new InvalidDataException("Signature");
                if (this.Size1 != ValidSize)
                    throw new InvalidDataException("Size1");

                var stages = Utils.GetEnumerator<Stage>();
                var levels = Utils.GetEnumerator<Level>();
                var numPairs = stages.Count() * levels.Count();
                this.PlayCounts = new Dictionary<StageLevelPair, int>(numPairs);
                this.HighScores = new Dictionary<StageLevelPair, int>(numPairs);

                using (var reader = new BinaryReader(new MemoryStream(this.Data, false)))
                {
                    //// The fields for Stage.Extra and Level.Extra actually exist...

                    reader.ReadUInt32();    // always 0x00000002?

                    foreach (var stage in stages)
                    {
                        foreach (var level in levels)
                        {
                            var key = new StageLevelPair(stage, level);
                            if (!this.PlayCounts.ContainsKey(key))
                                this.PlayCounts.Add(key, 0);
                            this.PlayCounts[key] = reader.ReadInt32();
                        }
                    }

                    foreach (var stage in stages)
                    {
                        foreach (var level in levels)
                        {
                            var key = new StageLevelPair(stage, level);
                            if (!this.HighScores.ContainsKey(key))
                                this.HighScores.Add(key, 0);
                            this.HighScores[key] = reader.ReadInt32();
                        }
                    }

                    this.Chara = Utils.ToEnum<Chara>(reader.ReadByte());
                    reader.ReadExactBytes(3);   // always 0x000001?
                }
            }

            public Dictionary<StageLevelPair, int> PlayCounts { get; private set; }

            public Dictionary<StageLevelPair, int> HighScores { get; private set; }     // * 10

            public Chara Chara { get; private set; }        // size: 1Byte
        }

        private class FLSP : Th06.Chapter    // FIXME
        {
            public const string ValidSignature = "FLSP";
            public const short ValidSize = 0x0020;

            public FLSP(Th06.Chapter chapter)
                : base(chapter)
            {
                if (!this.Signature.Equals(ValidSignature, StringComparison.Ordinal))
                    throw new InvalidDataException("Signature");
                if (this.Size1 != ValidSize)
                    throw new InvalidDataException("Size1");

                using (var reader = new BinaryReader(new MemoryStream(this.Data, false)))
                {
                    reader.ReadExactBytes(0x18);
                }
            }
        }

        private class PlayStatus : Th06.Chapter
        {
            public const string ValidSignature = "PLST";
            public const short ValidSize = 0x0228;

            public PlayStatus(Th06.Chapter chapter)
                : base(chapter)
            {
                if (!this.Signature.Equals(ValidSignature, StringComparison.Ordinal))
                    throw new InvalidDataException("Signature");
                if (this.Size1 != ValidSize)
                    throw new InvalidDataException("Size1");

                var levels = Utils.GetEnumerator<Level>();
                var numLevels = levels.Count();
                this.PlayCounts = new Dictionary<Level, PlayCount>(numLevels);
                this.TotalPlayCount = new PlayCount();

                using (var reader = new BinaryReader(new MemoryStream(this.Data, false)))
                {
                    reader.ReadUInt32();    // always 0x00000002?
                    var hours = reader.ReadInt32();
                    var minutes = reader.ReadInt32();
                    var seconds = reader.ReadInt32();
                    var milliseconds = reader.ReadInt32();
                    this.TotalRunningTime = new Time(hours, minutes, seconds, milliseconds, false);
                    hours = reader.ReadInt32();
                    minutes = reader.ReadInt32();
                    seconds = reader.ReadInt32();
                    milliseconds = reader.ReadInt32();
                    this.TotalPlayTime = new Time(hours, minutes, seconds, milliseconds, false);

                    foreach (var level in levels)
                    {
                        var playCount = new PlayCount();
                        playCount.ReadFrom(reader);
                        if (!this.PlayCounts.ContainsKey(level))
                            this.PlayCounts.Add(level, playCount);
                    }

                    new PlayCount().ReadFrom(reader);   // always all 0?
                    this.TotalPlayCount.ReadFrom(reader);
                    this.BgmFlags = reader.ReadExactBytes(21);
                    reader.ReadExactBytes(11);
                }
            }

            public Time TotalRunningTime { get; private set; }

            public Time TotalPlayTime { get; private set; }

            public Dictionary<Level, PlayCount> PlayCounts { get; private set; }

            public PlayCount TotalPlayCount { get; private set; }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public byte[] BgmFlags { get; private set; }            // .Length = 21
        }

        private class PlayCount : IBinaryReadable   // per level-with-total
        {
            public PlayCount()
            {
                this.Trials = new Dictionary<Chara, int>(Enum.GetValues(typeof(Chara)).Length);
            }

            public int TotalTrial { get; private set; }

            public Dictionary<Chara, int> Trials { get; private set; }

            public int TotalClear { get; private set; }

            public int TotalContinue { get; private set; }

            public int TotalPractice { get; private set; }

            public void ReadFrom(BinaryReader reader)
            {
                if (reader == null)
                    throw new ArgumentNullException(nameof(reader));

                this.TotalTrial = reader.ReadInt32();
                foreach (var chara in Utils.GetEnumerator<Chara>())
                    this.Trials.Add(chara, reader.ReadInt32());
                reader.ReadUInt32();    // always 0x00000000?
                this.TotalClear = reader.ReadInt32();
                this.TotalContinue = reader.ReadInt32();
                this.TotalPractice = reader.ReadInt32();
            }
        }
    }
}

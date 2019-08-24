//-----------------------------------------------------------------------
// <copyright file="Th075Converter.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable 1591
#pragma warning disable SA1600 // ElementsMustBeDocumented
#pragma warning disable SA1602 // EnumerationItemsMustBeDocumented

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using ThScoreFileConverter.Properties;

namespace ThScoreFileConverter.Models
{
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Reviewed.")]
    internal class Th075Converter : ThConverter
    {
        private const string CharTable =
            @"ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
            @"abcdefghijklmnopqrstuvwxyz" +
            @"0123456789+-/*=%#!?.,:;_@$" +
            @"(){}[]<>&\|~^             ";

        // Thanks to thwiki.info
        [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1008:OpeningParenthesisMustBeSpacedCorrectly", Justification = "Reviewed.")]
        private static readonly Dictionary<int, SpellCardInfo> CardTable =
            new Dictionary<int, SpellCardInfo>()
            {
                {   1, new SpellCardInfo("符の壱「夢想妙珠連」-Easy-",                        Chara.Reimu,     Level.Easy) },
                {   2, new SpellCardInfo("符の壱「夢想妙珠連」",                              Chara.Reimu,     Level.Normal) },
                {   3, new SpellCardInfo("符の壱「夢想妙珠連」-Hard-",                        Chara.Reimu,     Level.Hard) },
                {   4, new SpellCardInfo("符の壱「夢想妙珠連」-Lunatic-",                     Chara.Reimu,     Level.Lunatic) },
                {   5, new SpellCardInfo("符の弐「陰陽散華」-Easy-",                          Chara.Reimu,     Level.Easy) },
                {   6, new SpellCardInfo("符の弐「陰陽散華」",                                Chara.Reimu,     Level.Normal) },
                {   7, new SpellCardInfo("符の弐「陰陽散華」-Hard-",                          Chara.Reimu,     Level.Hard) },
                {   8, new SpellCardInfo("符の弐「陰陽散華」-Lunatic-",                       Chara.Reimu,     Level.Lunatic) },
                {   9, new SpellCardInfo("符の参「魔浄閃結」-Easy-",                          Chara.Reimu,     Level.Easy) },
                {  10, new SpellCardInfo("符の参「魔浄閃結」",                                Chara.Reimu,     Level.Normal) },
                {  11, new SpellCardInfo("符の参「魔浄閃結」-Hard-",                          Chara.Reimu,     Level.Hard) },
                {  12, new SpellCardInfo("符の参「魔浄閃結」-Lunatic-",                       Chara.Reimu,     Level.Lunatic) },
                {  13, new SpellCardInfo("力符「陰陽玉将」-Easy-",                            Chara.Reimu,     Level.Easy) },
                {  14, new SpellCardInfo("力符「陰陽玉将」",                                  Chara.Reimu,     Level.Normal) },
                {  15, new SpellCardInfo("力符「陰陽玉将」-Hard-",                            Chara.Reimu,     Level.Hard) },
                {  16, new SpellCardInfo("力符「陰陽玉将」-Lunatic-",                         Chara.Reimu,     Level.Lunatic) },
                {  17, new SpellCardInfo("夢戦「幻想之月」-Easy-",                            Chara.Reimu,     Level.Easy) },
                {  18, new SpellCardInfo("夢戦「幻想之月」",                                  Chara.Reimu,     Level.Normal) },
                {  19, new SpellCardInfo("夢戦「幻想之月」-Hard-",                            Chara.Reimu,     Level.Hard) },
                {  20, new SpellCardInfo("夢戦「幻想之月」-Lunatic-",                         Chara.Reimu,     Level.Lunatic) },
                {  21, new SpellCardInfo("無題「空を飛ぶ不思議な巫女」",                      Chara.Reimu,     Level.Easy) },
                {  22, new SpellCardInfo("無題「空を飛ぶ不思議な巫女」",                      Chara.Reimu,     Level.Normal) },
                {  23, new SpellCardInfo("無題「空を飛ぶ不思議な巫女」",                      Chara.Reimu,     Level.Hard) },
                {  24, new SpellCardInfo("無題「空を飛ぶ不思議な巫女」",                      Chara.Reimu,     Level.Lunatic) },
                {  25, new SpellCardInfo("符の壱「スターダストレヴァリエ」-Easy-",            Chara.Marisa,    Level.Easy) },
                {  26, new SpellCardInfo("符の壱「スターダストレヴァリエ」",                  Chara.Marisa,    Level.Normal) },
                {  27, new SpellCardInfo("符の壱「スターダストレヴァリエ」-Hard-",            Chara.Marisa,    Level.Hard) },
                {  28, new SpellCardInfo("符の壱「スターダストレヴァリエ」-Lunatic-",         Chara.Marisa,    Level.Lunatic) },
                {  29, new SpellCardInfo("符の弐「アステロイドベルト」-Easy-",                Chara.Marisa,    Level.Easy) },
                {  30, new SpellCardInfo("符の弐「アステロイドベルト」",                      Chara.Marisa,    Level.Normal) },
                {  31, new SpellCardInfo("符の弐「アステロイドベルト」-Hard-",                Chara.Marisa,    Level.Hard) },
                {  32, new SpellCardInfo("符の弐「アステロイドベルト」-Lunatic-",             Chara.Marisa,    Level.Lunatic) },
                {  33, new SpellCardInfo("符の参「マスタースパーク」-Easy-",                  Chara.Marisa,    Level.Easy) },
                {  34, new SpellCardInfo("符の参「マスタースパーク」",                        Chara.Marisa,    Level.Normal) },
                {  35, new SpellCardInfo("符の参「マスタースパーク」-Hard-",                  Chara.Marisa,    Level.Hard) },
                {  36, new SpellCardInfo("符の参「マスタースパーク」-Lunatic-",               Chara.Marisa,    Level.Lunatic) },
                {  37, new SpellCardInfo("星符「ドラゴンメテオ」-Easy-",                      Chara.Marisa,    Level.Easy) },
                {  38, new SpellCardInfo("星符「ドラゴンメテオ」",                            Chara.Marisa,    Level.Normal) },
                {  39, new SpellCardInfo("星符「ドラゴンメテオ」-Hard-",                      Chara.Marisa,    Level.Hard) },
                {  40, new SpellCardInfo("星符「ドラゴンメテオ」-Lunatic-",                   Chara.Marisa,    Level.Lunatic) },
                {  41, new SpellCardInfo("符の壱「連続殺人ドール」-Easy-",                    Chara.Sakuya,    Level.Easy) },
                {  42, new SpellCardInfo("符の壱「連続殺人ドール」",                          Chara.Sakuya,    Level.Normal) },
                {  43, new SpellCardInfo("符の壱「連続殺人ドール」-Hard-",                    Chara.Sakuya,    Level.Hard) },
                {  44, new SpellCardInfo("符の壱「連続殺人ドール」-Lunatic-",                 Chara.Sakuya,    Level.Lunatic) },
                {  45, new SpellCardInfo("符の弐「チェックメイド」-Easy-",                    Chara.Sakuya,    Level.Easy) },
                {  46, new SpellCardInfo("符の弐「チェックメイド」",                          Chara.Sakuya,    Level.Normal) },
                {  47, new SpellCardInfo("符の弐「チェックメイド」-Hard-",                    Chara.Sakuya,    Level.Hard) },
                {  48, new SpellCardInfo("符の弐「チェックメイド」-Lunatic-",                 Chara.Sakuya,    Level.Lunatic) },
                {  49, new SpellCardInfo("手品「クロースアップ殺人鬼」-Easy-",                Chara.Sakuya,    Level.Easy) },
                {  50, new SpellCardInfo("手品「クロースアップ殺人鬼」",                      Chara.Sakuya,    Level.Normal) },
                {  51, new SpellCardInfo("手品「クロースアップ殺人鬼」-Hard-",                Chara.Sakuya,    Level.Hard) },
                {  52, new SpellCardInfo("手品「クロースアップ殺人鬼」-Lunatic-",             Chara.Sakuya,    Level.Lunatic) },
                {  53, new SpellCardInfo("符の壱「アーティフルチャンター」-Easy-",            Chara.Alice,     Level.Easy) },
                {  54, new SpellCardInfo("符の壱「アーティフルチャンター」",                  Chara.Alice,     Level.Normal) },
                {  55, new SpellCardInfo("符の壱「アーティフルチャンター」-Hard-",            Chara.Alice,     Level.Hard) },
                {  56, new SpellCardInfo("符の壱「アーティフルチャンター」-Lunatic-",         Chara.Alice,     Level.Lunatic) },
                {  57, new SpellCardInfo("符の弐「ドールクルセイダー」-Easy-",                Chara.Alice,     Level.Easy) },
                {  58, new SpellCardInfo("符の弐「ドールクルセイダー」",                      Chara.Alice,     Level.Normal) },
                {  59, new SpellCardInfo("符の弐「ドールクルセイダー」-Hard-",                Chara.Alice,     Level.Hard) },
                {  60, new SpellCardInfo("符の弐「ドールクルセイダー」-Lunatic-",             Chara.Alice,     Level.Lunatic) },
                {  61, new SpellCardInfo("魔光「デヴィリーライトレイ」-Easy-",                Chara.Alice,     Level.Easy) },
                {  62, new SpellCardInfo("魔光「デヴィリーライトレイ」",                      Chara.Alice,     Level.Normal) },
                {  63, new SpellCardInfo("魔光「デヴィリーライトレイ」-Hard-",                Chara.Alice,     Level.Hard) },
                {  64, new SpellCardInfo("魔光「デヴィリーライトレイ」-Lunatic-",             Chara.Alice,     Level.Lunatic) },
                {  65, new SpellCardInfo("符の壱「セントエルモエクスプロージョン」-Easy-",    Chara.Patchouli, Level.Easy) },
                {  66, new SpellCardInfo("符の壱「セントエルモエクスプロージョン」",          Chara.Patchouli, Level.Normal) },
                {  67, new SpellCardInfo("符の壱「セントエルモエクスプロージョン」-Hard-",    Chara.Patchouli, Level.Hard) },
                {  68, new SpellCardInfo("符の壱「セントエルモエクスプロージョン」-Lunatic-", Chara.Patchouli, Level.Lunatic) },
                {  69, new SpellCardInfo("符の弐「デリュージュフォーティディ」-Easy-",        Chara.Patchouli, Level.Easy) },
                {  70, new SpellCardInfo("符の弐「デリュージュフォーティディ」",              Chara.Patchouli, Level.Normal) },
                {  71, new SpellCardInfo("符の弐「デリュージュフォーティディ」-Hard-",        Chara.Patchouli, Level.Hard) },
                {  72, new SpellCardInfo("符の弐「デリュージュフォーティディ」-Lunatic-",     Chara.Patchouli, Level.Lunatic) },
                {  73, new SpellCardInfo("金土符「ジンジャガスト」-Easy-",                    Chara.Patchouli, Level.Easy) },
                {  74, new SpellCardInfo("金土符「ジンジャガスト」",                          Chara.Patchouli, Level.Normal) },
                {  75, new SpellCardInfo("金土符「ジンジャガスト」-Hard-",                    Chara.Patchouli, Level.Hard) },
                {  76, new SpellCardInfo("金土符「ジンジャガスト」-Lunatic-",                 Chara.Patchouli, Level.Lunatic) },
                {  77, new SpellCardInfo("符の壱「二重の苦輪」-Easy-",                        Chara.Youmu,     Level.Easy) },
                {  78, new SpellCardInfo("符の壱「二重の苦輪」",                              Chara.Youmu,     Level.Normal) },
                {  79, new SpellCardInfo("符の壱「二重の苦輪」-Hard-",                        Chara.Youmu,     Level.Hard) },
                {  80, new SpellCardInfo("符の壱「二重の苦輪」-Lunatic-",                     Chara.Youmu,     Level.Lunatic) },
                {  81, new SpellCardInfo("符の弐「心眼迷想斬」-Easy-",                        Chara.Youmu,     Level.Easy) },
                {  82, new SpellCardInfo("符の弐「心眼迷想斬」",                              Chara.Youmu,     Level.Normal) },
                {  83, new SpellCardInfo("符の弐「心眼迷想斬」-Hard-",                        Chara.Youmu,     Level.Hard) },
                {  84, new SpellCardInfo("符の弐「心眼迷想斬」-Lunatic-",                     Chara.Youmu,     Level.Lunatic) },
                {  85, new SpellCardInfo("符の参「業風神閃斬」-Easy-",                        Chara.Youmu,     Level.Easy) },
                {  86, new SpellCardInfo("符の参「業風神閃斬」",                              Chara.Youmu,     Level.Normal) },
                {  87, new SpellCardInfo("符の参「業風神閃斬」-Hard-",                        Chara.Youmu,     Level.Hard) },
                {  88, new SpellCardInfo("符の参「業風神閃斬」-Lunatic-",                     Chara.Youmu,     Level.Lunatic) },
                {  89, new SpellCardInfo("奥義「西行春風斬」",                                Chara.Youmu,     Level.Easy) },
                {  90, new SpellCardInfo("奥義「西行春風斬」",                                Chara.Youmu,     Level.Normal) },
                {  91, new SpellCardInfo("奥義「西行春風斬」",                                Chara.Youmu,     Level.Hard) },
                {  92, new SpellCardInfo("奥義「西行春風斬」",                                Chara.Youmu,     Level.Lunatic) },
                {  93, new SpellCardInfo("符の壱「バッドレディスクランブル」-Easy-",          Chara.Remilia,   Level.Easy) },
                {  94, new SpellCardInfo("符の壱「バッドレディスクランブル」",                Chara.Remilia,   Level.Normal) },
                {  95, new SpellCardInfo("符の壱「バッドレディスクランブル」-Hard-",          Chara.Remilia,   Level.Hard) },
                {  96, new SpellCardInfo("符の壱「バッドレディスクランブル」-Lunatic-",       Chara.Remilia,   Level.Lunatic) },
                {  97, new SpellCardInfo("符の弐「マイハートブレイク」-Easy-",                Chara.Remilia,   Level.Easy) },
                {  98, new SpellCardInfo("符の弐「マイハートブレイク」",                      Chara.Remilia,   Level.Normal) },
                {  99, new SpellCardInfo("符の弐「マイハートブレイク」-Hard-",                Chara.Remilia,   Level.Hard) },
                { 100, new SpellCardInfo("符の弐「マイハートブレイク」-Lunatic-",             Chara.Remilia,   Level.Lunatic) },
                { 101, new SpellCardInfo("符の参「ヘルカタストロフィ」-Easy-",                Chara.Remilia,   Level.Easy) },
                { 102, new SpellCardInfo("符の参「ヘルカタストロフィ」",                      Chara.Remilia,   Level.Normal) },
                { 103, new SpellCardInfo("符の参「ヘルカタストロフィ」-Hard-",                Chara.Remilia,   Level.Hard) },
                { 104, new SpellCardInfo("符の参「ヘルカタストロフィ」-Lunatic-",             Chara.Remilia,   Level.Lunatic) },
                { 105, new SpellCardInfo("夜符「クイーン・オブ・ミッドナイト」-Easy-",        Chara.Remilia,   Level.Easy) },
                { 106, new SpellCardInfo("夜符「クイーン・オブ・ミッドナイト」",              Chara.Remilia,   Level.Normal) },
                { 107, new SpellCardInfo("夜符「クイーン・オブ・ミッドナイト」-Hard-",        Chara.Remilia,   Level.Hard) },
                { 108, new SpellCardInfo("夜符「クイーン・オブ・ミッドナイト」-Lunatic-",     Chara.Remilia,   Level.Lunatic) },
                { 109, new SpellCardInfo("「レッドマジック」",                                Chara.Remilia,   Level.Easy) },
                { 110, new SpellCardInfo("「レッドマジック」",                                Chara.Remilia,   Level.Normal) },
                { 111, new SpellCardInfo("「レッドマジック」",                                Chara.Remilia,   Level.Hard) },
                { 112, new SpellCardInfo("「レッドマジック」",                                Chara.Remilia,   Level.Lunatic) },
                { 113, new SpellCardInfo("符の壱「幽夢の胡蝶」-Easy-",                        Chara.Yuyuko,    Level.Easy) },
                { 114, new SpellCardInfo("符の壱「幽夢の胡蝶」",                              Chara.Yuyuko,    Level.Normal) },
                { 115, new SpellCardInfo("符の壱「幽夢の胡蝶」-Hard-",                        Chara.Yuyuko,    Level.Hard) },
                { 116, new SpellCardInfo("符の壱「幽夢の胡蝶」-Lunatic-",                     Chara.Yuyuko,    Level.Lunatic) },
                { 117, new SpellCardInfo("符の弐「白玉楼の垂れ彼岸」-Easy-",                  Chara.Yuyuko,    Level.Easy) },
                { 118, new SpellCardInfo("符の弐「白玉楼の垂れ彼岸」",                        Chara.Yuyuko,    Level.Normal) },
                { 119, new SpellCardInfo("符の弐「白玉楼の垂れ彼岸」-Hard-",                  Chara.Yuyuko,    Level.Hard) },
                { 120, new SpellCardInfo("符の弐「白玉楼の垂れ彼岸」-Lunatic-",               Chara.Yuyuko,    Level.Lunatic) },
                { 121, new SpellCardInfo("符の参「果てしなく昔の死地」-Easy-",                Chara.Yuyuko,    Level.Easy) },
                { 122, new SpellCardInfo("符の参「果てしなく昔の死地」",                      Chara.Yuyuko,    Level.Normal) },
                { 123, new SpellCardInfo("符の参「果てしなく昔の死地」-Hard-",                Chara.Yuyuko,    Level.Hard) },
                { 124, new SpellCardInfo("符の参「果てしなく昔の死地」-Lunatic-",             Chara.Yuyuko,    Level.Lunatic) },
                { 125, new SpellCardInfo("桜花「未練未酌宴」-Easy-",                          Chara.Yuyuko,    Level.Easy) },
                { 126, new SpellCardInfo("桜花「未練未酌宴」",                                Chara.Yuyuko,    Level.Normal) },
                { 127, new SpellCardInfo("桜花「未練未酌宴」-Hard-",                          Chara.Yuyuko,    Level.Hard) },
                { 128, new SpellCardInfo("桜花「未練未酌宴」-Lunatic-",                       Chara.Yuyuko,    Level.Lunatic) },
                { 129, new SpellCardInfo("「紫(むらさき)の彼岸は遅れて輝く」",                Chara.Yuyuko,    Level.Easy) },
                { 130, new SpellCardInfo("「紫(むらさき)の彼岸は遅れて輝く」",                Chara.Yuyuko,    Level.Normal) },
                { 131, new SpellCardInfo("「紫(むらさき)の彼岸は遅れて輝く」",                Chara.Yuyuko,    Level.Hard) },
                { 132, new SpellCardInfo("「紫(むらさき)の彼岸は遅れて輝く」",                Chara.Yuyuko,    Level.Lunatic) },
                { 133, new SpellCardInfo("符の壱「四重結界」-Easy-",                          Chara.Yukari,    Level.Easy) },
                { 134, new SpellCardInfo("符の壱「四重結界」",                                Chara.Yukari,    Level.Normal) },
                { 135, new SpellCardInfo("符の壱「四重結界」-Hard-",                          Chara.Yukari,    Level.Hard) },
                { 136, new SpellCardInfo("符の壱「四重結界」-Lunatic-",                       Chara.Yukari,    Level.Lunatic) },
                { 137, new SpellCardInfo("符の弐「八雲卍傘」-Easy-",                          Chara.Yukari,    Level.Easy) },
                { 138, new SpellCardInfo("符の弐「八雲卍傘」",                                Chara.Yukari,    Level.Normal) },
                { 139, new SpellCardInfo("符の弐「八雲卍傘」-Hard-",                          Chara.Yukari,    Level.Hard) },
                { 140, new SpellCardInfo("符の弐「八雲卍傘」-Lunatic-",                       Chara.Yukari,    Level.Lunatic) },
                { 141, new SpellCardInfo("符の参「八雲藍」-Easy-",                            Chara.Yukari,    Level.Easy) },
                { 142, new SpellCardInfo("符の参「八雲藍」",                                  Chara.Yukari,    Level.Normal) },
                { 143, new SpellCardInfo("符の参「八雲藍」-Hard-",                            Chara.Yukari,    Level.Hard) },
                { 144, new SpellCardInfo("符の参「八雲藍」-Lunatic-",                         Chara.Yukari,    Level.Lunatic) },
                { 145, new SpellCardInfo("外力「無限の超高速飛行体」-Easy-",                  Chara.Yukari,    Level.Easy) },
                { 146, new SpellCardInfo("外力「無限の超高速飛行体」",                        Chara.Yukari,    Level.Normal) },
                { 147, new SpellCardInfo("外力「無限の超高速飛行体」-Hard-",                  Chara.Yukari,    Level.Hard) },
                { 148, new SpellCardInfo("外力「無限の超高速飛行体」-Lunatic-",               Chara.Yukari,    Level.Lunatic) },
                { 149, new SpellCardInfo("幻想「第一種永久機関」",                            Chara.Yukari,    Level.Easy) },
                { 150, new SpellCardInfo("幻想「第一種永久機関」",                            Chara.Yukari,    Level.Normal) },
                { 151, new SpellCardInfo("幻想「第一種永久機関」",                            Chara.Yukari,    Level.Hard) },
                { 152, new SpellCardInfo("幻想「第一種永久機関」",                            Chara.Yukari,    Level.Lunatic) },
                { 153, new SpellCardInfo("符の壱「投擲の天岩戸」-Easy-",                      Chara.Suika,     Level.Easy) },
                { 154, new SpellCardInfo("符の壱「投擲の天岩戸」",                            Chara.Suika,     Level.Normal) },
                { 155, new SpellCardInfo("符の壱「投擲の天岩戸」-Hard-",                      Chara.Suika,     Level.Hard) },
                { 156, new SpellCardInfo("符の壱「投擲の天岩戸」-Lunatic-",                   Chara.Suika,     Level.Lunatic) },
                { 157, new SpellCardInfo("符の弐「坤軸の大鬼」-Easy-",                        Chara.Suika,     Level.Easy) },
                { 158, new SpellCardInfo("符の弐「坤軸の大鬼」",                              Chara.Suika,     Level.Normal) },
                { 159, new SpellCardInfo("符の弐「坤軸の大鬼」-Hard-",                        Chara.Suika,     Level.Hard) },
                { 160, new SpellCardInfo("符の弐「坤軸の大鬼」-Lunatic-",                     Chara.Suika,     Level.Lunatic) },
                { 161, new SpellCardInfo("符の参「追儺返しブラックホール」-Easy-",            Chara.Suika,     Level.Easy) },
                { 162, new SpellCardInfo("符の参「追儺返しブラックホール」",                  Chara.Suika,     Level.Normal) },
                { 163, new SpellCardInfo("符の参「追儺返しブラックホール」-Hard-",            Chara.Suika,     Level.Hard) },
                { 164, new SpellCardInfo("符の参「追儺返しブラックホール」-Lunatic-",         Chara.Suika,     Level.Lunatic) },
                { 165, new SpellCardInfo("鬼火「超高密度燐禍術」-Easy-",                      Chara.Suika,     Level.Easy) },
                { 166, new SpellCardInfo("鬼火「超高密度燐禍術」",                            Chara.Suika,     Level.Normal) },
                { 167, new SpellCardInfo("鬼火「超高密度燐禍術」-Hard-",                      Chara.Suika,     Level.Hard) },
                { 168, new SpellCardInfo("鬼火「超高密度燐禍術」-Lunatic-",                   Chara.Suika,     Level.Lunatic) },
                { 169, new SpellCardInfo("疎符「六里霧中」-Easy-",                            Chara.Suika,     Level.Easy) },
                { 170, new SpellCardInfo("疎符「六里霧中」",                                  Chara.Suika,     Level.Normal) },
                { 171, new SpellCardInfo("疎符「六里霧中」-Hard-",                            Chara.Suika,     Level.Hard) },
                { 172, new SpellCardInfo("疎符「六里霧中」-Lunatic-",                         Chara.Suika,     Level.Lunatic) },
                { 173, new SpellCardInfo("「百万鬼夜行」",                                    Chara.Suika,     Level.Easy) },
                { 174, new SpellCardInfo("「百万鬼夜行」",                                    Chara.Suika,     Level.Normal) },
                { 175, new SpellCardInfo("「百万鬼夜行」",                                    Chara.Suika,     Level.Hard) },
                { 176, new SpellCardInfo("「百万鬼夜行」",                                    Chara.Suika,     Level.Lunatic) },
            };

        private static readonly Dictionary<Chara, IEnumerable<int>> CardIdTable = InitializeCardIdTable();

        private static new readonly EnumShortNameParser<Level> LevelParser =
            new EnumShortNameParser<Level>();

        private static new readonly EnumShortNameParser<LevelWithTotal> LevelWithTotalParser =
            new EnumShortNameParser<LevelWithTotal>();

        private static readonly EnumShortNameParser<Chara> CharaParser =
            new EnumShortNameParser<Chara>();

        private AllScoreData allScoreData = null;

        public enum Level
        {
#pragma warning disable SA1134 // Attributes should not share line
            [EnumAltName("E")] Easy,
            [EnumAltName("N")] Normal,
            [EnumAltName("H")] Hard,
            [EnumAltName("L")] Lunatic,
#pragma warning restore SA1134 // Attributes should not share line
        }

        public new enum LevelWithTotal
        {
#pragma warning disable SA1134 // Attributes should not share line
            [EnumAltName("E")] Easy,
            [EnumAltName("N")] Normal,
            [EnumAltName("H")] Hard,
            [EnumAltName("L")] Lunatic,
            [EnumAltName("T")] Total,
#pragma warning restore SA1134 // Attributes should not share line
        }

        public enum Chara
        {
#pragma warning disable SA1134 // Attributes should not share line
            [EnumAltName("RM")] Reimu,
            [EnumAltName("MR")] Marisa,
            [EnumAltName("SK")] Sakuya,
            [EnumAltName("AL")] Alice,
            [EnumAltName("PC")] Patchouli,
            [EnumAltName("YM")] Youmu,
            [EnumAltName("RL")] Remilia,
            [EnumAltName("YU")] Yuyuko,
            [EnumAltName("YK")] Yukari,
            [EnumAltName("SU")] Suika,
            [EnumAltName("ML")] Meiling,
#pragma warning restore SA1134 // Attributes should not share line
        }

        public enum CharaWithTotal
        {
#pragma warning disable SA1134 // Attributes should not share line
            [EnumAltName("RM")] Reimu,
            [EnumAltName("MR")] Marisa,
            [EnumAltName("SK")] Sakuya,
            [EnumAltName("AL")] Alice,
            [EnumAltName("PC")] Patchouli,
            [EnumAltName("YM")] Youmu,
            [EnumAltName("RL")] Remilia,
            [EnumAltName("YU")] Yuyuko,
            [EnumAltName("YK")] Yukari,
            [EnumAltName("SU")] Suika,
            [EnumAltName("ML")] Meiling,
            [EnumAltName("TL")] Total,
#pragma warning restore SA1134 // Attributes should not share line
        }

        public new enum Stage
        {
#pragma warning disable SA1134 // Attributes should not share line
            [EnumAltName("1")] St1,
            [EnumAltName("2")] St2,
            [EnumAltName("3")] St3,
            [EnumAltName("4")] St4,
            [EnumAltName("5")] St5,
            [EnumAltName("6")] St6,
            [EnumAltName("7")] St7,
#pragma warning restore SA1134 // Attributes should not share line
        }

        public override string SupportedVersions
        {
            get { return "1.11"; }
        }

        protected override bool ReadScoreFile(Stream input)
        {
#if DEBUG
            using (var decoded = new FileStream("th075decoded.dat", FileMode.Create, FileAccess.ReadWrite))
            {
                var size = (int)input.Length;
                var data = new byte[size];
                input.Read(data, 0, size);
                decoded.Write(data, 0, size);
                decoded.Flush();
                decoded.SetLength(decoded.Position);
            }
#endif

            input.Seek(0, SeekOrigin.Begin);
            this.allScoreData = Read(input);

            return this.allScoreData != null;
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
                new CharaReplacer(this),
            };
        }

        private static Dictionary<Chara, IEnumerable<int>> InitializeCardIdTable()
        {
            var charaStageEnemyTable = new Dictionary<Chara, List<(Stage Stage, Chara Enemy)>>
            {
                {
                    Chara.Reimu,
                    new List<(Stage, Chara)>
                    {
                        (Stage.St1, Chara.Marisa),
                        (Stage.St2, Chara.Alice),
                        (Stage.St3, Chara.Youmu),
                        (Stage.St4, Chara.Sakuya),
                        (Stage.St5, Chara.Remilia),
                        (Stage.St6, Chara.Yukari),
                        (Stage.St7, Chara.Suika),
                    }
                },
                {
                    Chara.Marisa,
                    new List<(Stage, Chara)>
                    {
                        (Stage.St1, Chara.Alice),
                        (Stage.St2, Chara.Sakuya),
                        (Stage.St3, Chara.Patchouli),
                        (Stage.St4, Chara.Remilia),
                        (Stage.St5, Chara.Reimu),
                        (Stage.St6, Chara.Yukari),
                        (Stage.St7, Chara.Suika),
                    }
                },
                {
                    Chara.Sakuya,
                    new List<(Stage, Chara)>
                    {
                        (Stage.St1, Chara.Reimu),
                        (Stage.St2, Chara.Alice),
                        (Stage.St3, Chara.Marisa),
                        (Stage.St4, Chara.Youmu),
                        (Stage.St5, Chara.Yuyuko),
                        (Stage.St6, Chara.Yukari),
                        (Stage.St7, Chara.Suika),
                    }
                },
                {
                    Chara.Alice,
                    new List<(Stage, Chara)>
                    {
                        (Stage.St1, Chara.Marisa),
                        (Stage.St2, Chara.Reimu),
                        (Stage.St3, Chara.Sakuya),
                        (Stage.St4, Chara.Patchouli),
                        (Stage.St5, Chara.Youmu),
                        (Stage.St6, Chara.Yukari),
                        (Stage.St7, Chara.Suika),
                    }
                },
                {
                    Chara.Patchouli,
                    new List<(Stage, Chara)>
                    {
                        (Stage.St1, Chara.Marisa),
                        (Stage.St2, Chara.Sakuya),
                        (Stage.St3, Chara.Alice),
                        (Stage.St4, Chara.Youmu),
                        (Stage.St5, Chara.Yuyuko),
                        (Stage.St6, Chara.Yukari),
                        (Stage.St7, Chara.Suika),
                    }
                },
                {
                    Chara.Youmu,
                    new List<(Stage, Chara)>
                    {
                        (Stage.St1, Chara.Reimu),
                        (Stage.St2, Chara.Marisa),
                        (Stage.St3, Chara.Patchouli),
                        (Stage.St4, Chara.Sakuya),
                        (Stage.St5, Chara.Remilia),
                        (Stage.St6, Chara.Yukari),
                        (Stage.St7, Chara.Suika),
                    }
                },
                {
                    Chara.Remilia,
                    new List<(Stage, Chara)>
                    {
                        (Stage.St1, Chara.Sakuya),
                        (Stage.St2, Chara.Marisa),
                        (Stage.St3, Chara.Reimu),
                        (Stage.St4, Chara.Youmu),
                        (Stage.St5, Chara.Yuyuko),
                        (Stage.St6, Chara.Yukari),
                        (Stage.St7, Chara.Suika),
                    }
                },
                {
                    Chara.Yuyuko,
                    new List<(Stage, Chara)>
                    {
                        (Stage.St1, Chara.Youmu),
                        (Stage.St2, Chara.Marisa),
                        (Stage.St3, Chara.Reimu),
                        (Stage.St4, Chara.Sakuya),
                        (Stage.St5, Chara.Remilia),
                        (Stage.St6, Chara.Yukari),
                        (Stage.St7, Chara.Suika),
                    }
                },
                {
                    Chara.Yukari,
                    new List<(Stage, Chara)>
                    {
                        (Stage.St1, Chara.Remilia),
                        (Stage.St2, Chara.Alice),
                        (Stage.St3, Chara.Youmu),
                        (Stage.St4, Chara.Marisa),
                        (Stage.St5, Chara.Reimu),
                        (Stage.St6, Chara.Yuyuko),
                        (Stage.St7, Chara.Suika),
                    }
                },
                {
                    Chara.Suika,
                    new List<(Stage, Chara)>
                    {
                        (Stage.St1, Chara.Sakuya),
                        (Stage.St2, Chara.Alice),
                        (Stage.St3, Chara.Youmu),
                        (Stage.St4, Chara.Patchouli),
                        (Stage.St5, Chara.Marisa),
                        (Stage.St6, Chara.Remilia),
                        (Stage.St7, Chara.Reimu),
                    }
                },
            };

            var cardNumberTable = CardTable.ToLookup(pair => pair.Value.Enemy, pair => pair.Key);

            return charaStageEnemyTable.ToDictionary(
                charaStageEnemyPair => charaStageEnemyPair.Key,
                charaStageEnemyPair => charaStageEnemyPair.Value.SelectMany(stageEnemyPair =>
                {
                    switch (stageEnemyPair.Stage)
                    {
                        case Stage.St1:
                        case Stage.St2:
                            return cardNumberTable[stageEnemyPair.Enemy].Take(8);
                        case Stage.St3:
                        case Stage.St4:
                            return cardNumberTable[stageEnemyPair.Enemy].Take(12);
                        case Stage.St5:
                            return cardNumberTable[stageEnemyPair.Enemy].Take(16);
                        case Stage.St6:
                            return cardNumberTable[stageEnemyPair.Enemy].Take(20);
                        case Stage.St7:
                            return cardNumberTable[stageEnemyPair.Enemy].Take(24);
                        default:
                            return null;    // unreachable
                    }
                }));
        }

        private static AllScoreData Read(Stream input)
        {
            using (var reader = new BinaryReader(input, Encoding.UTF8, true))
            {
                var allScoreData = new AllScoreData();

                try
                {
                    allScoreData.ReadFrom(reader);
                }
                catch (EndOfStreamException)
                {
                }

                var numPairs = Enum.GetValues(typeof(Chara)).Length * Enum.GetValues(typeof(Level)).Length;
                if ((allScoreData.ClearData.Sum(data => data.Value.Count) == numPairs) &&
                    (allScoreData.Status != null))
                    return allScoreData;
                else
                    return null;
            }
        }

        // %T75SCR[w][xx][y][z]
        private class ScoreReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T75SCR({0})({1})(\d)([1-3])", LevelParser.Pattern, CharaParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public ScoreReplacer(Th075Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var level = LevelParser.Parse(match.Groups[1].Value);
                    var chara = CharaParser.Parse(match.Groups[2].Value);
                    var rank = Utils.ToZeroBased(
                        int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture));
                    var type = int.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture);

                    if (chara == Chara.Meiling)
                        return match.ToString();

                    var score = parent.allScoreData.ClearData[chara][level].Ranking[rank];

                    switch (type)
                    {
                        case 1:     // name
                            return score.Name;
                        case 2:     // score
                            return Utils.ToNumberString(score.Score);
                        case 3:     // date
                            return Utils.Format("{0:D2}/{1:D2}", score.Month, score.Day);
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

        // %T75C[xxx][yy][z]
        private class CareerReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T75C(\d{{3}})({0})([1-4])", CharaParser.Pattern);

            private readonly MatchEvaluator evaluator;

            [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
            public CareerReplacer(Th075Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var number = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
                    var chara = CharaParser.Parse(match.Groups[2].Value);
                    var type = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

                    if (chara == Chara.Meiling)
                        return match.ToString();

                    if (type == 4)
                    {
                        if ((number > 0) && (number <= CardIdTable[chara].Count()))
                        {
                            return parent.allScoreData.ClearData[chara].Values
                                .Any(data => data.CardTrulyGot[number - 1] != 0x00) ? "★" : string.Empty;
                        }
                        else
                        {
                            return match.ToString();
                        }
                    }

                    Func<short, int> toInteger = (value => (int)value);
                    Func<ClearData, IEnumerable<int>> getValues;
                    if (type == 1)
                        getValues = (data => data.MaxBonuses);
                    else if (type == 2)
                        getValues = (data => data.CardGotCount.Select(toInteger));
                    else
                        getValues = (data => data.CardTrialCount.Select(toInteger));

                    if (number == 0)
                    {
                        return Utils.ToNumberString(
                            parent.allScoreData.ClearData[chara].Values.Sum(data => getValues(data).Sum()));
                    }
                    else if (number <= CardIdTable[chara].Count())
                    {
                        return Utils.ToNumberString(
                            parent.allScoreData.ClearData[chara].Values.Sum(data =>
                                getValues(data).ElementAt(number - 1)));
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

        // %T75CARD[xxx][yy][z]
        private class CardReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T75CARD(\d{{3}})({0})([NR])", CharaParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public CardReplacer(Th075Converter parent, bool hideUntriedCards)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var number = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
                    var chara = CharaParser.Parse(match.Groups[2].Value);
                    var type = match.Groups[3].Value.ToUpperInvariant();

                    if (chara == Chara.Meiling)
                        return match.ToString();

                    if ((number > 0) && (number <= CardIdTable[chara].Count()))
                    {
                        if (hideUntriedCards)
                        {
                            var dataList = parent.allScoreData.ClearData[chara]
                                .Select(pair => pair.Value);
                            if (dataList.All(data => data.CardTrialCount[number - 1] <= 0))
                                return (type == "N") ? "??????????" : "?????";
                        }

                        var cardId = CardIdTable[chara].ElementAt(number - 1);
                        return (type == "N") ? CardTable[cardId].Name : CardTable[cardId].Level.ToString();
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

        // %T75CRG[x][yy][z]
        private class CollectRateReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T75CRG({0})({1})([1-3])", LevelWithTotalParser.Pattern, CharaParser.Pattern);

            private readonly MatchEvaluator evaluator;

            [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
            public CollectRateReplacer(Th075Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var level = LevelWithTotalParser.Parse(match.Groups[1].Value);
                    var chara = CharaParser.Parse(match.Groups[2].Value);
                    var type = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

                    if (chara == Chara.Meiling)
                        return match.ToString();

                    Func<ClearData, IEnumerable<short>> getValues;
                    if (type == 1)
                        getValues = (data => data.CardGotCount);
                    else if (type == 2)
                        getValues = (data => data.CardTrialCount);
                    else
                        getValues = (data => data.CardTrulyGot.Select(got => (short)got));

                    Func<short, bool> isPositive = (value => value > 0);

                    if (level == LevelWithTotal.Total)
                    {
                        return Utils.ToNumberString(
                            parent.allScoreData.ClearData[chara].Values.Sum(data =>
                                getValues(data).Count(isPositive)));
                    }
                    else
                    {
                        var cardIndexIdPairs = CardIdTable[chara]
                            .Select((id, index) => new KeyValuePair<int, int>(index, id))
                            .Where(pair => CardTable[pair.Value].Level == (Level)level);
                        return Utils.ToNumberString(
                            getValues(parent.allScoreData.ClearData[chara][Level.Easy])
                                .Where((value, index) => cardIndexIdPairs.Any(pair => pair.Key == index))
                                .Count(isPositive));
                    }
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        // %T75CHR[x][yy][z]
        private class CharaReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T75CHR({0})({1})([1-4])", LevelParser.Pattern, CharaParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public CharaReplacer(Th075Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var level = LevelParser.Parse(match.Groups[1].Value);
                    var chara = CharaParser.Parse(match.Groups[2].Value);
                    var type = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

                    if (chara == Chara.Meiling)
                        return match.ToString();

                    var data = parent.allScoreData.ClearData[chara][level];
                    switch (type)
                    {
                        case 1:
                            return Utils.ToNumberString(data.UseCount);
                        case 2:
                            return Utils.ToNumberString(data.ClearCount);
                        case 3:
                            return Utils.ToNumberString(data.MaxCombo);
                        case 4:
                            return Utils.ToNumberString(data.MaxDamage);
                        default:
                            return match.ToString();
                    }
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        private class SpellCardInfo
        {
            public SpellCardInfo(string name, Chara enemy, Level level)
            {
                this.Name = name;
                this.Enemy = enemy;
                this.Level = level;
            }

            public string Name { get; }

            public Chara Enemy { get; }

            public Level Level { get; }
        }

        private class AllScoreData : IBinaryReadable
        {
            public AllScoreData()
            {
                var charas = Utils.GetEnumerator<Chara>();
                var numLevels = Enum.GetValues(typeof(Level)).Length;
                this.ClearData = new Dictionary<Chara, Dictionary<Level, ClearData>>(charas.Count());
                foreach (var chara in charas)
                    this.ClearData.Add(chara, new Dictionary<Level, ClearData>(numLevels));
            }

            public Dictionary<Chara, Dictionary<Level, ClearData>> ClearData { get; private set; }

            public Status Status { get; private set; }

            [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "unknownChara", Justification = "Reviewed.")]
            [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "knownLevel", Justification = "Reviewed.")]
            public void ReadFrom(BinaryReader reader)
            {
                var levels = Utils.GetEnumerator<Level>();

                foreach (var chara in Utils.GetEnumerator<Chara>())
                {
                    foreach (var level in levels)
                    {
                        var clearData = new ClearData();
                        clearData.ReadFrom(reader);
                        if (!this.ClearData[chara].ContainsKey(level))
                            this.ClearData[chara].Add(level, clearData);
                    }
                }

                foreach (var unknownChara in Enumerable.Range(1, 4))
                {
                    foreach (var knownLevel in levels)
                        new ClearData().ReadFrom(reader);
                }

                var status = new Status();
                status.ReadFrom(reader);
                this.Status = status;
            }
        }

        private class ClearData : IBinaryReadable   // per character, level
        {
            public ClearData()
            {
                this.MaxBonuses = new List<int>(100);
                this.CardGotCount = new List<short>(100);
                this.CardTrialCount = new List<short>(100);
                this.CardTrulyGot = new List<byte>(100);
                this.Ranking = new List<HighScore>(10);
            }

            public int UseCount { get; private set; }

            public int ClearCount { get; private set; }

            public int MaxCombo { get; private set; }

            public int MaxDamage { get; private set; }

            public List<int> MaxBonuses { get; private set; }

            public List<short> CardGotCount { get; private set; }

            public List<short> CardTrialCount { get; private set; }

            public List<byte> CardTrulyGot { get; private set; }

            public List<HighScore> Ranking { get; private set; }

            [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "num", Justification = "Reviewed.")]
            public void ReadFrom(BinaryReader reader)
            {
                if (reader == null)
                    throw new ArgumentNullException(nameof(reader));

                var numbers = Enumerable.Range(1, 100);

                this.UseCount = reader.ReadInt32();
                this.ClearCount = reader.ReadInt32();
                this.MaxCombo = reader.ReadInt32();
                this.MaxDamage = reader.ReadInt32();
                foreach (var num in numbers)
                    this.MaxBonuses.Add(reader.ReadInt32());
                reader.ReadExactBytes(0xC8);
                foreach (var num in numbers)
                    this.CardGotCount.Add(reader.ReadInt16());
                reader.ReadExactBytes(0x64);
                foreach (var num in numbers)
                    this.CardTrialCount.Add(reader.ReadInt16());
                reader.ReadExactBytes(0x64);
                foreach (var num in numbers)
                    this.CardTrulyGot.Add(reader.ReadByte());
                reader.ReadExactBytes(0x32);
                reader.ReadExactBytes(6);   // 07 00 00 00 00 00

                foreach (var num in Enumerable.Range(1, 10))
                {
                    var score = new HighScore();
                    score.ReadFrom(reader);
                    this.Ranking.Add(score);
                }
            }
        }

        private class HighScore : IBinaryReadable
        {
            public HighScore()
            {
            }

            public string Name { get; private set; }

            public byte Month { get; private set; }     // 1-based

            public byte Day { get; private set; }       // 1-based

            public int Score { get; private set; }

            public void ReadFrom(BinaryReader reader)
            {
                if (reader == null)
                    throw new ArgumentNullException(nameof(reader));

                this.Name = new string(reader.ReadExactBytes(8).Select(ch => CharTable[ch]).ToArray());

                this.Month = reader.ReadByte();
                this.Day = reader.ReadByte();
                if ((this.Month == 0) && (this.Day == 0))
                {
                    // It's allowed.
                }
                else
                {
                    if ((this.Month <= 0) || (this.Month > 12))
                    {
                        throw new InvalidDataException(
                            Utils.Format(Resources.InvalidDataExceptionPropertyIsOutOfRange, nameof(this.Month)));
                    }

                    if ((this.Day <= 0) || (this.Day > DateTime.DaysInMonth(2000, this.Month)))
                    {
                        throw new InvalidDataException(
                            Utils.Format(Resources.InvalidDataExceptionPropertyIsOutOfRange, nameof(this.Day)));
                    }
                }

                reader.ReadUInt16();    // always 0x0000?
                this.Score = reader.ReadInt32();
            }
        }

        private class Status : IBinaryReadable
        {
            public Status()
            {
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public string LastName { get; private set; }

            public Dictionary<Chara, Dictionary<Chara, int>> ArcadeScores { get; private set; }

            [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "unknownChara", Justification = "Reviewed.")]
            [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "knownEnemy", Justification = "Reviewed.")]
            [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "unknownEnemy", Justification = "Reviewed.")]
            public void ReadFrom(BinaryReader reader)
            {
                if (reader == null)
                    throw new ArgumentNullException(nameof(reader));

                var charas = Utils.GetEnumerator<Chara>();
                var unknownCharas = Enumerable.Range(1, 4);
                var numScores = charas.Count() + unknownCharas.Count();

                this.LastName = new string(reader.ReadExactBytes(8).Select(ch => CharTable[ch]).ToArray());

                this.ArcadeScores = new Dictionary<Chara, Dictionary<Chara, int>>(numScores);
                foreach (var chara in charas)
                {
                    this.ArcadeScores[chara] = new Dictionary<Chara, int>(numScores);
                    foreach (var enemy in charas)
                        this.ArcadeScores[chara][enemy] = reader.ReadInt32() - 10;
                    foreach (var unknownEnemy in unknownCharas)
                        reader.ReadInt32();
                }

                foreach (var unknownChara in unknownCharas)
                {
                    foreach (var knownEnemy in charas)
                        reader.ReadInt32();
                    foreach (var unknownEnemy in unknownCharas)
                        reader.ReadInt32();
                }

                // FIXME... BGM flags?
                reader.ReadExactBytes(0x28);

                reader.ReadExactBytes(0x100);
            }
        }
    }
}

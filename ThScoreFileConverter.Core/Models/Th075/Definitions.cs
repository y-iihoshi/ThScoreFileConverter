﻿//-----------------------------------------------------------------------
// <copyright file="Definitions.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Core.Models.Th075;

/// <summary>
/// Provides several IMP specific definitions.
/// </summary>
public static class Definitions
{
    /// <summary>
    /// Gets the dictionary of IMP spell cards.
    /// Thanks to thwiki.info.
    /// </summary>
    public static IReadOnlyDictionary<int, SpellCardInfo> CardTable { get; } = new Dictionary<int, SpellCardInfo>
    {
        {   1, new("符の壱「夢想妙珠連」-Easy-",                        Chara.Reimu,     Level.Easy) },
        {   2, new("符の壱「夢想妙珠連」",                              Chara.Reimu,     Level.Normal) },
        {   3, new("符の壱「夢想妙珠連」-Hard-",                        Chara.Reimu,     Level.Hard) },
        {   4, new("符の壱「夢想妙珠連」-Lunatic-",                     Chara.Reimu,     Level.Lunatic) },
        {   5, new("符の弐「陰陽散華」-Easy-",                          Chara.Reimu,     Level.Easy) },
        {   6, new("符の弐「陰陽散華」",                                Chara.Reimu,     Level.Normal) },
        {   7, new("符の弐「陰陽散華」-Hard-",                          Chara.Reimu,     Level.Hard) },
        {   8, new("符の弐「陰陽散華」-Lunatic-",                       Chara.Reimu,     Level.Lunatic) },
        {   9, new("符の参「魔浄閃結」-Easy-",                          Chara.Reimu,     Level.Easy) },
        {  10, new("符の参「魔浄閃結」",                                Chara.Reimu,     Level.Normal) },
        {  11, new("符の参「魔浄閃結」-Hard-",                          Chara.Reimu,     Level.Hard) },
        {  12, new("符の参「魔浄閃結」-Lunatic-",                       Chara.Reimu,     Level.Lunatic) },
        {  13, new("力符「陰陽玉将」-Easy-",                            Chara.Reimu,     Level.Easy) },
        {  14, new("力符「陰陽玉将」",                                  Chara.Reimu,     Level.Normal) },
        {  15, new("力符「陰陽玉将」-Hard-",                            Chara.Reimu,     Level.Hard) },
        {  16, new("力符「陰陽玉将」-Lunatic-",                         Chara.Reimu,     Level.Lunatic) },
        {  17, new("夢戦「幻想之月」-Easy-",                            Chara.Reimu,     Level.Easy) },
        {  18, new("夢戦「幻想之月」",                                  Chara.Reimu,     Level.Normal) },
        {  19, new("夢戦「幻想之月」-Hard-",                            Chara.Reimu,     Level.Hard) },
        {  20, new("夢戦「幻想之月」-Lunatic-",                         Chara.Reimu,     Level.Lunatic) },
        {  21, new("無題「空を飛ぶ不思議な巫女」",                      Chara.Reimu,     Level.Easy) },
        {  22, new("無題「空を飛ぶ不思議な巫女」",                      Chara.Reimu,     Level.Normal) },
        {  23, new("無題「空を飛ぶ不思議な巫女」",                      Chara.Reimu,     Level.Hard) },
        {  24, new("無題「空を飛ぶ不思議な巫女」",                      Chara.Reimu,     Level.Lunatic) },
        {  25, new("符の壱「スターダストレヴァリエ」-Easy-",            Chara.Marisa,    Level.Easy) },
        {  26, new("符の壱「スターダストレヴァリエ」",                  Chara.Marisa,    Level.Normal) },
        {  27, new("符の壱「スターダストレヴァリエ」-Hard-",            Chara.Marisa,    Level.Hard) },
        {  28, new("符の壱「スターダストレヴァリエ」-Lunatic-",         Chara.Marisa,    Level.Lunatic) },
        {  29, new("符の弐「アステロイドベルト」-Easy-",                Chara.Marisa,    Level.Easy) },
        {  30, new("符の弐「アステロイドベルト」",                      Chara.Marisa,    Level.Normal) },
        {  31, new("符の弐「アステロイドベルト」-Hard-",                Chara.Marisa,    Level.Hard) },
        {  32, new("符の弐「アステロイドベルト」-Lunatic-",             Chara.Marisa,    Level.Lunatic) },
        {  33, new("符の参「マスタースパーク」-Easy-",                  Chara.Marisa,    Level.Easy) },
        {  34, new("符の参「マスタースパーク」",                        Chara.Marisa,    Level.Normal) },
        {  35, new("符の参「マスタースパーク」-Hard-",                  Chara.Marisa,    Level.Hard) },
        {  36, new("符の参「マスタースパーク」-Lunatic-",               Chara.Marisa,    Level.Lunatic) },
        {  37, new("星符「ドラゴンメテオ」-Easy-",                      Chara.Marisa,    Level.Easy) },
        {  38, new("星符「ドラゴンメテオ」",                            Chara.Marisa,    Level.Normal) },
        {  39, new("星符「ドラゴンメテオ」-Hard-",                      Chara.Marisa,    Level.Hard) },
        {  40, new("星符「ドラゴンメテオ」-Lunatic-",                   Chara.Marisa,    Level.Lunatic) },
        {  41, new("符の壱「連続殺人ドール」-Easy-",                    Chara.Sakuya,    Level.Easy) },
        {  42, new("符の壱「連続殺人ドール」",                          Chara.Sakuya,    Level.Normal) },
        {  43, new("符の壱「連続殺人ドール」-Hard-",                    Chara.Sakuya,    Level.Hard) },
        {  44, new("符の壱「連続殺人ドール」-Lunatic-",                 Chara.Sakuya,    Level.Lunatic) },
        {  45, new("符の弐「チェックメイド」-Easy-",                    Chara.Sakuya,    Level.Easy) },
        {  46, new("符の弐「チェックメイド」",                          Chara.Sakuya,    Level.Normal) },
        {  47, new("符の弐「チェックメイド」-Hard-",                    Chara.Sakuya,    Level.Hard) },
        {  48, new("符の弐「チェックメイド」-Lunatic-",                 Chara.Sakuya,    Level.Lunatic) },
        {  49, new("手品「クロースアップ殺人鬼」-Easy-",                Chara.Sakuya,    Level.Easy) },
        {  50, new("手品「クロースアップ殺人鬼」",                      Chara.Sakuya,    Level.Normal) },
        {  51, new("手品「クロースアップ殺人鬼」-Hard-",                Chara.Sakuya,    Level.Hard) },
        {  52, new("手品「クロースアップ殺人鬼」-Lunatic-",             Chara.Sakuya,    Level.Lunatic) },
        {  53, new("符の壱「アーティフルチャンター」-Easy-",            Chara.Alice,     Level.Easy) },
        {  54, new("符の壱「アーティフルチャンター」",                  Chara.Alice,     Level.Normal) },
        {  55, new("符の壱「アーティフルチャンター」-Hard-",            Chara.Alice,     Level.Hard) },
        {  56, new("符の壱「アーティフルチャンター」-Lunatic-",         Chara.Alice,     Level.Lunatic) },
        {  57, new("符の弐「ドールクルセイダー」-Easy-",                Chara.Alice,     Level.Easy) },
        {  58, new("符の弐「ドールクルセイダー」",                      Chara.Alice,     Level.Normal) },
        {  59, new("符の弐「ドールクルセイダー」-Hard-",                Chara.Alice,     Level.Hard) },
        {  60, new("符の弐「ドールクルセイダー」-Lunatic-",             Chara.Alice,     Level.Lunatic) },
        {  61, new("魔光「デヴィリーライトレイ」-Easy-",                Chara.Alice,     Level.Easy) },
        {  62, new("魔光「デヴィリーライトレイ」",                      Chara.Alice,     Level.Normal) },
        {  63, new("魔光「デヴィリーライトレイ」-Hard-",                Chara.Alice,     Level.Hard) },
        {  64, new("魔光「デヴィリーライトレイ」-Lunatic-",             Chara.Alice,     Level.Lunatic) },
        {  65, new("符の壱「セントエルモエクスプロージョン」-Easy-",    Chara.Patchouli, Level.Easy) },
        {  66, new("符の壱「セントエルモエクスプロージョン」",          Chara.Patchouli, Level.Normal) },
        {  67, new("符の壱「セントエルモエクスプロージョン」-Hard-",    Chara.Patchouli, Level.Hard) },
        {  68, new("符の壱「セントエルモエクスプロージョン」-Lunatic-", Chara.Patchouli, Level.Lunatic) },
        {  69, new("符の弐「デリュージュフォーティディ」-Easy-",        Chara.Patchouli, Level.Easy) },
        {  70, new("符の弐「デリュージュフォーティディ」",              Chara.Patchouli, Level.Normal) },
        {  71, new("符の弐「デリュージュフォーティディ」-Hard-",        Chara.Patchouli, Level.Hard) },
        {  72, new("符の弐「デリュージュフォーティディ」-Lunatic-",     Chara.Patchouli, Level.Lunatic) },
        {  73, new("金土符「ジンジャガスト」-Easy-",                    Chara.Patchouli, Level.Easy) },
        {  74, new("金土符「ジンジャガスト」",                          Chara.Patchouli, Level.Normal) },
        {  75, new("金土符「ジンジャガスト」-Hard-",                    Chara.Patchouli, Level.Hard) },
        {  76, new("金土符「ジンジャガスト」-Lunatic-",                 Chara.Patchouli, Level.Lunatic) },
        {  77, new("符の壱「二重の苦輪」-Easy-",                        Chara.Youmu,     Level.Easy) },
        {  78, new("符の壱「二重の苦輪」",                              Chara.Youmu,     Level.Normal) },
        {  79, new("符の壱「二重の苦輪」-Hard-",                        Chara.Youmu,     Level.Hard) },
        {  80, new("符の壱「二重の苦輪」-Lunatic-",                     Chara.Youmu,     Level.Lunatic) },
        {  81, new("符の弐「心眼迷想斬」-Easy-",                        Chara.Youmu,     Level.Easy) },
        {  82, new("符の弐「心眼迷想斬」",                              Chara.Youmu,     Level.Normal) },
        {  83, new("符の弐「心眼迷想斬」-Hard-",                        Chara.Youmu,     Level.Hard) },
        {  84, new("符の弐「心眼迷想斬」-Lunatic-",                     Chara.Youmu,     Level.Lunatic) },
        {  85, new("符の参「業風神閃斬」-Easy-",                        Chara.Youmu,     Level.Easy) },
        {  86, new("符の参「業風神閃斬」",                              Chara.Youmu,     Level.Normal) },
        {  87, new("符の参「業風神閃斬」-Hard-",                        Chara.Youmu,     Level.Hard) },
        {  88, new("符の参「業風神閃斬」-Lunatic-",                     Chara.Youmu,     Level.Lunatic) },
        {  89, new("奥義「西行春風斬」",                                Chara.Youmu,     Level.Easy) },
        {  90, new("奥義「西行春風斬」",                                Chara.Youmu,     Level.Normal) },
        {  91, new("奥義「西行春風斬」",                                Chara.Youmu,     Level.Hard) },
        {  92, new("奥義「西行春風斬」",                                Chara.Youmu,     Level.Lunatic) },
        {  93, new("符の壱「バッドレディスクランブル」-Easy-",          Chara.Remilia,   Level.Easy) },
        {  94, new("符の壱「バッドレディスクランブル」",                Chara.Remilia,   Level.Normal) },
        {  95, new("符の壱「バッドレディスクランブル」-Hard-",          Chara.Remilia,   Level.Hard) },
        {  96, new("符の壱「バッドレディスクランブル」-Lunatic-",       Chara.Remilia,   Level.Lunatic) },
        {  97, new("符の弐「マイハートブレイク」-Easy-",                Chara.Remilia,   Level.Easy) },
        {  98, new("符の弐「マイハートブレイク」",                      Chara.Remilia,   Level.Normal) },
        {  99, new("符の弐「マイハートブレイク」-Hard-",                Chara.Remilia,   Level.Hard) },
        { 100, new("符の弐「マイハートブレイク」-Lunatic-",             Chara.Remilia,   Level.Lunatic) },
        { 101, new("符の参「ヘルカタストロフィ」-Easy-",                Chara.Remilia,   Level.Easy) },
        { 102, new("符の参「ヘルカタストロフィ」",                      Chara.Remilia,   Level.Normal) },
        { 103, new("符の参「ヘルカタストロフィ」-Hard-",                Chara.Remilia,   Level.Hard) },
        { 104, new("符の参「ヘルカタストロフィ」-Lunatic-",             Chara.Remilia,   Level.Lunatic) },
        { 105, new("夜符「クイーン・オブ・ミッドナイト」-Easy-",        Chara.Remilia,   Level.Easy) },
        { 106, new("夜符「クイーン・オブ・ミッドナイト」",              Chara.Remilia,   Level.Normal) },
        { 107, new("夜符「クイーン・オブ・ミッドナイト」-Hard-",        Chara.Remilia,   Level.Hard) },
        { 108, new("夜符「クイーン・オブ・ミッドナイト」-Lunatic-",     Chara.Remilia,   Level.Lunatic) },
        { 109, new("「レッドマジック」",                                Chara.Remilia,   Level.Easy) },
        { 110, new("「レッドマジック」",                                Chara.Remilia,   Level.Normal) },
        { 111, new("「レッドマジック」",                                Chara.Remilia,   Level.Hard) },
        { 112, new("「レッドマジック」",                                Chara.Remilia,   Level.Lunatic) },
        { 113, new("符の壱「幽夢の胡蝶」-Easy-",                        Chara.Yuyuko,    Level.Easy) },
        { 114, new("符の壱「幽夢の胡蝶」",                              Chara.Yuyuko,    Level.Normal) },
        { 115, new("符の壱「幽夢の胡蝶」-Hard-",                        Chara.Yuyuko,    Level.Hard) },
        { 116, new("符の壱「幽夢の胡蝶」-Lunatic-",                     Chara.Yuyuko,    Level.Lunatic) },
        { 117, new("符の弐「白玉楼の垂れ彼岸」-Easy-",                  Chara.Yuyuko,    Level.Easy) },
        { 118, new("符の弐「白玉楼の垂れ彼岸」",                        Chara.Yuyuko,    Level.Normal) },
        { 119, new("符の弐「白玉楼の垂れ彼岸」-Hard-",                  Chara.Yuyuko,    Level.Hard) },
        { 120, new("符の弐「白玉楼の垂れ彼岸」-Lunatic-",               Chara.Yuyuko,    Level.Lunatic) },
        { 121, new("符の参「果てしなく昔の死地」-Easy-",                Chara.Yuyuko,    Level.Easy) },
        { 122, new("符の参「果てしなく昔の死地」",                      Chara.Yuyuko,    Level.Normal) },
        { 123, new("符の参「果てしなく昔の死地」-Hard-",                Chara.Yuyuko,    Level.Hard) },
        { 124, new("符の参「果てしなく昔の死地」-Lunatic-",             Chara.Yuyuko,    Level.Lunatic) },
        { 125, new("桜花「未練未酌宴」-Easy-",                          Chara.Yuyuko,    Level.Easy) },
        { 126, new("桜花「未練未酌宴」",                                Chara.Yuyuko,    Level.Normal) },
        { 127, new("桜花「未練未酌宴」-Hard-",                          Chara.Yuyuko,    Level.Hard) },
        { 128, new("桜花「未練未酌宴」-Lunatic-",                       Chara.Yuyuko,    Level.Lunatic) },
        { 129, new("「紫(むらさき)の彼岸は遅れて輝く」",                Chara.Yuyuko,    Level.Easy) },
        { 130, new("「紫(むらさき)の彼岸は遅れて輝く」",                Chara.Yuyuko,    Level.Normal) },
        { 131, new("「紫(むらさき)の彼岸は遅れて輝く」",                Chara.Yuyuko,    Level.Hard) },
        { 132, new("「紫(むらさき)の彼岸は遅れて輝く」",                Chara.Yuyuko,    Level.Lunatic) },
        { 133, new("符の壱「四重結界」-Easy-",                          Chara.Yukari,    Level.Easy) },
        { 134, new("符の壱「四重結界」",                                Chara.Yukari,    Level.Normal) },
        { 135, new("符の壱「四重結界」-Hard-",                          Chara.Yukari,    Level.Hard) },
        { 136, new("符の壱「四重結界」-Lunatic-",                       Chara.Yukari,    Level.Lunatic) },
        { 137, new("符の弐「八雲卍傘」-Easy-",                          Chara.Yukari,    Level.Easy) },
        { 138, new("符の弐「八雲卍傘」",                                Chara.Yukari,    Level.Normal) },
        { 139, new("符の弐「八雲卍傘」-Hard-",                          Chara.Yukari,    Level.Hard) },
        { 140, new("符の弐「八雲卍傘」-Lunatic-",                       Chara.Yukari,    Level.Lunatic) },
        { 141, new("符の参「八雲藍」-Easy-",                            Chara.Yukari,    Level.Easy) },
        { 142, new("符の参「八雲藍」",                                  Chara.Yukari,    Level.Normal) },
        { 143, new("符の参「八雲藍」-Hard-",                            Chara.Yukari,    Level.Hard) },
        { 144, new("符の参「八雲藍」-Lunatic-",                         Chara.Yukari,    Level.Lunatic) },
        { 145, new("外力「無限の超高速飛行体」-Easy-",                  Chara.Yukari,    Level.Easy) },
        { 146, new("外力「無限の超高速飛行体」",                        Chara.Yukari,    Level.Normal) },
        { 147, new("外力「無限の超高速飛行体」-Hard-",                  Chara.Yukari,    Level.Hard) },
        { 148, new("外力「無限の超高速飛行体」-Lunatic-",               Chara.Yukari,    Level.Lunatic) },
        { 149, new("幻想「第一種永久機関」",                            Chara.Yukari,    Level.Easy) },
        { 150, new("幻想「第一種永久機関」",                            Chara.Yukari,    Level.Normal) },
        { 151, new("幻想「第一種永久機関」",                            Chara.Yukari,    Level.Hard) },
        { 152, new("幻想「第一種永久機関」",                            Chara.Yukari,    Level.Lunatic) },
        { 153, new("符の壱「投擲の天岩戸」-Easy-",                      Chara.Suika,     Level.Easy) },
        { 154, new("符の壱「投擲の天岩戸」",                            Chara.Suika,     Level.Normal) },
        { 155, new("符の壱「投擲の天岩戸」-Hard-",                      Chara.Suika,     Level.Hard) },
        { 156, new("符の壱「投擲の天岩戸」-Lunatic-",                   Chara.Suika,     Level.Lunatic) },
        { 157, new("符の弐「坤軸の大鬼」-Easy-",                        Chara.Suika,     Level.Easy) },
        { 158, new("符の弐「坤軸の大鬼」",                              Chara.Suika,     Level.Normal) },
        { 159, new("符の弐「坤軸の大鬼」-Hard-",                        Chara.Suika,     Level.Hard) },
        { 160, new("符の弐「坤軸の大鬼」-Lunatic-",                     Chara.Suika,     Level.Lunatic) },
        { 161, new("符の参「追儺返しブラックホール」-Easy-",            Chara.Suika,     Level.Easy) },
        { 162, new("符の参「追儺返しブラックホール」",                  Chara.Suika,     Level.Normal) },
        { 163, new("符の参「追儺返しブラックホール」-Hard-",            Chara.Suika,     Level.Hard) },
        { 164, new("符の参「追儺返しブラックホール」-Lunatic-",         Chara.Suika,     Level.Lunatic) },
        { 165, new("鬼火「超高密度燐禍術」-Easy-",                      Chara.Suika,     Level.Easy) },
        { 166, new("鬼火「超高密度燐禍術」",                            Chara.Suika,     Level.Normal) },
        { 167, new("鬼火「超高密度燐禍術」-Hard-",                      Chara.Suika,     Level.Hard) },
        { 168, new("鬼火「超高密度燐禍術」-Lunatic-",                   Chara.Suika,     Level.Lunatic) },
        { 169, new("疎符「六里霧中」-Easy-",                            Chara.Suika,     Level.Easy) },
        { 170, new("疎符「六里霧中」",                                  Chara.Suika,     Level.Normal) },
        { 171, new("疎符「六里霧中」-Hard-",                            Chara.Suika,     Level.Hard) },
        { 172, new("疎符「六里霧中」-Lunatic-",                         Chara.Suika,     Level.Lunatic) },
        { 173, new("「百万鬼夜行」",                                    Chara.Suika,     Level.Easy) },
        { 174, new("「百万鬼夜行」",                                    Chara.Suika,     Level.Normal) },
        { 175, new("「百万鬼夜行」",                                    Chara.Suika,     Level.Hard) },
        { 176, new("「百万鬼夜行」",                                    Chara.Suika,     Level.Lunatic) },
    };

    /// <summary>
    /// Gets the dictionary of spell card identifiers keyed by playable character.
    /// </summary>
    public static IReadOnlyDictionary<Chara, IEnumerable<int>> CardIdTable { get; } = InitializeCardIdTable();

    private static Dictionary<Chara, IEnumerable<int>> InitializeCardIdTable()
    {
        var charaStageEnemyTable = new Dictionary<Chara, IEnumerable<(Stage Stage, Chara Enemy)>>
        {
            {
                Chara.Reimu,
                new[]
                {
                    (Stage.One,   Chara.Marisa),
                    (Stage.Two,   Chara.Alice),
                    (Stage.Three, Chara.Youmu),
                    (Stage.Four,  Chara.Sakuya),
                    (Stage.Five,  Chara.Remilia),
                    (Stage.Six,   Chara.Yukari),
                    (Stage.Seven, Chara.Suika),
                }
            },
            {
                Chara.Marisa,
                new[]
                {
                    (Stage.One,   Chara.Alice),
                    (Stage.Two,   Chara.Sakuya),
                    (Stage.Three, Chara.Patchouli),
                    (Stage.Four,  Chara.Remilia),
                    (Stage.Five,  Chara.Reimu),
                    (Stage.Six,   Chara.Yukari),
                    (Stage.Seven, Chara.Suika),
                }
            },
            {
                Chara.Sakuya,
                new[]
                {
                    (Stage.One,   Chara.Reimu),
                    (Stage.Two,   Chara.Alice),
                    (Stage.Three, Chara.Marisa),
                    (Stage.Four,  Chara.Youmu),
                    (Stage.Five,  Chara.Yuyuko),
                    (Stage.Six,   Chara.Yukari),
                    (Stage.Seven, Chara.Suika),
                }
            },
            {
                Chara.Alice,
                new[]
                {
                    (Stage.One,   Chara.Marisa),
                    (Stage.Two,   Chara.Reimu),
                    (Stage.Three, Chara.Sakuya),
                    (Stage.Four,  Chara.Patchouli),
                    (Stage.Five,  Chara.Youmu),
                    (Stage.Six,   Chara.Yukari),
                    (Stage.Seven, Chara.Suika),
                }
            },
            {
                Chara.Patchouli,
                new[]
                {
                    (Stage.One,   Chara.Marisa),
                    (Stage.Two,   Chara.Sakuya),
                    (Stage.Three, Chara.Alice),
                    (Stage.Four,  Chara.Youmu),
                    (Stage.Five,  Chara.Yuyuko),
                    (Stage.Six,   Chara.Yukari),
                    (Stage.Seven, Chara.Suika),
                }
            },
            {
                Chara.Youmu,
                new[]
                {
                    (Stage.One,   Chara.Reimu),
                    (Stage.Two,   Chara.Marisa),
                    (Stage.Three, Chara.Patchouli),
                    (Stage.Four,  Chara.Sakuya),
                    (Stage.Five,  Chara.Remilia),
                    (Stage.Six,   Chara.Yukari),
                    (Stage.Seven, Chara.Suika),
                }
            },
            {
                Chara.Remilia,
                new[]
                {
                    (Stage.One,   Chara.Sakuya),
                    (Stage.Two,   Chara.Marisa),
                    (Stage.Three, Chara.Reimu),
                    (Stage.Four,  Chara.Youmu),
                    (Stage.Five,  Chara.Yuyuko),
                    (Stage.Six,   Chara.Yukari),
                    (Stage.Seven, Chara.Suika),
                }
            },
            {
                Chara.Yuyuko,
                new[]
                {
                    (Stage.One,   Chara.Youmu),
                    (Stage.Two,   Chara.Marisa),
                    (Stage.Three, Chara.Reimu),
                    (Stage.Four,  Chara.Sakuya),
                    (Stage.Five,  Chara.Remilia),
                    (Stage.Six,   Chara.Yukari),
                    (Stage.Seven, Chara.Suika),
                }
            },
            {
                Chara.Yukari,
                new[]
                {
                    (Stage.One,   Chara.Remilia),
                    (Stage.Two,   Chara.Alice),
                    (Stage.Three, Chara.Youmu),
                    (Stage.Four,  Chara.Marisa),
                    (Stage.Five,  Chara.Reimu),
                    (Stage.Six,   Chara.Yuyuko),
                    (Stage.Seven, Chara.Suika),
                }
            },
            {
                Chara.Suika,
                new[]
                {
                    (Stage.One,   Chara.Sakuya),
                    (Stage.Two,   Chara.Alice),
                    (Stage.Three, Chara.Youmu),
                    (Stage.Four,  Chara.Patchouli),
                    (Stage.Five,  Chara.Marisa),
                    (Stage.Six,   Chara.Remilia),
                    (Stage.Seven, Chara.Reimu),
                }
            },
        };

        var cardNumberTable = CardTable.ToLookup(static pair => pair.Value.Enemy, static pair => pair.Key);

        return charaStageEnemyTable.ToDictionary(
            static charaStageEnemyPair => charaStageEnemyPair.Key,
            charaStageEnemyPair => charaStageEnemyPair.Value.SelectMany(
                stageEnemyPair => cardNumberTable[stageEnemyPair.Enemy].Take(
                    stageEnemyPair.Stage switch
                    {
                        Stage.One or Stage.Two => 8,
                        Stage.Three or Stage.Four => 12,
                        Stage.Five => 16,
                        Stage.Six => 20,
                        Stage.Seven => 24,
                        _ => 0, // unreachable
                    })));
    }
}

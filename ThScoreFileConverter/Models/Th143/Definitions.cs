//-----------------------------------------------------------------------
// <copyright file="Definitions.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using ThScoreFileConverter.Core.Models.Th143;

namespace ThScoreFileConverter.Models.Th143;

internal static class Definitions
{
    public static IReadOnlyDictionary<(Day Day, int Scene), (Enemy[] Enemies, string Card)> SpellCards { get; } =
        Core.Models.Th143.Definitions.SpellCards;

    public static IReadOnlyList<string> Nicknames { get; } = new List<string>
    {
        "弾幕アマノジャク",
        "ひよっこアマノジャク",
        "慣れてきたアマノジャク",
        "一人前アマノジャク",
        "無敵のアマノジャク",
        "不滅のアマノジャク",
        "伝説のアマノジャク",
        "神話のアマノジャク",
        "全てを敵に回した天邪鬼",
        "逃げ切ったアマノジャク",
        "はじめてのアマノジャク",
        "新たなアイテム使い",
        "ミラクル不思議道具使い",
        "おや、片手が空いていた",
        "そろそろお茶でも",
        "ドライアイにご注意",
        "悟りでも開けるよ",
        "もう痛みを感じない",
        "もしかして、快感？",
        "彼女の屍を超えてゆけ",
        "初日マスター",
        "２日目マスター",
        "３日目マスター",
        "４日目マスター",
        "５日目マスター",
        "６日目マスター",
        "７日目マスター",
        "８日目マスター",
        "９日目マスター",
        "最終日マスター",
        "おひらりさん",
        "カメラ小僧",
        "仕舞いっぱなしの傘",
        "とおりすがりの亡霊さん",
        "たま使い",
        "手持ち花火",
        "お地蔵さん",
        "お人形屋さん",
        "物理で殴れ",
        "反則嫌い",
        "ひらりスター",
        "カメラ大人",
        "お気に入りの傘",
        "もしかして生霊さん？",
        "たま職人",
        "スターマインさん",
        "地蔵菩薩",
        "人形蒐集家",
        "ピコピコハンマー",
        "正々堂々屋さん",
        "ひらりマスター",
        "カメラ紳士",
        "傘ハウス",
        "りっぱな霊体",
        "たま仙人",
        "クレイジーボマー",
        "まさに地蔵の様な人",
        "人形原型師",
        "小槌でスマッシュ！",
        "モッタイナイ精神",
        "ひらり宇宙神",
        "カメラ魔人",
        "傘のパラダイス",
        "生まれながらの亡霊",
        "たまたまデスター",
        "花火曼荼羅",
        "世界は地蔵で廻っている",
        "呪われ人形メイク",
        "脳みそ金時",
        "究極反則生命体",
    };

    public static string FormatPrefix { get; } = "%T143";
}

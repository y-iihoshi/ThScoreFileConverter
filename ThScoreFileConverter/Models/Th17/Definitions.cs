//-----------------------------------------------------------------------
// <copyright file="Definitions.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using ThScoreFileConverter.Core.Models.Th17;
using CardInfo = ThScoreFileConverter.Core.Models.SpellCardInfo<
    ThScoreFileConverter.Core.Models.Stage, ThScoreFileConverter.Core.Models.Level>;

namespace ThScoreFileConverter.Models.Th17;

internal static class Definitions
{
    public static IReadOnlyDictionary<int, CardInfo> CardTable { get; } = Core.Models.Th17.Definitions.CardTable;

    // Thanks to en.touhouwiki.net
    public static IReadOnlyList<string> Achievements { get; } = new List<string>
    {
        "霊夢（オオカミ霊）クリア",
        "霊夢（カワウソ霊）クリア",
        "霊夢（オオワシ霊）クリア",
        "魔理沙（オオカミ霊）クリア",
        "魔理沙（カワウソ霊）クリア",
        "魔理沙（オオワシ霊）クリア",
        "妖夢（オオカミ霊）クリア",
        "妖夢（カワウソ霊）クリア",
        "妖夢（オオワシ霊）クリア",
        "霊夢バッドエンド",
        "魔理沙BADクリア",
        "妖夢BADクリア",
        "霊夢（オオカミ霊）EXクリア",
        "霊夢（カワウソ霊）EXクリア",
        "霊夢（オオワシ霊）EXクリア",
        "魔理沙（オオカミ霊）EXクリア",
        "魔理沙（カワウソ霊）EXクリア",
        "魔理沙（オオワシ霊）EXクリア",
        "妖夢（オオカミ霊）EXクリア",
        "妖夢（カワウソ霊）EXクリア",
        "妖夢（オオワシ霊）EXクリア",
        "イージークリア",
        "ノーマルクリア",
        "ハードクリア",
        "ルナティッククリア",
        "イージーノーコンクリア",
        "ノーマルノーコンクリア",
        "ハードNCクリア",
        "ルナティックNCクリア",
        "パーフェクトクリア",
        "畜生界の破壊神",
        "畜生を蹂躙する者",
        "スペルカードコレクター",
        "クラゲ大好き",
        "そうだ牛乳を飲もう",
        "生まれたてのピヨちゃん",
        "のんびりコツコツと",
        "おーいハニマルさん",
        "土の馬",
        "ひよこが三匹",
    };

    public static string FormatPrefix { get; } = "%T17";

    public static bool IsTotal(CharaWithTotal chara)
    {
        return chara is CharaWithTotal.Total;
    }
}

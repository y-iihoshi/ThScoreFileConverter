//-----------------------------------------------------------------------
// <copyright file="Definitions.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Collections.Generic;
using System.Linq;
using ThScoreFileConverter.Core.Models.Th105;
using StageInfo = ThScoreFileConverter.Core.Models.Th105.StageInfo<ThScoreFileConverter.Core.Models.Th105.Chara>;

namespace ThScoreFileConverter.Models.Th105;

internal static class Definitions
{
    public static IReadOnlyDictionary<int, string> SystemCardNameTable { get; } =
        Core.Models.Th105.Definitions.SystemCardNameTable;

    public static IReadOnlyDictionary<(Chara Chara, int CardId), string> CardNameTable { get; } =
        Core.Models.Th105.Definitions.CardNameTable;

    public static IReadOnlyDictionary<Chara, IReadOnlyList<StageInfo>> StageInfoTable { get; } =
        Core.Models.Th105.Definitions.StageInfoTable;

    public static IReadOnlyDictionary<Chara, IEnumerable<(Chara Enemy, int CardId)>> EnemyCardIdTable { get; } =
        StageInfoTable.ToDictionary(
            stageInfoPair => stageInfoPair.Key,
            stageInfoPair => stageInfoPair.Value.SelectMany(
                stageInfo => stageInfo.CardIds.Select(id => (stageInfo.Enemy, id))));

    public static string FormatPrefix { get; } = "%T105";

    public static bool HasStory(Chara chara)
    {
        return Enum.IsDefined(typeof(Chara), chara);
    }
}

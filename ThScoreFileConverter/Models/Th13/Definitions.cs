//-----------------------------------------------------------------------
// <copyright file="Definitions.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using ThScoreFileConverter.Core.Models.Th13;
using CardInfo = ThScoreFileConverter.Core.Models.SpellCardInfo<
    ThScoreFileConverter.Core.Models.Th13.StagePractice, ThScoreFileConverter.Core.Models.Th13.LevelPractice>;

namespace ThScoreFileConverter.Models.Th13;

internal static class Definitions
{
    public static IReadOnlyDictionary<int, CardInfo> CardTable { get; } = Core.Models.Th13.Definitions.CardTable;

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

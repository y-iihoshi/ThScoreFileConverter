//-----------------------------------------------------------------------
// <copyright file="Definitions.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using ThScoreFileConverter.Core.Models.Th15;
using CardInfo = ThScoreFileConverter.Core.Models.SpellCardInfo<
    ThScoreFileConverter.Core.Models.Stage, ThScoreFileConverter.Core.Models.Level>;

namespace ThScoreFileConverter.Models.Th15;

internal static class Definitions
{
    // Thanks to thwiki.info
    public static IReadOnlyDictionary<int, CardInfo> CardTable { get; } = Core.Models.Th15.Definitions.CardTable;

    public static string FormatPrefix { get; } = "%T15";

    public static bool IsTotal(CharaWithTotal chara)
    {
        return chara is CharaWithTotal.Total;
    }
}

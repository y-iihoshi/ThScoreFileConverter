//-----------------------------------------------------------------------
// <copyright file="Definitions.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using ThScoreFileConverter.Core.Models.Th11;
using CardInfo = ThScoreFileConverter.Core.Models.SpellCardInfo<
    ThScoreFileConverter.Core.Models.Stage, ThScoreFileConverter.Core.Models.Level>;

namespace ThScoreFileConverter.Models.Th11;

internal static class Definitions
{
    public static IReadOnlyDictionary<int, CardInfo> CardTable { get; } = Core.Models.Th11.Definitions.CardTable;

    public static string FormatPrefix { get; } = "%T11";

    public static bool IsTotal(CharaWithTotal chara)
    {
        return chara is CharaWithTotal.Total;
    }
}

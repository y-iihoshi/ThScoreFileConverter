//-----------------------------------------------------------------------
// <copyright file="Definitions.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using ThScoreFileConverter.Core.Models.Th16;
using CardInfo = ThScoreFileConverter.Core.Models.SpellCardInfo<
    ThScoreFileConverter.Core.Models.Stage, ThScoreFileConverter.Core.Models.Level>;

namespace ThScoreFileConverter.Models.Th16;

internal static class Definitions
{
    public static IReadOnlyDictionary<int, CardInfo> CardTable { get; } = Core.Models.Th16.Definitions.CardTable;

    public static string FormatPrefix { get; } = "%T16";

    public static bool IsTotal(CharaWithTotal chara)
    {
        return chara is CharaWithTotal.Total;
    }
}

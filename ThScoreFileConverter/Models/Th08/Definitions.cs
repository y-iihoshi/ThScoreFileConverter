//-----------------------------------------------------------------------
// <copyright file="Definitions.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using CardInfo = ThScoreFileConverter.Core.Models.SpellCardInfo<
    ThScoreFileConverter.Core.Models.Th08.StagePractice, ThScoreFileConverter.Core.Models.Th08.LevelPractice>;

namespace ThScoreFileConverter.Models.Th08;

internal static class Definitions
{
    public static IReadOnlyDictionary<int, CardInfo> CardTable { get; } = Core.Models.Th08.Definitions.CardTable;

    public static IReadOnlyList<IHighScore> InitialRanking { get; } =
        Enumerable.Range(1, 10).Reverse().Select(index => new HighScore((uint)index * 10000)).ToList();

    public static string FormatPrefix { get; } = "%T08";
}

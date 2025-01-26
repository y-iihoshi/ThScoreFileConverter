//-----------------------------------------------------------------------
// <copyright file="IClearDataPerGameMode.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using ThScoreFileConverter.Core.Models;

namespace ThScoreFileConverter.Models.Th15;

internal interface IClearDataPerGameMode
{
    IReadOnlyDictionary<int, Th13.ISpellCard<Level>> Cards { get; }

    IReadOnlyDictionary<LevelWithTotal, int> ClearCounts { get; }

    IReadOnlyDictionary<LevelWithTotal, int> ClearFlags { get; }

    int PlayTime { get; }

    IReadOnlyDictionary<LevelWithTotal, IReadOnlyList<IScoreData>> Rankings { get; }

    int TotalPlayCount { get; }
}

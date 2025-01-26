//-----------------------------------------------------------------------
// <copyright file="IClearData.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th128;
using ThScoreFileConverter.Models.Th10;

namespace ThScoreFileConverter.Models.Th128;

internal interface IClearData : Th095.IChapter
{
    IReadOnlyDictionary<Level, int> ClearCounts { get; }

    int PlayTime { get; }

    IReadOnlyDictionary<Level, IReadOnlyList<IScoreData<StageProgress>>> Rankings { get; }

    RouteWithTotal Route { get; }

    int TotalPlayCount { get; }
}

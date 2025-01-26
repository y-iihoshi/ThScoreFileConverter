//-----------------------------------------------------------------------
// <copyright file="IPlayStatus.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using ThScoreFileConverter.Core.Models;

namespace ThScoreFileConverter.Models.Th08;

internal interface IPlayStatus : Th06.IChapter
{
    IEnumerable<byte> BgmFlags { get; }

    IReadOnlyDictionary<Level, IPlayCount> PlayCounts { get; }

    IPlayCount TotalPlayCount { get; }

    Time TotalPlayTime { get; }

    Time TotalRunningTime { get; }
}

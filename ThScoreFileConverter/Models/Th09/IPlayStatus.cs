//-----------------------------------------------------------------------
// <copyright file="IPlayStatus.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using ThScoreFileConverter.Core.Models.Th09;

namespace ThScoreFileConverter.Models.Th09;

internal interface IPlayStatus : Th06.IChapter
{
    IEnumerable<byte> BgmFlags { get; }

    IReadOnlyDictionary<Chara, IClearCount> ClearCounts { get; }

    IReadOnlyDictionary<Chara, byte> ExtraFlags { get; }

    IReadOnlyDictionary<Chara, byte> MatchFlags { get; }

    IReadOnlyDictionary<Chara, byte> StoryFlags { get; }

    Time TotalPlayTime { get; }

    Time TotalRunningTime { get; }
}

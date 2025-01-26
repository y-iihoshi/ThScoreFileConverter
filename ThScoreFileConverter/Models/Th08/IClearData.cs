//-----------------------------------------------------------------------
// <copyright file="IClearData.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th08;

namespace ThScoreFileConverter.Models.Th08;

internal interface IClearData : Th06.IChapter
{
    CharaWithTotal Chara { get; }

    IReadOnlyDictionary<Level, PlayableStages> PracticeFlags { get; }

    IReadOnlyDictionary<Level, PlayableStages> StoryFlags { get; }
}

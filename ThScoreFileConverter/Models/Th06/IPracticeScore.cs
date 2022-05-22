//-----------------------------------------------------------------------
// <copyright file="IPracticeScore.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th06;

namespace ThScoreFileConverter.Models.Th06;

internal interface IPracticeScore : IChapter
{
    Chara Chara { get; }

    int HighScore { get; }

    Level Level { get; }

    Stage Stage { get; }
}

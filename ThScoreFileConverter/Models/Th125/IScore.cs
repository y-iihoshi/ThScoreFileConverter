//-----------------------------------------------------------------------
// <copyright file="IScore.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

namespace ThScoreFileConverter.Models.Th125;

internal interface IScore : Th095.IChapter
{
    int BestshotScore { get; }

    Chara Chara { get; }

    uint DateTime { get; }

    int FirstSuccess { get; }

    int HighScore { get; }

    (Level Level, int Scene) LevelScene { get; }

    int TrialCount { get; }
}

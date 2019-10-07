//-----------------------------------------------------------------------
// <copyright file="IScore.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

namespace ThScoreFileConverter.Models.Th165
{
    internal interface IScore : Th095.IChapter
    {
        int ChallengeCount { get; }

        int ClearCount { get; }

        int HighScore { get; }

        int Number { get; }

        int NumPhotos { get; }
    }
}

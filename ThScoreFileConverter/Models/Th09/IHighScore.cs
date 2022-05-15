//-----------------------------------------------------------------------
// <copyright file="IHighScore.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;

namespace ThScoreFileConverter.Models.Th09;

internal interface IHighScore : Th06.IChapter
{
    Chara Chara { get; }

    byte ContinueCount { get; }

    IEnumerable<byte> Date { get; }

    Level Level { get; }

    IEnumerable<byte> Name { get; }

    short Rank { get; }

    uint Score { get; }
}

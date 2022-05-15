//-----------------------------------------------------------------------
// <copyright file="IHighScore.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Collections.Generic;

namespace ThScoreFileConverter.Models.Th07;

internal interface IHighScore<TChara, TLevel, TStageProgress> : Th06.IHighScore<TChara, TLevel, TStageProgress>
    where TChara : struct, Enum
    where TLevel : struct, Enum
    where TStageProgress : struct, Enum
{
    ushort ContinueCount { get; }

    IEnumerable<byte> Date { get; }

    float SlowRate { get; }
}

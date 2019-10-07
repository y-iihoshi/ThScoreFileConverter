//-----------------------------------------------------------------------
// <copyright file="IHighScore.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Collections.Generic;

namespace ThScoreFileConverter.Models.Th06
{
    internal interface IHighScore<TChara, TLevel, TStageProgress> : IChapter
        where TChara : struct, Enum
        where TLevel : struct, Enum
        where TStageProgress : struct, Enum
    {
        TChara Chara { get; }

        TLevel Level { get; }

        IEnumerable<byte> Name { get; }

        uint Score { get; }

        TStageProgress StageProgress { get; }
    }
}

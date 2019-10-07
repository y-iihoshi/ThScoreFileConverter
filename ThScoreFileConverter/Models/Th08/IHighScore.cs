//-----------------------------------------------------------------------
// <copyright file="IHighScore.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Collections.Generic;

namespace ThScoreFileConverter.Models.Th08
{
    internal interface IHighScore<TChara, TLevel, TStageProgress> : Th07.IHighScore<TChara, TLevel, TStageProgress>
        where TChara : struct, Enum
        where TLevel : struct, Enum
        where TStageProgress : struct, Enum
    {
        int BombCount { get; }

        IReadOnlyDictionary<int, byte> CardFlags { get; }

        int HumanRate { get; }

        int LastSpellCount { get; }

        int MissCount { get; }

        int PauseCount { get; }

        byte PlayerNum { get; }

        uint PlayTime { get; }

        int PointItem { get; }

        int TimePoint { get; }
    }
}

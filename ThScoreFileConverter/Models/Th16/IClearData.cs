﻿//-----------------------------------------------------------------------
// <copyright file="IClearData.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;

namespace ThScoreFileConverter.Models.Th16
{
    internal interface IClearData : Th095.IChapter
    {
        IReadOnlyDictionary<int, Th13.ISpellCard<Level>> Cards { get; }

        CharaWithTotal Chara { get; }

        IReadOnlyDictionary<LevelWithTotal, int> ClearCounts { get; }

        IReadOnlyDictionary<LevelWithTotal, int> ClearFlags { get; }

        int PlayTime { get; }

        IReadOnlyDictionary<(Level, StagePractice), Th13.IPractice> Practices { get; }

        IReadOnlyDictionary<LevelWithTotal, IReadOnlyList<IScoreData>> Rankings { get; }

        int TotalPlayCount { get; }
    }
}

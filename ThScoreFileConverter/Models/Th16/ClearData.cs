﻿//-----------------------------------------------------------------------
// <copyright file="ClearData.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using ClearDataBase = ThScoreFileConverter.Models.Th16.ClearDataBase<
    ThScoreFileConverter.Core.Models.Th16.CharaWithTotal,
    ThScoreFileConverter.Core.Models.Level,
    ThScoreFileConverter.Core.Models.Level,
    ThScoreFileConverter.Core.Models.Th14.LevelPracticeWithTotal,
    ThScoreFileConverter.Core.Models.Th14.StagePractice,
    ThScoreFileConverter.Models.Th16.IScoreData,
    ThScoreFileConverter.Models.Th16.ScoreData>;

namespace ThScoreFileConverter.Models.Th16;

internal sealed class ClearData(Th10.Chapter chapter) // per character
    : ClearDataBase(chapter, ValidVersion, ValidSize, Definitions.CardTable.Count)
{
    public const ushort ValidVersion = 0x0001;
    public const int ValidSize = 0x00005318;

    public static new bool CanInitialize(Th10.Chapter chapter)
    {
        return ClearDataBase.CanInitialize(chapter)
            && (chapter.Version == ValidVersion)
            && (chapter.Size == ValidSize);
    }
}

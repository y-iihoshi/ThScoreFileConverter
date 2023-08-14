//-----------------------------------------------------------------------
// <copyright file="ClearData.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using ClearDataBase = ThScoreFileConverter.Models.Th13.ClearDataBase<
    ThScoreFileConverter.Core.Models.Th14.CharaWithTotal,
    ThScoreFileConverter.Core.Models.Level,
    ThScoreFileConverter.Core.Models.Th14.LevelPractice,
    ThScoreFileConverter.Core.Models.Th14.LevelPracticeWithTotal,
    ThScoreFileConverter.Models.Th14.StagePractice>;

namespace ThScoreFileConverter.Models.Th14;

internal class ClearData : ClearDataBase    // per character
{
    public const int ValidSize = 0x00005298;

    public ClearData(Th10.Chapter chapter)
        : base(chapter, ValidSize, Definitions.CardTable.Count)
    {
    }

    public static new bool CanInitialize(Th10.Chapter chapter)
    {
        return ClearDataBase.CanInitialize(chapter) && (chapter.Size == ValidSize);
    }
}

//-----------------------------------------------------------------------
// <copyright file="ClearData.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using ClearDataBase = ThScoreFileConverter.Models.Th13.ClearDataBase<
    ThScoreFileConverter.Core.Models.Th13.CharaWithTotal,
    ThScoreFileConverter.Core.Models.Th13.LevelPractice,
    ThScoreFileConverter.Core.Models.Th13.LevelPractice,
    ThScoreFileConverter.Core.Models.Th13.LevelPracticeWithTotal,
    ThScoreFileConverter.Core.Models.Th13.StagePractice>;

namespace ThScoreFileConverter.Models.Th13;

internal sealed class ClearData(Th10.Chapter chapter) // per character
    : ClearDataBase(chapter, ValidSize, Definitions.CardTable.Count)
{
    public const int ValidSize = 0x000056DC;

    public static new bool CanInitialize(Th10.Chapter chapter)
    {
        return ClearDataBase.CanInitialize(chapter) && (chapter.Size == ValidSize);
    }
}

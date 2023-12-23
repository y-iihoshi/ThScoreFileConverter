//-----------------------------------------------------------------------
// <copyright file="ClearData.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using ThScoreFileConverter.Core.Models.Th10;

namespace ThScoreFileConverter.Models.Th10;

internal sealed class ClearData(Chapter chapter) // per character
    : ClearDataBase<CharaWithTotal, ScoreData>(chapter, ValidVersion, ValidSize, Definitions.CardTable.Count)
{
    public const ushort ValidVersion = 0x0000;
    public const int ValidSize = 0x0000437C;

    public static new bool CanInitialize(Chapter chapter)
    {
        return ClearDataBase<CharaWithTotal, ScoreData>.CanInitialize(chapter)
            && (chapter.Version == ValidVersion)
            && (chapter.Size == ValidSize);
    }
}

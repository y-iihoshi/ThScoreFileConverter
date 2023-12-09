//-----------------------------------------------------------------------
// <copyright file="ClearData.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using ThScoreFileConverter.Core.Models.Th12;

namespace ThScoreFileConverter.Models.Th12;

internal sealed class ClearData : Th10.ClearDataBase<CharaWithTotal, Th11.ScoreData>   // per character
{
    public const ushort ValidVersion = 0x0002;
    public const int ValidSize = 0x000045F4;

    public ClearData(Th10.Chapter chapter)
        : base(chapter, ValidVersion, ValidSize, Definitions.CardTable.Count)
    {
    }

    public static new bool CanInitialize(Th10.Chapter chapter)
    {
        return Th10.ClearDataBase<CharaWithTotal, Th11.ScoreData>.CanInitialize(chapter)
            && (chapter.Version == ValidVersion)
            && (chapter.Size == ValidSize);
    }
}

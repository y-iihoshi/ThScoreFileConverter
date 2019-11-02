//-----------------------------------------------------------------------
// <copyright file="ClearData.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

namespace ThScoreFileConverter.Models.Th11
{
    internal class ClearData : Th10.ClearDataBase<CharaWithTotal, StageProgress, ScoreData> // per character
    {
        public const ushort ValidVersion = 0x0000;
        public const int ValidSize = 0x000068D4;

        public ClearData(Th10.Chapter chapter)
            : base(chapter, ValidVersion, ValidSize, Definitions.CardTable.Count)
        {
        }

        public static new bool CanInitialize(Th10.Chapter chapter)
        {
            return Th10.ClearDataBase<CharaWithTotal, StageProgress, ScoreData>.CanInitialize(chapter)
                && (chapter.Version == ValidVersion)
                && (chapter.Size == ValidSize);
        }
    }
}

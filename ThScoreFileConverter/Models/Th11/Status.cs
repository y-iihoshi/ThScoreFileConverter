//-----------------------------------------------------------------------
// <copyright file="Status.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

namespace ThScoreFileConverter.Models.Th11
{
    internal class Status : Th10.StatusBase
    {
        public const ushort ValidVersion = 0x0000;
        public const int NumBgms = 17;

        public Status(Th10.Chapter chapter)
            : base(chapter, ValidVersion, NumBgms)
        {
        }

        public static new bool CanInitialize(Th10.Chapter chapter)
        {
            return Th10.StatusBase.CanInitialize(chapter) && (chapter.Version == ValidVersion);
        }
    }
}

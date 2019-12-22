//-----------------------------------------------------------------------
// <copyright file="Status.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

namespace ThScoreFileConverter.Models.Th128
{
    internal class Status : StatusBase
    {
        public const ushort ValidVersion = 0x0002;

        public Status(Th10.Chapter chapter)
            : base(chapter, ValidVersion, 10, 0x18)
        {
        }

        public static new bool CanInitialize(Th10.Chapter chapter)
        {
            return StatusBase.CanInitialize(chapter) && (chapter.Version == ValidVersion);
        }
    }
}

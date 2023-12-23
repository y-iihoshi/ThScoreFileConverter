//-----------------------------------------------------------------------
// <copyright file="Status.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

namespace ThScoreFileConverter.Models.Th10;

internal sealed class Status(Chapter chapter) : StatusBase(chapter, ValidVersion, NumBgms)
{
    public const ushort ValidVersion = 0x0000;
    public const int NumBgms = 18;

    public static new bool CanInitialize(Chapter chapter)
    {
        return StatusBase.CanInitialize(chapter) && (chapter.Version == ValidVersion);
    }
}

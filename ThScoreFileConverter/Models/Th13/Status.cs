//-----------------------------------------------------------------------
// <copyright file="Status.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

namespace ThScoreFileConverter.Models.Th13;

internal sealed class Status : Th128.StatusBase
{
    public const ushort ValidVersion = 0x0001;

    public Status(Th10.Chapter chapter)
        : base(chapter, ValidVersion, 17, 0x11)
    {
    }

    public static new bool CanInitialize(Th10.Chapter chapter)
    {
        return Th128.StatusBase.CanInitialize(chapter) && (chapter.Version == ValidVersion);
    }
}

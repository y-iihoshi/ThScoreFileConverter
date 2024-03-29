﻿//-----------------------------------------------------------------------
// <copyright file="Status.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

namespace ThScoreFileConverter.Models.Th12;

internal sealed class Status(Th10.Chapter chapter) : Th10.StatusBase(chapter, ValidVersion, NumBgms)
{
    public const ushort ValidVersion = 0x0002;
    public const int NumBgms = 17;

    public static new bool CanInitialize(Th10.Chapter chapter)
    {
        return Th10.StatusBase.CanInitialize(chapter) && (chapter.Version == ValidVersion);
    }
}

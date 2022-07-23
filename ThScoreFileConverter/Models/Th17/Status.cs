//-----------------------------------------------------------------------
// <copyright file="Status.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Collections.Generic;
using System.IO;
using ThScoreFileConverter.Extensions;

namespace ThScoreFileConverter.Models.Th17;

internal class Status : Th10.Chapter, IStatus
{
    public const string ValidSignature = "ST";
    public const ushort ValidVersion = 0x0002;
    public const int ValidSize = 0x000004B0;

    public Status(Th10.Chapter chapter)
        : base(chapter, ValidSignature, ValidVersion, ValidSize)
    {
        using var stream = new MemoryStream(this.Data, false);
        using var reader = new BinaryReader(stream);

        this.LastName = reader.ReadExactBytes(10);
        _ = reader.ReadExactBytes(0x10);
        this.BgmFlags = reader.ReadExactBytes(17);
        _ = reader.ReadExactBytes(0x11);
        this.TotalPlayTime = reader.ReadInt32();
        _ = reader.ReadExactBytes(0x4);
        this.Achievements = reader.ReadExactBytes(40);
        _ = reader.ReadExactBytes(0x0438);
    }

    public IEnumerable<byte> LastName { get; }  // The last 2 bytes are always 0x00 ?

    public IEnumerable<byte> BgmFlags { get; }

    public int TotalPlayTime { get; }   // unit: 10ms

    public IEnumerable<byte> Achievements { get; }

    public static bool CanInitialize(Th10.Chapter chapter)
    {
        return chapter.Signature.Equals(ValidSignature, StringComparison.Ordinal)
            && (chapter.Version == ValidVersion)
            && (chapter.Size == ValidSize);
    }
}

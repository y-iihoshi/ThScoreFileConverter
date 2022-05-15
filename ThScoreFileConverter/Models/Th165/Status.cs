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

namespace ThScoreFileConverter.Models.Th165;

internal class Status : Th10.Chapter, IStatus
{
    public const string ValidSignature = "ST";
    public const ushort ValidVersion = 0x0002;
    public const int ValidSize = 0x00000224;

    public Status(Th10.Chapter chapter)
        : base(chapter, ValidSignature, ValidVersion, ValidSize)
    {
        using var stream = new MemoryStream(this.Data, false);
        using var reader = new BinaryReader(stream);

        this.LastName = reader.ReadExactBytes(14);
        _ = reader.ReadExactBytes(0x12);
        this.BgmFlags = reader.ReadExactBytes(8);
        _ = reader.ReadExactBytes(0x18);
        this.TotalPlayTime = reader.ReadInt32();
        _ = reader.ReadInt32(); // always 0?
        _ = reader.ReadInt32(); // 0x15?
        _ = reader.ReadInt32(); // always 0?
        _ = reader.ReadExactBytes(0x40);    // story flags?
        this.NicknameFlags = reader.ReadExactBytes(51);
        _ = reader.ReadExactBytes(0x155);
    }

    public IEnumerable<byte> LastName { get; }  // The last 2 bytes are always 0x00 ?

    public IEnumerable<byte> BgmFlags { get; }

    public int TotalPlayTime { get; }   // unit: 10ms

    public IEnumerable<byte> NicknameFlags { get; } // The first byte should be ignored

    public static bool CanInitialize(Th10.Chapter chapter)
    {
        return chapter.Signature.Equals(ValidSignature, StringComparison.Ordinal)
            && (chapter.Version == ValidVersion)
            && (chapter.Size == ValidSize);
    }
}

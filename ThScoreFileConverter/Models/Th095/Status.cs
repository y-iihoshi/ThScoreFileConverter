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

namespace ThScoreFileConverter.Models.Th095;

internal class Status : Chapter, IStatus
{
    public const string ValidSignature = "ST";
    public const ushort ValidVersion = 0x0000;
    public const int ValidSize = 0x00000458;

    public Status(Chapter chapter)
        : base(chapter, ValidSignature, ValidVersion, ValidSize)
    {
        using var stream = new MemoryStream(this.Data, false);
        using var reader = new BinaryReader(stream);

        this.LastName = reader.ReadExactBytes(10);
        _ = reader.ReadExactBytes(0x0442);
    }

    public IEnumerable<byte> LastName { get; }  // The last 2 bytes are always 0x00 ?

    public static bool CanInitialize(Chapter chapter)
    {
        return chapter.Signature.Equals(ValidSignature, StringComparison.Ordinal)
            && (chapter.Version == ValidVersion)
            && (chapter.Size == ValidSize);
    }
}

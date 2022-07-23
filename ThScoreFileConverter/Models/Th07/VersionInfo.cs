//-----------------------------------------------------------------------
// <copyright file="VersionInfo.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using System.IO;
using ThScoreFileConverter.Extensions;

namespace ThScoreFileConverter.Models.Th07;

internal class VersionInfo : Th06.Chapter
{
    public const string ValidSignature = "VRSM";
    public const short ValidSize = 0x001C;

    public VersionInfo(Th06.Chapter chapter)
        : base(chapter, ValidSignature, ValidSize)
    {
        using var stream = new MemoryStream(this.Data, false);
        using var reader = new BinaryReader(stream);

        _ = reader.ReadUInt16();    // always 0x0001?
        _ = reader.ReadUInt16();
        this.Version = reader.ReadExactBytes(6);
        _ = reader.ReadExactBytes(3);
        _ = reader.ReadExactBytes(3);
        _ = reader.ReadUInt32();
    }

    public IEnumerable<byte> Version { get; }   // Null-terminated
}

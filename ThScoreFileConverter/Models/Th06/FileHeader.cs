//-----------------------------------------------------------------------
// <copyright file="FileHeader.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.IO;

namespace ThScoreFileConverter.Models.Th06;

internal class FileHeader : IBinaryReadable, IBinaryWritable
{
    public const short ValidVersion = 0x0010;
    public const int ValidSize = 0x00000014;

    private ushort unknown1;
    private ushort unknown2;
    private uint unknown3;

    public FileHeader()
    {
    }

    public ushort Checksum { get; private set; }

    public short Version { get; private set; }

    public int Size { get; private set; }

    public int DecodedAllSize { get; private set; }

    public bool IsValid => (this.Version == ValidVersion) && (this.Size == ValidSize);

    public void ReadFrom(BinaryReader reader)
    {
        this.unknown1 = reader.ReadUInt16();
        this.Checksum = reader.ReadUInt16();
        this.Version = reader.ReadInt16();
        this.unknown2 = reader.ReadUInt16();
        this.Size = reader.ReadInt32();
        this.unknown3 = reader.ReadUInt32();
        this.DecodedAllSize = reader.ReadInt32();
    }

    public void WriteTo(BinaryWriter writer)
    {
        writer.Write(this.unknown1);
        writer.Write(this.Checksum);
        writer.Write(this.Version);
        writer.Write(this.unknown2);
        writer.Write(this.Size);
        writer.Write(this.unknown3);
        writer.Write(this.DecodedAllSize);
    }
}

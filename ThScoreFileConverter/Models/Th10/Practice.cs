//-----------------------------------------------------------------------
// <copyright file="Practice.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.IO;

namespace ThScoreFileConverter.Models.Th10;

internal sealed class Practice : IBinaryReadable, IPractice
{
    public uint Score { get; private set; }     // Divided by 10

    public byte Cleared { get; private set; }   // 0x00: Not clear, 0x01: Cleared

    public byte Unlocked { get; private set; }  // 0x00: Locked, 0x01: Unlocked

    public void ReadFrom(BinaryReader reader)
    {
        this.Score = reader.ReadUInt32();
        this.Cleared = reader.ReadByte();
        this.Unlocked = reader.ReadByte();
        _ = reader.ReadUInt16();    // always 0x0000?
    }
}

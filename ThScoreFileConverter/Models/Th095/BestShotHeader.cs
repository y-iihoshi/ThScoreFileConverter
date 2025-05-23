﻿//-----------------------------------------------------------------------
// <copyright file="BestShotHeader.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.IO;
using CommunityToolkit.Diagnostics;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models.Th095;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th095;

internal sealed class BestShotHeader : IBinaryReadable, IBestShotHeader<Level>
{
    public const string ValidSignature = "BSTS";
    public const int SignatureSize = 4;

    public string Signature { get; private set; } = string.Empty;

    public Level Level { get; private set; }

    public short Scene { get; private set; }    // 1-based

    public short Width { get; private set; }

    public short Height { get; private set; }

    public int ResultScore { get; private set; }

    public float SlowRate { get; private set; }

    public IEnumerable<byte> CardName { get; private set; } = ReadOnlyCP932Bytes.Empty;

    public void ReadFrom(BinaryReader reader)
    {
        this.Signature = EncodingHelper.Default.GetString(reader.ReadExactBytes(SignatureSize));
        if (!this.Signature.Equals(ValidSignature, StringComparison.Ordinal))
            ThrowHelper.ThrowInvalidDataException();

        _ = reader.ReadUInt16();
        this.Level = EnumHelper.To<Level>(reader.ReadInt16() - 1);
        this.Scene = reader.ReadInt16();
        _ = reader.ReadUInt16();    // 0x0102 ... Version?
        this.Width = reader.ReadInt16();
        this.Height = reader.ReadInt16();
        this.ResultScore = reader.ReadInt32();
        this.SlowRate = reader.ReadSingle();
#pragma warning disable IDE0306 // Simplify collection initialization
        this.CardName = new ReadOnlyCP932Bytes(reader.ReadExactBytes(0x50));
#pragma warning restore IDE0306 // Simplify collection initialization
    }
}

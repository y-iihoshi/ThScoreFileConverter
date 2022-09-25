//-----------------------------------------------------------------------
// <copyright file="ReadOnlyCP932Bytes.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Collections.Generic;
using System.Linq;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models;

internal class ReadOnlyCP932Bytes
{
    private readonly byte[] bytes;
    private readonly string str;

    public ReadOnlyCP932Bytes(IEnumerable<byte> bytes)
    {
        this.bytes = bytes.ToArray();
        this.str = EncodingHelper.CP932.GetString(this.bytes).Split('\0')[0];
    }

    public static ReadOnlyCP932Bytes Empty { get; } = new ReadOnlyCP932Bytes(Array.Empty<byte>());

    public IEnumerable<byte> Bytes => this.bytes;

    public override string ToString()
    {
        return this.str;
    }
}

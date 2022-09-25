//-----------------------------------------------------------------------
// <copyright file="ReadOnlyCP932String.cs" company="None">
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

internal class ReadOnlyCP932String
{
    private readonly byte[] bytes;
    private readonly string str;

    public ReadOnlyCP932String(IEnumerable<byte> bytes)
    {
        this.bytes = bytes.ToArray();
        this.str = EncodingHelper.CP932.GetString(this.bytes).Split('\0')[0];
    }

    public static ReadOnlyCP932String Empty { get; } = new ReadOnlyCP932String(Array.Empty<byte>());

    public IEnumerable<byte> Bytes => this.bytes;

    public override string ToString()
    {
        return this.str;
    }
}

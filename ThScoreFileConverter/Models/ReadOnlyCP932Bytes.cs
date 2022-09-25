//-----------------------------------------------------------------------
// <copyright file="ReadOnlyCP932Bytes.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models;

internal class ReadOnlyCP932Bytes : IEnumerable<byte>
{
    private readonly byte[] bytes;
    private string? str;

    public ReadOnlyCP932Bytes(IEnumerable<byte> bytes)
    {
        this.bytes = bytes.ToArray();
        this.str = default;
    }

    public static ReadOnlyCP932Bytes Empty { get; } = new ReadOnlyCP932Bytes(Array.Empty<byte>());

    public IEnumerable<byte> Bytes => this.bytes;

    public IEnumerator<byte> GetEnumerator()
    {
        return this.Bytes.GetEnumerator();
    }

    public override string ToString()
    {
        return this.str ??= EncodingHelper.CP932.GetString(this.bytes).Split('\0')[0];
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }
}

//-----------------------------------------------------------------------
// <copyright file="ReadOnlyCP932Bytes.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models;

internal sealed class ReadOnlyCP932Bytes(IEnumerable<byte> bytes) : IEnumerable<byte>
{
    private readonly byte[] bytes = bytes.ToArray();
    private string? str;

    public static ReadOnlyCP932Bytes Empty { get; } = new ReadOnlyCP932Bytes([]);

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

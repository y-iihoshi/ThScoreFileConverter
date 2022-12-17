//-----------------------------------------------------------------------
// <copyright file="BitReader.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.IO;
using CommunityToolkit.Diagnostics;

namespace ThScoreFileConverter.Models;

/// <summary>
/// Represents a reader that reads data by bitwise from a stream.
/// </summary>
public class BitReader
{
    /// <summary>
    /// The stream to read.
    /// </summary>
    private readonly Stream stream;

    /// <summary>
    /// The byte that is currently reading.
    /// </summary>
    private int current;

    /// <summary>
    /// The mask value that represents the reading bit position.
    /// </summary>
    private byte mask;

    /// <summary>
    /// Initializes a new instance of the <see cref="BitReader"/> class.
    /// </summary>
    /// <param name="stream">
    /// The stream to read. Since a <see cref="BitReader"/> instance does not own <paramref name="stream"/>,
    /// it is responsible for the caller to close <paramref name="stream"/>.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="stream"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="stream"/> is not readable.</exception>
    public BitReader(Stream stream)
    {
        Guard.IsNotNull(stream);
        Guard.CanRead(stream);

        this.stream = stream;
        this.current = 0;
        this.mask = 0x80;
    }

    /// <summary>
    /// Reads the specified number of bits from the stream.
    /// </summary>
    /// <param name="num">The number of reading bits.</param>
    /// <returns>The value that is read from the stream.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="num"/> is negative.</exception>
    public int ReadBits(int num)
    {
        Guard.IsGreaterThanOrEqualTo(num, 0);

        var value = 0;
        for (var i = 0; i < num; i++)
        {
            if (this.mask == 0x80)
            {
                this.current = this.stream.ReadByte();
                if (this.current < 0)   // EOF
                    this.current = 0;
            }

            value <<= 1;
            if (((byte)this.current & this.mask) != 0)
                value |= 1;
            this.mask >>= 1;
            if (this.mask == 0)
                this.mask = 0x80;
        }

        return value;
    }
}

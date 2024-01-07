//-----------------------------------------------------------------------
// <copyright file="BinaryReaderExtensions.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CommunityToolkit.Diagnostics;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Extensions;

/// <summary>
/// Provides some extension methods for <see cref="BinaryReader"/>.
/// </summary>
public static class BinaryReaderExtensions
{
    /// <summary>
    /// Reads the specified number of bytes from the current stream into a byte array and advances the current
    /// position by that number of bytes.
    /// </summary>
    /// <param name="reader">The <see cref="BinaryReader"/> to be used for reading data.</param>
    /// <param name="count">The number of bytes to read.</param>
    /// <returns>
    /// A byte array containing data read from the underlying stream.
    /// The length is ensured to be equal to <paramref name="count"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="reader"/> is <see langword="null"/>.</exception>
    /// <exception cref="EndOfStreamException">The end of stream is reached.</exception>
    public static byte[] ReadExactBytes(this BinaryReader reader, int count)
    {
        Guard.IsNotNull(reader);

        var bytes = reader.ReadBytes(count);
        if (bytes.Length < count)
            throw new EndOfStreamException();

        return bytes;
    }

    /// <summary>
    /// Reads a null-terminated string from the current stream.
    /// </summary>
    /// <param name="reader">The <see cref="BinaryReader"/> to be used for reading data.</param>
    /// <returns>The string being read.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="reader"/> is <see langword="null"/>.</exception>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    public static string ReadNullTerminatedString(this BinaryReader reader)
    {
        return ReadNullTerminatedString(reader, EncodingHelper.UTF8NoBOM);
    }

    /// <summary>
    /// Reads a null-terminated string from the current stream.
    /// </summary>
    /// <param name="reader">The <see cref="BinaryReader"/> to be used for reading data.</param>
    /// <param name="encoding">The character encoding to use.</param>
    /// <returns>The string being read.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="reader"/> or <paramref name="encoding"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    public static string ReadNullTerminatedString(this BinaryReader reader, Encoding encoding)
    {
        Guard.IsNotNull(reader);
        Guard.IsNotNull(encoding);

        var bytes = new List<byte>();
        var b = reader.ReadByte();
        while (b != 0)
        {
            bytes.Add(b);
            b = reader.ReadByte();
        }

        return encoding.GetString([.. bytes]);
    }
}

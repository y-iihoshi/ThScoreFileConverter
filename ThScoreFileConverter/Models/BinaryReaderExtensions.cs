//-----------------------------------------------------------------------
// <copyright file="BinaryReaderExtensions.cs" company="None">
//     (c) 2019 IIHOSHI Yoshinori
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Models
{
    using System;
    using System.IO;

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
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is <c>null</c>.</exception>
        /// <exception cref="EndOfStreamException">The end of stream is reached.</exception>
        public static byte[] ReadExactBytes(this BinaryReader reader, int count)
        {
            if (reader is null)
                throw new ArgumentNullException(nameof(reader));

            var bytes = reader.ReadBytes(count);
            if (bytes.Length < count)
                throw new EndOfStreamException();

            return bytes;
        }
    }
}

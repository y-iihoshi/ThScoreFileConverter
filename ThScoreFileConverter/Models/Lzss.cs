//-----------------------------------------------------------------------
// <copyright file="Lzss.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.IO;
using ThScoreFileConverter.Properties;

namespace ThScoreFileConverter.Models
{
    /// <summary>
    /// Provides static methods for compressing and decompressing LZSS formatted data.
    /// </summary>
    internal static class Lzss
    {
        /// <summary>
        /// The size of the dictionary.
        /// </summary>
        private const int DicSize = 0x2000;

        /// <summary>
        /// Compresses data by LZSS format.
        /// </summary>
        /// <param name="input">The stream to input data.</param>
        /// <param name="output">The stream that is output the compressed data.</param>
#pragma warning disable IDE0060 // Remove unused parameter
        public static void Compress(Stream input, Stream output)
        {
            throw new NotImplementedException(Resources.NotImplementedExceptionLzssCompressionIsNotSupported);
        }
#pragma warning restore IDE0060 // Remove unused parameter

        /// <summary>
        /// Decompresses the LZSS formatted data.
        /// </summary>
        /// <param name="input">The stream to input data.</param>
        /// <param name="output">The stream that is output the decompressed data.</param>
        public static void Decompress(Stream input, Stream output)
        {
            var reader = new BitReader(input);
            var dictionary = new byte[DicSize];
            var dicIndex = 1;

            while (dicIndex < dictionary.Length)
            {
                var flag = reader.ReadBits(1);
                if (flag != 0)
                {
                    var ch = (byte)reader.ReadBits(8);
                    output.WriteByte(ch);
                    dictionary[dicIndex] = ch;
                    dicIndex = (dicIndex + 1) & 0x1FFF;
                }
                else
                {
                    var offset = reader.ReadBits(13);
                    if (offset == 0)
                    {
                        break;
                    }
                    else
                    {
                        var length = reader.ReadBits(4) + 3;
                        for (var i = 0; i < length; i++)
                        {
                            var ch = dictionary[(offset + i) & 0x1FFF];
                            output.WriteByte(ch);
                            dictionary[dicIndex] = ch;
                            dicIndex = (dicIndex + 1) & 0x1FFF;
                        }
                    }
                }
            }
        }
    }
}

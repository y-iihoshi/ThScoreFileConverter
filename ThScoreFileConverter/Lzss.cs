//-----------------------------------------------------------------------
// <copyright file="Lzss.cs" company="None">
//     (c) 2013-2014 IIHOSHI Yoshinori
// </copyright>
//-----------------------------------------------------------------------

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage(
    "StyleCop.CSharp.LayoutRules",
    "SA1503:CurlyBracketsMustNotBeOmitted",
    Justification = "Reviewed.")]

namespace ThScoreFileConverter
{
    using System;
    using System.IO;

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
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Usage",
            "CA1801:ReviewUnusedParameters",
            Justification = "Reviewed.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Performance",
            "CA1811:AvoidUncalledPrivateCode",
            Justification = "For future use.")]
        public static void Compress(Stream input, Stream output)
        {
            throw new NotImplementedException("LZSS complession is not supported.");
        }

        /// <summary>
        /// Decompresses the LZSS formatted data.
        /// </summary>
        /// <param name="input">The stream to input data.</param>
        /// <param name="output">The stream that is output the decompressed data.</param>
        public static void Extract(Stream input, Stream output)
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
                        break;
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

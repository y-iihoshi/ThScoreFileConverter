//-----------------------------------------------------------------------
// <copyright file="ThCrypt.cs" company="None">
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
    using System.Diagnostics.CodeAnalysis;
    using System.IO;

    /// <summary>
    /// The static class that treats the encryption format used for Touhou Project works.
    /// Thanks to Touhou Toolkit 5.
    /// </summary>
    public static class ThCrypt
    {
        /// <summary>
        /// Encrypts data.
        /// </summary>
        /// <param name="input">The stream to input data to encrypt.</param>
        /// <param name="output">The stream that is output the encrypted data.</param>
        /// <param name="size">The size of input data. (Unit: [Byte])</param>
        /// <param name="key">The encryption key.</param>
        /// <param name="step">The step.</param>
        /// <param name="block">The size of block.</param>
        /// <param name="limit">The limit value.</param>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", Justification = "Reviewed.")]
        public static void Encrypt(
            Stream input, Stream output, int size, byte key, byte step, int block, int limit)
        {
            throw new NotImplementedException("Encryption is not supported.");
        }

        /// <summary>
        /// Decrypts data.
        /// </summary>
        /// <param name="input">The stream to input data to decrypt.</param>
        /// <param name="output">The stream that is output the decrypted data.</param>
        /// <param name="size">The size of input data. (Unit: [Byte])</param>
        /// <param name="key">The decryption key.</param>
        /// <param name="step">The step.</param>
        /// <param name="block">The size of block.</param>
        /// <param name="limit">The limit value.</param>
        public static void Decrypt(
            Stream input, Stream output, int size, byte key, byte step, int block, int limit)
        {
            var inBlock = new byte[block];
            var outBlock = new byte[block];
            int addup;

            addup = size % block;
            if (addup >= block / 4)
                addup = 0;
            addup += size % 2;
            size -= addup;

            while ((size > 0) && (limit > 0))
            {
                if (size < block)
                    block = size;
                if (input.Read(inBlock, 0, block) != block)
                    return;

                var inIndex = 0;
                for (var j = 0; j < 2; ++j)
                {
                    var outIndex = block - j - 1;
                    for (var i = 0; i < (block - j + 1) / 2; ++i)
                    {
                        outBlock[outIndex] = (byte)(inBlock[inIndex] ^ key);
                        inIndex++;
                        outIndex -= 2;
                        key += step;
                    }
                }

                output.Write(outBlock, 0, block);
                limit -= block;
                size -= block;
            }

            size += addup;
            if (size > 0)
            {
                var restbuf = new byte[size];
                if (input.Read(restbuf, 0, size) != size)
                    return;
                output.Write(restbuf, 0, size);
            }
        }
    }
}

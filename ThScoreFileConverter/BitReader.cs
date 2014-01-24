//-----------------------------------------------------------------------
// <copyright file="BitReader.cs" company="None">
//     (c) 2013-2014 IIHOSHI Yoshinori
// </copyright>
//-----------------------------------------------------------------------

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage(
    "StyleCop.CSharp.LayoutRules",
    "SA1503:CurlyBracketsMustNotBeOmitted",
    Justification = "Reviewed.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage(
    "StyleCop.CSharp.OrderingRules",
    "SA1201:ElementsMustAppearInTheCorrectOrder",
    Justification = "Reviewed.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage(
    "StyleCop.CSharp.OrderingRules",
    "SA1202:ElementsMustBeOrderedByAccess",
    Justification = "Reviewed.")]

namespace ThScoreFileConverter
{
    using System;
    using System.IO;

    /// <summary>
    /// Represents a reader that reads data by bitwise from a stream.
    /// </summary>
    public class BitReader : IDisposable
    {
        /// <summary>
        /// The stream to read.
        /// </summary>
        private Stream stream;
        
        /// <summary>
        /// The flag that represents whether <see cref="Dispose(bool)"/> has been called.
        /// </summary>
        private bool disposed;

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
        /// <param name="stream">The stream to read.</param>
        public BitReader(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");
            if (!stream.CanRead)
                throw new ArgumentException("stream");

            this.stream = stream;
            this.disposed = false;
            this.current = 0;
            this.mask = 0x80;
        }

        /// <summary>
        /// Implements the <see cref="IDisposable.Dispose"/> method.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the resources of the current instance.
        /// </summary>
        /// <param name="disposing">
        /// <c>true</c> if calls from the <see cref="Dispose()"/> method; <c>false</c> for the destructor.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                    this.stream.Dispose();
                this.disposed = true;
            }
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="BitReader"/> class.
        /// </summary>
        ~BitReader()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// Reads the specified number of bits from the stream.
        /// </summary>
        /// <param name="num">The number of reading bits.</param>
        /// <returns>The value that is read from the stream.</returns>
        public int ReadBits(int num)
        {
            if (this.disposed)
                throw new ObjectDisposedException(this.ToString());

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
}

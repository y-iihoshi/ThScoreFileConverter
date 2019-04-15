//-----------------------------------------------------------------------
// <copyright file="BitReader.cs" company="None">
//     (c) 2013-2015 IIHOSHI Yoshinori
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Models
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
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentException"/>
        public BitReader(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));
            if (!stream.CanRead)
                throw new ArgumentException("stream must be readable", nameof(stream));

            this.stream = stream;
            this.disposed = false;
            this.current = 0;
            this.mask = 0x80;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="BitReader"/> class.
        /// </summary>
        ~BitReader()
        {
            this.Dispose(false);
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
        /// Reads the specified number of bits from the stream.
        /// </summary>
        /// <param name="num">The number of reading bits.</param>
        /// <returns>The value that is read from the stream.</returns>
        /// <exception cref="ArgumentOutOfRangeException"/>
        /// <exception cref="ObjectDisposedException"/>
        public int ReadBits(int num)
        {
            if (num < 0)
                throw new ArgumentOutOfRangeException(nameof(num));
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
    }
}

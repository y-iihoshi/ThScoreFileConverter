//-----------------------------------------------------------------------
// <copyright file="Chapter.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

namespace ThScoreFileConverter.Models.Th06
{
    using System;
    using System.IO;
    using System.Text;

    internal class Chapter : IBinaryReadable
    {
        public Chapter()
        {
            this.Signature = string.Empty;
            this.Size1 = 0;
            this.Size2 = 0;
            this.Data = new byte[] { };
        }

        protected Chapter(Chapter chapter)
        {
            if (chapter is null)
                throw new ArgumentNullException(nameof(chapter));

            this.Signature = chapter.Signature;
            this.Size1 = chapter.Size1;
            this.Size2 = chapter.Size2;
            this.Data = new byte[chapter.Data.Length];
            chapter.Data.CopyTo(this.Data, 0);
        }

        public string Signature { get; private set; }   // .Length = 4

        public short Size1 { get; private set; }

        public short Size2 { get; private set; }        // always equal to size1?

        public byte FirstByteOfData => this.Data?.Length > 0 ? this.Data[0] : default;

        protected byte[] Data { get; private set; }

        public void ReadFrom(BinaryReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));

            this.Signature = Encoding.Default.GetString(reader.ReadExactBytes(4));
            this.Size1 = reader.ReadInt16();
            this.Size2 = reader.ReadInt16();
            this.Data = reader.ReadExactBytes(this.Size1 - this.Signature.Length - (sizeof(short) * 2));
        }
    }
}

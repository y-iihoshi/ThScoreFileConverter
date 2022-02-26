//-----------------------------------------------------------------------
// <copyright file="Chapter.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Helpers;
using ThScoreFileConverter.Properties;

namespace ThScoreFileConverter.Models.Th10
{
    internal class Chapter : IBinaryReadable, Th095.IChapter
    {
        public const int SignatureSize = 2;

        public Chapter()
        {
            this.Signature = string.Empty;
            this.Version = 0;
            this.Checksum = 0;
            this.Size = 0;
            this.Data = Array.Empty<byte>();
        }

        protected Chapter(Chapter chapter)
        {
            this.Signature = chapter.Signature;
            this.Version = chapter.Version;
            this.Checksum = chapter.Checksum;
            this.Size = chapter.Size;
            this.Data = new byte[chapter.Data.Length];
            chapter.Data.CopyTo(this.Data, 0);
        }

        protected Chapter(Chapter chapter, string expectedSignature, ushort expectedVersion, int expectedSize)
            : this(chapter)
        {
            if (!this.Signature.Equals(expectedSignature, StringComparison.Ordinal))
            {
                throw new InvalidDataException(
                    Utils.Format(Resources.InvalidDataExceptionPropertyIsInvalid, nameof(this.Signature)));
            }

            if (this.Version != expectedVersion)
            {
                throw new InvalidDataException(
                    Utils.Format(Resources.InvalidDataExceptionPropertyIsInvalid, nameof(this.Version)));
            }

            if (this.Size != expectedSize)
            {
                throw new InvalidDataException(
                    Utils.Format(Resources.InvalidDataExceptionPropertyIsInvalid, nameof(this.Size)));
            }
        }

        public string Signature { get; private set; }

        public ushort Version { get; private set; }

        public uint Checksum { get; private set; }

        public int Size { get; private set; }

        public bool IsValid
        {
            get
            {
                var sum = BitConverter.GetBytes(this.Size).Concat(this.Data).Sum(elem => (uint)elem);
                return sum == this.Checksum;
            }
        }

        protected byte[] Data { get; private set; }

        public void ReadFrom(BinaryReader reader)
        {
            this.Signature = EncodingHelper.Default.GetString(reader.ReadExactBytes(SignatureSize));
            this.Version = reader.ReadUInt16();
            this.Checksum = reader.ReadUInt32();
            this.Size = reader.ReadInt32();
            this.Data = reader.ReadExactBytes(
                this.Size - SignatureSize - sizeof(ushort) - sizeof(uint) - sizeof(int));
        }
    }
}

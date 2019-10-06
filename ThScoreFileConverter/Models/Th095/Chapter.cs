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
using ThScoreFileConverter.Properties;

namespace ThScoreFileConverter.Models.Th095
{
    internal class Chapter : IBinaryReadable, IChapter
    {
        public const int SignatureSize = 2;

        public Chapter()
        {
            this.Signature = string.Empty;
            this.Version = 0;
            this.Size = 0;
            this.Checksum = 0;
            this.Data = new byte[] { };
        }

        protected Chapter(Chapter chapter)
        {
            if (chapter is null)
                throw new ArgumentNullException(nameof(chapter));

            this.Signature = chapter.Signature;
            this.Version = chapter.Version;
            this.Size = chapter.Size;
            this.Checksum = chapter.Checksum;
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

        public int Size { get; private set; }

        public uint Checksum { get; private set; }

        public bool IsValid
        {
            get
            {
                var sigVer = Encoding.Default.GetBytes(this.Signature)
                    .Concat(BitConverter.GetBytes(this.Version))
                    .ToArray();
                if (sigVer.Length < sizeof(uint))
                    return false;
                long sum = BitConverter.ToUInt32(sigVer, 0) + this.Size;
                if (this.Data.Length % sizeof(uint) != 0)
                    return false;
                for (var index = 0; index < this.Data.Length; index += sizeof(uint))
                    sum += BitConverter.ToUInt32(this.Data, index);
                return (uint)sum == this.Checksum;
            }
        }

        protected byte[] Data { get; private set; }

        public void ReadFrom(BinaryReader reader)
        {
            if (reader is null)
                throw new ArgumentNullException(nameof(reader));

            this.Signature = Encoding.Default.GetString(reader.ReadExactBytes(SignatureSize));
            this.Version = reader.ReadUInt16();
            this.Size = reader.ReadInt32();
            this.Checksum = reader.ReadUInt32();
            this.Data = reader.ReadExactBytes(
                this.Size - SignatureSize - sizeof(ushort) - sizeof(int) - sizeof(uint));
        }
    }
}

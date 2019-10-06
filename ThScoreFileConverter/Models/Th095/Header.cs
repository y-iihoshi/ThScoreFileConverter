//-----------------------------------------------------------------------
// <copyright file="Header.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.IO;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Properties;

namespace ThScoreFileConverter.Models.Th095
{
    internal class Header : IBinaryReadable, IBinaryWritable
    {
        public const int SignatureSize = 4;
        public const int Size = SignatureSize + (sizeof(int) * 3) + (sizeof(uint) * 2);

        private uint unknown1;
        private uint unknown2;

        public Header()
        {
            this.Signature = string.Empty;
            this.EncodedAllSize = 0;
            this.EncodedBodySize = 0;
            this.DecodedBodySize = 0;
        }

        public string Signature { get; private set; }

        public int EncodedAllSize { get; private set; }

        public int EncodedBodySize { get; private set; }

        public int DecodedBodySize { get; private set; }

        public virtual bool IsValid => (this.EncodedAllSize - this.EncodedBodySize) == Size;

        public void ReadFrom(BinaryReader reader)
        {
            if (reader is null)
                throw new ArgumentNullException(nameof(reader));

            this.Signature = Encoding.Default.GetString(reader.ReadExactBytes(SignatureSize));

            this.EncodedAllSize = reader.ReadInt32();
            if (this.EncodedAllSize < 0)
            {
                throw new InvalidDataException(
                    Utils.Format(Resources.InvalidDataExceptionPropertyIsInvalid, nameof(this.EncodedAllSize)));
            }

            this.unknown1 = reader.ReadUInt32();
            this.unknown2 = reader.ReadUInt32();

            this.EncodedBodySize = reader.ReadInt32();
            if (this.EncodedBodySize < 0)
            {
                throw new InvalidDataException(
                    Utils.Format(Resources.InvalidDataExceptionPropertyIsInvalid, nameof(this.EncodedBodySize)));
            }

            this.DecodedBodySize = reader.ReadInt32();
            if (this.DecodedBodySize < 0)
            {
                throw new InvalidDataException(
                    Utils.Format(Resources.InvalidDataExceptionPropertyIsInvalid, nameof(this.DecodedBodySize)));
            }
        }

        public void WriteTo(BinaryWriter writer)
        {
            if (writer is null)
                throw new ArgumentNullException(nameof(writer));

            writer.Write(Encoding.Default.GetBytes(this.Signature));
            writer.Write(this.EncodedAllSize);
            writer.Write(this.unknown1);
            writer.Write(this.unknown2);
            writer.Write(this.EncodedBodySize);
            writer.Write(this.DecodedBodySize);
        }
    }
}

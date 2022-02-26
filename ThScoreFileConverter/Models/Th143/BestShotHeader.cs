//-----------------------------------------------------------------------
// <copyright file="BestShotHeader.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.IO;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th143
{
    internal class BestShotHeader : IBinaryReadable, IBestShotHeader
    {
        public const string ValidSignature = "BST3";
        public const int SignatureSize = 4;

        public string Signature { get; private set; } = string.Empty;

        public Day Day { get; private set; }

        public short Scene { get; private set; }    // 1-based

        public short Width { get; private set; }

        public short Height { get; private set; }

        public uint DateTime { get; private set; }

        public float SlowRate { get; private set; }

        public void ReadFrom(BinaryReader reader)
        {
            this.Signature = EncodingHelper.Default.GetString(reader.ReadExactBytes(SignatureSize));
            if (!this.Signature.Equals(ValidSignature, StringComparison.Ordinal))
                throw new InvalidDataException();

            _ = reader.ReadUInt16();    // always 0xDF01?
            this.Day = EnumHelper.To<Day>(reader.ReadInt16());
            this.Scene = (short)(reader.ReadInt16() + 1);
            _ = reader.ReadUInt16();    // 0x0100 ... Version?
            this.Width = reader.ReadInt16();
            this.Height = reader.ReadInt16();
            _ = reader.ReadUInt32();    // always 0x0005E800?
            this.DateTime = reader.ReadUInt32();
            this.SlowRate = reader.ReadSingle();    // really...?
            _ = reader.ReadExactBytes(0x58);
        }
    }
}

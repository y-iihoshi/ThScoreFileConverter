//-----------------------------------------------------------------------
// <copyright file="Practice.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

namespace ThScoreFileConverter.Models.Th13
{
    using System;
    using System.IO;

    internal class Practice : IBinaryReadable
    {
        public uint Score { get; private set; }         // Divided by 10

        public byte ClearFlag { get; private set; }     // 0x00: Not clear, 0x01: Cleared

        public byte EnableFlag { get; private set; }    // 0x00: Disable, 0x01: Enable

        public void ReadFrom(BinaryReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));

            this.Score = reader.ReadUInt32();
            this.ClearFlag = reader.ReadByte();
            this.EnableFlag = reader.ReadByte();
            reader.ReadUInt16();    // always 0x0000?
        }
    }
}

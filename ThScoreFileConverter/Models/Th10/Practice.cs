//-----------------------------------------------------------------------
// <copyright file="Practice.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.IO;

namespace ThScoreFileConverter.Models.Th10
{
    internal class Practice : IBinaryReadable, IPractice
    {
        public uint Score { get; private set; }     // Divided by 10

        public uint StageFlag { get; private set; } // 0x00000000: disable, 0x00000101: enable ?

        public void ReadFrom(BinaryReader reader)
        {
            if (reader is null)
                throw new ArgumentNullException(nameof(reader));

            this.Score = reader.ReadUInt32();
            this.StageFlag = reader.ReadUInt32();
        }
    }
}

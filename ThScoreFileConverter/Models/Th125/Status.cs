//-----------------------------------------------------------------------
// <copyright file="Status.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Collections.Generic;
using System.IO;
using ThScoreFileConverter.Extensions;

namespace ThScoreFileConverter.Models.Th125
{
    internal class Status : Th095.Chapter, IStatus
    {
        public const string ValidSignature = "ST";
        public const ushort ValidVersion = 0x0001;
        public const int ValidSize = 0x00000474;

        public Status(Th095.Chapter chapter)
            : base(chapter, ValidSignature, ValidVersion, ValidSize)
        {
            using (var reader = new BinaryReader(new MemoryStream(this.Data, false)))
            {
                this.LastName = reader.ReadExactBytes(10);
                _ = reader.ReadExactBytes(2);
                this.BgmFlags = reader.ReadExactBytes(6);
                _ = reader.ReadExactBytes(0x2E);
                this.TotalPlayTime = reader.ReadInt32();
                _ = reader.ReadExactBytes(0x424);
            }
        }

        public IEnumerable<byte> LastName { get; }  // The last 2 bytes are always 0x00 ?

        public IEnumerable<byte> BgmFlags { get; }

        public int TotalPlayTime { get; }   // unit: 10ms

        public static bool CanInitialize(Th095.Chapter chapter)
        {
            return chapter.Signature.Equals(ValidSignature, StringComparison.Ordinal)
                && (chapter.Version == ValidVersion)
                && (chapter.Size == ValidSize);
        }
    }
}

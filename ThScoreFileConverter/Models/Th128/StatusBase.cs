//-----------------------------------------------------------------------
// <copyright file="StatusBase.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Collections.Generic;
using System.IO;
using ThScoreFileConverter.Extensions;

namespace ThScoreFileConverter.Models.Th128
{
    internal class StatusBase : Th10.Chapter, Th125.IStatus
    {
        public const string ValidSignature = "ST";
        public const int ValidSize = 0x0000042C;

        protected StatusBase(Th10.Chapter chapter, ushort validVersion, int numBgms, int gapSize)
            : base(chapter, ValidSignature, validVersion, ValidSize)
        {
            using var stream = new MemoryStream(this.Data, false);
            using var reader = new BinaryReader(stream);

            this.LastName = reader.ReadExactBytes(10);
            _ = reader.ReadExactBytes(0x10);
            this.BgmFlags = reader.ReadExactBytes(numBgms);
            _ = reader.ReadExactBytes(gapSize);
            this.TotalPlayTime = reader.ReadInt32();
            _ = reader.ReadExactBytes(0x03E0);
        }

        public IEnumerable<byte> LastName { get; }  // The last 2 bytes are always 0x00 ?

        public IEnumerable<byte> BgmFlags { get; }

        public int TotalPlayTime { get; }   // unit: 10ms

        public static bool CanInitialize(Th10.Chapter chapter)
        {
            return chapter.Signature.Equals(ValidSignature, StringComparison.Ordinal) && (chapter.Size == ValidSize);
        }
    }
}

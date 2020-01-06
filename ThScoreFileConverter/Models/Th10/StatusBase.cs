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

namespace ThScoreFileConverter.Models.Th10
{
    internal class StatusBase : Chapter, IStatus
    {
        public const string ValidSignature = "ST";
        public const int ValidSize = 0x00000448;

        protected StatusBase(Chapter chapter, ushort validVersion, int numBgms)
            : base(chapter, ValidSignature, validVersion, ValidSize)
        {
            using var stream = new MemoryStream(this.Data, false);
            using var reader = new BinaryReader(stream);

            this.LastName = reader.ReadExactBytes(10);
            _ = reader.ReadExactBytes(0x10);
            this.BgmFlags = reader.ReadExactBytes(numBgms);
            _ = reader.ReadExactBytes(0x422 - numBgms);
        }

        public IEnumerable<byte> LastName { get; }  // The last 2 bytes are always 0x00 ?

        public IEnumerable<byte> BgmFlags { get; }

        protected static bool CanInitialize(Chapter chapter)
        {
            return chapter.Signature.Equals(ValidSignature, StringComparison.Ordinal) && (chapter.Size == ValidSize);
        }
    }
}

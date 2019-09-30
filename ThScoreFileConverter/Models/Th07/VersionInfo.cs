//-----------------------------------------------------------------------
// <copyright file="VersionInfo.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using System.IO;

namespace ThScoreFileConverter.Models.Th07
{
    internal class VersionInfo : Th06.Chapter
    {
        public const string ValidSignature = "VRSM";
        public const short ValidSize = 0x001C;

        public VersionInfo(Th06.Chapter chapter)
            : base(chapter, ValidSignature, ValidSize)
        {
            using (var reader = new BinaryReader(new MemoryStream(this.Data, false)))
            {
                reader.ReadUInt16();    // always 0x0001?
                reader.ReadUInt16();
                this.Version = reader.ReadExactBytes(6);
                reader.ReadExactBytes(3);
                reader.ReadExactBytes(3);
                reader.ReadUInt32();
            }
        }

        public IEnumerable<byte> Version { get; }   // Null-terminated
    }
}

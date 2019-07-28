//-----------------------------------------------------------------------
// <copyright file="LastName.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.IO;

namespace ThScoreFileConverter.Models.Th07
{
    internal class LastName : Th06.Chapter
    {
        public const string ValidSignature = "LSNM";
        public const short ValidSize = 0x0018;

        public LastName(Th06.Chapter chapter)
            : base(chapter, ValidSignature, ValidSize)
        {
            using (var reader = new BinaryReader(new MemoryStream(this.Data, false)))
            {
                reader.ReadUInt32();    // always 0x00000001?
                this.Name = reader.ReadExactBytes(12);
            }
        }

        public byte[] Name { get; } // Null-terminated
    }
}

//-----------------------------------------------------------------------
// <copyright file="Header.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.IO;
using ThScoreFileConverter.Extensions;

namespace ThScoreFileConverter.Models.Th09
{
    internal class Header : Th06.Chapter
    {
        public const string ValidSignature = "TH9K";
        public const short ValidSize = 0x000C;

        public Header(Th06.Chapter chapter)
            : base(chapter, ValidSignature, ValidSize)
        {
            using var stream = new MemoryStream(this.Data, false);
            using var reader = new BinaryReader(stream);
            _ = reader.ReadByte();      // always 0x01?
            _ = reader.ReadExactBytes(3);
        }
    }
}

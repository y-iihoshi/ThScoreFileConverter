//-----------------------------------------------------------------------
// <copyright file="Header.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.IO;

namespace ThScoreFileConverter.Models.Th06
{
    internal class Header : Chapter
    {
        public const string ValidSignature = "TH6K";
        public const short ValidSize = 0x000C;

        public Header(Chapter chapter)
            : base(chapter, ValidSignature, ValidSize)
        {
            using (var reader = new BinaryReader(new MemoryStream(this.Data, false)))
            {
                _ = reader.ReadUInt32();    // always 0x00000010?
            }
        }
    }
}

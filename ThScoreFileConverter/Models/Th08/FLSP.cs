//-----------------------------------------------------------------------
// <copyright file="FLSP.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.IO;
using ThScoreFileConverter.Extensions;

namespace ThScoreFileConverter.Models.Th08
{
    internal class FLSP : Th06.Chapter  // FIXME
    {
        public const string ValidSignature = "FLSP";
        public const short ValidSize = 0x0020;

        public FLSP(Th06.Chapter chapter)
            : base(chapter, ValidSignature, ValidSize)
        {
            using var stream = new MemoryStream(this.Data, false);
            using var reader = new BinaryReader(stream);
            _ = reader.ReadExactBytes(0x18);
        }
    }
}

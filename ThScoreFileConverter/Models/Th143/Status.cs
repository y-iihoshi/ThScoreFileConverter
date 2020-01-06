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

namespace ThScoreFileConverter.Models.Th143
{
    internal class Status : Th10.Chapter, IStatus
    {
        public const string ValidSignature = "ST";
        public const ushort ValidVersion = 0x0001;
        public const int ValidSize = 0x00000224;

        public Status(Th10.Chapter chapter)
            : base(chapter, ValidSignature, ValidVersion, ValidSize)
        {
            using var stream = new MemoryStream(this.Data, false);
            using var reader = new BinaryReader(stream);

            this.LastName = reader.ReadExactBytes(14);
            _ = reader.ReadExactBytes(0x12);
            this.BgmFlags = reader.ReadExactBytes(9);
            _ = reader.ReadExactBytes(0x17);
            this.TotalPlayTime = reader.ReadInt32();
            _ = reader.ReadInt32();
            this.LastMainItem = Utils.ToEnum<ItemWithTotal>(reader.ReadInt32());
            this.LastSubItem = Utils.ToEnum<ItemWithTotal>(reader.ReadInt32());
            _ = reader.ReadExactBytes(0x54);
            this.NicknameFlags = reader.ReadExactBytes(71);
            _ = reader.ReadExactBytes(0x12D);
        }

        public IEnumerable<byte> LastName { get; }  // The last 2 bytes are always 0x00 ?

        public IEnumerable<byte> BgmFlags { get; }

        public int TotalPlayTime { get; }   // unit: 10ms

        public ItemWithTotal LastMainItem { get; }

        public ItemWithTotal LastSubItem { get; }

        public IEnumerable<byte> NicknameFlags { get; }

        public static bool CanInitialize(Th10.Chapter chapter)
        {
            return chapter.Signature.Equals(ValidSignature, StringComparison.Ordinal)
                && (chapter.Version == ValidVersion)
                && (chapter.Size == ValidSize);
        }
    }
}

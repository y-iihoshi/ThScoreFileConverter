//-----------------------------------------------------------------------
// <copyright file="HighScore.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using System.IO;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th09
{
    internal class HighScore : Th06.Chapter, IHighScore // per character, level, rank
    {
        public const string ValidSignature = "HSCR";
        public const short ValidSize = 0x002C;

        public HighScore(Th06.Chapter chapter)
            : base(chapter, ValidSignature, ValidSize)
        {
            using var stream = new MemoryStream(this.Data, false);
            using var reader = new BinaryReader(stream);

            _ = reader.ReadUInt32();    // always 0x00000002?
            this.Score = reader.ReadUInt32();
            _ = reader.ReadUInt32();    // always 0x00000000?
            this.Chara = EnumHelper.To<Chara>(reader.ReadByte());
            this.Level = EnumHelper.To<Level>(reader.ReadByte());
            this.Rank = reader.ReadInt16();
            this.Name = reader.ReadExactBytes(9);
            this.Date = reader.ReadExactBytes(9);
            _ = reader.ReadByte();      // always 0x00?
            this.ContinueCount = reader.ReadByte();
        }

        public uint Score { get; }  // Divided by 10

        public Chara Chara { get; }

        public Level Level { get; }

        public short Rank { get; }  // 0-based

        public IEnumerable<byte> Name { get; }  // Null-terminated

        public IEnumerable<byte> Date { get; }  // "yy/mm/dd\0"

        public byte ContinueCount { get; }
    }
}

//-----------------------------------------------------------------------
// <copyright file="ClearData.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ThScoreFileConverter.Models.Th07
{
    internal class ClearData : Th06.Chapter, IClearData // per character
    {
        public const string ValidSignature = "CLRD";
        public const short ValidSize = 0x001C;

        public ClearData(Th06.Chapter chapter)
            : base(chapter, ValidSignature, ValidSize)
        {
            var levels = Utils.GetEnumerator<Level>();

            using (var reader = new BinaryReader(new MemoryStream(this.Data, false)))
            {
                reader.ReadUInt32();    // always 0x00000001?
                this.StoryFlags = levels.ToDictionary(level => level, level => reader.ReadByte());
                this.PracticeFlags = levels.ToDictionary(level => level, level => reader.ReadByte());
                this.Chara = Utils.ToEnum<Chara>(reader.ReadInt32());
            }
        }

        public IReadOnlyDictionary<Level, byte> StoryFlags { get; }     // really...?

        public IReadOnlyDictionary<Level, byte> PracticeFlags { get; }  // really...?

        public Chara Chara { get; }
    }
}

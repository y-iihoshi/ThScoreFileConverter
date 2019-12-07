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

namespace ThScoreFileConverter.Models.Th08
{
    internal class ClearData : Th06.Chapter, IClearData // per character-with-total
    {
        public const string ValidSignature = "CLRD";
        public const short ValidSize = 0x0024;

        public ClearData(Th06.Chapter chapter)
            : base(chapter, ValidSignature, ValidSize)
        {
            var levels = Utils.GetEnumerator<Level>();

            using (var reader = new BinaryReader(new MemoryStream(this.Data, false)))
            {
                _ = reader.ReadUInt32();    // always 0x00000004?
                this.StoryFlags = levels.ToDictionary(level => level, _ => (PlayableStages)reader.ReadUInt16());
                this.PracticeFlags = levels.ToDictionary(level => level, _ => (PlayableStages)reader.ReadUInt16());
                _ = reader.ReadByte();      // always 0x00?
                this.Chara = Utils.ToEnum<CharaWithTotal>(reader.ReadByte());
                _ = reader.ReadUInt16();    // always 0x0000?
            }
        }

        public IReadOnlyDictionary<Level, PlayableStages> StoryFlags { get; }    // really...?

        public IReadOnlyDictionary<Level, PlayableStages> PracticeFlags { get; } // really...?

        public CharaWithTotal Chara { get; }
    }
}

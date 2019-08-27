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
    internal class ClearData : Th06.Chapter   // per character
    {
        public const string ValidSignature = "CLRD";
        public const short ValidSize = 0x001C;

        public ClearData(Th06.Chapter chapter)
            : base(chapter, ValidSignature, ValidSize)
        {
            var levels = Utils.GetEnumerator<Level>();
            var numLevels = levels.Count();
            this.StoryFlags = new Dictionary<Level, byte>(numLevels);
            this.PracticeFlags = new Dictionary<Level, byte>(numLevels);

            using (var reader = new BinaryReader(new MemoryStream(this.Data, false)))
            {
                reader.ReadUInt32();    // always 0x00000001?
                foreach (var level in levels)
                    this.StoryFlags.Add(level, reader.ReadByte());
                foreach (var level in levels)
                    this.PracticeFlags.Add(level, reader.ReadByte());
                this.Chara = Utils.ToEnum<Chara>(reader.ReadInt32());
            }
        }

        public Dictionary<Level, byte> StoryFlags { get; }    // really...?

        public Dictionary<Level, byte> PracticeFlags { get; } // really...?

        public Chara Chara { get; }
    }
}

//-----------------------------------------------------------------------
// <copyright file="ClearData.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

namespace ThScoreFileConverter.Models.Th06
{
    internal class ClearData : Chapter, IClearData  // per character
    {
        public const string ValidSignature = "CLRD";
        public const short ValidSize = 0x0018;

        public ClearData(Chapter chapter)
            : base(chapter, ValidSignature, ValidSize)
        {
            var levels = Utils.GetEnumerator<Level>();
            var numLevels = levels.Count();
            var storyFlags = new Dictionary<Level, byte>(numLevels);
            var practiceFlags = new Dictionary<Level, byte>(numLevels);

            using (var reader = new BinaryReader(new MemoryStream(this.Data, false)))
            {
                reader.ReadUInt32();    // always 0x00000010?

                foreach (var level in levels)
                    storyFlags.Add(level, reader.ReadByte());
                this.StoryFlags = storyFlags;

                foreach (var level in levels)
                    practiceFlags.Add(level, reader.ReadByte());
                this.PracticeFlags = practiceFlags;

                this.Chara = Utils.ToEnum<Chara>(reader.ReadInt16());
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
        public IReadOnlyDictionary<Level, byte> StoryFlags { get; }     // really...?

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
        public IReadOnlyDictionary<Level, byte> PracticeFlags { get; }  // really...?

        public Chara Chara { get; }
    }
}

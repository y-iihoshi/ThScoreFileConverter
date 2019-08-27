//-----------------------------------------------------------------------
// <copyright file="PracticeScore.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.IO;

namespace ThScoreFileConverter.Models.Th07
{
    internal class PracticeScore : Th06.Chapter // per character, level, stage
    {
        public const string ValidSignature = "PSCR";
        public const short ValidSize = 0x0018;

        public PracticeScore(Th06.Chapter chapter)
            : base(chapter, ValidSignature, ValidSize)
        {
            using (var reader = new BinaryReader(new MemoryStream(this.Data, false)))
            {
                reader.ReadUInt32();    // always 0x00000001?
                this.TrialCount = reader.ReadInt32();
                this.HighScore = reader.ReadInt32();
                this.Chara = Utils.ToEnum<Th07Converter.Chara>(reader.ReadByte());
                this.Level = Utils.ToEnum<Th07Converter.Level>(reader.ReadByte());
                this.Stage = Utils.ToEnum<Th07Converter.Stage>(reader.ReadByte());
                reader.ReadByte();      // always 0x00?
            }
        }

        public int TrialCount { get; }  // really...?

        public int HighScore { get; }

        public Th07Converter.Chara Chara { get; }

        public Th07Converter.Level Level { get; }

        public Th07Converter.Stage Stage { get; }
    }
}

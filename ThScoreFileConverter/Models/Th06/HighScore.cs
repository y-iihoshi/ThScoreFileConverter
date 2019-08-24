//-----------------------------------------------------------------------
// <copyright file="HighScore.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.IO;

namespace ThScoreFileConverter.Models.Th06
{
    internal class HighScore : Chapter  // per character, level, rank
    {
        public const string ValidSignature = "HSCR";
        public const short ValidSize = 0x001C;

        public HighScore(Chapter chapter)
            : base(chapter, ValidSignature, ValidSize)
        {
            using (var reader = new BinaryReader(new MemoryStream(this.Data, false)))
            {
                reader.ReadUInt32();    // always 0x00000001?
                this.Score = reader.ReadUInt32();
                this.Chara = Utils.ToEnum<Th06Converter.Chara>(reader.ReadByte());
                this.Level = Utils.ToEnum<Level>(reader.ReadByte());
                this.StageProgress = Utils.ToEnum<Th06Converter.StageProgress>(reader.ReadByte());
                this.Name = reader.ReadExactBytes(9);
            }
        }

        public HighScore(uint score)    // for InitialRanking only
            : base()
        {
            this.Score = score;
            this.Name = Encoding.Default.GetBytes("Nanashi\0\0");
        }

        public uint Score { get; }

        public Th06Converter.Chara Chara { get; }

        public Level Level { get; }

        public Th06Converter.StageProgress StageProgress { get; }

        public byte[] Name { get; } // Null-terminated
    }
}

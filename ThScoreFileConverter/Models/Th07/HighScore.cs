//-----------------------------------------------------------------------
// <copyright file="HighScore.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.IO;

namespace ThScoreFileConverter.Models.Th07
{
    internal class HighScore : Th06.Chapter   // per character, level, rank
    {
        public const string ValidSignature = "HSCR";
        public const short ValidSize = 0x0028;

        public HighScore(Th06.Chapter chapter)
            : base(chapter, ValidSignature, ValidSize)
        {
            using (var reader = new BinaryReader(new MemoryStream(this.Data, false)))
            {
                reader.ReadUInt32();    // always 0x00000001?
                this.Score = reader.ReadUInt32();
                this.SlowRate = reader.ReadSingle();
                this.Chara = Utils.ToEnum<Chara>(reader.ReadByte());
                this.Level = Utils.ToEnum<Level>(reader.ReadByte());
                this.StageProgress = Utils.ToEnum<Th07Converter.StageProgress>(reader.ReadByte());
                this.Name = reader.ReadExactBytes(9);
                this.Date = reader.ReadExactBytes(6);
                this.ContinueCount = reader.ReadUInt16();
            }
        }

        public HighScore(uint score)    // for InitialRanking only
            : base()
        {
            this.Score = score;
            this.Name = Encoding.Default.GetBytes("--------\0");
            this.Date = Encoding.Default.GetBytes("--/--\0");
        }

        public uint Score { get; }  // Divided by 10

        public float SlowRate { get; }

        public Chara Chara { get; }

        public Level Level { get; }

        public Th07Converter.StageProgress StageProgress { get; }

        public byte[] Name { get; } // Null-terminated

        public byte[] Date { get; } // "mm/dd\0"

        public ushort ContinueCount { get; }
    }
}

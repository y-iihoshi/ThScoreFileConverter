//-----------------------------------------------------------------------
// <copyright file="Score.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.IO;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th125
{
    internal class Score : Th095.Chapter, IScore    // per scene
    {
        public const string ValidSignature = "SC";
        public const ushort ValidVersion = 0x0000;
        public const int ValidSize = 0x00000048;

        public Score(Th095.Chapter chapter)
            : base(chapter, ValidSignature, ValidVersion, ValidSize)
        {
            using var stream = new MemoryStream(this.Data, false);
            using var reader = new BinaryReader(stream);

            var number = reader.ReadUInt32();
            this.LevelScene = (EnumHelper.To<Level>(number / 10), (int)((number % 10) + 1));
            this.HighScore = reader.ReadInt32();
            _ = reader.ReadExactBytes(0x04);
            this.Chara = EnumHelper.To<Chara>(reader.ReadInt32());
            _ = reader.ReadExactBytes(0x04);
            this.TrialCount = reader.ReadInt32();
            this.FirstSuccess = reader.ReadInt32();
            _ = reader.ReadUInt32();    // always 0x00000000?
            this.DateTime = reader.ReadUInt32();
            _ = reader.ReadUInt32();    // always 0x00000000?
            _ = reader.ReadUInt32();    // checksum of the bestshot file?
            _ = reader.ReadUInt32();    // always 0x00000001?
            this.BestshotScore = reader.ReadInt32();
            _ = reader.ReadExactBytes(0x08);
        }

        public (Level Level, int Scene) LevelScene { get; }

        public int HighScore { get; }

        public Chara Chara { get; }

        public int TrialCount { get; }

        public int FirstSuccess { get; }

        public uint DateTime { get; }   // UNIX time

        public int BestshotScore { get; }

        public static bool CanInitialize(Th095.Chapter chapter)
        {
            return chapter.Signature.Equals(ValidSignature, StringComparison.Ordinal)
                && (chapter.Version == ValidVersion)
                && (chapter.Size == ValidSize);
        }
    }
}

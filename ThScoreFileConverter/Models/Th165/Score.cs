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

namespace ThScoreFileConverter.Models.Th165
{
    internal class Score : Th10.Chapter, IScore  // per scene
    {
        public const string ValidSignature = "SN";
        public const ushort ValidVersion = 0x0001;
        public const int ValidSize = 0x00000234;

        public Score(Th10.Chapter chapter)
            : base(chapter, ValidSignature, ValidVersion, ValidSize)
        {
            using (var reader = new BinaryReader(new MemoryStream(this.Data, false)))
            {
                this.Number = reader.ReadInt32();
                this.ClearCount = reader.ReadInt32();
                _ = reader.ReadInt32(); // always same as ClearCount?
                this.ChallengeCount = reader.ReadInt32();
                this.NumPhotos = reader.ReadInt32();
                this.HighScore = reader.ReadInt32();
                _ = reader.ReadExactBytes(0x210);   // always all 0x00?
            }
        }

        public int Number { get; }

        public int ClearCount { get; }

        public int ChallengeCount { get; }

        public int NumPhotos { get; }

        public int HighScore { get; }

        public static bool CanInitialize(Th10.Chapter chapter)
        {
            return chapter.Signature.Equals(ValidSignature, StringComparison.Ordinal)
                && (chapter.Version == ValidVersion)
                && (chapter.Size == ValidSize);
        }
    }
}

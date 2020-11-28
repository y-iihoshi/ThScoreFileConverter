//-----------------------------------------------------------------------
// <copyright file="Score.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th143
{
    internal class Score : Th10.Chapter, IScore // per scene
    {
        public const string ValidSignature = "SN";
        public const ushort ValidVersion = 0x0001;
        public const int ValidSize = 0x00000314;

        public Score(Th10.Chapter chapter)
            : base(chapter, ValidSignature, ValidVersion, ValidSize)
        {
            var items = EnumHelper<ItemWithTotal>.Enumerable;

            using var stream = new MemoryStream(this.Data, false);
            using var reader = new BinaryReader(stream);

            this.Number = reader.ReadInt32();
            this.ClearCounts = items.ToDictionary(item => item, _ => reader.ReadInt32());
            this.ChallengeCounts = items.ToDictionary(item => item, _ => reader.ReadInt32());
            this.HighScore = reader.ReadInt32();
            _ = reader.ReadExactBytes(0x2A8);   // always all 0x00?
        }

        public int Number { get; }

        public IReadOnlyDictionary<ItemWithTotal, int> ClearCounts { get; }

        public IReadOnlyDictionary<ItemWithTotal, int> ChallengeCounts { get; }

        public int HighScore { get; }   // Divided by 10

        public static bool CanInitialize(Th10.Chapter chapter)
        {
            return chapter.Signature.Equals(ValidSignature, StringComparison.Ordinal)
                && (chapter.Version == ValidVersion)
                && (chapter.Size == ValidSize);
        }
    }
}

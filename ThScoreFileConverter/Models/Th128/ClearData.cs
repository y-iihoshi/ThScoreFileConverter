//-----------------------------------------------------------------------
// <copyright file="ClearData.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th128
{
    internal class ClearData : Th10.Chapter, IClearData // per route
    {
        public const string ValidSignature = "CR";
        public const ushort ValidVersion = 0x0003;
        public const int ValidSize = 0x0000066C;

        public ClearData(Th10.Chapter chapter)
            : base(chapter, ValidSignature, ValidVersion, ValidSize)
        {
            var levels = EnumHelper.GetEnumerable<Level>();

            using var stream = new MemoryStream(this.Data, false);
            using var reader = new BinaryReader(stream);

            this.Route = (RouteWithTotal)reader.ReadInt32();

            this.Rankings = levels.ToDictionary(
                level => level,
                _ => Enumerable.Range(0, 10).Select(rank =>
                {
                    var score = new ScoreData();
                    score.ReadFrom(reader);
                    return score;
                }).ToList() as IReadOnlyList<Th10.IScoreData<StageProgress>>);

            this.TotalPlayCount = reader.ReadInt32();
            this.PlayTime = reader.ReadInt32();
            this.ClearCounts = levels.ToDictionary(level => level, _ => reader.ReadInt32());
        }

        public RouteWithTotal Route { get; }

        public IReadOnlyDictionary<Level, IReadOnlyList<Th10.IScoreData<StageProgress>>> Rankings { get; }

        public int TotalPlayCount { get; }

        public int PlayTime { get; }    // = seconds * 60fps

        public IReadOnlyDictionary<Level, int> ClearCounts { get; }

        public static bool CanInitialize(Th10.Chapter chapter)
        {
            return chapter.Signature.Equals(ValidSignature, StringComparison.Ordinal)
                && (chapter.Version == ValidVersion)
                && (chapter.Size == ValidSize);
        }
    }
}

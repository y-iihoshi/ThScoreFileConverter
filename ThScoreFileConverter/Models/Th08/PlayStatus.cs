//-----------------------------------------------------------------------
// <copyright file="PlayStatus.cs" company="None">
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

namespace ThScoreFileConverter.Models.Th08
{
    internal class PlayStatus : Th06.Chapter, IPlayStatus
    {
        public const string ValidSignature = "PLST";
        public const short ValidSize = 0x0228;

        public PlayStatus(Th06.Chapter chapter)
            : base(chapter, ValidSignature, ValidSize)
        {
            var levels = Utils.GetEnumerator<Level>();
            var numLevels = levels.Count();

            using (var reader = new BinaryReader(new MemoryStream(this.Data, false)))
            {
                _ = reader.ReadUInt32();    // always 0x00000002?
                var hours = reader.ReadInt32();
                var minutes = reader.ReadInt32();
                var seconds = reader.ReadInt32();
                var milliseconds = reader.ReadInt32();
                this.TotalRunningTime = new Time(hours, minutes, seconds, milliseconds, false);
                hours = reader.ReadInt32();
                minutes = reader.ReadInt32();
                seconds = reader.ReadInt32();
                milliseconds = reader.ReadInt32();
                this.TotalPlayTime = new Time(hours, minutes, seconds, milliseconds, false);

                var playCounts = Utils.GetEnumerator<LevelPracticeWithTotal>().ToDictionary(level => level, _ =>
                {
                    var playCount = new PlayCount();
                    playCount.ReadFrom(reader);
                    return playCount as IPlayCount;
                });
                this.PlayCounts = playCounts
                    .Where(pair => Enum.IsDefined(typeof(Level), (int)pair.Key))
                    .ToDictionary(pair => (Level)pair.Key, pair => pair.Value);
                this.TotalPlayCount = playCounts[LevelPracticeWithTotal.Total];

                this.BgmFlags = reader.ReadExactBytes(21);
                _ = reader.ReadExactBytes(11);
            }
        }

        public Time TotalRunningTime { get; }

        public Time TotalPlayTime { get; }

        public IReadOnlyDictionary<Level, IPlayCount> PlayCounts { get; }

        public IPlayCount TotalPlayCount { get; }

        public IEnumerable<byte> BgmFlags { get; }
    }
}

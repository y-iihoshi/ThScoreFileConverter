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
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th16
{
    internal class ClearData : Th10.Chapter, IClearData  // per character
    {
        public const string ValidSignature = "CR";
        public const ushort ValidVersion = 0x0001;
        public const int ValidSize = 0x00005318;

        public ClearData(Th10.Chapter chapter)
            : base(chapter, ValidSignature, ValidVersion, ValidSize)
        {
            var levelsWithTotal = EnumHelper.GetEnumerable<LevelWithTotal>();
            var levels = EnumHelper.GetEnumerable<Level>();
            var stages = EnumHelper.GetEnumerable<StagePractice>();

            using var stream = new MemoryStream(this.Data, false);
            using var reader = new BinaryReader(stream);

            this.Chara = (CharaWithTotal)reader.ReadInt32();

            this.Rankings = levelsWithTotal.ToDictionary(
                level => level,
                _ => Enumerable.Range(0, 10).Select(rank =>
                {
                    var score = new ScoreData();
                    score.ReadFrom(reader);
                    return score;
                }).ToList() as IReadOnlyList<IScoreData>);

            _ = reader.ReadExactBytes(0x140);

            this.Cards = Enumerable.Range(0, Definitions.CardTable.Count).Select(_ =>
            {
                var card = new Th13.SpellCard<Level>();
                card.ReadFrom(reader);
                return card as Th13.ISpellCard<Level>;
            }).ToDictionary(card => card.Id);

            this.TotalPlayCount = reader.ReadInt32();
            this.PlayTime = reader.ReadInt32();
            _ = reader.ReadUInt32();
            this.ClearCounts = levelsWithTotal.ToDictionary(level => level, _ => reader.ReadInt32());
            _ = reader.ReadUInt32();
            this.ClearFlags = levelsWithTotal.ToDictionary(level => level, _ => reader.ReadInt32());
            _ = reader.ReadUInt32();

            this.Practices = levels
                .SelectMany(level => stages.Select(stage => (level, stage)))
                .ToDictionary(pair => pair, _ =>
                {
                    var practice = new Th13.Practice();
                    practice.ReadFrom(reader);
                    return practice as Th13.IPractice;
                });
        }

        public CharaWithTotal Chara { get; }

        public IReadOnlyDictionary<LevelWithTotal, IReadOnlyList<IScoreData>> Rankings { get; }

        public int TotalPlayCount { get; }

        public int PlayTime { get; }    // unit: 10ms

        public IReadOnlyDictionary<LevelWithTotal, int> ClearCounts { get; }

        public IReadOnlyDictionary<LevelWithTotal, int> ClearFlags { get; }  // Really...?

        public IReadOnlyDictionary<(Level, StagePractice), Th13.IPractice> Practices { get; }

        public IReadOnlyDictionary<int, Th13.ISpellCard<Level>> Cards { get; }

        public static bool CanInitialize(Th10.Chapter chapter)
        {
            return chapter.Signature.Equals(ValidSignature, StringComparison.Ordinal)
                && (chapter.Version == ValidVersion)
                && (chapter.Size == ValidSize);
        }
    }
}

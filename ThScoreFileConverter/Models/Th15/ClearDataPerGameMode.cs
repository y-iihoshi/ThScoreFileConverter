﻿//-----------------------------------------------------------------------
// <copyright file="ClearDataPerGameMode.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th15
{
    internal class ClearDataPerGameMode : IBinaryReadable, IClearDataPerGameMode
    {
        public ClearDataPerGameMode()
        {
            this.Rankings = ImmutableDictionary<LevelWithTotal, IReadOnlyList<IScoreData>>.Empty;
            this.ClearCounts = ImmutableDictionary<LevelWithTotal, int>.Empty;
            this.ClearFlags = ImmutableDictionary<LevelWithTotal, int>.Empty;
            this.Cards = ImmutableDictionary<int, Th13.ISpellCard<Level>>.Empty;
        }

        public IReadOnlyDictionary<LevelWithTotal, IReadOnlyList<IScoreData>> Rankings { get; private set; }

        public int TotalPlayCount { get; private set; }

        public int PlayTime { get; private set; }   // unit: 10ms

        public IReadOnlyDictionary<LevelWithTotal, int> ClearCounts { get; private set; }

        public IReadOnlyDictionary<LevelWithTotal, int> ClearFlags { get; private set; } // Really...?

        public IReadOnlyDictionary<int, Th13.ISpellCard<Level>> Cards { get; private set; }

        public void ReadFrom(BinaryReader reader)
        {
            if (reader is null)
                throw new ArgumentNullException(nameof(reader));

            var levelsWithTotal = EnumHelper<LevelWithTotal>.Enumerable;

            this.Rankings = levelsWithTotal.ToDictionary(
                level => level,
                _ => Enumerable.Range(0, 10).Select(rank =>
                {
                    var score = new ScoreData();
                    score.ReadFrom(reader);
                    return score;
                }).ToList() as IReadOnlyList<IScoreData>);

            _ = reader.ReadBytes(0x140);

            this.Cards = Enumerable.Range(0, Definitions.CardTable.Count).Select(_ =>
            {
                var card = new SpellCard();
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
        }
    }
}

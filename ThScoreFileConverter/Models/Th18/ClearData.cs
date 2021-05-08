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
using IClearData = ThScoreFileConverter.Models.Th13.IClearData<
    ThScoreFileConverter.Models.Th18.CharaWithTotal,
    ThScoreFileConverter.Models.Level,
    ThScoreFileConverter.Models.Level,
    ThScoreFileConverter.Models.Th14.LevelPracticeWithTotal,
    ThScoreFileConverter.Models.Stage,
    ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th13.StageProgress>>;
using IScoreData = ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th13.StageProgress>;

namespace ThScoreFileConverter.Models.Th18
{
    internal class ClearData : Th10.Chapter, IClearData // per character
    {
        public const string ValidSignature = "CR";
        public const ushort ValidVersion = 0x0003;
        public const int ValidSize = 0x000130F0;

        public ClearData(Th10.Chapter chapter)
            : base(chapter, ValidSignature, ValidVersion, ValidSize)
        {
            var levelsWithTotal = EnumHelper<Th14.LevelPracticeWithTotal>.Enumerable;
            var levelsExceptExtra = EnumHelper<Level>.Enumerable.Where(level => level != Level.Extra);
            var stagesExceptExtra = EnumHelper<Stage>.Enumerable.Where(stage => stage != Stage.Extra);

            using var stream = new MemoryStream(this.Data, false);
            using var reader = new BinaryReader(stream);

            this.Chara = (CharaWithTotal)reader.ReadInt32();

            this.Rankings = levelsWithTotal.ToDictionary(
                level => level,
                _ => Enumerable.Range(0, 10)
                    .Select(_ => BinaryReadableHelper.Create<Th17.ScoreData>(reader) as IScoreData)
                    .ToList() as IReadOnlyList<IScoreData>);

            this.Cards = Enumerable.Range(0, Definitions.CardTable.Count)
                .Select(_ => BinaryReadableHelper.Create<SpellCard>(reader) as Th13.ISpellCard<Level>)
                .ToDictionary(card => card.Id);

            _ = Enumerable.Range(0, 10).Select(_ => BinaryReadableHelper.Create<SpellCard>(reader)).ToList();

            this.TotalPlayCount = reader.ReadInt32();
            this.PlayTime = reader.ReadInt32();
            _ = reader.ReadUInt32();
            this.ClearCounts = levelsWithTotal.ToDictionary(level => level, _ => reader.ReadInt32());
            this.ClearFlags = levelsWithTotal.ToDictionary(level => level, _ => reader.ReadInt32());

            _ = reader.ReadExactBytes(0xCA08);

            this.Practices = levelsExceptExtra
                .SelectMany(level => stagesExceptExtra.Select(stage => (level, stage)))
                .ToDictionary(pair => pair, _ => BinaryReadableHelper.Create<Th13.Practice>(reader) as Th13.IPractice);

            _ = reader.ReadExactBytes(0x120);
        }

        public CharaWithTotal Chara { get; }

        public IReadOnlyDictionary<Th14.LevelPracticeWithTotal, IReadOnlyList<IScoreData>> Rankings { get; }

        public int TotalPlayCount { get; }

        public int PlayTime { get; }    // unit: 10ms

        public IReadOnlyDictionary<Th14.LevelPracticeWithTotal, int> ClearCounts { get; }

        public IReadOnlyDictionary<Th14.LevelPracticeWithTotal, int> ClearFlags { get; }  // Really...?

        public IReadOnlyDictionary<(Level Level, Stage Stage), Th13.IPractice> Practices { get; }

        public IReadOnlyDictionary<int, Th13.ISpellCard<Level>> Cards { get; }

        public static bool CanInitialize(Th10.Chapter chapter)
        {
            return chapter.Signature.Equals(ValidSignature, StringComparison.Ordinal)
                && (chapter.Version == ValidVersion)
                && (chapter.Size == ValidSize);
        }
    }
}

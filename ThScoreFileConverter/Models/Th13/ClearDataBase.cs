//-----------------------------------------------------------------------
// <copyright file="ClearDataBase.cs" company="None">
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

namespace ThScoreFileConverter.Models.Th13
{
    internal class ClearDataBase<
        TCharaWithTotal, TLevel, TLevelPractice, TLevelPracticeWithTotal, TStagePractice>
        : Th10.Chapter,
          IClearData<TCharaWithTotal, TLevel, TLevelPractice, TLevelPracticeWithTotal, TStagePractice>
        where TCharaWithTotal : struct, Enum
        where TLevel : struct, Enum
        where TLevelPractice : struct, Enum
        where TLevelPracticeWithTotal : struct, Enum
        where TStagePractice : struct, Enum
    {
        public const string ValidSignature = "CR";
        public const ushort ValidVersion = 0x0001;

        protected ClearDataBase(Th10.Chapter chapter, int validSize, int numCards)
            : base(chapter, ValidSignature, ValidVersion, validSize)
        {
            var levelsWithTotal = EnumHelper<TLevelPracticeWithTotal>.Enumerable;

            using var stream = new MemoryStream(this.Data, false);
            using var reader = new BinaryReader(stream);

            this.Chara = EnumHelper.To<TCharaWithTotal>(reader.ReadInt32());

            this.Rankings = levelsWithTotal.ToDictionary(
                level => level,
                _ => Enumerable.Range(0, 10).Select(rank =>
                {
                    var score = new ScoreData();
                    score.ReadFrom(reader);
                    return score;
                }).ToList() as IReadOnlyList<Th10.IScoreData<StageProgress>>);

            this.TotalPlayCount = reader.ReadInt32();
            this.PlayTime = reader.ReadInt32();
            this.ClearCounts = levelsWithTotal.ToDictionary(level => level, _ => reader.ReadInt32());
            this.ClearFlags = levelsWithTotal.ToDictionary(level => level, _ => reader.ReadInt32());

            this.Practices = EnumHelper<TLevelPractice>.Enumerable
                .SelectMany(level => EnumHelper<TStagePractice>.Enumerable.Select(stage => (level, stage)))
                .ToDictionary(pair => pair, _ =>
                {
                    var practice = new Practice();
                    practice.ReadFrom(reader);
                    return practice as IPractice;
                });

            this.Cards = Enumerable.Range(0, numCards).Select(_ =>
            {
                var card = new SpellCard<TLevel>();
                card.ReadFrom(reader);
                return card as ISpellCard<TLevel>;
            }).ToDictionary(card => card.Id);
        }

        public TCharaWithTotal Chara { get; }

        public IReadOnlyDictionary<TLevelPracticeWithTotal, IReadOnlyList<Th10.IScoreData<StageProgress>>> Rankings { get; }

        public int TotalPlayCount { get; }

        public int PlayTime { get; }    // = seconds * 60fps

        public IReadOnlyDictionary<TLevelPracticeWithTotal, int> ClearCounts { get; }

        public IReadOnlyDictionary<TLevelPracticeWithTotal, int> ClearFlags { get; }  // Really...?

        public IReadOnlyDictionary<(TLevelPractice Level, TStagePractice Stage), IPractice> Practices { get; }

        public IReadOnlyDictionary<int, ISpellCard<TLevel>> Cards { get; }

        protected static bool CanInitialize(Th10.Chapter chapter)
        {
            return chapter.Signature.Equals(ValidSignature, StringComparison.Ordinal) && (chapter.Version == ValidVersion);
        }
    }
}

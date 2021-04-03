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
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th16
{
    internal class ClearDataBase<
        TCharaWithTotal, TLevel, TLevelPractice, TLevelPracticeWithTotal, TStagePractice, TIScoreData, TScoreData>
        : Th10.Chapter,
          Th13.IClearData<
              TCharaWithTotal,
              TLevel,
              TLevelPractice,
              TLevelPracticeWithTotal,
              TStagePractice,
              TIScoreData>
        where TCharaWithTotal : struct, Enum
        where TLevel : struct, Enum
        where TLevelPractice : struct, Enum
        where TLevelPracticeWithTotal : struct, Enum
        where TStagePractice : struct, Enum
        where TIScoreData : class, Th10.IScoreData<Th13.StageProgress>
        where TScoreData : IBinaryReadable, Th10.IScoreData<Th13.StageProgress>, new()
    {
        public const string ValidSignature = "CR";

        protected ClearDataBase(Th10.Chapter chapter, ushort validVersion, int validSize, int numCards)
            : base(chapter, ValidSignature, validVersion, validSize)
        {
            var levelsWithTotal = EnumHelper<TLevelPracticeWithTotal>.Enumerable;

            using var stream = new MemoryStream(this.Data, false);
            using var reader = new BinaryReader(stream);

            this.Chara = EnumHelper.To<TCharaWithTotal>(reader.ReadInt32());

            this.Rankings = levelsWithTotal.ToDictionary(
                level => level,
                _ => Enumerable.Range(0, 10).Select(rank =>
                {
                    var score = new TScoreData();
                    score.ReadFrom(reader);
                    return score as TIScoreData;
                }).ToList() as IReadOnlyList<TIScoreData>);

            this.Cards = Enumerable.Range(0, numCards).Select(_ =>
            {
                var card = new Th13.SpellCard<TLevel>();
                card.ReadFrom(reader);
                return card as Th13.ISpellCard<TLevel>;
            }).ToDictionary(card => card.Id);

            this.TotalPlayCount = reader.ReadInt32();
            this.PlayTime = reader.ReadInt32();
            _ = reader.ReadUInt32();
            this.ClearCounts = levelsWithTotal.ToDictionary(level => level, _ => reader.ReadInt32());
            this.ClearFlags = levelsWithTotal.ToDictionary(level => level, _ => reader.ReadInt32());

            this.Practices = EnumHelper<TLevelPractice>.Enumerable
                .SelectMany(level => EnumHelper<TStagePractice>.Enumerable.Select(stage => (level, stage)))
                .ToDictionary(pair => pair, _ =>
                {
                    var practice = new Th13.Practice();
                    practice.ReadFrom(reader);
                    return practice as Th13.IPractice;
                });
        }

        public TCharaWithTotal Chara { get; }

        public IReadOnlyDictionary<TLevelPracticeWithTotal, IReadOnlyList<TIScoreData>> Rankings { get; }

        public int TotalPlayCount { get; }

        public int PlayTime { get; }    // unit: 10ms

        public IReadOnlyDictionary<TLevelPracticeWithTotal, int> ClearCounts { get; }

        public IReadOnlyDictionary<TLevelPracticeWithTotal, int> ClearFlags { get; }  // Really...?

        public IReadOnlyDictionary<(TLevelPractice Level, TStagePractice Stage), Th13.IPractice> Practices { get; }

        public IReadOnlyDictionary<int, Th13.ISpellCard<TLevel>> Cards { get; }

        protected static bool CanInitialize(Th10.Chapter chapter)
        {
            return chapter.Signature.Equals(ValidSignature, StringComparison.Ordinal);
        }
    }
}

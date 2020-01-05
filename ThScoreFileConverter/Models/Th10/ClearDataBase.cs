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

namespace ThScoreFileConverter.Models.Th10
{
    internal class ClearDataBase<TCharaWithTotal, TStageProgress, TScoreData>
        : Chapter, IClearData<TCharaWithTotal, TStageProgress>
        where TCharaWithTotal : struct, Enum
        where TStageProgress : struct, Enum
        where TScoreData : IBinaryReadable, IScoreData<TStageProgress>, new()
    {
        public const string ValidSignature = "CR";

        protected ClearDataBase(Chapter chapter, ushort validVersion, int validSize, int numCards)
            : base(chapter, ValidSignature, validVersion, validSize)
        {
            var levels = Utils.GetEnumerator<Level>();
            var levelsExceptExtra = levels.Where(lv => lv != Level.Extra);
            var stages = Utils.GetEnumerator<Stage>();
            var stagesExceptExtra = stages.Where(st => st != Stage.Extra);

            using var reader = new BinaryReader(new MemoryStream(this.Data, false));

            this.Chara = Utils.ToEnum<TCharaWithTotal>(reader.ReadInt32());

            this.Rankings = levels.ToDictionary(
                level => level,
                _ => (IReadOnlyList<IScoreData<TStageProgress>>)Enumerable.Range(0, 10).Select(rank =>
                {
                    var score = new TScoreData();
                    score.ReadFrom(reader);
                    return score;
                }).ToList());

            this.TotalPlayCount = reader.ReadInt32();
            this.PlayTime = reader.ReadInt32();
            this.ClearCounts = levels.ToDictionary(level => level, _ => reader.ReadInt32());

            this.Practices = levelsExceptExtra
                .SelectMany(level => stagesExceptExtra.Select(stage => (level, stage)))
                .ToDictionary(pair => pair, _ =>
                {
                    var practice = new Practice();
                    practice.ReadFrom(reader);
                    return practice as IPractice;
                });

            this.Cards = Enumerable.Range(0, numCards).Select(_ =>
            {
                var card = new SpellCard();
                card.ReadFrom(reader);
                return card as ISpellCard<Level>;
            }).ToDictionary(card => card.Id);
        }

        public TCharaWithTotal Chara { get; }

        public IReadOnlyDictionary<Level, IReadOnlyList<IScoreData<TStageProgress>>> Rankings { get; }

        public int TotalPlayCount { get; }

        public int PlayTime { get; }    // = seconds * 60fps

        public IReadOnlyDictionary<Level, int> ClearCounts { get; }

        public IReadOnlyDictionary<(Level, Stage), IPractice> Practices { get; }

        public IReadOnlyDictionary<int, ISpellCard<Level>> Cards { get; }

        protected static bool CanInitialize(Chapter chapter)
        {
            return chapter.Signature.Equals(ValidSignature, StringComparison.Ordinal);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using ThScoreFileConverter.Models.Th13;
using IScoreData = ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th13.StageProgress>;

namespace ThScoreFileConverterTests.Models.Th13.Stubs
{
    internal class ClearDataStub<TCharaWithTotal, TLevel, TLevelPractice, TLevelPracticeWithTotal, TStagePractice>
        : IClearData<TCharaWithTotal, TLevel, TLevelPractice, TLevelPracticeWithTotal, TStagePractice>
        where TCharaWithTotal : struct, Enum
        where TLevel : struct, Enum
        where TLevelPractice : struct, Enum
        where TLevelPracticeWithTotal : struct, Enum
        where TStagePractice : struct, Enum
    {
        public ClearDataStub()
        {
            this.Cards = ImmutableDictionary<int, ISpellCard<TLevel>>.Empty;
            this.ClearCounts = ImmutableDictionary<TLevelPracticeWithTotal, int>.Empty;
            this.ClearFlags = ImmutableDictionary<TLevelPracticeWithTotal, int>.Empty;
            this.Practices = ImmutableDictionary<(TLevelPractice, TStagePractice), IPractice>.Empty;
            this.Rankings = ImmutableDictionary<TLevelPracticeWithTotal, IReadOnlyList<IScoreData>>.Empty;
            this.Signature = string.Empty;
        }

        public ClearDataStub(
            IClearData<TCharaWithTotal, TLevel, TLevelPractice, TLevelPracticeWithTotal, TStagePractice> clearData)
        {
            this.Cards = clearData.Cards.ToDictionary(
                pair => pair.Key, pair => new SpellCardStub<TLevel>(pair.Value) as ISpellCard<TLevel>);
            this.Chara = clearData.Chara;
            this.ClearCounts = clearData.ClearCounts.ToDictionary(pair => pair.Key, pair => pair.Value);
            this.ClearFlags = clearData.ClearFlags.ToDictionary(pair => pair.Key, pair => pair.Value);
            this.PlayTime = clearData.PlayTime;
            this.Practices = clearData.Practices.ToDictionary(
                pair => pair.Key, pair => new PracticeStub(pair.Value) as IPractice);
            this.Rankings = clearData.Rankings.ToDictionary(
                pair => pair.Key,
                pair => pair.Value.Select(score => new Th10.Stubs.ScoreDataStub<StageProgress>(score)).ToList()
                    as IReadOnlyList<IScoreData>);
            this.TotalPlayCount = clearData.TotalPlayCount;
            this.Checksum = clearData.Checksum;
            this.IsValid = clearData.IsValid;
            this.Signature = clearData.Signature;
            this.Size = clearData.Size;
            this.Version = clearData.Version;
        }

        public IReadOnlyDictionary<int, ISpellCard<TLevel>> Cards { get; set; }

        public TCharaWithTotal Chara { get; set; }

        public IReadOnlyDictionary<TLevelPracticeWithTotal, int> ClearCounts { get; set; }

        public IReadOnlyDictionary<TLevelPracticeWithTotal, int> ClearFlags { get; set; }

        public int PlayTime { get; set; }

        public IReadOnlyDictionary<(TLevelPractice, TStagePractice), IPractice> Practices { get; set; }

        public IReadOnlyDictionary<TLevelPracticeWithTotal, IReadOnlyList<IScoreData>> Rankings { get; set; }

        public int TotalPlayCount { get; set; }

        public uint Checksum { get; set; }

        public bool IsValid { get; set; }

        public string Signature { get; set; }

        public int Size { get; set; }

        public ushort Version { get; set; }
    }
}

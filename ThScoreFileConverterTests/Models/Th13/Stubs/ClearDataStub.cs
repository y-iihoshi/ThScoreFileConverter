using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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

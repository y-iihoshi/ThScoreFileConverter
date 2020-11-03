using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th10;

namespace ThScoreFileConverterTests.Models.Th10.Stubs
{
    internal class ClearDataStub<TCharaWithTotal, TStageProgress> : IClearData<TCharaWithTotal, TStageProgress>
        where TCharaWithTotal : struct, Enum
        where TStageProgress : struct, Enum
    {
        public ClearDataStub()
        {
            this.Cards = ImmutableDictionary<int, ISpellCard<Level>>.Empty;
            this.ClearCounts = ImmutableDictionary<Level, int>.Empty;
            this.Practices = ImmutableDictionary<(Level, Stage), IPractice>.Empty;
            this.Rankings = ImmutableDictionary<Level, IReadOnlyList<IScoreData<TStageProgress>>>.Empty;
            this.Signature = string.Empty;
        }

        public IReadOnlyDictionary<int, ISpellCard<Level>> Cards { get; set; }

        public TCharaWithTotal Chara { get; set; }

        public IReadOnlyDictionary<Level, int> ClearCounts { get; set; }

        public int PlayTime { get; set; }

        public IReadOnlyDictionary<(Level, Stage), IPractice> Practices { get; set; }

        public IReadOnlyDictionary<Level, IReadOnlyList<IScoreData<TStageProgress>>> Rankings { get; set; }

        public int TotalPlayCount { get; set; }

        public uint Checksum { get; set; }

        public bool IsValid { get; set; }

        public string Signature { get; set; }

        public int Size { get; set; }

        public ushort Version { get; set; }
    }
}

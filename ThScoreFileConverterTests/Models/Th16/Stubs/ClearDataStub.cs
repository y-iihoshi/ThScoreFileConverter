using System.Collections.Generic;
using System.Collections.Immutable;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th16;
using IPractice = ThScoreFileConverter.Models.Th13.IPractice;
using ISpellCard = ThScoreFileConverter.Models.Th13.ISpellCard<ThScoreFileConverter.Models.Level>;

namespace ThScoreFileConverterTests.Models.Th16.Stubs
{
    internal class ClearDataStub : IClearData
    {
        public ClearDataStub()
        {
            this.Cards = ImmutableDictionary<int, ISpellCard>.Empty;
            this.ClearCounts = ImmutableDictionary<LevelWithTotal, int>.Empty;
            this.ClearFlags = ImmutableDictionary<LevelWithTotal, int>.Empty;
            this.Practices = ImmutableDictionary<(Level, StagePractice), IPractice>.Empty;
            this.Rankings = ImmutableDictionary<LevelWithTotal, IReadOnlyList<IScoreData>>.Empty;
            this.Signature = string.Empty;
        }

        public IReadOnlyDictionary<int, ISpellCard> Cards { get; set; }

        public CharaWithTotal Chara { get; set; }

        public IReadOnlyDictionary<LevelWithTotal, int> ClearCounts { get; set; }

        public IReadOnlyDictionary<LevelWithTotal, int> ClearFlags { get; set; }

        public int PlayTime { get; set; }

        public IReadOnlyDictionary<(Level, StagePractice), IPractice> Practices { get; set; }

        public IReadOnlyDictionary<LevelWithTotal, IReadOnlyList<IScoreData>> Rankings { get; set; }

        public int TotalPlayCount { get; set; }

        public uint Checksum { get; set; }

        public bool IsValid { get; set; }

        public string Signature { get; set; }

        public int Size { get; set; }

        public ushort Version { get; set; }
    }
}

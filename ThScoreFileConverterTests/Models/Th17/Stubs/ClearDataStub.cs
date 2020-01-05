using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th17;
using ThScoreFileConverterTests.Models.Th13.Stubs;
using IPractice = ThScoreFileConverter.Models.Th13.IPractice;
using ISpellCard = ThScoreFileConverter.Models.Th13.ISpellCard<ThScoreFileConverter.Models.Level>;

namespace ThScoreFileConverterTests.Models.Th17.Stubs
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

        public ClearDataStub(IClearData clearData)
        {
            this.Cards = clearData.Cards.ToDictionary(
                pair => pair.Key, pair => new SpellCardStub<Level>(pair.Value) as ISpellCard);
            this.Chara = clearData.Chara;
            this.ClearCounts = clearData.ClearCounts.ToDictionary(pair => pair.Key, pair => pair.Value);
            this.ClearFlags = clearData.ClearFlags.ToDictionary(pair => pair.Key, pair => pair.Value);
            this.PlayTime = clearData.PlayTime;
            this.Practices = clearData.Practices.ToDictionary(
                pair => pair.Key, pair => new PracticeStub(pair.Value) as IPractice);
            this.Rankings = clearData.Rankings.ToDictionary(
                pair => pair.Key,
                pair => pair.Value.Select(score => new ScoreDataStub(score)).ToList() as IReadOnlyList<IScoreData>);
            this.TotalPlayCount = clearData.TotalPlayCount;
            this.Checksum = clearData.Checksum;
            this.IsValid = clearData.IsValid;
            this.Signature = clearData.Signature;
            this.Size = clearData.Size;
            this.Version = clearData.Version;
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

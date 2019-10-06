using System;
using System.Collections.Generic;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th10;

namespace ThScoreFileConverterTests.Models.Th10.Stubs
{
    internal class ClearDataStub<TCharaWithTotal, TStageProgress> : IClearData<TCharaWithTotal, TStageProgress>
        where TCharaWithTotal : struct, Enum
        where TStageProgress : struct, Enum
    {
        public ClearDataStub() { }

        public ClearDataStub(IClearData<TCharaWithTotal, TStageProgress> clearData)
            : this()
        {
            this.Cards = clearData.Cards.ToDictionary(pair => pair.Key, pair => pair.Value);
            this.Chara = clearData.Chara;
            this.ClearCounts = clearData.ClearCounts.ToDictionary(pair => pair.Key, pair => pair.Value);
            this.PlayTime = clearData.PlayTime;
            this.Practices = clearData.Practices.ToDictionary(pair => pair.Key, pair => pair.Value);
            this.Rankings = clearData.Rankings.ToDictionary(
                pair => pair.Key, pair => pair.Value.ToList() as IReadOnlyList<IScoreData<TStageProgress>>);
            this.TotalPlayCount = clearData.TotalPlayCount;
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

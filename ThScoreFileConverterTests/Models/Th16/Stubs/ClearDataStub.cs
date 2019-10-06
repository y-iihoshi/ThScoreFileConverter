using System.Collections.Generic;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th13;
using ThScoreFileConverter.Models.Th16;

namespace ThScoreFileConverterTests.Models.Th16.Stubs
{
    internal class ClearDataStub : IClearData
    {
        public ClearDataStub() { }

        public ClearDataStub(IClearData clearData)
            : this()
        {
            this.Cards = clearData.Cards?.ToDictionary(
                pair => pair.Key, pair => new Th13.Stubs.SpellCardStub<Level>(pair.Value) as ISpellCard<Level>);
            this.Chara = clearData.Chara;
            this.ClearCounts = clearData.ClearCounts?.ToDictionary(pair => pair.Key, pair => pair.Value);
            this.ClearFlags = clearData.ClearFlags?.ToDictionary(pair => pair.Key, pair => pair.Value);
            this.PlayTime = clearData.PlayTime;
            this.Practices = clearData.Practices?.ToDictionary(
                pair => pair.Key, pair => new Th13.Stubs.PracticeStub(pair.Value) as IPractice);
            this.Rankings = clearData.Rankings?.ToDictionary(
                pair => pair.Key,
                pair => pair.Value?.Select(score => new ScoreDataStub(score))?.ToList() as IReadOnlyList<IScoreData>);
            this.TotalPlayCount = clearData.TotalPlayCount;
            this.Checksum = clearData.Checksum;
            this.IsValid = clearData.IsValid;
            this.Signature = clearData.Signature;
            this.Size = clearData.Size;
            this.Version = clearData.Version;
        }

        public IReadOnlyDictionary<int, ISpellCard<Level>> Cards { get; set; }

        public Th16Converter.CharaWithTotal Chara { get; set; }

        public IReadOnlyDictionary<LevelWithTotal, int> ClearCounts { get; set; }

        public IReadOnlyDictionary<LevelWithTotal, int> ClearFlags { get; set; }

        public int PlayTime { get; set; }

        public IReadOnlyDictionary<(Level, Th16Converter.StagePractice), IPractice> Practices { get; set; }

        public IReadOnlyDictionary<LevelWithTotal, IReadOnlyList<IScoreData>> Rankings { get; set; }

        public int TotalPlayCount { get; set; }

        public uint Checksum { get; set; }

        public bool IsValid { get; set; }

        public string Signature { get; set; }

        public int Size { get; set; }

        public ushort Version { get; set; }
    }
}

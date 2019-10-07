using System.Collections.Generic;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th10;
using ThScoreFileConverter.Models.Th128;
using ThScoreFileConverterTests.Models.Th10.Stubs;

namespace ThScoreFileConverterTests.Models.Th128.Stubs
{
    internal class ClearDataStub : IClearData
    {
        public ClearDataStub() { }

        public ClearDataStub(IClearData clearData)
            : this()
        {
            this.ClearCounts = clearData.ClearCounts?.ToDictionary(pair => pair.Key, pair => pair.Value);
            this.PlayTime = clearData.PlayTime;
            this.Rankings = clearData.Rankings?.ToDictionary(
                pair => pair.Key,
                pair => pair.Value?.Select(score => new ScoreDataStub<Th128Converter.StageProgress>(score))?.ToList()
                    as IReadOnlyList<IScoreData<Th128Converter.StageProgress>>);
            this.Route = clearData.Route;
            this.TotalPlayCount = clearData.TotalPlayCount;
            this.Checksum = clearData.Checksum;
            this.IsValid = clearData.IsValid;
            this.Signature = clearData.Signature;
            this.Size = clearData.Size;
            this.Version = clearData.Version;
        }

        public IReadOnlyDictionary<Level, int> ClearCounts { get; set; }

        public int PlayTime { get; set; }

        public IReadOnlyDictionary<Level, IReadOnlyList<IScoreData<Th128Converter.StageProgress>>> Rankings { get; set; }

        public Th128Converter.RouteWithTotal Route { get; set; }

        public int TotalPlayCount { get; set; }

        public uint Checksum { get; set; }

        public bool IsValid { get; set; }

        public string Signature { get; set; }

        public int Size { get; set; }

        public ushort Version { get; set; }
    }
}

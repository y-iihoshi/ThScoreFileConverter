using System.Collections.Generic;
using System.Collections.Immutable;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th128;
using IScoreData = ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th128.StageProgress>;

namespace ThScoreFileConverterTests.Models.Th128.Stubs
{
    internal class ClearDataStub : IClearData
    {
        public ClearDataStub()
        {
            this.ClearCounts = ImmutableDictionary<Level, int>.Empty;
            this.Rankings = ImmutableDictionary<Level, IReadOnlyList<IScoreData>>.Empty;
            this.Signature = string.Empty;
        }

        public IReadOnlyDictionary<Level, int> ClearCounts { get; set; }

        public int PlayTime { get; set; }

        public IReadOnlyDictionary<Level, IReadOnlyList<IScoreData>> Rankings { get; set; }

        public RouteWithTotal Route { get; set; }

        public int TotalPlayCount { get; set; }

        public uint Checksum { get; set; }

        public bool IsValid { get; set; }

        public string Signature { get; set; }

        public int Size { get; set; }

        public ushort Version { get; set; }
    }
}

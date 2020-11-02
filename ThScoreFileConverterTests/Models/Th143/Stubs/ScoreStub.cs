using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Models.Th143;

namespace ThScoreFileConverterTests.Models.Th143.Stubs
{
    internal class ScoreStub : IScore
    {
        public ScoreStub()
        {
            this.ChallengeCounts = ImmutableDictionary<ItemWithTotal, int>.Empty;
            this.ClearCounts = ImmutableDictionary<ItemWithTotal, int>.Empty;
            this.Signature = string.Empty;
        }

        public ScoreStub(IScore score)
        {
            this.ChallengeCounts = score.ChallengeCounts.ToDictionary();
            this.ClearCounts = score.ClearCounts.ToDictionary();
            this.HighScore = score.HighScore;
            this.Number = score.Number;
            this.Checksum = score.Checksum;
            this.IsValid = score.IsValid;
            this.Signature = score.Signature;
            this.Size = score.Size;
            this.Version = score.Version;
        }

        public IReadOnlyDictionary<ItemWithTotal, int> ChallengeCounts { get; set; }

        public IReadOnlyDictionary<ItemWithTotal, int> ClearCounts { get; set; }

        public int HighScore { get; set; }

        public int Number { get; set; }

        public uint Checksum { get; set; }

        public bool IsValid { get; set; }

        public string Signature { get; set; }

        public int Size { get; set; }

        public ushort Version { get; set; }
    }
}

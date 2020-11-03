using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th08;

namespace ThScoreFileConverterTests.Models.Th08.Stubs
{
    internal class PlayStatusStub : IPlayStatus
    {
        public PlayStatusStub()
        {
            this.BgmFlags = Enumerable.Empty<byte>();
            this.PlayCounts = ImmutableDictionary<Level, IPlayCount>.Empty;
            this.TotalPlayCount = PlayCountTests.MockInitialPlayCount().Object;
            this.TotalPlayTime = new Time(0);
            this.TotalRunningTime = new Time(0);
            this.Signature = string.Empty;
        }

        public PlayStatusStub(IPlayStatus playStatus)
        {
            this.BgmFlags = playStatus.BgmFlags.ToArray();
            this.PlayCounts = playStatus.PlayCounts.ToDictionary(
                pair => pair.Key, pair => PlayCountTests.MockPlayCount().Object);
            this.TotalPlayCount = PlayCountTests.MockPlayCount().Object;
            this.TotalPlayTime = playStatus.TotalPlayTime;
            this.TotalRunningTime = playStatus.TotalRunningTime;
            this.FirstByteOfData = playStatus.FirstByteOfData;
            this.Signature = playStatus.Signature;
            this.Size1 = playStatus.Size1;
            this.Size2 = playStatus.Size2;
        }

        public IEnumerable<byte> BgmFlags { get; set; }

        public IReadOnlyDictionary<Level, IPlayCount> PlayCounts { get; set; }

        public IPlayCount TotalPlayCount { get; set; }

        public Time TotalPlayTime { get; set; }

        public Time TotalRunningTime { get; set; }

        public byte FirstByteOfData { get; set; }

        public string Signature { get; set; }

        public short Size1 { get; set; }

        public short Size2 { get; set; }
    }
}

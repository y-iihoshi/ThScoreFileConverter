using System.Collections.Generic;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th09;

namespace ThScoreFileConverterTests.Models.Th09.Stubs
{
    internal class PlayStatusStub : IPlayStatus
    {
        public PlayStatusStub() { }

        public PlayStatusStub(IPlayStatus playStatus)
            : this()
        {
            this.BgmFlags = playStatus.BgmFlags?.ToArray();
            this.ClearCounts = playStatus.ClearCounts?.ToDictionary(pair => pair.Key, pair => pair.Value);
            this.ExtraFlags = playStatus.ExtraFlags?.ToDictionary(pair => pair.Key, pair => pair.Value);
            this.MatchFlags = playStatus.MatchFlags?.ToDictionary(pair => pair.Key, pair => pair.Value);
            this.StoryFlags = playStatus.StoryFlags?.ToDictionary(pair => pair.Key, pair => pair.Value);
            this.TotalPlayTime = playStatus.TotalPlayTime;
            this.TotalRunningTime = playStatus.TotalRunningTime;
            this.FirstByteOfData = playStatus.FirstByteOfData;
            this.Signature = playStatus.Signature;
            this.Size1 = playStatus.Size1;
            this.Size2 = playStatus.Size2;
        }

        public IEnumerable<byte> BgmFlags { get; set; }

        public IReadOnlyDictionary<Th09Converter.Chara, IClearCount> ClearCounts { get; set; }

        public IReadOnlyDictionary<Th09Converter.Chara, byte> ExtraFlags { get; set; }

        public IReadOnlyDictionary<Th09Converter.Chara, byte> MatchFlags { get; set; }

        public IReadOnlyDictionary<Th09Converter.Chara, byte> StoryFlags { get; set; }

        public Time TotalPlayTime { get; set; }

        public Time TotalRunningTime { get; set; }

        public byte FirstByteOfData { get; set; }

        public string Signature { get; set; }

        public short Size1 { get; set; }

        public short Size2 { get; set; }
    }
}

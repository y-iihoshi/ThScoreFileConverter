using System.Collections.Generic;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th16;

namespace ThScoreFileConverterTests.Models.Th16.Stubs
{
    internal class ScoreDataStub : IScoreData
    {
        public ScoreDataStub() { }

        public ScoreDataStub(IScoreData scoreData)
            : this()
        {
            this.Season = scoreData.Season;
            this.ContinueCount = scoreData.ContinueCount;
            this.DateTime = scoreData.DateTime;
            this.Name = scoreData.Name?.ToArray();
            this.Score = scoreData.Score;
            this.SlowRate = scoreData.SlowRate;
            this.StageProgress = scoreData.StageProgress;
        }

        public Th16Converter.Season Season { get; set; }

        public byte ContinueCount { get; set; }

        public uint DateTime { get; set; }

        public IEnumerable<byte> Name { get; set; }

        public uint Score { get; set; }

        public float SlowRate { get; set; }

        public Th16Converter.StageProgress StageProgress { get; set; }
    }
}

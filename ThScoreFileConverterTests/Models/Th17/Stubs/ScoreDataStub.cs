using System.Collections.Generic;
using System.Linq;
using ThScoreFileConverter.Models.Th17;
using StageProgress = ThScoreFileConverter.Models.Th13.StageProgress;

namespace ThScoreFileConverterTests.Models.Th17.Stubs
{
    internal class ScoreDataStub : IScoreData
    {
        public ScoreDataStub() { }

        public ScoreDataStub(IScoreData scoreData)
            : this()
        {
            this.ContinueCount = scoreData.ContinueCount;
            this.DateTime = scoreData.DateTime;
            this.Name = scoreData.Name?.ToArray();
            this.Score = scoreData.Score;
            this.SlowRate = scoreData.SlowRate;
            this.StageProgress = scoreData.StageProgress;
        }

        public byte ContinueCount { get; set; }

        public uint DateTime { get; set; }

        public IEnumerable<byte> Name { get; set; }

        public uint Score { get; set; }

        public float SlowRate { get; set; }

        public StageProgress StageProgress { get; set; }
    }
}

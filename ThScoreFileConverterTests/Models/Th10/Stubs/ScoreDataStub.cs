using System;
using System.Collections.Generic;
using System.Linq;
using ThScoreFileConverter.Models.Th10;

namespace ThScoreFileConverterTests.Models.Th10.Stubs
{
    internal class ScoreDataStub<TStageProgress> : IScoreData<TStageProgress>
        where TStageProgress : struct, Enum
    {
        public ScoreDataStub()
        {
            this.Name = Enumerable.Empty<byte>();
        }

        public ScoreDataStub(IScoreData<TStageProgress> scoreData)
        {
            this.ContinueCount = scoreData.ContinueCount;
            this.DateTime = scoreData.DateTime;
            this.Name = scoreData.Name.ToArray();
            this.Score = scoreData.Score;
            this.SlowRate = scoreData.SlowRate;
            this.StageProgress = scoreData.StageProgress;
        }

        public byte ContinueCount { get; set; }

        public uint DateTime { get; set; }

        public IEnumerable<byte> Name { get; set; }

        public uint Score { get; set; }

        public float SlowRate { get; set; }

        public TStageProgress StageProgress { get; set; }
    }
}

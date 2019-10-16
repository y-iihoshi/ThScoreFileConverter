using ThScoreFileConverter.Models.Th095;

namespace ThScoreFileConverterTests.Models.Th095.Stubs
{
    internal class ScoreStub : IScore
    {
        public ScoreStub() { }

        public ScoreStub(IScore score)
            : this()
        {
            this.BestshotScore = score.BestshotScore;
            this.DateTime = score.DateTime;
            this.HighScore = score.HighScore;
            this.LevelScene = score.LevelScene;
            this.SlowRate1 = score.SlowRate1;
            this.SlowRate2 = score.SlowRate2;
            this.TrialCount = score.TrialCount;
            this.Checksum = score.Checksum;
            this.IsValid = score.IsValid;
            this.Signature = score.Signature;
            this.Size = score.Size;
            this.Version = score.Version;
        }

        public int BestshotScore { get; set; }

        public uint DateTime { get; set; }

        public int HighScore { get; set; }

        public (Level Level, int Scene) LevelScene { get; set; }

        public float SlowRate1 { get; set; }

        public float SlowRate2 { get; set; }

        public int TrialCount { get; set; }

        public uint Checksum { get; set; }

        public bool IsValid { get; set; }

        public string Signature { get; set; }

        public int Size { get; set; }

        public ushort Version { get; set; }
    }
}

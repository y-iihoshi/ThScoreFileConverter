using ThScoreFileConverter.Models.Th125;

namespace ThScoreFileConverterTests.Models.Th125.Stubs
{
    internal class ScoreStub : IScore
    {
        public ScoreStub() { }

        public ScoreStub(IScore score)
            : this()
        {
            this.BestshotScore = score.BestshotScore;
            this.Chara = score.Chara;
            this.DateTime = score.DateTime;
            this.FirstSuccess = score.FirstSuccess;
            this.HighScore = score.HighScore;
            this.LevelScene = score.LevelScene;
            this.TrialCount = score.TrialCount;
            this.Checksum = score.Checksum;
            this.IsValid = score.IsValid;
            this.Signature = score.Signature;
            this.Size = score.Size;
            this.Version = score.Version;
        }

        public int BestshotScore { get; set; }

        public Chara Chara { get; set; }

        public uint DateTime { get; set; }

        public int FirstSuccess { get; set; }

        public int HighScore { get; set; }

        public (Level Level, int Scene) LevelScene { get; set; }

        public int TrialCount { get; set; }

        public uint Checksum { get; set; }

        public bool IsValid { get; set; }

        public string Signature { get; set; }

        public int Size { get; set; }

        public ushort Version { get; set; }
    }
}

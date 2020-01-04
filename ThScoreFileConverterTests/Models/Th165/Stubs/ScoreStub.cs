using ThScoreFileConverter.Models.Th165;

namespace ThScoreFileConverterTests.Models.Th165.Stubs
{
    internal class ScoreStub : IScore
    {
        public ScoreStub()
        {
            this.Signature = string.Empty;
        }

        public ScoreStub(IScore score)
        {
            this.ChallengeCount = score.ChallengeCount;
            this.ClearCount = score.ClearCount;
            this.HighScore = score.HighScore;
            this.Number = score.Number;
            this.NumPhotos = score.NumPhotos;
            this.Checksum = score.Checksum;
            this.IsValid = score.IsValid;
            this.Signature = score.Signature;
            this.Size = score.Size;
            this.Version = score.Version;
        }

        public int ChallengeCount { get; set; }

        public int ClearCount { get; set; }

        public int HighScore { get; set; }

        public int Number { get; set; }

        public int NumPhotos { get; set; }

        public uint Checksum { get; set; }

        public bool IsValid { get; set; }

        public string Signature { get; set; }

        public int Size { get; set; }

        public ushort Version { get; set; }
    }
}

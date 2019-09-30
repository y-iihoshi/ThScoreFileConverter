using ThScoreFileConverter.Models.Th07;

namespace ThScoreFileConverterTests.Models.Th07.Stubs
{
    internal class PracticeScoreStub : IPracticeScore
    {
        public PracticeScoreStub() { }

        public PracticeScoreStub(IPracticeScore score)
            : this()
        {
            this.Chara = score.Chara;
            this.HighScore = score.HighScore;
            this.Level = score.Level;
            this.Stage = score.Stage;
            this.TrialCount = score.TrialCount;
            this.FirstByteOfData = score.FirstByteOfData;
            this.Signature = score.Signature;
            this.Size1 = score.Size1;
            this.Size2 = score.Size2;
        }

        public Chara Chara { get; set; }

        public int HighScore { get; set; }

        public Level Level { get; set; }

        public Stage Stage { get; set; }

        public int TrialCount { get; set; }

        public byte FirstByteOfData { get; set; }

        public string Signature { get; set; }

        public short Size1 { get; set; }

        public short Size2 { get; set; }
    }
}

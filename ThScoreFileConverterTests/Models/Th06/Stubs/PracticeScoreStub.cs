using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th06;

namespace ThScoreFileConverterTests.Models.Th06.Stubs
{
    internal class PracticeScoreStub : IPracticeScore
    {
        public PracticeScoreStub()
        {
            this.Signature = string.Empty;
        }

        public PracticeScoreStub(IPracticeScore score)
        {
            this.Chara = score.Chara;
            this.HighScore = score.HighScore;
            this.Level = score.Level;
            this.Stage = score.Stage;
            this.FirstByteOfData = score.FirstByteOfData;
            this.Signature = score.Signature;
            this.Size1 = score.Size1;
            this.Size2 = score.Size2;
        }

        public Chara Chara { get; set; }

        public int HighScore { get; set; }

        public Level Level { get; set; }

        public Stage Stage { get; set; }

        public byte FirstByteOfData { get; set; }

        public string Signature { get; set; }

        public short Size1 { get; set; }

        public short Size2 { get; set; }
    }
}

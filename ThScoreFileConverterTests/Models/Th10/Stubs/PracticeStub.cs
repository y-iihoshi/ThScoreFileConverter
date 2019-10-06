using ThScoreFileConverter.Models.Th10;

namespace ThScoreFileConverterTests.Models.Th10.Stubs
{
    internal class PracticeStub : IPractice
    {
        public PracticeStub() { }

        public PracticeStub(IPractice practice)
            : this()
        {
            this.Score = practice.Score;
            this.StageFlag = practice.StageFlag;
        }

        public uint Score { get; set; }

        public uint StageFlag { get; set; }
    }
}

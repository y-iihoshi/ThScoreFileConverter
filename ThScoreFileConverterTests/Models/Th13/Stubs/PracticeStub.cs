using ThScoreFileConverter.Models.Th13;

namespace ThScoreFileConverterTests.Models.Th13.Stubs
{
    internal class PracticeStub : IPractice
    {
        public PracticeStub() { }

        public PracticeStub(IPractice practice)
            : this()
        {
            this.ClearFlag = practice.ClearFlag;
            this.EnableFlag = practice.EnableFlag;
            this.Score = practice.Score;
        }

        public byte ClearFlag { get; set; }

        public byte EnableFlag { get; set; }

        public uint Score { get; set; }
    }
}

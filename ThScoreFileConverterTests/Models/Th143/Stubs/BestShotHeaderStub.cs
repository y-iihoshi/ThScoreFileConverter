using ThScoreFileConverter.Models.Th143;

namespace ThScoreFileConverterTests.Models.Th143.Stubs
{
    internal class BestShotHeaderStub : IBestShotHeader
    {
        public BestShotHeaderStub() { }

        public BestShotHeaderStub(IBestShotHeader header)
            : base()
        {
            this.DateTime = header.DateTime;
            this.Day = header.Day;
            this.Height = header.Height;
            this.Scene = header.Scene;
            this.Signature = header.Signature;
            this.SlowRate = header.SlowRate;
            this.Width = header.Width;
        }

        public uint DateTime { get; set; }

        public Day Day { get; set; }

        public short Height { get; set; }

        public short Scene { get; set; }

        public string Signature { get; set; }

        public float SlowRate { get; set; }

        public short Width { get; set; }
    }
}

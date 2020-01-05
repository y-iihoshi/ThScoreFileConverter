using System.Collections.Generic;
using System.Linq;
using ThScoreFileConverter.Models.Th095;

namespace ThScoreFileConverterTests.Models.Th095.Stubs
{
    internal class BestShotHeaderStub : IBestShotHeader
    {
        public BestShotHeaderStub()
        {
            this.CardName = Enumerable.Empty<byte>();
            this.Signature = string.Empty;
        }

        public BestShotHeaderStub(IBestShotHeader header)
        {
            this.CardName = header.CardName.ToArray();
            this.Height = header.Height;
            this.Level = header.Level;
            this.ResultScore = header.ResultScore;
            this.Scene = header.Scene;
            this.Signature = header.Signature;
            this.SlowRate = header.SlowRate;
            this.Width = header.Width;
        }

        public IEnumerable<byte> CardName { get; set; }

        public short Height { get; set; }

        public Level Level { get; set; }

        public int ResultScore { get; set; }

        public short Scene { get; set; }

        public string Signature { get; set; }

        public float SlowRate { get; set; }

        public short Width { get; set; }
    }
}

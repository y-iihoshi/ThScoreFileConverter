using System.Collections.Generic;
using System.Linq;
using ThScoreFileConverter.Models.Th095;

namespace ThScoreFileConverterTests.Models.Th095.Stubs
{
    internal class BestShotHeaderStub : IBestShotHeader
    {
        public BestShotHeaderStub() { }

        public BestShotHeaderStub(IBestShotHeader header)
            : this()
        {
            this.CardName = header.CardName?.ToArray();
            this.Height = header.Height;
            this.Level = header.Level;
            this.Scene = header.Scene;
            this.Score = header.Score;
            this.Signature = header.Signature;
            this.SlowRate = header.SlowRate;
            this.Width = header.Width;
        }

        public IEnumerable<byte> CardName { get; set; }

        public short Height { get; set; }

        public Level Level { get; set; }

        public short Scene { get; set; }

        public int Score { get; set; }

        public string Signature { get; set; }

        public float SlowRate { get; set; }

        public short Width { get; set; }
    }
}

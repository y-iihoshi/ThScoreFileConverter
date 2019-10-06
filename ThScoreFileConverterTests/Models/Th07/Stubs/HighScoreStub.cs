using System.Collections.Generic;
using System.Linq;
using ThScoreFileConverter.Models.Th07;

namespace ThScoreFileConverterTests.Models.Th07.Stubs
{
    internal class HighScoreStub : IHighScore
    {
        public HighScoreStub() { }

        public HighScoreStub(IHighScore highScore)
            : this()
        {
            this.Chara = highScore.Chara;
            this.ContinueCount = highScore.ContinueCount;
            this.Date = highScore.Date?.ToArray();
            this.Level = highScore.Level;
            this.Name = highScore.Name?.ToArray();
            this.Score = highScore.Score;
            this.SlowRate = highScore.SlowRate;
            this.StageProgress = highScore.StageProgress;
            this.FirstByteOfData = highScore.FirstByteOfData;
            this.Signature = highScore.Signature;
            this.Size1 = highScore.Size1;
            this.Size2 = highScore.Size2;
        }

        public Chara Chara { get; set; }

        public ushort ContinueCount { get; set; }

        public IEnumerable<byte> Date { get; set; }

        public Level Level { get; set; }

        public IEnumerable<byte> Name { get; set; }

        public uint Score { get; set; }

        public float SlowRate { get; set; }

        public StageProgress StageProgress { get; set; }

        public byte FirstByteOfData { get; set; }

        public string Signature { get; set; }

        public short Size1 { get; set; }

        public short Size2 { get; set; }
    }
}

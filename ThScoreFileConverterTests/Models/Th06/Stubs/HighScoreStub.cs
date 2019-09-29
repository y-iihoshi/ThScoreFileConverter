using System.Collections.Generic;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th06;

namespace ThScoreFileConverterTests.Models.Th06.Stubs
{
    internal class HighScoreStub : IHighScore
    {
        public HighScoreStub() { }

        public HighScoreStub(IHighScore highScore)
            : this()
        {
            this.Chara = highScore.Chara;
            this.Level = highScore.Level;
            this.Score = highScore.Score;
            this.Name = highScore.Name.ToArray();
            this.StageProgress = highScore.StageProgress;
            this.FirstByteOfData = highScore.FirstByteOfData;
            this.Signature = highScore.Signature;
            this.Size1 = highScore.Size1;
            this.Size2 = highScore.Size2;
        }

        public Chara Chara { get; set; }

        public Level Level { get; set; }

        public IEnumerable<byte> Name { get; set; }

        public uint Score { get; set; }

        public StageProgress StageProgress { get; set; }

        public byte FirstByteOfData { get; set; }

        public string Signature { get; set; }

        public short Size1 { get; set; }

        public short Size2 { get; set; }
    }
}

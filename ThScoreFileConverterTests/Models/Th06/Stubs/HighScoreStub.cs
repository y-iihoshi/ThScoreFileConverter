using System.Collections.Generic;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th06;
using IHighScore = ThScoreFileConverter.Models.Th06.IHighScore<
    ThScoreFileConverter.Models.Th06.Chara,
    ThScoreFileConverter.Models.Level,
    ThScoreFileConverter.Models.Th06.StageProgress>;

namespace ThScoreFileConverterTests.Models.Th06.Stubs
{
    internal class HighScoreStub : IHighScore
    {
        public HighScoreStub()
        {
            this.Name = Enumerable.Empty<byte>();
            this.Signature = string.Empty;
        }

        public HighScoreStub(IHighScore highScore)
        {
            this.Chara = highScore.Chara;
            this.Level = highScore.Level;
            this.Name = highScore.Name.ToArray();
            this.Score = highScore.Score;
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

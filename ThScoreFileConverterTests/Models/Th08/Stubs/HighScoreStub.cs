using System.Collections.Generic;
using System.Linq;
using ThScoreFileConverter.Models;
using IHighScore = ThScoreFileConverter.Models.Th08.IHighScore<
    ThScoreFileConverter.Models.Th08Converter.Chara,
    ThScoreFileConverter.Models.Level,
    ThScoreFileConverter.Models.Th08Converter.StageProgress>;

namespace ThScoreFileConverterTests.Models.Th08.Stubs
{
    internal class HighScoreStub : IHighScore
    {
        public HighScoreStub() { }

        public HighScoreStub(IHighScore highScore)
            : this()
        {
            this.BombCount = highScore.BombCount;
            this.CardFlags = highScore.CardFlags?.ToDictionary(pair => pair.Key, pair => pair.Value);
            this.HumanRate = highScore.HumanRate;
            this.LastSpellCount = highScore.LastSpellCount;
            this.MissCount = highScore.MissCount;
            this.PauseCount = highScore.PauseCount;
            this.PlayerNum = highScore.PlayerNum;
            this.PlayTime = highScore.PlayTime;
            this.PointItem = highScore.PointItem;
            this.TimePoint = highScore.TimePoint;
            this.ContinueCount = highScore.ContinueCount;
            this.Date = highScore.Date?.ToArray();
            this.SlowRate = highScore.SlowRate;
            this.Chara = highScore.Chara;
            this.Level = highScore.Level;
            this.Name = highScore.Name?.ToArray();
            this.Score = highScore.Score;
            this.StageProgress = highScore.StageProgress;
            this.FirstByteOfData = highScore.FirstByteOfData;
            this.Signature = highScore.Signature;
            this.Size1 = highScore.Size1;
            this.Size2 = highScore.Size2;
        }

        public int BombCount { get; set; }

        public IReadOnlyDictionary<int, byte> CardFlags { get; set; }

        public int HumanRate { get; set; }

        public int LastSpellCount { get; set; }

        public int MissCount { get; set; }

        public int PauseCount { get; set; }

        public byte PlayerNum { get; set; }

        public uint PlayTime { get; set; }

        public int PointItem { get; set; }

        public int TimePoint { get; set; }

        public ushort ContinueCount { get; set; }

        public IEnumerable<byte> Date { get; set; }

        public float SlowRate { get; set; }

        public Th08Converter.Chara Chara { get; set; }

        public Level Level { get; set; }

        public IEnumerable<byte> Name { get; set; }

        public uint Score { get; set; }

        public Th08Converter.StageProgress StageProgress { get; set; }

        public byte FirstByteOfData { get; set; }

        public string Signature { get; set; }

        public short Size1 { get; set; }

        public short Size2 { get; set; }
    }
}

using System.Collections.Generic;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th08;
using Stage = ThScoreFileConverter.Models.Th08.Stage;

namespace ThScoreFileConverterTests.Models.Th08.Stubs
{
    internal class PracticeScoreStub : IPracticeScore
    {
        public PracticeScoreStub() { }

        public PracticeScoreStub(IPracticeScore score)
            : this()
        {
            this.Chara = score.Chara;
            this.HighScores = score.HighScores?.ToDictionary(pair => pair.Key, pair => pair.Value);
            this.PlayCounts = score.PlayCounts?.ToDictionary(pair => pair.Key, pair => pair.Value);
            this.FirstByteOfData = score.FirstByteOfData;
            this.Signature = score.Signature;
            this.Size1 = score.Size1;
            this.Size2 = score.Size2;
        }

        public Chara Chara { get; set; }

        public IReadOnlyDictionary<(Stage, Level), int> HighScores { get; set; }

        public IReadOnlyDictionary<(Stage, Level), int> PlayCounts { get; set; }

        public byte FirstByteOfData { get; set; }

        public string Signature { get; set; }

        public short Size1 { get; set; }

        public short Size2 { get; set; }
    }
}

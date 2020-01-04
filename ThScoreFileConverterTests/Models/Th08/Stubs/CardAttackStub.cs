using System.Collections.Generic;
using System.Linq;
using ThScoreFileConverter.Models.Th08;

namespace ThScoreFileConverterTests.Models.Th08.Stubs
{
    internal class CardAttackStub : ICardAttack
    {
        public CardAttackStub()
        {
            this.CardName = Enumerable.Empty<byte>();
            this.Comment = Enumerable.Empty<byte>();
            this.EnemyName = Enumerable.Empty<byte>();
            this.Signature = string.Empty;
            this.PracticeCareer = new CardAttackCareerStub();
            this.StoryCareer = new CardAttackCareerStub();
        }

        public CardAttackStub(ICardAttack attack)
        {
            this.CardId = attack.CardId;
            this.CardName = attack.CardName.ToArray();
            this.Comment = attack.Comment.ToArray();
            this.EnemyName = attack.EnemyName.ToArray();
            this.Level = attack.Level;
            this.PracticeCareer = new CardAttackCareerStub(attack.PracticeCareer);
            this.StoryCareer = new CardAttackCareerStub(attack.StoryCareer);
            this.FirstByteOfData = attack.FirstByteOfData;
            this.Signature = attack.Signature;
            this.Size1 = attack.Size1;
            this.Size2 = attack.Size2;
        }

        public short CardId { get; set; }

        public IEnumerable<byte> CardName { get; set; }

        public IEnumerable<byte> Comment { get; set; }

        public IEnumerable<byte> EnemyName { get; set; }

        public LevelPracticeWithTotal Level { get; set; }

        public ICardAttackCareer PracticeCareer { get; set; }

        public ICardAttackCareer StoryCareer { get; set; }

        public byte FirstByteOfData { get; set; }

        public string Signature { get; set; }

        public short Size1 { get; set; }

        public short Size2 { get; set; }

        public bool HasTried()
        {
            return this.StoryCareer.TrialCounts[CharaWithTotal.Total] > 0
                || this.PracticeCareer.TrialCounts[CharaWithTotal.Total] > 0;
        }
    }
}

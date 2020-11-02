using System.Collections.Generic;
using System.Linq;
using Moq;
using ThScoreFileConverter.Extensions;
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
            this.PracticeCareer = CardAttackCareerTests.MockInitialCardAttackCareer().Object;
            this.StoryCareer = CardAttackCareerTests.MockInitialCardAttackCareer().Object;
        }

        public CardAttackStub(ICardAttack attack)
        {
            static Mock<ICardAttackCareer> Copy(ICardAttackCareer career)
            {
                var mock = new Mock<ICardAttackCareer>();
                _ = mock.SetupGet(m => m.ClearCounts).Returns(career.ClearCounts.ToDictionary());
                _ = mock.SetupGet(m => m.MaxBonuses).Returns(career.MaxBonuses.ToDictionary());
                _ = mock.SetupGet(m => m.TrialCounts).Returns(career.TrialCounts.ToDictionary());
                return mock;
            }

            this.CardId = attack.CardId;
            this.CardName = attack.CardName.ToArray();
            this.Comment = attack.Comment.ToArray();
            this.EnemyName = attack.EnemyName.ToArray();
            this.Level = attack.Level;
            this.PracticeCareer = Copy(attack.PracticeCareer).Object;
            this.StoryCareer = Copy(attack.StoryCareer).Object;
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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th06;
using ThScoreFileConverterTests.Models.Th06.Stubs;
using IClearData = ThScoreFileConverter.Models.Th06.IClearData<
    ThScoreFileConverter.Models.Th06.Chara, ThScoreFileConverter.Models.Level>;

namespace ThScoreFileConverterTests.Models.Th06
{
    [TestClass]
    public class AllScoreDataTests
    {
        [TestMethod]
        public void AllScoreDataTest()
        {
            var allScoreData = new AllScoreData();

            Assert.IsNull(allScoreData.Header);
            Assert.AreEqual(0, allScoreData.Rankings.Count);
            Assert.AreEqual(0, allScoreData.ClearData.Count);
            Assert.AreEqual(0, allScoreData.CardAttacks.Count);
            Assert.AreEqual(0, allScoreData.PracticeScores.Count);
        }

        [TestMethod]
        public void SetHeaderTest()
        {
            var chapter = TestUtils.Create<Chapter>(HeaderTests.MakeByteArray(HeaderTests.ValidProperties));
            var header = new Header(chapter);

            var allScoreData = new AllScoreData();
            allScoreData.Set(header);

            Assert.AreSame(header, allScoreData.Header);
        }

        [TestMethod]
        public void SetHeaderTestTwice()
        {
            var chapter = TestUtils.Create<Chapter>(HeaderTests.MakeByteArray(HeaderTests.ValidProperties));
            var header1 = new Header(chapter);
            var header2 = new Header(chapter);

            var allScoreData = new AllScoreData();
            allScoreData.Set(header1);
            allScoreData.Set(header2);

            Assert.AreNotSame(header1, allScoreData.Header);
            Assert.AreSame(header2, allScoreData.Header);
        }

        [TestMethod]
        public void SetHighScoreTest()
        {
            var score = new HighScoreStub(HighScoreTests.ValidStub)
            {
                Score = 876543u,
            };

            var allScoreData = new AllScoreData();
            allScoreData.Set(score);

            Assert.AreSame(score, allScoreData.Rankings[(score.Chara, score.Level)][2]);
        }

        [TestMethod]
        public void SetHighScoreTestTwice()
        {
            var score1 = new HighScoreStub(HighScoreTests.ValidStub)
            {
                Score = 876543u,
            };
            var score2 = new HighScoreStub(score1);

            var allScoreData = new AllScoreData();
            allScoreData.Set(score1);
            allScoreData.Set(score2);

            Assert.AreSame(score1, allScoreData.Rankings[(score1.Chara, score1.Level)][2]);
            Assert.AreSame(score2, allScoreData.Rankings[(score2.Chara, score2.Level)][3]);
        }

        [TestMethod]
        public void SetClearDataTest()
        {
            var clearData = new Mock<IClearData>();

            var allScoreData = new AllScoreData();
            allScoreData.Set(clearData.Object);

            Assert.AreSame(clearData.Object, allScoreData.ClearData[clearData.Object.Chara]);
        }

        [TestMethod]
        public void SetClearDataTestTwice()
        {
            var clearData1 = new Mock<IClearData>();
            var clearData2 = new Mock<IClearData>();
            _ = clearData2.SetupGet(m => m.Chara).Returns(clearData1.Object.Chara);

            var allScoreData = new AllScoreData();
            allScoreData.Set(clearData1.Object);
            allScoreData.Set(clearData2.Object);

            Assert.AreSame(clearData1.Object, allScoreData.ClearData[clearData1.Object.Chara]);
            Assert.AreNotSame(clearData2.Object, allScoreData.ClearData[clearData2.Object.Chara]);
        }

        [TestMethod]
        public void SetCardAttackTest()
        {
            var attack = new Mock<ICardAttack>();
            _ = attack.SetupGet(m => m.CardId).Returns(1);

            var allScoreData = new AllScoreData();
            allScoreData.Set(attack.Object);

            Assert.AreSame(attack.Object, allScoreData.CardAttacks[attack.Object.CardId]);
        }

        [TestMethod]
        public void SetCardAttackTestTwice()
        {
            var attack1 = new Mock<ICardAttack>();
            _ = attack1.SetupGet(m => m.CardId).Returns(1);
            var attack2 = new Mock<ICardAttack>();
            _ = attack2.SetupGet(m => m.CardId).Returns(attack1.Object.CardId);

            var allScoreData = new AllScoreData();
            allScoreData.Set(attack1.Object);
            allScoreData.Set(attack2.Object);

            Assert.AreSame(attack1.Object, allScoreData.CardAttacks[attack1.Object.CardId]);
            Assert.AreNotSame(attack2.Object, allScoreData.CardAttacks[attack2.Object.CardId]);
        }

        [TestMethod]
        public void SetPracticeScoreTest()
        {
            var score = new PracticeScoreStub(PracticeScoreTests.ValidStub)
            {
                Level = Level.Normal,
                Stage = Stage.Six,
            };

            var allScoreData = new AllScoreData();
            allScoreData.Set(score);

            Assert.AreSame(score, allScoreData.PracticeScores[(score.Chara, score.Level, score.Stage)]);
        }

        [TestMethod]
        public void SetPracticeScoreTestTwice()
        {
            var score1 = new PracticeScoreStub(PracticeScoreTests.ValidStub)
            {
                Level = Level.Normal,
                Stage = Stage.Six,
            };
            var score2 = new PracticeScoreStub(score1);

            var allScoreData = new AllScoreData();
            allScoreData.Set(score1);
            allScoreData.Set(score2);

            Assert.AreSame(score1, allScoreData.PracticeScores[(score1.Chara, score1.Level, score1.Stage)]);
            Assert.AreNotSame(score2, allScoreData.PracticeScores[(score2.Chara, score2.Level, score2.Stage)]);
        }

        [DataTestMethod]
        [DataRow(Level.Easy, Stage.Six)]
        [DataRow(Level.Extra, Stage.Extra)]
        [DataRow(Level.Extra, Stage.Six)]
        [DataRow(Level.Normal, Stage.Extra)]
        public void SetPracticeScoreTestInvalidPracticeStage(int level, int stage)
        {
            var score = new PracticeScoreStub(PracticeScoreTests.ValidStub)
            {
                Level = TestUtils.Cast<Level>(level),
                Stage = TestUtils.Cast<Stage>(stage),
            };

            var allScoreData = new AllScoreData();
            allScoreData.Set(score);

            Assert.AreEqual(0, allScoreData.PracticeScores.Count);
        }
    }
}

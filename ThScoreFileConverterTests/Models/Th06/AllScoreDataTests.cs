using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th06;
using IClearData = ThScoreFileConverter.Models.Th06.IClearData<
    ThScoreFileConverter.Models.Th06.Chara, ThScoreFileConverter.Models.Level>;
using IHighScore = ThScoreFileConverter.Models.Th06.IHighScore<
    ThScoreFileConverter.Models.Th06.Chara,
    ThScoreFileConverter.Models.Level,
    ThScoreFileConverter.Models.Th06.StageProgress>;

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
            var score = new Mock<IHighScore>();
            _ = score.SetupGet(m => m.Score).Returns(876543u);

            var allScoreData = new AllScoreData();
            allScoreData.Set(score.Object);

            Assert.AreSame(score.Object, allScoreData.Rankings[(score.Object.Chara, score.Object.Level)][2]);
        }

        [TestMethod]
        public void SetHighScoreTestTwice()
        {
            var score1 = new Mock<IHighScore>();
            _ = score1.SetupGet(m => m.Score).Returns(876543u);
            var score2 = new Mock<IHighScore>();
            _ = score2.SetupGet(m => m.Score).Returns(876543u);

            var allScoreData = new AllScoreData();
            allScoreData.Set(score1.Object);
            allScoreData.Set(score2.Object);

            Assert.AreSame(score1.Object, allScoreData.Rankings[(score1.Object.Chara, score1.Object.Level)][2]);
            Assert.AreSame(score2.Object, allScoreData.Rankings[(score2.Object.Chara, score2.Object.Level)][3]);
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
            var score = new Mock<IPracticeScore>();

            var allScoreData = new AllScoreData();
            allScoreData.Set(score.Object);

            Assert.AreSame(
                score.Object,
                allScoreData.PracticeScores[(score.Object.Chara, score.Object.Level, score.Object.Stage)]);
        }

        [TestMethod]
        public void SetPracticeScoreTestTwice()
        {
            var score1 = new Mock<IPracticeScore>();
            var score2 = new Mock<IPracticeScore>();
            _ = score2.SetupGet(m => m.Level).Returns(score1.Object.Level);
            _ = score2.SetupGet(m => m.Stage).Returns(score1.Object.Stage);

            var allScoreData = new AllScoreData();
            allScoreData.Set(score1.Object);
            allScoreData.Set(score2.Object);

            Assert.AreSame(
                score1.Object,
                allScoreData.PracticeScores[(score1.Object.Chara, score1.Object.Level, score1.Object.Stage)]);
            Assert.AreNotSame(
                score2.Object,
                allScoreData.PracticeScores[(score2.Object.Chara, score2.Object.Level, score2.Object.Stage)]);
        }

        [DataTestMethod]
        [DataRow(Level.Easy, Stage.Six)]
        [DataRow(Level.Extra, Stage.Extra)]
        [DataRow(Level.Extra, Stage.Six)]
        [DataRow(Level.Normal, Stage.Extra)]
        public void SetPracticeScoreTestInvalidPracticeStage(int level, int stage)
        {
            var score = new Mock<IPracticeScore>();
            _ = score.SetupGet(m => m.Level).Returns(TestUtils.Cast<Level>(level));
            _ = score.SetupGet(m => m.Stage).Returns(TestUtils.Cast<Stage>(stage));

            var allScoreData = new AllScoreData();
            allScoreData.Set(score.Object);

            Assert.AreEqual(0, allScoreData.PracticeScores.Count);
        }
    }
}

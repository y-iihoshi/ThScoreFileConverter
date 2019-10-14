using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th06;
using ThScoreFileConverterTests.Models.Th06.Stubs;
using ClearDataStub = ThScoreFileConverterTests.Models.Th06.Stubs.ClearDataStub<
    ThScoreFileConverter.Models.Th06.Chara, ThScoreFileConverter.Models.Level>;

namespace ThScoreFileConverterTests.Models.Th06
{
    [TestClass]
    public class AllScoreDataTests
    {
        [TestMethod]
        public void AllScoreDataTest() => TestUtils.Wrap(() =>
        {
            var allScoreData = new AllScoreData();

            Assert.IsNull(allScoreData.Header);
            Assert.AreEqual(0, allScoreData.Rankings.Count);
            Assert.AreEqual(0, allScoreData.ClearData.Count);
            Assert.AreEqual(0, allScoreData.CardAttacks.Count);
            Assert.AreEqual(0, allScoreData.PracticeScores.Count);
        });

        [TestMethod]
        public void SetHeaderTest() => TestUtils.Wrap(() =>
        {
            var chapter = TestUtils.Create<Chapter>(HeaderTests.MakeByteArray(HeaderTests.ValidProperties));
            var header = new Header(chapter);

            var allScoreData = new AllScoreData();
            allScoreData.Set(header);

            Assert.AreSame(header, allScoreData.Header);
        });

        [TestMethod]
        public void SetHeaderTestTwice() => TestUtils.Wrap(() =>
        {
            var chapter = TestUtils.Create<Chapter>(HeaderTests.MakeByteArray(HeaderTests.ValidProperties));
            var header1 = new Header(chapter);
            var header2 = new Header(chapter);

            var allScoreData = new AllScoreData();
            allScoreData.Set(header1);
            allScoreData.Set(header2);

            Assert.AreNotSame(header1, allScoreData.Header);
            Assert.AreSame(header2, allScoreData.Header);
        });

        [TestMethod]
        public void SetHighScoreTest() => TestUtils.Wrap(() =>
        {
            var score = new HighScoreStub(HighScoreTests.ValidStub)
            {
                Score = 876543u,
            };

            var allScoreData = new AllScoreData();
            allScoreData.Set(score);

            Assert.AreSame(score, allScoreData.Rankings[(score.Chara, score.Level)][2]);
        });

        [TestMethod]
        public void SetHighScoreTestTwice() => TestUtils.Wrap(() =>
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
        });

        [TestMethod]
        public void SetClearDataTest() => TestUtils.Wrap(() =>
        {
            var clearData = new ClearDataStub(ClearDataTests.ValidStub);

            var allScoreData = new AllScoreData();
            allScoreData.Set(clearData);

            Assert.AreSame(clearData, allScoreData.ClearData[clearData.Chara]);
        });

        [TestMethod]
        public void SetClearDataTestTwice() => TestUtils.Wrap(() =>
        {
            var clearData1 = new ClearDataStub(ClearDataTests.ValidStub);
            var clearData2 = new ClearDataStub(clearData1);

            var allScoreData = new AllScoreData();
            allScoreData.Set(clearData1);
            allScoreData.Set(clearData2);

            Assert.AreSame(clearData1, allScoreData.ClearData[clearData1.Chara]);
            Assert.AreNotSame(clearData2, allScoreData.ClearData[clearData2.Chara]);
        });

        [TestMethod]
        public void SetCardAttackTest() => TestUtils.Wrap(() =>
        {
            var attack = new CardAttackStub(CardAttackTests.ValidStub);

            var allScoreData = new AllScoreData();
            allScoreData.Set(attack);

            Assert.AreSame(attack, allScoreData.CardAttacks[attack.CardId]);
        });

        [TestMethod]
        public void SetCardAttackTestTwice() => TestUtils.Wrap(() =>
        {
            var attack1 = new CardAttackStub(CardAttackTests.ValidStub);
            var attack2 = new CardAttackStub(attack1);

            var allScoreData = new AllScoreData();
            allScoreData.Set(attack1);
            allScoreData.Set(attack2);

            Assert.AreSame(attack1, allScoreData.CardAttacks[attack1.CardId]);
            Assert.AreNotSame(attack2, allScoreData.CardAttacks[attack2.CardId]);
        });

        [TestMethod]
        public void SetPracticeScoreTest() => TestUtils.Wrap(() =>
        {
            var score = new PracticeScoreStub(PracticeScoreTests.ValidStub)
            {
                Level = Level.Normal,
                Stage = Stage.Six,
            };

            var allScoreData = new AllScoreData();
            allScoreData.Set(score);

            Assert.AreSame(score, allScoreData.PracticeScores[(score.Chara, score.Level, score.Stage)]);
        });

        [TestMethod]
        public void SetPracticeScoreTestTwice() => TestUtils.Wrap(() =>
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
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(Level.Easy, Stage.Six)]
        [DataRow(Level.Extra, Stage.Extra)]
        [DataRow(Level.Extra, Stage.Six)]
        [DataRow(Level.Normal, Stage.Extra)]
        public void SetPracticeScoreTestInvalidPracticeStage(int level, int stage) => TestUtils.Wrap(() =>
        {
            var score = new PracticeScoreStub(PracticeScoreTests.ValidStub)
            {
                Level = TestUtils.Cast<Level>(level),
                Stage = TestUtils.Cast<Stage>(stage),
            };

            var allScoreData = new AllScoreData();
            allScoreData.Set(score);

            Assert.AreEqual(0, allScoreData.PracticeScores.Count);
        });
    }
}

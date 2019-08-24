using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th06;
using ThScoreFileConverterTests.Models.Th06;
using ThScoreFileConverterTests.Models.Th06.Wrappers;
using ThScoreFileConverterTests.Models.Wrappers;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th06AllScoreDataTests
    {
        [TestMethod]
        public void Th06AllScoreDataTest() => TestUtils.Wrap(() =>
        {
            var allScoreData = new Th06AllScoreDataWrapper();

            Assert.IsNull(allScoreData.Header);
            Assert.AreEqual(0, allScoreData.Rankings.Count);
            Assert.AreEqual(0, allScoreData.ClearData.Count);
            Assert.AreEqual(0, allScoreData.CardAttacks.Count);
            Assert.AreEqual(0, allScoreData.PracticeScores.Count);
        });

        [TestMethod]
        public void Th06AllScoreDataSetHeaderTest() => TestUtils.Wrap(() =>
        {
            var chapter = ChapterWrapper.Create(
                HeaderTests.MakeByteArray(HeaderTests.ValidProperties));
            var header = new Header(chapter.Target as Chapter);

            var allScoreData = new Th06AllScoreDataWrapper();
            allScoreData.Set(header);

            Assert.AreSame(header, allScoreData.Header);
        });

        [TestMethod]
        public void Th06AllScoreDataSetHeaderTestTwice() => TestUtils.Wrap(() =>
        {
            var chapter = ChapterWrapper.Create(
                HeaderTests.MakeByteArray(HeaderTests.ValidProperties));
            var header1 = new Header(chapter.Target as Chapter);
            var header2 = new Header(chapter.Target as Chapter);

            var allScoreData = new Th06AllScoreDataWrapper();
            allScoreData.Set(header1);
            allScoreData.Set(header2);

            Assert.AreNotSame(header1, allScoreData.Header);
            Assert.AreSame(header2, allScoreData.Header);
        });

        [TestMethod]
        public void Th06AllScoreDataSetHighScoreTest() => TestUtils.Wrap(() =>
        {
            var properties = HighScoreTests.ValidProperties;
            properties.score = 876543u;
            var chapter = ChapterWrapper.Create(HighScoreTests.MakeByteArray(properties));
            var score = new HighScore(chapter.Target as Chapter);

            var allScoreData = new Th06AllScoreDataWrapper();
            allScoreData.Set(score);

            Assert.AreSame(score, allScoreData.Rankings[(properties.chara, properties.level)][2]);
        });

        [TestMethod]
        public void Th06AllScoreDataSetHighScoreTestTwice() => TestUtils.Wrap(() =>
        {
            var properties = HighScoreTests.ValidProperties;
            properties.score = 876543u;
            var chapter = ChapterWrapper.Create(HighScoreTests.MakeByteArray(properties));
            var score1 = new HighScore(chapter.Target as Chapter);
            var score2 = new HighScore(chapter.Target as Chapter);

            var allScoreData = new Th06AllScoreDataWrapper();
            allScoreData.Set(score1);
            allScoreData.Set(score2);

            Assert.AreSame(score1, allScoreData.Rankings[(properties.chara, properties.level)][2]);
            Assert.AreSame(score2, allScoreData.Rankings[(properties.chara, properties.level)][3]);
        });

        [TestMethod]
        public void Th06AllScoreDataSetClearDataTest() => TestUtils.Wrap(() =>
        {
            var properties = ClearDataTests.ValidProperties;
            var chapter = ChapterWrapper.Create(ClearDataTests.MakeByteArray(properties));
            var clearData = new ClearData(chapter.Target as Chapter);

            var allScoreData = new Th06AllScoreDataWrapper();
            allScoreData.Set(clearData);

            Assert.AreSame(clearData, allScoreData.ClearData[properties.chara]);
        });

        [TestMethod]
        public void Th06AllScoreDataSetClearDataTestTwice() => TestUtils.Wrap(() =>
        {
            var properties = ClearDataTests.ValidProperties;
            var chapter = ChapterWrapper.Create(ClearDataTests.MakeByteArray(properties));
            var clearData1 = new ClearData(chapter.Target as Chapter);
            var clearData2 = new ClearData(chapter.Target as Chapter);

            var allScoreData = new Th06AllScoreDataWrapper();
            allScoreData.Set(clearData1);
            allScoreData.Set(clearData2);

            Assert.AreSame(clearData1, allScoreData.ClearData[properties.chara]);
            Assert.AreNotSame(clearData2, allScoreData.ClearData[properties.chara]);
        });

        [TestMethod]
        public void Th06AllScoreDataSetCardAttackTest() => TestUtils.Wrap(() =>
        {
            var properties = CardAttackTests.ValidProperties;
            var chapter = ChapterWrapper.Create(CardAttackTests.MakeByteArray(properties));
            var attack = new CardAttack(chapter.Target as Chapter);

            var allScoreData = new Th06AllScoreDataWrapper();
            allScoreData.Set(attack);

            Assert.AreSame(attack, allScoreData.CardAttacks[properties.cardId]);
        });

        [TestMethod]
        public void Th06AllScoreDataSetCardAttackTestTwice() => TestUtils.Wrap(() =>
        {
            var properties = CardAttackTests.ValidProperties;
            var chapter = ChapterWrapper.Create(CardAttackTests.MakeByteArray(properties));
            var attack1 = new CardAttack(chapter.Target as Chapter);
            var attack2 = new CardAttack(chapter.Target as Chapter);

            var allScoreData = new Th06AllScoreDataWrapper();
            allScoreData.Set(attack1);
            allScoreData.Set(attack2);

            Assert.AreSame(attack1, allScoreData.CardAttacks[properties.cardId]);
            Assert.AreNotSame(attack2, allScoreData.CardAttacks[properties.cardId]);
        });

        [TestMethod]
        public void Th06AllScoreDataSetPracticeScoreTest() => TestUtils.Wrap(() =>
        {
            var properties = PracticeScoreTests.ValidProperties;
            properties.level = Level.Normal;
            properties.stage = Stage.St6;
            var chapter = ChapterWrapper.Create(PracticeScoreTests.MakeByteArray(properties));
            var score = new PracticeScore(chapter.Target as Chapter);

            var allScoreData = new Th06AllScoreDataWrapper();
            allScoreData.Set(score);

            Assert.AreSame(
                score,
                allScoreData.PracticeScores[(properties.chara, properties.level)][properties.stage]);
        });

        [TestMethod]
        public void Th06AllScoreDataSetPracticeScoreTestTwice() => TestUtils.Wrap(() =>
        {
            var properties = PracticeScoreTests.ValidProperties;
            properties.level = Level.Normal;
            properties.stage = Stage.St6;
            var chapter = ChapterWrapper.Create(PracticeScoreTests.MakeByteArray(properties));
            var score1 = new PracticeScore(chapter.Target as Chapter);
            var score2 = new PracticeScore(chapter.Target as Chapter);

            var allScoreData = new Th06AllScoreDataWrapper();
            allScoreData.Set(score1);
            allScoreData.Set(score2);

            Assert.AreSame(
                score1,
                allScoreData.PracticeScores[(properties.chara, properties.level)][properties.stage]);
            Assert.AreNotSame(
                score2,
                allScoreData.PracticeScores[(properties.chara, properties.level)][properties.stage]);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(Level.Easy, Stage.St6)]
        [DataRow(Level.Extra, Stage.Extra)]
        [DataRow(Level.Extra, Stage.St6)]
        [DataRow(Level.Normal, Stage.Extra)]
        public void Th06AllScoreDataSetPracticeScoreTestInvalidPracticeStage(int level, int stage)
            => TestUtils.Wrap(() =>
            {
                var properties = PracticeScoreTests.ValidProperties;
                properties.level = TestUtils.Cast<Level>(level);
                properties.stage = TestUtils.Cast<Stage>(stage);
                var chapter = ChapterWrapper.Create(
                    PracticeScoreTests.MakeByteArray(properties));
                var score = new PracticeScore(chapter.Target as Chapter);

                var allScoreData = new Th06AllScoreDataWrapper();
                allScoreData.Set(score);

                Assert.AreEqual(0, allScoreData.PracticeScores.Count);
            });
    }
}

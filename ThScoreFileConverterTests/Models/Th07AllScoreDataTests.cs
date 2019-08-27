using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;
using ThScoreFileConverter.Models.Th07;
using ThScoreFileConverterTests.Models.Th06.Wrappers;
using ThScoreFileConverterTests.Models.Th07;
using ThScoreFileConverterTests.Models.Wrappers;
using Chapter = ThScoreFileConverter.Models.Th06.Chapter;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th07AllScoreDataTests
    {
        [TestMethod]
        public void Th07AllScoreDataTest() => TestUtils.Wrap(() =>
        {
            var allScoreData = new Th07AllScoreDataWrapper();

            Assert.IsNull(allScoreData.Header);
            Assert.AreEqual(0, allScoreData.Rankings.Count);
            Assert.AreEqual(0, allScoreData.ClearData.Count);
            Assert.AreEqual(0, allScoreData.CardAttacks.Count);
            Assert.AreEqual(0, allScoreData.PracticeScores.Count);
            Assert.IsNull(allScoreData.PlayStatus);
            Assert.IsNull(allScoreData.LastName);
            Assert.IsNull(allScoreData.VersionInfo);
        });

        [TestMethod]
        public void Th07AllScoreDataSetHeaderTest() => TestUtils.Wrap(() =>
        {
            var chapter = ChapterWrapper.Create(
                HeaderTests.MakeByteArray(HeaderTests.ValidProperties));
            var header = new Header(chapter.Target as Chapter);

            var allScoreData = new Th07AllScoreDataWrapper();
            allScoreData.Set(header);

            Assert.AreSame(header, allScoreData.Header);
        });

        [TestMethod]
        public void Th07AllScoreDataSetHeaderTestTwice() => TestUtils.Wrap(() =>
        {
            var chapter = ChapterWrapper.Create(
                HeaderTests.MakeByteArray(HeaderTests.ValidProperties));
            var header1 = new Header(chapter.Target as Chapter);
            var header2 = new Header(chapter.Target as Chapter);

            var allScoreData = new Th07AllScoreDataWrapper();
            allScoreData.Set(header1);
            allScoreData.Set(header2);

            Assert.AreNotSame(header1, allScoreData.Header);
            Assert.AreSame(header2, allScoreData.Header);
        });

        [TestMethod]
        public void Th07AllScoreDataSetHighScoreTest() => TestUtils.Wrap(() =>
        {
            var properties = HighScoreTests.ValidProperties;
            properties.score = 87654u;
            var chapter = ChapterWrapper.Create(HighScoreTests.MakeByteArray(properties));
            var score = new HighScore(chapter.Target as Chapter);

            var allScoreData = new Th07AllScoreDataWrapper();
            allScoreData.Set(score);

            Assert.AreSame(score, allScoreData.Rankings[(properties.chara, properties.level)][2]);
        });

        [TestMethod]
        public void Th07AllScoreDataSetHighScoreTestTwice() => TestUtils.Wrap(() =>
        {
            var properties = HighScoreTests.ValidProperties;
            properties.score = 87654u;
            var chapter = ChapterWrapper.Create(HighScoreTests.MakeByteArray(properties));
            var score1 = new HighScore(chapter.Target as Chapter);
            var score2 = new HighScore(chapter.Target as Chapter);

            var allScoreData = new Th07AllScoreDataWrapper();
            allScoreData.Set(score1);
            allScoreData.Set(score2);

            Assert.AreSame(score1, allScoreData.Rankings[(properties.chara, properties.level)][2]);
            Assert.AreSame(score2, allScoreData.Rankings[(properties.chara, properties.level)][3]);
        });

        [TestMethod]
        public void Th07AllScoreDataSetClearDataTest() => TestUtils.Wrap(() =>
        {
            var properties = ClearDataTests.ValidProperties;
            var chapter = ChapterWrapper.Create(ClearDataTests.MakeByteArray(properties));
            var clearData = new ClearData(chapter.Target as Chapter);

            var allScoreData = new Th07AllScoreDataWrapper();
            allScoreData.Set(clearData);

            Assert.AreSame(clearData, allScoreData.ClearData[properties.chara]);
        });

        [TestMethod]
        public void Th07AllScoreDataSetClearDataTestTwice() => TestUtils.Wrap(() =>
        {
            var properties = ClearDataTests.ValidProperties;
            var chapter = ChapterWrapper.Create(ClearDataTests.MakeByteArray(properties));
            var clearData1 = new ClearData(chapter.Target as Chapter);
            var clearData2 = new ClearData(chapter.Target as Chapter);

            var allScoreData = new Th07AllScoreDataWrapper();
            allScoreData.Set(clearData1);
            allScoreData.Set(clearData2);

            Assert.AreSame(clearData1, allScoreData.ClearData[properties.chara]);
            Assert.AreNotSame(clearData2, allScoreData.ClearData[properties.chara]);
        });

        [TestMethod]
        public void Th07AllScoreDataSetCardAttackTest() => TestUtils.Wrap(() =>
        {
            var properties = CardAttackTests.ValidProperties;
            var chapter = ChapterWrapper.Create(CardAttackTests.MakeByteArray(properties));
            var attack = new CardAttack(chapter.Target as Chapter);

            var allScoreData = new Th07AllScoreDataWrapper();
            allScoreData.Set(attack);

            Assert.AreSame(attack, allScoreData.CardAttacks[properties.cardId]);
        });

        [TestMethod]
        public void Th07AllScoreDataSetCardAttackTestTwice() => TestUtils.Wrap(() =>
        {
            var properties = CardAttackTests.ValidProperties;
            var chapter = ChapterWrapper.Create(CardAttackTests.MakeByteArray(properties));
            var attack1 = new CardAttack(chapter.Target as Chapter);
            var attack2 = new CardAttack(chapter.Target as Chapter);

            var allScoreData = new Th07AllScoreDataWrapper();
            allScoreData.Set(attack1);
            allScoreData.Set(attack2);

            Assert.AreSame(attack1, allScoreData.CardAttacks[properties.cardId]);
            Assert.AreNotSame(attack2, allScoreData.CardAttacks[properties.cardId]);
        });

        [TestMethod]
        public void Th07AllScoreDataSetPracticeScoreTest() => TestUtils.Wrap(() =>
        {
            var properties = PracticeScoreTests.ValidProperties;
            properties.level = Level.Normal;
            properties.stage = Stage.St6;
            var chapter = ChapterWrapper.Create(PracticeScoreTests.MakeByteArray(properties));
            var score = new PracticeScore(chapter.Target as Chapter);

            var allScoreData = new Th07AllScoreDataWrapper();
            allScoreData.Set(score);

            Assert.AreSame(
                score,
                allScoreData.PracticeScores[(properties.chara, properties.level, properties.stage)]);
        });

        [TestMethod]
        public void Th07AllScoreDataSetPracticeScoreTestTwice() => TestUtils.Wrap(() =>
        {
            var properties = PracticeScoreTests.ValidProperties;
            properties.level = Level.Normal;
            properties.stage = Stage.St6;
            var chapter = ChapterWrapper.Create(PracticeScoreTests.MakeByteArray(properties));
            var score1 = new PracticeScore(chapter.Target as Chapter);
            var score2 = new PracticeScore(chapter.Target as Chapter);

            var allScoreData = new Th07AllScoreDataWrapper();
            allScoreData.Set(score1);
            allScoreData.Set(score2);

            Assert.AreSame(
                score1,
                allScoreData.PracticeScores[(properties.chara, properties.level, properties.stage)]);
            Assert.AreNotSame(
                score2,
                allScoreData.PracticeScores[(properties.chara, properties.level, properties.stage)]);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(Level.Extra, Stage.Extra)]
        [DataRow(Level.Extra, Stage.St6)]
        [DataRow(Level.Normal, Stage.Extra)]
        [DataRow(Level.Phantasm, Stage.Phantasm)]
        [DataRow(Level.Phantasm, Stage.St6)]
        [DataRow(Level.Normal, Stage.Phantasm)]
        public void Th07AllScoreDataSetPracticeScoreTestInvalidPracticeStage(int level, int stage)
            => TestUtils.Wrap(() =>
            {
                var properties = PracticeScoreTests.ValidProperties;
                properties.level = TestUtils.Cast<Level>(level);
                properties.stage = TestUtils.Cast<Stage>(stage);
                var chapter = ChapterWrapper.Create(
                    PracticeScoreTests.MakeByteArray(properties));
                var score = new PracticeScore(chapter.Target as Chapter);

                var allScoreData = new Th07AllScoreDataWrapper();
                allScoreData.Set(score);

                Assert.AreEqual(0, allScoreData.PracticeScores.Count);
            });

        [TestMethod]
        public void Th07AllScoreDataSetPlayStatusTest() => TestUtils.Wrap(() =>
        {
            var chapter = ChapterWrapper.Create(
                PlayStatusTests.MakeByteArray(PlayStatusTests.ValidProperties));
            var status = new PlayStatus(chapter.Target as Chapter);

            var allScoreData = new Th07AllScoreDataWrapper();
            allScoreData.Set(status);

            Assert.AreSame(status, allScoreData.PlayStatus);
        });

        [TestMethod]
        public void Th07AllScoreDataSetPlayStatusTestTwice() => TestUtils.Wrap(() =>
        {
            var chapter = ChapterWrapper.Create(
                PlayStatusTests.MakeByteArray(PlayStatusTests.ValidProperties));
            var status1 = new PlayStatus(chapter.Target as Chapter);
            var status2 = new PlayStatus(chapter.Target as Chapter);

            var allScoreData = new Th07AllScoreDataWrapper();
            allScoreData.Set(status1);
            allScoreData.Set(status2);

            Assert.AreNotSame(status1, allScoreData.PlayStatus);
            Assert.AreSame(status2, allScoreData.PlayStatus);
        });

        [TestMethod]
        public void Th07AllScoreDataSetLastNameTest() => TestUtils.Wrap(() =>
        {
            var chapter = ChapterWrapper.Create(
                LastNameTests.MakeByteArray(LastNameTests.ValidProperties));
            var name = new LastName(chapter.Target as Chapter);

            var allScoreData = new Th07AllScoreDataWrapper();
            allScoreData.Set(name);

            Assert.AreSame(name, allScoreData.LastName);
        });

        [TestMethod]
        public void Th07AllScoreDataSetLastNameTestTwice() => TestUtils.Wrap(() =>
        {
            var chapter = ChapterWrapper.Create(
                LastNameTests.MakeByteArray(LastNameTests.ValidProperties));
            var name1 = new LastName(chapter.Target as Chapter);
            var name2 = new LastName(chapter.Target as Chapter);

            var allScoreData = new Th07AllScoreDataWrapper();
            allScoreData.Set(name1);
            allScoreData.Set(name2);

            Assert.AreNotSame(name1, allScoreData.LastName);
            Assert.AreSame(name2, allScoreData.LastName);
        });

        [TestMethod]
        public void Th07AllScoreDataSetVersionInfoTest() => TestUtils.Wrap(() =>
        {
            var chapter = ChapterWrapper.Create(
                VersionInfoTests.MakeByteArray(VersionInfoTests.ValidProperties));
            var info = new VersionInfo(chapter.Target as Chapter);

            var allScoreData = new Th07AllScoreDataWrapper();
            allScoreData.Set(info);

            Assert.AreSame(info, allScoreData.VersionInfo);
        });

        [TestMethod]
        public void Th07AllScoreDataSetVersionInfoTestTwice() => TestUtils.Wrap(() =>
        {
            var chapter = ChapterWrapper.Create(
                VersionInfoTests.MakeByteArray(VersionInfoTests.ValidProperties));
            var info1 = new VersionInfo(chapter.Target as Chapter);
            var info2 = new VersionInfo(chapter.Target as Chapter);

            var allScoreData = new Th07AllScoreDataWrapper();
            allScoreData.Set(info1);
            allScoreData.Set(info2);

            Assert.AreNotSame(info1, allScoreData.VersionInfo);
            Assert.AreSame(info2, allScoreData.VersionInfo);
        });
    }
}

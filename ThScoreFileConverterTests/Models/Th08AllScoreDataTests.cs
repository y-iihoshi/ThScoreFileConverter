using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th08;
using ThScoreFileConverterTests.Models.Th06.Wrappers;
using ThScoreFileConverterTests.Models.Th08;
using ThScoreFileConverterTests.Models.Wrappers;
using LastName = ThScoreFileConverter.Models.Th07.LastName;
using VersionInfo = ThScoreFileConverter.Models.Th07.VersionInfo;
using LastNameTests = ThScoreFileConverterTests.Models.Th07.LastNameTests;
using VersionInfoTests = ThScoreFileConverterTests.Models.Th07.VersionInfoTests;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th08AllScoreDataTests
    {
        [TestMethod]
        public void Th08AllScoreDataTest() => TestUtils.Wrap(() =>
        {
            var allScoreData = new Th08AllScoreDataWrapper();

            Assert.IsNull(allScoreData.Header);
            Assert.AreEqual(0, allScoreData.RankingsCount);
            Assert.AreEqual(0, allScoreData.ClearDataCount);
            Assert.AreEqual(0, allScoreData.CardAttacksCount);
            Assert.AreEqual(0, allScoreData.PracticeScoresCount);
            Assert.IsNull(allScoreData.Flsp);
            Assert.IsNull(allScoreData.PlayStatus);
            Assert.IsNull(allScoreData.LastName);
            Assert.IsNull(allScoreData.VersionInfo);
        });

        [TestMethod]
        public void Th08AllScoreDataSetHeaderTest() => TestUtils.Wrap(() =>
        {
            var chapter = ChapterWrapper.Create(
                HeaderTests.MakeByteArray(HeaderTests.ValidProperties));
            var header = new Header(chapter.Target);

            var allScoreData = new Th08AllScoreDataWrapper();
            allScoreData.Set(header);

            Assert.AreSame(header, allScoreData.Header);
        });

        [TestMethod]
        public void Th08AllScoreDataSetHeaderTestTwice() => TestUtils.Wrap(() =>
        {
            var chapter = ChapterWrapper.Create(
                HeaderTests.MakeByteArray(HeaderTests.ValidProperties));
            var header1 = new Header(chapter.Target);
            var header2 = new Header(chapter.Target);

            var allScoreData = new Th08AllScoreDataWrapper();
            allScoreData.Set(header1);
            allScoreData.Set(header2);

            Assert.AreNotSame(header1, allScoreData.Header);
            Assert.AreSame(header2, allScoreData.Header);
        });

        [TestMethod]
        public void Th08AllScoreDataSetHighScoreTest() => TestUtils.Wrap(() =>
        {
            var properties = Th08HighScoreTests.ValidProperties;
            properties.score = 87654u;
            var chapter = ChapterWrapper.Create(Th08HighScoreTests.MakeByteArray(properties));
            var score = new Th08HighScoreWrapper(chapter);

            var allScoreData = new Th08AllScoreDataWrapper();
            allScoreData.Set(score);

            Assert.AreSame(score.Target, allScoreData.RankingItem(properties.chara, properties.level, 2).Target);
        });

        [TestMethod]
        public void Th08AllScoreDataSetHighScoreTestTwice() => TestUtils.Wrap(() =>
        {
            var properties = Th08HighScoreTests.ValidProperties;
            properties.score = 87654u;
            var chapter = ChapterWrapper.Create(Th08HighScoreTests.MakeByteArray(properties));
            var score1 = new Th08HighScoreWrapper(chapter);
            var score2 = new Th08HighScoreWrapper(chapter);

            var allScoreData = new Th08AllScoreDataWrapper();
            allScoreData.Set(score1);
            allScoreData.Set(score2);

            Assert.AreSame(score1.Target, allScoreData.RankingItem(properties.chara, properties.level, 2).Target);
            Assert.AreSame(score2.Target, allScoreData.RankingItem(properties.chara, properties.level, 3).Target);
        });

        [TestMethod]
        public void Th08AllScoreDataSetClearDataTest() => TestUtils.Wrap(() =>
        {
            var properties = Th08ClearDataTests.ValidProperties;
            var chapter = ChapterWrapper.Create(Th08ClearDataTests.MakeByteArray(properties));
            var clearData = new Th08ClearDataWrapper(chapter);

            var allScoreData = new Th08AllScoreDataWrapper();
            allScoreData.Set(clearData);

            Assert.AreSame(clearData.Target, allScoreData.ClearDataItem(properties.chara).Target);
        });

        [TestMethod]
        public void Th08AllScoreDataSetClearDataTestTwice() => TestUtils.Wrap(() =>
        {
            var properties = Th08ClearDataTests.ValidProperties;
            var chapter = ChapterWrapper.Create(Th08ClearDataTests.MakeByteArray(properties));
            var clearData1 = new Th08ClearDataWrapper(chapter);
            var clearData2 = new Th08ClearDataWrapper(chapter);

            var allScoreData = new Th08AllScoreDataWrapper();
            allScoreData.Set(clearData1);
            allScoreData.Set(clearData2);

            Assert.AreSame(clearData1.Target, allScoreData.ClearDataItem(properties.chara).Target);
            Assert.AreNotSame(clearData2.Target, allScoreData.ClearDataItem(properties.chara).Target);
        });

        [TestMethod]
        public void Th08AllScoreDataSetCardAttackTest() => TestUtils.Wrap(() =>
        {
            var properties = Th08CardAttackTests.ValidProperties;
            var chapter = ChapterWrapper.Create(Th08CardAttackTests.MakeByteArray(properties));
            var attack = new Th08CardAttackWrapper(chapter);

            var allScoreData = new Th08AllScoreDataWrapper();
            allScoreData.Set(attack);

            Assert.AreSame(attack.Target, allScoreData.CardAttacksItem(properties.cardId).Target);
        });

        [TestMethod]
        public void Th08AllScoreDataSetCardAttackTestTwice() => TestUtils.Wrap(() =>
        {
            var properties = Th08CardAttackTests.ValidProperties;
            var chapter = ChapterWrapper.Create(Th08CardAttackTests.MakeByteArray(properties));
            var attack1 = new Th08CardAttackWrapper(chapter);
            var attack2 = new Th08CardAttackWrapper(chapter);

            var allScoreData = new Th08AllScoreDataWrapper();
            allScoreData.Set(attack1);
            allScoreData.Set(attack2);

            Assert.AreSame(attack1.Target, allScoreData.CardAttacksItem(properties.cardId).Target);
            Assert.AreNotSame(attack2.Target, allScoreData.CardAttacksItem(properties.cardId).Target);
        });

        [TestMethod]
        public void Th08AllScoreDataSetPracticeScoreTest() => TestUtils.Wrap(() =>
        {
            var properties = Th08PracticeScoreTests.ValidProperties;
            var chapter = ChapterWrapper.Create(Th08PracticeScoreTests.MakeByteArray(properties));
            var score = new Th08PracticeScoreWrapper(chapter);

            var allScoreData = new Th08AllScoreDataWrapper();
            allScoreData.Set(score);

            Assert.AreSame(score.Target, allScoreData.PracticeScoresItem(properties.chara).Target);
        });

        [TestMethod]
        public void Th08AllScoreDataSetPracticeScoreTestTwice() => TestUtils.Wrap(() =>
        {
            var properties = Th08PracticeScoreTests.ValidProperties;
            var chapter = ChapterWrapper.Create(Th08PracticeScoreTests.MakeByteArray(properties));
            var score1 = new Th08PracticeScoreWrapper(chapter);
            var score2 = new Th08PracticeScoreWrapper(chapter);

            var allScoreData = new Th08AllScoreDataWrapper();
            allScoreData.Set(score1);
            allScoreData.Set(score2);

            Assert.AreSame(score1.Target, allScoreData.PracticeScoresItem(properties.chara).Target);
            Assert.AreNotSame(score2.Target, allScoreData.PracticeScoresItem(properties.chara).Target);
        });

        [TestMethod]
        public void Th08AllScoreDataSetFlspTest() => TestUtils.Wrap(() =>
        {
            var chapter = ChapterWrapper.Create(
                Th08FlspTests.MakeByteArray(Th08FlspTests.ValidProperties));
            var header = new Th08FlspWrapper(chapter);

            var allScoreData = new Th08AllScoreDataWrapper();
            allScoreData.Set(header);

            Assert.AreSame(header.Target, allScoreData.Flsp.Target);
        });

        [TestMethod]
        public void Th08AllScoreDataSetFlspTestTwice() => TestUtils.Wrap(() =>
        {
            var chapter = ChapterWrapper.Create(
                Th08FlspTests.MakeByteArray(Th08FlspTests.ValidProperties));
            var header1 = new Th08FlspWrapper(chapter);
            var header2 = new Th08FlspWrapper(chapter);

            var allScoreData = new Th08AllScoreDataWrapper();
            allScoreData.Set(header1);
            allScoreData.Set(header2);

            Assert.AreNotSame(header1.Target, allScoreData.Flsp.Target);
            Assert.AreSame(header2.Target, allScoreData.Flsp.Target);
        });

        [TestMethod]
        public void Th08AllScoreDataSetPlayStatusTest() => TestUtils.Wrap(() =>
        {
            var chapter = ChapterWrapper.Create(
                Th08PlayStatusTests.MakeByteArray(Th08PlayStatusTests.ValidProperties));
            var header = new Th08PlayStatusWrapper(chapter);

            var allScoreData = new Th08AllScoreDataWrapper();
            allScoreData.Set(header);

            Assert.AreSame(header.Target, allScoreData.PlayStatus.Target);
        });

        [TestMethod]
        public void Th08AllScoreDataSetPlayStatusTestTwice() => TestUtils.Wrap(() =>
        {
            var chapter = ChapterWrapper.Create(
                Th08PlayStatusTests.MakeByteArray(Th08PlayStatusTests.ValidProperties));
            var header1 = new Th08PlayStatusWrapper(chapter);
            var header2 = new Th08PlayStatusWrapper(chapter);

            var allScoreData = new Th08AllScoreDataWrapper();
            allScoreData.Set(header1);
            allScoreData.Set(header2);

            Assert.AreNotSame(header1.Target, allScoreData.PlayStatus.Target);
            Assert.AreSame(header2.Target, allScoreData.PlayStatus.Target);
        });

        [TestMethod]
        public void Th08AllScoreDataSetLastNameTest() => TestUtils.Wrap(() =>
        {
            var chapter = ChapterWrapper.Create(
                LastNameTests.MakeByteArray(LastNameTests.ValidProperties));
            var name = new LastName(chapter.Target);

            var allScoreData = new Th08AllScoreDataWrapper();
            allScoreData.Set(name);

            Assert.AreSame(name, allScoreData.LastName);
        });

        [TestMethod]
        public void Th08AllScoreDataSetLastNameTestTwice() => TestUtils.Wrap(() =>
        {
            var chapter = ChapterWrapper.Create(
                LastNameTests.MakeByteArray(LastNameTests.ValidProperties));
            var name1 = new LastName(chapter.Target);
            var name2 = new LastName(chapter.Target);

            var allScoreData = new Th08AllScoreDataWrapper();
            allScoreData.Set(name1);
            allScoreData.Set(name2);

            Assert.AreNotSame(name1, allScoreData.LastName);
            Assert.AreSame(name2, allScoreData.LastName);
        });

        [TestMethod]
        public void Th08AllScoreDataSetVersionInfoTest() => TestUtils.Wrap(() =>
        {
            var chapter = ChapterWrapper.Create(
                VersionInfoTests.MakeByteArray(VersionInfoTests.ValidProperties));
            var info = new VersionInfo(chapter.Target);

            var allScoreData = new Th08AllScoreDataWrapper();
            allScoreData.Set(info);

            Assert.AreSame(info, allScoreData.VersionInfo);
        });

        [TestMethod]
        public void Th08AllScoreDataSetVersionInfoTestTwice() => TestUtils.Wrap(() =>
        {
            var chapter = ChapterWrapper.Create(
                VersionInfoTests.MakeByteArray(VersionInfoTests.ValidProperties));
            var info1 = new VersionInfo(chapter.Target);
            var info2 = new VersionInfo(chapter.Target);

            var allScoreData = new Th08AllScoreDataWrapper();
            allScoreData.Set(info1);
            allScoreData.Set(info2);

            Assert.AreNotSame(info1, allScoreData.VersionInfo);
            Assert.AreSame(info2, allScoreData.VersionInfo);
        });
    }
}

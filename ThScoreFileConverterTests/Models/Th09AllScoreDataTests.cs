using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;
using ThScoreFileConverter.Models;
using ThScoreFileConverterTests.Models.Th06.Wrappers;
using ThScoreFileConverterTests.Models.Th07;
using ThScoreFileConverterTests.Models.Th07.Wrappers;
using ThScoreFileConverterTests.Models.Wrappers;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th09AllScoreDataTests
    {
        [TestMethod]
        public void Th09AllScoreDataTest() => TestUtils.Wrap(() =>
        {
            var allScoreData = new Th09AllScoreDataWrapper();

            Assert.IsNull(allScoreData.Header);
            Assert.AreEqual(0, allScoreData.RankingsCount);
            Assert.IsNull(allScoreData.PlayStatus);
            Assert.IsNull(allScoreData.LastName);
            Assert.IsNull(allScoreData.VersionInfo);
        });

        [TestMethod]
        public void Th09AllScoreDataSetHeaderTest() => TestUtils.Wrap(() =>
        {
            var chapter = ChapterWrapper.Create(
                Th06HeaderTests.MakeByteArray(Th06HeaderTests.GetValidProperties("TH9K")));
            var header = new Th06HeaderWrapper<Th09Converter>(chapter);

            var allScoreData = new Th09AllScoreDataWrapper();
            allScoreData.Set(header);

            Assert.AreSame(header.Target, allScoreData.Header.Target);
        });

        [TestMethod]
        public void Th09AllScoreDataSetHeaderTestTwice() => TestUtils.Wrap(() =>
        {
            var chapter = ChapterWrapper.Create(
                Th06HeaderTests.MakeByteArray(Th06HeaderTests.GetValidProperties("TH9K")));
            var header1 = new Th06HeaderWrapper<Th09Converter>(chapter);
            var header2 = new Th06HeaderWrapper<Th09Converter>(chapter);

            var allScoreData = new Th09AllScoreDataWrapper();
            allScoreData.Set(header1);
            allScoreData.Set(header2);

            Assert.AreNotSame(header1.Target, allScoreData.Header.Target);
            Assert.AreSame(header2.Target, allScoreData.Header.Target);
        });

        [TestMethod]
        public void Th09AllScoreDataSetHighScoreTest() => TestUtils.Wrap(() =>
        {
            var properties = Th09HighScoreTests.ValidProperties;
            properties.score = 87654u;
            properties.rank = 2;
            var chapter = ChapterWrapper.Create(Th09HighScoreTests.MakeByteArray(properties));
            var score = new Th09HighScoreWrapper(chapter);

            var allScoreData = new Th09AllScoreDataWrapper();
            allScoreData.Set(score);

            Assert.AreSame(
                score.Target,
                allScoreData.RankingItem(properties.chara, properties.level, properties.rank).Target);
        });

        [TestMethod]
        public void Th09AllScoreDataSetHighScoreTestTwice() => TestUtils.Wrap(() =>
        {
            var properties = Th09HighScoreTests.ValidProperties;
            properties.score = 87654u;
            properties.rank = 2;
            var chapter = ChapterWrapper.Create(Th09HighScoreTests.MakeByteArray(properties));
            var score1 = new Th09HighScoreWrapper(chapter);
            var score2 = new Th09HighScoreWrapper(chapter);

            var allScoreData = new Th09AllScoreDataWrapper();
            allScoreData.Set(score1);
            allScoreData.Set(score2);

            Assert.AreNotSame(
                score1.Target,
                allScoreData.RankingItem(properties.chara, properties.level, properties.rank).Target);
            Assert.AreSame(
                score2.Target,
                allScoreData.RankingItem(properties.chara, properties.level, properties.rank).Target);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow((short)-1)]
        [DataRow((short)5)]
        public void Th09AllScoreDataSetHighScoreTestInvalidRank(short rank) => TestUtils.Wrap(() =>
        {
            var properties = Th09HighScoreTests.ValidProperties;
            properties.score = 87654u;
            properties.rank = rank;
            var chapter = ChapterWrapper.Create(Th09HighScoreTests.MakeByteArray(properties));
            var score = new Th09HighScoreWrapper(chapter);

            var allScoreData = new Th09AllScoreDataWrapper();
            allScoreData.Set(score);

            for (var index = 0; index < allScoreData.Ranking(properties.chara, properties.level).Length; ++index)
            {
                Assert.IsNull(allScoreData.RankingItem(properties.chara, properties.level, index));
            }
        });

        [TestMethod]
        public void Th09AllScoreDataSetPlayStatusTest() => TestUtils.Wrap(() =>
        {
            var chapter = ChapterWrapper.Create(
                Th09PlayStatusTests.MakeByteArray(Th09PlayStatusTests.ValidProperties));
            var header = new Th09PlayStatusWrapper(chapter);

            var allScoreData = new Th09AllScoreDataWrapper();
            allScoreData.Set(header);

            Assert.AreSame(header.Target, allScoreData.PlayStatus.Target);
        });

        [TestMethod]
        public void Th09AllScoreDataSetPlayStatusTestTwice() => TestUtils.Wrap(() =>
        {
            var chapter = ChapterWrapper.Create(
                Th09PlayStatusTests.MakeByteArray(Th09PlayStatusTests.ValidProperties));
            var header1 = new Th09PlayStatusWrapper(chapter);
            var header2 = new Th09PlayStatusWrapper(chapter);

            var allScoreData = new Th09AllScoreDataWrapper();
            allScoreData.Set(header1);
            allScoreData.Set(header2);

            Assert.AreNotSame(header1.Target, allScoreData.PlayStatus.Target);
            Assert.AreSame(header2.Target, allScoreData.PlayStatus.Target);
        });

        [TestMethod]
        public void Th09AllScoreDataSetLastNameTest() => TestUtils.Wrap(() =>
        {
            var chapter = ChapterWrapper.Create(
                LastNameTests.MakeByteArray(LastNameTests.ValidProperties));
            var header = new LastNameWrapper(chapter);

            var allScoreData = new Th09AllScoreDataWrapper();
            allScoreData.Set(header);

            Assert.AreSame(header.Target, allScoreData.LastName.Target);
        });

        [TestMethod]
        public void Th09AllScoreDataSetLastNameTestTwice() => TestUtils.Wrap(() =>
        {
            var chapter = ChapterWrapper.Create(
                LastNameTests.MakeByteArray(LastNameTests.ValidProperties));
            var header1 = new LastNameWrapper(chapter);
            var header2 = new LastNameWrapper(chapter);

            var allScoreData = new Th09AllScoreDataWrapper();
            allScoreData.Set(header1);
            allScoreData.Set(header2);

            Assert.AreNotSame(header1.Target, allScoreData.LastName.Target);
            Assert.AreSame(header2.Target, allScoreData.LastName.Target);
        });

        [TestMethod]
        public void Th09AllScoreDataSetVersionInfoTest() => TestUtils.Wrap(() =>
        {
            var chapter = ChapterWrapper.Create(
                Th07VersionInfoTests.MakeByteArray(Th07VersionInfoTests.ValidProperties));
            var header = new Th07VersionInfoWrapper(chapter);

            var allScoreData = new Th09AllScoreDataWrapper();
            allScoreData.Set(header);

            Assert.AreSame(header.Target, allScoreData.VersionInfo.Target);
        });

        [TestMethod]
        public void Th09AllScoreDataSetVersionInfoTestTwice() => TestUtils.Wrap(() =>
        {
            var chapter = ChapterWrapper.Create(
                Th07VersionInfoTests.MakeByteArray(Th07VersionInfoTests.ValidProperties));
            var header1 = new Th07VersionInfoWrapper(chapter);
            var header2 = new Th07VersionInfoWrapper(chapter);

            var allScoreData = new Th09AllScoreDataWrapper();
            allScoreData.Set(header1);
            allScoreData.Set(header2);

            Assert.AreNotSame(header1.Target, allScoreData.VersionInfo.Target);
            Assert.AreSame(header2.Target, allScoreData.VersionInfo.Target);
        });
    }
}

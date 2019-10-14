using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;
using ThScoreFileConverter.Models.Th09;
using ThScoreFileConverterTests.Models.Th06.Wrappers;
using ThScoreFileConverterTests.Models.Th09;
using ThScoreFileConverterTests.Models.Th09.Stubs;
using ThScoreFileConverterTests.Models.Wrappers;
using Chapter = ThScoreFileConverter.Models.Th06.Chapter;
using LastName = ThScoreFileConverter.Models.Th07.LastName;
using LastNameTests = ThScoreFileConverterTests.Models.Th07.LastNameTests;
using VersionInfo = ThScoreFileConverter.Models.Th07.VersionInfo;
using VersionInfoTests = ThScoreFileConverterTests.Models.Th07.VersionInfoTests;

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
            var chapter = TestUtils.Create<Chapter>(HeaderTests.MakeByteArray(HeaderTests.ValidProperties));
            var header = new Header(chapter);

            var allScoreData = new Th09AllScoreDataWrapper();
            allScoreData.Set(header);

            Assert.AreSame(header, allScoreData.Header);
        });

        [TestMethod]
        public void Th09AllScoreDataSetHeaderTestTwice() => TestUtils.Wrap(() =>
        {
            var chapter = TestUtils.Create<Chapter>(HeaderTests.MakeByteArray(HeaderTests.ValidProperties));
            var header1 = new Header(chapter);
            var header2 = new Header(chapter);

            var allScoreData = new Th09AllScoreDataWrapper();
            allScoreData.Set(header1);
            allScoreData.Set(header2);

            Assert.AreNotSame(header1, allScoreData.Header);
            Assert.AreSame(header2, allScoreData.Header);
        });

        [TestMethod]
        public void Th09AllScoreDataSetHighScoreTest() => TestUtils.Wrap(() =>
        {
            var stub = new HighScoreStub(Th09HighScoreTests.ValidStub)
            {
                Score = 87654u,
                Rank = 2,
            };

            var chapter = ChapterWrapper.Create(Th09HighScoreTests.MakeByteArray(stub));
            var score = new Th09HighScoreWrapper(chapter);

            var allScoreData = new Th09AllScoreDataWrapper();
            allScoreData.Set(score);

            Assert.AreSame(score.Target, allScoreData.RankingItem(stub.Chara, stub.Level, stub.Rank).Target);
        });

        [TestMethod]
        public void Th09AllScoreDataSetHighScoreTestTwice() => TestUtils.Wrap(() =>
        {
            var stub = new HighScoreStub(Th09HighScoreTests.ValidStub)
            {
                Score = 87654u,
                Rank = 2,
            };

            var chapter = ChapterWrapper.Create(Th09HighScoreTests.MakeByteArray(stub));
            var score1 = new Th09HighScoreWrapper(chapter);
            var score2 = new Th09HighScoreWrapper(chapter);

            var allScoreData = new Th09AllScoreDataWrapper();
            allScoreData.Set(score1);
            allScoreData.Set(score2);

            Assert.AreNotSame(score1.Target, allScoreData.RankingItem(stub.Chara, stub.Level, stub.Rank).Target);
            Assert.AreSame(score2.Target, allScoreData.RankingItem(stub.Chara, stub.Level, stub.Rank).Target);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow((short)-1)]
        [DataRow((short)5)]
        public void Th09AllScoreDataSetHighScoreTestInvalidRank(short rank) => TestUtils.Wrap(() =>
        {
            var stub = new HighScoreStub(Th09HighScoreTests.ValidStub)
            {
                Score = 87654u,
                Rank = rank,
            };

            var chapter = ChapterWrapper.Create(Th09HighScoreTests.MakeByteArray(stub));
            var score = new Th09HighScoreWrapper(chapter);

            var allScoreData = new Th09AllScoreDataWrapper();
            allScoreData.Set(score);

            for (var index = 0; index < allScoreData.Ranking(stub.Chara, stub.Level).Count; ++index)
            {
                Assert.IsNull(allScoreData.RankingItem(stub.Chara, stub.Level, index));
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
            var name = new LastName(chapter.Target);

            var allScoreData = new Th09AllScoreDataWrapper();
            allScoreData.Set(name);

            Assert.AreSame(name, allScoreData.LastName);
        });

        [TestMethod]
        public void Th09AllScoreDataSetLastNameTestTwice() => TestUtils.Wrap(() =>
        {
            var chapter = ChapterWrapper.Create(
                LastNameTests.MakeByteArray(LastNameTests.ValidProperties));
            var name1 = new LastName(chapter.Target);
            var name2 = new LastName(chapter.Target);

            var allScoreData = new Th09AllScoreDataWrapper();
            allScoreData.Set(name1);
            allScoreData.Set(name2);

            Assert.AreNotSame(name1, allScoreData.LastName);
            Assert.AreSame(name2, allScoreData.LastName);
        });

        [TestMethod]
        public void Th09AllScoreDataSetVersionInfoTest() => TestUtils.Wrap(() =>
        {
            var chapter = ChapterWrapper.Create(
                VersionInfoTests.MakeByteArray(VersionInfoTests.ValidProperties));
            var info = new VersionInfo(chapter.Target);

            var allScoreData = new Th09AllScoreDataWrapper();
            allScoreData.Set(info);

            Assert.AreSame(info, allScoreData.VersionInfo);
        });

        [TestMethod]
        public void Th09AllScoreDataSetVersionInfoTestTwice() => TestUtils.Wrap(() =>
        {
            var chapter = ChapterWrapper.Create(
                VersionInfoTests.MakeByteArray(VersionInfoTests.ValidProperties));
            var info1 = new VersionInfo(chapter.Target);
            var info2 = new VersionInfo(chapter.Target);

            var allScoreData = new Th09AllScoreDataWrapper();
            allScoreData.Set(info1);
            allScoreData.Set(info2);

            Assert.AreNotSame(info1, allScoreData.VersionInfo);
            Assert.AreSame(info2, allScoreData.VersionInfo);
        });
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th095;
using ThScoreFileConverter.Models.Th143;
using ThScoreFileConverterTests.Models.Th095;
using ThScoreFileConverterTests.Models.Th143.Stubs;
using ThScoreFileConverterTests.Models.Wrappers;
using ChapterWrapper = ThScoreFileConverterTests.Models.Th10.Wrappers.ChapterWrapper;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th143AllScoreDataTests
    {
        [TestMethod]
        public void Th143AllScoreDataTest() => TestUtils.Wrap(() =>
        {
            var allScoreData = new Th143AllScoreDataWrapper();

            Assert.IsNull(allScoreData.Header);
            Assert.AreEqual(0, allScoreData.Scores.Count);
            Assert.AreEqual(0, allScoreData.ItemStatuses.Count);
            Assert.IsNull(allScoreData.Status);
        });

        [TestMethod]
        public void Th143AllScoreDataSetHeaderTest() => TestUtils.Wrap(() =>
        {
            var array = HeaderBaseTests.MakeByteArray(HeaderBaseTests.ValidProperties);
            var header = TestUtils.Create<HeaderBase>(array);

            var allScoreData = new Th143AllScoreDataWrapper();
            allScoreData.Set(header);

            Assert.AreSame(header, allScoreData.Header);
        });

        [TestMethod]
        public void Th143AllScoreDataSetHeaderTestTwice() => TestUtils.Wrap(() =>
        {
            var array = HeaderBaseTests.MakeByteArray(HeaderBaseTests.ValidProperties);
            var header1 = TestUtils.Create<HeaderBase>(array);
            var header2 = TestUtils.Create<HeaderBase>(array);

            var allScoreData = new Th143AllScoreDataWrapper();
            allScoreData.Set(header1);
            allScoreData.Set(header2);

            Assert.AreNotSame(header1, allScoreData.Header);
            Assert.AreSame(header2, allScoreData.Header);
        });

        [TestMethod]
        public void Th143AllScoreDataSetScoreTest() => TestUtils.Wrap(() =>
        {
            var score = new ScoreStub();

            var allScoreData = new Th143AllScoreDataWrapper();
            allScoreData.Set(score);

            Assert.AreSame(score, allScoreData.Scores[0]);
        });

        [TestMethod]
        public void Th143AllScoreDataSetScoreTestTwice() => TestUtils.Wrap(() =>
        {
            var score1 = new ScoreStub();
            var score2 = new ScoreStub();

            var allScoreData = new Th143AllScoreDataWrapper();
            allScoreData.Set(score1);
            allScoreData.Set(score2);

            Assert.AreSame(score1, allScoreData.Scores[0]);
            Assert.AreSame(score2, allScoreData.Scores[1]);
        });

        [TestMethod]
        public void Th143AllScoreDataSetItemStatusTest() => TestUtils.Wrap(() =>
        {
            var item = ItemWithTotal.Fablic;
            var status = new ItemStatusStub { Item = item };

            var allScoreData = new Th143AllScoreDataWrapper();
            allScoreData.Set(status);

            Assert.AreSame(status, allScoreData.ItemStatuses[item]);
        });

        [TestMethod]
        public void Th143AllScoreDataSetItemStatusTestTwice() => TestUtils.Wrap(() =>
        {
            var item = ItemWithTotal.Fablic;
            var status1 = new ItemStatusStub { Item = item };
            var status2 = new ItemStatusStub { Item = item };

            var allScoreData = new Th143AllScoreDataWrapper();
            allScoreData.Set(status1);
            allScoreData.Set(status2);

            Assert.AreSame(status1, allScoreData.ItemStatuses[item]);
            Assert.AreNotSame(status2, allScoreData.ItemStatuses[item]);
        });

        [TestMethod]
        public void Th143AllScoreDataSetStatusTest() => TestUtils.Wrap(() =>
        {
            var chapter = ChapterWrapper.Create(
                Th143StatusTests.MakeByteArray(Th143StatusTests.ValidStub));
            var status = new Th143StatusWrapper(chapter);

            var allScoreData = new Th143AllScoreDataWrapper();
            allScoreData.Set(status);

            Assert.AreSame(status.Target, allScoreData.Status.Target);
        });

        [TestMethod]
        public void Th143AllScoreDataSetStatusTestTwice() => TestUtils.Wrap(() =>
        {
            var chapter = ChapterWrapper.Create(
                Th143StatusTests.MakeByteArray(Th143StatusTests.ValidStub));
            var status1 = new Th143StatusWrapper(chapter);
            var status2 = new Th143StatusWrapper(chapter);

            var allScoreData = new Th143AllScoreDataWrapper();
            allScoreData.Set(status1);
            allScoreData.Set(status2);

            Assert.AreNotSame(status1.Target, allScoreData.Status.Target);
            Assert.AreSame(status2.Target, allScoreData.Status.Target);
        });
    }
}

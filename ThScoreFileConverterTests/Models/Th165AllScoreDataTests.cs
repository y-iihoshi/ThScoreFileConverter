using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverterTests.Models.Th095;
using ThScoreFileConverterTests.Models.Th165.Stubs;
using ThScoreFileConverterTests.Models.Wrappers;
using ChapterWrapper = ThScoreFileConverterTests.Models.Th10.Wrappers.ChapterWrapper;
using HeaderBase = ThScoreFileConverter.Models.Th095.HeaderBase;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th165AllScoreDataTests
    {
        [TestMethod]
        public void Th165AllScoreDataTest() => TestUtils.Wrap(() =>
        {
            var allScoreData = new Th165AllScoreDataWrapper();

            Assert.IsNull(allScoreData.Header);
            Assert.AreEqual(0, allScoreData.Scores.Count);
            Assert.IsNull(allScoreData.Status);
        });

        [TestMethod]
        public void Th165AllScoreDataSetHeaderTest() => TestUtils.Wrap(() =>
        {
            var array = HeaderBaseTests.MakeByteArray(HeaderBaseTests.ValidProperties);
            var header = TestUtils.Create<HeaderBase>(array);

            var allScoreData = new Th165AllScoreDataWrapper();
            allScoreData.Set(header);

            Assert.AreSame(header, allScoreData.Header);
        });

        [TestMethod]
        public void Th165AllScoreDataSetHeaderTestTwice() => TestUtils.Wrap(() =>
        {
            var array = HeaderBaseTests.MakeByteArray(HeaderBaseTests.ValidProperties);
            var header1 = TestUtils.Create<HeaderBase>(array);
            var header2 = TestUtils.Create<HeaderBase>(array);

            var allScoreData = new Th165AllScoreDataWrapper();
            allScoreData.Set(header1);
            allScoreData.Set(header2);

            Assert.AreNotSame(header1, allScoreData.Header);
            Assert.AreSame(header2, allScoreData.Header);
        });

        [TestMethod]
        public void Th165AllScoreDataSetClearDataTest() => TestUtils.Wrap(() =>
        {
            var score = new ScoreStub();

            var allScoreData = new Th165AllScoreDataWrapper();
            allScoreData.Set(score);

            Assert.AreSame(score, allScoreData.Scores[0]);
        });

        [TestMethod]
        public void Th165AllScoreDataSetClearDataTestTwice() => TestUtils.Wrap(() =>
        {
            var score1 = new ScoreStub();
            var score2 = new ScoreStub();

            var allScoreData = new Th165AllScoreDataWrapper();
            allScoreData.Set(score1);
            allScoreData.Set(score2);

            Assert.AreSame(score1, allScoreData.Scores[0]);
            Assert.AreSame(score2, allScoreData.Scores[1]);
        });

        [TestMethod]
        public void Th165AllScoreDataSetStatusTest() => TestUtils.Wrap(() =>
        {
            var chapter = ChapterWrapper.Create(
                Th165StatusTests.MakeByteArray(Th165StatusTests.ValidStub));
            var status = new Th165StatusWrapper(chapter);

            var allScoreData = new Th165AllScoreDataWrapper();
            allScoreData.Set(status);

            Assert.AreSame(status.Target, allScoreData.Status.Target);
        });

        [TestMethod]
        public void Th165AllScoreDataSetStatusTestTwice() => TestUtils.Wrap(() =>
        {
            var chapter = ChapterWrapper.Create(
                Th165StatusTests.MakeByteArray(Th165StatusTests.ValidStub));
            var status1 = new Th165StatusWrapper(chapter);
            var status2 = new Th165StatusWrapper(chapter);

            var allScoreData = new Th165AllScoreDataWrapper();
            allScoreData.Set(status1);
            allScoreData.Set(status2);

            Assert.AreNotSame(status1.Target, allScoreData.Status.Target);
            Assert.AreSame(status2.Target, allScoreData.Status.Target);
        });
    }
}

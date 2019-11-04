using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th125;
using ThScoreFileConverterTests.Models.Th095;
using ThScoreFileConverterTests.Models.Th095.Wrappers;
using ThScoreFileConverterTests.Models.Wrappers;
using HeaderBase = ThScoreFileConverter.Models.Th095.HeaderBase;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th125AllScoreDataTests
    {
        [TestMethod]
        public void Th125AllScoreDataTest() => TestUtils.Wrap(() =>
        {
            var allScoreData = new Th125AllScoreDataWrapper();

            Assert.IsNull(allScoreData.Header);
            Assert.AreEqual(0, allScoreData.Scores.Count);
            Assert.IsNull(allScoreData.Status);
        });

        [TestMethod]
        public void Th125AllScoreDataSetHeaderTest() => TestUtils.Wrap(() =>
        {
            var array = HeaderBaseTests.MakeByteArray(HeaderBaseTests.ValidProperties);
            var header = TestUtils.Create<HeaderBase>(array);

            var allScoreData = new Th125AllScoreDataWrapper();
            allScoreData.Set(header);

            Assert.AreSame(header, allScoreData.Header);
        });

        [TestMethod]
        public void Th125AllScoreDataSetHeaderTestTwice() => TestUtils.Wrap(() =>
        {
            var array = HeaderBaseTests.MakeByteArray(HeaderBaseTests.ValidProperties);
            var header1 = TestUtils.Create<HeaderBase>(array);
            var header2 = TestUtils.Create<HeaderBase>(array);

            var allScoreData = new Th125AllScoreDataWrapper();
            allScoreData.Set(header1);
            allScoreData.Set(header2);

            Assert.AreNotSame(header1, allScoreData.Header);
            Assert.AreSame(header2, allScoreData.Header);
        });

        [TestMethod]
        public void Th125AllScoreDataSetClearDataTest() => TestUtils.Wrap(() =>
        {
            var chapter = ChapterWrapper.Create(
                Th125.ScoreTests.MakeByteArray(Th125.ScoreTests.ValidStub));
            var score = new Score(chapter.Target);

            var allScoreData = new Th125AllScoreDataWrapper();
            allScoreData.Set(score);

            Assert.AreSame(score, allScoreData.Scores[0]);
        });

        [TestMethod]
        public void Th125AllScoreDataSetClearDataTestTwice() => TestUtils.Wrap(() =>
        {
            var chapter = ChapterWrapper.Create(
                Th125.ScoreTests.MakeByteArray(Th125.ScoreTests.ValidStub));
            var score1 = new Score(chapter.Target);
            var score2 = new Score(chapter.Target);

            var allScoreData = new Th125AllScoreDataWrapper();
            allScoreData.Set(score1);
            allScoreData.Set(score2);

            Assert.AreSame(score1, allScoreData.Scores[0]);
            Assert.AreSame(score2, allScoreData.Scores[1]);
        });

        [TestMethod]
        public void Th125AllScoreDataSetStatusTest() => TestUtils.Wrap(() =>
        {
            var chapter = ChapterWrapper.Create(
                Th125.StatusTests.MakeByteArray(Th125.StatusTests.ValidStub));
            var status = new Status(chapter.Target);

            var allScoreData = new Th125AllScoreDataWrapper();
            allScoreData.Set(status);

            Assert.AreSame(status, allScoreData.Status);
        });

        [TestMethod]
        public void Th125AllScoreDataSetStatusTestTwice() => TestUtils.Wrap(() =>
        {
            var chapter = ChapterWrapper.Create(
                Th125.StatusTests.MakeByteArray(Th125.StatusTests.ValidStub));
            var status1 = new Status(chapter.Target);
            var status2 = new Status(chapter.Target);

            var allScoreData = new Th125AllScoreDataWrapper();
            allScoreData.Set(status1);
            allScoreData.Set(status2);

            Assert.AreNotSame(status1, allScoreData.Status);
            Assert.AreSame(status2, allScoreData.Status);
        });
    }
}

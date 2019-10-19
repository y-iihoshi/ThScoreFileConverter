using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th095;
using ThScoreFileConverterTests.Models.Th095;
using ThScoreFileConverterTests.Models.Th095.Wrappers;
using ThScoreFileConverterTests.Models.Wrappers;

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
            Assert.AreEqual(0, allScoreData.ScoresCount);
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
                Th125ScoreTests.MakeByteArray(Th125ScoreTests.ValidStub));
            var clearData = new Th125ScoreWrapper(chapter);

            var allScoreData = new Th125AllScoreDataWrapper();
            allScoreData.Set(clearData);

            Assert.AreSame(clearData.Target, allScoreData.ScoresItem(0).Target);
        });

        [TestMethod]
        public void Th125AllScoreDataSetClearDataTestTwice() => TestUtils.Wrap(() =>
        {
            var chapter = ChapterWrapper.Create(
                Th125ScoreTests.MakeByteArray(Th125ScoreTests.ValidStub));
            var clearData1 = new Th125ScoreWrapper(chapter);
            var clearData2 = new Th125ScoreWrapper(chapter);

            var allScoreData = new Th125AllScoreDataWrapper();
            allScoreData.Set(clearData1);
            allScoreData.Set(clearData2);

            Assert.AreSame(clearData1.Target, allScoreData.ScoresItem(0).Target);
            Assert.AreSame(clearData2.Target, allScoreData.ScoresItem(1).Target);
        });

        [TestMethod]
        public void Th125AllScoreDataSetStatusTest() => TestUtils.Wrap(() =>
        {
            var chapter = ChapterWrapper.Create(
                Th125StatusTests.MakeByteArray(Th125StatusTests.ValidStub));
            var status = new Th125StatusWrapper(chapter);

            var allScoreData = new Th125AllScoreDataWrapper();
            allScoreData.Set(status);

            Assert.AreSame(status.Target, allScoreData.Status.Target);
        });

        [TestMethod]
        public void Th125AllScoreDataSetStatusTestTwice() => TestUtils.Wrap(() =>
        {
            var chapter = ChapterWrapper.Create(
                Th125StatusTests.MakeByteArray(Th125StatusTests.ValidStub));
            var status1 = new Th125StatusWrapper(chapter);
            var status2 = new Th125StatusWrapper(chapter);

            var allScoreData = new Th125AllScoreDataWrapper();
            allScoreData.Set(status1);
            allScoreData.Set(status2);

            Assert.AreNotSame(status1.Target, allScoreData.Status.Target);
            Assert.AreSame(status2.Target, allScoreData.Status.Target);
        });
    }
}

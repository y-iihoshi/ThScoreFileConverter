using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models;
using ThScoreFileConverterTests.Models.Th095;
using ThScoreFileConverterTests.Models.Th095.Wrappers;
using ChapterWrapper = ThScoreFileConverterTests.Models.Th10.Wrappers.ChapterWrapper;
using ThScoreFileConverterTests.Models.Wrappers;

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
            Assert.AreEqual(0, allScoreData.ScoresCount);
            Assert.IsNull(allScoreData.Status);
        });

        [TestMethod]
        public void Th165AllScoreDataSetHeaderTest() => TestUtils.Wrap(() =>
        {
            var array = HeaderTests.MakeByteArray(HeaderTests.GetValidProperties("T165"));
            var header = HeaderWrapper<Th165Converter>.Create(array);

            var allScoreData = new Th165AllScoreDataWrapper();
            allScoreData.Set(header);

            Assert.AreSame(header.Target, allScoreData.Header.Target);
        });

        [TestMethod]
        public void Th165AllScoreDataSetHeaderTestTwice() => TestUtils.Wrap(() =>
        {
            var array = HeaderTests.MakeByteArray(HeaderTests.GetValidProperties("T165"));
            var header1 = HeaderWrapper<Th165Converter>.Create(array);
            var header2 = HeaderWrapper<Th165Converter>.Create(array);

            var allScoreData = new Th165AllScoreDataWrapper();
            allScoreData.Set(header1);
            allScoreData.Set(header2);

            Assert.AreNotSame(header1.Target, allScoreData.Header.Target);
            Assert.AreSame(header2.Target, allScoreData.Header.Target);
        });

        [TestMethod]
        public void Th165AllScoreDataSetClearDataTest() => TestUtils.Wrap(() =>
        {
            var properties = Th165ScoreTests.ValidProperties;
            var chapter = ChapterWrapper.Create(Th165ScoreTests.MakeByteArray(properties));
            var clearData = new Th165ScoreWrapper(chapter);

            var allScoreData = new Th165AllScoreDataWrapper();
            allScoreData.Set(clearData);

            Assert.AreSame(clearData.Target, allScoreData.ScoresItem(0).Target);
        });

        [TestMethod]
        public void Th165AllScoreDataSetClearDataTestTwice() => TestUtils.Wrap(() =>
        {
            var properties = Th165ScoreTests.ValidProperties;
            var chapter = ChapterWrapper.Create(Th165ScoreTests.MakeByteArray(properties));
            var clearData1 = new Th165ScoreWrapper(chapter);
            var clearData2 = new Th165ScoreWrapper(chapter);

            var allScoreData = new Th165AllScoreDataWrapper();
            allScoreData.Set(clearData1);
            allScoreData.Set(clearData2);

            Assert.AreSame(clearData1.Target, allScoreData.ScoresItem(0).Target);
            Assert.AreSame(clearData2.Target, allScoreData.ScoresItem(1).Target);
        });

        [TestMethod]
        public void Th165AllScoreDataSetStatusTest() => TestUtils.Wrap(() =>
        {
            var chapter = ChapterWrapper.Create(
                Th165StatusTests.MakeByteArray(Th165StatusTests.ValidProperties));
            var status = new Th165StatusWrapper(chapter);

            var allScoreData = new Th165AllScoreDataWrapper();
            allScoreData.Set(status);

            Assert.AreSame(status.Target, allScoreData.Status.Target);
        });

        [TestMethod]
        public void Th165AllScoreDataSetStatusTestTwice() => TestUtils.Wrap(() =>
        {
            var chapter = ChapterWrapper.Create(
                Th165StatusTests.MakeByteArray(Th165StatusTests.ValidProperties));
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

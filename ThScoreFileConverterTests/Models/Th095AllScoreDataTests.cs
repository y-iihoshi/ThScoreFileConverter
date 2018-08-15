using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th095AllScoreDataTests
    {
        [TestMethod]
        public void Th095AllScoreDataTest() => TestUtils.Wrap(() =>
        {
            var allScoreData = new Th095AllScoreDataWrapper();

            Assert.IsNull(allScoreData.Header);
            Assert.AreEqual(0, allScoreData.ScoresCount);
            Assert.IsNull(allScoreData.Status);
        });

        [TestMethod]
        public void Th095AllScoreDataSetHeaderTest() => TestUtils.Wrap(() =>
        {
            var array = Th095HeaderTests.MakeByteArray(Th095HeaderTests.GetValidProperties("TH95"));
            var header = Th095HeaderWrapper<Th095Converter>.Create(array);

            var allScoreData = new Th095AllScoreDataWrapper();
            allScoreData.Set(header);

            Assert.AreSame(header.Target, allScoreData.Header.Target);
        });

        [TestMethod]
        public void Th095AllScoreDataSetHeaderTestTwice() => TestUtils.Wrap(() =>
        {
            var array = Th095HeaderTests.MakeByteArray(Th095HeaderTests.GetValidProperties("TH95"));
            var header1 = Th095HeaderWrapper<Th095Converter>.Create(array);
            var header2 = Th095HeaderWrapper<Th095Converter>.Create(array);

            var allScoreData = new Th095AllScoreDataWrapper();
            allScoreData.Set(header1);
            allScoreData.Set(header2);

            Assert.AreNotSame(header1.Target, allScoreData.Header.Target);
            Assert.AreSame(header2.Target, allScoreData.Header.Target);
        });

        [TestMethod]
        public void Th095AllScoreDataSetClearDataTest() => TestUtils.Wrap(() =>
        {
            var properties = Th095ScoreTests.ValidProperties;
            var chapter = Th095ChapterWrapper<Th095Converter>.Create(Th095ScoreTests.MakeByteArray(properties));
            var clearData = new Th095ScoreWrapper(chapter);

            var allScoreData = new Th095AllScoreDataWrapper();
            allScoreData.Set(clearData);

            Assert.AreSame(clearData.Target, allScoreData.ScoresItem(0).Target);
        });

        [TestMethod]
        public void Th095AllScoreDataSetClearDataTestTwice() => TestUtils.Wrap(() =>
        {
            var properties = Th095ScoreTests.ValidProperties;
            var chapter = Th095ChapterWrapper<Th095Converter>.Create(Th095ScoreTests.MakeByteArray(properties));
            var clearData1 = new Th095ScoreWrapper(chapter);
            var clearData2 = new Th095ScoreWrapper(chapter);

            var allScoreData = new Th095AllScoreDataWrapper();
            allScoreData.Set(clearData1);
            allScoreData.Set(clearData2);

            Assert.AreSame(clearData1.Target, allScoreData.ScoresItem(0).Target);
            Assert.AreSame(clearData2.Target, allScoreData.ScoresItem(1).Target);
        });

        [TestMethod]
        public void Th095AllScoreDataSetStatusTest() => TestUtils.Wrap(() =>
        {
            var chapter = Th095ChapterWrapper<Th095Converter>.Create(
                Th095StatusTests.MakeByteArray(Th095StatusTests.ValidProperties));
            var status = new Th095StatusWrapper(chapter);

            var allScoreData = new Th095AllScoreDataWrapper();
            allScoreData.Set(status);

            Assert.AreSame(status.Target, allScoreData.Status.Target);
        });

        [TestMethod]
        public void Th095AllScoreDataSetStatusTestTwice() => TestUtils.Wrap(() =>
        {
            var chapter = Th095ChapterWrapper<Th095Converter>.Create(
                Th095StatusTests.MakeByteArray(Th095StatusTests.ValidProperties));
            var status1 = new Th095StatusWrapper(chapter);
            var status2 = new Th095StatusWrapper(chapter);

            var allScoreData = new Th095AllScoreDataWrapper();
            allScoreData.Set(status1);
            allScoreData.Set(status2);

            Assert.AreNotSame(status1.Target, allScoreData.Status.Target);
            Assert.AreSame(status2.Target, allScoreData.Status.Target);
        });
    }
}

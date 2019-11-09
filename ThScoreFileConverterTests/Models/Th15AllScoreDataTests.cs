using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th095;
using ThScoreFileConverterTests.Models.Th095;
using ThScoreFileConverterTests.Models.Wrappers;
using ChapterWrapper = ThScoreFileConverterTests.Models.Th10.Wrappers.ChapterWrapper;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th15AllScoreDataTests
    {
        [TestMethod]
        public void Th15AllScoreDataTest() => TestUtils.Wrap(() =>
        {
            var allScoreData = new Th15AllScoreDataWrapper();

            Assert.IsNull(allScoreData.Header);
            Assert.AreEqual(0, allScoreData.ClearDataCount);
            Assert.IsNull(allScoreData.Status);
        });

        [TestMethod]
        public void Th15AllScoreDataSetHeaderTest() => TestUtils.Wrap(() =>
        {
            var array = HeaderBaseTests.MakeByteArray(HeaderBaseTests.ValidProperties);
            var header = TestUtils.Create<HeaderBase>(array);

            var allScoreData = new Th15AllScoreDataWrapper();
            allScoreData.Set(header);

            Assert.AreSame(header, allScoreData.Header);
        });

        [TestMethod]
        public void Th15AllScoreDataSetHeaderTestTwice() => TestUtils.Wrap(() =>
        {
            var array = HeaderBaseTests.MakeByteArray(HeaderBaseTests.ValidProperties);
            var header1 = TestUtils.Create<HeaderBase>(array);
            var header2 = TestUtils.Create<HeaderBase>(array);

            var allScoreData = new Th15AllScoreDataWrapper();
            allScoreData.Set(header1);
            allScoreData.Set(header2);

            Assert.AreNotSame(header1, allScoreData.Header);
            Assert.AreSame(header2, allScoreData.Header);
        });

        [TestMethod]
        public void Th15AllScoreDataSetClearDataTest() => TestUtils.Wrap(() =>
        {
            var stub = Th15ClearDataTests.GetValidStub();
            var chapter = ChapterWrapper.Create(Th15ClearDataTests.MakeByteArray(stub));
            var clearData = new Th15ClearDataWrapper(chapter);

            var allScoreData = new Th15AllScoreDataWrapper();
            allScoreData.Set(clearData);

            Assert.AreSame(clearData.Target, allScoreData.ClearDataItem(stub.Chara).Target);
        });

        [TestMethod]
        public void Th15AllScoreDataSetClearDataTestTwice() => TestUtils.Wrap(() =>
        {
            var stub = Th15ClearDataTests.GetValidStub();
            var chapter = ChapterWrapper.Create(Th15ClearDataTests.MakeByteArray(stub));
            var clearData1 = new Th15ClearDataWrapper(chapter);
            var clearData2 = new Th15ClearDataWrapper(chapter);

            var allScoreData = new Th15AllScoreDataWrapper();
            allScoreData.Set(clearData1);
            allScoreData.Set(clearData2);

            Assert.AreSame(clearData1.Target, allScoreData.ClearDataItem(stub.Chara).Target);
            Assert.AreNotSame(clearData2.Target, allScoreData.ClearDataItem(stub.Chara).Target);
        });

        [TestMethod]
        public void Th15AllScoreDataSetStatusTest() => TestUtils.Wrap(() =>
        {
            var chapter = ChapterWrapper.Create(Th13StatusTests.MakeByteArray(Th13StatusTests.ValidStub));
            var status = new Th128StatusWrapper<Th15Converter>(chapter);

            var allScoreData = new Th15AllScoreDataWrapper();
            allScoreData.Set(status);

            Assert.AreSame(status.Target, allScoreData.Status.Target);
        });

        [TestMethod]
        public void Th15AllScoreDataSetStatusTestTwice() => TestUtils.Wrap(() =>
        {
            var chapter = ChapterWrapper.Create(Th13StatusTests.MakeByteArray(Th13StatusTests.ValidStub));
            var status1 = new Th128StatusWrapper<Th15Converter>(chapter);
            var status2 = new Th128StatusWrapper<Th15Converter>(chapter);

            var allScoreData = new Th15AllScoreDataWrapper();
            allScoreData.Set(status1);
            allScoreData.Set(status2);

            Assert.AreNotSame(status1.Target, allScoreData.Status.Target);
            Assert.AreSame(status2.Target, allScoreData.Status.Target);
        });
    }
}

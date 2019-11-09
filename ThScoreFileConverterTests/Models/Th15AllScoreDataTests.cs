using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverterTests.Models.Th095;
using ThScoreFileConverterTests.Models.Th125.Stubs;
using ThScoreFileConverterTests.Models.Wrappers;
using ChapterWrapper = ThScoreFileConverterTests.Models.Th10.Wrappers.ChapterWrapper;
using HeaderBase = ThScoreFileConverter.Models.Th095.HeaderBase;

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
            var status = new StatusStub();

            var allScoreData = new Th15AllScoreDataWrapper();
            allScoreData.Set(status);

            Assert.AreSame(status, allScoreData.Status);
        });

        [TestMethod]
        public void Th15AllScoreDataSetStatusTestTwice() => TestUtils.Wrap(() =>
        {
            var status1 = new StatusStub();
            var status2 = new StatusStub();

            var allScoreData = new Th15AllScoreDataWrapper();
            allScoreData.Set(status1);
            allScoreData.Set(status2);

            Assert.AreNotSame(status1, allScoreData.Status);
            Assert.AreSame(status2, allScoreData.Status);
        });
    }
}

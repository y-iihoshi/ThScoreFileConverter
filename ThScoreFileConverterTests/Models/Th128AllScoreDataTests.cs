using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th128;
using ThScoreFileConverterTests.Models.Th128.Stubs;
using ThScoreFileConverterTests.Models.Wrappers;
using ChapterWrapper = ThScoreFileConverterTests.Models.Th10.Wrappers.ChapterWrapper;
using HeaderBase = ThScoreFileConverter.Models.Th095.HeaderBase;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th128AllScoreDataTests
    {
        [TestMethod]
        public void Th128AllScoreDataTest() => TestUtils.Wrap(() =>
        {
            var allScoreData = new Th128AllScoreDataWrapper();

            Assert.IsNull(allScoreData.Header);
            Assert.AreEqual(0, allScoreData.ClearData.Count);
            Assert.IsNull(allScoreData.CardData);
            Assert.IsNull(allScoreData.Status);
        });

        [TestMethod]
        public void Th128AllScoreDataSetHeaderTest() => TestUtils.Wrap(() =>
        {
            var array = Th095.HeaderBaseTests.MakeByteArray(Th095.HeaderBaseTests.ValidProperties);
            var header = TestUtils.Create<HeaderBase>(array);

            var allScoreData = new Th128AllScoreDataWrapper();
            allScoreData.Set(header);

            Assert.AreSame(header, allScoreData.Header);
        });

        [TestMethod]
        public void Th128AllScoreDataSetHeaderTestTwice() => TestUtils.Wrap(() =>
        {
            var array = Th095.HeaderBaseTests.MakeByteArray(Th095.HeaderBaseTests.ValidProperties);
            var header1 = TestUtils.Create<HeaderBase>(array);
            var header2 = TestUtils.Create<HeaderBase>(array);

            var allScoreData = new Th128AllScoreDataWrapper();
            allScoreData.Set(header1);
            allScoreData.Set(header2);

            Assert.AreNotSame(header1, allScoreData.Header);
            Assert.AreSame(header2, allScoreData.Header);
        });

        [TestMethod]
        public void Th128AllScoreDataSetClearDataTest() => TestUtils.Wrap(() =>
        {
            var route = RouteWithTotal.A2;
            var clearData = new ClearDataStub { Route = route };

            var allScoreData = new Th128AllScoreDataWrapper();
            allScoreData.Set(clearData);

            Assert.AreSame(clearData, allScoreData.ClearData[route]);
        });

        [TestMethod]
        public void Th128AllScoreDataSetClearDataTestTwice() => TestUtils.Wrap(() =>
        {
            var route = RouteWithTotal.A2;
            var clearData1 = new ClearDataStub { Route = route };
            var clearData2 = new ClearDataStub { Route = route };

            var allScoreData = new Th128AllScoreDataWrapper();
            allScoreData.Set(clearData1);
            allScoreData.Set(clearData2);

            Assert.AreSame(clearData1, allScoreData.ClearData[route]);
            Assert.AreNotSame(clearData2, allScoreData.ClearData[route]);
        });

        [TestMethod]
        public void Th128AllScoreDataSetCardDataTest() => TestUtils.Wrap(() =>
        {
            var chapter = ChapterWrapper.Create(
                Th128CardDataTests.MakeByteArray(Th128CardDataTests.ValidStub));
            var clearData = new Th128CardDataWrapper(chapter);

            var allScoreData = new Th128AllScoreDataWrapper();
            allScoreData.Set(clearData);

            Assert.AreSame(clearData.Target, allScoreData.CardData.Target);
        });

        [TestMethod]
        public void Th128AllScoreDataSetCardDataTestTwice() => TestUtils.Wrap(() =>
        {
            var chapter = ChapterWrapper.Create(
                Th128CardDataTests.MakeByteArray(Th128CardDataTests.ValidStub));
            var clearData1 = new Th128CardDataWrapper(chapter);
            var clearData2 = new Th128CardDataWrapper(chapter);

            var allScoreData = new Th128AllScoreDataWrapper();
            allScoreData.Set(clearData1);
            allScoreData.Set(clearData2);

            Assert.AreNotSame(clearData1.Target, allScoreData.CardData.Target);
            Assert.AreSame(clearData2.Target, allScoreData.CardData.Target);
        });

        [TestMethod]
        public void Th128AllScoreDataSetStatusTest() => TestUtils.Wrap(() =>
        {
            var chapter = ChapterWrapper.Create(
                Th128StatusTests.MakeByteArray(Th128StatusTests.GetValidStub(2, 0x42C, 10), 0x10, 0x18));
            var status = new Th128StatusWrapper<Th128Converter>(chapter);

            var allScoreData = new Th128AllScoreDataWrapper();
            allScoreData.Set(status);

            Assert.AreSame(status.Target, allScoreData.Status.Target);
        });

        [TestMethod]
        public void Th128AllScoreDataSetStatusTestTwice() => TestUtils.Wrap(() =>
        {
            var chapter = ChapterWrapper.Create(
                Th128StatusTests.MakeByteArray(Th128StatusTests.GetValidStub(2, 0x42C, 10), 0x10, 0x18));
            var status1 = new Th128StatusWrapper<Th128Converter>(chapter);
            var status2 = new Th128StatusWrapper<Th128Converter>(chapter);

            var allScoreData = new Th128AllScoreDataWrapper();
            allScoreData.Set(status1);
            allScoreData.Set(status2);

            Assert.AreNotSame(status1.Target, allScoreData.Status.Target);
            Assert.AreSame(status2.Target, allScoreData.Status.Target);
        });
    }
}

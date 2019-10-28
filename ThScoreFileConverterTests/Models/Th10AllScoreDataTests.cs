using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th10;
using ThScoreFileConverterTests.Models.Th10;
using ThScoreFileConverterTests.Models.Th10.Wrappers;
using ThScoreFileConverterTests.Models.Wrappers;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th10AllScoreDataTests
    {
        [TestMethod]
        public void Th10AllScoreDataTest() => TestUtils.Wrap(() =>
        {
            var allScoreData = new Th10AllScoreDataWrapper<Th10Converter, CharaWithTotal, StageProgress>();

            Assert.IsNull(allScoreData.Header);
            Assert.AreEqual(0, allScoreData.ClearDataCount);
            Assert.IsNull(allScoreData.Status);
        });

        [TestMethod]
        public void Th10AllScoreDataSetHeaderTest() => TestUtils.Wrap(() =>
        {
            var array = Th095.HeaderBaseTests.MakeByteArray(Th095.HeaderBaseTests.ValidProperties);
            var header = TestUtils.Create<ThScoreFileConverter.Models.Th095.HeaderBase>(array);

            var allScoreData = new Th10AllScoreDataWrapper<Th10Converter, CharaWithTotal, StageProgress>();
            allScoreData.Set(header);

            Assert.AreSame(header, allScoreData.Header);
        });

        [TestMethod]
        public void Th10AllScoreDataSetHeaderTestTwice() => TestUtils.Wrap(() =>
        {
            var array = Th095.HeaderBaseTests.MakeByteArray(Th095.HeaderBaseTests.ValidProperties);
            var header1 = TestUtils.Create<ThScoreFileConverter.Models.Th095.HeaderBase>(array);
            var header2 = TestUtils.Create<ThScoreFileConverter.Models.Th095.HeaderBase>(array);

            var allScoreData = new Th10AllScoreDataWrapper<Th10Converter, CharaWithTotal, StageProgress>();
            allScoreData.Set(header1);
            allScoreData.Set(header2);

            Assert.AreNotSame(header1, allScoreData.Header);
            Assert.AreSame(header2, allScoreData.Header);
        });

        [TestMethod]
        public void Th10AllScoreDataSetClearDataTest() => TestUtils.Wrap(() =>
        {
            var stub = ClearDataTests.MakeValidStub();
            var chapter = ChapterWrapper.Create(ClearDataTests.MakeByteArray(stub));
            var clearData = new ClearData(chapter.Target);

            var allScoreData = new Th10AllScoreDataWrapper<Th10Converter, CharaWithTotal, StageProgress>();
            allScoreData.Set(clearData);

            Assert.AreSame(clearData, allScoreData.ClearDataItem(stub.Chara).Target);
        });

        [TestMethod]
        public void Th10AllScoreDataSetClearDataTestTwice() => TestUtils.Wrap(() =>
        {
            var stub = ClearDataTests.MakeValidStub();
            var chapter = ChapterWrapper.Create(ClearDataTests.MakeByteArray(stub));
            var clearData1 = new ClearData(chapter.Target);
            var clearData2 = new ClearData(chapter.Target);

            var allScoreData = new Th10AllScoreDataWrapper<Th10Converter, CharaWithTotal, StageProgress>();
            allScoreData.Set(clearData1);
            allScoreData.Set(clearData2);

            Assert.AreSame(clearData1, allScoreData.ClearDataItem(stub.Chara).Target);
            Assert.AreNotSame(clearData2, allScoreData.ClearDataItem(stub.Chara).Target);
        });

        [TestMethod]
        public void Th10AllScoreDataSetStatusTest() => TestUtils.Wrap(() =>
        {
            var chapter = ChapterWrapper.Create(StatusTests.MakeByteArray(StatusTests.ValidStub));
            var status = new Status(chapter.Target);

            var allScoreData = new Th10AllScoreDataWrapper<Th10Converter, CharaWithTotal, StageProgress>();
            allScoreData.Set(status);

            Assert.AreSame(status, allScoreData.Status);
        });

        [TestMethod]
        public void Th10AllScoreDataSetStatusTestTwice() => TestUtils.Wrap(() =>
        {
            var chapter = ChapterWrapper.Create(StatusTests.MakeByteArray(StatusTests.ValidStub));
            var status1 = new Status(chapter.Target);
            var status2 = new Status(chapter.Target);

            var allScoreData = new Th10AllScoreDataWrapper<Th10Converter, CharaWithTotal, StageProgress>();
            allScoreData.Set(status1);
            allScoreData.Set(status2);

            Assert.AreNotSame(status1, allScoreData.Status);
            Assert.AreSame(status2, allScoreData.Status);
        });
    }
}

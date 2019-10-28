using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th095;
using ThScoreFileConverter.Models.Th10;
using ThScoreFileConverterTests.Models.Th095;
using ThScoreFileConverterTests.Models.Wrappers;
using ChapterWrapper = ThScoreFileConverterTests.Models.Th10.Wrappers.ChapterWrapper;

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
            var array = HeaderBaseTests.MakeByteArray(HeaderBaseTests.ValidProperties);
            var header = TestUtils.Create<HeaderBase>(array);

            var allScoreData = new Th10AllScoreDataWrapper<Th10Converter, CharaWithTotal, StageProgress>();
            allScoreData.Set(header);

            Assert.AreSame(header, allScoreData.Header);
        });

        [TestMethod]
        public void Th10AllScoreDataSetHeaderTestTwice() => TestUtils.Wrap(() =>
        {
            var array = HeaderBaseTests.MakeByteArray(HeaderBaseTests.ValidProperties);
            var header1 = TestUtils.Create<HeaderBase>(array);
            var header2 = TestUtils.Create<HeaderBase>(array);

            var allScoreData = new Th10AllScoreDataWrapper<Th10Converter, CharaWithTotal, StageProgress>();
            allScoreData.Set(header1);
            allScoreData.Set(header2);

            Assert.AreNotSame(header1, allScoreData.Header);
            Assert.AreSame(header2, allScoreData.Header);
        });

        [TestMethod]
        public void Th10AllScoreDataSetClearDataTest() => TestUtils.Wrap(() =>
        {
            var stub = Th10ClearDataTests.MakeValidStub();
            var chapter = ChapterWrapper.Create(Th10ClearDataTests.MakeByteArray(stub));
            var clearData = new Th10ClearDataWrapper<Th10Converter, CharaWithTotal, StageProgress>(chapter);

            var allScoreData = new Th10AllScoreDataWrapper<Th10Converter, CharaWithTotal, StageProgress>();
            allScoreData.Set(clearData);

            Assert.AreSame(clearData.Target, allScoreData.ClearDataItem(stub.Chara).Target);
        });

        [TestMethod]
        public void Th10AllScoreDataSetClearDataTestTwice() => TestUtils.Wrap(() =>
        {
            var stub = Th10ClearDataTests.MakeValidStub();
            var chapter = ChapterWrapper.Create(Th10ClearDataTests.MakeByteArray(stub));
            var clearData1 = new Th10ClearDataWrapper<Th10Converter, CharaWithTotal, StageProgress>(chapter);
            var clearData2 = new Th10ClearDataWrapper<Th10Converter, CharaWithTotal, StageProgress>(chapter);

            var allScoreData = new Th10AllScoreDataWrapper<Th10Converter, CharaWithTotal, StageProgress>();
            allScoreData.Set(clearData1);
            allScoreData.Set(clearData2);

            Assert.AreSame(clearData1.Target, allScoreData.ClearDataItem(stub.Chara).Target);
            Assert.AreNotSame(clearData2.Target, allScoreData.ClearDataItem(stub.Chara).Target);
        });

        [TestMethod]
        public void Th10AllScoreDataSetStatusTest() => TestUtils.Wrap(() =>
        {
            var chapter = ChapterWrapper.Create(Th10.StatusTests.MakeByteArray(Th10.StatusTests.ValidStub));
            var status = new ThScoreFileConverter.Models.Th10.Status(chapter.Target);

            var allScoreData = new Th10AllScoreDataWrapper<Th10Converter, CharaWithTotal, StageProgress>();
            allScoreData.Set(status);

            Assert.AreSame(status, allScoreData.Status);
        });

        [TestMethod]
        public void Th10AllScoreDataSetStatusTestTwice() => TestUtils.Wrap(() =>
        {
            var chapter = ChapterWrapper.Create(Th10.StatusTests.MakeByteArray(Th10.StatusTests.ValidStub));
            var status1 = new ThScoreFileConverter.Models.Th10.Status(chapter.Target);
            var status2 = new ThScoreFileConverter.Models.Th10.Status(chapter.Target);

            var allScoreData = new Th10AllScoreDataWrapper<Th10Converter, CharaWithTotal, StageProgress>();
            allScoreData.Set(status1);
            allScoreData.Set(status2);

            Assert.AreNotSame(status1, allScoreData.Status);
            Assert.AreSame(status2, allScoreData.Status);
        });
    }
}

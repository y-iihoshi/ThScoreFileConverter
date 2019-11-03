using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th095;
using ThScoreFileConverter.Models.Th12;
using ThScoreFileConverterTests.Models.Th095;
using ThScoreFileConverterTests.Models.Th12;
using ThScoreFileConverterTests.Models.Wrappers;
using ChapterWrapper = ThScoreFileConverterTests.Models.Th10.Wrappers.ChapterWrapper;
using StageProgress = ThScoreFileConverter.Models.Th10.StageProgress;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th12AllScoreDataTests
    {
        [TestMethod]
        public void Th12AllScoreDataTest() => TestUtils.Wrap(() =>
        {
            var allScoreData = new Th10AllScoreDataWrapper<Th12Converter, CharaWithTotal, StageProgress>();

            Assert.IsNull(allScoreData.Header);
            Assert.AreEqual(0, allScoreData.ClearData.Count);
            Assert.IsNull(allScoreData.Status);
        });

        [TestMethod]
        public void Th12AllScoreDataSetHeaderTest() => TestUtils.Wrap(() =>
        {
            var array = HeaderBaseTests.MakeByteArray(HeaderBaseTests.ValidProperties);
            var header = TestUtils.Create<HeaderBase>(array);

            var allScoreData = new Th10AllScoreDataWrapper<Th12Converter, CharaWithTotal, StageProgress>();
            allScoreData.Set(header);

            Assert.AreSame(header, allScoreData.Header);
        });

        [TestMethod]
        public void Th12AllScoreDataSetHeaderTestTwice() => TestUtils.Wrap(() =>
        {
            var array = HeaderBaseTests.MakeByteArray(HeaderBaseTests.ValidProperties);
            var header1 = TestUtils.Create<HeaderBase>(array);
            var header2 = TestUtils.Create<HeaderBase>(array);

            var allScoreData = new Th10AllScoreDataWrapper<Th12Converter, CharaWithTotal, StageProgress>();
            allScoreData.Set(header1);
            allScoreData.Set(header2);

            Assert.AreNotSame(header1, allScoreData.Header);
            Assert.AreSame(header2, allScoreData.Header);
        });

        [TestMethod]
        public void Th12AllScoreDataSetClearDataTest() => TestUtils.Wrap(() =>
        {
            var stub = ClearDataTests.MakeValidStub();
            var chapter = ChapterWrapper.Create(ClearDataTests.MakeByteArray(stub));
            var clearData = new ClearData(chapter.Target);

            var allScoreData = new Th10AllScoreDataWrapper<Th12Converter, CharaWithTotal, StageProgress>();
            allScoreData.Set(clearData);

            Assert.AreSame(clearData, allScoreData.ClearData[stub.Chara]);
        });

        [TestMethod]
        public void Th12AllScoreDataSetClearDataTestTwice() => TestUtils.Wrap(() =>
        {
            var stub = ClearDataTests.MakeValidStub();
            var chapter = ChapterWrapper.Create(ClearDataTests.MakeByteArray(stub));
            var clearData1 = new ClearData(chapter.Target);
            var clearData2 = new ClearData(chapter.Target);

            var allScoreData = new Th10AllScoreDataWrapper<Th12Converter, CharaWithTotal, StageProgress>();
            allScoreData.Set(clearData1);
            allScoreData.Set(clearData2);

            Assert.AreSame(clearData1, allScoreData.ClearData[stub.Chara]);
            Assert.AreNotSame(clearData2, allScoreData.ClearData[stub.Chara]);
        });

        [TestMethod]
        public void Th12AllScoreDataSetStatusTest() => TestUtils.Wrap(() =>
        {
            var chapter = ChapterWrapper.Create(Th10.StatusTests.MakeByteArray(Th12.StatusTests.ValidStub));
            var status = new ThScoreFileConverter.Models.Th12.Status(chapter.Target);

            var allScoreData = new Th10AllScoreDataWrapper<Th12Converter, CharaWithTotal, StageProgress>();
            allScoreData.Set(status);

            Assert.AreSame(status, allScoreData.Status);
        });

        [TestMethod]
        public void Th12AllScoreDataSetStatusTestTwice() => TestUtils.Wrap(() =>
        {
            var chapter = ChapterWrapper.Create(Th10.StatusTests.MakeByteArray(Th12.StatusTests.ValidStub));
            var status1 = new ThScoreFileConverter.Models.Th12.Status(chapter.Target);
            var status2 = new ThScoreFileConverter.Models.Th12.Status(chapter.Target);

            var allScoreData = new Th10AllScoreDataWrapper<Th12Converter, CharaWithTotal, StageProgress>();
            allScoreData.Set(status1);
            allScoreData.Set(status2);

            Assert.AreNotSame(status1, allScoreData.Status);
            Assert.AreSame(status2, allScoreData.Status);
        });
    }
}

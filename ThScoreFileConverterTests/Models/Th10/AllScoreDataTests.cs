using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th10;
using ThScoreFileConverterTests.Models.Th10.Wrappers;

namespace ThScoreFileConverterTests.Models.Th10
{
    [TestClass]
    public class AllScoreDataTests
    {
        [TestMethod]
        public void AllScoreDataTest() => TestUtils.Wrap(() =>
        {
            var allScoreData = new AllScoreData();

            Assert.IsNull(allScoreData.Header);
            Assert.AreEqual(0, allScoreData.ClearData.Count);
            Assert.IsNull(allScoreData.Status);
        });

        [TestMethod]
        public void SetHeaderTest() => TestUtils.Wrap(() =>
        {
            var array = Th095.HeaderBaseTests.MakeByteArray(Th095.HeaderBaseTests.ValidProperties);
            var header = TestUtils.Create<ThScoreFileConverter.Models.Th095.HeaderBase>(array);

            var allScoreData = new AllScoreData();
            allScoreData.Set(header);

            Assert.AreSame(header, allScoreData.Header);
        });

        [TestMethod]
        public void SetHeaderTestTwice() => TestUtils.Wrap(() =>
        {
            var array = Th095.HeaderBaseTests.MakeByteArray(Th095.HeaderBaseTests.ValidProperties);
            var header1 = TestUtils.Create<ThScoreFileConverter.Models.Th095.HeaderBase>(array);
            var header2 = TestUtils.Create<ThScoreFileConverter.Models.Th095.HeaderBase>(array);

            var allScoreData = new AllScoreData();
            allScoreData.Set(header1);
            allScoreData.Set(header2);

            Assert.AreNotSame(header1, allScoreData.Header);
            Assert.AreSame(header2, allScoreData.Header);
        });

        [TestMethod]
        public void SetClearDataTest() => TestUtils.Wrap(() =>
        {
            var stub = ClearDataTests.MakeValidStub();
            var chapter = ChapterWrapper.Create(ClearDataTests.MakeByteArray(stub));
            var clearData = new ClearData(chapter.Target);

            var allScoreData = new AllScoreData();
            allScoreData.Set(clearData);

            Assert.AreSame(clearData, allScoreData.ClearData[stub.Chara]);
        });

        [TestMethod]
        public void SetClearDataTestTwice() => TestUtils.Wrap(() =>
        {
            var stub = ClearDataTests.MakeValidStub();
            var chapter = ChapterWrapper.Create(ClearDataTests.MakeByteArray(stub));
            var clearData1 = new ClearData(chapter.Target);
            var clearData2 = new ClearData(chapter.Target);

            var allScoreData = new AllScoreData();
            allScoreData.Set(clearData1);
            allScoreData.Set(clearData2);

            Assert.AreSame(clearData1, allScoreData.ClearData[stub.Chara]);
            Assert.AreNotSame(clearData2, allScoreData.ClearData[stub.Chara]);
        });

        [TestMethod]
        public void SetStatusTest() => TestUtils.Wrap(() =>
        {
            var chapter = ChapterWrapper.Create(StatusTests.MakeByteArray(StatusTests.ValidStub));
            var status = new Status(chapter.Target);

            var allScoreData = new AllScoreData();
            allScoreData.Set(status);

            Assert.AreSame(status, allScoreData.Status);
        });

        [TestMethod]
        public void SetStatusTestTwice() => TestUtils.Wrap(() =>
        {
            var chapter = ChapterWrapper.Create(StatusTests.MakeByteArray(StatusTests.ValidStub));
            var status1 = new Status(chapter.Target);
            var status2 = new Status(chapter.Target);

            var allScoreData = new AllScoreData();
            allScoreData.Set(status1);
            allScoreData.Set(status2);

            Assert.AreNotSame(status1, allScoreData.Status);
            Assert.AreSame(status2, allScoreData.Status);
        });
    }
}

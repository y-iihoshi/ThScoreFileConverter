using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models;
using ThScoreFileConverterTests.Models.Th095;
using ThScoreFileConverterTests.Models.Th095.Wrappers;
using ThScoreFileConverterTests.Models.Wrappers;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th16AllScoreDataTests
    {
        [TestMethod]
        public void Th16AllScoreDataTest() => TestUtils.Wrap(() =>
        {
            var allScoreData = new Th16AllScoreDataWrapper();

            Assert.IsNull(allScoreData.Header);
            Assert.AreEqual(0, allScoreData.ClearDataCount);
            Assert.IsNull(allScoreData.Status);
        });

        [TestMethod]
        public void Th16AllScoreDataSetHeaderTest() => TestUtils.Wrap(() =>
        {
            var array = HeaderTests.MakeByteArray(HeaderTests.GetValidProperties("TH51"));
            var header = HeaderWrapper<Th16Converter>.Create(array);

            var allScoreData = new Th16AllScoreDataWrapper();
            allScoreData.Set(header);

            Assert.AreSame(header.Target, allScoreData.Header.Target);
        });

        [TestMethod]
        public void Th16AllScoreDataSetHeaderTestTwice() => TestUtils.Wrap(() =>
        {
            var array = HeaderTests.MakeByteArray(HeaderTests.GetValidProperties("TH51"));
            var header1 = HeaderWrapper<Th16Converter>.Create(array);
            var header2 = HeaderWrapper<Th16Converter>.Create(array);

            var allScoreData = new Th16AllScoreDataWrapper();
            allScoreData.Set(header1);
            allScoreData.Set(header2);

            Assert.AreNotSame(header1.Target, allScoreData.Header.Target);
            Assert.AreSame(header2.Target, allScoreData.Header.Target);
        });

        [TestMethod]
        public void Th16AllScoreDataSetClearDataTest() => TestUtils.Wrap(() =>
        {
            var properties = Th16ClearDataTests.GetValidProperties();
            var chapter = Th10ChapterWrapper.Create(Th16ClearDataTests.MakeByteArray(properties));
            var clearData = new Th16ClearDataWrapper(chapter);

            var allScoreData = new Th16AllScoreDataWrapper();
            allScoreData.Set(clearData);

            Assert.AreSame(clearData.Target, allScoreData.ClearDataItem(properties.chara).Target);
        });

        [TestMethod]
        public void Th16AllScoreDataSetClearDataTestTwice() => TestUtils.Wrap(() =>
        {
            var properties = Th16ClearDataTests.GetValidProperties();
            var chapter = Th10ChapterWrapper.Create(Th16ClearDataTests.MakeByteArray(properties));
            var clearData1 = new Th16ClearDataWrapper(chapter);
            var clearData2 = new Th16ClearDataWrapper(chapter);

            var allScoreData = new Th16AllScoreDataWrapper();
            allScoreData.Set(clearData1);
            allScoreData.Set(clearData2);

            Assert.AreSame(clearData1.Target, allScoreData.ClearDataItem(properties.chara).Target);
            Assert.AreNotSame(clearData2.Target, allScoreData.ClearDataItem(properties.chara).Target);
        });

        [TestMethod]
        public void Th16AllScoreDataSetStatusTest() => TestUtils.Wrap(() =>
        {
            var chapter = Th10ChapterWrapper.Create(Th128StatusTests.MakeByteArray(
                Th128StatusTests.GetValidProperties(1, 0x42C, 17), 0x10, 0x11));
            var status = new Th128StatusWrapper<Th16Converter>(chapter);

            var allScoreData = new Th16AllScoreDataWrapper();
            allScoreData.Set(status);

            Assert.AreSame(status.Target, allScoreData.Status.Target);
        });

        [TestMethod]
        public void Th16AllScoreDataSetStatusTestTwice() => TestUtils.Wrap(() =>
        {
            var chapter = Th10ChapterWrapper.Create(Th128StatusTests.MakeByteArray(
                Th128StatusTests.GetValidProperties(1, 0x42C, 17), 0x10, 0x11));
            var status1 = new Th128StatusWrapper<Th16Converter>(chapter);
            var status2 = new Th128StatusWrapper<Th16Converter>(chapter);

            var allScoreData = new Th16AllScoreDataWrapper();
            allScoreData.Set(status1);
            allScoreData.Set(status2);

            Assert.AreNotSame(status1.Target, allScoreData.Status.Target);
            Assert.AreSame(status2.Target, allScoreData.Status.Target);
        });
    }
}

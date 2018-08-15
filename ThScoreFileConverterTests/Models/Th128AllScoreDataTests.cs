using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ThScoreFileConverter.Models;

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
            Assert.AreEqual(0, allScoreData.ClearDataCount);
            Assert.IsNull(allScoreData.CardData);
            Assert.IsNull(allScoreData.Status);
        });

        [TestMethod]
        public void Th128AllScoreDataSetHeaderTest() => TestUtils.Wrap(() =>
        {
            var array = Th095HeaderTests.MakeByteArray(Th095HeaderTests.GetValidProperties("T821"));
            var header = Th095HeaderWrapper<Th128Converter>.Create(array);

            var allScoreData = new Th128AllScoreDataWrapper();
            allScoreData.Set(header);

            Assert.AreSame(header.Target, allScoreData.Header.Target);
        });

        [TestMethod]
        public void Th128AllScoreDataSetHeaderTestTwice() => TestUtils.Wrap(() =>
        {
            var array = Th095HeaderTests.MakeByteArray(Th095HeaderTests.GetValidProperties("T821"));
            var header1 = Th095HeaderWrapper<Th128Converter>.Create(array);
            var header2 = Th095HeaderWrapper<Th128Converter>.Create(array);

            var allScoreData = new Th128AllScoreDataWrapper();
            allScoreData.Set(header1);
            allScoreData.Set(header2);

            Assert.AreNotSame(header1.Target, allScoreData.Header.Target);
            Assert.AreSame(header2.Target, allScoreData.Header.Target);
        });

        [TestMethod]
        public void Th128AllScoreDataSetClearDataTest() => TestUtils.Wrap(() =>
        {
            var properties = Th128ClearDataTests.GetValidProperties();
            var chapter = Th10ChapterWrapper<Th128Converter>.Create(Th128ClearDataTests.MakeByteArray(properties));
            var clearData = new Th128ClearDataWrapper(chapter);

            var allScoreData = new Th128AllScoreDataWrapper();
            allScoreData.Set(clearData);

            Assert.AreSame(clearData.Target, allScoreData.ClearDataItem(properties.route).Target);
        });

        [TestMethod]
        public void Th128AllScoreDataSetClearDataTestTwice() => TestUtils.Wrap(() =>
        {
            var properties = Th128ClearDataTests.GetValidProperties();
            var chapter = Th10ChapterWrapper<Th128Converter>.Create(Th128ClearDataTests.MakeByteArray(properties));
            var clearData1 = new Th128ClearDataWrapper(chapter);
            var clearData2 = new Th128ClearDataWrapper(chapter);

            var allScoreData = new Th128AllScoreDataWrapper();
            allScoreData.Set(clearData1);
            allScoreData.Set(clearData2);

            Assert.AreSame(clearData1.Target, allScoreData.ClearDataItem(properties.route).Target);
            Assert.AreNotSame(clearData2.Target, allScoreData.ClearDataItem(properties.route).Target);
        });

        [TestMethod]
        public void Th128AllScoreDataSetCardDataTest() => TestUtils.Wrap(() =>
        {
            var properties = Th128CardDataTests.ValidProperties;
            var chapter = Th10ChapterWrapper<Th128Converter>.Create(Th128CardDataTests.MakeByteArray(properties));
            var clearData = new Th128CardDataWrapper(chapter);

            var allScoreData = new Th128AllScoreDataWrapper();
            allScoreData.Set(clearData);

            Assert.AreSame(clearData.Target, allScoreData.CardData.Target);
        });

        [TestMethod]
        public void Th128AllScoreDataSetCardDataTestTwice() => TestUtils.Wrap(() =>
        {
            var properties = Th128CardDataTests.ValidProperties;
            var chapter = Th10ChapterWrapper<Th128Converter>.Create(Th128CardDataTests.MakeByteArray(properties));
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
            var chapter = Th10ChapterWrapper<Th128Converter>.Create(
                Th128StatusTests.MakeByteArray(Th128StatusTests.GetValidProperties(2, 0x42C, 10), 0x10, 0x18));
            var status = new Th128StatusWrapper<Th128Converter>(chapter);

            var allScoreData = new Th128AllScoreDataWrapper();
            allScoreData.Set(status);

            Assert.AreSame(status.Target, allScoreData.Status.Target);
        });

        [TestMethod]
        public void Th128AllScoreDataSetStatusTestTwice() => TestUtils.Wrap(() =>
        {
            var chapter = Th10ChapterWrapper<Th128Converter>.Create(
                Th128StatusTests.MakeByteArray(Th128StatusTests.GetValidProperties(2, 0x42C, 10), 0x10, 0x18));
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

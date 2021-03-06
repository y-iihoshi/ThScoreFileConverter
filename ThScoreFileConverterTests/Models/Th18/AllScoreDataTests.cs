﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Models.Th18;
using ThScoreFileConverterTests.UnitTesting;
using IClearData = ThScoreFileConverter.Models.Th13.IClearData<
    ThScoreFileConverter.Models.Th18.CharaWithTotal,
    ThScoreFileConverter.Models.Level,
    ThScoreFileConverter.Models.Level,
    ThScoreFileConverter.Models.Th14.LevelPracticeWithTotal,
    ThScoreFileConverter.Models.Stage,
    ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th13.StageProgress>>;

namespace ThScoreFileConverterTests.Models.Th18
{
    [TestClass]
    public class AllScoreDataTests
    {
        [TestMethod]
        public void AllScoreDataTest()
        {
            var allScoreData = new AllScoreData();

            Assert.IsNull(allScoreData.Header);
            Assert.AreEqual(0, allScoreData.ClearData.Count);
            Assert.IsNull(allScoreData.Status);
        }

        [TestMethod]
        public void SetHeaderTest()
        {
            var array = Th095.HeaderBaseTests.MakeByteArray(Th095.HeaderBaseTests.ValidProperties);
            var header = TestUtils.Create<ThScoreFileConverter.Models.Th095.HeaderBase>(array);

            var allScoreData = new AllScoreData();
            allScoreData.Set(header);

            Assert.AreSame(header, allScoreData.Header);
        }

        [TestMethod]
        public void SetHeaderTestTwice()
        {
            var array = Th095.HeaderBaseTests.MakeByteArray(Th095.HeaderBaseTests.ValidProperties);
            var header1 = TestUtils.Create<ThScoreFileConverter.Models.Th095.HeaderBase>(array);
            var header2 = TestUtils.Create<ThScoreFileConverter.Models.Th095.HeaderBase>(array);

            var allScoreData = new AllScoreData();
            allScoreData.Set(header1);
            allScoreData.Set(header2);

            Assert.AreNotSame(header1, allScoreData.Header);
            Assert.AreSame(header2, allScoreData.Header);
        }

        [TestMethod]
        public void SetClearDataTest()
        {
            var chara = CharaWithTotal.Reimu;
            var clearData = Mock.Of<IClearData>(m => m.Chara == chara);

            var allScoreData = new AllScoreData();
            allScoreData.Set(clearData);

            Assert.AreSame(clearData, allScoreData.ClearData[chara]);
        }

        [TestMethod]
        public void SetClearDataTestTwice()
        {
            var chara = CharaWithTotal.Reimu;
            var clearData1 = Mock.Of<IClearData>(m => m.Chara == chara);
            var clearData2 = Mock.Of<IClearData>(m => m.Chara == chara);

            var allScoreData = new AllScoreData();
            allScoreData.Set(clearData1);
            allScoreData.Set(clearData2);

            Assert.AreSame(clearData1, allScoreData.ClearData[chara]);
            Assert.AreNotSame(clearData2, allScoreData.ClearData[chara]);
        }

        [TestMethod]
        public void SetStatusTest()
        {
            var status = Mock.Of<IStatus>();

            var allScoreData = new AllScoreData();
            allScoreData.Set(status);

            Assert.AreSame(status, allScoreData.Status);
        }

        [TestMethod]
        public void SetStatusTestTwice()
        {
            var status1 = Mock.Of<IStatus>();
            var status2 = Mock.Of<IStatus>();

            var allScoreData = new AllScoreData();
            allScoreData.Set(status1);
            allScoreData.Set(status2);

            Assert.AreNotSame(status1, allScoreData.Status);
            Assert.AreSame(status2, allScoreData.Status);
        }
    }
}

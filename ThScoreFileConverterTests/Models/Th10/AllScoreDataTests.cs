using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Models.Th10;
using ThScoreFileConverterTests.UnitTesting;
using HeaderBase = ThScoreFileConverter.Models.Th095.HeaderBase;

namespace ThScoreFileConverterTests.Models.Th10
{
    [TestClass]
    public class AllScoreDataTests
    {
        internal static void AllScoreDataTestHelper<TCharaWithTotal, TStageProgress>()
            where TCharaWithTotal : struct, Enum
            where TStageProgress : struct, Enum
        {
            var allScoreData = new AllScoreData<TCharaWithTotal, TStageProgress>();

            Assert.IsNull(allScoreData.Header);
            Assert.AreEqual(0, allScoreData.ClearData.Count);
            Assert.IsNull(allScoreData.Status);
        }

        internal static void SetHeaderTestHelper<TCharaWithTotal, TStageProgress>()
            where TCharaWithTotal : struct, Enum
            where TStageProgress : struct, Enum
        {
            var header = new HeaderBase();

            var allScoreData = new AllScoreData<TCharaWithTotal, TStageProgress>();
            allScoreData.Set(header);

            Assert.AreSame(header, allScoreData.Header);
        }

        internal static void SetHeaderTestTwiceHelper<TCharaWithTotal, TStageProgress>()
            where TCharaWithTotal : struct, Enum
            where TStageProgress : struct, Enum
        {
            var header1 = new HeaderBase();
            var header2 = new HeaderBase();

            var allScoreData = new AllScoreData<TCharaWithTotal, TStageProgress>();
            allScoreData.Set(header1);
            allScoreData.Set(header2);

            Assert.AreNotSame(header1, allScoreData.Header);
            Assert.AreSame(header2, allScoreData.Header);
        }

        internal static void SetClearDataTestHelper<TCharaWithTotal, TStageProgress>()
            where TCharaWithTotal : struct, Enum
            where TStageProgress : struct, Enum
        {
            var chara = TestUtils.Cast<TCharaWithTotal>(1);
#if false
            var clearData = Mock.Of<IClearData<TCharaWithTotal, TStageProgress>>(m => m.Chara == chara);
#else
            var mock = new Mock<IClearData<TCharaWithTotal, TStageProgress>>();
            _ = mock.SetupGet(m => m.Chara).Returns(chara);
            var clearData = mock.Object;
#endif

            var allScoreData = new AllScoreData<TCharaWithTotal, TStageProgress>();
            allScoreData.Set(clearData);

            Assert.AreSame(clearData, allScoreData.ClearData[chara]);
        }

        internal static void SetClearDataTestTwiceHelper<TCharaWithTotal, TStageProgress>()
            where TCharaWithTotal : struct, Enum
            where TStageProgress : struct, Enum
        {
            var chara = TestUtils.Cast<TCharaWithTotal>(1);
#if false
            var clearData1 = Mock.Of<IClearData<TCharaWithTotal, TStageProgress>>(m => m.Chara == chara);
            var clearData2 = Mock.Of<IClearData<TCharaWithTotal, TStageProgress>>(m => m.Chara == chara);
#else
            var mock1 = new Mock<IClearData<TCharaWithTotal, TStageProgress>>();
            _ = mock1.SetupGet(m => m.Chara).Returns(chara);
            var clearData1 = mock1.Object;
            var mock2 = new Mock<IClearData<TCharaWithTotal, TStageProgress>>();
            _ = mock2.SetupGet(m => m.Chara).Returns(chara);
            var clearData2 = mock2.Object;
#endif

            var allScoreData = new AllScoreData<TCharaWithTotal, TStageProgress>();
            allScoreData.Set(clearData1);
            allScoreData.Set(clearData2);

            Assert.AreSame(clearData1, allScoreData.ClearData[chara]);
            Assert.AreNotSame(clearData2, allScoreData.ClearData[chara]);
        }

        internal static void SetStatusTestHelper<TCharaWithTotal, TStageProgress>()
            where TCharaWithTotal : struct, Enum
            where TStageProgress : struct, Enum
        {
            var status = Mock.Of<IStatus>();

            var allScoreData = new AllScoreData<TCharaWithTotal, TStageProgress>();
            allScoreData.Set(status);

            Assert.AreSame(status, allScoreData.Status);
        }

        internal static void SetStatusTestTwiceHelper<TCharaWithTotal, TStageProgress>()
            where TCharaWithTotal : struct, Enum
            where TStageProgress : struct, Enum
        {
            var status1 = Mock.Of<IStatus>();
            var status2 = Mock.Of<IStatus>();

            var allScoreData = new AllScoreData<CharaWithTotal, StageProgress>();
            allScoreData.Set(status1);
            allScoreData.Set(status2);

            Assert.AreNotSame(status1, allScoreData.Status);
            Assert.AreSame(status2, allScoreData.Status);
        }

        [TestMethod]
        public void AllScoreDataTest() => AllScoreDataTestHelper<CharaWithTotal, StageProgress>();

        [TestMethod]
        public void SetHeaderTest() => SetHeaderTestHelper<CharaWithTotal, StageProgress>();

        [TestMethod]
        public void SetHeaderTestTwice() => SetHeaderTestTwiceHelper<CharaWithTotal, StageProgress>();

        [TestMethod]
        public void SetClearDataTest() => SetClearDataTestHelper<CharaWithTotal, StageProgress>();

        [TestMethod]
        public void SetClearDataTestTwice() => SetClearDataTestTwiceHelper<CharaWithTotal, StageProgress>();

        [TestMethod]
        public void SetStatusTest() => SetStatusTestHelper<CharaWithTotal, StageProgress>();

        [TestMethod]
        public void SetStatusTestTwice() => SetStatusTestTwiceHelper<CharaWithTotal, StageProgress>();
    }
}

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th10;
using ThScoreFileConverterTests.Models.Th10.Stubs;

namespace ThScoreFileConverterTests.Models.Th10
{
    [TestClass]
    public class AllScoreDataTests
    {
        internal static void AllScoreDataTestHelper<TCharaWithTotal, TStageProgress>()
            where TCharaWithTotal : struct, Enum
            where TStageProgress : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var allScoreData = new AllScoreData<TCharaWithTotal, TStageProgress>();

                Assert.IsNull(allScoreData.Header);
                Assert.AreEqual(0, allScoreData.ClearData.Count);
                Assert.IsNull(allScoreData.Status);
            });

        internal static void SetHeaderTestHelper<TCharaWithTotal, TStageProgress>()
            where TCharaWithTotal : struct, Enum
            where TStageProgress : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var header = new ThScoreFileConverter.Models.Th095.HeaderBase();

                var allScoreData = new AllScoreData<TCharaWithTotal, TStageProgress>();
                allScoreData.Set(header);

                Assert.AreSame(header, allScoreData.Header);
            });

        internal static void SetHeaderTestTwiceHelper<TCharaWithTotal, TStageProgress>()
            where TCharaWithTotal : struct, Enum
            where TStageProgress : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var header1 = new ThScoreFileConverter.Models.Th095.HeaderBase();
                var header2 = new ThScoreFileConverter.Models.Th095.HeaderBase();

                var allScoreData = new AllScoreData<TCharaWithTotal, TStageProgress>();
                allScoreData.Set(header1);
                allScoreData.Set(header2);

                Assert.AreNotSame(header1, allScoreData.Header);
                Assert.AreSame(header2, allScoreData.Header);
            });

        internal static void SetClearDataTestHelper<TCharaWithTotal, TStageProgress>()
            where TCharaWithTotal : struct, Enum
            where TStageProgress : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var chara = TestUtils.Cast<TCharaWithTotal>(1);
                var clearData = new ClearDataStub<TCharaWithTotal, TStageProgress> { Chara = chara };

                var allScoreData = new AllScoreData<TCharaWithTotal, TStageProgress>();
                allScoreData.Set(clearData);

                Assert.AreSame(clearData, allScoreData.ClearData[chara]);
            });

        internal static void SetClearDataTestTwiceHelper<TCharaWithTotal, TStageProgress>()
            where TCharaWithTotal : struct, Enum
            where TStageProgress : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var chara = TestUtils.Cast<TCharaWithTotal>(1);
                var clearData1 = new ClearDataStub<TCharaWithTotal, TStageProgress> { Chara = chara };
                var clearData2 = new ClearDataStub<TCharaWithTotal, TStageProgress> { Chara = chara };

                var allScoreData = new AllScoreData<TCharaWithTotal, TStageProgress>();
                allScoreData.Set(clearData1);
                allScoreData.Set(clearData2);

                Assert.AreSame(clearData1, allScoreData.ClearData[chara]);
                Assert.AreNotSame(clearData2, allScoreData.ClearData[chara]);
            });

        internal static void SetStatusTestHelper<TCharaWithTotal, TStageProgress>()
            where TCharaWithTotal : struct, Enum
            where TStageProgress : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var status = new StatusStub();

                var allScoreData = new AllScoreData<TCharaWithTotal, TStageProgress>();
                allScoreData.Set(status);

                Assert.AreSame(status, allScoreData.Status);
            });

        internal static void SetStatusTestTwiceHelper<TCharaWithTotal, TStageProgress>()
            where TCharaWithTotal : struct, Enum
            where TStageProgress : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var status1 = new StatusStub();
                var status2 = new StatusStub();

                var allScoreData = new AllScoreData<CharaWithTotal, StageProgress>();
                allScoreData.Set(status1);
                allScoreData.Set(status2);

                Assert.AreNotSame(status1, allScoreData.Status);
                Assert.AreSame(status2, allScoreData.Status);
            });

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

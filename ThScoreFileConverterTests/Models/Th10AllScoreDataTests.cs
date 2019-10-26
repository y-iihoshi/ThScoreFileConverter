using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
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
        internal static void Th10AllScoreDataTestHelper<TParent, TCharaWithTotal, TStageProgress>()
            where TParent : ThConverter
            where TCharaWithTotal : struct, Enum
            where TStageProgress : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var allScoreData = new Th10AllScoreDataWrapper<TParent, TCharaWithTotal, TStageProgress>();

                Assert.IsNull(allScoreData.Header);
                Assert.AreEqual(0, allScoreData.ClearDataCount);
                Assert.IsNull(allScoreData.Status);
            });

        internal static void Th10AllScoreDataSetHeaderTestHelper<TParent, TCharaWithTotal, TStageProgress>()
            where TParent : ThConverter
            where TCharaWithTotal : struct, Enum
            where TStageProgress : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var array = HeaderBaseTests.MakeByteArray(HeaderBaseTests.ValidProperties);
                var header = TestUtils.Create<HeaderBase>(array);

                var allScoreData = new Th10AllScoreDataWrapper<TParent, TCharaWithTotal, TStageProgress>();
                allScoreData.Set(header);

                Assert.AreSame(header, allScoreData.Header);
            });

        internal static void Th10AllScoreDataSetHeaderTestTwiceHelper<TParent, TCharaWithTotal, TStageProgress>()
            where TParent : ThConverter
            where TCharaWithTotal : struct, Enum
            where TStageProgress : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var array = HeaderBaseTests.MakeByteArray(HeaderBaseTests.ValidProperties);
                var header1 = TestUtils.Create<HeaderBase>(array);
                var header2 = TestUtils.Create<HeaderBase>(array);

                var allScoreData = new Th10AllScoreDataWrapper<TParent, TCharaWithTotal, TStageProgress>();
                allScoreData.Set(header1);
                allScoreData.Set(header2);

                Assert.AreNotSame(header1, allScoreData.Header);
                Assert.AreSame(header2, allScoreData.Header);
            });

        internal static void Th10AllScoreDataSetClearDataTestHelper<TParent, TCharaWithTotal, TStageProgress>(
            ushort version, int size, int numCards)
            where TParent : ThConverter
            where TCharaWithTotal : struct, Enum
            where TStageProgress : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var stub = Th10ClearDataTests.GetValidStub<TCharaWithTotal, TStageProgress>(version, size, numCards);
                var chapter = ChapterWrapper.Create(
                    Th10ClearDataTests.MakeByteArray<TParent, TCharaWithTotal, TStageProgress>(stub));
                var clearData = new Th10ClearDataWrapper<TParent, TCharaWithTotal, TStageProgress>(chapter);

                var allScoreData = new Th10AllScoreDataWrapper<TParent, TCharaWithTotal, TStageProgress>();
                allScoreData.Set(clearData);

                Assert.AreSame(clearData.Target, allScoreData.ClearDataItem(stub.Chara).Target);
            });

        internal static void Th10AllScoreDataSetClearDataTestTwiceHelper<TParent, TCharaWithTotal, TStageProgress>(
            ushort version, int size, int numCards)
            where TParent : ThConverter
            where TCharaWithTotal : struct, Enum
            where TStageProgress : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var stub = Th10ClearDataTests.GetValidStub<TCharaWithTotal, TStageProgress>(version, size, numCards);
                var chapter = ChapterWrapper.Create(
                    Th10ClearDataTests.MakeByteArray<TParent, TCharaWithTotal, TStageProgress>(stub));
                var clearData1 = new Th10ClearDataWrapper<TParent, TCharaWithTotal, TStageProgress>(chapter);
                var clearData2 = new Th10ClearDataWrapper<TParent, TCharaWithTotal, TStageProgress>(chapter);

                var allScoreData = new Th10AllScoreDataWrapper<TParent, TCharaWithTotal, TStageProgress>();
                allScoreData.Set(clearData1);
                allScoreData.Set(clearData2);

                Assert.AreSame(clearData1.Target, allScoreData.ClearDataItem(stub.Chara).Target);
                Assert.AreNotSame(clearData2.Target, allScoreData.ClearDataItem(stub.Chara).Target);
            });

        #region Th10

        [TestMethod]
        public void Th10AllScoreDataTest()
            => Th10AllScoreDataTestHelper<Th10Converter, CharaWithTotal, StageProgress>();

        [TestMethod]
        public void Th10AllScoreDataSetHeaderTest()
            => Th10AllScoreDataSetHeaderTestHelper<Th10Converter, CharaWithTotal, StageProgress>();

        [TestMethod]
        public void Th10AllScoreDataSetHeaderTestTwice()
            => Th10AllScoreDataSetHeaderTestTwiceHelper<Th10Converter, CharaWithTotal, StageProgress>();

        [TestMethod]
        public void Th10AllScoreDataSetClearDataTest()
            => Th10AllScoreDataSetClearDataTestHelper<Th10Converter, CharaWithTotal, StageProgress>(0, 0x437C, 110);

        [TestMethod]
        public void Th10AllScoreDataSetClearDataTestTwice()
            => Th10AllScoreDataSetClearDataTestTwiceHelper<
                Th10Converter, CharaWithTotal, StageProgress>(0, 0x437C, 110);

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

        #endregion

        #region Th11

        [TestMethod]
        public void Th11AllScoreDataTest()
            => Th10AllScoreDataTestHelper<Th11Converter, Th11Converter.CharaWithTotal, Th11Converter.StageProgress>();

        [TestMethod]
        public void Th11AllScoreDataSetHeaderTest()
            => Th10AllScoreDataSetHeaderTestHelper<
                Th11Converter, Th11Converter.CharaWithTotal, Th11Converter.StageProgress>();

        [TestMethod]
        public void Th11AllScoreDataSetHeaderTestTwice()
            => Th10AllScoreDataSetHeaderTestTwiceHelper<
                Th11Converter, Th11Converter.CharaWithTotal, Th11Converter.StageProgress>();

        [TestMethod]
        public void Th11AllScoreDataSetClearDataTest()
            => Th10AllScoreDataSetClearDataTestHelper<
                Th11Converter, Th11Converter.CharaWithTotal, Th11Converter.StageProgress>(0, 0x68D4, 175);

        [TestMethod]
        public void Th11AllScoreDataSetClearDataTestTwice()
            => Th10AllScoreDataSetClearDataTestTwiceHelper<
                Th11Converter, Th11Converter.CharaWithTotal, Th11Converter.StageProgress>(0, 0x68D4, 175);

        [TestMethod]
        public void Th11AllScoreDataSetStatusTest() => TestUtils.Wrap(() =>
        {
            var chapter = ChapterWrapper.Create(
                Th10.StatusTests.MakeByteArray(Th11.StatusTests.ValidStub));
            var status = new ThScoreFileConverter.Models.Th11.Status(chapter.Target);

            var allScoreData = new Th10AllScoreDataWrapper<
                Th11Converter, Th11Converter.CharaWithTotal, Th11Converter.StageProgress>();
            allScoreData.Set(status);

            Assert.AreSame(status, allScoreData.Status);
        });

        [TestMethod]
        public void Th11AllScoreDataSetStatusTestTwice() => TestUtils.Wrap(() =>
        {
            var chapter = ChapterWrapper.Create(Th10.StatusTests.MakeByteArray(Th11.StatusTests.ValidStub));
            var status1 = new ThScoreFileConverter.Models.Th11.Status(chapter.Target);
            var status2 = new ThScoreFileConverter.Models.Th11.Status(chapter.Target);

            var allScoreData = new Th10AllScoreDataWrapper<
                Th11Converter, Th11Converter.CharaWithTotal, Th11Converter.StageProgress>();
            allScoreData.Set(status1);
            allScoreData.Set(status2);

            Assert.AreNotSame(status1, allScoreData.Status);
            Assert.AreSame(status2, allScoreData.Status);
        });

        #endregion

        #region Th12

        [TestMethod]
        public void Th12AllScoreDataTest()
            => Th10AllScoreDataTestHelper<Th12Converter, Th12Converter.CharaWithTotal, Th12Converter.StageProgress>();

        [TestMethod]
        public void Th12AllScoreDataSetHeaderTest()
            => Th10AllScoreDataSetHeaderTestHelper<
                Th12Converter, Th12Converter.CharaWithTotal, Th12Converter.StageProgress>();

        [TestMethod]
        public void Th12AllScoreDataSetHeaderTestTwice()
            => Th10AllScoreDataSetHeaderTestTwiceHelper<
                Th12Converter, Th12Converter.CharaWithTotal, Th12Converter.StageProgress>();

        [TestMethod]
        public void Th12AllScoreDataSetClearDataTest()
            => Th10AllScoreDataSetClearDataTestHelper<
                Th12Converter, Th12Converter.CharaWithTotal, Th12Converter.StageProgress>(2, 0x45F4, 113);

        [TestMethod]
        public void Th12AllScoreDataSetClearDataTestTwice()
            => Th10AllScoreDataSetClearDataTestTwiceHelper<
                Th12Converter, Th12Converter.CharaWithTotal, Th12Converter.StageProgress>(2, 0x45F4, 113);

        [TestMethod]
        public void Th12AllScoreDataSetStatusTest() => TestUtils.Wrap(() =>
        {
            var chapter = ChapterWrapper.Create(Th10.StatusTests.MakeByteArray(Th12.StatusTests.ValidStub));
            var status = new ThScoreFileConverter.Models.Th12.Status(chapter.Target);

            var allScoreData = new Th10AllScoreDataWrapper<
                Th12Converter, Th12Converter.CharaWithTotal, Th12Converter.StageProgress>();
            allScoreData.Set(status);

            Assert.AreSame(status, allScoreData.Status);
        });

        [TestMethod]
        public void Th12AllScoreDataSetStatusTestTwice() => TestUtils.Wrap(() =>
        {
            var chapter = ChapterWrapper.Create(Th10.StatusTests.MakeByteArray(Th12.StatusTests.ValidStub));
            var status1 = new ThScoreFileConverter.Models.Th12.Status(chapter.Target);
            var status2 = new ThScoreFileConverter.Models.Th12.Status(chapter.Target);

            var allScoreData = new Th10AllScoreDataWrapper<
                Th12Converter, Th12Converter.CharaWithTotal, Th12Converter.StageProgress>();
            allScoreData.Set(status1);
            allScoreData.Set(status2);

            Assert.AreNotSame(status1, allScoreData.Status);
            Assert.AreSame(status2, allScoreData.Status);
        });

        #endregion
    }
}

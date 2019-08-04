using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ThScoreFileConverter.Models;
using ThScoreFileConverterTests.Models.Th095;
using ThScoreFileConverterTests.Models.Th095.Wrappers;
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

        internal static void Th10AllScoreDataSetHeaderTestHelper<TParent, TCharaWithTotal, TStageProgress>(
            string signature)
            where TParent : ThConverter
            where TCharaWithTotal : struct, Enum
            where TStageProgress : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var array = HeaderTests.MakeByteArray(HeaderTests.GetValidProperties(signature));
                var header = HeaderWrapper<TParent>.Create(array);

                var allScoreData = new Th10AllScoreDataWrapper<TParent, TCharaWithTotal, TStageProgress>();
                allScoreData.Set(header);

                Assert.AreSame(header.Target, allScoreData.Header.Target);
            });

        internal static void Th10AllScoreDataSetHeaderTestTwiceHelper<TParent, TCharaWithTotal, TStageProgress>(
            string signature)
            where TParent : ThConverter
            where TCharaWithTotal : struct, Enum
            where TStageProgress : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var array = HeaderTests.MakeByteArray(HeaderTests.GetValidProperties(signature));
                var header1 = HeaderWrapper<TParent>.Create(array);
                var header2 = HeaderWrapper<TParent>.Create(array);

                var allScoreData = new Th10AllScoreDataWrapper<TParent, TCharaWithTotal, TStageProgress>();
                allScoreData.Set(header1);
                allScoreData.Set(header2);

                Assert.AreNotSame(header1.Target, allScoreData.Header.Target);
                Assert.AreSame(header2.Target, allScoreData.Header.Target);
            });

        internal static void Th10AllScoreDataSetClearDataTestHelper<TParent, TCharaWithTotal, TStageProgress>(
            ushort version, int size, int numCards)
            where TParent : ThConverter
            where TCharaWithTotal : struct, Enum
            where TStageProgress : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var properties =
                    Th10ClearDataTests.GetValidProperties<TCharaWithTotal, TStageProgress>(version, size, numCards);
                var chapter = ChapterWrapper.Create(
                    Th10ClearDataTests.MakeByteArray<TParent, TCharaWithTotal, TStageProgress>(properties));
                var clearData = new Th10ClearDataWrapper<TParent, TCharaWithTotal, TStageProgress>(chapter);

                var allScoreData = new Th10AllScoreDataWrapper<TParent, TCharaWithTotal, TStageProgress>();
                allScoreData.Set(clearData);

                Assert.AreSame(clearData.Target, allScoreData.ClearDataItem(properties.chara).Target);
            });

        internal static void Th10AllScoreDataSetClearDataTestTwiceHelper<TParent, TCharaWithTotal, TStageProgress>(
            ushort version, int size, int numCards)
            where TParent : ThConverter
            where TCharaWithTotal : struct, Enum
            where TStageProgress : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var properties =
                    Th10ClearDataTests.GetValidProperties<TCharaWithTotal, TStageProgress>(version, size, numCards);
                var chapter = ChapterWrapper.Create(
                    Th10ClearDataTests.MakeByteArray<TParent, TCharaWithTotal, TStageProgress>(properties));
                var clearData1 = new Th10ClearDataWrapper<TParent, TCharaWithTotal, TStageProgress>(chapter);
                var clearData2 = new Th10ClearDataWrapper<TParent, TCharaWithTotal, TStageProgress>(chapter);

                var allScoreData = new Th10AllScoreDataWrapper<TParent, TCharaWithTotal, TStageProgress>();
                allScoreData.Set(clearData1);
                allScoreData.Set(clearData2);

                Assert.AreSame(clearData1.Target, allScoreData.ClearDataItem(properties.chara).Target);
                Assert.AreNotSame(clearData2.Target, allScoreData.ClearDataItem(properties.chara).Target);
            });

        internal static void Th10AllScoreDataSetStatusTestHelper<TParent, TCharaWithTotal, TStageProgress>(
            ushort version, int size, int numBgms)
            where TParent : ThConverter
            where TCharaWithTotal : struct, Enum
            where TStageProgress : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var chapter = ChapterWrapper.Create(
                    Th10StatusTests.MakeByteArray(Th10StatusTests.GetValidProperties(version, size, numBgms)));
                var status = new Th10StatusWrapper<TParent>(chapter);

                var allScoreData = new Th10AllScoreDataWrapper<TParent, TCharaWithTotal, TStageProgress>();
                allScoreData.Set(status);

                Assert.AreSame(status.Target, allScoreData.Status.Target);
            });

        internal static void Th10AllScoreDataSetStatusTestTwiceHelper<TParent, TCharaWithTotal, TStageProgress>(
            ushort version, int size, int numBgms)
            where TParent : ThConverter
            where TCharaWithTotal : struct, Enum
            where TStageProgress : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var chapter = ChapterWrapper.Create(
                    Th10StatusTests.MakeByteArray(Th10StatusTests.GetValidProperties(version, size, numBgms)));
                var status1 = new Th10StatusWrapper<TParent>(chapter);
                var status2 = new Th10StatusWrapper<TParent>(chapter);

                var allScoreData = new Th10AllScoreDataWrapper<TParent, TCharaWithTotal, TStageProgress>();
                allScoreData.Set(status1);
                allScoreData.Set(status2);

                Assert.AreNotSame(status1.Target, allScoreData.Status.Target);
                Assert.AreSame(status2.Target, allScoreData.Status.Target);
            });

        #region Th10

        [TestMethod]
        public void Th10AllScoreDataTest()
            => Th10AllScoreDataTestHelper<Th10Converter, Th10Converter.CharaWithTotal, Th10Converter.StageProgress>();

        [TestMethod]
        public void Th10AllScoreDataSetHeaderTest()
            => Th10AllScoreDataSetHeaderTestHelper<
                Th10Converter, Th10Converter.CharaWithTotal, Th10Converter.StageProgress>("TH10");

        [TestMethod]
        public void Th10AllScoreDataSetHeaderTestTwice()
            => Th10AllScoreDataSetHeaderTestTwiceHelper<
                Th10Converter, Th10Converter.CharaWithTotal, Th10Converter.StageProgress>("TH10");

        [TestMethod]
        public void Th10AllScoreDataSetClearDataTest()
            => Th10AllScoreDataSetClearDataTestHelper<
                Th10Converter, Th10Converter.CharaWithTotal, Th10Converter.StageProgress>(0, 0x437C, 110);

        [TestMethod]
        public void Th10AllScoreDataSetClearDataTestTwice()
            => Th10AllScoreDataSetClearDataTestTwiceHelper<
                Th10Converter, Th10Converter.CharaWithTotal, Th10Converter.StageProgress>(0, 0x437C, 110);

        [TestMethod]
        public void Th10AllScoreDataSetStatusTest()
            => Th10AllScoreDataSetStatusTestHelper<
                Th10Converter, Th10Converter.CharaWithTotal, Th10Converter.StageProgress>(0, 0x448, 18);

        [TestMethod]
        public void Th10AllScoreDataSetStatusTestTwice()
            => Th10AllScoreDataSetStatusTestTwiceHelper<
                Th10Converter, Th10Converter.CharaWithTotal, Th10Converter.StageProgress>(0, 0x448, 18);

        #endregion

        #region Th11

        [TestMethod]
        public void Th11AllScoreDataTest()
            => Th10AllScoreDataTestHelper<Th11Converter, Th11Converter.CharaWithTotal, Th11Converter.StageProgress>();

        [TestMethod]
        public void Th11AllScoreDataSetHeaderTest()
            => Th10AllScoreDataSetHeaderTestHelper<
                Th11Converter, Th11Converter.CharaWithTotal, Th11Converter.StageProgress>("TH11");

        [TestMethod]
        public void Th11AllScoreDataSetHeaderTestTwice()
            => Th10AllScoreDataSetHeaderTestTwiceHelper<
                Th11Converter, Th11Converter.CharaWithTotal, Th11Converter.StageProgress>("TH11");

        [TestMethod]
        public void Th11AllScoreDataSetClearDataTest()
            => Th10AllScoreDataSetClearDataTestHelper<
                Th11Converter, Th11Converter.CharaWithTotal, Th11Converter.StageProgress>(0, 0x68D4, 175);

        [TestMethod]
        public void Th11AllScoreDataSetClearDataTestTwice()
            => Th10AllScoreDataSetClearDataTestTwiceHelper<
                Th11Converter, Th11Converter.CharaWithTotal, Th11Converter.StageProgress>(0, 0x68D4, 175);

        [TestMethod]
        public void Th11AllScoreDataSetStatusTest()
            => Th10AllScoreDataSetStatusTestHelper<
                Th11Converter, Th11Converter.CharaWithTotal, Th11Converter.StageProgress>(0, 0x448, 17);

        [TestMethod]
        public void Th11AllScoreDataSetStatusTestTwice()
            => Th10AllScoreDataSetStatusTestTwiceHelper<
                Th11Converter, Th11Converter.CharaWithTotal, Th11Converter.StageProgress>(0, 0x448, 17);

        #endregion

        #region Th12

        [TestMethod]
        public void Th12AllScoreDataTest()
            => Th10AllScoreDataTestHelper<Th12Converter, Th12Converter.CharaWithTotal, Th12Converter.StageProgress>();

        [TestMethod]
        public void Th12AllScoreDataSetHeaderTest()
            => Th10AllScoreDataSetHeaderTestHelper<
                Th12Converter, Th12Converter.CharaWithTotal, Th12Converter.StageProgress>("TH21");

        [TestMethod]
        public void Th12AllScoreDataSetHeaderTestTwice()
            => Th10AllScoreDataSetHeaderTestTwiceHelper<
                Th12Converter, Th12Converter.CharaWithTotal, Th12Converter.StageProgress>("TH21");

        [TestMethod]
        public void Th12AllScoreDataSetClearDataTest()
            => Th10AllScoreDataSetClearDataTestHelper<
                Th12Converter, Th12Converter.CharaWithTotal, Th12Converter.StageProgress>(2, 0x45F4, 113);

        [TestMethod]
        public void Th12AllScoreDataSetClearDataTestTwice()
            => Th10AllScoreDataSetClearDataTestTwiceHelper<
                Th12Converter, Th12Converter.CharaWithTotal, Th12Converter.StageProgress>(2, 0x45F4, 113);

        [TestMethod]
        public void Th12AllScoreDataSetStatusTest()
            => Th10AllScoreDataSetStatusTestHelper<
                Th12Converter, Th12Converter.CharaWithTotal, Th12Converter.StageProgress>(2, 0x448, 17);

        [TestMethod]
        public void Th12AllScoreDataSetStatusTestTwice()
            => Th10AllScoreDataSetStatusTestTwiceHelper<
                Th12Converter, Th12Converter.CharaWithTotal, Th12Converter.StageProgress>(2, 0x448, 17);

        #endregion
    }
}

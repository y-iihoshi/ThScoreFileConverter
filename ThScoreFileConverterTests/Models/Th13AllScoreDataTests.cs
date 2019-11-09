using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th13;
using ThScoreFileConverterTests.Models.Th095;
using ThScoreFileConverterTests.Models.Th125.Stubs;
using ThScoreFileConverterTests.Models.Wrappers;
using ChapterWrapper = ThScoreFileConverterTests.Models.Th10.Wrappers.ChapterWrapper;
using HeaderBase = ThScoreFileConverter.Models.Th095.HeaderBase;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th13AllScoreDataTests
    {
        internal static void Th13AllScoreDataTestHelper<
            TParent, TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TStProg>()
            where TParent : ThConverter
            where TChWithT : struct, Enum       // TCharaWithTotal
            where TLv : struct, Enum            // TLevel
            where TLvPrac : struct, Enum        // TLevelPractice
            where TLvPracWithT : struct, Enum   // TLevelPracticeWithTotal
            where TStPrac : struct, Enum        // TStagePractice
            where TStProg : struct, Enum        // TStageProgress
            => TestUtils.Wrap(() =>
            {
                var allScoreData =
                    new Th13AllScoreDataWrapper<TParent, TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TStProg>();

                Assert.IsNull(allScoreData.Header);
                Assert.AreEqual(0, allScoreData.ClearDataCount);
                Assert.IsNull(allScoreData.Status);
            });

        internal static void Th13AllScoreDataSetHeaderTestHelper<
            TParent, TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TStProg>()
            where TParent : ThConverter
            where TChWithT : struct, Enum
            where TLv : struct, Enum
            where TLvPrac : struct, Enum
            where TLvPracWithT : struct, Enum
            where TStPrac : struct, Enum
            where TStProg : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var array = HeaderBaseTests.MakeByteArray(HeaderBaseTests.ValidProperties);
                var header = TestUtils.Create<HeaderBase>(array);

                var allScoreData =
                    new Th13AllScoreDataWrapper<TParent, TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TStProg>();
                allScoreData.Set(header);

                Assert.AreSame(header, allScoreData.Header);
            });

        internal static void Th13AllScoreDataSetHeaderTestTwiceHelper<
            TParent, TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TStProg>()
            where TParent : ThConverter
            where TChWithT : struct, Enum
            where TLv : struct, Enum
            where TLvPrac : struct, Enum
            where TLvPracWithT : struct, Enum
            where TStPrac : struct, Enum
            where TStProg : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var array = HeaderBaseTests.MakeByteArray(HeaderBaseTests.ValidProperties);
                var header1 = TestUtils.Create<HeaderBase>(array);
                var header2 = TestUtils.Create<HeaderBase>(array);

                var allScoreData =
                    new Th13AllScoreDataWrapper<TParent, TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TStProg>();
                allScoreData.Set(header1);
                allScoreData.Set(header2);

                Assert.AreNotSame(header1, allScoreData.Header);
                Assert.AreSame(header2, allScoreData.Header);
            });

        internal static void Th13AllScoreDataSetClearDataTestHelper<
            TParent, TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TStProg>(ushort version, int size, int numCards)
            where TParent : ThConverter
            where TChWithT : struct, Enum
            where TLv : struct, Enum
            where TLvPrac : struct, Enum
            where TLvPracWithT : struct, Enum
            where TStPrac : struct, Enum
            where TStProg : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var stub = Th13ClearDataTests.GetValidStub<
                    TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TStProg>(version, size, numCards);
                var chapter = ChapterWrapper.Create(Th13ClearDataTests.MakeByteArray<
                    TParent, TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TStProg>(stub));
                var clearData =
                    new Th13ClearDataWrapper<TParent, TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TStProg>(chapter);

                var allScoreData =
                    new Th13AllScoreDataWrapper<TParent, TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TStProg>();
                allScoreData.Set(clearData);

                Assert.AreSame(clearData.Target, allScoreData.ClearDataItem(stub.Chara).Target);
            });

        internal static void Th13AllScoreDataSetClearDataTestTwiceHelper<
            TParent, TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TStProg>(ushort version, int size, int numCards)
            where TParent : ThConverter
            where TChWithT : struct, Enum
            where TLv : struct, Enum
            where TLvPrac : struct, Enum
            where TLvPracWithT : struct, Enum
            where TStPrac : struct, Enum
            where TStProg : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var stub = Th13ClearDataTests.GetValidStub<
                    TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TStProg>(version, size, numCards);
                var chapter = ChapterWrapper.Create(Th13ClearDataTests.MakeByteArray<
                    TParent, TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TStProg>(stub));
                var clearData1 =
                    new Th13ClearDataWrapper<TParent, TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TStProg>(chapter);
                var clearData2 =
                    new Th13ClearDataWrapper<TParent, TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TStProg>(chapter);

                var allScoreData =
                    new Th13AllScoreDataWrapper<TParent, TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TStProg>();
                allScoreData.Set(clearData1);
                allScoreData.Set(clearData2);

                Assert.AreSame(clearData1.Target, allScoreData.ClearDataItem(stub.Chara).Target);
                Assert.AreNotSame(clearData2.Target, allScoreData.ClearDataItem(stub.Chara).Target);
            });

        internal static void Th13AllScoreDataSetStatusTestHelper<
            TParent, TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TStProg>(
            ushort version, int size, int numBgms, int gap1Size, int gap2Size)
            where TParent : ThConverter
            where TChWithT : struct, Enum
            where TLv : struct, Enum
            where TLvPrac : struct, Enum
            where TLvPracWithT : struct, Enum
            where TStPrac : struct, Enum
            where TStProg : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var status = new StatusStub();

                var allScoreData =
                    new Th13AllScoreDataWrapper<TParent, TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TStProg>();
                allScoreData.Set(status);

                Assert.AreSame(status, allScoreData.Status);
            });

        internal static void Th13AllScoreDataSetStatusTestTwiceHelper<
            TParent, TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TStProg>(
            ushort version, int size, int numBgms, int gap1Size, int gap2Size)
            where TParent : ThConverter
            where TChWithT : struct, Enum
            where TLv : struct, Enum
            where TLvPrac : struct, Enum
            where TLvPracWithT : struct, Enum
            where TStPrac : struct, Enum
            where TStProg : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var status1 = new StatusStub();
                var status2 = new StatusStub();

                var allScoreData =
                    new Th13AllScoreDataWrapper<TParent, TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TStProg>();
                allScoreData.Set(status1);
                allScoreData.Set(status2);

                Assert.AreNotSame(status1, allScoreData.Status);
                Assert.AreSame(status2, allScoreData.Status);
            });

        [TestMethod]
        public void Th13AllScoreDataTest()
            => Th13AllScoreDataTestHelper<
                Th13Converter,
                CharaWithTotal,
                LevelPractice,
                LevelPractice,
                LevelPracticeWithTotal,
                StagePractice,
                StageProgress>();

        [TestMethod]
        public void Th13AllScoreDataSetHeaderTest()
            => Th13AllScoreDataSetHeaderTestHelper<
                Th13Converter,
                CharaWithTotal,
                LevelPractice,
                LevelPractice,
                LevelPracticeWithTotal,
                StagePractice,
                StageProgress>();

        [TestMethod]
        public void Th13AllScoreDataSetHeaderTestTwice()
            => Th13AllScoreDataSetHeaderTestTwiceHelper<
                Th13Converter,
                CharaWithTotal,
                LevelPractice,
                LevelPractice,
                LevelPracticeWithTotal,
                StagePractice,
                StageProgress>();

        [TestMethod]
        public void Th13AllScoreDataSetClearDataTest()
            => Th13AllScoreDataSetClearDataTestHelper<
                Th13Converter,
                CharaWithTotal,
                LevelPractice,
                LevelPractice,
                LevelPracticeWithTotal,
                StagePractice,
                StageProgress>(1, 0x56DC, 127);

        [TestMethod]
        public void Th13AllScoreDataSetClearDataTestTwice()
            => Th13AllScoreDataSetClearDataTestTwiceHelper<
                Th13Converter,
                CharaWithTotal,
                LevelPractice,
                LevelPractice,
                LevelPracticeWithTotal,
                StagePractice,
                StageProgress>(1, 0x56DC, 127);

        [TestMethod]
        public void Th13AllScoreDataSetStatusTest()
            => Th13AllScoreDataSetStatusTestHelper<
                Th13Converter,
                CharaWithTotal,
                LevelPractice,
                LevelPractice,
                LevelPracticeWithTotal,
                StagePractice,
                StageProgress>(1, 0x42C, 17, 0x10, 0x11);

        [TestMethod]
        public void Th13AllScoreDataSetStatusTestTwice()
            => Th13AllScoreDataSetStatusTestTwiceHelper<
                Th13Converter,
                CharaWithTotal,
                LevelPractice,
                LevelPractice,
                LevelPracticeWithTotal,
                StagePractice,
                StageProgress>(1, 0x42C, 17, 0x10, 0x11);
    }
}

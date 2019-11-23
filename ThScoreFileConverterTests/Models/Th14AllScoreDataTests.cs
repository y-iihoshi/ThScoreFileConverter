using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th14;
using ThScoreFileConverterTests.Models.Th095;
using ThScoreFileConverterTests.Models.Th125.Stubs;
using ThScoreFileConverterTests.Models.Wrappers;
using ChapterWrapper = ThScoreFileConverterTests.Models.Th10.Wrappers.ChapterWrapper;
using HeaderBase = ThScoreFileConverter.Models.Th095.HeaderBase;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th14AllScoreDataTests
    {
        internal static void Th13AllScoreDataTestHelper<TParent, TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac>()
            where TParent : ThConverter
            where TChWithT : struct, Enum       // TCharaWithTotal
            where TLv : struct, Enum            // TLevel
            where TLvPrac : struct, Enum        // TLevelPractice
            where TLvPracWithT : struct, Enum   // TLevelPracticeWithTotal
            where TStPrac : struct, Enum        // TStagePractice
            => TestUtils.Wrap(() =>
            {
                var allScoreData =
                    new Th13AllScoreDataWrapper<TParent, TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac>();

                Assert.IsNull(allScoreData.Header);
                Assert.AreEqual(0, allScoreData.ClearDataCount);
                Assert.IsNull(allScoreData.Status);
            });

        internal static void Th13AllScoreDataSetHeaderTestHelper<
            TParent, TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac>()
            where TParent : ThConverter
            where TChWithT : struct, Enum
            where TLv : struct, Enum
            where TLvPrac : struct, Enum
            where TLvPracWithT : struct, Enum
            where TStPrac : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var array = HeaderBaseTests.MakeByteArray(HeaderBaseTests.ValidProperties);
                var header = TestUtils.Create<HeaderBase>(array);

                var allScoreData =
                    new Th13AllScoreDataWrapper<TParent, TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac>();
                allScoreData.Set(header);

                Assert.AreSame(header, allScoreData.Header);
            });

        internal static void Th13AllScoreDataSetHeaderTestTwiceHelper<
            TParent, TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac>()
            where TParent : ThConverter
            where TChWithT : struct, Enum
            where TLv : struct, Enum
            where TLvPrac : struct, Enum
            where TLvPracWithT : struct, Enum
            where TStPrac : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var array = HeaderBaseTests.MakeByteArray(HeaderBaseTests.ValidProperties);
                var header1 = TestUtils.Create<HeaderBase>(array);
                var header2 = TestUtils.Create<HeaderBase>(array);

                var allScoreData =
                    new Th13AllScoreDataWrapper<TParent, TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac>();
                allScoreData.Set(header1);
                allScoreData.Set(header2);

                Assert.AreNotSame(header1, allScoreData.Header);
                Assert.AreSame(header2, allScoreData.Header);
            });

        internal static void Th13AllScoreDataSetClearDataTestHelper<
            TParent, TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac>()
            where TParent : ThConverter
            where TChWithT : struct, Enum
            where TLv : struct, Enum
            where TLvPrac : struct, Enum
            where TLvPracWithT : struct, Enum
            where TStPrac : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var stub = Th14ClearDataTests.MakeValidStub<TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac>();
                var chapter = ChapterWrapper.Create(Th14ClearDataTests.MakeByteArray<
                    TParent, TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac>(stub));
                var clearData =
                    new Th13ClearDataWrapper<TParent, TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac>(chapter);

                var allScoreData =
                    new Th13AllScoreDataWrapper<TParent, TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac>();
                allScoreData.Set(clearData);

                Assert.AreSame(clearData.Target, allScoreData.ClearDataItem(stub.Chara).Target);
            });

        internal static void Th13AllScoreDataSetClearDataTestTwiceHelper<
            TParent, TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac>()
            where TParent : ThConverter
            where TChWithT : struct, Enum
            where TLv : struct, Enum
            where TLvPrac : struct, Enum
            where TLvPracWithT : struct, Enum
            where TStPrac : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var stub = Th14ClearDataTests.MakeValidStub<TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac>();
                var chapter = ChapterWrapper.Create(Th14ClearDataTests.MakeByteArray<
                    TParent, TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac>(stub));
                var clearData1 =
                    new Th13ClearDataWrapper<TParent, TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac>(chapter);
                var clearData2 =
                    new Th13ClearDataWrapper<TParent, TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac>(chapter);

                var allScoreData =
                    new Th13AllScoreDataWrapper<TParent, TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac>();
                allScoreData.Set(clearData1);
                allScoreData.Set(clearData2);

                Assert.AreSame(clearData1.Target, allScoreData.ClearDataItem(stub.Chara).Target);
                Assert.AreNotSame(clearData2.Target, allScoreData.ClearDataItem(stub.Chara).Target);
            });

        internal static void Th13AllScoreDataSetStatusTestHelper<
            TParent, TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac>()
            where TParent : ThConverter
            where TChWithT : struct, Enum
            where TLv : struct, Enum
            where TLvPrac : struct, Enum
            where TLvPracWithT : struct, Enum
            where TStPrac : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var status = new StatusStub();

                var allScoreData =
                    new Th13AllScoreDataWrapper<TParent, TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac>();
                allScoreData.Set(status);

                Assert.AreSame(status, allScoreData.Status);
            });

        internal static void Th13AllScoreDataSetStatusTestTwiceHelper<
            TParent, TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac>()
            where TParent : ThConverter
            where TChWithT : struct, Enum
            where TLv : struct, Enum
            where TLvPrac : struct, Enum
            where TLvPracWithT : struct, Enum
            where TStPrac : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var status1 = new StatusStub();
                var status2 = new StatusStub();

                var allScoreData =
                    new Th13AllScoreDataWrapper<TParent, TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac>();
                allScoreData.Set(status1);
                allScoreData.Set(status2);

                Assert.AreNotSame(status1, allScoreData.Status);
                Assert.AreSame(status2, allScoreData.Status);
            });

        [TestMethod]
        public void Th14AllScoreDataTest()
            => Th13AllScoreDataTestHelper<
                Th14Converter, CharaWithTotal, Level, LevelPractice, LevelPracticeWithTotal, StagePractice>();

        [TestMethod]
        public void Th14AllScoreDataSetHeaderTest()
            => Th13AllScoreDataSetHeaderTestHelper<
                Th14Converter, CharaWithTotal, Level, LevelPractice, LevelPracticeWithTotal, StagePractice>();

        [TestMethod]
        public void Th14AllScoreDataSetHeaderTestTwice()
            => Th13AllScoreDataSetHeaderTestTwiceHelper<
                Th14Converter, CharaWithTotal, Level, LevelPractice, LevelPracticeWithTotal, StagePractice>();

        [TestMethod]
        public void Th14AllScoreDataSetClearDataTest()
            => Th13AllScoreDataSetClearDataTestHelper<
                Th14Converter, CharaWithTotal, Level, LevelPractice, LevelPracticeWithTotal, StagePractice>();

        [TestMethod]
        public void Th14AllScoreDataSetClearDataTestTwice()
            => Th13AllScoreDataSetClearDataTestTwiceHelper<
                Th14Converter, CharaWithTotal, Level, LevelPractice, LevelPracticeWithTotal, StagePractice>();

        [TestMethod]
        public void Th14AllScoreDataSetStatusTest()
            => Th13AllScoreDataSetStatusTestHelper<
                Th14Converter, CharaWithTotal, Level, LevelPractice, LevelPracticeWithTotal, StagePractice>();

        [TestMethod]
        public void Th14AllScoreDataSetStatusTestTwice()
            => Th13AllScoreDataSetStatusTestTwiceHelper<
                Th14Converter, CharaWithTotal, Level, LevelPractice, LevelPracticeWithTotal, StagePractice>();
    }
}

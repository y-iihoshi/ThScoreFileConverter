using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th13;
using ThScoreFileConverterTests.Extensions;
using ThScoreFileConverterTests.Models.Th10.Stubs;
using ThScoreFileConverterTests.Models.Th10.Wrappers;
using ThScoreFileConverterTests.Models.Th13;
using ThScoreFileConverterTests.Models.Th13.Stubs;
using ThScoreFileConverterTests.Models.Wrappers;
using PracticeStub = ThScoreFileConverterTests.Models.Th13.Stubs.PracticeStub;
using ScoreDataTests = ThScoreFileConverterTests.Models.Th10.ScoreDataTests;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th13ClearDataTests
    {
        internal static ClearDataStub<TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TStProg>
            GetValidStub<TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TStProg>(
                ushort version, int size, int numCards)
            where TChWithT : struct, Enum       // TCharaWithTotal
            where TLv : struct, Enum            // TLevel
            where TLvPrac : struct, Enum        // TLevelPractice
            where TLvPracWithT : struct, Enum   // TLevelPracticeWithTotal
            where TStPrac : struct, Enum        // TStagePractice
            where TStProg : struct, Enum        // TStageProgress
        {
            var levels = Utils.GetEnumerator<TLvPrac>();
            var levelsWithTotal = Utils.GetEnumerator<TLvPracWithT>();
            var stages = Utils.GetEnumerator<TStPrac>();

            return new ClearDataStub<TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TStProg>
            {
                Signature = "CR",
                Version = version,
                Checksum = 0u,
                Size = size,
                Chara = TestUtils.Cast<TChWithT>(1),
                Rankings = levelsWithTotal.ToDictionary(
                    level => level,
                    level => Enumerable.Range(0, 10).Select(
                        index => new ScoreDataStub<TStProg>()
                        {
                            Score = 12345670u - (uint)index * 1000u,
                            StageProgress = TestUtils.Cast<TStProg>(5),
                            ContinueCount = (byte)index,
                            Name = TestUtils.MakeRandomArray<byte>(10),
                            DateTime = 34567890u,
                            SlowRate = 1.2f
                        }).ToList() as IReadOnlyList<ThScoreFileConverter.Models.Th10.IScoreData<TStProg>>),
                TotalPlayCount = 23,
                PlayTime = 4567890,
                ClearCounts = levelsWithTotal.ToDictionary(level => level, level => 100 - TestUtils.Cast<int>(level)),
                ClearFlags = levelsWithTotal.ToDictionary(level => level, level => TestUtils.Cast<int>(level) % 2),
                Practices = levels
                    .SelectMany(level => stages.Select(stage => (level, stage)))
                    .ToDictionary(
                        pair => pair,
                        pair => new PracticeStub()
                        {
                            Score = 123456u - TestUtils.Cast<uint>(pair.level) * 10u,
                            ClearFlag = (byte)(TestUtils.Cast<int>(pair.stage) % 2),
                            EnableFlag = (byte)(TestUtils.Cast<int>(pair.level) % 2)
                        } as IPractice),
                Cards = Enumerable.Range(1, numCards).ToDictionary(
                    index => index,
                    index => new SpellCardStub<TLv>()
                    {
                        Name = TestUtils.MakeRandomArray<byte>(0x80),
                        ClearCount = 12 + index,
                        PracticeClearCount = 34 + index,
                        TrialCount = 56 + index,
                        PracticeTrialCount = 78 + index,
                        Id = index,
                        Level = TestUtils.Cast<TLv>(2),
                        PracticeScore = 90123
                    } as ISpellCard<TLv>)
            };
        }

        internal static byte[] MakeData<TParent, TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TStProg>(
            IClearData<TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TStProg> clearData)
            where TParent : ThConverter
            where TChWithT : struct, Enum
            where TLv : struct, Enum
            where TLvPrac : struct, Enum
            where TLvPracWithT : struct, Enum
            where TStPrac : struct, Enum
            where TStProg : struct, Enum
            => TestUtils.MakeByteArray(
                TestUtils.Cast<int>(clearData.Chara),
                clearData.Rankings.Values.SelectMany(
                    ranking => ranking.SelectMany(
                        scoreData => ScoreDataTests.MakeByteArray<TParent, TStProg>(scoreData))).ToArray(),
                clearData.TotalPlayCount,
                clearData.PlayTime,
                clearData.ClearCounts.Values.ToArray(),
                clearData.ClearFlags.Values.ToArray(),
                clearData.Practices.Values.SelectMany(practice => PracticeTests.MakeByteArray(practice)).ToArray(),
                clearData.Cards.Values.SelectMany(card => SpellCardTests.MakeByteArray(card)).ToArray());

        internal static byte[] MakeByteArray<TParent, TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TStProg>(
            IClearData<TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TStProg> clearData)
            where TParent : ThConverter
            where TChWithT : struct, Enum
            where TLv : struct, Enum
            where TLvPrac : struct, Enum
            where TLvPracWithT : struct, Enum
            where TStPrac : struct, Enum
            where TStProg : struct, Enum
            => TestUtils.MakeByteArray(
                clearData.Signature.ToCharArray(),
                clearData.Version,
                clearData.Checksum,
                clearData.Size,
                MakeData<TParent, TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TStProg>(clearData));

        internal static void Validate<TParent, TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TStProg>(
            IClearData<TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TStProg> expected,
            in Th13ClearDataWrapper<TParent, TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TStProg> actual)
            where TParent : ThConverter
            where TChWithT : struct, Enum
            where TLv : struct, Enum
            where TLvPrac : struct, Enum
            where TLvPracWithT : struct, Enum
            where TStPrac : struct, Enum
            where TStProg : struct, Enum
        {
            var data = MakeData<TParent, TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TStProg>(expected);

            Assert.AreEqual(expected.Signature, actual.Signature);
            Assert.AreEqual(expected.Version, actual.Version);
            Assert.AreEqual(expected.Checksum, actual.Checksum);
            Assert.AreEqual(expected.Size, actual.Size);
            CollectionAssert.That.AreEqual(data, actual.Data);
            Assert.AreEqual(expected.Chara, actual.Chara);

            foreach (var pair in expected.Rankings)
            {
                for (var index = 0; index < pair.Value.Count(); ++index)
                {
                    ScoreDataTests.Validate(pair.Value[index], actual.Rankings[pair.Key][index]);
                }
            }

            Assert.AreEqual(expected.TotalPlayCount, actual.TotalPlayCount);
            Assert.AreEqual(expected.PlayTime, actual.PlayTime);
            CollectionAssert.That.AreEqual(expected.ClearCounts.Values, actual.ClearCounts.Values);
            CollectionAssert.That.AreEqual(expected.ClearFlags.Values, actual.ClearFlags.Values);

            foreach (var pair in expected.Practices)
            {
                PracticeTests.Validate(pair.Value, actual.Practices[pair.Key]);
            }

            foreach (var pair in expected.Cards)
            {
                SpellCardTests.Validate(pair.Value, actual.CardsItem(pair.Key));
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void
            ClearDataTestChapterHelper<TParent, TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TStProg>(
                ushort version, int size, int numCards)
            where TParent : ThConverter
            where TChWithT : struct, Enum
            where TLv : struct, Enum
            where TLvPrac : struct, Enum
            where TLvPracWithT : struct, Enum
            where TStPrac : struct, Enum
            where TStProg : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var stub =
                    GetValidStub<TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TStProg>(version, size, numCards);

                var chapter = ChapterWrapper.Create(
                    MakeByteArray<TParent, TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TStProg>(stub));
                var clearData =
                    new Th13ClearDataWrapper<TParent, TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TStProg>(chapter);

                Validate(stub, clearData);
                Assert.IsFalse(clearData.IsValid.Value);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void
            ClearDataTestNullChapterHelper<TParent, TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TStProg>()
            where TParent : ThConverter
            where TChWithT : struct, Enum
            where TLv : struct, Enum
            where TLvPrac : struct, Enum
            where TLvPracWithT : struct, Enum
            where TStPrac : struct, Enum
            where TStProg : struct, Enum
            => TestUtils.Wrap(() =>
            {
                _ = new Th13ClearDataWrapper<TParent, TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TStProg>(null);

                Assert.Fail(TestUtils.Unreachable);
            });

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        internal static void
            ClearDataTestInvalidSignatureHelper<TParent, TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TStProg>(
                ushort version, int size, int numCards)
            where TParent : ThConverter
            where TChWithT : struct, Enum
            where TLv : struct, Enum
            where TLvPrac : struct, Enum
            where TLvPracWithT : struct, Enum
            where TStPrac : struct, Enum
            where TStProg : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var stub =
                    GetValidStub<TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TStProg>(version, size, numCards);
                stub.Signature = stub.Signature.ToLowerInvariant();

                var chapter = ChapterWrapper.Create(
                    MakeByteArray<TParent, TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TStProg>(stub));
                _ = new Th13ClearDataWrapper<TParent, TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TStProg>(chapter);

                Assert.Fail(TestUtils.Unreachable);
            });

        internal static void
            ClearDataTestInvalidVersionHelper<TParent, TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TStProg>(
                ushort version, int size, int numCards)
            where TParent : ThConverter
            where TChWithT : struct, Enum
            where TLv : struct, Enum
            where TLvPrac : struct, Enum
            where TLvPracWithT : struct, Enum
            where TStPrac : struct, Enum
            where TStProg : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var stub =
                    GetValidStub<TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TStProg>(version, size, numCards);
                ++stub.Version;

                var chapter = ChapterWrapper.Create(
                    MakeByteArray<TParent, TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TStProg>(stub));
                _ = new Th13ClearDataWrapper<TParent, TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TStProg>(chapter);

                Assert.Fail(TestUtils.Unreachable);
            });

        internal static void
            ClearDataTestInvalidSizeHelper<TParent, TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TStProg>(
                ushort version, int size, int numCards)
            where TParent : ThConverter
            where TChWithT : struct, Enum
            where TLv : struct, Enum
            where TLvPrac : struct, Enum
            where TLvPracWithT : struct, Enum
            where TStPrac : struct, Enum
            where TStProg : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var stub =
                    GetValidStub<TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TStProg>(version, size, numCards);
                --stub.Size;

                var chapter = ChapterWrapper.Create(
                    MakeByteArray<TParent, TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TStProg>(stub));
                _ = new Th13ClearDataWrapper<TParent, TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TStProg>(chapter);

                Assert.Fail(TestUtils.Unreachable);
            });

        internal static void
            CanInitializeTestHelper<TParent, TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TStProg>(
                string signature, ushort version, int size, bool expected)
            where TParent : ThConverter
            where TChWithT : struct, Enum
            where TLv : struct, Enum
            where TLvPrac : struct, Enum
            where TLvPracWithT : struct, Enum
            where TStPrac : struct, Enum
            where TStProg : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var checksum = 0u;
                var data = new byte[size];

                var chapter = ChapterWrapper.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), version, checksum, size, data));

                Assert.AreEqual(
                    expected,
                    Th13ClearDataWrapper<TParent, TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TStProg>.CanInitialize(
                        chapter));
            });

        #region Th13

        [TestMethod]
        public void Th13ClearDataTestChapter()
            => ClearDataTestChapterHelper<
                Th13Converter,
                Th13Converter.CharaWithTotal,
                Th13Converter.LevelPractice,
                Th13Converter.LevelPractice,
                Th13Converter.LevelPracticeWithTotal,
                Th13Converter.StagePractice,
                Th13Converter.StageProgress>(1, 0x56DC, 127);

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th13ClearDataTestNullChapter()
            => ClearDataTestNullChapterHelper<
                Th13Converter,
                Th13Converter.CharaWithTotal,
                Th13Converter.LevelPractice,
                Th13Converter.LevelPractice,
                Th13Converter.LevelPracticeWithTotal,
                Th13Converter.StagePractice,
                Th13Converter.StageProgress>();

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th13ClearDataTestInvalidSignature()
            => ClearDataTestInvalidSignatureHelper<
                Th13Converter,
                Th13Converter.CharaWithTotal,
                Th13Converter.LevelPractice,
                Th13Converter.LevelPractice,
                Th13Converter.LevelPracticeWithTotal,
                Th13Converter.StagePractice,
                Th13Converter.StageProgress>(1, 0x56DC, 127);

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th13ClearDataTestInvalidVersion()
            => ClearDataTestInvalidVersionHelper<
                Th13Converter,
                Th13Converter.CharaWithTotal,
                Th13Converter.LevelPractice,
                Th13Converter.LevelPractice,
                Th13Converter.LevelPracticeWithTotal,
                Th13Converter.StagePractice,
                Th13Converter.StageProgress>(1, 0x56DC, 127);

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th13ClearDataTestInvalidSize()
            => ClearDataTestInvalidSizeHelper<
                Th13Converter,
                Th13Converter.CharaWithTotal,
                Th13Converter.LevelPractice,
                Th13Converter.LevelPractice,
                Th13Converter.LevelPracticeWithTotal,
                Th13Converter.StagePractice,
                Th13Converter.StageProgress>(1, 0x56DC, 127);

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow("CR", (ushort)1, 0x56DC, true)]
        [DataRow("cr", (ushort)1, 0x56DC, false)]
        [DataRow("CR", (ushort)0, 0x56DC, false)]
        [DataRow("CR", (ushort)1, 0x56DD, false)]
        public void Th13ClearDataCanInitializeTest(string signature, ushort version, int size, bool expected)
            => CanInitializeTestHelper<
                Th13Converter,
                Th13Converter.CharaWithTotal,
                Th13Converter.LevelPractice,
                Th13Converter.LevelPractice,
                Th13Converter.LevelPracticeWithTotal,
                Th13Converter.StagePractice,
                Th13Converter.StageProgress>(
                signature, version, size, expected);

        #endregion

        #region Th14

        [TestMethod]
        public void Th14ClearDataTestChapter()
            => ClearDataTestChapterHelper<
                Th14Converter,
                Th14Converter.CharaWithTotal,
                Level,
                Th14Converter.LevelPractice,
                Th14Converter.LevelPracticeWithTotal,
                Th14Converter.StagePractice,
                Th14Converter.StageProgress>(1, 0x5298, 120);

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th14ClearDataTestNullChapter()
            => ClearDataTestNullChapterHelper<
                Th14Converter,
                Th14Converter.CharaWithTotal,
                Level,
                Th14Converter.LevelPractice,
                Th14Converter.LevelPracticeWithTotal,
                Th14Converter.StagePractice,
                Th14Converter.StageProgress>();

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th14ClearDataTestInvalidSignature()
            => ClearDataTestInvalidSignatureHelper<
                Th14Converter,
                Th14Converter.CharaWithTotal,
                Level,
                Th14Converter.LevelPractice,
                Th14Converter.LevelPracticeWithTotal,
                Th14Converter.StagePractice,
                Th14Converter.StageProgress>(1, 0x5298, 120);

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th14ClearDataTestInvalidVersion()
            => ClearDataTestInvalidVersionHelper<
                Th14Converter,
                Th14Converter.CharaWithTotal,
                Level,
                Th14Converter.LevelPractice,
                Th14Converter.LevelPracticeWithTotal,
                Th14Converter.StagePractice,
                Th14Converter.StageProgress>(1, 0x5298, 120);

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th14ClearDataTestInvalidSize()
            => ClearDataTestInvalidSizeHelper<
                Th14Converter,
                Th14Converter.CharaWithTotal,
                Level,
                Th14Converter.LevelPractice,
                Th14Converter.LevelPracticeWithTotal,
                Th14Converter.StagePractice,
                Th14Converter.StageProgress>(1, 0x5298, 120);

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow("CR", (ushort)1, 0x5298, true)]
        [DataRow("cr", (ushort)1, 0x5298, false)]
        [DataRow("CR", (ushort)0, 0x5298, false)]
        [DataRow("CR", (ushort)1, 0x5299, false)]
        public void Th14ClearDataCanInitializeTest(string signature, ushort version, int size, bool expected)
            => CanInitializeTestHelper<
                Th14Converter,
                Th14Converter.CharaWithTotal,
                Level,
                Th14Converter.LevelPractice,
                Th14Converter.LevelPracticeWithTotal,
                Th14Converter.StagePractice,
                Th14Converter.StageProgress>(
                signature, version, size, expected);

        #endregion
    }
}

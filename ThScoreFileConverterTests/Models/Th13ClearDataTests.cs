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
using ClearDataStub = ThScoreFileConverterTests.Models.Th13.Stubs.ClearDataStub<
    ThScoreFileConverter.Models.Th13.CharaWithTotal,
    ThScoreFileConverter.Models.Th13.LevelPractice,
    ThScoreFileConverter.Models.Th13.LevelPractice,
    ThScoreFileConverter.Models.Th13.LevelPracticeWithTotal,
    ThScoreFileConverter.Models.Th13.StagePractice>;
using IClearData = ThScoreFileConverter.Models.Th13.IClearData<
    ThScoreFileConverter.Models.Th13.CharaWithTotal,
    ThScoreFileConverter.Models.Th13.LevelPractice,
    ThScoreFileConverter.Models.Th13.LevelPractice,
    ThScoreFileConverter.Models.Th13.LevelPracticeWithTotal,
    ThScoreFileConverter.Models.Th13.StagePractice>;
using PracticeStub = ThScoreFileConverterTests.Models.Th13.Stubs.PracticeStub;
using ScoreDataTests = ThScoreFileConverterTests.Models.Th10.ScoreDataTests;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th13ClearDataTests
    {
        internal static ClearDataStub
            GetValidStub(ushort version, int size, int numCards)
        {
            var levels = Utils.GetEnumerator<LevelPractice>();
            var levelsWithTotal = Utils.GetEnumerator<LevelPracticeWithTotal>();
            var stages = Utils.GetEnumerator<StagePractice>();

            return new ClearDataStub
            {
                Signature = "CR",
                Version = version,
                Checksum = 0u,
                Size = size,
                Chara = CharaWithTotal.Marisa,
                Rankings = levelsWithTotal.ToDictionary(
                    level => level,
                    level => Enumerable.Range(0, 10).Select(
                        index => new ScoreDataStub<StageProgress>()
                        {
                            Score = 12345670u - (uint)index * 1000u,
                            StageProgress = StageProgress.Five,
                            ContinueCount = (byte)index,
                            Name = TestUtils.MakeRandomArray<byte>(10),
                            DateTime = 34567890u,
                            SlowRate = 1.2f
                        }).ToList() as IReadOnlyList<ThScoreFileConverter.Models.Th10.IScoreData<StageProgress>>),
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
                    index => new SpellCardStub<LevelPractice>()
                    {
                        Name = TestUtils.MakeRandomArray<byte>(0x80),
                        ClearCount = 12 + index,
                        PracticeClearCount = 34 + index,
                        TrialCount = 56 + index,
                        PracticeTrialCount = 78 + index,
                        Id = index,
                        Level = LevelPractice.Hard,
                        PracticeScore = 90123
                    } as ISpellCard<LevelPractice>)
            };
        }

        internal static byte[] MakeData(IClearData clearData)
            => TestUtils.MakeByteArray(
                TestUtils.Cast<int>(clearData.Chara),
                clearData.Rankings.Values.SelectMany(
                    ranking => ranking.SelectMany(
                        scoreData => ScoreDataTests.MakeByteArray<Th13Converter, StageProgress>(scoreData))).ToArray(),
                clearData.TotalPlayCount,
                clearData.PlayTime,
                clearData.ClearCounts.Values.ToArray(),
                clearData.ClearFlags.Values.ToArray(),
                clearData.Practices.Values.SelectMany(practice => PracticeTests.MakeByteArray(practice)).ToArray(),
                clearData.Cards.Values.SelectMany(card => SpellCardTests.MakeByteArray(card)).ToArray());

        internal static byte[] MakeByteArray(IClearData clearData)
            => TestUtils.MakeByteArray(
                clearData.Signature.ToCharArray(),
                clearData.Version,
                clearData.Checksum,
                clearData.Size,
                MakeData(clearData));

        internal static void Validate<TParent, TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac>(
            IClearData expected,
            in Th13ClearDataWrapper<TParent, TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac> actual)
            where TParent : ThConverter
            where TChWithT : struct, Enum
            where TLv : struct, Enum
            where TLvPrac : struct, Enum
            where TLvPracWithT : struct, Enum
            where TStPrac : struct, Enum
        {
            var data = MakeData(expected);

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
                    ScoreDataTests.Validate(
                        pair.Value[index], actual.Rankings[TestUtils.Cast<TLvPracWithT>(pair.Key)][index]);
                }
            }

            Assert.AreEqual(expected.TotalPlayCount, actual.TotalPlayCount);
            Assert.AreEqual(expected.PlayTime, actual.PlayTime);
            CollectionAssert.That.AreEqual(expected.ClearCounts.Values, actual.ClearCounts.Values);
            CollectionAssert.That.AreEqual(expected.ClearFlags.Values, actual.ClearFlags.Values);

            foreach (var pair in expected.Practices)
            {
                var key = (TestUtils.Cast<TLvPrac>(pair.Key.Item1), TestUtils.Cast<TStPrac>(pair.Key.Item2));
                PracticeTests.Validate(pair.Value, actual.Practices[key]);
            }

            foreach (var pair in expected.Cards)
            {
                SpellCardTests.Validate(pair.Value, actual.Cards[pair.Key] as ISpellCard<LevelPractice>);
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        [TestMethod]
        public void Th13ClearDataTestChapter()
            => ClearDataTestChapterHelper<
                Th13Converter,
                CharaWithTotal,
                LevelPractice,
                LevelPractice,
                LevelPracticeWithTotal,
                StagePractice>(1, 0x56DC, 127);
        internal static void
            ClearDataTestChapterHelper<TParent, TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac>(
                ushort version, int size, int numCards)
            where TParent : ThConverter
            where TChWithT : struct, Enum
            where TLv : struct, Enum
            where TLvPrac : struct, Enum
            where TLvPracWithT : struct, Enum
            where TStPrac : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var stub = GetValidStub(version, size, numCards);

                var chapter = ChapterWrapper.Create(MakeByteArray(stub));
                var clearData =
                    new Th13ClearDataWrapper<TParent, TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac>(chapter);

                Validate(stub, clearData);
                Assert.IsFalse(clearData.IsValid.Value);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th13ClearDataTestNullChapter()
            => ClearDataTestNullChapterHelper<
                Th13Converter,
                CharaWithTotal,
                LevelPractice,
                LevelPractice,
                LevelPracticeWithTotal,
                StagePractice>();
        internal static void ClearDataTestNullChapterHelper<TParent, TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac>()
            where TParent : ThConverter
            where TChWithT : struct, Enum
            where TLv : struct, Enum
            where TLvPrac : struct, Enum
            where TLvPracWithT : struct, Enum
            where TStPrac : struct, Enum
            => TestUtils.Wrap(() =>
            {
                _ = new Th13ClearDataWrapper<TParent, TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac>(null);

                Assert.Fail(TestUtils.Unreachable);
            });

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th13ClearDataTestInvalidSignature()
            => ClearDataTestInvalidSignatureHelper<
                Th13Converter,
                CharaWithTotal,
                LevelPractice,
                LevelPractice,
                LevelPracticeWithTotal,
                StagePractice>(1, 0x56DC, 127);
        internal static void
            ClearDataTestInvalidSignatureHelper<TParent, TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac>(
                ushort version, int size, int numCards)
            where TParent : ThConverter
            where TChWithT : struct, Enum
            where TLv : struct, Enum
            where TLvPrac : struct, Enum
            where TLvPracWithT : struct, Enum
            where TStPrac : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var stub = GetValidStub(version, size, numCards);
                stub.Signature = stub.Signature.ToLowerInvariant();

                var chapter = ChapterWrapper.Create(MakeByteArray(stub));
                _ = new Th13ClearDataWrapper<TParent, TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac>(chapter);

                Assert.Fail(TestUtils.Unreachable);
            });

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th13ClearDataTestInvalidVersion()
            => ClearDataTestInvalidVersionHelper<
                Th13Converter,
                CharaWithTotal,
                LevelPractice,
                LevelPractice,
                LevelPracticeWithTotal,
                StagePractice>(1, 0x56DC, 127);
        internal static void
            ClearDataTestInvalidVersionHelper<TParent, TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac>(
                ushort version, int size, int numCards)
            where TParent : ThConverter
            where TChWithT : struct, Enum
            where TLv : struct, Enum
            where TLvPrac : struct, Enum
            where TLvPracWithT : struct, Enum
            where TStPrac : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var stub = GetValidStub(version, size, numCards);
                ++stub.Version;

                var chapter = ChapterWrapper.Create(MakeByteArray(stub));
                _ = new Th13ClearDataWrapper<TParent, TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac>(chapter);

                Assert.Fail(TestUtils.Unreachable);
            });

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th13ClearDataTestInvalidSize()
            => ClearDataTestInvalidSizeHelper<
                Th13Converter,
                CharaWithTotal,
                LevelPractice,
                LevelPractice,
                LevelPracticeWithTotal,
                StagePractice>(1, 0x56DC, 127);
        internal static void
            ClearDataTestInvalidSizeHelper<TParent, TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac>(
                ushort version, int size, int numCards)
            where TParent : ThConverter
            where TChWithT : struct, Enum
            where TLv : struct, Enum
            where TLvPrac : struct, Enum
            where TLvPracWithT : struct, Enum
            where TStPrac : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var stub = GetValidStub(version, size, numCards);
                --stub.Size;

                var chapter = ChapterWrapper.Create(MakeByteArray(stub));
                _ = new Th13ClearDataWrapper<TParent, TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac>(chapter);

                Assert.Fail(TestUtils.Unreachable);
            });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow("CR", (ushort)1, 0x56DC, true)]
        [DataRow("cr", (ushort)1, 0x56DC, false)]
        [DataRow("CR", (ushort)0, 0x56DC, false)]
        [DataRow("CR", (ushort)1, 0x56DD, false)]
        public void CanInitializeTest(string signature, ushort version, int size, bool expected) => TestUtils.Wrap(() =>
        {
            var checksum = 0u;
            var data = new byte[size];

            var chapter = ChapterWrapper.Create(
                TestUtils.MakeByteArray(signature.ToCharArray(), version, checksum, size, data));

            Assert.AreEqual(
                expected,
                Th13ClearDataWrapper<
                    Th13Converter,
                    CharaWithTotal,
                    LevelPractice,
                    LevelPractice,
                    LevelPracticeWithTotal,
                    StagePractice>.CanInitialize(chapter));
        });
    }
}

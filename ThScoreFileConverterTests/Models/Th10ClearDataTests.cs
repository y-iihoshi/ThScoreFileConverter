using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th10;
using ThScoreFileConverterTests.Extensions;
using ThScoreFileConverterTests.Models.Th10;
using ThScoreFileConverterTests.Models.Th10.Stubs;
using ThScoreFileConverterTests.Models.Th10.Wrappers;
using ThScoreFileConverterTests.Models.Wrappers;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th10ClearDataTests
    {
        internal static ClearDataStub<TCharaWithTotal, TStageProgress>
            GetValidStub<TCharaWithTotal, TStageProgress>(ushort version, int size, int numCards)
            where TCharaWithTotal : struct, Enum
            where TStageProgress : struct, Enum
        {
            var levels = Utils.GetEnumerator<Level>();
            var levelsExceptExtra = levels.Where(level => level != Level.Extra);
            var stages = Utils.GetEnumerator<Stage>();
            var stagesExceptExtra = stages.Where(stage => stage != Stage.Extra);

            return new ClearDataStub<TCharaWithTotal, TStageProgress>()
            {
                Signature = "CR",
                Version = version,
                Checksum = 0u,
                Size = size,
                Chara = TestUtils.Cast<TCharaWithTotal>(1),
                Rankings = levels.ToDictionary(
                    level => level,
                    level => Enumerable.Range(0, 10).Select(
                        index => new ScoreDataStub<TStageProgress>()
                        {
                            Score = 12345670u - (uint)index * 1000u,
                            StageProgress = TestUtils.Cast<TStageProgress>(5),
                            ContinueCount = (byte)index,
                            Name = TestUtils.MakeRandomArray<byte>(10),
                            DateTime = 34567890u,
                            SlowRate = 1.2f
                        }).ToList() as IReadOnlyList<IScoreData<TStageProgress>>),
                TotalPlayCount = 23,
                PlayTime = 4567890,
                ClearCounts = levels.ToDictionary(level => level, level => 100 - (int)level),
                Practices = levelsExceptExtra
                    .SelectMany(level => stagesExceptExtra.Select(stage => (level, stage)))
                    .ToDictionary(
                        pair => pair,
                        pair => new PracticeStub()
                        {
                            Score = 123456u - (uint)pair.level * 10u,
                            StageFlag = (uint)pair.stage % 2u
                        } as IPractice),
                Cards = Enumerable.Range(1, numCards).ToDictionary(
                    index => index,
                    index => new SpellCardStub()
                    {
                        Name = TestUtils.MakeRandomArray<byte>(0x80),
                        ClearCount = 123 + index,
                        TrialCount = 456 + index,
                        Id = index,
                        Level = Level.Hard
                    } as ISpellCard<Level>)
            };
        }

        internal static byte[] MakeData<TParent, TCharaWithTotal, TStageProgress>(
            IClearData<TCharaWithTotal, TStageProgress> clearData)
            where TParent : ThConverter
            where TCharaWithTotal : struct, Enum
            where TStageProgress : struct, Enum
            => TestUtils.MakeByteArray(
                TestUtils.Cast<int>(clearData.Chara),
                clearData.Rankings.Values.SelectMany(
                    ranking => ranking.SelectMany(
                        scoreData => ScoreDataTests.MakeByteArray<TParent, TStageProgress>(scoreData))).ToArray(),
                clearData.TotalPlayCount,
                clearData.PlayTime,
                clearData.ClearCounts.Values.ToArray(),
                clearData.Practices.Values.SelectMany(
                    practice => PracticeTests.MakeByteArray(practice)).ToArray(),
                clearData.Cards.Values.SelectMany(
                    card => SpellCardTests.MakeByteArray(card)).ToArray());

        internal static byte[] MakeByteArray<TParent, TCharaWithTotal, TStageProgress>(
            IClearData<TCharaWithTotal, TStageProgress> clearData)
            where TParent : ThConverter
            where TCharaWithTotal : struct, Enum
            where TStageProgress : struct, Enum
            => TestUtils.MakeByteArray(
                clearData.Signature.ToCharArray(),
                clearData.Version,
                clearData.Checksum,
                clearData.Size,
                MakeData<TParent, TCharaWithTotal, TStageProgress>(clearData));

        internal static void Validate<TParent, TCharaWithTotal, TStageProgress>(
            IClearData<TCharaWithTotal, TStageProgress> expected,
            in Th10ClearDataWrapper<TParent, TCharaWithTotal, TStageProgress> actual)
            where TParent : ThConverter
            where TCharaWithTotal : struct, Enum
            where TStageProgress : struct, Enum
        {
            var data = MakeData<TParent, TCharaWithTotal, TStageProgress>(expected);

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
                    ScoreDataTests.Validate(pair.Value[index], actual.RankingItem(pair.Key, index));
                }
            }

            Assert.AreEqual(expected.TotalPlayCount, actual.TotalPlayCount);
            Assert.AreEqual(expected.PlayTime, actual.PlayTime);
            CollectionAssert.That.AreEqual(expected.ClearCounts.Values, actual.ClearCounts.Values);

            foreach (var pair in expected.Practices)
            {
                PracticeTests.Validate(pair.Value, actual.Practices[pair.Key]);
            }

            foreach (var pair in expected.Cards)
            {
                SpellCardTests.Validate(pair.Value, actual.Cards[pair.Key]);
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ClearDataTestChapterHelper<TParent, TCharaWithTotal, TStageProgress>(
            ushort version, int size, int numCards)
            where TParent : ThConverter
            where TCharaWithTotal : struct, Enum
            where TStageProgress : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var stub = GetValidStub<TCharaWithTotal, TStageProgress>(version, size, numCards);

                var chapter = ChapterWrapper.Create(
                    MakeByteArray<TParent, TCharaWithTotal, TStageProgress>(stub));
                var clearData = new Th10ClearDataWrapper<TParent, TCharaWithTotal, TStageProgress>(chapter);

                Validate(stub, clearData);
                Assert.IsFalse(clearData.IsValid.Value);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ClearDataTestNullChapterHelper<TParent, TCharaWithTotal, TStageProgress>()
            where TParent : ThConverter
            where TCharaWithTotal : struct, Enum
            where TStageProgress : struct, Enum
            => TestUtils.Wrap(() =>
            {
                _ = new Th10ClearDataWrapper<TParent, TCharaWithTotal, TStageProgress>(null);

                Assert.Fail(TestUtils.Unreachable);
            });

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        internal static void ClearDataTestInvalidSignatureHelper<TParent, TCharaWithTotal, TStageProgress>(
            ushort version, int size, int numCards)
            where TParent : ThConverter
            where TCharaWithTotal : struct, Enum
            where TStageProgress : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var stub = GetValidStub<TCharaWithTotal, TStageProgress>(version, size, numCards);
                stub.Signature = stub.Signature.ToLowerInvariant();

                var chapter = ChapterWrapper.Create(
                    MakeByteArray<TParent, TCharaWithTotal, TStageProgress>(stub));
                _ = new Th10ClearDataWrapper<TParent, TCharaWithTotal, TStageProgress>(chapter);

                Assert.Fail(TestUtils.Unreachable);
            });

        internal static void ClearDataTestInvalidVersionHelper<TParent, TCharaWithTotal, TStageProgress>(
            ushort version, int size, int numCards)
            where TParent : ThConverter
            where TCharaWithTotal : struct, Enum
            where TStageProgress : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var stub = GetValidStub<TCharaWithTotal, TStageProgress>(version, size, numCards);
                ++stub.Version;

                var chapter = ChapterWrapper.Create(
                    MakeByteArray<TParent, TCharaWithTotal, TStageProgress>(stub));
                _ = new Th10ClearDataWrapper<TParent, TCharaWithTotal, TStageProgress>(chapter);

                Assert.Fail(TestUtils.Unreachable);
            });

        internal static void ClearDataTestInvalidSizeHelper<TParent, TCharaWithTotal, TStageProgress>(
            ushort version, int size, int numCards)
            where TParent : ThConverter
            where TCharaWithTotal : struct, Enum
            where TStageProgress : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var stub = GetValidStub<TCharaWithTotal, TStageProgress>(version, size, numCards);
                --stub.Size;

                var chapter = ChapterWrapper.Create(
                    MakeByteArray<TParent, TCharaWithTotal, TStageProgress>(stub));
                _ = new Th10ClearDataWrapper<TParent, TCharaWithTotal, TStageProgress>(chapter);

                Assert.Fail(TestUtils.Unreachable);
            });

        internal static void CanInitializeTestHelper<TParent, TCharaWithTotal, TStageProgress>(
            string signature, ushort version, int size, bool expected)
            where TParent : ThConverter
            where TCharaWithTotal : struct, Enum
            where TStageProgress : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var checksum = 0u;
                var data = new byte[size];

                var chapter = ChapterWrapper.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), version, checksum, size, data));

                Assert.AreEqual(
                    expected, Th10ClearDataWrapper<TParent, TCharaWithTotal, TStageProgress>.CanInitialize(chapter));
            });

        #region Th10

        [TestMethod]
        public void Th10ClearDataTestChapter()
            => ClearDataTestChapterHelper<
                Th10Converter, Th10Converter.CharaWithTotal, Th10Converter.StageProgress>(0, 0x437C, 110);

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th10ClearDataTestNullChapter()
            => ClearDataTestNullChapterHelper<
                Th10Converter, Th10Converter.CharaWithTotal, Th10Converter.StageProgress>();

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th10ClearDataTestInvalidSignature()
            => ClearDataTestInvalidSignatureHelper<
                Th10Converter, Th10Converter.CharaWithTotal, Th10Converter.StageProgress>(0, 0x437C, 110);

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th10ClearDataTestInvalidVersion()
            => ClearDataTestInvalidVersionHelper<
                Th10Converter, Th10Converter.CharaWithTotal, Th10Converter.StageProgress>(0, 0x437C, 110);

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th10ClearDataTestInvalidSize()
            => ClearDataTestInvalidSizeHelper<
                Th10Converter, Th10Converter.CharaWithTotal, Th10Converter.StageProgress>(0, 0x437C, 110);

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow("CR", (ushort)0, 0x437C, true)]
        [DataRow("cr", (ushort)0, 0x437C, false)]
        [DataRow("CR", (ushort)1, 0x437C, false)]
        [DataRow("CR", (ushort)0, 0x437D, false)]
        public void Th10ClearDataCanInitializeTest(string signature, ushort version, int size, bool expected)
            => CanInitializeTestHelper<
                Th10Converter, Th10Converter.CharaWithTotal, Th10Converter.StageProgress>(
                signature, version, size, expected);

        #endregion

        #region Th11

        [TestMethod]
        public void Th11ClearDataTestChapter()
            => ClearDataTestChapterHelper<
                Th11Converter, Th11Converter.CharaWithTotal, Th11Converter.StageProgress>(0, 0x68D4, 175);

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th11ClearDataTestNullChapter()
            => ClearDataTestNullChapterHelper<
                Th11Converter, Th11Converter.CharaWithTotal, Th11Converter.StageProgress>();

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th11ClearDataTestInvalidSignature()
            => ClearDataTestInvalidSignatureHelper<
                Th11Converter, Th11Converter.CharaWithTotal, Th11Converter.StageProgress>(0, 0x68D4, 175);

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th11ClearDataTestInvalidVersion()
            => ClearDataTestInvalidVersionHelper<
                Th11Converter, Th11Converter.CharaWithTotal, Th11Converter.StageProgress>(0, 0x68D4, 175);

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th11ClearDataTestInvalidSize()
            => ClearDataTestInvalidSizeHelper<
                Th11Converter, Th11Converter.CharaWithTotal, Th11Converter.StageProgress>(0, 0x68D4, 175);

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow("CR", (ushort)0, 0x68D4, true)]
        [DataRow("cr", (ushort)0, 0x68D4, false)]
        [DataRow("CR", (ushort)1, 0x68D4, false)]
        [DataRow("CR", (ushort)0, 0x68D5, false)]
        public void Th11ClearDataCanInitializeTest(string signature, ushort version, int size, bool expected)
            => CanInitializeTestHelper<
                Th11Converter, Th11Converter.CharaWithTotal, Th11Converter.StageProgress>(
                signature, version, size, expected);

        #endregion

        #region Th12

        [TestMethod]
        public void Th12ClearDataTestChapter()
            => ClearDataTestChapterHelper<
                Th12Converter, Th12Converter.CharaWithTotal, Th12Converter.StageProgress>(2, 0x45F4, 113);

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th12ClearDataTestNullChapter()
            => ClearDataTestNullChapterHelper<
                Th12Converter, Th12Converter.CharaWithTotal, Th12Converter.StageProgress>();

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th12ClearDataTestInvalidSignature()
            => ClearDataTestInvalidSignatureHelper<
                Th12Converter, Th12Converter.CharaWithTotal, Th12Converter.StageProgress>(2, 0x45F4, 113);

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th12ClearDataTestInvalidVersion()
            => ClearDataTestInvalidVersionHelper<
                Th12Converter, Th12Converter.CharaWithTotal, Th12Converter.StageProgress>(2, 0x45F4, 113);

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th12ClearDataTestInvalidSize()
            => ClearDataTestInvalidSizeHelper<
                Th12Converter, Th12Converter.CharaWithTotal, Th12Converter.StageProgress>(2, 0x45F4, 113);

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow("CR", (ushort)2, 0x45F4, true)]
        [DataRow("cr", (ushort)2, 0x45F4, false)]
        [DataRow("CR", (ushort)1, 0x45F4, false)]
        [DataRow("CR", (ushort)2, 0x45F5, false)]
        public void Th12ClearDataCanInitializeTest(string signature, ushort version, int size, bool expected)
            => CanInitializeTestHelper<
                Th12Converter, Th12Converter.CharaWithTotal, Th12Converter.StageProgress>(
                signature, version, size, expected);

        #endregion
    }
}

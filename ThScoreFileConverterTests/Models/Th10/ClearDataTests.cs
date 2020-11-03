using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th10;
using ThScoreFileConverterTests.Extensions;

namespace ThScoreFileConverterTests.Models.Th10
{
    [TestClass]
    public class ClearDataTests
    {
        internal static Mock<IClearData<CharaWithTotal, StageProgress>> MockClearData()
        {
            var levels = Utils.GetEnumerable<Level>();
            var levelsExceptExtra = levels.Where(level => level != Level.Extra);
            var stages = Utils.GetEnumerable<Stage>();
            var stagesExceptExtra = stages.Where(stage => stage != Stage.Extra);

            var mock = new Mock<IClearData<CharaWithTotal, StageProgress>>();
            _ = mock.SetupGet(m => m.Signature).Returns("CR");
            _ = mock.SetupGet(m => m.Version).Returns(0x0000);
            _ = mock.SetupGet(m => m.Checksum).Returns(0u);
            _ = mock.SetupGet(m => m.Size).Returns(0x437C);
            _ = mock.SetupGet(m => m.Chara).Returns(CharaWithTotal.ReimuB);
            _ = mock.SetupGet(m => m.Rankings).Returns(
                levels.ToDictionary(
                    level => level,
                    level => Enumerable.Range(0, 10).Select(
                        index => Mock.Of<IScoreData<StageProgress>>(
                            m => (m.Score == 12345670u - ((uint)index * 1000u))
                                 && (m.StageProgress == StageProgress.Five)
                                 && (m.ContinueCount == (byte)index)
                                 && (m.Name == TestUtils.CP932Encoding.GetBytes($"Player{index}\0\0\0"))
                                 && (m.DateTime == 34567890u)
                                 && (m.SlowRate == 1.2f))).ToList() as IReadOnlyList<IScoreData<StageProgress>>));
            _ = mock.SetupGet(m => m.TotalPlayCount).Returns(23);
            _ = mock.SetupGet(m => m.PlayTime).Returns(4567890);
            _ = mock.SetupGet(m => m.ClearCounts).Returns(
                levels.ToDictionary(level => level, level => 100 - (int)level));
            _ = mock.SetupGet(m => m.Practices).Returns(
                levelsExceptExtra
                    .SelectMany(level => stagesExceptExtra.Select(stage => (level, stage)))
                    .ToDictionary(
                        pair => pair,
                        pair => Mock.Of<IPractice>(
                            m => (m.Score == 123456u - ((uint)pair.level * 10u))
                                 && (m.StageFlag == (uint)pair.stage % 2u))));
            _ = mock.SetupGet(m => m.Cards).Returns(
                Enumerable.Range(1, 110).ToDictionary(
                    index => index,
                    index => Mock.Of<ISpellCard<Level>>(
                        m => (m.Name == TestUtils.MakeRandomArray<byte>(0x80))
                             && (m.ClearCount == 123 + index)
                             && (m.TrialCount == 456 + index)
                             && (m.Id == index)
                             && (m.Level == Level.Hard))));
            return mock;
        }

        internal static byte[] MakeData<TCharaWithTotal, TStageProgress>(
            IClearData<TCharaWithTotal, TStageProgress> clearData, int scoreDataUnknownSize)
            where TCharaWithTotal : struct, Enum
            where TStageProgress : struct, Enum
            => TestUtils.MakeByteArray(
                TestUtils.Cast<int>(clearData.Chara),
                clearData.Rankings.Values.SelectMany(
                    ranking => ranking.SelectMany(
                        scoreData => ScoreDataTests.MakeByteArray(scoreData, scoreDataUnknownSize))).ToArray(),
                clearData.TotalPlayCount,
                clearData.PlayTime,
                clearData.ClearCounts.Values.ToArray(),
                clearData.Practices.Values.SelectMany(
                    practice => PracticeTests.MakeByteArray(practice)).ToArray(),
                clearData.Cards.Values.SelectMany(
                    card => SpellCardTests.MakeByteArray(card)).ToArray());

        internal static byte[] MakeByteArray<TCharaWithTotal, TStageProgress>(
            IClearData<TCharaWithTotal, TStageProgress> clearData, int scoreDataUnknownSize)
            where TCharaWithTotal : struct, Enum
            where TStageProgress : struct, Enum
            => TestUtils.MakeByteArray(
                clearData.Signature.ToCharArray(),
                clearData.Version,
                clearData.Checksum,
                clearData.Size,
                MakeData(clearData, scoreDataUnknownSize));

        internal static byte[] MakeByteArray(IClearData<CharaWithTotal, StageProgress> clearData)
            => MakeByteArray(clearData, 0);

        internal static void Validate<TCharaWithTotal, TStageProgress>(
            IClearData<TCharaWithTotal, TStageProgress> expected, IClearData<TCharaWithTotal, TStageProgress> actual)
            where TCharaWithTotal : struct, Enum
            where TStageProgress : struct, Enum
        {
            Assert.AreEqual(expected.Signature, actual.Signature);
            Assert.AreEqual(expected.Version, actual.Version);
            Assert.AreEqual(expected.Checksum, actual.Checksum);
            Assert.AreEqual(expected.Size, actual.Size);
            Assert.AreEqual(expected.Chara, actual.Chara);

            foreach (var pair in expected.Rankings)
            {
                for (var index = 0; index < pair.Value.Count; ++index)
                {
                    ScoreDataTests.Validate(pair.Value[index], actual.Rankings[pair.Key][index]);
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

        [TestMethod]
        public void ClearDataTestChapter()
        {
            var mock = MockClearData();

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
            var clearData = new ClearData(chapter);

            Validate(mock.Object, clearData);
            Assert.IsFalse(clearData.IsValid);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ClearDataTestNullChapter()
        {
            _ = new ClearData(null!);

            Assert.Fail(TestUtils.Unreachable);
        }

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void ClearDataTestInvalidSignature()
        {
            var mock = MockClearData();
            var signature = mock.Object.Signature;
            _ = mock.SetupGet(m => m.Signature).Returns(signature.ToLowerInvariant());

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
            _ = new ClearData(chapter);

            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void ClearDataTestInvalidVersion()
        {
            var mock = MockClearData();
            var version = mock.Object.Version;
            _ = mock.SetupGet(m => m.Version).Returns(++version);

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
            _ = new ClearData(chapter);

            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void ClearDataTestInvalidSize()
        {
            var mock = MockClearData();
            var size = mock.Object.Size;
            _ = mock.SetupGet(m => m.Size).Returns(--size);

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
            _ = new ClearData(chapter);

            Assert.Fail(TestUtils.Unreachable);
        }

        [DataTestMethod]
        [DataRow("CR", (ushort)0, 0x437C, true)]
        [DataRow("cr", (ushort)0, 0x437C, false)]
        [DataRow("CR", (ushort)1, 0x437C, false)]
        [DataRow("CR", (ushort)0, 0x437D, false)]
        public void CanInitializeTest(string signature, ushort version, int size, bool expected)
        {
            var checksum = 0u;
            var data = new byte[size];

            var chapter = TestUtils.Create<Chapter>(
                TestUtils.MakeByteArray(signature.ToCharArray(), version, checksum, size, data));

            Assert.AreEqual(expected, ClearData.CanInitialize(chapter));
        }
    }
}

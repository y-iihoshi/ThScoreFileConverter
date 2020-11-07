using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th17;
using ThScoreFileConverterTests.Extensions;
using Chapter = ThScoreFileConverter.Models.Th10.Chapter;
using IPractice = ThScoreFileConverter.Models.Th13.IPractice;
using ISpellCard = ThScoreFileConverter.Models.Th13.ISpellCard<ThScoreFileConverter.Models.Level>;
using StageProgress = ThScoreFileConverter.Models.Th13.StageProgress;

namespace ThScoreFileConverterTests.Models.Th17
{
    [TestClass]
    public class ClearDataTests
    {
        internal static Mock<IClearData> MockClearData()
        {
            var levels = Utils.GetEnumerable<Level>();
            var levelsWithTotal = Utils.GetEnumerable<LevelWithTotal>();
            var stages = Utils.GetEnumerable<StagePractice>();

            var mock = new Mock<IClearData>();
            _ = mock.SetupGet(m => m.Signature).Returns("CR");
            _ = mock.SetupGet(m => m.Version).Returns(2);
            _ = mock.SetupGet(m => m.Checksum).Returns(0u);
            _ = mock.SetupGet(m => m.Size).Returns(0x4820);
            _ = mock.SetupGet(m => m.Chara).Returns(CharaWithTotal.MarisaB);
            _ = mock.SetupGet(m => m.Rankings).Returns(
                levelsWithTotal.ToDictionary(
                    level => level,
                    level => Enumerable.Range(0, 10).Select(
                        index => Mock.Of<IScoreData>(
                            s => (s.Score == 12345670u - ((uint)index * 1000u))
                                 && (s.StageProgress == (StageProgress)index)
                                 && (s.ContinueCount == (byte)index)
                                 && (s.Name == TestUtils.MakeByteArray($"Player{index}\0\0\0").Skip(1).ToArray())   // skip length
                                 && (s.DateTime == 34567890u)
                                 && (s.SlowRate == 1.2f))).ToList() as IReadOnlyList<IScoreData>));
            _ = mock.SetupGet(m => m.TotalPlayCount).Returns(23);
            _ = mock.SetupGet(m => m.PlayTime).Returns(4567890);
            _ = mock.SetupGet(m => m.ClearCounts).Returns(
                levelsWithTotal.ToDictionary(level => level, level => 100 - TestUtils.Cast<int>(level)));
            _ = mock.SetupGet(m => m.ClearFlags).Returns(
                levelsWithTotal.ToDictionary(level => level, level => TestUtils.Cast<int>(level) % 2));
            _ = mock.SetupGet(m => m.Practices).Returns(
                levels
                    .SelectMany(level => stages.Select(stage => (level, stage)))
                    .ToDictionary(
                        pair => pair,
                        pair => Mock.Of<IPractice>(
                            m => (m.Score == 123456u - (TestUtils.Cast<uint>(pair.level) * 10u))
                                 && (m.ClearFlag == (byte)(TestUtils.Cast<int>(pair.stage) % 2))
                                 && (m.EnableFlag == (byte)(TestUtils.Cast<int>(pair.level) % 2)))));
            _ = mock.SetupGet(m => m.Cards).Returns(
                Definitions.CardTable.ToDictionary(
                    pair => pair.Key,
                    pair => Mock.Of<ISpellCard>(
                        m => (m.Name == TestUtils.MakeRandomArray<byte>(0x80))
                             && (m.ClearCount == ((pair.Key % 2 == 0) ? 0 : 12 + pair.Key))
                             && (m.PracticeClearCount == ((pair.Key % 3 == 0) ? 0 : 34 + pair.Key))
                             && (m.TrialCount == ((pair.Key % 4 == 0) ? 0 : 56 + pair.Key))
                             && (m.PracticeTrialCount == ((pair.Key % 5 == 0) ? 0 : 78 + pair.Key))
                             && (m.Id == pair.Value.Id)
                             && (m.Level == pair.Value.Level)
                             && (m.PracticeScore == 90123))));
            return mock;
        }

        internal static byte[] MakeByteArray(IClearData clearData)
            => TestUtils.MakeByteArray(
                clearData.Signature.ToCharArray(),
                clearData.Version,
                clearData.Checksum,
                clearData.Size,
                (int)clearData.Chara,
                clearData.Rankings.Values.SelectMany(
                    ranking => ranking.SelectMany(scoreData => ScoreDataTests.MakeByteArray(scoreData))).ToArray(),
                new byte[0x140],
                clearData.Cards.Values.SelectMany(card => Th13.SpellCardTests.MakeByteArray(card)).ToArray(),
                clearData.TotalPlayCount,
                clearData.PlayTime,
                0u,
                clearData.ClearCounts.Values.ToArray(),
                0u,
                clearData.ClearFlags.Values.ToArray(),
                0u,
                clearData.Practices.Values.SelectMany(practice => Th13.PracticeTests.MakeByteArray(practice)).ToArray(),
                new byte[0x40]);

        internal static void Validate(IClearData expected, IClearData actual)
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
                    ScoreDataTests.Validate(actual.Rankings[pair.Key][index], pair.Value[index]);
                }
            }

            Assert.AreEqual(expected.TotalPlayCount, actual.TotalPlayCount);
            Assert.AreEqual(expected.PlayTime, actual.PlayTime);
            CollectionAssert.That.AreEqual(expected.ClearCounts.Values, actual.ClearCounts.Values);
            CollectionAssert.That.AreEqual(expected.ClearFlags.Values, actual.ClearFlags.Values);

            foreach (var pair in expected.Practices)
            {
                Th13.PracticeTests.Validate(pair.Value, actual.Practices[pair.Key]);
            }

            foreach (var pair in expected.Cards)
            {
                Th13.SpellCardTests.Validate(pair.Value, actual.Cards[pair.Key]);
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
        [DataRow("CR", (ushort)2, 0x4820, true)]
        [DataRow("cr", (ushort)2, 0x4820, false)]
        [DataRow("CR", (ushort)1, 0x4820, false)]
        [DataRow("CR", (ushort)2, 0x4821, false)]
        public void ClearDataCanInitializeTest(string signature, ushort version, int size, bool expected)
        {
            var checksum = 0u;
            var data = new byte[size];

            var chapter = TestUtils.Create<Chapter>(
                TestUtils.MakeByteArray(signature.ToCharArray(), version, checksum, size, data));

            Assert.AreEqual(expected, ClearData.CanInitialize(chapter));
        }
    }
}

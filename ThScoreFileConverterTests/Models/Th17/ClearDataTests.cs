using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Helpers;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th17;
using ThScoreFileConverterTests.Extensions;
using Chapter = ThScoreFileConverter.Models.Th10.Chapter;
using IPractice = ThScoreFileConverter.Models.Th13.IPractice;
using IScoreData = ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th13.StageProgress>;
using ISpellCard = ThScoreFileConverter.Models.Th13.ISpellCard<ThScoreFileConverter.Models.Level>;
using StageProgress = ThScoreFileConverter.Models.Th13.StageProgress;

namespace ThScoreFileConverterTests.Models.Th17
{
    [TestClass]
    public class ClearDataTests
    {
        internal static Mock<IClearData> MockClearData()
        {
            static IScoreData CreateScoreData(int index)
            {
                var mock = new Mock<IScoreData>();
                _ = mock.SetupGet(s => s.Score).Returns(12345670u - ((uint)index * 1000u));
                _ = mock.SetupGet(s => s.StageProgress).Returns((StageProgress)index);
                _ = mock.SetupGet(s => s.ContinueCount).Returns((byte)index);
                _ = mock.SetupGet(s => s.Name).Returns(
                    TestUtils.MakeByteArray($"Player{index}\0\0\0").Skip(1).ToArray());  // skip length
                _ = mock.SetupGet(s => s.DateTime).Returns(34567890u);
                _ = mock.SetupGet(s => s.SlowRate).Returns(1.2f);
                return mock.Object;
            }

            static IPractice CreatePractice((Level, StagePractice) pair)
            {
                var mock = new Mock<IPractice>();
                _ = mock.SetupGet(p => p.Score).Returns(123456u - (TestUtils.Cast<uint>(pair.Item1) * 10u));
                _ = mock.SetupGet(p => p.ClearFlag).Returns((byte)(TestUtils.Cast<int>(pair.Item2) % 2));
                _ = mock.SetupGet(p => p.EnableFlag).Returns((byte)(TestUtils.Cast<int>(pair.Item1) % 2));
                return mock.Object;
            }

            static ISpellCard CreateSpellCard(
                int clear, int practiceClear, int trial, int practiceTrial, int id, Level level)
            {
                var mock = new Mock<ISpellCard>();
                _ = mock.SetupGet(s => s.Name).Returns(TestUtils.MakeRandomArray<byte>(0x80));
                _ = mock.SetupGet(s => s.ClearCount).Returns(clear);
                _ = mock.SetupGet(s => s.PracticeClearCount).Returns(practiceClear);
                _ = mock.SetupGet(s => s.TrialCount).Returns(trial);
                _ = mock.SetupGet(s => s.PracticeTrialCount).Returns(practiceTrial);
                _ = mock.SetupGet(s => s.Id).Returns(id);
                _ = mock.SetupGet(s => s.Level).Returns(level);
                _ = mock.SetupGet(s => s.PracticeScore).Returns(90123);
                return mock.Object;
            }

            var levelsWithTotal = EnumHelper<LevelWithTotal>.Enumerable;

            var mock = new Mock<IClearData>();
            _ = mock.SetupGet(m => m.Signature).Returns("CR");
            _ = mock.SetupGet(m => m.Version).Returns(2);
            _ = mock.SetupGet(m => m.Checksum).Returns(0u);
            _ = mock.SetupGet(m => m.Size).Returns(0x4820);
            _ = mock.SetupGet(m => m.Chara).Returns(CharaWithTotal.MarisaB);
            _ = mock.SetupGet(m => m.Rankings).Returns(
                levelsWithTotal.ToDictionary(
                    level => level,
                    level => Enumerable.Range(0, 10).Select(index => CreateScoreData(index)).ToList()
                        as IReadOnlyList<IScoreData>));
            _ = mock.SetupGet(m => m.TotalPlayCount).Returns(23);
            _ = mock.SetupGet(m => m.PlayTime).Returns(4567890);
            _ = mock.SetupGet(m => m.ClearCounts).Returns(
                levelsWithTotal.ToDictionary(level => level, level => 100 - TestUtils.Cast<int>(level)));
            _ = mock.SetupGet(m => m.ClearFlags).Returns(
                levelsWithTotal.ToDictionary(level => level, level => TestUtils.Cast<int>(level) % 2));
            _ = mock.SetupGet(m => m.Practices).Returns(
                EnumHelper<Level>.Enumerable
                    .SelectMany(level => EnumHelper<StagePractice>.Enumerable.Select(stage => (level, stage)))
                    .ToDictionary(pair => pair, pair => CreatePractice(pair)));
            _ = mock.SetupGet(m => m.Cards).Returns(
                Definitions.CardTable.ToDictionary(
                    pair => pair.Key,
                    pair => CreateSpellCard(
                        (pair.Key % 2 == 0) ? 0 : 12 + pair.Key,
                        (pair.Key % 3 == 0) ? 0 : 34 + pair.Key,
                        (pair.Key % 4 == 0) ? 0 : 56 + pair.Key,
                        (pair.Key % 5 == 0) ? 0 : 78 + pair.Key,
                        pair.Value.Id,
                        pair.Value.Level)));
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
                    Th10.ScoreDataTests.Validate(actual.Rankings[pair.Key][index], pair.Value[index]);
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
        public void ClearDataTestInvalidSignature()
        {
            var mock = MockClearData();
            var signature = mock.Object.Signature;
            _ = mock.SetupGet(m => m.Signature).Returns(signature.ToLowerInvariant());

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
            _ = Assert.ThrowsException<InvalidDataException>(() => _ = new ClearData(chapter));
        }

        [TestMethod]
        public void ClearDataTestInvalidVersion()
        {
            var mock = MockClearData();
            var version = mock.Object.Version;
            _ = mock.SetupGet(m => m.Version).Returns(++version);

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
            _ = Assert.ThrowsException<InvalidDataException>(() => _ = new ClearData(chapter));
        }

        [TestMethod]
        public void ClearDataTestInvalidSize()
        {
            var mock = MockClearData();
            var size = mock.Object.Size;
            _ = mock.SetupGet(m => m.Size).Returns(--size);

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
            _ = Assert.ThrowsException<InvalidDataException>(() => _ = new ClearData(chapter));
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

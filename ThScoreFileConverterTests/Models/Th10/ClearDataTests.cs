using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Helpers;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th10;
using ThScoreFileConverterTests.UnitTesting;

namespace ThScoreFileConverterTests.Models.Th10
{
    [TestClass]
    public class ClearDataTests
    {
        internal static Mock<IClearData<CharaWithTotal, StageProgress>> MockClearData()
        {
            static IScoreData<StageProgress> CreateScoreData(int index)
            {
                var mock = new Mock<IScoreData<StageProgress>>();
                _ = mock.SetupGet(s => s.Score).Returns(12345670u - ((uint)index * 1000u));
                _ = mock.SetupGet(s => s.StageProgress).Returns(StageProgress.Five);
                _ = mock.SetupGet(s => s.ContinueCount).Returns((byte)index);
                _ = mock.SetupGet(s => s.Name).Returns(TestUtils.CP932Encoding.GetBytes($"Player{index}\0\0\0"));
                _ = mock.SetupGet(s => s.DateTime).Returns(34567890u);
                _ = mock.SetupGet(s => s.SlowRate).Returns(1.2f);
                return mock.Object;
            }

            static IPractice CreatePractice((Level, Stage) pair)
            {
                var mock = new Mock<IPractice>();
                _ = mock.SetupGet(p => p.Score).Returns(123456u - ((uint)pair.Item1 * 10u));
                _ = mock.SetupGet(p => p.StageFlag).Returns((uint)pair.Item2 % 2u);
                return mock.Object;
            }

            static ISpellCard<Level> CreateSpellCard(int index)
            {
                var mock = new Mock<ISpellCard<Level>>();
                _ = mock.SetupGet(s => s.Name).Returns(TestUtils.MakeRandomArray<byte>(0x80));
                _ = mock.SetupGet(s => s.ClearCount).Returns(123 + index);
                _ = mock.SetupGet(s => s.TrialCount).Returns(456 + index);
                _ = mock.SetupGet(s => s.Id).Returns(index);
                _ = mock.SetupGet(s => s.Level).Returns(Level.Hard);
                return mock.Object;
            }

            var levels = EnumHelper<Level>.Enumerable;
            var levelsExceptExtra = levels.Where(level => level != Level.Extra);
            var stages = EnumHelper<Stage>.Enumerable;
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
                    level => Enumerable.Range(0, 10).Select(index => CreateScoreData(index)).ToList()
                        as IReadOnlyList<IScoreData<StageProgress>>));
            _ = mock.SetupGet(m => m.TotalPlayCount).Returns(23);
            _ = mock.SetupGet(m => m.PlayTime).Returns(4567890);
            _ = mock.SetupGet(m => m.ClearCounts).Returns(
                levels.ToDictionary(level => level, level => 100 - (int)level));
            _ = mock.SetupGet(m => m.Practices).Returns(
                levelsExceptExtra
                    .SelectMany(level => stagesExceptExtra.Select(stage => (level, stage)))
                    .ToDictionary(pair => pair, pair => CreatePractice(pair)));
            _ = mock.SetupGet(m => m.Cards).Returns(
                Enumerable.Range(1, 110).ToDictionary(index => index, index => CreateSpellCard(index)));
            return mock;
        }

        internal static byte[] MakeData<TCharaWithTotal, TStageProgress>(
            IClearData<TCharaWithTotal, TStageProgress> clearData, int scoreDataUnknownSize)
            where TCharaWithTotal : struct, Enum
            where TStageProgress : struct, Enum
        {
            return TestUtils.MakeByteArray(
                TestUtils.Cast<int>(clearData.Chara),
                clearData.Rankings.Values.SelectMany(
                    ranking => ranking.SelectMany(
                        scoreData => ScoreDataTests.MakeByteArray(scoreData, scoreDataUnknownSize))),
                clearData.TotalPlayCount,
                clearData.PlayTime,
                clearData.ClearCounts.Values,
                clearData.Practices.Values.SelectMany(practice => PracticeTests.MakeByteArray(practice)),
                clearData.Cards.Values.SelectMany(card => SpellCardTests.MakeByteArray(card)));
        }

        internal static byte[] MakeByteArray<TCharaWithTotal, TStageProgress>(
            IClearData<TCharaWithTotal, TStageProgress> clearData, int scoreDataUnknownSize)
            where TCharaWithTotal : struct, Enum
            where TStageProgress : struct, Enum
        {
            return TestUtils.MakeByteArray(
                clearData.Signature.ToCharArray(),
                clearData.Version,
                clearData.Checksum,
                clearData.Size,
                MakeData(clearData, scoreDataUnknownSize));
        }

        internal static byte[] MakeByteArray(IClearData<CharaWithTotal, StageProgress> clearData)
        {
            return MakeByteArray(clearData, 0);
        }

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
        public void ClearDataTestInvalidSignature()
        {
            var mock = MockClearData();
            var signature = mock.Object.Signature;
            _ = mock.SetupGet(m => m.Signature).Returns(signature.ToLowerInvariant());

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
            _ = Assert.ThrowsException<InvalidDataException>(() => new ClearData(chapter));
        }

        [TestMethod]
        public void ClearDataTestInvalidVersion()
        {
            var mock = MockClearData();
            var version = mock.Object.Version;
            _ = mock.SetupGet(m => m.Version).Returns(++version);

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
            _ = Assert.ThrowsException<InvalidDataException>(() => new ClearData(chapter));
        }

        [TestMethod]
        public void ClearDataTestInvalidSize()
        {
            var mock = MockClearData();
            var size = mock.Object.Size;
            _ = mock.SetupGet(m => m.Size).Returns(--size);

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
            _ = Assert.ThrowsException<InvalidDataException>(() => new ClearData(chapter));
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

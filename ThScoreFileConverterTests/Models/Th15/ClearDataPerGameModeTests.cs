using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th13;
using ThScoreFileConverter.Models.Th15;
using ThScoreFileConverterTests.Extensions;
using ISpellCard = ThScoreFileConverter.Models.Th13.ISpellCard<ThScoreFileConverter.Models.Level>;
using StageProgress = ThScoreFileConverter.Models.Th13.StageProgress;

namespace ThScoreFileConverterTests.Models.Th15
{
    [TestClass]
    public class ClearDataPerGameModeTests
    {
        internal static Mock<IClearDataPerGameMode> MockInitialClearDataPerGameMode()
        {
            var mock = new Mock<IClearDataPerGameMode>();
            _ = mock.SetupGet(m => m.Cards).Returns(ImmutableDictionary<int, ISpellCard<Level>>.Empty);
            _ = mock.SetupGet(m => m.ClearCounts).Returns(ImmutableDictionary<LevelWithTotal, int>.Empty);
            _ = mock.SetupGet(m => m.ClearFlags).Returns(ImmutableDictionary<LevelWithTotal, int>.Empty);
            _ = mock.SetupGet(m => m.Rankings).Returns(
                ImmutableDictionary<LevelWithTotal, IReadOnlyList<IScoreData>>.Empty);
            return mock;
        }

        internal static Mock<IClearDataPerGameMode> MockClearDataPerGameMode()
        {
            var levelsWithTotal = Utils.GetEnumerable<LevelWithTotal>();

            var mock = new Mock<IClearDataPerGameMode>();
            _ = mock.SetupGet(m => m.Rankings).Returns(
                levelsWithTotal.ToDictionary(
                    level => level,
                    level => Enumerable.Range(0, 10).Select(
                        index => Mock.Of<IScoreData>(
                            m => (m.Score == 12345670u - ((uint)index * 1000u))
                                 && (m.StageProgress == StageProgress.Five)
                                 && (m.ContinueCount == (byte)index)
                                 && (m.Name == TestUtils.CP932Encoding.GetBytes($"Player{index}\0\0\0"))
                                 && (m.DateTime == 34567890u)
                                 && (m.SlowRate == 1.2f)
                                 && (m.RetryCount == (uint)index % 4u))).ToList() as IReadOnlyList<IScoreData>));
            _ = mock.SetupGet(m => m.TotalPlayCount).Returns(23);
            _ = mock.SetupGet(m => m.PlayTime).Returns(4567890);
            _ = mock.SetupGet(m => m.ClearCounts).Returns(
                levelsWithTotal.ToDictionary(level => level, level => 100 - TestUtils.Cast<int>(level)));
            _ = mock.SetupGet(m => m.ClearFlags).Returns(
                levelsWithTotal.ToDictionary(level => level, level => TestUtils.Cast<int>(level) % 2));
            _ = mock.SetupGet(m => m.Cards).Returns(
                Enumerable.Range(1, 119).ToDictionary(
                    index => index,
                    index => Mock.Of<ISpellCard>(
                        m => (m.Name == TestUtils.MakeRandomArray<byte>(0x80))
                             && (m.ClearCount == 12 + index)
                             && (m.PracticeClearCount == 34 + index)
                             && (m.TrialCount == 56 + index)
                             && (m.PracticeTrialCount == 78 + index)
                             && (m.Id == index)
                             && (m.Level == Level.Hard)
                             && (m.PracticeScore == 90123))));
            return mock;
        }

        internal static byte[] MakeByteArray(IClearDataPerGameMode clearData)
            => TestUtils.MakeByteArray(
                clearData.Rankings.Values.SelectMany(
                    ranking => ranking.SelectMany(scoreData => ScoreDataTests.MakeByteArray(scoreData))).ToArray(),
                new byte[0x140],
                clearData.Cards.Values.SelectMany(card => SpellCardTests.MakeByteArray(card)).ToArray(),
                clearData.TotalPlayCount,
                clearData.PlayTime,
                0u,
                clearData.ClearCounts.Values.ToArray(),
                0u,
                clearData.ClearFlags.Values.ToArray(),
                0u);

        internal static void Validate(IClearDataPerGameMode expected, IClearDataPerGameMode actual)
        {
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
            CollectionAssert.That.AreEqual(expected.ClearFlags.Values, actual.ClearFlags.Values);

            foreach (var pair in expected.Cards)
            {
                SpellCardTests.Validate(pair.Value, actual.Cards[pair.Key]);
            }
        }

        [TestMethod]
        public void ClearDataPerGameModeTest()
        {
            var mock = MockInitialClearDataPerGameMode();

            var clearData = new ClearDataPerGameMode();

            Validate(mock.Object, clearData);
        }

        [TestMethod]
        public void ReadFromTest()
        {
            var mock = MockClearDataPerGameMode();

            var clearData = TestUtils.Create<ClearDataPerGameMode>(MakeByteArray(mock.Object));

            Validate(mock.Object, clearData);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReadFromTestNull()
        {
            var clearData = new ClearDataPerGameMode();
            clearData.ReadFrom(null!);

            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void ReadFromTestShortened()
        {
            var mock = MockClearDataPerGameMode();
            var array = MakeByteArray(mock.Object).SkipLast(1).ToArray();

            _ = TestUtils.Create<ClearDataPerGameMode>(array);

            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void ReadFromTestExceeded()
        {
            var mock = MockClearDataPerGameMode();
            var array = MakeByteArray(mock.Object).Concat(new byte[1] { 1 }).ToArray();

            var clearData = TestUtils.Create<ClearDataPerGameMode>(array);

            Validate(mock.Object, clearData);
        }
    }
}

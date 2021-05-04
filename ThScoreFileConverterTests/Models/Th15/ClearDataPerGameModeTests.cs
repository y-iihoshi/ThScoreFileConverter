using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Helpers;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th13;
using ThScoreFileConverter.Models.Th15;
using ThScoreFileConverterTests.UnitTesting;
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
            static IScoreData CreateScoreData(int index)
            {
                var mock = new Mock<IScoreData>();
                _ = mock.SetupGet(s => s.Score).Returns(12345670u - ((uint)index * 1000u));
                _ = mock.SetupGet(s => s.StageProgress).Returns(StageProgress.Five);
                _ = mock.SetupGet(s => s.ContinueCount).Returns((byte)index);
                _ = mock.SetupGet(s => s.Name).Returns(TestUtils.CP932Encoding.GetBytes($"Player{index}\0\0\0"));
                _ = mock.SetupGet(s => s.DateTime).Returns(34567890u);
                _ = mock.SetupGet(s => s.SlowRate).Returns(1.2f);
                _ = mock.SetupGet(s => s.RetryCount).Returns((uint)index % 4u);
                return mock.Object;
            }

            static ISpellCard CreateSpellCard(int index)
            {
                var mock = new Mock<ISpellCard>();
                _ = mock.SetupGet(s => s.Name).Returns(TestUtils.MakeRandomArray<byte>(0x80));
                _ = mock.SetupGet(s => s.ClearCount).Returns(12 + index);
                _ = mock.SetupGet(s => s.PracticeClearCount).Returns(34 + index);
                _ = mock.SetupGet(s => s.TrialCount).Returns(56 + index);
                _ = mock.SetupGet(s => s.PracticeTrialCount).Returns(78 + index);
                _ = mock.SetupGet(s => s.Id).Returns(index);
                _ = mock.SetupGet(s => s.Level).Returns(Level.Hard);
                _ = mock.SetupGet(s => s.PracticeScore).Returns(90123);
                return mock.Object;
            }

            var levelsWithTotal = EnumHelper<LevelWithTotal>.Enumerable;

            var mock = new Mock<IClearDataPerGameMode>();
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
            _ = mock.SetupGet(m => m.Cards).Returns(
                Enumerable.Range(1, 119).ToDictionary(index => index, index => CreateSpellCard(index)));
            return mock;
        }

        internal static byte[] MakeByteArray(IClearDataPerGameMode clearData)
        {
            return TestUtils.MakeByteArray(
                clearData.Rankings.Values.SelectMany(
                    ranking => ranking.Select(scoreData => ScoreDataTests.MakeByteArray(scoreData))),
                new byte[0x140],
                clearData.Cards.Values.Select(card => SpellCardTests.MakeByteArray(card)),
                clearData.TotalPlayCount,
                clearData.PlayTime,
                0u,
                clearData.ClearCounts.Values,
                0u,
                clearData.ClearFlags.Values,
                0u);
        }

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
        public void ReadFromTestShortened()
        {
            var mock = MockClearDataPerGameMode();
            var array = MakeByteArray(mock.Object).SkipLast(1).ToArray();

            _ = Assert.ThrowsException<EndOfStreamException>(() => TestUtils.Create<ClearDataPerGameMode>(array));
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

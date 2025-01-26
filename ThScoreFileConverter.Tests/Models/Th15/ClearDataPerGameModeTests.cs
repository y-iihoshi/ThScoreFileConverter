using System.Collections.Immutable;
using NSubstitute;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Models.Th13;
using ThScoreFileConverter.Models.Th15;
using ISpellCard = ThScoreFileConverter.Models.Th13.ISpellCard<ThScoreFileConverter.Core.Models.Level>;
using StageProgress = ThScoreFileConverter.Models.Th13.StageProgress;

namespace ThScoreFileConverter.Tests.Models.Th15;

[TestClass]
public class ClearDataPerGameModeTests
{
    internal static IClearDataPerGameMode MockInitialClearDataPerGameMode()
    {
        var mock = Substitute.For<IClearDataPerGameMode>();
        _ = mock.Cards.Returns(ImmutableDictionary<int, ISpellCard<Level>>.Empty);
        _ = mock.ClearCounts.Returns(ImmutableDictionary<LevelWithTotal, int>.Empty);
        _ = mock.ClearFlags.Returns(ImmutableDictionary<LevelWithTotal, int>.Empty);
        _ = mock.Rankings.Returns(ImmutableDictionary<LevelWithTotal, IReadOnlyList<IScoreData>>.Empty);
        return mock;
    }

    internal static IClearDataPerGameMode MockClearDataPerGameMode()
    {
        static IScoreData MockScoreData(int index)
        {
            var mock = Substitute.For<IScoreData>();
            _ = mock.Score.Returns(12345670u - ((uint)index * 1000u));
            _ = mock.StageProgress.Returns(StageProgress.Five);
            _ = mock.ContinueCount.Returns((byte)index);
            _ = mock.Name.Returns(TestUtils.CP932Encoding.GetBytes($"Player{index}\0\0\0"));
            _ = mock.DateTime.Returns(34567890u);
            _ = mock.SlowRate.Returns(1.2f);
            _ = mock.RetryCount.Returns((uint)index % 4u);
            return mock;
        }

        static ISpellCard MockSpellCard(int index)
        {
            var mock = Substitute.For<ISpellCard>();
            _ = mock.Name.Returns(TestUtils.MakeRandomArray(0x80));
            _ = mock.ClearCount.Returns(12 + index);
            _ = mock.PracticeClearCount.Returns(34 + index);
            _ = mock.TrialCount.Returns(56 + index);
            _ = mock.PracticeTrialCount.Returns(78 + index);
            _ = mock.Id.Returns(index);
            _ = mock.Level.Returns(Level.Hard);
            _ = mock.PracticeScore.Returns(90123);
            return mock;
        }

        var levelsWithTotal = EnumHelper<LevelWithTotal>.Enumerable;
        var rankings = levelsWithTotal.ToDictionary(
            level => level,
            level => Enumerable.Range(0, 10).Select(MockScoreData).ToList() as IReadOnlyList<IScoreData>);
        var cards = Enumerable.Range(1, 119).ToDictionary(index => index, MockSpellCard);

        var mock = Substitute.For<IClearDataPerGameMode>();
        _ = mock.Rankings.Returns(rankings);
        _ = mock.TotalPlayCount.Returns(23);
        _ = mock.PlayTime.Returns(4567890);
        _ = mock.ClearCounts.Returns(levelsWithTotal.ToDictionary(level => level, level => 100 - (int)level));
        _ = mock.ClearFlags.Returns(levelsWithTotal.ToDictionary(level => level, level => (int)level % 2));
        _ = mock.Cards.Returns(cards);
        return mock;
    }

    internal static byte[] MakeByteArray(IClearDataPerGameMode clearData)
    {
        return TestUtils.MakeByteArray(
            clearData.Rankings.Values.SelectMany(ranking => ranking.Select(ScoreDataTests.MakeByteArray)),
            new byte[0x140],
            clearData.Cards.Values.Select(SpellCardTests.MakeByteArray),
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

        Validate(mock, clearData);
    }

    [TestMethod]
    public void ReadFromTest()
    {
        var mock = MockClearDataPerGameMode();

        var clearData = TestUtils.Create<ClearDataPerGameMode>(MakeByteArray(mock));

        Validate(mock, clearData);
    }

    [TestMethod]
    public void ReadFromTestShortened()
    {
        var mock = MockClearDataPerGameMode();
        var array = MakeByteArray(mock).SkipLast(1).ToArray();

        _ = Assert.ThrowsException<EndOfStreamException>(() => TestUtils.Create<ClearDataPerGameMode>(array));
    }

    [TestMethod]
    public void ReadFromTestExceeded()
    {
        var mock = MockClearDataPerGameMode();
        var array = MakeByteArray(mock).Concat(new byte[1] { 1 }).ToArray();

        var clearData = TestUtils.Create<ClearDataPerGameMode>(array);

        Validate(mock, clearData);
    }
}

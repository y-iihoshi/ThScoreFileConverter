using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NSubstitute;
using ThScoreFileConverter.Core.Tests.UnitTesting;
using ThScoreFileConverter.Models.Th16;
using ThScoreFileConverter.Tests.UnitTesting;
using StageProgress = ThScoreFileConverter.Models.Th13.StageProgress;

namespace ThScoreFileConverter.Tests.Models.Th16;

[TestClass]
public class ScoreDataTests
{
    internal static IScoreData MockScoreData()
    {
        var mock = Substitute.For<IScoreData>();
        _ = mock.Score.Returns(12u);
        _ = mock.StageProgress.Returns(StageProgress.Three);
        _ = mock.ContinueCount.Returns((byte)4);
        _ = mock.Name.Returns(TestUtils.MakeRandomArray(10));
        _ = mock.DateTime.Returns(567u);
        _ = mock.SlowRate.Returns(8.9f);
        _ = mock.Season.Returns(Season.Full);
        return mock;
    }

    internal static byte[] MakeByteArray(IScoreData scoreData)
    {
        return TestUtils.MakeByteArray(
            scoreData.Score,
            (byte)scoreData.StageProgress,
            scoreData.ContinueCount,
            scoreData.Name,
            scoreData.DateTime,
            0u,
            scoreData.SlowRate,
            (int)scoreData.Season);
    }

    internal static void Validate(IScoreData expected, IScoreData actual)
    {
        Assert.AreEqual(expected.Score, actual.Score);
        Assert.AreEqual(expected.StageProgress, actual.StageProgress);
        Assert.AreEqual(expected.ContinueCount, actual.ContinueCount);
        CollectionAssert.That.AreEqual(expected.Name, actual.Name);
        Assert.AreEqual(expected.DateTime, actual.DateTime);
        Assert.AreEqual(expected.SlowRate, actual.SlowRate);
        Assert.AreEqual(expected.Season, actual.Season);
    }

    [TestMethod]
    public void ScoreDataTest()
    {
        var mock = Substitute.For<IScoreData>();
        var scoreData = new ScoreData();

        Validate(mock, scoreData);
    }

    [TestMethod]
    public void ReadFromTest()
    {
        var mock = MockScoreData();
        var scoreData = TestUtils.Create<ScoreData>(MakeByteArray(mock));

        Validate(mock, scoreData);
    }

    public static IEnumerable<object[]> InvalidStageProgresses => TestUtils.GetInvalidEnumerators<StageProgress>();

    [DataTestMethod]
    [DynamicData(nameof(InvalidStageProgresses))]
    public void ReadFromTestInvalidStageProgress(int stageProgress)
    {
        var mock = MockScoreData();
        _ = mock.StageProgress.Returns((StageProgress)stageProgress);

        _ = Assert.ThrowsException<InvalidCastException>(
            () => TestUtils.Create<ScoreData>(MakeByteArray(mock)));
    }

    [TestMethod]
    public void ReadFromTestShortenedName()
    {
        var mock = MockScoreData();
        var name = mock.Name;
        _ = mock.Name.Returns(name.SkipLast(1).ToArray());

        _ = Assert.ThrowsException<EndOfStreamException>(
            () => TestUtils.Create<ScoreData>(MakeByteArray(mock)));
    }

    [TestMethod]
    public void ReadFromTestExceededName()
    {
        var mock = MockScoreData();
        var name = mock.Name;
        _ = mock.Name.Returns(name.Concat(TestUtils.MakeRandomArray(1)).ToArray());

        _ = Assert.ThrowsException<InvalidCastException>(
            () => TestUtils.Create<ScoreData>(MakeByteArray(mock)));
    }

    public static IEnumerable<object[]> InvalidSeasons => TestUtils.GetInvalidEnumerators<Season>();

    [DataTestMethod]
    [DynamicData(nameof(InvalidSeasons))]
    public void ReadFromTestInvalidSeason(int season)
    {
        var mock = MockScoreData();
        _ = mock.Season.Returns((Season)season);

        _ = Assert.ThrowsException<InvalidCastException>(
            () => TestUtils.Create<ScoreData>(MakeByteArray(mock)));
    }
}

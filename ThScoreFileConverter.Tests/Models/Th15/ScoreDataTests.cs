using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NSubstitute;
using ThScoreFileConverter.Models.Th15;
using ThScoreFileConverter.Tests.UnitTesting;
using StageProgress = ThScoreFileConverter.Models.Th13.StageProgress;

namespace ThScoreFileConverter.Tests.Models.Th15;

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
        _ = mock.DateTime.Returns(56u);
        _ = mock.SlowRate.Returns(7.8f);
        _ = mock.RetryCount.Returns(9u);
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
            scoreData.RetryCount);
    }

    internal static void Validate(IScoreData expected, IScoreData actual)
    {
        Assert.AreEqual(expected.Score, actual.Score);
        Assert.AreEqual(expected.StageProgress, actual.StageProgress);
        Assert.AreEqual(expected.ContinueCount, actual.ContinueCount);
        CollectionAssert.That.AreEqual(expected.Name, actual.Name);
        Assert.AreEqual(expected.DateTime, actual.DateTime);
        Assert.AreEqual(expected.SlowRate, actual.SlowRate);
        Assert.AreEqual(expected.RetryCount, actual.RetryCount);
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
        var validNameLength = name.Count();
        _ = mock.Name.Returns(name.Concat(TestUtils.MakeRandomArray(1)).ToArray());

        var scoreData = TestUtils.Create<ScoreData>(MakeByteArray(mock));

        Assert.AreEqual(mock.Score, scoreData.Score);
        Assert.AreEqual(mock.StageProgress, scoreData.StageProgress);
        Assert.AreEqual(mock.ContinueCount, scoreData.ContinueCount);
        CollectionAssert.That.AreNotEqual(mock.Name, scoreData.Name);
        CollectionAssert.That.AreEqual(mock.Name.Take(validNameLength), scoreData.Name);
        Assert.AreNotEqual(mock.DateTime, scoreData.DateTime);
        Assert.AreNotEqual(mock.SlowRate, scoreData.SlowRate);
        Assert.AreNotEqual(mock.RetryCount, scoreData.RetryCount);
    }
}

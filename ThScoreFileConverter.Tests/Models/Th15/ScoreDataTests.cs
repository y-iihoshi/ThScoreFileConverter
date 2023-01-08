using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Moq;
using ThScoreFileConverter.Core.Tests.UnitTesting;
using ThScoreFileConverter.Models.Th15;
using ThScoreFileConverter.Tests.UnitTesting;
using StageProgress = ThScoreFileConverter.Models.Th13.StageProgress;

#if NETFRAMEWORK
using ThScoreFileConverter.Core.Extensions;
#endif

namespace ThScoreFileConverter.Tests.Models.Th15;

[TestClass]
public class ScoreDataTests
{
    internal static Mock<IScoreData> MockScoreData()
    {
        var mock = new Mock<IScoreData>();
        _ = mock.SetupGet(m => m.Score).Returns(12u);
        _ = mock.SetupGet(m => m.StageProgress).Returns(StageProgress.Three);
        _ = mock.SetupGet(m => m.ContinueCount).Returns(4);
        _ = mock.SetupGet(m => m.Name).Returns(TestUtils.MakeRandomArray<byte>(10));
        _ = mock.SetupGet(m => m.DateTime).Returns(56u);
        _ = mock.SetupGet(m => m.SlowRate).Returns(7.8f);
        _ = mock.SetupGet(m => m.RetryCount).Returns(9u);
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
        var mock = new Mock<IScoreData>();
        var scoreData = new ScoreData();

        Validate(mock.Object, scoreData);
    }

    [TestMethod]
    public void ReadFromTest()
    {
        var mock = MockScoreData();
        var scoreData = TestUtils.Create<ScoreData>(MakeByteArray(mock.Object));

        Validate(mock.Object, scoreData);
    }

    public static IEnumerable<object[]> InvalidStageProgresses => TestUtils.GetInvalidEnumerators<StageProgress>();

    [DataTestMethod]
    [DynamicData(nameof(InvalidStageProgresses))]
    public void ReadFromTestInvalidStageProgress(int stageProgress)
    {
        var mock = MockScoreData();
        _ = mock.SetupGet(m => m.StageProgress).Returns(TestUtils.Cast<StageProgress>(stageProgress));

        _ = Assert.ThrowsException<InvalidCastException>(
            () => TestUtils.Create<ScoreData>(MakeByteArray(mock.Object)));
    }

    [TestMethod]
    public void ReadFromTestShortenedName()
    {
        var mock = MockScoreData();
        var name = mock.Object.Name;
        _ = mock.SetupGet(m => m.Name).Returns(name.SkipLast(1).ToArray());

        _ = Assert.ThrowsException<EndOfStreamException>(
            () => TestUtils.Create<ScoreData>(MakeByteArray(mock.Object)));
    }

    [TestMethod]
    public void ReadFromTestExceededName()
    {
        var mock = MockScoreData();
        var name = mock.Object.Name;
        var validNameLength = name.Count();
        _ = mock.SetupGet(m => m.Name).Returns(name.Concat(TestUtils.MakeRandomArray<byte>(1)).ToArray());

        var scoreData = TestUtils.Create<ScoreData>(MakeByteArray(mock.Object));

        Assert.AreEqual(mock.Object.Score, scoreData.Score);
        Assert.AreEqual(mock.Object.StageProgress, scoreData.StageProgress);
        Assert.AreEqual(mock.Object.ContinueCount, scoreData.ContinueCount);
        CollectionAssert.That.AreNotEqual(mock.Object.Name, scoreData.Name);
        CollectionAssert.That.AreEqual(mock.Object.Name.Take(validNameLength), scoreData.Name);
        Assert.AreNotEqual(mock.Object.DateTime, scoreData.DateTime);
        Assert.AreNotEqual(mock.Object.SlowRate, scoreData.SlowRate);
        Assert.AreNotEqual(mock.Object.RetryCount, scoreData.RetryCount);
    }
}

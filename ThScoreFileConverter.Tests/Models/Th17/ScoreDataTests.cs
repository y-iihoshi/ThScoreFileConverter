using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NSubstitute;
using ThScoreFileConverter.Core.Tests.UnitTesting;
using ThScoreFileConverter.Models.Th17;
using ThScoreFileConverter.Tests.UnitTesting;
using IScoreData = ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th13.StageProgress>;
using StageProgress = ThScoreFileConverter.Models.Th13.StageProgress;

#if NETFRAMEWORK
using ThScoreFileConverter.Core.Extensions;
#endif

namespace ThScoreFileConverter.Tests.Models.Th17;

[TestClass]
public class ScoreDataTests
{
    internal static byte[] MakeByteArray(in IScoreData scoreData)
    {
        return TestUtils.MakeByteArray(
            scoreData.Score,
            (byte)scoreData.StageProgress,
            scoreData.ContinueCount,
            scoreData.Name,
            scoreData.DateTime,
            0u,
            scoreData.SlowRate,
            0u);
    }

    [TestMethod]
    public void ReadFromTest()
    {
        var mock = Th10.ScoreDataTests.MockScoreData<StageProgress>();
        var scoreData = TestUtils.Create<ScoreData>(MakeByteArray(mock));

        Th10.ScoreDataTests.Validate(mock, scoreData);
    }

    public static IEnumerable<object[]> InvalidStageProgresses => TestUtils.GetInvalidEnumerators<StageProgress>();

    [DataTestMethod]
    [DynamicData(nameof(InvalidStageProgresses))]
    public void ReadFromTestInvalidStageProgress(int stageProgress)
    {
        var mock = Th10.ScoreDataTests.MockScoreData<StageProgress>();
        _ = mock.StageProgress.Returns((StageProgress)stageProgress);

        _ = Assert.ThrowsException<InvalidCastException>(
            () => TestUtils.Create<ScoreData>(MakeByteArray(mock)));
    }

    [TestMethod]
    public void ReadFromTestShortenedName()
    {
        var mock = Th10.ScoreDataTests.MockScoreData<StageProgress>();
        var name = mock.Name;
        _ = mock.Name.Returns(name.SkipLast(1).ToArray());

        _ = Assert.ThrowsException<EndOfStreamException>(
            () => TestUtils.Create<ScoreData>(MakeByteArray(mock)));
    }

    [TestMethod]
    public void ReadFromTestExceededName()
    {
        var mock = Th10.ScoreDataTests.MockScoreData<StageProgress>();
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
    }
}

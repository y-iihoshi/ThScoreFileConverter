using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NSubstitute;
using ThScoreFileConverter.Core.Tests.UnitTesting;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th10;
using ThScoreFileConverter.Tests.UnitTesting;

#if NETFRAMEWORK
using ThScoreFileConverter.Core.Extensions;
#endif

namespace ThScoreFileConverter.Tests.Models.Th10;

[TestClass]
public class ScoreDataTests
{
    internal static IScoreData<TStageProgress> MockScoreData<TStageProgress>()
        where TStageProgress : struct, Enum
    {
        var mock = Substitute.For<IScoreData<TStageProgress>>();
        _ = mock.Score.Returns(12u);
        _ = mock.StageProgress.Returns(TestUtils.Cast<TStageProgress>(3));
        _ = mock.ContinueCount.Returns((byte)4);
        _ = mock.Name.Returns(TestUtils.MakeRandomArray(10));
        _ = mock.DateTime.Returns(567u);
        _ = mock.SlowRate.Returns(8.9f);
        return mock;
    }

    internal static byte[] MakeByteArray<TStageProgress>(IScoreData<TStageProgress> scoreData, int unknownSize)
        where TStageProgress : struct, Enum
    {
        return TestUtils.MakeByteArray(
            scoreData.Score,
            (byte)TestUtils.Cast<int>(scoreData.StageProgress),
            scoreData.ContinueCount,
            scoreData.Name,
            scoreData.DateTime,
            scoreData.SlowRate,
            new byte[unknownSize]);
    }

    internal static void Validate<TStageProgress>(
        IScoreData<TStageProgress> expected, IScoreData<TStageProgress> actual)
        where TStageProgress : struct, Enum
    {
        Assert.AreEqual(expected.Score, actual.Score);
        Assert.AreEqual(expected.StageProgress, actual.StageProgress);
        Assert.AreEqual(expected.ContinueCount, actual.ContinueCount);
        CollectionAssert.That.AreEqual(expected.Name, actual.Name);
        Assert.AreEqual(expected.DateTime, actual.DateTime);
        Assert.AreEqual(expected.SlowRate, actual.SlowRate);
    }

    internal static void ScoreDataTestHelper<TScoreData, TStageProgress>()
        where TScoreData : IScoreData<TStageProgress>, new()
        where TStageProgress : struct, Enum
    {
        var mock = Substitute.For<IScoreData<TStageProgress>>();
        var scoreData = new TScoreData();
        Validate(mock, scoreData);
    }

    internal static void ReadFromTestHelper<TScoreData, TStageProgress>(int unknownSize)
        where TScoreData : IScoreData<TStageProgress>, IBinaryReadable, new()
        where TStageProgress : struct, Enum
    {
        var mock = MockScoreData<TStageProgress>();
        var scoreData = TestUtils.Create<TScoreData>(MakeByteArray(mock, unknownSize));
        Validate(mock, scoreData);
    }

    internal static void ReadFromTestShortenedNameHelper<TScoreData, TStageProgress>(int unknownSize)
        where TScoreData : IScoreData<TStageProgress>, IBinaryReadable, new()
        where TStageProgress : struct, Enum
    {
        var mock = MockScoreData<TStageProgress>();
        var name = mock.Name;
        _ = mock.Name.Returns(name.SkipLast(1).ToArray());

        _ = Assert.ThrowsException<EndOfStreamException>(
            () => TestUtils.Create<TScoreData>(MakeByteArray(mock, unknownSize)));
    }

    internal static void ReadFromTestExceededNameHelper<TScoreData, TStageProgress>(int unknownSize)
        where TScoreData : IScoreData<TStageProgress>, IBinaryReadable, new()
        where TStageProgress : struct, Enum
    {
        var mock = MockScoreData<TStageProgress>();
        var name = mock.Name;
        _ = mock.Name.Returns(name.Concat(TestUtils.MakeRandomArray(1)).ToArray());

        var scoreData = TestUtils.Create<TScoreData>(MakeByteArray(mock, unknownSize));

        Assert.AreEqual(mock.Score, scoreData.Score);
        Assert.AreEqual(mock.StageProgress, scoreData.StageProgress);
        Assert.AreEqual(mock.ContinueCount, scoreData.ContinueCount);
        CollectionAssert.That.AreNotEqual(mock.Name, scoreData.Name);
        CollectionAssert.That.AreEqual(mock.Name.SkipLast(1), scoreData.Name);
        Assert.AreNotEqual(mock.DateTime, scoreData.DateTime);
        Assert.AreNotEqual(mock.SlowRate, scoreData.SlowRate);
    }

    internal static void ReadFromTestInvalidStageProgressHelper<TScoreData, TStageProgress>(
        int unknownSize, int stageProgress)
        where TScoreData : IScoreData<TStageProgress>, IBinaryReadable, new()
        where TStageProgress : struct, Enum
    {
        var mock = MockScoreData<TStageProgress>();
        _ = mock.StageProgress.Returns(TestUtils.Cast<TStageProgress>(stageProgress));

        _ = Assert.ThrowsException<InvalidCastException>(
            () => TestUtils.Create<TScoreData>(MakeByteArray(mock, unknownSize)));
    }

    internal static int UnknownSize { get; }

    [TestMethod]
    public void ScoreDataTest()
    {
        ScoreDataTestHelper<ScoreData, StageProgress>();
    }

    [TestMethod]
    public void ReadFromTest()
    {
        ReadFromTestHelper<ScoreData, StageProgress>(UnknownSize);
    }

    [TestMethod]
    public void ReadFromTestShortenedName()
    {
        ReadFromTestShortenedNameHelper<ScoreData, StageProgress>(UnknownSize);
    }

    [TestMethod]
    public void ReadFromTestExceededName()
    {
        ReadFromTestExceededNameHelper<ScoreData, StageProgress>(UnknownSize);
    }

    public static IEnumerable<object[]> InvalidStageProgresses => TestUtils.GetInvalidEnumerators<StageProgress>();

    [DataTestMethod]
    [DynamicData(nameof(InvalidStageProgresses))]
    public void ReadFromTestInvalidStageProgress(int stageProgress)
    {
        ReadFromTestInvalidStageProgressHelper<ScoreData, StageProgress>(UnknownSize, stageProgress);
    }
}

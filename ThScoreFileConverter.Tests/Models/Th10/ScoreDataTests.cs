using NSubstitute;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th10;

namespace ThScoreFileConverter.Tests.Models.Th10;

internal static class ScoreDataExtensions
{
    internal static void ShouldBe<TStageProgress>(this IScoreData<TStageProgress> actual, IScoreData<TStageProgress> expected)
        where TStageProgress : struct, Enum
    {
        actual.Score.ShouldBe(expected.Score);
        actual.StageProgress.ShouldBe(expected.StageProgress);
        actual.ContinueCount.ShouldBe(expected.ContinueCount);
        actual.Name.ShouldBe(expected.Name);
        actual.DateTime.ShouldBe(expected.DateTime);
        actual.SlowRate.ShouldBe(expected.SlowRate);
    }
}

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

    internal static void ScoreDataTestHelper<TScoreData, TStageProgress>()
        where TScoreData : IScoreData<TStageProgress>, new()
        where TStageProgress : struct, Enum
    {
        var mock = Substitute.For<IScoreData<TStageProgress>>();
        var scoreData = new TScoreData();
        scoreData.ShouldBe(mock);
    }

    internal static void ReadFromTestHelper<TScoreData, TStageProgress>(int unknownSize)
        where TScoreData : IScoreData<TStageProgress>, IBinaryReadable, new()
        where TStageProgress : struct, Enum
    {
        var mock = MockScoreData<TStageProgress>();
        var scoreData = TestUtils.Create<TScoreData>(MakeByteArray(mock, unknownSize));
        scoreData.ShouldBe(mock);
    }

    internal static void ReadFromTestShortenedNameHelper<TScoreData, TStageProgress>(int unknownSize)
        where TScoreData : IScoreData<TStageProgress>, IBinaryReadable, new()
        where TStageProgress : struct, Enum
    {
        var mock = MockScoreData<TStageProgress>();
        var name = mock.Name;
        _ = mock.Name.Returns(name.SkipLast(1).ToArray());

        _ = Should.Throw<EndOfStreamException>(
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

        scoreData.Score.ShouldBe(mock.Score);
        scoreData.StageProgress.ShouldBe(mock.StageProgress);
        scoreData.ContinueCount.ShouldBe(mock.ContinueCount);
        scoreData.Name.ShouldNotBe(mock.Name);
        scoreData.Name.ShouldBe(mock.Name.SkipLast(1));
        scoreData.DateTime.ShouldNotBe(mock.DateTime);
        scoreData.SlowRate.ShouldNotBe(mock.SlowRate);
    }

    internal static void ReadFromTestInvalidStageProgressHelper<TScoreData, TStageProgress>(
        int unknownSize, int stageProgress)
        where TScoreData : IScoreData<TStageProgress>, IBinaryReadable, new()
        where TStageProgress : struct, Enum
    {
        var mock = MockScoreData<TStageProgress>();
        _ = mock.StageProgress.Returns(TestUtils.Cast<TStageProgress>(stageProgress));

        _ = Should.Throw<InvalidCastException>(
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

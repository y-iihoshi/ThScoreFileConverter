using NSubstitute;
using ThScoreFileConverter.Models.Th15;
using StageProgress = ThScoreFileConverter.Models.Th13.StageProgress;

namespace ThScoreFileConverter.Tests.Models.Th15;

internal static class ScoreDataExtensions
{
    internal static void ShouldBe(this IScoreData actual, IScoreData expected)
    {
        actual.Score.ShouldBe(expected.Score);
        actual.StageProgress.ShouldBe(expected.StageProgress);
        actual.ContinueCount.ShouldBe(expected.ContinueCount);
        actual.Name.ShouldBe(expected.Name);
        actual.DateTime.ShouldBe(expected.DateTime);
        actual.SlowRate.ShouldBe(expected.SlowRate);
        actual.RetryCount.ShouldBe(expected.RetryCount);
    }
}

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

    [TestMethod]
    public void ScoreDataTest()
    {
        var mock = Substitute.For<IScoreData>();
        var scoreData = new ScoreData();

        scoreData.ShouldBe(mock);
    }

    [TestMethod]
    public void ReadFromTest()
    {
        var mock = MockScoreData();
        var scoreData = TestUtils.Create<ScoreData>(MakeByteArray(mock));

        scoreData.ShouldBe(mock);
    }

    public static IEnumerable<object[]> InvalidStageProgresses => TestUtils.GetInvalidEnumerators<StageProgress>();

    [DataTestMethod]
    [DynamicData(nameof(InvalidStageProgresses))]
    public void ReadFromTestInvalidStageProgress(int stageProgress)
    {
        var mock = MockScoreData();
        _ = mock.StageProgress.Returns((StageProgress)stageProgress);

        _ = Should.Throw<InvalidCastException>(
            () => TestUtils.Create<ScoreData>(MakeByteArray(mock)));
    }

    [TestMethod]
    public void ReadFromTestShortenedName()
    {
        var mock = MockScoreData();
        var name = mock.Name;
        _ = mock.Name.Returns(name.SkipLast(1).ToArray());

        _ = Should.Throw<EndOfStreamException>(
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

        scoreData.Score.ShouldBe(mock.Score);
        scoreData.StageProgress.ShouldBe(mock.StageProgress);
        scoreData.ContinueCount.ShouldBe(mock.ContinueCount);
        scoreData.Name.ShouldNotBe(mock.Name);
        scoreData.Name.ShouldBe(mock.Name.Take(validNameLength));
        scoreData.DateTime.ShouldNotBe(mock.DateTime);
        scoreData.SlowRate.ShouldNotBe(mock.SlowRate);
        scoreData.RetryCount.ShouldNotBe(mock.RetryCount);
    }
}

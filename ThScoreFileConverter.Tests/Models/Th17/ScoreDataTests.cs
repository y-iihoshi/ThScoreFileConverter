using NSubstitute;
using ThScoreFileConverter.Models.Th17;
using static ThScoreFileConverter.Tests.Models.Th10.ScoreDataExtensions;
using IScoreData = ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th13.StageProgress>;
using StageProgress = ThScoreFileConverter.Models.Th13.StageProgress;

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

        scoreData.ShouldBe(mock);
    }

    public static IEnumerable<object[]> InvalidStageProgresses => TestUtils.GetInvalidEnumerators<StageProgress>();

    [DataTestMethod]
    [DynamicData(nameof(InvalidStageProgresses))]
    public void ReadFromTestInvalidStageProgress(int stageProgress)
    {
        var mock = Th10.ScoreDataTests.MockScoreData<StageProgress>();
        _ = mock.StageProgress.Returns((StageProgress)stageProgress);

        _ = Should.Throw<InvalidCastException>(
            () => TestUtils.Create<ScoreData>(MakeByteArray(mock)));
    }

    [TestMethod]
    public void ReadFromTestShortenedName()
    {
        var mock = Th10.ScoreDataTests.MockScoreData<StageProgress>();
        var name = mock.Name;
        _ = mock.Name.Returns(name.SkipLast(1).ToArray());

        _ = Should.Throw<EndOfStreamException>(
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

        scoreData.Score.ShouldBe(mock.Score);
        scoreData.StageProgress.ShouldBe(mock.StageProgress);
        scoreData.ContinueCount.ShouldBe(mock.ContinueCount);
        scoreData.Name.ShouldNotBe(mock.Name);
        scoreData.Name.ShouldBe(mock.Name.Take(validNameLength));
        scoreData.DateTime.ShouldNotBe(mock.DateTime);
        scoreData.SlowRate.ShouldNotBe(mock.SlowRate);
    }
}
